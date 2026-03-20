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
using Ambertation.Geometry;
using Ambertation.Geometry.Collections;

namespace Ambertation.XSI.Template;

public sealed class Shape : ShapeBase
{
	private string matn;

	public string MaterialName
	{
		get
		{
			return matn;
		}
		set
		{
			matn = value;
			if (matn == null)
			{
				matn = "";
			}
		}
	}

	public string PrimitiveName
	{
		get
		{
			return base.Argument1.Replace("SHP-", "").Replace("-ORG", "");
		}
		set
		{
			base.Argument1 = "SHP-" + value + "-ORG";
		}
	}

	public Vector3Collection Vertices => (Vector3Collection)GetElementList(ElementTypes.POSITION);

	public Vector3Collection Normals => (Vector3Collection)GetElementList(ElementTypes.NORMAL);

	public Vector4Collection Colors => (Vector4Collection)GetElementList(ElementTypes.COLOR);

	public Vector2Collection TextureCoords => (Vector2Collection)GetElementList(ElementTypes.TEX_COORD_UV);

	public Vector2Collection TextureCoords1 => (Vector2Collection)GetElementList(ElementTypes.TEX_COORD_UV1);

	public Vector2Collection TextureCoords2 => (Vector2Collection)GetElementList(ElementTypes.TEX_COORD_UV2);

	public Vector2Collection TextureCoords3 => (Vector2Collection)GetElementList(ElementTypes.TEX_COORD_UV3);

	public Vector2Collection TextureCoords0 => (Vector2Collection)GetElementList(ElementTypes.TEX_COORD_UV0);

	public Shape(Container parent, string args)
		: base(parent, args)
	{
		matn = "";
	}

	protected override void ResetArgs()
	{
		ResetArgs("SHP-mesh-ORG");
	}

	private void ReadElementOrdered(ref int index, ElementTypes t, IElementCollection list)
	{
		object v;
		switch (t)
		{
		case ElementTypes.POSITION:
		case ElementTypes.NORMAL:
			v = ReadVector3(ref index);
			break;
		case ElementTypes.COLOR:
			v = ReadVector4(ref index);
			break;
		default:
			v = ReadVector2(ref index);
			break;
		}
		list.Add(v);
	}

	private void ReadElementIndexed(ref int index, ElementTypes t, IElementCollection list)
	{
		int ct;
		switch (t)
		{
		case ElementTypes.POSITION:
		case ElementTypes.NORMAL:
			ct = 4;
			break;
		case ElementTypes.COLOR:
			ct = 5;
			break;
		default:
			ct = 3;
			break;
		}
		double[] array = ReadFloatSequence(ref index, ct);
		object zero;
		switch (t)
		{
		case ElementTypes.POSITION:
		case ElementTypes.NORMAL:
			zero = Vector3.Zero;
			break;
		case ElementTypes.COLOR:
			zero = Vector3.Zero;
			break;
		default:
			zero = Vector3.Zero;
			break;
		}
		while (list.Count <= (int)array[0])
		{
			list.Add(zero);
		}
		object o;
		switch (t)
		{
		case ElementTypes.POSITION:
		case ElementTypes.NORMAL:
			o = new Vector3(array[1], array[2], array[3]);
			break;
		case ElementTypes.COLOR:
			o = new Vector4(array[1], array[2], array[3], array[4]);
			break;
		default:
			o = new Vector2(array[1], array[2]);
			break;
		}
		list.SetItem((int)array[0], o);
	}

	private void ReadElementArray(ref int index, Layouts l)
	{
		int num = (int)Line(index++).GetFloat(0);
		ElementTypes elementTypes = (ElementTypes)EnumValue(index++, typeof(ElementTypes));
		IElementCollection elementList = GetElementList(elementTypes);
		if (IsNewTexture(elementTypes))
		{
			matn = Line(index++).StripQuotes();
		}
		if (l == Layouts.ORDERED)
		{
			for (int i = 0; i < num; i++)
			{
				ReadElementOrdered(ref index, elementTypes, elementList);
			}
		}
		else
		{
			for (int j = 0; j < num; j++)
			{
				ReadElementIndexed(ref index, elementTypes, elementList);
			}
		}
		map[elementTypes.ToString()] = elementList;
	}

	protected override void FinishDeSerialize()
	{
		base.FinishDeSerialize();
		Reset();
		int index = 0;
		int num = (int)Line(index++).GetFloat(0);
		Layouts l = (Layouts)EnumValue(index++, typeof(Layouts));
		for (int i = 0; i < num; i++)
		{
			ReadElementArray(ref index, l);
		}
		CustomClear();
	}

	protected override void PrepareSerialize()
	{
		base.PrepareSerialize();
		if (base.Owner.Header.Version < 196688)
		{
			if (TextureCoords.Count == 0 && TextureCoords0.Count >= 0)
			{
				TextureCoords0.CopyTo(TextureCoords, clear: false);
			}
			TextureCoords0.Clear();
			TextureCoords1.Clear();
			TextureCoords2.Clear();
			TextureCoords3.Clear();
		}
		int num = 0;
		foreach (string key in map.Keys)
		{
			if (((IElementCollection)map[key]).Count > 0)
			{
				num++;
			}
		}
		Clear(rec: false);
		AddLiteral(num);
		AddLiteral("\"" + Layouts.ORDERED.ToString() + "\"");
		Array values = Enum.GetValues(typeof(ElementTypes));
		foreach (ElementTypes item in values)
		{
			IElementCollection elementList = GetElementList(item);
			if (elementList.Count == 0)
			{
				continue;
			}
			AddLiteral(elementList.Count);
			AddLiteral("\"" + item.ToString() + "\"");
			switch (item)
			{
			case ElementTypes.POSITION:
			case ElementTypes.NORMAL:
				foreach (Vector3 item2 in elementList)
				{
					WriteVector3(item2, oneline: true);
				}
				continue;
			case ElementTypes.COLOR:
				foreach (Vector4 item3 in elementList)
				{
					WriteVector4(item3, oneline: true);
				}
				continue;
			}
			if (IsNewTexture(item))
			{
				AddQuotedLiteral(matn);
			}
			foreach (Vector2 item4 in elementList)
			{
				WriteVector2(item4, oneline: true);
			}
		}
	}

	private static bool IsNewTexture(ElementTypes e)
	{
		if (e != ElementTypes.TEX_COORD_UV0 && e != ElementTypes.TEX_COORD_UV1 && e != ElementTypes.TEX_COORD_UV2)
		{
			return e == ElementTypes.TEX_COORD_UV3;
		}
		return true;
	}
}
