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
 
 using System.Drawing;
using Ambertation.Geometry;

namespace Ambertation.XSI.Template;

public class ColorContainer : ExtendedContainer
{
	public ColorContainer(Container parent, string args)
		: base(parent, args)
	{
	}

	protected Color ReadColor(ref int startline, bool inclalpha)
	{
		Vector3 vector = ((!inclalpha) ? ReadVector3(ref startline) : ReadVector4(ref startline));
		int red = (int)(vector.X * 255.0);
		int green = (int)(vector.Y * 255.0);
		int blue = (int)(vector.Z * 255.0);
		int alpha = 255;
		if (inclalpha)
		{
			alpha = (int)(((Vector4)vector).W * 255.0);
		}
		return Color.FromArgb(alpha, red, green, blue);
	}

	protected void WriteColor(bool inclalpha, Color cl)
	{
		AddLiteral((float)(int)cl.R / 255f);
		AddLiteral((float)(int)cl.G / 255f);
		AddLiteral((float)(int)cl.B / 255f);
		if (inclalpha)
		{
			AddLiteral((float)(int)cl.A / 255f);
		}
	}
}
