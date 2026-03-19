// WinForms compatibility shims — allows cross-platform compilation without System.Windows.Forms.
// These stubs provide the minimum API surface needed for the codebase to compile.
// They will be superseded when the UI layer is rebuilt with Avalonia.

using System;
using System.Collections.Generic;
using System.Linq;

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
    public enum ColumnHeaderStyle { None, Nonclickable, Clickable }
    public enum ComboBoxStyle { Simple, DropDown, DropDownList }
    [Flags] public enum ControlStyles { None = 0, OptimizedDoubleBuffer = 0x20000, UserPaint = 2, AllPaintingInWmPaint = 0x2000, ResizeRedraw = 0x10, Selectable = 0x200, ContainerControl = 0x40000, DoubleBuffer = 0x10, SupportsTransparentBackColor = 0x400, StandardDoubleClick = 0x4000, Opaque = 4, UserMouse = 0x400000 }
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

    public struct Padding
    {
        public int Left, Top, Right, Bottom;
        public Padding(int all) { Left = Top = Right = Bottom = all; }
        public Padding(int left, int top, int right, int bottom) { Left = left; Top = top; Right = right; Bottom = bottom; }
        public int All { set { Left = Top = Right = Bottom = value; } }
        public static Padding Empty => new Padding(0);
    }

    [Flags] public enum BoundsSpecified { None = 0, X = 1, Y = 2, Width = 4, Height = 8, Location = X | Y, Size = Width | Height, All = Location | Size }
    [Flags] public enum DragDropEffects { None = 0, Copy = 1, Move = 2, Link = 4, Scroll = unchecked((int)0x80000000), All = unchecked((int)0x80000007) }
    public enum ImageLayout { None, Tile, Center, Stretch, Zoom }
    public enum FormWindowState { Normal, Minimized, Maximized }
    public enum RightToLeft { No, Yes, Inherit }
    public enum ImeMode { Inherit = -1, NoControl, On, Off, Disable, Hiragana, Katakana, KatakanaHalf, AlphaFull, Alpha, HangulFull, Hangul, Close, OnHalf }
    public enum ToolStripRenderMode { Custom, ManagerRenderMode, Professional, System }
    public enum ColorDepth { Depth4Bit, Depth8Bit, Depth16Bit, Depth24Bit, Depth32Bit }
    public enum ItemActivation { Standard, OneClick, TwoClick }
    public enum FixedPanel { None, Panel1, Panel2 }
    public enum ToolStripItemAlignment { Left, Right }
    public enum ToolStripItemDisplayStyle { None, Text, Image, ImageAndText }
    public enum ToolStripLayoutStyle { StackWithOverflow, HorizontalStackWithOverflow, VerticalStackWithOverflow, Flow, Table }
    public enum MessageBoxDefaultButton { Button1, Button2, Button3 }
    public enum TextDataFormat { Text, UnicodeText, Rtf, Html, CommaSeparatedValue }
    public enum SizeType { AutoSize, Absolute, Percent }
    public enum SizeGripStyle { Auto, Show, Hide }
    public enum ToolStripItemOverflow { Never, Always, AsNeeded }

    public static class SystemInformation
    {
        public static System.Drawing.Size IconSize => new System.Drawing.Size(32, 32);
        public static System.Drawing.Size SmallIconSize => new System.Drawing.Size(16, 16);
        public static int MouseWheelScrollLines => 3;
        public static System.Drawing.Size PrimaryMonitorSize => new System.Drawing.Size(1920, 1080);
        public static int BorderSize => 1;
        public static int VerticalScrollBarWidth => 17;
        public static int HorizontalScrollBarHeight => 17;
    }

    // ── Event args & delegates ─────────────────────────────────────────────

    public class MouseEventArgs : EventArgs
    {
        public System.Drawing.Point Location { get; }
        public int X => Location.X;
        public int Y => Location.Y;
        public int Delta { get; }
        public int Clicks { get; }
        public System.Windows.Forms.MouseButtons Button { get; }
        public MouseEventArgs(MouseButtons button, int clicks, int x, int y, int delta)
        { Button = button; Clicks = clicks; Location = new System.Drawing.Point(x, y); Delta = delta; }
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

    public class LinkLabelLinkClickedEventArgs : EventArgs { public LinkLabelLink Link { get; } = new LinkLabelLink(); }
    public delegate void LinkLabelLinkClickedEventHandler(object sender, LinkLabelLinkClickedEventArgs e);

    public class DragEventArgs : EventArgs
    {
        public int X { get; }
        public int Y { get; }
        public DragDropEffects AllowedEffect { get; }
        public DragDropEffects Effect { get; set; }
        public System.Windows.Forms.IDataObject Data { get; }
        public int KeyState { get; }
        public DragEventArgs(IDataObject data, int keyState, int x, int y, DragDropEffects allowedEffect, DragDropEffects effect)
        { Data = data; KeyState = keyState; X = x; Y = y; AllowedEffect = allowedEffect; Effect = effect; }
    }
    public delegate void DragEventHandler(object sender, DragEventArgs e);

    public class QueryContinueDragEventArgs : EventArgs
    {
        public int KeyState { get; }
        public bool EscapePressed { get; }
        public DragAction Action { get; set; }
    }
    public delegate void QueryContinueDragEventHandler(object sender, QueryContinueDragEventArgs e);
    public enum DragAction { Continue, Drop, Cancel }

    public interface IDataObject
    {
        object GetData(string format);
        object GetData(Type format);
        bool GetDataPresent(string format);
        bool GetDataPresent(Type format);
        string[] GetFormats();
        void SetData(string format, object data);
        void SetData(object data);
    }

    public class FormClosedEventArgs : EventArgs { }
    public delegate void FormClosedEventHandler(object sender, FormClosedEventArgs e);

    public class LabelEditEventArgs : EventArgs
    {
        public int Item { get; }
        public string Label { get; }
        public bool CancelEdit { get; set; }
        public LabelEditEventArgs(int item, string label = null) { Item = item; Label = label; }
    }
    public delegate void LabelEditEventHandler(object sender, LabelEditEventArgs e);

    public class ItemCheckEventArgs : EventArgs
    {
        public int Index { get; }
        public CheckState NewValue { get; set; }
        public CheckState CurrentValue { get; }
        public ItemCheckEventArgs(int index, CheckState newValue, CheckState currentValue) { Index = index; NewValue = newValue; CurrentValue = currentValue; }
    }
    public delegate void ItemCheckEventHandler(object sender, ItemCheckEventArgs e);

    public class ItemCheckedEventArgs : EventArgs
    {
        public ListViewItem Item { get; }
        public ItemCheckedEventArgs(ListViewItem item) { Item = item; }
    }
    public delegate void ItemCheckedEventHandler(object sender, ItemCheckedEventArgs e);

    public class LinkClickedEventArgs : EventArgs
    {
        public string LinkText { get; }
        public LinkClickedEventArgs(string linkText) { LinkText = linkText; }
    }
    public delegate void LinkClickedEventHandler(object sender, LinkClickedEventArgs e);

    public class ColumnClickEventArgs : EventArgs
    {
        public int Column { get; }
        public ColumnClickEventArgs(int column) { Column = column; }
    }
    public delegate void ColumnClickEventHandler(object sender, ColumnClickEventArgs e);

    public class ControlEventArgs : EventArgs
    {
        public Control Control { get; }
        public ControlEventArgs(Control control) { Control = control; }
    }
    public delegate void ControlEventHandler(object sender, ControlEventArgs e);

    public class CacheVirtualItemsEventArgs : EventArgs
    {
        public int StartIndex { get; }
        public int EndIndex { get; }
        public CacheVirtualItemsEventArgs(int startIndex, int endIndex) { StartIndex = startIndex; EndIndex = endIndex; }
    }
    public delegate void CacheVirtualItemsEventHandler(object sender, CacheVirtualItemsEventArgs e);

    public class RetrieveVirtualItemEventArgs : EventArgs
    {
        public int ItemIndex { get; }
        public ListViewItem Item { get; set; }
        public RetrieveVirtualItemEventArgs(int itemIndex) { ItemIndex = itemIndex; }
    }
    public delegate void RetrieveVirtualItemEventHandler(object sender, RetrieveVirtualItemEventArgs e);

    public class SearchForVirtualItemEventArgs : EventArgs
    {
        public string Text { get; }
        public bool IsTextSearch { get; }
        public bool IsPrefixSearch { get; }
        public bool IncludeSubItemsInSearch { get; }
        public int StartIndex { get; }
        public int Index { get; set; } = -1;
        public SearchForVirtualItemEventArgs(bool isTextSearch, bool isPrefixSearch, bool includeSubItemsInSearch, string text, System.Drawing.Point startingPoint, SearchDirectionHint direction, int startIndex) { IsTextSearch = isTextSearch; IsPrefixSearch = isPrefixSearch; IncludeSubItemsInSearch = includeSubItemsInSearch; Text = text; StartIndex = startIndex; }
    }
    public delegate void SearchForVirtualItemEventHandler(object sender, SearchForVirtualItemEventArgs e);
    public enum SearchDirectionHint { Left, Right, Up, Down }

    public class ListViewVirtualItemsSelectionRangeChangedEventArgs : EventArgs
    {
        public int StartIndex { get; }
        public int EndIndex { get; }
        public bool IsSelected { get; }
        public ListViewVirtualItemsSelectionRangeChangedEventArgs(int startIndex, int endIndex, bool isSelected) { StartIndex = startIndex; EndIndex = endIndex; IsSelected = isSelected; }
    }
    public delegate void ListViewVirtualItemsSelectionRangeChangedEventHandler(object sender, ListViewVirtualItemsSelectionRangeChangedEventArgs e);

    public class SplitterEventArgs : EventArgs
    {
        public int X { get; }
        public int Y { get; }
        public int SplitX { get; set; }
        public int SplitY { get; set; }
        public SplitterEventArgs(int x, int y, int splitX, int splitY) { X = x; Y = y; SplitX = splitX; SplitY = splitY; }
    }
    public delegate void SplitterEventHandler(object sender, SplitterEventArgs e);

    public class ColumnWidthChangedEventArgs : EventArgs
    {
        public int ColumnIndex { get; }
        public ColumnWidthChangedEventArgs(int columnIndex) { ColumnIndex = columnIndex; }
    }
    public delegate void ColumnWidthChangedEventHandler(object sender, ColumnWidthChangedEventArgs e);

    public class FormClosingEventArgs : EventArgs
    {
        public bool Cancel { get; set; }
        public System.Windows.Forms.CloseReason CloseReason { get; }
    }
    public delegate void FormClosingEventHandler(object sender, FormClosingEventArgs e);
    public enum CloseReason { None, WindowsShutDown, MdiFormClosing, UserClosing, TaskManagerClosing, FormOwnerClosing, ApplicationExitCall }

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
        public static void Run(Form mainForm) { }
        public static void Run() { }
        public static void Exit() { }
        public static void ExitThread() { }
        public static void EnableVisualStyles() { }
        public static void SetCompatibleTextRenderingDefault(bool defaultValue) { }
        public static FormCollection OpenForms { get; } = new FormCollection();
        public static event EventHandler Idle;
        public static event System.Threading.ThreadExceptionEventHandler ThreadException;
        public static event EventHandler ApplicationExit;
    }
    public class FormCollection : System.Collections.IEnumerable
    {
        readonly System.Collections.Generic.List<Form> _forms = new();
        public Form this[int i] => i < _forms.Count ? _forms[i] : null;
        public int Count => _forms.Count;
        public System.Collections.IEnumerator GetEnumerator() => _forms.GetEnumerator();
    }

    // ── Control base class ──────────────────────────────────────────────────

    public class Control : IDisposable, System.ComponentModel.IComponent
    {
        // Nested alias so Control.ControlCollection works as a type reference
        public class ControlCollection : System.Windows.Forms.ControlCollection { }

        ControlStyles _styles;
        public virtual string Text { get; set; } = "";
        public virtual bool Visible { get; set; } = true;
        public virtual bool Enabled { get; set; } = true;
        public virtual System.Drawing.Color BackColor { get; set; } = System.Drawing.Color.Transparent;
        public virtual System.Drawing.Color ForeColor { get; set; } = System.Drawing.Color.Black;
        public virtual System.Drawing.Font Font { get; set; }
        public virtual bool AutoSize { get; set; }
        public AutoSizeMode AutoSizeMode { get; set; }
        public virtual System.Drawing.Size ClientSize { get; set; }
        public virtual System.Drawing.Rectangle ClientRectangle => new System.Drawing.Rectangle(System.Drawing.Point.Empty, ClientSize);
        public System.Drawing.Size Size { get; set; }
        public System.Drawing.Point Location { get; set; }
        public string Name { get; set; } = "";
        public int TabIndex { get; set; }
        public AnchorStyles Anchor { get; set; }
        public DockStyle Dock { get; set; }
        public BorderStyle BorderStyle { get; set; }
        public new ControlCollection Controls { get; } = new ControlCollection();
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
        public AutoScaleMode AutoScaleMode { get; set; }
        public IAsyncResult BeginInvoke(Delegate method) => null;
        public IAsyncResult BeginInvoke(Delegate method, params object[] args) => null;
        public object EndInvoke(IAsyncResult asyncResult) => null;
        public System.Drawing.Rectangle Bounds { get => new System.Drawing.Rectangle(Location, Size); set { Location = value.Location; Size = value.Size; } }

        public event EventHandler HandleCreated;
        public event EventHandler Click;
        public event EventHandler DoubleClick;
        public event MouseEventHandler MouseDown;
        public event MouseEventHandler MouseMove;
        public event MouseEventHandler MouseUp;
        public event EventHandler MouseLeave;
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
        protected virtual void OnControlAdded(ControlEventArgs e) { }
        protected virtual void OnControlRemoved(ControlEventArgs e) { }
        public virtual void Refresh() { }
        protected virtual void OnResize(EventArgs e) => Resize?.Invoke(this, e);
        protected virtual void OnVisibleChanged(EventArgs e) => VisibleChanged?.Invoke(this, e);
        protected virtual void OnMouseLeave(EventArgs e) => MouseLeave?.Invoke(this, e);
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
        // IComponent
        public System.ComponentModel.ISite Site { get; set; }
        public event EventHandler Disposed;

        public virtual ImageLayout BackgroundImageLayout { get; set; }
        public virtual System.Drawing.Image BackgroundImage { get; set; }
        public RightToLeft RightToLeft { get; set; } = RightToLeft.No;
        public ImeMode ImeMode { get; set; } = ImeMode.NoControl;
        public string AccessibleDescription { get; set; } = "";
        public string AccessibleName { get; set; } = "";
        public Cursor Cursor { get; set; }
        public System.Drawing.Size MaximumSize { get; set; }
        public System.Drawing.Size MinimumSize { get; set; }
        public Padding Padding { get; set; }
        public Padding Margin { get; set; }
        public bool UseWaitCursor { get; set; }

        public bool ContainsFocus => false;
        public bool CausesValidation { get; set; } = true;
        public bool TabStop { get; set; } = true;
        public event EventHandler Leave;
        public event EventHandler TextChanged;
        public event EventHandler Enter;
        public System.Drawing.Point PointToScreen(System.Drawing.Point p) => p;
        public System.Drawing.Point PointToClient(System.Drawing.Point p) => p;
        public void PerformLayout() { }
        public void PerformLayout(Control affectedControl, string affectedProperty) { }
        public System.Drawing.Graphics CreateGraphics() => null;
        public void Invalidate(System.Drawing.Rectangle rc, bool invalidateChildren) { }
        protected virtual void OnLeave(EventArgs e) => Leave?.Invoke(this, e);
        protected virtual void OnTextChanged(EventArgs e) => TextChanged?.Invoke(this, e);
        protected virtual void OnEnter(EventArgs e) => Enter?.Invoke(this, e);
        protected virtual void OnBackColorChanged(EventArgs e) { }
        protected virtual void OnForeColorChanged(EventArgs e) { }
        protected virtual void OnFontChanged(EventArgs e) { }
        protected virtual void OnActivated(EventArgs e) { }
        public virtual System.Drawing.Rectangle DisplayRectangle => ClientRectangle;
        public void Select() { Focus(); }
        public event EventHandler GotFocus;
        public event EventHandler LostFocus;
        public event EventHandler MouseEnter;
        protected virtual void OnGotFocus(EventArgs e) => GotFocus?.Invoke(this, e);
        protected virtual void OnLostFocus(EventArgs e) => LostFocus?.Invoke(this, e);
        public DragDropEffects DoDragDrop(object data, DragDropEffects allowedEffects) => DragDropEffects.None;
        public bool AllowDrop { get; set; }
        public event DragEventHandler DragEnter;
        public event DragEventHandler DragDrop;
        public event DragEventHandler DragOver;
        public event EventHandler DragLeave;
        public event QueryContinueDragEventHandler QueryContinueDrag;
        protected virtual void OnDragEnter(DragEventArgs e) => DragEnter?.Invoke(this, e);
        protected virtual void OnDragDrop(DragEventArgs e) => DragDrop?.Invoke(this, e);
        protected virtual void OnDragOver(DragEventArgs e) => DragOver?.Invoke(this, e);
        public void BringToFront() { }
        public void SendToBack() { }
        public System.Drawing.Point RectangleToScreen(System.Drawing.Rectangle r) => r.Location;
        public System.Drawing.Rectangle RectangleToClient(System.Drawing.Rectangle r) => r;
        public ContextMenuStrip ContextMenuStrip { get; set; }
        public bool InvokeRequired => false;
        public object Invoke(Delegate d) { d.DynamicInvoke(); return null; }
        public object Invoke(Delegate d, params object[] args) { d.DynamicInvoke(args); return null; }
    }

    // ── Container controls ─────────────────────────────────────────────────

    public class DockPaddingEdges
    {
        public int Left { get; set; }
        public int Right { get; set; }
        public int Top { get; set; }
        public int Bottom { get; set; }
        public int All { set { Left = Right = Top = Bottom = value; } }
    }

    public class Cursor
    {
        public static Cursor Default { get; } = new Cursor();
        public static Cursor Current { get; set; } = new Cursor();
    }
    public static class Cursors
    {
        public static Cursor Default { get; } = new Cursor();
        public static Cursor WaitCursor { get; } = new Cursor();
        public static Cursor Hand { get; } = new Cursor();
        public static Cursor Arrow { get; } = new Cursor();
        public static Cursor AppStarting { get; } = new Cursor();
        public static Cursor Cross { get; } = new Cursor();
        public static Cursor IBeam { get; } = new Cursor();
        public static Cursor No { get; } = new Cursor();
        public static Cursor SizeAll { get; } = new Cursor();
    }

    public class ScrollableControl : Control
    {
        public virtual bool AutoScroll { get; set; }
        public System.Drawing.Size AutoScrollMargin { get; set; }
        public System.Drawing.Size AutoScrollMinSize { get; set; }
        public System.Drawing.Point AutoScrollPosition { get; set; }
        public DockPaddingEdges DockPadding { get; } = new DockPaddingEdges();
        protected virtual void AdjustFormScrollbars(bool displayScrollbars) { }
    }

    public enum AutoSizeMode { GrowAndShrink, GrowOnly }
    public class Panel : ScrollableControl { }
    public class FlowLayoutPanel : Panel
    {
        public FlowDirection FlowDirection { get; set; }
        public bool WrapContents { get; set; } = true;
        public void SetFlowBreak(Control c, bool value) { }
    }
    public class TableLayoutStyleCollection<T> : System.Collections.IEnumerable
    {
        readonly List<T> _items = new();
        public int Add(T item) { _items.Add(item); return _items.Count - 1; }
        public void Clear() => _items.Clear();
        public int Count => _items.Count;
        public System.Collections.IEnumerator GetEnumerator() => _items.GetEnumerator();
    }
    public class TableLayoutPanel : Panel
    {
        public int ColumnCount { get; set; }
        public int RowCount { get; set; }
        public TableLayoutStyleCollection<ColumnStyle> ColumnStyles { get; } = new();
        public TableLayoutStyleCollection<RowStyle> RowStyles { get; } = new();
        public void SetColumnSpan(Control c, int value) { }
        public void SetRowSpan(Control c, int value) { }
        public void PerformLayout() { }
    }
    public enum FlowDirection { LeftToRight, TopDown, RightToLeft, BottomUp }
    public class GroupBox : Control
    {
        public FlatStyle FlatStyle { get; set; }
    }
    public class TabPage : Panel
    {
        public new string Text { get; set; } = "";
        public bool UseVisualStyleBackColor { get; set; }
    }

    public class TabControl : Control
    {
        public TabPageCollection TabPages { get; } = new TabPageCollection();
        public int SelectedIndex { get; set; } = 0;
        public TabPage SelectedTab { get => TabPages.Count > SelectedIndex && SelectedIndex >= 0 ? TabPages[SelectedIndex] : null; set { } }
        public bool Multiline { get; set; }
        public int TabCount => TabPages.Count;
        public System.Drawing.Size ItemSize { get; set; }
        public void SelectTab(int index) { if (index >= 0 && index < TabPages.Count) SelectedIndex = index; }
        public void SelectTab(TabPage page) { for (int i = 0; i < TabPages.Count; i++) { if (TabPages[i] == page) { SelectedIndex = i; break; } } }
        public bool Contains(TabPage page) { for (int i = 0; i < TabPages.Count; i++) if (TabPages[i] == page) return true; return false; }
        public event EventHandler SelectedIndexChanged;
    }

    // ── Form ───────────────────────────────────────────────────────────────

    public class Form : ScrollableControl, IWin32Window
    {
        public static Form ActiveForm => null;
        public Form Owner { get; set; }
        public DialogResult DialogResult { get; set; } = DialogResult.None;
        public bool ShowInTaskbar { get; set; } = true;
        public FormBorderStyle FormBorderStyle { get; set; }
        public FormStartPosition StartPosition { get; set; }
        public FormWindowState WindowState { get; set; }
        public System.Drawing.Size AutoScaleBaseSize { get; set; }
        public System.Drawing.Icon Icon { get; set; }
        public bool TopMost { get; set; }
        public bool ControlBox { get; set; } = true;
        public bool MaximizeBox { get; set; } = true;
        public bool MinimizeBox { get; set; } = true;
        public Button AcceptButton { get; set; }
        public Button CancelButton { get; set; }
        public System.Drawing.SizeF AutoScaleDimensions { get; set; }
        public AutoScaleMode AutoScaleMode { get; set; }

        public double Opacity { get; set; } = 1.0;
        public MenuStrip MainMenuStrip { get; set; }

        public event EventHandler Activated;
        public event FormClosedEventHandler FormClosed;
        public event EventHandler Load;
        public event EventHandler Shown;
        public event FormClosingEventHandler FormClosing;
        public event System.ComponentModel.CancelEventHandler Closing;
        public event EventHandler Closed;
        public bool ShowIcon { get; set; } = true;
        public SizeGripStyle SizeGripStyle { get; set; }

        public virtual void Activate() { Activated?.Invoke(this, EventArgs.Empty); }
        public virtual void Show() { }
        public virtual void Show(IWin32Window owner) { }
        public virtual void Hide() { }
        public virtual DialogResult ShowDialog() { return DialogResult; }
        public virtual DialogResult ShowDialog(IWin32Window owner) { return DialogResult; }
        public virtual void Close() { FormClosed?.Invoke(this, new FormClosedEventArgs()); }
        protected virtual void OnLoad(EventArgs e) => Load?.Invoke(this, e);
        protected virtual void OnClosing(System.ComponentModel.CancelEventArgs e) { }
    }

    public enum AutoScaleMode { None, Font, Dpi, Inherit }

    public class UserControl : ScrollableControl
    {
        protected bool DoubleBuffered { get; set; }
    }

    // ── Simple controls ────────────────────────────────────────────────────

    public class Label : Control
    {
        public System.Drawing.ContentAlignment TextAlign { get; set; }
        public bool UseCompatibleTextRendering { get; set; }
        public System.Drawing.Image Image { get; set; }
        public System.Drawing.ContentAlignment ImageAlign { get; set; }
        public int ImageIndex { get; set; } = -1;
        public ImageList ImageList { get; set; }
        public bool AutoEllipsis { get; set; }
        public bool UseMnemonic { get; set; } = true;
    }
    public class LinkLabel : Label
    {
        public event LinkLabelLinkClickedEventHandler LinkClicked;
        public LinkCollection Links { get; } = new LinkCollection();
        public LinkArea LinkArea { get; set; }
        public bool LinkVisited { get; set; }
        public System.Drawing.Color ActiveLinkColor { get; set; }
        public System.Drawing.Color DisabledLinkColor { get; set; }
        public System.Drawing.Color VisitedLinkColor { get; set; }
        public System.Drawing.Color LinkColor { get; set; }
        public FlatStyle FlatStyle { get; set; }
    }
    public class Button : Control
    {
        public event EventHandler Click;
        public DialogResult DialogResult { get; set; }
        public FlatStyle FlatStyle { get; set; }
        public bool UseVisualStyleBackColor { get; set; }
        public bool UseCompatibleTextRendering { get; set; }
        public System.Drawing.ContentAlignment TextAlign { get; set; }
        public System.Drawing.Image Image { get; set; }
        public System.Drawing.ContentAlignment ImageAlign { get; set; }
        public int ImageIndex { get; set; } = -1;
        public ImageList ImageList { get; set; }
    }
    public class RadioButton : Control
    {
        public bool Checked { get; set; }
        public FlatStyle FlatStyle { get; set; }
        public bool UseVisualStyleBackColor { get; set; }
        public System.Drawing.ContentAlignment TextAlign { get; set; }
        public System.Drawing.ContentAlignment CheckAlign { get; set; }
        public event EventHandler CheckedChanged;
    }
    public class CheckBox : Control
    {
        public CheckState CheckState { get; set; }
        public bool Checked { get; set; }
        public FlatStyle FlatStyle { get; set; }
        public System.Drawing.ContentAlignment CheckAlign { get; set; }
        public System.Drawing.ContentAlignment TextAlign { get; set; }
        public bool UseVisualStyleBackColor { get; set; }
        public bool ThreeState { get; set; }
        public Appearance Appearance { get; set; }
        public System.Drawing.Image Image { get; set; }
        public System.Drawing.ContentAlignment ImageAlign { get; set; }
        public int ImageIndex { get; set; } = -1;
        public event EventHandler CheckedChanged;
        public event EventHandler CheckStateChanged;
    }
    public enum Appearance { Normal, Button }
    public enum ScrollBars { None, Horizontal, Vertical, Both }
    public enum RichTextBoxScrollBars { None, Horizontal, Vertical, Both, ForcedHorizontal, ForcedVertical, ForcedBoth }

    public class TextBox : Control
    {
        public bool Multiline { get; set; }
        public bool ReadOnly { get; set; }
        public ScrollBars ScrollBars { get; set; }
        public int MaxLength { get; set; }
        public bool WordWrap { get; set; } = true;
        public char PasswordChar { get; set; }
        public System.Windows.Forms.HorizontalAlignment TextAlign { get; set; }
        public int SelectionStart { get; set; }
        public int SelectionLength { get; set; }
        public string SelectedText { get; set; } = "";
        public bool HideSelection { get; set; } = true;
        public new void Select() => Focus();
        public void Select(int start, int length) { SelectionStart = start; SelectionLength = length; }
        public void SelectAll() { SelectionStart = 0; SelectionLength = Text?.Length ?? 0; }
        public void AppendText(string text) { Text = (Text ?? "") + text; }
        public void Clear() { Text = ""; }
        public event EventHandler Validated;
        public event System.ComponentModel.CancelEventHandler Validating;
    }
    public class TextBoxBase : TextBox { }
    public enum HorizontalAlignment { Left, Right, Center }

    public class RichTextBox : TextBox
    {
        public string[] Lines { get; set; } = Array.Empty<string>();
        public float ZoomFactor { get; set; } = 1f;
        public int RightMargin { get; set; }
        public int BulletIndent { get; set; }
        public bool DetectUrls { get; set; } = true;
        public bool ShowSelectionMargin { get; set; }
        public new RichTextBoxScrollBars ScrollBars { get; set; }
        public string Rtf { get; set; } = "";
        public System.Windows.Forms.RichTextBoxSelectionTypes SelectionType { get; }
        public bool AcceptsTab { get; set; }
        public event LinkClickedEventHandler LinkClicked;
        public void ScrollToCaret() { }
    }
    public enum RichTextBoxSelectionTypes { Empty, Text, Object, MultiChar, MultiObject }
    public enum PictureBoxSizeMode { Normal, StretchImage, AutoSize, CenterImage, Zoom }

    public class PictureBox : Control, System.ComponentModel.ISupportInitialize
    {
        public System.Drawing.Image Image { get; set; }
        public PictureBoxSizeMode SizeMode { get; set; }
        void System.ComponentModel.ISupportInitialize.BeginInit() { }
        void System.ComponentModel.ISupportInitialize.EndInit() { }
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
        public ComboBoxStyle DropDownStyle { get; set; }
        public int ItemHeight { get; set; } = 13;
        public int MaxDropDownItems { get; set; } = 8;
        public bool IntegralHeight { get; set; } = true;
        public bool FormattingEnabled { get; set; }
        public object SelectedValue { get; set; }
        public object DataSource { get; set; }
        public string DisplayMember { get; set; } = "";
        public string ValueMember { get; set; } = "";
        public void Select() { }
        public void Select(int start, int length) { }
        public int DropDownWidth { get; set; }
        public int DropDownHeight { get; set; }
        public int FindStringExact(string s) => -1;
        public int FindStringExact(string s, int startIndex) => -1;
        public new void SelectAll() { }
        public event EventHandler SelectedIndexChanged;
        public event EventHandler Validated;
        public event System.ComponentModel.CancelEventHandler Validating;
        public void BeginUpdate() { }
        public void EndUpdate() { }
    }

    public class ListBox : Control
    {
        public System.Collections.ArrayList Items { get; } = new System.Collections.ArrayList();
        public int SelectedIndex { get; set; } = -1;
        public object SelectedItem { get; set; }
        public bool Sorted { get; set; }
        public bool IntegralHeight { get; set; } = true;
        public bool HorizontalScrollbar { get; set; }
        public int ItemHeight { get; set; } = 13;
        public int ColumnWidth { get; set; }
        public bool MultiColumn { get; set; }
        public SelectionMode SelectionMode { get; set; }
        public System.Collections.ArrayList SelectedItems { get; } = new System.Collections.ArrayList();
        public System.Collections.ArrayList SelectedIndices { get; } = new System.Collections.ArrayList();
        public AccessibleRole AccessibleRole { get; set; }
        public int HorizontalExtent { get; set; }
        public bool ScrollAlwaysVisible { get; set; }
        public event EventHandler SelectedIndexChanged;
        public void BeginUpdate() { }
        public void EndUpdate() { }
        public void ClearSelected() { SelectedIndices.Clear(); }
    }
    public enum SelectionMode { None, One, MultiSimple, MultiExtended }
    public enum AccessibleRole { Default, None, TitleBar, MenuBar, ScrollBar, Grip, Sound, Cursor, Caret, Alert, Window, Client, MenuPopup, MenuItem, ToolTip, Application, Document, Pane, Chart, Dialog, Border, Grouping, Separator, ToolBar, StatusBar, Table, ColumnHeader, RowHeader, Column, Row, Cell, Link, HelpBalloon, Character, List, ListItem, Outline, OutlineItem, PageTab, PropertyPage, Indicator, Graphic, StaticText, Text, PushButton, CheckButton, RadioButton, ComboBox, DropList, ProgressBar, Dial, HotkeyField, Slider, SpinButton, Diagram, Animation, Equation, ButtonDropDown, ButtonMenu, ButtonDropDownGrid, WhiteSpace, PageTabList, Clock }

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
        public string Description { get; set; } = "";
        public Environment.SpecialFolder RootFolder { get; set; } = Environment.SpecialFolder.Desktop;
        public bool ShowNewFolderButton { get; set; } = true;
        public DialogResult ShowDialog() => DialogResult.Cancel;
        public DialogResult ShowDialog(IWin32Window owner) => DialogResult.Cancel;
        public void Dispose() { }
    }

    // ── ListViewItem ───────────────────────────────────────────────────────

    public class ListViewGroup
    {
        public string Name { get; set; } = "";
        public string Header { get; set; } = "";
        public ListViewGroup(string name, string header = "") { Name = name; Header = header; }
        public ListViewGroup(string header, HorizontalAlignment alignment) { Header = header; }
    }

    public class ListViewGroupCollection : System.Collections.IEnumerable
    {
        readonly List<ListViewGroup> _list = new();
        readonly Dictionary<string, ListViewGroup> _groups = new();
        public ListViewGroup this[string key] => _groups.TryGetValue(key, out var g) ? g : null;
        public ListViewGroup this[int i] => _list[i];
        public int Count => _list.Count;
        public void Add(ListViewGroup g) { _list.Add(g); _groups[g.Name] = g; }
        public void Clear() { _list.Clear(); _groups.Clear(); }
        public int IndexOf(ListViewGroup g) => _list.IndexOf(g);
        public void Remove(ListViewGroup g) { _list.Remove(g); _groups.Remove(g.Name); }
        public System.Collections.IEnumerator GetEnumerator() => _list.GetEnumerator();
    }

    public class ListViewItem
    {
        public string Text { get; set; } = "";
        public ListViewGroup Group { get; set; }
        public ListView ListView { get; internal set; }
        public ListViewSubItemCollection SubItems { get; }
        public int ImageIndex { get; set; } = -1;
        public string ImageKey { get; set; } = "";
        public int StateImageIndex { get; set; } = -1;
        public int Index { get; internal set; } = -1;
        public ImageList ImageList { get; internal set; }
        public bool Checked { get; set; }
        public bool Selected { get; set; }
        public bool Focused { get; set; }
        public bool UseItemStyleForSubItems { get; set; } = true;
        public object Tag { get; set; }
        public System.Drawing.Color ForeColor { get; set; }
        public System.Drawing.Color BackColor { get; set; }
        public System.Drawing.Font Font { get; set; }
        public string ToolTipText { get; set; } = "";

        public ListViewItem() { SubItems = new ListViewSubItemCollection(); }
        public ListViewItem(string text) : this() { Text = text; }
        public ListViewItem(string text, int imageIndex) : this() { Text = text; ImageIndex = imageIndex; }
        public ListViewItem(string[] items) : this() { if (items?.Length > 0) { Text = items[0]; for (int i = 1; i < items.Length; i++) SubItems.Add(items[i]); } }
        public System.Drawing.Rectangle GetBounds(ItemBoundsPortion portion) => System.Drawing.Rectangle.Empty;
        public void EnsureVisible() { }
        public ListViewItem Clone() { var c = new ListViewItem(Text) { ImageIndex = ImageIndex, ImageKey = ImageKey, StateImageIndex = StateImageIndex, Checked = Checked, Tag = Tag, ForeColor = ForeColor, BackColor = BackColor, Font = Font, Group = Group }; foreach (SubItem s in SubItems) c.SubItems.Add(s.Text); return c; }
        // public alias used by code that references ListViewItem.ListViewSubItem
        public class ListViewSubItem : SubItem { }

        public class ListViewSubItemCollection : System.Collections.IEnumerable
        {
            readonly List<SubItem> _items = new();
            public SubItem this[int i] => _items[i];
            public int Count => _items.Count;
            public SubItem Add(string text) { var s = new SubItem { Text = text }; _items.Add(s); return s; }
            public SubItem Add(string text, System.Drawing.Color foreColor, System.Drawing.Color backColor, System.Drawing.Font font) { var s = new SubItem { Text = text, ForeColor = foreColor, BackColor = backColor, Font = font }; _items.Add(s); return s; }
            public void Clear() => _items.Clear();
            public void Remove(SubItem s) => _items.Remove(s);
            public System.Collections.IEnumerator GetEnumerator() => _items.GetEnumerator();
        }

        public class SubItem
        {
            public string Text { get; set; } = "";
            public System.Drawing.Color ForeColor { get; set; }
            public System.Drawing.Color BackColor { get; set; }
            public System.Drawing.Font Font { get; set; }
        }
    }

    public class ColumnHeaderCollection : System.Collections.IEnumerable
    {
        readonly List<ColumnHeader> _cols = new();
        public ColumnHeader this[int i] => _cols[i];
        public int Count => _cols.Count;
        public void Add(ColumnHeader h) => _cols.Add(h);
        public void AddRange(ColumnHeader[] headers) { foreach (var h in headers) _cols.Add(h); }
        public void Clear() => _cols.Clear();
        public void Remove(ColumnHeader h) => _cols.Remove(h);
        public System.Collections.IEnumerator GetEnumerator() => _cols.GetEnumerator();
    }

    public class ListView : Control
    {
        public ListViewGroupCollection Groups { get; } = new ListViewGroupCollection();
        public ListViewItemCollection Items { get; }
        public ColumnHeaderCollection Columns { get; } = new ColumnHeaderCollection();
        public View View { get; set; }
        public bool FullRowSelect { get; set; }
        public bool MultiSelect { get; set; } = true;
        public bool ShowGroups { get; set; } = true;
        public bool GridLines { get; set; }
        public ColumnHeaderStyle HeaderStyle { get; set; } = ColumnHeaderStyle.Clickable;
        public ContextMenuStrip ContextMenuStrip { get; set; }
        public ImageList SmallImageList { get; set; }
        public ImageList LargeImageList { get; set; }
        public bool HideSelection { get; set; } = true;
        public bool UseCompatibleStateImageBehavior { get; set; }
        public SortOrder Sorting { get; set; }
        public SelectedListViewItemCollection SelectedItems { get; private set; }
        public SelectedIndexCollection SelectedIndices { get; private set; }
        public bool CheckBoxes { get; set; }
        public System.Collections.IEnumerable CheckedItems => Items;
        public System.Collections.IList CheckedIndices => new System.Collections.ArrayList();
        public System.Collections.IComparer ListViewItemSorter { get; set; }
        public System.Drawing.Size TileSize { get; set; }
        public bool LabelEdit { get; set; }
        public bool VirtualMode { get; set; }
        public int VirtualListSize { get; set; }
        public event EventHandler SelectedIndexChanged;
        public event LabelEditEventHandler AfterLabelEdit;
        public event LabelEditEventHandler BeforeLabelEdit;
        public event ItemCheckedEventHandler ItemChecked;
        public event ColumnClickEventHandler ColumnClick;
        public event CacheVirtualItemsEventHandler CacheVirtualItems;
        public event RetrieveVirtualItemEventHandler RetrieveVirtualItem;
        public event SearchForVirtualItemEventHandler SearchForVirtualItem;
        public event ListViewVirtualItemsSelectionRangeChangedEventHandler VirtualItemsSelectionRangeChanged;
        public ImageList StateImageList { get; set; }
        public ListViewItem FocusedItem { get; set; }
        public bool Focused { get; set; }
        public bool ShowItemToolTips { get; set; }
        public ItemActivation Activation { get; set; }
        public event EventHandler ItemActivate;
        public ListViewItem GetItemAt(int x, int y) => null;
        public void Clear() { Items.Clear(); Groups.Clear(); Columns.Clear(); }
        public void EnsureVisible(int index) { }
        public void Sort() { }
        public void BeginUpdate() { }
        public void EndUpdate() { }

        public ListView() { Items = new ListViewItemCollection(this); SelectedItems = new SelectedListViewItemCollection(); SelectedIndices = new SelectedIndexCollection(); }

        public class ListViewItemCollection : System.Collections.IEnumerable
        {
            readonly List<ListViewItem> _items = new();
            readonly ListView _owner;
            public ListViewItemCollection(ListView owner) { _owner = owner; }
            public ListViewItem this[int i] { get => _items[i]; set { if (i >= 0 && i < _items.Count) _items[i] = value; } }
            public int Count => _items.Count;
            public ListViewItem Add(ListViewItem item) { item.ListView = _owner; item.Index = _items.Count; item.ImageList = _owner?.SmallImageList; _items.Add(item); return item; }
            public ListViewItem Add(string text) { var item = new ListViewItem(text); return Add(item); }
            public ListViewItem Add(string text, int imageIndex) { var item = new ListViewItem(text) { ImageIndex = imageIndex }; return Add(item); }
            public ListViewItem Add(string text, string imageKey) { var item = new ListViewItem(text); return Add(item); }
            public void Insert(int index, ListViewItem item) { item.ListView = _owner; if (index < 0) index = 0; if (index > _items.Count) index = _items.Count; _items.Insert(index, item); }
            public void AddRange(ListViewItem[] items) { foreach (var i in items) Add(i); }
            public void Remove(ListViewItem item) => _items.Remove(item);
            public void RemoveAt(int i) { if (i >= 0 && i < _items.Count) _items.RemoveAt(i); }
            public bool Contains(ListViewItem item) => _items.Contains(item);
            public int IndexOf(ListViewItem item) => _items.IndexOf(item);
            public void Clear() => _items.Clear();
            public void CopyTo(Array array, int index) => ((System.Collections.ICollection)_items).CopyTo(array, index);
            public System.Collections.IEnumerator GetEnumerator() => _items.GetEnumerator();
        }

        public class SelectedIndexCollection : System.Collections.IEnumerable
        {
            readonly List<int> _indices = new();
            public int this[int i] => _indices[i];
            public int Count => _indices.Count;
            public bool Contains(int index) => _indices.Contains(index);
            public void Add(int index) { if (!_indices.Contains(index)) _indices.Add(index); }
            public void Clear() => _indices.Clear();
            public System.Collections.IEnumerator GetEnumerator() => _indices.GetEnumerator();
        }

        public class SelectedListViewItemCollection : System.Collections.IEnumerable
        {
            readonly List<ListViewItem> _items = new();
            public ListViewItem this[int i] => _items[i];
            public int Count => _items.Count;
            public bool Contains(ListViewItem item) => _items.Contains(item);
            public void Add(ListViewItem item) { if (!_items.Contains(item)) _items.Add(item); }
            public void Clear() => _items.Clear();
            public System.Collections.IEnumerator GetEnumerator() => _items.GetEnumerator();
        }
    }

    // ── ImageList ─────────────────────────────────────────────────────────

    public class ImageList : IDisposable
    {
        public ImageList() { }
        public ImageList(System.ComponentModel.IContainer container) { }
        public ImageListImageCollection Images { get; } = new ImageListImageCollection();
        public System.Drawing.Size ImageSize { get; set; } = new System.Drawing.Size(16, 16);
        public ColorDepth ColorDepth { get; set; } = ColorDepth.Depth8Bit;
        public System.Drawing.Color TransparentColor { get; set; } = System.Drawing.Color.Transparent;
        public ImageListStreamer ImageStream { get; set; }
        public void Dispose() { }
    }

    public class ImageListImageCollection
    {
        readonly List<System.Drawing.Image> _images = new();
        public System.Drawing.Image this[int i] { get => i < _images.Count ? _images[i] : null; set { while (_images.Count <= i) _images.Add(null); _images[i] = value; } }
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
        public Timer() { }
        public Timer(System.ComponentModel.IContainer container) { }
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
        public static DialogResult Show(string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon, MessageBoxDefaultButton defaultButton) => Show(text, caption);
        public static DialogResult Show(IWin32Window owner, string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon) => Show(text, caption);
    }

    // ── CheckedListBox ────────────────────────────────────────────────────

    public class CheckedListBox : Control
    {
        readonly List<object> _items = new();
        readonly List<bool> _checked = new();
        public int SelectedIndex { get; set; } = -1;
        public CheckedListBoxItemCollection Items { get; }
        public CheckedListBox() { Items = new CheckedListBoxItemCollection(_items, _checked); }
        public void SetItemChecked(int index, bool value) { if (index < _checked.Count) _checked[index] = value; }
        public bool GetItemChecked(int index) => index < _checked.Count && _checked[index];
        public void SetSelected(int index, bool value) { SelectedIndex = value ? index : -1; }
        public bool IntegralHeight { get; set; } = true;
        public bool CheckOnClick { get; set; }
        public bool HorizontalScrollbar { get; set; }
        public bool Sorted { get; set; }
        public CheckedListBoxItemCollection CheckedItems => Items; // same collection, callers just query checked ones
        public System.Collections.IEnumerable CheckedIndices => Items.GetCheckedIndices();
        public event ItemCheckEventHandler ItemCheck;
        public event ItemCheckedEventHandler ItemChecked;
    }

    public class CheckedListBoxItemCollection : System.Collections.IEnumerable
    {
        readonly List<object> _items;
        readonly List<bool> _checked;
        public CheckedListBoxItemCollection(List<object> items, List<bool> chk) { _items = items; _checked = chk; }
        public object this[int i] => _items[i];
        public int Count => _items.Count;
        public void Clear() { _items.Clear(); _checked.Clear(); }
        public void AddRange(string[] items) { foreach (var s in items) { _items.Add(s); _checked.Add(false); } }
        public void AddRange(object[] items) { foreach (var s in items) { _items.Add(s); _checked.Add(false); } }
        public void Add(string item) { _items.Add(item); _checked.Add(false); }
        public void Add(object item) { _items.Add(item); _checked.Add(false); }
        public void Add(object item, bool isChecked) { _items.Add(item); _checked.Add(isChecked); }
        public void Insert(int index, object item) { _items.Insert(index, item); _checked.Insert(index, false); }
        public void Insert(int index, object item, bool isChecked) { _items.Insert(index, item); _checked.Insert(index, isChecked); }
        public System.Collections.IEnumerable GetCheckedIndices() { for (int i = 0; i < _checked.Count; i++) if (_checked[i]) yield return i; }
        public System.Collections.IEnumerator GetEnumerator() => _items.GetEnumerator();
    }

    // ── Collection helpers ─────────────────────────────────────────────────

    public class ControlCollection : System.Collections.ICollection, System.Collections.IEnumerable
    {
        readonly List<Control> _items = new();
        public Control this[int i] => _items[i];
        public Control this[string key] => _items.FirstOrDefault(c => c.Name == key);
        public void Add(Control c) => _items.Add(c);
        public void Add(Control c, int column, int row) => _items.Add(c); // TableLayoutPanel overload
        public void AddRange(Control[] controls) { foreach (var c in controls) _items.Add(c); }
        public void Remove(Control c) => _items.Remove(c);
        public bool Contains(Control c) => _items.Contains(c);
        public void Clear() => _items.Clear();
        public int Count => _items.Count;
        public int IndexOfKey(string key) => _items.FindIndex(c => c.Name == key);
        public void SetChildIndex(Control c, int index) { _items.Remove(c); if (index >= 0 && index <= _items.Count) _items.Insert(index, c); else _items.Add(c); }
        public bool IsSynchronized => false;
        public object SyncRoot => this;
        public void CopyTo(Array array, int index) => ((System.Collections.ICollection)_items).CopyTo(array, index);
        public System.Collections.IEnumerator GetEnumerator() => _items.GetEnumerator();
    }

    public class TabPageCollection : System.Collections.IEnumerable
    {
        readonly List<TabPage> _items = new();
        public TabPage this[int i] => _items[i];
        public int Count => _items.Count;
        public void Add(TabPage p) => _items.Add(p);
        public void Remove(TabPage p) => _items.Remove(p);
        public void RemoveAt(int i) { if (i >= 0 && i < _items.Count) _items.RemoveAt(i); }
        public void Insert(int i, TabPage p) => _items.Insert(i, p);
        public void Clear() => _items.Clear();
        public bool Contains(TabPage p) => _items.Contains(p);
        public System.Collections.IEnumerator GetEnumerator() => _items.GetEnumerator();
    }

    public class LinkLabelLink
    {
        public bool Enabled { get; set; } = true;
        public object LinkData { get; set; }
        public int Start { get; set; }
        public int Length { get; set; }
        public bool Visited { get; set; }
    }

    public class LinkCollection : System.Collections.IEnumerable
    {
        readonly List<LinkLabelLink> _links = new();
        public LinkCollection() { _links.Add(new LinkLabelLink()); } // default one link
        public LinkLabelLink this[int i] => i < _links.Count ? _links[i] : null;
        public int Count => _links.Count;
        public void Add(LinkLabelLink link) => _links.Add(link);
        public void Add(object link) => _links.Add(new LinkLabelLink { LinkData = link });
        public void Add(int start, int length, object linkData) => _links.Add(new LinkLabelLink { LinkData = linkData });
        public void Clear() => _links.Clear();
        public System.Collections.IEnumerator GetEnumerator() => _links.GetEnumerator();
    }

    // ── Tree & toolbar controls ────────────────────────────────────────────

    public class TreeNode
    {
        public string Text { get; set; } = "";
        public TreeNodeCollection Nodes { get; } = new TreeNodeCollection();
        public TreeNode Parent { get; internal set; }
        public object Tag { get; set; }
        public int ImageIndex { get; set; } = -1;
        public int SelectedImageIndex { get; set; } = -1;
        public System.Drawing.Color ForeColor { get; set; }
        public System.Drawing.Color BackColor { get; set; }
        public System.Drawing.Font NodeFont { get; set; }
        public bool IsExpanded { get; protected set; }
        public void EnsureVisible() { }
        public void Expand() { IsExpanded = true; }
        public void Collapse() { IsExpanded = false; }
        public void ExpandAll() { }
        public TreeNode() { }
        public TreeNode(string text) { Text = text; }
        public TreeNode(string text, int imageIndex, int selectedImageIndex) { Text = text; ImageIndex = imageIndex; SelectedImageIndex = selectedImageIndex; }
        public TreeNode(string text, TreeNode[] children) { Text = text; foreach (var c in children) Nodes.Add(c); }
    }

    public class TreeNodeCollection : System.Collections.IEnumerable
    {
        readonly List<TreeNode> _nodes = new();
        public TreeNode this[int i] => _nodes[i];
        public int Count => _nodes.Count;
        public TreeNode Add(string text) { var n = new TreeNode(text); _nodes.Add(n); return n; }
        public void Add(TreeNode n) { _nodes.Add(n); }
        public void Remove(TreeNode n) => _nodes.Remove(n);
        public void RemoveAt(int i) { if (i >= 0 && i < _nodes.Count) _nodes.RemoveAt(i); }
        public void Clear() => _nodes.Clear();
        public bool Contains(TreeNode n) => _nodes.Contains(n);
        public System.Collections.IEnumerator GetEnumerator() => _nodes.GetEnumerator();
    }

    public enum TreeViewAction { Unknown, ByKeyboard, ByMouse, Collapse, Expand }

    public class TreeViewEventArgs : EventArgs
    {
        public TreeNode Node { get; }
        public TreeViewAction Action { get; }
        public TreeViewEventArgs(TreeNode node) { Node = node; }
        public TreeViewEventArgs(TreeNode node, TreeViewAction action) { Node = node; Action = action; }
    }
    public delegate void TreeViewEventHandler(object sender, TreeViewEventArgs e);

    public class TreeView : Control
    {
        public TreeNodeCollection Nodes { get; } = new TreeNodeCollection();
        public TreeNode SelectedNode { get; set; }
        public bool Sorted { get; set; }
        public int ImageIndex { get; set; } = -1;
        public int SelectedImageIndex { get; set; } = -1;
        public ImageList ImageList { get; set; }
        public ContextMenuStrip ContextMenuStrip { get; set; }
        public bool HideSelection { get; set; } = true;
        public bool FullRowSelect { get; set; }
        public bool ShowLines { get; set; } = true;
        public bool ShowPlusMinus { get; set; } = true;
        public bool ShowRootLines { get; set; } = true;
        public bool CheckBoxes { get; set; }
        public bool LabelEdit { get; set; }
        public int ItemHeight { get; set; } = 16;
        public int Indent { get; set; } = 19;
        public ImageList StateImageList { get; set; }
        public event TreeViewEventHandler AfterSelect;
        public event TreeViewEventHandler BeforeSelect;
        public event TreeViewEventHandler AfterExpand;
        public event TreeViewEventHandler BeforeExpand;
        public event EventHandler DoubleClick;
        public void BeginUpdate() { }
        public void EndUpdate() { }
        public void ExpandAll() { }
        public void CollapseAll() { }
    }

    public class SplitContainer : Control
    {
        public Panel Panel1 { get; } = new Panel();
        public Panel Panel2 { get; } = new Panel();
        public Orientation Orientation { get; set; }
        public int SplitterDistance { get; set; }
        public int SplitterWidth { get; set; } = 4;
        public FixedPanel FixedPanel { get; set; }
        public bool IsSplitterFixed { get; set; }
    }

    public class Splitter : Control
    {
        public int MinSize { get; set; }
        public int MinExtra { get; set; }
        public int SplitterDistance { get; set; }
    }

    public enum ItemBoundsPortion { Entire, Icon, Label, ItemOnly }

    public class ColumnHeader
    {
        static int _counter;
        public string Text { get; set; } = "";
        public int Width { get; set; }
        public HorizontalAlignment TextAlign { get; set; }
        public int Index { get; internal set; } = _counter++;
        public int DisplayIndex { get; set; } = -1;
        public ListView ListView { get; internal set; }
        public ColumnHeader() { }
        public ColumnHeader(string text, int width = 100) { Text = text; Width = width; }
    }

    public class ContextMenuStrip : Control
    {
        public ToolStripItemCollection Items { get; } = new ToolStripItemCollection();
        public ToolStripRenderer Renderer { get; set; }
        public ToolStripRenderMode RenderMode { get; set; }
        public ContextMenuStrip() { }
        public ContextMenuStrip(System.ComponentModel.IContainer container) { }
        public event System.ComponentModel.CancelEventHandler Opening;
        public event System.ComponentModel.CancelEventHandler Closing;
        public void Show(Control control, System.Drawing.Point point) { }
        public void Show(Control control, int x, int y) { }
    }

    public class ToolStripMenuItem : ToolStripItem
    {
        public bool Checked { get; set; }
        public Keys ShortcutKeys { get; set; }
        public ToolStripItemCollection DropDownItems { get; } = new ToolStripItemCollection();
        public event EventHandler CheckedChanged;
        public ToolStripMenuItem() { }
        public ToolStripMenuItem(string text) { Text = text; }
    }

    public class ToolStripSeparator : ToolStripItem { }

    public enum TextImageRelation { Overlay, ImageAboveText, TextAboveImage, ImageBeforeText, TextBeforeImage }
    public enum ToolStripStatusLabelBorderSides { None = 0, Left = 1, Top = 2, Right = 4, Bottom = 8, All = Left | Top | Right | Bottom }

    public class ToolStripItem
    {
        public string Text { get; set; } = "";
        public string Name { get; set; } = "";
        public bool Enabled { get; set; } = true;
        public bool Visible { get; set; } = true;
        public bool Selected { get; protected set; }
        public bool Pressed { get; protected set; }
        public object Tag { get; set; }
        public System.Drawing.Image Image { get; set; }
        public System.Drawing.Size Size { get; set; }
        public string ToolTipText { get; set; } = "";
        public System.Drawing.ContentAlignment ImageAlign { get; set; }
        public System.Drawing.ContentAlignment TextAlign { get; set; }
        public int ImageIndex { get; set; } = -1;
        public Padding Padding { get; set; }
        public Padding Margin { get; set; }
        public AnchorStyles Anchor { get; set; }
        public DockStyle Dock { get; set; }
        public bool AutoSize { get; set; } = true;
        public ToolStripItemImageScaling ImageScaling { get; set; } = ToolStripItemImageScaling.SizeToFit;
        public event EventHandler Click;
        public event EventHandler EnabledChanged;
        protected void OnClick(EventArgs e) => Click?.Invoke(this, e);
        public void PerformClick() => OnClick(EventArgs.Empty);
    }

    public class ToolStripItemCollection : System.Collections.IEnumerable
    {
        readonly List<object> _items = new();
        public ToolStripItem this[int i] => _items[i] as ToolStripItem;
        public void Add(string text) => _items.Add(text == "-" ? (object)new ToolStripSeparator() : new ToolStripMenuItem(text));
        public void Add(ToolStripMenuItem item) => _items.Add(item);
        public void Add(ToolStripSeparator sep) => _items.Add(sep);
        public void Add(ToolStripItem item) => _items.Add(item);
        public void Insert(int index, ToolStripItem item) => _items.Insert(index, item);
        public void Insert(int index, ToolStripMenuItem item) => _items.Insert(index, item);
        public void AddRange(ToolStripMenuItem[] items) { foreach (var i in items) _items.Add(i); }
        public void AddRange(ToolStripItem[] items) { foreach (var i in items) _items.Add(i); }
        public int Count => _items.Count;
        public void Clear() => _items.Clear();
        public bool Contains(ToolStripItem item) => _items.Contains(item);
        public void Remove(ToolStripItem item) => _items.Remove(item);
        public void RemoveAt(int index) { if (index >= 0 && index < _items.Count) _items.RemoveAt(index); }
        public int IndexOf(ToolStripItem item) => _items.IndexOf(item);
        public System.Collections.IEnumerator GetEnumerator() => _items.GetEnumerator();
    }

    public class ToolStrip : Control
    {
        public ToolStripItemCollection Items { get; } = new ToolStripItemCollection();
        public ToolStripRenderer Renderer { get; set; }
        public ToolStripRenderMode RenderMode { get; set; }
        public ToolStripGripStyle GripStyle { get; set; }
        public System.Drawing.Size ImageScalingSize { get; set; } = new System.Drawing.Size(16, 16);
        public ToolStripLayoutStyle LayoutStyle { get; set; }
        public bool ShowItemToolTips { get; set; } = true;
    }
    public enum ToolStripGripStyle { Visible, Hidden }
    public enum ToolStripItemImageScaling { None, SizeToFit }

    public class MenuStrip : ToolStrip { }
    public class StatusStrip : ToolStrip { }
    public class ToolStripStatusLabel : ToolStripItem
    {
        public ToolStripStatusLabelBorderSides BorderSides { get; set; }
        public BorderStyle BorderStyle { get; set; }
        public bool Spring { get; set; }
        public ToolStripStatusLabel() { }
        public ToolStripStatusLabel(string text) { Text = text; }
    }
    public class ToolStripProgressBar : ToolStripItem
    {
        public int Minimum { get; set; }
        public int Maximum { get; set; } = 100;
        public int Value { get; set; }
        public int Step { get; set; } = 10;
        public void PerformStep() { Value = Math.Min(Value + Step, Maximum); }
    }

    public abstract class FileDialog : IDisposable
    {
        public string FileName { get; set; } = "";
        public string Filter { get; set; } = "";
        public string Title { get; set; } = "";
        public string DefaultExt { get; set; } = "";
        public string InitialDirectory { get; set; } = "";
        public int FilterIndex { get; set; } = 1;
        public bool AddExtension { get; set; } = true;
        public bool ValidateNames { get; set; } = true;
        public bool CheckFileExists { get; set; } = true;
        public bool CheckPathExists { get; set; } = true;
        public bool DereferenceLinks { get; set; } = true;
        public bool ShowHelp { get; set; }
        public bool RestoreDirectory { get; set; }
        public string[] FileNames { get; set; } = Array.Empty<string>();
        public virtual DialogResult ShowDialog() => DialogResult.Cancel;
        public virtual DialogResult ShowDialog(IWin32Window owner) => DialogResult.Cancel;
        public void Dispose() { }
    }

    public class OpenFileDialog : FileDialog
    {
        public bool Multiselect { get; set; }
        public bool ReadOnlyChecked { get; set; }
        public bool ShowReadOnly { get; set; }
    }

    public class SaveFileDialog : FileDialog
    {
        public bool OverwritePrompt { get; set; } = true;
    }

    public class ImageListStreamer { }

    public static class DataFormats
    {
        public static readonly string Text = "Text";
        public static readonly string UnicodeText = "UnicodeText";
        public static readonly string FileDrop = "FileDrop";
        public static readonly string Bitmap = "Bitmap";
        public static readonly string Html = "HTML Format";
        public static readonly string Rtf = "Rich Text Format";
        public static readonly string CommaSeparatedValue = "CSV";
        public static readonly string Dib = "DeviceIndependentBitmap";
        public static readonly string MetafilePict = "MetaFilePict";
        public static readonly string OemText = "OemText";
        public static readonly string Serializable = "WindowsForms10PersistentObject";
        public static readonly string SymbolicLink = "SymLink";
    }

    public class ColorDialog : IDisposable
    {
        public System.Drawing.Color Color { get; set; } = System.Drawing.Color.Black;
        public bool AllowFullOpen { get; set; } = true;
        public bool AnyColor { get; set; }
        public bool FullOpen { get; set; }
        public int[] CustomColors { get; set; } = Array.Empty<int>();
        public DialogResult ShowDialog() => DialogResult.Cancel;
        public DialogResult ShowDialog(IWin32Window owner) => DialogResult.Cancel;
        public void Dispose() { }
    }

    public class FontDialog : IDisposable
    {
        public System.Drawing.Font Font { get; set; }
        public System.Drawing.Color Color { get; set; } = System.Drawing.Color.Black;
        public bool ShowColor { get; set; }
        public bool ShowEffects { get; set; } = true;
        public DialogResult ShowDialog() => DialogResult.Cancel;
        public DialogResult ShowDialog(IWin32Window owner) => DialogResult.Cancel;
        public void Dispose() { }
    }

    public struct LinkArea
    {
        public int Start;
        public int Length;
        public LinkArea(int start, int length) { Start = start; Length = length; }
    }

    public class OSFeature
    {
        public static readonly OSFeature Feature = new OSFeature();
        public static readonly object LayeredWindows = new object();
        public static readonly object Themes = new object();
        public bool IsPresent(object feature) => false;
    }

    public static class Clipboard
    {
        public static string GetText() => "";
        public static string GetText(TextDataFormat format) => "";
        public static void SetText(string text) { }
        public static void SetText(string text, TextDataFormat format) { }
        public static bool ContainsText() => false;
        public static System.Drawing.Image GetImage() => null;
        public static void SetImage(System.Drawing.Image image) { }
        public static bool ContainsImage() => false;
        public static object GetData(string format) => null;
        public static void SetData(string format, object data) { }
        public static void SetDataObject(object data) { }
        public static void SetDataObject(object data, bool copy) { }
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
        public bool ReadOnly { get; set; }
        public event EventHandler ValueChanged;
    }

    public class ToolTip : IDisposable
    {
        public ToolTip() { }
        public ToolTip(System.ComponentModel.IContainer container) { }
        public bool Active { get; set; } = true;
        public int AutomaticDelay { get; set; } = 500;
        public int AutoPopDelay { get; set; } = 5000;
        public int InitialDelay { get; set; } = 500;
        public int ReshowDelay { get; set; } = 100;
        public bool ShowAlways { get; set; }
        public bool IsBalloon { get; set; }
        public void SetToolTip(Control c, string text) { }
        public string GetToolTip(Control c) => "";
        public void RemoveAll() { }
        public string ToolTipTitle { get; set; } = "";
        public ToolTipIcon ToolTipIcon { get; set; }
        public void Dispose() { }
    }
    public enum ToolTipIcon { None, Info, Warning, Error }

    public class NotifyIcon : IDisposable
    {
        public System.Drawing.Icon Icon { get; set; }
        public string Text { get; set; } = "";
        public bool Visible { get; set; }
        public ContextMenuStrip ContextMenuStrip { get; set; }
        public void Dispose() { }
    }

    public class KeysConverter : System.ComponentModel.TypeConverter
    {
        public override object ConvertFrom(System.ComponentModel.ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value)
            => value is string s ? (Keys)Enum.Parse(typeof(Keys), s, true) : base.ConvertFrom(context, culture, value);
        public override object ConvertTo(System.ComponentModel.ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, Type destinationType)
            => destinationType == typeof(string) && value is Keys k ? k.ToString() : base.ConvertTo(context, culture, value, destinationType);
    }

    public class ToolStripDropDownMenu : ContextMenuStrip
    {
        public ToolStripDropDownMenu() { }
    }

    public static class Help
    {
        public static void ShowHelp(Control parent, string url) { }
        public static void ShowHelp(Control parent, string url, string keyword) { }
        public static void ShowHelpIndex(Control parent, string url) { }
        public static void ShowPopup(Control parent, string caption, System.Drawing.Point location) { }
    }

    public class GridItem { public string Label { get; } public object Value { get; } }

    public class PropertyValueChangedEventArgs : EventArgs
    {
        public GridItem ChangedItem { get; }
        public object OldValue { get; }
    }
    public delegate void PropertyValueChangedEventHandler(object sender, PropertyValueChangedEventArgs e);

    public class PropertyGrid : Control
    {
        public object SelectedObject { get; set; }
        public object[] SelectedObjects { get; set; } = Array.Empty<object>();
        public System.Drawing.Color LineColor { get; set; }
        public System.Drawing.Color ViewForeColor { get; set; }
        public System.Drawing.Color ViewBackColor { get; set; }
        public System.Drawing.Color CommandsBackColor { get; set; }
        public bool HelpVisible { get; set; } = true;
        public bool ToolbarVisible { get; set; } = true;
        public bool LargeButtons { get; set; }
        public bool CommandsVisibleIfAvailable { get; set; } = true;
        public PropertyGridViewMode PropertyViewMode { get; set; }
        public PropertySort PropertySort { get; set; } = PropertySort.CategorizedAlphabetical;
        public event EventHandler SelectedObjectsChanged;
        public event PropertyValueChangedEventHandler PropertyValueChanged;
        public new void Refresh() { }
        public void CollapseAllGridItems() { }
        public void ExpandAllGridItems() { }
    }
    public enum PropertyGridViewMode { Default }
    public enum PropertySort { NoSort, Alphabetical, Categorized, CategorizedAlphabetical }

    public class ToolStripButton : ToolStripItem
    {
        public bool Checked { get; set; }
        public ToolStripItemDisplayStyle DisplayStyle { get; set; } = ToolStripItemDisplayStyle.ImageAndText;
        public TextImageRelation TextImageRelation { get; set; }
        public ToolStripItemAlignment Alignment { get; set; }
        public bool CheckOnClick { get; set; }
        public CheckState CheckState { get; set; }
        public System.Drawing.Font Font { get; set; }
        public ToolStripItemOverflow Overflow { get; set; }
        public ToolStripButton() { }
        public ToolStripButton(string text) { Text = text; }
        public ToolStripButton(System.Drawing.Image image) { Image = image; }
        public ToolStripButton(string text, System.Drawing.Image image) { Text = text; Image = image; }
    }

    public class ToolStripPanel : Control
    {
        public ToolStripRenderer Renderer { get; set; }
        public ToolStripRenderMode RenderMode { get; set; }
    }

    public class ToolStripContainer : Control
    {
        public ToolStripPanel TopToolStripPanel { get; } = new ToolStripPanel();
        public ToolStripPanel BottomToolStripPanel { get; } = new ToolStripPanel();
        public ToolStripPanel LeftToolStripPanel { get; } = new ToolStripPanel();
        public ToolStripPanel RightToolStripPanel { get; } = new ToolStripPanel();
        public Panel ContentPanel { get; } = new Panel();
    }

    public class ToolStripRenderEventArgs : EventArgs
    {
        public System.Drawing.Graphics Graphics { get; }
        public ToolStrip ToolStrip { get; }
        public System.Drawing.Rectangle AffectedBounds { get; }
        public System.Drawing.Color BackColor { get; }
    }

    public class ToolStripItemRenderEventArgs : EventArgs
    {
        public System.Drawing.Graphics Graphics { get; }
        public ToolStripItem Item { get; }
    }

    public class ToolStripRenderer
    {
        protected virtual void OnRenderToolStripBackground(ToolStripRenderEventArgs e) { }
        protected virtual void OnRenderToolStripBorder(ToolStripRenderEventArgs e) { }
        protected virtual void OnRenderImageMargin(ToolStripRenderEventArgs e) { }
        protected virtual void OnRenderMenuItemBackground(ToolStripItemRenderEventArgs e) { }
        protected virtual void OnRenderButtonBackground(ToolStripItemRenderEventArgs e) { }
        protected virtual void OnRenderItemText(ToolStripItemRenderEventArgs e) { }
        protected virtual void OnRenderItemImage(ToolStripItemRenderEventArgs e) { }
        protected virtual void OnRenderSeparator(ToolStripSeparatorRenderEventArgs e) { }
        protected virtual void OnRenderOverflowButtonBackground(ToolStripItemRenderEventArgs e) { }
        protected virtual void OnRenderItemBackground(ToolStripItemRenderEventArgs e) { }
        public event EventHandler<ToolStripRenderEventArgs> RenderToolStripBackground;
        public event EventHandler<ToolStripItemRenderEventArgs> RenderButtonBackground;
    }

    public class ToolStripSeparatorRenderEventArgs : ToolStripItemRenderEventArgs { }

    public class ProfessionalColorTable
    {
        static System.Drawing.Color T => System.Drawing.Color.Transparent;
        public virtual System.Drawing.Color ButtonCheckedGradientBegin { get; } = T;
        public virtual System.Drawing.Color ButtonCheckedGradientEnd { get; } = T;
        public virtual System.Drawing.Color ButtonCheckedGradientMiddle { get; } = T;
        public virtual System.Drawing.Color ButtonCheckedHighlight { get; } = T;
        public virtual System.Drawing.Color ButtonCheckedHighlightBorder { get; } = T;
        public virtual System.Drawing.Color ButtonPressedBorder { get; } = T;
        public virtual System.Drawing.Color ButtonPressedGradientBegin { get; } = T;
        public virtual System.Drawing.Color ButtonPressedGradientEnd { get; } = T;
        public virtual System.Drawing.Color ButtonPressedGradientMiddle { get; } = T;
        public virtual System.Drawing.Color ButtonSelectedBorder { get; } = T;
        public virtual System.Drawing.Color ButtonSelectedGradientBegin { get; } = T;
        public virtual System.Drawing.Color ButtonSelectedGradientEnd { get; } = T;
        public virtual System.Drawing.Color ButtonSelectedGradientMiddle { get; } = T;
        public virtual System.Drawing.Color CheckBackground { get; } = T;
        public virtual System.Drawing.Color CheckPressedBackground { get; } = T;
        public virtual System.Drawing.Color CheckSelectedBackground { get; } = T;
        public virtual System.Drawing.Color ImageMarginGradientBegin { get; } = T;
        public virtual System.Drawing.Color ImageMarginGradientEnd { get; } = T;
        public virtual System.Drawing.Color ImageMarginGradientMiddle { get; } = T;
        public virtual System.Drawing.Color ImageMarginRevealedGradientBegin { get; } = T;
        public virtual System.Drawing.Color ImageMarginRevealedGradientEnd { get; } = T;
        public virtual System.Drawing.Color ImageMarginRevealedGradientMiddle { get; } = T;
        public virtual System.Drawing.Color MenuBorder { get; } = T;
        public virtual System.Drawing.Color MenuItemBorder { get; } = T;
        public virtual System.Drawing.Color MenuItemPressedGradientBegin { get; } = T;
        public virtual System.Drawing.Color MenuItemPressedGradientEnd { get; } = T;
        public virtual System.Drawing.Color MenuItemPressedGradientMiddle { get; } = T;
        public virtual System.Drawing.Color MenuItemSelected { get; } = T;
        public virtual System.Drawing.Color MenuItemSelectedGradientBegin { get; } = T;
        public virtual System.Drawing.Color MenuItemSelectedGradientEnd { get; } = T;
        public virtual System.Drawing.Color MenuStripGradientBegin { get; } = T;
        public virtual System.Drawing.Color MenuStripGradientEnd { get; } = T;
        public virtual System.Drawing.Color OverflowButtonGradientBegin { get; } = T;
        public virtual System.Drawing.Color OverflowButtonGradientEnd { get; } = T;
        public virtual System.Drawing.Color OverflowButtonGradientMiddle { get; } = T;
        public virtual System.Drawing.Color SeparatorDark { get; } = T;
        public virtual System.Drawing.Color SeparatorLight { get; } = T;
        public virtual System.Drawing.Color ToolStripBorder { get; } = T;
        public virtual System.Drawing.Color ToolStripGradientBegin { get; } = T;
        public virtual System.Drawing.Color ToolStripGradientEnd { get; } = T;
        public virtual System.Drawing.Color ToolStripGradientMiddle { get; } = T;
        public virtual System.Drawing.Color ToolStripPanelGradientBegin { get; } = T;
        public virtual System.Drawing.Color ToolStripPanelGradientEnd { get; } = T;
    }

    public class ToolStripProfessionalRenderer : ToolStripRenderer
    {
        ProfessionalColorTable _table = new ProfessionalColorTable();
        public ToolStripProfessionalRenderer() { }
        public ToolStripProfessionalRenderer(ProfessionalColorTable table) { _table = table ?? _table; }
        public ProfessionalColorTable ColorTable => _table;
        public bool RoundedEdges { get; set; } = true;
    }

    public class Screen
    {
        public static Screen PrimaryScreen { get; } = new Screen();
        public System.Drawing.Rectangle Bounds { get; } = new System.Drawing.Rectangle(0, 0, 1920, 1080);
        public System.Drawing.Rectangle WorkingArea { get; } = new System.Drawing.Rectangle(0, 0, 1920, 1040);
        public static Screen[] AllScreens { get; } = new[] { PrimaryScreen };
        public static Screen FromControl(Control c) => PrimaryScreen;
        public static Screen FromPoint(System.Drawing.Point p) => PrimaryScreen;
    }

    public static class ControlPaint
    {
        public static void DrawBorder(System.Drawing.Graphics g, System.Drawing.Rectangle bounds, System.Drawing.Color c, ButtonBorderStyle style) { }
        public static System.Drawing.Color Light(System.Drawing.Color baseColor) => baseColor;
        public static System.Drawing.Color Light(System.Drawing.Color baseColor, float percOfLightLight) => baseColor;
        public static System.Drawing.Color Dark(System.Drawing.Color baseColor) => baseColor;
        public static System.Drawing.Color Dark(System.Drawing.Color baseColor, float percOfDarkDark) => baseColor;
        public static void DrawBorder(System.Drawing.Graphics g, System.Drawing.Rectangle bounds, System.Drawing.Color leftColor, int leftWidth, ButtonBorderStyle leftStyle, System.Drawing.Color topColor, int topWidth, ButtonBorderStyle topStyle, System.Drawing.Color rightColor, int rightWidth, ButtonBorderStyle rightStyle, System.Drawing.Color bottomColor, int bottomWidth, ButtonBorderStyle bottomStyle) { }
        public static void DrawFocusRectangle(System.Drawing.Graphics g, System.Drawing.Rectangle r) { }
    }
    public enum ButtonBorderStyle { None, Dotted, Dashed, Solid, Inset, Outset }

    public class ColumnStyle
    {
        public SizeType SizeType { get; set; }
        public float Width { get; set; }
        public ColumnStyle() { }
        public ColumnStyle(SizeType sizeType) { SizeType = sizeType; }
        public ColumnStyle(SizeType sizeType, float width) { SizeType = sizeType; Width = width; }
    }
    public class RowStyle
    {
        public SizeType SizeType { get; set; }
        public float Height { get; set; }
        public RowStyle() { }
        public RowStyle(SizeType sizeType) { SizeType = sizeType; }
        public RowStyle(SizeType sizeType, float height) { SizeType = sizeType; Height = height; }
    }

    public class WebBrowserNavigatingEventArgs : System.ComponentModel.CancelEventArgs
    {
        public Uri Url { get; } = new Uri("about:blank");
        public string TargetFrameName { get; } = "";
    }
    public class WebBrowserNavigatedEventArgs : EventArgs
    {
        public Uri Url { get; } = new Uri("about:blank");
    }
    public delegate void WebBrowserNavigatingEventHandler(object sender, WebBrowserNavigatingEventArgs e);
    public delegate void WebBrowserNavigatedEventHandler(object sender, WebBrowserNavigatedEventArgs e);

    public class WebBrowser : Control
    {
        public bool IsWebBrowserContextMenuEnabled { get; set; } = true;
        public bool AllowWebBrowserDrop { get; set; } = true;
        public bool WebBrowserShortcutsEnabled { get; set; } = true;
        public bool AllowNavigation { get; set; } = true;
        public Uri Url { get; set; }
        public string DocumentText { get; set; } = "";
        public string StatusText { get; } = "";
        public bool IsBusy { get; } = false;
        public bool IsOffline { get; set; }
        public bool ScriptErrorsSuppressed { get; set; }
        public void Navigate(string urlString) { }
        public void Navigate(Uri url) { }
        public void Navigate(string urlString, bool newWindow) { }
        public void GoBack() { }
        public void GoForward() { }
        public void Stop() { }
        public void Refresh(WebBrowserRefreshOption opt) { }
        public event WebBrowserNavigatingEventHandler Navigating;
        public event WebBrowserNavigatedEventHandler Navigated;
        public event EventHandler DocumentCompleted;
    }
    public enum WebBrowserRefreshOption { Normal, IfExpired, Continue, Completely }
}

