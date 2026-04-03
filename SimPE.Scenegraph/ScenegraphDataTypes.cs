/***************************************************************************
 *   Copyright (C) 2025 by GramzeSweatShop                                 *
 *   rhiamom@mac.com                                                       *
 *                                                                         *
 *   This program is free software; you can redistribute it and/or modify  *
 *   it under the terms of the GNU General Public License as published by  *
 *   the Free Software Foundation; either version 2 of the License, or    *
 *   (at your option) any later version.                                   *
 ***************************************************************************/

// Data model types for SimPE.Scenegraph forms (Avalonia port).
// These are plain C# data classes used by form code-behind to hold list/tree data.
// They have no dependency on System.Windows.Forms — they mimic the relevant API
// so the business logic compiles unchanged while the actual Avalonia UI is wired up
// separately via AXAML layouts.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Layout;
using Avalonia.Media;

namespace SimPe.Scenegraph.Compat
{
    // ── ListViewItem / SubItem ───────────────────────────────────────────────

    /// <summary>Minimal ListViewItem — replaces System.Windows.Forms.ListViewItem.</summary>
    public class ListViewItem
    {
        public class ListViewSubItemCollection
        {
            private readonly List<SubItem> _items = new List<SubItem>();
            public SubItem this[int index] => _items[index];
            public void Add(string text) => _items.Add(new SubItem(text));
            public int Count => _items.Count;
            public void Clear() => _items.Clear();
            public IEnumerator GetEnumerator() => _items.GetEnumerator();
        }

        public class SubItem
        {
            public string Text { get; set; }
            public System.Drawing.Color ForeColor { get; set; } = System.Drawing.Color.Empty;
            public SubItem(string text) { Text = text; }
        }

        public string Text { get; set; }
        public object Tag { get; set; }
        public int ImageIndex { get; set; } = -1;
        public bool Selected { get; set; }
        public bool Focused { get; set; }
        public int Index { get; set; } = -1;
        public ListViewSubItemCollection SubItems { get; } = new ListViewSubItemCollection();

        public ListViewItem() { Text = ""; }
        public ListViewItem(string text) { Text = text; }
        public ListViewItem(string text, int imageIndex) { Text = text; ImageIndex = imageIndex; }
        public ListViewItem(string[] items)
        {
            Text = items.Length > 0 ? items[0] : "";
            for (int i = 1; i < items.Length; i++) SubItems.Add(items[i]);
        }

        public bool Checked { get; set; }
        public System.Drawing.Color ForeColor { get; set; } = System.Drawing.Color.Empty;
        public void EnsureVisible() { }
        public ListViewItem Clone() { return (ListViewItem)MemberwiseClone(); }
        /// <summary>Back-reference to the owning ListView (set by ListViewItemCollection.Add).</summary>
        public ListView ListView { get; set; }
        public int GroupIndex { get; set; } = -1;
        public bool UseItemStyleForSubItems { get; set; } = true;
        public ListViewGroup Group { get; set; }
    }

    // ── ListView ─────────────────────────────────────────────────────────────

    /// <summary>Minimal ListView — replaces System.Windows.Forms.ListView.</summary>
    public enum CheckState { Unchecked = 0, Checked = 1, Indeterminate = 2 }

    public class ListViewGroup
    {
        public string Header { get; set; }
        public ListViewGroup(string header) { Header = header; }
        public ListViewGroup() { Header = ""; }
    }

    public class ListViewGroupCollection : System.Collections.IEnumerable
    {
        private readonly List<ListViewGroup> _groups = new List<ListViewGroup>();
        public int Count => _groups.Count;
        public ListViewGroup this[int index] => _groups[index];
        public void Add(ListViewGroup group) => _groups.Add(group);
        public void Clear() => _groups.Clear();
        public int IndexOf(ListViewGroup group) => _groups.IndexOf(group);
        public System.Collections.IEnumerator GetEnumerator() => _groups.GetEnumerator();
    }

    public class ListView : Avalonia.Controls.Control
    {
        public class ListViewItemCollection
        {
            private ListView _owner;
            internal ListViewItemCollection(ListView owner) { _owner = owner; }
            private readonly List<ListViewItem> _items = new List<ListViewItem>();
            public ListViewItem this[int index] { get => _items[index]; set => _items[index] = value; }
            public int Count => _items.Count;
            public void Add(ListViewItem item) { item.ListView = _owner; _items.Add(item); }
            public void AddRange(ListViewItem[] items) { foreach (var item in items) Add(item); }
            public void Clear() => _items.Clear();
            public void Remove(ListViewItem item) => _items.Remove(item);
            public void RemoveAt(int index) => _items.RemoveAt(index);
            public bool Contains(ListViewItem item) => _items.Contains(item);
            public IEnumerator GetEnumerator() => _items.GetEnumerator();
        }

        public class SelectedListViewItemCollection : System.Collections.IEnumerable
        {
            private readonly ListViewItemCollection _all;
            public SelectedListViewItemCollection(ListViewItemCollection all) { _all = all; }
            public int Count
            {
                get { int c = 0; foreach (ListViewItem i in _all) { if (((ListViewItem)i).Selected) c++; } return c; }
            }
            public ListViewItem this[int index]
            {
                get { int c = 0; foreach (object o in _all) { var i = (ListViewItem)o; if (i.Selected) { if (c == index) return i; c++; } } throw new IndexOutOfRangeException(); }
            }
            public System.Collections.IEnumerator GetEnumerator()
            {
                var list = new List<ListViewItem>();
                foreach (ListViewItem i in _all) if (i.Selected) list.Add(i);
                return list.GetEnumerator();
            }
        }

        public class SelectedIndexCollection
        {
            private readonly ListViewItemCollection _all;
            public SelectedIndexCollection(ListViewItemCollection all) { _all = all; }
            public int Count
            {
                get { int c = 0; foreach (object o in _all) { if (((ListViewItem)o).Selected) c++; } return c; }
            }
            public int this[int index]
            {
                get { int c = 0; foreach (object o in _all) { var i = (ListViewItem)o; if (i.Selected) { if (c == index) return i.Index; c++; } } throw new IndexOutOfRangeException(); }
            }
            public void Add(int index) { if (index >= 0 && index < _all.Count) _all[index].Selected = true; }
            public void Clear() { foreach (object o in _all) ((ListViewItem)o).Selected = false; }
        }

