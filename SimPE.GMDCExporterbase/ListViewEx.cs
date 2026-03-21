/***************************************************************************
 *   Copyright (C) 2005 by Ambertation                                     *
 *   quaxi@ambertation.de                                                  *
 *                                                                         *
 *   Copyright (C) 2025 by GramzeSweatshop                                 *
 *   rhiamom@mac.com                                                       *
 *                                                                         *
 *   This program is free software; you can redistribute it and/or modify  *
 *   it under the terms of the GNU General Public License as published by  *
 *   the Free Software Foundation; either version 2 of the License, or     *
 *   (at your option) any later version.                                   *
 *                                                                         *
 *   This program is distributed in the hope that it will be useful,       *
 *   but WITHOUT ANY WARRANTY; without even the implied warranty of        *
 *   MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the         *
 *   GNU General Public License for more details.                          *
 *                                                                         *
 *   You should have received a copy of the GNU General Public License     *
 *   along with this program; if not, write to the                         *
 *   Free Software Foundation, Inc.,                                       *
 *   59 Temple Place - Suite 330, Boston, MA  02111-1307, USA.             *
 ***************************************************************************/

// Ported from WinForms to Avalonia (Mac port).
// The P/Invoke embedded-control ListView is replaced with a stub that compiles
// but no-ops all WinForms-specific APIs.  Business logic in callers is unchanged.

using System;
using System.Collections.Generic;
using System.Drawing;

namespace System.Windows.Forms
{
    // ─── DockStyle enum ────────────────────────────────────────────────────────
    public enum DockStyle { None, Top, Bottom, Left, Right, Fill }

    // ─── View enum ─────────────────────────────────────────────────────────────
    public enum View { LargeIcon, SmallIcon, List, Details, Tile }

    // ─── BorderStyle enum ──────────────────────────────────────────────────────
    public enum BorderStyle { None, FixedSingle, Fixed3D }

    // ─── ColumnHeaderStyle enum ────────────────────────────────────────────────
    public enum ColumnHeaderStyle { None, Nonclickable, Clickable }

    // ─── FlatStyle enum ────────────────────────────────────────────────────────
    public enum FlatStyle { Flat, Popup, Standard, System }

    // ─── ComboBoxStyle enum ────────────────────────────────────────────────────
    public enum ComboBoxStyle { Simple, DropDown, DropDownList }

    // ─── ScrollBars enum ──────────────────────────────────────────────────────
    public enum ScrollBars { None, Horizontal, Vertical, Both }

    // ─── ImeMode enum ─────────────────────────────────────────────────────────
    public enum ImeMode { NoControl, On, Off, Disable, Hiragana, Katakana, KatakanaHalf, AlphaFull, Alpha, HangulFull, Hangul, Close, OnHalf, Inherit }

    // ─── DragDropEffects enum ─────────────────────────────────────────────────
    public enum DragDropEffects { None = 0, Copy = 1, Move = 2, Link = 4, Scroll = unchecked((int)0x80000000), All = -1 }

    // ─── DragEventHandler ────────────────────────────────────────────────────
    public delegate void DragEventHandler(object sender, DragEventArgs e);

    // ContentAlignment: use System.Drawing.ContentAlignment directly (available in .NET 8)

    // AnchorStyles is defined in SimPE.WorkSpaceHelper/WinFormsStubs.cs

    // ─── MessageBoxButtons defined in WorkSpaceHelper (System.Windows.Forms) ───
    // Callers should use System.Windows.Forms.MessageBoxButtons.

    // ─── DialogResult (forwarded to SimPe) ────────────────────────────────────
    // Use SimPe.DialogResult directly.

    // ─── ColumnHeader ─────────────────────────────────────────────────────────
    public class ColumnHeader
    {
        public string Text  { get; set; } = "";
        public int    Width { get; set; } = 80;
        public int    Index { get; private set; }

        internal void SetIndex(int i) { Index = i; }
    }

    // ─── ListViewItem ─────────────────────────────────────────────────────────
    public class ListViewItem
    {
        public string Text     { get; set; } = "";
        public Color  ForeColor { get; set; } = Color.Black;
        public object Tag        { get; set; }
        public bool   Selected   { get; set; }
        public int    ImageIndex { get; set; } = -1;

