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
using Avalonia.Controls;

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
                RelatedImage = Helper.LoadImage(asm.GetManifestResourceStream("SimPe.IconXmlResources.related.png"));
            }
            InitializeComponent();
            showrel = true;
            cbRelation.IsChecked = showrel;

            shownorel = false;
            cbNoRelation.IsChecked = shownorel;
            intern = false;

            // SendToBack() is WinForms-only; no-op in Avalonia
            this.RightClickSelect = true;            
        }

        public void UpdateIcon()
        {
            System.Drawing.Image img = UpdateIcon(this.SelectedSim);
            if (img != null && gp.SelectedItems.Count > 0)
            {
                // ImageList not available in Avalonia; image update is a no-op here
                gp.Refresh();
            }
        }

        protected System.Drawing.Image UpdateIcon(SimPe.PackedFiles.Wrapper.ExtSDesc sdsc)
        {
            if (sim != null && sdsc != null)
            {
                System.Drawing.Image img = SimListView.BuildSimPreviewImage(sdsc, GetBackgroundColor(sdsc));
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

        private static void MakeRelationIcon(System.Drawing.Image img)
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
                    this.cbRelation.IsChecked = value;
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
                    this.cbNoRelation.IsChecked = value;
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
            ShowNotRelatedSims = cbNoRelation.IsChecked == true;
        }

        private void cbRelation_CheckedChanged(object sender, EventArgs e)
        {
            if (intern) return;
            ShowRelatedSims = cbRelation.IsChecked == true;
        }
    }
}
