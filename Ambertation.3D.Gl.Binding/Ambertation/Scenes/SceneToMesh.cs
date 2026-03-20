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
using Ambertation.Geometry;
using Ambertation.Graphics;
using Ambertation.Scenes.Collections;
using OpenTK.Mathematics;
using TkVec2 = OpenTK.Mathematics.Vector2;
using TkVec3 = OpenTK.Mathematics.Vector3;
using GeoVec3i = Ambertation.Geometry.Vector3i;

namespace Ambertation.Scenes;

public class SceneToMesh : IConvertScene, IDisposable
{
	protected static Color[] Colors = new Color[10]
	{
		Color.Orange, Color.YellowGreen, Color.Magenta, Color.Maroon, Color.LimeGreen,
		Color.Red, Color.Yellow, Color.Blue, Color.BlueViolet, Color.ForestGreen
	};

	protected int index;
	private static Random rnd = new Random();
	private Hashtable colormap;
	private Scene scn;
	private float scale;
	internal DirectXPanel dxp;

	protected float Scale => dxp != null ? dxp.Settings.LineWidth * dxp.Settings.JointScale : scale;

	public Color GetRandomColor()
	{
		if (index < Colors.Length) return Colors[index++];
		return Color.FromArgb(rnd.Next(190) + 30, rnd.Next(190) + 30, rnd.Next(190) + 30);
	}

	public SceneToMesh(Scene scn, DirectXPanel dp) : this(scn, dp.Settings.LineWidth)
	{
		dxp = dp; colormap = new Hashtable();
	}

	public Color GetJointColor(Joint j)
	{
		if (j == null) return Color.Black;
		colormap ??= new Hashtable();
		object obj = colormap[j.Name];
		if (obj == null) { obj = GetRandomColor(); colormap[j.Name] = obj; }
		return (Color)obj;
	}

	public SceneToMesh(Scene scn, double scale) { this.scn = scn; this.scale = (float)scale; dxp = null; }

	public object Convert() => ConvertToDx();

	protected void AddJointMesh(JointCollectionBase selected, MeshList ret, Joint joint)
	{
		float num = Scale;
		if (selected != null && selected.Contains(joint)) num *= 2f;
		Matrix4 transform = Converter.ToDx(joint);
		MeshBox meshBox = new MeshBox(GlMesh.CreateSphere(num, 24, 24), 1,
			DirectXPanel.GetMaterial(GetJointColor(joint)), transform);
		meshBox.Wire = false; meshBox.JointMesh = true;
		ret.Add(meshBox);
		if (dxp != null && !joint.Parent.Root)
		{
			TkVec3 stop = TkVec3.TransformPosition(TkVec3.Zero, Converter.ToDx(joint));
			MeshBox[] arr = dxp.CreateLineMesh(TkVec3.Zero, stop,
				DirectXPanel.GetMaterial(Color.LightYellow), wire: false, arrow: false);
			foreach (MeshBox mb in arr) mb.JointMesh = true;
			ret.AddRange(arr);
		}
		foreach (Joint item in joint) AddJointMesh(selected, meshBox, item);
	}

	protected void AddJointMeshs(JointCollectionBase selected, MeshList ret, Joint root)
	{ foreach (Joint item in root) AddJointMesh(selected, ret, item); }

	public MeshList ConvertToDx(JointCollectionBase joints)
	{
		scn.ClearTags();
		Scene scene = new Scene();
		scene.DefaultMaterial.Diffuse = Color.Black; scene.DefaultMaterial.Ambient = Color.Black;
		scene.DefaultMaterial.Specular = Color.FromArgb(32, 32, 32); scene.DefaultMaterial.SpecularPower = 300.0;
		scene.DefaultMaterial.Mode = Material.TextureModes.Default;
		MeshList meshList = new MeshList();
		AddJointMeshs(joints, meshList, scn.RootJoint);
		if (joints.Count == 0) return meshList;
		foreach (Mesh item in scn.SceneRoot)
		{
			Mesh dst = scene.CreateMesh(item.Name);
			for (int i = 0; i < item.FaceIndices.Count; i++) CopyElement(joints, item, dst, i);
		}
		scn.ClearTags();
		SceneToMesh stm = dxp == null ? new SceneToMesh(scene, Scale) : new SceneToMesh(scene, dxp);
		MeshList m = stm.ConvertToDx(); meshList.AddRange(m); scene.Dispose(); return meshList;
	}

