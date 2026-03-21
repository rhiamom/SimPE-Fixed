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

namespace SimPe.Plugin
{
	/// <summary>
	/// This Form is no longer Used.
	/// Avalonia UserControl replacement for fExtension (was WinForms Form).
	/// </summary>
	public class fExtension : Avalonia.Controls.UserControl
	{
		// Externally-accessed fields
		internal Avalonia.Controls.TabItem tExtension;
		internal Avalonia.Controls.TextBox tb_ver;
		internal Avalonia.Controls.TextBox tb_type;
		internal Avalonia.Controls.TextBox tb_name;
		internal Avalonia.Controls.ListBox lb_items;
		internal Avalonia.Controls.Border gbIems;  // was GroupBox, used as a container with .Tag and Parent

		// Value-type group borders (were GroupBoxes, visibility toggled)
		private Avalonia.Controls.Border gbval;
		private Avalonia.Controls.Border gbtrans;
		private Avalonia.Controls.Border gbrot;
		private Avalonia.Controls.Border gbbin;
		private Avalonia.Controls.Border gbstr;
		private Avalonia.Controls.Border gbar;
		private Avalonia.Controls.Border gbfloat;

		// Fields inside the groups
		private Avalonia.Controls.TextBox tbval;
		private Avalonia.Controls.TextBox tbstr;
		private Avalonia.Controls.TextBox tbbin;
		private Avalonia.Controls.TextBox tbtrans1;
		private Avalonia.Controls.TextBox tbtrans2;
		private Avalonia.Controls.TextBox tbtrans3;
		private Avalonia.Controls.TextBox tbrot1;
		private Avalonia.Controls.TextBox tbrot2;
		private Avalonia.Controls.TextBox tbrot3;
		private Avalonia.Controls.TextBox tbrot4;
		private Avalonia.Controls.TextBox tbfloat;
		private Avalonia.Controls.Button btedit;
		private Avalonia.Controls.ComboBox cbtype;
		private Avalonia.Controls.Button linkLabel1;  // was LinkLabel "add"
		private Avalonia.Controls.Button lldel;       // was LinkLabel "delete"
		private Avalonia.Controls.TextBox tb_itemname;

		// Right-side panel that swaps visible group
		private Avalonia.Controls.Panel detailPanel;

		public fExtension()
		{
			BuildLayout();
			HideAll();

			cbtype.Items.Add(ExtensionItem.ItemTypes.Array);
			cbtype.Items.Add(ExtensionItem.ItemTypes.Binary);
			cbtype.Items.Add(ExtensionItem.ItemTypes.Float);
			cbtype.Items.Add(ExtensionItem.ItemTypes.Rotation);
			cbtype.Items.Add(ExtensionItem.ItemTypes.String);
			cbtype.Items.Add(ExtensionItem.ItemTypes.Translation);
			cbtype.Items.Add(ExtensionItem.ItemTypes.Value);
			cbtype.SelectedIndex = 0;
		}

