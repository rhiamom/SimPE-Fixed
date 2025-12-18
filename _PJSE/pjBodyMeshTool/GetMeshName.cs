/***************************************************************************
 *   Copyright (C) 2005 by Peter L Jones                                   *
 *   pljones@users.sf.net                                                  *
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
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace pj
{
    public class GetMeshName : Form
    {
        private Label label1;
        private TextBox tbMeshName;
        private Label label2;
        private Button btnOK;
        private Button btnBrowse;
        private Button btnCancel;
        private Label label3;
        private CheckBox cbusecres;
        private Panel gradientpanel1;

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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GetMeshName));
            this.label1 = new System.Windows.Forms.Label();
            this.tbMeshName = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnBrowse = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.cbusecres = new System.Windows.Forms.CheckBox();
            this.gradientpanel1 = new System.Windows.Forms.Panel();
            this.gradientpanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // tbMeshName
            // 
            resources.ApplyResources(this.tbMeshName, "tbMeshName");
            this.tbMeshName.Name = "tbMeshName";
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // btnOK
            // 
            resources.ApplyResources(this.btnOK, "btnOK");
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.Name = "btnOK";
            this.btnOK.UseVisualStyleBackColor = true;
            // 
            // btnBrowse
            // 
            resources.ApplyResources(this.btnBrowse, "btnBrowse");
            this.btnBrowse.DialogResult = System.Windows.Forms.DialogResult.Retry;
            this.btnBrowse.Name = "btnBrowse";
            this.btnBrowse.UseVisualStyleBackColor = true;
            // 
            // btnCancel
            // 
            resources.ApplyResources(this.btnCancel, "btnCancel");
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // cbusecres
            // 
            resources.ApplyResources(this.cbusecres, "cbusecres");
            this.cbusecres.Name = "cbusecres";
            this.cbusecres.UseVisualStyleBackColor = true;
            this.cbusecres.CheckedChanged += new System.EventHandler(this.cbusecres_CheckedChanged);
            // 
            // gradientpanel1
            // 
            this.gradientpanel1.BackColor = System.Drawing.Color.Transparent;
            this.gradientpanel1.Controls.Add(this.cbusecres);
            this.gradientpanel1.Controls.Add(this.label3);
            this.gradientpanel1.Controls.Add(this.label2);
            this.gradientpanel1.Controls.Add(this.btnOK);
            this.gradientpanel1.Controls.Add(this.btnBrowse);
            this.gradientpanel1.Controls.Add(this.btnCancel);
            this.gradientpanel1.Controls.Add(this.tbMeshName);
            this.gradientpanel1.Controls.Add(this.label1);
            resources.ApplyResources(this.gradientpanel1, "gradientpanel1");
            
            this.gradientpanel1.Name = "gradientpanel1";
            
            // 
            // GetMeshName
            // 
            this.AcceptButton = this.btnOK;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ControlBox = false;
            this.Controls.Add(this.gradientpanel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "GetMeshName";
            this.ShowInTaskbar = false;
            this.gradientpanel1.ResumeLayout(false);
            this.gradientpanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        public GetMeshName()
        {
            InitializeComponent();

            if (SimPe.Helper.WindowsRegistry.UseBigIcons)
                this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.25F);

            this.cbusecres.Checked = Settings.BodyMeshExtractUseCres;
        }

        public String MeshName
        {
            get
            {
                return tbMeshName.Text;
            }
        }

        private void cbusecres_CheckedChanged(object sender, EventArgs e)
        {
            Settings.BodyMeshExtractUseCres = this.cbusecres.Checked;
        }
    }
}
