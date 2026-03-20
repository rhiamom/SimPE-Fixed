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
 
 using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;

namespace Ambertation.Windows.Forms;

[ToolboxItem(false)]
public class RubberBandHelper : Control
{
	private DockContainer dc;

	private Dictionary<Control, bool> map;

	private DockStyle dock;

	public DockStyle ContainerDock => dock;

	internal RubberBandHelper(DockContainer dc)
	{
		SetStyle(ControlStyles.ResizeRedraw, value: true);
		SetStyle(ControlStyles.OptimizedDoubleBuffer, value: true);
		SetStyle(ControlStyles.UserPaint, value: true);
		SetStyle(ControlStyles.AllPaintingInWmPaint, value: true);
		this.dc = dc;
		map = new Dictionary<Control, bool>();
		foreach (Control control in dc.Controls)
		{
			map[control] = control.Visible;
			control.Visible = false;
		}
		Dock = DockStyle.Fill;
		dock = dc.Dock;
		dc.Controls.Add(this);
	}

	internal void Close()
	{
		dc.Controls.Remove(this);
		foreach (Control key in map.Keys)
		{
			if (key is DockPanel)
			{
				((DockPanel)key).NCRefresh();
			}
			key.Visible = map[key];
		}
	}

	protected override void OnMouseMove(MouseEventArgs e)
	{
		base.OnMouseMove(e);
	}

	protected override void OnPaint(PaintEventArgs e)
	{
		base.OnPaint(e);
		dc.Manager.Renderer.DockPanelRenderer.RenderResizePanel(dc, this, e);
	}
}
