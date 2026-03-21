/***************************************************************************
 *   Copyright (C) 2005 by Ambertation                                     *
 *   quaxi@ambertation.de                                                  *
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
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Layout;

namespace SimPe
{
    /// <summary>
    /// Displays a drag-and-drop helper listing the files in a package.
    /// Ported from WinForms Form to Avalonia Window.
    /// </summary>
    public class PackageSelectorForm : Window
    {
        private TextBlock lbfile;
        private ListBox   lbfiles;

        public PackageSelectorForm()
        {
            Title     = "Package Selector";
            Width     = 592;
            Height    = 324;
            MinWidth  = 440;
            MinHeight = 232;
            CanResize = true;

            lbfile  = new TextBlock { Margin = new Thickness(16, 16, 0, 4), FontWeight = Avalonia.Media.FontWeight.Bold };
            lbfiles = new ListBox   { Margin = new Thickness(24, 0, 24, 0) };

            var helpText = new TextBlock
            {
                Text       = "You can use this helper to drag & drop files from the current package to a reference list.",
                Margin     = new Thickness(8, 4, 8, 8),
                TextWrapping = Avalonia.Media.TextWrapping.Wrap
            };

            var grid = new Grid();
            grid.RowDefinitions.Add(new RowDefinition(GridLength.Auto));
            grid.RowDefinitions.Add(new RowDefinition(GridLength.Star));
            grid.RowDefinitions.Add(new RowDefinition(GridLength.Auto));

            Grid.SetRow(lbfile,   0);
            Grid.SetRow(lbfiles,  1);
            Grid.SetRow(helpText, 2);

            grid.Children.Add(lbfile);
            grid.Children.Add(lbfiles);
            grid.Children.Add(helpText);

            Content = grid;

            lbfiles.PointerMoved += StartDrop;
        }

        /// <summary>
        /// Displays the tool with the content of the passed package.
        /// </summary>
        public void Execute(SimPe.Interfaces.Files.IPackageFile package)
        {
            lbfiles.Items.Clear();
            lbfile.Text = package.FileName;

            foreach (Interfaces.Files.IPackedFileDescriptor pfd in package.Index)
                lbfiles.Items.Add(pfd);

            Show();
        }

        private void StartDrop(object sender, PointerEventArgs e)
        {
            if (lbfiles.SelectedIndex < 0) return;

            if (e.GetCurrentPoint(lbfiles).Properties.IsLeftButtonPressed)
            {
                var descriptor = (Interfaces.Files.IPackedFileDescriptor)lbfiles.Items[lbfiles.SelectedIndex];
                var dataObj    = new DataObject();
                dataObj.Set(DataFormats.Text, descriptor.ToString());
                DragDrop.DoDragDrop(e, dataObj, DragDropEffects.Copy | DragDropEffects.Link);
            }
        }
    }
}
