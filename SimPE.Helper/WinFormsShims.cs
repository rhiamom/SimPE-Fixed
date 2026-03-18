// WinForms compatibility shims — allows cross-platform compilation without System.Windows.Forms.
// These stubs provide the minimum API surface needed for the codebase to compile.
// They will be superseded when the UI layer is rebuilt with Avalonia.

using System;
using System.Collections.Generic;

// ── System.Windows.Forms stubs ──────────────────────────────────────────────

namespace System.Windows.Forms
{
    public interface IWin32Window { IntPtr Handle { get; } }

    // ── Enums ──────────────────────────────────────────────────────────────

    public enum DialogResult { None, OK, Cancel, Abort, Retry, Ignore, Yes, No }
    public enum SortOrder { None, Ascending, Descending }
    public enum View { LargeIcon, Details, SmallIcon, List, Tile }
    public enum CheckState { Unchecked, Checked, Indeterminate }
    public enum ScrollEventType { SmallDecrement, SmallIncrement, LargeDecrement, LargeIncrement, ThumbPosition, ThumbTrack, First, Last, EndScroll }
    public enum Orientation { Horizontal, Vertical }
    public enum BorderStyle { None, FixedSingle, Fixed3D }
    [Flags] public enum ControlStyles { None = 0, OptimizedDoubleBuffer = 0x20000, UserPaint = 2, AllPaintingInWmPaint = 0x2000, ResizeRedraw = 0x10, Selectable = 0x200, ContainerControl = 0x40000, DoubleBuffer = 0x10, SupportsTransparentBackColor = 0x400, StandardDoubleClick = 0x4000, Opaque = 4 }
    public enum AnchorStyles { None = 0, Top = 1, Bottom = 2, Left = 4, Right = 8 }
    public enum DockStyle { None, Top, Bottom, Left, Right, Fill }
    public enum FlatStyle { Flat, Popup, Standard, System }
    public enum FormBorderStyle { None, FixedSingle, Fixed3D, FixedDialog, Sizable, FixedToolWindow, SizableToolWindow }
    public enum FormStartPosition { Manual, CenterScreen, WindowsDefaultLocation, WindowsDefaultBounds, CenterParent }
    public enum MessageBoxButtons { OK, OKCancel, AbortRetryIgnore, YesNoCancel, YesNo, RetryCancel }
    public enum MessageBoxIcon { None, Hand, Question, Exclamation, Asterisk, Stop, Error, Warning, Information }

    public enum Keys
    {
        None = 0,
        Back = 8, Tab = 9,
        Shift = 0x10000, Control = 0x20000, Alt = 0x40000,
        Insert = 0x2D, Delete = 0x2E,
        Up = 0x26, Down = 0x28, Left = 0x25, Right = 0x27,
        Home = 0x24, End = 0x23, PageUp = 0x21, PageDown = 0x22,
        D0 = 0x30, D1 = 0x31, D2 = 0x32, D3 = 0x33, D4 = 0x34,
        D5 = 0x35, D6 = 0x36, D7 = 0x37, D8 = 0x38, D9 = 0x39,
        A = 0x41, B = 0x42, C = 0x43, D = 0x44, E = 0x45,
        F = 0x46, G = 0x47, H = 0x48, I = 0x49, J = 0x4A,
        K = 0x4B, L = 0x4C, M = 0x4D, N = 0x4E, O = 0x4F,
        P = 0x50, Q = 0x51, R = 0x52, S = 0x53, T = 0x54,
        U = 0x55, V = 0x56, W = 0x57, X = 0x58, Y = 0x59, Z = 0x5A,
        F1 = 0x70, F2 = 0x71, F3 = 0x72, F4 = 0x73, F5 = 0x74,
        F6 = 0x75, F7 = 0x76, F8 = 0x77, F9 = 0x78, F10 = 0x79,
        F11 = 0x7A, F12 = 0x7B,
        Enter = 0x0D,
    }

