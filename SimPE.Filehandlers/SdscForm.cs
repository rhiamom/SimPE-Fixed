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
using Avalonia.Controls;

namespace SimPe.PackedFiles.UserInterface
{
    /// <summary>
    /// Minimal Sim Description panel for testing SDSC logic.
    /// Avalonia port of the original WinForms SdscPanel.
    /// </summary>
    public class SdscPanel : UserControl
    {
        // BASIC SIM FIELDS
        internal TextBox tbsim = new TextBox();
        internal TextBox tbsimdescname = new TextBox();
        internal TextBox tbsimdescfamname = new TextBox();
        internal TextBox tbfaminst = new TextBox();

        // GENDER
        internal RadioButton rbFemale = new RadioButton { Content = "Female", GroupName = "gender" };
        internal RadioButton rbMale = new RadioButton { Content = "Male", GroupName = "gender" };

        // LIFE SECTION / AGE
        internal ComboBox cblifesection = new ComboBox();
        internal TextBox tbagedur = new TextBox();

        // SPECIES + ASPIRATION
        internal ComboBox cbspecies = new ComboBox();
        internal ComboBox cbaspiration = new ComboBox();

        // Commit button
        internal Button btcommit = new Button { Content = "Commit" };

        public SdscPanel()
        {
            var grid = new Grid();
            grid.ColumnDefinitions.Add(new ColumnDefinition(GridLength.Auto));
            grid.ColumnDefinitions.Add(new ColumnDefinition(GridLength.Star));

            int row = 0;
            void AddRow(string label, Control ctrl)
            {
                grid.RowDefinitions.Add(new RowDefinition(GridLength.Auto));
                var lbl = new TextBlock { Text = label };
                Grid.SetRow(lbl, row); Grid.SetColumn(lbl, 0); grid.Children.Add(lbl);
                Grid.SetRow(ctrl, row); Grid.SetColumn(ctrl, 1); grid.Children.Add(ctrl);
                row++;
            }

            AddRow("Sim ID:", tbsim);
            AddRow("First name:", tbsimdescname);
            AddRow("Last name:", tbsimdescfamname);
            AddRow("Family instance:", tbfaminst);

            // Gender row
            grid.RowDefinitions.Add(new RowDefinition(GridLength.Auto));
            var genderLbl = new TextBlock { Text = "Gender:" };
            var genderPanel = new StackPanel { Orientation = Avalonia.Layout.Orientation.Horizontal };
            genderPanel.Children.Add(rbFemale);
            genderPanel.Children.Add(rbMale);
            Grid.SetRow(genderLbl, row); Grid.SetColumn(genderLbl, 0); grid.Children.Add(genderLbl);
            Grid.SetRow(genderPanel, row); Grid.SetColumn(genderPanel, 1); grid.Children.Add(genderPanel);
            row++;

            AddRow("Life section:", cblifesection);
            AddRow("Remaining days:", tbagedur);
            AddRow("Species:", cbspecies);
            AddRow("Aspiration:", cbaspiration);
            AddRow("", btcommit);

            Content = grid;
        }

        public void Dispose() { }
    }
}
