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
	/// Zusammenfassung fuer ShpeForm.
	/// </summary>
	public class ShpeGraphNode : Avalonia.Controls.TabItem
	{
		internal Avalonia.Controls.TextBox tbnodeflname;
		private Avalonia.Controls.TextBlock label8;
		internal Avalonia.Controls.ListBox lbnode;
		private Avalonia.Controls.TextBox tbnode1;
		private Avalonia.Controls.TextBlock label9;
		private Avalonia.Controls.TextBox tbnode2;
		private Avalonia.Controls.TextBox tbnode3;
		private Avalonia.Controls.Button linkLabel9;
		private Avalonia.Controls.Button linkLabel10;
		private Avalonia.Controls.TextBlock label20;
		private Avalonia.Controls.TextBlock label11;

		public ShpeGraphNode()
		{
			this.Header = "Graph Node";

			label8 = new Avalonia.Controls.TextBlock { Text = "Filename:" };
			tbnodeflname = new Avalonia.Controls.TextBox { Text = "" };
			lbnode = new Avalonia.Controls.ListBox();
			lbnode.SelectionChanged += new EventHandler<Avalonia.Controls.SelectionChangedEventArgs>(this.SelectNode);
			label9 = new Avalonia.Controls.TextBlock { Text = "Enabled?:" };
			tbnode1 = new Avalonia.Controls.TextBox { Text = "0x00" };
			label20 = new Avalonia.Controls.TextBlock { Text = "Dependant:" };
			tbnode2 = new Avalonia.Controls.TextBox { Text = "0x00" };
			label11 = new Avalonia.Controls.TextBlock { Text = "Index:" };
			tbnode3 = new Avalonia.Controls.TextBox { Text = "0x00000000" };
			linkLabel10 = new Avalonia.Controls.Button { Content = "add" };
			linkLabel10.Click += new EventHandler<Avalonia.Interactivity.RoutedEventArgs>(this.linkLabel10_LinkClicked);
			linkLabel9 = new Avalonia.Controls.Button { Content = "delete" };
			linkLabel9.Click += new EventHandler<Avalonia.Interactivity.RoutedEventArgs>(this.linkLabel9_LinkClicked);

			Content = new Avalonia.Controls.StackPanel { Children = {
				label8, tbnodeflname, lbnode,
				label9, tbnode1, label20, tbnode2, label11, tbnode3,
				linkLabel10, linkLabel9
			}};
		}

		private void UpdateLists()
		{
			try
			{
				SimPe.Plugin.Shape shape = (SimPe.Plugin.Shape)this.Tag;

				ObjectGraphNodeItem[] ogni = new ObjectGraphNodeItem[lbnode.Items.Count];
				for (int i=0; i<ogni.Length; i++) ogni[i] = (ObjectGraphNodeItem)lbnode.Items[i];
				shape.GraphNode.Items = ogni;
			}
			catch (Exception){}
		}

		private void Commit(object sender, System.EventArgs e)
		{
		}

		private void SelectNode(object sender, System.EventArgs e)
		{
			if (lbnode.Tag!=null) return;
			if (lbnode.SelectedIndex<0) return;

			try
			{
				lbnode.Tag = true;
				ObjectGraphNodeItem item = (ObjectGraphNodeItem)lbnode.Items[lbnode.SelectedIndex];
				tbnode1.Text = "0x"+Helper.HexString(item.Enabled);
				tbnode2.Text = "0x"+Helper.HexString(item.Dependant);
				tbnode2.Text = "0x"+Helper.HexString(item.Index);
			}
			catch (Exception){}
			finally
			{
				lbnode.Tag = null;
			}
		}

		private void linkLabel10_LinkClicked(object sender, Avalonia.Interactivity.RoutedEventArgs e)
		{
			try
			{
				Shape shape = (Shape)this.Tag;

				ObjectGraphNodeItem val = new ObjectGraphNodeItem();
				val.Enabled = Convert.ToByte(tbnode1.Text,16);
				val.Dependant = Convert.ToByte(tbnode2.Text,16);
				val.Index = Convert.ToUInt32(tbnode3.Text,16);

				lbnode.Items.Add(val);
				UpdateLists();
			}
			catch (Exception) {}
		}

		private void linkLabel9_LinkClicked(object sender, Avalonia.Interactivity.RoutedEventArgs e)
		{
			if (lbnode.SelectedIndex < 0) return;
			lbnode.Items.RemoveAt(lbnode.SelectedIndex);
			UpdateLists();
		}
	}
}
