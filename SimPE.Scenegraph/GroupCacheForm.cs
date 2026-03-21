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
using System.Collections.Generic;
using Avalonia.Controls;

namespace SimPe.PackedFiles.UserInterface
{
	/// <summary>
	/// Thin Avalonia wrapper around ListBox that adds WinForms-compatible BeginUpdate/EndUpdate/Sorted API.
	/// </summary>
	internal class SortableListBox : Avalonia.Controls.ListBox
	{
		private readonly System.Collections.ObjectModel.ObservableCollection<object> _items
			= new System.Collections.ObjectModel.ObservableCollection<object>();

		public new SimPe.PackedFiles.UserInterface.SortableListBox.ItemsCollection Items { get; }
			= new SimPe.PackedFiles.UserInterface.SortableListBox.ItemsCollection();

		public bool Sorted { get; set; }

		public void BeginUpdate() { }
		public void EndUpdate()
		{
			if (Sorted)
				Items.Sort();
			ItemsSource = Items.InnerList;
		}

		public class ItemsCollection
		{
			internal readonly System.Collections.ArrayList InnerList = new System.Collections.ArrayList();
			public void Clear() => InnerList.Clear();
			public void Add(object item) => InnerList.Add(item);
			public int Count => InnerList.Count;
			public void Sort() => InnerList.Sort();
		}
	}

	/// <summary>
	/// Summary description for GroupCacheForm.
	/// </summary>
	internal class GroupCacheForm : Avalonia.Controls.UserControl
	{
		internal SortableListBox lbgroup;
		internal Avalonia.Controls.Panel GropPanel;

		public GroupCacheForm()
		{
			lbgroup = new SortableListBox();
			GropPanel = new Avalonia.Controls.Panel();
			GropPanel.Children.Add(lbgroup);
		}

		public void Dispose() { }
	}
}
