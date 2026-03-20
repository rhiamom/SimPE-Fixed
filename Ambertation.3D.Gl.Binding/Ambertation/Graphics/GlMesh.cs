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
using System.Collections.Generic;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace Ambertation.Graphics;

/// <summary>
/// OpenGL mesh: stores vertex data in CPU arrays and lazily uploads to VAO/VBO/EBO on first draw.
/// Interleaved vertex layout (stride 48 bytes):
///   location 0: vec3 position  (offset  0)
///   location 1: vec3 normal    (offset 12)
///   location 2: vec2 texcoord  (offset 24)
///   location 3: vec4 color     (offset 32)
/// </summary>
public class GlMesh : IDisposable
{
    // Vertex stride in floats: 3+3+2+4 = 12, in bytes: 48
    private const int Stride = 12; // floats
    private const int StrideBytes = Stride * sizeof(float);

    // CPU data
    private float[] _vertices; // interleaved: pos(3) + normal(3) + uv(2) + color(4)
    private int[] _indices;
    private int _vertexCount;
    private int _faceCount;

    // GPU objects
    private int _vao, _vbo, _ebo;
    private bool _uploaded;
    private bool _disposed;

    public int NumberVertices => _vertexCount;
    public int NumberFaces => _faceCount;
    public int NumberAttributes => 1; // always 1 subset
    public bool Disposed => _disposed;

    private GlMesh(float[] vertices, int[] indices, int vertexCount, int faceCount)
    {
        _vertices = vertices;
        _indices = indices;
        _vertexCount = vertexCount;
        _faceCount = faceCount;
    }

    // -----------------------------------------------------------------------
    // Public factory: build from explicit arrays
    // -----------------------------------------------------------------------

    /// <summary>
    /// Build a mesh from position+normal+uv arrays (PositionNormalTextured).
    /// </summary>
    public static GlMesh FromPositionNormalTextured(
        Vector3[] positions, Vector3[] normals, Vector2[] uvs, int[] indices)
    {
        int n = positions.Length;
        float[] verts = new float[n * Stride];
        for (int i = 0; i < n; i++)
        {
            int b = i * Stride;
            verts[b + 0] = positions[i].X; verts[b + 1] = positions[i].Y; verts[b + 2] = positions[i].Z;
            verts[b + 3] = normals[i].X;   verts[b + 4] = normals[i].Y;   verts[b + 5] = normals[i].Z;
            verts[b + 6] = uvs[i].X;       verts[b + 7] = uvs[i].Y;
            verts[b + 8] = 1f; verts[b + 9] = 1f; verts[b + 10] = 1f; verts[b + 11] = 1f; // white
        }
        return new GlMesh(verts, indices, n, indices.Length / 3);
    }

    /// <summary>
    /// Build a mesh from position+normal+ARGB-color arrays (PositionNormalColored).
    /// </summary>
    public static GlMesh FromPositionNormalColored(
        Vector3[] positions, Vector3[] normals, int[] argbColors, int[] indices)
    {
        int n = positions.Length;
        float[] verts = new float[n * Stride];
        for (int i = 0; i < n; i++)
        {
            int b = i * Stride;
            verts[b + 0] = positions[i].X; verts[b + 1] = positions[i].Y; verts[b + 2] = positions[i].Z;
            verts[b + 3] = normals[i].X;   verts[b + 4] = normals[i].Y;   verts[b + 5] = normals[i].Z;
            verts[b + 6] = 0f;             verts[b + 7] = 0f; // no uv
            ArgbToFloats(argbColors[i], out verts[b + 8], out verts[b + 9], out verts[b + 10], out verts[b + 11]);
        }
        return new GlMesh(verts, indices, n, indices.Length / 3);
    }

    /// <summary>
    /// Build a mesh from position+normal arrays (PositionNormal).
    /// </summary>
    public static GlMesh FromPositionNormal(
        Vector3[] positions, Vector3[] normals, int[] indices)
    {
        int n = positions.Length;
        float[] verts = new float[n * Stride];
        for (int i = 0; i < n; i++)
        {
            int b = i * Stride;
            verts[b + 0] = positions[i].X; verts[b + 1] = positions[i].Y; verts[b + 2] = positions[i].Z;
            verts[b + 3] = normals[i].X;   verts[b + 4] = normals[i].Y;   verts[b + 5] = normals[i].Z;
            verts[b + 6] = 0f; verts[b + 7] = 0f;
            verts[b + 8] = 1f; verts[b + 9] = 1f; verts[b + 10] = 1f; verts[b + 11] = 1f;
        }
        return new GlMesh(verts, indices, n, indices.Length / 3);
    }

