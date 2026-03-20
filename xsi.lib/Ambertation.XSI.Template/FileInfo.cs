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

namespace Ambertation.XSI.Template;

public sealed class FileInfo : ExtendedContainer
{
	private string projectname;

	private string username;

	private string orginator;

	private DateTime saved;

	public string ProjectName
	{
		get
		{
			return projectname;
		}
		set
		{
			projectname = value;
			if (projectname == null)
			{
				projectname = "";
			}
		}
	}

	public string UserName
	{
		get
		{
			return username;
		}
		set
		{
			username = value;
			if (username == null)
			{
				username = "";
			}
		}
	}

	public string Orginator => orginator;

	public DateTime Saved => saved;

	public FileInfo(Container parent, string args)
		: base(parent, args)
	{
		projectname = "";
		username = "";
		orginator = "";
		saved = DateTime.Now;
	}

	protected override void FinishDeSerialize()
	{
		base.FinishDeSerialize();
		projectname = Line(0).StripQuotes();
		username = Line(1).StripQuotes();
		string s = Line(2).StripQuotes();
		saved = Helpers.ToDateTime(s, saved);
		orginator = Line(3).StripQuotes();
		CustomClear();
	}

	protected override void PrepareSerialize()
	{
		base.PrepareSerialize();
		saved = DateTime.Now;
		Clear(rec: false);
		AddLiteral("\"" + projectname.Trim() + "\"");
		AddLiteral("\"" + username.Trim() + "\"");
		AddLiteral("\"" + saved.ToLongDateString() + " " + saved.ToLongTimeString() + "\"");
		AddLiteral("\" Ambertation's .NET XSI Library (" + GetType().Assembly.GetName().Version.ToString() + ")\"");
	}
}
