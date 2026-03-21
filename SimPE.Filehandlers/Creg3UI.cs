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
using Avalonia.Controls;

namespace SimPe.PackedFiles.Wrapper
{
	/// <summary>
	/// Avalonia port of Creg3UI. Displays GUID, CRC and Version for a Creg3 object.
	/// </summary>
	internal class Creg3UI : UserControl
	{
		private TextBlock label1 = new TextBlock { Text = "GUID:" };
		private TextBlock label2 = new TextBlock { Text = "CRC:" };
		private TextBlock label3 = new TextBlock { Text = "Version:" };
		private TextBox tbGuid = new TextBox { IsReadOnly = true };
		private TextBox tbCrc = new TextBox { IsReadOnly = true };
		private TextBox tbVer = new TextBox();

		public Creg3UI()
		{
			tbGuid.TextChanged += tbGuid_TextChanged;
			tbCrc.TextChanged += tbCrc_TextChanged;
			tbVer.TextChanged += tbVer_TextChanged;

			var grid = new Grid();
			grid.ColumnDefinitions.Add(new ColumnDefinition(GridLength.Auto));
			grid.ColumnDefinitions.Add(new ColumnDefinition(GridLength.Star));
			for (int i = 0; i < 3; i++)
				grid.RowDefinitions.Add(new RowDefinition(GridLength.Auto));

			Grid.SetRow(label1, 0); Grid.SetColumn(label1, 0); grid.Children.Add(label1);
			Grid.SetRow(tbGuid,  0); Grid.SetColumn(tbGuid,  1); grid.Children.Add(tbGuid);
			Grid.SetRow(label2, 1); Grid.SetColumn(label2, 0); grid.Children.Add(label2);
			Grid.SetRow(tbCrc,  1); Grid.SetColumn(tbCrc,  1); grid.Children.Add(tbCrc);
			Grid.SetRow(label3, 2); Grid.SetColumn(label3, 0); grid.Children.Add(label3);
			Grid.SetRow(tbVer,  2); Grid.SetColumn(tbVer,  1); grid.Children.Add(tbVer);

			Content = grid;
		}

		public void Dispose() { }

		Creg3 creg;
		public Creg3 Creg
		{
			get { return creg; }
			set
			{
				creg = value;
				SetContent();
			}
		}

		bool intern;
		void SetContent()
		{
			if (intern) return;
			intern = true;
			if (creg == null)
			{
				tbGuid.Text = "";
				tbCrc.Text = "";
				tbVer.Text = "";
			}
			else
			{
				tbGuid.Text = creg.Guid.ToString();
				tbCrc.Text = creg.Crc.ToString();
				tbVer.Text = creg.Version.ToString();
			}
			intern = false;
		}

		private void tbCrc_TextChanged(object sender, Avalonia.Controls.TextChangedEventArgs e)
		{
			if (intern) return;
			if (creg == null) return;
			creg.Guid = new System.Guid(tbGuid.Text);
		}

		private void tbGuid_TextChanged(object sender, Avalonia.Controls.TextChangedEventArgs e)
		{
			if (intern) return;
			if (creg == null) return;
			creg.Crc = new System.Guid(tbCrc.Text);
		}

		private void tbVer_TextChanged(object sender, Avalonia.Controls.TextChangedEventArgs e)
		{
			if (intern) return;
			if (creg == null) return;
			creg.Version = Helper.StringToInt32(tbVer.Text, creg.Version, 10);
		}
	}
}