        public class ColumnHeaderCollection
        {
            private readonly List<ColumnHeader> _columns = new List<ColumnHeader>();
            public int Count => _columns.Count;
            public ColumnHeader this[int index] { get => _columns[index]; set => _columns[index] = value; }
            public void Add(ColumnHeader col) => _columns.Add(col);
            public void AddRange(ColumnHeader[] cols) { foreach (var col in cols) Add(col); }
            public void Clear() => _columns.Clear();
            public IEnumerator GetEnumerator() => _columns.GetEnumerator();
        }

        public class CheckedListViewItemCollection : System.Collections.IEnumerable
        {
            private readonly ListViewItemCollection _all;
            internal CheckedListViewItemCollection(ListViewItemCollection all) { _all = all; }
            public int Count { get { int c = 0; foreach (ListViewItem i in _all) if (i.Checked) c++; return c; } }
            public System.Collections.IEnumerator GetEnumerator()
            {
                var list = new List<ListViewItem>();
                foreach (ListViewItem i in _all) if (i.Checked) list.Add(i);
                return list.GetEnumerator();
            }
        }

        public ListViewItemCollection Items { get; }
        public SelectedListViewItemCollection SelectedItems { get; }
        public SelectedIndexCollection SelectedIndices { get; }
        public CheckedListViewItemCollection CheckedItems { get; }
        public ColumnHeaderCollection Columns { get; } = new ColumnHeaderCollection();
        public object HeaderStyle { get; set; }
        public new object Tag { get; set; }
        public bool Focused { get; set; }
        public bool Enabled { get; set; } = true;
        public bool MultiSelect { get; set; } = true;
        public bool HideSelection { get; set; }
        public bool FullRowSelect { get; set; }
        public bool GridLines { get; set; }
        public ImageList LargeImageList { get; set; }
        public ImageList SmallImageList { get; set; }
        public ImageList StateImageList { get; set; }
        public System.Drawing.Font Font { get; set; }
        public object View { get; set; }
        public bool UseCompatibleStateImageBehavior { get; set; }
        public bool CheckBoxes { get; set; }
        public System.Drawing.Color BackColor { get; set; }

        // Layout/position properties (no-ops in Avalonia port)
        public int Left { get; set; }
        public int Top { get; set; }
        public new int Width { get; set; }
        public new int Height { get; set; }
        public new Avalonia.Controls.Control Parent { get; set; }
        public bool Visible { get => IsVisible; set => IsVisible = value; }
        public new bool IsVisible { get; set; } = true;
        public object Anchor { get; set; }
        public ListViewItem FocusedItem => Items.Count > 0 ? Items[0] : null;

        public event EventHandler SelectedIndexChanged;
        public event EventHandler SelectionChanged;
        public event EventHandler Click;
        public event EventHandler DoubleClick;
        public event EventHandler ColumnClick;
        public event EventHandler ItemActivate;
        public event EventHandler Resize;
        public object Activation { get; set; }

        public new string Name { get; set; } = "";
        public object ListViewItemSorter { get; set; }
        public SimPe.SortOrder Sorting { get; set; } = SimPe.SortOrder.None;
        public void SelectAll() { foreach (object o in Items) ((ListViewItem)o).Selected = true; }

        // No-ops: update batching has no effect in this compat stub
        public void BeginUpdate() { }
        public void EndUpdate() { }
        public void Refresh() { }
        public void Sort() { }

        public bool DoubleBuffering { get; set; }
        public bool ShowGroups { get; set; }
        public BorderStyle BorderStyle { get; set; }
        public int[] TileColumns { get; set; } = new int[0];
        public ListViewGroupCollection Groups { get; } = new ListViewGroupCollection();

        public ListView()
        {
            Items = new ListViewItemCollection(this);
            SelectedItems = new SelectedListViewItemCollection(Items);
            SelectedIndices = new SelectedIndexCollection(Items);
            CheckedItems = new CheckedListViewItemCollection(Items);
        }
    }

    // ── ColumnHeader ─────────────────────────────────────────────────────────

    /// <summary>Minimal ColumnHeader — replaces System.Windows.Forms.ColumnHeader.</summary>
    public class ColumnHeader
    {
        public string Text { get; set; }
        public int Width { get; set; }
    }

    // ── ImageList ────────────────────────────────────────────────────────────

    /// <summary>Minimal ImageList — replaces System.Windows.Forms.ImageList.</summary>
    public class ImageList
    {
        public class ImageCollection
        {
            private readonly List<System.Drawing.Image> _images = new List<System.Drawing.Image>();
            public int Count => _images.Count;
            public void Add(System.Drawing.Image img) => _images.Add(img);
            public void Clear() => _images.Clear();
            public System.Drawing.Image this[int index] { get => _images[index]; set => _images[index] = value; }
        }

        public System.Drawing.Size ImageSize { get; set; } = new System.Drawing.Size(16, 16);
        public ColorDepth ColorDepth { get; set; }
        public ImageCollection Images { get; } = new ImageCollection();
    }

    // ── ColorDepth enum ──────────────────────────────────────────────────────
    public enum ColorDepth { Depth4Bit, Depth8Bit, Depth16Bit, Depth24Bit, Depth32Bit }

    // ── CheckBox compat (for SubsetSelectForm programmatic use) ──────────────

    /// <summary>
    /// Extends Avalonia CheckBox with WinForms-compatible API used in SubsetSelectForm.
    /// </summary>
    public class CheckBoxCompat : Avalonia.Controls.CheckBox
    {
        public new bool Checked
        {
            get => IsChecked == true;
            set => IsChecked = value;
        }
        public Avalonia.Layout.HorizontalAlignment Anchor { get; set; }
        public object FlatStyle { get; set; }

        public event EventHandler CheckedChanged;

        public CheckBoxCompat()
        {
            IsCheckedChanged += (s, e) => CheckedChanged?.Invoke(this, EventArgs.Empty);
        }
    }

    // ── PictureBoxCompat ─────────────────────────────────────────────────────

    /// <summary>
    /// Avalonia UserControl that renders a System.Drawing.Image using an inner
    /// Avalonia.Controls.Image (converted on assignment via Helper.ToAvaloniaBitmap).
    /// </summary>
    public class PictureBoxCompat : Avalonia.Controls.UserControl
    {
        private readonly Avalonia.Controls.Image _imgCtrl = new Avalonia.Controls.Image
        {
            Stretch = Avalonia.Media.Stretch.Uniform,
            HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Stretch,
            VerticalAlignment   = Avalonia.Layout.VerticalAlignment.Stretch,
        };

