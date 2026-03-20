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

using System.Threading.Tasks;
using Avalonia;
using Size = System.Drawing.Size;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Layout;

namespace SimPe.Plugin
{
    /// <summary>
    /// Summary description for ImageSize.
    /// </summary>
    public class ImageSize : Window
    {
        internal TextBox tbheight;
        internal TextBox tbwidth;

        ImageSize()
        {
            Title = "Image Size";
            Width = 200;
            Height = 100;
            CanResize = false;
            WindowStartupLocation = WindowStartupLocation.CenterOwner;

            tbwidth  = new TextBox { Width = 56 };
            tbheight = new TextBox { Width = 56 };

            var label4 = new TextBlock { Text = "Size:", FontWeight = Avalonia.Media.FontWeight.Bold, VerticalAlignment = VerticalAlignment.Center };
            var label5 = new TextBlock { Text = "x",    FontWeight = Avalonia.Media.FontWeight.Bold, VerticalAlignment = VerticalAlignment.Center };

            var button1 = new Button { Content = "OK", HorizontalAlignment = HorizontalAlignment.Right };
            button1.Click += button1_Click;

            var row1 = new StackPanel
            {
                Orientation = Avalonia.Layout.Orientation.Horizontal,
                Spacing = 8,
                Margin = new Thickness(8),
                Children = { label4, tbwidth, label5, tbheight }
            };

            var root = new StackPanel
            {
                Margin = new Thickness(8),
                Spacing = 8,
                Children = { row1, button1 }
            };

            Content = root;
        }

        public static async Task<Size> Execute(Size sz)
        {
            ImageSize f = new ImageSize();
            f.tbheight.Text = sz.Height.ToString();
            f.tbwidth.Text  = sz.Width.ToString();

            await f.ShowDialog(SimPe.RemoteControl.ApplicationForm);

            return new Size(
                Helper.StringToInt32(f.tbwidth.Text,  sz.Width,  10),
                Helper.StringToInt32(f.tbheight.Text, sz.Height, 10));
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
