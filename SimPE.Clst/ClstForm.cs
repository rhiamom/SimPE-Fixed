/***************************************************************************
 *   Copyright (C) 2005 by Peter L Jones                                   *
 *   pljones@users.sf.net                                                  *
 *                                                                         *
 *   Copyright (C) 2005 by Ambertation                                     *
 *   quaxi@ambertation.de                                                  *
 *                                                                         *
 *   Copyright (C) 2025 by GramzeSweatShop                                 *
 *   rhiamom@mac.com                                                       *
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
using Avalonia.Layout;
using Avalonia.Media;
using SimPe.Interfaces.Plugin;
using SimPe.PackedFiles.Wrapper;

namespace SimPe.PackedFiles.UserInterface
{
    /// <summary>
    /// Displays a CompressedFileList (CLST) resource.
    /// Ported from WinForms Form to Avalonia UserControl.
    /// </summary>
    public class ClstForm : UserControl, IPackedFileUI
    {
        private TextBlock lbformat;
        private ListBox   lbclst;
        private CompressedFileList wrapper;

        public ClstForm()
        {
            var grid = new Grid();
            grid.RowDefinitions.Add(new RowDefinition(30, GridUnitType.Pixel));
            grid.RowDefinitions.Add(new RowDefinition(GridLength.Auto));
            grid.RowDefinitions.Add(new RowDefinition(GridLength.Star));

            var header = new Border
            {
                Background = new SolidColorBrush(Colors.Gray),
                Height     = 30
            };
            Grid.SetRow(header, 0);

            lbformat = new TextBlock();
            var formatRow = new StackPanel
            {
                Orientation = Orientation.Horizontal,
                Margin      = new Thickness(10, 4, 0, 4)
            };
            formatRow.Children.Add(new TextBlock { Text = "Format: " });
            formatRow.Children.Add(lbformat);
            Grid.SetRow(formatRow, 1);

            lbclst = new ListBox();
            Grid.SetRow(lbclst, 2);

            if (Helper.XmlRegistry.UseBigIcons)
                lbclst.FontSize = 14;

            grid.Children.Add(header);
            grid.Children.Add(formatRow);
            grid.Children.Add(lbclst);

            Content = grid;
        }

        // IPackedFileUI
        public Avalonia.Controls.Control GUIHandle => this;

        public void UpdateGUI(IFileWrapper wrp)
        {
            wrapper = (CompressedFileList)wrp;
            lbformat.Text = wrapper.IndexType.ToString();
            lbclst.Items.Clear();
            foreach (ClstItem i in wrapper.Items)
                lbclst.Items.Add(i != null ? (object)i : "Error");
        }

        public void Dispose() { }
    }
}