// ── System.Windows.Forms.VisualStyles stub ───────────────────────────────────

namespace System.Windows.Forms.VisualStyles
{
    public class VisualStyleElement
    {
        public static class Tab
        {
            public static class TabItem
            {
                public static VisualStyleElement Normal { get; } = new VisualStyleElement();
            }
            public static class Pane
            {
                public static VisualStyleElement Normal { get; } = new VisualStyleElement();
            }
        }
        public static class Globals
        {
            public static VisualStyleElement FlatHorizontal { get; } = new VisualStyleElement();
        }
    }
    public class VisualStyleRenderer
    {
        public VisualStyleRenderer(VisualStyleElement element) { }
        public static bool IsSupported => false;
        public static bool IsElementDefined(VisualStyleElement element) => false;
        public void DrawBackground(System.Drawing.Graphics g, System.Drawing.Rectangle r) { }
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
        protected object GetService(Type serviceType) => null;
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

// ── Ambertation.Windows.Forms docking stubs ────────────────────────────────
// DockPanel, DockContainer, DockManager live here (not in AdvancedForms/NetDocks stubs)
// so that projects referencing SimPE.Helper get these types without a circular dependency.

namespace Ambertation.Windows.Forms
{
    /// <summary>Stub dock panel — will be replaced with Avalonia docking.</summary>
    public class DockPanel : System.Windows.Forms.Panel
    {
        public string Caption { get; set; } = "";
        public string CaptionText { get; set; } = "";
        public string ButtonText { get; set; } = "";
        public string TabText { get; set; } = "";
        public System.Drawing.Image Image { get; set; }
        public System.Drawing.Image TabImage { get; set; }
        public System.Drawing.Size FloatingSize { get; set; }
        public bool CanClose { get; set; } = true;
        public bool CanHide { get; set; } = true;
        public bool CanResize { get; set; } = true;
        public bool CanUndock { get; set; } = true;
        public bool AllowClose { get; set; } = true;
        public bool AllowCollapse { get; set; } = true;
        public bool AllowDockBottom { get; set; } = true;
        public bool AllowDockCenter { get; set; } = true;
        public bool AllowDockLeft { get; set; } = true;
        public bool AllowDockRight { get; set; } = true;
        public bool AllowDockTop { get; set; } = true;
        public bool AllowFloat { get; set; } = true;
        public bool IsDocked { get; set; }
        public bool IsFloating { get; set; }
        public bool DragBorder { get; set; }
        public bool ShowCloseButton { get; set; } = true;
        public bool ShowCollapseButton { get; set; } = true;
        public int UndockByCaptionThreshold { get; set; }
        public System.Windows.Forms.Panel DockContainer { get; set; }
        public DockManager DockManager { get; set; }
        public DockManager Manager { get; set; }
        protected virtual void OnControlRemoved(System.Windows.Forms.ControlEventArgs e) { }

