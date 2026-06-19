using System;
using System.Drawing;
using SimPe.Interfaces;
using SimPe.Interfaces.Files;
using SimPe.Interfaces.Plugin;
using System.Threading;
using SimPe.Plugin.UI;

namespace SimPe.Plugin
{
	/// <summary>
	/// Summary description for GeneticCategorizerTool.
	/// </summary>
	public class GeneticCategorizerTool :  AbstractTool, IToolPlugin, ITool
	{

		#region IToolPlugin Members

		public override string ToString()
		{
            // Theo distributes this tool standalone on MTS; surface his name in
            // the menu so users know which Color Binning Tool this is.
            return "Object Creation\\Theo's Color Binning Tool";
		}

		#endregion

		public override bool Visible
		{
			get { return true; }
		}

		#region ITool Members

		public bool IsEnabled(IPackedFileDescriptor pfd, IPackageFile package)
		{
			return true;
		}

		public IToolResult ShowDialog(ref IPackedFileDescriptor pfd, ref IPackageFile package)
		{
			// EnsureFileTable();
			MainForm form = new MainForm();
			form.Show();

			return new ToolResult(false, false);
		}


		#endregion

		public override System.Drawing.Image Icon
		{
			get
			{
                return System.Drawing.Image.FromStream(this.GetType().Assembly.GetManifestResourceStream("SimPe.Plugin.ColorBinningTool.png"));
			}
		}
    }
}
