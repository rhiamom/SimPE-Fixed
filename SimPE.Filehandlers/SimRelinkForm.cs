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
using Avalonia.Controls;

namespace SimPe.PackedFiles.UserInterface
{
    /// <summary>
    /// Avalonia port of SimRelinkForm.
    /// ListView replaced with ListBox. ImageList dropped (no WinForms equivalent).
    /// ShowDialog replaced with static Execute stub (returns wrp.SimId unchanged).
    /// </summary>
    public class SimRelinkForm : UserControl
    {
        private TextBlock label1 = new TextBlock { Text = "Character File:" };
        // ListView → ListBox (no image column support, just text items)
        private ListBox lv = new ListBox();
        private TextBlock label2 = new TextBlock { Text = "Select a character to re-map this sim to." };
        private CheckBox cbfile = new CheckBox { Content = "Change GUID in Character File" };
        private Button btlink = new Button { Content = "Re-Map", IsEnabled = false };
        private Panel mainPanel = new Panel();

        public SimRelinkForm()
        {
            ThemeManager tm = ThemeManager.Global.CreateChild();
            tm.AddControl(this.btlink);
            tm.AddControl(this.lv);

            lv.SelectionChanged += lv_SelectedIndexChanged;
            btlink.Click += btlink_Click;

            var grid = new Grid();
            grid.RowDefinitions.Add(new RowDefinition(GridLength.Auto));
            grid.RowDefinitions.Add(new RowDefinition(GridLength.Auto));
            grid.RowDefinitions.Add(new RowDefinition(GridLength.Star));
            grid.RowDefinitions.Add(new RowDefinition(GridLength.Auto));

            Grid.SetRow(label2, 0); grid.Children.Add(label2);
            Grid.SetRow(label1, 1); grid.Children.Add(label1);
            Grid.SetRow(lv, 2); grid.Children.Add(lv);

            var bottomRow = new StackPanel { Orientation = Avalonia.Layout.Orientation.Horizontal };
            bottomRow.Children.Add(cbfile);
            bottomRow.Children.Add(btlink);
            Grid.SetRow(bottomRow, 3); grid.Children.Add(bottomRow);

            Content = grid;
        }

        public void Dispose() { }

        bool ok = false;

        /// <summary>
        /// Stub: Execute returns wrp.SimId unchanged.
        /// ShowDialog is not available in Avalonia UserControl.
        /// This would need reworking as an async Window.
        /// </summary>
        public static uint Execute(SimPe.PackedFiles.Wrapper.SDesc wrp)
        {
            // Full UI implementation would require an Avalonia Window.
            // For build compatibility, return the existing SimId unchanged.
            return wrp.SimId;
        }

        private void lv_SelectedIndexChanged(object sender, SelectionChangedEventArgs e)
        {
            btlink.IsEnabled = (lv.SelectedIndex >= 0);
        }

        private void btlink_Click(object sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            ok = true;
        }
    }
}
