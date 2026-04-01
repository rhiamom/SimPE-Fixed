/***************************************************************************
 *   Copyright (C) 2008 by Peter L Jones                                   *
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
using System.Collections.Generic;

namespace pjOBJDTool
{
    public class cOBJDChooser : Avalonia.Controls.Window
    {
        private bool _dialogAccepted = false;
        public bool DialogAccepted => _dialogAccepted;

        private pfOBJD _value = null;
        public pfOBJD Value => _value;

        private List<pfOBJD> _items = null;

        private Avalonia.Controls.Label label1 = new Avalonia.Controls.Label();
        private Avalonia.Controls.Label label2 = new Avalonia.Controls.Label();
        private Avalonia.Controls.Button btnOK = new Avalonia.Controls.Button();
        private Avalonia.Controls.Button btnCancel = new Avalonia.Controls.Button();
        private Avalonia.Controls.ListBox lbItems = new Avalonia.Controls.ListBox();

        public cOBJDChooser()
        {
            Title = "Choose OBJD";
            Width = 400;
            Height = 300;
            CanResize = false;

            label1.Content = "Select an OBJD:";
            label2.Content = "* = lead OBJD";
            btnOK.Content = "OK";
            btnCancel.Content = "Cancel";

            btnOK.Click += (s, e) => { _dialogAccepted = true; Close(); };
            btnCancel.Click += (s, e) => Close();

            lbItems.DoubleTapped += (s, e) => { _dialogAccepted = true; Close(); };
            lbItems.SelectionChanged += lbItems_SelectionChanged;

            var buttonRow = new Avalonia.Controls.StackPanel
            {
                Orientation = Avalonia.Layout.Orientation.Horizontal,
                HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Right,
                Spacing = 6,
                Margin = new Avalonia.Thickness(0, 6, 0, 0)
            };
            buttonRow.Children.Add(btnOK);
            buttonRow.Children.Add(btnCancel);

            var layout = new Avalonia.Controls.DockPanel { Margin = new Avalonia.Thickness(8) };
            Avalonia.Controls.DockPanel.SetDock(label1, Avalonia.Controls.Dock.Top);
            Avalonia.Controls.DockPanel.SetDock(label2, Avalonia.Controls.Dock.Top);
            Avalonia.Controls.DockPanel.SetDock(buttonRow, Avalonia.Controls.Dock.Bottom);
            layout.Children.Add(label1);
            layout.Children.Add(label2);
            layout.Children.Add(buttonRow);
            layout.Children.Add(lbItems);

            Content = layout;
        }

        public void Execute(List<pfOBJD> items)
        {
            _items = items;
            _value = null;
            _dialogAccepted = false;

            lbItems.Items.Clear();
            foreach (pfOBJD item in items)
            {
                lbItems.Items.Add((IsLead(item) ? "* " : "   ") + item.Filename);
                if (IsLead(item)) lbItems.SelectedIndex = lbItems.Items.Count - 1;
            }

            ShowDialog(null).GetAwaiter().GetResult();
        }

        bool IsLead(pfOBJD item)
        {
            return (item[0x0a] == 0 || (item[0x0a] > 0 && (short)item[0x0b] < 0));
        }

        private void lbItems_SelectionChanged(object sender, Avalonia.Controls.SelectionChangedEventArgs e)
        {
            if (lbItems.SelectedIndex >= 0 && _items != null)
                _value = _items[lbItems.SelectedIndex];
        }
    }
}
