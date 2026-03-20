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

public class Vector2Collection : IDisposable, IElementCollection, IEnumerable
{
	private ArrayList list;

	public int Count => list.Count;

	public Vector2 this[int index] => (Vector2)list[index];

	public Vector2Collection()
	{
		list = new ArrayList();
	}

	public void Add(double x, double y)
	{
		Add(new Vector2(x, y));
	}

	public void Add(object o)
	{
		if (!(o is Vector2))
		{
			throw new GeometryException("This collection takes only Instances of the class Ambertation.Vector2.");
		}
		Add((Vector2)o);
	}

	public void Add(Vector2 v)
	{
		list.Add(v);
	}

	public bool Contains(Vector2 v)
	{
		return list.Contains(v);
	}

	public int ContainsAt(Vector2 v)
	{
		return ContainsAt(v, 0);
	}

	public int ContainsAt(Vector2 v, int start)
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

	public void Remove(Vector2 v)
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
		return (Vector2)list[index];
	}

	public void SetItem(int index, object o)
	{
		if (index >= 0 && index < Count)
		{
			if (!(o is Vector2))
			{
				throw new Exception("This collection takes only Instances of the class Ambertation.Vector2.");
			}
			list[index] = o as Vector2;
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

	public void CopyTo(Vector2Collection v, bool clear)
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