        private readonly List<SubItem> _subItems = new List<SubItem>();
        public SubItemCollection SubItems { get; }

        public ListViewItem()           { SubItems = new SubItemCollection(_subItems); }
        public ListViewItem(string text) : this() { Text = text; }

        public class SubItem
        {
            public string Text { get; set; }
            public SubItem(string text) { Text = text; }
        }

        public class SubItemCollection
        {
            private readonly List<SubItem> _items;
            public SubItemCollection(List<SubItem> items) { _items = items; }
            public SubItem this[int i] => _items[i];
            public int    Count        => _items.Count;
            public SubItem Add(string text) { var s = new SubItem(text); _items.Add(s); return s; }
            public void    Clear()          { _items.Clear(); }
        }
    }

    // ─── ColumnHeaderCollection ───────────────────────────────────────────────
    public class ColumnHeaderCollection
    {
        private readonly List<ColumnHeader> _cols = new List<ColumnHeader>();
        public int Count => _cols.Count;
        public ColumnHeader this[int i] => _cols[i];

        public void Add(ColumnHeader h)
        {
            h.SetIndex(_cols.Count);
            _cols.Add(h);
        }

        public void AddRange(ColumnHeader[] headers)
        {
            foreach (var h in headers) Add(h);
        }

        public void Clear() { _cols.Clear(); }
    }

    // ─── ListViewItemCollection ───────────────────────────────────────────────
    public class ListViewItemCollection : System.Collections.Generic.IEnumerable<ListViewItem>
    {
        private readonly List<ListViewItem> _items = new List<ListViewItem>();
        public int Count => _items.Count;
        public ListViewItem this[int i] => _items[i];
        public void Add(ListViewItem item)    { _items.Add(item); }
        public void Remove(ListViewItem item) { _items.Remove(item); }
        public void Clear()                   { _items.Clear(); }
        public bool Contains(ListViewItem item) => _items.Contains(item);

        // stub for SelectedItems.Clear() called by old code
        public void ClearSelection() { foreach (var it in _items) it.Selected = false; }

        public System.Collections.Generic.IEnumerator<ListViewItem> GetEnumerator()
            => _items.GetEnumerator();
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
            => _items.GetEnumerator();
    }

    // ─── SelectedListViewItemCollection ───────────────────────────────────────
    public class SelectedListViewItemCollection
    {
        private readonly ListViewItemCollection _all;
        public SelectedListViewItemCollection(ListViewItemCollection all) { _all = all; }
        public int         Count    => 0;  // stub — embedded-control selection is no-op
        public ListViewItem this[int i] => _all[i];
        public void        Clear()  { }
    }

    // ─── Form ─────────────────────────────────────────────────────────────────
    /// <summary>
    /// Stub base class for WinForms Form; backs the Mac Avalonia port.
    /// Extend Avalonia.Controls.UserControl for real UI; this is for compile compatibility.
    /// </summary>
    public class Form : Avalonia.Controls.UserControl
    {
        public string Text     { get; set; } = "";
        public bool   ShowInTaskbar { get; set; } = true;
        public FormBorderStyle FormBorderStyle { get; set; }
        public FormStartPosition StartPosition { get; set; }
        public bool   MaximizeBox { get; set; } = true;
        public bool   MinimizeBox { get; set; } = true;
        public System.Drawing.Size AutoScaleBaseSize { get; set; }
        public System.Drawing.Size ClientSize { get; set; }

        public Panel.ControlCollection Controls    { get; } = new Panel.ControlCollection();
        public System.Drawing.Font     Font       { get; set; }
        public FormWindowState         WindowState { get; set; }

        public void ShowDialog()  { }
        public void Close()       { }

        public event System.ComponentModel.CancelEventHandler Closing;

        protected virtual void SuspendLayout()   { }
        protected virtual void ResumeLayout(bool performLayout = true) { }
        protected virtual void PerformLayout()   { }
        protected virtual void Dispose(bool disposing) { }
        public    void         Dispose() { Dispose(true); }
    }

    // ─── SelectionMode enum ───────────────────────────────────────────────────
    public enum SelectionMode { One, MultiSimple, MultiExtended, None }

    // ─── CheckState enum ─────────────────────────────────────────────────────
    public enum CheckState { Unchecked, Checked, Indeterminate }