    public enum Shortcut
    {
        None = 0,
        Ins = 0x2D, Del = 0x2E,
        ShiftIns = 0x1002D, ShiftDel = 0x1002E,
        Ctrl0 = 0x20030, Ctrl1 = 0x20031, Ctrl2 = 0x20032, Ctrl3 = 0x20033,
        Ctrl4 = 0x20034, Ctrl5 = 0x20035, Ctrl6 = 0x20036, Ctrl7 = 0x20037,
        Ctrl8 = 0x20038, Ctrl9 = 0x20039,
        CtrlA = 0x20041, CtrlB = 0x20042, CtrlC = 0x20043, CtrlD = 0x20044,
        CtrlE = 0x20045, CtrlF = 0x20046, CtrlG = 0x20047, CtrlH = 0x20048,
        CtrlI = 0x20049, CtrlJ = 0x2004A, CtrlK = 0x2004B, CtrlL = 0x2004C,
        CtrlM = 0x2004D, CtrlN = 0x2004E, CtrlO = 0x2004F, CtrlP = 0x20050,
        CtrlQ = 0x20051, CtrlR = 0x20052, CtrlS = 0x20053, CtrlT = 0x20054,
        CtrlU = 0x20055, CtrlV = 0x20056, CtrlW = 0x20057, CtrlX = 0x20058,
        CtrlY = 0x20059, CtrlZ = 0x2005A,
        CtrlShiftA = 0x30041, CtrlShiftD = 0x30044, CtrlShiftI = 0x30049,
        CtrlShiftN = 0x3004E, CtrlShiftS = 0x30053, CtrlShiftW = 0x30057,
        Alt0 = 0x40030, Alt1 = 0x40031, Alt2 = 0x40032, Alt3 = 0x40033,
        Alt4 = 0x40034, Alt5 = 0x40035, Alt6 = 0x40036, Alt7 = 0x40037,
        Alt8 = 0x40038, Alt9 = 0x40039,
    }

    [Flags] public enum BoundsSpecified { None = 0, X = 1, Y = 2, Width = 4, Height = 8, Location = X | Y, Size = Width | Height, All = Location | Size }

    // ── Event args & delegates ─────────────────────────────────────────────

    public class MouseEventArgs : EventArgs
    {
        public System.Drawing.Point Location { get; }
        public int X => Location.X;
        public int Y => Location.Y;
        public int Delta { get; }
        public System.Windows.Forms.MouseButtons Button { get; }
        public MouseEventArgs(MouseButtons button, int clicks, int x, int y, int delta)
        { Button = button; Location = new System.Drawing.Point(x, y); Delta = delta; }
    }
    [Flags] public enum MouseButtons { None = 0, Left = 0x100000, Right = 0x200000, Middle = 0x400000, XButton1 = 0x800000, XButton2 = 0x1000000 }
    public delegate void MouseEventHandler(object sender, MouseEventArgs e);

    public class PaintEventArgs : EventArgs
    {
        public System.Drawing.Graphics Graphics { get; }
        public System.Drawing.Rectangle ClipRectangle { get; }
        public PaintEventArgs(System.Drawing.Graphics g, System.Drawing.Rectangle clip) { Graphics = g; ClipRectangle = clip; }
    }
    public delegate void PaintEventHandler(object sender, PaintEventArgs e);

    public class KeyEventArgs : EventArgs
    {
        public Keys KeyCode { get; }
        public bool Control { get; }
        public bool Shift { get; }
        public bool Alt { get; }
        public bool Handled { get; set; }
        public KeyEventArgs(Keys keyData) { KeyCode = keyData & (Keys)0xFFFF; Control = (keyData & Keys.Control) != 0; Shift = (keyData & Keys.Shift) != 0; Alt = (keyData & Keys.Alt) != 0; }
    }
    public class KeyPressEventArgs : EventArgs { public char KeyChar { get; set; } }
    public delegate void KeyEventHandler(object sender, KeyEventArgs e);
    public delegate void KeyPressEventHandler(object sender, KeyPressEventArgs e);

    public class ScrollEventArgs : EventArgs
    {
        public int NewValue { get; }
        public ScrollEventType Type { get; }
        public ScrollEventArgs(ScrollEventType type, int newValue) { Type = type; NewValue = newValue; }
    }
    public delegate void ScrollEventHandler(object sender, ScrollEventArgs e);

    public class LinkLabelLinkClickedEventArgs : EventArgs { public object Link { get; } }
    public delegate void LinkLabelLinkClickedEventHandler(object sender, LinkLabelLinkClickedEventArgs e);

