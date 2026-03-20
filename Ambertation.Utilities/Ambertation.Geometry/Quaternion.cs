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

namespace Ambertation.Geometry;

public class Quaternion : Vector4
{
	public new double Norm
	{
		get
		{
			double num = Imaginary.Norm + Math.Pow(W, 2.0);
			return num;
		}
	}

	public new double Length => Math.Sqrt(Norm);

	public Quaternion Conjugate => new Quaternion(QuaternionParameterType.ImaginaryReal, -1.0 * Imaginary, W);

	public Vector3 Euler => GetEulerAngles();

	public Vector3 Imaginary => this;

	public static Quaternion Identity => new Quaternion(QuaternionParameterType.ImaginaryReal, 0.0, 0.0, 0.0, 1.0);

	public new static Quaternion Zero => new Quaternion(QuaternionParameterType.ImaginaryReal, 0.0, 0.0, 0.0, 0.0);

	public double Angle
	{
		get
		{
			MakeRobust();
			MakeUnitQuaternion();
			return Math.Acos(W) * 2.0;
		}
	}

	public Vector3 Axis
	{
		get
		{
			MakeRobust();
			MakeUnitQuaternion();
			double num = Math.Sqrt(1.0 - Math.Pow(W, 2.0));
			if (num == 0.0)
			{
				return new Vector3(0.0, 0.0, 0.0);
			}
			return new Vector3(base.X / num, base.Y / num, base.Z / num);
		}
	}

	public Matrix Matrix
	{
		get
		{
			MakeRobust();
			MakeUnitQuaternion();
			Matrix matrix = new Matrix(4, 4);
			double num = Math.Pow(base.X, 2.0);
			double num2 = Math.Pow(base.Y, 2.0);
			double num3 = Math.Pow(base.Z, 2.0);
			double num4 = Math.Pow(W, 2.0);
			matrix[0, 0] = 1.0 - 2.0 * (num2 + num3);
			matrix[0, 1] = 2.0 * (base.X * base.Y - W * base.Z);
			matrix[0, 2] = 2.0 * (base.X * base.Z + W * base.Y);
			matrix[0, 3] = 0.0;
			matrix[1, 0] = 2.0 * (base.X * base.Y + W * base.Z);
			matrix[1, 1] = 1.0 - 2.0 * (num + num3);
			matrix[1, 2] = 2.0 * (base.Y * base.Z - W * base.X);
			matrix[1, 3] = 0.0;
			matrix[2, 0] = 2.0 * (base.X * base.Z - W * base.Y);
			matrix[2, 1] = 2.0 * (base.Y * base.Z + W * base.X);
			matrix[2, 2] = 1.0 - 2.0 * (num + num2);
			matrix[2, 3] = 0.0;
			matrix[3, 0] = 0.0;
			matrix[3, 1] = 0.0;
			matrix[3, 2] = 0.0;
			matrix[3, 3] = 1.0;
			return matrix;
		}
	}

	internal Quaternion(QuaternionParameterType p, double x, double y, double z, double w)
	{
		switch (p)
		{
		case QuaternionParameterType.ImaginaryReal:
			base.X = x;
			base.Y = y;
			base.Z = z;
			W = w;
			break;
		case QuaternionParameterType.UnitAxisAngle:
			SetFromAxisAngle(new Vector3(x, y, z), w);
			break;
		}
	}

	internal Quaternion(QuaternionParameterType p, Vector3 v, double a)
	{
		switch (p)
		{
		case QuaternionParameterType.ImaginaryReal:
			base.X = v.X;
			base.Y = v.Y;
			base.Z = v.Z;
			W = a;
			break;
		case QuaternionParameterType.UnitAxisAngle:
			SetFromAxisAngle(v, a);
			break;
		}
	}

	public Quaternion()
	{
	}

	public bool IsComplex(double z)
	{
		return Math.Pow(base.X + base.Y + base.Z + W, 2.0) - 4.0 * (Norm - z) < 0.0;
	}

	public double GetMovePlus(double z)
	{
		double num = (-2.0 * (base.X + base.Y + base.Z + W) + 2.0 * Math.Sqrt(Math.Pow(base.X + base.Y + base.Z + W, 2.0) - 4.0 * (Norm - z))) / 8.0;
		double num2 = (-2.0 * (base.X + base.Y + base.Z + W) - 2.0 * Math.Sqrt(Math.Pow(base.X + base.Y + base.Z + W, 2.0) - 4.0 * (Norm - z))) / 8.0;
		if (num < num2)
		{
			return num2;
		}
		return num;
	}

