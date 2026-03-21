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

namespace SimPe.Plugin.TabPage
{
	/// <summary>
	/// Summary description for fExtension.
	/// </summary>
	public class Extension : Avalonia.Controls.TabItem
	{
		private Avalonia.Controls.Border groupBox10;
		internal Avalonia.Controls.TextBox tb_ver;
		private Avalonia.Controls.TextBlock label28;
		internal Avalonia.Controls.TextBox tb_type;
		private Avalonia.Controls.TextBlock label1;
		internal Avalonia.Controls.TextBox tb_name;
		private Avalonia.Controls.TextBlock label2;
		internal Avalonia.Controls.ListBox lb_items;
		private Avalonia.Controls.Border gbval;
		private Avalonia.Controls.Border gbtrans;
		private Avalonia.Controls.Border gbrot;
		private Avalonia.Controls.Border gbbin;
		private Avalonia.Controls.Border gbstr;
		private Avalonia.Controls.Border gbar;
		private Avalonia.Controls.Button btedit;
		private Avalonia.Controls.Border gfootprintbar;
		private Avalonia.Controls.Button btfootprintedit;
		internal Avalonia.Controls.Border gbIems;
		internal Avalonia.Controls.TextBox tbFootprint;
		private Avalonia.Controls.ComboBox cbtype;
		private Avalonia.Controls.Button linkLabel1;
		private Avalonia.Controls.Button lldel;
		private Avalonia.Controls.TextBlock label3;
		private Avalonia.Controls.TextBox tb_itemname;
		private Avalonia.Controls.TextBox tbval;
		private Avalonia.Controls.TextBox tbstr;
		private Avalonia.Controls.TextBox tbbin;
		private Avalonia.Controls.TextBox tbtrans2;
		private Avalonia.Controls.TextBox tbtrans3;
		private Avalonia.Controls.TextBox tbtrans1;
		private Avalonia.Controls.TextBox tbrot1;
		private Avalonia.Controls.TextBox tbrot3;
		private Avalonia.Controls.TextBox tbrot2;
		private Avalonia.Controls.TextBox tbrot4;
		private Avalonia.Controls.Border gbfloat;
		private Avalonia.Controls.TextBox tbfloat;

