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

namespace SimPe.Plugin
{
    /// <summary>
    /// Summary description for StringsForm.
    /// </summary>
    public class StringsForm : Avalonia.Controls.Window
    {
        private Avalonia.Controls.Panel GradientPanel = new Avalonia.Controls.Panel();
        private Avalonia.Controls.Label label1 = new Avalonia.Controls.Label();
        private Avalonia.Controls.Label label2 = new Avalonia.Controls.Label();
        private Avalonia.Controls.Label label3 = new Avalonia.Controls.Label();
        private Avalonia.Controls.Label label4 = new Avalonia.Controls.Label();
        private Avalonia.Controls.Label label5 = new Avalonia.Controls.Label();
        private Avalonia.Controls.Button button1 = new Avalonia.Controls.Button();
        internal Avalonia.Controls.TextBox tbMMAT = new Avalonia.Controls.TextBox();
        internal Avalonia.Controls.TextBox tbCreator = new Avalonia.Controls.TextBox();
        internal Avalonia.Controls.TextBox tbLicense = new Avalonia.Controls.TextBox();
        internal Avalonia.Controls.TextBox tbDate = new Avalonia.Controls.TextBox();
        internal Avalonia.Controls.TextBox tbVersion = new Avalonia.Controls.TextBox();
        ThemeManager tm;

        public StringsForm()
        {
            Title = "Copyright Text";
            Width = 700;
            SizeToContent = Avalonia.Controls.SizeToContent.Height;

            label1.Content = "MMAT Text:";
            label2.Content = "Created by:";
            label3.Content = "License:";
            label4.Content = "Release Date:";
            label5.Content = "Version:";
            button1.Content = "OK";
            button1.Click += (s, e) => Close();

            tbMMAT.Text = "Created for CEP Extra";
            tbCreator.Text = "Anonymous";
            tbLicense.Text = "This File was created as Part of a ColourEnabler Extra Package  If you payed for a package that contains this File please report it.";
            tbDate.Text = DateTime.Now.ToString();
            tbVersion.Text = "CEP Extra";

            if (Helper.XmlRegistry.Username.Trim() != "")
                tbCreator.Text = Helper.XmlRegistry.Username;

            void addRow(Avalonia.Controls.Label lbl, Avalonia.Controls.TextBox tb, Avalonia.Controls.StackPanel parent)
            {
                var row = new Avalonia.Controls.StackPanel
                {
                    Orientation = Avalonia.Layout.Orientation.Horizontal,
                    Margin = new Avalonia.Thickness(4, 2),
                    Spacing = 8
                };
                lbl.Width = 120;
                lbl.VerticalAlignment = Avalonia.Layout.VerticalAlignment.Center;
                tb.MinWidth = 400;
                row.Children.Add(lbl);
                row.Children.Add(tb);
                parent.Children.Add(row);
            }

            var stack = new Avalonia.Controls.StackPanel { Margin = new Avalonia.Thickness(8) };
            addRow(label1, tbMMAT, stack);
            addRow(label2, tbCreator, stack);
            addRow(label3, tbLicense, stack);
            addRow(label4, tbDate, stack);
            addRow(label5, tbVersion, stack);

            var btnRow = new Avalonia.Controls.StackPanel
            {
                Orientation = Avalonia.Layout.Orientation.Horizontal,
                HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Right,
                Margin = new Avalonia.Thickness(4, 8)
            };
            btnRow.Children.Add(button1);
            stack.Children.Add(btnRow);

            GradientPanel.Children.Add(stack);
            Content = GradientPanel;

            tm = ThemeManager.Global.CreateChild();
            tm.AddControl(GradientPanel);
        }
    }
}
