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
using System.IO;
using System.Text;

namespace CCB;

public class IO
{
	public static string[] FetchResourceArray(string PackagePath)
	{
		string text = "";
		string text2 = "";
		string text3 = "";
		string text4 = "";
		string text5 = "";
		string text6 = "";
		long num = 0L;
		if (FetchResourceCount(PackagePath) == 0)
		{
			return new string[1] { "Error" };
		}
		string[] array = new string[FetchResourceCount(PackagePath)];
		long position = FetchIndexOffset(PackagePath);
		long num2 = FetchIndexSize(PackagePath);
		long num3 = ((num2 / array.Length != 24) ? 1 : 2);
		FileStream fileStream = new FileStream(PackagePath, FileMode.Open, FileAccess.ReadWrite);
		BinaryReader binaryReader = new BinaryReader(fileStream);
		fileStream.Position = position;
		for (; num != array.Length; num++)
		{
			byte[] array2 = binaryReader.ReadBytes(4);
			Array.Reverse((Array)array2);
			text = getBytesAsHexString(array2, bSpaced: false);
			array2 = binaryReader.ReadBytes(4);
			Array.Reverse((Array)array2);
			text2 = getBytesAsHexString(array2, bSpaced: false);
			array2 = binaryReader.ReadBytes(4);
			Array.Reverse((Array)array2);
			text3 = getBytesAsHexString(array2, bSpaced: false);
			if (num3 == 2)
			{
				array2 = binaryReader.ReadBytes(4);
				Array.Reverse((Array)array2);
				text4 = getBytesAsHexString(array2, bSpaced: false);
			}
			array2 = binaryReader.ReadBytes(4);
			Array.Reverse((Array)array2);
			text5 = getBytesAsHexString(array2, bSpaced: false);
			array2 = binaryReader.ReadBytes(4);
			Array.Reverse((Array)array2);
			text6 = getBytesAsHexString(array2, bSpaced: false);
			if (num3 == 2)
			{
				array[num] = text + "," + text2 + "," + text3 + "," + text4 + "," + text5 + "," + text6;
			}
			else
			{
				array[num] = text + "," + text2 + "," + text3 + "," + text5 + "," + text6;
			}
		}
		binaryReader.Close();
		fileStream.Close();
		return array;
	}

	public static long FetchResourceCount(string PackagePath)
	{
		FileStream fileStream = new FileStream(PackagePath, FileMode.Open, FileAccess.ReadWrite);
		BinaryReader binaryReader = new BinaryReader(fileStream);
		fileStream.Position = 36L;
		byte[] array = new byte[4];
		array = binaryReader.ReadBytes(4);
		Array.Reverse((Array)array);
		long result = Math.ToDec(getBytesAsHexString(array, bSpaced: false));
		binaryReader.Close();
		fileStream.Close();
		return result;
	}

	public static long FetchIndexOffset(string PackagePath)
	{
		FileStream fileStream = new FileStream(PackagePath, FileMode.Open, FileAccess.ReadWrite);
		BinaryReader binaryReader = new BinaryReader(fileStream);
		fileStream.Position = 40L;
		byte[] array = new byte[4];
		array = binaryReader.ReadBytes(4);
		Array.Reverse((Array)array);
		long result = Math.ToDec(getBytesAsHexString(array, bSpaced: false));
		binaryReader.Close();
		fileStream.Close();
		return result;
	}

	public static long FetchIndexSize(string PackagePath)
	{
		FileStream fileStream = new FileStream(PackagePath, FileMode.Open, FileAccess.ReadWrite);
		BinaryReader binaryReader = new BinaryReader(fileStream);
		fileStream.Position = 44L;
		byte[] array = new byte[4];
		array = binaryReader.ReadBytes(4);
		Array.Reverse((Array)array);
		long result = Math.ToDec(getBytesAsHexString(array, bSpaced: false));
		binaryReader.Close();
		fileStream.Close();
		return result;
	}

