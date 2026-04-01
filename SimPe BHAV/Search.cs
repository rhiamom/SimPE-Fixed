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
using SimPe.PackedFiles.Wrapper;

namespace SimPe.Plugin
{
    /// <summary>
    /// Summary description for Search.
    /// </summary>
    public class Search : Avalonia.Controls.Window
    {
        private Avalonia.Controls.TabControl tabControl1 = new Avalonia.Controls.TabControl();
        private Avalonia.Controls.TabItem tabPage1 = new Avalonia.Controls.TabItem();
        private Avalonia.Controls.ListBox lblist = new Avalonia.Controls.ListBox();
        private Avalonia.Controls.Button btopen = new Avalonia.Controls.Button();
        private Avalonia.Controls.Label label1 = new Avalonia.Controls.Label();
        private Avalonia.Controls.TextBox tbOpcode = new Avalonia.Controls.TextBox();
        private Avalonia.Controls.Button llsearch = new Avalonia.Controls.Button();
        private Avalonia.Controls.ProgressBar pb = new Avalonia.Controls.ProgressBar();
        private Avalonia.Controls.TabItem tabPage2 = new Avalonia.Controls.TabItem();
        private Avalonia.Controls.Button linkLabel1 = new Avalonia.Controls.Button();
        private Avalonia.Controls.TextBox tbflname = new Avalonia.Controls.TextBox();
        private Avalonia.Controls.Label label2 = new Avalonia.Controls.Label();
        private Avalonia.Controls.TabItem tabPage3 = new Avalonia.Controls.TabItem();
        private Avalonia.Controls.Button linkLabel2 = new Avalonia.Controls.Button();
        private Avalonia.Controls.Label label3 = new Avalonia.Controls.Label();
        private Avalonia.Controls.TextBox tbsimname = new Avalonia.Controls.TextBox();
        private Avalonia.Controls.TabItem tabPage4 = new Avalonia.Controls.TabItem();
        private Avalonia.Controls.Label label4 = new Avalonia.Controls.Label();
        private Avalonia.Controls.Button linkLabel3 = new Avalonia.Controls.Button();
        private Avalonia.Controls.TextBox tbpropname = new Avalonia.Controls.TextBox();
        private Avalonia.Controls.TextBox tbbhavgroup = new Avalonia.Controls.TextBox();
        private Avalonia.Controls.Label label5 = new Avalonia.Controls.Label();
        private Avalonia.Controls.RadioButton rbfull = new Avalonia.Controls.RadioButton();
        private Avalonia.Controls.RadioButton rbstart = new Avalonia.Controls.RadioButton();
        private Avalonia.Controls.RadioButton rbend = new Avalonia.Controls.RadioButton();
        private Avalonia.Controls.RadioButton rbcont = new Avalonia.Controls.RadioButton();
        private Avalonia.Controls.CheckBox cbusefileindex = new Avalonia.Controls.CheckBox();
        private Avalonia.Controls.TabItem tabPage5 = new Avalonia.Controls.TabItem();
        private Avalonia.Controls.Button linkLabel4 = new Avalonia.Controls.Button();
        private Avalonia.Controls.TextBox tbguid = new Avalonia.Controls.TextBox();
        private Avalonia.Controls.Label label6 = new Avalonia.Controls.Label();
        private Avalonia.Controls.TextBox tbpropval = new Avalonia.Controls.TextBox();
        private Avalonia.Controls.Label label7 = new Avalonia.Controls.Label();
        private Avalonia.Controls.CheckBox cblastname = new Avalonia.Controls.CheckBox();
        private Avalonia.Controls.Panel panel1 = new Avalonia.Controls.Panel();

        public Search()
        {
            Title = "Search";
            Width = 800;
            Height = 540;

            prov = null;
            BuildLayout();

            ThemeManager tm = ThemeManager.Global.CreateChild();
            tm.AddControl(panel1);

            if (SimPe.Helper.XmlRegistry.UseBigIcons) lblist.FontSize = 14;
        }

