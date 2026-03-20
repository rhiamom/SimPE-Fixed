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
using System.Drawing;
using System.IO;
using OpenTK.Mathematics;

namespace Ambertation.Graphics;

[TypeConverter(typeof(ExpandableObjectConverter))]
public class ViewportSetting : ViewportSettingBasic
{
	protected DirectXPanel parent;
	protected float angx, angy, angz, rad, camoffset;
	protected Vector3 campos, camtarget, pos, center;
	protected float fov, aspect, near, far;
	protected Matrix4 rotbase;
	protected bool alphablend, paused, useleft, useeff, autolightmesh;
	protected GlCullMode acull, mcull;
	protected float ascale, lscale;
	protected Color amb, lcol, lscol, bg;
	private string flname;

	[Category("Settings")] public Color BackgroundColor { get => bg; set { if (bg != value) { bg = value; FireStateChangeEvent(); } } }
	[Category("Settings")] public Color AmbientColor { get => amb; set { if (amb != value) { amb = value; FireStateChangeEvent(); } } }
	[Category("Light")] public Color LightColorDiffuse { get => lcol; set { if (lcol != value) { lcol = value; FireStateChangeEvent(); } } }
	[Category("Light")] public Color LightColorSpecular { get => lscol; set { if (lscol != value) { lscol = value; FireStateChangeEvent(); } } }
	[Category("Settings")] public bool AddLightIndicators { get => autolightmesh; set { if (autolightmesh != value) { autolightmesh = value; FireStateChangeEvent(); } } }
	[ReadOnly(true)][Category("Settings")] public float AxisScale { get => ascale; set { if (ascale != value) { ascale = value; FireStateChangeEvent(); } } }
	[Category("Settings")][ReadOnly(true)] public float LineWidth { get => lscale; set { if (lscale != value) { lscale = value; FireStateChangeEvent(); } } }
	[Category("Culling")] public GlCullMode MeshPassCullMode { get => mcull; set { if (mcull != value) { mcull = value; FireStateChangeEvent(); } } }
	[Category("Culling")] public GlCullMode AlphaPassCullMode { get => acull; set { if (acull != value) { acull = value; FireStateChangeEvent(); } } }
	[Category("Render state")] public bool Paused { get => paused; set { paused = value; } }
	[Category("Render state")] public bool EnableShaderEffectPass { get => useeff; set { if (useeff != value) { useeff = value; FireStateChangeEvent(); } } }
	[Category("Render state")] public bool UseLefthandedCoordinates { get => useleft; set { if (useleft != value) { useleft = value; FireStateChangeEvent(); } } }
	[Category("Render state")] public bool EnableAlphaBlendPass { get => alphablend; set { if (alphablend != value) { alphablend = value; FireStateChangeEvent(); } } }

	internal Matrix4 AngelRotation => Matrix4.Mult(Matrix4.CreateRotationY(AngelY), Matrix4.Mult(Matrix4.CreateRotationX(AngelX), Matrix4.CreateRotationZ(AngelZ)));
	internal Matrix4 Rotation { get => rotbase; set { rotbase = value; FireAttributeChangeEvent(); } }

