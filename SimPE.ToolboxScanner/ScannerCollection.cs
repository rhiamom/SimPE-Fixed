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
using SimPe.Interfaces.Plugin.Scanner;

namespace SimPe.Plugin.Scanner
{
	/// <summary>
	/// Summary description for ScannerCollection.
	/// </summary>
	public class ScannerCollection : System.Collections.IEnumerable, System.IDisposable
	{
		ArrayList list;
		internal ScannerCollection()
		{
			list = new ArrayList();
		}

		public virtual void Add(IScannerPluginBase item)
		{
			if (item==null) return;
			list.Add(item);
		}

		public int Count
		{
			get {return list.Count;}
		}

		public bool Contains(IScannerPluginBase item)
		{
			return list.Contains(item);
		}

		public void Sort(System.Collections.IComparer cmp)
		{
			list.Sort(cmp);
		}

		internal void Clear()
		{
			list.Clear();
		}

		public IScannerPluginBase this[int index]
		{
			get {return list[index] as IScannerPluginBase;}
		}

		#region IEnumerable Member

		public System.Collections.IEnumerator GetEnumerator()
		{
			return list.GetEnumerator();
		}

		#endregion

		#region IDisposable Member

		public void Dispose()
		{
			if (list!=null) list.Clear();
			list = null;
		}

		#endregion
	}
}