    public class FormClosedEventArgs : EventArgs { }
    public delegate void FormClosedEventHandler(object sender, FormClosedEventArgs e);

    public class Message
    {
        public static Message Create(IntPtr hWnd, int msg, IntPtr wparam, IntPtr lparam) => new Message();
        public IntPtr HWnd { get; set; }
        public int Msg { get; set; }
        public IntPtr WParam { get; set; }
        public IntPtr LParam { get; set; }
        public IntPtr Result { get; set; }
    }

    public class CreateParams
    {
        public int ExStyle { get; set; }
        public int Style { get; set; }
        public string ClassName { get; set; }
        public string Caption { get; set; }
    }

    // ── Application ────────────────────────────────────────────────────────

    public static class Application
    {
        public static void DoEvents() { }
        public static bool UseWaitCursor { get; set; }
        public static string ExecutablePath => Environment.ProcessPath ?? AppContext.BaseDirectory;
    }

    // ── Control base class ──────────────────────────────────────────────────

    public class Control : IDisposable
    {
        ControlStyles _styles;
        public virtual string Text { get; set; } = "";
        public virtual bool Visible { get; set; } = true;
        public virtual bool Enabled { get; set; } = true;
        public virtual System.Drawing.Color BackColor { get; set; } = System.Drawing.Color.Transparent;
        public virtual System.Drawing.Color ForeColor { get; set; } = System.Drawing.Color.Black;
        public virtual System.Drawing.Font Font { get; set; }
        public virtual bool AutoSize { get; set; }
        public virtual System.Drawing.Size ClientSize { get; set; }
        public virtual System.Drawing.Rectangle ClientRectangle => new System.Drawing.Rectangle(System.Drawing.Point.Empty, ClientSize);
        public System.Drawing.Size Size { get; set; }
        public System.Drawing.Point Location { get; set; }
        public string Name { get; set; } = "";
        public int TabIndex { get; set; }
        public AnchorStyles Anchor { get; set; }
        public DockStyle Dock { get; set; }
        public BorderStyle BorderStyle { get; set; }
        public ControlCollection Controls { get; } = new ControlCollection();
        public Control Parent { get; set; }
        public IntPtr Handle => IntPtr.Zero;
        public int Height { get => Size.Height; set { var s = Size; s.Height = value; Size = s; } }
        public int Width { get => Size.Width; set { var s = Size; s.Width = value; Size = s; } }
        public int Left { get => Location.X; set { var p = Location; p.X = value; Location = p; } }
        public int Top { get => Location.Y; set { var p = Location; p.Y = value; Location = p; } }
        public int Right => Left + Width;
        public int Bottom => Top + Height;
        public bool DesignMode => false;
        public object Tag { get; set; }
        public bool IsHandleCreated => false;
        public System.Drawing.Rectangle Bounds { get => new System.Drawing.Rectangle(Location, Size); set { Location = value.Location; Size = value.Size; } }

        public event EventHandler Click;
        public event MouseEventHandler MouseDown;
        public event MouseEventHandler MouseMove;
        public event MouseEventHandler MouseUp;
        public event MouseEventHandler MouseLeave;
        public event KeyEventHandler KeyDown;
        public event KeyEventHandler KeyUp;
        public event PaintEventHandler Paint;
        public event EventHandler Resize;
        public event EventHandler VisibleChanged;
        public event EventHandler SizeChanged;
        public event EventHandler LocationChanged;

        protected bool GetStyle(ControlStyles flag) => (_styles & flag) != 0;
        protected void SetStyle(ControlStyles flag, bool value) { if (value) _styles |= flag; else _styles &= ~flag; }

