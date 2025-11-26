/***************************************************************************
 *   Copyright (C) 2005 by Ambertation                                     *
 *   quaxi@ambertation.de                                                  *
 *                                                                         *
 *   This program is free software; you can redistribute it and/or modify  *
 *   it under the terms of the GNU General Public License as published by  *
 *   the Free Software Foundation; either version 2 of the License, or     *
 *   (at your option) any later version.                                   *
 *                                                                         *
 *   This program is distributed in the hope that it will be useful,       *
 *   but WITHOUT ANY WARRANTY; without even the implied warranty of        *
 *   MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the         *
 *   GNU General Public License for more details.                          *
 *                                                                         *
 *   You should have received a copy of the GNU General Public License     *
 *   along with this program; if not, write to the                         *
 *   Free Software Foundation, Inc.,                                       *
 *   59 Temple Place - Suite 330, Boston, MA  02111-1307, USA.             *
 ***************************************************************************/
using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.IO;

namespace SimPe.Plugin.Tool
{
	/// <summary>
	/// Summary description for Report.
	/// </summary>
	internal class Report : System.Windows.Forms.Form
	{
        private booby.gradientpanel xpGradientPanel1;
        private System.Windows.Forms.RichTextBox rtb;
		private System.Windows.Forms.SaveFileDialog sfd;
        private Button button1;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public Report()
		{
			//
			// Required designer variable.
			//
            InitializeComponent();

            booby.ThemeManager tm = booby.ThemeManager.Global.CreateChild();
            tm.AddControl(this.xpGradientPanel1);
            if (booby.ThemeManager.ThemedForms)
            {
                tm.AddControl(this.button1);
                this.rtb.BackColor = booby.ThemeManager.Global.ThemeColorLight;
            }

			//
			// TODO: Fügen Sie den Konstruktorcode nach dem Aufruf von InitializeComponent hinzu
			//
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
			if (csv != null)
			{
				csv.Close();
				csv.Dispose();
				csv = null;
			}
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Report));
            this.xpGradientPanel1 = new booby.gradientpanel();
            this.button1 = new System.Windows.Forms.Button();
            this.rtb = new System.Windows.Forms.RichTextBox();
            this.sfd = new System.Windows.Forms.SaveFileDialog();
            this.xpGradientPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // xpGradientPanel1
            // 
            this.xpGradientPanel1.BackColor = System.Drawing.Color.Transparent;
            this.xpGradientPanel1.Controls.Add(this.button1);
            this.xpGradientPanel1.Controls.Add(this.rtb);
            resources.ApplyResources(this.xpGradientPanel1, "xpGradientPanel1");
            this.xpGradientPanel1.Name = "xpGradientPanel1";
            // 
            // button1
            // 
            resources.ApplyResources(this.button1, "button1");
            this.button1.Name = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // rtb
            // 
            resources.ApplyResources(this.rtb, "rtb");
            this.rtb.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.rtb.Name = "rtb";
            this.rtb.ReadOnly = true;
            this.rtb.ShowSelectionMargin = true;
            // 
            // sfd
            // 
            resources.ApplyResources(this.sfd, "sfd");
            // 
            // Report
            // 
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this.xpGradientPanel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Name = "Report";
            this.xpGradientPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

		}
		#endregion

		System.IO.StreamWriter csv;
		public void Execute(System.IO.StreamWriter csv)
		{
			csv.Flush();
			csv.BaseStream.Seek(0, SeekOrigin.Begin);
			StreamReader sr = new StreamReader(csv.BaseStream);
			sr.BaseStream.Seek(0, SeekOrigin.Begin);			

			this.csv = csv;
			this.rtb.Text = sr.ReadToEnd();
			this.ShowDialog();
		}


        private void button1_Click(object sender, EventArgs e)
        {
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                csv.BaseStream.Seek(0, SeekOrigin.Begin);
                StreamReader sr = new StreamReader(csv.BaseStream);
                sr.BaseStream.Seek(0, SeekOrigin.Begin);

                System.IO.StreamWriter sw = System.IO.File.CreateText(sfd.FileName);
                try
                {
                    sw.Write(sr.ReadToEnd());
                }
                finally
                {
                    sw.Close();
                    sw.Dispose();
                    sw = null;
                    sr = null;
                }
            }

        }
	}
}
