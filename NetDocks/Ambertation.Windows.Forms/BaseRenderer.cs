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

namespace Ambertation.Windows.Forms;

public abstract class BaseRenderer
{
	private IColorTable table;

	private IFontTable fnt;

	private IRenderDockHints dock;

	private IDockPanelRenderer panel;

	public IColorTable ColorTable => table;

	public IFontTable FontTable => fnt;

	public IRenderDockHints DockRenderer
	{
		get
		{
			if (dock == null)
			{
				CreateDockRenderer(out dock);
			}
			return dock;
		}
	}

	public IDockPanelRenderer DockPanelRenderer
	{
		get
		{
			if (panel == null)
			{
				CreateDockPanelRenderer(out panel);
			}
			return panel;
		}
	}

	public BaseRenderer(IColorTable ct, IFontTable fnt)
	{
		table = ct;
		this.fnt = fnt;
	}

	protected abstract void CreateDockRenderer(out IRenderDockHints rnd);

	protected abstract void CreateDockPanelRenderer(out IDockPanelRenderer rnd);

	private byte Interpolate(byte cl1, byte cl2, float val)
	{
		return (byte)Math.Min(255f, Math.Max(0f, (float)(int)cl2 * val + (float)(int)cl1 * (1f - val)));
	}

	public Color Interpolate(Color cl1, Color cl2, float val)
	{
		return Color.FromArgb(Interpolate(cl1.R, cl2.R, val), Interpolate(cl1.G, cl2.G, val), Interpolate(cl1.B, cl2.B, val));
	}
}