        public Avalonia.Media.Stretch Stretch
        {
            get => _imgCtrl.Stretch;
            set
            {
                _imgCtrl.Stretch = value;
                _imgCtrl.HorizontalAlignment = value == Avalonia.Media.Stretch.None
                    ? Avalonia.Layout.HorizontalAlignment.Left
                    : Avalonia.Layout.HorizontalAlignment.Stretch;
                _imgCtrl.VerticalAlignment = value == Avalonia.Media.Stretch.None
                    ? Avalonia.Layout.VerticalAlignment.Top
                    : Avalonia.Layout.VerticalAlignment.Stretch;
            }
        }

        private SkiaSharp.SKBitmap _image;
        public SkiaSharp.SKBitmap Image
        {
            get => _image;
            set
            {
                _image = value;
                _imgCtrl.Source = (value != null) ? SimPe.Helper.ToAvaloniaBitmap(value) : null;
            }
        }

        public PictureBoxCompat()
        {
            Content = _imgCtrl;
        }
    }

    // ── PropertyGridStub ─────────────────────────────────────────────────────

    /// <summary>Placeholder for WinForms PropertyGrid — renders nothing on Mac.</summary>
    public class PropertyGridStub : Avalonia.Controls.Control
    {
        public object SelectedObject { get; set; }
    }

    // ── Panel compat ──────────────────────────────────────────────────────────

    /// <summary>
    /// Extends Avalonia Panel with WinForms-compatible layout API for SubsetSelectForm.
    /// </summary>
    public class PanelCompat : Avalonia.Controls.Panel
    {
        public int Top { get; set; }
        public int Left { get; set; }
        public new int Width { get; set; }
        public new int Height { get; set; }
        public bool Visible { get; set; } = true;
        public bool Enabled { get => IsEnabled; set => IsEnabled = value; }
        public Avalonia.Layout.HorizontalAlignment Anchor { get; set; }
        public Control ParentControl { get; set; }
        public new Avalonia.Controls.Control Parent { get; set; }
        public class ControlCollection : System.Collections.IEnumerable
        {
            private readonly List<object> _items = new List<object>();
            public void Add(object c) => _items.Add(c);
            public void Clear() => _items.Clear();
            public int Count => _items.Count;
            public System.Collections.IEnumerator GetEnumerator() => _items.GetEnumerator();
        }
        public ControlCollection Controls { get; } = new ControlCollection();
    }

    // ── GroupBox compat ───────────────────────────────────────────────────────

    /// <summary>WinForms GroupBox — extends Avalonia ContentControl.</summary>
    public class GroupBox : Avalonia.Controls.ContentControl
    {
        public class ControlCollection : IEnumerable<object>
        {
            private readonly List<object> _controls = new List<object>();
            public int Count => _controls.Count;
            public void Add(object c) => _controls.Add(c);
            public void Clear() => _controls.Clear();
            public IEnumerator<object> GetEnumerator() => _controls.GetEnumerator();
            System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() => _controls.GetEnumerator();
        }

        private string _text = "";
        public string Text { get => _text; set => _text = value; }
        public bool TabStop { get; set; }
        public object FlatStyle { get; set; }
        public int Left { get; set; }
        public int Top { get; set; }
        public new int Width { get; set; }
        public new int Height { get; set; }
        public new string Name { get => base.Name; set => base.Name = value; }
        public System.Drawing.Point Location { get; set; }
        public bool Visible { get => IsVisible; set => IsVisible = value; }
        public bool Enabled { get => IsEnabled; set => IsEnabled = value; }
        public Avalonia.Layout.HorizontalAlignment Anchor { get; set; }
        public System.Drawing.Color ForeColor { get; set; } = System.Drawing.Color.Empty;
        public ControlCollection Controls { get; } = new ControlCollection();
        public ControlCollection Children => Controls;
    }

    // ── CheckedListBox ───────────────────────────────────────────────────────

    /// <summary>Avalonia substitute for System.Windows.Forms.CheckedListBox.</summary>
    public class CheckedListBox : Avalonia.Controls.UserControl
    {
        private readonly List<(object Item, bool Checked)> _items = new List<(object, bool)>();
        private readonly Avalonia.Controls.StackPanel _panel = new Avalonia.Controls.StackPanel();

        public bool CheckOnClick { get; set; } = true;
        public bool HorizontalScrollbar { get; set; }
        public bool IntegralHeight { get; set; }
        public bool Sorted { get; set; }

        public CheckedListBoxItemCollection Items { get; }

        public CheckedListBox()
        {
            Items = new CheckedListBoxItemCollection(_items, _panel);
            Content = new Avalonia.Controls.ScrollViewer { Content = _panel };
        }

        public bool GetItemChecked(int index) => _items[index].Checked;

        public void SetItemChecked(int index, bool value)
        {
            var (item, _) = _items[index];
            _items[index] = (item, value);
            if (_panel.Children[index] is Avalonia.Controls.CheckBox cb)
                cb.IsChecked = value;
        }

        public class CheckedListBoxItemCollection
        {
            private readonly List<(object Item, bool Checked)> _items;
            private readonly Avalonia.Controls.StackPanel _panel;

            internal CheckedListBoxItemCollection(List<(object, bool)> items, Avalonia.Controls.StackPanel panel)
            {
                _items = items;
                _panel = panel;
            }

            public int Count => _items.Count;
            public object this[int i] => _items[i].Item;

            public void Clear()
            {
                _items.Clear();
                _panel.Children.Clear();
            }

            public void Add(object item, bool isChecked = false)
            {
                _items.Add((item, isChecked));
                var cb = new Avalonia.Controls.CheckBox
                {
                    Content = item?.ToString() ?? "",
                    IsChecked = isChecked,
                    Margin = new Avalonia.Thickness(2)
                };
                cb.IsCheckedChanged += (s, e) =>
                {
                    int idx = _panel.Children.IndexOf(cb);
                    if (idx >= 0 && idx < _items.Count)
                        _items[idx] = (_items[idx].Item, cb.IsChecked == true);
                };
                _panel.Children.Add(cb);
            }

