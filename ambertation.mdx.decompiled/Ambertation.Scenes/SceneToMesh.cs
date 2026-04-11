using System;
using System.Collections;
using System.Drawing;
using Ambertation.Geometry;
using Ambertation.Graphics;
using Ambertation.Scenes.Collections;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;

namespace Ambertation.Scenes;

public class SceneToMesh : IConvertScene, IDisposable
{
	protected static Color[] Colors;

	protected int index;

	private static Random rnd;

	private Hashtable colormap;

	private Scene scn;

	private Device dev;

	private float scale;

	internal DirectXPanel dxp;

	protected float Scale
	{
		get
		{
			if (dxp == null)
			{
				return scale;
			}
			return dxp.Settings.LineWidth * dxp.Settings.JointScale;
		}
	}

	static SceneToMesh()
	{
		Colors = new Color[10]
		{
			Color.Orange,
			Color.YellowGreen,
			Color.Magenta,
			Color.Maroon,
			Color.LimeGreen,
			Color.Red,
			Color.Yellow,
			Color.Blue,
			Color.BlueViolet,
			Color.ForestGreen
		};
		rnd = new Random();
	}

	public SceneToMesh(Scene scn, DirectXPanel dp)
		: this(scn, dp.Device, dp.Settings.LineWidth)
	{
		dxp = dp;
		colormap = new Hashtable();
	}

	public SceneToMesh(Scene scn, Device dev, double scale)
	{
		this.scn = scn;
		this.dev = dev;
		this.scale = (float)scale;
		dxp = null;
	}

	protected void AddJointMesh(JointCollectionBase selected, MeshList ret, Joint joint)
	{
		//IL_00ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0106: Expected O, but got Unknown
		float num = Scale;
		if (selected != null && ((NodeCollectionBase)selected).Contains((Node)(object)joint))
		{
			num *= 2f;
		}
		Matrix transform = Converter.ToDx((Transformation)(object)joint);
		MeshBox meshBox = new MeshBox(Mesh.Sphere(dev, num, 24, 24), 1, DirectXPanel.GetMaterial(GetJointColor(joint)), transform)
		{
			Wire = false,
			JointMesh = true
		};
		ret.Add(meshBox);
		if (dxp != null && !((Node)joint.Parent).Root)
		{
			Vector3 stop = new Vector3(0f, 0f, 0f);
			stop.TransformCoordinate(Converter.ToDx((Transformation)(object)joint));
			MeshBox[] array = dxp.CreateLineMesh(new Vector3(0f, 0f, 0f), stop, DirectXPanel.GetMaterial(Color.LightYellow), wire: false, arrow: false);
			MeshBox[] array2 = array;
			for (int i = 0; i < array2.Length; i++)
			{
				array2[i].JointMesh = true;
			}
			ret.AddRange(array);
		}
		foreach (Joint item in (Node)joint)
		{
			Joint joint2 = item;
			AddJointMesh(selected, meshBox, joint2);
		}
	}

	protected void AddJointMeshs(JointCollectionBase selected, MeshList ret, Joint root)
	{
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Expected O, but got Unknown
		foreach (Joint item in (Node)root)
		{
			Joint joint = item;
			AddJointMesh(selected, ret, joint);
		}
	}

	private void AddMesh(MeshList ret, Mesh m)
	{
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Expected O, but got Unknown
		MeshBox meshBox = AddMesh(ret, m, isbb: false);
		if (meshBox == null)
		{
			return;
		}
		foreach (Mesh item in (NodeCollectionBase)m.Childs)
		{
			Mesh m2 = item;
			AddMesh(meshBox, m2);
		}
	}

	private MeshBox AddMesh(MeshList ret, Mesh m, bool isbb)
	{
		MeshBox meshBox = CreateDxMesh(dev, m, isbb);
		if (meshBox != null)
		{
			ret.Add(meshBox);
		}
		return meshBox;
	}

	private Color Blend(double w, Color mincl, Color maxcl)
	{
		return Color.FromArgb(Blend(w, mincl.A, maxcl.A), Blend(w, mincl.R, maxcl.R), Blend(w, mincl.G, maxcl.G), Blend(w, mincl.B, maxcl.B));
	}

	private int Blend(double w, int none, int full)
	{
		return (int)Math.Min(255.0, Math.Max(0.0, w * (double)(float)full + (1.0 - w) * (double)(float)none));
	}

