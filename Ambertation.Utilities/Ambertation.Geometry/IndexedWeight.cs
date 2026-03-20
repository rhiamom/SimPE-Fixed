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
public class IndexedWeight
{
	private double w;

	private int index;

	public double Weight
	{
		get
		{
			return w;
		}
		set
		{
			w = value;
		}
	}

	public int Index
	{
		get
		{
			return index;
		}
		set
		{
			index = value;
		}
	}

	public IndexedWeight(Vector2 v)
		: this((int)v.X, v.Y)
	{
	}

	public IndexedWeight(int index, double w)
	{
		this.index = index;
		this.w = w;
	}

	public static implicit operator Vector2(IndexedWeight w)
	{
		return new Vector2(w.Index, w.Weight);
	}

	public static implicit operator IndexedWeight(Vector2 v)
	{
		return new IndexedWeight(v);
	}

	public override string ToString()
	{
		return index + ":" + w;
	}
}
