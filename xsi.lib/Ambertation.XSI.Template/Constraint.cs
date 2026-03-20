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
 
 using Ambertation.Collections;

namespace Ambertation.XSI.Template;

public sealed class Constraint : JoinedArgumentContainer
{
	public enum Types
	{
		POSITION,
		SCALING,
		DIRECTION,
		ORIENTATION,
		UP_VECTOR,
		PREFERED_AXIS,
		INTEREST
	}

	private StringCollection cobjects;

	private string oname;

	private Types t;

	private TemplateCollection consts;

	public string ConstraintName
	{
		get
		{
			return base.JoinedArgument1;
		}
		set
		{
			base.JoinedArgument1 = value;
		}
	}

	public TemplateCollection SubConstraints => consts;

	public StringCollection ConstraintObjectNames => cobjects;

	public Types Type
	{
		get
		{
			return t;
		}
		set
		{
			t = value;
		}
	}

	public Constraint(Container parent, string args)
		: base(parent, args)
	{
		consts = new TemplateCollection(this);
		t = Types.POSITION;
		cobjects = new StringCollection();
		Reset();
	}

	private void Reset()
	{
		consts.Clear(rec: false);
		cobjects.Clear();
		oname = "";
	}

	protected override void ResetArgs()
	{
		base.ResetArgs("DefaultLib.Scene_Material");
	}

	protected override void CustomClear()
	{
	}

	protected override void FinishDeSerialize()
	{
		base.FinishDeSerialize();
		Reset();
		int num = 0;
		oname = Line(num++).StripQuotes();
		t = (Types)EnumValue(num++, typeof(Types));
		int num2 = (int)Line(num++).GetFloat(0);
		for (int i = 0; i < num2; i++)
		{
			cobjects.Add(Line(num++).StripQuotes());
		}
		for (int j = num; j < base.Count; j++)
		{
			consts.Add(base[num++]);
		}
		CustomClear();
	}

	protected override void PrepareSerialize()
	{
		base.PrepareSerialize();
		Clear(rec: false);
		base.JoinedArgument2 = t.ToString();
		AddQuotedLiteral(oname);
		AddQuotedLiteral(t.ToString());
		AddLiteral(cobjects.Count);
		foreach (string cobject in cobjects)
		{
			AddQuotedLiteral(cobject);
		}
		foreach (Container @const in consts)
		{
			Add(@const);
		}
	}
}
