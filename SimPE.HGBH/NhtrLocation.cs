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
using SimPe.Geometry;

namespace SimPe.Plugin
{
	/// <summary>
	/// Summary description for NhtrLocation.
	/// </summary>
	[System.ComponentModel.TypeConverter(typeof(System.ComponentModel.ExpandableObjectConverter))]
	public class NhtrLocation
	{
		Vector3f pos;
		float o1, o2;

		public NhtrLocation()
		{
			pos = Vector3f.Zero;
		}

		public Vector3f Position
		{
			get {return pos;}
			set {pos = value;}
		}

		public float Orientation1
		{
			get { return o1;}
			set { o1 = value;}
		}

		public float Orientation2
		{
			get { return o2;}
			set { o2 = value;}
		}

		internal void Unserialize(System.IO.BinaryReader reader)
		{						
			pos.Y = reader.ReadSingle();
			pos.X = reader.ReadSingle();
			pos.Z = reader.ReadSingle();

			o1 = reader.ReadSingle();
			o2 = reader.ReadSingle();
		}

		internal void Serialize(System.IO.BinaryWriter writer) 
		{		
			writer.Write((float)pos.Y);
			writer.Write((float)pos.X);
			writer.Write((float)pos.Z);

			writer.Write(o1);
			writer.Write(o2);
		}

		public override string ToString()
		{
			return pos.ToString()+" ["+o1.ToString()+", "+o1.ToString()+"]";
		}

	}
}