        private void BuildLayout()
        {
            // Tab 1: BHAV
            label1.Content = "Contains Opcode:";
            label5.Content = "Group:";
            tbOpcode.Text = "0x0000";
            llsearch.Content = "search";
            llsearch.Click += BhavSearch;
            var tab1Panel = new Avalonia.Controls.StackPanel { Margin = new Avalonia.Thickness(4) };
            var bhavRow1 = new Avalonia.Controls.StackPanel { Orientation = Avalonia.Layout.Orientation.Horizontal, Spacing = 4 };
            bhavRow1.Children.Add(label1); bhavRow1.Children.Add(tbOpcode);
            bhavRow1.Children.Add(label5); bhavRow1.Children.Add(tbbhavgroup);
            var bhavRow2 = new Avalonia.Controls.StackPanel { Orientation = Avalonia.Layout.Orientation.Horizontal, HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Right };
            bhavRow2.Children.Add(llsearch);
            tab1Panel.Children.Add(bhavRow1); tab1Panel.Children.Add(bhavRow2);
            tabPage1.Header = "BHAV"; tabPage1.Content = tab1Panel;

            // Tab 2: RCOL
            label2.Content = "Filename:";
            linkLabel1.Content = "search";
            linkLabel1.Click += RcolSearch;
            cbusefileindex.Content = "scan in all Files";
            var tab2Panel = new Avalonia.Controls.StackPanel { Margin = new Avalonia.Thickness(4) };
            var rcolRow1 = new Avalonia.Controls.StackPanel { Orientation = Avalonia.Layout.Orientation.Horizontal, Spacing = 4 };
            rcolRow1.Children.Add(label2); rcolRow1.Children.Add(tbflname);
            var rcolRow2 = new Avalonia.Controls.StackPanel { Orientation = Avalonia.Layout.Orientation.Horizontal, Spacing = 8, HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Right };
            rcolRow2.Children.Add(cbusefileindex); rcolRow2.Children.Add(linkLabel1);
            tab2Panel.Children.Add(rcolRow1); tab2Panel.Children.Add(rcolRow2);
            tabPage2.Header = "RCOL"; tabPage2.Content = tab2Panel;

            // Tab 3: Sims
            label3.Content = "Sim Name:";
            linkLabel2.Content = "search";
            linkLabel2.Click += FindSim;
            cblastname.Content = "Use Last or Family name only";
            var tab3Panel = new Avalonia.Controls.StackPanel { Margin = new Avalonia.Thickness(4) };
            var simsRow1 = new Avalonia.Controls.StackPanel { Orientation = Avalonia.Layout.Orientation.Horizontal, Spacing = 4 };
            simsRow1.Children.Add(label3); simsRow1.Children.Add(tbsimname);
            var simsRow2 = new Avalonia.Controls.StackPanel { Orientation = Avalonia.Layout.Orientation.Horizontal, Spacing = 8, HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Right };
            simsRow2.Children.Add(cblastname); simsRow2.Children.Add(linkLabel2);
            tab3Panel.Children.Add(simsRow1); tab3Panel.Children.Add(simsRow2);
            tabPage3.Header = "Sims"; tabPage3.Content = tab3Panel;

            // Tab 4: Property Set
            label4.Content = "Name:"; label7.Content = "Value:";
            tbpropname.Text = "name";
            rbfull.Content = "match"; rbstart.Content = "start"; rbend.Content = "end"; rbcont.Content = "contains";
            rbstart.IsChecked = true;
            linkLabel3.Content = "search";
            linkLabel3.Click += GzpsSearch;
            var tab4Panel = new Avalonia.Controls.StackPanel { Margin = new Avalonia.Thickness(4) };
            var propRow1 = new Avalonia.Controls.StackPanel { Orientation = Avalonia.Layout.Orientation.Horizontal, Spacing = 4 };
            propRow1.Children.Add(label4); propRow1.Children.Add(tbpropname);
            propRow1.Children.Add(label7); propRow1.Children.Add(tbpropval);
            var propRow2 = new Avalonia.Controls.StackPanel { Orientation = Avalonia.Layout.Orientation.Horizontal, Spacing = 4 };
            propRow2.Children.Add(rbfull); propRow2.Children.Add(rbstart);
            propRow2.Children.Add(rbend); propRow2.Children.Add(rbcont);
            var propRow3 = new Avalonia.Controls.StackPanel { Orientation = Avalonia.Layout.Orientation.Horizontal, HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Right };
            propRow3.Children.Add(linkLabel3);
            tab4Panel.Children.Add(propRow1); tab4Panel.Children.Add(propRow2); tab4Panel.Children.Add(propRow3);
            tabPage4.Header = "Property Set"; tabPage4.Content = tab4Panel;

            // Tab 5: GUID
            label6.Content = "GUID:";
            tbguid.Text = "0x00000000";
            linkLabel4.Content = "search";
            linkLabel4.Click += GuidSearch;
            var tab5Panel = new Avalonia.Controls.StackPanel { Margin = new Avalonia.Thickness(4) };
            var guidRow = new Avalonia.Controls.StackPanel { Orientation = Avalonia.Layout.Orientation.Horizontal, Spacing = 4 };
            guidRow.Children.Add(label6); guidRow.Children.Add(tbguid);
            var guidRow2 = new Avalonia.Controls.StackPanel { Orientation = Avalonia.Layout.Orientation.Horizontal, HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Right };
            guidRow2.Children.Add(linkLabel4);
            tab5Panel.Children.Add(guidRow); tab5Panel.Children.Add(guidRow2);
            tabPage5.Header = "GUID"; tabPage5.Content = tab5Panel;

            tabControl1.Items.Add(tabPage1);
            tabControl1.Items.Add(tabPage2);
            tabControl1.Items.Add(tabPage3);
            tabControl1.Items.Add(tabPage4);
            tabControl1.Items.Add(tabPage5);

            btopen.Content = "Open";
            btopen.IsEnabled = false;
            btopen.Click += Open;
            lblist.SelectionChanged += SelectFile;

            pb.Maximum = 1000;
            pb.Value = 0;

            var bottomRow = new Avalonia.Controls.StackPanel
            {
                Orientation = Avalonia.Layout.Orientation.Horizontal,
                Margin = new Avalonia.Thickness(0, 4)
            };
            bottomRow.Children.Add(pb);
            bottomRow.Children.Add(btopen);

            var dock = new Avalonia.Controls.DockPanel { Margin = new Avalonia.Thickness(8) };
            Avalonia.Controls.DockPanel.SetDock(tabControl1, Avalonia.Controls.Dock.Top);
            Avalonia.Controls.DockPanel.SetDock(bottomRow, Avalonia.Controls.Dock.Bottom);
            dock.Children.Add(tabControl1);
            dock.Children.Add(bottomRow);
            dock.Children.Add(lblist);

            panel1.Children.Add(dock);
            Content = panel1;
        }

