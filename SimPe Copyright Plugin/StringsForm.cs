using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace SimPe.Plugin
{
	/// <summary>
	/// Summary description for StringsForm.
	/// </summary>
	public class StringsForm : System.Windows.Forms.Form
	{
		/// <summary>
		/// Required designer variable.
        /// </summary>
        private System.Windows.Forms.Panel GradientPanel;
		private System.ComponentModel.Container components = null;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button button1;
		internal System.Windows.Forms.TextBox tbMMAT;
		internal System.Windows.Forms.TextBox tbCreator;
		internal System.Windows.Forms.TextBox tbLicense;
		internal System.Windows.Forms.TextBox tbDate;
		internal System.Windows.Forms.TextBox tbVersion;
		ThemeManager tm;
		public StringsForm()
		{
			//
			// Required designer variable.
			//
			InitializeComponent();

            tm = ThemeManager.Global.CreateChild();
            tm.AddControl(this.GradientPanel);

			this.tbDate.Text = DateTime.Now.ToString();
            if (Helper.WindowsRegistry.Username.Trim() != "")
                this.tbCreator.Text = Helper.WindowsRegistry.Username;
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if (tm!=null) 
				{
					tm.Parent = null;
					tm.Clear();
					tm = null;
				}
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
            this.GradientPanel = new System.Windows.Forms.Panel();
            this.button1 = new System.Windows.Forms.Button();
            this.tbVersion = new System.Windows.Forms.TextBox();
            this.tbDate = new System.Windows.Forms.TextBox();
            this.tbLicense = new System.Windows.Forms.TextBox();
            this.tbCreator = new System.Windows.Forms.TextBox();
            this.tbMMAT = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.GradientPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // GradientPanel
            // 
            this.GradientPanel.BackColor = System.Drawing.Color.Transparent;
            this.GradientPanel.Controls.Add(this.button1);
            this.GradientPanel.Controls.Add(this.tbVersion);
            this.GradientPanel.Controls.Add(this.tbDate);
            this.GradientPanel.Controls.Add(this.tbLicense);
            this.GradientPanel.Controls.Add(this.tbCreator);
            this.GradientPanel.Controls.Add(this.tbMMAT);
            this.GradientPanel.Controls.Add(this.label5);
            this.GradientPanel.Controls.Add(this.label4);
            this.GradientPanel.Controls.Add(this.label3);
            this.GradientPanel.Controls.Add(this.label2);
            this.GradientPanel.Controls.Add(this.label1);
            this.GradientPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.GradientPanel.Location = new System.Drawing.Point(0, 0);
            this.GradientPanel.Name = "GradientPanel";
            this.GradientPanel.Size = new System.Drawing.Size(1466, 609);
            this.GradientPanel.TabIndex = 0;
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button1.Location = new System.Drawing.Point(1335, 217);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(120, 33);
            this.button1.TabIndex = 10;
            this.button1.Text = "OK";
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // tbVersion
            // 
            this.tbVersion.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbVersion.Location = new System.Drawing.Point(184, 171);
            this.tbVersion.Name = "tbVersion";
            this.tbVersion.Size = new System.Drawing.Size(1269, 27);
            this.tbVersion.TabIndex = 9;
            this.tbVersion.Text = "CEP Extra";
            // 
            // tbDate
            // 
            this.tbDate.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbDate.Location = new System.Drawing.Point(184, 137);
            this.tbDate.Name = "tbDate";
            this.tbDate.Size = new System.Drawing.Size(1269, 27);
            this.tbDate.TabIndex = 8;
            this.tbDate.Text = "today";
            // 
            // tbLicense
            // 
            this.tbLicense.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbLicense.Location = new System.Drawing.Point(184, 103);
            this.tbLicense.Name = "tbLicense";
            this.tbLicense.Size = new System.Drawing.Size(1269, 27);
            this.tbLicense.TabIndex = 7;
            this.tbLicense.Text = "This File was created as Part of a ColourEnabler Extra Package  If you payed for " +
    "a package that contains this File please report it.";
            // 
            // tbCreator
            // 
            this.tbCreator.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbCreator.Location = new System.Drawing.Point(184, 69);
            this.tbCreator.Name = "tbCreator";
            this.tbCreator.Size = new System.Drawing.Size(1269, 27);
            this.tbCreator.TabIndex = 6;
            this.tbCreator.Text = "Anonymous";
            // 
            // tbMMAT
            // 
            this.tbMMAT.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbMMAT.Location = new System.Drawing.Point(184, 11);
            this.tbMMAT.Name = "tbMMAT";
            this.tbMMAT.Size = new System.Drawing.Size(1269, 27);
            this.tbMMAT.TabIndex = 5;
            this.tbMMAT.Text = "Created for CEP Extra";
            // 
            // label5
            // 
            this.label5.BackColor = System.Drawing.Color.Transparent;
            this.label5.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(16, 171);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(160, 33);
            this.label5.TabIndex = 4;
            this.label5.Text = "Version:";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label4
            // 
            this.label4.BackColor = System.Drawing.Color.Transparent;
            this.label4.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(16, 137);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(160, 33);
            this.label4.TabIndex = 3;
            this.label4.Text = "Release Date:";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label3
            // 
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(16, 103);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(160, 33);
            this.label3.TabIndex = 2;
            this.label3.Text = "License:";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label2
            // 
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(16, 69);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(160, 32);
            this.label2.TabIndex = 1;
            this.label2.Text = "Created by:";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(16, 11);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(160, 33);
            this.label1.TabIndex = 0;
            this.label1.Text = "MMAT Text:";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // StringsForm
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(8, 20);
            this.ClientSize = new System.Drawing.Size(1466, 609);
            this.Controls.Add(this.GradientPanel);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Name = "StringsForm";
            this.Text = "Copyright Text";
            this.GradientPanel.ResumeLayout(false);
            this.GradientPanel.PerformLayout();
            this.ResumeLayout(false);

		}
		#endregion

		private void button1_Click(object sender, System.EventArgs e)
		{
			Close();
		}
	}
}
