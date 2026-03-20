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
 
 namespace Ambertation.Scenes.Collections;

public class MeshCollection : NodeCollectionBase
{
	public Mesh this[int index] => GetItem(index) as Mesh;

	public Mesh this[string name] => GetItem(name) as Mesh;

	internal Mesh CreateMesh(Mesh parent, Scene scn, string name)
	{
		return CreateMesh(parent, scn, name, scn.DefaultMaterial);
	}

	internal Mesh CreateMesh(Mesh parent, Scene scn, string name, Material mat)
	{
		Mesh mesh = new Mesh(parent, name, mat, scn);
		DoAdd(mesh);
		return mesh;
	}
}