            public void Insert(int index, object item)
            {
                _items.Insert(index, (item, false));
                var cb = new Avalonia.Controls.CheckBox
                {
                    Content = item?.ToString() ?? "",
                    IsChecked = false,
                    Margin = new Avalonia.Thickness(2)
                };
                cb.IsCheckedChanged += (s, e) =>
                {
                    int idx = _panel.Children.IndexOf(cb);
                    if (idx >= 0 && idx < _items.Count)
                        _items[idx] = (_items[idx].Item, cb.IsChecked == true);
                };
                _panel.Children.Insert(index, cb);
            }

            public System.Collections.IEnumerator GetEnumerator()
            {
                foreach (var (item, _) in _items) yield return item;
            }
        }
    }

    // ── PictureBox ────────────────────────────────────────────────────────────

    /// <summary>Minimal PictureBox — replaces System.Windows.Forms.PictureBox.</summary>
    public class PictureBox : Avalonia.Controls.Control
    {
        public System.Drawing.Image Image { get; set; }
        public System.Drawing.Size Size { get; set; } = new System.Drawing.Size(100, 100);
        public object SizeMode { get; set; }
        public object BorderStyle { get; set; }
        public System.Drawing.Point Location { get; set; }
        public int Left { get; set; }
        public int Top { get; set; }
        public new int Width { get; set; }
        public new int Height { get; set; }
        public bool Visible { get; set; } = true;
        public Avalonia.Layout.HorizontalAlignment Anchor { get; set; }
        public System.Drawing.Color BackColor { get; set; }
        public new object Parent { get; set; }
        public new object Tag { get; set; }
        public new object Cursor { get; set; }
        public event EventHandler MouseEnter;
        public event EventHandler MouseLeave;
        public event EventHandler Click;
    }

    // ── SortableComboBox ─────────────────────────────────────────────────────

    /// <summary>
    /// Extends Avalonia ComboBox with a no-op Sorted property (WinForms compat).
    /// Use this when code sets .Sorted = true/false to sort items.
    /// </summary>
    public class SortableComboBox : Avalonia.Controls.ComboBox
    {
        /// <summary>No-op in Avalonia port — WinForms sorted the dropdown items automatically.</summary>
        public bool Sorted { get; set; }
    }

    // ── FlatStyle / View / BorderStyle enums (WinForms compat) ───────────────
    public enum FlatStyle { Flat, Popup, Standard, System }
    public enum View { Details, LargeIcon, List, SmallIcon, Tile }
    public enum BorderStyle { None, FixedSingle, Fixed3D }
    public enum PictureBoxSizeMode { Normal, StretchImage, AutoSize, CenterImage, Zoom }
    public enum ColumnHeaderStyle { None, Nonclickable, Clickable }
    public enum ComboBoxStyle { Simple, DropDown, DropDownList }

    // ── LinkLabel compat ──────────────────────────────────────────────────────

    public class LinkData
    {
        public object Data { get; set; }
        public int Start { get; set; }
        public int Length { get; set; }
    }

    public class LinkArea
    {
        public int Start { get; set; }
        public int Length { get; set; }
        public LinkArea(int start, int length) { Start = start; Length = length; }
    }

    public class LinkCollection
    {
        private readonly List<LinkData> _links = new List<LinkData>();
        public int Count => _links.Count;
        public LinkData this[int index] { get => _links[index]; }
        public void Add(int start, int length, object data) => _links.Add(new LinkData { Data =data });
        public void Clear() => _links.Clear();
    }

    public class LinkLabelLinkClickedEventArgs : EventArgs
    {
        public LinkData Link { get; set; }
        public LinkLabelLinkClickedEventArgs() { Link = new LinkData(); }
        public LinkLabelLinkClickedEventArgs(LinkData link) { Link = link; }
    }
    public delegate void LinkLabelLinkClickedEventHandler(object sender, LinkLabelLinkClickedEventArgs e);

    /// <summary>Minimal LinkLabel — wraps Avalonia Button, fires LinkClicked on Click.</summary>
    public class LinkLabel : Avalonia.Controls.Button
    {
        public bool Visible { get => IsVisible; set => IsVisible = value; }
        public string Text { get => Content?.ToString() ?? ""; set => Content = value; }
        public LinkArea LinkArea { get; set; } = new LinkArea(0, 0);
        public LinkCollection Links { get; } = new LinkCollection();
        public System.Drawing.Point Location { get; set; }
        public int Left { get; set; }
        public int Right { get; set; }
        public int Top { get; set; }
        public int Bottom { get; set; }
        public System.Drawing.Size Size { get; set; }
        public new int Width { get; set; }
        public new int Height { get; set; }
        public new string Name { get => base.Name; set => base.Name = value; }
        public bool AutoSize { get; set; }
        public System.Drawing.Font Font { get; set; }
        public bool Enabled { get => IsEnabled; set => IsEnabled = value; }
        public new object Parent { get; set; }

        public event LinkLabelLinkClickedEventHandler LinkClicked;

        public LinkLabel()
        {
            Click += (s, e) => LinkClicked?.Invoke(this, new LinkLabelLinkClickedEventArgs { Link = new LinkData { Data =Tag } });
        }
    }

    // ── MessageBox ────────────────────────────────────────────────────────────
    // DialogResult is defined in SimPe.Helper (SimPe.DialogResult) — no duplicate here.

    public enum MessageBoxButtons { OK, OKCancel, YesNo, YesNoCancel, AbortRetryIgnore, RetryCancel }
    public enum MessageBoxIcon { None, Error, Warning, Information, Question, Hand, Stop, Asterisk, Exclamation }

