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
using System.ComponentModel;

namespace Ambertation.Geometry;

[TypeConverter(typeof(ExpandableObjectConverter))]
public class Vector2
{
	protected double x;

	protected double y;

	public static Vector2 Zero => new Vector2(0.0, 0.0);

	public double X
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

	public double Y
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

	public Vector2(double x, double y)
	{
		this.x = x;
		this.y = y;
	}

	public override string ToString()
	{
		return x + ";" + y;
	}

	public override int GetHashCode()
	{
		return (int)X;
	}

	public override bool Equals(object obj)
	{
		if (obj is Vector2)
		{
			Vector2 vector = obj as Vector2;
			return Math.Abs(vector.x - x) < 1.401298464324817E-45 && Math.Abs(vector.x - x) < 1.401298464324817E-45;
		}
		return base.Equals(obj);
	}
}
