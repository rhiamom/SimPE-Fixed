using Ambertation.Geometry;
using Microsoft.DirectX;

namespace Ambertation.Scenes;

public class Converter
{
	public static Matrix FromDx(Matrix m)
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

	public static Matrix ToDx(Transformation t)
	{
		return Matrix.Multiply(Matrix.Scaling(ToDx(t.Scaling)), Matrix.Multiply(Matrix.RotationX((float)((Vector2)t.Rotation).X), Matrix.Multiply(Matrix.RotationY((float)((Vector2)t.Rotation).Y), Matrix.Multiply(Matrix.RotationZ((float)t.Rotation.Z), Matrix.Translation(ToDx(t.Translation))))));
	}

	public static Matrix ToDx(Matrix t)
	{
		Matrix result = new Matrix();
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