	public static string DumpResourceData(string PackagePath, int ResourceOffset, int ResourceNumber, int ResourceSize, string ResourceType, string Path)
	{
		FileStream fileStream = new FileStream(PackagePath, FileMode.Open, FileAccess.ReadWrite);
		BinaryReader binaryReader = new BinaryReader(fileStream);
		fileStream.Position = ResourceOffset;
		byte[] buffer = binaryReader.ReadBytes(ResourceSize);
		FileStream fileStream2 = new FileStream(Path + "/" + ResourceType + ResourceNumber + ".dat", FileMode.OpenOrCreate, FileAccess.ReadWrite);
		BinaryWriter binaryWriter = new BinaryWriter(fileStream2);
		binaryWriter.Write(buffer);
		binaryWriter.Close();
		fileStream2.Close();
		binaryReader.Close();
		fileStream.Close();
		return ResourceType + ResourceNumber + ".dat";
	}

	public static string Decompress(string ResourceName, int ResourceNumber, string ProgramPath)
	{
		FileStream fileStream = new FileStream(ProgramPath + "/" + ResourceName, FileMode.Open, FileAccess.ReadWrite);
		BinaryReader binaryReader = new BinaryReader(fileStream);
		byte[] array = new byte[4];
		array = binaryReader.ReadBytes(4);
		Array.Reverse((Array)array);
		long num = Math.ToDec(getBytesAsHexString(array, bSpaced: false));
		fileStream.Position = 6L;
		array = new byte[3];
		array = binaryReader.ReadBytes(3);
		long num2 = Math.ToDec(getBytesAsHexString(array, bSpaced: false));
		if (num != fileStream.Length)
		{
			fileStream.Close();
			binaryReader.Close();
			return ResourceName;
		}
		FileStream fileStream2 = new FileStream(ProgramPath + "/" + ResourceNumber + ResourceName, FileMode.OpenOrCreate, FileAccess.ReadWrite);
		BinaryWriter binaryWriter = new BinaryWriter(fileStream2);
		BinaryReader binaryReader2 = new BinaryReader(fileStream2);
		int num3 = 9;
		int num4 = 0;
		int num5 = (int)fileStream.Length;
		while (num4 != num2)
		{
			fileStream.Position = num3;
			byte b = binaryReader.ReadByte();
			num3++;
			if (num3 > num5 || num5 == num2)
			{
				break;
			}
			int num6;
			if (b >= 224 && b <= byte.MaxValue)
			{
				num6 = (b & 0x1F) * 4 + 4;
				num6 += num3;
				while (num3 != num6 && num4 != num2)
				{
					binaryReader.BaseStream.Position = num3;
					byte value = binaryReader.ReadByte();
					binaryWriter.BaseStream.Position = num4;
					binaryWriter.Write(value);
					num3++;
					num4++;
				}
				continue;
			}
			byte b2;
			int num8;
			int num7;
			if (b >= 0 && b <= 127)
			{
				binaryReader.BaseStream.Position = num3;
				b2 = binaryReader.ReadByte();
				num3++;
				num6 = b & 3;
				num7 = (b & 0x1C) / 4 + 3;
				num8 = (b & 0x60) * 8 + b2 + 1;
				num8 = num4 - num8 + num6;
				num7 += num8;
				num6 += num3;
				while (num3 != num6 && num5 != num2)
				{
					binaryReader.BaseStream.Position = num3;
					byte value = binaryReader.ReadByte();
					binaryWriter.BaseStream.Position = num4;
					binaryWriter.Write(value);
					num3++;
					num4++;
				}
				while (num8 != num7 && num5 != num2)
				{
					binaryReader2.BaseStream.Position = num8;
					byte value = binaryReader2.ReadByte();
					binaryWriter.BaseStream.Position = num4;
					binaryWriter.Write(value);
					num8++;
					num4++;
				}
				continue;
			}
			byte b3;
			if (b >= 128 && b <= 191)
			{
				binaryReader.BaseStream.Position = num3;
				b2 = binaryReader.ReadByte();
				num3++;
				binaryReader.BaseStream.Position = num3;
				b3 = binaryReader.ReadByte();
				num3++;
				num6 = ((b2 & 0xC0) / 64) & 3;
				num7 = (b & 0x3F) + 4;
				num8 = (b2 & 0x3F) * 256 + b3 + 1;
				num8 = num4 - num8 + num6;
				num7 += num8;
				num6 += num3;
				while (num3 != num6 && num5 != num2)
				{
					binaryReader.BaseStream.Position = num3;
					byte value = binaryReader.ReadByte();
					binaryWriter.BaseStream.Position = num4;
					binaryWriter.Write(value);
					num3++;
					num4++;
				}
				while (num8 != num7 && num5 != num2)
				{
					binaryReader2.BaseStream.Position = num8;
					byte value = binaryReader2.ReadByte();
					binaryWriter.BaseStream.Position = num4;
					binaryWriter.Write(value);
					num8++;
					num4++;
				}
				continue;
			}
			binaryReader.BaseStream.Position = num3;
			b2 = binaryReader.ReadByte();
			num3++;
			binaryReader.BaseStream.Position = num3;
			b3 = binaryReader.ReadByte();
			num3++;
			binaryReader.BaseStream.Position = num3;
			byte b4 = binaryReader.ReadByte();
			num3++;
			num6 = b & 3;
			num7 = (b & 0xC) * 64 + b4 + 5;
			num8 = (b & 0x10) * 4096 + b2 * 256 + b3 + 1;
			num8 = num4 - num8 + num6;
			num7 += num8;
			num6 += num3;
			while (num3 != num6 && num5 != num2)
			{
				binaryReader.BaseStream.Position = num3;
				byte value = binaryReader.ReadByte();
				binaryWriter.BaseStream.Position = num4;
				binaryWriter.Write(value);
				num3++;
				num4++;
			}
			while (num8 != num7 && num5 != num2)
			{
				binaryReader2.BaseStream.Position = num8;
				byte value = binaryReader2.ReadByte();
				binaryWriter.BaseStream.Position = num4;
				binaryWriter.Write(value);
				num8++;
				num4++;
			}
		}
		binaryWriter.Close();
		binaryReader2.Close();
		fileStream2.Close();
		binaryReader.Close();
		fileStream.Close();
		return ResourceNumber + ResourceName;
	}