	private int Clamp(int i) => Math.Max(0, Math.Min(255, i));

	private void CopyElement(JointCollectionBase joints, Mesh src, Mesh dst, int findex)
	{
		var v3i = new GeoVec3i(0, 0, 0);
		for (int i = 0; i < 3; i++)
		{
			int num = src.FaceIndices[findex][i]; v3i[i] = dst.Vertices.Count;
			dst.Vertices.Add(src.Vertices[num]);
			if (src.Normals.Count > 0) dst.Normals.Add(src.Normals[num]);
			Color c = Color.FromArgb(255, Color.Black);
			foreach (Envelope envelope in src.Envelopes)
			{
				if (joints.Contains(envelope.Joint))
				{
					double w = envelope.Weights[num];
					Color color = Blend(w, Color.Black, GetJointColor(envelope.Joint));
					c = Color.FromArgb(Clamp(c.A + color.A), Clamp(c.R + color.R), Clamp(c.G + color.G), Clamp(c.B + color.B));
				}
			}
			dst.Colors.Add(Helpers.ToVector4(c));
		}
		dst.FaceIndices.Add(v3i);
	}

	public MeshList ConvertToDx(Joint j) => ConvertToDx(j, GetJointColor(j));
	public MeshList ConvertToDx(Joint j, Color maxcl) => ConvertToDx(j, Color.FromArgb(0, maxcl), maxcl);

	public MeshList ConvertToDx(Joint j, Color mincl, Color maxcl)
	{
		scn.ClearTags();
		Scene scene = new Scene();
		scene.DefaultMaterial.Diffuse = Color.Transparent; scene.DefaultMaterial.Ambient = Color.Transparent;
		scene.DefaultMaterial.Specular = Color.Transparent; scene.DefaultMaterial.SpecularPower = 100.0;
		scene.DefaultMaterial.Mode = Material.TextureModes.Default;
		MeshList meshList = new MeshList();
		JointCollection jc = new JointCollection(); jc.Add(j);
		AddJointMeshs(jc, meshList, scn.RootJoint); jc.Clear(); jc.Dispose();
		foreach (Mesh item in scn.SceneRoot)
		{
			Envelope envelope = null;
			foreach (Envelope e2 in item.Envelopes) { if (e2.Joint == j) { envelope = e2; break; } }
			if (envelope == null) continue;
			Mesh dst = scene.CreateMesh(item.Name);
			for (int i = 0; i < item.FaceIndices.Count; i++)
				if (HasWeight(item, i, envelope)) CopyElement(item, dst, i, mincl, maxcl, envelope);
		}
		scn.ClearTags();
		SceneToMesh stm = dxp == null ? new SceneToMesh(scene, Scale) : new SceneToMesh(scene, dxp);
		MeshList m = stm.ConvertToDx(); meshList.AddRange(m); scene.Dispose(); return meshList;
	}

	private bool HasWeight(Mesh src, int findex, Envelope e)
	{ for (int i = 0; i < 3; i++) if (e.Weights[src.FaceIndices[findex][i]] != 0.0) return true; return false; }

	private void CopyElement(Mesh src, Mesh dst, int findex, Color mincl, Color maxcl, Envelope e)
	{
		var v3i = new GeoVec3i(0, 0, 0);
		for (int i = 0; i < 3; i++)
		{
			int num = src.FaceIndices[findex][i]; v3i[i] = dst.Vertices.Count;
			dst.Vertices.Add(src.Vertices[num]);
			if (src.Normals.Count > 0) dst.Normals.Add(src.Normals[num]);
			if (src.Colors.Count > 0 && e == null) { dst.Colors.Add(src.Colors[num]); continue; }
			dst.Colors.Add(Helpers.ToVector4(Blend(e.Weights[num], mincl, maxcl)));
		}
		dst.FaceIndices.Add(v3i);
	}

	private Color Blend(double w, Color a, Color b)
		=> Color.FromArgb(Blend(w, a.A, b.A), Blend(w, a.R, b.R), Blend(w, a.G, b.G), Blend(w, a.B, b.B));
	private int Blend(double w, int none, int full)
		=> (int)Math.Min(255.0, Math.Max(0.0, w * full + (1.0 - w) * none));

	public MeshList ConvertToDx()
	{
		scn.ClearTags(); MeshList meshList = new MeshList();
		AddJointMeshs(null, meshList, scn.RootJoint);
		foreach (Mesh item in scn.SceneRoot) AddMesh(meshList, item);
		scn.ClearTags(); return meshList;
	}

