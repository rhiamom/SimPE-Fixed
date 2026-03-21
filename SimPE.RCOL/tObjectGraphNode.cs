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
	public class ObjectGraphNode : Avalonia.Controls.TabItem
	{
		private Avalonia.Controls.Button ll_ogn_add;
		private Avalonia.Controls.TextBox tb_ogn_2;
		private Avalonia.Controls.TextBlock label20;
		private Avalonia.Controls.TextBox tb_ogn_1;
		private Avalonia.Controls.TextBlock label21;
		internal Avalonia.Controls.ListBox lb_ogn;
		private Avalonia.Controls.Button ll_ogn_delete;
		private Avalonia.Controls.TextBox tb_ogn_3;
		private Avalonia.Controls.TextBlock label23;
		internal Avalonia.Controls.TextBox tb_ogn_file;
		private Avalonia.Controls.TextBlock label18;
		internal Avalonia.Controls.TextBox tb_ogn_ver;
		private Avalonia.Controls.TextBlock label27;

		public ObjectGraphNode()
		{
			this.Header = "ObjectGraphNode";

			tb_ogn_ver = new Avalonia.Controls.TextBox { Text = "0x00000000" };
			tb_ogn_ver.TextChanged += new EventHandler<Avalonia.Controls.TextChangedEventArgs>(this.OGNChangeSettings);
			label27 = new Avalonia.Controls.TextBlock { Text = "Version:" };
			tb_ogn_file = new Avalonia.Controls.TextBox { Text = "0x0000" };
			tb_ogn_file.TextChanged += new EventHandler<Avalonia.Controls.TextChangedEventArgs>(this.OGNChangeSettings);
			label18 = new Avalonia.Controls.TextBlock { Text = "Filename:" };
			tb_ogn_3 = new Avalonia.Controls.TextBox { Text = "0x00000000" };
			tb_ogn_3.TextChanged += new EventHandler<Avalonia.Controls.TextChangedEventArgs>(this.OGNChangedItems);
			label23 = new Avalonia.Controls.TextBlock { Text = "Index:" };
			ll_ogn_add = new Avalonia.Controls.Button { Content = "add" };
			ll_ogn_add.Click += new EventHandler<Avalonia.Interactivity.RoutedEventArgs>(this.OGNItemsAdd);
			tb_ogn_2 = new Avalonia.Controls.TextBox { Text = "0x00" };
			tb_ogn_2.TextChanged += new EventHandler<Avalonia.Controls.TextChangedEventArgs>(this.OGNChangedItems);
			label20 = new Avalonia.Controls.TextBlock { Text = "Dependant:" };
			tb_ogn_1 = new Avalonia.Controls.TextBox { Text = "0x00" };
			tb_ogn_1.TextChanged += new EventHandler<Avalonia.Controls.TextChangedEventArgs>(this.OGNChangedItems);
			label21 = new Avalonia.Controls.TextBlock { Text = "Enabled:" };
			lb_ogn = new Avalonia.Controls.ListBox();
			lb_ogn.SelectionChanged += new EventHandler<Avalonia.Controls.SelectionChangedEventArgs>(this.OGNSelect);
			ll_ogn_delete = new Avalonia.Controls.Button { Content = "delete" };
			ll_ogn_delete.Click += new EventHandler<Avalonia.Interactivity.RoutedEventArgs>(this.OGNItemsDelete);

			Content = new Avalonia.Controls.StackPanel { Children = {
				label27, tb_ogn_ver, label18, tb_ogn_file,
				lb_ogn, label21, tb_ogn_1, label20, tb_ogn_2, label23, tb_ogn_3,
				ll_ogn_add, ll_ogn_delete
			}};
		}

		private void OGNChangeSettings(object sender, System.EventArgs e)
		{
			if (Tag==null) return;
			try
			{
				SimPe.Plugin.ObjectGraphNode ogn = (SimPe.Plugin.ObjectGraphNode)Tag;

				ogn.FileName = tb_ogn_file.Text;
				ogn.Version = Convert.ToUInt32(tb_ogn_ver.Text, 16);
				ogn.Changed = true;
			}
			catch (Exception)
			{
				//Helper.ExceptionMessage("", ex);
			}
		}

		#region Select OGN Items
		private void OGNSelect(object sender, System.EventArgs e)
		{
			if (Tag == null) return;
			if (lb_ogn.Tag!=null) return;
			if (this.lb_ogn.SelectedIndex<0) return;

			try
			{
				lb_ogn.Tag = true;
				SimPe.Plugin.ObjectGraphNode ogn = (SimPe.Plugin.ObjectGraphNode)Tag;
				ObjectGraphNodeItem b = (ObjectGraphNodeItem)lb_ogn.Items[lb_ogn.SelectedIndex];

				tb_ogn_1.Text = "0x"+Helper.HexString(b.Enabled);
				tb_ogn_2.Text = "0x"+Helper.HexString(b.Dependant);
				tb_ogn_3.Text = "0x"+Helper.HexString(b.Index);
				ogn.Changed = true;
			}
			catch (Exception)
			{
				//Helper.ExceptionMessage("", ex);
			}
			finally
			{
				lb_ogn.Tag = null;
			}
		}

		private void OGNChangedItems(object sender, System.EventArgs e)
		{
			if (Tag == null) return;
			if (lb_ogn.Tag!=null) return;
			if (this.lb_ogn.SelectedIndex<0) return;

			try
			{
				lb_ogn.Tag = true;
				SimPe.Plugin.ObjectGraphNode ogn = (SimPe.Plugin.ObjectGraphNode)Tag;
				ObjectGraphNodeItem b = (ObjectGraphNodeItem)lb_ogn.Items[lb_ogn.SelectedIndex];

				b.Enabled = Convert.ToByte(tb_ogn_1.Text, 16);
				b.Dependant = Convert.ToByte(tb_ogn_2.Text, 16);
				b.Index = Convert.ToUInt32(tb_ogn_3.Text, 16);

				lb_ogn.Items[lb_ogn.SelectedIndex] = b;
				ogn.Changed = true;
			}
			catch (Exception)
			{
				//Helper.ExceptionMessage("", ex);
			}
			finally
			{
				lb_ogn.Tag = null;
			}
		}

		private void OGNItemsAdd(object sender, Avalonia.Interactivity.RoutedEventArgs e)
		{
			if (Tag==null) return;
			try
			{
				lb_ogn.Tag = true;
				SimPe.Plugin.ObjectGraphNode ogn = (SimPe.Plugin.ObjectGraphNode)Tag;
				ObjectGraphNodeItem b = new ObjectGraphNodeItem();

				tb_ogn_1.Text = "0x"+Helper.HexString(b.Enabled);
				tb_ogn_2.Text = "0x"+Helper.HexString(b.Dependant);
				tb_ogn_3.Text = "0x"+Helper.HexString(b.Index);

				ogn.Items = (ObjectGraphNodeItem[])Helper.Add(ogn.Items, b);
				lb_ogn.Items.Add(b);
				ogn.Changed = true;
			}
			catch (Exception ex)
			{
				Helper.ExceptionMessage("", ex);
			}
			finally
			{
				lb_ogn.Tag = null;
			}
		}

		private void OGNItemsDelete(object sender, Avalonia.Interactivity.RoutedEventArgs e)
		{
			if (Tag==null) return;
			if (lb_ogn.SelectedIndex<0) return;
			try
			{
				lb_ogn.Tag = true;
				SimPe.Plugin.ObjectGraphNode ogn = (SimPe.Plugin.ObjectGraphNode)Tag;
				ObjectGraphNodeItem b = (ObjectGraphNodeItem)lb_ogn.Items[lb_ogn.SelectedIndex];

				ogn.Items = (ObjectGraphNodeItem[])Helper.Delete(ogn.Items, b);
				lb_ogn.Items.Remove(b);
				ogn.Changed = true;
			}
			catch (Exception ex)
			{
				Helper.ExceptionMessage("", ex);
			}
			finally
			{
				lb_ogn.Tag = null;
			}
		}
		#endregion
	}
}
