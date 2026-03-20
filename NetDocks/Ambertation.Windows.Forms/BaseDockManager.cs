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
using System.Windows.Forms;

namespace Ambertation.Windows.Forms;

public abstract class BaseDockManager : DockContainer
{
	private BaseRenderer rnd;

	protected bool dockmode;

	protected DockButtonBar.DockPanelList floatingpanels;

	public BaseRenderer Renderer
	{
		get
		{
			return rnd;
		}
		set
		{
			rnd = value;
		}
	}

	public bool DockMode => dockmode;

	protected abstract bool MeAsCenterDock { get; }

	public BaseDockManager(BaseRenderer renderer)
		: base(null)
	{
		rnd = renderer;
		floatingpanels = new DockButtonBar.DockPanelList();
	}

	internal void NotifyFloating(DockPanel dp)
	{
		if (dp.Floating && !floatingpanels.Contains(dp))
		{
			floatingpanels.Add(dp);
		}
		else if (!dp.Floating && floatingpanels.Contains(dp))
		{
			floatingpanels.Remove(dp);
		}
	}

	internal abstract void StartDockMode(DockPanel dock);

	internal abstract void StopDockMode(DockPanel dock);

	internal abstract void MouseMoved(Point scrpt);

	internal virtual void DockPanelInt(DockPanel dp, DockStyle style)
	{
		bool flag = false;
		SuspendLayout();
		if (style == DockStyle.Fill && MeAsCenterDock)
		{
			flag = true;
			dp.DockControl(this);
			ResumeLayout();
			return;
		}
		foreach (DockContainer container in containers)
		{
			if (container.Dock == style)
			{
				flag = true;
				dp.DockControl(container);
				break;
			}
		}
		if (!flag)
		{
			DockContainer dockContainer = CreateNewContainer(-1, after: false, toplevel: true, style);
			dockContainer.SetNoCleanUpIntern(val: true);
			dockContainer.Visible = true;
			dockContainer.Width = Math.Max(dockContainer.Width, dp.Width);
			dockContainer.Height = Math.Max(dockContainer.Height, dp.Height);
			dp.DockControl(dockContainer);
			dockContainer.SetNoCleanUpIntern(val: false);
		}
		ResumeLayout();
	}
}
