/***************************************************************************
 *   Copyright (C) 2006-2007 by JFade / DJS Sims                            *
 *   (The Sims Programming Group)                                           *
 *                                                                          *
 *   Originally written in VB.NET for Sims 2 Collection Creator.            *
 *   Decompiled with ILSpy 2026-06-26 and included in this repository as    *
 *   reference for the SimPE Tool-plugin port.                              *
 *                                                                          *
 *   Used by permission of the original author (granted 2026-06-26).        *
 *   Reference only — not part of the SimPE-Fixed build, not relicensed.    *
 ***************************************************************************/
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
