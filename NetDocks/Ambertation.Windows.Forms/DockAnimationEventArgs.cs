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
 
 namespace Ambertation.Windows.Forms;

public class DockAnimationEventArgs
{
	public enum Alignment
	{
		Horizontal,
		Vertical,
		Undefined
	}

	public enum Type
	{
		Collapse,
		Expand
	}

	private DockContainer dc;

	private Type tp;

	private Alignment a;

	public DockContainer Container => dc;

	public Type AnimationType => tp;

	public Alignment DockAlignment => a;

	public DockAnimationEventArgs(DockContainer dc, Type tp, Alignment a)
	{
		this.dc = dc;
		this.tp = tp;
		this.a = a;
	}

	public override string ToString()
	{
		return string.Concat("type:", tp, ", align:", a, ", name:", dc.Name);
	}
}