    // ─── FormWindowState enum ─────────────────────────────────────────────────
    public enum FormWindowState { Normal, Minimized, Maximized }

    // ─── MessageBox stub ──────────────────────────────────────────────────────
    public static class MessageBox
    {
        public static SimPe.DialogResult Show(string text) { return SimPe.DialogResult.OK; }
        public static SimPe.DialogResult Show(string text, string caption) { return SimPe.DialogResult.OK; }
        public static SimPe.DialogResult Show(string text, string caption, MessageBoxButtons buttons) { return SimPe.DialogResult.OK; }
    }

    // ─── FormBorderStyle enum ─────────────────────────────────────────────────
    public enum FormBorderStyle
    {
        None, FixedSingle, Fixed3D, FixedDialog,
        Sizable, FixedToolWindow, SizableToolWindow
    }

    // ─── FormStartPosition enum ───────────────────────────────────────────────
    public enum FormStartPosition { Manual, CenterScreen, WindowsDefaultLocation, WindowsDefaultBounds, CenterParent }

    // ─── PictureBoxSizeMode enum ──────────────────────────────────────────────
    public enum PictureBoxSizeMode { Normal, StretchImage, AutoSize, CenterImage, Zoom }

    // ─── ColorDepth enum ─────────────────────────────────────────────────────
    public enum ColorDepth { Depth4Bit, Depth8Bit, Depth16Bit, Depth24Bit, Depth32Bit }

    // ─── LinkArea ─────────────────────────────────────────────────────────────
    public struct LinkArea
    {
        public int Start { get; set; }
        public int Length { get; set; }
        public LinkArea(int start, int length) { Start = start; Length = length; }
    }

    // ─── TreeNode ─────────────────────────────────────────────────────────────
    public class TreeNode
    {
        public string Text { get; set; }
        public object Tag  { get; set; }
        private readonly List<TreeNode> _nodes = new List<TreeNode>();
        public TreeNodeCollection Nodes { get; }
        public TreeNode() { Nodes = new TreeNodeCollection(_nodes); Text = ""; }
        public TreeNode(string text) : this() { Text = text; }

        public class TreeNodeCollection
        {
            private readonly List<TreeNode> _items;
            public TreeNodeCollection(List<TreeNode> items) { _items = items; }
            public TreeNode this[int i] => _items[i];
            public int Count => _items.Count;
            public void Add(TreeNode node) { _items.Add(node); }
            public void Clear() { _items.Clear(); }
            public System.Collections.Generic.IEnumerator<TreeNode> GetEnumerator()
                => _items.GetEnumerator();
        }
    }

    // ─── TreeViewEventArgs ────────────────────────────────────────────────────
    public class TreeViewEventArgs : EventArgs
    {
        public TreeNode Node { get; }
        public TreeViewEventArgs(TreeNode node) { Node = node; }
    }

    // ─── TreeViewEventHandler ─────────────────────────────────────────────────
    public delegate void TreeViewEventHandler(object sender, TreeViewEventArgs e);

    // ─── LinkLabelLinkClickedEventArgs ────────────────────────────────────────
    public class LinkLabelLinkClickedEventArgs : EventArgs
    {
        public LinkLabelLinkClickedEventArgs() { }
    }

    // ─── LinkLabelLinkClickedEventHandler ────────────────────────────────────
    public delegate void LinkLabelLinkClickedEventHandler(object sender, LinkLabelLinkClickedEventArgs e);

    // ─── CancelEventArgs / CancelEventHandler (if not in System.ComponentModel) ──
    // System.ComponentModel.CancelEventArgs already exists in .NET 8; no need to stub.

    // ─── ImageList ────────────────────────────────────────────────────────────
    public class ImageList
    {
        public System.Drawing.Size ImageSize { get; set; } = new System.Drawing.Size(16, 16);
        public ColorDepth ColorDepth { get; set; } = ColorDepth.Depth32Bit;
        public ImageListCollection Images { get; } = new ImageListCollection();

        public class ImageListCollection
        {
            private readonly List<System.Drawing.Image> _list = new List<System.Drawing.Image>();
            public int Count => _list.Count;
            public void Add(System.Drawing.Image img) { _list.Add(img); }
        }
    }

