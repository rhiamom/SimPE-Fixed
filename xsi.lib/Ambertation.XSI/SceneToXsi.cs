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
 
 using Ambertation.Geometry;
using Ambertation.Geometry.Collections;
using Ambertation.Scenes;
using Ambertation.XSI.IO;
using Ambertation.XSI.Template;

namespace Ambertation.XSI;

public class SceneToXsi : IConvertScene
{
	private Ambertation.Scenes.Scene scn;

	public SceneToXsi(Ambertation.Scenes.Scene scn)
	{
		this.scn = scn;
	}

	public object Convert()
	{
		return ConvertToXsi();
	}

	public AsciiFile ConvertToXsi()
	{
		AsciiFile asciiFile = new AsciiFile("");
		Ambertation.XSI.Template.Scene s = AddMeta(asciiFile);
		AddMaterial(asciiFile, s);
		Container root = asciiFile.Root;
		AddJoints(asciiFile.Root, scn.RootJoint);
		AddMeshes(root);
		AddEnvelope(asciiFile);
		return asciiFile;
	}

	private void AddEnvelope(AsciiFile xsi)
	{
		EnvelopeList elist = null;
		foreach (Ambertation.Scenes.Mesh item in scn.MeshCollection)
		{
			foreach (Ambertation.Scenes.Envelope envelope in item.Envelopes)
			{
				elist = AddEnvelope(xsi, elist, envelope);
			}
		}
	}

	private double SetWeight(double i)
	{
		i *= 1.0;
		if (i < 0.0)
		{
			i = 0.0;
		}
		if (i > 100.0)
		{
			i = 100.0;
		}
		return i;
	}

	private EnvelopeList AddEnvelope(AsciiFile xsi, EnvelopeList elist, Ambertation.Scenes.Envelope env)
	{
		IndexedWeightCollection indexedWeightCollection = new IndexedWeightCollection();
		for (int i = 0; i < env.Weights.Count; i++)
		{
			if (env.Weights[i] > 0.0)
			{
				indexedWeightCollection.Add(new IndexedWeight(i, SetWeight(env.Weights[i])));
			}
		}
		if (indexedWeightCollection.Count > 0)
		{
			if (elist == null)
			{
				elist = (EnvelopeList)xsi.Root.CreateChild("SI_EnvelopeList");
			}
			Ambertation.XSI.Template.Envelope envelope = (Ambertation.XSI.Template.Envelope)elist.CreateChild("SI_Envelope");
			indexedWeightCollection.CopyTo(envelope.Weights, clear: true);
			envelope.Deformer = env.Joint.Name;
			envelope.EnvelopModel = env.Mesh.Name;
		}
		return elist;
	}

	private void AddJoints(Container model, Joint joint)
	{
		foreach (Joint child in joint.Childs)
		{
			AddJoint(model, child);
		}
	}

	private void AddJoint(Container model, Joint joint)
	{
		Model model2 = (Model)model.CreateChild("SI_Model");
		model2.ModelName = joint.Name;
		Transform transform = (Transform)model2.CreateChild("SI_Transform");
		transform.Type = Transform.Types.SRT;
		transform.ModelName = model2.ModelName;
		transform.Rotate = joint.Rotation.RadiantsToDegrees();
		transform.Translate = joint.Translation;
		transform.Scale = joint.Scaling;
		Visibility visibility = (Visibility)model2.CreateChild("SI_Visibility");
		visibility.State = Visibility.States.Visible;
		Null obj = (Null)model2.CreateChild("SI_Null");
		obj.NullName = model2.ModelName;
		foreach (Joint item in joint)
		{
			AddJoint(model2, item);
		}
	}

	private void AddMeshes(Container model)
	{
		foreach (Ambertation.Scenes.Mesh child in scn.SceneRoot.Childs)
		{
			AddMesh(model, child);
		}
	}

