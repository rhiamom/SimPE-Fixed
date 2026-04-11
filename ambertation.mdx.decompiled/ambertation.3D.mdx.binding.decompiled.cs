using System;
using System.Collections;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Ambertation.Drawing;
using Ambertation.Geometry;
using Ambertation.Graphics;
using Ambertation.Scenes;
using Ambertation.Scenes.Collections;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;

[assembly: AssemblyCompany("Ambertation")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCopyright("Copyright © Ambertation 2006")]
[assembly: AssemblyDescription("A dotXSI Library")]
[assembly: AssemblyFileVersion("1.0.0.0")]
[assembly: AssemblyProduct("dotXSI Library")]
[assembly: AssemblyTitle("dotXSI Library")]
[assembly: AssemblyTrademark("")]
[assembly: CompilationRelaxations(8)]
[assembly: ComVisible(false)]
[assembly: Debuggable(DebuggableAttribute.DebuggingModes.IgnoreSymbolStoreSequencePoints)]
[assembly: Guid("441fdf61-d639-47d6-b6ea-f9b165277f23")]
[assembly: RuntimeCompatibility(WrapNonExceptionThrows = true)]
[assembly: AssemblyVersion("1.0.0.0")]
namespace Ambertation.Scenes
{
	public class Converter
	{
		public static Matrix FromDx(Microsoft.DirectX.Matrix m)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			//IL_0034: Unknown result type (might be due to invalid IL or missing references)
			//IL_0043: Unknown result type (might be due to invalid IL or missing references)
			//IL_0052: Unknown result type (might be due to invalid IL or missing references)
			//IL_0061: Unknown result type (might be due to invalid IL or missing references)
			//IL_0070: Unknown result type (might be due to invalid IL or missing references)
			//IL_007f: Unknown result type (might be due to invalid IL or missing references)
			//IL_008e: Unknown result type (might be due to invalid IL or missing references)
			//IL_009d: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bb: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ca: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f8: Expected O, but got Unknown
			return new Matrix(4, 4)
			{
				[0, 0] = m.M11,
				[0, 1] = m.M21,
				[0, 2] = m.M31,
				[0, 3] = m.M41,
				[1, 0] = m.M12,
				[1, 1] = m.M22,
				[1, 2] = m.M32,
				[1, 3] = m.M42,
				[2, 0] = m.M13,
				[2, 1] = m.M23,
				[2, 2] = m.M33,
				[2, 3] = m.M43,
				[3, 0] = m.M14,
				[3, 1] = m.M24,
				[3, 2] = m.M34,
				[3, 3] = m.M44
			};
		}

		public static Vector2 ToDx(Vector2 v)
		{
			return new Vector2((float)v.X, (float)v.Y);
		}

		public static Vector3 ToDx(Vector3 v)
		{
			return new Vector3((float)((Vector2)v).X, (float)((Vector2)v).Y, (float)v.Z);
		}

		public static Vector4 ToDx(Vector4 v)
		{
			return new Vector4((float)((Vector2)v).X, (float)((Vector2)v).Y, (float)((Vector3)v).Z, (float)v.W);
		}

		public static Microsoft.DirectX.Matrix ToDx(Transformation t)
		{
			return Microsoft.DirectX.Matrix.Multiply(Microsoft.DirectX.Matrix.Scaling(ToDx(t.Scaling)), Microsoft.DirectX.Matrix.Multiply(Microsoft.DirectX.Matrix.RotationX((float)((Vector2)t.Rotation).X), Microsoft.DirectX.Matrix.Multiply(Microsoft.DirectX.Matrix.RotationY((float)((Vector2)t.Rotation).Y), Microsoft.DirectX.Matrix.Multiply(Microsoft.DirectX.Matrix.RotationZ((float)t.Rotation.Z), Microsoft.DirectX.Matrix.Translation(ToDx(t.Translation))))));
		}

		public static Microsoft.DirectX.Matrix ToDx(Matrix t)
		{
			Microsoft.DirectX.Matrix result = new Microsoft.DirectX.Matrix();
			result.M11 = (float)t[0, 0];
			result.M21 = (float)t[0, 1];
			result.M31 = (float)t[0, 2];
			result.M41 = (float)t[0, 3];
			result.M12 = (float)t[1, 0];
			result.M22 = (float)t[1, 1];
			result.M32 = (float)t[1, 2];
			result.M42 = (float)t[1, 3];
			result.M13 = (float)t[2, 0];
			result.M23 = (float)t[2, 1];
			result.M33 = (float)t[2, 2];
			result.M43 = (float)t[2, 3];
			result.M14 = (float)t[3, 0];
			result.M24 = (float)t[3, 1];
			result.M34 = (float)t[3, 2];
			result.M44 = (float)t[3, 3];
			return result;
		}
	}
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
			Microsoft.DirectX.Matrix transform = Converter.ToDx((Transformation)(object)joint);
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
}
namespace Ambertation.Graphics
{
	public class PrepareEffectEventArgs : EventArgs
	{
		private MeshBox mb;

		public MeshBox MeshBox => mb;

		internal PrepareEffectEventArgs(MeshBox mb)
		{
			this.mb = mb;
		}
	}
	public class RenderSelection : UserControl
	{
		private IContainer components;

		private ListBox lb;

		private Scene scn;

		private SceneToMesh stm;

		private DirectXPanel dx;

		public DirectXPanel DirectXPanel
		{
			get
			{
				return dx;
			}
			set
			{
				if (dx != null)
				{
					dx.ResetDevice -= dx_ResetDevice;
				}
				dx = value;
				if (dx != null)
				{
					dx.ResetDevice += dx_ResetDevice;
				}
				SetContent();
			}
		}

		public Scene Scene
		{
			get
			{
				return scn;
			}
			set
			{
				scn = value;
				SetContent();
			}
		}

		public RenderSelection()
		{
			InitializeComponent();
			lb.DrawMode = DrawMode.OwnerDrawVariable;
			lb.DrawItem += DrawItemHandler;
			lb.MeasureItem += MeasureItemHandler;
		}

		protected override void Dispose(bool disposing)
		{
			dx = null;
			scn = null;
			if (stm != null)
			{
				stm.Dispose();
			}
			stm = null;
			if (disposing && components != null)
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		private void DrawItemHandler(object sender, DrawItemEventArgs e)
		{
			e.DrawBackground();
			if (e.Index < lb.Items.Count && e.Index >= 0)
			{
				object obj = lb.Items[e.Index];
				Color foreColor = lb.ForeColor;
				if (!(obj is Joint))
				{
					System.Drawing.Graphics graphics = e.Graphics;
					string s = obj.ToString();
					System.Drawing.Font font = lb.Font;
					SolidBrush brush = new SolidBrush(foreColor);
					Rectangle bounds = e.Bounds;
					Rectangle bounds2 = e.Bounds;
					Rectangle bounds3 = e.Bounds;
					Rectangle bounds4 = e.Bounds;
					graphics.DrawString(s, font, brush, new Rectangle(bounds.Left + 3, bounds2.Top + 4, bounds3.Width - 6, bounds4.Height - 4));
					return;
				}
				e.Graphics.CompositingQuality = CompositingQuality.HighQuality;
				e.Graphics.SmoothingMode = SmoothingMode.HighQuality;
				e.Graphics.InterpolationMode = InterpolationMode.High;
				foreColor = stm.GetJointColor((Joint)((obj is Joint) ? obj : null));
				System.Drawing.Graphics graphics2 = e.Graphics;
				SolidBrush solidBrush = new SolidBrush(foreColor);
				Rectangle bounds5 = e.Bounds;
				Rectangle bounds6 = e.Bounds;
				Rectangle bounds7 = e.Bounds;
				Rectangle bounds8 = e.Bounds;
				Rectangle rectangle = new Rectangle(bounds5.Left + 3, bounds6.Top + 2, bounds7.Width - 6, bounds8.Height - 5);
				Routines.FillRoundRect(graphics2, (Brush)solidBrush, rectangle, e.Bounds.Height / 3);
				System.Drawing.Graphics graphics3 = e.Graphics;
				SolidBrush brush2 = new SolidBrush(foreColor);
				Rectangle bounds9 = e.Bounds;
				int top = e.Bounds.Top;
				Rectangle bounds10 = e.Bounds;
				Rectangle bounds11 = e.Bounds;
				graphics3.FillEllipse(brush2, new Rectangle(bounds9.Left + 1, top, bounds10.Height + 4, bounds11.Height - 1));
				System.Drawing.Graphics graphics4 = e.Graphics;
				string s2 = obj.ToString();
				System.Drawing.Font font2 = new System.Drawing.Font(lb.Font.FontFamily, lb.Font.Size, FontStyle.Bold, lb.Font.Unit);
				SolidBrush brush3 = new SolidBrush(Color.Black);
				int left = e.Bounds.Left;
				Rectangle bounds12 = e.Bounds;
				Rectangle bounds13 = e.Bounds;
				int num = e.Bounds.Width;
				Rectangle bounds14 = e.Bounds;
				Rectangle bounds15 = e.Bounds;
				graphics4.DrawString(s2, font2, brush3, new Rectangle(left + bounds12.Height + 4, bounds13.Top + 3, num - bounds14.Height - 5, bounds15.Height - 4));
			}
		}

		private void dx_ResetDevice(object sender, EventArgs e)
		{
			//IL_0080: Unknown result type (might be due to invalid IL or missing references)
			//IL_0086: Expected O, but got Unknown
			if (!(sender is DirectXPanel directXPanel))
			{
				return;
			}
			try
			{
				directXPanel.Meshes.Clear(dispose: true);
				if (lb.SelectedItem == null)
				{
					directXPanel.Meshes.AddRange(stm.ConvertToDx());
				}
				else if (!(lb.SelectedItem is Joint))
				{
					directXPanel.Meshes.AddRange(stm.ConvertToDx());
				}
				else if (lb.SelectedItems.Count != 1)
				{
					JointCollection val = new JointCollection();
					foreach (object selectedItem2 in lb.SelectedItems)
					{
						if (selectedItem2 is Joint)
						{
							val.Add((Joint)((selectedItem2 is Joint) ? selectedItem2 : null));
						}
					}
					directXPanel.Meshes.AddRange(stm.ConvertToDx((JointCollectionBase)(object)val));
				}
				else
				{
					MeshList meshes = directXPanel.Meshes;
					SceneToMesh sceneToMesh = stm;
					object selectedItem = lb.SelectedItem;
					meshes.AddRange(sceneToMesh.ConvertToDx((Joint)((selectedItem is Joint) ? selectedItem : null)));
				}
				_ = stm;
			}
			catch
			{
			}
		}

		private void InitializeComponent()
		{
			this.lb = new System.Windows.Forms.ListBox();
			base.SuspendLayout();
			this.lb.Dock = System.Windows.Forms.DockStyle.Fill;
			this.lb.IntegralHeight = false;
			this.lb.Location = new System.Drawing.Point(0, 0);
			this.lb.Name = "lb";
			this.lb.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
			this.lb.Size = new System.Drawing.Size(172, 329);
			this.lb.TabIndex = 0;
			this.lb.SelectedIndexChanged += new System.EventHandler(lb_SelectedIndexChanged);
			base.Controls.Add(this.lb);
			base.Name = "RenderSelection";
			base.Size = new System.Drawing.Size(172, 329);
			base.ResumeLayout(false);
		}

		private void lb_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (dx != null)
			{
				dx.Reset();
			}
		}

		private void MeasureItemHandler(object sender, MeasureItemEventArgs e)
		{
			e.ItemHeight += 8;
		}

		private void SetContent()
		{
			//IL_0089: Unknown result type (might be due to invalid IL or missing references)
			//IL_008f: Expected O, but got Unknown
			lb.Items.Clear();
			stm = null;
			if (scn == null || dx == null)
			{
				return;
			}
			stm = new SceneToMesh(scn, dx);
			dx.Reset();
			dx.ResetDefaultViewport();
			lb.Items.Add("--- [Display Mesh] ---");
			foreach (Joint item2 in scn.JointCollection)
			{
				Joint item = item2;
				lb.Items.Add(item);
			}
		}
	}
	public delegate void PrepareEffectEventHandler(object sender, object e);
	public class MeshList : IEnumerable, IDisposable
	{
		private ArrayList list = new ArrayList();

