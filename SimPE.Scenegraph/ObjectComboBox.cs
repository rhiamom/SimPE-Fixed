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
using SimPe.Cache;

namespace SimPe.PackedFiles.Wrapper
{
	/// <summary>
	/// Summary description for SimComboBox.
	/// </summary>
	[System.ComponentModel.DefaultEvent("SelectedObjectChanged")]
	public class ObjectComboBox : Avalonia.Controls.UserControl
	{
		static MemoryCacheFile cachefile;

		/// <summary>
		/// Returns the MemoryObject Cache
		/// </summary>
		public static MemoryCacheFile ObjectCache
		{
			get
			{
				if (cachefile == null) cachefile = MemoryCacheFile.InitCacheFile();
				return cachefile;
			}
		}

		private Avalonia.Controls.ComboBox cb;

		public ObjectComboBox()
		{
			cb = new Avalonia.Controls.ComboBox();
			cb.SelectionChanged += cb_SelectionChanged;

			var panel = new Avalonia.Controls.StackPanel();
			panel.Children.Add(cb);
			Content = panel;

			loaded = false;
			si = true;
			sm = false;
			st = false;
			sjd = false;
			sa = false;
			sb = false;
			sk = false;
		}

		void SetContent()
		{
			try
			{
				if (!loaded) return;

				cb.Items.Clear();
				var list = new System.Collections.Generic.List<SimPe.Interfaces.IAlias>();
				foreach (SimPe.Cache.MemoryCacheItem mci in ObjectCache.List)
				{
					bool use = false;
					if (this.ShowInventory && mci.IsInventory && !mci.IsToken && !mci.IsMemory && !mci.IsJobData) use = true;
					if (this.ShowTokens && mci.IsToken) use = true;
					if (this.ShowMemories && !mci.IsToken && mci.IsMemory) use = true;
					if (this.ShowJobData && mci.IsJobData) use = true;
					if (this.ShowAspiration && mci.IsAspiration) use = true;
					if (this.ShowBadge && mci.IsBadge) use = true;
					if (this.ShowSkill && mci.IsSkill) use = true;

					if (!use) continue;

					SimPe.Interfaces.IAlias a = new SimPe.Data.StaticAlias(mci.Guid,
						mci.Name + " {" + mci.ObjdName + "}",
						new object[] { mci });

					list.Add(a);
				}
				list.Sort((x, y) => string.Compare(x.ToString(), y.ToString(), StringComparison.Ordinal));
				foreach (var a in list)
					cb.Items.Add(a);
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
			}
		}

		bool si, st, sm, sjd, sa, sb, sk;
		public bool ShowTokens
		{
			get { return st; }
			set { if (st != value) { st = value; SetContent(); } }
		}

		public bool ShowAspiration
		{
			get { return sa; }
			set { if (sa != value) { sa = value; SetContent(); } }
		}

		public bool ShowBadge
		{
			get { return sb; }
			set { if (sb != value) { sb = value; SetContent(); } }
		}

		public bool ShowSkill
		{
			get { return sk; }
			set { if (sk != value) { sk = value; SetContent(); } }
		}

		public bool ShowMemories
		{
			get { return sm; }
			set { if (sm != value) { sm = value; SetContent(); } }
		}

		public bool ShowInventory
		{
			get { return si; }
			set { if (si != value) { si = value; SetContent(); } }
		}

		public bool ShowJobData
		{
			get { return sjd; }
			set { if (sjd != value) { sjd = value; SetContent(); } }
		}

		public uint SelectedGuid
		{
			get
			{
				SimPe.Cache.MemoryCacheItem mci = SelectedItem;
				if (mci == null) return 0xffffffff;
				return mci.Guid;
			}
			set
			{
				int id = -1;
				int ct = 0;
				foreach (SimPe.Interfaces.IAlias a in cb.Items)
				{
					SimPe.Cache.MemoryCacheItem mci = a.Tag[0] as SimPe.Cache.MemoryCacheItem;
					if (mci.Guid == value)
					{
						id = ct;
						break;
					}
					ct++;
				}
				cb.SelectedIndex = id;
			}
		}

		public SimPe.Cache.MemoryCacheItem SelectedItem
		{
			get
			{
				if (cb.SelectedItem == null) return null;
				SimPe.Interfaces.IAlias a = cb.SelectedItem as SimPe.Interfaces.IAlias;
				return a.Tag[0] as SimPe.Cache.MemoryCacheItem;
			}
			set
			{
				int id = -1;
				if (value != null)
				{
					int ct = 0;
					foreach (SimPe.Interfaces.IAlias a in cb.Items)
					{
						SimPe.Cache.MemoryCacheItem mci = a.Tag[0] as SimPe.Cache.MemoryCacheItem;
						if (mci.Guid == value.Guid)
						{
							id = ct;
							break;
						}
						ct++;
					}
				}
				cb.SelectedIndex = id;
			}
		}

		bool loaded;
		public void Reload()
		{
			loaded = true;
			SetContent();
		}

		public event EventHandler SelectedObjectChanged;
		private void cb_SelectionChanged(object sender, Avalonia.Controls.SelectionChangedEventArgs e)
		{
			if (SelectedObjectChanged != null) SelectedObjectChanged(this, new EventArgs());
		}
	}
}
