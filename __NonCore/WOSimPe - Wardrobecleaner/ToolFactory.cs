using System;
using SimPe.Interfaces;
using SimPe.Interfaces.Plugin;

namespace SimPe.Plugin
{
    public class WardrobeWranglerTool : AbstractTool, ITool
    {
        public bool IsEnabled(SimPe.Interfaces.Files.IPackedFileDescriptor pfd, SimPe.Interfaces.Files.IPackageFile package)
            => true;

        public IToolResult ShowDialog(ref SimPe.Interfaces.Files.IPackedFileDescriptor pfd, ref SimPe.Interfaces.Files.IPackageFile package)
        {
            new WizardHostForm(new Step1()).ShowDialog(null).GetAwaiter().GetResult();
            return new ToolResult(false, false);
        }

        public override string ToString() => "Wizards\\Wardrobe Wrangler...";
    }

    public class WardrobeToolFactory : AbstractWrapperFactory, IToolFactory
    {
        public override IWrapper[] KnownWrappers => new IWrapper[0];

        public IToolPlugin[] KnownTools => new IToolPlugin[] { new WardrobeWranglerTool() };
    }
}
