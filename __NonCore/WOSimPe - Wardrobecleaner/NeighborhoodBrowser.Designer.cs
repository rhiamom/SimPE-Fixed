using System.Windows.Forms;
namespace SimPe.Plugin.UI
{
	partial class NeighborhoodBrowser
	{
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary> 
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Component Designer generated code

		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.components = new System.ComponentModel.Container();
            this.ilist = new System.Windows.Forms.ImageList(this.components);
            this.lv = new System.Windows.Forms.ListView();
            this.SuspendLayout();
            // 
            // ilist
            // 
            this.ilist.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit;
            this.ilist.ImageSize = new System.Drawing.Size(133, 100);
            this.ilist.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // lv
            // 
            this.lv.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lv.HideSelection = false;
            this.lv.LargeImageList = this.ilist;
            this.lv.Location = new System.Drawing.Point(0, 0);
            this.lv.MultiSelect = false;
            this.lv.Name = "lv";
            this.lv.Size = new System.Drawing.Size(572, 429);
            this.lv.TabIndex = 1;
            this.lv.UseCompatibleStateImageBehavior = false;
            this.lv.Click += new System.EventHandler(this.NgbOpen);
            // 
            // NeighborhoodBrowser
            // 
            this.Controls.Add(this.lv);
            this.Size = new System.Drawing.Size(575, 432);
            this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.ImageList ilist;
		private System.Windows.Forms.ListView lv;
	}
}
