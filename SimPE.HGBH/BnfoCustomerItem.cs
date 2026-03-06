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
	/// Summary description for BnfoCustomerItem.
	/// </summary>
	public class BnfoCustomerItem
	{
		ushort siminst;
		public ushort SimInstance
		{
			get {return siminst;}
			set {
				siminst = value;
				sdsc = null;
			}
		}
		int loyalty;
		public int LoyaltyScore
		{
			get {return loyalty;}
			set {loyalty = value;}
		}

		public int LoyaltyStars
		{
			get {return (int)Math.Ceiling((float)LoyaltyScore / 200.0);}
			set {LoyaltyScore = (value * 200);}
		}

		int lloyalty;
		public int LoadedLoyalty
		{
			get {return loyalty;}
			set {loyalty = value;}
		}
		
		byte[] data;

		internal byte[] Data 
		{
			get {return data;}
		}

		Bnfo parent;
		SimPe.PackedFiles.Wrapper.ExtSDesc sdsc;
		public SimPe.PackedFiles.Wrapper.ExtSDesc SimDescription
		{
			get 
			{
				if (sdsc==null) 				
					sdsc = FileTable.ProviderRegistry.SimDescriptionProvider.SimInstance[SimInstance] as SimPe.PackedFiles.Wrapper.ExtSDesc;				
				return sdsc;
			}
		}		

		internal BnfoCustomerItem(Bnfo parent)
		{
			this.parent = parent;
			data = new byte[0x60];			
		}

		long endpos;
        internal void Unserialize(System.IO.BinaryReader reader)
        {
            SimInstance = reader.ReadUInt16();
            loyalty = reader.ReadInt32();
            data = reader.ReadBytes(data.Length);
            lloyalty = reader.ReadInt32();
            endpos = reader.BaseStream.Position;
        }

		internal  void Serialize(System.IO.BinaryWriter writer) 
		{		
			writer.Write(siminst);
			writer.Write(loyalty);
			writer.Write(data);
			writer.Write(this.LoyaltyStars);
		}

		public override string ToString()
		{
			string s = "";
            if (SimDescription != null)
            {
                s = SimDescription.SimName + " " + SimDescription.SimFamilyName;
                if (SimDescription.CharacterDescription.NPCType == 41) s += " [Reporter]";
            }
            else s = SimPe.Localization.GetString("Unknown");

			if (Helper.WindowsRegistry.HiddenMode) 
			{
				return s + " (0x"+Helper.HexString(SimInstance)+"): "+" "+loyalty.ToString()+" ("+this.LoyaltyStars.ToString()+")";
			} 
			else 
			{
				return s + ": "+" "+this.LoyaltyStars.ToString();
			}
		}
	}
}