		private void BuildLayout()
		{
			// ---- Settings group (top-left, was groupBox10) ----
			var label28 = new TextBlock { Text = "Version:" };
			tb_ver   = new TextBox { Text = "0x00000000" };
			tb_ver.TextChanged += GNSettingsChange;

			var label1 = new TextBlock { Text = "Typecode:" };
			tb_type  = new TextBox { Text = "0x00" };
			tb_type.TextChanged += GNSettingsChange;

			var label2 = new TextBlock { Text = "Name" };
			tb_name  = new TextBox { Text = "0x00" };
			tb_name.TextChanged += GNSettingsChange;

			var settingsGrid = new Grid();
			settingsGrid.ColumnDefinitions.Add(new ColumnDefinition(GridLength.Auto));
			settingsGrid.ColumnDefinitions.Add(new ColumnDefinition(new GridLength(1, GridUnitType.Star)));
			settingsGrid.RowDefinitions.Add(new RowDefinition(GridLength.Auto));
			settingsGrid.RowDefinitions.Add(new RowDefinition(GridLength.Auto));
			settingsGrid.RowDefinitions.Add(new RowDefinition(GridLength.Auto));

			Grid.SetRow(label28, 0); Grid.SetColumn(label28, 0);
			Grid.SetRow(tb_ver,  0); Grid.SetColumn(tb_ver,  1);
			Grid.SetRow(label1,  1); Grid.SetColumn(label1,  0);
			Grid.SetRow(tb_type, 1); Grid.SetColumn(tb_type, 1);
			Grid.SetRow(label2,  2); Grid.SetColumn(label2,  0);
			Grid.SetRow(tb_name, 2); Grid.SetColumn(tb_name, 1);

			settingsGrid.Children.Add(label28);
			settingsGrid.Children.Add(tb_ver);
			settingsGrid.Children.Add(label1);
			settingsGrid.Children.Add(tb_type);
			settingsGrid.Children.Add(label2);
			settingsGrid.Children.Add(tb_name);

			var settingsBorder = new Border
			{
				BorderThickness = new Avalonia.Thickness(1),
				Padding = new Avalonia.Thickness(4),
				Child = settingsGrid
			};

			// ---- Value-type detail groups (right side, only one visible at a time) ----

			// gbval
			tbval = new TextBox { Text = "0x00000000" };
			tbval.TextChanged += ValChange;
			gbval = MakeGroup("Value", tbval);

			// gbfloat
			tbfloat = new TextBox { Text = "0" };
			tbfloat.TextChanged += FloatChange;
			gbfloat = MakeGroup("Float Value", tbfloat);

			// gbtrans
			tbtrans1 = new TextBox { Text = "0x00000000" };
			tbtrans2 = new TextBox { Text = "0x00000000" };
			tbtrans3 = new TextBox { Text = "0x00000000" };
			tbtrans1.TextChanged += TransChange;
			tbtrans2.TextChanged += TransChange;
			tbtrans3.TextChanged += TransChange;
			var transPanel = new StackPanel { Spacing = 2 };
			transPanel.Children.Add(tbtrans1);
			transPanel.Children.Add(tbtrans2);
			transPanel.Children.Add(tbtrans3);
			gbtrans = MakeGroup("Translation", transPanel);

			// gbrot
			tbrot1 = new TextBox { Text = "0x00000000" };
			tbrot2 = new TextBox { Text = "0x00000000" };
			tbrot3 = new TextBox { Text = "0x00000000" };
			tbrot4 = new TextBox { Text = "0x00000000" };
			tbrot1.TextChanged += RotChange;
			tbrot2.TextChanged += RotChange;
			tbrot3.TextChanged += RotChange;
			tbrot4.TextChanged += RotChange;
			var rotPanel = new StackPanel { Spacing = 2 };
			rotPanel.Children.Add(tbrot1);
			rotPanel.Children.Add(tbrot2);
			rotPanel.Children.Add(tbrot3);
			rotPanel.Children.Add(tbrot4);
			gbrot = MakeGroup("Rotation", rotPanel);

			// gbbin
			tbbin = new TextBox { Text = "", AcceptsReturn = true };
			tbbin.TextChanged += BinChange;
			gbbin = MakeGroup("Binary", tbbin);

			// gbstr
			tbstr = new TextBox { Text = "" };
			tbstr.TextChanged += StrChange;
			gbstr = MakeGroup("String", tbstr);

			// gbar (Array — has Edit button)
			btedit = new Button { Content = "Edit" };
			btedit.Click += OpenArrayEditor;
			gbar = MakeGroup("Array", btedit);

			// Detail panel: holds all value groups stacked; visibility controls which shows
			detailPanel = new Panel();
			detailPanel.Children.Add(gbval);
			detailPanel.Children.Add(gbfloat);
			detailPanel.Children.Add(gbtrans);
			detailPanel.Children.Add(gbrot);
			detailPanel.Children.Add(gbbin);
			detailPanel.Children.Add(gbstr);
			detailPanel.Children.Add(gbar);

			// ---- Item name + add/delete row ----
			var label3 = new TextBlock { Text = "Name:" };
			tb_itemname = new TextBox { Text = "" };
			tb_itemname.TextChanged += ChangeName;

			cbtype = new ComboBox();

			linkLabel1 = new Button { Content = "add" };
			linkLabel1.Click += AddItem;

			lldel = new Button { Content = "delete" };
			lldel.Click += DeleteItem;

			var nameRow = new StackPanel { Orientation = Avalonia.Layout.Orientation.Horizontal, Spacing = 4 };
			nameRow.Children.Add(label3);
			nameRow.Children.Add(tb_itemname);

			var buttonRow = new StackPanel { Orientation = Avalonia.Layout.Orientation.Horizontal, Spacing = 4 };
			buttonRow.Children.Add(cbtype);
			buttonRow.Children.Add(linkLabel1);
			buttonRow.Children.Add(lldel);

			// lb_items
			lb_items = new ListBox();
			lb_items.SelectionChanged += SelectItem;

			// ---- Items group (was gbIems) — left list + right details ----
			var itemsMainGrid = new Grid();
			itemsMainGrid.ColumnDefinitions.Add(new ColumnDefinition(new GridLength(200)));
			itemsMainGrid.ColumnDefinitions.Add(new ColumnDefinition(new GridLength(1, GridUnitType.Star)));
			itemsMainGrid.RowDefinitions.Add(new RowDefinition(new GridLength(1, GridUnitType.Star)));
			itemsMainGrid.RowDefinitions.Add(new RowDefinition(GridLength.Auto));
			itemsMainGrid.RowDefinitions.Add(new RowDefinition(GridLength.Auto));

			Grid.SetRow(lb_items, 0); Grid.SetColumn(lb_items, 0); Grid.SetRowSpan(lb_items, 3);
			Grid.SetRow(nameRow, 0);  Grid.SetColumn(nameRow, 1);
			Grid.SetRow(detailPanel, 1); Grid.SetColumn(detailPanel, 1);
			Grid.SetRow(buttonRow, 2); Grid.SetColumn(buttonRow, 1);

			itemsMainGrid.Children.Add(lb_items);
			itemsMainGrid.Children.Add(nameRow);
			itemsMainGrid.Children.Add(detailPanel);
			itemsMainGrid.Children.Add(buttonRow);

			// gbIems is a Border wrapping the items grid (externally accessed for .Tag)
			gbIems = new Border
			{
				BorderThickness = new Avalonia.Thickness(1),
				Padding = new Avalonia.Thickness(4),
				Child = itemsMainGrid
			};

			// ---- Top-level tab content: settings left, items right ----
			var topGrid = new Grid();
			topGrid.ColumnDefinitions.Add(new ColumnDefinition(new GridLength(240)));
			topGrid.ColumnDefinitions.Add(new ColumnDefinition(new GridLength(1, GridUnitType.Star)));

			Grid.SetColumn(settingsBorder, 0);
			Grid.SetColumn(gbIems, 1);

			topGrid.Children.Add(settingsBorder);
			topGrid.Children.Add(gbIems);

			// ---- tExtension TabItem ----
			tExtension = new TabItem
			{
				Header = "Extension",
				Content = topGrid
			};

			var tabControl1 = new TabControl();
			tabControl1.Items.Add(tExtension);

			Content = tabControl1;
		}

