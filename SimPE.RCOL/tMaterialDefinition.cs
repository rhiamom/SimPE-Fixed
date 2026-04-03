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
	public class MaterialDefinition : Avalonia.Controls.TabItem
	{
		protected override Type StyleKeyOverride => typeof(Avalonia.Controls.TabItem);
		internal Avalonia.Controls.TextBox tbdsc;
		internal Avalonia.Controls.TextBox tbtype;
		private Avalonia.Controls.TextBlock label4;
		private Avalonia.Controls.TextBlock label5;
		internal Avalonia.Controls.TextBox tb_ver;
		private Avalonia.Controls.TextBlock label28;

		public MaterialDefinition()
		{
			this.Header = "cMeterialDefinition";
			this.FontSize = 11;

			label5  = new Avalonia.Controls.TextBlock { Text = "Type:",        VerticalAlignment = Avalonia.Layout.VerticalAlignment.Center, Margin = new Avalonia.Thickness(0, 0, 6, 0) };
			label4  = new Avalonia.Controls.TextBlock { Text = "Description:", VerticalAlignment = Avalonia.Layout.VerticalAlignment.Center, Margin = new Avalonia.Thickness(0, 0, 6, 0) };
			label28 = new Avalonia.Controls.TextBlock { Text = "Version:",     VerticalAlignment = Avalonia.Layout.VerticalAlignment.Center, Margin = new Avalonia.Thickness(0, 0, 6, 0) };

			tb_ver = new Avalonia.Controls.TextBox { Background = Avalonia.Media.Brushes.White, Text = "0x00000000", MinHeight = 0, Padding = new Avalonia.Thickness(4, 2), Width = 120 };
			tb_ver.TextChanged += new EventHandler<Avalonia.Controls.TextChangedEventArgs>(this.FileNameChanged);
			tbdsc = new Avalonia.Controls.TextBox { Background = Avalonia.Media.Brushes.White, Text = "", MinHeight = 0, Padding = new Avalonia.Thickness(4, 2) };
			tbdsc.TextChanged += new EventHandler<Avalonia.Controls.TextChangedEventArgs>(this.FileNameChanged);
			tbtype = new Avalonia.Controls.TextBox { Background = Avalonia.Media.Brushes.White, Text = "", MinHeight = 0, Padding = new Avalonia.Thickness(4, 2) };
			tbtype.TextChanged += new EventHandler<Avalonia.Controls.TextChangedEventArgs>(this.FileNameChanged);

			// Grid: label col (Auto) | textbox col (Star)
			// Row 0: Version   | tb_ver (narrow — Width=120, HAlign=Left)
			// Row 1: Description | tbdsc (full width)
			// Row 2: Type      | tbtype (full width)
			var fieldsGrid = new Avalonia.Controls.Grid { Margin = new Avalonia.Thickness(6) };
			fieldsGrid.RowDefinitions.Add(new Avalonia.Controls.RowDefinition(Avalonia.Controls.GridLength.Auto));
			fieldsGrid.RowDefinitions.Add(new Avalonia.Controls.RowDefinition(Avalonia.Controls.GridLength.Auto));
			fieldsGrid.RowDefinitions.Add(new Avalonia.Controls.RowDefinition(Avalonia.Controls.GridLength.Auto));
			fieldsGrid.ColumnDefinitions.Add(new Avalonia.Controls.ColumnDefinition(Avalonia.Controls.GridLength.Auto));
			fieldsGrid.ColumnDefinitions.Add(new Avalonia.Controls.ColumnDefinition(new Avalonia.Controls.GridLength(1, Avalonia.Controls.GridUnitType.Star)));

			tb_ver.HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Left;
			label28.Margin = new Avalonia.Thickness(0, 0, 6, 4);
			label4.Margin  = new Avalonia.Thickness(0, 4, 6, 4);
			label5.Margin  = new Avalonia.Thickness(0, 4, 6, 0);

			Avalonia.Controls.Grid.SetRow(label28, 0); Avalonia.Controls.Grid.SetColumn(label28, 0);
			Avalonia.Controls.Grid.SetRow(tb_ver,  0); Avalonia.Controls.Grid.SetColumn(tb_ver,  1);
			Avalonia.Controls.Grid.SetRow(label4,  1); Avalonia.Controls.Grid.SetColumn(label4,  0);
			Avalonia.Controls.Grid.SetRow(tbdsc,   1); Avalonia.Controls.Grid.SetColumn(tbdsc,   1);
			Avalonia.Controls.Grid.SetRow(label5,  2); Avalonia.Controls.Grid.SetColumn(label5,  0);
			Avalonia.Controls.Grid.SetRow(tbtype,  2); Avalonia.Controls.Grid.SetColumn(tbtype,  1);
			fieldsGrid.Children.Add(label28);
			fieldsGrid.Children.Add(tb_ver);
			fieldsGrid.Children.Add(label4);
			fieldsGrid.Children.Add(tbdsc);
			fieldsGrid.Children.Add(label5);
			fieldsGrid.Children.Add(tbtype);

			// "Settings" groupbox — blue-gray style matching Reference tab
			var settingsHeader = new Avalonia.Controls.Border {
				Background = new Avalonia.Media.LinearGradientBrush {
					StartPoint = new Avalonia.RelativePoint(0, 0.5, Avalonia.RelativeUnit.Relative),
					EndPoint   = new Avalonia.RelativePoint(1, 0.5, Avalonia.RelativeUnit.Relative),
					GradientStops = { new Avalonia.Media.GradientStop(Avalonia.Media.Color.FromArgb(220, 60, 60, 80), 0.0), new Avalonia.Media.GradientStop(Avalonia.Media.Color.FromArgb(200, 80, 80, 110), 1.0) }
				},
				Child = new Avalonia.Controls.TextBlock { Text = "Settings", Foreground = Avalonia.Media.Brushes.White, FontSize = 11, FontWeight = Avalonia.Media.FontWeight.SemiBold, Margin = new Avalonia.Thickness(6, 3) }
			};
			var settingsBox = new Avalonia.Controls.Border {
				VerticalAlignment   = Avalonia.Layout.VerticalAlignment.Top,
				Background          = new Avalonia.Media.SolidColorBrush(Avalonia.Media.Color.FromRgb(220, 228, 238)),
				BorderBrush         = new Avalonia.Media.SolidColorBrush(Avalonia.Media.Color.FromRgb(170, 185, 205)),
				BorderThickness     = new Avalonia.Thickness(1),
				CornerRadius        = new Avalonia.CornerRadius(3),
				Margin              = new Avalonia.Thickness(4),
				ClipToBounds        = true,
				Child = new Avalonia.Controls.StackPanel { Children = { settingsHeader, fieldsGrid } }
			};

			Content = settingsBox;
		}

		private void FileNameChanged(object sender, System.EventArgs e)
		{
			if (this.Tag==null) return;
			if (tbdsc.Tag!=null) return;
			try
			{
				tbdsc.Tag = true;
				SimPe.Plugin.MaterialDefinition md = (SimPe.Plugin.MaterialDefinition)this.Tag;

				md.Version = Convert.ToUInt32(this.tb_ver.Text, 16);
				md.FileDescription = tbdsc.Text;
				md.MatterialType = tbtype.Text;

				md.Changed = true;
			}
			catch (Exception ex)
			{
				Helper.ExceptionMessage(Localization.Manager.GetString("erropenfile"), ex);
			}

			finally
			{
				tbdsc.Tag = null;
			}
		}

		private void linkLabel1_LinkClicked(object sender, Avalonia.Interactivity.RoutedEventArgs e)
		{
			if (this.Tag==null) return;

			SimPe.Plugin.MaterialDefinition md = (SimPe.Plugin.MaterialDefinition)this.Tag;
			md.Sort();
			md.Refresh();
		}
	}
}
