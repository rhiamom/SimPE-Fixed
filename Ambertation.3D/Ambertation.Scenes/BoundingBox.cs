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
 
 using Ambertation.Geometry;
using Ambertation.Geometry.Collections;

namespace Ambertation.Scenes;

public class BoundingBox
{
	private Vector3 min;

	private Vector3 max;

	public Vector3 Max => max;

	public Vector3 Min => min;

	public Vector3 Size => max - min;

	public BoundingBox()
	{
		min = new Vector3(double.MaxValue, double.MaxValue, double.MaxValue);
		max = new Vector3(double.MinValue, double.MinValue, double.MinValue);
	}

	public BoundingBox(Vector3 min, Vector3 max)
	{
		this.min = min;
		this.max = max;
	}

	public BoundingBox(Vector3Collection vertices)
	{
		min = new Vector3(double.MaxValue, double.MaxValue, double.MaxValue);
		max = new Vector3(double.MinValue, double.MinValue, double.MinValue);
		foreach (Vector3 vertex in vertices)
		{
			min.SetMin(vertex);
			max.SetMax(vertex);
		}
		if (min.X > max.X)
		{
			min.X = 0.0;
			max.X = 0.0;
		}
		if (min.Y > max.Y)
		{
			min.Y = 0.0;
			max.Y = 0.0;
		}
		if (min.Z > max.Z)
		{
			min.Z = 0.0;
			max.Z = 0.0;
		}
	}

	public Mesh ToMesh(Scene owner)
	{
		Mesh mesh = new Mesh(null, "BBox", owner.DefaultMaterial, owner);
		mesh.Vertices.Add(new Vector3(min.X, min.Y, max.Z));
		mesh.Vertices.Add(new Vector3(max.X, min.Y, max.Z));
		mesh.Vertices.Add(new Vector3(max.X, max.Y, max.Z));
		mesh.Vertices.Add(new Vector3(min.X, max.Y, max.Z));
		mesh.Vertices.Add(new Vector3(min.X, min.Y, min.Z));
		mesh.Vertices.Add(new Vector3(max.X, min.Y, min.Z));
		mesh.Vertices.Add(new Vector3(max.X, max.Y, min.Z));
		mesh.Vertices.Add(new Vector3(min.X, max.Y, min.Z));
		mesh.FaceIndices.Add(new Vector3i(0, 1, 2));
		mesh.FaceIndices.Add(new Vector3i(2, 3, 0));
		mesh.FaceIndices.Add(new Vector3i(6, 7, 4));
		mesh.FaceIndices.Add(new Vector3i(4, 5, 6));
		mesh.FaceIndices.Add(new Vector3i(1, 2, 6));
		mesh.FaceIndices.Add(new Vector3i(6, 5, 1));
		mesh.FaceIndices.Add(new Vector3i(2, 3, 7));
		mesh.FaceIndices.Add(new Vector3i(7, 6, 2));
		mesh.FaceIndices.Add(new Vector3i(3, 0, 4));
		mesh.FaceIndices.Add(new Vector3i(4, 7, 3));
		mesh.FaceIndices.Add(new Vector3i(0, 1, 5));
		mesh.FaceIndices.Add(new Vector3i(5, 4, 0));
		return mesh;
	}

	public static BoundingBox operator +(BoundingBox b1, BoundingBox b2)
	{
		Vector3 vector = new Vector3(double.MaxValue, double.MaxValue, double.MaxValue);
		Vector3 vector2 = new Vector3(double.MinValue, double.MinValue, double.MinValue);
		vector2.SetMax(b1.Max);
		vector.SetMin(b1.Min);
		vector2.SetMax(b2.Max);
		vector.SetMin(b2.Min);
		return new BoundingBox(vector, vector2);
	}
}