        #region Seeker Infrastructure
        /// <summary>
        /// Delegate for Search Functions
        /// </summary>
        public delegate SearchItem SeekerFunction(Interfaces.Files.IPackedFileDescriptor pfd, Interfaces.Files.IPackageFile package, Interfaces.IProviderRegistry prov);

        protected void StartSearch(SeekerFunction fkt, Interfaces.Files.IPackedFileDescriptor[] pfds)
        {
            try
            {
                pb.Value = 0;
                btopen.Tag = null;
                lblist.Items.Clear();
                btopen.IsEnabled = false;
                Cursor = new Avalonia.Input.Cursor(Avalonia.Input.StandardCursorType.Wait);

                int count = 0;
                foreach (Interfaces.Files.IPackedFileDescriptor pfd in pfds)
                {
                    pb.Value = (count++ * pb.Maximum) / pfds.Length;
                    SearchItem si = fkt(pfd, package, prov);
                    if (si != null) lblist.Items.Add(si);
                }

                if (lblist.Items.Count == 0) SimPe.Scenegraph.Compat.MessageBox.ShowAsync("No Files were found").GetAwaiter().GetResult();
                else lblist.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                Helper.ExceptionMessage("", ex);
            }
            finally
            {
                Cursor = null;
                pb.Value = 0;
            }
        }
        #endregion

