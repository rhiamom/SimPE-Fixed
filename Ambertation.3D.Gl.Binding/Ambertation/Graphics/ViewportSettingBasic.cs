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
using System.ComponentModel;

namespace Ambertation.Graphics;

[TypeConverter(typeof(ExpandableObjectConverter))]
public class ViewportSettingBasic
{
	public enum FillModes
	{
		Default,
		Solid,
		WireframeOverlay,
		Wireframe,
		Point
	}

	protected bool autoaxismesh;
	protected bool usespec;
	protected bool uselight;
	protected bool joints;
	protected bool allowscr;
	protected bool txtr;
	protected bool bb;
	protected GlShadeMode smode;
	protected FillModes fm;
	protected float jsz;
	private bool fstate;
	private bool fattr;
	private bool eventpause;

	[Category("Settings")]
	public bool EnableTextures
	{
		get => txtr;
		set { if (txtr != value) { txtr = value; FireStateChangeEvent(); } }
	}

	[Category("Settings")]
	public bool RenderBoundingBoxes
	{
		get => bb;
		set { if (bb != value) { bb = value; FireStateChangeEvent(); } }
	}

	[Category("Settings")]
	public FillModes FillMode
	{
		get => fm;
		set { if (fm != value) { fm = value; FireStateChangeEvent(); } }
	}

	[Browsable(false)]
	[Category("Settings")]
	public bool AllowSettingsDialog
	{
		get => allowscr;
		set { if (allowscr != value) { allowscr = value; FireStateChangeEvent(); } }
	}

	[Category("Settings")]
	public bool RenderJoints
	{
		get => joints;
		set { if (joints != value) { joints = value; FireStateChangeEvent(); } }
	}

	[Category("Settings")]
	public bool EnableSpecularHighlights
	{
		get => usespec;
		set { if (usespec != value) { usespec = value; FireStateChangeEvent(); } }
	}

	[Category("Settings")]
	public bool EnableLights
	{
		get => uselight;
		set { if (uselight != value) { uselight = value; FireStateChangeEvent(); } }
	}

	[Category("Settings")]
	public GlShadeMode ShadeMode
	{
		get => smode;
		set { if (smode != value) { smode = value; FireStateChangeEvent(); } }
	}

	[Category("Settings")]
	public bool AddAxis
	{
		get => autoaxismesh;
		set { if (autoaxismesh != value) { autoaxismesh = value; FireStateChangeEvent(); } }
	}

	[Category("Settings")]
	public float JointScale
	{
		get => jsz;
		set { if (jsz != value) { jsz = value; FireStateChangeEvent(); } }
	}

	public event EventHandler ChangedAttribute;
	public event EventHandler ChangedState;

	internal ViewportSettingBasic(DirectXPanel parent)
	{
		txtr = true;
		fm = FillModes.Default;
		allowscr = true;
		joints = true;
		uselight = true;
		usespec = true;
		smode = GlShadeMode.Phong;
		autoaxismesh = true;
		jsz = 10f;
		bb = false;
		eventpause = false;
	}

	protected void FireStateChangeEvent()
	{
		if (!eventpause)
			this.ChangedState?.Invoke(this, new EventArgs());
		else
			fstate = true;
	}

	protected void FireAttributeChangeEvent()
	{
		if (!eventpause)
			this.ChangedAttribute?.Invoke(this, new EventArgs());
		else
			fattr = true;
	}

	public void BeginUpdate()
	{
		fstate = false;
		fattr = false;
		eventpause = true;
	}

	public void EndUpdate()
	{
		EndUpdate(fattr, fstate);
	}

	public void EndUpdate(bool fireattr, bool firestat)
	{
		eventpause = false;
		fstate = false;
		fattr = false;
		if (fireattr && firestat)
			FireStateChangeEvent();
		else if (fireattr)
			FireAttributeChangeEvent();
		else if (firestat)
			FireStateChangeEvent();
	}

	internal FillModes GetFillMode(MeshBox box, int pass = 0)
	{
		if (fm == FillModes.Default || box.SpecialMesh)
			return box.Wire ? FillModes.Wireframe : FillModes.Solid;
		if (fm == FillModes.WireframeOverlay)
			return pass == 1 ? FillModes.Solid : FillModes.Wireframe;
		return fm;
	}
}
