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
using System.Windows.Forms;

namespace Ambertation.Windows.Forms;

public class ToolStripRuntimeDesigner
{
	public static void Add(ToolStrip ts)
	{
		List<ToolStripButton> list = new List<ToolStripButton>();
		int top = 0;
		foreach (ToolStripItem item3 in ts.Items)
		{
			ToolStripButton item2 = new ToolStripButtonExt(item3, ref top);
			list.Add(item2);
		}
		foreach (ToolStripButton item4 in list)
		{
			ts.Items.Add(item4);
		}
	}

	protected static void Add(ToolStripPanel pn, ContextMenuStrip men)
	{
		foreach (Control control in pn.Controls)
		{
			if (control is ToolStrip toolStrip)
			{
				ToolStripButton value = new MenuStripButtonExt(toolStrip);
				men.Items.Add(value);
				if (toolStrip != null)
				{
					Add(toolStrip);
				}
			}
		}
		pn.ContextMenuStrip = men;
	}

	public static void Add(ToolStripContainer cnt)
	{
		ContextMenuStrip men = new ContextMenuStrip();
		Add(cnt.TopToolStripPanel, men);
		Add(cnt.LeftToolStripPanel, men);
		Add(cnt.BottomToolStripPanel, men);
		Add(cnt.RightToolStripPanel, men);
	}

	public static void LineUpToolBars(ToolStripContainer cnt)
	{
		LineUpToolBars(cnt.TopToolStripPanel, horz: true);
		LineUpToolBars(cnt.LeftToolStripPanel, horz: false);
		LineUpToolBars(cnt.BottomToolStripPanel, horz: true);
		LineUpToolBars(cnt.RightToolStripPanel, horz: false);
	}

	public static void LineUpToolBars(ToolStripPanel pn, bool horz)
	{
		int left = 0;
		int top = 0;
		foreach (Control control in pn.Controls)
		{
			if (control is ToolStrip toolStrip)
			{
				toolStrip.Left = left;
				toolStrip.Top = top;
				if (horz)
				{
					left = toolStrip.Left + toolStrip.Width + 1;
				}
				else
				{
					top = toolStrip.Top + toolStrip.Height + 1;
				}
			}
		}
	}
}
