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
	/// Summary description for ShpeForm.
	/// </summary>
	public class ShpeItems : Avalonia.Controls.TabItem
	{
		private Avalonia.Controls.TextBox tbitemunk4;
		private Avalonia.Controls.TextBox tbitemunk3;
		private Avalonia.Controls.TextBox tbitemunk2;
		private Avalonia.Controls.TextBox tbitemunk1;
		private Avalonia.Controls.TextBlock label4;
		private Avalonia.Controls.TextBox tbitemflname;
		private Avalonia.Controls.TextBlock label3;
		internal Avalonia.Controls.ListBox lbitem;
		private Avalonia.Controls.Button linkLabel5;
		private Avalonia.Controls.Button linkLabel6;

		public ShpeItems()
		{
			this.Header = "Items";

			lbitem = new Avalonia.Controls.ListBox();
			lbitem.SelectionChanged += new EventHandler<Avalonia.Controls.SelectionChangedEventArgs>(this.SelectItems);
			label3 = new Avalonia.Controls.TextBlock { Text = "Filename:" };
			tbitemflname = new Avalonia.Controls.TextBox { Text = "" };
			tbitemflname.TextChanged += new EventHandler<Avalonia.Controls.TextChangedEventArgs>(this.ChangedItemFilename);
			label4 = new Avalonia.Controls.TextBlock { Text = "Unknown:" };
			tbitemunk1 = new Avalonia.Controls.TextBox { Text = "0x00000000" };
			tbitemunk1.TextChanged += new EventHandler<Avalonia.Controls.TextChangedEventArgs>(this.ChangeItemUnknown);
			tbitemunk2 = new Avalonia.Controls.TextBox { Text = "0x00" };
			tbitemunk2.TextChanged += new EventHandler<Avalonia.Controls.TextChangedEventArgs>(this.ChangeItemUnknown);
			tbitemunk3 = new Avalonia.Controls.TextBox { Text = "0x00000000" };
			tbitemunk3.TextChanged += new EventHandler<Avalonia.Controls.TextChangedEventArgs>(this.ChangeItemUnknown);
			tbitemunk4 = new Avalonia.Controls.TextBox { Text = "0x00" };
			tbitemunk4.TextChanged += new EventHandler<Avalonia.Controls.TextChangedEventArgs>(this.ChangeItemUnknown);
			linkLabel6 = new Avalonia.Controls.Button { Content = "add" };
			linkLabel6.Click += new EventHandler<Avalonia.Interactivity.RoutedEventArgs>(this.linkLabel6_LinkClicked);
			linkLabel5 = new Avalonia.Controls.Button { Content = "delete" };
			linkLabel5.Click += new EventHandler<Avalonia.Interactivity.RoutedEventArgs>(this.linkLabel5_LinkClicked);

			Content = new Avalonia.Controls.StackPanel { Children = {
				lbitem, label3, tbitemflname, label4,
				tbitemunk1, tbitemunk2, tbitemunk3, tbitemunk4,
				linkLabel6, linkLabel5
			}};
		}

		private void UpdateLists()
		{
			try
			{
				SimPe.Plugin.Shape shape = (SimPe.Plugin.Shape)this.Tag;

				ShapeItem[] items = new ShapeItem[lbitem.Items.Count];
				for (int i=0; i<items.Length; i++) items[i] = (ShapeItem)lbitem.Items[i];
				shape.Items = items;
			}
			catch (Exception){}
		}

		private void linkLabel6_LinkClicked(object sender, Avalonia.Interactivity.RoutedEventArgs e)
		{
			try
			{
				SimPe.Plugin.Shape shape = (SimPe.Plugin.Shape)this.Tag;

				ShapeItem val = new ShapeItem(shape);
				val.FileName = tbitemflname.Text;
				val.Unknown1 = Convert.ToInt32(tbitemunk1.Text, 16);
				val.Unknown2 = Convert.ToByte(tbitemunk2.Text, 16);
				val.Unknown3 = Convert.ToInt32(tbitemunk3.Text, 16);
				val.Unknown4 = Convert.ToByte(tbitemunk4.Text, 16);

				lbitem.Items.Add(val);
				UpdateLists();
			}
			catch (Exception) {}
		}

		private void linkLabel5_LinkClicked(object sender, Avalonia.Interactivity.RoutedEventArgs e)
		{
			if (lbitem.SelectedIndex < 0) return;
			lbitem.Items.RemoveAt(lbitem.SelectedIndex);
			UpdateLists();
		}

		private void ChangeItemUnknown(object sender, System.EventArgs e)
		{
			if (lbitem.Tag!=null) return;
			if (lbitem.SelectedIndex<0) return;

			try
			{
				lbitem.Tag = true;
				ShapeItem item = (ShapeItem)lbitem.Items[lbitem.SelectedIndex];
				item.Unknown1 = (int)Convert.ToUInt32(tbitemunk1.Text, 16);
				item.Unknown2 = Convert.ToByte(tbitemunk2.Text, 16);
				item.Unknown3 = (int)Convert.ToUInt32(tbitemunk3.Text, 16);
				item.Unknown4 = Convert.ToByte(tbitemunk4.Text, 16);
				lbitem.Items[lbitem.SelectedIndex] = item;
			}
			catch (Exception){}
			finally
			{
				lbitem.Tag = null;
			}
		}

		private void ChangedItemFilename(object sender, System.EventArgs e)
		{
			if (lbitem.Tag!=null) return;
			if (lbitem.SelectedIndex<0) return;

			try
			{
				lbitem.Tag = true;
				ShapeItem item = (ShapeItem)lbitem.Items[lbitem.SelectedIndex];
				item.FileName = tbitemflname.Text;
				lbitem.Items[lbitem.SelectedIndex] = item;
			}
			catch (Exception){}
			finally
			{
				lbitem.Tag = null;
			}
		}

		private void SelectItems(object sender, System.EventArgs e)
		{
			if (lbitem.Tag!=null) return;
			if (lbitem.SelectedIndex<0) return;

			try
			{
				lbitem.Tag = true;
				ShapeItem item = (ShapeItem)lbitem.Items[lbitem.SelectedIndex];
				tbitemflname.Text = item.FileName;

				tbitemunk1.Text = "0x"+Helper.HexString((uint)item.Unknown1);
				tbitemunk2.Text = "0x"+Helper.HexString(item.Unknown2);
				tbitemunk3.Text = "0x"+Helper.HexString((uint)item.Unknown3);
				tbitemunk4.Text = "0x"+Helper.HexString(item.Unknown4);
			}
			catch (Exception){}
			finally
			{
				lbitem.Tag = null;
			}
		}
	}
}
