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
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Ambertation.Windows.Forms;

[Designer(typeof(DockContainerDesigner))]
[ToolboxBitmap(typeof(DockManager), "Floaters.dockimg.png")]
[ToolboxItem(true)]
public class TabControl : BaseDockManager
{
	public override bool OneChild => false;

	protected override bool MeAsCenterDock => true;

	public TabControl()
		: base(ManagerSingelton.Global.TabRenderer)
	{
	}

	protected override void CleanUp()
	{
	}

	protected override void OnControlAdded(ControlEventArgs e)
	{
		TabPage tabPage = e.Control as TabPage;
		if (tabPage == null)
		{
			throw new Exception("You can only add TabPage Controls to a TabControl! (tried to add " + e.Control.GetType().Name + ")");
		}
		base.OnControlAdded(e);
	}

	internal override void StartDockMode(DockPanel dock)
	{
	}

	internal override void StopDockMode(DockPanel dock)
	{
		AddPage(dock as TabPage);
	}

	internal override void MouseMoved(Point scrpt)
	{
	}

	public void AddPage(TabPage tp)
	{
		DockPanelInt(tp, DockStyle.Fill);
	}

	public override void Collapse(bool animated)
	{
	}

	public override void Expand(bool animated)
	{
	}
}
