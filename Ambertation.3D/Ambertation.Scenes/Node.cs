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
using Ambertation.Scenes.Collections;

namespace Ambertation.Scenes;

public class Node : Transformation, IEnumerable, IDisposable
{
	protected Node parent;

	protected NodeCollectionBase childs;

	protected string name;

	protected Scene owner;

	private Transformation abspos;

	public Scene Owner => owner;

	public Transformation WorldPosition => abspos;

	public string Name
	{
		get
		{
			if (name == null)
			{
				return "";
			}
			return name;
		}
		set
		{
			name = value;
		}
	}

	public bool Root => parent == null;

	internal NodeCollectionBase NodeChilds => childs;

	internal Node(Node parent, Scene owner)
		: this(parent, "", owner)
	{
	}

	internal Node(Node parent, string name, Scene owner)
	{
		this.owner = owner;
		this.name = name;
		this.parent = parent;
		abspos = new Transformation();
	}

	internal void SetOwner(Scene scn)
	{
		foreach (Node child in childs)
		{
			child.SetOwner(scn);
		}
		owner = scn;
	}

	public IEnumerator GetEnumerator()
	{
		return childs.GetEnumerator();
	}

	internal void Clear(bool childs, bool dispose)
	{
		if (childs)
		{
			for (int i = 0; i < this.childs.Count; i++)
			{
				this.childs.GetItem(i).Clear(childs: true, dispose);
				this.childs.GetItem(i).Dispose();
			}
		}
		this.childs.Clear();
	}

	public virtual void Dispose()
	{
		owner = null;
		parent = null;
		parent = null;
		name = null;
		abspos = null;
		childs.Clear();
	}
}
