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
using Ambertation.Geometry;

namespace Ambertation.XSI.Template;

public sealed class Camera : ArgumentContainer
{
	private Vector3 p;

	private Vector3 i;

	private double roll;

	private double fov;

	private double near;

	private double far;

	public string CameraName
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

	public Vector3 Position
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

	public Vector3 PointOfInterest
	{
		get
		{
			return i;
		}
		set
		{
			i = value;
		}
	}

	public double Roll
	{
		get
		{
			return roll;
		}
		set
		{
			roll = value;
		}
	}

	public double FoV
	{
		get
		{
			return fov;
		}
		set
		{
			fov = value;
		}
	}

	public double Near
	{
		get
		{
			return near;
		}
		set
		{
			near = value;
		}
	}

	public double Far
	{
		get
		{
			return far;
		}
		set
		{
			far = value;
		}
	}

	public Camera(Container parent, string args)
		: base(parent, args)
	{
		p = Vector3.Zero;
		i = Vector3.Zero;
		fov = Math.PI / 4.0;
		roll = 0.0;
		near = 0.1;
		far = 32767.0;
	}

	protected override void ResetArgs()
	{
		base.ResetArgs("Camera");
	}

	protected override void FinishDeSerialize()
	{
		base.FinishDeSerialize();
		int index = 0;
		p = ReadVector3(ref index);
		i = ReadVector3(ref index);
		roll = Line(index++).GetFloat(0);
		fov = Line(index++).GetFloat(0);
		near = Line(index++).GetFloat(0);
		far = Line(index++).GetFloat(0);
		CustomClear();
	}

	protected override void PrepareSerialize()
	{
		base.PrepareSerialize();
		Clear(rec: false);
		WriteVector3(p, oneline: false);
		WriteVector3(i, oneline: false);
		AddLiteral(roll);
		AddLiteral(fov);
		AddLiteral(near);
		AddLiteral(far);
	}
}