        protected virtual void Dispose(bool disposing) { }
        public void Dispose() { Dispose(true); GC.SuppressFinalize(this); }
        public virtual void Refresh() { }
        protected virtual void OnResize(EventArgs e) => Resize?.Invoke(this, e);
        protected virtual void OnVisibleChanged(EventArgs e) => VisibleChanged?.Invoke(this, e);
        protected virtual void OnMouseLeave(EventArgs e) => MouseLeave?.Invoke(this, new MouseEventArgs(MouseButtons.None, 0, 0, 0, 0));
        protected virtual void OnMouseDown(MouseEventArgs e) => MouseDown?.Invoke(this, e);
        protected virtual void OnMouseMove(MouseEventArgs e) => MouseMove?.Invoke(this, e);
        protected virtual void OnMouseUp(MouseEventArgs e) => MouseUp?.Invoke(this, e);
        protected virtual void OnKeyDown(KeyEventArgs e) => KeyDown?.Invoke(this, e);
        protected virtual void OnKeyUp(KeyEventArgs e) => KeyUp?.Invoke(this, e);
        protected virtual void OnKeyPress(KeyPressEventArgs e) { }
        protected virtual void OnPaint(PaintEventArgs e) => Paint?.Invoke(this, e);
        protected virtual void OnSizeChanged(EventArgs e) => OnResize(e);
        protected virtual void OnMouseEnter(EventArgs e) { }
        protected virtual void OnMouseWheel(MouseEventArgs e) { }
        protected virtual void OnPaintBackground(PaintEventArgs e) { }
        protected virtual CreateParams CreateParams => new CreateParams();
        protected virtual void WndProc(ref Message m) { }
        protected virtual object GetService(Type serviceType) => null;
        public virtual void SetBounds(int x, int y, int width, int height) { Location = new System.Drawing.Point(x, y); Size = new System.Drawing.Size(width, height); }
        public virtual void SetBounds(int x, int y, int width, int height, BoundsSpecified specified) => SetBounds(x, y, width, height);

        public void SuspendLayout() { }
        public void ResumeLayout(bool performLayout = false) { }
        public void Invalidate() { }
        public void Invalidate(System.Drawing.Rectangle rc) { }
        public void Update() { }
        public void Focus() { }
        public bool ContainsFocus => false;
        public System.Drawing.Point PointToScreen(System.Drawing.Point p) => p;
        public System.Drawing.Point PointToClient(System.Drawing.Point p) => p;
    }

    // ── Container controls ─────────────────────────────────────────────────

    public class ScrollableControl : Control
    {
        public virtual bool AutoScroll { get; set; }
        protected virtual void AdjustFormScrollbars(bool displayScrollbars) { }
    }

    public class Panel : ScrollableControl { }
    public class GroupBox : Control { }
    public class TabPage : Panel { public string Text { get; set; } = ""; }

    public class TabControl : Control
    {
        public TabPageCollection TabPages { get; } = new TabPageCollection();
        public int SelectedIndex { get; set; } = 0;
    }

    // ── Form ───────────────────────────────────────────────────────────────

    public class Form : ScrollableControl
    {
        public static Form ActiveForm => null;
        public Form Owner { get; set; }
        public DialogResult DialogResult { get; set; } = DialogResult.None;
        public bool ShowInTaskbar { get; set; } = true;
        public FormBorderStyle FormBorderStyle { get; set; }
        public FormStartPosition StartPosition { get; set; }

        public event EventHandler Activated;
        public event FormClosedEventHandler FormClosed;

        public virtual void Activate() { Activated?.Invoke(this, EventArgs.Empty); }
        public virtual DialogResult ShowDialog() { return DialogResult; }
        public virtual DialogResult ShowDialog(IWin32Window owner) { return DialogResult; }
        public virtual void Close() { FormClosed?.Invoke(this, new FormClosedEventArgs()); }
    }

    public class UserControl : ScrollableControl { }

    // ── Simple controls ────────────────────────────────────────────────────

    public class Label : Control { public System.Drawing.ContentAlignment TextAlign { get; set; } }
    public class LinkLabel : Label
    {
        public event LinkLabelLinkClickedEventHandler LinkClicked;
        public LinkCollection Links { get; } = new LinkCollection();
    }
    public class Button : Control
    {
        public event EventHandler Click;
        public DialogResult DialogResult { get; set; }
        public FlatStyle FlatStyle { get; set; }
    }
    public class RadioButton : Control { public bool Checked { get; set; } }
    public class CheckBox : Control
    {
        public CheckState CheckState { get; set; }
        public bool Checked { get; set; }
        public FlatStyle FlatStyle { get; set; }
        public event EventHandler CheckedChanged;
    }
    public class TextBox : Control
    {
        public bool Multiline { get; set; }
        public bool ReadOnly { get; set; }
        public bool ScrollBars { get; set; }
    }
    public class RichTextBox : TextBox
    {
        public string[] Lines { get; set; } = Array.Empty<string>();
    }
    public class PictureBox : Control
    {
        public System.Drawing.Image Image { get; set; }
    }

