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
public class PointUV
{
	private int u;

	private int v;

	public int U
	{
		get
		{
			return u;
		}
		set
		{
			u = value;
		}
	}

	public int V
	{
		get
		{
			return v;
		}
		set
		{
			v = value;
		}
	}

	public PointUV(double u, double v)
		: this((int)u, (int)v)
	{
	}

	public PointUV(int u, int v)
	{
		this.u = u;
		this.v = v;
	}

	public static implicit operator PointUV(Vector2 v)
	{
		return new PointUV((int)v.X, (int)v.Y);
	}

	public override string ToString()
	{
		return u + ";" + v;
	}

	public override int GetHashCode()
	{
		return base.GetHashCode();
	}

	public override bool Equals(object obj)
	{
		if (obj is PointUV)
		{
			PointUV pointUV = obj as PointUV;
			return pointUV.U == U && pointUV.V == V;
		}
		return base.Equals(obj);
	}
}
