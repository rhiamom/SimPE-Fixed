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
using SimPe.Scenegraph.Compat;

namespace SimPe
{
    /// <summary>
    /// Summary description for ImportSemi.
    /// </summary>
    public class ImportSemi : Avalonia.Controls.Window
    {
        private Avalonia.Controls.Button btimport = new Avalonia.Controls.Button();
        private Avalonia.Controls.ComboBox cbsemi = new Avalonia.Controls.ComboBox();
        private Avalonia.Controls.Label label1 = new Avalonia.Controls.Label();
        private Avalonia.Controls.Button llscan = new Avalonia.Controls.Button();
        private SimPe.Scenegraph.Compat.CheckedListBox lbfiles = new SimPe.Scenegraph.Compat.CheckedListBox();
        private Avalonia.Controls.Label label2 = new Avalonia.Controls.Label();
        private Avalonia.Controls.Button linkLabel1 = new Avalonia.Controls.Button();
        private Avalonia.Controls.CheckBox cbfix = new Avalonia.Controls.CheckBox();
        private Avalonia.Controls.CheckBox cbname = new Avalonia.Controls.CheckBox();
        private Avalonia.Controls.Panel panel1 = new Avalonia.Controls.Panel();

        public ImportSemi()
        {
            Title = "Import SemiGlobals";
            Width = 800;
            Height = 540;

            label1.Content = "Semi Global:";
            label2.Content = "Files:";
            btimport.Content = "Import";
            llscan.Content = "scan";
            linkLabel1.Content = "uncheck all";
            cbfix.Content = "Fix Package References";
            cbname.Content = "Add name Prefix";

            btimport.IsEnabled = false;
            btimport.Click += ImportFiles;
            llscan.Click += ScanSemiGlobals;
            linkLabel1.Click += linkLabel1_Click;

            var headerRow = new Avalonia.Controls.StackPanel
            {
                Orientation = Avalonia.Layout.Orientation.Horizontal,
                Margin = new Avalonia.Thickness(8, 4)
            };
            headerRow.Children.Add(label1);
            headerRow.Children.Add(cbsemi);
            headerRow.Children.Add(llscan);

            var filesRow = new Avalonia.Controls.StackPanel
            {
                Orientation = Avalonia.Layout.Orientation.Horizontal,
                Margin = new Avalonia.Thickness(8, 2)
            };
            filesRow.Children.Add(label2);

            var bottomRow = new Avalonia.Controls.StackPanel
            {
                Orientation = Avalonia.Layout.Orientation.Horizontal,
                HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Right,
                Margin = new Avalonia.Thickness(8, 4),
                Spacing = 8
            };
            bottomRow.Children.Add(linkLabel1);
            bottomRow.Children.Add(cbname);
            bottomRow.Children.Add(cbfix);
            bottomRow.Children.Add(btimport);

            var dock = new Avalonia.Controls.DockPanel { Margin = new Avalonia.Thickness(4) };
            Avalonia.Controls.DockPanel.SetDock(headerRow, Avalonia.Controls.Dock.Top);
            Avalonia.Controls.DockPanel.SetDock(filesRow, Avalonia.Controls.Dock.Top);
            Avalonia.Controls.DockPanel.SetDock(bottomRow, Avalonia.Controls.Dock.Bottom);
            dock.Children.Add(headerRow);
            dock.Children.Add(filesRow);
            dock.Children.Add(bottomRow);
            dock.Children.Add(lbfiles);

            panel1.Children.Add(dock);
            Content = panel1;

            ThemeManager tm = ThemeManager.Global.CreateChild();
            tm.AddControl(panel1);

            WaitingScreen.Wait();
            try
            {
                WaitingScreen.UpdateMessage("getting all SemiGlobal Groups");
                FileTable.FileIndex.Load();

                Interfaces.Scenegraph.IScenegraphFileIndexItem[] globs = FileTable.FileIndex.FindFile(Data.MetaData.GLOB_FILE, true);
                ArrayList names = new ArrayList();
                string max = " / " + globs.Length.ToString();
                int ct = 0;
                foreach (Interfaces.Scenegraph.IScenegraphFileIndexItem item in globs)
                {
                    if (ct % 17 == 0) WaitingScreen.UpdateMessage(ct.ToString() + max);
                    ct++;

                    SimPe.Plugin.NamedGlob glob = new SimPe.Plugin.NamedGlob();
                    glob.ProcessData(item.FileDescriptor, item.Package);

                    if (!names.Contains(glob.SemiGlobalName.Trim().ToLower()))
                    {
                        cbsemi.Items.Add(glob);
                        names.Add(glob.SemiGlobalName.Trim().ToLower());
                    }
                }
            }
            finally { WaitingScreen.Stop(); }
        }

