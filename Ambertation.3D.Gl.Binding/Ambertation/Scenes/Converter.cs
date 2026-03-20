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
 
 using OpenTK.Mathematics;
using GeoVec2 = Ambertation.Geometry.Vector2;
using GeoVec3 = Ambertation.Geometry.Vector3;
using GeoVec4 = Ambertation.Geometry.Vector4;
using GeoMatrix = Ambertation.Geometry.Matrix;

namespace Ambertation.Scenes;

public class Converter
{
	public static Vector2 ToDx(GeoVec2 v) => new Vector2((float)v.X, (float)v.Y);
	public static Vector3 ToDx(GeoVec3 v) => new Vector3((float)v.X, (float)v.Y, (float)v.Z);
	public static Vector4 ToDx(GeoVec4 v) => new Vector4((float)v.X, (float)v.Y, (float)v.Z, (float)v.W);

	public static Matrix4 ToDx(Transformation t)
	{
		return Matrix4.Mult(
			Matrix4.CreateScale(ToDx(t.Scaling)),
			Matrix4.Mult(Matrix4.CreateRotationX((float)t.Rotation.X),
			Matrix4.Mult(Matrix4.CreateRotationY((float)t.Rotation.Y),
			Matrix4.Mult(Matrix4.CreateRotationZ((float)t.Rotation.Z),
			Matrix4.CreateTranslation(ToDx(t.Translation))))));
	}

	public static GeoMatrix FromDx(Matrix4 m)
	{
		var matrix = new GeoMatrix(4, 4);
		matrix[0, 0] = m.M11; matrix[0, 1] = m.M21; matrix[0, 2] = m.M31; matrix[0, 3] = m.M41;
		matrix[1, 0] = m.M12; matrix[1, 1] = m.M22; matrix[1, 2] = m.M32; matrix[1, 3] = m.M42;
		matrix[2, 0] = m.M13; matrix[2, 1] = m.M23; matrix[2, 2] = m.M33; matrix[2, 3] = m.M43;
		matrix[3, 0] = m.M14; matrix[3, 1] = m.M24; matrix[3, 2] = m.M34; matrix[3, 3] = m.M44;
		return matrix;
	}

	public static Matrix4 ToDx(GeoMatrix t)
	{
		Matrix4 result = new Matrix4();
		result.M11 = (float)t[0, 0]; result.M21 = (float)t[0, 1]; result.M31 = (float)t[0, 2]; result.M41 = (float)t[0, 3];
		result.M12 = (float)t[1, 0]; result.M22 = (float)t[1, 1]; result.M32 = (float)t[1, 2]; result.M42 = (float)t[1, 3];
		result.M13 = (float)t[2, 0]; result.M23 = (float)t[2, 1]; result.M33 = (float)t[2, 2]; result.M43 = (float)t[2, 3];
		result.M14 = (float)t[3, 0]; result.M24 = (float)t[3, 1]; result.M34 = (float)t[3, 2]; result.M44 = (float)t[3, 3];
		return result;
	}
}
