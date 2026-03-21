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

// Ported from WinForms Form to Avalonia UserControl.
// wrapperPanel removed — NmapForm itself is now the UserControl returned via GUIHandle.
// LinkLabels → Buttons.  ListBox, TextBox, GroupBox → Avalonia equivalents.
// SaveFileDialog → StorageProvider.SaveFilePickerAsync (async void handler).
// WinForms drag-drop → Avalonia DragDrop attached events.
// tbgroup.Tag re-entrancy guard → bool _updating field.

using System;
using System.Windows.Forms;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Layout;

namespace SimPe.Plugin
{
    public class NmapForm : Avalonia.Controls.UserControl
    {
        // ── Fields accessed by NmapUI ──────────────────────────────────────
        internal ListBox lblist;
        internal TextBox tbname, tbgroup, tbinstance, tbfindname;
        internal Button lladd, lldelete, llcommit, linkLabel1;
        internal Button btref, btnCommit;
        internal Nmap wrapper;

        // Re-entrancy guard (replaces tbgroup.Tag trick)
        private bool _updating;

        public void Dispose() { }

        public NmapForm()
        {
            lblist    = new ListBox();
            tbname    = new TextBox();
            tbgroup   = new TextBox();
            tbinstance= new TextBox();
            tbfindname= new TextBox();

            lladd     = new Button { Content = "Add" };
            lldelete  = new Button { Content = "Delete" };
            llcommit  = new Button { Content = "Commit Item" };
            linkLabel1= new Button { Content = "Create Text File" };
            btref     = new Button { Content = "Package…" };
            btnCommit = new Button { Content = "Commit" };

            // Wire events
            lblist.SelectionChanged        += SelectFile;
            tbname.TextChanged             += AutoChange;
            tbgroup.TextChanged            += AutoChange;
            tbinstance.TextChanged         += AutoChange;
            tbfindname.TextChanged         += tbFindName_TextChanged;
            lladd.Click                    += AddFile;
            lldelete.Click                 += DeleteFile;
            llcommit.Click                 += ChangeFile;
            linkLabel1.Click               += CreateTextFile;
            btref.Click                    += ShowPackageSelector;
            btnCommit.Click                += CommitAll;

            // Drag-drop onto list
            DragDrop.SetAllowDrop(lblist, true);
            lblist.AddHandler(DragDrop.DropEvent,     PackageItemDrop);
            lblist.AddHandler(DragDrop.DragOverEvent, PackageItemDragOver);

            ThemeManager tm = ThemeManager.Global.CreateChild();
            tm.AddControl(this);
            tm.AddControl(lblist);

            Content = BuildLayout();
        }

