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
using System.Drawing;
using Ambertation.Geometry;

namespace Ambertation;

public static class Helpers
{
	public static string ForceLength(string s, int len, char fill, bool front)
	{
		if (front)
		{
			while (s.Length < len)
			{
				s = fill + s;
			}
			if (s.Length > len)
			{
				s = s.Substring(s.Length - len);
			}
		}
		else
		{
			while (s.Length < len)
			{
				s += fill;
			}
			if (s.Length > len)
			{
				s = s.Substring(0, len);
			}
		}
		return s;
	}

	public static string RemoveDoubleSpaces(string s)
	{
		if (s == null)
		{
			return "";
		}
		while (s.IndexOf("  ") >= 0)
		{
			s = s.Replace("  ", " ");
		}
		return s.Trim();
	}

	public static Vector4 ToVector4(Color c)
	{
		return new Vector4((float)(int)c.R / 255f, (float)(int)c.G / 255f, (float)(int)c.B / 255f, (float)(int)c.A / 255f);
	}

	public static Vector3 ToVector3(Color c)
	{
		return new Vector3((float)(int)c.R / 255f, (float)(int)c.G / 255f, (float)(int)c.B / 255f);
	}

	public static Color ToColor(Vector4 v)
	{
		return Color.FromArgb((int)(v.W * 255.0), (int)(v.X * 255.0), (int)(v.Y * 255.0), (int)(v.Z * 255.0));
	}

	public static Color ToColor(Vector3 v)
	{
		return Color.FromArgb((int)(v.X * 255.0), (int)(v.Y * 255.0), (int)(v.Z * 255.0));
	}

	public static int ToInt(string s)
	{
		return ToInt(s, 0);
	}

	public static int ToInt(string s, int def)
	{
		try
		{
			return Convert.ToInt32(s);
		}
		catch
		{
			return def;
		}
	}

	public static float ToFloat(string s)
	{
		return ToInt(s, 0);
	}

	public static float ToFloat(string s, float def)
	{
		try
		{
			return Convert.ToSingle(s);
		}
		catch
		{
			return def;
		}
	}

	public static DateTime ToDateTime(string s, DateTime def)
	{
		try
		{
			return Convert.ToDateTime(s);
		}
		catch
		{
			return def;
		}
	}

	public static string ToString(byte[] data)
	{
		string text = "";
		foreach (byte b in data)
		{
			text += (char)b;
		}
		return text;
	}

	public static double RadToDeg(double rad)
	{
		return rad * 180.0 / Math.PI;
	}

	public static double DegToRad(double deg)
	{
		return deg * Math.PI / 180.0;
	}
}