    /// <summary>Async MessageBox replacement for Avalonia (no blocking UI thread).</summary>
    public static class MessageBox
    {
        public static async Task<SimPe.DialogResult> ShowAsync(
            string text,
            string caption = "",
            MessageBoxButtons buttons = MessageBoxButtons.OK,
            MessageBoxIcon icon = MessageBoxIcon.None)
        {
            var tcs = new TaskCompletionSource<SimPe.DialogResult>();
            var win = new Window
            {
                Title = caption,
                SizeToContent = SizeToContent.WidthAndHeight,
                WindowStartupLocation = WindowStartupLocation.CenterScreen,
                CanResize = false,
                MinWidth = 300
            };

            var panel = new StackPanel { Spacing = 16, Margin = new Avalonia.Thickness(20) };
            panel.Children.Add(new TextBlock
            {
                Text = text,
                TextWrapping = TextWrapping.Wrap,
                MaxWidth = 400
            });

            var btnRow = new StackPanel { Orientation = Orientation.Horizontal, Spacing = 8, HorizontalAlignment = HorizontalAlignment.Center };

            void AddBtn(string label, SimPe.DialogResult res)
            {
                var btn = new Avalonia.Controls.Button { Content = label, MinWidth = 80 };
                btn.Click += (s, e) => { win.Close(); tcs.TrySetResult(res); };
                btnRow.Children.Add(btn);
            }

            switch (buttons)
            {
                case MessageBoxButtons.OK:              AddBtn("OK", SimPe.DialogResult.OK); break;
                case MessageBoxButtons.OKCancel:        AddBtn("OK", SimPe.DialogResult.OK); AddBtn("Cancel", SimPe.DialogResult.Cancel); break;
                case MessageBoxButtons.YesNo:           AddBtn("Yes", SimPe.DialogResult.Yes); AddBtn("No", SimPe.DialogResult.No); break;
                case MessageBoxButtons.YesNoCancel:     AddBtn("Yes", SimPe.DialogResult.Yes); AddBtn("No", SimPe.DialogResult.No); AddBtn("Cancel", SimPe.DialogResult.Cancel); break;
                case MessageBoxButtons.RetryCancel:     AddBtn("Retry", SimPe.DialogResult.Retry); AddBtn("Cancel", SimPe.DialogResult.Cancel); break;
                case MessageBoxButtons.AbortRetryIgnore: AddBtn("Abort", SimPe.DialogResult.Abort); AddBtn("Retry", SimPe.DialogResult.Retry); AddBtn("Ignore", SimPe.DialogResult.Ignore); break;
                default: AddBtn("OK", SimPe.DialogResult.OK); break;
            }

            panel.Children.Add(btnRow);
            win.Content = panel;
            win.Closed += (s, e) => tcs.TrySetResult(SimPe.DialogResult.Cancel);
            win.Show();
            return await tcs.Task;
        }

        /// <summary>Synchronous wrapper — shows the dialog without awaiting. Returns Cancel immediately.</summary>
        public static SimPe.DialogResult Show(
            string text,
            string caption = "",
            MessageBoxButtons buttons = MessageBoxButtons.OK,
            MessageBoxIcon icon = MessageBoxIcon.None)
        {
            _ = ShowAsync(text, caption, buttons, icon);
            return SimPe.DialogResult.Cancel;
        }

        // Overloads accepting SimPe.MessageBoxButtons/Icon (from SimPe.WorkSpaceHelper)
        public static async Task<SimPe.DialogResult> ShowAsync(
            string text, string caption,
            SimPe.MessageBoxButtons buttons, SimPe.MessageBoxIcon icon)
            => await ShowAsync(text, caption, (MessageBoxButtons)(int)buttons, (MessageBoxIcon)(int)icon);

        public static async Task<SimPe.DialogResult> ShowAsync(
            string text, string caption,
            SimPe.MessageBoxButtons buttons)
            => await ShowAsync(text, caption, (MessageBoxButtons)(int)buttons);

        public static SimPe.DialogResult Show(
            string text, string caption,
            SimPe.MessageBoxButtons buttons, SimPe.MessageBoxIcon icon)
            => Show(text, caption, (MessageBoxButtons)(int)buttons, (MessageBoxIcon)(int)icon);

        public static SimPe.DialogResult Show(
            string text, string caption,
            SimPe.MessageBoxButtons buttons)
            => Show(text, caption, (MessageBoxButtons)(int)buttons);
    }

    // ── TreeNode / TreeView compat ────────────────────────────────────────────

    /// <summary>Common interface for both TreeNode.TreeNodeCollection and TreeView.TreeNodeCollection.</summary>
    public interface ITreeNodeCollection : IEnumerable
    {
        int Count { get; }
        TreeNode this[int index] { get; }
        TreeNode Add(TreeNode node);
        void Clear();
    }

    /// <summary>Minimal TreeNode — replaces System.Windows.Forms.TreeNode.</summary>
    public class TreeNode
    {
        public class TreeNodeCollection : ITreeNodeCollection
        {
            private readonly List<TreeNode> _nodes = new List<TreeNode>();
            private readonly TreeNode _owner;
            public TreeNodeCollection(TreeNode owner) { _owner = owner; }
            public int Count => _nodes.Count;
            public TreeNode this[int index] => _nodes[index];
            public TreeNode Add(TreeNode node) { node.Parent = _owner; _nodes.Add(node); return node; }
            public void Clear() => _nodes.Clear();
            public IEnumerator GetEnumerator() => _nodes.GetEnumerator();
        }

        public string Text { get; set; }
        public object Tag { get; set; }
        public int ImageIndex { get; set; } = -1;
        public int SelectedImageIndex { get; set; } = -1;
        public TreeNodeCollection Nodes { get; }
        public TreeNode Parent { get; set; }
        public TreeView TreeView { get; set; }

        public TreeNode() { Nodes = new TreeNodeCollection(this); }
        public TreeNode(string text) : this() { Text = text; }

        public void EnsureVisible() { }
        public void Expand() { }
    }

    /// <summary>Event args for TreeView AfterSelect — replaces System.Windows.Forms.TreeViewEventArgs.</summary>
    public class TreeViewEventArgs : EventArgs
    {
        public TreeNode Node { get; }
        public TreeViewEventArgs(TreeNode node) { Node = node; }
    }

    /// <summary>Delegate for TreeView AfterSelect — replaces System.Windows.Forms.TreeViewEventHandler.</summary>
    public delegate void TreeViewEventHandler(object sender, TreeViewEventArgs e);

    /// <summary>Minimal TreeView — replaces System.Windows.Forms.TreeView.</summary>
    public class TreeView
    {
        public class TreeNodeCollection : ITreeNodeCollection
        {
            private readonly List<TreeNode> _nodes = new List<TreeNode>();
            public int Count => _nodes.Count;
            public TreeNode this[int index] => _nodes[index];
            public TreeNode Add(TreeNode node) { _nodes.Add(node); return node; }
            public void Clear() => _nodes.Clear();
            public IEnumerator GetEnumerator() => _nodes.GetEnumerator();
        }

