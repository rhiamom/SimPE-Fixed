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
	public enum NgbhValueDescriptorType : byte
	{
		Skill,
		ToddlerSkill,
		Badge
	}
	/// <summary>
	/// Describes Skills, and Badges
	/// </summary>
	public class NgbhValueDescriptor
	{
		
		string name;
		uint guid, fullguid;
		int valuenr, fullnr;
		short min, max;
		NgbhValueDescriptorType type;
		bool intern;
		
		public NgbhValueDescriptor(string name, bool intern, NgbhValueDescriptorType type, uint guid, int valuenr, short min, short max, int fullnr) : this (name, intern, type, guid, valuenr, min, max, guid, fullnr) {}
		public NgbhValueDescriptor(string name, bool intern, NgbhValueDescriptorType type, uint guid) : this (name, intern, type, guid, 0, -1, -1, 0xffffffff, -1) {}		
		public NgbhValueDescriptor(string name, bool intern, NgbhValueDescriptorType type, uint guid, int valuenr) : this (name, intern, type, guid, valuenr, -1, -1, 0xffffffff, -1) {}		
		public NgbhValueDescriptor(string name, bool intern, NgbhValueDescriptorType type, uint guid, int valuenr, short min, short max) : this (name, intern, type, guid, valuenr, min, max, 0xffffffff, -1) {}		
		NgbhValueDescriptor(string name, bool intern, NgbhValueDescriptorType type, uint guid, int valuenr, short min, short max, uint fullguid, int fullnr)
		{
			this.name = name;
			this.guid = guid;
			this.fullguid = fullguid;
			this.valuenr = valuenr;
			this.fullnr = fullnr;
			this.max = max;
			this.min = min;
			this.type = type;
			this.intern = intern;
		}

		public bool Intern
		{
			get {return intern;}
		}

		public NgbhValueDescriptorType Type
		{
			get 
			{
				return type;
			}
		}

		public bool HasComplededFlag
		{
			get {return fullnr>=0;}
		}

		 uint CompletedGuid
		{
			get {return fullguid;}
		}

		public uint Guid
		{
			get {return guid;}
		}

		public int CompletedDataNumber
		{
			get {return fullnr;}
		}

		public int DataNumber
		{
			get {return valuenr;}
		}

		public short Minimum
		{
			get {return min;}
		}

		public short Maximum
		{
			get {return max;}
		}


		public override string ToString()
		{
			return name;
		}

	}
}
