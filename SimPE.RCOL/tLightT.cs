/***************************************************************************
 *   Copyright (C) 2005 by Ambertation                                     *
 *   quaxi@ambertation.de                                                  *
 *                                                                         *
 *   Copyright (C) 2025 by GramzeSweatShop                                 *
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
using Avalonia.Controls;

namespace SimPe.Plugin.TabPage
{
	/// <summary>
	/// Summary description for fShapeRefNode.
	/// </summary>
	public class LightT : Avalonia.Controls.TabItem
	{
		private Avalonia.Controls.TextBlock label47;
		internal Avalonia.Controls.TextBox tb_lt_ver;
		private Avalonia.Controls.TextBlock label48;
		internal Avalonia.Controls.TextBox tb_lt_name;

		public LightT()
		{
			this.Header = "LightT";

			label47 = new Avalonia.Controls.TextBlock { Text = "Version:" };
			tb_lt_ver = new Avalonia.Controls.TextBox { Text = "0x00000000" };
			tb_lt_ver.TextChanged += new EventHandler<Avalonia.Controls.TextChangedEventArgs>(this.LTSettingsChanged);
			label48 = new Avalonia.Controls.TextBlock { Text = "Name:" };
			tb_lt_name = new Avalonia.Controls.TextBox { Text = "" };
			tb_lt_name.TextChanged += new EventHandler<Avalonia.Controls.TextChangedEventArgs>(this.LTSettingsChanged);

			Content = new Avalonia.Controls.StackPanel { Children = { label47, tb_lt_ver, label48, tb_lt_name } };
		}

		private void LTSettingsChanged(object sender, System.EventArgs e)
		{
			if (this.Tag==null) return;
			try
			{
				Plugin.LightT lt = (Plugin.LightT)Tag;

				lt.Version = Convert.ToUInt32(tb_lt_ver.Text, 16);
				lt.NameResource.FileName = tb_lt_name.Text;

				lt.Changed = true;
			}
			catch (Exception)
			{
				//Helper.ExceptionMessage("", ex);
			}
		}
	}
}
