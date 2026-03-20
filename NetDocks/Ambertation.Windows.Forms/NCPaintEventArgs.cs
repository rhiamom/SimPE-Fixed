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

namespace Ambertation.Windows.Forms;

public class NCPaintEventArgs : EventArgs
{
	private Rectangle clientRect;

	private Rectangle windowRect;

	private Region paintRegion;

	private Graphics gr;

	public Graphics Graphics => gr;

	public Rectangle ClientRectangle => clientRect;

	public Rectangle WindowRectangle => windowRect;

	public Region PaintRegion => paintRegion;

	public NCPaintEventArgs(Graphics g, Rectangle cr, Rectangle wr, Region pr)
	{
		gr = g;
		clientRect = cr;
		windowRect = wr;
		paintRegion = pr;
	}
}
