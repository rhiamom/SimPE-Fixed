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
using Avalonia;
using Avalonia.Controls;
using Avalonia.Layout;
using SimPe.Interfaces.Files;
using SimPe.Interfaces;
using SimPe.PackedFiles.Wrapper;
using SimPe.PackedFiles;
using SimPe.Scenegraph.Compat;

namespace SimPe.Plugin.UI
{
    public class HouseholdBrowser : Avalonia.Controls.UserControl
    {
        IPackageFile package;
        internal bool suppressEvents;

        private SimPe.Scenegraph.Compat.ListView lvFam   = new SimPe.Scenegraph.Compat.ListView();
        private SimPe.Scenegraph.Compat.ListView lvItems  = new SimPe.Scenegraph.Compat.ListView();
        private Label label1   = new Label { Content = "Households" };
        private Label label2   = new Label { Content = "Items" };
        private CheckBox cbCheckAll = new CheckBox { Content = "Check All" };
        private SimPe.Scenegraph.Compat.ColumnHeader columnHeader1 = new SimPe.Scenegraph.Compat.ColumnHeader { Text = "Family Name", Width = 190 };
        private SimPe.Scenegraph.Compat.ColumnHeader columnHeader2 = new SimPe.Scenegraph.Compat.ColumnHeader { Text = "Instance" };

        public IPackageFile NeighborhoodPackage
        {
            get { return package; }
            set
            {
                package = value;
                PopulateHouseholdList();
            }
        }

        public event EventHandler HouseholdSelectionChanged;

        public HouseholdBrowser()
        {
            lvFam.CheckBoxes = true;
            lvFam.Columns.Add(columnHeader1);
            lvFam.Columns.Add(columnHeader2);
            lvFam.FullRowSelect = true;
            lvFam.MultiSelect = false;

            lvFam.SelectionChanged += lvFam_SelectedIndexChanged;

            cbCheckAll.IsCheckedChanged += cbCheckAll_CheckedChanged;

            BuildLayout();
        }

        void BuildLayout()
        {
            // Left pane: label + lvFam
            var leftPanel = new DockPanel();
            DockPanel.SetDock(label1, Dock.Top);
            leftPanel.Children.Add(label1);
            leftPanel.Children.Add(lvFam);

            // Right pane: label + lvItems
            var rightPanel = new DockPanel();
            DockPanel.SetDock(label2, Dock.Top);
            rightPanel.Children.Add(label2);
            rightPanel.Children.Add(lvItems);

            // Split: left 38%, right 62%
            var grid = new Grid();
            grid.ColumnDefinitions.Add(new ColumnDefinition(0.38, GridUnitType.Star));
            grid.ColumnDefinitions.Add(new ColumnDefinition(4,    GridUnitType.Pixel));
            grid.ColumnDefinitions.Add(new ColumnDefinition(0.62, GridUnitType.Star));
            grid.RowDefinitions.Add(new RowDefinition(1, GridUnitType.Star));
            grid.RowDefinitions.Add(new RowDefinition(GridLength.Auto));

            Grid.SetColumn(leftPanel,  0); Grid.SetRow(leftPanel,  0);
            Grid.SetColumn(rightPanel, 2); Grid.SetRow(rightPanel, 0);
            Grid.SetColumn(cbCheckAll, 0); Grid.SetRow(cbCheckAll, 1);
            Grid.SetColumnSpan(cbCheckAll, 3);

            grid.Children.Add(leftPanel);
            grid.Children.Add(rightPanel);
            grid.Children.Add(cbCheckAll);

            Content = grid;
        }

        protected virtual void OnHouseholdSelectionChanged(EventArgs e)
        {
            if (this.HouseholdSelectionChanged != null)
                this.HouseholdSelectionChanged(this, e);
        }

        void PopulateHouseholdList()
        {
            this.lvFam.Items.Clear();

            if (this.package != null)
            {
                IPackedFileDescriptor[] pfds = this.package.FindFiles(0x46414D49u); // FAMI
                foreach (IPackedFileDescriptor pfd in pfds)
                {
                    Fami fam = new Fami(WizardController.Instance.ProviderRegistry.SimNameProvider);
                    fam.ProcessData(pfd, package, false);

                    SimPe.Scenegraph.Compat.ListViewItem li = new SimPe.Scenegraph.Compat.ListViewItem();
                    li.ImageIndex = 0;
                    li.Text = fam.Name;
                    li.SubItems.Add(String.Format("0x{0:X4}", fam.FileDescriptor.Instance));
                    li.Tag = fam;

                    this.lvFam.Items.Add(li);
                }
            }
        }

        void PopulateItemList()
        {
            this.lvItems.BeginUpdate();
            this.lvItems.Items.Clear();

            if (this.lvFam.SelectedItems.Count > 0)
            {
                List<uint> famInstances = this.GetSelectedFamilyInstances();
                List<RefFile> refList = WizardController.Instance.BuildWardrobes(famInstances);
                foreach (RefFile idr in refList)
                {
                    SimPe.Scenegraph.Compat.ListViewItem li = new SimPe.Scenegraph.Compat.ListViewItem();
                    li.ImageIndex = 0;
                    li.Text = String.Format("{0:X8}-{1:X8}", idr.FileDescriptor.SubType, idr.FileDescriptor.Instance);
                    // Strikeout for MarkForDelete not supported in compat ListView
                    this.lvItems.Items.Add(li);
                }
            }

            this.lvItems.EndUpdate();
        }

        List<uint> GetSelectedFamilyInstances()
        {
            List<uint> famInstances = new List<uint>();
            foreach (SimPe.Scenegraph.Compat.ListViewItem li in lvFam.SelectedItems)
            {
                Fami fam = li.Tag as Fami;
                if (fam != null)
                    famInstances.Add(fam.FileDescriptor.Instance);
            }
            return famInstances;
        }

        public List<uint> GetCheckedFamilyInstances()
        {
            List<uint> famInstances = new List<uint>();
            foreach (SimPe.Scenegraph.Compat.ListViewItem li in lvFam.CheckedItems)
            {
                Fami fam = li.Tag as Fami;
                famInstances.Add(fam.FileDescriptor.Instance);
            }
            return famInstances;
        }

        private void lvFam_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!suppressEvents)
            {
                this.OnHouseholdSelectionChanged(e);
                this.PopulateItemList();
            }
        }

        private void cbCheckAll_CheckedChanged(object sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            bool check = this.cbCheckAll.IsChecked == true;

            this.suppressEvents = true;
            foreach (SimPe.Scenegraph.Compat.ListViewItem li in this.lvFam.Items)
                li.Checked = check;
            this.suppressEvents = false;

            this.OnHouseholdSelectionChanged(EventArgs.Empty);
        }
    }
}
