using System;
using System.Collections;
using System.Drawing;
using System.Windows.Forms;
using Ambertation.Scenes;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;

namespace Ambertation.Graphics;

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

	private Matrix world = Matrix.Identity;

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

	public virtual Matrix BillboardMatrix
	{
		get
		{
			Matrix result = Matrix.Multiply(vp.Rotation, vp.AngelRotation);
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

	public virtual Matrix ProjectionMatrix
	{
		get
		{
			float num = vp.FarPlane / vp.NearPlane;
			float num2 = Math.Max(vp.BoundingSphereRadius / 30f, vp.NearPlane + vp.Z);
			Math.Max(num2 * num, num2 * 1000f);
			num2 = vp.NearPlane + vp.Z;
			if (Settings.UseLefthandedCoordinates)
			{
				return Matrix.PerspectiveFovLH(vp.FoV, vp.Aspect, vp.NearPlane, vp.FarPlane);
			}
			return Matrix.PerspectiveFovRH(vp.FoV, vp.Aspect, vp.NearPlane, vp.FarPlane);
		}
	}

	public ViewportSetting Settings => vp;

	public virtual Matrix ViewMatrix => Matrix.Multiply(vp.Rotation, Matrix.Multiply(vp.AngelRotation, Matrix.Translation(vp.RealCameraPosition)));

	public Matrix WorldMatrix
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
		Matrix rotationMatrix = GetRotationMatrix(new Vector3(0f, 0f, 1f), dir);
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
			meshBox.Transform = Matrix.Translation(light.Position);
			meshBox.IgnoreDuringCameraReset = true;
			Meshes.Add(meshBox);
			meshBox = (light.Enabled ? new MeshBox(mesh2, 1, mat) : new MeshBox(mesh2, 1, mat2));
			meshBox.Transform = Matrix.Translation(light.Position + 0.4f * light.Direction);
			meshBox.IgnoreDuringCameraReset = true;
			Meshes.Add(meshBox);
			meshBox = (light.Enabled ? new MeshBox(mesh2, 1, mat) : new MeshBox(mesh2, 1, mat2));
			meshBox.Transform = Matrix.Translation(light.Position + 0.5f * light.Direction);
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
		Matrix transform = Matrix.Multiply(GetRotationMatrix(new Vector3(0f, 0f, 1f), n), Matrix.Translation(start));
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
		Matrix rotationMatrix = GetRotationMatrix(new Vector3(0f, 0f, 1f), dir);
		Matrix transform = Matrix.Multiply(Matrix.Translation(new Vector3(0f, 0f, (float)(len / 2.0))), rotationMatrix);
		transform.Multiply(Matrix.Translation(start));
		MeshBox meshBox = new MeshBox(mesh, 1, mat, transform)
		{
			Wire = wire
		};
		if (arrow)
		{
			Mesh mesh2 = CreatePyramidMesh(7f * num, 7f * num);
			transform = Matrix.Multiply(Matrix.Translation(new Vector3(0f, 0f, (float)len)), rotationMatrix);
			transform.Multiply(Matrix.Translation(start));
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
		return CreateNamedCube(sz, bcl, GetTextColor(bcl), Matrix.Identity);
	}

	public MeshBox[] CreateNamedCube(double sz, Color bcl, Color tcl)
	{
		return CreateNamedCube(sz, bcl, tcl, Matrix.Identity);
	}

	public MeshBox[] CreateNamedCube(double sz, Color bcl, Color tcl, Matrix basetrans)
	{
		MeshBox[] array = new MeshBox[7];
		double num = sz / 2.0;
		array[0] = CreateCube(sz, bcl);
		array[0].Transform = basetrans;
		array[1] = CreateTextMesh(0.0, 0.0, num, sz * 0.5, "+pz", tcl, Matrix.RotationY((float)Math.PI));
		array[1].Transform = Matrix.Multiply(array[1].Transform, array[0].Transform);
		array[2] = CreateTextMesh(0.0, 0.0, 0.0 - num, sz * 0.5, "-pz", tcl, Matrix.Identity);
		array[2].Transform = Matrix.Multiply(array[2].Transform, array[0].Transform);
		array[3] = CreateTextMesh(0.0, num, 0.0, sz * 0.5, "+py", tcl, Matrix.RotationX((float)Math.PI / 2f));
		array[3].Transform = Matrix.Multiply(array[3].Transform, array[0].Transform);
		array[4] = CreateTextMesh(0.0, 0.0 - num, 0.0, sz * 0.5, "-py", tcl, Matrix.RotationX(-(float)Math.PI / 2f));
		array[4].Transform = Matrix.Multiply(array[4].Transform, array[0].Transform);
		array[5] = CreateTextMesh(num, 0.0, 0.0, sz * 0.5, "+px", tcl, Matrix.RotationY(-(float)Math.PI / 2f));
		array[5].Transform = Matrix.Multiply(array[5].Transform, array[0].Transform);
		array[6] = CreateTextMesh(0.0 - num, 0.0, 0.0, sz * 0.5, "-px", tcl, Matrix.RotationY((float)Math.PI / 2f));
		array[6].Transform = Matrix.Multiply(array[6].Transform, array[0].Transform);
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
		return CreateTextMesh(x, y, z, textsz, text, cl, Matrix.Identity);
	}

	public MeshBox CreateTextMesh(double x, double y, double z, double textsz, string text, Color cl, Matrix trans)
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
		meshBox.Transform = Matrix.Multiply(Matrix.Translation(new Vector3((float)num, (float)num2, (float)num3)), Matrix.Multiply(Matrix.Scaling((float)textsz, (float)textsz, 1f), Matrix.Multiply(trans, Matrix.Translation(new Vector3((float)x, (float)y, (float)z)))));
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

	public static Matrix GetRotationMatrix(Vector3 src, Vector3 dst)
	{
		return Matrix.RotationQuaternion(GetRotationQuaternion(src, dst));
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
		vp.Rotation = Matrix.Multiply(vp.Rotation, vp.AngelRotation);
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
		Matrix viewMatrix = ViewMatrix;
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
		cameraPosition.TransformCoordinate(Matrix.RotationY(-(float)Math.PI / 6f));
		Vector3 vector = -vp.CameraPosition;
		vector.TransformCoordinate(Matrix.RotationY(-0.9239978f));
		vector.TransformCoordinate(Matrix.RotationZ(-0.9239978f));
		Vector3 vector2 = -1f * vp.CameraPosition;
		vector2.TransformCoordinate(Matrix.RotationZ(0.9817477f));
		vector2.TransformCoordinate(Matrix.RotationX(0.74799824f));
		vector2.TransformCoordinate(Matrix.RotationY(0.8975979f));
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
			device.RenderState.SourceBlend = Blend.SourceAlpha;
			device.RenderState.AlphaSourceBlend = Blend.SourceAlpha;
			device.RenderState.DestinationBlend = Blend.InvSourceAlpha;
			device.RenderState.AlphaDestinationBlend = Blend.InvSourceAlpha;
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
			device.RenderState.SourceBlend = Blend.Zero;
			device.RenderState.DestinationBlend = Blend.InvSourceColor;
			device.RenderState.AlphaSourceBlend = Blend.SourceColor;
			device.RenderState.AlphaDestinationBlend = Blend.One;
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
			device.RenderState.SourceBlend = Blend.SourceAlpha;
			device.RenderState.DestinationBlend = Blend.SourceColor;
			device.RenderState.AlphaSourceBlend = Blend.SourceAlpha;
			device.RenderState.AlphaDestinationBlend = Blend.SourceAlpha;
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
			device.RenderState.SourceBlend = Blend.SourceAlpha;
			device.RenderState.AlphaSourceBlend = Blend.SourceAlpha;
			device.RenderState.DestinationBlend = Blend.InvSourceAlpha;
			device.RenderState.AlphaDestinationBlend = Blend.InvSourceAlpha;
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