	public double GetMoveMinus(double z)
	{
		double num = (-2.0 * (base.X + base.Y + base.Z + W) + 2.0 * Math.Sqrt(Math.Pow(base.X + base.Y + base.Z + W, 2.0) - 4.0 * (Norm - z))) / 8.0;
		double num2 = (-2.0 * (base.X + base.Y + base.Z + W) - 2.0 * Math.Sqrt(Math.Pow(base.X + base.Y + base.Z + W, 2.0) - 4.0 * (Norm - z))) / 8.0;
		if (num > num2)
		{
			return num2;
		}
		return num;
	}

	public static Quaternion operator !(Quaternion q)
	{
		return q.GetInverse();
	}

	public Quaternion GetInverse()
	{
		return Conjugate * (1.0 / Norm);
	}

	public static Quaternion operator *(Quaternion q2, Quaternion q1)
	{
		return new Quaternion(QuaternionParameterType.ImaginaryReal, (q1.Imaginary | q2.Imaginary) + q2.W * q1.Imaginary + q1.W * q2.Imaginary, q1.W * q2.W - (q1.Imaginary & q2.Imaginary));
	}

	public static double operator &(Quaternion q1, Quaternion q2)
	{
		return q1.W * q2.W + (q1.Imaginary & q2.Imaginary);
	}

	public static Quaternion operator |(Quaternion q1, Quaternion q2)
	{
		return new Quaternion(QuaternionParameterType.ImaginaryReal, q2.Imaginary | q1.Imaginary, 0.0);
	}

	public static Quaternion operator *(Quaternion q1, double d)
	{
		return new Quaternion(QuaternionParameterType.ImaginaryReal, q1.Imaginary * d, q1.W * d);
	}

	public static Quaternion operator *(double d, Quaternion q1)
	{
		return q1 * d;
	}

	public static Quaternion operator +(Quaternion q1, Quaternion q2)
	{
		return new Quaternion(QuaternionParameterType.ImaginaryReal, q1.Imaginary + q2.Imaginary, q1.W + q2.W);
	}

	public static double RadToDeg(double rad)
	{
		return rad * 180.0 / Math.PI;
	}

	public static double DegToRad(double deg)
	{
		return deg * Math.PI / 180.0;
	}

	public void MakeUnitQuaternion()
	{
		double length = Length;
		if (length != 0.0)
		{
			base.X /= length;
			base.Y /= length;
			base.Z /= length;
			W /= length;
		}
	}

	private bool IsNear(double val, double near, double delta)
	{
		return Math.Abs(Math.Abs(val) - near) < delta;
	}

	private double MakeRobustAngle(double rad)
	{
		return rad;
	}

	private void MakeRobust()
	{
	}

	private void DoMakeRobust()
	{
	}

	public void SetFromAxisAngle(Vector3 axis, double a)
	{
		axis.Normalize();
		double num = Math.Sin(a / 2.0);
		base.X = axis.X * num;
		base.Y = axis.Y * num;
		base.Z = axis.Z * num;
		W = Math.Cos(a / 2.0);
		MakeRobust();
		MakeUnitQuaternion();
	}

	public Vector3 GetEulerAngles()
	{
		Quaternion quaternion = Clone();
		quaternion.DoMakeRobust();
		Vector3 eulerAnglesZYX = quaternion.GetEulerAnglesZYX();
		eulerAnglesZYX.X = MakeRobustAngle(eulerAnglesZYX.X);
		eulerAnglesZYX.Y = MakeRobustAngle(eulerAnglesZYX.Y);
		eulerAnglesZYX.Z = MakeRobustAngle(eulerAnglesZYX.Z);
		return eulerAnglesZYX;
	}

	protected double Clip1(double d)
	{
		if (d < -1.0)
		{
			return -1.0;
		}
		if (d > 1.0)
		{
			return 1.0;
		}
		return d;
	}

