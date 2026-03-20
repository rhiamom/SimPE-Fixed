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
 
 using System.IO;

namespace Ambertation.XSI.IO;

public abstract class Header
{
	public enum Format
	{
		txt,
		bin,
		com,
		unk
	}

	public enum Compression
	{
		lzw,
		zip,
		non
	}

	public const int MAGIC = 543781752;

	public const short MAJOR = 3;

	public const short MINOR = 0;

	public const int FLOAT = 32;

	protected int magic;

	protected short major;

	protected short minor;

	protected int floatsz;

	protected Format format;

	protected Compression compression;

	private bool valid;

	public short MajorVersion
	{
		get
		{
			return major;
		}
		set
		{
			major = value;
		}
	}

	public short MinorVersion
	{
		get
		{
			return minor;
		}
		set
		{
			minor = value;
		}
	}

	public int Version => (major << 16) + minor;

	public bool Valid => valid;

	internal Header()
	{
		magic = 543781752;
		minor = 0;
		major = 3;
		floatsz = 32;
		format = Format.unk;
		compression = Compression.non;
		valid = true;
	}

	protected Format ToFormat(string s)
	{
		return s switch
		{
			"txt " => Format.txt, 
			"bin " => Format.bin, 
			"com " => Format.com, 
			_ => Format.unk, 
		};
	}

	protected Compression ToCompression(string s)
	{
		if (s == "zip ")
		{
			return Compression.zip;
		}
		if (s == "lzw ")
		{
			return Compression.lzw;
		}
		return Compression.non;
	}

	internal void DeSerialize(StreamReader sr)
	{
		valid = DeSerializeHeader(sr);
	}

	protected abstract bool DeSerializeHeader(StreamReader sr);

	internal abstract void Serialize(StreamWriter sw);
}
