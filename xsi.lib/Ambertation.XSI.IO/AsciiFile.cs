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
using System.Collections;
using System.IO;
using Ambertation.Scenes;
using Ambertation.XSI.Template;

namespace Ambertation.XSI.IO;

public sealed class AsciiFile : File, IConvertToScene
{
	private string flname;

	private TokenParser tp;

	public override string FileName
	{
		get
		{
			if (flname == null)
			{
				return "";
			}
			return flname;
		}
	}

	public override Container Root => tp;

	public int Version => header.Version;

	public bool Valid => header.Valid;

	private AsciiFile()
		: this("memory.xsi")
	{
	}

	internal AsciiFile(string filename)
	{
		flname = filename;
		header = new AsciiHeader();
		tp = new TokenParser(this, new string[2] { "", "" });
	}

	private AsciiFile(StreamReader sr, string filename)
		: this(filename)
	{
		sr.BaseStream.Seek(0L, SeekOrigin.Begin);
		header.DeSerialize(sr);
		ArrayList arrayList = new ArrayList { "" };
		if (header.Valid)
		{
			try
			{
				while (true)
				{
					arrayList.Add(sr.ReadLine().Trim());
				}
			}
			catch
			{
			}
		}
		arrayList.Add("");
		string[] array = new string[arrayList.Count];
		arrayList.CopyTo(array);
		tp = new TokenParser(this, array);
	}

	public override string ToString()
	{
		string text = "[" + GetType().Name + "] " + flname;
		if (!Valid)
		{
			return text + " (not valid)";
		}
		return text + " (valid)";
	}

	public static AsciiFile FromFile(string flname)
	{
		if (!System.IO.File.Exists(flname))
		{
			return null;
		}
		try
		{
			StreamReader streamReader = new StreamReader(System.IO.File.OpenRead(flname));
			try
			{
				return FromStream(streamReader, flname);
			}
			finally
			{
				streamReader.Close();
			}
		}
		catch (Exception ex)
		{
			Console.WriteLine(ex.Message + "\n" + ex.StackTrace);
		}
		return null;
	}

	public static AsciiFile FromStream(StreamReader sr, string usedflname)
	{
		if (!sr.BaseStream.CanRead || !sr.BaseStream.CanSeek)
		{
			return null;
		}
		try
		{
			AsciiFile asciiFile = new AsciiFile(sr, usedflname);
			if (!asciiFile.Valid)
			{
				return null;
			}
			return asciiFile;
		}
		catch (Exception ex)
		{
			Console.WriteLine(ex.Message + "\n" + ex.StackTrace);
		}
		return null;
	}

	public void Save()
	{
		Save(flname);
	}

	public void Save(string flname)
	{
		try
		{
			StreamWriter streamWriter = System.IO.File.CreateText(flname);
			try
			{
				SaveToStream(streamWriter);
			}
			finally
			{
				streamWriter.Close();
			}
		}
		catch (Exception ex)
		{
			Console.WriteLine(ex.Message + "\n" + ex.StackTrace);
		}
	}

	public Stream SaveToStream()
	{
		try
		{
			StreamWriter streamWriter = new StreamWriter(new MemoryStream());
			SaveToStream(streamWriter);
			streamWriter.BaseStream.Seek(0L, SeekOrigin.Begin);
			return streamWriter.BaseStream;
		}
		catch (Exception ex)
		{
			Console.WriteLine(ex.Message + "\n" + ex.StackTrace);
		}
		return null;
	}

	public void SaveToStream(StreamWriter sw)
	{
		header.Serialize(sw);
		tp.Serialize(sw, "");
	}

	public static AsciiFile FromScene(Ambertation.Scenes.Scene scn, string flname)
	{
		SceneToXsi sceneToXsi = new SceneToXsi(scn);
		AsciiFile asciiFile = sceneToXsi.ConvertToXsi();
		asciiFile.flname = flname;
		return asciiFile;
	}

	public Ambertation.Scenes.Scene ToScene()
	{
		meshstack.Clear();
		Ambertation.Scenes.Scene scene = new Ambertation.Scenes.Scene();
		Root.ToScene(scene);
		return scene;
	}
}