	public Vector3 GetEulerAnglesYXZ()
	{
		Matrix matrix = Matrix;
		Vector3 vector = new Vector3(0.0, 0.0, 0.0);
		vector.X = Math.Asin(0.0 - Clip1(matrix[1, 2]));
		if (vector.X < Math.PI / 2.0)
		{
			if (vector.X > -Math.PI / 2.0)
			{
				vector.Y = (float)Math.Atan2(Clip1(matrix[0, 2]), Clip1(matrix[2, 2]));
				vector.Z = (float)Math.Atan2(Clip1(matrix[1, 0]), Clip1(matrix[1, 1]));
			}
			else
			{
				vector.Y = (float)(-1.0 * Math.Atan2(0.0 - Clip1(matrix[0, 1]), Clip1(matrix[0, 0])));
			}
		}
		else
		{
			vector.Y = (float)Math.Atan2(0.0 - Clip1(matrix[0, 1]), Clip1(matrix[0, 0]));
		}
		return vector;
	}

	public Vector3 GetEulerAnglesZXY()
	{
		Matrix matrix = Matrix;
		Vector3 vector = new Vector3(0.0, 0.0, 0.0);
		vector.X = Math.Asin(matrix[2, 1]);
		if (vector.X < Math.PI / 2.0)
		{
			if (vector.X > -Math.PI / 2.0)
			{
				vector.Z = (float)Math.Atan2(0.0 - matrix[0, 1], matrix[1, 1]);
				vector.Y = (float)Math.Atan2(0.0 - matrix[2, 0], matrix[2, 2]);
			}
			else
			{
				vector.Z = (float)(-1.0 * Math.Atan2(0.0 - matrix[0, 2], matrix[0, 0]));
			}
		}
		else
		{
			vector.Z = (float)Math.Atan2(matrix[0, 2], matrix[0, 0]);
		}
		return vector;
	}

	public Vector3 GetEulerAnglesZYX()
	{
		Matrix matrix = Matrix;
		Vector3 vector = new Vector3(0.0, 0.0, 0.0);
		vector.Y = Math.Asin(0.0 - matrix[2, 0]);
		if (vector.Y < Math.PI / 2.0)
		{
			if (vector.Y > -Math.PI / 2.0)
			{
				vector.Z = (float)Math.Atan2(matrix[1, 0], matrix[0, 0]);
				vector.X = (float)Math.Atan2(matrix[2, 1], matrix[2, 2]);
			}
			else
			{
				vector.Z = (float)(-1.0 * Math.Atan2(0.0 - matrix[0, 1], matrix[0, 2]));
			}
		}
		else
		{
			vector.Z = (float)Math.Atan2(0.0 - matrix[0, 1], matrix[0, 2]);
		}
		return vector;
	}

	public static Quaternion FromRotationMatrix(Matrix r)
	{
		double num = 0.0;
		double num2 = 0.0;
		double num3 = 0.0;
		double num4 = 0.0;
		double trace = r.Trace;
		if (trace > 0.0)
		{
			num4 = Math.Sqrt(trace) / 2.0;
			num = (r[2, 1] - r[1, 2]) / (4.0 * num4);
			num2 = (r[0, 2] - r[2, 0]) / (4.0 * num4);
			num3 = (r[1, 0] - r[0, 1]) / (4.0 * num4);
		}
		else if (r[0, 0] >= r[1, 1] && r[0, 0] >= r[2, 2])
		{
			num = Math.Sqrt(r[0, 0] - r[1, 1] - r[2, 2] + 1.0) / 2.0;
			num4 = (r[1, 2] - r[2, 1]) / (4.0 * num);
			num2 = (r[0, 1] - r[1, 0]) / (4.0 * num);
			num3 = (r[2, 0] - r[0, 2]) / (4.0 * num);
		}
		else if (r[1, 1] >= r[0, 0] && r[1, 1] >= r[2, 2])
		{
			num2 = Math.Sqrt(r[1, 1] - r[0, 0] - r[2, 2] + 1.0) / 2.0;
			num4 = (r[2, 0] - r[0, 2]) / (4.0 * num2);
			num = (r[0, 1] - r[1, 0]) / (4.0 * num2);
			num3 = (r[1, 2] - r[2, 1]) / (4.0 * num2);
		}
		else if (r[2, 2] >= r[0, 0] && r[2, 2] >= r[1, 1])
		{
			num3 = Math.Sqrt(r[2, 2] - r[0, 0] - r[1, 1] + 1.0) / 2.0;
			num4 = (r[0, 1] - r[1, 0]) / (4.0 * num3);
			num = (r[2, 0] - r[0, 2]) / (4.0 * num3);
			num2 = (r[1, 2] - r[2, 1]) / (4.0 * num3);
		}
		Quaternion quaternion = FromImaginaryReal(num, num2, num3, num4);
		quaternion.MakeRobust();
		quaternion.MakeUnitQuaternion();
		return quaternion;
	}

