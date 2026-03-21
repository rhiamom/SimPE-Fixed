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
using System.Drawing;
using System.Collections;
using SimPe.PackedFiles.Wrapper;
using Avalonia.Controls;

namespace SimPe.PackedFiles.UserInterface
{
    /// <summary>
    /// Avalonia port of FamilyTiesForm.
    /// The graph display (FamilyTieGraph / GraphPanel) is Windows-only;
    /// the UI here retains the data-management logic and exposes pnfamt
    /// as a plain Panel so ExtFamilyTies.GUIHandle can compile.
    /// </summary>
    public class FamilyTiesForm : Avalonia.Controls.UserControl
    {
        // Panel returned by ExtFamilyTiesUI as the plugin's GUIHandle
        internal Panel pnfamt = new Panel();
        private Panel panel2 = new Panel();
        private Panel panel1 = new Panel();
        private Panel panel3 = new Panel();
        private Panel GradientPanel = new Panel();
        internal TextBlock label12 = new TextBlock();
        internal TextBlock label1 = new TextBlock();
        private TextBlock label2 = new TextBlock();
        private TextBlock lbname = new TextBlock();
        private TextBlock label3 = new TextBlock();
        private TextBlock labelnid = new TextBlock();
        private TextBlock labelidd = new TextBlock();
        internal CheckBox cbLock = new CheckBox { Content = "Lock" };
        private CheckBox cbkeep = new CheckBox { Content = "Keep", IsChecked = true };
        private Button llrem = new Button { Content = "Remove" };
        private TextBox xpLine1 = new TextBox { IsReadOnly = true };
        // cbrel: EnumComboBox → plain ComboBox (enum values populated manually)
        private ComboBox cbrel = new ComboBox();
        private MenuItem miAddTie = new MenuItem();
        private MenuItem miOpenSdesc = new MenuItem();
        internal SimPe.PackedFiles.Wrapper.SimPoolControl pool = new SimPe.PackedFiles.Wrapper.SimPoolControl();
        internal SimPe.PackedFiles.Wrapper.FamilyTieGraph ties = new SimPe.PackedFiles.Wrapper.FamilyTieGraph();
        private SimPe.Windows.Forms.WrapperBaseControl panel4 = new SimPe.Windows.Forms.WrapperBaseControl();

        internal Wrapper.ExtFamilyTies wrapper;

        SimPe.PackedFiles.Wrapper.SDesc lastsdsc, currentsdsc;
        System.Drawing.Image thumb;

        public FamilyTiesForm()
        {
            // Populate cbrel with FamilyTieTypes enum values
            foreach (Data.MetaData.FamilyTieTypes v in Enum.GetValues(typeof(Data.MetaData.FamilyTieTypes)))
                cbrel.Items.Add(new Data.LocalizedFamilyTieTypes(v));

            // Wire events
            ties.SelectedSimChanged += new SimPe.PackedFiles.Wrapper.SimPoolControl.SelectedSimHandler(ties_SelectedSimChanged);
            ties.DoubleClickSim += new SimPe.PackedFiles.Wrapper.SimPoolControl.SelectedSimHandler(ties_DoubleClickSim);
            pool.ClickOverSim += new SimPe.PackedFiles.Wrapper.SimPoolControl.SelectedSimHandler(pool_ClickOverSim);
            pool.SelectedSimChanged += new SimPe.PackedFiles.Wrapper.SimPoolControl.SelectedSimHandler(pool_SelectedSimChanged);
            cbrel.SelectionChanged += new EventHandler<SelectionChangedEventArgs>(cbrel_SelectedIndexChanged);
            llrem.Click += new EventHandler<Avalonia.Interactivity.RoutedEventArgs>(llrem_LinkClicked);
            panel4.Commited += new System.EventHandler(button1_Click);
        }

        public void Dispose() { }

        internal void pool_SelectedSimChanged(object sender, System.Drawing.Image thumb, SimPe.PackedFiles.Wrapper.SDesc sdesc)
        {
            if (cbLock.IsChecked == true) return;

            thumb = null;
            lastsdsc = null;
            currentsdsc = sdesc;
            ties.UpdateGraph(sdesc, wrapper);
        }

        private void pool_ClickOverSim(object sender, System.Drawing.Image thumb, SimPe.PackedFiles.Wrapper.SDesc sdesc)
        {
            lastsdsc = sdesc;
            this.thumb = thumb;
        }

