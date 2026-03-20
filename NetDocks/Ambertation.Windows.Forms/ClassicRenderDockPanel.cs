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
using System.Drawing.Drawing2D;

namespace Ambertation.Windows.Forms;

public class ClassicRenderDockPanel : WhidbeyRenderDockPanel
{
	private const int SIZE_DELTA = 10;

	private const int SPEED = 20;

	public ClassicRenderDockPanel(BaseRenderer parent)
		: base(parent)
	{
		dim = new Dimensions(14, 21, 1, 1, 4, 4, 10, 2, 2);
	}

	protected override void RenderCaptionButton(DockPanel dp, DockPanelCaptionButton but, string iname, NCPaintEventArgs e)
	{
		if (but.State != CaptionButtonState.Normal)
		{
			Pen pen = new Pen(Color.FromArgb(112, 0, 0, 0));
			SolidBrush brush = new SolidBrush(Color.Transparent);
			if (but.State == CaptionButtonState.Highlight)
			{
				brush = new SolidBrush(Color.FromArgb(112, 255, 255, 255));
			}
			else if (but.State == CaptionButtonState.Selected)
			{
				brush = new SolidBrush(Color.FromArgb(64, 255, 255, 255));
			}
			e.Graphics.FillRectangle(brush, but.Bounds);
			e.Graphics.DrawRectangle(pen, but.Bounds);
		}
		DrawButtonImage(e.Graphics, SetupCaptionButtonName(dp, iname), but.Bounds, dp.CaptionState == CaptionState.Focused);
	}

	protected override void RenderCaptionBackground(CaptionState state, NCPaintEventArgs e, Rectangle caprect)
	{
		Color color = base.Parent.ColorTable.DockCaptionColorTop;
		if (state == CaptionState.Focused)
		{
			color = base.Parent.ColorTable.DockCaptionFocusColorTop;
		}
		e.Graphics.FillRectangle(new SolidBrush(color), caprect);
	}

	protected override void RenderButtonBackground(NCPaintEventArgs e, Rectangle barr, Rectangle r, Point pt1, Point pt2, DockPanel dp)
	{
		RenderButtonBarBackground(e, r, dp.BestOrientation);
	}

	public override void RenderButtonBarBackground(NCPaintEventArgs e, Rectangle r, ButtonOrientation orient)
	{
		Color dockButtonBarBackgroundTop = base.ColorTable.DockButtonBarBackgroundTop;
		e.Graphics.FillRectangle(new SolidBrush(dockButtonBarBackgroundTop), r);
	}

	protected override void ModifyButtonRectangle(ref Rectangle r, string caption, Image img, ButtonOrientation orient, ButtonState state, bool renderbackgroundbar)
	{
		base.ModifyButtonRectangle(ref r, caption, img, orient, state, renderbackgroundbar);
		switch (orient)
		{
		case ButtonOrientation.Bottom:
			r = new Rectangle(r.Left + 1, r.Top, r.Width - 2, r.Height);
			break;
		case ButtonOrientation.Top:
			r = new Rectangle(r.Left + 1, r.Top, r.Width - 2, r.Height);
			break;
		case ButtonOrientation.Left:
			r = new Rectangle(r.Left, r.Top + 1, r.Width, r.Height - 2);
			break;
		default:
			r = new Rectangle(r.Left, r.Top + 1, r.Width, r.Height - 2);
			break;
		}
	}

	protected override void SetupButtonColors(Rectangle r, Color c, Color ci, Color fontc, ButtonOrientation orient, ButtonState state, out SolidBrush fontbrush, out SolidBrush linebackgroundbrush, out Brush backgroundbrush, out Pen borderpen, out Pen borderpeninner)
	{
		base.SetupButtonColors(r, c, ci, fontc, orient, state, out fontbrush, out linebackgroundbrush, out backgroundbrush, out borderpen, out borderpeninner);
		if (state == ButtonState.Normal)
		{
			backgroundbrush = new SolidBrush(base.ColorTable.DockButtonBackgroundTop);
		}
		else
		{
			backgroundbrush = new SolidBrush(base.ColorTable.DockButtonHighlightBackgroundTop);
		}
	}

	protected override GraphicsPath ButtonFullPath(Rectangle r)
	{
		GraphicsPath graphicsPath = new GraphicsPath();
		graphicsPath.AddRectangle(new Rectangle(r.Left, r.Top, r.Width, r.Height));
		return graphicsPath;
	}

	protected override void FixButtonCorners(Graphics g, Brush bg, Pen p, ref Rectangle linerectangle, ref Point linept1, ref Point linept2, Pen pp)
	{
	}

	protected override void RenderButtonIcon(Graphics g, Image img, Rectangle imgrect)
	{
		g.DrawImage(img, imgrect, new Rectangle(0, 0, img.Width, img.Height), GraphicsUnit.Pixel);
	}
}
