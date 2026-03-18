// NetDocks stub — WinForms docking replaced with Avalonia docking in a future pass.
// All original source is preserved in the Ambertation.Windows.Forms/ subfolder.
// Note: cannot reference System.Windows.Forms here (SimPE.Helper is not a dependency).

using System;

namespace Ambertation.Windows.Forms
{
    // ── Minimal API stub so callers compile without NetDocks source ──────────

    public static class APIHelp
    {
        public const uint WM_APP = 0x8000;

        public static IntPtr SendMessage(IntPtr hWnd, uint msg, long wParam, long lParam)
            => IntPtr.Zero; // no-op on non-Windows
    }

    /// <summary>
    /// Stub base class for transparent/layered WinForms windows.
    /// Inheritors (SplashForm, HelpForm) will be replaced with Avalonia windows.
    /// </summary>
    public class TransparentForm : IDisposable
    {
        public virtual void Dispose() { }
        public virtual void Close() { }
        public virtual void Show() { }
        public void SuspendLayout() { }
        public void ResumeLayout(bool performLayout = false) { }

        // Virtual overrides for subclasses live in separate projects that
        // reference SimPE.Helper (which has the full WinForms shims).
    }

    /// <summary>Stub base class for layered forms.</summary>
    public class LayeredForm : TransparentForm { }

    // Empty marker namespace so 'using Ambertation.Windows.Forms;' compiles.
}