    public class ProgressBar : Control
    {
        public int Minimum { get; set; }
        public int Maximum { get; set; } = 100;
        public int Value { get; set; }
        public int Step { get; set; } = 10;
        public void PerformStep() { Value = Math.Min(Value + Step, Maximum); }
    }

    public class ComboBox : Control
    {
        public System.Collections.ArrayList Items { get; } = new System.Collections.ArrayList();
        public int SelectedIndex { get; set; } = -1;
        public object SelectedItem { get; set; }
        public string SelectedText { get; set; } = "";
        public bool Sorted { get; set; }
    }

    public class ListBox : Control
    {
        public System.Collections.ArrayList Items { get; } = new System.Collections.ArrayList();
        public int SelectedIndex { get; set; } = -1;
        public object SelectedItem { get; set; }
    }

    public class ScrollBar : Control
    {
        public int Minimum { get; set; }
        public int Maximum { get; set; } = 100;
        public int Value { get; set; }
        public int LargeChange { get; set; } = 10;
        public int SmallChange { get; set; } = 1;
        public event ScrollEventHandler Scroll;
    }
    public class HScrollBar : ScrollBar { }
    public class VScrollBar : ScrollBar { }

    public class FolderBrowserDialog : IDisposable
    {
        public string SelectedPath { get; set; } = "";
        public DialogResult ShowDialog() => DialogResult.Cancel;
        public void Dispose() { }
    }

    // ── ListViewItem ───────────────────────────────────────────────────────

    public class ListViewGroup
    {
        public string Name { get; set; } = "";
        public string Header { get; set; } = "";
        public ListViewGroup(string name, string header = "") { Name = name; Header = header; }
    }

    public class ListViewGroupCollection
    {
        readonly Dictionary<string, ListViewGroup> _groups = new();
        public ListViewGroup this[string key] => _groups.TryGetValue(key, out var g) ? g : null;
        public void Add(ListViewGroup g) => _groups[g.Name] = g;
    }

    public class ListViewItem
    {
        public string Text { get; set; } = "";
        public ListViewGroup Group { get; set; }
        public ListView ListView { get; internal set; }
        public ListViewSubItemCollection SubItems { get; }

        public ListViewItem() { SubItems = new ListViewSubItemCollection(); }
        public ListViewItem(string text) : this() { Text = text; }

        public class ListViewSubItemCollection
        {
            readonly List<SubItem> _items = new();
            public SubItem this[int i] => _items[i];
            public int Count => _items.Count;
            public void Add(string text) => _items.Add(new SubItem { Text = text });
        }

        public class SubItem
        {
            public string Text { get; set; } = "";
        }
    }

    public class ListView : Control
    {
        public ListViewGroupCollection Groups { get; } = new ListViewGroupCollection();
        public ListViewItemCollection Items { get; }
        public View View { get; set; }
        public bool FullRowSelect { get; set; }
        public bool MultiSelect { get; set; } = true;
        public bool ShowGroups { get; set; } = true;

        public ListView() { Items = new ListViewItemCollection(this); }

        public class ListViewItemCollection
        {
            readonly List<ListViewItem> _items = new();
            readonly ListView _owner;
            public ListViewItemCollection(ListView owner) { _owner = owner; }
            public ListViewItem this[int i] => _items[i];
            public int Count => _items.Count;
            public void Add(ListViewItem item) { item.ListView = _owner; _items.Add(item); }
            public void Clear() => _items.Clear();
        }
    }

    // ── ImageList ─────────────────────────────────────────────────────────

    public class ImageList : IDisposable
    {
        public ImageListImageCollection Images { get; } = new ImageListImageCollection();
        public System.Drawing.Size ImageSize { get; set; } = new System.Drawing.Size(16, 16);
        public void Dispose() { }
    }

    public class ImageListImageCollection
    {
        readonly List<System.Drawing.Image> _images = new();
        public System.Drawing.Image this[int i] => i < _images.Count ? _images[i] : null;
        public int Count => _images.Count;
        public void Add(System.Drawing.Image img) => _images.Add(img);
        public void Clear() => _images.Clear();
    }

    // ── Timer ─────────────────────────────────────────────────────────────

