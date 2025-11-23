using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace SimPe.PackedFiles.UserInterface
{
	/// <summary>
	/// Summary description for GroupCacheForm.
	/// </summary>
	internal class GroupCacheForm : System.Windows.Forms.Form
	{
		internal System.Windows.Forms.ListBox lbgroup;
        private booby.panelheader panel4;
		internal booby.gradientpanel GropPanel;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public GroupCacheForm()
		{
            InitializeComponent();

            if (booby.ThemeManager.ThemedForms)
            {
                booby.ThemeManager tm = booby.ThemeManager.Global.CreateChild();
                tm.AddControl(this.GropPanel);
                tm.AddControl(this.lbgroup);
            }
            if (Helper.WindowsRegistry.UseBigIcons) this.lbgroup.Font = new System.Drawing.Font(this.lbgroup.Font.FontFamily, 11F);

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

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.GropPanel = new booby.gradientpanel();
            this.lbgroup = new System.Windows.Forms.ListBox();
            this.panel4 = new booby.panelheader();
            this.GropPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // GropPanel
            // 
            this.GropPanel.BackColor = System.Drawing.Color.Transparent;
            this.GropPanel.Controls.Add(this.lbgroup);
            this.GropPanel.Controls.Add(this.panel4);
            this.GropPanel.EndColour = System.Drawing.SystemColors.Control;
            this.GropPanel.Font = new System.Drawing.Font("Verdana", 8.25F);
            this.GropPanel.Location = new System.Drawing.Point(14, 29);
            this.GropPanel.MiddleColour = System.Drawing.SystemColors.Control;
            this.GropPanel.Name = "GropPanel";
            this.GropPanel.Size = new System.Drawing.Size(264, 208);
            this.GropPanel.StartColour = System.Drawing.SystemColors.Control;
            this.GropPanel.TabIndex = 8;
            // 
            // lbgroup
            // 
            this.lbgroup.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lbgroup.HorizontalScrollbar = true;
            this.lbgroup.IntegralHeight = false;
            this.lbgroup.Location = new System.Drawing.Point(8, 32);
            this.lbgroup.Name = "lbgroup";
            this.lbgroup.Size = new System.Drawing.Size(248, 168);
            this.lbgroup.TabIndex = 3;
            // 
            // panel4
            // 
            this.panel4.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.panel4.HeaderText = "Group Cache Viewer";
            this.panel4.Location = new System.Drawing.Point(0, 0);
            this.panel4.Margin = new System.Windows.Forms.Padding(0);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(264, 24);
            this.panel4.TabIndex = 0;
            // 
            // GroupCacheForm
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(292, 266);
            this.Controls.Add(this.GropPanel);
            this.Name = "GroupCacheForm";
            this.Text = "GroupCacheForm";
            this.GropPanel.ResumeLayout(false);
            this.ResumeLayout(false);

		}
		#endregion
	}
}
