/***************************************************************************
 *   Copyright (C) 2005 by Ambertation                                     *
 *   quaxi@ambertation.de                                                  *
 *                                                                         *
 *   Copyright (C) 2025 by GramzeSweatShop                                 *
 *   rhiamom@mac.com                                                       *
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

namespace SimPe.PackedFiles.Wrapper.SCOR
{
    partial class ScoreItemBusinessRewards
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.lb = new System.Windows.Forms.ListBox();
            this.llAdd = new System.Windows.Forms.LinkLabel();
            this.llRemove = new System.Windows.Forms.LinkLabel();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.llRemove);
            this.panel1.Controls.Add(this.llAdd);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 300);
            this.panel1.Name = "panel1";
            this.panel1.Padding = new System.Windows.Forms.Padding(4);
            this.panel1.Size = new System.Drawing.Size(394, 48);
            this.panel1.TabIndex = 0;
            // 
            // lb
            // 
            this.lb.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lb.FormattingEnabled = true;
            this.lb.IntegralHeight = false;
            this.lb.Location = new System.Drawing.Point(0, 0);
            this.lb.Name = "lb";
            this.lb.Size = new System.Drawing.Size(394, 300);
            this.lb.TabIndex = 1;
            this.lb.SelectedIndexChanged += new System.EventHandler(this.lb_SelectedIndexChanged);
            // 
            // llAdd
            // 
            this.llAdd.AutoSize = true;
            this.llAdd.Enabled = false;
            this.llAdd.Location = new System.Drawing.Point(60, 4);
            this.llAdd.Name = "llAdd";
            this.llAdd.Size = new System.Drawing.Size(26, 13);
            this.llAdd.TabIndex = 0;
            this.llAdd.TabStop = true;
            this.llAdd.Text = "Add";
            // 
            // llRemove
            // 
            this.llRemove.AutoSize = true;
            this.llRemove.Enabled = false;
            this.llRemove.Location = new System.Drawing.Point(7, 4);
            this.llRemove.Name = "llRemove";
            this.llRemove.Size = new System.Drawing.Size(47, 13);
            this.llRemove.TabIndex = 1;
            this.llRemove.TabStop = true;
            this.llRemove.Text = "Remove";
            this.llRemove.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.llRemove_LinkClicked);
            // 
            // ScoreItemBusinessRewards
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.lb);
            this.Controls.Add(this.panel1);
            this.Name = "ScoreItemBusinessRewards";
            this.Size = new System.Drawing.Size(394, 348);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.ListBox lb;
        private System.Windows.Forms.LinkLabel llRemove;
        private System.Windows.Forms.LinkLabel llAdd;
    }
}
