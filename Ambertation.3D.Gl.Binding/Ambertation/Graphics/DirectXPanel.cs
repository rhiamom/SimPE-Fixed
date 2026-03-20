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
using System.IO;
using System.Windows.Forms;
using Ambertation.Scenes;
using OpenTK.Graphics.OpenGL4;
using OpenTK.GLControl;
using OpenTK.Mathematics;

namespace Ambertation.Graphics;

/// <summary>Simple matrix stack for hierarchical transforms.</summary>
internal class MatrixStack : IDisposable
{
    private System.Collections.Generic.Stack<Matrix4> stack = new();
    public MatrixStack() { stack.Push(Matrix4.Identity); }
    public Matrix4 Top => stack.Peek();
    public void Push() { stack.Push(stack.Peek()); }
    public void Pop() { if (stack.Count > 1) stack.Pop(); }
    public void MultiplyMatrixLocal(Matrix4 m) { var top = stack.Pop(); stack.Push(Matrix4.Mult(top, m)); }
    public void LoadMatrix(Matrix4 m) { stack.Pop(); stack.Push(m); }
    public void Dispose() { stack.Clear(); }
}

public class DirectXPanel : GLControl, IDisposable
{
    // -----------------------------------------------------------------------
    // GLSL Shaders
    // -----------------------------------------------------------------------
    private const string VertSrc = @"#version 330 core
layout(location=0) in vec3 aPosition;
layout(location=1) in vec3 aNormal;
layout(location=2) in vec2 aTexCoord;
layout(location=3) in vec4 aColor;
uniform mat4 uModel;
uniform mat4 uView;
uniform mat4 uProjection;
out vec3 vFragPos;
out vec3 vNormal;
out vec2 vTexCoord;
out vec4 vColor;
void main() {
    vec4 worldPos = uModel * vec4(aPosition, 1.0);
    vFragPos = worldPos.xyz;
    vNormal = normalize(mat3(transpose(inverse(uModel))) * aNormal);
    vTexCoord = aTexCoord;
    vColor = aColor;
    gl_Position = uProjection * uView * worldPos;
}";
    private const string FragSrc = @"#version 330 core
in vec3 vFragPos;
in vec3 vNormal;
in vec2 vTexCoord;
in vec4 vColor;
uniform bool uUseTexture;
uniform bool uUseVertexColor;
uniform bool uEnableLighting;
uniform bool uEnableSpecular;
uniform vec4 uMatDiffuse;
uniform vec4 uMatAmbient;
uniform vec4 uMatSpecular;
uniform vec4 uMatEmissive;
uniform float uMatShininess;
uniform vec3 uAmbient;
uniform vec3 uCamPos;
uniform vec3 uLightDir0, uLightDir1, uLightDir2;
uniform vec4 uLightDiff0, uLightDiff1, uLightDiff2;
uniform vec4 uLightSpec0, uLightSpec1, uLightSpec2;
uniform sampler2D uTexture;
out vec4 fragColor;
vec4 CalcLight(vec3 toLight, vec4 lightDiff, vec4 lightSpec, vec3 normal, vec3 viewDir, vec4 diffMat) {
    float diff = max(dot(normal, toLight), 0.0);
    vec4 diffuse = diff * lightDiff * diffMat;
    vec3 reflectDir = reflect(-toLight, normal);
    float spec = pow(max(dot(viewDir, reflectDir), 0.0), max(uMatShininess, 1.0));
    vec4 specular = uEnableSpecular ? spec * lightSpec * uMatSpecular : vec4(0.0);
    return diffuse + specular;
}
void main() {
    vec4 baseColor = uUseVertexColor ? vColor : uMatDiffuse;
    if (uUseTexture) baseColor = texture(uTexture, vTexCoord) * baseColor;
    if (!uEnableLighting) { fragColor = clamp(baseColor + uMatEmissive, 0.0, 1.0); return; }
    vec3 normal = normalize(vNormal);
    vec3 viewDir = normalize(uCamPos - vFragPos);
    vec4 ambient = vec4(uAmbient, 1.0) * uMatAmbient;
    vec4 light = CalcLight(-normalize(uLightDir0), uLightDiff0, uLightSpec0, normal, viewDir, baseColor)
               + CalcLight(-normalize(uLightDir1), uLightDiff1, uLightSpec1, normal, viewDir, baseColor)
               + CalcLight(-normalize(uLightDir2), uLightDiff2, uLightSpec2, normal, viewDir, baseColor);
    fragColor = vec4(clamp((ambient + light).rgb + uMatEmissive.rgb, 0.0, 1.0), baseColor.a);
}";

    // -----------------------------------------------------------------------
    // Fields
    // -----------------------------------------------------------------------
    private int _prog;
    private bool _glReady;
    private int _uModel, _uView, _uProj;
    private int _uUseTexture, _uUseVertexColor, _uEnableLighting, _uEnableSpecular;
    private int _uMatDiffuse, _uMatAmbient, _uMatSpecular, _uMatEmissive, _uMatShininess;
    private int _uAmbient, _uCamPos;
    private int _uLightDir0, _uLightDir1, _uLightDir2;
    private int _uLightDiff0, _uLightDiff1, _uLightDiff2;
    private int _uLightSpec0, _uLightSpec1, _uLightSpec2;
    private int _uTexture;

