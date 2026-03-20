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
using System.IO;
using System.Xml.Serialization;

namespace Ambertation;

internal class Serializer
{
	public static void Serialize(object o, string flname)
	{
		Stream stream = File.Create(flname);
		try
		{
			Serialize(o, stream);
		}
		finally
		{
			stream.Close();
		}
	}

	public static object DeSerialize(Type t, string flname)
	{
		Stream stream = File.OpenRead(flname);
		try
		{
			return DeSerialize(t, stream);
		}
		finally
		{
			stream.Close();
		}
	}

	public static void Serialize(object o, Stream s)
	{
		XmlSerializer xmlSerializer = new XmlSerializer(o.GetType());
		xmlSerializer.Serialize(s, o);
	}

	public static object DeSerialize(Type t, Stream s)
	{
		XmlSerializer xmlSerializer = new XmlSerializer(t);
		return xmlSerializer.Deserialize(s);
	}
}
