using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;
using Ambertation.Windows.Forms;

namespace SimPe.Plugin.Tool.Dockable
{
	/// <summary>
	/// Summary description for DebugDock.
	/// </summary>
	public class DebugDock : Ambertation.Windows.Forms.DockPanel, SimPe.Interfaces.IDockableTool
	{
        bool dun = false;
        private System.Windows.Forms.Panel xpGradientPanel1;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label lbMem;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.ListBox lbft;
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public DebugDock()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();
            ThemeManager.Global.AddControl(this.xpGradientPanel1);
		}

		/// <summary> 
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Component Designer generated code
		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DebugDock));
            this.xpGradientPanel1 = new System.Windows.Forms.Panel();
            this.lbft = new System.Windows.Forms.ListBox();
            this.label2 = new System.Windows.Forms.Label();
            this.lbMem = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.xpGradientPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // xpGradientPanel1
            // 
            this.xpGradientPanel1.BackColor = System.Drawing.Color.Transparent;
            this.xpGradientPanel1.Controls.Add(this.lbft);
            this.xpGradientPanel1.Controls.Add(this.label2);
            this.xpGradientPanel1.Controls.Add(this.lbMem);
            this.xpGradientPanel1.Controls.Add(this.label1);
            this.xpGradientPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.xpGradientPanel1.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xpGradientPanel1.Location = new System.Drawing.Point(0, 0);
            this.xpGradientPanel1.Name = "xpGradientPanel1";
            this.xpGradientPanel1.Size = new System.Drawing.Size(844, 356);
            this.xpGradientPanel1.TabIndex = 0;
            // 
            // lbft
            // 
            this.lbft.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lbft.Location = new System.Drawing.Point(16, 72);
            this.lbft.Name = "lbft";
            this.lbft.Size = new System.Drawing.Size(818, 264);
            this.lbft.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.Blue;
            this.label2.Location = new System.Drawing.Point(8, 56);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(128, 23);
            this.label2.TabIndex = 2;
            this.label2.Text = "FileTable Content:";
            this.label2.Click += new System.EventHandler(this.label2_Click);
            // 
            // lbMem
            // 
            this.lbMem.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lbMem.BackColor = System.Drawing.Color.Transparent;
            this.lbMem.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbMem.Location = new System.Drawing.Point(16, 24);
            this.lbMem.Name = "lbMem";
            this.lbMem.Size = new System.Drawing.Size(818, 23);
            this.lbMem.TabIndex = 1;
            this.lbMem.Text = "0";
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.Blue;
            this.label1.Location = new System.Drawing.Point(8, 8);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(100, 23);
            this.label1.TabIndex = 0;
            this.label1.Text = "Memory Usage:";
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // DebugDock
            // 
            this.ButtonText = "Debug";
            this.CaptionText = "Debug Dock";
            this.Controls.Add(this.xpGradientPanel1);
            this.FloatingSize = new System.Drawing.Size(856, 382);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Image = ((System.Drawing.Image)(resources.GetObject("$this.Image")));
            this.Name = "DebugDock";
            this.Size = new System.Drawing.Size(844, 356);
            this.TabImage = ((System.Drawing.Image)(resources.GetObject("$this.TabImage")));
            this.TabText = "Debug";
            this.xpGradientPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

		}
		#endregion

        public Ambertation.Windows.Forms.DockPanel GetDockableControl()
		{
			return this;
		}

		public event SimPe.Events.ChangedResourceEvent ShowNewResource;

		public void RefreshDock(object sender, SimPe.Events.ResourceEventArgs es)
		{
			lbMem.Text = GC.GetTotalMemory(false).ToString("N0") + " Byte";					
		}


		#region IToolPlugin Member

		public override string ToString()
		{
			return this.Text;
		}

		#endregion

        private void label1_Click(object sender, System.EventArgs e)
        {
            RefreshDock(null, null);
		}

		private void label2_Click(object sender, System.EventArgs e)
		{
            if (dun) return; // prevent running while running
            this.label2.ForeColor = System.Drawing.SystemColors.WindowText;
            dun = true;
            string savey = "replicated";
            int savnum = 0;
            while (System.IO.File.Exists(System.IO.Path.Combine(PathProvider.SimSavegameFolder, savey + ".txt")))
            {
                savnum++;
                savey += Convert.ToString(savnum);
            }
            System.IO.StreamWriter sw = System.IO.File.CreateText(System.IO.Path.Combine(PathProvider.SimSavegameFolder, savey + ".txt"));
            string objname = System.IO.Path.Combine(PathProvider.Global.Latest.InstallFolder, @"TSData\Res\Objects\objects.package");
            sw.WriteLine(PathProvider.Global.Latest.DisplayName);
			sw.WriteLine(System.IO.Path.GetFileName(objname)+"----------------------------------------");
			SimPe.Interfaces.Files.IPackageFile pkg = SimPe.Packages.File.LoadFromFile(objname);
            Wait.Start(pkg.Index.Length);
            Wait.Message = "Loading " + System.IO.Path.GetFileName(objname);
			FileTable.FileIndex.Load();
			lbft.Items.Clear();
            lbft.Items.Add(PathProvider.Global.Latest.DisplayName + " : " + System.IO.Path.GetFileName(objname));

            foreach (SimPe.Interfaces.Files.IPackedFileDescriptor pfd in pkg.Index)
            {
                SimPe.Interfaces.Scenegraph.IScenegraphFileIndexItem[] items = FileTable.FileIndex.FindFile(pfd, null);
                lbft.Items.Add(pfd.ToString());
                sw.WriteLine(pfd.ToString());
                Wait.Progress++;
            }
				
			lbft.Items.Add(" m: "+pkg.Index.Length.ToString());

			sw.Close();
			sw.Dispose();
			sw = null;
            dun = false;
            Wait.Stop(true);
            this.label2.ForeColor = System.Drawing.Color.Blue;
		}

		#region IToolExt Member

		public System.Windows.Forms.Shortcut Shortcut
		{
			get
			{
				return System.Windows.Forms.Shortcut.None;
			}
		}

		public System.Drawing.Image Icon
		{
			get
			{
				return this.TabImage;
			}
		}	

		public new virtual bool Visible 
		{
			get { return this.IsDocked ||  this.IsFloating; }
		}

		#endregion
	}
}
