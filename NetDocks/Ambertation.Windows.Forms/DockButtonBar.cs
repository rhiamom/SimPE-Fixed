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
using System.Drawing;
using System.Windows.Forms;

namespace Ambertation.Windows.Forms;

[ToolboxItem(false)]
public class DockButtonBar : UserControl, IButtonContainer
{
	public class DockPanelList : List<DockPanel>
	{
	}

	private IContainer components;

	private DockPanelList panels;

	private DockPanelList hidden;

	private Dictionary<DockContainer, DockPanelList> containers;

	private DockManager manager;

	private DockPanelButtonManager buttonData;

	public DockManager Manager => manager;

	protected DockPanelButtonManager ButtonData => buttonData;

	public DockPanel Highlight
	{
		get
		{
			return null;
		}
		set
		{
		}
	}

	public ButtonOrientation BestOrientation
	{
		get
		{
			if (Dock == DockStyle.Bottom)
			{
				return ButtonOrientation.Top;
			}
			if (Dock == DockStyle.Left)
			{
				return ButtonOrientation.Right;
			}
			if (Dock == DockStyle.Top)
			{
				return ButtonOrientation.Bottom;
			}
			return ButtonOrientation.Left;
		}
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing && components != null)
		{
			components.Dispose();
		}
		base.Dispose(disposing);
	}

	private void InitializeComponent()
	{
		this.components = new System.ComponentModel.Container();
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
	}

	public DockButtonBar(DockManager manager)
	{
		InitializeComponent();
		this.manager = manager;
		panels = new DockPanelList();
		containers = new Dictionary<DockContainer, DockPanelList>();
	}

	protected void SetVisibleState()
	{
		bool flag = panels.Count > 0;
		if (flag == base.Visible)
		{
			Refresh();
		}
		else
		{
			base.Width = Manager.Renderer.DockPanelRenderer.Dimension.Buttons;
			base.Height = base.Width;
		}
		base.Visible = flag;
	}

	public bool Contains(DockContainer dc)
	{
		return containers.ContainsKey(dc);
	}

	public void Add(DockContainer c)
	{
		DockPanelList dockedPanels = c.GetDockedPanels();
		containers[c] = dockedPanels;
		foreach (DockPanel item in dockedPanels)
		{
			item.SeperateInDockBar = false;
			if (!panels.Contains(item))
			{
				AddPanel(item);
			}
		}
		if (panels.Count > 0)
		{
			panels[panels.Count - 1].SeperateInDockBar = true;
		}
		SetVisibleState();
	}

	private void AddPanel(DockPanel p)
	{
		panels.Add(p);
	}

	public void Remove(DockContainer c)
	{
		if (containers.ContainsKey(c))
		{
			DoRemove(c);
			SetVisibleState();
		}
	}

	internal void SilentRemove(DockContainer c)
	{
		if (containers.ContainsKey(c))
		{
			DoRemove(c);
		}
	}

	private void DoRemove(DockContainer c)
	{
		DockPanelList dockPanelList = containers[c];
		if (dockPanelList == null)
		{
			return;
		}
		containers.Remove(c);
		foreach (DockPanel item in dockPanelList)
		{
			RemovePanel(item);
		}
	}

	private void RemovePanel(DockPanel p)
	{
		p.SeperateInDockBar = false;
		panels.Remove(p);
	}

	public void Clear()
	{
		foreach (DockPanel panel in panels)
		{
			panel.SeperateInDockBar = false;
		}
		panels.Clear();
		containers.Clear();
		SetVisibleState();
	}

	private DockContainer FindDock(DockPanel p)
	{
		foreach (DockContainer key in containers.Keys)
		{
			DockPanelList dockPanelList = containers[key];
			foreach (DockPanel item in dockPanelList)
			{
				if (item == p)
				{
					return key;
				}
			}
		}
		return null;
	}

	protected override void OnMouseDown(MouseEventArgs e)
	{
		base.OnMouseDown(e);
		if (ButtonData == null || e.Button != MouseButtons.Left)
		{
			return;
		}
		Point mouse = new Point(e.X, e.Y);
		DockPanel hitPanel = ButtonData.GetHitPanel(mouse);
		if (hitPanel != null)
		{
			DockContainer dockContainer = FindDock(hitPanel);
			if (dockContainer != null)
			{
				dockContainer.Expand();
				hitPanel.EnsureVisible();
			}
		}
	}

	protected override void OnPaint(PaintEventArgs e)
	{
		base.OnPaint(e);
		Manager.Renderer.DockPanelRenderer.RenderButtonBarBackground(new NCPaintEventArgs(e.Graphics, base.Bounds, base.Bounds, null), new Rectangle(0, 0, base.Width, base.Height), BestOrientation);
		NCPaintEventArgs e2 = new NCPaintEventArgs(e.Graphics, base.ClientRectangle, base.Bounds, null);
		buttonData = Manager.Renderer.DockPanelRenderer.ConstructButtonData(this, e2);
		buttonData.Render(renderbackgroundbar: false);
	}

	public DockPanelList GetButtons()
	{
		return panels;
	}

	public Padding GetBorderSize(ButtonOrientation orient)
	{
		return manager.Renderer.DockPanelRenderer.GetBarBorderSize(orient);
	}
}
