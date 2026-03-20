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

namespace Ambertation.Scenes.Collections;

public class NodeCollectionBase : IDisposable, IEnumerable
{
	protected ArrayList list;

	public int Count => list.Count;

	public NodeCollectionBase()
	{
		list = new ArrayList();
	}

	internal void DoAdd(Node pd)
	{
		list.Add(pd);
	}

	public bool Contains(Node pd)
	{
		return list.Contains(pd);
	}

	public void Remove(Node pd)
	{
		list.Remove(pd);
	}

	public void Clear()
	{
		list.Clear();
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

	public Node GetItem(int index)
	{
		return (Node)list[index];
	}

	public Node GetItem(string name)
	{
		IEnumerator enumerator = GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				Node node = (Node)enumerator.Current;
				if (node.Name == name)
				{
					return node;
				}
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
		return null;
	}
}
