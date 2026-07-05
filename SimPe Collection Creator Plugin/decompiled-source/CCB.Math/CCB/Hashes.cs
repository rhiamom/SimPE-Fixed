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
using System.Text;
using Classless.Hasher;

namespace CCB;

public class Hashes
{
	public const uint CRC24Seed = 11994318u;

	public const uint CRC24Poly = 25578747u;

	public const uint CRC32Seed = 11994318u;

	public const uint CRC32Poly = 25578747u;

	private static CRC crc24 = new CRC(CRCParameters.GetParameters(CRCStandard.CRC24));

	private static CRC crc32 = new CRC(new CRCParameters(32, 79764919L, 4294967295L, 0L, reflectIn: false));

	public static CRC Crc24 => crc24;

	public static CRC Crc32 => crc32;

	public static uint GetCrc32(string s)
	{
		byte[] input = crc32.ComputeHash(ToBytes(s, 0));
		return (uint)ToLong(input);
	}

	public static uint GetCrc24(string s)
	{
		byte[] input = crc24.ComputeHash(ToBytes(s, 0));
		return (uint)ToLong(input);
	}

	public static byte[] ToBytes(string str)
	{
		return ToBytes(str, 0);
	}

	public static byte[] ToBytes(string str, int len)
	{
		byte[] array = null;
		if (len != 0)
		{
			array = new byte[len];
			Encoding.ASCII.GetBytes(str, 0, System.Math.Min(len, str.Length), array, 0);
		}
		else
		{
			array = Encoding.ASCII.GetBytes(str);
		}
		return array;
	}

	public static ulong ToLong(byte[] input)
	{
		ulong num = 0uL;
		foreach (byte b in input)
		{
			num <<= 8;
			num += b;
		}
		return num;
	}
}
