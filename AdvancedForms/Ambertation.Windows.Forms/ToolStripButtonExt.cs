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

internal class ToolStripButtonExt : ToolStripButton
{
	private ToolStripItem item;

	private bool intern;

	internal ToolStripItem Item => item;

	internal ToolStripButtonExt(ToolStripItem item, ref int top)
	{
		intern = false;
		item.Overflow = ToolStripItemOverflow.Never;
		Text = item.Text;
		base.Name = "tsbe_" + item.Name;
		Image = item.Image;
		base.ImageScaling = item.ImageScaling;
		base.Overflow = ToolStripItemOverflow.Always;
		base.ImageAlign = ContentAlignment.MiddleLeft;
		this.item = item;
		SetBounds(new Rectangle(0, top, Bounds.Width, Bounds.Height));
		base.Visible = true;
		item.VisibleChanged += item_VisibleChanged;
		item.AvailableChanged += item_AvailableChanged;
		base.Alignment = ToolStripItemAlignment.Left;
		DisplayStyle = ToolStripItemDisplayStyle.ImageAndText;
		base.TextImageRelation = TextImageRelation.ImageBeforeText;
		top += Bounds.Height;
		UpdateChecked();
	}

	private void item_AvailableChanged(object sender, EventArgs e)
	{
		UpdateChecked();
	}

	protected override void OnCheckedChanged(EventArgs e)
	{
		base.OnCheckedChanged(e);
		if (!intern)
		{
			item.Available = base.Checked;
		}
	}

	private void item_VisibleChanged(object sender, EventArgs e)
	{
		UpdateChecked();
	}

	protected override void OnClick(EventArgs e)
	{
		base.OnClick(e);
		item.Available = !item.Available;
	}

	protected override void OnPaint(PaintEventArgs e)
	{
		base.OnPaint(e);
		UpdateChecked();
	}

	private void UpdateChecked()
	{
		intern = true;
		base.Checked = item.Available;
		intern = false;
	}
}