        public TreeNodeCollection Nodes { get; } = new TreeNodeCollection();
        public TreeNode SelectedNode { get; set; }
        public bool HideSelection { get; set; }
        public bool Sorted { get; set; }
        public int ImageIndex { get; set; }
        public int SelectedImageIndex { get; set; }
        public ImageList ImageList { get; set; }

        public event TreeViewEventHandler AfterSelect;

        // Layout/position no-ops
        public int Left { get; set; }
        public int Top { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public bool Visible { get => IsVisible; set => IsVisible = value; }
        public bool IsVisible { get; set; } = true;
        public bool Enabled { get => IsEnabled; set => IsEnabled = value; }
        public bool IsEnabled { get; set; } = true;
        public Avalonia.Layout.HorizontalAlignment Anchor { get; set; }
        public System.Drawing.Font Font { get; set; }

        public void BeginUpdate() { }
        public void EndUpdate() { }
        public void Refresh() { }
    }

    /// <summary>WinForms FlowLayoutPanel → Avalonia StackPanel.</summary>
    public class FlowLayoutPanel : Avalonia.Controls.StackPanel
    {
        public bool AutoSize { get; set; }
        public new int FlowDirection { get; set; }
        public Avalonia.Controls.Controls Controls => Children;
        public int Left { get; set; }
        public int Top { get; set; }
        public int Right { get; set; }
        public int Bottom { get; set; }
        public new int Width { get; set; }
        public new int Height { get; set; }
        public System.Drawing.Point Location { get; set; }
        public System.Drawing.Size Size { get; set; }
        public Avalonia.Layout.HorizontalAlignment Anchor { get; set; }
    }

    /// <summary>WinForms TableLayoutPanel → Avalonia Grid.</summary>
    public class TableLayoutPanel : Avalonia.Controls.Grid
    {
        public bool AutoSize { get; set; }
        public int ColumnCount { get; set; }
        public int RowCount { get; set; }
        public Avalonia.Controls.Controls Controls => Children;
    }

    /// <summary>WinForms SplitContainer → minimal stub.</summary>
    public class SplitContainer : Avalonia.Controls.Grid
    {
        public Avalonia.Controls.StackPanel Panel1 { get; } = new Avalonia.Controls.StackPanel();
        public Avalonia.Controls.StackPanel Panel2 { get; } = new Avalonia.Controls.StackPanel();
        public int SplitterDistance { get; set; }
        public bool FixedPanel { get; set; }
    }

    /// <summary>WinForms TabPage → Avalonia TabItem.</summary>
    public class TabPage : Avalonia.Controls.TabItem
    {
        protected override Type StyleKeyOverride => typeof(Avalonia.Controls.TabItem);
        public string Text { get => Header?.ToString() ?? ""; set => Header = value; }
        public Avalonia.Controls.Controls Controls { get; } = new Avalonia.Controls.Controls();
    }

    /// <summary>WinForms ContextMenuStrip → Avalonia ContextMenu.</summary>
    public class ContextMenuStrip : Avalonia.Controls.ContextMenu
    {
        public new event System.EventHandler Opening;
        // Items inherited from ContextMenu
        public void Show(Avalonia.Controls.Control control, System.Drawing.Point position)
            => Open(control);
        public void Show(Avalonia.Controls.Control control, Avalonia.Point position)
            => Open(control);
    }

    /// <summary>WinForms ToolStripMenuItem → Avalonia MenuItem.</summary>
    public class ToolStripMenuItem : Avalonia.Controls.MenuItem
    {
        public string Text { get => Header?.ToString() ?? ""; set => Header = value; }
        public Avalonia.Controls.ItemCollection DropDownItems => Items;
    }

    /// <summary>WinForms PaintEventArgs stub.</summary>
    public class PaintEventArgs : System.EventArgs
    {
        public System.Drawing.Graphics Graphics { get; }
    }

    public enum DragAction { Continue, Drop, Cancel }

    /// <summary>WinForms QueryContinueDragEventArgs stub.</summary>
    public class QueryContinueDragEventArgs : System.EventArgs
    {
        public bool Cancel { get; set; }
        public bool EscapePressed { get; }
        public object Action { get; set; }
        public int KeyState { get; set; }
    }

    /// <summary>WinForms DragEventArgs → maps to Avalonia.Input.DragEventArgs via alias.</summary>
    public class DragEventArgs : System.EventArgs
    {
        public Avalonia.Input.IDataObject Data { get; }
        public int X { get; }
        public int Y { get; }
        public Avalonia.Input.DragDropEffects Effect { get; set; }
        public Avalonia.Input.DragDropEffects AllowedEffects { get; }
    }

    public delegate void ColumnClickEventHandler(object sender, ColumnClickEventArgs e);

    /// <summary>WinForms ColumnClickEventArgs stub.</summary>
    public class ColumnClickEventArgs : System.EventArgs
    {
        public int Column { get; }
        public ColumnClickEventArgs(int column) { Column = column; }
    }

    /// <summary>WinForms ColumnWidthChangedEventArgs stub.</summary>
    public class ColumnWidthChangedEventArgs : System.EventArgs
    {
        public int ColumnIndex { get; }
        public ColumnWidthChangedEventArgs(int columnIndex) { ColumnIndex = columnIndex; }
    }

    /// <summary>WinForms SplitterEventArgs stub.</summary>
    public class SplitterEventArgs : System.EventArgs
    {
        public int SplitX { get; }
        public int SplitY { get; }
        public SplitterEventArgs(int splitX, int splitY) { SplitX = splitX; SplitY = splitY; }
    }

    /// <summary>WinForms ToolTip stub — Avalonia uses attached ToolTip.Tip property instead.</summary>
    public class ToolTip
    {
        public bool ShowAlways { get; set; }
        public void SetToolTip(Avalonia.Controls.Control control, string text)
        {
            if (control != null)
                Avalonia.Controls.ToolTip.SetTip(control, text);
        }
    }

    // ── WinForms-compat wrappers for common Avalonia controls ─────────────────
    // These extend Avalonia built-ins with WinForms properties used in business logic.

