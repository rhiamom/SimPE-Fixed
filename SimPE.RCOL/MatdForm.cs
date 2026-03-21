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
using Avalonia.Controls;
using Avalonia.Interactivity;
using SimPe.Interfaces.Plugin;

namespace SimPe.Plugin
{
	/// <summary>
	/// This Form is no longer used...
	/// Avalonia UserControl replacement for MatdForm (was WinForms Form).
	/// </summary>
	public class MatdForm : Avalonia.Controls.UserControl
	{
		// tbname is used as a reentrancy guard (Tag property acts as a flag)
		private Avalonia.Controls.TextBox tbname_guard;

		// Externally-accessed fields
		internal Avalonia.Controls.TextBox tbdsc;
		internal Avalonia.Controls.TextBox tbtype;
		internal Avalonia.Controls.TextBox tb_ver;
		internal Avalonia.Controls.TabItem tabPage3;

		public MatdForm()
		{
			tbname_guard = new Avalonia.Controls.TextBox();
			BuildLayout();
		}

		private void BuildLayout()
		{
			// --- Labels ---
			var label28 = new Avalonia.Controls.TextBlock { Text = "Version:" };
			var label4  = new Avalonia.Controls.TextBlock { Text = "Description:" };
			var label5  = new Avalonia.Controls.TextBlock { Text = "Type:" };

			// --- TextBoxes ---
			tb_ver = new Avalonia.Controls.TextBox { Text = "0x00000000" };
			tb_ver.TextChanged += FileNameChanged;

			tbdsc = new Avalonia.Controls.TextBox { Text = "" };
			tbdsc.TextChanged += FileNameChanged;

			tbtype = new Avalonia.Controls.TextBox { Text = "" };
			tbtype.TextChanged += FileNameChanged;

			// --- Settings group (was groupBox10) ---
			var settingsGrid = new Avalonia.Controls.Grid();
			settingsGrid.ColumnDefinitions.Add(new Avalonia.Controls.ColumnDefinition(Avalonia.Controls.GridLength.Auto));
			settingsGrid.ColumnDefinitions.Add(new Avalonia.Controls.ColumnDefinition(new Avalonia.Controls.GridLength(1, Avalonia.Controls.GridUnitType.Star)));
			settingsGrid.RowDefinitions.Add(new Avalonia.Controls.RowDefinition(Avalonia.Controls.GridLength.Auto));
			settingsGrid.RowDefinitions.Add(new Avalonia.Controls.RowDefinition(Avalonia.Controls.GridLength.Auto));
			settingsGrid.RowDefinitions.Add(new Avalonia.Controls.RowDefinition(Avalonia.Controls.GridLength.Auto));

			Avalonia.Controls.Grid.SetRow(label28, 0); Avalonia.Controls.Grid.SetColumn(label28, 0);
			Avalonia.Controls.Grid.SetRow(tb_ver,  0); Avalonia.Controls.Grid.SetColumn(tb_ver,  1);
			Avalonia.Controls.Grid.SetRow(label4,  1); Avalonia.Controls.Grid.SetColumn(label4,  0);
			Avalonia.Controls.Grid.SetRow(tbdsc,   1); Avalonia.Controls.Grid.SetColumn(tbdsc,   1);
			Avalonia.Controls.Grid.SetRow(label5,  2); Avalonia.Controls.Grid.SetColumn(label5,  0);
			Avalonia.Controls.Grid.SetRow(tbtype,  2); Avalonia.Controls.Grid.SetColumn(tbtype,  1);

			settingsGrid.Children.Add(label28);
			settingsGrid.Children.Add(tb_ver);
			settingsGrid.Children.Add(label4);
			settingsGrid.Children.Add(tbdsc);
			settingsGrid.Children.Add(label5);
			settingsGrid.Children.Add(tbtype);

			var settingsBorder = new Avalonia.Controls.Border
			{
				BorderThickness = new Avalonia.Thickness(1),
				Padding = new Avalonia.Thickness(4),
				Child = settingsGrid
			};

			// --- TabItem (was tabPage3) ---
			tabPage3 = new Avalonia.Controls.TabItem
			{
				Header = "cMeterialDefinition",
				Content = settingsBorder
			};

			// --- TabControl ---
			var tabControl1 = new Avalonia.Controls.TabControl();
			tabControl1.Items.Add(tabPage3);

			Content = tabControl1;
		}

		private void FileNameChanged(object sender, Avalonia.Controls.TextChangedEventArgs e)
		{
			if (this.tabPage3.Tag == null) return;
			if (tbname_guard.Tag != null) return;
			try
			{
				tbname_guard.Tag = true;
				MaterialDefinition md = (MaterialDefinition)this.tabPage3.Tag;

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
				tbname_guard.Tag = null;
			}
		}

		private void linkLabel1_Click(object sender, Avalonia.Interactivity.RoutedEventArgs e)
		{
			if (this.tabPage3.Tag == null) return;

			MaterialDefinition md = (MaterialDefinition)this.tabPage3.Tag;
			md.Sort();
			md.Refresh();
		}
	}
}