        #region Seekers
        public SearchItem BhavSearch(Interfaces.Files.IPackedFileDescriptor pfd, Interfaces.Files.IPackageFile package, Interfaces.IProviderRegistry prov)
        {
            ushort opcode = Convert.ToUInt16(this.tbOpcode.Text, 16);

            if (tbbhavgroup.Text.Trim() != "")
            {
                uint group = Convert.ToUInt32(this.tbbhavgroup.Text, 16);
                if (pfd.Group != group) return null;
            }

            Bhav bhav = new Bhav(prov.OpcodeProvider);
            bhav.ProcessData(pfd, package);

            foreach (Instruction i in bhav)
            {
                if (i.OpCode == opcode)
                    return new SearchItem(bhav.FileName, pfd);
            }

            return null;
        }

        public SearchItem RcolSearch(Interfaces.Files.IPackedFileDescriptor pfd, Interfaces.Files.IPackageFile package, Interfaces.IProviderRegistry prov)
        {
            string flname = Hashes.StripHashFromName(tbflname.Text);
            uint inst = Hashes.InstanceHash(flname);
            uint st = Hashes.SubTypeHash(flname);

            if ((pfd.Instance == inst) && ((pfd.SubType == st) || pfd.SubType == 0))
            {
                SimPe.Plugin.Rcol rcol = new GenericRcol(prov, false);
                rcol.ProcessData(pfd, package);
                return new SearchItem(rcol.FileName, pfd);
            }

            return null;
        }

        public SearchItem SdscSearch(Interfaces.Files.IPackedFileDescriptor pfd, Interfaces.Files.IPackageFile package, Interfaces.IProviderRegistry prov)
        {
            string name = tbsimname.Text.Trim().ToLower();

            SimPe.PackedFiles.Wrapper.SDesc sdesc = new SimPe.PackedFiles.Wrapper.SDesc(prov.SimNameProvider, prov.SimFamilynameProvider, prov.SimDescriptionProvider);
            sdesc.ProcessData(pfd, package);

            string ext = "";
            if (sdesc.Unlinked != 0x00) ext += "unlinked";
            if (!sdesc.AvailableCharacterData)
            {
                if (ext.Trim() != "") ext += ", no Character Data";
            }

            if (ext.Trim() != "") ext = " (" + ext + ")";

            string simname = "";

            if (cblastname.IsChecked == true)
            {
                simname = sdesc.SimFamilyName;
                simname = simname.Trim().ToLower();
                if (simname == name) return new SearchItem(sdesc.SimName + " " + sdesc.SimFamilyName + ext, pfd);

                simname = sdesc.HouseholdName;
                simname = simname.Trim().ToLower();
                if (simname == name) return new SearchItem(sdesc.SimName + " " + sdesc.SimFamilyName + ext, pfd);
            }
            else
            {
                simname = sdesc.SimName + " " + sdesc.SimFamilyName;
                simname = simname.Trim().ToLower();
                if (simname == name) return new SearchItem(sdesc.SimName + " " + sdesc.SimFamilyName + ext, pfd);

                simname = sdesc.SimName + " " + sdesc.HouseholdName;
                simname = simname.Trim().ToLower();
                if (simname == name) return new SearchItem(sdesc.SimName + " " + sdesc.SimFamilyName + ext, pfd);

                simname = sdesc.SimName;
                simname = simname.Trim().ToLower();
                if (simname == name) return new SearchItem(sdesc.SimName + " " + sdesc.SimFamilyName + ext, pfd);
            }

            return null;
        }

        public SearchItem GzpsSearch(Interfaces.Files.IPackedFileDescriptor pfd, Interfaces.Files.IPackageFile package, Interfaces.IProviderRegistry prov)
        {
            SimPe.PackedFiles.Wrapper.Cpf cpf = new SimPe.PackedFiles.Wrapper.Cpf();
            cpf.ProcessData(pfd, package);

            bool check = false;
            string s1 = cpf.GetSaveItem(tbpropname.Text).StringValue.Trim().ToLower();
            string s2 = tbpropval.Text.Trim().ToLower();
            if (rbfull.IsChecked == true) check = (s1 == s2);
            if (rbstart.IsChecked == true) check = (s1.StartsWith(s2));
            if (rbend.IsChecked == true) check = (s1.EndsWith(s2));
            if (rbcont.IsChecked == true) check = (s1.IndexOf(s2) != -1);
            if (check)
                return new SearchItem(cpf.FileDescriptor.ToString(), pfd);

            return null;
        }

