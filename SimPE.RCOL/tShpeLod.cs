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
	public class ShpeLod : Avalonia.Controls.TabItem
	{
		private Avalonia.Controls.TextBox tbunk;
		internal Avalonia.Controls.ListBox lbunk;
		private Avalonia.Controls.TextBlock label1;
		private Avalonia.Controls.Button linkLabel3;
		private Avalonia.Controls.Button linkLabel4;

		public ShpeLod()
		{
			this.Header = "Level of Detail Listing";

			lbunk = new Avalonia.Controls.ListBox();
			lbunk.SelectionChanged += new EventHandler<Avalonia.Controls.SelectionChangedEventArgs>(this.SelectUnknown);
			label1 = new Avalonia.Controls.TextBlock { Text = "Value:" };
			tbunk = new Avalonia.Controls.TextBox { Text = "0x00000000" };
			tbunk.TextChanged += new EventHandler<Avalonia.Controls.TextChangedEventArgs>(this.ChangeUnknown);
			linkLabel3 = new Avalonia.Controls.Button { Content = "add" };
			linkLabel3.Click += new EventHandler<Avalonia.Interactivity.RoutedEventArgs>(this.linkLabel3_LinkClicked);
			linkLabel4 = new Avalonia.Controls.Button { Content = "delete" };
			linkLabel4.Click += new EventHandler<Avalonia.Interactivity.RoutedEventArgs>(this.linkLabel4_LinkClicked);

			Content = new Avalonia.Controls.StackPanel { Children = { lbunk, label1, tbunk, linkLabel3, linkLabel4 }};
		}

		private void UpdateLists()
		{
			try
			{
				SimPe.Plugin.Shape shape = (SimPe.Plugin.Shape)this.Tag;

				uint[] unknown = new uint[lbunk.Items.Count];
				for (int i=0; i<unknown.Length; i++) unknown[i] = (uint)lbunk.Items[i];
				shape.Unknwon = unknown;
			}
			catch (Exception){}
		}

		private void SelectUnknown(object sender, System.EventArgs e)
		{
			if (tbunk.Tag!=null) return;
			if (lbunk.SelectedIndex<0) return;

			try
			{
				tbunk.Tag = true;
				tbunk.Text = "0x"+Helper.HexString((uint)lbunk.Items[lbunk.SelectedIndex]);
			}
			catch (Exception) {}
			finally
			{
				tbunk.Tag = null;
			}
		}

		private void ChangeUnknown(object sender, System.EventArgs e)
		{
			if (tbunk.Tag!=null) return;
			if (lbunk.SelectedIndex<0) return;

			try
			{
				tbunk.Tag = true;
				lbunk.Items[lbunk.SelectedIndex] = Convert.ToUInt32(tbunk.Text, 16);
			}
			catch (Exception) {}
			finally
			{
				tbunk.Tag = null;
			}
		}

		private void linkLabel3_LinkClicked(object sender, Avalonia.Interactivity.RoutedEventArgs e)
		{
			try
			{
				uint val = Convert.ToUInt32(tbunk.Text, 16);
				lbunk.Items.Add(val);
				UpdateLists();
			}
			catch (Exception) {}
		}

		private void linkLabel4_LinkClicked(object sender, Avalonia.Interactivity.RoutedEventArgs e)
		{
			if (lbunk.SelectedIndex < 0) return;
			lbunk.Items.RemoveAt(lbunk.SelectedIndex);
			UpdateLists();
		}
	}
}
