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
using SimPe.Interfaces.Plugin;

namespace SimPe.Plugin.TabPage
{
	/// <summary>
	/// Summary description for MatdForm.
	/// </summary>
	public class MaterialDefinition : Avalonia.Controls.TabItem
	{
		internal Avalonia.Controls.TextBox tbdsc;
		internal Avalonia.Controls.TextBox tbtype;
		private Avalonia.Controls.TextBlock label4;
		private Avalonia.Controls.TextBlock label5;
		internal Avalonia.Controls.TextBox tb_ver;
		private Avalonia.Controls.TextBlock label28;

		public MaterialDefinition()
		{
			this.Header = "cMeterialDefinition";

			label5 = new Avalonia.Controls.TextBlock { Text = "Type:" };
			label4 = new Avalonia.Controls.TextBlock { Text = "Description:" };
			tbtype = new Avalonia.Controls.TextBox { Text = "" };
			tbtype.TextChanged += new EventHandler<Avalonia.Controls.TextChangedEventArgs>(this.FileNameChanged);
			tbdsc = new Avalonia.Controls.TextBox { Text = "" };
			tbdsc.TextChanged += new EventHandler<Avalonia.Controls.TextChangedEventArgs>(this.FileNameChanged);
			label28 = new Avalonia.Controls.TextBlock { Text = "Version:" };
			tb_ver = new Avalonia.Controls.TextBox { Text = "0x00000000" };
			tb_ver.TextChanged += new EventHandler<Avalonia.Controls.TextChangedEventArgs>(this.FileNameChanged);

			Content = new Avalonia.Controls.StackPanel { Children = { label28, tb_ver, label4, tbdsc, label5, tbtype } };
		}

		private void FileNameChanged(object sender, System.EventArgs e)
		{
			if (this.Tag==null) return;
			if (tbdsc.Tag!=null) return;
			try
			{
				tbdsc.Tag = true;
				SimPe.Plugin.MaterialDefinition md = (SimPe.Plugin.MaterialDefinition)this.Tag;

				md.Version = Convert.ToUInt32(this.tb_ver.Text, 16);
				md.FileDescription = tbdsc.Text;
				md.MatterialType = tbtype.Text;

				md.Changed = true;
			}
			catch (Exception ex)
			{
				Helper.ExceptionMessage(Localization.Manager.GetString("erropenfile"), ex);
			}

			finally
			{
				tbdsc.Tag = null;
			}
		}

		private void linkLabel1_LinkClicked(object sender, Avalonia.Interactivity.RoutedEventArgs e)
		{
			if (this.Tag==null) return;

			SimPe.Plugin.MaterialDefinition md = (SimPe.Plugin.MaterialDefinition)this.Tag;
			md.Sort();
			md.Refresh();
		}
	}
}
