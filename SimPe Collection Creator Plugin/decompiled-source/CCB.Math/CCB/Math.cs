using System;
using System.Globalization;

namespace CCB;

public class Math
{
	public static int ToDec(string HexString)
	{
		return Convert.ToInt32(int.Parse(HexString, NumberStyles.HexNumber));
	}

	public static string ToHex(string DecimalString)
	{
		return int.Parse(DecimalString).ToString("x");
	}
}