    public class Timer : IDisposable
    {
        System.Threading.Timer _timer;
        public int Interval { get; set; } = 100;
        public bool Enabled { get; set; }
        public event EventHandler Tick;
        public void Start() { Enabled = true; }
        public void Stop() { Enabled = false; _timer?.Dispose(); _timer = null; }
        public void Dispose() { Stop(); }
    }

    // ── MessageBox ────────────────────────────────────────────────────────

    public static class MessageBox
    {
        public static DialogResult Show(string text) { Console.WriteLine("[SimPE] " + text); return DialogResult.OK; }
        public static DialogResult Show(string text, string caption) { Console.WriteLine($"[SimPE] {caption}: {text}"); return DialogResult.OK; }
        public static DialogResult Show(string text, string caption, MessageBoxButtons buttons) => Show(text, caption);
        public static DialogResult Show(IWin32Window owner, string text, string caption, MessageBoxButtons buttons) => Show(text, caption);
        public static DialogResult Show(string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon) => Show(text, caption);
    }

    // ── CheckedListBox ────────────────────────────────────────────────────

    public class CheckedListBox : Control
    {
        readonly List<string> _items = new();
        readonly List<bool> _checked = new();
        public int SelectedIndex { get; set; } = -1;
        public CheckedListBoxItemCollection Items { get; }
        public CheckedListBox() { Items = new CheckedListBoxItemCollection(_items, _checked); }
        public void SetItemChecked(int index, bool value) { if (index < _checked.Count) _checked[index] = value; }
        public bool GetItemChecked(int index) => index < _checked.Count && _checked[index];
        public void SetSelected(int index, bool value) { SelectedIndex = value ? index : -1; }
    }

    public class CheckedListBoxItemCollection
    {
        readonly List<string> _items;
        readonly List<bool> _checked;
        public CheckedListBoxItemCollection(List<string> items, List<bool> chk) { _items = items; _checked = chk; }
        public int Count => _items.Count;
        public void Clear() { _items.Clear(); _checked.Clear(); }
        public void AddRange(string[] items) { foreach (var s in items) { _items.Add(s); _checked.Add(false); } }
        public void Add(string item) { _items.Add(item); _checked.Add(false); }
    }

    // ── Collection helpers ─────────────────────────────────────────────────

    public class ControlCollection : System.Collections.ICollection
    {
        readonly List<Control> _items = new();
        public void Add(Control c) => _items.Add(c);
        public void Remove(Control c) => _items.Remove(c);
        public bool Contains(Control c) => _items.Contains(c);
        public void Clear() => _items.Clear();
        public int Count => _items.Count;
        public bool IsSynchronized => false;
        public object SyncRoot => this;
        public void CopyTo(Array array, int index) => ((System.Collections.ICollection)_items).CopyTo(array, index);
        public System.Collections.IEnumerator GetEnumerator() => _items.GetEnumerator();
    }

    public class TabPageCollection
    {
        readonly List<TabPage> _items = new();
        public TabPage this[int i] => _items[i];
        public int Count => _items.Count;
        public void Add(TabPage p) => _items.Add(p);
    }

    public class LinkCollection
    {
        public void Add(object link) { }
    }

    // ── Tree & toolbar controls ────────────────────────────────────────────

    public class TreeNode
    {
        public string Text { get; set; } = "";
        public TreeNodeCollection Nodes { get; } = new TreeNodeCollection();
        public TreeNode Parent { get; internal set; }
        public object Tag { get; set; }
        public TreeNode() { }
        public TreeNode(string text) { Text = text; }
    }

    public class TreeNodeCollection
    {
        readonly List<TreeNode> _nodes = new();
        public TreeNode this[int i] => _nodes[i];
        public int Count => _nodes.Count;
        public void Add(TreeNode n) => _nodes.Add(n);
        public void Clear() => _nodes.Clear();
    }

    public class TreeViewEventArgs : EventArgs
    {
        public TreeNode Node { get; }
        public TreeViewEventArgs(TreeNode node) { Node = node; }
    }
    public delegate void TreeViewEventHandler(object sender, TreeViewEventArgs e);

    public class TreeView : Control
    {
        public TreeNodeCollection Nodes { get; } = new TreeNodeCollection();
        public TreeNode SelectedNode { get; set; }
        public event TreeViewEventHandler AfterSelect;
    }

