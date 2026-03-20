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
 
 using Ambertation.Scenes;

namespace Ambertation.XSI.Template;

public sealed class Mesh : JoinedArgumentContainer
{
	public enum Types
	{
		Unknown,
		TriangleList
	}

	public string MeshName
	{
		get
		{
			return base.JoinedArgument2;
		}
		set
		{
			base.JoinedArgument2 = value;
		}
	}

	public Types Type => GetMeshType();

	public Mesh(Container parent, string args)
		: base(parent, args)
	{
	}

	protected override void ResetArgs()
	{
		base.ResetArgs("MSH", "Scene_Root");
	}

	protected override void CustomClear()
	{
	}

	private Types GetMeshType()
	{
		if (!(base.Parent is Model))
		{
			return Types.Unknown;
		}
		foreach (ITemplate child in childs)
		{
			if (child is TriangleList)
			{
				return Types.TriangleList;
			}
		}
		return Types.Unknown;
	}

	internal override void ToScene(Ambertation.Scenes.Scene scn)
	{
		Ambertation.Scenes.Mesh mesh = scn.SceneRoot;
		if (!((Model)base.Parent).IsRoot)
		{
			Ambertation.Scenes.Mesh mesh2 = scn.SceneRoot.FindMesh((base.Parent.Parent as Model).ModelName);
			if (mesh2 != null)
			{
				mesh = mesh2;
			}
		}
		if (Type != Types.TriangleList)
		{
			return;
		}
		Shape shape = base[typeof(Shape)] as Shape;
		TriangleList triangleList = base[typeof(TriangleList)] as TriangleList;
		if (shape != null && triangleList != null)
		{
			Ambertation.Scenes.Mesh mesh3 = mesh.CreateMesh(MeshName);
			mesh3.BuildData(shape.Vertices, shape.Normals, shape.TextureCoords, shape.Colors, triangleList.Vertices, triangleList.Normals, triangleList.TextureCoords, triangleList.Colors);
			mesh3.Material = scn.MaterialCollection[triangleList.MaterialName];
			Transform transform = FindTransform(base.Parent, Transform.Types.BASEPOSE);
			if (transform != null)
			{
				Transformation transformation = transform.ToSceneTransform();
				mesh3.WorldPosition.Translation = transformation.Translation;
				mesh3.WorldPosition.Rotation = transformation.Rotation;
				mesh3.WorldPosition.Scaling = transformation.Scaling;
			}
			transform = FindTransform(base.Parent, Transform.Types.SRT);
			if (transform != null)
			{
				Transformation transformation2 = transform.ToSceneTransform();
				mesh3.Translation = transformation2.Translation;
				mesh3.Rotation = transformation2.Rotation;
				mesh3.Scaling = transformation2.Scaling;
			}
		}
	}
}
