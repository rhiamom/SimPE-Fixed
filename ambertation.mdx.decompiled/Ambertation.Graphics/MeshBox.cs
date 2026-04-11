using System;
using System.Collections;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using Ambertation.Geometry;
using Ambertation.Scenes;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;

namespace Ambertation.Graphics;

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

	private Matrix trans;

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

	private Matrix wrld;

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

	public Matrix Transform
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

	internal Matrix World => wrld;

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
		: this(mesh, subsetcount, new Material(), Matrix.Identity)
	{
	}

	public MeshBox(Mesh mesh)
		: this(mesh, new Material(), Matrix.Identity)
	{
	}

	public MeshBox(Mesh mesh, Material mat)
		: this(mesh, mat, Matrix.Identity)
	{
	}

	public MeshBox(Mesh mesh, int subsetcount, Material mat)
		: this(mesh, subsetcount, mat, Matrix.Identity)
	{
	}

	public MeshBox(Mesh mesh, Material mat, Matrix transform)
		: this(mesh, mesh.NumberAttributes, mat, transform)
	{
	}

	public MeshBox(Mesh mesh, int subsetcount, Material mat, Matrix transform)
		: this(mesh, subsetcount, mat, transform, wire: true, opaque: true)
	{
	}

	public MeshBox(Mesh mesh, int subsetcount, Material mat, Matrix transform, bool wire, bool opaque)
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

	internal void SetupSortWorld(Matrix world, Vector3 campos)
	{
		wrld = world;
		dist = GetDistance(campos);
	}
}
