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
using System.Drawing;
using System.Collections;
using System.ComponentModel;

namespace SimPe.Plugin.Gmdc
{

	class BoneListViewItemExt : BoneListViewItem
	{
		public BoneListViewItemExt(Ambertation.Scenes.Joint joint, GenericMeshImport gmi, ActionChangedEvent fkt)
			:base(joint, gmi, fkt)
		{
		}				

		public void AssignVertices()
		{			
			joint.Tag = -1;
			if (this.Joint==null && this.Action == GenericMeshImport.JointImportAction.Update) this.Action = GenericMeshImport.JointImportAction.Ignore;
			if (Action == GenericMeshImport.JointImportAction.Ignore) return;

			joint.Tag = Joint.Index;
		}

		#region IDisposable Member

		public override void Dispose()
		{			
			base.Dispose();
		}

		#endregion
	}
}
