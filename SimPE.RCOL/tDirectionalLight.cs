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
	public class DirectionalLight : Avalonia.Controls.TabItem
	{
		internal Avalonia.Controls.TextBox tb_l_ver;
		private Avalonia.Controls.TextBlock label32;
		internal Avalonia.Controls.TextBox tb_l_name;
		private Avalonia.Controls.TextBlock label34;
		private Avalonia.Controls.TextBlock label38;
		internal Avalonia.Controls.TextBox tb_l_1;
		internal Avalonia.Controls.TextBox tb_l_6;
		internal Avalonia.Controls.TextBlock label39;
		internal Avalonia.Controls.TextBox tb_l_2;
		private Avalonia.Controls.TextBlock label40;
		internal Avalonia.Controls.TextBox tb_l_3;
		private Avalonia.Controls.TextBlock label41;
		internal Avalonia.Controls.TextBox tb_l_4;
		private Avalonia.Controls.TextBlock label42;
		internal Avalonia.Controls.TextBox tb_l_5;
		private Avalonia.Controls.TextBlock label43;
		internal Avalonia.Controls.TextBox tb_l_7;
		internal Avalonia.Controls.TextBlock label44;
		internal Avalonia.Controls.TextBox tb_l_8;
		internal Avalonia.Controls.TextBlock label45;
		internal Avalonia.Controls.TextBox tb_l_9;
		internal Avalonia.Controls.TextBlock label46;

		public DirectionalLight()
		{
			this.Header = "DirectionalLight";

			tb_l_ver = new Avalonia.Controls.TextBox { Text = "0x00000000" };
			tb_l_ver.TextChanged += new EventHandler<Avalonia.Controls.TextChangedEventArgs>(this.LSettingsChanged);
			label32 = new Avalonia.Controls.TextBlock { Text = "Version:" };
			tb_l_name = new Avalonia.Controls.TextBox { Text = "" };
			tb_l_name.TextChanged += new EventHandler<Avalonia.Controls.TextChangedEventArgs>(this.LSettingsChanged);
			label34 = new Avalonia.Controls.TextBlock { Text = "Name:" };
			label38 = new Avalonia.Controls.TextBlock { Text = "Val1:" };
			tb_l_1 = new Avalonia.Controls.TextBox { Text = "0" };
			tb_l_1.TextChanged += new EventHandler<Avalonia.Controls.TextChangedEventArgs>(this.LSettingsChanged);
			label39 = new Avalonia.Controls.TextBlock { Text = "Val6:" };
			tb_l_6 = new Avalonia.Controls.TextBox { Text = "0" };
			tb_l_6.TextChanged += new EventHandler<Avalonia.Controls.TextChangedEventArgs>(this.LSettingsChanged);
			label40 = new Avalonia.Controls.TextBlock { Text = "Val2:" };
			tb_l_2 = new Avalonia.Controls.TextBox { Text = "0" };
			tb_l_2.TextChanged += new EventHandler<Avalonia.Controls.TextChangedEventArgs>(this.LSettingsChanged);
			label41 = new Avalonia.Controls.TextBlock { Text = "Red:" };
			tb_l_3 = new Avalonia.Controls.TextBox { Text = "0" };
			tb_l_3.TextChanged += new EventHandler<Avalonia.Controls.TextChangedEventArgs>(this.LSettingsChanged);
			label42 = new Avalonia.Controls.TextBlock { Text = "Green:" };
			tb_l_4 = new Avalonia.Controls.TextBox { Text = "0" };
			tb_l_4.TextChanged += new EventHandler<Avalonia.Controls.TextChangedEventArgs>(this.LSettingsChanged);
			label43 = new Avalonia.Controls.TextBlock { Text = "Blue:" };
			tb_l_5 = new Avalonia.Controls.TextBox { Text = "0" };
			tb_l_5.TextChanged += new EventHandler<Avalonia.Controls.TextChangedEventArgs>(this.LSettingsChanged);
			label44 = new Avalonia.Controls.TextBlock { Text = "Val7:" };
			tb_l_7 = new Avalonia.Controls.TextBox { Text = "0" };
			tb_l_7.TextChanged += new EventHandler<Avalonia.Controls.TextChangedEventArgs>(this.LSettingsChanged);
			label45 = new Avalonia.Controls.TextBlock { Text = "Val8:" };
			tb_l_8 = new Avalonia.Controls.TextBox { Text = "0" };
			tb_l_8.TextChanged += new EventHandler<Avalonia.Controls.TextChangedEventArgs>(this.LSettingsChanged);
			label46 = new Avalonia.Controls.TextBlock { Text = "Val9:" };
			tb_l_9 = new Avalonia.Controls.TextBox { Text = "0" };
			tb_l_9.TextChanged += new EventHandler<Avalonia.Controls.TextChangedEventArgs>(this.LSettingsChanged);

			Content = new Avalonia.Controls.StackPanel { Children = {
				label32, tb_l_ver, label34, tb_l_name,
				label38, tb_l_1, label40, tb_l_2,
				label41, tb_l_3, label42, tb_l_4, label43, tb_l_5,
				label39, tb_l_6, label44, tb_l_7, label45, tb_l_8, label46, tb_l_9
			}};
		}

		private void LSettingsChanged(object sender, System.EventArgs e)
		{
			if (this.Tag==null) return;
			try
			{
				SimPe.Plugin.DirectionalLight dl = (SimPe.Plugin.DirectionalLight)Tag;

				dl.Version = Convert.ToUInt32(tb_l_ver.Text, 16);
				dl.Name = tb_l_name.Text;
				dl.Val1 = Convert.ToSingle(tb_l_1.Text);
				dl.Val2 = Convert.ToSingle(tb_l_2.Text);
				dl.Red = Convert.ToSingle(tb_l_3.Text);
				dl.Green = Convert.ToSingle(tb_l_4.Text);
				dl.Blue = Convert.ToSingle(tb_l_5.Text);

				if (Tag.GetType() == typeof(PointLight))
				{
					PointLight pl = (PointLight)Tag;

					pl.Val6 = Convert.ToSingle(tb_l_6.Text);
					pl.Val7 = Convert.ToSingle(tb_l_7.Text);
				}

				if (Tag.GetType() == typeof(SpotLight))
				{
					SpotLight sl = (SpotLight)Tag;

					sl.Val6 = Convert.ToSingle(tb_l_6.Text);
					sl.Val7 = Convert.ToSingle(tb_l_7.Text);
					sl.Val8 = Convert.ToSingle(tb_l_8.Text);
					sl.Val9 = Convert.ToSingle(tb_l_9.Text);
				}

				dl.Changed = true;
			}
			catch (Exception)
			{
				//Helper.ExceptionMessage("", ex);
			}
		}
	}
}
