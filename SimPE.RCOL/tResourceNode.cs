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
	/// Summary description for fShapeRefNode.
	/// </summary>
	public class ResourceNode : Avalonia.Controls.TabItem
	{
		private Avalonia.Controls.Button ll_rn_add;
		private Avalonia.Controls.TextBox tb_rn_2;
		private Avalonia.Controls.TextBlock label13;
		private Avalonia.Controls.TextBox tb_rn_1;
		private Avalonia.Controls.TextBlock label14;
		internal Avalonia.Controls.ListBox lb_rn;
		private Avalonia.Controls.Button ll_rn_delete;
		internal Avalonia.Controls.TextBox tb_rn_uk1;
		private Avalonia.Controls.TextBlock label22;
		internal Avalonia.Controls.TextBox tb_rn_uk2;
		private Avalonia.Controls.TextBlock label15;
		internal Avalonia.Controls.TextBox tb_rn_ver;
		private Avalonia.Controls.TextBlock label25;

		public ResourceNode()
		{
			this.Header = "ResourceNode";

			tb_rn_ver = new Avalonia.Controls.TextBox { Text = "0x00000000" };
			tb_rn_ver.TextChanged += new EventHandler<Avalonia.Controls.TextChangedEventArgs>(this.RNChangeSettings);
			label25 = new Avalonia.Controls.TextBlock { Text = "Version:" };
			tb_rn_uk1 = new Avalonia.Controls.TextBox { Text = "0x00000000" };
			tb_rn_uk1.TextChanged += new EventHandler<Avalonia.Controls.TextChangedEventArgs>(this.RNChangeSettings);
			label22 = new Avalonia.Controls.TextBlock { Text = "Unknown 1:" };
			tb_rn_uk2 = new Avalonia.Controls.TextBox { Text = "0x00000000" };
			tb_rn_uk2.TextChanged += new EventHandler<Avalonia.Controls.TextChangedEventArgs>(this.RNChangeSettings);
			label15 = new Avalonia.Controls.TextBlock { Text = "Unknown 2:" };
			lb_rn = new Avalonia.Controls.ListBox();
			lb_rn.SelectionChanged += new EventHandler<Avalonia.Controls.SelectionChangedEventArgs>(this.RNSelect);
			ll_rn_add = new Avalonia.Controls.Button { Content = "add" };
			ll_rn_add.Click += new EventHandler<Avalonia.Interactivity.RoutedEventArgs>(this.RNItemsAdd);
			tb_rn_1 = new Avalonia.Controls.TextBox { Text = "0x0000" };
			tb_rn_1.TextChanged += new EventHandler<Avalonia.Controls.TextChangedEventArgs>(this.RNChangedItems);
			label14 = new Avalonia.Controls.TextBlock { Text = "Unknown 1:" };
			tb_rn_2 = new Avalonia.Controls.TextBox { Text = "0x00000000" };
			tb_rn_2.TextChanged += new EventHandler<Avalonia.Controls.TextChangedEventArgs>(this.RNChangedItems);
			label13 = new Avalonia.Controls.TextBlock { Text = "Unknown 2:" };
			ll_rn_delete = new Avalonia.Controls.Button { Content = "delete" };
			ll_rn_delete.Click += new EventHandler<Avalonia.Interactivity.RoutedEventArgs>(this.RNItemsDelete);

			Content = new Avalonia.Controls.StackPanel { Children = {
				label25, tb_rn_ver, label22, tb_rn_uk1, label15, tb_rn_uk2,
				lb_rn, label14, tb_rn_1, label13, tb_rn_2, ll_rn_add, ll_rn_delete
			}};
		}

		private void RNChangeSettings(object sender, System.EventArgs e)
		{
			if (Tag==null) return;
			try
			{
				SimPe.Plugin.ResourceNode rn = (SimPe.Plugin.ResourceNode)Tag;

				rn.Unknown1 = (int)Convert.ToInt32(this.tb_rn_uk1.Text, 16);
				rn.Unknown2 = (int)Convert.ToInt32(this.tb_rn_uk2.Text, 16);
				rn.Version = Convert.ToUInt32(tb_rn_ver.Text, 16);

				rn.Changed = true;
			}
			catch (Exception)
			{
				//Helper.ExceptionMessage("", ex);
			}
		}

		#region Select RN Items
		private void RNSelect(object sender, System.EventArgs e)
		{
			if (lb_rn.Tag != null) return;
			if (this.lb_rn.SelectedIndex<0) return;

			try
			{
				lb_rn.Tag = true;
				SimPe.Plugin.ResourceNode rn = (SimPe.Plugin.ResourceNode)Tag;
				ResourceNodeItem b = (ResourceNodeItem)lb_rn.Items[lb_rn.SelectedIndex];

				tb_rn_1.Text = "0x"+Helper.HexString((ushort)b.Unknown1);
				tb_rn_2.Text = "0x"+Helper.HexString((uint)b.Unknown2);
				rn.Changed = true;
			}
			catch (Exception)
			{
				//Helper.ExceptionMessage("", ex);
			}
			finally
			{
				lb_rn.Tag = null;
			}
		}

		private void RNChangedItems(object sender, System.EventArgs e)
		{
			if (lb_rn.Tag != null) return;
			if (this.lb_rn.SelectedIndex<0) return;

			try
			{
				lb_rn.Tag = true;
				SimPe.Plugin.ResourceNode rn = (SimPe.Plugin.ResourceNode)Tag;
				ResourceNodeItem b = (ResourceNodeItem)lb_rn.Items[lb_rn.SelectedIndex];

				b.Unknown1 = (short)Convert.ToUInt16(tb_rn_1.Text, 16);
				b.Unknown2 = (int)Convert.ToUInt32(tb_rn_2.Text, 16);

				lb_rn.Items[lb_rn.SelectedIndex] = b;
				rn.Changed = true;
			}
			catch (Exception)
			{
				//Helper.ExceptionMessage("", ex);
			}
			finally
			{
				lb_rn.Tag = null;
			}
		}

		private void RNItemsAdd(object sender, Avalonia.Interactivity.RoutedEventArgs e)
		{
			if (Tag==null) return;
			try
			{
				lb_rn.Tag = true;
				SimPe.Plugin.ResourceNode rn = (SimPe.Plugin.ResourceNode)Tag;
				ResourceNodeItem b = new ResourceNodeItem();

				b.Unknown1 = (short)Convert.ToUInt16(tb_rn_1.Text, 16);
				b.Unknown2 = (int)Convert.ToUInt32(tb_rn_2.Text, 16);

				rn.Items = (ResourceNodeItem[])Helper.Add(rn.Items, b);
				lb_rn.Items.Add(b);
				rn.Changed = true;
			}
			catch (Exception ex)
			{
				Helper.ExceptionMessage("", ex);
			}
			finally
			{
				lb_rn.Tag = null;
			}
		}

		private void RNItemsDelete(object sender, Avalonia.Interactivity.RoutedEventArgs e)
		{
			if (Tag==null) return;
			if (lb_rn.SelectedIndex<0) return;
			try
			{
				lb_rn.Tag = true;
				SimPe.Plugin.ResourceNode rn = (SimPe.Plugin.ResourceNode)Tag;
				ResourceNodeItem b = (ResourceNodeItem)lb_rn.Items[lb_rn.SelectedIndex];

				rn.Items = (ResourceNodeItem[])Helper.Delete(rn.Items, b);
				lb_rn.Items.Remove(b);
				rn.Changed = true;
			}
			catch (Exception ex)
			{
				Helper.ExceptionMessage("", ex);
			}
			finally
			{
				lb_rn.Tag = null;
			}
		}
		#endregion
	}
}
