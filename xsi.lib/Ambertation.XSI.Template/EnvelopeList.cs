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

namespace Ambertation.XSI.Template;

public sealed class EnvelopeList : ExtendedContainer
{
	public EnvelopeList(Container parent, string args)
		: base(parent, args)
	{
	}

	protected override void CustomClear()
	{
	}

	protected override void FinishDeSerialize()
	{
		base.FinishDeSerialize();
		CustomClear();
	}

	protected override void PrepareSerialize()
	{
		base.PrepareSerialize();
		TemplateCollection templateCollection = new TemplateCollection(this);
		IEnumerator enumerator = GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				ITemplate pd = (ITemplate)enumerator.Current;
				templateCollection.Add(pd);
			}
		}
		finally
		{
			IDisposable disposable = enumerator as IDisposable;
			if (disposable != null)
			{
				disposable.Dispose();
			}
		}
		Clear(rec: false);
		AddLiteral(templateCollection.Count);
		foreach (ITemplate item in templateCollection)
		{
			Add(item);
		}
	}
}
