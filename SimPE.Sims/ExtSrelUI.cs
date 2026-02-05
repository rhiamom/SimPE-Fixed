/***************************************************************************
 *   Copyright (C) 2005 by Ambertation                                     *
 *   quaxi@ambertation.de                                                  *
 *                                                                         *
 *   Copyright (C) 2025 by GramzeSweatshop                                 *
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

using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;
using SimPe.Interfaces.Plugin;
using SimPe.Interfaces;
using SimPe.PackedFiles.Wrapper.Supporting;
using SimPe.Data;
using Ambertation.Windows.Forms;

namespace SimPe.PackedFiles.UserInterface
{
    /// <summary>
    /// Summary description for ExtSrelUI.
    /// </summary>
    public class ExtSrel : SimPe.Windows.Forms.WrapperBaseControl, IPackedFileUI
    {
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lbsims;
        private SimPe.PackedFiles.UserInterface.CommonSrel sc;
        private System.Windows.Forms.PictureBox pb;
        
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.Container components = null;

        public ExtSrel()
        {
            Text = SimPe.Localization.GetString("Sim Relation Editor");

            InitializeComponent();

            if (Helper.WindowsRegistry.UseBigIcons)
                this.lbsims.Font = new System.Drawing.Font("Tahoma", 12);

            // NEW: create and add the CommonSrel control
            sc = new CommonSrel();
            sc.Dock = DockStyle.Fill;
            // optional: hook change event if you want auto-sync
            // sc.ChangedContent += new EventHandler(this.ExtSrel_Commited);

            this.Controls.Add(sc);
            this.Controls.SetChildIndex(sc, 0);
        }


        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ExtSrel));
            this.label1 = new System.Windows.Forms.Label();
            this.lbsims = new System.Windows.Forms.Label();
            this.pb = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pb)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoEllipsis = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // lbsims
            // 
            resources.ApplyResources(this.lbsims, "lbsims");
            this.lbsims.BackColor = System.Drawing.Color.Transparent;
            this.lbsims.Name = "lbsims";
            // 
            // pb
            // 
            this.pb.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.pb, "pb");
            this.pb.Name = "pb";
            this.pb.TabStop = false;
            // 
            // ExtSrel
            // 
            this.Controls.Add(this.pb);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lbsims);
            resources.ApplyResources(this, "$this");
            this.Name = "ExtSrel";
            this.Commited += new System.EventHandler(this.ExtSrel_Commited);
            this.Controls.SetChildIndex(this.lbsims, 0);
            this.Controls.SetChildIndex(this.label1, 0);
            this.Controls.SetChildIndex(this.pb, 0);
            ((System.ComponentModel.ISupportInitialize)(this.pb)).EndInit();
            this.ResumeLayout(false);

        }
        #endregion

        public SimPe.PackedFiles.Wrapper.ExtSrel Srel
        {
            get { return (SimPe.PackedFiles.Wrapper.ExtSrel)Wrapper; }
        }

        public override void RefreshGUI()
        {
            base.RefreshGUI();

            // If there is no relation loaded (e.g. target cleared), blank the UI.
            if (this.Srel == null)
            {
                sc.Srel = null;
                this.lbsims.Text = "";
                this.pb.Image = null;
                return;
            }

            sc.Srel = this.Srel;

            this.lbsims.Text = sc.SourceSimName + " " + SimPe.Localization.GetString("towards") + " " + sc.TargetSimName;
            this.pb.Image = Ambertation.Drawing.GraphicRoutines.ScaleImage(sc.Image, pb.Size, true);
        }


        private void ExtSrel_Commited(object sender, System.EventArgs e)
        {
            Srel.SynchronizeUserData();
        }
    }
}