	private void AddMesh(MeshList ret, Mesh m)
	{
		MeshBox mb = AddMesh(ret, m, isbb: false);
		if (mb == null) return;
		foreach (Mesh child in m.Childs) AddMesh(mb, child);
	}

	private MeshBox AddMesh(MeshList ret, Mesh m, bool isbb)
	{
		MeshBox mb = CreateDxMesh(m, isbb);
		if (mb != null) ret.Add(mb);
		return mb;
	}

	public static MeshBox CreateDxMesh(Mesh m, bool isbb)
	{
		if (m.Vertices.Count == 0 || m.FaceIndices.Count == 0) return null;
		short[] fd = m.FaceIndices.ToArrayOfShort();
		int[] indices = new int[fd.Length];
		for (int i = 0; i < fd.Length; i++) indices[i] = fd[i];
		GlMesh glMesh = BuildGlMesh(m, indices);
		if (glMesh == null) return null;
		GlMaterial mat = LoadMaterial(m);
		MeshBox meshBox = new MeshBox(glMesh, 1, mat);
		meshBox.Wire = false;
		if (m.Material.Texture.TextureImage == null) m.Material.Texture.ImportTextureImage();
		meshBox.SetTexture(m.Material.Texture.TextureImage);
		meshBox.Transform = Converter.ToDx(m);
		meshBox.TextureMode = m.Material.Mode;
		if (isbb)
		{
			meshBox.CullMode = MeshBox.Cull.None;
			meshBox.Material = DirectXPanel.GetMaterial(Color.Black);
			meshBox.Wire = true; meshBox.IgnoreDuringCameraReset = true;
		}
		return meshBox;
	}

	private static GlMesh BuildGlMesh(Mesh m, int[] indices)
	{
		int vc = m.Vertices.Count;
		bool hasN = m.Normals.Count == vc, hasUV = m.TextureCoordinates.Count == vc, hasC = m.Colors.Count == vc;
		var pos = new TkVec3[vc];
		for (int i = 0; i < vc; i++) pos[i] = Converter.ToDx(m.Vertices[i]);
		if (hasN && hasUV)
		{
			var n = new TkVec3[vc]; var uv = new TkVec2[vc];
			for (int i = 0; i < vc; i++) { n[i] = Converter.ToDx(m.Normals[i]); uv[i] = new TkVec2((float)m.TextureCoordinates[i].X, (float)(0.0 - m.TextureCoordinates[i].Y)); }
			return GlMesh.FromPositionNormalTextured(pos, n, uv, indices);
		}
		if (hasN && hasC)
		{
			var n = new TkVec3[vc]; var c = new int[vc];
			for (int i = 0; i < vc; i++) { n[i] = Converter.ToDx(m.Normals[i]); c[i] = Helpers.ToColor(m.Colors[i]).ToArgb(); }
			return GlMesh.FromPositionNormalColored(pos, n, c, indices);
		}
		if (hasN)
		{
			var n = new TkVec3[vc];
			for (int i = 0; i < vc; i++) n[i] = Converter.ToDx(m.Normals[i]);
			return GlMesh.FromPositionNormal(pos, n, indices);
		}
		if (hasUV)
		{
			var n = new TkVec3[vc]; var uv = new TkVec2[vc];
			for (int i = 0; i < vc; i++) uv[i] = new TkVec2((float)m.TextureCoordinates[i].X, (float)(0.0 - m.TextureCoordinates[i].Y));
			return GlMesh.FromPositionNormalTextured(pos, n, uv, indices);
		}
		if (hasC)
		{
			var c = new int[vc];
			for (int i = 0; i < vc; i++) c[i] = Helpers.ToColor(m.Colors[i]).ToArgb();
			return GlMesh.FromPositionColored(pos, c, indices);
		}
		return GlMesh.FromPositionNormal(pos, new TkVec3[vc], indices);
	}

	private static GlMaterial LoadMaterial(Mesh m)
	{
		var mat = new GlMaterial();
		m.Material.Tag = mat; mat.Diffuse = m.Material.Diffuse; mat.Specular = m.Material.Specular;
		mat.SpecularSharpness = (float)m.Material.SpecularPower; mat.Emissive = m.Material.Emmissive; mat.Ambient = m.Material.Ambient;
		return mat;
	}

	public void Dispose() { dxp = null; colormap?.Clear(); colormap = null; scn = null; }
}