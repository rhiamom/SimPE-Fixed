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
using System.Collections;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using Ambertation.Geometry;
using Ambertation.Scenes;
using OpenTK.Mathematics;
using Vector3 = OpenTK.Mathematics.Vector3;

namespace Ambertation.Graphics;

public class MeshBox : MeshList, IDisposable
{
	public enum Cull { Default, None, Clockwise, CounterClockwise }

	private MeshBox parent;
	private GlMesh mesh;
	private GlMaterial mat;
	private Matrix4 trans;
	private int ssc;
	private bool wire;
	private bool opaque;
	private bool billboard;
	private bool sort;
	private bool ztest;
	private Cull cull;
	private Stream txtrstream;
	private bool ignoreforcam;
	private bool isjointmesh;
	private Ambertation.Scenes.Material.TextureModes blend;
	private GlTexture txtr;
	private MeshBox txtrmb;
	private Matrix4 wrld;
	private double dist;

	public bool SpecialMesh => JointMesh || IgnoreDuringCameraReset;
	public bool JointMesh { get => isjointmesh; set => isjointmesh = value; }
	public bool Billboard { get => billboard; set => billboard = value; }
	public bool Sort { get => sort; set => sort = value; }
	public bool ZTest { get => ztest; set => ztest = value; }
	public MeshBox Parent => parent;
	public Ambertation.Scenes.Material.TextureModes TextureMode { get => blend; set => blend = value; }
	public bool IgnoreDuringCameraReset { get => ignoreforcam; set => ignoreforcam = value; }
	public Stream TextureStream => txtrstream;

	public GlTexture Texture
	{
		get
		{
			if (txtrmb != null) return txtrmb.Texture;
			return txtr;
		}
	}

	public Cull CullMode { get => cull; set => cull = value; }

	public bool Opaque
	{
		get
		{
			if (TextureMode == Ambertation.Scenes.Material.TextureModes.MaterialWithAlpha) return false;
			if (mat.Diffuse.A != byte.MaxValue) return mat.Diffuse.A == 0;
			return true;
		}
		set => opaque = value;
	}

	public bool Wire { get => wire; set => wire = value; }
	public GlMesh Mesh => mesh;

	public GlMaterial Material { get => mat; set => mat = value; }

	public Matrix4 Transform { get => trans; set => trans = value; }
	public int SubSetCount => ssc;
	internal Matrix4 World => wrld;
	internal double Distance => dist;

	public MeshBox(GlMesh mesh, int subsetcount)
		: this(mesh, subsetcount, new GlMaterial(), Matrix4.Identity) { }

	public MeshBox(GlMesh mesh)
		: this(mesh, new GlMaterial(), Matrix4.Identity) { }

	public MeshBox(GlMesh mesh, GlMaterial mat)
		: this(mesh, mat, Matrix4.Identity) { }

	public MeshBox(GlMesh mesh, int subsetcount, GlMaterial mat)
		: this(mesh, subsetcount, mat, Matrix4.Identity) { }

	public MeshBox(GlMesh mesh, GlMaterial mat, Matrix4 transform)
		: this(mesh, mesh != null ? mesh.NumberAttributes : 1, mat, transform) { }

	public MeshBox(GlMesh mesh, int subsetcount, GlMaterial mat, Matrix4 transform)
		: this(mesh, subsetcount, mat, transform, wire: true, opaque: true) { }

	public MeshBox(GlMesh mesh, int subsetcount, GlMaterial mat, Matrix4 transform, bool wire, bool opaque)
	{
		billboard = false; sort = false; ztest = true;
		this.mesh = mesh; this.mat = mat; trans = transform; ssc = subsetcount;
		this.wire = wire; this.opaque = opaque;
		cull = Cull.Default; txtrstream = null;
		ignoreforcam = false; parent = null; isjointmesh = false;
		blend = Ambertation.Scenes.Material.TextureModes.Default;
	}

	protected void SetParent(MeshBox p) => parent = p;

	public void PrepareTexture()
	{
		if (txtrmb != null) { txtrmb.PrepareTexture(); return; }
		if (txtr != null && !txtr.Disposed) return;
		txtr?.Dispose();
		txtr = null;
		if (TextureStream != null && TextureStream.CanSeek && TextureStream.CanRead)
		{
			try
			{
				TextureStream.Seek(0, SeekOrigin.Begin);
				txtr = GlTexture.FromStream(TextureStream);
			}
			catch { }
		}
	}

	public void SetTexture(Image img)
	{
		txtrstream?.Close();
		txtrmb = null;
		if (txtr != null) { txtr.Dispose(); txtr = null; }
		if (img != null)
		{
			txtrstream = new MemoryStream();
			img.Save(txtrstream, ImageFormat.Bmp);
			txtrstream.Seek(0, SeekOrigin.Begin);
		}
		else txtrstream = null;
	}

