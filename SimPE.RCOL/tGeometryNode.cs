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
	public class GeometryNode : Avalonia.Controls.TabItem
	{
		internal Avalonia.Controls.TextBox tb_gn_ver;
		private Avalonia.Controls.TextBlock label29;
		internal Avalonia.Controls.TextBox tb_gn_uk3;
		private Avalonia.Controls.TextBlock label33;
		internal Avalonia.Controls.TextBox tb_gn_uk2;
		private Avalonia.Controls.TextBlock label35;
		internal Avalonia.Controls.TextBox tb_gn_count;
		private Avalonia.Controls.TextBlock label36;
		internal Avalonia.Controls.TextBox tb_gn_uk1;
		private Avalonia.Controls.TextBlock label37;
		internal Avalonia.Controls.ComboBox cb_gn_list;
		internal Avalonia.Controls.TabControl tc_gn;

		public GeometryNode()
		{
			this.Header = "GeometryNode";

			tb_gn_ver = new Avalonia.Controls.TextBox { Text = "0x00000000" };
			tb_gn_ver.TextChanged += new EventHandler<Avalonia.Controls.TextChangedEventArgs>(this.GrNSettingsChange);
			label29 = new Avalonia.Controls.TextBlock { Text = "Version:" };
			tb_gn_uk3 = new Avalonia.Controls.TextBox { Text = "0x00" };
			tb_gn_uk3.TextChanged += new EventHandler<Avalonia.Controls.TextChangedEventArgs>(this.GrNSettingsChange);
			label33 = new Avalonia.Controls.TextBlock { Text = "Unknown 3:" };
			tb_gn_uk2 = new Avalonia.Controls.TextBox { Text = "0x0000" };
			tb_gn_uk2.TextChanged += new EventHandler<Avalonia.Controls.TextChangedEventArgs>(this.GrNSettingsChange);
			label35 = new Avalonia.Controls.TextBlock { Text = "Unknown 2:" };
			tb_gn_count = new Avalonia.Controls.TextBox { Text = "0x00000000" };
			label36 = new Avalonia.Controls.TextBlock { Text = "Count:" };
			tb_gn_uk1 = new Avalonia.Controls.TextBox { Text = "0x0000" };
			tb_gn_uk1.TextChanged += new EventHandler<Avalonia.Controls.TextChangedEventArgs>(this.GrNSettingsChange);
			label37 = new Avalonia.Controls.TextBlock { Text = "Unknown 1:" };
			tc_gn = new Avalonia.Controls.TabControl();
			cb_gn_list = new Avalonia.Controls.ComboBox();
			cb_gn_list.SelectionChanged += new EventHandler<Avalonia.Controls.SelectionChangedEventArgs>(this.SelectGmndChildBlock);

			Content = new Avalonia.Controls.StackPanel { Children = {
				label29, tb_gn_ver, label37, tb_gn_uk1,
				label35, tb_gn_uk2, label33, tb_gn_uk3,
				label36, tb_gn_count, cb_gn_list, tc_gn
			}};
		}

		private void GrNSettingsChange(object sender, System.EventArgs e)
		{
			if (this.Tag==null) return;
			try
			{
				SimPe.Plugin.GeometryNode arb = (SimPe.Plugin.GeometryNode)Tag;

				arb.Version = Convert.ToUInt32(tb_gn_ver.Text, 16);
				arb.Unknown1 = (short)Convert.ToUInt16(tb_gn_uk1.Text, 16);
				arb.Unknown2 = (short)Convert.ToUInt16(tb_gn_uk2.Text, 16);
				arb.Unknown3 = Convert.ToByte(tb_gn_uk3.Text, 16);

				arb.Changed = true;
			}
			catch (Exception)
			{
				//Helper.ExceptionMessage("", ex);
			}
		}

		private void SelectGmndChildBlock(object sender, Avalonia.Controls.SelectionChangedEventArgs e)
		{
			if (this.cb_gn_list.Tag!=null) return;
			if (cb_gn_list.SelectedIndex<0) return;
			try
			{
				cb_gn_list.Tag = true;
				SimPe.CountedListItem cli = (SimPe.CountedListItem)cb_gn_list.Items[cb_gn_list.SelectedIndex];
				AbstractRcolBlock rb = (AbstractRcolBlock)cli.Object;

				BuildChildTabControl(rb);
			}
			catch (Exception ex)
			{
				Helper.ExceptionMessage("", ex);
			}
			finally
			{
				cb_gn_list.Tag = null;
			}
		}

		internal void BuildChildTabControl(AbstractRcolBlock rb)
		{
			this.tc_gn.Items.Clear();

			if (rb==null) return;
			if (rb.TabPage!=null) rb.AddToTabControl(tc_gn);
		}
	}
}
