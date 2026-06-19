#define WRAPPER_PLUGIN

using System;
using SimPe.Interfaces;

namespace SimPe.Plugin
{
	/// <summary>
	/// Lists all Plugins (=FileType Wrappers) available in this Package
	/// </summary>
	/// <remarks>
	/// GetWrappers() has to return a list of all Plugins provided by this Library. 
	/// If a Plugin isn't returned, SimPe won't recognize it!
	/// </remarks>
    public class FamiuWrapperFactory 
        :
        SimPe.Interfaces.Plugin.AbstractWrapperFactory   //This Interface allows your Plugin to offer packed File Wrappers
	{
		#region AbstractWrapperFactory Member
		/// <summary>
		/// Returns a List of all available Plugins in this Package
		/// </summary>
		/// <returns>A List of all provided Plugins (=FileType Wrappers)</returns>
		public override SimPe.Interfaces.IWrapper[] KnownWrappers
		{
            get
            {
                // Chris Hatch's original code gated this on Helper.SimPeVersionLong
                // and Executable.Classic, with a comment "requires updated GDF" —
                // GDF.dll being his NSFW theme library. We don't ship GDF and the
                // wrapper has no actual dependency on it (the booby refs were all
                // in the UI layer and were de-boobified). Return the wrapper
                // unconditionally so FAMH resources register and appear in the
                // resource tree.
                return new IWrapper[] { new FamiuPackedFileWrapper() };
            }
		}

		#endregion
    }
}