    /// <summary>
    /// Build a mesh from position+ARGB-color arrays (PositionColored, no normals).
    /// </summary>
    public static GlMesh FromPositionColored(
        Vector3[] positions, int[] argbColors, int[] indices)
    {
        int n = positions.Length;
        float[] verts = new float[n * Stride];
        for (int i = 0; i < n; i++)
        {
            int b = i * Stride;
            verts[b + 0] = positions[i].X; verts[b + 1] = positions[i].Y; verts[b + 2] = positions[i].Z;
            verts[b + 3] = 0f; verts[b + 4] = 0f; verts[b + 5] = 0f; // no normal
            verts[b + 6] = 0f; verts[b + 7] = 0f;
            ArgbToFloats(argbColors[i], out verts[b + 8], out verts[b + 9], out verts[b + 10], out verts[b + 11]);
        }
        return new GlMesh(verts, indices, n, indices.Length / 3);
    }

    private static void ArgbToFloats(int argb, out float r, out float g, out float b, out float a)
    {
        a = ((argb >> 24) & 0xFF) / 255f;
        r = ((argb >> 16) & 0xFF) / 255f;
        g = ((argb >>  8) & 0xFF) / 255f;
        b = ((argb      ) & 0xFF) / 255f;
    }

    // -----------------------------------------------------------------------
    // Primitive factories
    // -----------------------------------------------------------------------

    public static GlMesh CreateBox(float width, float height, float depth)
    {
        float w = width * 0.5f, h = height * 0.5f, d = depth * 0.5f;
        // 6 faces × 4 vertices = 24 vertices, 6×2×3 = 36 indices
        var pos = new Vector3[24];
        var nrm = new Vector3[24];
        int vi = 0;
        Action<Vector3, Vector3, Vector3, Vector3, Vector3> face = (p0, p1, p2, p3, n) =>
        {
            pos[vi] = p0; nrm[vi++] = n;
            pos[vi] = p1; nrm[vi++] = n;
            pos[vi] = p2; nrm[vi++] = n;
            pos[vi] = p3; nrm[vi++] = n;
        };
        face(new(-w,-h,-d), new( w,-h,-d), new( w, h,-d), new(-w, h,-d), new(0,0,-1)); // -Z
        face(new( w,-h, d), new(-w,-h, d), new(-w, h, d), new( w, h, d), new(0,0, 1)); // +Z
        face(new(-w,-h, d), new( w,-h, d), new( w,-h,-d), new(-w,-h,-d), new(0,-1,0)); // -Y
        face(new(-w, h,-d), new( w, h,-d), new( w, h, d), new(-w, h, d), new(0, 1,0)); // +Y
        face(new(-w,-h, d), new(-w,-h,-d), new(-w, h,-d), new(-w, h, d), new(-1,0,0)); // -X
        face(new( w,-h,-d), new( w,-h, d), new( w, h, d), new( w, h,-d), new( 1,0,0)); // +X

        var idx = new int[36];
        for (int f = 0; f < 6; f++)
        {
            int b = f * 6, v = f * 4;
            idx[b+0]=v; idx[b+1]=v+1; idx[b+2]=v+2;
            idx[b+3]=v; idx[b+4]=v+2; idx[b+5]=v+3;
        }
        return FromPositionNormal(pos, nrm, idx);
    }

