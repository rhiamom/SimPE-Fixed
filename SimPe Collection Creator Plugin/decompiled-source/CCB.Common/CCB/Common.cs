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
using System.IO;

namespace CCB;

public class Common
{
	public static string Translate(string SearchValue, string FilePath, long ArrayPosToMatch, long ArrayPosToUse, char Seperator)
	{
		StreamReader streamReader = new StreamReader(FilePath, detectEncodingFromByteOrderMarks: true);
		string[] array = new string[GetLineCount(FilePath)];
		for (int i = 0; i < array.Length; i++)
		{
			array[i] = streamReader.ReadLine();
			string[] array2 = array[i].Split(new char[1] { Seperator });
			string text;
			try
			{
				text = array2[ArrayPosToMatch];
			}
			catch
			{
				text = "ErrorKludgeMush";
			}
			if (SearchValue == text)
			{
				streamReader.Close();
				try
				{
					return array2[ArrayPosToUse];
				}
				catch
				{
					return "Error In Pulling Data From Array";
				}
			}
		}
		streamReader.Close();
		return "No Match Found";
	}

	public static long GetLineCount(string FilePath)
	{
		long num = 0L;
		StreamReader streamReader = File.OpenText(FilePath);
		while (streamReader.ReadLine() != null)
		{
			num++;
		}
		streamReader.Close();
		return num;
	}

	public static void AppendToFile(string StringToAppend, string FilePath)
	{
		StreamWriter streamWriter = new StreamWriter(FilePath, append: true);
		streamWriter.WriteLine(StringToAppend);
		streamWriter.Close();
	}
}
