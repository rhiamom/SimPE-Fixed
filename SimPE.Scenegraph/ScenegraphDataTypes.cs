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
        }

        public class SubItem
        {
            public string Text { get; set; }
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
        public ListViewItem(string[] items)
        {
            Text = items.Length > 0 ? items[0] : "";
            for (int i = 1; i < items.Length; i++) SubItems.Add(items[i]);
        }

        public bool Checked { get; set; }
        public void EnsureVisible() { }
        public ListViewItem Clone() { return (ListViewItem)MemberwiseClone(); }
        /// <summary>Back-reference to the owning ListView (set by ListViewItemCollection.Add).</summary>
        public ListView ListView { get; set; }
        public bool UseItemStyleForSubItems { get; set; } = true;
    }

    // ── ListView ─────────────────────────────────────────────────────────────

    /// <summary>Minimal ListView — replaces System.Windows.Forms.ListView.</summary>
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
        public object Tag { get; set; }
        public bool Focused { get; set; }
        public bool Enabled { get; set; } = true;
        public bool MultiSelect { get; set; } = true;
        public bool HideSelection { get; set; }
        public bool FullRowSelect { get; set; }
        public bool GridLines { get; set; }
        public ImageList LargeImageList { get; set; }
        public ImageList SmallImageList { get; set; }
        public System.Drawing.Font Font { get; set; }
        public object View { get; set; }
        public bool UseCompatibleStateImageBehavior { get; set; }
        public bool CheckBoxes { get; set; }
        public System.Drawing.Color BackColor { get; set; }

        // Layout/position properties (no-ops in Avalonia port)
        public int Left { get; set; }
        public int Top { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public new Avalonia.Controls.Control Parent { get; set; }
        public bool Visible { get => IsVisible; set => IsVisible = value; }
        public bool IsVisible { get; set; } = true;
        public Avalonia.Layout.HorizontalAlignment Anchor { get; set; }
        public ListViewItem FocusedItem => Items.Count > 0 ? Items[0] : null;

        public event EventHandler SelectedIndexChanged;
        public event EventHandler SelectionChanged;
        public event EventHandler Click;
        public event EventHandler DoubleClick;
        public event EventHandler ColumnClick;
        public event EventHandler ItemActivate;
        public event EventHandler Resize;
        public object Activation { get; set; }

        public string Name { get; set; } = "";
        public object ListViewItemSorter { get; set; }
        public void SelectAll() { foreach (object o in Items) ((ListViewItem)o).Selected = true; }

        // No-ops: update batching has no effect in this compat stub
        public void BeginUpdate() { }
        public void EndUpdate() { }
        public void Refresh() { }
        public void Sort() { }

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
        public bool Checked
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
    /// Minimal PictureBox compat — replaces System.Windows.Forms.PictureBox.
    /// Holds a System.Drawing.Image; Avalonia rendering is a separate concern.
    /// </summary>
    public class PictureBoxCompat : Avalonia.Controls.Control
    {
        /// <summary>The current image (System.Drawing.Image).</summary>
        public System.Drawing.Image Image { get; set; }
        public System.Drawing.Size Size { get; set; } = new System.Drawing.Size(100, 100);
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
        public new Avalonia.Layout.HorizontalAlignment Anchor { get; set; }
        public Control ParentControl { get; set; }
    }

    // ── GroupBox compat ───────────────────────────────────────────────────────

    /// <summary>WinForms GroupBox — extends Avalonia ContentControl.</summary>
    public class GroupBox : Avalonia.Controls.ContentControl
    {
        public class ControlCollection
        {
            private readonly List<object> _controls = new List<object>();
            public int Count => _controls.Count;
            public void Add(object c) => _controls.Add(c);
            public void Clear() => _controls.Clear();
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
        }
    }

    // ── PictureBox ────────────────────────────────────────────────────────────

    /// <summary>Minimal PictureBox — replaces System.Windows.Forms.PictureBox.</summary>
    public class PictureBox : Avalonia.Controls.Control
    {
        public System.Drawing.Image Image { get; set; }
        public new System.Drawing.Size Size { get; set; } = new System.Drawing.Size(100, 100);
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
        public new System.Drawing.Size Size { get; set; }
        public new int Width { get; set; }
        public new int Height { get; set; }
        public new string Name { get => base.Name; set => base.Name = value; }

        public event LinkLabelLinkClickedEventHandler LinkClicked;

        public LinkLabel()
        {
            Click += (s, e) => LinkClicked?.Invoke(this, new LinkLabelLinkClickedEventArgs { Link = new LinkData { Data =Tag } });
        }
    }

    // ── MessageBox / DialogResult ─────────────────────────────────────────────

    public enum DialogResult { None, OK, Cancel, Yes, No, Abort, Retry, Ignore }
    public enum MessageBoxButtons { OK, OKCancel, YesNo, YesNoCancel, AbortRetryIgnore, RetryCancel }
    public enum MessageBoxIcon { None, Error, Warning, Information, Question, Hand, Stop, Asterisk, Exclamation }

    /// <summary>Async MessageBox replacement for Avalonia (no blocking UI thread).</summary>
    public static class MessageBox
    {
        public static async Task<DialogResult> ShowAsync(
            string text,
            string caption = "",
            MessageBoxButtons buttons = MessageBoxButtons.OK,
            MessageBoxIcon icon = MessageBoxIcon.None)
        {
            var tcs = new TaskCompletionSource<DialogResult>();
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

            void AddBtn(string label, DialogResult res)
            {
                var btn = new Avalonia.Controls.Button { Content = label, MinWidth = 80 };
                btn.Click += (s, e) => { win.Close(); tcs.TrySetResult(res); };
                btnRow.Children.Add(btn);
            }

            switch (buttons)
            {
                case MessageBoxButtons.OK:              AddBtn("OK", DialogResult.OK); break;
                case MessageBoxButtons.OKCancel:        AddBtn("OK", DialogResult.OK); AddBtn("Cancel", DialogResult.Cancel); break;
                case MessageBoxButtons.YesNo:           AddBtn("Yes", DialogResult.Yes); AddBtn("No", DialogResult.No); break;
                case MessageBoxButtons.YesNoCancel:     AddBtn("Yes", DialogResult.Yes); AddBtn("No", DialogResult.No); AddBtn("Cancel", DialogResult.Cancel); break;
                case MessageBoxButtons.RetryCancel:     AddBtn("Retry", DialogResult.Retry); AddBtn("Cancel", DialogResult.Cancel); break;
                case MessageBoxButtons.AbortRetryIgnore: AddBtn("Abort", DialogResult.Abort); AddBtn("Retry", DialogResult.Retry); AddBtn("Ignore", DialogResult.Ignore); break;
                default: AddBtn("OK", DialogResult.OK); break;
            }

            panel.Children.Add(btnRow);
            win.Content = panel;
            win.Closed += (s, e) => tcs.TrySetResult(DialogResult.Cancel);
            win.Show();
            return await tcs.Task;
        }

        /// <summary>Synchronous wrapper — shows the dialog without awaiting. Returns Cancel immediately.</summary>
        public static DialogResult Show(
            string text,
            string caption = "",
            MessageBoxButtons buttons = MessageBoxButtons.OK,
            MessageBoxIcon icon = MessageBoxIcon.None)
        {
            _ = ShowAsync(text, caption, buttons, icon);
            return DialogResult.Cancel;
        }

        // Overloads accepting SimPe.MessageBoxButtons/Icon (from SimPe.WorkSpaceHelper)
        public static async Task<DialogResult> ShowAsync(
            string text, string caption,
            SimPe.MessageBoxButtons buttons, SimPe.MessageBoxIcon icon)
            => await ShowAsync(text, caption, (MessageBoxButtons)(int)buttons, (MessageBoxIcon)(int)icon);

        public static async Task<DialogResult> ShowAsync(
            string text, string caption,
            SimPe.MessageBoxButtons buttons)
            => await ShowAsync(text, caption, (MessageBoxButtons)(int)buttons);

        public static DialogResult Show(
            string text, string caption,
            SimPe.MessageBoxButtons buttons, SimPe.MessageBoxIcon icon)
            => Show(text, caption, (MessageBoxButtons)(int)buttons, (MessageBoxIcon)(int)icon);

        public static DialogResult Show(
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
        public int FlowDirection { get; set; }
        public new Avalonia.Controls.Controls Controls => Children;
        public int Left { get; set; }
        public int Top { get; set; }
        public int Right { get; set; }
        public int Bottom { get; set; }
        public new int Width { get; set; }
        public new int Height { get; set; }
        public System.Drawing.Point Location { get; set; }
        public new System.Drawing.Size Size { get; set; }
        public Avalonia.Layout.HorizontalAlignment Anchor { get; set; }
    }

    /// <summary>WinForms TableLayoutPanel → Avalonia Grid.</summary>
    public class TableLayoutPanel : Avalonia.Controls.Grid
    {
        public bool AutoSize { get; set; }
        public int ColumnCount { get; set; }
        public int RowCount { get; set; }
        public new Avalonia.Controls.Controls Controls => Children;
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
        public string Text { get => Header?.ToString() ?? ""; set => Header = value; }
        public new Avalonia.Controls.Controls Controls { get; } = new Avalonia.Controls.Controls();
    }

    /// <summary>WinForms ContextMenuStrip → Avalonia ContextMenu.</summary>
    public class ContextMenuStrip : Avalonia.Controls.ContextMenu
    {
        public event System.EventHandler Opening;
        // Items inherited from ContextMenu
        public void Show(Avalonia.Controls.Control control, System.Drawing.Point position)
            => Open(control);
        public void Show(Avalonia.Controls.Control control, Avalonia.Point position)
            => Open(control);
    }

    /// <summary>WinForms ToolStripMenuItem → Avalonia MenuItem.</summary>
    public class ToolStripMenuItem : Avalonia.Controls.MenuItem
    {
        public new string Text { get => Header?.ToString() ?? ""; set => Header = value; }
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
        public string Content { get => Text; set => Text = value; }
        public System.Drawing.Point Location { get; set; }
        public new System.Drawing.Size Size { get; set; }
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
        public new System.Drawing.Font Font { get; set; }
        public new string Name { get => base.Name; set => base.Name = value; }
        public void SuspendLayout() { }
        public void ResumeLayout(bool performLayout = false) { }
        public void SelectAll() => SelectionStart = 0;
    }

    /// <summary>WinForms-compat Label — extends Avalonia Label.</summary>
    public class LabelCompat : Avalonia.Controls.Label
    {
        public string Text { get => Content?.ToString() ?? ""; set => Content = value; }
        public bool Enabled { get => IsEnabled; set => IsEnabled = value; }
        public System.Drawing.Point Location { get; set; }
        public new System.Drawing.Size Size { get; set; }
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
        public new System.Drawing.Font Font { get; set; }
        public new string Name { get => base.Name; set => base.Name = value; }
        public event EventHandler Click;
        public LabelCompat() { PointerPressed += (s, e) => Click?.Invoke(this, EventArgs.Empty); }
    }

    /// <summary>WinForms-compat Button — extends Avalonia Button.</summary>
    public class ButtonCompat : Avalonia.Controls.Button
    {
        public bool Enabled { get => IsEnabled; set => IsEnabled = value; }
        public string Text { get => Content?.ToString() ?? ""; set => Content = value; }
        public System.Drawing.Point Location { get; set; }
        public new System.Drawing.Size Size { get; set; }
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
        public new System.Drawing.Font Font { get; set; }
        public new string Name { get => base.Name; set => base.Name = value; }
    }

    /// <summary>WinForms-compat CheckBox — extends Avalonia CheckBox with WinForms API.</summary>
    public class CheckBoxCompat2 : Avalonia.Controls.CheckBox
    {
        public bool Checked { get => IsChecked == true; set => IsChecked = value; }
        public bool Enabled { get => IsEnabled; set => IsEnabled = value; }
        public bool Visible { get => IsVisible; set => IsVisible = value; }
        public string Text { get => Content?.ToString() ?? ""; set => Content = value; }
        public System.Drawing.Point Location { get; set; }
        public new System.Drawing.Size Size { get; set; }
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
        public new System.Drawing.Font Font { get; set; }
        public new string Name { get => base.Name; set => base.Name = value; }
        public event EventHandler CheckedChanged;
        public CheckBoxCompat2() { IsCheckedChanged += (s, e) => CheckedChanged?.Invoke(this, EventArgs.Empty); }
    }

    /// <summary>WinForms-compat ComboBox — extends Avalonia ComboBox with WinForms API.</summary>
    public class ComboBoxCompat : Avalonia.Controls.ComboBox
    {
        public bool Enabled { get => IsEnabled; set => IsEnabled = value; }
        public string Text
        {
            get => SelectedItem?.ToString() ?? "";
            set { /* no-op for compat */ }
        }
        public new object Content { get => SelectedItem; set => SelectedItem = value; }
        public bool FormattingEnabled { get; set; }
        public bool Sorted { get; set; }
        public System.Drawing.Point Location { get; set; }
        public new System.Drawing.Size Size { get; set; }
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
        public new System.Drawing.Font Font { get; set; }
        public new string Name { get => base.Name; set => base.Name = value; }
        public void Select(int start, int length) { /* no-op in Avalonia ComboBox */ }
        public new event EventHandler TextChanged;
        public new event EventHandler SelectionChanged;
        public event EventHandler DragDrop;
        public event EventHandler DragEnter;
        public event EventHandler DragOver;
        public event EventHandler QueryContinueDrag;
        public void SelectAll() { }
        public int FindStringExact(string s) { int i = 0; foreach (var item in Items) { if (item?.ToString() == s) return i; i++; } return -1; }
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
        public DialogResult ShowDialog() => DialogResult.Cancel;
        public DialogResult ShowDialog(object owner) => DialogResult.Cancel;
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
        public new System.Drawing.Size Size { get; set; }
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
