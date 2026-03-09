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

namespace SimPe.Windows.Forms
{
    partial class ResourceTreeViewExt
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

        #region Windows Form Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ResourceTreeViewExt));
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.tbInst = new System.Windows.Forms.ToolStripButton();
            this.tbGroup = new System.Windows.Forms.ToolStripButton();
            this.tbType = new System.Windows.Forms.ToolStripButton();
            this.tv = new System.Windows.Forms.TreeView();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.AccessibleDescription = null;
            this.toolStrip1.AccessibleName = null;
            resources.ApplyResources(this.toolStrip1, "toolStrip1");
            this.toolStrip1.BackgroundImage = null;
            this.toolStrip1.Font = null;
            this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tbInst,
            this.tbGroup,
            this.tbType});
            this.toolStrip1.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.Flow;
            this.toolStrip1.Name = "toolStrip1";
            // 
            // tbInst
            // 
            this.tbInst.AccessibleDescription = null;
            this.tbInst.AccessibleName = null;
            this.tbInst.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            resources.ApplyResources(this.tbInst, "tbInst");
            this.tbInst.BackgroundImage = null;
            this.tbInst.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tbInst.Name = "tbInst";
            this.tbInst.Click += new System.EventHandler(this.SelectTreeBuilder);
            // 
            // tbGroup
            // 
            this.tbGroup.AccessibleDescription = null;
            this.tbGroup.AccessibleName = null;
            resources.ApplyResources(this.tbGroup, "tbGroup");
            this.tbGroup.BackgroundImage = null;
            this.tbGroup.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tbGroup.Name = "tbGroup";
            this.tbGroup.Click += new System.EventHandler(this.SelectTreeBuilder);
            // 
            // tbType
            // 
            this.tbType.AccessibleDescription = null;
            this.tbType.AccessibleName = null;
            resources.ApplyResources(this.tbType, "tbType");
            this.tbType.BackgroundImage = null;
            this.tbType.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tbType.Name = "tbType";
            this.tbType.Click += new System.EventHandler(this.SelectTreeBuilder);
            // 
            // tv
            // 
            this.tv.AccessibleDescription = null;
            this.tv.AccessibleName = null;
            resources.ApplyResources(this.tv, "tv");
            this.tv.BackgroundImage = null;
            this.tv.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.tv.Font = null;
            this.tv.HideSelection = false;
            this.tv.Name = "tv";
            this.tv.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.tv_AfterSelect);
            // 
            // ResourceTreeViewExt
            // 
            this.AccessibleDescription = null;
            this.AccessibleName = null;
            resources.ApplyResources(this, "$this");
            this.BackgroundImage = null;
            this.Controls.Add(this.tv);
            this.Controls.Add(this.toolStrip1);
            this.Name = "ResourceTreeViewExt";
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.TreeView tv;
        private System.Windows.Forms.ToolStripButton tbInst;
        private System.Windows.Forms.ToolStripButton tbGroup;
        private System.Windows.Forms.ToolStripButton tbType;
    }
}