    // ─── ListView ─────────────────────────────────────────────────────────────
    public class ListView
    {
        public ColumnHeaderCollection Columns   { get; } = new ColumnHeaderCollection();
        public ListViewItemCollection Items      { get; } = new ListViewItemCollection();
        public SelectedListViewItemCollection SelectedItems
            => new SelectedListViewItemCollection(Items);

        public View              View          { get; set; } = View.Details;
        public bool              FullRowSelect  { get; set; }
        public bool              HideSelection  { get; set; }
        public bool              MultiSelect    { get; set; } = true;
        public bool              LabelEdit      { get; set; }
        public ColumnHeaderStyle HeaderStyle    { get; set; }
        public BorderStyle       BorderStyle    { get; set; }
        public ImageList         LargeImageList { get; set; }
        public object            Tag            { get; set; }
        public bool              IsEnabled      { get; set; } = true;
        public bool              Enabled        { get => IsEnabled; set => IsEnabled = value; }
        public System.Collections.IComparer ListViewItemSorter { get; set; }

        public System.Drawing.Font  Font     { get; set; }
        public System.Drawing.Color BackColor { get; set; }
        public System.Drawing.Point Location  { get; set; }
        public System.Drawing.Size  Size      { get; set; }
        public string  Name     { get; set; } = "";
        public int     TabIndex { get; set; }
        public bool    Visible  { get; set; } = true;
        public bool    TabStop  { get; set; }
        public bool    GridLines { get; set; }
        public bool    UseCompatibleStateImageBehavior { get; set; }
        public DockStyle Dock   { get; set; }
        public AnchorStyles Anchor { get; set; }
        public int     Left     { get; set; }
        public int     Top      { get; set; }
        public int     Width    { get => Size.Width;  set => Size = new System.Drawing.Size(value, Size.Height); }
        public int     Height   { get => Size.Height; set => Size = new System.Drawing.Size(Size.Width, value); }
        public object  Parent   { get; set; }
        public ContextMenuStrip ContextMenuStrip { get; set; }

        public event EventHandler SelectedIndexChanged;
        public void BeginUpdate() { }
        public void EndUpdate()   { }
    }

    // ─── TreeView ─────────────────────────────────────────────────────────────
    public class TreeView
    {
        public TreeNode.TreeNodeCollection Nodes { get; }
        public object Tag { get; set; }
        public bool HideSelection { get; set; }

        private readonly List<TreeNode> _nodes = new List<TreeNode>();
        public TreeView() { Nodes = new TreeNode.TreeNodeCollection(_nodes); }

        public event TreeViewEventHandler AfterSelect;
    }

    // ─── Panel ────────────────────────────────────────────────────────────────
    /// <summary>
    /// WinForms Panel stub backed by Avalonia.Controls.Panel so it can be
    /// returned as Avalonia.Controls.Control from GUIHandle implementations.
    /// </summary>
    public class Panel : Avalonia.Controls.Panel
    {
        public new ControlCollection Controls { get; } = new ControlCollection();
        public object Tag      { get; set; }
        public bool AutoScroll { get; set; }
        public int  Top        { get; set; }
        public int  Left       { get; set; }
        public new object Parent { get; set; }
        public AnchorStyles Anchor { get; set; }
        public new bool IsVisible { get; set; } = true;
        public bool Visible { get => IsVisible; set => IsVisible = value; }
        public System.Drawing.Color BackColor { get; set; }
        public System.Drawing.Font Font { get; set; }
        public int TabIndex { get; set; }
        public bool AutoSize { get; set; }
        public new System.Drawing.Size Size { get; set; } = new System.Drawing.Size(100, 100);
        public new System.Drawing.Point Location { get; set; }
        public new string Name { get; set; }
        public new Padding Margin { get; set; }

        public System.Drawing.Size AutoScrollMinSize { get; set; }
        private int _width = 100; private int _height = 100;
        public new int Width  { get => _width;  set { _width  = value; } }
        public new int Height { get => _height; set { _height = value; } }
        public new void SuspendLayout()  { }
        public new void ResumeLayout(bool performLayout = true) { }
        public new void PerformLayout()  { }

        public class ControlCollection
        {
            private readonly List<object> _list = new List<object>();
            public int Count => _list.Count;
            public void Add(object c) { _list.Add(c); }
            public void Remove(object c) { _list.Remove(c); }
        }
    }