    private ViewportSetting vp;
    private MeshList meshes;
    private MeshList sortedlist;
    private Vector3 lastcampos;
    private Matrix4 world = Matrix4.Identity;
    private MatrixStack mstack;
    private MouseEventArgs last;
    private ViewPortSetup vpsf;
    private bool ignorechangeevent;

    // -----------------------------------------------------------------------
    // Public surface
    // -----------------------------------------------------------------------
    public MeshList Meshes => meshes;
    // Device property kept for API compat â€” returns null (callers should not use it)
    public object Device => null;
    public object Effect { get; set; }
    public ViewportSetting Settings => vp;
    public override Color BackColor
    {
        get => vp.BackgroundColor;
        set => vp.BackgroundColor = value;
    }

    public virtual Matrix4 ProjectionMatrix
    {
        get
        {
            float fov = vp.FoV, aspect = vp.Aspect, near = vp.NearPlane, far = vp.FarPlane;
            return Matrix4.CreatePerspectiveFieldOfView(fov, aspect, near, far);
        }
    }

    public virtual Matrix4 BillboardMatrix
    {
        get
        {
            Matrix4 result = Matrix4.Mult(vp.Rotation, vp.AngelRotation);
            return result.Inverted();
        }
    }

    public virtual Matrix4 ViewMatrix =>
        Matrix4.Mult(vp.Rotation, Matrix4.Mult(vp.AngelRotation, Matrix4.CreateTranslation(vp.RealCameraPosition)));

    public Matrix4 WorldMatrix { get => world; set => world = value; }

    public event EventHandler ResetDevice;
    public event PrepareEffectEventHandler PrepareEffect;
    public event EventHandler ChangedLights;
    public event EventHandler RotationChanged;

    // -----------------------------------------------------------------------
    // Construction
    // -----------------------------------------------------------------------
    public DirectXPanel() : this(0.1) { }

    public DirectXPanel(double linewd) : base(new GLControlSettings())
    {
        vp = new ViewportSetting(this);
        vp.ChangedState += vp_ChangedState;
        vp.ChangedAttribute += vp_ChangedAttribute;
        Settings.BeginUpdate();
        Settings.LineWidth = Settings.LineWidth;
        meshes = new MeshList();
        base.ClientSize = new Size(400, 300);
        Text = "GL Panel";
        BackColor = Color.Gray;
        ResetView();
        Settings.EndUpdate(fireattr: false, firestat: false);
    }

    public void LoadSettings(string flname) { vp.Load(flname); }

    private void vp_ChangedAttribute(object sender, EventArgs e)
    {
        if (!ignorechangeevent) { ignorechangeevent = true; Render(); ignorechangeevent = false; }
    }

    private void vp_ChangedState(object sender, EventArgs e)
    {
        if (!ignorechangeevent) { ignorechangeevent = true; Reset(); ignorechangeevent = false; }
    }