	private static VertexFormats BuildVertexBuffer(Mesh m, ref object vertices, ref bool computenormals)
	{
		VertexFormats result;
		if (m.Vertices.Count == m.Normals.Count && m.Vertices.Count == m.TextureCoordinates.Count)
		{
			CustomVertex.PositionNormalTextured[] array = (CustomVertex.PositionNormalTextured[])(vertices = new CustomVertex.PositionNormalTextured[m.Vertices.Count]);
			result = VertexFormats.PositionNormal | VertexFormats.Texture1;
			computenormals = false;
			for (int i = 0; i < m.Vertices.Count; i++)
			{
				array[i] = new CustomVertex.PositionNormalTextured(Converter.ToDx(m.Vertices[i]), Converter.ToDx(m.Normals[i]), (float)m.TextureCoordinates[i].X, (float)(0.0 - m.TextureCoordinates[i].Y));
			}
		}
		else if (m.Vertices.Count == m.Normals.Count && m.Vertices.Count == m.Colors.Count)
		{
			CustomVertex.PositionNormalColored[] array2 = (CustomVertex.PositionNormalColored[])(vertices = new CustomVertex.PositionNormalColored[m.Vertices.Count]);
			result = VertexFormats.PositionNormal | VertexFormats.Diffuse;
			computenormals = false;
			for (int j = 0; j < m.Vertices.Count; j++)
			{
				Vector3 pos = Converter.ToDx(m.Vertices[j]);
				Vector3 nor = Converter.ToDx(m.Normals[j]);
				array2[j] = new CustomVertex.PositionNormalColored(pos, nor, Helpers.ToColor(m.Colors[j]).ToArgb());
			}
		}
		else if (m.Vertices.Count == m.Normals.Count)
		{
			CustomVertex.PositionNormal[] array3 = (CustomVertex.PositionNormal[])(vertices = new CustomVertex.PositionNormal[m.Vertices.Count]);
			result = VertexFormats.PositionNormal;
			computenormals = false;
			for (int k = 0; k < m.Vertices.Count; k++)
			{
				array3[k] = new CustomVertex.PositionNormal(Converter.ToDx(m.Vertices[k]), Converter.ToDx(m.Normals[k]));
			}
		}
		else if (m.Vertices.Count == m.TextureCoordinates.Count)
		{
			CustomVertex.PositionNormalTextured[] array4 = (CustomVertex.PositionNormalTextured[])(vertices = new CustomVertex.PositionNormalTextured[m.Vertices.Count]);
			result = VertexFormats.PositionNormal | VertexFormats.Texture1;
			for (int l = 0; l < m.Vertices.Count; l++)
			{
				array4[l] = new CustomVertex.PositionNormalTextured(Converter.ToDx(m.Vertices[l]), Converter.ToDx(Vector3.Zero), (float)m.TextureCoordinates[l].X, (float)(0.0 - m.TextureCoordinates[l].Y));
			}
		}
		else if (m.Vertices.Count != m.Colors.Count)
		{
			CustomVertex.PositionNormal[] array5 = (CustomVertex.PositionNormal[])(vertices = new CustomVertex.PositionNormal[m.Vertices.Count]);
			result = VertexFormats.PositionNormal;
			for (int n = 0; n < m.Vertices.Count; n++)
			{
				array5[n] = new CustomVertex.PositionNormal(Converter.ToDx(m.Vertices[n]), Converter.ToDx(Vector3.Zero));
			}
		}
		else
		{
			CustomVertex.PositionColored[] array6 = (CustomVertex.PositionColored[])(vertices = new CustomVertex.PositionColored[m.Vertices.Count]);
			result = VertexFormats.Diffuse | VertexFormats.Position;
			computenormals = false;
			for (int num = 0; num < m.Vertices.Count; num++)
			{
				Vector3 value = Converter.ToDx(m.Vertices[num]);
				array6[num] = new CustomVertex.PositionColored(value, Helpers.ToColor(m.Colors[num]).ToArgb());
			}
		}
		return result;
	}

	private int Clamp(int i)
	{
		if (i < 0)
		{
			i = 0;
		}
		if (i > 255)
		{
			i = 255;
		}
		return i;
	}

	public object Convert()
	{
		return ConvertToDx();
	}

