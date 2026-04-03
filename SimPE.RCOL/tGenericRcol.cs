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
	public class GenericRcol : Avalonia.Controls.TabItem
	{
		protected override Type StyleKeyOverride => typeof(Avalonia.Controls.TabItem);
		private Avalonia.Controls.Border groupBox10;
		internal Avalonia.Controls.TextBox tb_ver;
		private Avalonia.Controls.TextBlock label28;
		internal SimPe.Plugin.TabPage.PropertyGridControl gen_pg;

		public GenericRcol()
		{
			this.Header = "GenericRcol";
			this.FontSize = 11;

			tb_ver = new Avalonia.Controls.TextBox { Background = Avalonia.Media.Brushes.White, Text = "0x00000000" };
			tb_ver.TextChanged += new EventHandler<Avalonia.Controls.TextChangedEventArgs>(this.GNSettingsChange);
			label28 = new Avalonia.Controls.TextBlock { Text = "Version:" };
			gen_pg = new SimPe.Plugin.TabPage.PropertyGridControl();
			groupBox10 = new Avalonia.Controls.Border();

			Content = new Avalonia.Controls.StackPanel { Children = { label28, tb_ver } };
		}

		private void GNSettingsChange(object sender, System.EventArgs e)
		{
			if (this.Tag==null) return;
			try
			{
				AbstractRcolBlock arb = (AbstractRcolBlock)Tag;

				arb.Version = Convert.ToUInt32(tb_ver.Text, 16);
				arb.Changed = true;
			}
			catch (Exception)
			{
				//Helper.ExceptionMessage("", ex);
			}
		}

	}
}
