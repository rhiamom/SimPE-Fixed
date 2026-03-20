using Avalonia.Controls;
using Avalonia.Controls.Selection;
using Avalonia.Platform.Storage;
using AvaloniaHex.Document;
using SimPe.Interfaces.Files;
using SimPe.Interfaces.Scenegraph;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimPe
{
    public partial class MainForm : Window
    {
        private SimPe.Packages.GeneratableFile _package;
        private byte[] _currentBytes;

        public MainForm()
        {
            InitializeComponent();
            WireEvents();
        }

        // ─────────────────────────────────────────────────────────────
        // Event wiring
        // ─────────────────────────────────────────────────────────────

        public event Action<IScenegraphFileIndexItem> ResourceSelected;

        private void WireEvents()
        {
            MenuFileOpen.Click  += async (_, _) => await OpenPackage();

            MenuFileClose.Click += (_, _) => ResetPackageUI();

            MenuFileExit.Click  += (_, _) => Close();

            ResourceTree.SelectionChanged += OnTreeSelectionChanged;
            ResourceList.SelectionChanged += OnListSelectionChanged;

            HexEditor.Caret.LocationChanged += (_, _) => UpdateConverter();
        }

        // ─────────────────────────────────────────────────────────────
        // Open
        // ─────────────────────────────────────────────────────────────

        private async Task OpenPackage()
        {
            try
            {
                var topLevel = TopLevel.GetTopLevel(this);
                if (topLevel == null) { StatusText.Text = "Error: no TopLevel"; return; }

                var files = await topLevel.StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions
                {
                    Title         = "Open Package",
                    AllowMultiple = false,
                    FileTypeFilter = new[]
                    {
                        new FilePickerFileType("Sims 2 Package")
                        {
                            Patterns = new[] { "*.package", "*.package.disabled" }
                        },
                        new FilePickerFileType("All Files") { Patterns = new[] { "*.*" } }
                    }
                });

                if (files.Count == 0) return;

                ResetPackageUI();

                string path = files[0].Path.LocalPath;
                StatusText.Text = "Loading…";
                _package = SimPe.Packages.File.LoadFromFile(path, true);
                if (_package == null) { StatusText.Text = "Error: LoadFromFile returned null"; return; }
                PopulateTree();
                ShowAll();
                Title = $"SimPE — {System.IO.Path.GetFileName(path)}";
            }
            catch (Exception ex)
            {
                StatusText.Text = $"Error: {ex.GetType().Name}: {ex.Message}";
                System.Diagnostics.Trace.WriteLine(ex.ToString());
            }
        }

        // ─────────────────────────────────────────────────────────────
        // Close
        // ─────────────────────────────────────────────────────────────

        private void ResetPackageUI()
        {
            if (_package == null) return;
            _package = null;
            _currentBytes = null;
            ResourceTree.ItemsSource = null;
            ResourceList.ItemsSource = null;
            HexEditor.Document = new MemoryBinaryDocument(Array.Empty<byte>());
            ClearConverter();
            Title = "SimPE — Sims Package Editor";
            StatusText.Text = "Ready";
        }

        // ─────────────────────────────────────────────────────────────
        // Tree
        // ─────────────────────────────────────────────────────────────

        private void PopulateTree()
        {
            var nodes = _package.Index
                .GroupBy(pfd => pfd.Type)
                .OrderBy(g => SimPe.Data.MetaData.FindTypeAlias(g.Key)?.Name ?? $"0x{g.Key:X8}")
                .Select(g => new ResourceTypeNode(SimPe.Data.MetaData.FindTypeAlias(g.Key), g.ToList()))
                .ToList();

            ResourceTree.ItemsSource = nodes;
        }

        private void OnTreeSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ResourceTree.SelectedItem is ResourceTypeNode node)
            {
                PopulateList(node.Descriptors);
                StatusText.Text = $"{node.Descriptors.Count} resources";
            }
        }

        // ─────────────────────────────────────────────────────────────
        // List
        // ─────────────────────────────────────────────────────────────

        // Shows every resource in the package (no tree filter active)
        private void ShowAll()
        {
            PopulateList(_package.Index);
            StatusText.Text = $"{_package.Index.Length} resources";
        }

        private void PopulateList(IEnumerable<IPackedFileDescriptor> descriptors)
        {
            ResourceList.ItemsSource = descriptors
                .Select(pfd => new ResourceListItem(pfd, _package))
                .ToList();
        }

        // ─────────────────────────────────────────────────────────────
        // Resource selection
        // ─────────────────────────────────────────────────────────────

        private void OnListSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ResourceList.SelectedItem is ResourceListItem item)
            {
                ResInfoPanel.Show(item.Descriptor, _package);

                try
                {
                    var pf = _package.Read(item.Descriptor);
                    _currentBytes = pf?.UncompressedData ?? Array.Empty<byte>();
                }
                catch
                {
                    _currentBytes = Array.Empty<byte>();
                }

                HexEditor.Document = new MemoryBinaryDocument(_currentBytes);
                ClearConverter();

                var fileItem = new SimPe.Plugin.FileIndexItem(item.Descriptor, _package);
                ResourceSelected?.Invoke(fileItem);
            }
            else
            {
                _currentBytes = null;
                HexEditor.Document = new MemoryBinaryDocument(Array.Empty<byte>());
                ClearConverter();
                PluginPanel.Content = null;
            }
        }

        // ─────────────────────────────────────────────────────────────
        // Converter
        // ─────────────────────────────────────────────────────────────

        private void UpdateConverter()
        {
            byte[] b = _currentBytes;
            if (b == null || b.Length == 0) { ClearConverter(); return; }

            long pos = (long)HexEditor.Caret.Location.ByteIndex;
            if (pos < 0 || pos >= b.Length) { ClearConverter(); return; }

            CvOffset.Text = $"0x{pos:X8}";
            CvByte.Text   = $"0x{b[pos]:X2}";
            CvBits.Text   = Convert.ToString(b[pos], 2).PadLeft(8, '0');

            if (pos + 1 < b.Length)
            {
                ushort us = BitConverter.ToUInt16(b, (int)pos);
                CvUShort.Text = $"0x{us:X4}";
                CvShort.Text  = BitConverter.ToInt16(b, (int)pos).ToString();
            }

            if (pos + 3 < b.Length)
            {
                uint ui = BitConverter.ToUInt32(b, (int)pos);
                CvUInt.Text  = $"0x{ui:X8}";
                CvInt.Text   = BitConverter.ToInt32(b, (int)pos).ToString();
                CvFloat.Text = BitConverter.ToSingle(b, (int)pos).ToString("G9");
            }

            if (pos + 7 < b.Length)
            {
                ulong ul = BitConverter.ToUInt64(b, (int)pos);
                CvULong.Text  = $"0x{ul:X16}";
                CvDouble.Text = BitConverter.ToDouble(b, (int)pos).ToString("G17");
            }
        }

        private void ClearConverter()
        {
            CvOffset.Text = CvByte.Text = CvUShort.Text = CvShort.Text =
            CvUInt.Text   = CvInt.Text  = CvULong.Text  = CvFloat.Text =
            CvDouble.Text = CvBits.Text = string.Empty;
        }
    }
}
