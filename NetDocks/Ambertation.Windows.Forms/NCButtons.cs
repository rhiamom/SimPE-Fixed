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
 
 using System.Windows.Forms;

namespace Ambertation.Windows.Forms;

public class NCButtons
{
	private bool left;

	private bool right;

	private bool mid;

	internal bool LeftInt
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

	internal bool RightInt
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

	internal bool MiddleInt
	{
		get
		{
			return mid;
		}
		set
		{
			mid = value;
		}
	}

	public bool Left => left;

	public bool Right => right;

	public bool Middle => mid;

	internal MouseButtons ToMouseButtons()
	{
		if (Left)
		{
			return MouseButtons.Left;
		}
		if (Right)
		{
			return MouseButtons.Right;
		}
		if (Middle)
		{
			return MouseButtons.Middle;
		}
		return MouseButtons.None;
	}

	internal void Reset()
	{
		left = false;
		right = false;
		mid = false;
	}

	public override string ToString()
	{
		if (!Left && !Right && !Middle)
		{
			return "None";
		}
		string text = "";
		if (Left)
		{
			text += "Left ";
		}
		if (Right)
		{
			text += "Right ";
		}
		if (Middle)
		{
			text += "Middele ";
		}
		return text;
	}
}
