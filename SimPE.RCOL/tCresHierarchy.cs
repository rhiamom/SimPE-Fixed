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
	public class Cres : Avalonia.Controls.TabItem
	{
		internal Avalonia.Controls.TreeView cres_tv;
		private Avalonia.Controls.TextBlock label58;
		internal Avalonia.Controls.TextBox tbfjoint;

		public Cres()
		{
			this.Header = SimPe.Localization.GetString("CRES Hierarchie");

			tbfjoint = new Avalonia.Controls.TextBox { Text = "" };
			tbfjoint.TextChanged += new EventHandler<Avalonia.Controls.TextChangedEventArgs>(this.tbfjoint_TextChanged);
			label58 = new Avalonia.Controls.TextBlock { Text = "Find Joint:" };
			cres_tv = new Avalonia.Controls.TreeView();
			cres_tv.SelectionChanged += new EventHandler<Avalonia.Controls.SelectionChangedEventArgs>(this.SelectCresTv);

			Content = new Avalonia.Controls.StackPanel { Children = { label58, tbfjoint, cres_tv } };
		}

		internal void ClearCresTv()
		{
			if (cres_tv==null) return;
			try
			{
				ClearCresTv(cres_tv.Items);
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex);
			}
		}

		void ClearCresTv(Avalonia.Controls.ItemCollection nodes)
		{
			foreach (Avalonia.Controls.TreeViewItem n in nodes)
			{
				n.Tag = null;
				ClearCresTv(n.Items);
			}

			nodes.Clear();
		}

		private void tbfjoint_TextChanged(object sender, System.EventArgs e)
		{
			tbfjoint.Tag = true;
			try
			{
				string name = tbfjoint.Text.Trim().ToLower();
				if (name!="")
					SelectJoint(cres_tv.Items, name);
			}
			finally
			{
				tbfjoint.Tag = null;
			}
		}

		private void SelectCresTv(object sender, Avalonia.Controls.SelectionChangedEventArgs e)
		{
			if (tbfjoint.Tag!=null) return;
			if (e==null) return;
			if (cres_tv.SelectedItem == null) return;
			Avalonia.Controls.TreeViewItem node = cres_tv.SelectedItem as Avalonia.Controls.TreeViewItem;
			if (node == null) return;
			if (node.Tag==null) return;

			int index = (int)node.Tag;
			if (index<0) return;

			Avalonia.Controls.ComboBox cb = (Avalonia.Controls.ComboBox)(((Avalonia.Controls.TabControl)this.Parent).Tag);
			cb.SelectedIndex = index;
			((Avalonia.Controls.TabControl)this.Parent).SelectedIndex = 0;
		}

		bool SelectJoint(Avalonia.Controls.ItemCollection nodes, string name)
		{
			foreach (Avalonia.Controls.TreeViewItem tn in nodes)
			{
				if (tn.Tag!=null)
				{
					Avalonia.Controls.ComboBox cb = (Avalonia.Controls.ComboBox)(((Avalonia.Controls.TabControl)this.Parent).Tag);

					object o = (cb.Items[(int)tn.Tag] as CountedListItem).Object;
					if ( o is AbstractCresChildren)
					{
						if (((AbstractCresChildren)o).GetName().Trim().ToLower().StartsWith(name))
						{
							cres_tv.SelectedItem = tn;
							return true;
						}
					}
				}
				if (SelectJoint(tn.Items, name)) return true;
			}

			return false;
		}
	}
}