    // ─── Button ───────────────────────────────────────────────────────────────
    public class Button
    {
        public string Text     { get; set; } = "";
        public bool   IsEnabled { get; set; } = true;
        public bool   Enabled  { get => IsEnabled; set => IsEnabled = value; }
        public object Tag      { get; set; }
        public FlatStyle FlatStyle { get; set; }
        public AnchorStyles Anchor { get; set; }
        public System.Drawing.Font  Font     { get; set; }
        public System.Drawing.Color BackColor { get; set; }
        public System.Drawing.Point Location  { get; set; }
        public System.Drawing.Size  Size      { get; set; }
        public System.Drawing.Color ForeColor { get; set; }
        public string  Name     { get; set; } = "";
        public int     TabIndex { get; set; }
        public bool    Visible  { get; set; } = true;
        public DockStyle Dock   { get; set; }
        public ImeMode ImeMode  { get; set; }
        public event EventHandler Click;
        protected virtual void OnClick(EventArgs e) => Click?.Invoke(this, e);
    }

    // ─── TextBox ──────────────────────────────────────────────────────────────
    public class TextBox
    {
        public string Text     { get; set; } = "";
        public bool   ReadOnly { get; set; }
        public bool   IsEnabled { get; set; } = true;
        public bool   Enabled  { get => IsEnabled; set => IsEnabled = value; }
        public AnchorStyles Anchor { get; set; }
        public System.Drawing.Font  Font      { get; set; }
        public System.Drawing.Color BackColor { get; set; }
        public System.Drawing.Point Location  { get; set; }
        public System.Drawing.Size  Size      { get; set; }
        public string  Name     { get; set; } = "";
        public int     TabIndex { get; set; }
        public bool    Visible  { get; set; } = true;
        public bool    Multiline { get; set; }
        public bool    TabStop  { get; set; }
        public DockStyle Dock   { get; set; }
        public ScrollBars ScrollBars { get; set; }
        public object  Tag      { get; set; }
        public event EventHandler TextChanged;
        protected virtual void OnTextChanged(EventArgs e) => TextChanged?.Invoke(this, e);
    }

    // ─── Label ────────────────────────────────────────────────────────────────
    public class Label
    {
        public string Text      { get; set; } = "";
        public bool   AutoSize  { get; set; }
        public AnchorStyles Anchor { get; set; }
        public System.Drawing.Font  Font      { get; set; }
        public System.Drawing.Color BackColor { get; set; }
        public System.Drawing.Size  Size      { get; set; }
        public System.Drawing.Point Location  { get; set; }
        public System.Drawing.ContentAlignment TextAlign { get; set; }
        public System.Drawing.Color ForeColor { get; set; }
        public string  Name     { get; set; } = "";
        public int     TabIndex { get; set; }
        public bool    Visible  { get; set; } = true;
        public ImeMode ImeMode  { get; set; }
    }

    // ─── LinkLabel ────────────────────────────────────────────────────────────
    public class LinkLabel
    {
        public string   Text      { get; set; } = "";
        public bool     AutoSize  { get; set; }
        public bool     Visible   { get; set; } = true;
        public bool     IsEnabled { get; set; } = true;
        public bool     Enabled   { get => IsEnabled; set => IsEnabled = value; }
        public bool     TabStop   { get; set; }
        public int      TabIndex  { get; set; }
        public LinkArea LinkArea  { get; set; }
        public AnchorStyles Anchor { get; set; }
        public bool     UseCompatibleTextRendering { get; set; }
        public System.Drawing.Font  Font     { get; set; }
        public System.Drawing.Color BackColor { get; set; }
        public System.Drawing.Point Location  { get; set; }
        public System.Drawing.Size  Size      { get; set; }
        public System.Drawing.ContentAlignment TextAlign { get; set; }
        public string   Name      { get; set; } = "";
        public ImeMode  ImeMode   { get; set; }
        public event LinkLabelLinkClickedEventHandler LinkClicked;
        protected virtual void OnLinkClicked(LinkLabelLinkClickedEventArgs e)
            => LinkClicked?.Invoke(this, e);
    }