	private void AddMesh(Container model, Ambertation.Scenes.Mesh msh)
	{
		Model model2 = (Model)model.CreateChild("SI_Model");
		model2.ModelName = msh.Name;
		GlobalMaterial globalMaterial = (GlobalMaterial)model2.CreateChild("SI_GlobalMaterial");
		globalMaterial.ReferencedMaterialName = msh.Material.Name;
		Transform transform = (Transform)model2.CreateChild("SI_Transform");
		transform.Type = Transform.Types.SRT;
		transform.ModelName = model2.ModelName;
		transform.Rotate = msh.Rotation.RadiantsToDegrees();
		transform.Translate = msh.Translation;
		transform.Scale = msh.Scaling;
		Transform transform2 = (Transform)model2.CreateChild("SI_Transform");
		transform2.Type = Transform.Types.BASEPOSE;
		transform2.ModelName = model2.ModelName;
		transform2.Rotate = msh.WorldPosition.Rotation.RadiantsToDegrees();
		transform2.Translate = msh.WorldPosition.Translation;
		transform2.Scale = msh.WorldPosition.Scaling;
		Visibility visibility = (Visibility)model2.CreateChild("SI_Visibility");
		visibility.State = Visibility.States.Visible;
		if (msh.Vertices.Count > 0)
		{
			Ambertation.XSI.Template.Mesh mesh = (Ambertation.XSI.Template.Mesh)model2.CreateChild("SI_Mesh");
			mesh.MeshName = msh.Name;
			Shape shape = (Shape)mesh.CreateChild("SI_Shape");
			shape.PrimitiveName = msh.Name;
			shape.MaterialName = msh.Material.Name;
			msh.Vertices.CopyTo(shape.Vertices, clear: true);
			msh.Normals.CopyTo(shape.Normals, clear: true);
			msh.TextureCoordinates.CopyTo(shape.TextureCoords, clear: true);
			msh.Colors.CopyTo(shape.Colors, clear: true);
			TriangleList triangleList = (TriangleList)mesh.CreateChild("SI_TriangleList");
			msh.FaceIndices.CopyTo(triangleList.Vertices, clear: true);
			if (shape.Normals.Count > 0)
			{
				msh.FaceIndices.CopyTo(triangleList.Normals, clear: true);
			}
			if (shape.TextureCoords.Count > 0)
			{
				msh.FaceIndices.CopyTo(triangleList.TextureCoords, clear: true);
			}
			triangleList.MaterialName = msh.Material.Name;
			triangleList.PrimitiveName = msh.Name;
		}
		else
		{
			Null obj = (Null)model2.CreateChild("SI_Null");
			obj.NullName = msh.Name;
		}
		foreach (Ambertation.Scenes.Mesh child in msh.Childs)
		{
			AddMesh(model2, child);
		}
	}

	private Light SetLight(Container model)
	{
		Light light = (Light)model.CreateChild("SI_Light");
		light.LightName = "light";
		return light;
	}

	private Camera SetCamera(Container model)
	{
		Model model2 = (Model)model.CreateChild("SI_Model");
		model2.ModelName = "Camera_Root";
		Transform transform = (Transform)model2.CreateChild("SI_Transform");
		transform.Type = Transform.Types.SRT;
		transform.ModelName = model2.ModelName;
		Visibility visibility = (Visibility)model2.CreateChild("SI_Visibility");
		visibility.State = Visibility.States.NotVisible;
		Null obj = (Null)model2.CreateChild("SI_Null");
		obj.NullName = model2.ModelName;
		return (Camera)model2.CreateChild("SI_Camera");
	}

	private Model PrepareModel(AsciiFile xsi)
	{
		Model model = (Model)xsi.Root.CreateChild("SI_Model");
		model.ModelName = "Mesh_Root";
		GlobalMaterial globalMaterial = (GlobalMaterial)model.CreateChild("SI_GlobalMaterial");
		globalMaterial.ReferencedMaterialName = scn.DefaultMaterial.Name;
		Transform transform = (Transform)model.CreateChild("SI_Transform");
		transform.Type = Transform.Types.SRT;
		transform.ModelName = model.ModelName;
		Visibility visibility = (Visibility)model.CreateChild("SI_Visibility");
		visibility.State = Visibility.States.Visible;
		return model;
	}

	private void AddMaterial(AsciiFile xsi, Ambertation.XSI.Template.Scene s)
	{
		MaterialLibrary materialLibrary = (MaterialLibrary)xsi.Root.CreateChild("SI_MaterialLibrary");
		materialLibrary.SceneName = s.SceneName;
		foreach (Ambertation.Scenes.Material item in scn.MaterialCollection)
		{
			Ambertation.XSI.Template.Material material2 = (Ambertation.XSI.Template.Material)materialLibrary.Materials.CreateChild("SI_Material");
			material2.Diffuse = item.Diffuse;
			material2.Emmissive = item.Emmissive;
			material2.Ambient = item.Ambient;
			material2.Specular = item.Specular;
			material2.SpecularPower = item.SpecularPower;
			material2.MaterialName = item.Name;
			if (item.Texture.Available)
			{
				Texture2D texture2D = (Texture2D)material2.Textures.CreateChild("SI_Texture2D");
				texture2D.TextureName = item.Name;
				texture2D.ImageFileName = item.Texture.FileName;
				texture2D.ImageDimension = item.Texture.Size;
				texture2D.UVCropMax.U = item.Texture.Size.Width - 1;
				texture2D.UVCropMax.V = item.Texture.Size.Height - 1;
			}
		}
	}

	private Ambertation.XSI.Template.Scene AddMeta(AsciiFile xsi)
	{
		_ = (FileInfo)xsi.Root.CreateChild("SI_FileInfo");
		Ambertation.XSI.Template.Scene result = (Ambertation.XSI.Template.Scene)xsi.Root.CreateChild("SI_Scene");
		_ = (CoordinateSystem)xsi.Root.CreateChild("SI_CoordinateSystem");
		Angle angle = (Angle)xsi.Root.CreateChild("SI_Angle");
		angle.Representation = Angle.Representations.Degrees;
		_ = (Ambience)xsi.Root.CreateChild("SI_Ambience");
		return result;
	}
}
