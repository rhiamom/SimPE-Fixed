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
using Ambertation.Scenes;

namespace SimPe.Plugin.Gmdc
{
	/// <summary>
	/// Summary description for GenericMeshImport.
	/// </summary>
	public class GenericMeshImport 
	{
		public enum ImportAction 
		{			
			Ignore,
			Update,
			Replace,
			Add
		}

		public enum JointImportAction 
		{			
			Ignore,
			Update					
		}

		Scene scn;
		GeometryDataContainer gmdc;

		MeshListViewItemExt[] meshes;
		BoneListViewItemExt[] bones;
		ElementOrder eo;

		public bool ClearGroupsOnImport;

		internal void SetMeshList(MeshListViewItemExt[] m)
		{
			meshes = m;
		}

		internal void SetBoneList(BoneListViewItemExt[] b)
		{
			bones = b;
		}

		public GenericMeshImport(Scene scn, GeometryDataContainer gmdc, ElementOrder component)
		{
			eo = component;
			this.scn = scn;
			this.gmdc = gmdc;
			ClearGroupsOnImport = false;

		}

		public ElementOrder Component
		{
			get {return eo;}
		}

		public Scene Scene
		{
			get {return scn;}
		}

		public GeometryDataContainer Gmdc
		{
			get {return gmdc;}
		}

		public bool Run()
		{
			scn.ClearTags();
			meshes = new MeshListViewItemExt[0];
			bones = new BoneListViewItemExt[0];

			GenericImportForm.Execute(this);

			if (meshes.Length==0) return false;

			if (this.ClearGroupsOnImport)		
			{	
				for (int i=Gmdc.Groups.Length-1; i>=0; i--) Gmdc.RemoveGroup(i);		
				foreach (MeshListViewItemExt m in meshes)
					m.Group = null;
			}

			foreach (BoneListViewItemExt b in bones)
				b.AssignVertices();

			foreach (MeshListViewItemExt m in meshes)		
				m.BuildGroup();			
						
			scn.ClearTags();
			return true;
		}
	}
}
