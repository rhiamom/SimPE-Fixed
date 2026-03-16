using System;
using SimPe.Interfaces;
using SimPe.Plugin;

namespace SimPe.Wizards
{
    public class BsokTool : Interfaces.AbstractTool, Interfaces.ITool
    {
        public bool IsEnabled(SimPe.Interfaces.Files.IPackedFileDescriptor pfd, SimPe.Interfaces.Files.IPackageFile package)
        {
            return true;
        }

        public Interfaces.Plugin.IToolResult ShowDialog(ref SimPe.Interfaces.Files.IPackedFileDescriptor pfd, ref SimPe.Interfaces.Files.IPackageFile package)
        {
            using (var form = new BsokWizardForm())
                form.ShowDialog();
            return new ToolResult(false, false);
        }

        public override string ToString()
        {
            return "Object Creation\\BSOK Wizard...";
        }

        public override System.Windows.Forms.Shortcut Shortcut
        {
            get { return System.Windows.Forms.Shortcut.None; }
        }
    }
}