	public static void AppendDats(string PackagePath, string ResourcePath, int ResourceSize)
	{
		FileStream fileStream = new FileStream(ResourcePath, FileMode.Open, FileAccess.ReadWrite);
		BinaryReader binaryReader = new BinaryReader(fileStream);
		byte[] buffer = binaryReader.ReadBytes(ResourceSize);
		FileStream fileStream2 = new FileStream(PackagePath, FileMode.Append, FileAccess.Write);
		BinaryWriter binaryWriter = new BinaryWriter(fileStream2);
		binaryWriter.Write(buffer);
		binaryWriter.Close();
		fileStream2.Close();
		binaryReader.Close();
		fileStream.Close();
	}

	public static string getBytesAsHexString(byte[] byBytes, bool bSpaced)
	{
		char[] array = new char[16]
		{
			'0', '1', '2', '3', '4', '5', '6', '7', '8', '9',
			'A', 'B', 'C', 'D', 'E', 'F'
		};
		int num = byBytes.Length * 2;
		if (bSpaced)
		{
			num = byBytes.Length * 3;
		}
		char[] array2 = new char[num];
		for (int i = 0; i < byBytes.Length; i++)
		{
			int num2 = byBytes[i];
			if (bSpaced)
			{
				array2[i * 3] = array[num2 >> 4];
				array2[i * 3 + 1] = array[num2 & 0xF];
				array2[i * 3 + 2] = ' ';
			}
			else
			{
				array2[i * 2] = array[num2 >> 4];
				array2[i * 2 + 1] = array[num2 & 0xF];
			}
		}
		return new string(array2);
	}

	public static string getBytesAsCharString(byte[] byBytes)
	{
		int num = byBytes.Length;
		char[] array = new char[num];
		for (int i = 0; i < byBytes.Length; i++)
		{
			int num2 = byBytes[i];
			array[i] = (char)num2;
		}
		return new string(array);
	}

