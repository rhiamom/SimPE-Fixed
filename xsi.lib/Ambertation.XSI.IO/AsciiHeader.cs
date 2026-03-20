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

internal sealed class AsciiHeader : Header
{
	internal AsciiHeader()
	{
		format = Format.txt;
	}

	protected override bool DeSerializeHeader(StreamReader sr)
	{
		if (sr.BaseStream.Length < 24)
		{
			return false;
		}
		BinaryReader binaryReader = new BinaryReader(sr.BaseStream);
		magic = binaryReader.ReadInt32();
		if (magic != 543781752)
		{
			return false;
		}
		major = (short)Helpers.ToInt(Helpers.ToString(binaryReader.ReadBytes(2)));
		minor = (short)Helpers.ToInt(Helpers.ToString(binaryReader.ReadBytes(2)));
		format = ToFormat(Helpers.ToString(binaryReader.ReadBytes(4)));
		if (format == Format.com)
		{
			compression = ToCompression(Helpers.ToString(binaryReader.ReadBytes(4)));
		}
		else
		{
			compression = Compression.non;
		}
		floatsz = Helpers.ToInt(Helpers.ToString(binaryReader.ReadBytes(4)));
		if (format != Format.unk)
		{
			return major <= 3;
		}
		return false;
	}

	internal override void Serialize(StreamWriter sw)
	{
		BinaryWriter binaryWriter = new BinaryWriter(sw.BaseStream);
		binaryWriter.Write(543781752);
		sw.Write(Helpers.ForceLength(major.ToString(), 2, '0', front: true));
		sw.Write(Helpers.ForceLength(minor.ToString(), 2, '0', front: true));
		sw.Write(Helpers.ForceLength(format.ToString(), 4, ' ', front: false));
		if (format == Format.com)
		{
			sw.Write(Helpers.ForceLength(compression.ToString(), 4, ' ', front: false));
		}
		sw.Write(Helpers.ForceLength(32.ToString(), 4, '0', front: true));
		sw.WriteLine();
	}
}