        public class DockPanelClosingEvent : EventArgs
        {
            public bool Cancel { get; set; }
        }
        public delegate void ClosingHandler(object sender, DockPanelClosingEvent e);
        public event ClosingHandler Closing;
        public event System.EventHandler OpenedStateChanged;

        public bool IsOpen { get; set; } = true;
        public bool Collapsed { get; set; }
        public void EnsureVisible() { }
        public void Expand() { Collapsed = false; }
        public void Expand(bool animate) { Collapsed = false; }
        public void Collapse() { Collapsed = true; }
        public void Collapse(bool animate) { Collapsed = true; }
        public void Open() { IsOpen = true; }
        public void OpenFloating() { }
        public new void Close() { IsOpen = false; }
        public new void Show() { IsOpen = true; }
        public System.Drawing.Size AutoScrollMinSize { get; set; }
    }

    /// <summary>Stub dock container — will be replaced with Avalonia docking.</summary>
    public class DockContainer : System.Windows.Forms.Panel
    {
        public DockPanel DockPanel { get; set; }
        public string TabText { get; set; } = "";
        public System.Drawing.Image TabImage { get; set; }
        public bool DragBorder { get; set; }
        public bool NoCleanup { get; set; }
        public DockManager Manager { get; set; }
    }

    /// <summary>Stub dock manager — will be replaced with Avalonia docking.</summary>
    public class DockManager : System.Windows.Forms.Panel
    {
        readonly List<DockPanel> _panels = new();
        public void DockPanel(DockPanel panel, System.Windows.Forms.DockStyle style) { _panels.Add(panel); }
        public System.Collections.Generic.List<DockPanel> GetPanels() => _panels;
        public object Renderer { get; set; }
        public System.Drawing.Size DefaultSize { get; set; }
        public bool DragBorder { get; set; }
        public bool NoCleanup { get; set; }
        public DockManager Manager { get; set; }
        public string TabText { get; set; } = "";
        public System.Drawing.Image TabImage { get; set; }
    }

