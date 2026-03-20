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

namespace Ambertation.Windows.Forms;

public class NCHitTestEventArgs : NCMouseEventArgs
{
	public enum Results
	{
		HTERROR = -2,
		HTTRANSPARENT,
		HTNOWHERE,
		HTCLIENT,
		HTCAPTION,
		HTSYSMENU,
		HTGROWBOX,
		HTMENU,
		HTHSCROLL,
		HTVSCROLL,
		HTMINBUTTON,
		HTMAXBUTTON,
		HTLEFT,
		HTRIGHT,
		HTTOP,
		HTTOPLEFT,
		HTTOPRIGHT,
		HTBOTTOM,
		HTBOTTOMLEFT,
		HTBOTTOMRIGHT,
		HTBORDER,
		HTOBJECT,
		HTCLOSE,
		HTHELP
	}

	private Results res;

	public Results Result
	{
		get
		{
			return res;
		}
		set
		{
			res = value;
		}
	}

	public NCHitTestEventArgs(Point scrpt, Point ctrlpt, Point absctrlpt, Point delta, IntPtr res, NCButtons mb)
		: base(res, scrpt, ctrlpt, absctrlpt, delta, mb)
	{
		this.res = (Results)res.ToInt32();
	}

	internal IntPtr GetResult()
	{
		return new IntPtr((int)Result);
	}

	public override string ToString()
	{
		return base.ToString() + " " + Result;
	}
}