    // ─── CheckBox ─────────────────────────────────────────────────────────────
    public class CheckBox
    {
        public string  Text      { get; set; } = "";
        public bool    Checked   { get; set; }
        public CheckState CheckState { get; set; }
        public FlatStyle FlatStyle  { get; set; }
        public object  Tag        { get; set; }
        public bool    Visible    { get; set; } = true;
        public bool    IsEnabled  { get; set; } = true;
        public bool    Enabled    { get => IsEnabled; set => IsEnabled = value; }
        public int     Width      { get; set; } = 100;
        public int     Height     { get; set; } = 24;
        public int     Top        { get; set; }
        public int     Left       { get; set; }
        public AnchorStyles Anchor { get; set; }
        public System.Drawing.Font  Font      { get; set; }
        public System.Drawing.Color BackColor { get; set; }
        public System.Drawing.Point Location  { get; set; }
        public System.Drawing.Size  Size      { get; set; }
        public string  Name     { get; set; } = "";
        public int     TabIndex { get; set; }
        public bool    TabStop  { get; set; }
        public object Parent { get; set; }
        public event EventHandler CheckedChanged;
        protected virtual void OnCheckedChanged(EventArgs e) => CheckedChanged?.Invoke(this, e);
    }

    // ─── ComboBox ─────────────────────────────────────────────────────────────
    public class ComboBox
    {
        private readonly System.Collections.ArrayList _items = new System.Collections.ArrayList();
        public ComboBoxItemCollection Items { get; }
        public int    SelectedIndex { get; set; } = -1;
        public object SelectedItem  => SelectedIndex >= 0 && SelectedIndex < _items.Count ? _items[SelectedIndex] : null;
        public bool   IsEnabled     { get; set; } = true;
        public bool   Enabled       { get => IsEnabled; set => IsEnabled = value; }
        public ComboBoxStyle DropDownStyle { get; set; }
        public object Tag      { get; set; }
        public System.Drawing.Font Font { get; set; }
        public string Text     { get => SelectedItem?.ToString() ?? ""; set { } }
        public bool   Sorted   { get; set; }
        public int    ItemHeight { get; set; }
        public AnchorStyles Anchor { get; set; }

        public System.Drawing.Point Location  { get; set; }
        public System.Drawing.Size  Size      { get; set; }
        public string  Name     { get; set; } = "";
        public int     TabIndex { get; set; }
        public bool    Visible  { get; set; } = true;

        public event EventHandler SelectedIndexChanged;
        public event EventHandler TextChanged;

        public ComboBox() { Items = new ComboBoxItemCollection(_items, this); }

        public class ComboBoxItemCollection
        {
            private readonly System.Collections.ArrayList _list;
            private readonly ComboBox _owner;
            public ComboBoxItemCollection(System.Collections.ArrayList list, ComboBox owner)
            { _list = list; _owner = owner; }
            public int Count => _list.Count;
            public object this[int i] { get => _list[i]; set => _list[i] = value; }
            public void Add(object item) { _list.Add(item); }
            public void Clear() { _list.Clear(); }
            public System.Collections.IEnumerator GetEnumerator() => _list.GetEnumerator();
        }

        protected virtual void OnSelectedIndexChanged(EventArgs e)
            => SelectedIndexChanged?.Invoke(this, e);
    }

    // ─── TabControl / TabPage ─────────────────────────────────────────────────
    public class TabControl
    {
        public TabPageCollection Controls { get; } = new TabPageCollection();
        public int SelectedIndex { get; set; }
        public object Tag { get; set; }
        public int Width { get; set; } = 300;
        public string Name { get; set; } = "";
        public int    TabIndex { get; set; }
        public System.Drawing.Size  Size     { get; set; }
        public System.Drawing.Point Location { get; set; }
        public AnchorStyles Anchor { get; set; }
        public void SuspendLayout()  { }
        public void ResumeLayout(bool performLayout = true) { }
        public void PerformLayout()  { }

        public class TabPageCollection
        {
            private readonly List<TabPage> _items = new List<TabPage>();
            public int Count => _items.Count;
            public TabPage this[int i] => _items[i];
            public void Add(TabPage p) { _items.Add(p); }
            public void Remove(TabPage p) { _items.Remove(p); }
            public void Clear() { _items.Clear(); }
        }
    }

