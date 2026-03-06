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
	public class NhtrBridgeItem : NhtrRoadItem
	{		
		byte marker3;	
		byte[] data2;
		internal NhtrBridgeItem(NhtrList parent) : base(parent)
		{					
			marker3 = 3;
			data2 = new byte[40];
		}					

		public byte[] Data2
		{
			get {return data2;}
		}

		public byte Marker3
		{
			get {return marker3;}
		}

		protected override void DoUnserialize(System.IO.BinaryReader reader)
		{	
			base.DoUnserialize(reader);
			
			marker3 = reader.ReadByte();
			data2 = reader.ReadBytes(data2.Length);
		}

		protected override void DoSerialize(System.IO.BinaryWriter writer) 
		{		
			base.DoSerialize(writer);

			writer.Write(marker3);
			writer.Write(data2);
		}

		public override string ToLongString()
		{
			string s = base.ToLongString()+Helper.lbr;
			s += "Marker 3: "+Helper.HexString(marker3)+Helper.lbr;	
			s += Helper.BytesToHexList(data2, 4);
			return s;
		}

				
		public override string ToString()
		{
			string s =base.ToString()+"   ";
			s += Helper.HexString(marker3)+"   ";
			s += Helper.BytesToHexList(data2);
						
			if (s.Length>0xff) s = s.Substring(0, 0xff)+"...";
			return s;
		}

	}
}
