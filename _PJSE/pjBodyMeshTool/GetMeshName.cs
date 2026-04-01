/***************************************************************************
 *   Copyright (C) 2005 by Peter L Jones                                   *
 *   peter@users.sf.net                                                    *
 *                                                                         *
 *   Copyright (C) 2025 by GramzeSweatShop                                 *
 *   Rhiamom@mac.com                                                       *
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
using Avalonia.Controls;

namespace pj
{
    public class GetMeshName : Avalonia.Controls.Window
    {
        private bool _dialogAccepted = false;
        private bool _browsedForPackage = false;

        public bool DialogAccepted => _dialogAccepted;
        public bool BrowsedForPackage => _browsedForPackage;

        private Avalonia.Controls.TextBox tbMeshName;
        private Avalonia.Controls.CheckBox cbusecres;
        private Avalonia.Controls.TextBlock label1;

        public GetMeshName()
        {
            InitializeComponent();

            if (SimPe.Helper.XmlRegistry.UseBigIcons)
                this.label1.FontSize = 10.25;

            this.cbusecres.IsChecked = Settings.BodyMeshExtractUseCres;
        }

        public String MeshName => tbMeshName.Text;

        private void cbusecres_CheckedChanged(object sender, EventArgs e)
        {
            Settings.BodyMeshExtractUseCres = this.cbusecres.IsChecked == true;
        }

        private void InitializeComponent()
        {
            this.Title = "Sim Mesh Extractor";
            this.Width = 450;
            this.CanResize = false;
            this.WindowStartupLocation = Avalonia.Controls.WindowStartupLocation.CenterOwner;
            this.ShowInTaskbar = false;

            this.label1 = new Avalonia.Controls.TextBlock
            {
                Text = "This imports the four mesh-related resources into SimPe. You should either enter the base part of the mesh name (eg tfbodydressknee) or browse to the bodyshop-created package that contains a PropertySet or XMOL that contains the relevant name.",
                TextWrapping = Avalonia.Media.TextWrapping.Wrap,
                Margin = new Avalonia.Thickness(8, 8, 8, 4)
            };

            var label2 = new Avalonia.Controls.Label
            {
                Content = "Base mesh name",
                FontWeight = Avalonia.Media.FontWeight.Bold,
                VerticalContentAlignment = Avalonia.Layout.VerticalAlignment.Center,
                Margin = new Avalonia.Thickness(8, 0, 4, 0)
            };

            this.tbMeshName = new Avalonia.Controls.TextBox
            {
                Width = 200,
                Margin = new Avalonia.Thickness(0, 0, 4, 0)
            };

            var btnOK = new Avalonia.Controls.Button
            {
                Content = "OK",
                Margin = new Avalonia.Thickness(4, 0, 8, 0)
            };
            btnOK.Click += (s, e) => { _dialogAccepted = true; Close(); };

            var label3 = new Avalonia.Controls.Label
            {
                Content = "Select package",
                FontWeight = Avalonia.Media.FontWeight.Bold,
                VerticalContentAlignment = Avalonia.Layout.VerticalAlignment.Center,
                Margin = new Avalonia.Thickness(8, 0, 4, 0)
            };

            var btnBrowse = new Avalonia.Controls.Button
            {
                Content = "Browse...",
                Margin = new Avalonia.Thickness(0, 0, 4, 0)
            };
            btnBrowse.Click += (s, e) => { _browsedForPackage = true; Close(); };

            this.cbusecres = new Avalonia.Controls.CheckBox
            {
                Content = "Try a 3ID Resource",
                Margin = new Avalonia.Thickness(4, 0, 4, 0)
            };
            this.cbusecres.IsCheckedChanged += (s, e) => cbusecres_CheckedChanged(s, e);

            var btnCancel = new Avalonia.Controls.Button
            {
                Content = "Cancel",
                Margin = new Avalonia.Thickness(4, 0, 8, 0)
            };
            btnCancel.Click += (s, e) => Close();

            var row1 = new Avalonia.Controls.StackPanel
            {
                Orientation = Avalonia.Layout.Orientation.Horizontal,
                Margin = new Avalonia.Thickness(0, 4, 0, 4)
            };
            row1.Children.Add(label2);
            row1.Children.Add(this.tbMeshName);
            row1.Children.Add(btnOK);

            var row2 = new Avalonia.Controls.StackPanel
            {
                Orientation = Avalonia.Layout.Orientation.Horizontal,
                Margin = new Avalonia.Thickness(0, 0, 0, 8)
            };
            row2.Children.Add(label3);
            row2.Children.Add(btnBrowse);
            row2.Children.Add(this.cbusecres);
            row2.Children.Add(btnCancel);

            var mainPanel = new Avalonia.Controls.StackPanel();
            mainPanel.Children.Add(this.label1);
            mainPanel.Children.Add(row1);
            mainPanel.Children.Add(row2);

            this.Content = mainPanel;
        }
    }
}
