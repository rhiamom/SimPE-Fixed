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
using System.Security.Permissions;
using System.Windows.Forms;

namespace Ambertation.Windows.Forms;

internal class DockPanelFloatingForm : Form, IMessageFilter
{
	private DockPanel dock;

	private DockContainer cnt;

	private Size lastsz;

	public DockPanel DockControl => dock;

	public BaseDockManager Manager
	{
		get
		{
			if (DockControl == null)
			{
				return null;
			}
			return DockControl.Manager;
		}
	}

	public bool HasContainer => cnt != null;

	public DockPanelFloatingForm(DockPanel dock)
	{
		ManagerSingelton.Global.AddFloatForm(this);
		base.TopMost = ManagerSingelton.Global.TopmostFloats;
		this.dock = dock;
	}

	~DockPanelFloatingForm()
	{
		ManagerSingelton.Global.RemoveFloatForm(this);
	}

	public void DragContainerAlong(DockContainer cnt)
	{
		this.cnt = cnt;
		foreach (DockPanel dockedPanel in cnt.GetDockedPanels())
		{
			dockedPanel.RefreshMargin();
		}
	}

	public bool PreFilterMessage(ref Message m)
	{
		if (m.Msg == 28)
		{
			OnActivateApplication((int)m.WParam != 0);
		}
		return false;
	}

	[PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
	protected override void WndProc(ref Message m)
	{
		if (m.Msg == 162 || m.Msg == 562)
		{
			if (Manager != null && Manager.DockMode)
			{
				StopFloating();
			}
			base.WndProc(ref m);
		}
		else if (m.Msg == 561)
		{
			lastsz = base.Size;
			base.WndProc(ref m);
		}
		else if (m.Msg == 132)
		{
			base.WndProc(ref m);
		}
		else if (m.Msg == 534)
		{
			_ = (APIHelp.RECT)m.GetLParam(typeof(APIHelp.RECT));
			FireLocationChangeEvent();
			base.WndProc(ref m);
		}
		else
		{
			base.WndProc(ref m);
		}
	}

	internal void SendeActivateEvent(bool active)
	{
		OnActivateApplication(active);
	}

	protected virtual void OnActivateApplication(bool active)
	{
		Console.WriteLine("Activate Application " + active);
		if (active && ManagerSingelton.Global.TopmostFloats)
		{
			base.TopMost = true;
		}
		else
		{
			base.TopMost = false;
		}
	}

	internal void StartFloatingBlocked(DockPanel p)
	{
		StartFloating();
		Text = p.CaptionText;
		APIHelp.ReleaseCapture(base.Handle);
		APIHelp.SendMessage(base.Handle, 161u, 2, 0);
		StopFloating();
	}

	protected override void OnControlRemoved(ControlEventArgs e)
	{
		base.OnControlRemoved(e);
		if (Manager == null)
		{
			Close();
		}
		if (base.Controls.Count == 0 && HasContainer && !Manager.DockMode)
		{
			dock = null;
			Close();
		}
	}

	protected void StartFloating()
	{
		OnStartFloating();
	}

	protected virtual void OnStartFloating()
	{
	}

	protected void StopFloating()
	{
		if (dock == null)
		{
			return;
		}
		if (HasContainer)
		{
			DockControl.UnFloat(this);
			if (cnt.GetDockedPanels().Count == 0)
			{
				cnt.Parent = null;
				dock = null;
			}
		}
		else
		{
			DockControl.UnFloat(this);
		}
		OnStopFloating();
		if (base.Controls.Count == 0)
		{
			dock = null;
		}
		if (dock == null)
		{
			cnt = null;
			Close();
		}
	}

	protected virtual void OnStopFloating()
	{
	}

	protected override void OnMouseUp(MouseEventArgs e)
	{
		base.OnMouseUp(e);
		if (Manager != null && Manager.DockMode)
		{
			APIHelp.ReleaseCapture(base.Handle);
			StopFloating();
		}
	}

	protected override void OnLocationChanged(EventArgs e)
	{
		base.OnLocationChanged(e);
		FireLocationChangeEvent();
	}

	private void FireLocationChangeEvent()
	{
		if (base.Size.Width == lastsz.Width && base.Size.Height == lastsz.Height && Manager != null)
		{
			if (!Manager.DockMode && base.Visible)
			{
				Manager.StartDockMode(DockControl);
			}
			Manager.MouseMoved(Cursor.Position);
		}
	}

	protected override void OnClosing(CancelEventArgs e)
	{
		base.TopMost = false;
		if (cnt != null && dock != null)
		{
			DockButtonBar.DockPanelList dockedPanels = cnt.GetDockedPanels();
			DockButtonBar.DockPanelList dockPanelList = new DockButtonBar.DockPanelList();
			foreach (DockPanel item in dockedPanels)
			{
				dockPanelList.Add(item);
			}
			{
				foreach (DockPanel item2 in dockPanelList)
				{
					item2.CloseFromForm();
				}
				return;
			}
		}
		if (dock != null)
		{
			dock.CloseFromForm();
		}
	}

	protected override void Dispose(bool disposing)
	{
		Application.RemoveMessageFilter(this);
		base.Dispose(disposing);
	}
}