	public MeshList ConvertToDx(JointCollectionBase joints)
	{
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Expected O, but got Unknown
		//IL_00a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00aa: Expected O, but got Unknown
		scn.ClearTags();
		Scene val = new Scene();
		val.DefaultMaterial.Diffuse = Color.Black;
		val.DefaultMaterial.Ambient = Color.Black;
		val.DefaultMaterial.Specular = Color.FromArgb(32, 32, 32);
		val.DefaultMaterial.SpecularPower = 300.0;
		val.DefaultMaterial.Mode = (TextureModes)0;
		MeshList meshList = new MeshList();
		AddJointMeshs(joints, meshList, scn.RootJoint);
		if (((NodeCollectionBase)joints).Count == 0)
		{
			return meshList;
		}
		foreach (Mesh item in (Node)scn.SceneRoot)
		{
			Mesh val2 = item;
			Mesh dst = val.CreateMesh(((Node)val2).Name);
			for (int i = 0; i < val2.FaceIndices.Count; i++)
			{
				CopyElement(joints, val2, dst, i);
			}
		}
		scn.ClearTags();
		SceneToMesh sceneToMesh = null;
		sceneToMesh = ((dxp == null) ? new SceneToMesh(val, dev, Scale) : new SceneToMesh(val, dxp));
		meshList.AddRange(sceneToMesh.ConvertToDx());
		val.Dispose();
		return meshList;
	}

	public MeshList ConvertToDx(Joint j)
	{
		return ConvertToDx(j, GetJointColor(j));
	}

	public MeshList ConvertToDx(Joint j, Color maxcl)
	{
		return ConvertToDx(j, Color.FromArgb(0, maxcl), maxcl);
	}

	public MeshList ConvertToDx(Joint j, Color mincl, Color maxcl)
	{
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Expected O, but got Unknown
		//IL_0067: Unknown result type (might be due to invalid IL or missing references)
		//IL_006c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0074: Expected O, but got Unknown
		//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b8: Expected O, but got Unknown
		//IL_00d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d9: Expected O, but got Unknown
		scn.ClearTags();
		Scene val = new Scene();
		val.DefaultMaterial.Diffuse = Color.Transparent;
		val.DefaultMaterial.Ambient = Color.Transparent;
		val.DefaultMaterial.Specular = Color.Transparent;
		val.DefaultMaterial.SpecularPower = 100.0;
		val.DefaultMaterial.Mode = (TextureModes)0;
		MeshList meshList = new MeshList();
		JointCollection val2 = new JointCollection();
		val2.Add(j);
		JointCollection val3 = val2;
		AddJointMeshs((JointCollectionBase)(object)val3, meshList, scn.RootJoint);
		((NodeCollectionBase)val3).Clear();
		((NodeCollectionBase)val3).Dispose();
		foreach (Mesh item in (Node)scn.SceneRoot)
		{
			Mesh val4 = item;
			Envelope val5 = null;
			foreach (Envelope envelope in val4.Envelopes)
			{
				Envelope val6 = envelope;
				if (val6.Joint == j)
				{
					val5 = val6;
					break;
				}
			}
			if (val5 == null)
			{
				continue;
			}
			Mesh dst = val.CreateMesh(((Node)val4).Name);
			for (int i = 0; i < val4.FaceIndices.Count; i++)
			{
				if (HasWeight(val4, i, val5))
				{
					CopyElement(val4, dst, i, mincl, maxcl, val5);
				}
			}
		}
		scn.ClearTags();
		SceneToMesh sceneToMesh = null;
		sceneToMesh = ((dxp == null) ? new SceneToMesh(val, dev, Scale) : new SceneToMesh(val, dxp));
		meshList.AddRange(sceneToMesh.ConvertToDx());
		val.Dispose();
		return meshList;
	}

	public MeshList ConvertToDx()
	{
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Expected O, but got Unknown
		scn.ClearTags();
		MeshList meshList = new MeshList();
		AddJointMeshs(null, meshList, scn.RootJoint);
		foreach (Mesh item in (Node)scn.SceneRoot)
		{
			Mesh m = item;
			AddMesh(meshList, m);
		}
		scn.ClearTags();
		return meshList;
	}

	private void CopyElement(JointCollectionBase joints, Mesh src, Mesh dst, int findex)
	{
		//IL_0003: Unknown result type (might be due to invalid IL or missing references)
		//IL_0009: Expected O, but got Unknown
		//IL_009b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a2: Expected O, but got Unknown
		Vector3i val = new Vector3i(0, 0, 0);
		for (int i = 0; i < 3; i++)
		{
			int num = src.FaceIndices[findex][i];
			val[i] = dst.Vertices.Count;
			dst.Vertices.Add(src.Vertices[num]);
			if (src.Normals.Count > 0)
			{
				dst.Normals.Add(src.Normals[num]);
			}
			Color color = Color.FromArgb(255, Color.Black);
			foreach (Envelope envelope in src.Envelopes)
			{
				Envelope val2 = envelope;
				if (((NodeCollectionBase)joints).Contains((Node)(object)val2.Joint))
				{
					double w = val2.Weights[num];
					Color color2 = Blend(w, Color.Black, GetJointColor(val2.Joint));
					color = Color.FromArgb(Clamp(color.A + color2.A), Clamp(color.R + color2.R), Clamp(color.G + color2.G), Clamp(color.B + color2.B));
				}
			}
			dst.Colors.Add(Helpers.ToVector4(color));
		}
		dst.FaceIndices.Add(val);
	}