        public SearchItem GuidSearch(Interfaces.Files.IPackedFileDescriptor pfd, Interfaces.Files.IPackageFile package, Interfaces.IProviderRegistry prov)
        {
            uint guid = Convert.ToUInt32(tbguid.Text, 16);

            SimPe.PackedFiles.Wrapper.ExtObjd objd = new SimPe.PackedFiles.Wrapper.ExtObjd();
            objd.ProcessData(pfd, package);

            if (objd.Guid == guid) return new SearchItem(objd.FileName, pfd);
            return null;
        }
        #endregion

        private void FindSim(object sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            StartSearch(new SeekerFunction(SdscSearch), package.FindFiles(Data.MetaData.SIM_DESCRIPTION_FILE));
        }

        Interfaces.Files.IPackageFile package;
        Interfaces.Files.IPackedFileDescriptor pfd;
        internal Interfaces.IProviderRegistry prov;

        internal Interfaces.Files.IPackedFileDescriptor Execute(Interfaces.Files.IPackageFile package)
        {
            this.package = package;
            this.pfd = null;
            RemoteControl.ShowSubForm(this);

            return pfd;
        }

        internal void Reset()
        {
            lblist.Items.Clear();
            btopen.IsEnabled = false;
        }

        private void BhavSearch(object sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            StartSearch(new SeekerFunction(BhavSearch), package.FindFiles(Data.MetaData.BHAV_FILE));
        }

        private void Open(object sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            if (lblist.SelectedIndex < 0) return;
            try
            {
                SearchItem si = (SearchItem)lblist.Items[lblist.SelectedIndex];
                pfd = si.Descriptor;
                Close();
            }
            catch (Exception ex)
            {
                Helper.ExceptionMessage("", ex);
            }
        }

        private void SelectFile(object sender, Avalonia.Controls.SelectionChangedEventArgs e)
        {
            btopen.IsEnabled = false;
            if (lblist.SelectedIndex < 0) return;
            btopen.IsEnabled = (btopen.Tag == null);
        }

        private void GzpsSearch(object sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            Interfaces.Files.IPackedFileDescriptor[] pfds = (Interfaces.Files.IPackedFileDescriptor[])Helper.Merge(package.FindFiles(0xEBCF3E27), package.FindFiles(0x4C697E5A), typeof(Interfaces.Files.IPackedFileDescriptor));
            StartSearch(new SeekerFunction(GzpsSearch), pfds);
        }

        private void GuidSearch(object sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            Interfaces.Files.IPackedFileDescriptor[] pfds = package.FindFiles(Data.MetaData.OBJD_FILE);
            StartSearch(new SeekerFunction(GuidSearch), pfds);
        }

        private void RcolSearch(object sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            if (cbusefileindex.IsChecked == true)
            {
                WaitingScreen.Wait();
                try { SimPe.FileTable.FileIndex.Load(); }
                finally { WaitingScreen.Stop(this); }

                lblist.Items.Clear();
                SimPe.Packages.PackedFileDescriptor pfd = new SimPe.Packages.PackedFileDescriptor();
                pfd.SubType = Hashes.SubTypeHash(Hashes.StripHashFromName(tbflname.Text));
                pfd.Instance = Hashes.InstanceHash(Hashes.StripHashFromName(tbflname.Text));

                SimPe.Interfaces.Scenegraph.IScenegraphFileIndexItem[] items = FileTable.FileIndex.FindFileByInstance(pfd.LongInstance);

                if (items.Length == 0)
                {
                    pfd.SubType = 0;
                    items = FileTable.FileIndex.FindFileByInstance(pfd.LongInstance);
                }

                foreach (SimPe.Interfaces.Scenegraph.IScenegraphFileIndexItem item in items)
                    lblist.Items.Add(item.Package.FileName);

                btopen.Tag = true;
            }
            else
            {
                StartSearch(new SeekerFunction(RcolSearch), package.FindFile(Hashes.StripHashFromName(tbflname.Text)));
            }
        }
    }
}
