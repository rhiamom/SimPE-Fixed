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
using System.Windows.Forms;

namespace Ambertation.Windows.Forms;

internal class MenuStripButtonExt : ToolStripButton
{
	private ToolStrip item;

	internal MenuStripButtonExt(ToolStrip item)
	{
		Text = item.Text;
		base.Name = "msbe_" + item.Name;
		if (Text == "")
		{
			Text = item.Name;
		}
		Text += " ";
		this.item = item;
		base.Visible = true;
		base.Available = true;
		item.VisibleChanged += item_VisibleChanged;
		UpdateChecked();
	}

	private void item_VisibleChanged(object sender, EventArgs e)
	{
		UpdateChecked();
	}

	protected override void OnClick(EventArgs e)
	{
		base.OnClick(e);
		item.Visible = !item.Visible;
	}

	protected override void OnPaint(PaintEventArgs e)
	{
		base.OnPaint(e);
		UpdateChecked();
	}

	private void UpdateChecked()
	{
		base.Checked = item.Visible;
	}
}