	public static Quaternion FromEulerAngles(Vector3 ea)
	{
		return FromRotationMatrix(Matrix.RotateZ(ea.Z) * Matrix.RotateY(ea.Y) * Matrix.RotateX(ea.X));
	}

	public static Quaternion FromEulerAngles(double yaw, double pitch, double roll)
	{
		return FromEulerAngles(new Vector3(pitch, yaw, roll));
	}

	public static Quaternion FromAxisAngle(Vector3 v, double angle)
	{
		v.Normalize();
		return new Quaternion(QuaternionParameterType.UnitAxisAngle, v.X, v.Y, v.Z, angle);
	}

	public static Quaternion FromAxisAngle(double x, double y, double z, double angle)
	{
		return new Quaternion(QuaternionParameterType.UnitAxisAngle, x, y, z, angle);
	}

	public static Quaternion FromImaginaryReal(Vector3 v, double w)
	{
		return new Quaternion(QuaternionParameterType.ImaginaryReal, v.X, v.Y, v.Z, w);
	}

	public static Quaternion FromImaginaryReal(Vector4 v)
	{
		return new Quaternion(QuaternionParameterType.ImaginaryReal, v.X, v.Y, v.Z, v.W);
	}

	public static Quaternion FromImaginaryReal(double x, double y, double z, double w)
	{
		return new Quaternion(QuaternionParameterType.ImaginaryReal, x, y, z, w);
	}

	private double NormalizeRad(double rad)
	{
		while (rad > Math.PI)
		{
			rad -= Math.PI * 2.0;
		}
		while (rad < -Math.PI)
		{
			rad += Math.PI * 2.0;
		}
		return rad;
	}

	public string ToLinedString()
	{
		string text = "";
		text = text + "X: " + base.X + "\n";
		text = text + "Y: " + base.Y + "\n";
		text = text + "Z: " + base.Z + "\n";
		text = text + "W: " + W + "\n";
		text += "-----\n";
		text = text + "X: " + Axis.X + "\n";
		text = text + "Y: " + Axis.Y + "\n";
		text = text + "Z: " + Axis.Z + "\n";
		object obj = text;
		text = string.Concat(obj, "A: ", RadToDeg(Angle), "\n");
		text += "-----\n";
		obj = text;
		text = string.Concat(obj, "Y: ", RadToDeg(GetEulerAngles().Y), "\n");
		obj = text;
		text = string.Concat(obj, "P: ", RadToDeg(GetEulerAngles().X), "\n");
		obj = text;
		return string.Concat(obj, "R: ", RadToDeg(GetEulerAngles().Z), "\n");
	}

	public override string ToString()
	{
		return base.ToString() + " (X=" + Axis.X.ToString("N2") + ", Y=" + Axis.Y.ToString("N2") + ", Z=" + Axis.Z.ToString("N2") + ", a=" + RadToDeg(Angle).ToString("N1") + "    euler=y:" + RadToDeg(GetEulerAngles().Y).ToString("N1") + "; p:" + RadToDeg(GetEulerAngles().X).ToString("N1") + "; r:" + RadToDeg(GetEulerAngles().Z).ToString("N1") + ")";
	}

	public Vector3 Rotate(Vector3 v)
	{
		Quaternion quaternion = new Quaternion(QuaternionParameterType.ImaginaryReal, v.X, v.Y, v.Z, 0.0);
		quaternion = this * quaternion * Conjugate;
		return new Vector3(quaternion.X, quaternion.Y, quaternion.Z);
	}

	public new Quaternion Clone()
	{
		return new Quaternion(QuaternionParameterType.ImaginaryReal, base.X, base.Y, base.Z, W);
	}

	private void LoadCorrection()
	{
	}
}