		public Extension()
		{
			this.Header = "Extension";

			// Settings group
			label28 = new Avalonia.Controls.TextBlock { Text = "Version:" };
			tb_ver = new Avalonia.Controls.TextBox { Text = "0x00000000" };
			tb_ver.TextChanged += new EventHandler<Avalonia.Controls.TextChangedEventArgs>(this.GNSettingsChange);
			label1 = new Avalonia.Controls.TextBlock { Text = "Typecode:" };
			tb_type = new Avalonia.Controls.TextBox { Text = "0x00" };
			tb_type.TextChanged += new EventHandler<Avalonia.Controls.TextChangedEventArgs>(this.GNSettingsChange);
			label2 = new Avalonia.Controls.TextBlock { Text = "Name" };
			tb_name = new Avalonia.Controls.TextBox { Text = "0x00" };
			tb_name.TextChanged += new EventHandler<Avalonia.Controls.TextChangedEventArgs>(this.GNSettingsChange);
			groupBox10 = new Avalonia.Controls.Border();

			// Items list
			lb_items = new Avalonia.Controls.ListBox();
			lb_items.SelectionChanged += new EventHandler<Avalonia.Controls.SelectionChangedEventArgs>(this.SelectItem);
			label3 = new Avalonia.Controls.TextBlock { Text = "Name:" };
			tb_itemname = new Avalonia.Controls.TextBox();
			tb_itemname.TextChanged += new EventHandler<Avalonia.Controls.TextChangedEventArgs>(this.ChangeName);
			cbtype = new Avalonia.Controls.ComboBox();
			linkLabel1 = new Avalonia.Controls.Button { Content = "add" };
			linkLabel1.Click += new EventHandler<Avalonia.Interactivity.RoutedEventArgs>(this.AddItem);
			lldel = new Avalonia.Controls.Button { Content = "delete" };
			lldel.Click += new EventHandler<Avalonia.Interactivity.RoutedEventArgs>(this.DeleteItem);

			// Value sub-panels (shown/hidden based on selected item type)
			tbval = new Avalonia.Controls.TextBox { Text = "0x00000000" };
			tbval.TextChanged += new EventHandler<Avalonia.Controls.TextChangedEventArgs>(this.ValChange);
			gbval = new Avalonia.Controls.Border { Child = tbval };

			tbfloat = new Avalonia.Controls.TextBox { Text = "0" };
			tbfloat.TextChanged += new EventHandler<Avalonia.Controls.TextChangedEventArgs>(this.FloatChange);
			gbfloat = new Avalonia.Controls.Border { Child = tbfloat };

			tbtrans1 = new Avalonia.Controls.TextBox { Text = "0x00000000" };
			tbtrans1.TextChanged += new EventHandler<Avalonia.Controls.TextChangedEventArgs>(this.TransChange);
			tbtrans2 = new Avalonia.Controls.TextBox { Text = "0x00000000" };
			tbtrans2.TextChanged += new EventHandler<Avalonia.Controls.TextChangedEventArgs>(this.TransChange);
			tbtrans3 = new Avalonia.Controls.TextBox { Text = "0x00000000" };
			tbtrans3.TextChanged += new EventHandler<Avalonia.Controls.TextChangedEventArgs>(this.TransChange);
			gbtrans = new Avalonia.Controls.Border { Child = new Avalonia.Controls.StackPanel { Children = { tbtrans1, tbtrans2, tbtrans3 } } };

			tbrot1 = new Avalonia.Controls.TextBox { Text = "0x00000000" };
			tbrot1.TextChanged += new EventHandler<Avalonia.Controls.TextChangedEventArgs>(this.RotChange);
			tbrot2 = new Avalonia.Controls.TextBox { Text = "0x00000000" };
			tbrot2.TextChanged += new EventHandler<Avalonia.Controls.TextChangedEventArgs>(this.RotChange);
			tbrot3 = new Avalonia.Controls.TextBox { Text = "0x00000000" };
			tbrot3.TextChanged += new EventHandler<Avalonia.Controls.TextChangedEventArgs>(this.RotChange);
			tbrot4 = new Avalonia.Controls.TextBox { Text = "0x00000000" };
			tbrot4.TextChanged += new EventHandler<Avalonia.Controls.TextChangedEventArgs>(this.RotChange);
			gbrot = new Avalonia.Controls.Border { Child = new Avalonia.Controls.StackPanel { Children = { tbrot1, tbrot2, tbrot3, tbrot4 } } };

			tbstr = new Avalonia.Controls.TextBox();
			tbstr.TextChanged += new EventHandler<Avalonia.Controls.TextChangedEventArgs>(this.StrChange);
			gbstr = new Avalonia.Controls.Border { Child = tbstr };

			tbbin = new Avalonia.Controls.TextBox();
			tbbin.TextChanged += new EventHandler<Avalonia.Controls.TextChangedEventArgs>(this.BinChange);
			gbbin = new Avalonia.Controls.Border { Child = tbbin };

			btedit = new Avalonia.Controls.Button { Content = "Edit" };
			btedit.Click += new EventHandler<Avalonia.Interactivity.RoutedEventArgs>(this.OpenArrayEditor);
			gbar = new Avalonia.Controls.Border { Child = btedit };

			btfootprintedit = new Avalonia.Controls.Button { Content = "Draw" };
			btfootprintedit.Click += new EventHandler<Avalonia.Interactivity.RoutedEventArgs>(this.OpenFootprintEditor);
			tbFootprint = new Avalonia.Controls.TextBox();
			gfootprintbar = new Avalonia.Controls.Border { Child = new Avalonia.Controls.StackPanel { Children = { btfootprintedit, tbFootprint } } };

			gbIems = new Avalonia.Controls.Border { Child = new Avalonia.Controls.StackPanel { Children = {
				lb_items, label3, tb_itemname, cbtype, linkLabel1, lldel,
				gbval, gbfloat, gbtrans, gbrot, gbstr, gbbin, gbar, gfootprintbar
			}}};

			HideAll();
			cbtype.Items.Add(ExtensionItem.ItemTypes.Array);
			cbtype.Items.Add(ExtensionItem.ItemTypes.Binary);
			cbtype.Items.Add(ExtensionItem.ItemTypes.Float);
			cbtype.Items.Add(ExtensionItem.ItemTypes.Rotation);
			cbtype.Items.Add(ExtensionItem.ItemTypes.String);
			cbtype.Items.Add(ExtensionItem.ItemTypes.Translation);
			cbtype.Items.Add(ExtensionItem.ItemTypes.Value);
			cbtype.SelectedIndex = 0;

			Content = new Avalonia.Controls.StackPanel { Children = {
				label28, tb_ver, label1, tb_type, label2, tb_name,
				gbIems
			}};
		}

