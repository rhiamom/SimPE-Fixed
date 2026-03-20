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
using System.Windows.Forms;

namespace Ambertation.Windows.Forms;

public class ManagedLayeredForm : LayeredForm
{
	private DockManager manager;

	internal DockManager Manager => manager;

	protected ManagedLayeredForm(DockManager manager)
	{
		this.manager = manager;
	}

	internal ManagedLayeredForm(DockManager manager, Bitmap bitmap)
		: base(bitmap)
	{
		this.manager = manager;
	}

	internal ManagedLayeredForm(DockManager manager, Color cl, Size sz)
		: base(cl, sz)
	{
		this.manager = manager;
	}

	internal virtual void MouseOver(Point pt, bool hit)
	{
	}

	protected override void OnMouseUp(MouseEventArgs e)
	{
		base.OnMouseUp(e);
	}

	private void InitializeComponent()
	{
		base.SuspendLayout();
		base.ClientSize = new System.Drawing.Size(1115, 759);
		base.Name = "ManagedLayeredForm";
		base.ResumeLayout(false);
	}
}
