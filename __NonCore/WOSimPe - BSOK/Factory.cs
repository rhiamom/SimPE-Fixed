using SimPe.Interfaces;

namespace SimPe.Wizards
{
    public class BsokFactory : SimPe.Interfaces.Plugin.AbstractWrapperFactory, SimPe.Interfaces.Plugin.IToolFactory
    {
        public override IWrapper[] KnownWrappers
        {
            get { return new IWrapper[0]; }
        }

        public IToolPlugin[] KnownTools
        {
            get { return new IToolPlugin[] { new BsokTool() }; }
        }
    }
}