    public static GlMesh CreateSphere(float radius, int slices, int stacks)
    {
        slices = Math.Max(3, slices);
        stacks = Math.Max(2, stacks);
        int n = (slices + 1) * (stacks + 1);
        var pos = new Vector3[n];
        var nrm = new Vector3[n];
        var uvs = new Vector2[n];
        int vi = 0;
        for (int j = 0; j <= stacks; j++)
        {
            float v = j / (float)stacks;
            float phi = v * MathF.PI;
            for (int i = 0; i <= slices; i++)
            {
                float u = i / (float)slices;
                float theta = u * 2f * MathF.PI;
                float x = MathF.Sin(phi) * MathF.Cos(theta);
                float y = MathF.Cos(phi);
                float z = MathF.Sin(phi) * MathF.Sin(theta);
                nrm[vi] = new Vector3(x, y, z);
                pos[vi] = nrm[vi] * radius;
                uvs[vi] = new Vector2(u, v);
                vi++;
            }
        }
        var idx = new List<int>();
        for (int j = 0; j < stacks; j++)
        {
            for (int i = 0; i < slices; i++)
            {
                int a = j * (slices + 1) + i;
                int b = a + slices + 1;
                idx.Add(a); idx.Add(b); idx.Add(a + 1);
                idx.Add(b); idx.Add(b + 1); idx.Add(a + 1);
            }
        }
        return FromPositionNormalTextured(pos, nrm, uvs, idx.ToArray());
    }

    public static GlMesh CreateCylinder(float bottomRadius, float topRadius, float height, int slices, int stacks)
    {
        slices = Math.Max(3, slices);
        stacks = Math.Max(1, stacks);
        float halfH = height * 0.5f;
        var posList = new List<Vector3>();
        var nrmList = new List<Vector3>();
        var idxList = new List<int>();

        // Side
        for (int j = 0; j <= stacks; j++)
        {
            float t = j / (float)stacks;
            float y = -halfH + t * height;
            float r = bottomRadius + t * (topRadius - bottomRadius);
            for (int i = 0; i <= slices; i++)
            {
                float theta = i / (float)slices * 2f * MathF.PI;
                float x = MathF.Cos(theta), z = MathF.Sin(theta);
                posList.Add(new Vector3(x * r, y, z * r));
                float slope = (bottomRadius - topRadius) / height;
                var n = new Vector3(x, slope, z);
                nrmList.Add(Vector3.Normalize(n));
            }
        }
        for (int j = 0; j < stacks; j++)
        {
            for (int i = 0; i < slices; i++)
            {
                int a = j * (slices + 1) + i;
                int b = a + slices + 1;
                idxList.Add(a); idxList.Add(b); idxList.Add(a + 1);
                idxList.Add(b); idxList.Add(b + 1); idxList.Add(a + 1);
            }
        }

        // Bottom cap
        int centerBot = posList.Count;
        posList.Add(new Vector3(0, -halfH, 0));
        nrmList.Add(new Vector3(0, -1, 0));
        int firstBot = posList.Count;
        for (int i = 0; i <= slices; i++)
        {
            float theta = i / (float)slices * 2f * MathF.PI;
            posList.Add(new Vector3(MathF.Cos(theta) * bottomRadius, -halfH, MathF.Sin(theta) * bottomRadius));
            nrmList.Add(new Vector3(0, -1, 0));
        }
        for (int i = 0; i < slices; i++)
        {
            idxList.Add(centerBot);
            idxList.Add(firstBot + i + 1);
            idxList.Add(firstBot + i);
        }

        // Top cap
        int centerTop = posList.Count;
        posList.Add(new Vector3(0, halfH, 0));
        nrmList.Add(new Vector3(0, 1, 0));
        int firstTop = posList.Count;
        for (int i = 0; i <= slices; i++)
        {
            float theta = i / (float)slices * 2f * MathF.PI;
            posList.Add(new Vector3(MathF.Cos(theta) * topRadius, halfH, MathF.Sin(theta) * topRadius));
            nrmList.Add(new Vector3(0, 1, 0));
        }
        for (int i = 0; i < slices; i++)
        {
            idxList.Add(centerTop);
            idxList.Add(firstTop + i);
            idxList.Add(firstTop + i + 1);
        }

        return FromPositionNormal(posList.ToArray(), nrmList.ToArray(), idxList.ToArray());
    }