		private static Border MakeGroup(string header, Avalonia.Controls.Control child)
		{
			var sp = new StackPanel { Spacing = 2 };
			sp.Children.Add(new TextBlock { Text = header, FontWeight = Avalonia.Media.FontWeight.Bold });
			sp.Children.Add(child);
			return new Border
			{
				BorderThickness = new Avalonia.Thickness(1),
				Padding = new Avalonia.Thickness(4),
				Child = sp
			};
		}

		internal void HideAll()
		{
			gbval.IsVisible   = false;
			gbar.IsVisible    = false;
			gbfloat.IsVisible = false;
			gbbin.IsVisible   = false;
			gbrot.IsVisible   = false;
			gbstr.IsVisible   = false;
			gbtrans.IsVisible = false;
		}

		internal void ShowGroup(Border gb)
		{
			gb.IsVisible = true;
		}

		private void GNSettingsChange(object sender, TextChangedEventArgs e)
		{
			if (tExtension.Tag == null) return;
			try
			{
				Extension ext = (Extension)tExtension.Tag;

				ext.Version  = Convert.ToUInt32(tb_ver.Text, 16);
				ext.VarName  = tb_name.Text;
				ext.TypeCode = Convert.ToByte(tb_type.Text, 16);
				ext.Changed  = true;
				lldel.IsEnabled = false;
			}
			catch (Exception)
			{
				//Helper.ExceptionMessage("", ex);
			}
		}

