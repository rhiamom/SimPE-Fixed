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
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Layout;
using Avalonia.Platform.Storage;

namespace SimPe.Plugin.Tool.Window
{
    /// <summary>
    /// Summary description for PackageRepairForm.
    /// </summary>
    class PackageRepairForm : Avalonia.Controls.Window
    {
        private TextBox tbPkg;
        private Button btBrowse;
        private Button llRepair;
        private Button llOpen;
        private TextBlock pg;

        public PackageRepairForm()
        {
            Title = "Package Repair";
            Width = 594;
            Height = 361;
            CanResize = true;
            WindowStartupLocation = WindowStartupLocation.CenterOwner;

            var label1 = new TextBlock
            {
                Text = "Package:",
                FontWeight = Avalonia.Media.FontWeight.Bold,
                VerticalAlignment = VerticalAlignment.Center,
                Width = 64
            };

            tbPkg = new TextBox { IsReadOnly = true, HorizontalAlignment = HorizontalAlignment.Stretch };
            btBrowse = new Button { Content = "Browse..." };
            btBrowse.Click += btBrowse_Click;

            var topRow = new Grid { ColumnDefinitions = new ColumnDefinitions("Auto,*,Auto"), Margin = new Thickness(8) };
            Grid.SetColumn(label1,  0); topRow.Children.Add(label1);
            Grid.SetColumn(tbPkg,   1); topRow.Children.Add(tbPkg);
            Grid.SetColumn(btBrowse,2); topRow.Children.Add(btBrowse);

            pg = new TextBlock
            {
                TextWrapping = Avalonia.Media.TextWrapping.Wrap,
                VerticalAlignment   = VerticalAlignment.Top
            };

            llRepair = new Button { Content = "Repair", IsEnabled = false, HorizontalAlignment = HorizontalAlignment.Right };
            llOpen   = new Button { Content = "Open",   IsEnabled = false, HorizontalAlignment = HorizontalAlignment.Right };
            llRepair.Click += llRepair_Click;
            llOpen.Click   += llOpen_Click;

            var bottomRow = new StackPanel
            {
                Orientation = Orientation.Horizontal,
                HorizontalAlignment = HorizontalAlignment.Right,
                Spacing = 8,
                Margin = new Thickness(4),
                Children = { llRepair, llOpen }
            };

            var root = new Grid
            {
                RowDefinitions = new RowDefinitions("Auto,*,Auto"),
                Margin = new Thickness(4)
            };
            var pgScroll = new ScrollViewer { Content = pg };
            Grid.SetRow(topRow,    0); root.Children.Add(topRow);
            Grid.SetRow(pgScroll,  1); root.Children.Add(pgScroll);
            Grid.SetRow(bottomRow, 2); root.Children.Add(bottomRow);

            Content = root;

            Setup(null);
        }

        SimPe.Packages.StreamItem si;
        SimPe.Packages.PackageRepair pr;

        public void Setup(string pkgname)
        {
            if (pkgname == null) pkgname = "";

            this.tbPkg.Text = pkgname;
            this.Title = System.IO.Path.GetFileNameWithoutExtension(pkgname);

            si = null;
            pr = null;
            pg.Text = "";
            llOpen.IsEnabled = false;
            try
            {
                if (System.IO.File.Exists(pkgname))
                    si = SimPe.Packages.StreamFactory.UseStream(pkgname, System.IO.FileAccess.ReadWrite, false);

                if (!si.FileStream.CanWrite || !si.FileStream.CanRead)
                    si = null;

                if (si != null)
                {
                    pr = new SimPe.Packages.PackageRepair(SimPe.Packages.GeneratableFile.LoadFromFile(pkgname));

                    pg.Text = BuildPropertyText(pr.IndexDetailsAdvanced);
                    llOpen.IsEnabled = (pr.Package != null);
                }
            }
            catch { }

            llRepair.IsEnabled = (si != null);
        }

        private string BuildPropertyText(object obj)
        {
            if (obj == null) return "";
            var sb = new System.Text.StringBuilder();
            foreach (var prop in obj.GetType().GetProperties())
            {
                try { sb.AppendLine($"{prop.Name}: {prop.GetValue(obj)}"); }
                catch { }
            }
            return sb.ToString();
        }

        private async void btBrowse_Click(object sender, RoutedEventArgs e)
        {
            var topLevel = TopLevel.GetTopLevel(this);
            if (topLevel == null) return;
            var files = await topLevel.StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions
            {
                Title = "Open Package",
                AllowMultiple = false,
                FileTypeFilter = new[]
                {
                    new FilePickerFileType("Sims 2 Package") { Patterns = new[] { "*.package" } },
                    new FilePickerFileType("All Files") { Patterns = new[] { "*.*" } }
                }
            });
            if (files.Count > 0)
                Setup(files[0].Path.LocalPath);
        }

        private void llRepair_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                pr.UseIndexData(pr.FindIndexOffset());
                Message.Show(SimPe.Localization.GetString("FinishedPackageRepair"));

                pg.Text = BuildPropertyText(pr.IndexDetailsAdvanced);
            }
            catch (Exception ex)
            {
                Helper.ExceptionMessage(ex);
            }
        }

        private void llOpen_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                SimPe.RemoteControl.OpenMemoryPackageFkt(pr.Package);
                Close();
            }
            catch (Exception x)
            {
                Helper.ExceptionMessage(x);
            }
        }
    }
}
