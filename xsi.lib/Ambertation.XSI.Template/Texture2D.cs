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
 
 using System.ComponentModel;
using System.Drawing;
using Ambertation.Geometry;

namespace Ambertation.XSI.Template;

public sealed class Texture2D : ArgumentContainer
{
	public enum BlendingTypes
	{
		AlphaMask = 1,
		IntensityMask,
		NoMask,
		RGBModulation
	}

	public enum MappingTypes
	{
		XYProjection,
		XZProjection,
		YZProjection,
		UVMapUnwrapped,
		UVMapWrapped,
		CylindricalProjection,
		SphericalProjection,
		ReflectionMap
	}

	private string flname;

	private MappingTypes t;

	private BlendingTypes bt;

	private Size imgsz;

	private PointUV cmin;

	private PointUV cmax;

	private PointUV repeat;

	private bool uvswap;

	private bool ualt;

	private bool valt;

	private PointUVf scale;

	private PointUVf offset;

	private Matrix proj;

	private double blending;

	private double ambient;

	private double diffuse;

	private double specular;

	private double trans;

	private double refl;

	private double rough;

	public double Blending
	{
		get
		{
			return blending;
		}
		set
		{
			blending = value;
		}
	}

	public double Ambient
	{
		get
		{
			return ambient;
		}
		set
		{
			ambient = value;
		}
	}

	public double Diffuse
	{
		get
		{
			return diffuse;
		}
		set
		{
			diffuse = value;
		}
	}

	public double Specular
	{
		get
		{
			return specular;
		}
		set
		{
			specular = value;
		}
	}

	public double Transparency
	{
		get
		{
			return trans;
		}
		set
		{
			trans = value;
		}
	}

	public double Reflectivity
	{
		get
		{
			return refl;
		}
		set
		{
			refl = value;
		}
	}

	public double Roughness
	{
		get
		{
			return rough;
		}
		set
		{
			rough = value;
		}
	}

	public BlendingTypes BlendingType
	{
		get
		{
			return bt;
		}
		set
		{
			bt = value;
		}
	}

	public Matrix ProjectionMatrix => proj;

	[Category("UV-Mapping")]
	public bool UAlternat
	{
		get
		{
			return ualt;
		}
		set
		{
			ualt = value;
		}
	}

	[Category("UV-Mapping")]
	public bool VAlternat
	{
		get
		{
			return valt;
		}
		set
		{
			valt = value;
		}
	}

	[Category("UV-Mapping")]
	public bool UVSwap
	{
		get
		{
			return uvswap;
		}
		set
		{
			uvswap = value;
		}
	}

	[Category("UV-Mapping")]
	public PointUVf UVScale
	{
		get
		{
			return scale;
		}
		set
		{
			scale = value;
		}
	}

	[Category("UV-Mapping")]
	public PointUVf UVOffset
	{
		get
		{
			return offset;
		}
		set
		{
			offset = value;
		}
	}

	[Category("UV-Mapping")]
	public PointUV UVRepeat
	{
		get
		{
			return repeat;
		}
		set
		{
			repeat = value;
		}
	}

	[Category("UV-Mapping")]
	public PointUV UVCropMin
	{
		get
		{
			return cmin;
		}
		set
		{
			cmin = value;
		}
	}

	[Category("UV-Mapping")]
	public PointUV UVCropMax
	{
		get
		{
			return cmax;
		}
		set
		{
			cmax = value;
		}
	}

	public string TextureName
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

	public MappingTypes MappingType
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

	public Size ImageDimension
	{
		get
		{
			return imgsz;
		}
		set
		{
			imgsz = value;
		}
	}

	public string ImageFileName
	{
		get
		{
			return flname;
		}
		set
		{
			flname = value;
			if (flname == null)
			{
				flname = "";
			}
		}
	}

	public Texture2D(Container parent, string args)
		: base(parent, args)
	{
		proj = new Matrix();
		cmin = new PointUV(0, 0);
		cmax = new PointUV(255, 255);
		repeat = new PointUV(1, 1);
		scale = new PointUVf(1.0, 1.0);
		offset = new PointUVf(0.0, 0.0);
		Reset();
	}

	private void Reset()
	{
		flname = "";
		t = MappingTypes.UVMapUnwrapped;
		bt = BlendingTypes.NoMask;
		imgsz = new Size(0, 0);
		proj.MakeIdentity();
		blending = 1.0;
		ambient = 0.75;
		diffuse = 1.0;
		specular = 0.0;
		trans = 1.0;
		refl = 0.0;
		rough = 0.0;
	}

	protected override void ResetArgs()
	{
		base.ResetArgs("Scene_Root");
	}

	protected override void FinishDeSerialize()
	{
		base.FinishDeSerialize();
		Reset();
		int index = 0;
		flname = Line(index++).StripQuotes();
		t = (MappingTypes)EnumValue(index++, typeof(MappingTypes));
		int width = Line(index++).GetInt(0);
		int height = Line(index++).GetInt(0);
		imgsz = new Size(width, height);
		if (index < base.Count)
		{
			double[] array = ReadFloatSequence(ref index, 13);
			cmin.U = (int)array[0];
			cmax.U = (int)array[1];
			cmin.V = (int)array[2];
			cmax.V = (int)array[3];
			uvswap = array[4] != 0.0;
			repeat.U = (int)array[5];
			repeat.V = (int)array[6];
			ualt = array[7] != 0.0;
			valt = array[8] != 0.0;
			scale.U = array[9];
			scale.V = array[10];
			offset.U = array[11];
			offset.V = array[12];
			proj.SetFields(ReadFloatSequence(ref index, 16));
			bt = (BlendingTypes)EnumValue(index++, typeof(BlendingTypes));
			blending = Line(index++).GetFloat(0);
			ambient = Line(index++).GetFloat(0);
			diffuse = Line(index++).GetFloat(0);
			specular = Line(index++).GetFloat(0);
			trans = Line(index++).GetFloat(0);
			refl = Line(index++).GetFloat(0);
			rough = Line(index++).GetFloat(0);
		}
		CustomClear();
	}

	protected override void PrepareSerialize()
	{
		base.PrepareSerialize();
		Clear(rec: false);
		AddQuotedLiteral(flname);
		AddLiteral((int)t);
		AddLiteral(imgsz.Width);
		AddLiteral(imgsz.Height);
		AddLiteral(cmin.U);
		AddLiteral(cmax.U);
		AddLiteral(cmin.V);
		AddLiteral(cmax.V);
		AddLiteral(uvswap);
		AddLiteral(repeat.U);
		AddLiteral(repeat.V);
		AddLiteral(ualt);
		AddLiteral(valt);
		AddLiteral(scale.U);
		AddLiteral(scale.V);
		AddLiteral(offset.U);
		AddLiteral(offset.V);
		WriteFloatSequence(proj.GetFields(), oneline: false, asint: false);
		AddLiteral((int)bt);
		AddLiteral(blending);
		AddLiteral(ambient);
		AddLiteral(diffuse);
		AddLiteral(specular);
		AddLiteral(trans);
		AddLiteral(refl);
		AddLiteral(rough);
	}
}
