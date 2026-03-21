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
	public class ShpeParts : Avalonia.Controls.TabItem
	{
		private Avalonia.Controls.TextBlock label5;
		internal Avalonia.Controls.ListBox lbpart;
		private Avalonia.Controls.TextBox tbparttype;
		private Avalonia.Controls.TextBox tbpartdsc;
		private Avalonia.Controls.TextBlock label6;
		private Avalonia.Controls.TextBox tbpartdata;
		private Avalonia.Controls.TextBlock label7;
		private Avalonia.Controls.Button linkLabel7;
		private Avalonia.Controls.Button linkLabel8;

		public ShpeParts()
		{
			this.Header = "Parts";

			lbpart = new Avalonia.Controls.ListBox();
			lbpart.SelectionChanged += new EventHandler<Avalonia.Controls.SelectionChangedEventArgs>(this.SelectPart);
			label5 = new Avalonia.Controls.TextBlock { Text = "Subset Name:" };
			tbparttype = new Avalonia.Controls.TextBox { Text = "" };
			tbparttype.TextChanged += new EventHandler<Avalonia.Controls.TextChangedEventArgs>(this.ChangedPart);
			label6 = new Avalonia.Controls.TextBlock { Text = "Material Definition File:" };
			tbpartdsc = new Avalonia.Controls.TextBox { Text = "" };
			tbpartdsc.TextChanged += new EventHandler<Avalonia.Controls.TextChangedEventArgs>(this.ChangedPart);
			label7 = new Avalonia.Controls.TextBlock { Text = "Data:" };
			tbpartdata = new Avalonia.Controls.TextBox { Text = "" };
			tbpartdata.TextChanged += new EventHandler<Avalonia.Controls.TextChangedEventArgs>(this.ChangedPart);
			linkLabel8 = new Avalonia.Controls.Button { Content = "add" };
			linkLabel8.Click += new EventHandler<Avalonia.Interactivity.RoutedEventArgs>(this.linkLabel8_LinkClicked);
			linkLabel7 = new Avalonia.Controls.Button { Content = "delete" };
			linkLabel7.Click += new EventHandler<Avalonia.Interactivity.RoutedEventArgs>(this.linkLabel7_LinkClicked);

			Content = new Avalonia.Controls.StackPanel { Children = {
				lbpart, label5, tbparttype, label6, tbpartdsc, label7, tbpartdata,
				linkLabel8, linkLabel7
			}};
		}

		private void SelectPart(object sender, System.EventArgs e)
		{
			if (lbpart.Tag!=null) return;
			if (lbpart.SelectedIndex<0) return;

			try
			{
				lbpart.Tag = true;
				ShapePart item = (ShapePart)lbpart.Items[lbpart.SelectedIndex];
				tbparttype.Text = item.Subset;
				tbpartdsc.Text = item.FileName;

				string s = "";
				foreach (byte b in item.Data) s += Helper.HexString(b)+" ";
				tbpartdata.Text = s;
			}
			catch (Exception){}
			finally
			{
				lbpart.Tag = null;
			}
		}

		private void ChangedPart(object sender, System.EventArgs e)
		{
			if (lbpart.Tag!=null) return;
			if (lbpart.SelectedIndex<0) return;

			try
			{
				lbpart.Tag = true;
				ShapePart item = (ShapePart)lbpart.Items[lbpart.SelectedIndex];
				item.Subset = tbparttype.Text;
				item.FileName = tbpartdsc.Text;

				string[] tokens = tbpartdata.Text.Trim().Split(" ".ToCharArray());
				byte[] data = new byte[tokens.Length];
				for (int i=0; i<data.Length; i++) data[i] = Convert.ToByte(tokens[i]);
				item.Data = data;

				lbpart.Items[lbpart.SelectedIndex] = item;
			}
			catch (Exception){}
			finally
			{
				lbpart.Tag = null;
			}
		}

		private void UpdateLists()
		{
			try
			{
				SimPe.Plugin.Shape shape = (SimPe.Plugin.Shape)this.Tag;

				ShapePart[] parts = new ShapePart[lbpart.Items.Count];
				for (int i=0; i<parts.Length; i++) parts[i] = (ShapePart)lbpart.Items[i];
				shape.Parts = parts;
			}
			catch (Exception){}
		}

		private void linkLabel8_LinkClicked(object sender, Avalonia.Interactivity.RoutedEventArgs e)
		{
			try
			{
				SimPe.Plugin.Shape shape = (SimPe.Plugin.Shape)this.Tag;

				ShapePart val = new ShapePart();
				val.Subset = tbparttype.Text;
				val.FileName = tbpartdsc.Text;
				val.Data = Helper.SetLength(Helper.HexListToBytes(tbpartdata.Text), 9);

				lbpart.Items.Add(val);
				UpdateLists();
			}
			catch (Exception) {}
		}

		private void linkLabel7_LinkClicked(object sender, Avalonia.Interactivity.RoutedEventArgs e)
		{
			if (lbpart.SelectedIndex < 0) return;
			lbpart.Items.RemoveAt(lbpart.SelectedIndex);
			UpdateLists();
		}
	}
}