		public int Count => list.Count;

		public MeshBox this[int index]
		{
			get
			{
				return (MeshBox)list[index];
			}
			set
			{
				OnRemove((MeshBox)list[index]);
				list[index] = value;
				OnAdd(value);
			}
		}

		public MeshList()
		{
			list = new ArrayList();
		}

		public void Add(MeshBox m)
		{
			OnAdd(m);
			list.Add(m);
		}

		public void AddRange(MeshBox[] m)
		{
			for (int i = 0; i < m.Length; i++)
			{
				Add(m[i]);
			}
		}

		public void AddRange(MeshList m)
		{
			foreach (MeshBox item in (IEnumerable)m)
			{
				Add(item);
			}
		}

		public void Clear()
		{
			Clear(dispose: false);
		}

		public void Clear(bool dispose)
		{
			if (dispose)
			{
				for (int i = 0; i < list.Count; i++)
				{
					this[i].Dispose();
				}
			}
			list.Clear();
		}

		public bool Contains(MeshBox m)
		{
			return list.Contains(m);
		}

		public virtual void Dispose()
		{
			if (list != null)
			{
				Clear(dispose: true);
			}
		}

		public void Insert(int index, MeshBox m)
		{
			OnAdd(m);
			list.Insert(index, m);
		}

		protected virtual void OnAdd(MeshBox m)
		{
		}

		protected virtual void OnRemove(MeshBox m)
		{
		}

		public void Remove(MeshBox m)
		{
			OnRemove(m);
			list.Remove(m);
		}

		public void RemoveAt(int index)
		{
			try
			{
				Remove(this[index]);
			}
			catch
			{
			}
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return list.GetEnumerator();
		}
	}
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

		protected ShadeMode smode;

		protected FillModes fm;

		protected float jsz;

		private bool fstate;

		private bool fattr;

		private bool eventpause;

		[Category("Settings")]
		public bool AddAxis
		{
			get
			{
				return autoaxismesh;
			}
			set
			{
				if (autoaxismesh != value)
				{
					autoaxismesh = value;
					FireStateChangeEvent();
				}
			}
		}

		[Browsable(false)]
		[Category("Settings")]
		public bool AllowSettingsDialog
		{
			get
			{
				return allowscr;
			}
			set
			{
				if (allowscr != value)
				{
					allowscr = value;
					FireStateChangeEvent();
				}
			}
		}

		[Category("Settings")]
		public bool EnableLights
		{
			get
			{
				return uselight;
			}
			set
			{
				if (uselight != value)
				{
					uselight = value;
					FireStateChangeEvent();
				}
			}
		}

		[Category("Settings")]
		public bool EnableSpecularHighlights
		{
			get
			{
				return usespec;
			}
			set
			{
				if (usespec != value)
				{
					usespec = value;
					FireStateChangeEvent();
				}
			}
		}

		[Category("Settings")]
		public bool EnableTextures
		{
			get
			{
				return txtr;
			}
			set
			{
				if (txtr != value)
				{
					txtr = value;
					FireStateChangeEvent();
				}
			}
		}

		[Category("Settings")]
		public FillModes FillMode
		{
			get
			{
				return fm;
			}
			set
			{
				if (fm != value)
				{
					fm = value;
					FireStateChangeEvent();
				}
			}
		}

		[Category("Settings")]
		public float JointScale
		{
			get
			{
				return jsz;
			}
			set
			{
				if (jsz != value)
				{
					jsz = value;
					FireStateChangeEvent();
				}
			}
		}

		[Category("Settings")]
		public bool RenderBoundingBoxes
		{
			get
			{
				return bb;
			}
			set
			{
				if (bb != value)
				{
					bb = value;
					FireStateChangeEvent();
				}
			}
		}

		[Category("Settings")]
		public bool RenderJoints
		{
			get
			{
				return joints;
			}
			set
			{
				if (joints != value)
				{
					joints = value;
					FireStateChangeEvent();
				}
			}
		}

		[Category("Settings")]
		public ShadeMode ShadeMode
		{
			get
			{
				return smode;
			}
			set
			{
				if (smode != value)
				{
					smode = value;
					FireStateChangeEvent();
				}
			}
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
			smode = ShadeMode.Phong;
			autoaxismesh = true;
			jsz = 10f;
			bb = false;
			eventpause = false;
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
			{
				FireStateChangeEvent();
			}
			else if (fireattr)
			{
				FireAttributeChangeEvent();
			}
			else if (firestat)
			{
				FireStateChangeEvent();
			}
		}

		protected void FireAttributeChangeEvent()
		{
			if (eventpause)
			{
				fattr = true;
			}
			else if (this.ChangedAttribute != null)
			{
				this.ChangedAttribute(this, new EventArgs());
			}
		}

