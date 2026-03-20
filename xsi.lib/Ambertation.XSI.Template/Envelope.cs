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
 
 using Ambertation.Collections;
using Ambertation.Geometry;
using Ambertation.Geometry.Collections;
using Ambertation.Scenes;

namespace Ambertation.XSI.Template;

public sealed class Envelope : ExtendedContainer
{
	private string e;

	private string d;

	private IndexedWeightCollection list;

	public string EnvelopModel
	{
		get
		{
			return e.Replace("MDL-", "");
		}
		set
		{
			e = "MDL-" + value;
		}
	}

	public string Deformer
	{
		get
		{
			return d.Replace("MDL-", "");
		}
		set
		{
			d = "MDL-" + value;
		}
	}

	public IndexedWeightCollection Weights => list;

	public Envelope(Container parent, string args)
		: base(parent, args)
	{
		list = new IndexedWeightCollection();
		Reset();
	}

	private void Reset()
	{
		e = "";
		d = "";
		list.Clear();
	}

	protected override void FinishDeSerialize()
	{
		base.FinishDeSerialize();
		int index = 0;
		e = Line(index++).StripQuotes();
		d = Line(index++).StripQuotes();
		int num = (int)Line(index++).GetFloat(0);
		for (int i = 0; i < num; i++)
		{
			Vector2 vector = ReadVector2(ref index);
			vector.Y /= 100.0;
			list.Add(vector);
		}
		CustomClear();
	}

	protected override void PrepareSerialize()
	{
		base.PrepareSerialize();
		Clear(rec: false);
		AddQuotedLiteral(e);
		AddQuotedLiteral(d);
		AddLiteral(list.Count);
		foreach (IndexedWeight item in list)
		{
			WriteVector2(item, oneline: true);
		}
	}

	internal override void ToScene(Ambertation.Scenes.Scene scn)
	{
		Joint joint = scn.RootJoint.FindJoint(Deformer);
		Ambertation.Scenes.Mesh mesh = scn.SceneRoot.FindMesh(EnvelopModel);
		if (joint == null || mesh == null)
		{
			return;
		}
		Ambertation.Scenes.Envelope jointEnvelope = mesh.GetJointEnvelope(joint, mesh.Vertices.Count);
		foreach (IndexedWeight weight in Weights)
		{
			IntCollection intCollection = mesh.MappedIndices(weight.Index);
			foreach (int item in intCollection)
			{
				if (item < jointEnvelope.Weights.Count)
				{
					jointEnvelope.Weights[item] = weight.Weight;
				}
			}
		}
	}
}