        private void Activate_miAddTie(object sender, System.EventArgs e)
        {
            SimPe.PackedFiles.Wrapper.Supporting.FamilyTieSim fts = wrapper.CreateTie(currentsdsc);
            SimPe.PackedFiles.Wrapper.Supporting.FamilyTieItem fti = fts.CreateTie(lastsdsc, Data.MetaData.FamilyTieTypes.MySiblingIs);

            ties.AddTieToGraph(lastsdsc, 0, 0, fti.Type);

            if (this.cbkeep.IsChecked == true)
            {
                fts = wrapper.CreateTie(lastsdsc);
                fti = fts.CreateTie(currentsdsc, Data.MetaData.FamilyTieTypes.MySiblingIs);
            }
            wrapper.Changed = true;
        }

        private void ties_SelectedSimChanged(object sender, System.Drawing.Image thumb, SimPe.PackedFiles.Wrapper.SDesc sdesc)
        {
            if (sdesc != null)
            {
                cbrel.Tag = null;
                this.lbname.Text = sdesc.SimName + " " + sdesc.SimFamilyName;
                this.labelidd.Text = "0x" + Helper.HexString(sdesc.Instance);
                cbrel.IsEnabled = (sdesc != currentsdsc);
                if (cbrel.IsEnabled)
                {
                    SimPe.PackedFiles.Wrapper.Supporting.FamilyTieSim fts = wrapper.FindTies(currentsdsc);
                    SimPe.PackedFiles.Wrapper.Supporting.FamilyTieItem fti = fts.FindTie(sdesc);
                    // select matching item
                    for (int i = 0; i < cbrel.ItemCount; i++)
                    {
                        if (cbrel.Items[i] is Data.LocalizedFamilyTieTypes lf && (Data.MetaData.FamilyTieTypes)lf == fti.Type)
                        {
                            cbrel.SelectedIndex = i;
                            break;
                        }
                    }
                    cbrel.Tag = fti;
                }
            }
            else
            {
                cbrel.IsEnabled = (ties.SelectedElement != null);
                if (!cbrel.IsEnabled)
                {
                    lbname.Text = "";
                    labelidd.Text = "";
                    cbrel.Tag = null;
                }
            }

            llrem.IsEnabled = cbrel.IsEnabled;
        }

        private void ties_DoubleClickSim(object sender, System.Drawing.Image thumb, SimPe.PackedFiles.Wrapper.SDesc sdesc)
        {
            if (sdesc != null && sdesc != currentsdsc)
            {
                pool.SelectedElement = sdesc;
            }
        }

        private void cbrel_SelectedIndexChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbrel.Tag != null)
            {
                SimPe.PackedFiles.Wrapper.Supporting.FamilyTieItem fti = (SimPe.PackedFiles.Wrapper.Supporting.FamilyTieItem)cbrel.Tag;
                Ambertation.Windows.Forms.Graph.ImagePanel ip = (Ambertation.Windows.Forms.Graph.ImagePanel)ties.SelectedElement;

                if (cbrel.SelectedItem is Data.LocalizedFamilyTieTypes lf)
                {
                    fti.Type = (Data.MetaData.FamilyTieTypes)lf;
                }
                wrapper.Changed = true;

                Ambertation.Windows.Forms.Graph.LinkGraphic lg = ties.MainSimElement != null ? ties.MainSimElement.GetChildLink(ip) : null;
                if (lg != null && cbrel.SelectedItem != null) lg.Text = cbrel.SelectedItem.ToString();

                if (this.cbkeep.IsChecked == true)
                {
                    SimPe.PackedFiles.Wrapper.Supporting.FamilyTieSim fts = wrapper.CreateTie(fti.SimDescription);
                    fts.CreateTie(currentsdsc, FamilyTieGraph.GetAntiTie(currentsdsc, fti.Type));
                }
            }
        }

        private void button1_Click(object sender, System.EventArgs e)
        {
            wrapper.SynchronizeUserData();
        }

        private void llrem_LinkClicked(object sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            if (cbrel.Tag != null)
            {
                SimPe.PackedFiles.Wrapper.Supporting.FamilyTieSim fts = wrapper.FindTies(currentsdsc);
                SimPe.PackedFiles.Wrapper.Supporting.FamilyTieItem fti = (SimPe.PackedFiles.Wrapper.Supporting.FamilyTieItem)cbrel.Tag;

                if (fts.RemoveTie(fti))
                {
                    Ambertation.Windows.Forms.Graph.ImagePanel ip = (Ambertation.Windows.Forms.Graph.ImagePanel)ties.SelectedElement;
                    if (ip != null)
                    {
                        ip.Parent = null;
                        ip.Dispose();
                    }
                    wrapper.Changed = true;
                }
            }
        }

        private void Activate_miOpenSDesc(object sender, System.EventArgs e)
        {
            if (lastsdsc != null)
                SimPe.RemoteControl.OpenPackedFile(lastsdsc.FileDescriptor, lastsdsc.Package);
        }
    }
}
