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
using System.Collections;
using System.Reflection;
using Ambertation.XSI.Template;

namespace Ambertation.XSI;

public sealed class TemplateRegistry
{
	private static TemplateRegistry reg;

	private Hashtable map;

	private Type generictemplate;

	public static TemplateRegistry Global
	{
		get
		{
			if (reg == null)
			{
				reg = new TemplateRegistry();
			}
			return reg;
		}
	}

	private TemplateRegistry()
	{
		map = new Hashtable();
		generictemplate = typeof(Container);
		RegisterTemplate(typeof(Ambience));
		RegisterTemplate(typeof(Angle));
		RegisterTemplate(typeof(Camera));
		RegisterTemplate(typeof(Cluster));
		RegisterTemplate(typeof(Constraint));
		RegisterTemplate(typeof(CoordinateSystem));
		RegisterTemplate(typeof(Envelope));
		RegisterTemplate(typeof(EnvelopeList));
		RegisterTemplate(typeof(FileInfo));
		RegisterTemplate(typeof(GlobalMaterial));
		RegisterTemplate(typeof(Light));
		RegisterTemplate(typeof(Material));
		RegisterTemplate(typeof(MaterialLibrary));
		RegisterTemplate(typeof(Mesh));
		RegisterTemplate(typeof(Model));
		RegisterTemplate(typeof(Null));
		RegisterTemplate(typeof(Scene));
		RegisterTemplate(typeof(Shape));
		RegisterTemplate(typeof(Texture2D));
		RegisterTemplate(typeof(Transform));
		RegisterTemplate(typeof(TriangleList));
		RegisterTemplate(typeof(Visibility));
	}

	public bool RegisterTemplate(Type template)
	{
		return RegisterTemplate(template, "SI_" + template.Name);
	}

	public bool RegisterTemplate(Type template, string name)
	{
		if (name == null)
		{
			return false;
		}
		if (template.IsAbstract || template.IsInterface)
		{
			return false;
		}
		if (template.GetInterface(typeof(ITemplate).Namespace + "." + typeof(ITemplate).Name) == null)
		{
			return false;
		}
		name = name.Trim();
		if (map.ContainsKey(name))
		{
			return false;
		}
		map[name] = template;
		return true;
	}

	internal Container ActivateTemplateClass(Container parent, string name)
	{
		string[] array = name.Split(' ');
		string text = name;
		if (array.Length > 1)
		{
			text = array[0];
		}
		text = text.Trim();
		object obj = map[text];
		Type t = generictemplate;
		if (obj != null)
		{
			t = obj as Type;
		}
		return ActivateTemplateClass(parent, name, t);
	}

	private Container ActivateTemplateClass(Container parent, string name, Type t)
	{
		ConstructorInfo constructor = t.GetConstructor(new Type[2]
		{
			typeof(Container),
			typeof(string)
		});
		if (constructor != null)
		{
			return (Container)Activator.CreateInstance(t, parent, name);
		}
		constructor = t.GetConstructor(new Type[2]
		{
			typeof(string),
			typeof(Container)
		});
		if (constructor != null)
		{
			return (Container)Activator.CreateInstance(t, name, parent);
		}
		constructor = t.GetConstructor(new Type[0]);
		if (constructor != null)
		{
			return (Container)Activator.CreateInstance(t, new object[0]);
		}
		return null;
	}
}