		protected void FireStateChangeEvent()
		{
			if (eventpause)
			{
				fstate = true;
			}
			else if (this.ChangedState != null)
			{
				this.ChangedState(this, new EventArgs());
			}
		}
	}
	[TypeConverter(typeof(ExpandableObjectConverter))]
	public class ViewportSetting : ViewportSettingBasic
	{
		protected DirectXPanel parent;

		protected float angx;

		protected float angy;

		protected float angz;

		protected float rad;

		protected float camoffset;

		protected Vector3 campos;

		protected Vector3 camtarget;

		protected Vector3 pos;

		protected Vector3 center;

		protected float fov;

		protected float aspect;

		protected float near;

		protected float far;

		protected Microsoft.DirectX.Matrix rotbase;

		protected bool alphablend;

		protected bool paused;

		protected bool useleft;

		protected bool useeff;

		protected bool autolightmesh;

		protected Cull acull;

		protected Cull mcull;

		protected float ascale;

		protected float lscale;

		protected Color amb;

		protected Color lcol;

		protected Color lscol;

		protected Color bg;

		private string flname;

		[Category("Settings")]
		public bool AddLightIndicators
		{
			get
			{
				return autolightmesh;
			}
			set
			{
				if (autolightmesh != value)
				{
					autolightmesh = value;
					FireStateChangeEvent();
				}
			}
		}

		[Category("Culling")]
		public Cull AlphaPassCullMode
		{
			get
			{
				return acull;
			}
			set
			{
				if (acull != value)
				{
					acull = value;
					FireStateChangeEvent();
				}
			}
		}

		[Category("Settings")]
		public Color AmbientColor
		{
			get
			{
				return amb;
			}
			set
			{
				if (amb != value)
				{
					amb = value;
					FireStateChangeEvent();
				}
			}
		}

		internal Microsoft.DirectX.Matrix AngelRotation => Microsoft.DirectX.Matrix.Multiply(Microsoft.DirectX.Matrix.RotationY(AngelY), Microsoft.DirectX.Matrix.Multiply(Microsoft.DirectX.Matrix.RotationX(AngelX), Microsoft.DirectX.Matrix.RotationZ(AngelZ)));

		[Category("Viewpoint")]
		public float AngelX
		{
			get
			{
				return angx;
			}
			set
			{
				angx = value;
				FireAttributeChangeEvent();
			}
		}

		[Category("Viewpoint")]
		public float AngelY
		{
			get
			{
				return angy;
			}
			set
			{
				angy = value;
				FireAttributeChangeEvent();
			}
		}

		[Category("Viewpoint")]
		public float AngelZ
		{
			get
			{
				return angz;
			}
			set
			{
				angz = value;
				FireAttributeChangeEvent();
			}
		}

		[Category("Camera")]
		[ReadOnly(true)]
		public float Aspect
		{
			get
			{
				return aspect;
			}
			set
			{
				aspect = value;
				FireAttributeChangeEvent();
			}
		}

		[Category("Settings")]
		[ReadOnly(true)]
		public float AxisScale
		{
			get
			{
				return ascale;
			}
			set
			{
				if (ascale != value)
				{
					ascale = value;
					FireStateChangeEvent();
				}
			}
		}

		[Category("Settings")]
		public Color BackgroundColor
		{
			get
			{
				return bg;
			}
			set
			{
				if (bg != value)
				{
					bg = value;
					FireStateChangeEvent();
				}
			}
		}

		[Category("Camera")]
		public float BoundingSphereRadius
		{
			get
			{
				return Math.Max(0.01f, rad);
			}
			set
			{
				rad = value;
				lscale = rad * 0.002f;
				near = rad / 10f;
				far = near * 10000f;
				FireAttributeChangeEvent();
			}
		}

		[Browsable(false)]
		[Category("Camera")]
		public Vector3 CameraPosition
		{
			get
			{
				return campos;
			}
			set
			{
				campos = value;
				FireAttributeChangeEvent();
			}
		}

		[Browsable(false)]
		[Category("Camera")]
		public Vector3 CameraTarget
		{
			get
			{
				return camtarget;
			}
			set
			{
				camtarget = value;
				FireAttributeChangeEvent();
			}
		}

		[Category("Render state")]
		public bool EnableAlphaBlendPass
		{
			get
			{
				return alphablend;
			}
			set
			{
				if (alphablend != value)
				{
					alphablend = value;
					FireStateChangeEvent();
				}
			}
		}

		[Category("Render state")]
		public bool EnableShaderEffectPass
		{
			get
			{
				return useeff;
			}
			set
			{
				if (useeff != value)
				{
					useeff = value;
					FireStateChangeEvent();
				}
			}
		}

		[Category("Camera")]
		public float FarPlane
		{
			get
			{
				return far;
			}
			set
			{
				far = value;
				FireAttributeChangeEvent();
			}
		}

		[Category("Camera")]
		public float FoV
		{
			get
			{
				return fov;
			}
			set
			{
				fov = value;
				FireAttributeChangeEvent();
			}
		}

		[Category("Camera")]
		public float InitialCameraOffsetScale
		{
			get
			{
				return camoffset;
			}
			set
			{
				if (value == camoffset)
				{
					camoffset = value;
					FireStateChangeEvent();
				}
			}
		}

		[Browsable(false)]
		public float InputRotationScale => 100f;

		[Browsable(false)]
		public float InputScaleScale => 100f;

		[Browsable(false)]
		public float InputTranslationScale => 0.5f;

		[Category("Light")]
		public Color LightColorDiffuse
		{
			get
			{
				return lcol;
			}
			set
			{
				if (lcol != value)
				{
					lcol = value;
					FireStateChangeEvent();
				}
			}
		}

		[Category("Light")]
		public Color LightColorSpecular
		{
			get
			{
				return lscol;
			}
			set
			{
				if (lscol != value)
				{
					lscol = value;
					FireStateChangeEvent();
				}
			}
		}

		[Category("Settings")]
		[ReadOnly(true)]
		public float LineWidth
		{
			get
			{
				return lscale;
			}
			set
			{
				if (lscale != value)
				{
					lscale = value;
					FireStateChangeEvent();
				}
			}
		}

		[Category("Culling")]
		public Cull MeshPassCullMode
		{
			get
			{
				return mcull;
			}
			set
			{
				if (mcull != value)
				{
					mcull = value;
					FireStateChangeEvent();
				}
			}
		}

		[Category("Camera")]
		public float NearPlane
		{
			get
			{
				return near;
			}
			set
			{
				near = value;
				FireAttributeChangeEvent();
			}
		}

		[Browsable(false)]
		[Category("Camera")]
		public Vector3 ObjectCenter
		{
			get
			{
				return center;
			}
			set
			{
				center = value;
				FireAttributeChangeEvent();
			}
		}

		[Category("Render state")]
		public bool Paused
		{
			get
			{
				return paused;
			}
			set
			{
				if (paused != value)
				{
					paused = value;
				}
			}
		}

		[Browsable(false)]
		[Category("Camera")]
		public Vector3 RealCameraPosition => new Vector3(X, Y, Z) + CameraPosition;

		internal Microsoft.DirectX.Matrix Rotation
		{
			get
			{
				return rotbase;
			}
			set
			{
				rotbase = value;
				FireAttributeChangeEvent();
			}
		}

		[Category("Camera")]
		public bool SetDefaultCamera
		{
			get
			{
				return false;
			}
			set
			{
				if (value)
				{
					parent.ResetDefaultViewport();
					NearPlane = BoundingSphereRadius / 10f;
					FarPlane = NearPlane * 10000f;
				}
			}
		}

		[Category("Render state")]
		public bool UseLefthandedCoordinates
		{
			get
			{
				return useleft;
			}
			set
			{
				if (useleft != value)
				{
					useleft = value;
					FireStateChangeEvent();
				}
			}
		}

		[Category("Viewpoint")]
		public float X
		{
			get
			{
				return pos.X;
			}
			set
			{
				pos.X = value;
				FireAttributeChangeEvent();
			}
		}

		[Category("Viewpoint")]
		public float Y
		{
			get
			{
				return pos.Y;
			}
			set
			{
				pos.Y = value;
				FireAttributeChangeEvent();
			}
		}

		[Category("Viewpoint")]
		public float Z
		{
			get
			{
				return pos.Z;
			}
			set
			{
				pos.Z = value;
				FireAttributeChangeEvent();
			}
		}

		internal ViewportSetting(DirectXPanel parent)
			: base(parent)
		{
			flname = null;
			this.parent = parent;
			Reset();
			autolightmesh = false;
			useleft = false;
			useeff = false;
			alphablend = true;
			paused = false;
			acull = Cull.None;
			mcull = Cull.Clockwise;
			ascale = 250f;
			lscale = 0.1f;
			amb = Color.FromArgb(128, 128, 128);
			bg = SystemColors.AppWorkspace;
			lcol = (lscol = Color.White);
			camoffset = 1.2f;
		}

		private void DeSerialize(string flname)
		{
			Stream stream = File.OpenRead(flname);
			try
			{
				BeginUpdate();
				BinaryReader binaryReader = new BinaryReader(stream);
				int num = binaryReader.ReadInt32();
				base.EnableTextures = binaryReader.ReadBoolean();
				base.FillMode = (FillModes)binaryReader.ReadInt32();
				base.RenderJoints = binaryReader.ReadBoolean();
				base.EnableSpecularHighlights = binaryReader.ReadBoolean();
				base.EnableLights = binaryReader.ReadBoolean();
				base.ShadeMode = (ShadeMode)binaryReader.ReadInt32();
				base.AddAxis = binaryReader.ReadBoolean();
				base.JointScale = binaryReader.ReadSingle();
				if (num >= 2)
				{
					InitialCameraOffsetScale = binaryReader.ReadSingle();
				}
				if (num >= 3)
				{
					base.RenderBoundingBoxes = binaryReader.ReadBoolean();
				}
				if (num >= 4)
				{
					BackgroundColor = Color.FromArgb(binaryReader.ReadInt32());
				}
				EndUpdate();
			}
			finally
			{
				stream.Close();
			}
		}

		~ViewportSetting()
		{
			Save();
		}

		internal Microsoft.DirectX.Direct3D.FillMode GetFillMode(MeshBox box)
		{
			return GetFillMode(box, 0);
		}

		internal Microsoft.DirectX.Direct3D.FillMode GetFillMode(MeshBox box, int pass)
		{
			if (fm == FillModes.Default || box.SpecialMesh)
			{
				if (box.Wire)
				{
					return Microsoft.DirectX.Direct3D.FillMode.WireFrame;
				}
				return Microsoft.DirectX.Direct3D.FillMode.Solid;
			}
			if (fm == FillModes.WireframeOverlay)
			{
				if (pass == 1)
				{
					return Microsoft.DirectX.Direct3D.FillMode.Solid;
				}
				return Microsoft.DirectX.Direct3D.FillMode.WireFrame;
			}
			if (fm == FillModes.Point)
			{
				return Microsoft.DirectX.Direct3D.FillMode.Point;
			}
			if (fm == FillModes.Wireframe)
			{
				return Microsoft.DirectX.Direct3D.FillMode.WireFrame;
			}
			return Microsoft.DirectX.Direct3D.FillMode.Solid;
		}

		public void Load(string flname)
		{
			if (flname == null)
			{
				return;
			}
			this.flname = flname;
			try
			{
				DeSerialize(flname);
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
			}
		}

		public void Reset()
		{
			ResetAngle();
			pos = new Vector3(0f, 0f, 0f);
			center = new Vector3(0f, 0f, 0f);
			fov = (float)Math.PI / 4f;
			near = 1f;
			far = 100f;
			rad = 0.01f;
			campos = new Vector3(0f, 3f, 5f);
			camtarget = new Vector3(0f, 0f, 0f);
			rotbase = Microsoft.DirectX.Matrix.Identity;
			FireAttributeChangeEvent();
		}

		public void ResetAngle()
		{
			float num = 0f;
			angz = num;
			angx = (angy = num);
		}

		public void Save()
		{
			if (flname == null)
			{
				return;
			}
			try
			{
				Serialize(flname);
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
			}
		}

		private void Serialize(string flname)
		{
			Stream stream = File.Create(flname);
			try
			{
				BinaryWriter binaryWriter = new BinaryWriter(stream);
				binaryWriter.Write(4);
				binaryWriter.Write(base.EnableTextures);
				binaryWriter.Write((int)base.FillMode);
				binaryWriter.Write(base.RenderJoints);
				binaryWriter.Write(base.EnableSpecularHighlights);
				binaryWriter.Write(base.EnableLights);
				binaryWriter.Write((int)base.ShadeMode);
				binaryWriter.Write(base.AddAxis);
				binaryWriter.Write(base.JointScale);
				binaryWriter.Write(InitialCameraOffsetScale);
				binaryWriter.Write(base.RenderBoundingBoxes);
				binaryWriter.Write(bg.ToArgb());
			}
			finally
			{
				stream.Close();
			}
		}
	}
	public class MeshBox : MeshList, IDisposable
	{
		public enum Cull
		{
			Default,
			None,
			Clockwise,
			CounterClockwise
		}

		private MeshBox parent;

		private Mesh mesh;

		private Material mat;

		private Microsoft.DirectX.Matrix trans;

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

		private TextureModes blend;

		private Texture txtr;

		private Device txtrdev;

		private MeshBox txtrmb;

		private Microsoft.DirectX.Matrix wrld;

		private double dist;

		public bool Billboard
		{
			get
			{
				return billboard;
			}
			set
			{
				billboard = value;
			}
		}

		public Cull CullMode
		{
			get
			{
				return cull;
			}
			set
			{
				cull = value;
			}
		}

		internal double Distance => dist;

		public bool IgnoreDuringCameraReset
		{
			get
			{
				return ignoreforcam;
			}
			set
			{
				ignoreforcam = value;
			}
		}

		public bool JointMesh
		{
			get
			{
				return isjointmesh;
			}
			set
			{
				isjointmesh = value;
			}
		}

		public Material Material
		{
			get
			{
				return mat;
			}
			set
			{
				mat = value;
			}
		}

		public Mesh Mesh => mesh;

		public bool Opaque
		{
			get
			{
				//IL_0001: Unknown result type (might be due to invalid IL or missing references)
				//IL_0007: Invalid comparison between Unknown and I4
				if ((int)TextureMode == 3)
				{
					return false;
				}
				if (mat.Diffuse.A == byte.MaxValue)
				{
					return true;
				}
				return mat.Diffuse.A == 0;
			}
			set
			{
				opaque = value;
			}
		}

		public MeshBox Parent => parent;

		public bool Sort
		{
			get
			{
				return sort;
			}
			set
			{
				sort = value;
			}
		}

		public bool SpecialMesh
		{
			get
			{
				if (JointMesh)
				{
					return true;
				}
				return IgnoreDuringCameraReset;
			}
		}

		public int SubSetCount => ssc;

		public Texture Texture
		{
			get
			{
				if (txtrmb == null)
				{
					return txtr;
				}
				return txtrmb.Texture;
			}
		}

		public TextureModes TextureMode
		{
			get
			{
				//IL_0001: Unknown result type (might be due to invalid IL or missing references)
				return blend;
			}
			set
			{
				//IL_0001: Unknown result type (might be due to invalid IL or missing references)
				//IL_0002: Unknown result type (might be due to invalid IL or missing references)
				blend = value;
			}
		}

		public Stream TextureStream => txtrstream;

		public Microsoft.DirectX.Matrix Transform
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

		public bool Wire
		{
			get
			{
				return wire;
			}
			set
			{
				wire = value;
			}
		}

		internal Microsoft.DirectX.Matrix World => wrld;

		public bool ZTest
		{
			get
			{
				return ztest;
			}
			set
			{
				ztest = value;
			}
		}

		public MeshBox(Mesh mesh, int subsetcount)
			: this(mesh, subsetcount, new Material(), Microsoft.DirectX.Matrix.Identity)
		{
		}

		public MeshBox(Mesh mesh)
			: this(mesh, new Material(), Microsoft.DirectX.Matrix.Identity)
		{
		}

		public MeshBox(Mesh mesh, Material mat)
			: this(mesh, mat, Microsoft.DirectX.Matrix.Identity)
		{
		}

		public MeshBox(Mesh mesh, int subsetcount, Material mat)
			: this(mesh, subsetcount, mat, Microsoft.DirectX.Matrix.Identity)
		{
		}

		public MeshBox(Mesh mesh, Material mat, Microsoft.DirectX.Matrix transform)
			: this(mesh, mesh.NumberAttributes, mat, transform)
		{
		}

		public MeshBox(Mesh mesh, int subsetcount, Material mat, Microsoft.DirectX.Matrix transform)
			: this(mesh, subsetcount, mat, transform, wire: true, opaque: true)
		{
		}

		public MeshBox(Mesh mesh, int subsetcount, Material mat, Microsoft.DirectX.Matrix transform, bool wire, bool opaque)
		{
			//IL_006d: Unknown result type (might be due to invalid IL or missing references)
			billboard = false;
			sort = false;
			ztest = true;
			this.mesh = mesh;
			this.mat = mat;
			trans = transform;
			ssc = subsetcount;
			this.wire = wire;
			this.opaque = opaque;
			cull = Cull.Default;
			txtrstream = null;
			ignoreforcam = false;
			parent = null;
			isjointmesh = false;
			blend = (TextureModes)0;
		}

		public static BoundingBox BoundingBoxFromMesh(Mesh mesh, Matrix m)
		{
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Expected O, but got Unknown
			//IL_003c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0042: Expected O, but got Unknown
			//IL_05e8: Unknown result type (might be due to invalid IL or missing references)
			//IL_05ee: Expected O, but got Unknown
			//IL_00c3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ca: Expected O, but got Unknown
			//IL_0204: Unknown result type (might be due to invalid IL or missing references)
			//IL_020b: Expected O, but got Unknown
			//IL_0345: Unknown result type (might be due to invalid IL or missing references)
			//IL_034c: Expected O, but got Unknown
			//IL_0486: Unknown result type (might be due to invalid IL or missing references)
			//IL_048d: Expected O, but got Unknown
			Vector3 val = new Vector3(double.MaxValue, double.MaxValue, double.MaxValue);
			Vector3 val2 = new Vector3(double.MinValue, double.MinValue, double.MinValue);
			if (mesh != null)
			{
				int[] ranks = new int[1] { mesh.NumberVertices };
				if (mesh.VertexBuffer.Description.VertexFormat == (VertexFormats.PositionNormal | VertexFormats.Texture1))
				{
					CustomVertex.PositionNormalTextured[] array = (CustomVertex.PositionNormalTextured[])mesh.LockVertexBuffer(typeof(CustomVertex.PositionNormalTextured), LockFlags.None, ranks);
					try
					{
						CustomVertex.PositionNormalTextured[] array2 = array;
						for (int i = 0; i < array2.Length; i++)
						{
							CustomVertex.PositionNormalTextured positionNormalTextured = array2[i];
							Vector3 val3 = new Vector3((double)positionNormalTextured.X, (double)positionNormalTextured.Y, (double)positionNormalTextured.Z);
							if (m != (Matrix)null)
							{
								val3 = m * val3;
							}
							if (((Vector2)val3).X < ((Vector2)val).X)
							{
								((Vector2)val).X = ((Vector2)val3).X;
							}
							if (((Vector2)val3).Y < ((Vector2)val).Y)
							{
								((Vector2)val).Y = ((Vector2)val3).Y;
							}
							if (val3.Z < val.Z)
							{
								val.Z = val3.Z;
							}
							if (((Vector2)val3).X > ((Vector2)val2).X)
							{
								((Vector2)val2).X = ((Vector2)val3).X;
							}
							if (((Vector2)val3).Y > ((Vector2)val2).Y)
							{
								((Vector2)val2).Y = ((Vector2)val3).Y;
							}
							if (val3.Z > val2.Z)
							{
								val2.Z = val3.Z;
							}
						}
					}
					finally
					{
						mesh.UnlockVertexBuffer();
					}
				}
				else if (mesh.VertexBuffer.Description.VertexFormat == VertexFormats.PositionNormal)
				{
					CustomVertex.PositionNormal[] array3 = (CustomVertex.PositionNormal[])mesh.LockVertexBuffer(typeof(CustomVertex.PositionNormal), LockFlags.None, ranks);
					try
					{
						CustomVertex.PositionNormal[] array4 = array3;
						for (int j = 0; j < array4.Length; j++)
						{
							CustomVertex.PositionNormal positionNormal = array4[j];
							Vector3 val4 = new Vector3((double)positionNormal.X, (double)positionNormal.Y, (double)positionNormal.Z);
							if (m != (Matrix)null)
							{
								val4 = m * val4;
							}
							if (((Vector2)val4).X < ((Vector2)val).X)
							{
								((Vector2)val).X = ((Vector2)val4).X;
							}
							if (((Vector2)val4).Y < ((Vector2)val).Y)
							{
								((Vector2)val).Y = ((Vector2)val4).Y;
							}
							if (val4.Z < val.Z)
							{
								val.Z = val4.Z;
							}
							if (((Vector2)val4).X > ((Vector2)val2).X)
							{
								((Vector2)val2).X = ((Vector2)val4).X;
							}
							if (((Vector2)val4).Y > ((Vector2)val2).Y)
							{
								((Vector2)val2).Y = ((Vector2)val4).Y;
							}
							if (val4.Z > val2.Z)
							{
								val2.Z = val4.Z;
							}
						}
					}
					finally
					{
						mesh.UnlockVertexBuffer();
					}
				}
				else if (mesh.VertexBuffer.Description.VertexFormat == (VertexFormats.PositionNormal | VertexFormats.Diffuse))
				{
					CustomVertex.PositionNormalColored[] array5 = (CustomVertex.PositionNormalColored[])mesh.LockVertexBuffer(typeof(CustomVertex.PositionNormalColored), LockFlags.None, ranks);
					try
					{
						CustomVertex.PositionNormalColored[] array6 = array5;
						for (int k = 0; k < array6.Length; k++)
						{
							CustomVertex.PositionNormalColored positionNormalColored = array6[k];
							Vector3 val5 = new Vector3((double)positionNormalColored.X, (double)positionNormalColored.Y, (double)positionNormalColored.Z);
							if (m != (Matrix)null)
							{
								val5 = m * val5;
							}
							if (((Vector2)val5).X < ((Vector2)val).X)
							{
								((Vector2)val).X = ((Vector2)val5).X;
							}
							if (((Vector2)val5).Y < ((Vector2)val).Y)
							{
								((Vector2)val).Y = ((Vector2)val5).Y;
							}
							if (val5.Z < val.Z)
							{
								val.Z = val5.Z;
							}
							if (((Vector2)val5).X > ((Vector2)val2).X)
							{
								((Vector2)val2).X = ((Vector2)val5).X;
							}
							if (((Vector2)val5).Y > ((Vector2)val2).Y)
							{
								((Vector2)val2).Y = ((Vector2)val5).Y;
							}
							if (val5.Z > val2.Z)
							{
								val2.Z = val5.Z;
							}
						}
					}
					finally
					{
						mesh.UnlockVertexBuffer();
					}
				}
				else if (mesh.VertexBuffer.Description.VertexFormat == (VertexFormats.Diffuse | VertexFormats.Position))
				{
					CustomVertex.PositionColored[] array7 = (CustomVertex.PositionColored[])mesh.LockVertexBuffer(typeof(CustomVertex.PositionColored), LockFlags.None, ranks);
					try
					{
						CustomVertex.PositionColored[] array8 = array7;
						for (int l = 0; l < array8.Length; l++)
						{
							CustomVertex.PositionColored positionColored = array8[l];
							Vector3 val6 = new Vector3((double)positionColored.X, (double)positionColored.Y, (double)positionColored.Z);
							if (m != (Matrix)null)
							{
								val6 = m * val6;
							}
							if (((Vector2)val6).X < ((Vector2)val).X)
							{
								((Vector2)val).X = ((Vector2)val6).X;
							}
							if (((Vector2)val6).Y < ((Vector2)val).Y)
							{
								((Vector2)val).Y = ((Vector2)val6).Y;
							}
							if (val6.Z < val.Z)
							{
								val.Z = val6.Z;
							}
							if (((Vector2)val6).X > ((Vector2)val2).X)
							{
								((Vector2)val2).X = ((Vector2)val6).X;
							}
							if (((Vector2)val6).Y > ((Vector2)val2).Y)
							{
								((Vector2)val2).Y = ((Vector2)val6).Y;
							}
							if (val6.Z > val2.Z)
							{
								val2.Z = val6.Z;
							}
						}
					}
					finally
					{
						mesh.UnlockVertexBuffer();
					}
				}
			}
			if (((Vector2)val).X > ((Vector2)val2).X)
			{
				((Vector2)val).X = 0.0;
				((Vector2)val2).X = 0.0;
			}
			if (((Vector2)val).Y > ((Vector2)val2).Y)
			{
				((Vector2)val).Y = 0.0;
				((Vector2)val2).Y = 0.0;
			}
			if (val.Z > val2.Z)
			{
				val.Z = 0.0;
				val2.Z = 0.0;
			}
			return new BoundingBox(val, val2);
		}

		public override void Dispose()
		{
			base.Dispose();
			txtrmb = null;
			txtrdev = null;
			parent = null;
			try
			{
				if (mesh != null)
				{
					mesh.Dispose();
				}
			}
			catch
			{
			}
			try
			{
				if (txtrstream != null && txtrstream.CanRead)
				{
					txtrstream.Close();
				}
			}
			catch
			{
			}
			if (txtr != null)
			{
				txtr.Dispose();
			}
			txtr = null;
			txtrstream = null;
			mesh = null;
		}

		public BoundingBox GetBoundingBox(bool rec, bool all)
		{
			return GetBoundingBox(Converter.FromDx(trans), rec, all);
		}

		public BoundingBox GetBoundingBox(Matrix basem, bool rec, bool all)
		{
			//IL_002e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0038: Expected O, but got Unknown
			//IL_0033: Unknown result type (might be due to invalid IL or missing references)
			//IL_0039: Expected O, but got Unknown
			//IL_0066: Unknown result type (might be due to invalid IL or missing references)
			//IL_0070: Expected O, but got Unknown
			//IL_006b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0071: Expected O, but got Unknown
			if (mesh == null)
			{
				return new BoundingBox(Vector3.Zero, new Vector3(0.0001, 0.0001, 0.0001));
			}
			if (mesh.Disposed)
			{
				return new BoundingBox(Vector3.Zero, new Vector3(0.0001, 0.0001, 0.0001));
			}
			BoundingBox val = BoundingBoxFromMesh(mesh, basem);
			foreach (MeshBox item in (IEnumerable)this)
			{
				if (all || !item.SpecialMesh)
				{
					val += item.GetBoundingBox(basem, rec: true, all);
				}
			}
			return val;
		}

		internal Vector3[] GetBoundingBoxVectors()
		{
			Vector3[] array = new Vector3[2]
			{
				new Vector3(float.MaxValue, float.MaxValue, float.MaxValue),
				new Vector3(float.MinValue, float.MinValue, float.MinValue)
			};
			if (mesh != null)
			{
				int[] ranks = new int[1] { mesh.NumberVertices };
				CustomVertex.PositionNormal[] array2 = (CustomVertex.PositionNormal[])mesh.LockVertexBuffer(typeof(CustomVertex.PositionNormal), LockFlags.None, ranks);
				try
				{
					CustomVertex.PositionNormal[] array3 = array2;
					for (int i = 0; i < array3.Length; i++)
					{
						CustomVertex.PositionNormal positionNormal = array3[i];
						if (positionNormal.X < array[0].X)
						{
							array[0].X = positionNormal.X;
						}
						if (positionNormal.Y < array[0].Y)
						{
							array[0].Y = positionNormal.Y;
						}
						if (positionNormal.Z < array[0].Z)
						{
							array[0].Z = positionNormal.Z;
						}
						if (positionNormal.X > array[1].X)
						{
							array[1].X = positionNormal.X;
						}
						if (positionNormal.Y > array[1].Y)
						{
							array[1].Y = positionNormal.Y;
						}
						if (positionNormal.Z > array[1].Z)
						{
							array[1].Z = positionNormal.Z;
						}
					}
				}
				finally
				{
					mesh.UnlockVertexBuffer();
				}
			}
			return array;
		}

		internal Vector3 GetCenterOfMass()
		{
			BoundingBox boundingBox = GetBoundingBox(Converter.FromDx(wrld), rec: false, all: true);
			return Converter.ToDx((boundingBox.Min + boundingBox.Max) / 2.0);
		}

		internal Microsoft.DirectX.Direct3D.Cull GetCullMode(Microsoft.DirectX.Direct3D.Cull def)
		{
			if (cull == Cull.Default)
			{
				return def;
			}
			if (cull == Cull.None)
			{
				return Microsoft.DirectX.Direct3D.Cull.None;
			}
			if (cull == Cull.Clockwise)
			{
				return Microsoft.DirectX.Direct3D.Cull.Clockwise;
			}
			if (cull == Cull.CounterClockwise)
			{
				return Microsoft.DirectX.Direct3D.Cull.CounterClockwise;
			}
			return def;
		}

		internal double GetDistance(Vector3 v)
		{
			v -= GetCenterOfMass();
			return v.Length();
		}

		protected override void OnAdd(MeshBox m)
		{
			base.OnAdd(m);
			m?.SetParent(this);
		}

		protected override void OnRemove(MeshBox m)
		{
			base.OnRemove(m);
			m?.SetParent(null);
		}

		public void PrepareTexture(Device dev)
		{
			if (txtrmb != null)
			{
				txtrmb.PrepareTexture(dev);
			}
			else if (!(txtr != null) || txtr.Disposed || !(dev == txtrdev))
			{
				txtrdev = dev;
				if (txtr != null)
				{
					txtr.Dispose();
				}
				txtr = null;
				if (TextureStream != null && TextureStream.CanSeek && TextureStream.CanRead)
				{
					TextureStream.Seek(0L, SeekOrigin.Begin);
					txtr = TextureLoader.FromStream(dev, TextureStream);
				}
			}
		}

		protected void SetParent(MeshBox parent)
		{
			this.parent = parent;
		}

		public void SetTexture(Image img)
		{
			if (txtrstream != null)
			{
				txtrstream.Close();
			}
			txtrdev = null;
			txtrmb = null;
			if (img == null)
			{
				txtrstream = null;
				return;
			}
			txtrstream = new MemoryStream();
			img.Save(txtrstream, ImageFormat.Bmp);
			txtrstream.Seek(0L, SeekOrigin.Begin);
		}

		public void SetTexture(MeshBox txtrmb)
		{
			if (txtrstream != null)
			{
				txtrstream.Close();
			}
			txtrstream = null;
			txtrdev = null;
			this.txtrmb = txtrmb;
		}

		internal void SetupSortWorld(Microsoft.DirectX.Matrix world, Vector3 campos)
		{
			wrld = world;
			dist = GetDistance(campos);
		}
	}
	public class DirectXPanel : UserControl, IDisposable
	{
		private Device device;

		private PresentParameters presentParams = new PresentParameters();

		private ViewportSetting vp;

		private Effect effect;

		private MeshList meshes;

		private bool ignorechangeevent;

		private MeshList sortedlist;

		private Vector3 lastcampos;

		private Microsoft.DirectX.Matrix world = Microsoft.DirectX.Matrix.Identity;

		private MatrixStack mstack;

		private MouseEventArgs last;

		private ViewPortSetup vpsf;

		public override Color BackColor
		{
			get
			{
				return vp.BackgroundColor;
			}
			set
			{
				vp.BackgroundColor = value;
			}
		}

		public virtual Microsoft.DirectX.Matrix BillboardMatrix
		{
			get
			{
				Microsoft.DirectX.Matrix result = Microsoft.DirectX.Matrix.Multiply(vp.Rotation, vp.AngelRotation);
				result.Invert();
				return result;
			}
		}

		public Device Device
		{
			get
			{
				if (device == null)
				{
					InitializeGraphics(force: true);
				}
				return device;
			}
		}

		public Effect Effect
		{
			get
			{
				return effect;
			}
			set
			{
				effect = value;
			}
		}

		public MeshList Meshes => meshes;

		public virtual Microsoft.DirectX.Matrix ProjectionMatrix
		{
			get
			{
				float num = vp.FarPlane / vp.NearPlane;
				float num2 = Math.Max(vp.BoundingSphereRadius / 30f, vp.NearPlane + vp.Z);
				Math.Max(num2 * num, num2 * 1000f);
				num2 = vp.NearPlane + vp.Z;
				if (Settings.UseLefthandedCoordinates)
				{
					return Microsoft.DirectX.Matrix.PerspectiveFovLH(vp.FoV, vp.Aspect, vp.NearPlane, vp.FarPlane);
				}
				return Microsoft.DirectX.Matrix.PerspectiveFovRH(vp.FoV, vp.Aspect, vp.NearPlane, vp.FarPlane);
			}
		}

		public ViewportSetting Settings => vp;

		public virtual Microsoft.DirectX.Matrix ViewMatrix => Microsoft.DirectX.Matrix.Multiply(vp.Rotation, Microsoft.DirectX.Matrix.Multiply(vp.AngelRotation, Microsoft.DirectX.Matrix.Translation(vp.RealCameraPosition)));

		public Microsoft.DirectX.Matrix WorldMatrix
		{
			get
			{
				return world;
			}
			set
			{
				world = value;
			}
		}

		public event EventHandler ChangedLights;

		public event PrepareEffectEventHandler PrepareEffect;

		public event EventHandler ResetDevice;

		public event EventHandler RotationChanged;

		public DirectXPanel()
			: this(0.1)
		{
		}

		public DirectXPanel(double linewd)
		{
			vp = new ViewportSetting(this);
			vp.ChangedState += vp_ChangedState;
			vp.ChangedAttribute += vp_ChangedAttribute;
			Settings.BeginUpdate();
			Settings.LineWidth = Settings.LineWidth;
			meshes = new MeshList();
			base.ClientSize = new Size(400, 300);
			Text = "Direct3D Panel";
			BackColor = Color.Gray;
			ResetView();
			Settings.EndUpdate(fireattr: false, firestat: false);
		}

		protected void AddAxisMesh()
		{
			System.Drawing.Font f = new System.Drawing.Font("Tahoma", 8.25f);
			AddAxisMesh(f, "X", Color.Green, new Vector3(1f, 0f, 0f));
			AddAxisMesh(f, "Y", Color.Blue, new Vector3(0f, 1f, 0f));
			AddAxisMesh(f, "Z", Color.Brown, new Vector3(0f, 0f, 1f));
		}

		protected void AddAxisMesh(System.Drawing.Font f, string txt, Color cl, Vector3 dir)
		{
			Vector3 vector = (0f - Settings.AxisScale) * Settings.LineWidth * dir;
			MeshBox[] array = CreateLineMesh(vector, dir, 2f * Settings.AxisScale * Settings.LineWidth, GetMaterial(cl), wire: false, arrow: true);
			MeshBox[] array2 = array;
			for (int i = 0; i < array2.Length; i++)
			{
				array2[i].IgnoreDuringCameraReset = true;
			}
			Meshes.AddRange(array);
			Microsoft.DirectX.Matrix rotationMatrix = GetRotationMatrix(new Vector3(0f, 0f, 1f), dir);
			vector = 1.01f * vector;
			MeshBox meshBox = CreateTextMesh(vector.X, vector.Y, vector.Z, 10f * Settings.LineWidth, txt, Darken(cl, 0.5), rotationMatrix);
			meshBox.IgnoreDuringCameraReset = true;
			Meshes.Add(meshBox);
		}

		protected void AddBoundingBoxMesh()
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Expected O, but got Unknown
			Scene val = new Scene();
			for (int num = meshes.Count - 1; num >= 0; num--)
			{
				if (!meshes[num].SpecialMesh)
				{
					Mesh m = meshes[num].GetBoundingBox(rec: false, all: false).ToMesh(val);
					MeshBox meshBox = SceneToMesh.CreateDxMesh(device, m, isbb: true);
					if (meshBox != null)
					{
						meshes.Add(meshBox);
					}
				}
			}
		}

		public void AddLightMesh()
		{
			Material material = new Material();
			material.Diffuse = Color.Yellow;
			material.Ambient = Color.Yellow;
			material.Specular = Color.Yellow;
			material.SpecularSharpness = 1f;
			Material mat = material;
			material = new Material();
			material.Diffuse = Color.DarkGray;
			material.Ambient = Color.DarkGray;
			material.Specular = Color.DarkGray;
			material.SpecularSharpness = 1f;
			Material mat2 = material;
			Mesh mesh = Mesh.Sphere(Device, 2f * Settings.LineWidth, 10, 4);
			Mesh mesh2 = Mesh.Box(Device, 2f * Settings.LineWidth, 2f * Settings.LineWidth, 2f * Settings.LineWidth);
			for (int i = 0; i < Device.Lights.Count; i++)
			{
				Light light = Device.Lights[i];
				MeshBox meshBox = (light.Enabled ? new MeshBox(mesh, 1, mat) : new MeshBox(mesh, 1, mat2));
				meshBox.Transform = Microsoft.DirectX.Matrix.Translation(light.Position);
				meshBox.IgnoreDuringCameraReset = true;
				Meshes.Add(meshBox);
				meshBox = (light.Enabled ? new MeshBox(mesh2, 1, mat) : new MeshBox(mesh2, 1, mat2));
				meshBox.Transform = Microsoft.DirectX.Matrix.Translation(light.Position + 0.4f * light.Direction);
				meshBox.IgnoreDuringCameraReset = true;
				Meshes.Add(meshBox);
				meshBox = (light.Enabled ? new MeshBox(mesh2, 1, mat) : new MeshBox(mesh2, 1, mat2));
				meshBox.Transform = Microsoft.DirectX.Matrix.Translation(light.Position + 0.5f * light.Direction);
				meshBox.IgnoreDuringCameraReset = true;
				Meshes.Add(meshBox);
			}
		}

		public void AddScene(Scene scn)
		{
			SceneToMesh sceneToMesh = new SceneToMesh(scn, this);
			meshes.AddRange(sceneToMesh.ConvertToDx());
		}

		private void AddToSortedList(MeshBox box)
		{
			int index = sortedlist.Count;
			int num = 0;
			foreach (MeshBox item in (IEnumerable)sortedlist)
			{
				if (item.Distance >= box.Distance)
				{
					num++;
					continue;
				}
				index = num;
				break;
			}
			sortedlist.Insert(index, box);
		}

		public static Color Brighten(Color cl, double fact)
		{
			fact += 1.0;
			return ChangeBrightness(cl, fact);
		}

		public static Color ChangeBrightness(Color cl, double fact)
		{
			return Color.FromArgb(cl.A, Clamp((double)(int)cl.R * fact), Clamp((double)(int)cl.G * fact), Clamp((double)(int)cl.B * fact));
		}

		public static int Clamp(double comp)
		{
			int num = (int)comp;
			if (num < 0)
			{
				num = 0;
			}
			if (num > 255)
			{
				num = 255;
			}
			return num;
		}

		public MeshBox CreateBillboard(double width, double height, Image img)
		{
			float num = (float)(width / 2.0);
			float num2 = (float)(height / 2.0);
			CustomVertex.PositionNormalTextured[] array = new CustomVertex.PositionNormalTextured[5]
			{
				new CustomVertex.PositionNormalTextured(0f - num, 0f - num2, 0f, 0f, 0f, 0f, 0f, 0f),
				new CustomVertex.PositionNormalTextured(0f - num, num2, 0f, 0f, 0f, 0f, 0f, -1f),
				new CustomVertex.PositionNormalTextured(num, num2, 0f, 0f, 0f, 0f, 1f, -1f),
				new CustomVertex.PositionNormalTextured(num, 0f - num2, 0f, 0f, 0f, 0f, 1f, 0f),
				default(CustomVertex.PositionNormalTextured)
			};
			short[] array2 = new short[6] { 2, 1, 0, 0, 3, 2 };
			Mesh mesh = new Mesh(array2.Length / 3, array.Length, (MeshFlags)0, VertexFormats.PositionNormal | VertexFormats.Texture1, device);
			mesh.SetVertexBufferData(array, LockFlags.None);
			mesh.SetIndexBufferData(array2, LockFlags.None);
			int[] array3 = new int[mesh.NumberFaces * 3];
			mesh.GenerateAdjacency(0.01f, array3);
			mesh.OptimizeInPlace(MeshFlags.OptimizeVertexCache, array3);
			mesh.ComputeNormals(array3);
			MeshBox meshBox = new MeshBox(mesh, 1, GetMaterial(Color.FromArgb(255, Color.White)));
			meshBox.Wire = false;
			meshBox.Billboard = true;
			meshBox.Sort = true;
			meshBox.CullMode = MeshBox.Cull.None;
			meshBox.SetTexture(img);
			return meshBox;
		}

		public MeshBox CreateCube(double sz, Color cl)
		{
			return new MeshBox(Mesh.Box(Device, (float)sz, (float)sz, (float)sz), 1, GetMaterial(cl))
			{
				Wire = false
			};
		}

		public MeshBox[] CreateLayerMesh(Vector3 start, Vector3 dir1, Vector3 dir2, double width, double height, Material mat, bool wire)
		{
			Vector3 n = Vector3.Cross(dir1, dir2);
			return CreateLayerMesh(start, n, width, height, mat, wire);
		}

		public MeshBox[] CreateLayerMesh(Vector3 start, Vector3 n, double width, double height, Material mat, bool wire)
		{
			Mesh mesh = Mesh.Box(device, (float)width, (float)height, Settings.LineWidth * 0.3f);
			try
			{
				int[] array = new int[mesh.NumberFaces * 3];
				mesh.GenerateAdjacency(Settings.LineWidth, array);
				mesh = Mesh.TessellateNPatches(mesh, array, 32f, quadraticInterpNormals: true);
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
			}
			Microsoft.DirectX.Matrix transform = Microsoft.DirectX.Matrix.Multiply(GetRotationMatrix(new Vector3(0f, 0f, 1f), n), Microsoft.DirectX.Matrix.Translation(start));
			MeshBox meshBox = new MeshBox(mesh, 1, mat, transform)
			{
				Opaque = (mat.Diffuse.A == byte.MaxValue || mat.Diffuse.A == 0),
				Wire = wire
			};
			return new MeshBox[1] { meshBox };
		}

		public MeshBox[] CreateLineMesh(Vector3 start, Vector3 stop, Material mat, bool wire, bool arrow)
		{
			Vector3 dir = stop - start;
			return CreateLineMesh(start, dir, dir.Length(), mat, wire, arrow);
		}

		public MeshBox[] CreateLineMesh(Vector3 start, Vector3 stop, Material mat, bool wire, bool arrow, double linewd)
		{
			Vector3 dir = stop - start;
			return CreateLineMesh(start, dir, dir.Length(), mat, wire, arrow, linewd);
		}

		public MeshBox[] CreateLineMesh(Vector3 dir, double len, Material mat, bool wire, bool arrow)
		{
			return CreateLineMesh(new Vector3(0f, 0f, 0f), dir, len, mat, wire, arrow);
		}

		public MeshBox[] CreateLineMesh(Vector3 start, Vector3 dir, double len, Material mat, bool wire, bool arrow)
		{
			return CreateLineMesh(start, dir, len, mat, wire, arrow, Settings.LineWidth);
		}

		public MeshBox[] CreateLineMesh(Vector3 start, Vector3 dir, double len, Material mat, bool wire, bool arrow, double linewd)
		{
			float num = (float)linewd;
			Mesh mesh = Mesh.Cylinder(device, num, num, (float)len, 8, 2);
			Microsoft.DirectX.Matrix rotationMatrix = GetRotationMatrix(new Vector3(0f, 0f, 1f), dir);
			Microsoft.DirectX.Matrix transform = Microsoft.DirectX.Matrix.Multiply(Microsoft.DirectX.Matrix.Translation(new Vector3(0f, 0f, (float)(len / 2.0))), rotationMatrix);
			transform.Multiply(Microsoft.DirectX.Matrix.Translation(start));
			MeshBox meshBox = new MeshBox(mesh, 1, mat, transform)
			{
				Wire = wire
			};
			if (arrow)
			{
				Mesh mesh2 = CreatePyramidMesh(7f * num, 7f * num);
				transform = Microsoft.DirectX.Matrix.Multiply(Microsoft.DirectX.Matrix.Translation(new Vector3(0f, 0f, (float)len)), rotationMatrix);
				transform.Multiply(Microsoft.DirectX.Matrix.Translation(start));
				MeshBox meshBox2 = new MeshBox(mesh2, 1, mat, transform);
				meshBox.Opaque = mat.Diffuse.A == byte.MaxValue || mat.Diffuse.A != 0;
				meshBox2.Opaque = meshBox.Opaque;
				meshBox2.Wire = wire;
				return new MeshBox[2] { meshBox, meshBox2 };
			}
			return new MeshBox[1] { meshBox };
		}

		public MeshBox[] CreateNamedCube(double sz, Color bcl)
		{
			return CreateNamedCube(sz, bcl, GetTextColor(bcl), Microsoft.DirectX.Matrix.Identity);
		}

		public MeshBox[] CreateNamedCube(double sz, Color bcl, Color tcl)
		{
			return CreateNamedCube(sz, bcl, tcl, Microsoft.DirectX.Matrix.Identity);
		}

		public MeshBox[] CreateNamedCube(double sz, Color bcl, Color tcl, Microsoft.DirectX.Matrix basetrans)
		{
			MeshBox[] array = new MeshBox[7];
			double num = sz / 2.0;
			array[0] = CreateCube(sz, bcl);
			array[0].Transform = basetrans;
			array[1] = CreateTextMesh(0.0, 0.0, num, sz * 0.5, "+pz", tcl, Microsoft.DirectX.Matrix.RotationY((float)Math.PI));
			array[1].Transform = Microsoft.DirectX.Matrix.Multiply(array[1].Transform, array[0].Transform);
			array[2] = CreateTextMesh(0.0, 0.0, 0.0 - num, sz * 0.5, "-pz", tcl, Microsoft.DirectX.Matrix.Identity);
			array[2].Transform = Microsoft.DirectX.Matrix.Multiply(array[2].Transform, array[0].Transform);
			array[3] = CreateTextMesh(0.0, num, 0.0, sz * 0.5, "+py", tcl, Microsoft.DirectX.Matrix.RotationX((float)Math.PI / 2f));
			array[3].Transform = Microsoft.DirectX.Matrix.Multiply(array[3].Transform, array[0].Transform);
			array[4] = CreateTextMesh(0.0, 0.0 - num, 0.0, sz * 0.5, "-py", tcl, Microsoft.DirectX.Matrix.RotationX(-(float)Math.PI / 2f));
			array[4].Transform = Microsoft.DirectX.Matrix.Multiply(array[4].Transform, array[0].Transform);
			array[5] = CreateTextMesh(num, 0.0, 0.0, sz * 0.5, "+px", tcl, Microsoft.DirectX.Matrix.RotationY(-(float)Math.PI / 2f));
			array[5].Transform = Microsoft.DirectX.Matrix.Multiply(array[5].Transform, array[0].Transform);
			array[6] = CreateTextMesh(0.0 - num, 0.0, 0.0, sz * 0.5, "-px", tcl, Microsoft.DirectX.Matrix.RotationY((float)Math.PI / 2f));
			array[6].Transform = Microsoft.DirectX.Matrix.Multiply(array[6].Transform, array[0].Transform);
			return array;
		}

		public Mesh CreatePyramidMesh(double width, double height)
		{
			float num = (float)(width / 2.0);
			float num2 = (float)(height / 2.0);
			CustomVertex.PositionNormal[] array = new CustomVertex.PositionNormal[5]
			{
				new CustomVertex.PositionNormal(0f - num, 0f - num, 0f - num2, 0f, 0f, 0f),
				new CustomVertex.PositionNormal(num, 0f - num, 0f - num2, 0f, 0f, 0f),
				new CustomVertex.PositionNormal(num, num, 0f - num2, 0f, 0f, 0f),
				new CustomVertex.PositionNormal(0f - num, num, 0f - num2, 0f, 0f, 0f),
				new CustomVertex.PositionNormal(0f, 0f, num2, 0f, 0f, 0f)
			};
			short[] array2 = new short[18]
			{
				2, 1, 0, 0, 3, 2, 0, 1, 4, 1,
				2, 4, 2, 3, 4, 3, 0, 4
			};
			Mesh mesh = new Mesh(array2.Length / 3, array.Length, (MeshFlags)0, VertexFormats.PositionNormal, device);
			mesh.SetVertexBufferData(array, LockFlags.None);
			mesh.SetIndexBufferData(array2, LockFlags.None);
			int[] array3 = new int[mesh.NumberFaces * 3];
			mesh.GenerateAdjacency(0.01f, array3);
			mesh.OptimizeInPlace(MeshFlags.OptimizeVertexCache, array3);
			mesh.ComputeNormals(array3);
			return mesh;
		}

		public MeshBox CreateTextMesh(double x, double y, double z, double textsz, string text, Color cl)
		{
			return CreateTextMesh(x, y, z, textsz, text, cl, Microsoft.DirectX.Matrix.Identity);
		}

		public MeshBox CreateTextMesh(double x, double y, double z, double textsz, string text, Color cl, Microsoft.DirectX.Matrix trans)
		{
			if (double.IsNaN(textsz))
			{
				textsz = 1.0;
			}
			System.Drawing.Font font = new System.Drawing.Font("Tahoma", (float)textsz);
			MeshBox meshBox = new MeshBox(Mesh.TextFromFont(Device, font, text, Settings.LineWidth, Settings.LineWidth), 1, GetMaterial(cl));
			Vector3[] boundingBoxVectors = meshBox.GetBoundingBoxVectors();
			double num = (double)Math.Abs(boundingBoxVectors[1].X - boundingBoxVectors[0].X) / -2.0;
			double num2 = (double)Math.Abs(boundingBoxVectors[1].Y - boundingBoxVectors[0].Y) / -2.0;
			double num3 = (double)Math.Abs(boundingBoxVectors[1].Z - boundingBoxVectors[0].Z) / -2.0;
			meshBox.Transform = Microsoft.DirectX.Matrix.Multiply(Microsoft.DirectX.Matrix.Translation(new Vector3((float)num, (float)num2, (float)num3)), Microsoft.DirectX.Matrix.Multiply(Microsoft.DirectX.Matrix.Scaling((float)textsz, (float)textsz, 1f), Microsoft.DirectX.Matrix.Multiply(trans, Microsoft.DirectX.Matrix.Translation(new Vector3((float)x, (float)y, (float)z)))));
			meshBox.Wire = false;
			return meshBox;
		}

		public static Color Darken(Color cl, double fact)
		{
			return ChangeBrightness(cl, fact);
		}

		private void device_DeviceLost(object sender, EventArgs e)
		{
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				try
				{
					if (device != null)
					{
						device.DeviceReset -= OnResetDevice;
						device.DeviceLost -= device_DeviceLost;
						device.EvictManagedResources();
						device.Dispose();
					}
				}
				catch
				{
				}
				device = null;
				vp = null;
				if (meshes != null)
				{
					meshes.Clear(dispose: true);
				}
				meshes = null;
			}
			base.Dispose(disposing);
		}

		private void DoRenderMeshBox(int ct, MeshBox box, Cull cull, int pass)
		{
			if (box.Mesh == null)
			{
				return;
			}
			device.RenderState.FillMode = Settings.GetFillMode(box, pass);
			device.RenderState.CullMode = box.GetCullMode(cull);
			if (pass != 0 || Settings.FillMode != ViewportSettingBasic.FillModes.WireframeOverlay)
			{
				device.Material = box.Material;
				if (Settings.EnableTextures)
				{
					if (box.Texture == null)
					{
						box.PrepareTexture(device);
					}
					device.SetTexture(0, box.Texture);
				}
			}
			else
			{
				device.Material = GetMaterial(255, Color.Black);
			}
			if (effect != null && this.PrepareEffect != null && Settings.EnableShaderEffectPass)
			{
				this.PrepareEffect(this, box);
			}
			for (int i = 0; i < ct; i++)
			{
				if (effect != null && Settings.EnableShaderEffectPass)
				{
					effect.BeginPass(i);
				}
				try
				{
					for (int j = 0; j < box.SubSetCount; j++)
					{
						box.Mesh.DrawSubset(j);
					}
				}
				catch
				{
				}
				if (effect != null && Settings.EnableShaderEffectPass)
				{
					effect.EndPass();
				}
			}
			if (Settings.FillMode == ViewportSettingBasic.FillModes.WireframeOverlay && pass == 0 && !box.SpecialMesh)
			{
				DoRenderMeshBox(ct, box, cull, 1);
			}
		}

		public static Material GetMaterial(int alpha, Color cl)
		{
			return GetMaterial(Color.FromArgb(alpha, cl));
		}

		public static Material GetMaterial(Color cl)
		{
			Material result = new Material();
			result.Diffuse = cl;
			result.Ambient = Color.FromArgb(cl.A, (int)Math.Floor((double)(int)cl.R * 0.1), (int)Math.Floor((double)(int)cl.G * 0.1), (int)Math.Floor((double)(int)cl.B * 0.1));
			result.Specular = cl;
			result.SpecularSharpness = 100f;
			return result;
		}

		public static Microsoft.DirectX.Matrix GetRotationMatrix(Vector3 src, Vector3 dst)
		{
			return Microsoft.DirectX.Matrix.RotationQuaternion(GetRotationQuaternion(src, dst));
		}

		public static Quaternion GetRotationQuaternion(Vector3 src, Vector3 dst)
		{
			src.Normalize();
			dst.Normalize();
			Vector3 vector = Vector3.Cross(src, dst);
			_ = Math.Asin(vector.Length()) / 2.0;
			double num = Math.Acos(Vector3.Dot(src, dst)) / 2.0;
			vector.Normalize();
			vector = (float)Math.Sin(num) * vector;
			return new Quaternion(vector.X, vector.Y, vector.Z, (float)Math.Cos(num));
		}

		public static Color GetTextColor(Color cl)
		{
			if (cl.GetBrightness() >= 0.5f)
			{
				return Darken(cl, 0.5);
			}
			return Brighten(cl, 0.5);
		}

		protected bool InitializeGraphics(bool force)
		{
			try
			{
				presentParams.Windowed = true;
				presentParams.SwapEffect = SwapEffect.Discard;
				presentParams.EnableAutoDepthStencil = true;
				presentParams.AutoDepthStencilFormat = DepthFormat.D16;
				SetMultiSampleIfAvail(MultiSampleType.NonMaskable);
				SetMultiSampleIfAvail(MultiSampleType.TwoSamples);
				SetMultiSampleIfAvail(MultiSampleType.ThreeSamples);
				SetMultiSampleIfAvail(MultiSampleType.FourSamples);
				SetMultiSampleIfAvail(MultiSampleType.FiveSamples);
				SetMultiSampleIfAvail(MultiSampleType.SixSamples);
				SetMultiSampleIfAvail(MultiSampleType.SevenSamples);
				SetMultiSampleIfAvail(MultiSampleType.EightSamples);
				SetMultiSampleIfAvail(MultiSampleType.NineSamples);
				SetMultiSampleIfAvail(MultiSampleType.TenSamples);
				SetMultiSampleIfAvail(MultiSampleType.ElevenSamples);
				SetMultiSampleIfAvail(MultiSampleType.TwelveSamples);
				SetMultiSampleIfAvail(MultiSampleType.ThirteenSamples);
				SetMultiSampleIfAvail(MultiSampleType.FourteenSamples);
				SetMultiSampleIfAvail(MultiSampleType.FifteenSamples);
				SetMultiSampleIfAvail(MultiSampleType.SixteenSamples);
				PresentParameters[] presentationParameters = new PresentParameters[1] { presentParams };
				device = new Device(0, DeviceType.Hardware, this, CreateFlags.SoftwareVertexProcessing, presentationParameters);
				device.DeviceReset += OnResetDevice;
				device.DeviceLost += device_DeviceLost;
				OnCreateDevice(device, null);
				OnResetDevice(device, null);
				SetDeviceSize();
				Settings.Paused = false;
				return true;
			}
			catch (DirectXException)
			{
				return false;
			}
		}

		protected static bool IsDeviceMultiSampleOK(MultiSampleType multisampleType, DepthFormat depthFmt, Format backbufferFmt, out int result, out int qualityLevels)
		{
			AdapterInformation adapterInformation = Manager.Adapters.Default;
			if ((backbufferFmt != Format.Unknown && !Manager.CheckDeviceMultiSampleType(adapterInformation.Adapter, DeviceType.Hardware, backbufferFmt, windowed: false, multisampleType, out result, out qualityLevels)) || !Manager.CheckDeviceMultiSampleType(adapterInformation.Adapter, DeviceType.Hardware, (Format)depthFmt, windowed: false, multisampleType, out result, out qualityLevels))
			{
				return false;
			}
			return true;
		}

		public void LoadSettings(string flname)
		{
			vp.Load(flname);
		}

		protected void OnCreateDevice(object sender, EventArgs e)
		{
			_ = (Device)sender;
		}

		protected override void OnDoubleClick(EventArgs e)
		{
			base.OnDoubleClick(e);
			if (Settings.AllowSettingsDialog)
			{
				if (vpsf == null)
				{
					vpsf = ViewPortSetup.Execute(Settings, this);
					return;
				}
				ViewPortSetup.Hide(vpsf);
				vpsf.Dispose();
				vpsf = null;
			}
		}

		protected override void OnHandleDestroyed(EventArgs e)
		{
			base.OnHandleDestroyed(e);
			if (vpsf != null)
			{
				vpsf.Dispose();
				vpsf = null;
			}
		}

		protected override void OnMouseMove(MouseEventArgs e)
		{
			ignorechangeevent = true;
			try
			{
				base.OnMouseMove(e);
				if (last != null)
				{
					int num = e.X - last.X;
					int num2 = e.Y - last.Y;
					float num3 = 1f;
					if (!Settings.UseLefthandedCoordinates)
					{
						num3 = -1f;
					}
					if (e.Button == MouseButtons.Right)
					{
						vp.AngelY -= num3 * ((float)num / vp.InputRotationScale);
						vp.AngelX -= num3 * ((float)num2 / vp.InputRotationScale);
						if (this.RotationChanged != null)
						{
							this.RotationChanged(this, new EventArgs());
						}
					}
					if (e.Button == MouseButtons.Left)
					{
						vp.X += (float)num / ((float)base.Width * vp.InputTranslationScale / vp.BoundingSphereRadius);
						vp.Y -= (float)num2 / ((float)base.Height * vp.InputTranslationScale / vp.BoundingSphereRadius);
					}
					if (e.Button == MouseButtons.Middle)
					{
						vp.Z += num3 * ((float)num2 / ((float)base.Height * vp.InputTranslationScale / vp.BoundingSphereRadius));
						vp.AngelZ -= (float)num / vp.InputRotationScale;
						if (this.RotationChanged != null)
						{
							this.RotationChanged(this, new EventArgs());
						}
					}
					Render();
				}
				last = e;
			}
			finally
			{
				ignorechangeevent = false;
			}
		}

		protected override void OnMouseUp(MouseEventArgs e)
		{
			ignorechangeevent = true;
			vp.Rotation = Microsoft.DirectX.Matrix.Multiply(vp.Rotation, vp.AngelRotation);
			vp.ResetAngle();
			ignorechangeevent = false;
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			Render();
		}

		protected virtual void OnResetDevice(object sender, EventArgs e)
		{
			ignorechangeevent = true;
			try
			{
				Device obj = (Device)sender;
				obj.RenderState.Lighting = Settings.EnableLights;
				obj.RenderState.AlphaBlendEnable = Settings.EnableAlphaBlendPass;
				obj.RenderState.LocalViewer = true;
				obj.RenderState.ShadeMode = Settings.ShadeMode;
				obj.RenderState.SpecularEnable = Settings.EnableSpecularHighlights;
				obj.RenderState.Ambient = Settings.AmbientColor;
				SetupLights();
				if (mstack != null)
				{
					mstack.Dispose();
				}
				mstack = new MatrixStack();
				if (this.ResetDevice != null)
				{
					this.ResetDevice(this, new EventArgs());
				}
				if (Settings.AddAxis)
				{
					AddAxisMesh();
				}
				if (Settings.AddLightIndicators)
				{
					AddLightMesh();
				}
				if (Settings.RenderBoundingBoxes)
				{
					AddBoundingBoxMesh();
				}
			}
			catch (Exception value)
			{
				Console.WriteLine(value);
			}
			finally
			{
				ignorechangeevent = false;
			}
		}

		protected override void OnResize(EventArgs e)
		{
			base.Width = Math.Max(1, base.Width);
			base.Height = Math.Max(1, base.Height);
			Settings.Paused = Math.Min(base.Width, base.Height) <= 0;
			SetDeviceSize();
			if (base.Height == 0)
			{
				vp.Aspect = 1f;
			}
			else
			{
				vp.Aspect = (float)base.Width / (float)base.Height;
			}
			base.OnResize(e);
		}

		protected override void OnSizeChanged(EventArgs e)
		{
			base.Width = Math.Max(1, base.Width);
			base.Height = Math.Max(1, base.Height);
			base.OnSizeChanged(e);
		}

		public void ReloadMeshes()
		{
			OnResetDevice(device, new EventArgs());
			Render();
		}

		public void Render()
		{
			if (device == null)
			{
				InitializeGraphics(force: false);
			}
			if (device == null || Settings.Paused)
			{
				return;
			}
			if (sortedlist != null)
			{
				sortedlist.Clear(dispose: false);
			}
			else
			{
				sortedlist = new MeshList();
			}
			device.Clear(ClearFlags.Target | ClearFlags.ZBuffer, BackColor, 1f, 0);
			device.BeginScene();
			int ct = 1;
			if (effect != null && Settings.EnableShaderEffectPass)
			{
				ct = effect.Begin(FX.None);
			}
			SetupLights();
			SetupMatrices();
			SetLastCameraPos();
			RenderMeshList(ct, Meshes, alphapass: false, sorted: false);
			if (Settings.EnableAlphaBlendPass)
			{
				RenderMeshList(ct, Meshes, alphapass: true, sorted: false);
			}
			RenderMeshList(ct, sortedlist, alphapass: true, sorted: true);
			if (effect != null && Settings.EnableShaderEffectPass)
			{
				effect.End();
			}
			device.EndScene();
			try
			{
				device.Present();
			}
			catch (Exception value)
			{
				Console.WriteLine(value);
			}
		}

		private void RenderMeshBox(int ct, MeshBox box, Cull cull, bool alphapass, bool sorted)
		{
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			device.RenderState.ZBufferEnable = box.ZTest;
			SetupTextures(box.TextureMode);
			if (sorted)
			{
				device.Transform.World = box.World;
			}
			else
			{
				mstack.Push();
				mstack.MultiplyMatrixLocal(box.Transform);
				if (box.Billboard)
				{
					mstack.MultiplyMatrixLocal(BillboardMatrix);
				}
				device.Transform.World = mstack.Top;
				if (box.Sort)
				{
					box.SetupSortWorld(device.Transform.World, lastcampos);
					AddToSortedList(box);
				}
			}
			if ((!box.JointMesh || Settings.RenderJoints) && (sorted || !box.Sort))
			{
				DoRenderMeshBox(ct, box, cull, 0);
			}
			RenderMeshList(ct, box, alphapass, sorted: false);
			mstack.Pop();
		}

		private void RenderMeshList(int ct, MeshList meshes, bool alphapass, bool sorted)
		{
			if (meshes == null || meshes.Count == 0)
			{
				return;
			}
			if (!alphapass && !sorted)
			{
				device.RenderState.ZBufferWriteEnable = true;
				device.RenderState.AlphaBlendEnable = true;
				{
					foreach (MeshBox item in (IEnumerable)meshes)
					{
						if (item.Opaque || !Settings.EnableAlphaBlendPass)
						{
							RenderMeshBox(ct, item, Settings.MeshPassCullMode, alphapass, sorted);
						}
					}
					return;
				}
			}
			if (!(Settings.EnableAlphaBlendPass || sorted))
			{
				return;
			}
			device.RenderState.ZBufferWriteEnable = false;
			device.RenderState.AlphaBlendEnable = true;
			foreach (MeshBox item2 in (IEnumerable)meshes)
			{
				if (sorted || !item2.Opaque)
				{
					RenderMeshBox(ct, item2, Settings.AlphaPassCullMode, alphapass, sorted);
				}
			}
		}

		public void Reset()
		{
			if (device != null)
			{
				device.EvictManagedResources();
				try
				{
					OnResize(null);
					device.Reset(presentParams);
				}
				catch (Exception value)
				{
					Console.WriteLine(value);
				}
			}
			Render();
		}

		public void ResetDefaultViewport()
		{
			ResetView();
			OnResetDevice(device, null);
			Render();
		}

		protected void ResetView()
		{
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0011: Expected O, but got Unknown
			vp.Reset();
			BoundingBox val = new BoundingBox();
			try
			{
				foreach (MeshBox item in (IEnumerable)Meshes)
				{
					if (!item.SpecialMesh)
					{
						val += item.GetBoundingBox(rec: true, all: false);
					}
				}
				ResetView(Converter.ToDx(val.Min), Converter.ToDx(val.Max));
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message + "\n\n" + ex.StackTrace);
			}
		}

		protected void ResetView(Vector3 min, Vector3 max)
		{
			try
			{
				Settings.BeginUpdate();
				ignorechangeevent = true;
				if (min.X <= max.X)
				{
					Vector3 objectCenter = new Vector3((float)((double)(max.X + min.X) / 2.0), (float)((double)(max.Y + min.Y) / 2.0), (float)((double)(max.Z + min.Z) / 2.0));
					double num = new Vector3(min.X - objectCenter.X, min.Y - objectCenter.Y, min.Z - objectCenter.Z).Length();
					double num2 = num / Math.Sin(vp.FoV / 2f);
					vp.ObjectCenter = objectCenter;
					vp.X = 0f - objectCenter.X;
					vp.Y = 0f - objectCenter.Y;
					vp.Z = 0f - objectCenter.Z;
					vp.CameraTarget = new Vector3(0f, 0f, 0f);
					if (!Settings.UseLefthandedCoordinates)
					{
						vp.CameraPosition = new Vector3(0f, 0f, (0f - (float)num2) * Settings.InitialCameraOffsetScale);
					}
					else
					{
						vp.CameraPosition = new Vector3(0f, 0f, (float)num2 * Settings.InitialCameraOffsetScale);
					}
					vp.NearPlane = (float)(num2 - num);
					vp.FarPlane = (float)(num2 + num);
					vp.NearPlane = 1f;
					vp.FarPlane = 10000f;
					vp.BoundingSphereRadius = (float)num;
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message + "\n\n" + ex.StackTrace);
			}
			finally
			{
				Settings.EndUpdate();
				ignorechangeevent = false;
			}
		}

		public void ResetViewport(Vector3 min, Vector3 max)
		{
			ResetView(min, max);
			OnResetDevice(device, null);
			Render();
		}

		public Image Screenshot()
		{
			return Screenshot(ImageFileFormat.Png);
		}

		public Image Screenshot(ImageFileFormat format)
		{
			Surface backBuffer = device.GetBackBuffer(0, 0, BackBufferType.Mono);
			Image result = Image.FromStream(SurfaceLoader.SaveToStream(format, backBuffer));
			backBuffer.Dispose();
			return result;
		}

		private void SetDeviceSize()
		{
			if (device != null)
			{
				Viewport viewport = new Viewport();
				viewport.Width = base.Width;
				viewport.Height = base.Height;
				Viewport viewport2 = viewport;
				device.Viewport = viewport2;
			}
		}

		private void SetLastCameraPos()
		{
			lastcampos = new Vector3(0f, 0f, 0f);
			Microsoft.DirectX.Matrix viewMatrix = ViewMatrix;
			viewMatrix.Invert();
			lastcampos.TransformCoordinate(viewMatrix);
		}

		protected void SetMultiSampleIfAvail(MultiSampleType multisampleType)
		{
			int result = 0;
			int qualityLevels = 0;
			if (IsDeviceMultiSampleOK(multisampleType, presentParams.AutoDepthStencilFormat, presentParams.BackBufferFormat, out result, out qualityLevels) && result == 0)
			{
				presentParams.MultiSample = multisampleType;
				presentParams.MultiSampleQuality = qualityLevels - 1;
			}
		}

		protected virtual void SetupLights()
		{
			Vector3 cameraPosition = vp.CameraPosition;
			cameraPosition.TransformCoordinate(Microsoft.DirectX.Matrix.RotationY(-(float)Math.PI / 6f));
			Vector3 vector = -vp.CameraPosition;
			vector.TransformCoordinate(Microsoft.DirectX.Matrix.RotationY(-0.9239978f));
			vector.TransformCoordinate(Microsoft.DirectX.Matrix.RotationZ(-0.9239978f));
			Vector3 vector2 = -1f * vp.CameraPosition;
			vector2.TransformCoordinate(Microsoft.DirectX.Matrix.RotationZ(0.9817477f));
			vector2.TransformCoordinate(Microsoft.DirectX.Matrix.RotationX(0.74799824f));
			vector2.TransformCoordinate(Microsoft.DirectX.Matrix.RotationY(0.8975979f));
			device.Lights[0].Type = LightType.Directional;
			device.Lights[0].Attenuation0 = 0.1f;
			device.Lights[0].Attenuation1 = 0.1f;
			device.Lights[0].Attenuation2 = 0.1f;
			device.Lights[0].Diffuse = Settings.LightColorDiffuse;
			device.Lights[0].Specular = Settings.LightColorSpecular;
			device.Lights[0].Position = 2f * cameraPosition;
			device.Lights[0].Direction = vp.CameraTarget - device.Lights[0].Position;
			device.Lights[0].Range = 1f * (vp.ObjectCenter - device.Lights[0].Position).Length();
			device.Lights[0].Enabled = true;
			device.Lights[1].Type = device.Lights[0].Type;
			device.Lights[1].Attenuation0 = 0.1f;
			device.Lights[1].Attenuation1 = 0.1f;
			device.Lights[1].Attenuation2 = 0.1f;
			device.Lights[1].Falloff = device.Lights[0].Falloff;
			device.Lights[1].Diffuse = device.Lights[0].Diffuse;
			device.Lights[1].Specular = device.Lights[0].Specular;
			device.Lights[1].Position = 4f * vector;
			device.Lights[1].Direction = vp.CameraTarget - device.Lights[1].Position;
			device.Lights[1].Range = 1f * (vp.ObjectCenter - device.Lights[1].Position).Length();
			device.Lights[1].Enabled = true;
			device.Lights[2].Type = LightType.Directional;
			device.Lights[2].Attenuation0 = 0.1f;
			device.Lights[2].Attenuation1 = 0.1f;
			device.Lights[2].Attenuation2 = 0.1f;
			device.Lights[2].Falloff = device.Lights[0].Falloff;
			device.Lights[2].Diffuse = device.Lights[0].Diffuse;
			device.Lights[2].Specular = device.Lights[0].Specular;
			device.Lights[2].Position = 2f * vector2;
			device.Lights[2].Direction = vp.CameraTarget - device.Lights[2].Position;
			device.Lights[2].Range = 1f * (vp.ObjectCenter - device.Lights[2].Position).Length();
			device.Lights[2].Enabled = true;
			if (this.ChangedLights != null)
			{
				this.ChangedLights(this, new EventArgs());
			}
		}

		private void SetupMatrices()
		{
			device.Transform.World = world;
			device.Transform.View = ViewMatrix;
			device.Transform.Projection = ProjectionMatrix;
			mstack.LoadMatrix(world);
		}

		private void SetupTextures(TextureModes mode)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0119: Unknown result type (might be due to invalid IL or missing references)
			//IL_011b: Invalid comparison between Unknown and I4
			//IL_0233: Unknown result type (might be due to invalid IL or missing references)
			//IL_0235: Invalid comparison between Unknown and I4
			if ((int)mode == 0)
			{
				device.RenderState.SourceBlend = Microsoft.DirectX.Direct3D.Blend.SourceAlpha;
				device.RenderState.AlphaSourceBlend = Microsoft.DirectX.Direct3D.Blend.SourceAlpha;
				device.RenderState.DestinationBlend = Microsoft.DirectX.Direct3D.Blend.InvSourceAlpha;
				device.RenderState.AlphaDestinationBlend = Microsoft.DirectX.Direct3D.Blend.InvSourceAlpha;
				device.RenderState.AlphaBlendOperation = BlendOperation.Add;
				device.TextureState[0].ColorOperation = TextureOperation.BlendCurrentAlpha;
				device.TextureState[0].ColorArgument0 = TextureArgument.Diffuse;
				device.TextureState[0].ColorArgument1 = TextureArgument.TextureColor;
				device.TextureState[0].ColorArgument2 = TextureArgument.Diffuse;
				device.TextureState[0].AlphaOperation = TextureOperation.Modulate;
				device.TextureState[0].AlphaArgument0 = TextureArgument.Diffuse;
				device.TextureState[0].AlphaArgument1 = TextureArgument.TextureColor;
				device.TextureState[0].AlphaArgument2 = TextureArgument.Current;
			}
			else if ((int)mode == 1)
			{
				device.RenderState.SourceBlend = Microsoft.DirectX.Direct3D.Blend.Zero;
				device.RenderState.DestinationBlend = Microsoft.DirectX.Direct3D.Blend.InvSourceColor;
				device.RenderState.AlphaSourceBlend = Microsoft.DirectX.Direct3D.Blend.SourceColor;
				device.RenderState.AlphaDestinationBlend = Microsoft.DirectX.Direct3D.Blend.One;
				device.RenderState.AlphaBlendOperation = BlendOperation.Add;
				device.TextureState[0].ColorOperation = TextureOperation.Subtract;
				device.TextureState[0].ColorArgument0 = TextureArgument.Current;
				device.TextureState[0].ColorArgument1 = TextureArgument.TextureColor;
				device.TextureState[0].ColorArgument2 = TextureArgument.Diffuse;
				device.TextureState[0].AlphaOperation = TextureOperation.Disable;
				device.TextureState[0].AlphaArgument0 = TextureArgument.Current;
				device.TextureState[0].AlphaArgument1 = TextureArgument.Current;
				device.TextureState[0].AlphaArgument2 = TextureArgument.TextureColor;
			}
			else if ((int)mode != 4)
			{
				device.RenderState.SourceBlend = Microsoft.DirectX.Direct3D.Blend.SourceAlpha;
				device.RenderState.DestinationBlend = Microsoft.DirectX.Direct3D.Blend.SourceColor;
				device.RenderState.AlphaSourceBlend = Microsoft.DirectX.Direct3D.Blend.SourceAlpha;
				device.RenderState.AlphaDestinationBlend = Microsoft.DirectX.Direct3D.Blend.SourceAlpha;
				device.RenderState.AlphaBlendOperation = BlendOperation.Add;
				device.TextureState[0].ColorOperation = TextureOperation.SelectArg1;
				device.TextureState[0].ColorArgument0 = TextureArgument.Current;
				device.TextureState[0].ColorArgument1 = TextureArgument.Diffuse;
				device.TextureState[0].ColorArgument2 = TextureArgument.Current;
				device.TextureState[0].AlphaOperation = TextureOperation.Disable;
				device.TextureState[0].AlphaArgument0 = TextureArgument.Diffuse;
				device.TextureState[0].AlphaArgument1 = TextureArgument.Diffuse;
				device.TextureState[0].AlphaArgument2 = TextureArgument.Current;
			}
			else
			{
				device.RenderState.SourceBlend = Microsoft.DirectX.Direct3D.Blend.SourceAlpha;
				device.RenderState.AlphaSourceBlend = Microsoft.DirectX.Direct3D.Blend.SourceAlpha;
				device.RenderState.DestinationBlend = Microsoft.DirectX.Direct3D.Blend.InvSourceAlpha;
				device.RenderState.AlphaDestinationBlend = Microsoft.DirectX.Direct3D.Blend.InvSourceAlpha;
				device.RenderState.AlphaBlendOperation = BlendOperation.Add;
				device.TextureState[0].ColorOperation = TextureOperation.BlendTextureAlpha;
				device.TextureState[0].ColorArgument0 = TextureArgument.Diffuse;
				device.TextureState[0].ColorArgument1 = TextureArgument.TextureColor;
				device.TextureState[0].ColorArgument2 = TextureArgument.Current;
				device.TextureState[0].AlphaOperation = TextureOperation.Disable;
				device.TextureState[0].AlphaArgument0 = TextureArgument.Diffuse;
				device.TextureState[0].AlphaArgument1 = TextureArgument.TextureColor;
				device.TextureState[0].AlphaArgument2 = TextureArgument.Current;
			}
			device.TextureState[1].ColorOperation = TextureOperation.Disable;
			device.TextureState[1].AlphaOperation = TextureOperation.Disable;
		}

		private static double Sign(double v)
		{
			return v / Math.Abs(v);
		}

		public void UpdateRotation()
		{
			OnMouseUp(null);
		}

		private void vp_ChangedAttribute(object sender, EventArgs e)
		{
			if (!ignorechangeevent)
			{
				ignorechangeevent = true;
				Render();
				ignorechangeevent = false;
			}
		}

		private void vp_ChangedState(object sender, EventArgs e)
		{
			if (!ignorechangeevent)
			{
				ignorechangeevent = true;
				Reset();
				ignorechangeevent = false;
			}
		}
	}
	public class ViewPortSetup : Form
	{
		private IContainer components;

		private PropertyGrid pg;

		private ViewportSetting vp;

		private static bool visible;

		private DirectXPanel panel;

		public new static bool Visible => visible;

		private ViewPortSetup()
		{
			InitializeComponent();
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing && components != null)
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		public static ViewPortSetup Execute(ViewportSetting vp, DirectXPanel panel)
		{
			visible = true;
			ViewPortSetup viewPortSetup = new ViewPortSetup();
			viewPortSetup.vp = vp;
			viewPortSetup.panel = panel;
			viewPortSetup.SetContent(vp);
			viewPortSetup.Show();
			return viewPortSetup;
		}

		public static void Hide(ViewPortSetup f)
		{
			try
			{
				f.Close();
				visible = false;
			}
			catch
			{
			}
		}

		private void InitializeComponent()
		{
			this.pg = new System.Windows.Forms.PropertyGrid();
			base.SuspendLayout();
			this.pg.Dock = System.Windows.Forms.DockStyle.Fill;
			this.pg.HelpVisible = false;
			this.pg.LineColor = System.Drawing.SystemColors.ScrollBar;
			this.pg.Location = new System.Drawing.Point(0, 0);
			this.pg.Name = "pg";
			this.pg.Size = new System.Drawing.Size(248, 429);
			this.pg.TabIndex = 4;
			this.pg.ToolbarVisible = false;
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 14);
			base.ClientSize = new System.Drawing.Size(248, 429);
			base.Controls.Add(this.pg);
			this.Font = new System.Drawing.Font("Tahoma", 8.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
			base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			base.Name = "ViewPortSetup";
			this.Text = "ViewPort Setup";
			base.ResumeLayout(false);
		}

		private void pg_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
		{
			panel.Reset();
		}

		private void SetContent(ViewportSetting vp)
		{
			pg.SelectedObject = vp;
		}
	}
}
