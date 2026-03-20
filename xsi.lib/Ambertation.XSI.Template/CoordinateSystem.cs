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

public sealed class CoordinateSystem : ExtendedContainer
{
	public enum CoordinateOrientations
	{
		LeftHanded,
		RightHanded
	}

	public enum VAxisOrientations
	{
		Down,
		Up
	}

	public enum UAxisOrientations
	{
		Right,
		Left
	}

	public enum AxisOrientations
	{
		Right,
		Left,
		Up,
		Down,
		In,
		Out
	}

	private CoordinateOrientations co;

	private VAxisOrientations vx;

	private UAxisOrientations ux;

	private AxisOrientations x;

	private AxisOrientations y;

	private AxisOrientations z;

	public CoordinateOrientations CoordinateOrientation => co;

	public VAxisOrientations VAxisOrientation => vx;

	public UAxisOrientations UAxisOrientation => ux;

	public AxisOrientations XAxisOrientation => x;

	public AxisOrientations YAxisOrientation => y;

	public AxisOrientations ZAxisOrientation => z;

	public CoordinateSystem(Container parent, string args)
		: base(parent, args)
	{
		co = CoordinateOrientations.RightHanded;
		ux = UAxisOrientations.Right;
		vx = VAxisOrientations.Up;
		x = AxisOrientations.Right;
		y = AxisOrientations.Up;
		z = AxisOrientations.Out;
	}

	protected override void FinishDeSerialize()
	{
		base.FinishDeSerialize();
		co = (CoordinateOrientations)Line(0).GetFloat(0);
		ux = (UAxisOrientations)Line(1).GetFloat(0);
		vx = (VAxisOrientations)Line(2).GetFloat(0);
		x = (AxisOrientations)Line(3).GetFloat(0);
		y = (AxisOrientations)Line(4).GetFloat(0);
		z = (AxisOrientations)Line(5).GetFloat(0);
		CustomClear();
	}

	protected override void PrepareSerialize()
	{
		base.PrepareSerialize();
		Clear(rec: false);
		AddLiteral((int)co);
		AddLiteral((int)ux);
		AddLiteral((int)vx);
		AddLiteral((int)x);
		AddLiteral((int)y);
		AddLiteral((int)z);
	}
}
