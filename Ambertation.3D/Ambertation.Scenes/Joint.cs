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
 
 using Ambertation.Scenes.Collections;

namespace Ambertation.Scenes;

public class Joint : Node
{
	public Joint Parent => parent as Joint;

	public JointCollectionBase Childs => childs as JointCollectionBase;

	internal Joint(Joint parent, Scene owner)
		: this(parent, "", owner)
	{
	}

	internal Joint(Joint parent, string name, Scene owner)
		: base(parent, name, owner)
	{
		childs = new JointCollectionBase();
	}

	public Joint CreateChild()
	{
		return CreateChild("");
	}

	public Joint CreateChild(string name)
	{
		Joint joint = new Joint(this, name, owner);
		childs.DoAdd(joint);
		return joint;
	}

	public int GetAssignedVertexCount()
	{
		int num = 0;
		foreach (Mesh item in owner.MeshCollection)
		{
			foreach (Envelope envelope in item.Envelopes)
			{
				if (envelope.Joint != this)
				{
					continue;
				}
				foreach (double weight in envelope.Weights)
				{
					if (weight > 0.0)
					{
						num++;
					}
				}
			}
		}
		return num;
	}

	public override string ToString()
	{
		return base.Name + " [" + GetType().Name + "]";
	}

	public void ClearTag(bool child)
	{
		base.Tag = null;
		if (!child)
		{
			return;
		}
		foreach (Joint child2 in childs)
		{
			child2.ClearTag(child: true);
		}
	}

	public Joint FindJoint(string name)
	{
		foreach (Joint child in childs)
		{
			if (child.Name == name)
			{
				return child;
			}
			Joint joint2 = child.FindJoint(name);
			if (joint2 != null)
			{
				return joint2;
			}
		}
		return null;
	}

	public override int GetHashCode()
	{
		return name.GetHashCode();
	}

	public override bool Equals(object obj)
	{
		if (obj is Joint)
		{
			return name == ((Joint)obj).name;
		}
		return base.Equals(obj);
	}
}
