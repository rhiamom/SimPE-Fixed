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

public sealed class Scene : ArgumentContainer
{
	public enum Timing
	{
		FRAMES,
		SECONDS
	}

	private Timing t;

	private double start;

	private double stop;

	private double rate;

	public string SceneName
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

	public double Start
	{
		get
		{
			return start;
		}
		set
		{
			start = value;
		}
	}

	public double Stop
	{
		get
		{
			return stop;
		}
		set
		{
			stop = value;
		}
	}

	public double FrameRate
	{
		get
		{
			return rate;
		}
		set
		{
			rate = value;
		}
	}

	public Timing TimingType
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

	public Scene(Container parent, string args)
		: base(parent, args)
	{
		start = 1.0;
		stop = 100.0;
		rate = 24.0;
		t = Timing.FRAMES;
		ResetArgs();
	}

	protected override void ResetArgs()
	{
		ResetArgs("Scene");
	}

	protected override void FinishDeSerialize()
	{
		base.FinishDeSerialize();
		ResetArgs();
		t = (Timing)EnumValue(0, typeof(Timing));
		start = Line(1).GetFloat(0);
		stop = Line(2).GetFloat(0);
		rate = Line(3).GetFloat(0);
		CustomClear();
	}

	protected override void PrepareSerialize()
	{
		base.PrepareSerialize();
		Clear(rec: false);
		AddLiteral("\"" + t.ToString() + "\"");
		AddLiteral(start);
		AddLiteral(stop);
		AddLiteral(rate);
	}
}