	public void SetTexture(MeshBox mb) { txtrstream?.Close(); txtrstream = null; txtrmb = mb; }

	internal GlCullMode GetCullMode(GlCullMode def)
	{
		return cull switch
		{
			Cull.Default => def,
			Cull.None => GlCullMode.None,
			Cull.Clockwise => GlCullMode.Clockwise,
			Cull.CounterClockwise => GlCullMode.CounterClockwise,
			_ => def
		};
	}

	internal Vector3[] GetBoundingBoxVectors()
	{
		var result = new Vector3[]
		{
			new(float.MaxValue, float.MaxValue, float.MaxValue),
			new(float.MinValue, float.MinValue, float.MinValue)
		};
		if (mesh != null && !mesh.Disposed)
		{
			foreach (var p in mesh.GetPositions())
			{
				if (p.X < result[0].X) result[0].X = p.X;
				if (p.Y < result[0].Y) result[0].Y = p.Y;
				if (p.Z < result[0].Z) result[0].Z = p.Z;
				if (p.X > result[1].X) result[1].X = p.X;
				if (p.Y > result[1].Y) result[1].Y = p.Y;
				if (p.Z > result[1].Z) result[1].Z = p.Z;
			}
		}
		return result;
	}

	protected override void OnAdd(MeshBox m) { base.OnAdd(m); m?.SetParent(this); }
	protected override void OnRemove(MeshBox m) { base.OnRemove(m); m?.SetParent(null); }

	public override void Dispose()
	{
		base.Dispose();
		txtrmb = null; parent = null;
		try { mesh?.Dispose(); } catch { }
		try { if (txtrstream != null && txtrstream.CanRead) txtrstream.Close(); } catch { }
		txtr?.Dispose();
		txtr = null; txtrstream = null; mesh = null;
	}

	public static BoundingBox BoundingBoxFromMesh(GlMesh mesh, Ambertation.Geometry.Matrix m)
	{
		var vmin = new Ambertation.Geometry.Vector3(double.MaxValue, double.MaxValue, double.MaxValue);
		var vmax = new Ambertation.Geometry.Vector3(double.MinValue, double.MinValue, double.MinValue);
		if (mesh != null && !mesh.Disposed)
		{
			foreach (var p in mesh.GetPositions())
			{
				var v = new Ambertation.Geometry.Vector3(p.X, p.Y, p.Z);
				if (m != null) v = m * v;
				if (v.X < vmin.X) vmin.X = v.X;
				if (v.Y < vmin.Y) vmin.Y = v.Y;
				if (v.Z < vmin.Z) vmin.Z = v.Z;
				if (v.X > vmax.X) vmax.X = v.X;
				if (v.Y > vmax.Y) vmax.Y = v.Y;
				if (v.Z > vmax.Z) vmax.Z = v.Z;
			}
		}
		if (vmin.X > vmax.X) { vmin.X = 0; vmax.X = 0; }
		if (vmin.Y > vmax.Y) { vmin.Y = 0; vmax.Y = 0; }
		if (vmin.Z > vmax.Z) { vmin.Z = 0; vmax.Z = 0; }
		return new BoundingBox(vmin, vmax);
	}

	public BoundingBox GetBoundingBox(bool rec, bool all)
	{
		return GetBoundingBox(Converter.FromDx(trans), rec, all);
	}

	public BoundingBox GetBoundingBox(Ambertation.Geometry.Matrix basem, bool rec, bool all)
	{
		if (mesh == null || mesh.Disposed)
			return new BoundingBox(Ambertation.Geometry.Vector3.Zero, new Ambertation.Geometry.Vector3(0.0001, 0.0001, 0.0001));
		BoundingBox result = BoundingBoxFromMesh(mesh, basem);
		foreach (MeshBox item in (IEnumerable)this)
		{
			if (all || !item.SpecialMesh)
				result += item.GetBoundingBox(basem, rec: true, all);
		}
		return result;
	}

	internal void SetupSortWorld(Matrix4 world, Vector3 camPos)
	{
		wrld = world;
		dist = GetDistance(camPos);
	}

	internal Vector3 GetCenterOfMass()
	{
		BoundingBox bb = GetBoundingBox(Converter.FromDx(wrld), rec: false, all: true);
		var v = (bb.Min + bb.Max) / 2.0;
		return Converter.ToDx(v);
	}

	internal double GetDistance(Vector3 v)
	{
		Vector3 com = GetCenterOfMass();
		return (v - com).Length;
	}
}