        SimPe.Interfaces.Files.IPackageFile package;
        SimPe.Interfaces.IWrapperRegistry reg;
        SimPe.Interfaces.IProviderRegistry prov;

        public void Execute(SimPe.Interfaces.Files.IPackageFile package, SimPe.Interfaces.IWrapperRegistry reg, SimPe.Interfaces.IProviderRegistry prov)
        {
            this.package = package;
            this.reg = reg;
            this.prov = prov;

            btimport.IsEnabled = false;
            lbfiles.Items.Clear();
            ShowDialog(null).GetAwaiter().GetResult();
        }

        private void ScanSemiGlobals(object sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            Cursor = new Avalonia.Input.Cursor(Avalonia.Input.StandardCursorType.Wait);
            lbfiles.Items.Clear();
            btimport.IsEnabled = false;

            if (cbsemi.SelectedIndex < 0) return;
            ArrayList loaded = new ArrayList();

            try
            {
                SimPe.Plugin.NamedGlob glob = (SimPe.Plugin.NamedGlob)cbsemi.Items[cbsemi.SelectedIndex];
                Interfaces.Scenegraph.IScenegraphFileIndexItem[] items = FileTable.FileIndex.FindFileByGroup(glob.SemiGlobalGroup);

                foreach (Interfaces.Scenegraph.IScenegraphFileIndexItem item in items)
                {
                    if (item.FileDescriptor.Type == Data.MetaData.BHAV_FILE)
                    {
                        SimPe.Plugin.Bhav bhav = new SimPe.Plugin.Bhav(null);
                        bhav.ProcessData(item);
                        item.FileDescriptor.Filename = item.FileDescriptor.TypeName.shortname + ": " + bhav.FileName + " (" + item.FileDescriptor.ToString() + ")";
                    }
                    else if (item.FileDescriptor.Type == Data.MetaData.STRING_FILE)
                    {
                        SimPe.PackedFiles.Wrapper.Str str = new SimPe.PackedFiles.Wrapper.Str();
                        str.ProcessData(item);
                        item.FileDescriptor.Filename = item.FileDescriptor.TypeName.shortname + ": " + str.FileName + " (" + item.FileDescriptor.ToString() + ")";
                    }
                    else if (item.FileDescriptor.Type == 0x42434F4E)  //BCON
                    {
                        SimPe.Plugin.Bcon bcon = new SimPe.Plugin.Bcon();
                        bcon.ProcessData(item);
                        item.FileDescriptor.Filename = item.FileDescriptor.TypeName.shortname + ": " + bcon.FileName + " (" + item.FileDescriptor.ToString() + ")";
                    }
                    else
                    {
                        item.FileDescriptor.Filename = item.FileDescriptor.ToString();
                    }

                    if (!loaded.Contains(item.FileDescriptor))
                    {
                        lbfiles.Items.Add(item, ((item.FileDescriptor.Type == Data.MetaData.BHAV_FILE) || (item.FileDescriptor.Type == 0x42434F4E)));
                        loaded.Add(item.FileDescriptor);
                    }
                }
                btimport.IsEnabled = (lbfiles.Items.Count > 0);
            }
            catch (Exception) { }

            Cursor = null;
        }