    public class Splitter : Control { }

    public class ColumnHeader
    {
        public string Text { get; set; } = "";
        public int Width { get; set; }
        public ColumnHeader() { }
        public ColumnHeader(string text, int width = 100) { Text = text; Width = width; }
    }

    public class ContextMenuStrip : Control
    {
        public ToolStripItemCollection Items { get; } = new ToolStripItemCollection();
    }

    public class ToolStripMenuItem
    {
        public string Text { get; set; } = "";
        public bool Checked { get; set; }
        public bool Enabled { get; set; } = true;
        public Keys ShortcutKeys { get; set; }
        public ToolStripItemCollection DropDownItems { get; } = new ToolStripItemCollection();
        public event EventHandler Click;
        public ToolStripMenuItem() { }
        public ToolStripMenuItem(string text) { Text = text; }
    }

    public class ToolStripSeparator { }

    public class ToolStripItemCollection
    {
        readonly List<object> _items = new();
        public void Add(ToolStripMenuItem item) => _items.Add(item);
        public void Add(ToolStripSeparator sep) => _items.Add(sep);
        public int Count => _items.Count;
    }

    public class ToolStrip : Control
    {
        public ToolStripItemCollection Items { get; } = new ToolStripItemCollection();
    }

    public class MenuStrip : ToolStrip { }
    public class StatusStrip : ToolStrip { }

    public class OpenFileDialog : IDisposable
    {
        public string FileName { get; set; } = "";
        public string Filter { get; set; } = "";
        public bool Multiselect { get; set; }
        public string[] FileNames { get; set; } = Array.Empty<string>();
        public DialogResult ShowDialog() => DialogResult.Cancel;
        public void Dispose() { }
    }

    public class SaveFileDialog : IDisposable
    {
        public string FileName { get; set; } = "";
        public string Filter { get; set; } = "";
        public DialogResult ShowDialog() => DialogResult.Cancel;
        public void Dispose() { }
    }

    public struct LinkArea
    {
        public int Start;
        public int Length;
        public LinkArea(int start, int length) { Start = start; Length = length; }
    }

    public static class OSFeature
    {
        public static bool IsPresent(object feature) => false;
        public static readonly object LayeredWindows = new object();
    }

    public class TrackBar : Control
    {
        public int Minimum { get; set; }
        public int Maximum { get; set; } = 10;
        public int Value { get; set; }
        public int TickFrequency { get; set; } = 1;
        public event EventHandler ValueChanged;
    }

    public class NumericUpDown : Control
    {
        public decimal Minimum { get; set; }
        public decimal Maximum { get; set; } = 100;
        public decimal Value { get; set; }
        public int DecimalPlaces { get; set; }
        public event EventHandler ValueChanged;
    }

    public class ToolTip : IDisposable
    {
        public void SetToolTip(Control c, string text) { }
        public void Dispose() { }
    }

    public class NotifyIcon : IDisposable
    {
        public System.Drawing.Icon Icon { get; set; }
        public string Text { get; set; } = "";
        public bool Visible { get; set; }
        public ContextMenuStrip ContextMenuStrip { get; set; }
        public void Dispose() { }
    }
}

// ── System.Windows.Forms.Design stub ────────────────────────────────────────

namespace System.Windows.Forms.Design
{
    public class ControlDesigner : IDisposable
    {
        public virtual System.ComponentModel.Design.DesignerVerbCollection Verbs { get; } = new System.ComponentModel.Design.DesignerVerbCollection();
        public virtual void Initialize(System.ComponentModel.IComponent component) { }
        public virtual System.Collections.ICollection AssociatedComponents => System.Array.Empty<object>();
        protected virtual void Dispose(bool disposing) { }
        public void Dispose() { Dispose(true); }
    }
}

// ── System.Drawing.Design stubs ─────────────────────────────────────────────

namespace System.Drawing.Design
{
    public enum UITypeEditorEditStyle { None, DropDown, Modal }

    public class UITypeEditor
    {
        public virtual UITypeEditorEditStyle GetEditStyle(System.ComponentModel.ITypeDescriptorContext context) => UITypeEditorEditStyle.None;
        public virtual object EditValue(System.ComponentModel.ITypeDescriptorContext context, IServiceProvider provider, object value) => value;
    }
}