    public class TabPage
    {
        public string Text     { get; set; } = "";
        public object Tag      { get; set; }
        public bool   Visible  { get; set; } = true;
        public string Name     { get; set; } = "";
        public int    TabIndex { get; set; }
        public System.Drawing.Size  Size     { get; set; }
        public System.Drawing.Point Location { get; set; }
        public AnchorStyles Anchor { get; set; }
        public Panel.ControlCollection Controls { get; } = new Panel.ControlCollection();
        public void SuspendLayout()  { }
        public void ResumeLayout(bool performLayout = true) { }
        public void PerformLayout()  { }
    }

    // ─── GroupBox ─────────────────────────────────────────────────────────────
    public class GroupBox
    {
        public string Text     { get; set; } = "";
        public string Name     { get; set; } = "";
        public int    TabIndex { get; set; }
        public bool   TabStop  { get; set; }
        public System.Drawing.Size  Size      { get; set; }
        public System.Drawing.Point Location  { get; set; }
        public System.Drawing.Font  Font      { get; set; }
        public System.Drawing.Color BackColor { get; set; }
        public AnchorStyles Anchor { get; set; }
        public Panel.ControlCollection Controls { get; } = new Panel.ControlCollection();
        public void SuspendLayout()  { }
        public void ResumeLayout(bool performLayout = true) { }
        public void PerformLayout()  { }
    }

    // ─── PictureBox ───────────────────────────────────────────────────────────
    public class PictureBox : System.ComponentModel.ISupportInitialize
    {
        public System.Drawing.Image Image            { get; set; }
        public System.Drawing.Image BackgroundImage  { get; set; }
        public PictureBoxSizeMode   SizeMode         { get; set; }
        public BorderStyle          BorderStyle      { get; set; }
        public System.Drawing.Size  Size             { get; set; } = new System.Drawing.Size(100, 100);
        public System.Drawing.Point Location         { get; set; }
        public System.Drawing.Color BackColor        { get; set; }
        public AnchorStyles         Anchor           { get; set; }
        public ImeMode              ImeMode          { get; set; }
        public ContextMenuStrip     ContextMenuStrip { get; set; }
        public string  Name     { get; set; } = "";
        public int     TabIndex { get; set; }
        public bool    TabStop  { get; set; }
        public bool    Visible  { get; set; } = true;
        public int     Width    { get => Size.Width;  set => Size = new System.Drawing.Size(value, Size.Height); }
        public int     Height   { get => Size.Height; set => Size = new System.Drawing.Size(Size.Width, value); }
        public event EventHandler SizeChanged;
        public void BeginInit() { }
        public void EndInit()   { }
        public void SuspendLayout()  { }
        public void ResumeLayout(bool performLayout = true) { }
    }

    // ─── SaveFileDialog ───────────────────────────────────────────────────────
    public class SaveFileDialog
    {
        public string FileName   { get; set; } = "";
        public string Filter     { get; set; } = "";
        public string Title      { get; set; } = "";
        public bool   OverwritePrompt { get; set; }
        public SimPe.DialogResult ShowDialog() { return SimPe.DialogResult.Cancel; }
    }

    // ─── OpenFileDialog ───────────────────────────────────────────────────────
    public class OpenFileDialog
    {
        public string FileName        { get; set; } = "";
        public string Filter          { get; set; } = "";
        public string Title           { get; set; } = "";
        public bool   CheckFileExists { get; set; }
        public int    FilterIndex     { get; set; } = 1;
        public SimPe.DialogResult ShowDialog() { return SimPe.DialogResult.Cancel; }
    }

    // ─── ContextMenuStrip ────────────────────────────────────────────────────
    public class ContextMenuStrip
    {
        public ToolStripItemCollection Items { get; } = new ToolStripItemCollection();
        public event System.ComponentModel.CancelEventHandler Opening;
    }

    public class ToolStripItemCollection
    {
        private readonly List<ToolStripItem> _items = new List<ToolStripItem>();
        public int Count => _items.Count;
        public ToolStripItem this[int i] => _items[i];
        public void Add(ToolStripItem item) { _items.Add(item); }
        public void AddRange(ToolStripItem[] items) { foreach (var it in items) _items.Add(it); }
        public void Clear() { _items.Clear(); }
    }

