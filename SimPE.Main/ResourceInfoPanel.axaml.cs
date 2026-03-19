using Avalonia.Controls;
using SimPe.Interfaces.Files;

namespace SimPe
{
    public partial class ResourceInfoPanel : UserControl
    {
        public ResourceInfoPanel()
        {
            InitializeComponent();
        }

        public void Show(IPackedFileDescriptor pfd, IPackageFile package)
        {
            LblType.Text   = pfd.TypeName?.Name ?? $"0x{pfd.Type:X8}";
            LblGroup.Text  = $"0x{pfd.Group:X8}";
            LblInstHi.Text = $"0x{pfd.SubType:X8}";
            LblInst.Text   = $"0x{pfd.Instance:X8}";
            LblOffset.Text = $"0x{pfd.Offset:X}";
            LblSize.Text   = $"{pfd.Size:N0} bytes";
            LblComp.Text   = pfd.WasCompressed ? "Yes" : "No";
            LblFile.Text   = string.IsNullOrEmpty(pfd.Filename) ? "—" : pfd.Filename;
        }

        public void Clear()
        {
            LblType.Text   = LblGroup.Text  = LblInstHi.Text = LblInst.Text =
            LblOffset.Text = LblSize.Text   = LblComp.Text   = LblFile.Text = string.Empty;
        }
    }
}
