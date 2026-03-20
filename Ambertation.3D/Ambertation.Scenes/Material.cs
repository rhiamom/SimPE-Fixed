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

namespace Ambertation.Scenes;

public class Material : IDisposable
{
	public enum TextureModes
	{
		Default,
		ShadowTexture,
		Material,
		MaterialWithAlpha,
		MaterialWithTexture
	}

	private Texture txt;

	private Color d;

	private Color s;

	private Color e;

	private Color a;

	private double sp;

	private string name;

	private TextureModes blend;

	private object tag;

	public TextureModes Mode
	{
		get
		{
			return blend;
		}
		set
		{
			blend = value;
		}
	}

	public string Name
	{
		get
		{
			return name;
		}
		set
		{
			name = value;
		}
	}

	public Texture Texture => txt;

	public Color Diffuse
	{
		get
		{
			return d;
		}
		set
		{
			d = value;
		}
	}

	public Color Specular
	{
		get
		{
			return s;
		}
		set
		{
			s = value;
		}
	}

	public double SpecularPower
	{
		get
		{
			return sp;
		}
		set
		{
			sp = value;
		}
	}

	public Color Ambient
	{
		get
		{
			return a;
		}
		set
		{
			a = value;
		}
	}

	public Color Emmissive
	{
		get
		{
			return e;
		}
		set
		{
			e = value;
		}
	}

	public object Tag
	{
		get
		{
			return tag;
		}
		set
		{
			tag = value;
		}
	}

	internal Material(string name)
	{
		this.name = name;
		blend = TextureModes.Default;
		txt = new Texture(null, new Size(0, 0));
		d = Color.Silver;
		s = Color.White;
		e = Color.Black;
		a = Color.FromArgb(16, 16, 16);
		sp = 50.0;
	}

	public override string ToString()
	{
		return Name + " [" + GetType().Name + "]";
	}

	public void Dispose()
	{
		if (txt != null)
		{
			txt.Dispose();
		}
		txt = null;
		name = null;
	}
}
