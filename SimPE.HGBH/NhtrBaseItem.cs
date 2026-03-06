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
	/// Summary description for TileItem.
	/// </summary>
	public abstract class NhtrBaseItem : NhtrItem
	{		
		protected SimPe.Geometry.Vector3f pos;
		protected SimPe.Geometry.Vector2f min, max;
		protected byte marker2;
		
		internal NhtrBaseItem(NhtrList parent, byte marker) : base(parent)
		{		
			marker2 = marker;
			pos = Vector3f.Zero;
			min = Vector2f.Zero;
			max = Vector2f.Zero;						
		}		

		public Vector3f Position
		{
			get {return pos;}
			set {pos = value;}
		}

		public Vector2f BoundingBoxMinimum
		{
			get {return min;}
			set {min = value;}
		}

		public Vector2f BoundingBoxMaximum
		{
			get {return max;}
			set {max = value;}
		}
		
		public byte Marker2
		{
			get { return marker2;}
		}

		protected abstract void DoUnserialize(System.IO.BinaryReader reader);
		protected abstract void DoSerialize(System.IO.BinaryWriter writer);
		

		internal override void Unserialize(System.IO.BinaryReader reader)
		{	
			base.Unserialize(reader);					
			pos.Y = reader.ReadSingle();
			pos.X = reader.ReadSingle();
			pos.Z = reader.ReadSingle();

			min.Y = reader.ReadSingle();
			min.X = reader.ReadSingle();

			max.Y = reader.ReadSingle();
			max.X = reader.ReadSingle();

			marker2 = reader.ReadByte();

			DoUnserialize(reader);
		}

		internal override void Serialize(System.IO.BinaryWriter writer) 
		{		
			base.Serialize(writer);

			writer.Write((float)pos.Y);
			writer.Write((float)pos.X);
			writer.Write((float)pos.Z);

			writer.Write((float)min.Y);
			writer.Write((float)min.X);
			
			writer.Write((float)max.Y);
			writer.Write((float)max.X);

			writer.Write(marker2);

			DoSerialize(writer);
		}		
				
		public override string ToString()
		{
			string s = Helper.HexString(marker)+"   ";
			s += Helper.HexString(marker2)+"   ";			
			s += pos.ToString()+"   ";
			s += min.ToString()+"   ";
			s += max.ToString()+"   ";

			if (s.Length>0xff) s = s.Substring(0, 0xff)+"...";
			return s;
		}

	}
}
