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
using Avalonia.Layout;

namespace SimPe.Plugin
{
	/// <summary>
	/// This Form is no longer used...
	/// </summary>
	public class ShpeForm : Avalonia.Controls.UserControl
	{
		private Avalonia.Controls.TabControl tabControl1;
		internal Avalonia.Controls.TabItem tabPage4;
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

		public ShpeForm()
		{
			// Build controls
			label8  = new Avalonia.Controls.TextBlock { Text = "Filename:" };
			tbnodeflname = new Avalonia.Controls.TextBox { Text = "" };

			label9  = new Avalonia.Controls.TextBlock { Text = "Enabled?:" };
			label20 = new Avalonia.Controls.TextBlock { Text = "Dependant:" };
			label11 = new Avalonia.Controls.TextBlock { Text = "Index:" };

			tbnode1 = new Avalonia.Controls.TextBox { Text = "0x00" };
			tbnode2 = new Avalonia.Controls.TextBox { Text = "0x00" };
			tbnode3 = new Avalonia.Controls.TextBox { Text = "0x00000000" };

			lbnode = new Avalonia.Controls.ListBox();
			lbnode.SelectionChanged += SelectNode;

			linkLabel9  = new Avalonia.Controls.Button { Content = "delete", IsVisible = false };
			linkLabel10 = new Avalonia.Controls.Button { Content = "add",    IsVisible = false };
			linkLabel9.Click  += linkLabel9_LinkClicked;
			linkLabel10.Click += linkLabel10_LinkClicked;

			// tabPage4 content
			var page4Content = new StackPanel { Orientation = Orientation.Vertical, Spacing = 4 };
			page4Content.Children.Add(label8);
			page4Content.Children.Add(tbnodeflname);
			page4Content.Children.Add(lbnode);
			page4Content.Children.Add(label9);
			page4Content.Children.Add(tbnode1);
			page4Content.Children.Add(label20);
			page4Content.Children.Add(tbnode2);
			page4Content.Children.Add(label11);
			page4Content.Children.Add(tbnode3);
			page4Content.Children.Add(linkLabel10);
			page4Content.Children.Add(linkLabel9);

			tabPage4 = new Avalonia.Controls.TabItem { Header = "Graph Node", Content = page4Content };

			tabControl1 = new Avalonia.Controls.TabControl();
			tabControl1.Items.Add(tabPage4);

			this.Content = tabControl1;
		}

		public void Dispose()
		{
			// no-op
		}

		private void UpdateLists()
		{
			try
			{
				SimPe.Plugin.Shape shape = (SimPe.Plugin.Shape)this.Tag;

				ObjectGraphNodeItem[] ogni = new ObjectGraphNodeItem[lbnode.Items.Count];
				for (int i = 0; i < ogni.Length; i++) ogni[i] = (ObjectGraphNodeItem)lbnode.Items[i];
				shape.GraphNode.Items = ogni;
			}
			catch (Exception) { }
		}

		private void Commit(object sender, System.EventArgs e)
		{
		}

		private void SelectNode(object sender, Avalonia.Controls.SelectionChangedEventArgs e)
		{
			if (lbnode.Tag != null) return;
			if (lbnode.SelectedIndex < 0) return;

			try
			{
				lbnode.Tag = true;
				ObjectGraphNodeItem item = (ObjectGraphNodeItem)lbnode.Items[lbnode.SelectedIndex];
				tbnode1.Text = "0x" + Helper.HexString(item.Enabled);
				tbnode2.Text = "0x" + Helper.HexString(item.Dependant);
				tbnode2.Text = "0x" + Helper.HexString(item.Index);
			}
			catch (Exception) { }
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
				val.Enabled   = Convert.ToByte(tbnode1.Text, 16);
				val.Dependant = Convert.ToByte(tbnode2.Text, 16);
				val.Index     = Convert.ToUInt32(tbnode3.Text, 16);

				lbnode.Items.Add(val);
				UpdateLists();
			}
			catch (Exception) { }
		}

		private void linkLabel9_LinkClicked(object sender, Avalonia.Interactivity.RoutedEventArgs e)
		{
			if (lbnode.SelectedIndex < 0) return;
			lbnode.Items.RemoveAt(lbnode.SelectedIndex);
			UpdateLists();
		}
	}
}
