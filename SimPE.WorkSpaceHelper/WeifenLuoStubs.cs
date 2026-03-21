// WeifenLuoStubs.cs — Stub for WeifenLuo.WinFormsUI.Docking.
// The DockPanelSuite package was removed; these stubs preserve compilation.
// Will be replaced with Avalonia docking in a future pass.

using System;

namespace WeifenLuo.WinFormsUI.Docking
{
    public enum DocumentStyle { DockingMdi, DockingWindow, DockingSdi, SystemMdi }
    public interface IDockContent { }
    public class ThemeBase { }
    public class VS2015LightTheme : ThemeBase { }
    public class VS2015BlueTheme : ThemeBase { }
    public class VS2015DarkTheme : ThemeBase { }

    /// <summary>Stub dock panel — will be replaced with Avalonia docking.</summary>
    public class DockPanel
    {
        public IDockContent ActiveDocument => null;
        public DocumentStyle DocumentStyle { get; set; }
        public ThemeBase Theme { get; set; }
        public IDockContent[] DocumentsToArray() => Array.Empty<IDockContent>();
    }

    /// <summary>Stub dockable content window — will be replaced with Avalonia docking.</summary>
    public class DockContent : IDockContent
    {
        public DockPanel DockPanel => null;
        public DockState DockState { get; set; } = DockState.Document;
        public DockAreas DockAreas { get; set; } = DockAreas.Document;
        public DockPaneHandler DockHandler { get; } = new DockPaneHandler();
        public bool CloseButton { get; set; } = true;
        public string Text { get; set; } = "";
        public object Tag { get; set; }
        public System.Drawing.Rectangle ClientRectangle => new System.Drawing.Rectangle(0, 0, 800, 600);
        public event EventHandler FormClosing;
        public void Show(DockPanel dp) { }
        public void Show(DockPanel dp, DockState state) { }
        public void Activate() { }
        protected void OnFormClosing(EventArgs e) => FormClosing?.Invoke(this, e);
    }

    public class DockPaneHandler
    {
        public bool CloseButton { get; set; } = true;
        public DockState DockState { get; set; } = DockState.Document;
        public DockPane Pane { get; }
        public DockPanel DockPanel { get; }
        public DockContent Form { get; }
    }

    public class DockPane { }

    public enum DockState
    {
        Unknown, Float, DockTopAutoHide, DockLeftAutoHide,
        DockBottomAutoHide, DockRightAutoHide,
        Document, DockTop, DockLeft, DockBottom, DockRight, Hidden
    }

    [Flags]
    public enum DockAreas
    {
        None = 0, Float = 1, DockLeft = 2, DockRight = 4,
        DockTop = 8, DockBottom = 16, Document = 32
    }
}
