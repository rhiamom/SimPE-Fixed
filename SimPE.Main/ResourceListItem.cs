using SimPe.Interfaces.Files;

namespace SimPe
{
    /// <summary>
    /// View model for one row in the Resource List DataGrid.
    /// </summary>
    public class ResourceListItem
    {
        public IPackedFileDescriptor Descriptor { get; }

        public string Name     => Descriptor.Filename ?? string.Empty;
        public string Type     => Descriptor.TypeName?.shortname ?? $"0x{Descriptor.Type:X8}";
        public string Group    => $"0x{Descriptor.Group:X8}";
        public string InstanceHi => $"0x{Descriptor.SubType:X8}";
        public string Instance => $"0x{Descriptor.Instance:X8}";
        public string Offset   => $"0x{Descriptor.Offset:X}";
        public string Size     => Descriptor.Size.ToString();

        public ResourceListItem(IPackedFileDescriptor pfd)
        {
            Descriptor = pfd;
        }
    }
}
