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

namespace SimPe.Plugin
{
	/// <summary>
	/// Summary description for Listing.
	/// </summary>
	public class Listing : Avalonia.Controls.UserControl
	{
		private Avalonia.Controls.ListBox lb;

		public Listing()
		{
			lb = new Avalonia.Controls.ListBox
			{
				SelectionMode = Avalonia.Controls.SelectionMode.Multiple
			};
			Content = lb;
		}

		public void ShowDialog() { }
		public void Close() { }

		public WorkshopMMAT[] Execute(WorkshopMMAT[] list)
		{
			lb.Items.Clear();
			foreach (WorkshopMMAT s in list)
			{
				lb.Items.Add(s);
			}
			if (lb.Items.Count > 0) lb.SelectedIndex = 0;

			this.ShowDialog();

			WorkshopMMAT[] str;
			var selectedItems = lb.SelectedItems;
			if (selectedItems != null && selectedItems.Count > 0)
			{
				str = new WorkshopMMAT[selectedItems.Count];
				for (int i = 0; i < selectedItems.Count; i++) str[i] = (WorkshopMMAT)selectedItems[i];
			}
			else
			{
				str = new WorkshopMMAT[0];
			}
			return str;
		}

		public string[] Execute(string[] list)
		{
			lb.Items.Clear();
			foreach (string s in list)
			{
				lb.Items.Add(s);
			}
			if (lb.Items.Count > 0) lb.SelectedIndex = 0;

			this.ShowDialog();

			string[] str;
			var selectedItems = lb.SelectedItems;
			if (selectedItems != null && selectedItems.Count > 0)
			{
				str = new string[selectedItems.Count];
				for (int i = 0; i < selectedItems.Count; i++) str[i] = (string)selectedItems[i];
			}
			else
			{
				str = new string[0];
			}
			return str;
		}
	}
}