		private void GNSettingsChange(object sender, System.EventArgs e)
		{
			if (Tag==null) return;
			try
			{
				SimPe.Plugin.Extension ext = (SimPe.Plugin.Extension)Tag;

				ext.Version = Convert.ToUInt32(tb_ver.Text, 16);
				ext.VarName = tb_name.Text;
				ext.TypeCode = Convert.ToByte(tb_type.Text, 16);
				ext.Changed = true;
				lldel.IsEnabled = false;
			}
			catch (Exception)
			{
				//Helper.ExceptionMessage("", ex);
			}
		}

		internal void HideAll()
		{
			this.gbval.IsVisible = false;
			this.gbar.IsVisible = false;
			this.gfootprintbar.IsVisible = false;
			this.gbfloat.IsVisible = false;
			this.gbbin.IsVisible = false;
			this.gbrot.IsVisible = false;
			this.gbstr.IsVisible = false;
			this.gbtrans.IsVisible = false;
		}

		internal void ShowGroup(Avalonia.Controls.Border gb)
		{
			gb.IsVisible = true;
		}

		private void SelectItem(object sender, System.EventArgs e)
		{
			if (tb_itemname.Tag != null) return;
			HideAll();
			if (lb_items.SelectedIndex<0) return;
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
						tbval.Text = "0x"+Helper.HexString((uint)ei.Value);
						ShowGroup(this.gbval);
						break;
					}
					case ExtensionItem.ItemTypes.Float:
					{
						tbfloat.Text = ei.Single.ToString();;
						ShowGroup(this.gbfloat);
						break;
					}
					case ExtensionItem.ItemTypes.Translation:
					{
						tbtrans1.Text = ei.Translation.X.ToString("N6");
						tbtrans2.Text = ei.Translation.Y.ToString("N6");
						tbtrans3.Text = ei.Translation.Z.ToString("N6");
						ShowGroup(this.gbtrans);
						break;
					}
					case ExtensionItem.ItemTypes.String:
					{
						tbstr.Text = ei.String;
						ShowGroup(this.gbstr);
						break;
					}
					case ExtensionItem.ItemTypes.Rotation:
					{
						tbrot1.Text = ei.Rotation.X.ToString("N6");
						tbrot2.Text = ei.Rotation.Y.ToString("N6");
						tbrot3.Text = ei.Rotation.Z.ToString("N6");
						tbrot4.Text = ei.Rotation.W.ToString("N6");
						ShowGroup(this.gbrot);
						break;
					}
					case ExtensionItem.ItemTypes.Binary:
					{
						tbbin.Text = Helper.BytesToHexList(ei.Data);
						ShowGroup(this.gbbin);
						break;
					}
					case ExtensionItem.ItemTypes.Array:
					{
						SimPe.Plugin.Extension ext = (SimPe.Plugin.Extension)Tag;

						if(ext.VarName.Equals("footprint"))
						{
							ShowGroup(this.gfootprintbar);
						}
						else
						{
							ShowGroup(this.gbar);
						}
						break;
					}
				} //switch
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

		private void OpenArrayEditor(object sender, Avalonia.Interactivity.RoutedEventArgs e)
		{
			// WinForms Form/Panel not available in Avalonia port — no-op
		}

		private void OpenFootprintEditor(object sender, Avalonia.Interactivity.RoutedEventArgs e)
		{
			// WinForms Form/Panel/CheckBox not available in Avalonia port — no-op
		}

		private void DeleteItem(object sender, Avalonia.Interactivity.RoutedEventArgs e)
		{
			if (lb_items.SelectedIndex<0) return;
			try
			{
				ExtensionItem[] list = (ExtensionItem[])gbIems.Tag;
				ExtensionItem ei = (ExtensionItem)lb_items.Items[lb_items.SelectedIndex];


				list = (ExtensionItem[])Helper.Delete(list, ei);
				gbIems.Tag = list;
				lb_items.Items.Remove(ei);

				//write back to the wrapper
				if (Tag != null)
				{
					SimPe.Plugin.Extension ext = (SimPe.Plugin.Extension)Tag;
					ext.Items = list;
				}
			}
			catch (Exception ex)
			{
				Helper.ExceptionMessage("", ex);
			}
		}

