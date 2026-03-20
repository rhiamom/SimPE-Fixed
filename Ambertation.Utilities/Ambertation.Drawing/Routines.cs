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
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

namespace Ambertation.Drawing;

public static class Routines
{
	public static void DrawRoundRect(Graphics g, Pen p, Rectangle rect, int radius)
	{
		DrawRoundRect(g, p, rect.X, rect.Y, rect.Width, rect.Height, radius);
	}

	public static void FillRoundRect(Graphics g, Brush b, Rectangle rect, int radius)
	{
		FillRoundRect(g, b, rect.X, rect.Y, rect.Width, rect.Height, radius);
	}

	public static void DrawRoundRect(Graphics g, Pen p, int x, int y, int width, int height, int radius)
	{
		g.DrawPath(p, RoundRectPath(x, y, width, height, radius));
	}

	public static void FillRoundRect(Graphics g, Brush b, int x, int y, int width, int height, int radius)
	{
		g.FillPath(b, RoundRectPath(x, y, width, height, radius));
	}

	public static GraphicsPath GethRoundRectPath(Rectangle rect, int radius)
	{
		return RoundRectPath(rect.X, rect.Y, rect.Width, rect.Height, radius);
	}

	public static GraphicsPath GethRoundRectPath(int x, int y, int width, int height, int radius)
	{
		return RoundRectPath(x, y, width, height, radius);
	}

	private static GraphicsPath RoundRectPath(int x, int y, int width, int height, int radius)
	{
		GraphicsPath graphicsPath = new GraphicsPath();
		if (radius > 1)
		{
			graphicsPath.AddLine(x + radius, y, x + width - radius, y);
			graphicsPath.AddArc(x + width - radius, y, radius, radius, 270f, 90f);
			graphicsPath.AddLine(x + width, y + radius, x + width, y + height - radius);
			graphicsPath.AddArc(x + width - radius, y + height - radius, radius, radius, 0f, 90f);
			graphicsPath.AddLine(x + width - radius, y + height, x + radius, y + height);
			graphicsPath.AddArc(x, y + height - radius, radius, radius, 90f, 90f);
			graphicsPath.AddLine(x, y + height - radius, x, y + radius);
			graphicsPath.AddArc(x, y, radius, radius, 180f, 90f);
			graphicsPath.CloseFigure();
		}
		else
		{
			graphicsPath.AddRectangle(new Rectangle(x, y, width, height));
		}
		return graphicsPath;
	}

	public static ColorMap[] CloseColors(Color cl, float tolerance, Color target)
	{
		int num = (int)Math.Floor(255f * tolerance);
		int num2 = Math.Max(0, Math.Min(255, cl.R - num));
		int num3 = Math.Max(0, Math.Min(255, cl.R + num));
		int num4 = Math.Max(0, Math.Min(255, cl.G - num));
		int num5 = Math.Max(0, Math.Min(255, cl.G + num));
		int num6 = Math.Max(0, Math.Min(255, cl.B - num));
		int num7 = Math.Max(0, Math.Min(255, cl.B + num));
		ArrayList arrayList = new ArrayList();
		for (int i = num2; i < num3; i++)
		{
			for (int j = num4; j < num5; j++)
			{
				for (int k = num6; k < num7; k++)
				{
					ColorMap colorMap = new ColorMap();
					colorMap.NewColor = target;
					colorMap.OldColor = Color.FromArgb(i, j, k);
					arrayList.Add(colorMap);
				}
			}
		}
		ColorMap[] array = new ColorMap[arrayList.Count];
		arrayList.CopyTo(array);
		return array;
	}

	public static ArrayList CloseColors(Color cl, float tolerance)
	{
		int num = (int)Math.Floor(255f * tolerance);
		int num2 = Math.Max(0, Math.Min(255, cl.R - num));
		int num3 = Math.Max(0, Math.Min(255, cl.R + num));
		int num4 = Math.Max(0, Math.Min(255, cl.G - num));
		int num5 = Math.Max(0, Math.Min(255, cl.G + num));
		int num6 = Math.Max(0, Math.Min(255, cl.B - num));
		int num7 = Math.Max(0, Math.Min(255, cl.B + num));
		ArrayList arrayList = new ArrayList();
		for (int i = num2; i < num3; i++)
		{
			for (int j = num4; j < num5; j++)
			{
				for (int k = num6; k < num7; k++)
				{
					arrayList.Add(Color.FromArgb(i, j, k));
				}
			}
		}
		return arrayList;
	}

	public static Color InterpolateColors(Color src, Color dst, float percentage)
	{
		int r = src.R;
		int g = src.G;
		int b = src.B;
		int r2 = dst.R;
		int g2 = dst.G;
		int b2 = dst.B;
		byte red = Convert.ToByte((float)r + (float)(r2 - r) * percentage);
		byte green = Convert.ToByte((float)g + (float)(g2 - g) * percentage);
		byte blue = Convert.ToByte((float)b + (float)(b2 - b) * percentage);
		return Color.FromArgb(red, green, blue);
	}
}