    /// <summary>Pyramid with square base, apex pointing +Z.</summary>
    public static GlMesh CreatePyramid(float width, float height)
    {
        float h = width * 0.5f;
        float ht = height * 0.5f;
        var pos = new Vector3[]
        {
            new(-h,-h,-ht), new( h,-h,-ht), new( h, h,-ht), new(-h, h,-ht),
            new( 0, 0, ht)
        };
        var idx = new int[] { 2,1,0, 0,3,2, 0,1,4, 1,2,4, 2,3,4, 3,0,4 };
        // Compute face normals (flat shading ok for small decorative meshes)
        var posFull = new List<Vector3>();
        var nrmFull = new List<Vector3>();
        var idxFull = new List<int>();
        for (int t = 0; t < idx.Length / 3; t++)
        {
            Vector3 a = pos[idx[t*3]], b = pos[idx[t*3+1]], c = pos[idx[t*3+2]];
            Vector3 n = Vector3.Normalize(Vector3.Cross(b - a, c - a));
            int vi = posFull.Count;
            posFull.Add(a); nrmFull.Add(n);
            posFull.Add(b); nrmFull.Add(n);
            posFull.Add(c); nrmFull.Add(n);
            idxFull.Add(vi); idxFull.Add(vi+1); idxFull.Add(vi+2);
        }
        return FromPositionNormal(posFull.ToArray(), nrmFull.ToArray(), idxFull.ToArray());
    }

    /// <summary>Flat quad in XY plane, centered at origin.</summary>
    public static GlMesh CreateBillboard(float width, float height)
    {
        float w = width * 0.5f, h = height * 0.5f;
        var pos = new Vector3[] { new(-w,-h,0), new(-w, h,0), new( w, h,0), new( w,-h,0) };
        var nrm = new Vector3[] { new(0,0,1), new(0,0,1), new(0,0,1), new(0,0,1) };
        var uvs = new Vector2[] { new(0,1), new(0,0), new(1,0), new(1,1) };
        var idx = new int[] { 0,1,2, 0,2,3 };
        return FromPositionNormalTextured(pos, nrm, uvs, idx);
    }

    // -----------------------------------------------------------------------
    // Rendering
    // -----------------------------------------------------------------------

    private void EnsureUploaded()
    {
        if (_uploaded) return;
        _vao = GL.GenVertexArray();
        _vbo = GL.GenBuffer();
        _ebo = GL.GenBuffer();

        GL.BindVertexArray(_vao);

        GL.BindBuffer(BufferTarget.ArrayBuffer, _vbo);
        GL.BufferData(BufferTarget.ArrayBuffer, _vertices.Length * sizeof(float), _vertices, BufferUsageHint.StaticDraw);

        GL.BindBuffer(BufferTarget.ElementArrayBuffer, _ebo);
        GL.BufferData(BufferTarget.ElementArrayBuffer, _indices.Length * sizeof(int), _indices, BufferUsageHint.StaticDraw);

        // position (location 0)
        GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, StrideBytes, 0);
        GL.EnableVertexAttribArray(0);
        // normal (location 1)
        GL.VertexAttribPointer(1, 3, VertexAttribPointerType.Float, false, StrideBytes, 3 * sizeof(float));
        GL.EnableVertexAttribArray(1);
        // texcoord (location 2)
        GL.VertexAttribPointer(2, 2, VertexAttribPointerType.Float, false, StrideBytes, 6 * sizeof(float));
        GL.EnableVertexAttribArray(2);
        // color (location 3)
        GL.VertexAttribPointer(3, 4, VertexAttribPointerType.Float, false, StrideBytes, 8 * sizeof(float));
        GL.EnableVertexAttribArray(3);

        GL.BindVertexArray(0);
        _uploaded = true;
    }

    public void DrawSubset(int subset)
    {
        EnsureUploaded();
        GL.BindVertexArray(_vao);
        GL.DrawElements(PrimitiveType.Triangles, _indices.Length, DrawElementsType.UnsignedInt, 0);
        GL.BindVertexArray(0);
    }

    // -----------------------------------------------------------------------
    // CPU-side position access (for bounding box computation)
    // -----------------------------------------------------------------------

    public IEnumerable<Vector3> GetPositions()
    {
        for (int i = 0; i < _vertexCount; i++)
        {
            int b = i * Stride;
            yield return new Vector3(_vertices[b], _vertices[b + 1], _vertices[b + 2]);
        }
    }

    // -----------------------------------------------------------------------
    // IDisposable
    // -----------------------------------------------------------------------

    public void Dispose()
    {
        if (_disposed) return;
        if (_uploaded)
        {
            GL.DeleteVertexArray(_vao);
            GL.DeleteBuffer(_vbo);
            GL.DeleteBuffer(_ebo);
            _uploaded = false;
        }
        _vertices = null;
        _indices = null;
        _disposed = true;
    }
}
