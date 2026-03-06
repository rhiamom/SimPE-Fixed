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

namespace SimPe.Plugin
{
	/// <summary>
	/// Summary description for TileItem.
	/// </summary>
	public abstract class NhtrItem
	{			
		protected byte marker;
		NhtrList parent;
		
		internal NhtrItem(NhtrList parent)
		{
			this.parent = parent;			
			marker = 2;			
		}
		
		public byte Marker
		{
			get {return marker;}
		}

		internal virtual void Unserialize(System.IO.BinaryReader reader)
		{				
			marker = reader.ReadByte();													
		}

		internal virtual void Serialize(System.IO.BinaryWriter writer) 
		{		
			writer.Write(marker);
		}

		public abstract string ToLongString();
	}	
}