		private void AddItem(object sender, Avalonia.Interactivity.RoutedEventArgs e)
		{
			try
			{
				ExtensionItem[] list = (ExtensionItem[])gbIems.Tag;
				ExtensionItem ei = new ExtensionItem();
				ei.Typecode = (ExtensionItem.ItemTypes)cbtype.Items[cbtype.SelectedIndex];

				list = (ExtensionItem[])Helper.Add(list, ei);
				gbIems.Tag = list;
				lb_items.Items.Add(ei);

				//write back to the wrapper
				if (Tag != null)
				{
					SimPe.Plugin.Extension ext = (SimPe.Plugin.Extension)Tag;
					ext.Items = list;
				}
			}
			catch (Exception ex)
			{
				Helper.ExceptionMessage("", ex);
			}
		}

		private void ChangeName(object sender, System.EventArgs e)
		{
			if (tb_itemname.Tag != null) return;
			if (lb_items.SelectedIndex<0) return;
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

		private void ValChange(object sender, System.EventArgs e)
		{
			if (tb_itemname.Tag != null) return;
			if (lb_items.SelectedIndex<0) return;
			try
			{
				tb_itemname.Tag = true;
				ExtensionItem ei = (ExtensionItem)lb_items.Items[lb_items.SelectedIndex];
				ei.Value = (int)Convert.ToUInt32(tbval.Text, 16);

				lb_items.Items[lb_items.SelectedIndex] = ei;
			}
			catch (Exception){}
			finally
			{
				tb_itemname.Tag = null;
			}
		}

		private void FloatChange(object sender, System.EventArgs e)
		{
			if (tb_itemname.Tag != null) return;
			if (lb_items.SelectedIndex<0) return;
			try
			{
				tb_itemname.Tag = true;
				ExtensionItem ei = (ExtensionItem)lb_items.Items[lb_items.SelectedIndex];
				ei.Single = Convert.ToSingle(tbfloat.Text);

				lb_items.Items[lb_items.SelectedIndex] = ei;
			}
			catch (Exception){}
			finally
			{
				tb_itemname.Tag = null;
			}
		}

		private void TransChange(object sender, System.EventArgs e)
		{
			if (tb_itemname.Tag != null) return;
			if (lb_items.SelectedIndex<0) return;
			try
			{
				tb_itemname.Tag = true;
				ExtensionItem ei = (ExtensionItem)lb_items.Items[lb_items.SelectedIndex];
				ei.Translation.X = Convert.ToSingle(tbtrans1.Text);
				ei.Translation.Y = Convert.ToSingle(tbtrans2.Text);
				ei.Translation.Z = Convert.ToSingle(tbtrans3.Text);

				lb_items.Items[lb_items.SelectedIndex] = ei;
			}
			catch (Exception){}
			finally
			{
				tb_itemname.Tag = null;
			}
		}

		private void RotChange(object sender, System.EventArgs e)
		{
			if (tb_itemname.Tag != null) return;
			if (lb_items.SelectedIndex<0) return;
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
			catch (Exception){}
			finally
			{
				tb_itemname.Tag = null;
			}
		}

		private void StrChange(object sender, System.EventArgs e)
		{
			if (tb_itemname.Tag != null) return;
			if (lb_items.SelectedIndex<0) return;
			try
			{
				tb_itemname.Tag = true;
				ExtensionItem ei = (ExtensionItem)lb_items.Items[lb_items.SelectedIndex];
				ei.String = tbstr.Text;

				lb_items.Items[lb_items.SelectedIndex] = ei;
			}
			catch (Exception){}
			finally
			{
				tb_itemname.Tag = null;
			}
		}

		private void BinChange(object sender, System.EventArgs e)
		{
			if (tb_itemname.Tag != null) return;
			if (lb_items.SelectedIndex<0) return;
			try
			{
				tb_itemname.Tag = true;
				ExtensionItem ei = (ExtensionItem)lb_items.Items[lb_items.SelectedIndex];
				ei.Data= Helper.HexListToBytes(tbbin.Text);

				lb_items.Items[lb_items.SelectedIndex] = ei;
			}
			catch (Exception){}
			finally
			{
				tb_itemname.Tag = null;
			}
		}
	}
}