    // -----------------------------------------------------------------------
    // GL Initialization
    // -----------------------------------------------------------------------
    private void EnsureGLInit()
    {
        if (_glReady || !IsHandleCreated) return;
        try
        {
            MakeCurrent();
            _prog = CreateShaderProgram(VertSrc, FragSrc);
            CacheUniforms();
            GL.Enable(EnableCap.DepthTest);
            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);
            _glReady = true;
        }
        catch (Exception ex) { Console.WriteLine("GL init failed: " + ex.Message); }
    }

    private void CacheUniforms()
    {
        _uModel   = GL.GetUniformLocation(_prog, "uModel");
        _uView    = GL.GetUniformLocation(_prog, "uView");
        _uProj    = GL.GetUniformLocation(_prog, "uProjection");
        _uUseTexture      = GL.GetUniformLocation(_prog, "uUseTexture");
        _uUseVertexColor  = GL.GetUniformLocation(_prog, "uUseVertexColor");
        _uEnableLighting  = GL.GetUniformLocation(_prog, "uEnableLighting");
        _uEnableSpecular  = GL.GetUniformLocation(_prog, "uEnableSpecular");
        _uMatDiffuse      = GL.GetUniformLocation(_prog, "uMatDiffuse");
        _uMatAmbient      = GL.GetUniformLocation(_prog, "uMatAmbient");
        _uMatSpecular     = GL.GetUniformLocation(_prog, "uMatSpecular");
        _uMatEmissive     = GL.GetUniformLocation(_prog, "uMatEmissive");
        _uMatShininess    = GL.GetUniformLocation(_prog, "uMatShininess");
        _uAmbient         = GL.GetUniformLocation(_prog, "uAmbient");
        _uCamPos          = GL.GetUniformLocation(_prog, "uCamPos");
        _uLightDir0 = GL.GetUniformLocation(_prog, "uLightDir0");
        _uLightDir1 = GL.GetUniformLocation(_prog, "uLightDir1");
        _uLightDir2 = GL.GetUniformLocation(_prog, "uLightDir2");
        _uLightDiff0 = GL.GetUniformLocation(_prog, "uLightDiff0");
        _uLightDiff1 = GL.GetUniformLocation(_prog, "uLightDiff1");
        _uLightDiff2 = GL.GetUniformLocation(_prog, "uLightDiff2");
        _uLightSpec0 = GL.GetUniformLocation(_prog, "uLightSpec0");
        _uLightSpec1 = GL.GetUniformLocation(_prog, "uLightSpec1");
        _uLightSpec2 = GL.GetUniformLocation(_prog, "uLightSpec2");
        _uTexture    = GL.GetUniformLocation(_prog, "uTexture");
    }

    private static int CreateShaderProgram(string vertSrc, string fragSrc)
    {
        int vert = GL.CreateShader(ShaderType.VertexShader);
        GL.ShaderSource(vert, vertSrc);
        GL.CompileShader(vert);
        GL.GetShader(vert, ShaderParameter.CompileStatus, out int vs);
        if (vs == 0) Console.WriteLine("Vert: " + GL.GetShaderInfoLog(vert));

        int frag = GL.CreateShader(ShaderType.FragmentShader);
        GL.ShaderSource(frag, fragSrc);
        GL.CompileShader(frag);
        GL.GetShader(frag, ShaderParameter.CompileStatus, out int fs);
        if (fs == 0) Console.WriteLine("Frag: " + GL.GetShaderInfoLog(frag));

        int prog = GL.CreateProgram();
        GL.AttachShader(prog, vert);
        GL.AttachShader(prog, frag);
        GL.LinkProgram(prog);
        GL.GetProgram(prog, GetProgramParameterName.LinkStatus, out int ls);
        if (ls == 0) Console.WriteLine("Link: " + GL.GetProgramInfoLog(prog));
        GL.DeleteShader(vert);
        GL.DeleteShader(frag);
        return prog;
    }

    // -----------------------------------------------------------------------
    // Render pipeline
    // -----------------------------------------------------------------------
    public void Render()
    {
        EnsureGLInit();
        if (!_glReady || Settings.Paused) return;
        MakeCurrent();
        var bg = BackColor;
        GL.ClearColor(bg.R / 255f, bg.G / 255f, bg.B / 255f, 1f);
        GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
        GL.UseProgram(_prog);
        if (sortedlist == null) sortedlist = new MeshList();
        else sortedlist.Clear(dispose: false);
        SetupLights();
        SetupMatrices();
        SetLastCameraPos();
        RenderMeshList(Meshes, alphapass: false, sorted: false);
        if (Settings.EnableAlphaBlendPass) RenderMeshList(Meshes, alphapass: true, sorted: false);
        RenderMeshList(sortedlist, alphapass: true, sorted: true);
        SwapBuffers();
    }

    private void SetLastCameraPos()
    {
        Matrix4 inv = ViewMatrix.Inverted();
        lastcampos = inv.Row3.Xyz;
    }

    private void RenderMeshList(MeshList meshes, bool alphapass, bool sorted)
    {
        if (meshes == null || meshes.Count == 0) return;
        if (!alphapass && !sorted)
        {
            GL.DepthMask(true);
            foreach (MeshBox item in (IEnumerable)meshes)
                if (item.Opaque || !Settings.EnableAlphaBlendPass)
                    RenderMeshBox(item, Settings.MeshPassCullMode, alphapass, sorted);
            return;
        }
        if (!Settings.EnableAlphaBlendPass && !sorted) return;
        GL.DepthMask(false);
        foreach (MeshBox item in (IEnumerable)meshes)
            if (sorted || !item.Opaque)
                RenderMeshBox(item, Settings.AlphaPassCullMode, alphapass, sorted);
        GL.DepthMask(true);
    }

    private void RenderMeshBox(MeshBox box, GlCullMode cull, bool alphapass, bool sorted)
    {
        GL.DepthFunc(box.ZTest ? DepthFunction.Less : DepthFunction.Always);
        if (!sorted)
        {
            mstack.Push();
            mstack.MultiplyMatrixLocal(box.Transform);
            if (box.Billboard) mstack.MultiplyMatrixLocal(BillboardMatrix);
            var top = mstack.Top;
            GL.UniformMatrix4(_uModel, true, ref top);
            if (box.Sort)
            {
                box.SetupSortWorld(top, lastcampos);
                AddToSortedList(box);
            }
        }
        else
        {
            var w = box.World;
            GL.UniformMatrix4(_uModel, true, ref w);
        }

        if ((!box.JointMesh || Settings.RenderJoints) && (sorted || !box.Sort))
            DoRenderMeshBox(box, cull, 0);

        RenderMeshList(box, alphapass, sorted: false);
        if (!sorted) mstack.Pop();
    }

    private void DoRenderMeshBox(MeshBox box, GlCullMode cull, int pass)
    {
        if (box.Mesh == null || box.Mesh.Disposed) return;

        // Fill mode
        var fillMode = Settings.GetFillMode(box, pass);
        GL.PolygonMode(MaterialFace.FrontAndBack,
            fillMode == ViewportSettingBasic.FillModes.Wireframe ? PolygonMode.Line :
            fillMode == ViewportSettingBasic.FillModes.Point ? PolygonMode.Point : PolygonMode.Fill);

        // Culling
        var effectiveCull = box.GetCullMode(cull);
        if (effectiveCull == GlCullMode.None)
        {
            GL.Disable(EnableCap.CullFace);
        }
        else
        {
            GL.Enable(EnableCap.CullFace);
            GL.CullFace(effectiveCull == GlCullMode.Clockwise ? CullFaceMode.Back : CullFaceMode.Front);
        }

        // Material
        GlMaterial mat;
        bool useVertexColor = false;
        if (pass == 0 && Settings.FillMode == ViewportSettingBasic.FillModes.WireframeOverlay)
        {
            mat = GetMaterial(Color.Black);
        }
        else
        {
            mat = box.Material;
            if (Settings.EnableTextures)
            {
                if (box.Texture == null || box.Texture.Disposed) box.PrepareTexture();
                if (box.Texture != null && !box.Texture.Disposed)
                {
                    GL.ActiveTexture(TextureUnit.Texture0);
                    GL.BindTexture(TextureTarget.Texture2D, box.Texture.Id);
                    GL.Uniform1(_uTexture, 0);
                    GL.Uniform1(_uUseTexture, 1);
                }
                else GL.Uniform1(_uUseTexture, 0);
            }
            else GL.Uniform1(_uUseTexture, 0);
        }

        GL.Uniform1(_uUseVertexColor, useVertexColor ? 1 : 0);
        GL.Uniform1(_uEnableLighting, Settings.EnableLights ? 1 : 0);
        GL.Uniform1(_uEnableSpecular, Settings.EnableSpecularHighlights ? 1 : 0);
        SetMaterialUniforms(mat);

        for (int j = 0; j < box.SubSetCount; j++)
        {
            try { box.Mesh.DrawSubset(j); } catch { }
        }

        GL.Uniform1(_uUseTexture, 0);
        GL.BindTexture(TextureTarget.Texture2D, 0);

        if (Settings.FillMode == ViewportSettingBasic.FillModes.WireframeOverlay && pass == 0 && !box.SpecialMesh)
            DoRenderMeshBox(box, cull, 1);
    }

    private void SetMaterialUniforms(GlMaterial mat)
    {
        var d = mat.Diffuse; GL.Uniform4(_uMatDiffuse, d.R/255f, d.G/255f, d.B/255f, d.A/255f);
        var a = mat.Ambient; GL.Uniform4(_uMatAmbient, a.R/255f, a.G/255f, a.B/255f, a.A/255f);
        var s = mat.Specular; GL.Uniform4(_uMatSpecular, s.R/255f, s.G/255f, s.B/255f, s.A/255f);
        var e = mat.Emissive; GL.Uniform4(_uMatEmissive, e.R/255f, e.G/255f, e.B/255f, e.A/255f);
        GL.Uniform1(_uMatShininess, mat.SpecularSharpness);
    }

    private void AddToSortedList(MeshBox box)
    {
        int index = sortedlist.Count, num = 0;
        foreach (MeshBox item in (IEnumerable)sortedlist)
        {
            if (item.Distance < box.Distance) { index = num; break; }
            num++;
        }
        sortedlist.Insert(index, box);
    }

    protected virtual void SetupLights()
    {
        Vector3 camPos = vp.CameraPosition;
        Vector3 cp0 = Vector3.TransformPosition(camPos, Matrix4.CreateRotationY(-MathF.PI / 6f));
        Vector3 cp1 = -vp.CameraPosition;
        cp1 = Vector3.TransformPosition(cp1, Matrix4.CreateRotationY(-0.9239978f));
        cp1 = Vector3.TransformPosition(cp1, Matrix4.CreateRotationZ(-0.9239978f));
        Vector3 cp2 = -1f * vp.CameraPosition;
        cp2 = Vector3.TransformPosition(cp2, Matrix4.CreateRotationZ(0.9817477f));
        cp2 = Vector3.TransformPosition(cp2, Matrix4.CreateRotationX(0.74799824f));
        cp2 = Vector3.TransformPosition(cp2, Matrix4.CreateRotationY(0.8975979f));

        Vector3 lp0 = 2f * cp0, lp1 = 4f * cp1, lp2 = 2f * cp2;
        Vector3 dir0 = vp.CameraTarget - lp0;
        Vector3 dir1 = vp.CameraTarget - lp1;
        Vector3 dir2 = vp.CameraTarget - lp2;

        var ld = Settings.LightColorDiffuse; var ls = Settings.LightColorSpecular;
        GL.Uniform3(_uLightDir0, dir0.X, dir0.Y, dir0.Z);
        GL.Uniform3(_uLightDir1, dir1.X, dir1.Y, dir1.Z);
        GL.Uniform3(_uLightDir2, dir2.X, dir2.Y, dir2.Z);
        GL.Uniform4(_uLightDiff0, ld.R/255f, ld.G/255f, ld.B/255f, 1f);
        GL.Uniform4(_uLightDiff1, ld.R/255f, ld.G/255f, ld.B/255f, 1f);
        GL.Uniform4(_uLightDiff2, ld.R/255f, ld.G/255f, ld.B/255f, 1f);
        GL.Uniform4(_uLightSpec0, ls.R/255f, ls.G/255f, ls.B/255f, 1f);
        GL.Uniform4(_uLightSpec1, ls.R/255f, ls.G/255f, ls.B/255f, 1f);
        GL.Uniform4(_uLightSpec2, ls.R/255f, ls.G/255f, ls.B/255f, 1f);
        var amb = Settings.AmbientColor;
        GL.Uniform3(_uAmbient, amb.R/255f, amb.G/255f, amb.B/255f);
        GL.Uniform3(_uCamPos, lastcampos.X, lastcampos.Y, lastcampos.Z);
        this.ChangedLights?.Invoke(this, new EventArgs());
    }

    private void SetupMatrices()
    {
        var view = ViewMatrix;
        var proj = ProjectionMatrix;
        GL.UniformMatrix4(_uView, true, ref view);
        GL.UniformMatrix4(_uProj, true, ref proj);
        var w = world;
        GL.UniformMatrix4(_uModel, true, ref w);
        if (mstack == null) mstack = new MatrixStack();
        mstack.LoadMatrix(world);
    }

    // -----------------------------------------------------------------------
    // Reset / device events (ported from DX)
    // -----------------------------------------------------------------------
    protected virtual void OnResetDevice(object sender, EventArgs e)
    {
        ignorechangeevent = true;
        try
        {
            if (mstack != null) { mstack.Dispose(); }
            mstack = new MatrixStack();
            this.ResetDevice?.Invoke(this, new EventArgs());
            if (Settings.AddAxis) AddAxisMesh();
            if (Settings.AddLightIndicators) AddLightMesh();
            if (Settings.RenderBoundingBoxes) AddBoundingBoxMesh();
        }
        catch (Exception ex) { Console.WriteLine(ex); }
        finally { ignorechangeevent = false; }
    }

    public void ReloadMeshes() { OnResetDevice(this, new EventArgs()); Render(); }

    public void AddScene(Scene scn)
    {
        var stm = new SceneToMesh(scn, this);
        meshes.AddRange(stm.ConvertToDx());
    }

    public void AddLightMesh()
    {
        var mat = GetMaterial(Color.Yellow);
        var mat2 = GetMaterial(Color.DarkGray);
        float lw = 2f * Settings.LineWidth;
        var sphere = GlMesh.CreateSphere(lw, 10, 4);
        var box = GlMesh.CreateBox(lw, lw, lw);
        // Just add 3 placeholder indicator meshes (no Device.Lights API)
        for (int i = 0; i < 3; i++)
        {
            var mb = new MeshBox(sphere, 1, mat);
            mb.Transform = Matrix4.Identity; mb.IgnoreDuringCameraReset = true;
            Meshes.Add(mb);
        }
    }

    protected void AddAxisMesh()
    {
        AddAxisMesh("X", Color.Green, new Vector3(1f, 0f, 0f));
        AddAxisMesh("Y", Color.Blue, new Vector3(0f, 1f, 0f));
        AddAxisMesh("Z", Color.Brown, new Vector3(0f, 0f, 1f));
    }

    protected void AddBoundingBoxMesh()
    {
        Scene owner = new Scene();
        for (int num = meshes.Count - 1; num >= 0; num--)
        {
            if (!meshes[num].SpecialMesh)
            {
                Ambertation.Scenes.Mesh m = meshes[num].GetBoundingBox(rec: false, all: false).ToMesh(owner);
                MeshBox mb = SceneToMesh.CreateDxMesh(m, isbb: true);
                if (mb != null) meshes.Add(mb);
            }
        }
    }

    protected void AddAxisMesh(string txt, Color cl, Vector3 dir)
    {
        Vector3 vector = (0f - Settings.AxisScale) * Settings.LineWidth * dir;
        MeshBox[] array = CreateLineMesh(vector, dir, 2f * Settings.AxisScale * Settings.LineWidth, GetMaterial(cl), wire: false, arrow: true);
        foreach (MeshBox mb in array) { mb.IgnoreDuringCameraReset = true; }
        Meshes.AddRange(array);
        Matrix4 rotm = GetRotationMatrix(new Vector3(0f, 0f, 1f), dir);
        Vector3 textPos = 1.01f * vector;
        MeshBox textMb = CreateTextMesh(textPos.X, textPos.Y, textPos.Z, 10f * Settings.LineWidth, txt, Darken(cl, 0.5), rotm);
        textMb.IgnoreDuringCameraReset = true;
        Meshes.Add(textMb);
    }

    // -----------------------------------------------------------------------
    // Reset / view
    // -----------------------------------------------------------------------
    public void Reset()
    {
        if (_glReady)
        {
            try
            {
                MakeCurrent();
                OnResetDevice(this, null);
            }
            catch (Exception ex) { Console.WriteLine(ex); }
        }
        Render();
    }

    public void ResetDefaultViewport()
    {
        ResetView();
        OnResetDevice(this, null);
        Render();
    }

    public void ResetViewport(Vector3 min, Vector3 max)
    {
        ResetView(min, max);
        OnResetDevice(this, null);
        Render();
    }

    protected void ResetView()
    {
        vp.Reset();
        BoundingBox boundingBox = new BoundingBox();
        try
        {
            foreach (MeshBox item in (IEnumerable)Meshes)
                if (!item.SpecialMesh) boundingBox += item.GetBoundingBox(rec: true, all: false);
            ResetView(Converter.ToDx(boundingBox.Min), Converter.ToDx(boundingBox.Max));
        }
        catch (Exception ex) { MessageBox.Show(ex.Message + "\n\n" + ex.StackTrace); }
    }

    protected void ResetView(Vector3 min, Vector3 max)
    {
        try
        {
            Settings.BeginUpdate();
            ignorechangeevent = true;
            if (!(min.X > max.X))
            {
                Vector3 objectCenter = new Vector3((max.X + min.X) / 2f, (max.Y + min.Y) / 2f, (max.Z + min.Z) / 2f);
                double num = new Vector3(min.X - objectCenter.X, min.Y - objectCenter.Y, min.Z - objectCenter.Z).Length;
                double num2 = num / Math.Sin(vp.FoV / 2f);
                vp.ObjectCenter = objectCenter;
                vp.X = -objectCenter.X; vp.Y = -objectCenter.Y; vp.Z = -objectCenter.Z;
                vp.CameraTarget = Vector3.Zero;
                vp.CameraPosition = Settings.UseLefthandedCoordinates
                    ? new Vector3(0f, 0f, (float)num2 * Settings.InitialCameraOffsetScale)
                    : new Vector3(0f, 0f, -(float)num2 * Settings.InitialCameraOffsetScale);
                vp.NearPlane = 1f; vp.FarPlane = 10000f;
                vp.BoundingSphereRadius = (float)num;
            }
        }
        catch (Exception ex) { MessageBox.Show(ex.Message + "\n\n" + ex.StackTrace); }
        finally { Settings.EndUpdate(); ignorechangeevent = false; }
    }

    // -----------------------------------------------------------------------
    // Mouse interaction
    // -----------------------------------------------------------------------
    public void UpdateRotation() { OnMouseUp(null); }

    protected override void OnMouseUp(MouseEventArgs e)
    {
        ignorechangeevent = true;
        vp.Rotation = Matrix4.Mult(vp.Rotation, vp.AngelRotation);
        vp.ResetAngle();
        ignorechangeevent = false;
    }

    protected override void OnMouseMove(MouseEventArgs e)
    {
        ignorechangeevent = true;
        try
        {
            base.OnMouseMove(e);
            if (last != null)
            {
                int dx = e.X - last.X, dy = e.Y - last.Y;
                float sign = Settings.UseLefthandedCoordinates ? 1f : -1f;
                if (e.Button == MouseButtons.Right)
                {
                    vp.AngelY -= sign * (dx / vp.InputRotationScale);
                    vp.AngelX -= sign * (dy / vp.InputRotationScale);
                    this.RotationChanged?.Invoke(this, new EventArgs());
                }
                if (e.Button == MouseButtons.Left)
                {
                    vp.X += dx / (base.Width * vp.InputTranslationScale / vp.BoundingSphereRadius);
                    vp.Y -= dy / (base.Height * vp.InputTranslationScale / vp.BoundingSphereRadius);
                }
                if (e.Button == MouseButtons.Middle)
                {
                    vp.Z += sign * (dy / (base.Height * vp.InputTranslationScale / vp.BoundingSphereRadius));
                    vp.AngelZ -= dx / vp.InputRotationScale;
                    this.RotationChanged?.Invoke(this, new EventArgs());
                }
                Render();
            }
            last = e;
        }
        finally { ignorechangeevent = false; }
    }

    protected override void OnPaint(PaintEventArgs e) { Render(); }

    protected override void OnSizeChanged(EventArgs e)
    {
        base.Width = Math.Max(1, base.Width); base.Height = Math.Max(1, base.Height);
        base.OnSizeChanged(e);
    }

    protected override void OnResize(EventArgs e)
    {
        base.Width = Math.Max(1, base.Width); base.Height = Math.Max(1, base.Height);
        Settings.Paused = Math.Min(base.Width, base.Height) <= 0;
        if (_glReady) { MakeCurrent(); GL.Viewport(0, 0, base.Width, base.Height); }
        vp.Aspect = base.Height != 0 ? (float)base.Width / base.Height : 1f;
        base.OnResize(e);
    }

    // -----------------------------------------------------------------------
    // Screenshot
    // -----------------------------------------------------------------------
    public Image Screenshot()
    {
        if (!_glReady) return null;
        MakeCurrent();
        var bmp = new System.Drawing.Bitmap(base.Width, base.Height);
        var data = bmp.LockBits(new Rectangle(0, 0, base.Width, base.Height),
            System.Drawing.Imaging.ImageLockMode.WriteOnly,
            System.Drawing.Imaging.PixelFormat.Format32bppArgb);
        GL.ReadPixels(0, 0, base.Width, base.Height,
            OpenTK.Graphics.OpenGL4.PixelFormat.Bgra, PixelType.UnsignedByte, data.Scan0);
        bmp.UnlockBits(data);
        bmp.RotateFlip(System.Drawing.RotateFlipType.RotateNoneFlipY);
        return bmp;
    }

    // -----------------------------------------------------------------------
    // Math helpers
    // -----------------------------------------------------------------------
    public static Matrix4 GetRotationMatrix(Vector3 src, Vector3 dst)
    {
        Quaternion q = GetRotationQuaternion(src, dst);
        return Matrix4.CreateFromQuaternion(q);
    }

    public static Quaternion GetRotationQuaternion(Vector3 src, Vector3 dst)
    {
        src = Vector3.Normalize(src); dst = Vector3.Normalize(dst);
        Vector3 axis = Vector3.Cross(src, dst);
        double angle = Math.Acos(Math.Clamp((double)Vector3.Dot(src, dst), -1.0, 1.0));
        axis = Vector3.Normalize(axis);
        axis *= (float)Math.Sin(angle / 2.0);
        return new Quaternion(axis.X, axis.Y, axis.Z, (float)Math.Cos(angle / 2.0));
    }

    // -----------------------------------------------------------------------
    // Mesh factories
    // -----------------------------------------------------------------------
    public GlMesh CreatePyramidMesh(double width, double height)
        => GlMesh.CreatePyramid((float)width, (float)height);

    public MeshBox[] CreateLineMesh(Vector3 start, Vector3 stop, GlMaterial mat, bool wire, bool arrow)
    {
        Vector3 dir = stop - start;
        return CreateLineMesh(start, dir, dir.Length, mat, wire, arrow);
    }

    public MeshBox[] CreateLineMesh(Vector3 start, Vector3 stop, GlMaterial mat, bool wire, bool arrow, double linewd)
    {
        Vector3 dir = stop - start;
        return CreateLineMesh(start, dir, dir.Length, mat, wire, arrow, linewd);
    }

    public MeshBox[] CreateLineMesh(Vector3 dir, double len, GlMaterial mat, bool wire, bool arrow)
        => CreateLineMesh(Vector3.Zero, dir, len, mat, wire, arrow);

    public MeshBox[] CreateLineMesh(Vector3 start, Vector3 dir, double len, GlMaterial mat, bool wire, bool arrow)
        => CreateLineMesh(start, dir, len, mat, wire, arrow, Settings.LineWidth);

    public MeshBox[] CreateLineMesh(Vector3 start, Vector3 dir, double len, GlMaterial mat, bool wire, bool arrow, double linewd)
    {
        float lw = (float)linewd;
        GlMesh mesh = GlMesh.CreateCylinder(lw, lw, (float)len, 8, 2);
        Matrix4 rotm = GetRotationMatrix(new Vector3(0, 0, 1), dir);
        Vector3 v = new Vector3(0, 0, (float)(len / 2.0));
        Matrix4 t1 = Matrix4.CreateTranslation(v);
        Matrix4 transform = Matrix4.Mult(t1, rotm);
        transform = Matrix4.Mult(transform, Matrix4.CreateTranslation(start));
        MeshBox mb = new MeshBox(mesh, 1, mat, transform);
        mb.Wire = wire;
        if (arrow)
        {
            GlMesh pyramid = GlMesh.CreatePyramid(7f * lw, 7f * lw);
            Vector3 v2 = new Vector3(0, 0, (float)len);
            Matrix4 t2 = Matrix4.CreateTranslation(v2);
            Matrix4 transform2 = Matrix4.Mult(t2, rotm);
            transform2 = Matrix4.Mult(transform2, Matrix4.CreateTranslation(start));
            MeshBox mb2 = new MeshBox(pyramid, 1, mat, transform2);
            mb.Opaque = mat.Diffuse.A == byte.MaxValue || mat.Diffuse.A != 0;
            mb2.Opaque = mb.Opaque; mb2.Wire = wire;
            return new MeshBox[] { mb, mb2 };
        }
        return new MeshBox[] { mb };
    }

    public MeshBox[] CreateNamedCube(double sz, Color bcl) => CreateNamedCube(sz, bcl, GetTextColor(bcl), Matrix4.Identity);
    public MeshBox[] CreateNamedCube(double sz, Color bcl, Color tcl) => CreateNamedCube(sz, bcl, tcl, Matrix4.Identity);

    public MeshBox[] CreateNamedCube(double sz, Color bcl, Color tcl, Matrix4 basetrans)
    {
        var array = new MeshBox[7];
        double h = sz / 2.0;
        array[0] = CreateCube(sz, bcl); array[0].Transform = basetrans;
        array[1] = CreateTextMesh(0, 0, h, sz*0.5, "+pz", tcl, Matrix4.CreateRotationY(MathF.PI)); array[1].Transform = Matrix4.Mult(array[1].Transform, basetrans);
        array[2] = CreateTextMesh(0, 0, -h, sz*0.5, "-pz", tcl, Matrix4.Identity); array[2].Transform = Matrix4.Mult(array[2].Transform, basetrans);
        array[3] = CreateTextMesh(0, h, 0, sz*0.5, "+py", tcl, Matrix4.CreateRotationX(MathF.PI/2f)); array[3].Transform = Matrix4.Mult(array[3].Transform, basetrans);
        array[4] = CreateTextMesh(0, -h, 0, sz*0.5, "-py", tcl, Matrix4.CreateRotationX(-MathF.PI/2f)); array[4].Transform = Matrix4.Mult(array[4].Transform, basetrans);
        array[5] = CreateTextMesh(h, 0, 0, sz*0.5, "+px", tcl, Matrix4.CreateRotationY(-MathF.PI/2f)); array[5].Transform = Matrix4.Mult(array[5].Transform, basetrans);
        array[6] = CreateTextMesh(-h, 0, 0, sz*0.5, "-px", tcl, Matrix4.CreateRotationY(MathF.PI/2f)); array[6].Transform = Matrix4.Mult(array[6].Transform, basetrans);
        return array;
    }

    public MeshBox CreateCube(double sz, Color cl)
    {
        GlMesh mesh = GlMesh.CreateBox((float)sz, (float)sz, (float)sz);
        MeshBox mb = new MeshBox(mesh, 1, GetMaterial(cl));
        mb.Wire = false; return mb;
    }

    public MeshBox CreateBillboard(double width, double height, Image img)
    {
        GlMesh mesh = GlMesh.CreateBillboard((float)width, (float)height);
        MeshBox mb = new MeshBox(mesh, 1, GetMaterial(Color.FromArgb(255, Color.White)));
        mb.Wire = false; mb.Billboard = true; mb.Sort = true;
        mb.CullMode = MeshBox.Cull.None; mb.SetTexture(img);
        return mb;
    }

    public MeshBox CreateTextMesh(double x, double y, double z, double textsz, string text, Color cl)
        => CreateTextMesh(x, y, z, textsz, text, cl, Matrix4.Identity);

    public MeshBox CreateTextMesh(double x, double y, double z, double textsz, string text, Color cl, Matrix4 trans)
    {
        if (double.IsNaN(textsz)) textsz = 1.0;
        float sz = (float)textsz;
        // Render text to bitmap and create a billboard quad
        var font = new System.Drawing.Font("Tahoma", sz);
        SizeF textSize;
        using (var tmp = new System.Drawing.Bitmap(1, 1))
        using (var g = System.Drawing.Graphics.FromImage(tmp))
            textSize = g.MeasureString(text, font);

        int bw = Math.Max(1, (int)textSize.Width), bh = Math.Max(1, (int)textSize.Height);
        var bmp = new System.Drawing.Bitmap(bw, bh, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
        using (var g = System.Drawing.Graphics.FromImage(bmp))
        {
            g.Clear(System.Drawing.Color.Transparent);
            g.DrawString(text, font, new System.Drawing.SolidBrush(cl), 0f, 0f);
        }
        font.Dispose();

        float qw = bw * sz * 0.01f, qh = bh * sz * 0.01f;
        GlMesh mesh = GlMesh.CreateBillboard(qw, qh);
        MeshBox mb = new MeshBox(mesh, 1, GetMaterial(Color.White));
        mb.SetTexture(bmp); bmp.Dispose();
        mb.Wire = false;
        mb.Transform = Matrix4.Mult(
            Matrix4.CreateTranslation(-(qw/2f), -(qh/2f), 0f),
            Matrix4.Mult(trans, Matrix4.CreateTranslation((float)x, (float)y, (float)z)));
        return mb;
    }

    public MeshBox[] CreateLayerMesh(Vector3 start, Vector3 dir1, Vector3 dir2, double width, double height, GlMaterial mat, bool wire)
    {
        Vector3 n = Vector3.Cross(dir1, dir2);
        return CreateLayerMesh(start, n, width, height, mat, wire);
    }

    public MeshBox[] CreateLayerMesh(Vector3 start, Vector3 n, double width, double height, GlMaterial mat, bool wire)
    {
        GlMesh mesh = GlMesh.CreateBox((float)width, (float)height, Settings.LineWidth * 0.3f);
        Matrix4 rotm = GetRotationMatrix(new Vector3(0, 0, 1), n);
        Matrix4 transform = Matrix4.Mult(rotm, Matrix4.CreateTranslation(start));
        MeshBox mb = new MeshBox(mesh, 1, mat, transform);
        mb.Opaque = mat.Diffuse.A == byte.MaxValue || mat.Diffuse.A == 0;
        mb.Wire = wire;
        return new MeshBox[] { mb };
    }

    // -----------------------------------------------------------------------
    // Material helpers
    // -----------------------------------------------------------------------
    public static GlMaterial GetMaterial(int alpha, Color cl) => GetMaterial(Color.FromArgb(alpha, cl));

    public static GlMaterial GetMaterial(Color cl)
    {
        return new GlMaterial
        {
            Diffuse = cl,
            Ambient = Color.FromArgb(cl.A, (int)(cl.R * 0.1), (int)(cl.G * 0.1), (int)(cl.B * 0.1)),
            Specular = cl,
            SpecularSharpness = 100f
        };
    }

    public static int Clamp(double comp)
    {
        int n = (int)comp;
        if (n < 0) n = 0;
        if (n > 255) n = 255;
        return n;
    }

    public static Color ChangeBrightness(Color cl, double fact)
        => Color.FromArgb(cl.A, Clamp(cl.R * fact), Clamp(cl.G * fact), Clamp(cl.B * fact));
    public static Color Brighten(Color cl, double fact) => ChangeBrightness(cl, fact + 1.0);
    public static Color Darken(Color cl, double fact) => ChangeBrightness(cl, fact);
    public static Color GetTextColor(Color cl)
        => cl.GetBrightness() >= 0.5f ? Darken(cl, 0.5) : Brighten(cl, 0.5);

    // -----------------------------------------------------------------------
    // UI events
    // -----------------------------------------------------------------------
    protected override void OnDoubleClick(EventArgs e)
    {
        base.OnDoubleClick(e);
        if (Settings.AllowSettingsDialog)
        {
            if (vpsf == null) { vpsf = ViewPortSetup.Execute(Settings, this); return; }
            ViewPortSetup.Hide(vpsf); vpsf.Dispose(); vpsf = null;
        }
    }

    protected override void OnHandleDestroyed(EventArgs e)
    {
        base.OnHandleDestroyed(e);
        if (vpsf != null) { vpsf.Dispose(); vpsf = null; }
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            try
            {
                if (_glReady) { MakeCurrent(); GL.DeleteProgram(_prog); }
            }
            catch { }
            _glReady = false;
            vp = null;
            meshes?.Clear(dispose: true); meshes = null;
        }
        base.Dispose(disposing);
    }
}