    /// <summary>WinForms-compat TextBox — extends Avalonia TextBox.</summary>
    public class TextBoxCompat : Avalonia.Controls.TextBox
    {
        public bool ReadOnly { get => IsReadOnly; set => IsReadOnly = value; }
        public bool Enabled { get => IsEnabled; set => IsEnabled = value; }
        public bool Visible { get => IsVisible; set => IsVisible = value; }
        public string Content { get => Text; set => Text = value; }
        public System.Drawing.Point Location { get; set; }
        public System.Drawing.Size Size { get; set; }
        public int Left { get; set; }
        public int Right { get; set; }
        public int Top { get; set; }
        public int Bottom { get; set; }
        public new int Width { get; set; }
        public new int Height { get; set; }
        public System.Drawing.Color BackColor { get; set; }
        public System.Drawing.Color ForeColor { get; set; }
        public Avalonia.Layout.HorizontalAlignment Anchor { get; set; }
        public bool AutoSize { get; set; }
        public object TextAlign { get; set; }
        public bool AutoScrollPosition { get; set; }
        public System.Drawing.Font Font { get; set; }
        public new string Name { get => base.Name; set => base.Name = value; }
        public void SuspendLayout() { }
        public void ResumeLayout(bool performLayout = false) { }
        public new void SelectAll() => SelectionStart = 0;
        public int SelectionLength
        {
            get => SelectionEnd - SelectionStart;
            set => SelectionEnd = SelectionStart + value;
        }
    }

    /// <summary>WinForms-compat Label — extends Avalonia Label.</summary>
    public class LabelCompat : Avalonia.Controls.Label
    {
        public string Text { get => Content?.ToString() ?? ""; set => Content = value; }
        public bool Enabled { get => IsEnabled; set => IsEnabled = value; }
        public bool Visible { get => IsVisible; set => IsVisible = value; }
        public System.Drawing.Point Location { get; set; }
        public System.Drawing.Size Size { get; set; }
        public int Left { get; set; }
        public int Right { get; set; }
        public int Top { get; set; }
        public int Bottom { get; set; }
        public new int Width { get; set; }
        public new int Height { get; set; }
        public System.Drawing.Color BackColor { get; set; }
        public System.Drawing.Color ForeColor { get; set; }
        public Avalonia.Layout.HorizontalAlignment Anchor { get; set; }
        public bool AutoSize { get; set; }
        public object TextAlign { get; set; }
        public System.Drawing.Font Font { get; set; }
        public new string Name { get => base.Name; set => base.Name = value; }
        public event EventHandler Click;
        public LabelCompat() { PointerPressed += (s, e) => Click?.Invoke(this, EventArgs.Empty); }
    }

    /// <summary>WinForms-compat Button — extends Avalonia Button.</summary>
    public class ButtonCompat : Avalonia.Controls.Button
    {
        public bool Enabled { get => IsEnabled; set => IsEnabled = value; }
        public bool Visible { get => IsVisible; set => IsVisible = value; }
        public string Text { get => Content?.ToString() ?? ""; set => Content = value; }
        public System.Drawing.Image Image { get; set; }
        public System.Drawing.Point Location { get; set; }
        public System.Drawing.Size Size { get; set; }
        public int Left { get; set; }
        public int Right { get; set; }
        public int Top { get; set; }
        public int Bottom { get; set; }
        public new int Width { get; set; }
        public new int Height { get; set; }
        public System.Drawing.Color BackColor { get; set; }
        public System.Drawing.Color ForeColor { get; set; }
        public Avalonia.Layout.HorizontalAlignment Anchor { get; set; }
        public bool AutoSize { get; set; }
        public System.Drawing.Font Font { get; set; }
        public new string Name { get => base.Name; set => base.Name = value; }
    }

    /// <summary>WinForms-compat CheckBox — extends Avalonia CheckBox with WinForms API.</summary>
    public class CheckBoxCompat2 : Avalonia.Controls.CheckBox
    {
        public new bool Checked { get => IsChecked == true; set => IsChecked = value; }
        public bool Enabled { get => IsEnabled; set => IsEnabled = value; }
        public bool Visible { get => IsVisible; set => IsVisible = value; }
        public string Text { get => Content?.ToString() ?? ""; set => Content = value; }
        public System.Drawing.Point Location { get; set; }
        public System.Drawing.Size Size { get; set; }
        public int Left { get; set; }
        public int Right { get; set; }
        public int Top { get; set; }
        public int Bottom { get; set; }
        public new int Width { get; set; }
        public new int Height { get; set; }
        public System.Drawing.Color BackColor { get; set; }
        public System.Drawing.Color ForeColor { get; set; }
        public Avalonia.Layout.HorizontalAlignment Anchor { get; set; }
        public bool AutoSize { get; set; }
        public object CheckAlign { get; set; }
        public System.Drawing.Font Font { get; set; }
        public new string Name { get => base.Name; set => base.Name = value; }
        public CheckState CheckState
        {
            get
            {
                if (IsChecked == null) return CheckState.Indeterminate;
                return IsChecked == true ? CheckState.Checked : CheckState.Unchecked;
            }
            set
            {
                if (value == CheckState.Indeterminate) IsChecked = null;
                else IsChecked = value == CheckState.Checked;
            }
        }
        public event EventHandler CheckedChanged;
        public CheckBoxCompat2() { IsCheckedChanged += (s, e) => CheckedChanged?.Invoke(this, EventArgs.Empty); }
    }

    /// <summary>WinForms-compat ComboBox — extends Avalonia ComboBox with WinForms API.</summary>
    public class ComboBoxCompat : Avalonia.Controls.ComboBox
    {
        public bool Enabled { get => IsEnabled; set => IsEnabled = value; }
        public bool Visible { get => IsVisible; set => IsVisible = value; }
        public string Text
        {
            get => SelectedItem?.ToString() ?? "";
            set { /* no-op for compat */ }
        }
        public object Content { get => SelectedItem; set => SelectedItem = value; }
        public bool FormattingEnabled { get; set; }
        public bool Sorted { get; set; }
        public System.Drawing.Point Location { get; set; }
        public System.Drawing.Size Size { get; set; }
        public int Left { get; set; }
        public int Right { get; set; }
        public int Top { get; set; }
        public int Bottom { get; set; }
        public new int Width { get; set; }
        public new int Height { get; set; }
        public int DropDownWidth { get; set; }
        public int DropDownHeight { get; set; }
        public ComboBoxStyle DropDownStyle { get; set; }
        public System.Drawing.Color BackColor { get; set; }
        public System.Drawing.Color ForeColor { get; set; }
        public Avalonia.Layout.HorizontalAlignment Anchor { get; set; }
        public bool AutoSize { get; set; }
        public object DisplayMember { get; set; }
        public object ValueMember { get; set; }
        public object DataSource { get => ItemsSource; set => ItemsSource = (System.Collections.IEnumerable)value; }
        public System.Drawing.Font Font { get; set; }
        public new string Name { get => base.Name; set => base.Name = value; }
        public void Select(int start, int length) { /* no-op in Avalonia ComboBox */ }
        public event EventHandler TextChanged;
        public new event EventHandler SelectionChanged;
        public event EventHandler DragDrop;
        public event EventHandler DragEnter;
        public event EventHandler DragOver;
        public event EventHandler QueryContinueDrag;
        public void SelectAll() { }
        public int FindStringExact(string s) { int i = 0; foreach (var item in Items) { if (item?.ToString() == s) return i; i++; } return -1; }
    }

