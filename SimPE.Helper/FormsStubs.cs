// FormsStubs.cs — Stub implementations for WinForms UI classes that have been
// excluded from compilation.  These preserve the public API so callers compile
// without changes.  Real Avalonia implementations will replace these later.

using System;
using System.Collections.Generic;
using System.Windows.Forms;

// ── LabelledBoolsetControl ──────────────────────────────────────────────────
// Boolset editor control — lives in System.Windows.Forms namespace to match
// original source.  Designer.cs is excluded, so we provide a standalone stub.

namespace System.Windows.Forms
{
    public class LabelledBoolsetControl : UserControl
    {
        System.Boolset boolset = (ushort)0;
        List<string> labels = new();

        public LabelledBoolsetControl() { }

        public bool ButtonsVisible { get; set; } = true;

        public ushort Value
        {
            get => (ushort)boolset;
            set
            {
                ushort old = boolset;
                boolset = value;
                if (old != boolset) ValueChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        public List<string> Labels
        {
            get => labels;
            set { labels = value; }
        }

        public event EventHandler ValueChanged;
        public virtual void OnValueChanged(object sender, EventArgs e) => ValueChanged?.Invoke(sender, e);
    }
}

namespace SimPe
{
    // ── WaitingForm ──────────────────────────────────────────────────────────
    // Used only by WaitingScreen to show a splash overlay during long operations.

    internal class WaitingForm : Form
    {
        System.Drawing.Image _image;
        string _message = "";

        public WaitingForm() { }

        public void SetImage(System.Drawing.Image image) { _image = image; }
        public System.Drawing.Image Image => _image;

        public void SetMessage(string message)
        {
            _message = message;
            System.Diagnostics.Trace.WriteLine("WaitingScreen: " + message);
        }
        public string Message => _message;

        public void StartSplash() { }
        public void StopSplash() { }
    }

    // ── ExceptionForm ────────────────────────────────────────────────────────

    public class ExceptionForm : Form
    {
        public ExceptionForm() { }

        public static void Execute(Exception ex)
        {
            Execute(ex?.Message, ex);
        }

        public static void Execute(string message, Exception ex)
        {
            if (Helper.NoErrors) return;
            if (message == null || message.Trim() == "") message = ex?.Message ?? "";
            Console.Error.WriteLine("[SimPE Error] " + message);
            if (ex != null) Console.Error.WriteLine(ex.ToString());
            System.Diagnostics.Trace.TraceError(message + "\n" + ex);
        }
    }

    // ── pjseMsgBox ───────────────────────────────────────────────────────────
    // Cross-platform stub: writes to console; Avalonia dialog comes later.

    public class pjseMsgBox : Form
    {
        public pjseMsgBox() { }

        public static DialogResult Show(string text)
            => Show(text, "");

        public static DialogResult Show(IWin32Window owner, string text)
            => Show(text, "");

        public static DialogResult Show(string text, string caption)
        {
            Console.WriteLine($"[SimPE] {caption}: {text}");
            return DialogResult.OK;
        }

        public static DialogResult Show(IWin32Window owner, string text, string caption)
            => Show(text, caption);

        public static DialogResult Show(string text, string caption, Boolset buttonsVisible)
            => Show(text, caption);

        public static DialogResult Show(IWin32Window owner, string text, string caption, Boolset buttonsVisible)
            => Show(text, caption);

        public static DialogResult Show(string text, string caption, Boolset buttonsVisible, Boolset buttonsOverride, string[] buttons)
            => Show(text, caption);

        public static DialogResult Show(IWin32Window owner, string text, string caption, Boolset buttonsVisible, Boolset buttonsOverride, string[] buttons)
            => Show(text, caption);

        public static DialogResult Show(string text, string caption, Boolset buttonsVisible, Boolset buttonsOverride, string[] buttons, DialogResult[] resultSet)
            => Show(text, caption);

        public static DialogResult Show(IWin32Window owner, string text, string caption, Boolset buttonsVisible, Boolset buttonsOverride, string[] buttons, DialogResult[] resultSet)
            => Show(text, caption);

        public static DialogResult Show(Avalonia.Controls.Window owner, string text)
            => Show(text, "");

        public static DialogResult Show(Avalonia.Controls.Window owner, string text, string caption)
            => Show(text, caption);

        public static DialogResult Show(Avalonia.Controls.Window owner, string text, string caption, Boolset buttonsVisible)
            => Show(text, caption);

        public static DialogResult Show(Avalonia.Controls.Window owner, string text, string caption, Boolset buttonsVisible, Boolset buttonsOverride, string[] buttons)
            => Show(text, caption);

        public static DialogResult Show(Avalonia.Controls.Window owner, string text, string caption, Boolset buttonsVisible, Boolset buttonsOverride, string[] buttons, DialogResult[] resultSet)
            => Show(text, caption);
    }

    // ── SelectSimFolder ──────────────────────────────────────────────────────
    // Folder picker dialog — stub returns path unchanged until Avalonia dialog is wired up.

    class SelectSimFolder : Form
    {
        SelectSimFolder() { }

        public static string ShowDialog(string path) => path;
    }

    // ── SelectSimFolderUITypeEditor ──────────────────────────────────────────

    public class SelectSimFolderUITypeEditor : System.Drawing.Design.UITypeEditor
    {
        public override System.Drawing.Design.UITypeEditorEditStyle GetEditStyle(
            System.ComponentModel.ITypeDescriptorContext context)
            => System.Drawing.Design.UITypeEditorEditStyle.Modal;

        public override object EditValue(
            System.ComponentModel.ITypeDescriptorContext context,
            IServiceProvider provider,
            object value)
            => value;
    }
}