	private void CopyElement(Mesh src, Mesh dst, int findex, Color mincl, Color maxcl, Envelope e)
	{
		//IL_0003: Unknown result type (might be due to invalid IL or missing references)
		//IL_0009: Expected O, but got Unknown
		Vector3i val = new Vector3i(0, 0, 0);
		for (int i = 0; i < 3; i++)
		{
			int num = src.FaceIndices[findex][i];
			val[i] = dst.Vertices.Count;
			dst.Vertices.Add(src.Vertices[num]);
			if (src.Normals.Count > 0)
			{
				dst.Normals.Add(src.Normals[num]);
			}
			if (src.Colors.Count <= 0 || e != null)
			{
				double w = e.Weights[num];
				Color color = Blend(w, mincl, maxcl);
				dst.Colors.Add(Helpers.ToVector4(color));
			}
			else
			{
				dst.Colors.Add(src.Colors[num]);
			}
		}
		dst.FaceIndices.Add(val);
	}

	public static MeshBox CreateDxMesh(Device dev, Mesh m, bool isbb)
	{
		//IL_010b: Unknown result type (might be due to invalid IL or missing references)
		short[] data = m.FaceIndices.ToArrayOfShort();
		object vertices = null;
		bool computenormals = true;
		VertexFormats vertexFormat = BuildVertexBuffer(m, ref vertices, ref computenormals);
		if (vertices == null || m.Vertices.Count <= 0 || m.FaceIndices.Count <= 0)
		{
			return null;
		}
		Mesh mesh = new Mesh(m.FaceIndices.Count, m.Vertices.Count, (MeshFlags)0, vertexFormat, dev);
		mesh.SetVertexBufferData(vertices, LockFlags.None);
		mesh.SetIndexBufferData(data, LockFlags.None);
		int[] array = new int[mesh.NumberFaces * 3];
		mesh.GenerateAdjacency(0.01f, array);
		mesh.OptimizeInPlace(MeshFlags.OptimizeVertexCache, array);
		if (computenormals)
		{
			mesh.ComputeNormals(array);
		}
		MeshBox meshBox = new MeshBox(mesh, 1, LoadMaterial(m))
		{
			Wire = false
		};
		if (m.Material.Texture.TextureImage == null)
		{
			m.Material.Texture.ImportTextureImage();
		}
		meshBox.SetTexture(m.Material.Texture.TextureImage);
		meshBox.Transform = Converter.ToDx((Transformation)(object)m);
		meshBox.TextureMode = m.Material.Mode;
		if (isbb)
		{
			meshBox.CullMode = MeshBox.Cull.None;
			meshBox.Material = DirectXPanel.GetMaterial(Color.Black);
			meshBox.Wire = true;
			meshBox.IgnoreDuringCameraReset = true;
		}
		return meshBox;
	}

	public void Dispose()
	{
		dev = null;
		dxp = null;
		if (colormap != null)
		{
			colormap.Clear();
		}
		colormap = null;
		scn = null;
	}

	public Color GetJointColor(Joint j)
	{
		if (j == null)
		{
			return Color.Black;
		}
		if (colormap == null)
		{
			colormap = new Hashtable();
		}
		object obj = colormap[((Node)j).Name];
		if (obj == null)
		{
			obj = GetRandomColor();
			colormap[((Node)j).Name] = obj;
		}
		return (Color)obj;
	}

	public Color GetRandomColor()
	{
		if (index < Colors.Length)
		{
			Color[] colors = Colors;
			int num = index;
			int num2 = num;
			index = num + 1;
			return colors[num2];
		}
		return Color.FromArgb(rnd.Next(190) + 30, rnd.Next(190) + 30, rnd.Next(190) + 30);
	}

	private bool HasWeight(Mesh src, int findex, Envelope e)
	{
		for (int i = 0; i < 3; i++)
		{
			int num = src.FaceIndices[findex][i];
			if (e.Weights[num] != 0.0)
			{
				return true;
			}
		}
		return false;
	}

	private static Material LoadMaterial(Mesh m)
	{
		Material material = new Material();
		m.Material.Tag = material;
		material.Diffuse = m.Material.Diffuse;
		material.Specular = m.Material.Specular;
		material.SpecularSharpness = (float)m.Material.SpecularPower;
		material.Emissive = m.Material.Emmissive;
		material.Ambient = m.Material.Ambient;
		return material;
	}
}