	public static string CreateDBPFHeader(string Path, int ResourceCount, int IndexOffset)
	{
		FileStream fileStream = new FileStream(Path, FileMode.OpenOrCreate, FileAccess.ReadWrite);
		BinaryWriter binaryWriter = new BinaryWriter(fileStream);
		byte[] buffer = MakeByteArray("DBPF", 0, Reverse: false, FromChars: true);
		binaryWriter.Write(buffer);
		buffer = MakeByteArray(Math.ToHex("1"), 4, Reverse: true, FromChars: false);
		binaryWriter.Write(buffer);
		buffer = MakeByteArray(Math.ToHex("1"), 4, Reverse: true, FromChars: false);
		binaryWriter.Write(buffer);
		buffer = MakeByteArray(Math.ToHex("0"), 20, Reverse: true, FromChars: false);
		binaryWriter.Write(buffer);
		buffer = MakeByteArray(Math.ToHex("7"), 4, Reverse: true, FromChars: false);
		binaryWriter.Write(buffer);
		buffer = MakeByteArray(Math.ToHex(ResourceCount.ToString()), 4, Reverse: true, FromChars: false);
		binaryWriter.Write(buffer);
		buffer = MakeByteArray(Math.ToHex(IndexOffset.ToString()), 4, Reverse: true, FromChars: false);
		binaryWriter.Write(buffer);
		buffer = MakeByteArray(Math.ToHex((ResourceCount * 24).ToString()), 4, Reverse: true, FromChars: false);
		binaryWriter.Write(buffer);
		buffer = MakeByteArray(Math.ToHex("0"), 12, Reverse: true, FromChars: false);
		binaryWriter.Write(buffer);
		buffer = MakeByteArray(Math.ToHex("2"), 4, Reverse: true, FromChars: false);
		binaryWriter.Write(buffer);
		buffer = MakeByteArray(Math.ToHex("0"), 32, Reverse: true, FromChars: false);
		binaryWriter.Write(buffer);
		binaryWriter.Close();
		fileStream.Close();
		return Path;
	}

	public static string CreateIndex(string[] IndexArray, string Path)
	{
		FileStream fileStream = new FileStream(Path, FileMode.OpenOrCreate, FileAccess.ReadWrite);
		BinaryWriter binaryWriter = new BinaryWriter(fileStream);
		for (int i = 0; i < IndexArray.Length; i++)
		{
			string[] array = IndexArray[i].Split(new char[1] { ',' });
			byte[] buffer = MakeByteArray(array[0], 0, Reverse: true, FromChars: false);
			binaryWriter.Write(buffer);
			buffer = MakeByteArray(array[1], 0, Reverse: true, FromChars: false);
			binaryWriter.Write(buffer);
			buffer = MakeByteArray(array[2], 0, Reverse: true, FromChars: false);
			binaryWriter.Write(buffer);
			buffer = MakeByteArray(array[3], 0, Reverse: true, FromChars: false);
			binaryWriter.Write(buffer);
			buffer = MakeByteArray(array[4], 4, Reverse: true, FromChars: false);
			binaryWriter.Write(buffer);
			buffer = MakeByteArray(array[5], 4, Reverse: true, FromChars: false);
			binaryWriter.Write(buffer);
		}
		binaryWriter.Close();
		fileStream.Close();
		return Path;
	}

	public static byte[] MakeByteArray(string ToConvert, int FillSize, bool Reverse, bool FromChars)
	{
		int length = ToConvert.Length;
		byte[] array = ((FillSize > 0) ? new byte[FillSize] : ((!FromChars) ? new byte[length / 2] : new byte[length]));
		for (int i = 0; i < array.Length; i++)
		{
			if (FromChars)
			{
				ASCIIEncoding aSCIIEncoding = new ASCIIEncoding();
				array = aSCIIEncoding.GetBytes(ToConvert);
				Array.Reverse((Array)array);
			}
			else if (FillSize > 0)
			{
				if (ToConvert.Length == i * 2 + 2)
				{
					array[i] = (byte)Math.ToDec(ToConvert.Substring(ToConvert.Length - (i * 2 + 2), 2));
				}
				else if (ToConvert.Length == i * 2 + 1)
				{
					array[i] = (byte)Math.ToDec(ToConvert.Substring(ToConvert.Length - (i * 2 + 1), 1));
				}
				else if (ToConvert.Length < i * 2 + 2)
				{
					array[i] = 0;
				}
				else
				{
					array[i] = (byte)Math.ToDec(ToConvert.Substring(ToConvert.Length - (i * 2 + 2), 2));
				}
			}
			else
			{
				array[i] = (byte)Math.ToDec(ToConvert.Substring(ToConvert.Length - (i * 2 + 2), 2));
			}
		}
		if (Reverse)
		{
			return array;
		}
		Array.Reverse((Array)array);
		return array;
	}
}
