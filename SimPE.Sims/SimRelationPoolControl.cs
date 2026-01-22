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

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace SimPe.PackedFiles.Wrapper
{
    [System.ComponentModel.DefaultEvent("SelectedSimChanged")]
    public partial class SimRelationPoolControl : SimPoolControl
    {
        static System.Drawing.Image RelatedImage;
        public SimRelationPoolControl()
        {
            if (RelatedImage == null)
            {
                var asm = typeof(SimPe.Helper).Assembly;
                RelatedImage = Image.FromStream(asm.GetManifestResourceStream("SimPe.IconXmlResources.related.png"));
            }
            InitializeComponent();
            showrel = true;
            cbRelation.Checked = showrel;

            shownorel = false;
            cbNoRelation.Checked = shownorel;
            intern = false;

           
            this.panel1.SendToBack();
            cbhousehold.SendToBack();
            this.RightClickSelect = true;            
        }

        public void UpdateIcon()
        {
            Image img = UpdateIcon(this.SelectedSim);
            if (img != null && gp.SelectedItems.Count > 0)
            {
                gp.SelectedItems[0].ImageList.Images[gp.SelectedItems[0].ImageIndex] = img;
                gp.Refresh();
            }
        }

        protected Image UpdateIcon(SimPe.PackedFiles.Wrapper.ExtSDesc sdsc)
        {
            if (sim != null && sdsc != null)
            {
                Image img = SimListView.BuildSimPreviewImage(sdsc, GetBackgroundColor(sdsc));
                bool hr = sim.HasRelationWith(sdsc);
                if (hr) MakeRelationIcon(img);

                return img;
            }
            return null;
        }

        protected override void OnAddSimToPool(SimPoolControl.AddSimToPoolEventArgs e)
        {
            if (sim != null)
            {
                bool hr = sim.HasRelationWith(e.SimDescription);
                bool res = false;
                if (hr && showrel) res = true;
                else if (!hr && shownorel) res = true;

                if (hr)
                {
                    MakeRelationIcon(e.Image);
                    e.GroupIndex = 0;
                }
                else e.GroupIndex = 1;

                if (e.SimDescription.FileDescriptor.Instance == sim.FileDescriptor.Instance) res = false;
                if (!res) e.Cancel = true;
            }
            base.OnAddSimToPool(e);
        }

        private static void MakeRelationIcon(Image img)
        {
            Graphics g = Graphics.FromImage(img);
            g.DrawImageUnscaled(RelatedImage, 0, 0, 16, 16);            
        }

        bool intern;

        bool showrel, shownorel;
        public bool ShowRelatedSims
        {
            get { return showrel; }
            set {
                if (value != showrel)
                {
                    showrel = value;
                    this.UpdateSimList();
                    intern = true;
                    this.cbRelation.Checked = value;
                    intern = false;
                }
            }
        }

        public bool ShowNotRelatedSims
        {
            get { return shownorel; }
            set
            {
                if (value != shownorel)
                {
                    shownorel = value;
                    this.UpdateSimList();
                    intern = true;
                    this.cbNoRelation.Checked = value;
                    intern = false;
                }
            }
        }

        [Browsable(false)]
        public bool FilteredBySim
        {
            get
            {
                return !ShowNotRelatedSims || !ShowRelatedSims;
            }
        }

        ExtSDesc sim;
        [Browsable(false)]
        public ExtSDesc Sim
        {
            get { return sim; }
            set
            {
            	// It seems that once set, "sim" somehow tracks "value"
                if (sim != value)
                    sim = value;
                // So we do this anyway...
                if (FilteredBySim && this.Package != null) this.UpdateSimList();
            }
        }

        private void cbNoRelation_CheckedChanged(object sender, EventArgs e)
        {
            if (intern) return;
            ShowNotRelatedSims = cbNoRelation.Checked;
        }

        private void cbRelation_CheckedChanged(object sender, EventArgs e)
        {
            if (intern) return;
            ShowRelatedSims = cbRelation.Checked;
        }
    }
}
