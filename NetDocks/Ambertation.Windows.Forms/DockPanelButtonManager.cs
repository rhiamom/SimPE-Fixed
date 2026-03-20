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
using System.Collections.Generic;
using System.Drawing;

namespace Ambertation.Windows.Forms;

public class DockPanelButtonManager : IEnumerable<DockPanelButtonManager.Place>, IEnumerable
{
	public class Place
	{
		private DockPanel panel;

		private Rectangle rect;

		public DockPanel Panel => panel;

		public Rectangle Rectangle => rect;

		public Place(DockPanel panel, Rectangle rect)
		{
			this.panel = panel;
			this.rect = rect;
		}
	}

	private List<Place> buttons;

	private BaseRenderer renderer;

	private NCPaintEventArgs eventarg;

	private IButtonContainer cnt;

	public DockPanelButtonManager(BaseRenderer renderer, NCPaintEventArgs e, IButtonContainer cnt)
	{
		this.renderer = renderer;
		this.cnt = cnt;
		eventarg = e;
		buttons = InitStructure(renderer, e, cnt);
	}

	protected virtual List<Place> InitStructure(BaseRenderer renderer, NCPaintEventArgs e, IButtonContainer cnt)
	{
		List<Place> result = new List<Place>();
		if (cnt != null)
		{
			DockButtonBar.DockPanelList panels = cnt.GetButtons();
			Rectangle buttonsRectangle = renderer.DockPanelRenderer.GetButtonsRectangle(cnt.BestOrientation, e, cnt as DockContainer);
			if (cnt.BestOrientation == ButtonOrientation.Bottom || cnt.BestOrientation == ButtonOrientation.Top)
			{
				PlanHorizontalButtons(result, panels, e, cnt.BestOrientation, buttonsRectangle, out var left, out var swd);
				result = SetHorizontalButtons(result, panels, e, left, swd, buttonsRectangle);
			}
			else
			{
				PlanVerticalButtons(result, panels, e, cnt.BestOrientation, buttonsRectangle, out var top, out var shg);
				result = SetVerticalButtons(result, panels, e, top, shg, buttonsRectangle);
			}
		}
		return result;
	}

	protected bool NeedSeperatorAfterPanel(DockPanel dp)
	{
		return dp.SeperateInDockBar;
	}

	private List<Place> SetVerticalButtons(List<Place> buttons, DockButtonBar.DockPanelList panels, NCPaintEventArgs e, int top, int shg, Rectangle pad)
	{
		int height = pad.Height;
		if (top > height)
		{
			if (shg * panels.Count > height)
			{
				shg = height / panels.Count;
			}
			List<Place> list = new List<Place>();
			int? num = null;
			{
				foreach (Place button in buttons)
				{
					if (!num.HasValue)
					{
						num = button.Rectangle.Top;
					}
					Rectangle rect = new Rectangle(button.Rectangle.Left, num.Value, button.Rectangle.Width, shg);
					num += shg;
					list.Add(new Place(button.Panel, rect));
				}
				return list;
			}
		}
		return buttons;
	}

	private void PlanVerticalButtons(List<Place> buttons, DockButtonBar.DockPanelList panels, NCPaintEventArgs e, ButtonOrientation orient, Rectangle pad, out int top, out int shg)
	{
		top = pad.Top;
		shg = int.MaxValue;
		foreach (DockPanel panel in panels)
		{
			Size buttonSize = renderer.DockPanelRenderer.GetButtonSize(panel, orient);
			Rectangle rect = new Rectangle(pad.Left, top, buttonSize.Width, buttonSize.Height);
			buttons.Add(new Place(panel, rect));
			top += buttonSize.Height;
			if (panel.SeperateInDockBar)
			{
				top += renderer.DockPanelRenderer.Dimension.DockBarButtonSpacing;
			}
			shg = Math.Min(buttonSize.Height, shg);
		}
	}

	private List<Place> SetHorizontalButtons(List<Place> buttons, DockButtonBar.DockPanelList panels, NCPaintEventArgs e, int left, int swd, Rectangle pad)
	{
		int width = pad.Width;
		if (left > width)
		{
			if (swd * panels.Count > width)
			{
				swd = width / panels.Count;
			}
			List<Place> list = new List<Place>();
			int? num = null;
			{
				foreach (Place button in buttons)
				{
					if (!num.HasValue)
					{
						num = button.Rectangle.Left;
					}
					Rectangle rect = new Rectangle(num.Value, button.Rectangle.Top, swd, button.Rectangle.Height);
					num += swd;
					list.Add(new Place(button.Panel, rect));
				}
				return list;
			}
		}
		return buttons;
	}

	private void PlanHorizontalButtons(List<Place> buttons, DockButtonBar.DockPanelList panels, NCPaintEventArgs e, ButtonOrientation orient, Rectangle pad, out int left, out int swd)
	{
		left = pad.Left;
		swd = int.MaxValue;
		foreach (DockPanel panel in panels)
		{
			Size buttonSize = renderer.DockPanelRenderer.GetButtonSize(panel, orient);
			Rectangle rect = new Rectangle(left, pad.Top, buttonSize.Width, buttonSize.Height);
			buttons.Add(new Place(panel, rect));
			left += buttonSize.Width;
			if (panel.SeperateInDockBar)
			{
				left += renderer.DockPanelRenderer.Dimension.DockBarButtonSpacing;
			}
			swd = Math.Min(buttonSize.Width, swd);
		}
	}

	public DockPanel GetHitPanel(Point mouse)
	{
		using (IEnumerator<Place> enumerator = GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				Place current = enumerator.Current;
				if (current.Rectangle.Contains(mouse))
				{
					return current.Panel;
				}
			}
		}
		return null;
	}

	public void Render(bool renderbackgroundbar)
	{
		foreach (Place button in buttons)
		{
			ButtonState state = ButtonState.Normal;
			if (button.Panel == cnt.Highlight)
			{
				state = ButtonState.Highlight;
			}
			renderer.DockPanelRenderer.RenderButton(eventarg.Graphics, button.Rectangle, button.Panel.ButtonText, button.Panel.Image, cnt.BestOrientation, state, renderbackgroundbar);
		}
	}

	public IEnumerator<Place> GetEnumerator()
	{
		return buttons.GetEnumerator();
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		return buttons.GetEnumerator();
	}
}
