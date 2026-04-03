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
using System.Collections;
using Avalonia.Controls;
using SimPe.Interfaces.Plugin;

namespace SimPe.Plugin.TabPage
{
	/// <summary>
	/// Summary description for MatdForm.
	/// </summary>
	public class MaterialDefinitionFiles : Avalonia.Controls.TabItem
	{
		protected override Type StyleKeyOverride => typeof(Avalonia.Controls.TabItem);
		internal Avalonia.Controls.ListBox lbfl;
		private Avalonia.Controls.TextBox tblistfile;
		private Avalonia.Controls.TextBlock label6;
		private Avalonia.Controls.Button linkLabel3;
		private Avalonia.Controls.Button linkLabel4;

		public MaterialDefinitionFiles()
		{
			this.Header = "File List";
			this.FontSize = 11;

			linkLabel4 = new Avalonia.Controls.Button { Content = "add", Padding = new Avalonia.Thickness(0), Background = Avalonia.Media.Brushes.Transparent, BorderThickness = new Avalonia.Thickness(0), Foreground = new Avalonia.Media.SolidColorBrush(Avalonia.Media.Color.FromRgb(0, 80, 180)), Cursor = new Avalonia.Input.Cursor(Avalonia.Input.StandardCursorType.Hand) };
			linkLabel4.Click += new EventHandler<Avalonia.Interactivity.RoutedEventArgs>(this.Add);
			linkLabel3 = new Avalonia.Controls.Button { Content = "delete", Padding = new Avalonia.Thickness(0), Background = Avalonia.Media.Brushes.Transparent, BorderThickness = new Avalonia.Thickness(0), Foreground = new Avalonia.Media.SolidColorBrush(Avalonia.Media.Color.FromRgb(0, 80, 180)), Cursor = new Avalonia.Input.Cursor(Avalonia.Input.StandardCursorType.Hand) };
			linkLabel3.Click += new EventHandler<Avalonia.Interactivity.RoutedEventArgs>(this.Delete);

			tblistfile = new Avalonia.Controls.TextBox { Background = Avalonia.Media.Brushes.White, Text = "" };
			tblistfile.MinHeight = 0;
			tblistfile.Padding   = new Avalonia.Thickness(4, 2);
			tblistfile.TextChanged += new EventHandler<Avalonia.Controls.TextChangedEventArgs>(this.ChangeListFile);

			label6 = new Avalonia.Controls.TextBlock { Text = "Filename:" };
			label6.VerticalAlignment = Avalonia.Layout.VerticalAlignment.Center;
			label6.Margin = new Avalonia.Thickness(0, 0, 4, 0);

			lbfl = new Avalonia.Controls.ListBox();
			lbfl.SelectionChanged += new EventHandler<Avalonia.Controls.SelectionChangedEventArgs>(this.SelectListFile);

			// Compact ListBox items
			var compactTheme = new Avalonia.Styling.ControlTheme(typeof(Avalonia.Controls.ListBoxItem));
			compactTheme.Setters.Add(new Avalonia.Styling.Setter(
				Avalonia.Controls.ListBoxItem.PaddingProperty,
				new Avalonia.Thickness(4, 1)));
			lbfl.ItemContainerTheme = compactTheme;

			// Right side: "Filename:" label, textbox, then add/delete links — no groupbox (matches original)
			var addDel = new Avalonia.Controls.StackPanel {
				Orientation = Avalonia.Layout.Orientation.Horizontal,
				Spacing = 8,
				HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Right,
				Margin = new Avalonia.Thickness(0, 4, 0, 0)
			};
			addDel.Children.Add(linkLabel4);
			addDel.Children.Add(linkLabel3);

			var editor = new Avalonia.Controls.StackPanel {
				MinWidth = 200,
				Margin = new Avalonia.Thickness(6, 0, 0, 0),
				Spacing = 3,
				VerticalAlignment = Avalonia.Layout.VerticalAlignment.Top,
				Children = { label6, tblistfile, addDel }
			};

			// 2-column grid: list fills remaining space, editor auto-sizes on the right
			var grid = new Avalonia.Controls.Grid();
			grid.ColumnDefinitions.Add(new Avalonia.Controls.ColumnDefinition(
				new Avalonia.Controls.GridLength(1, Avalonia.Controls.GridUnitType.Star)));
			grid.ColumnDefinitions.Add(new Avalonia.Controls.ColumnDefinition(
				Avalonia.Controls.GridLength.Auto));
			Avalonia.Controls.Grid.SetColumn(lbfl,   0);
			Avalonia.Controls.Grid.SetColumn(editor, 1);
			grid.Children.Add(lbfl);
			grid.Children.Add(editor);

			Content = grid;
		}

		private void SelectListFile(object sender, System.EventArgs e)
		{
			if (tblistfile.Tag!=null) return;
			if (lbfl.SelectedIndex<0) return;

			try
			{
				tblistfile.Tag = true;
				tblistfile.Text = (string)lbfl.Items[lbfl.SelectedIndex];
			}
			catch (Exception) {}
			finally
			{
				tblistfile.Tag = null;
			}
		}

		private void ChangeListFile(object sender, System.EventArgs e)
		{
			if (this.Tag==null) return;
			if (tblistfile.Tag!=null) return;
			if (lbfl.SelectedIndex<0) return;

			try
			{
				tblistfile.Tag = true;
				lbfl.Items[lbfl.SelectedIndex] = tblistfile.Text;

				SimPe.Plugin.MaterialDefinition md = (SimPe.Plugin.MaterialDefinition)this.Tag;
				md.Listing[lbfl.SelectedIndex] = tblistfile.Text;

				md.Changed = true;
			}
			catch (Exception) {}
			finally
			{
				tblistfile.Tag = null;
			}
		}

		private void Delete(object sender, Avalonia.Interactivity.RoutedEventArgs e)
		{
			if (this.Tag==null) return;
			if (lbfl.SelectedIndex<0) return;
			SimPe.Plugin.MaterialDefinition md = (SimPe.Plugin.MaterialDefinition)this.Tag;
			md.Listing = (string[])Helper.Delete(md.Listing, lbfl.Items[lbfl.SelectedIndex]);

			lbfl.Items.Remove(lbfl.Items[lbfl.SelectedIndex]);

			md.Changed = true;
		}

		private void Add(object sender, Avalonia.Interactivity.RoutedEventArgs e)
		{
			if (this.Tag==null) return;
			lbfl.Items.Add(tblistfile.Text);
			lbfl.SelectedIndex = lbfl.Items.Count-1;

			SimPe.Plugin.MaterialDefinition md = (SimPe.Plugin.MaterialDefinition)this.Tag;
			md.Listing = (string[])Helper.Add(md.Listing, tblistfile.Text);

			md.Changed = true;
		}
	}
}
