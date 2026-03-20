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

namespace Ambertation.Windows.Forms;

[TypeConverter(typeof(ExpandableObjectConverter))]
public class NCResizeBorders
{
	private bool left;

	private bool right;

	private bool top;

	private bool bottom;

	public bool Left
	{
		get
		{
			return left;
		}
		set
		{
			left = value;
		}
	}

	public bool Right
	{
		get
		{
			return right;
		}
		set
		{
			right = value;
		}
	}

	public bool Top
	{
		get
		{
			return top;
		}
		set
		{
			top = value;
		}
	}

	public bool Bottom
	{
		get
		{
			return bottom;
		}
		set
		{
			bottom = value;
		}
	}

	public NCResizeBorders()
		: this(l: false, t: false, r: true, b: true)
	{
	}

	public NCResizeBorders(bool l, bool t, bool r, bool b)
	{
		left = l;
		top = t;
		right = r;
		bottom = b;
	}

	public void SetAll(bool val)
	{
		bottom = val;
		top = val;
		left = val;
		right = val;
	}

	public override string ToString()
	{
		string text = "";
		if (Left)
		{
			text += "[Left] ";
		}
		if (Bottom)
		{
			text += "[Bottom] ";
		}
		if (Top)
		{
			text += "[Top] ";
		}
		if (Right)
		{
			text += "[Right] ";
		}
		text = text.Trim();
		if (text == "")
		{
			text = "[None]";
		}
		return text;
	}
}
