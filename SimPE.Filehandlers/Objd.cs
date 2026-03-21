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
using Avalonia.Controls;
using SimPe.Interfaces.Plugin;
using SimPe.Interfaces;

namespace SimPe.PackedFiles.UserInterface
{
	/// <summary>
	/// handles Packed SDSC Files
	/// </summary>
	public class Objd : UIBase, IPackedFileUI
	{
		#region IPackedFileHandler Member

		public Control GUIHandle
		{
			get
			{
				return form.objdPanel;
			}
		}

		public void UpdateGUI(SimPe.Interfaces.Plugin.IFileWrapper wrapper)
		{
			Wrapper.Objd objd = (Wrapper.Objd)wrapper;
			form.wrapper = objd;

			form.tbsimid.Text = "0x"+Helper.HexString(objd.SimId);
			form.tbsimname.Text = objd.FileName;
			form.tblottype.Text = "0x"+Helper.HexString(objd.Type);
			form.lbtypename.Text = ((Data.ObjectTypes)objd.Type).ToString().Trim();
			form.tborgguid.Text = "0x"+Helper.HexString(objd.OriginalGuid);
            form.tbproxguid.Text = "0x" + Helper.HexString(objd.ProxyGuid);

			Hashtable list = objd.Attributes;
			form.pnelements.Children.Clear();

			ArrayList keys = new ArrayList();
			foreach (string k in list.Keys) keys.Add("0x"+Helper.HexString((ushort)objd.GetAttributePosition(k))+": "+k);
			keys.Sort();

			foreach (string k in keys)
			{
				string[] s = k.Split(":".ToCharArray(), 2);

				TextBlock lb = new TextBlock();
				lb.Text = k + " = ";

				TextBox tb = new TextBox();
				tb.Text = "0x"+Helper.HexString(objd.GetAttributeShort(s[1].Trim()));
				tb.Tag = s[1].Trim();
				tb.TextChanged += new EventHandler<Avalonia.Controls.TextChangedEventArgs>(HexTextChanged);

				TextBox tb2 = new TextBox();
				tb2.Text = ((short)objd.GetAttributeShort(s[1].Trim())).ToString();
				tb2.Tag = null;
				tb2.TextChanged += new EventHandler<Avalonia.Controls.TextChangedEventArgs>(DecTextChanged);

				StackPanel row = new StackPanel { Orientation = Avalonia.Layout.Orientation.Horizontal };
				row.Children.Add(lb);
				row.Children.Add(tb);
				row.Children.Add(tb2);
				form.pnelements.Children.Add(row);
			}
		}

		#endregion

		bool systentextupdate = false;

		private void HexTextChanged(object sender, Avalonia.Controls.TextChangedEventArgs e)
		{
			if (systentextupdate) return;
			systentextupdate = true;
			TextBox tb = (TextBox)sender;

			if (tb.Parent is StackPanel row)
			{
				foreach (Control c in row.Children)
				{
					if (c is TextBox tb2 && tb2 != tb)
					{
						try { tb2.Text = Convert.ToInt16(tb.Text, 16).ToString(); }
						catch { }
						break;
					}
				}
			}
			systentextupdate = false;
		}

		private void DecTextChanged(object sender, Avalonia.Controls.TextChangedEventArgs e)
		{
			if (systentextupdate) return;
			systentextupdate = true;
			TextBox tb = (TextBox)sender;

			if (tb.Parent is StackPanel row)
			{
				foreach (Control c in row.Children)
				{
					if (c is TextBox tb2 && tb2 != tb)
					{
						try { tb2.Text = "0x"+Helper.HexString((ushort)Convert.ToInt16(tb.Text)); }
						catch { }
						break;
					}
				}
			}
			systentextupdate = false;
		}
	}
}
