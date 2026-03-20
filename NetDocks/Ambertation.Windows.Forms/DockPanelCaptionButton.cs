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

namespace Ambertation.Windows.Forms;

public abstract class DockPanelCaptionButton
{
	private DockPanel parent;

	private CaptionButtonState state;

	private bool visible;

	protected IDockPanelRenderer Renderer => parent.Manager.Renderer.DockPanelRenderer;

	public DockPanel Parent => parent;

	public CaptionButtonState State => state;

	public bool Visible => visible;

	public Rectangle Bounds
	{
		get
		{
			Rectangle captionRect = Renderer.GetCaptionRect(parent);
			return GetBounds(captionRect);
		}
	}

	internal string ImageName => GetImageName();

	public DockPanelCaptionButton(DockPanel dp)
	{
		visible = true;
		parent = dp;
		state = CaptionButtonState.Normal;
	}

	internal bool SetState(CaptionButtonState st)
	{
		bool result = state != st;
		state = st;
		return result;
	}

	public void PerformClick()
	{
		OnClick();
	}

	protected abstract void OnClick();

	internal bool SetVisible(bool vis)
	{
		bool result = vis != visible;
		visible = vis;
		return result;
	}

	internal bool Hit(NCMouseEventArgs e)
	{
		return Bounds.Contains(e.ControlPosition);
	}

	internal void Render(NCPaintEventArgs e)
	{
		Renderer.RenderCaptionButton(parent, this, e);
	}

	protected abstract Rectangle GetBounds(Rectangle captionrect);

	protected abstract string GetImageName();
}