        private Control BuildLayout()
        {
            // ── Search row ──────────────────────────────────────────────────
            var searchRow = new StackPanel
            {
                Orientation = Orientation.Horizontal,
                Margin      = new Thickness(0, 0, 0, 4)
            };
            searchRow.Children.Add(new TextBlock
            {
                Text = "Find:",
                VerticalAlignment = VerticalAlignment.Center,
                Margin = new Thickness(0, 0, 4, 0)
            });
            searchRow.Children.Add(tbfindname);
            searchRow.Children.Add(linkLabel1);

            // ── Edit fields ─────────────────────────────────────────────────
            var editGrid = new Grid { Margin = new Thickness(0, 4, 0, 4) };
            editGrid.ColumnDefinitions.Add(new ColumnDefinition(GridLength.Auto));
            editGrid.ColumnDefinitions.Add(new ColumnDefinition(GridLength.Star));
            void AddRow(string label, TextBox tb, int row)
            {
                editGrid.RowDefinitions.Add(new RowDefinition(GridLength.Auto));
                var lbl = new TextBlock { Text = label, VerticalAlignment = VerticalAlignment.Center, Margin = new Thickness(0, 2, 6, 2) };
                Grid.SetRow(lbl, row); Grid.SetColumn(lbl, 0);
                Grid.SetRow(tb, row);  Grid.SetColumn(tb, 1);
                editGrid.Children.Add(lbl);
                editGrid.Children.Add(tb);
            }
            AddRow("Name:",     tbname,     0);
            AddRow("Group:",    tbgroup,    1);
            AddRow("Instance:", tbinstance, 2);

            // ── Action buttons row ──────────────────────────────────────────
            var actionRow = new StackPanel
            {
                Orientation = Orientation.Horizontal,
                Margin      = new Thickness(0, 2, 0, 4)
            };
            actionRow.Children.Add(lladd);
            actionRow.Children.Add(lldelete);
            actionRow.Children.Add(llcommit);
            actionRow.Children.Add(btref);

            // ── Edit group border ───────────────────────────────────────────
            var editBorder = new Border
            {
                BorderBrush     = Avalonia.Media.Brushes.Gray,
                BorderThickness = new Thickness(1),
                Padding         = new Thickness(4),
                Margin          = new Thickness(0, 0, 0, 4),
                Child = new StackPanel
                {
                    Children = { editGrid, actionRow }
                }
            };

            // ── Bottom commit row ───────────────────────────────────────────
            var bottomRow = new StackPanel
            {
                Orientation = Orientation.Horizontal,
                HorizontalAlignment = HorizontalAlignment.Right,
                Margin = new Thickness(0, 4, 0, 0)
            };
            bottomRow.Children.Add(btnCommit);

            // ── Main layout: search + list + edit + commit ──────────────────
            var mainGrid = new Grid();
            mainGrid.RowDefinitions.Add(new RowDefinition(GridLength.Auto));   // search
            mainGrid.RowDefinitions.Add(new RowDefinition(GridLength.Star));   // list
            mainGrid.RowDefinitions.Add(new RowDefinition(GridLength.Auto));   // edit
            mainGrid.RowDefinitions.Add(new RowDefinition(GridLength.Auto));   // commit

            Grid.SetRow(searchRow,  0);
            Grid.SetRow(lblist,     1);
            Grid.SetRow(editBorder, 2);
            Grid.SetRow(bottomRow,  3);

            mainGrid.Children.Add(searchRow);
            mainGrid.Children.Add(lblist);
            mainGrid.Children.Add(editBorder);
            mainGrid.Children.Add(bottomRow);

            return mainGrid;
        }

        // ── Event handlers ─────────────────────────────────────────────────

        private void SelectFile(object sender, SelectionChangedEventArgs e)
        {
            llcommit.IsEnabled = false;
            lldelete.IsEnabled = false;
            if (lblist.SelectedIndex < 0) return;
            llcommit.IsEnabled = true;
            lldelete.IsEnabled = true;

            if (_updating) return;
            try
            {
                _updating = true;
                var pfd = (Interfaces.Files.IPackedFileDescriptor)lblist.Items[lblist.SelectedIndex];
                tbgroup.Text    = "0x" + Helper.HexString(pfd.Group);
                tbinstance.Text = "0x" + Helper.HexString(pfd.Instance);
                tbname.Text     = pfd.Filename;
            }
            catch (Exception ex)
            {
                Helper.ExceptionMessage(Localization.Manager.GetString("errconvert"), ex);
            }
            finally
            {
                _updating = false;
            }
        }

        private void ChangeFile(object sender, RoutedEventArgs e)
        {
            try
            {
                Packages.PackedFileDescriptor pfd = null;
                if (lblist.SelectedIndex >= 0)
                    pfd = (Packages.PackedFileDescriptor)lblist.Items[lblist.SelectedIndex];
                else
                    pfd = new NmapItem(wrapper);

                pfd.Group    = Convert.ToUInt32(tbgroup.Text, 16);
                pfd.Instance = Convert.ToUInt32(tbinstance.Text, 16);
                pfd.Filename = tbname.Text;

                if (lblist.SelectedIndex >= 0)
                {
                    var items = (Avalonia.Controls.ItemCollection)lblist.Items;
                    items[lblist.SelectedIndex] = pfd;
                }
                else
                    lblist.Items.Add(pfd);
            }
            catch (Exception ex)
            {
                Helper.ExceptionMessage(Localization.Manager.GetString("errconvert"), ex);
            }
        }

        private void AddFile(object sender, RoutedEventArgs e)
        {
            lblist.SelectedIndex = -1;
            ChangeFile(null, null);
            lblist.SelectedIndex = lblist.ItemCount - 1;
        }

