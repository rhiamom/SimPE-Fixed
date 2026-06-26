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
using System.Collections;
using System.IO;

namespace CCB;

public class IOTools
{
	public static string[] GetFiles(string path)
	{
		ArrayList files = new ArrayList();
		getFiles(path, ref files);
		return (string[])files.ToArray(typeof(string));
	}

	private static void getFiles(string path, ref ArrayList files)
	{
		try
		{
			string[] directories = Directory.GetDirectories(path);
			for (int i = 0; i < directories.Length; i++)
			{
				getFiles(directories[i], ref files);
			}
			string[] files2 = Directory.GetFiles(path);
			files.AddRange(files2);
		}
		catch
		{
		}
	}

	public static string[] GetFiles(string path, string[] searchPatterns, bool includeSubFolders)
	{
		ArrayList files = new ArrayList();
		if (includeSubFolders)
		{
			getFiles(path, searchPatterns, ref files);
		}
		else
		{
			try
			{
				for (int i = 0; i < searchPatterns.Length; i++)
				{
					string[] files2 = Directory.GetFiles(path, searchPatterns[i]);
					files.AddRange(files2);
				}
			}
			catch
			{
			}
		}
		return (string[])files.ToArray(typeof(string));
	}

	public static string[] GetFiles(string path, string searchPattern, bool includeSubFolders)
	{
		string[] searchPatterns = searchPattern.Split(new char[1] { ';' });
		return GetFiles(path, searchPatterns, includeSubFolders);
	}

	private static void getFiles(string path, string[] searchPattern, ref ArrayList files)
	{
		try
		{
			string[] directories = Directory.GetDirectories(path);
			for (int i = 0; i < directories.Length; i++)
			{
				getFiles(directories[i], searchPattern, ref files);
			}
			for (int j = 0; j < searchPattern.Length; j++)
			{
				string[] files2 = Directory.GetFiles(path, searchPattern[j]);
				files.AddRange(files2);
			}
		}
		catch
		{
		}
	}

	public static void killFiles(string path, string searchPattern)
	{
		string[] files = Directory.GetFiles(path, searchPattern);
		for (int i = 0; i < files.Length; i++)
		{
			File.Delete(files[i]);
		}
	}

	public static string[] GetsFilesNoDir(string path, string searchPattern)
	{
		string[] files = Directory.GetFiles(path, searchPattern);
		for (int i = 0; i < files.Length; i++)
		{
			files[i] = files[i].Remove(0, path.Length);
		}
		return files;
	}

	public static long FileSize(string FilePath)
	{
		FileInfo fileInfo = new FileInfo(FilePath);
		return fileInfo.Length;
	}
}
