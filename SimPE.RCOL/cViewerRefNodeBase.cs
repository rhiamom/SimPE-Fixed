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
using Avalonia.Controls;

namespace SimPe.Plugin
{
	/// <summary>
	/// Summary description for cViewerRefNodeBase.
	/// </summary>
	public class ViewerRefNodeBase
			: AbstractRcolBlock
		{
			#region Attributes

		
			#endregion
		

			/// <summary>
			/// Constructor
			/// </summary>
			public ViewerRefNodeBase(Rcol parent) : base( parent)
			{
				version = 0x5;
				BlockID = 0;
			}
		
			#region IRcolBlock Member

			/// <summary>
			/// Unserializes a BinaryStream into the Attributes of this Instance
			/// </summary>
			/// <param name="reader">The Stream that contains the FileData</param>
			public override void Unserialize(System.IO.BinaryReader reader)
			{
				version = reader.ReadUInt32();
			}

			/// <summary>
			/// Serializes a the Attributes stored in this Instance to the BinaryStream
			/// </summary>
			/// <param name="writer">The Stream the Data should be stored to</param>
			/// <remarks>
			/// Be sure that the Position of the stream is Proper on 
			/// return (i.e. must point to the first Byte after your actual File)
			/// </remarks>
			public override void Serialize(System.IO.BinaryWriter writer)
			{
				writer.Write(version);
			}

		//fShapeRefNode form = null;
		TabPage.GenericRcol tGenericRcol;
		public override Avalonia.Controls.TabItem TabPage
		{
			get
			{
				if (tGenericRcol==null) tGenericRcol = new SimPe.Plugin.TabPage.GenericRcol();
				return tGenericRcol;
			}
		}
		#endregion

		/// <summary>
		/// You can use this to setop the Controls on a TabPage befor it is dispplayed
		/// </summary>
		protected override void InitTabPage() 
		{
			if (tGenericRcol==null) tGenericRcol = new SimPe.Plugin.TabPage.GenericRcol();
			tGenericRcol.tb_ver.Text = "0x"+Helper.HexString(this.version);
			tGenericRcol.gen_pg.SelectedObject = this;
		}

		#region IDisposable Member

		public override void Dispose()
		{
			/* tGenericRcol.Dispose() — TabItem does not implement IDisposable */
			tGenericRcol = null;
		}

		#endregion
		}
}
