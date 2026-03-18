// SplashStubs.cs — Stub implementations for SplashForm and HelpForm.
// Original WinForms implementations excluded (inherit from NetDocks TransparentForm).
// These stubs preserve the public API for callers; Avalonia replacements come later.

using System;
using System.Windows.Forms;

namespace SimPe.Windows.Forms
{
    // ── SplashForm ───────────────────────────────────────────────────────────

    public class SplashForm : IDisposable
    {
        string _message = "";

        public SplashForm() { }

        public string Message
        {
            get => _message;
            set { _message = value; System.Diagnostics.Trace.WriteLine("Splash: " + value); }
        }

        public event FormClosedEventHandler FormClosed;

        public void StartSplash() { }
        public void StopSplash() { FormClosed?.Invoke(this, new FormClosedEventArgs()); }
        public void Dispose() { }
    }

    // ── HelpForm ─────────────────────────────────────────────────────────────
    // Base class for About dialogs (SimPE.Main.About, ColorBinningTool.ClbAbout).

    public class HelpForm : IDisposable
    {
        public HelpForm() { }
        public virtual void Dispose() { }
        public virtual void Show() { }
        public virtual void Close() { }
    }
}