		private void SelectItem(object sender, SelectionChangedEventArgs e)
		{
			if (tb_itemname.Tag != null) return;
			HideAll();
			if (lb_items.SelectedIndex < 0) return;
			lldel.IsEnabled = true;
			try
			{
				tb_itemname.Tag = true;
				ExtensionItem ei = (ExtensionItem)lb_items.Items[lb_items.SelectedIndex];
				tb_itemname.Text = ei.Name;

				switch (ei.Typecode)
				{
					case ExtensionItem.ItemTypes.Value:
					{
						tbval.Text = "0x" + Helper.HexString((uint)ei.Value);
						ShowGroup(gbval);
						break;
					}
					case ExtensionItem.ItemTypes.Float:
					{
						tbfloat.Text = ei.Single.ToString();
						ShowGroup(gbfloat);
						break;
					}
					case ExtensionItem.ItemTypes.Translation:
					{
						tbtrans1.Text = ei.Translation.X.ToString("N6");
						tbtrans2.Text = ei.Translation.Y.ToString("N6");
						tbtrans3.Text = ei.Translation.Z.ToString("N6");
						ShowGroup(gbtrans);
						break;
					}
					case ExtensionItem.ItemTypes.String:
					{
						tbstr.Text = ei.String;
						ShowGroup(gbstr);
						break;
					}
					case ExtensionItem.ItemTypes.Rotation:
					{
						tbrot1.Text = ei.Rotation.X.ToString("N6");
						tbrot2.Text = ei.Rotation.Y.ToString("N6");
						tbrot3.Text = ei.Rotation.Z.ToString("N6");
						tbrot4.Text = ei.Rotation.W.ToString("N6");
						ShowGroup(gbrot);
						break;
					}
					case ExtensionItem.ItemTypes.Binary:
					{
						tbbin.Text = Helper.BytesToHexList(ei.Data);
						ShowGroup(gbbin);
						break;
					}
					case ExtensionItem.ItemTypes.Array:
					{
						ShowGroup(gbar);
						break;
					}
				}
			}
			catch (Exception ex)
			{
				Helper.ExceptionMessage("", ex);
			}
			finally
			{
				tb_itemname.Tag = null;
			}
		}

		private void OpenArrayEditor(object sender, RoutedEventArgs e)
		{
			try
			{
				fExtension fe = new fExtension();

				ExtensionItem ei = (ExtensionItem)lb_items.Items[lb_items.SelectedIndex];
				fe.gbIems.Tag = ei.Items;
				foreach (ExtensionItem i in ei.Items) fe.lb_items.Items.Add(i);

				// In Avalonia UserControl context, show-dialog is not available.
				// The caller is responsible for hosting this UserControl in a window/panel.
				// Inline editing: apply changes immediately via gbIems.Tag
				ei.Items = (ExtensionItem[])fe.gbIems.Tag;
				lb_items.Items[lb_items.SelectedIndex] = ei;
			}
			catch (Exception ex)
			{
				Helper.ExceptionMessage("", ex);
			}
		}

		private void DeleteItem(object sender, RoutedEventArgs e)
		{
			if (lb_items.SelectedIndex < 0) return;
			try
			{
				ExtensionItem[] list = (ExtensionItem[])gbIems.Tag;
				ExtensionItem ei = (ExtensionItem)lb_items.Items[lb_items.SelectedIndex];

				list = (ExtensionItem[])Helper.Delete(list, ei);
				gbIems.Tag = list;
				lb_items.Items.Remove(ei);

				if (tExtension.Tag != null)
				{
					Extension ext = (Extension)tExtension.Tag;
					ext.Items = list;
				}
			}
			catch (Exception ex)
			{
				Helper.ExceptionMessage("", ex);
			}
		}

		private void AddItem(object sender, RoutedEventArgs e)
		{
			try
			{
				ExtensionItem[] list = (ExtensionItem[])gbIems.Tag;
				ExtensionItem ei = new ExtensionItem();
				ei.Typecode = (ExtensionItem.ItemTypes)cbtype.Items[cbtype.SelectedIndex];

				list = (ExtensionItem[])Helper.Add(list, ei);
				gbIems.Tag = list;
				lb_items.Items.Add(ei);

				if (tExtension.Tag != null)
				{
					Extension ext = (Extension)tExtension.Tag;
					ext.Items = list;
				}
			}
			catch (Exception ex)
			{
				Helper.ExceptionMessage("", ex);
			}
		}

