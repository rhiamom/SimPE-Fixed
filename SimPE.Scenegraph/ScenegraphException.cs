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
	/// SimPe was unable to load a File
	/// </summary>
	public class CorruptedFileException : Exception 
	{
		static string GetFileName(SimPe.Interfaces.Scenegraph.IScenegraphFileIndexItem item)
		{
			if (item==null) return "";
			if (item.Package==null) return "";
			if (item.Package.FileName == null) return "";

			return item.Package.FileName;
		}

		public CorruptedFileException(SimPe.Interfaces.Scenegraph.IScenegraphFileIndexItem item, Exception inner)
			:base (
					"A corrupted PackedFile was found.", 
					new Exception("The File '"+GetFileName(item)+"' contains a Corrupted File ("+item.FileDescriptor.ToString()+").\n\n SimPe will Ignore this File, but the resulting Package might be broken!", inner)
			)
		{
			FileTable.FileIndex.RemoveItem(item);
		}
	}

	/// <summary>
	/// En Error occurd during the attempt of walking the Scenegraph
	/// </summary>
	public class ScenegraphException : Exception
	{
		SimPe.Interfaces.Files.IPackedFileDescriptor pfd;
		public ScenegraphException(string message, SimPe.Interfaces.Files.IPackedFileDescriptor pfd) : base(message)
		{
			this.pfd = pfd;
		}

		public ScenegraphException(string message, Exception inner, SimPe.Interfaces.Files.IPackedFileDescriptor pfd) : base(message, inner)
		{
			this.pfd = pfd;
		}

		public override string Message
		{
			get
			{
				if (pfd!=null) 
				{
					return base.Message + " (in "+pfd+")";
				} 
				else 
				{
					return base.Message;
				}
			}
		}

	}
}
