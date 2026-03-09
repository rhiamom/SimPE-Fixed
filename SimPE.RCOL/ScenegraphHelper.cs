/***************************************************************************
 *   Copyright (C) 2005 by Ambertation                                     *
 *   quaxi@ambertation.de                                                  *
 *                                                                         *
 *   Copyright (C) 2025 by GramzeSweatShop                                 *
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
	/// Some Helper Methods for the Scenegraph Files
	/// </summary>
	public class ScenegraphHelper
	{
		#region Constant Repeat
		public const uint GMND = Data.MetaData.GMND;
		public const uint TXMT = Data.MetaData.TXMT;
		public const uint TXTR = Data.MetaData.TXTR;
		public const uint LIFO = Data.MetaData.LIFO;
		public const uint ANIM = Data.MetaData.ANIM;
		public const uint SHPE = Data.MetaData.SHPE;
		public const uint CRES = Data.MetaData.CRES;
		public const uint GMDC = Data.MetaData.GMDC;

		public const uint MMAT = Data.MetaData.MMAT;

		public static string GMND_PACKAGE = Data.MetaData.GMND_PACKAGE;
		public static string MMAT_PACKAGE = Data.MetaData.MMAT_PACKAGE;		
		#endregion
		
		/// <summary>
		/// Returns a PackedFile Descriptor for the given filename
		/// </summary>
		/// <param name="flname"></param>
		/// <param name="type"></param>
		/// <param name="defgroup"></param>
		/// <returns></returns>
		public static Interfaces.Files.IPackedFileDescriptor BuildPfd(string flname, uint type, uint defgroup)
		{
			string name = Hashes.StripHashFromName(flname);
			SimPe.Packages.PackedFileDescriptor pfd = new SimPe.Packages.PackedFileDescriptor();
			pfd.Type = type;
			pfd.Group = Hashes.GetHashGroupFromName(flname, defgroup);
			pfd.Instance = Hashes.InstanceHash(name);
			pfd.SubType = Hashes.SubTypeHash(name);
			pfd.Filename = flname;

			pfd.UserData = new byte[0];

			return pfd;
		}
	}
}
