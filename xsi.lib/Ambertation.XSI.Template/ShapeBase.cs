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
using Ambertation.Geometry.Collections;

namespace Ambertation.XSI.Template;

public class ShapeBase : ArgumentContainer
{
	public enum Layouts
	{
		ORDERED,
		INDEXED
	}

	public enum ElementTypes
	{
		POSITION = 1,
		NORMAL = 2,
		COLOR = 4,
		TEX_COORD_UV0 = 8,
		TEX_COORD_UV1 = 0x10,
		TEX_COORD_UV2 = 0x20,
		TEX_COORD_UV3 = 0x40,
		TEX_COORD_UV = 0x80
	}

	protected Hashtable map;

	internal ShapeBase(Container parent, string args)
		: base(parent, args)
	{
		map = new Hashtable();
		Reset();
	}

	protected override void PrepareSerialize()
	{
		base.PrepareSerialize();
	}

	protected virtual void Reset()
	{
		map.Clear();
		Array values = Enum.GetValues(typeof(ElementTypes));
		foreach (ElementTypes item in values)
		{
			map[item.ToString()] = CreateElementList(item);
		}
	}

	protected IElementCollection GetElementList(ElementTypes t)
	{
		return (IElementCollection)map[t.ToString()];
	}

	protected virtual IElementCollection CreateElementList(ElementTypes t)
	{
		switch (t)
		{
		case ElementTypes.POSITION:
		case ElementTypes.NORMAL:
			return new Vector3Collection();
		case ElementTypes.COLOR:
			return new Vector4Collection();
		default:
			return new Vector2Collection();
		}
	}
}
