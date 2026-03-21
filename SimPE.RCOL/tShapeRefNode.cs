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
	public class ShapeRefNode : Avalonia.Controls.TabItem
	{
		internal Avalonia.Controls.ListBox lb_srn_b;
		internal Avalonia.Controls.ListBox lb_srn_a;
		private Avalonia.Controls.TextBlock label1;
		private Avalonia.Controls.TextBlock label2;
		private Avalonia.Controls.TextBlock label3;
		private Avalonia.Controls.TextBlock label4;
		private Avalonia.Controls.Button ll_srn_dela;
		private Avalonia.Controls.Button ll_srn_delb;
		private Avalonia.Controls.Button linkLabel3;
		private Avalonia.Controls.Button linkLabel4;
		private Avalonia.Controls.TextBlock label5;
		private Avalonia.Controls.TextBlock label6;
		private Avalonia.Controls.TextBlock label7;
		private Avalonia.Controls.TextBlock label8;
		private Avalonia.Controls.TextBlock label9;
		private Avalonia.Controls.TextBlock label10;
		private Avalonia.Controls.TextBlock label11;
		private Avalonia.Controls.TextBlock label12;
		internal Avalonia.Controls.TextBox tb_srn_uk2;
		internal Avalonia.Controls.TextBox tb_srn_uk1;
		internal Avalonia.Controls.TextBox tb_srn_uk4;
		internal Avalonia.Controls.TextBox tb_srn_uk3;
		internal Avalonia.Controls.TextBox tb_srn_uk6;
		internal Avalonia.Controls.TextBox tb_srn_uk5;
		internal Avalonia.Controls.TextBox tb_srn_kind;
		internal Avalonia.Controls.TextBox tb_srn_data;
		private Avalonia.Controls.TextBox tb_srn_b_name;
		private Avalonia.Controls.TextBox tb_srn_b_1;
		private Avalonia.Controls.TextBox tb_srn_a_2;
		private Avalonia.Controls.TextBox tb_srn_a_1;
		internal Avalonia.Controls.TextBox tb_srn_ver;
		private Avalonia.Controls.TextBlock label24;

		public ShapeRefNode()
		{
			this.Header = "ShapeRefNode";

			tb_srn_ver = new Avalonia.Controls.TextBox { Text = "0x00000000" };
			tb_srn_ver.TextChanged += new EventHandler<Avalonia.Controls.TextChangedEventArgs>(this.SRNChangeSettings);
			label24 = new Avalonia.Controls.TextBlock { Text = "Version:" };
			tb_srn_uk1 = new Avalonia.Controls.TextBox { Text = "0x0000" };
			tb_srn_uk1.TextChanged += new EventHandler<Avalonia.Controls.TextChangedEventArgs>(this.SRNChangeSettings);
			label6 = new Avalonia.Controls.TextBlock { Text = "Unknown 1:" };
			tb_srn_uk2 = new Avalonia.Controls.TextBox { Text = "0x00000000" };
			tb_srn_uk2.TextChanged += new EventHandler<Avalonia.Controls.TextChangedEventArgs>(this.SRNChangeSettings);
			label5 = new Avalonia.Controls.TextBlock { Text = "Unknown 2:" };
			tb_srn_uk3 = new Avalonia.Controls.TextBox { Text = "0x00000000" };
			tb_srn_uk3.TextChanged += new EventHandler<Avalonia.Controls.TextChangedEventArgs>(this.SRNChangeSettings);
			label8 = new Avalonia.Controls.TextBlock { Text = "Unknown 3:" };
			tb_srn_uk4 = new Avalonia.Controls.TextBox { Text = "0x00" };
			tb_srn_uk4.TextChanged += new EventHandler<Avalonia.Controls.TextChangedEventArgs>(this.SRNChangeSettings);
			label7 = new Avalonia.Controls.TextBlock { Text = "Unknown 4:" };
			tb_srn_uk5 = new Avalonia.Controls.TextBox { Text = "0x00000000" };
			tb_srn_uk5.TextChanged += new EventHandler<Avalonia.Controls.TextChangedEventArgs>(this.SRNChangeSettings);
			label10 = new Avalonia.Controls.TextBlock { Text = "Unknown 5:" };
			tb_srn_uk6 = new Avalonia.Controls.TextBox { Text = "0x00000000" };
			tb_srn_uk6.TextChanged += new EventHandler<Avalonia.Controls.TextChangedEventArgs>(this.SRNChangeSettings);
			label9 = new Avalonia.Controls.TextBlock { Text = "Unknown 6:" };
			tb_srn_kind = new Avalonia.Controls.TextBox { Text = "0x0000" };
			tb_srn_kind.TextChanged += new EventHandler<Avalonia.Controls.TextChangedEventArgs>(this.SRNChangeSettings);
			label11 = new Avalonia.Controls.TextBlock { Text = "Kind:" };
			tb_srn_data = new Avalonia.Controls.TextBox { Text = "" };
			tb_srn_data.TextChanged += new EventHandler<Avalonia.Controls.TextChangedEventArgs>(this.SRNChangeSettings);
			label12 = new Avalonia.Controls.TextBlock { Text = "Data:" };

			// List A (Shape Reference Index)
			lb_srn_a = new Avalonia.Controls.ListBox();
			lb_srn_a.SelectionChanged += new EventHandler<Avalonia.Controls.SelectionChangedEventArgs>(this.SRNSelectA);
			label1 = new Avalonia.Controls.TextBlock { Text = "Unknown 1:" };
			tb_srn_a_1 = new Avalonia.Controls.TextBox { Text = "0x0000" };
			tb_srn_a_1.TextChanged += new EventHandler<Avalonia.Controls.TextChangedEventArgs>(this.SRNChangedItemsA);
			label2 = new Avalonia.Controls.TextBlock { Text = "Child Index:" };
			tb_srn_a_2 = new Avalonia.Controls.TextBox { Text = "0x00000000" };
			tb_srn_a_2.TextChanged += new EventHandler<Avalonia.Controls.TextChangedEventArgs>(this.SRNChangedItemsA);
			linkLabel3 = new Avalonia.Controls.Button { Content = "add" };
			linkLabel3.Click += new EventHandler<Avalonia.Interactivity.RoutedEventArgs>(this.SRNItemsAAdd);
			ll_srn_dela = new Avalonia.Controls.Button { Content = "delete" };
			ll_srn_dela.Click += new EventHandler<Avalonia.Interactivity.RoutedEventArgs>(this.SRNItemsADelete);

			// List B (Unknown List B)
			lb_srn_b = new Avalonia.Controls.ListBox();
			label3 = new Avalonia.Controls.TextBlock { Text = "Unknown 1:" };
			tb_srn_b_1 = new Avalonia.Controls.TextBox { Text = "0x00000000" };
			label4 = new Avalonia.Controls.TextBlock { Text = "Name:" };
			tb_srn_b_name = new Avalonia.Controls.TextBox { Text = "" };
			linkLabel4 = new Avalonia.Controls.Button { Content = "add" };
			linkLabel4.Click += new EventHandler<Avalonia.Interactivity.RoutedEventArgs>(this.SRNItemsBAdd);
			ll_srn_delb = new Avalonia.Controls.Button { Content = "delete" };
			ll_srn_delb.Click += new EventHandler<Avalonia.Interactivity.RoutedEventArgs>(this.SRNItemsBDelete);

			Content = new Avalonia.Controls.StackPanel { Children = {
				label24, tb_srn_ver,
				label6, tb_srn_uk1, label5, tb_srn_uk2,
				label8, tb_srn_uk3, label7, tb_srn_uk4,
				label10, tb_srn_uk5, label9, tb_srn_uk6,
				label11, tb_srn_kind, label12, tb_srn_data,
				lb_srn_a, label1, tb_srn_a_1, label2, tb_srn_a_2, linkLabel3, ll_srn_dela,
				lb_srn_b, label3, tb_srn_b_1, label4, tb_srn_b_name, linkLabel4, ll_srn_delb
			}};
		}

		private void SRNChangeSettings(object sender, System.EventArgs e)
		{
			if (Tag==null) return;
			try
			{
				SimPe.Plugin.ShapeRefNode srn = (SimPe.Plugin.ShapeRefNode)Tag;

				srn.Unknown1 = Convert.ToInt16(this.tb_srn_uk1.Text, 16);
				srn.Unknown2 = Convert.ToInt32(this.tb_srn_uk2.Text, 16);
				srn.Unknown3 = Convert.ToInt32(this.tb_srn_uk3.Text, 16);
				srn.Unknown4 = Convert.ToByte(this.tb_srn_uk4.Text, 16);
				srn.Unknown5 = Convert.ToInt32(this.tb_srn_uk5.Text, 16);
				srn.Unknown6 = Convert.ToInt32(this.tb_srn_uk6.Text, 16);

				srn.Name = this.tb_srn_kind.Text;
				srn.Data = Helper.HexListToBytes(tb_srn_data.Text);

				srn.Version = Convert.ToUInt32(tb_srn_ver.Text, 16);

				srn.Changed = true;
			}
			catch (Exception)
			{
				//Helper.ExceptionMessage("", ex);
			}
		}

		#region Select SRN Items A
		private void SRNSelectA(object sender, System.EventArgs e)
		{
			if (lb_srn_a.Tag != null) return;
			if (this.lb_srn_a.SelectedIndex<0) return;

			try
			{
				lb_srn_a.Tag = true;
				ShapeRefNodeItem_A a = (ShapeRefNodeItem_A)lb_srn_a.Items[lb_srn_a.SelectedIndex];

				tb_srn_a_1.Text = "0x"+Helper.HexString(a.Unknown1);
				tb_srn_a_2.Text = "0x"+Helper.HexString((uint)a.Unknown2);

				SimPe.Plugin.ShapeRefNode srn = (SimPe.Plugin.ShapeRefNode)Tag;
				srn.Changed = true;
			}
			catch (Exception)
			{
				//Helper.ExceptionMessage("", ex);
			}
			finally
			{
				lb_srn_a.Tag = null;
			}
		}

		private void SRNChangedItemsA(object sender, System.EventArgs e)
		{
			if (lb_srn_a.Tag != null) return;
			if (this.lb_srn_a.SelectedIndex<0) return;

			try
			{
				lb_srn_a.Tag = true;
				ShapeRefNodeItem_A a = (ShapeRefNodeItem_A)lb_srn_a.Items[lb_srn_a.SelectedIndex];

				a.Unknown1 = Convert.ToUInt16(tb_srn_a_1.Text, 16);
				a.Unknown2 = (int)Convert.ToUInt32(tb_srn_a_2.Text, 16);

				lb_srn_a.Items[lb_srn_a.SelectedIndex] = a;

				SimPe.Plugin.ShapeRefNode srn = (SimPe.Plugin.ShapeRefNode)Tag;
				srn.Changed = true;
			}
			catch (Exception)
			{
				//Helper.ExceptionMessage("", ex);
			}
			finally
			{
				lb_srn_a.Tag = null;
			}
		}

		private void SRNItemsAAdd(object sender, Avalonia.Interactivity.RoutedEventArgs e)
		{
			if (Tag==null) return;
			try
			{
				lb_srn_a.Tag = true;
				SimPe.Plugin.ShapeRefNode srn = (SimPe.Plugin.ShapeRefNode)Tag;
				ShapeRefNodeItem_A a = new ShapeRefNodeItem_A();

				tb_srn_a_1.Text = "0x"+Helper.HexString(a.Unknown1);
				tb_srn_a_2.Text = "0x"+Helper.HexString((uint)a.Unknown2);

				srn.ItemsA = (ShapeRefNodeItem_A[])Helper.Add(srn.ItemsA, a);
				lb_srn_a.Items.Add(a);

				srn.Changed = true;
			}
			catch (Exception ex)
			{
				Helper.ExceptionMessage("", ex);
			}
			finally
			{
				lb_srn_a.Tag = null;
			}
		}

		private void SRNItemsADelete(object sender, Avalonia.Interactivity.RoutedEventArgs e)
		{
			if (Tag==null) return;
			if (lb_srn_a.SelectedIndex<0) return;
			try
			{
				lb_srn_a.Tag = true;
				SimPe.Plugin.ShapeRefNode srn = (SimPe.Plugin.ShapeRefNode)Tag;
				ShapeRefNodeItem_A a = (ShapeRefNodeItem_A)lb_srn_a.Items[lb_srn_a.SelectedIndex];

				srn.ItemsA = (ShapeRefNodeItem_A[])Helper.Delete(srn.ItemsA, a);
				lb_srn_a.Items.Remove(a);

				srn.Changed = true;
			}
			catch (Exception ex)
			{
				Helper.ExceptionMessage("", ex);
			}
			finally
			{
				lb_srn_a.Tag = null;
			}
		}
		#endregion

		#region Select SRN Items B
		private void SRNSelectB(object sender, System.EventArgs e)
		{
			if (lb_srn_b.Tag != null) return;
			if (this.lb_srn_b.SelectedIndex<0) return;

			try
			{
				lb_srn_b.Tag = true;
				ShapeRefNodeItem_B b = (ShapeRefNodeItem_B)lb_srn_b.Items[lb_srn_b.SelectedIndex];

				tb_srn_b_1.Text = "0x"+Helper.HexString((uint)b.Unknown1);
				tb_srn_b_name.Text = b.Name;

				SimPe.Plugin.ShapeRefNode srn = (SimPe.Plugin.ShapeRefNode)Tag;
				srn.Changed = true;
			}
			catch (Exception)
			{
				//Helper.ExceptionMessage("", ex);
			}
			finally
			{
				lb_srn_b.Tag = null;
			}
		}

		private void SRNChangedItemsB(object sender, System.EventArgs e)
		{
			if (lb_srn_b.Tag != null) return;
			if (this.lb_srn_b.SelectedIndex<0) return;

			try
			{
				lb_srn_b.Tag = true;
				ShapeRefNodeItem_B b = (ShapeRefNodeItem_B)lb_srn_b.Items[lb_srn_b.SelectedIndex];

				b.Unknown1 = (int)Convert.ToUInt32(tb_srn_b_1.Text, 16);
				b.Name= tb_srn_b_name.Text;

				lb_srn_b.Items[lb_srn_b.SelectedIndex] = b;
				SimPe.Plugin.ShapeRefNode srn = (SimPe.Plugin.ShapeRefNode)Tag;
				srn.Changed = true;
			}
			catch (Exception)
			{
				//Helper.ExceptionMessage("", ex);
			}
			finally
			{
				lb_srn_b.Tag = null;
			}
		}

		private void SRNItemsBAdd(object sender, Avalonia.Interactivity.RoutedEventArgs e)
		{
			if (Tag==null) return;
			try
			{
				lb_srn_b.Tag = true;
				SimPe.Plugin.ShapeRefNode srn = (SimPe.Plugin.ShapeRefNode)Tag;
				ShapeRefNodeItem_B b = new ShapeRefNodeItem_B();

				b.Unknown1 = (int)Convert.ToUInt32(tb_srn_b_1.Text, 16);
				b.Name= tb_srn_b_name.Text;

				srn.ItemsB = (ShapeRefNodeItem_B[])Helper.Add(srn.ItemsB, b);
				lb_srn_b.Items.Add(b);
				srn.Changed = true;
			}
			catch (Exception ex)
			{
				Helper.ExceptionMessage("", ex);
			}
			finally
			{
				lb_srn_b.Tag = null;
			}
		}

		private void SRNItemsBDelete(object sender, Avalonia.Interactivity.RoutedEventArgs e)
		{
			if (Tag==null) return;
			if (lb_srn_b.SelectedIndex<0) return;
			try
			{
				lb_srn_b.Tag = true;
				SimPe.Plugin.ShapeRefNode srn = (SimPe.Plugin.ShapeRefNode)Tag;
				ShapeRefNodeItem_B b = (ShapeRefNodeItem_B)lb_srn_b.Items[lb_srn_b.SelectedIndex];

				srn.ItemsB = (ShapeRefNodeItem_B[])Helper.Delete(srn.ItemsB, b);
				lb_srn_b.Items.Remove(b);

				srn.Changed = true;
			}
			catch (Exception ex)
			{
				Helper.ExceptionMessage("", ex);
			}
			finally
			{
				lb_srn_b.Tag = null;
			}
		}
		#endregion
	}
}