    /// <summary>WinForms ListBox compat — wraps Avalonia ListBox with WinForms-compatible API.</summary>
    public class ListBoxCompat : Avalonia.Controls.ListBox
    {
        public bool Sorted { get; set; }            // no-op: Avalonia has no built-in sort
        public bool Enabled { get => IsEnabled; set => IsEnabled = value; }
        public bool IntegralHeight { get; set; }
        public int ItemHeight { get; set; }
        public Avalonia.Layout.HorizontalAlignment Anchor { get; set; }
        public System.Drawing.Point Location { get; set; }
        public System.Drawing.Size Size { get; set; }
        public new string Name { get => base.Name; set => base.Name = value; }
        public void BeginUpdate() { }
        public void EndUpdate() { }
        public void ClearSelected() { SelectedIndex = -1; }
        public event EventHandler SelectedIndexChanged;
        public ListBoxCompat()
        {
            SelectionChanged += (s, e) => SelectedIndexChanged?.Invoke(this, EventArgs.Empty);
        }
    }

    /// <summary>WinForms FileDialog compat base — replaces System.Windows.Forms.FileDialog.</summary>
    public abstract class FileDialogCompat
    {
        public bool AddExtension { get; set; } = true;
        public bool CheckFileExists { get; set; }
        public bool CheckPathExists { get; set; } = true;
        public string DefaultExt { get; set; } = "";
        public bool DereferenceLinks { get; set; } = true;
        public string FileName { get; set; } = "";
        public string Filter { get; set; } = "";
        public int FilterIndex { get; set; } = 1;
        public bool RestoreDirectory { get; set; }
        public string InitialDirectory { get; set; } = "";
        public bool ReadOnlyChecked { get; set; }
        public bool ShowHelp { get; set; }
        public bool ShowReadOnly { get; set; }
        public string Title { get; set; } = "";
        public bool ValidateNames { get; set; } = true;
        // Synchronous stub — always returns Cancel; real async show done separately
        public SimPe.DialogResult ShowDialog() => SimPe.DialogResult.Cancel;
        public SimPe.DialogResult ShowDialog(object owner) => SimPe.DialogResult.Cancel;
    }

    /// <summary>WinForms OpenFileDialog compat stub.</summary>
    public class OpenFileDialogCompat : FileDialogCompat
    {
        public string[] FileNames => FileName == "" ? new string[0] : new string[] { FileName };
        public bool Multiselect { get; set; }
        public Task<string[]> ShowAsync(Avalonia.Controls.Window parent = null) => Task.FromResult(new string[0]);
    }

    public class SaveFileDialogCompat : FileDialogCompat
    {
        public Task<string> ShowAsync(Avalonia.Controls.Window parent = null) => Task.FromResult((string)null);
    }

    /// <summary>WinForms-compat Panel — extends Avalonia StackPanel with WinForms layout API (no-ops).</summary>
    public class StackPanelCompat : Avalonia.Controls.StackPanel
    {
        public System.Drawing.Point Location { get; set; }
        public System.Drawing.Size Size { get; set; }
        public int Left { get; set; }
        public int Right { get; set; }
        public int Top { get; set; }
        public int Bottom { get; set; }
        public bool AutoSize { get; set; }
        public bool AutoScroll { get; set; }
        public System.Drawing.Point AutoScrollPosition { get; set; }
        public bool Visible { get => IsVisible; set => IsVisible = value; }
        public Avalonia.Controls.Controls Controls => Children;
        public event EventHandler Paint;
        public event EventHandler Resize;
        public new string Name { get => base.Name; set => base.Name = value; }
    }

    /// <summary>Proxy for TabControl.TabPages — maps to Avalonia TabControl.Items.</summary>
    public class TabPageCollection
    {
        private readonly Avalonia.Controls.TabControl _owner;
        public TabPageCollection(Avalonia.Controls.TabControl owner) { _owner = owner; }
        public void Add(object tab) => _owner.Items.Add(tab);
        public void Remove(object tab) => _owner.Items.Remove(tab);
        public bool Contains(object tab) => _owner.Items.Contains(tab);
    }

    /// <summary>WinForms-compat TabControl — extends Avalonia TabControl with TabPages.</summary>
    public class TabControlCompat : Avalonia.Controls.TabControl
    {
        public TabPageCollection TabPages { get; }
        public TabControlCompat() { TabPages = new TabPageCollection(this); }
        public new string Name { get => base.Name; set => base.Name = value; }
    }

    [Flags]
    public enum AnchorStyles { None = 0, Top = 1, Bottom = 2, Left = 4, Right = 8 }

    public static class Cursors
    {
        public static object Default { get; } = null;
        public static object Hand { get; } = null;
        public static object Arrow { get; } = null;
        public static object WaitCursor { get; } = null;
        public static object IBeam { get; } = null;
        public static object Cross { get; } = null;
        public static object SizeAll { get; } = null;
    }

    /// <summary>Extension methods for Avalonia types that need WinForms-compatible APIs.</summary>
    public static class AvaloniaCompatExtensions
    {
        public static void AddRange(this Avalonia.Controls.ItemCollection items, object[] arr)
        {
            if (arr == null) return;
            foreach (var item in arr) items.Add(item);
        }

        public static void AddRange(this Avalonia.Controls.ItemCollection items, System.Collections.IEnumerable enumerable)
        {
            if (enumerable == null) return;
            foreach (var item in enumerable) items.Add(item);
        }
    }
}
