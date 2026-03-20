/***************************************************************************
 *   Copyright (C) 2005 by Ambertation                                     *
 *   quaxi@ambertation.de                                                  *
 *                                                                         *
 *   Copyright (C) 2025 by GramzeSweatshop                                 *
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

namespace Ambertation.Geometry.Collections;

public class IndexedWeightCollection : IDisposable, IElementCollection, IEnumerable
{
	private ArrayList list;

	public int Count => list.Count;

	public IndexedWeight this[int index] => (IndexedWeight)list[index];

	public IndexedWeightCollection()
	{
		list = new ArrayList();
	}

	public void Add(object o)
	{
		if (!(o is IndexedWeight))
		{
			throw new GeometryException("This collection takes only Instances of the class Ambertation.IndexedWeight.");
		}
		Add((IndexedWeight)o);
	}

	public void Add(IndexedWeight pd)
	{
		list.Add(pd);
	}

	public bool Contains(IndexedWeight pd)
	{
		return list.Contains(pd);
	}

	public void Remove(IndexedWeight pd)
	{
		list.Remove(pd);
	}

	public void Clear()
	{
		list.Clear();
	}

	public object GetItem(int index)
	{
		if (index < 0 || index >= Count)
		{
			return null;
		}
		return (IndexedWeight)list[index];
	}

	public void SetItem(int index, object o)
	{
		if (index >= 0 && index < Count)
		{
			if (!(o is IndexedWeight))
			{
				throw new Exception("This collection takes only Instances of the class Ambertation.IndexedWeight.");
			}
			list[index] = o as IndexedWeight;
		}
	}

	public virtual void Dispose()
	{
		if (list != null)
		{
			list.Clear();
		}
		list = null;
	}

	public IEnumerator GetEnumerator()
	{
		return list.GetEnumerator();
	}

	public override string ToString()
	{
		return GetType().Name + " (" + Count + ")";
	}

	public void CopyTo(IndexedWeightCollection v, bool clear)
	{
		if (clear)
		{
			v.Clear();
		}
		IEnumerator enumerator = GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				object current = enumerator.Current;
				v.Add(current);
			}
		}
		finally
		{
			IDisposable disposable = enumerator as IDisposable;
			if (disposable != null)
			{
				disposable.Dispose();
			}
		}
	}
}
