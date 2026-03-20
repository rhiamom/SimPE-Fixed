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
 
 namespace Ambertation.XSI.Template;

public sealed class Visibility : ExtendedContainer
{
	public enum States
	{
		NotVisible,
		Visible
	}

	private States v;

	public States State
	{
		get
		{
			return v;
		}
		set
		{
			v = value;
		}
	}

	public Visibility(Container parent, string args)
		: base(parent, args)
	{
		v = States.Visible;
	}

	protected override void FinishDeSerialize()
	{
		base.FinishDeSerialize();
		v = (States)Line(0).GetFloat(0);
		CustomClear();
	}

	protected override void PrepareSerialize()
	{
		base.PrepareSerialize();
		Clear(rec: false);
		AddLiteral((int)v);
	}
}