        private void ImportFiles(object sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            Cursor = new Avalonia.Input.Cursor(Avalonia.Input.StandardCursorType.Wait);

            //first find the highest Instance of a BHAV, BCON in the package
            Interfaces.Files.IPackedFileDescriptor[] pfds = package.FindFiles(Data.MetaData.BHAV_FILE);
            uint maxbhavinst = 0x1000;
            foreach (Interfaces.Files.IPackedFileDescriptor pfd in pfds) if ((pfd.Instance < 0x2000) && (pfd.Instance > maxbhavinst)) maxbhavinst = pfd.Instance;
            Hashtable bhavalias = new Hashtable();
            maxbhavinst++;

            //her is the BCOn part
            pfds = package.FindFiles(0x42434F4E);
            uint maxbconinst = 0x1000;
            foreach (Interfaces.Files.IPackedFileDescriptor pfd in pfds) if ((pfd.Instance < 0x2000) && (pfd.Instance > maxbconinst)) maxbconinst = pfd.Instance;
            Hashtable bconalias = new Hashtable();
            maxbconinst++;

            ArrayList bhavs = new ArrayList();
            ArrayList ttabs = new ArrayList();
            package.BeginUpdate();
            try
            {
                for (int i = 0; i < lbfiles.Items.Count; i++)
                {
                    if (!lbfiles.GetItemChecked(i)) continue;
                    Interfaces.Scenegraph.IScenegraphFileIndexItem item = (Interfaces.Scenegraph.IScenegraphFileIndexItem)lbfiles.Items[i];
                    SimPe.Packages.PackedFileDescriptor npfd = new SimPe.Packages.PackedFileDescriptor();
                    npfd.Type = item.FileDescriptor.Type;
                    npfd.Group = item.FileDescriptor.Group;
                    npfd.Instance = item.FileDescriptor.Instance;
                    npfd.SubType = item.FileDescriptor.SubType;
                    npfd.UserData = item.Package.Read(item.FileDescriptor).UncompressedData;
                    package.Add(npfd);

                    if (npfd.Type == Data.MetaData.BHAV_FILE)
                    {
                        maxbhavinst++;
                        npfd.Group = 0xffffffff;
                        bhavalias[(ushort)npfd.Instance] = (ushort)maxbhavinst;
                        npfd.Instance = maxbhavinst;

                        SimPe.Plugin.Bhav bhav = new SimPe.Plugin.Bhav(prov.OpcodeProvider);
                        bhav.ProcessData(npfd, package);
                        if (cbname.IsChecked == true) bhav.FileName = "[" + cbsemi.SelectedItem?.ToString() + "] " + bhav.FileName;
                        bhav.SynchronizeUserData();

                        bhavs.Add(bhav);
                    }
                    else if (npfd.Type == 0x42434F4E) //BCON
                    {
                        npfd.Instance = maxbconinst++;
                        npfd.Group = 0xffffffff;
                        bconalias[(ushort)npfd.Instance] = (ushort)npfd.Instance;

                        SimPe.Plugin.Bcon bcon = new SimPe.Plugin.Bcon();
                        bcon.ProcessData(npfd, package);
                        if (cbname.IsChecked == true) bcon.FileName = "[" + cbsemi.SelectedItem?.ToString() + "] " + bcon.FileName;
                        bcon.SynchronizeUserData();
                    }
                    else if (npfd.Type == 0x54544142) //TTAB
                    {
                        SimPe.Plugin.Ttab ttab = new SimPe.Plugin.Ttab(prov.OpcodeProvider);
                        ttab.ProcessData(npfd, package);
                        ttabs.Add(ttab);
                    }
                }

                if (cbfix.IsChecked == true)
                {
                    pfds = package.FindFiles(Data.MetaData.BHAV_FILE);
                    foreach (Interfaces.Files.IPackedFileDescriptor pfd in pfds)
                    {
                        SimPe.Plugin.Bhav bhav = new SimPe.Plugin.Bhav(prov.OpcodeProvider);
                        bhav.ProcessData(pfd, package);
                        bhavs.Add(bhav);
                    }

                    pfds = package.FindFiles(0x54544142);
                    foreach (Interfaces.Files.IPackedFileDescriptor pfd in pfds)
                    {
                        SimPe.Plugin.Ttab ttab = new SimPe.Plugin.Ttab(prov.OpcodeProvider);
                        ttab.ProcessData(pfd, package);
                        ttabs.Add(ttab);
                    }
                }

                //Relink all SemiGlobals in imported BHAV's
                foreach (SimPe.Plugin.Bhav bhav in bhavs)
                {
                    foreach (SimPe.PackedFiles.Wrapper.Instruction i in bhav)
                    {
                        if (bhavalias.Contains(i.OpCode)) i.OpCode = (ushort)bhavalias[i.OpCode];
                    }
                    bhav.SynchronizeUserData();
                }

                //Relink all TTAbs
                foreach (SimPe.Plugin.Ttab ttab in ttabs)
                {
                    foreach (SimPe.PackedFiles.Wrapper.TtabItem item in ttab)
                    {
                        if (bhavalias.Contains(item.Guardian)) item.Guardian = (ushort)bhavalias[item.Guardian];
                        if (bhavalias.Contains(item.Action)) item.Action = (ushort)bhavalias[item.Action];
                    }
                    ttab.SynchronizeUserData();
                }
            }
            finally
            {
                package.EndUpdate();
            }
            Cursor = null;
            Close();
        }

        private void linkLabel1_Click(object sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            for (int i = 0; i < lbfiles.Items.Count; i++)
                lbfiles.SetItemChecked(i, false);
        }
    }
}