    // ─── ListBox ─────────────────────────────────────────────────────────────
    public class ListBox
    {
        private readonly System.Collections.ArrayList _items = new System.Collections.ArrayList();
        public ListBoxItemCollection Items { get; }
        public int  SelectedIndex  { get; set; } = -1;
        public object SelectedItem => SelectedIndex >= 0 && SelectedIndex < _items.Count ? _items[SelectedIndex] : null;
        public bool HorizontalScrollbar { get; set; }
        public bool IntegralHeight { get; set; }
        public bool Sorted         { get; set; }
        public bool AllowDrop      { get; set; }
        public SelectionMode SelectionMode { get; set; }
        public System.Drawing.Font  Font     { get; set; }
        public DockStyle            Dock     { get; set; }
        public AnchorStyles         Anchor   { get; set; }
        public System.Drawing.Size  Size     { get; set; }
        public System.Drawing.Point Location { get; set; }
        public string  Name     { get; set; } = "";
        public int     TabIndex { get; set; }
        public bool    Visible  { get; set; } = true;
        public bool    Enabled  { get; set; } = true;
        public object  Tag      { get; set; }
        public ContextMenuStrip ContextMenuStrip { get; set; }
        public event EventHandler SelectedIndexChanged;
        public event DragEventHandler DragDrop;
        public event DragEventHandler DragEnter;

        public SelectedItemCollection SelectedItems { get; }

        public ListBox()
        {
            Items = new ListBoxItemCollection(_items, this);
            SelectedItems = new SelectedItemCollection(_items);
        }

        public void BeginUpdate() { }
        public void EndUpdate()   { if (Sorted) _items.Sort(); }

        public class ListBoxItemCollection
        {
            private readonly System.Collections.ArrayList _list;
            private readonly ListBox _owner;
            public ListBoxItemCollection(System.Collections.ArrayList list, ListBox owner)
            { _list = list; _owner = owner; }
            public int Count => _list.Count;
            public object this[int i] { get => _list[i]; set => _list[i] = value; }
            public void Add(object item)    { _list.Add(item); }
            public void Clear()             { _list.Clear(); }
            public void Remove(object item) { _list.Remove(item); }
        }

        public class SelectedItemCollection
        {
            private readonly System.Collections.ArrayList _all;
            public SelectedItemCollection(System.Collections.ArrayList all) { _all = all; }
            public int Count => 0; // stub — no actual selection tracking
            public object this[int i] => null;
        }
    }

    // ─── IDataObject ─────────────────────────────────────────────────────────
    public interface IDataObject
    {
        object GetData(string format);
        object GetData(Type format);
        bool   GetDataPresent(string format);
        bool   GetDataPresent(Type format);
    }

    // ─── DragEventArgs ────────────────────────────────────────────────────────
    public class DragEventArgs : EventArgs
    {
        public IDataObject     Data   { get; }
        public int             X      { get; }
        public int             Y      { get; }
        public DragDropEffects Effect { get; set; }
        public DragEventArgs(IDataObject data, int x, int y) { Data = data; X = x; Y = y; }
    }

    // ─── ListViewEx ───────────────────────────────────────────────────────────
    /// <summary>
    /// Avalonia-compatible stub for the WinForms ListViewEx with embedded controls.
    /// All P/Invoke, WM_PAINT overrides, and layout code are removed.
    /// AddEmbeddedControl is a no-op; callers (BoneListViewItem, MeshListViewItem)
    /// use it only as a visual hint that is not needed in Avalonia.
    /// </summary>
    public class ListViewEx
    {
        public ListViewEx() { }

        public ColumnHeaderCollection Columns { get; } = new ColumnHeaderCollection();
        public ListViewItemCollection Items   { get; } = new ListViewItemCollection();

        public SelectedListViewItemCollection SelectedItems
            => new SelectedListViewItemCollection(Items);

        public View              View         { get; set; } = View.Details;
        public bool              FullRowSelect { get; set; }
        public bool              HideSelection { get; set; }
        public ColumnHeaderStyle HeaderStyle  { get; set; }
        public BorderStyle       BorderStyle  { get; set; }
        public System.Collections.IComparer ListViewItemSorter { get; set; }

        // no-op: embedded controls are a WinForms-only concept
        public void AddEmbeddedControl(Avalonia.Controls.Control c, int column, int row) { }

        public void RemoveEmbeddedControl(Avalonia.Controls.Control c) { }

        public Avalonia.Controls.Control GetEmbeddedControl(int col, int row) => null;
    }
}
