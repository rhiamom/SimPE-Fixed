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

public sealed class Cluster : ArgumentContainer
{
	public enum Weightings
	{
		AVERAGE,
		ADDITIVE
	}

	private Weightings w;

	private string mref;

	private string ccref;

	private IntCollection indices;

	public string ClusterName
	{
		get
		{
			return base.Argument1;
		}
		set
		{
			base.Argument1 = value;
		}
	}

	public IntCollection Indices => indices;

	public Weightings Weighting
	{
		get
		{
			return w;
		}
		set
		{
			w = value;
		}
	}

	public string ReferencedModelName
	{
		get
		{
			return mref;
		}
		set
		{
			mref = value;
			if (mref == null)
			{
				mref = "";
			}
		}
	}

	public string ClusterCenterReference
	{
		get
		{
			return ccref;
		}
		set
		{
			ccref = value;
			if (ccref == null)
			{
				ccref = "";
			}
		}
	}

	public Cluster(Container parent, string args)
		: base(parent, args)
	{
		indices = new IntCollection();
		w = Weightings.AVERAGE;
		Reset();
	}

	protected override void ResetArgs()
	{
		base.ResetArgs("Point_AUTO");
	}

	private void Reset()
	{
		indices.Clear();
		mref = "";
		ccref = "";
	}

	protected override void FinishDeSerialize()
	{
		base.FinishDeSerialize();
		Reset();
		int num = 0;
		mref = Line(num++).StripQuotes();
		w = (Weightings)EnumValue(num++, typeof(Weightings));
		ccref = Line(num++).StripQuotes();
		int num2 = (int)Line(num++).GetFloat(0);
		for (int i = 0; i < num2; i++)
		{
			indices.Add((int)Line(num++).GetFloat(0));
		}
		CustomClear();
	}

	protected override void PrepareSerialize()
	{
		base.PrepareSerialize();
		Clear(rec: false);
		AddQuotedLiteral(mref);
		AddQuotedLiteral(w.ToString());
		AddQuotedLiteral(ccref);
		AddLiteral(indices.Count);
		foreach (int index in indices)
		{
			AddLiteral(index);
		}
	}
}