	[Category("Camera")] public float InitialCameraOffsetScale { get => camoffset; set { camoffset = value; FireStateChangeEvent(); } }
	[Category("Camera")] public bool SetDefaultCamera { get => false; set { if (value) { parent.ResetDefaultViewport(); NearPlane = BoundingSphereRadius / 10f; FarPlane = NearPlane * 10000f; } } }
	[Category("Camera")] public float BoundingSphereRadius { get => Math.Max(0.01f, rad); set { rad = value; lscale = rad * 0.002f; near = rad / 10f; far = near * 10000f; FireAttributeChangeEvent(); } }
	[Category("Camera")] public float FarPlane { get => far; set { far = value; FireAttributeChangeEvent(); } }
	[Category("Camera")] public float NearPlane { get => near; set { near = value; FireAttributeChangeEvent(); } }
	[Browsable(false)][Category("Camera")] public Vector3 CameraPosition { get => campos; set { campos = value; FireAttributeChangeEvent(); } }
	[Browsable(false)][Category("Camera")] public Vector3 RealCameraPosition => new Vector3(X, Y, Z) + CameraPosition;
	[Browsable(false)][Category("Camera")] public Vector3 CameraTarget { get => camtarget; set { camtarget = value; FireAttributeChangeEvent(); } }
	[Category("Camera")][Browsable(false)] public Vector3 ObjectCenter { get => center; set { center = value; FireAttributeChangeEvent(); } }
	[Category("Camera")][ReadOnly(true)] public float Aspect { get => aspect; set { aspect = value; FireAttributeChangeEvent(); } }
	[Category("Camera")] public float FoV { get => fov; set { fov = value; FireAttributeChangeEvent(); } }
	[Category("Viewpoint")] public float AngelX { get => angx; set { angx = value; FireAttributeChangeEvent(); } }
	[Category("Viewpoint")] public float AngelZ { get => angz; set { angz = value; FireAttributeChangeEvent(); } }
	[Category("Viewpoint")] public float AngelY { get => angy; set { angy = value; FireAttributeChangeEvent(); } }
	[Category("Viewpoint")] public float X { get => pos.X; set { pos.X = value; FireAttributeChangeEvent(); } }
	[Category("Viewpoint")] public float Y { get => pos.Y; set { pos.Y = value; FireAttributeChangeEvent(); } }
	[Category("Viewpoint")] public float Z { get => pos.Z; set { pos.Z = value; FireAttributeChangeEvent(); } }
	[Browsable(false)] public float InputTranslationScale => 0.5f;
	[Browsable(false)] public float InputRotationScale => 100f;
	[Browsable(false)] public float InputScaleScale => 100f;

	internal ViewportSetting(DirectXPanel parent) : base(parent)
	{
		flname = null; this.parent = parent; Reset();
		autolightmesh = false; useleft = false; useeff = false; alphablend = true; paused = false;
		acull = GlCullMode.None; mcull = GlCullMode.Clockwise;
		ascale = 250f; lscale = 0.1f;
		amb = Color.FromArgb(128, 128, 128); bg = SystemColors.AppWorkspace;
		lcol = lscol = Color.White; camoffset = 1.2f;
	}

	~ViewportSetting() { try { Save(); } catch { } }

	private void Serialize(string fn)
	{
		using var s = File.Create(fn);
		var bw = new BinaryWriter(s);
		bw.Write(4); bw.Write(base.EnableTextures); bw.Write((int)base.FillMode);
		bw.Write(base.RenderJoints); bw.Write(base.EnableSpecularHighlights); bw.Write(base.EnableLights);
		bw.Write((int)base.ShadeMode); bw.Write(base.AddAxis); bw.Write(base.JointScale);
		bw.Write(InitialCameraOffsetScale); bw.Write(base.RenderBoundingBoxes); bw.Write(bg.ToArgb());
	}

	private void DeSerialize(string fn)
	{
		using var s = File.OpenRead(fn);
		BeginUpdate();
		var br = new BinaryReader(s);
		int num = br.ReadInt32();
		base.EnableTextures = br.ReadBoolean(); base.FillMode = (FillModes)br.ReadInt32();
		base.RenderJoints = br.ReadBoolean(); base.EnableSpecularHighlights = br.ReadBoolean();
		base.EnableLights = br.ReadBoolean(); base.ShadeMode = (GlShadeMode)br.ReadInt32();
		base.AddAxis = br.ReadBoolean(); base.JointScale = br.ReadSingle();
		if (num >= 2) InitialCameraOffsetScale = br.ReadSingle();
		if (num >= 3) base.RenderBoundingBoxes = br.ReadBoolean();
		if (num >= 4) BackgroundColor = Color.FromArgb(br.ReadInt32());
		EndUpdate();
	}

	public void Save() { if (flname == null) return; try { Serialize(flname); } catch (Exception ex) { Console.WriteLine(ex.Message); } }
	public void Load(string fn) { if (fn == null) return; flname = fn; try { DeSerialize(fn); } catch (Exception ex) { Console.WriteLine(ex.Message); } }

	public void ResetAngle() { angx = angy = angz = 0f; }
	public void Reset()
	{
		ResetAngle(); pos = Vector3.Zero; center = Vector3.Zero;
		fov = MathF.PI / 4f; near = 1f; far = 100f; rad = 0.01f;
		campos = new Vector3(0f, 3f, 5f); camtarget = Vector3.Zero;
		rotbase = Matrix4.Identity; FireAttributeChangeEvent();
	}
}