		private void ChangeName(object sender, TextChangedEventArgs e)
		{
			if (tb_itemname.Tag != null) return;
			if (lb_items.SelectedIndex < 0) return;
			try
			{
				tb_itemname.Tag = true;
				ExtensionItem ei = (ExtensionItem)lb_items.Items[lb_items.SelectedIndex];
				ei.Name = tb_itemname.Text;

				lb_items.Items[lb_items.SelectedIndex] = ei;
			}
			catch (Exception ex)
			{
				Helper.ExceptionMessage("", ex);
			}
			finally
			{
				tb_itemname.Tag = null;
			}
		}

		private void ValChange(object sender, TextChangedEventArgs e)
		{
			if (tb_itemname.Tag != null) return;
			if (lb_items.SelectedIndex < 0) return;
			try
			{
				tb_itemname.Tag = true;
				ExtensionItem ei = (ExtensionItem)lb_items.Items[lb_items.SelectedIndex];
				ei.Value = (int)Convert.ToUInt32(tbval.Text, 16);

				lb_items.Items[lb_items.SelectedIndex] = ei;
			}
			catch (Exception) { }
			finally
			{
				tb_itemname.Tag = null;
			}
		}

		private void FloatChange(object sender, TextChangedEventArgs e)
		{
			if (tb_itemname.Tag != null) return;
			if (lb_items.SelectedIndex < 0) return;
			try
			{
				tb_itemname.Tag = true;
				ExtensionItem ei = (ExtensionItem)lb_items.Items[lb_items.SelectedIndex];
				ei.Single = Convert.ToSingle(tbfloat.Text);

				lb_items.Items[lb_items.SelectedIndex] = ei;
			}
			catch (Exception) { }
			finally
			{
				tb_itemname.Tag = null;
			}
		}

		private void TransChange(object sender, TextChangedEventArgs e)
		{
			if (tb_itemname.Tag != null) return;
			if (lb_items.SelectedIndex < 0) return;
			try
			{
				tb_itemname.Tag = true;
				ExtensionItem ei = (ExtensionItem)lb_items.Items[lb_items.SelectedIndex];
				ei.Translation.X = Convert.ToSingle(tbtrans1.Text);
				ei.Translation.Y = Convert.ToSingle(tbtrans2.Text);
				ei.Translation.Z = Convert.ToSingle(tbtrans3.Text);

				lb_items.Items[lb_items.SelectedIndex] = ei;
			}
			catch (Exception) { }
			finally
			{
				tb_itemname.Tag = null;
			}
		}

		private void RotChange(object sender, TextChangedEventArgs e)
		{
			if (tb_itemname.Tag != null) return;
			if (lb_items.SelectedIndex < 0) return;
			try
			{
				tb_itemname.Tag = true;
				ExtensionItem ei = (ExtensionItem)lb_items.Items[lb_items.SelectedIndex];
				ei.Rotation.X = Convert.ToSingle(tbrot1.Text);
				ei.Rotation.Y = Convert.ToSingle(tbrot2.Text);
				ei.Rotation.Z = Convert.ToSingle(tbrot3.Text);
				ei.Rotation.W = Convert.ToSingle(tbrot4.Text);

				lb_items.Items[lb_items.SelectedIndex] = ei;
			}
			catch (Exception) { }
			finally
			{
				tb_itemname.Tag = null;
			}
		}

		private void StrChange(object sender, TextChangedEventArgs e)
		{
			if (tb_itemname.Tag != null) return;
			if (lb_items.SelectedIndex < 0) return;
			try
			{
				tb_itemname.Tag = true;
				ExtensionItem ei = (ExtensionItem)lb_items.Items[lb_items.SelectedIndex];
				ei.String = tbstr.Text;

				lb_items.Items[lb_items.SelectedIndex] = ei;
			}
			catch (Exception) { }
			finally
			{
				tb_itemname.Tag = null;
			}
		}

		private void BinChange(object sender, TextChangedEventArgs e)
		{
			if (tb_itemname.Tag != null) return;
			if (lb_items.SelectedIndex < 0) return;
			try
			{
				tb_itemname.Tag = true;
				ExtensionItem ei = (ExtensionItem)lb_items.Items[lb_items.SelectedIndex];
				ei.Data = Helper.HexListToBytes(tbbin.Text);

				lb_items.Items[lb_items.SelectedIndex] = ei;
			}
			catch (Exception) { }
			finally
			{
				tb_itemname.Tag = null;
			}
		}
	}
}
