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
 
 using System.Collections;

namespace Ambertation.Scenes.Collections;

public class JointHierarchyIteration : IEnumerator, IEnumerable
{
	private Scene scn;

	private Stack jstack;

	private Stack jindex;

	protected Joint CurrentJoint
	{
		get
		{
			if (jstack.Count == 0)
			{
				return null;
			}
			return jstack.Peek() as Joint;
		}
	}

	protected int CurrentIndex
	{
		get
		{
			if (jindex.Count == 0)
			{
				return 0;
			}
			return (int)jindex.Peek();
		}
	}

	public object Current => CurrentJoint;

	internal JointHierarchyIteration(Scene s)
	{
		scn = s;
		jstack = new Stack();
		jindex = new Stack();
		Reset();
	}

	public bool MoveNext()
	{
		int currentIndex = CurrentIndex;
		if (CurrentJoint == null)
		{
			Reset();
			return false;
		}
		if (currentIndex >= CurrentJoint.Childs.Count)
		{
			jstack.Pop();
			jindex.Pop();
			if (jindex.Count == 0)
			{
				Reset();
				return false;
			}
			return MoveNext();
		}
		jindex.Pop();
		jindex.Push(currentIndex + 1);
		Joint obj = CurrentJoint.Childs[currentIndex];
		jstack.Push(obj);
		jindex.Push(0);
		return true;
	}

	public void Reset()
	{
		jstack.Clear();
		jindex.Clear();
		jstack.Push(scn.RootJoint);
		jindex.Push(0);
	}

	public IEnumerator GetEnumerator()
	{
		return this;
	}
}
