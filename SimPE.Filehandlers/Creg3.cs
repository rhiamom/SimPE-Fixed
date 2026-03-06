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

namespace SimPe.PackedFiles.Wrapper
{
	/// <summary>
	/// Zusammenfassung für Creg3.
	/// </summary>
	public class Creg3 : ICregGroup
	{
		public class Creg3Item
		{
			string n, v;
			public Creg3Item(string name, string val)
			{
				n = name;
				v = val;
			}

			public string Name
			{
				get {return n;}				
			}

			public string Value
			{
				get {return v;}
				set {v = value;}
			}
		}


		Creg3Item[] items;
		public Creg3()
		{
			items = new Creg3Item[3];
			
			items[0] = new Creg3Item("GUID", System.Guid.NewGuid().ToString().Replace("-", ""));
			items[1] = new Creg3Item("CRC", "00000000000000000000000000000000");
			items[2] = new Creg3Item("Version", "01");
		}

		public Guid Guid
		{
			get 
			{
				Creg3Item i = FindItem("GUID");
				return new System.Guid(i.Value);
			}
			set 
			{
				Creg3Item i = FindItem("GUID");
				i.Value = value.ToString().Replace("-", "");
			}
		}

		public Guid Crc
		{
			get 
			{
				Creg3Item i = FindItem("CRC");
				return new System.Guid(i.Value);
			}
			set 
			{
				Creg3Item i = FindItem("CRC");
				i.Value = value.ToString().Replace("-", "");
			}
		}

		public int Version
		{
			get 
			{
				Creg3Item i = FindItem("Version");
				return Helper.StringToInt32(i.Value, 1, 10);
			}
			set 
			{
				Creg3Item i = FindItem("Version");
				i.Value = value.ToString();
			}
		}


		internal void Unserialize(System.IO.BinaryReader reader)
		{	
			int ct = reader.ReadInt32();
			items = new Creg3Item[ct];
			for (int i=0; i<ct; i++)
			{
				string s1 = StreamHelper.ReadString(reader);
				string s2 = StreamHelper.ReadString(reader);

				items[i] = new Creg3Item(s1, s2);
			}
		}

		internal void Serialize(System.IO.BinaryWriter writer) 
		{		
			writer.Write((int)items.Length);
			foreach (Creg3Item i in items) 
			{
				StreamHelper.WriteString(writer, i.Name);
				StreamHelper.WriteString(writer, i.Value);
			}
		}	

		Creg3Item FindItem(string n)
		{
			for (int i=0; i<items.Length; i++)
				if (items[i].Name == n)
					return items[i];

			return null;
		}

		public bool Contains(string name)
		{
			Creg3Item i = FindItem(name);
			return i!=null;
		}

		public Creg3Item this[string name]
		{
			get { return FindItem(name);}
			set 
			{
				for (int i=0; i<items.Length; i++)
					if (items[i].Name == name)
						items[i] = value;				
			}
		}

		#region ICregGroup Member

		public uint Group
		{
			get
			{
				return 3;
			}
		}		

		#endregion
	}
}
