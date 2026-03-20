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

public sealed class GlobalMaterial : ExtendedContainer
{
	public enum Propagations
	{
		BRANCH,
		NODE,
		INHERITED
	}

	private string refname;

	private Propagations p;

	public string ReferencedMaterialName
	{
		get
		{
			return refname;
		}
		set
		{
			refname = value;
			if (refname == null)
			{
				refname = "";
			}
		}
	}

	public Propagations Propagation
	{
		get
		{
			return p;
		}
		set
		{
			p = value;
		}
	}

	public GlobalMaterial(Container parent, string args)
		: base(parent, args)
	{
		refname = "DefaultLib.Scene_Material";
		p = Propagations.BRANCH;
	}

	protected override void FinishDeSerialize()
	{
		base.FinishDeSerialize();
		refname = Line(0).StripQuotes();
		Line(1).StripQuotes();
		p = (Propagations)EnumValue(1, typeof(Propagations));
		CustomClear();
	}

	protected override void PrepareSerialize()
	{
		base.PrepareSerialize();
		Clear(rec: false);
		AddLiteral("\"" + refname.Trim() + "\"");
		AddLiteral("\"" + p.ToString() + "\"");
	}
}
