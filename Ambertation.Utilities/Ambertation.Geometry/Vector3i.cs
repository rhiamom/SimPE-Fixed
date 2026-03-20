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
 
 using System.ComponentModel;

namespace Ambertation.Geometry;

[TypeConverter(typeof(ExpandableObjectConverter))]
public class Vector3i
{
	private int x;

	private int y;

	private int z;

	public static Vector3i Zero => new Vector3i(0, 0, 0);

	public int X
	{
		get
		{
			return x;
		}
		set
		{
			x = value;
		}
	}

	public int Y
	{
		get
		{
			return y;
		}
		set
		{
			y = value;
		}
	}

	public int Z
	{
		get
		{
			return z;
		}
		set
		{
			z = value;
		}
	}

	public int this[int index]
	{
		get
		{
			return index switch
			{
				0 => X, 
				1 => Y, 
				_ => Z, 
			};
		}
		set
		{
			if (index == 0)
			{
				X = value;
			}
			if (index == 1)
			{
				Y = value;
			}
			Z = value;
		}
	}

	public Vector3i(int x, int y, int z)
	{
		this.x = x;
		this.y = y;
		this.z = z;
	}

	public override string ToString()
	{
		return x + ";" + y + ";" + z;
	}

	public override int GetHashCode()
	{
		return base.GetHashCode();
	}

	public override bool Equals(object obj)
	{
		if (obj is Vector3i)
		{
			Vector3i vector3i = obj as Vector3i;
			return vector3i.X == X && vector3i.Y == Y && vector3i.Z == Z;
		}
		return base.Equals(obj);
	}
}