        private void DeleteFile(object sender, RoutedEventArgs e)
        {
            llcommit.IsEnabled = false;
            lldelete.IsEnabled = false;
            if (lblist.SelectedIndex < 0) return;

            lblist.Items.Remove(lblist.Items[lblist.SelectedIndex]);

            if (lblist.ItemCount > 0)
            {
                llcommit.IsEnabled = true;
                lldelete.IsEnabled = true;
            }
        }

        private void AutoChange(object sender, EventArgs e)
        {
            if (_updating) return;
            _updating = true;
            if (lblist.SelectedIndex >= 0) ChangeFile(null, null);
            _updating = false;
        }

        private void CommitAll(object sender, RoutedEventArgs e)
        {
            try
            {
                var pfds = new Interfaces.Files.IPackedFileDescriptor[lblist.ItemCount];
                for (int i = 0; i < pfds.Length; i++)
                    pfds[i] = (Interfaces.Files.IPackedFileDescriptor)lblist.Items[i];

                wrapper.Items = pfds;
                wrapper.SynchronizeUserData();
                Message.Show(Localization.Manager.GetString("commited"), "Info", MessageBoxButtons.OK);
            }
            catch (Exception ex)
            {
                Helper.ExceptionMessage(Localization.Manager.GetString("errwritingfile"), ex);
            }
        }

        private void ShowPackageSelector(object sender, RoutedEventArgs e)
        {
            var form = new PackageSelectorForm();
            form.Execute(wrapper.Package);
        }

        private void PackageItemDragOver(object sender, DragEventArgs e)
        {
            e.DragEffects = e.Data.Contains(typeof(SimPe.Packages.PackedFileDescriptor).FullName)
                ? DragDropEffects.Copy
                : DragDropEffects.None;
            e.Handled = true;
        }

        private void PackageItemDrop(object sender, DragEventArgs e)
        {
            try
            {
                var pfd = e.Data.Get(typeof(SimPe.Packages.PackedFileDescriptor).FullName)
                            as Interfaces.Files.IPackedFileDescriptor;
                if (pfd == null) return;

                NmapItem nmi = new NmapItem(wrapper)
                {
                    Group    = pfd.Group,
                    Type     = pfd.Type,
                    SubType  = pfd.SubType,
                    Instance = pfd.Instance,
                    Filename = Data.MetaData.FindTypeAlias(pfd.Type).Name
                };
                lblist.Items.Add(nmi);
            }
            catch (Exception ex)
            {
                Helper.ExceptionMessage("", ex);
            }
        }

        private void tbFindName_TextChanged(object sender, EventArgs e)
        {
            string name = tbfindname.Text.Trim().ToLower();
            for (int i = 0; i < lblist.ItemCount; i++)
            {
                var pfd = (Packages.PackedFileDescriptor)lblist.Items[i];
                if (pfd.Filename.Trim().ToLower().StartsWith(name))
                {
                    _updating = true;
                    tbfindname.Text           = pfd.Filename.Trim();
                    tbfindname.SelectionStart = name.Length;
                    tbfindname.SelectionEnd   = tbfindname.Text.Length;
                    _updating = false;
                    lblist.SelectedIndex = i;
                    break;
                }
            }
        }

        private async void CreateTextFile(object sender, RoutedEventArgs e)
        {
            var top = TopLevel.GetTopLevel(this);
            if (top == null) return;

            var file = await top.StorageProvider.SaveFilePickerAsync(new Avalonia.Platform.Storage.FilePickerSaveOptions
            {
                SuggestedFileName = System.IO.Path.GetFileNameWithoutExtension(wrapper.Package.FileName) + "_NameMap.txt",
                DefaultExtension  = "txt"
            });
            if (file == null) return;

            try
            {
                await using var stream = await file.OpenWriteAsync();
                await using var tw     = new System.IO.StreamWriter(stream);
                await tw.WriteLineAsync("Filename; Group; Instance; ");
                foreach (Packages.PackedFileDescriptor pfd in lblist.Items)
                    await tw.WriteLineAsync(
                        pfd.Filename + "; " +
                        "0x" + Helper.HexString(pfd.Group) + "; " +
                        "0x" + Helper.HexString(pfd.Instance) + "; ");
            }
            catch (Exception ex)
            {
                Helper.ExceptionMessage("", ex);
            }
        }
    }
}
