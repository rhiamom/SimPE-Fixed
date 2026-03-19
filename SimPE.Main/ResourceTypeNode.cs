using SimPe.Interfaces.Files;
using System.Collections.Generic;

namespace SimPe
{
    /// <summary>
    /// One node in the Resource Tree — represents all resources of a given type.
    /// </summary>
    public class ResourceTypeNode
    {
        public string DisplayName { get; }
        public List<IPackedFileDescriptor> Descriptors { get; }

        public ResourceTypeNode(SimPe.Data.TypeAlias alias, List<IPackedFileDescriptor> descriptors)
        {
            Descriptors = descriptors;
            string label = (alias != null && alias.Name != null)
                ? $"{alias.Name} ({alias.shortname}) ({descriptors.Count})"
                : $"0x{descriptors[0].Type:X8} ({descriptors.Count})";
            DisplayName = label;
        }
    }
}
