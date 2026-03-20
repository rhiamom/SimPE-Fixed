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

public class QuaternionCollection : IDisposable, IElementCollection, IEnumerable
{
	private ArrayList list;

	public int Count => list.Count;

	public Quaternion this[int index] => (Quaternion)list[index];

	public QuaternionCollection()
	{
		list = new ArrayList();
	}

	public void Add(object o)
	{
		if (!(o is Quaternion))
		{
			throw new GeometryException("This collection takes only Instances of the class Ambertation.Quaternion.");
		}
		Add((Quaternion)o);
	}

	public void Add(Quaternion q)
	{
		list.Add(q);
	}

	public bool Contains(Quaternion q)
	{
		return list.Contains(q);
	}

	public int ContainsAt(Quaternion v)
	{
		return ContainsAt(v, 0);
	}

	public int ContainsAt(Quaternion v, int start)
	{
		for (int i = start; i < Count; i++)
		{
			if (v.Equals(this[i]))
			{
				return i;
			}
		}
		return -1;
	}

	public void Remove(Quaternion v)
	{
		list.Remove(v);
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
		return (Quaternion)list[index];
	}

	public void SetItem(int index, object o)
	{
		if (index >= 0 && index < Count)
		{
			if (!(o is Quaternion))
			{
				throw new Exception("This collection takes only Instances of the class Ambertation.Quaternion.");
			}
			list[index] = o as Quaternion;
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

	public void CopyTo(QuaternionCollection v, bool clear)
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