    /// <summary>Stub color table for Whidbey/VS2005 toolbar theme.</summary>
    public class WhidbeyColorTable
    {
        public System.Drawing.Color MainGradientBegin { get; set; }
        public System.Drawing.Color MainGradientEnd { get; set; }
        public System.Drawing.Color DockBorderColor { get; set; }
        public System.Drawing.Color DockButtonBackgroundTop { get; set; }
        public System.Drawing.Color DockButtonBackgroundBottom { get; set; }
        public System.Drawing.Color DockButtonHighlightBackgroundTop { get; set; }
        public System.Drawing.Color DockButtonHighlightBackgroundBottom { get; set; }
        public System.Drawing.Color DockPanelBackground { get; set; }
        public System.Drawing.Color DockPanelCaption { get; set; }
    }

    public class WhidbeyRenderer : IDisposable { public void Dispose() { } }
    public class GlossyRenderer : IDisposable { public void Dispose() { } }
    public class ClassicRenderer : IDisposable { public void Dispose() { } }

    public class ManagerSingelton
    {
        static readonly ManagerSingelton _global = new ManagerSingelton();
        public static ManagerSingelton Global => _global;
        public DockPanel GetPanelWithName(string name) => null;
        public void MoveTo(DockPanel panel, System.Windows.Forms.Control container) { }
    }

    public class Serializer
    {
        static readonly Serializer _global = new Serializer();
        public static Serializer Global => _global;
        public void Register(object control) { }
        public void Save() { }
        public void Load() { }
    }

    public static class ToolStripRuntimeDesigner
    {
        public static void Add(object container) { }
        public static void LineUpToolBars(object container) { }
    }

}

// ── Ambertation.Windows.Forms.Debug stubs ────────────────────────────────────
// NetDocks excludes all .cs files; this provides the StructureTreeView stub.

namespace Ambertation.Windows.Forms.Debug
{
    public class StructureTreeView
    {
        public static void Execute(object manager) { }
    }
}

