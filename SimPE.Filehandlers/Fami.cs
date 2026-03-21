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
using SimPe.Interfaces.Plugin;
using SimPe.Interfaces;
using SimPe;

namespace SimPe.PackedFiles.UserInterface
{
	/// <summary>
	/// handles Packed Fami Files
	/// </summary>
	public class Fami : UIBase, IPackedFileUI
	{
		#region IPackedFileHandler Member

		public Control GUIHandle
		{
			get
			{
				return form.famiPanel;
            }
		}

		public void UpdateGUI(SimPe.Interfaces.Plugin.IFileWrapper wrapper)
		{
            Wrapper.Fami fami = (Wrapper.Fami)wrapper;
            form.wrapper = fami;

            // Thumbnail: Avalonia Image.Source requires an IImage; for now set to null
            // (Ambertation.Windows.Forms is WinForms-only — thumbnail display dropped for port)
            form.pb.Source = null;

			form.tbname.Text = fami.Name;
			form.tbmoney.Text = fami.Money.ToString();
			form.tbfamily.Text = fami.Friends.ToString();
            if (Helper.XmlRegistry.AllowLotZero && fami.LotInstance == 0 && fami.FileDescriptor.Instance > 0 && fami.FileDescriptor.Instance < 32512)
                form.tblotinst.Text = "Sim Bin";
            else
                form.tblotinst.Text = "0x"+Helper.HexString(fami.LotInstance);
			form.tbalbum.Text = "0x"+Helper.HexString(fami.AlbumGUID);
			form.tbflag.Text = "0x"+Helper.HexString(fami.Flags);
			form.tbsubhood.Text = "0x"+Helper.HexString(fami.SubHoodNumber);
            form.tbvac.Text = "0x" + Helper.HexString(fami.VacationLotInstance);
            form.tbblot.Text = "0x" + Helper.HexString(fami.CurrentlyOnLotInstance);
            form.tbbmoney.Text = fami.BusinessMoney.ToString();
			form.lbmembers.Items.Clear();
            form.label14.IsVisible = form.tbblot.IsVisible = (int)fami.Version >= (int)SimPe.PackedFiles.Wrapper.FamiVersions.Business;
            form.label7.IsVisible = form.tbvac.IsVisible = (int)fami.Version == (int)SimPe.PackedFiles.Wrapper.FamiVersions.Voyage;
            form.tbsubhood.IsEnabled = (int)fami.Version >= (int)SimPe.PackedFiles.Wrapper.FamiVersions.University;
            form.label16.IsVisible = form.tbbmoney.IsVisible = ((int)fami.Version >= (int)SimPe.PackedFiles.Wrapper.FamiVersions.Business && (int)fami.Version < (int)SimPe.PackedFiles.Wrapper.FamiVersions.Castaway);

            // Color-coding of label15 dropped (Avalonia TextBlock has no ForeColor)

			for(int i=0; i<fami.Members.Length; i++)
			{
				Data.Alias a = new SimPe.Data.Alias(fami.Members[i], fami.SimNames[i]);
				form.lbmembers.Items.Add(a);
			}

			form.cbsims.Items.Clear();
			foreach (IAlias a in fami.NameProvider.StoredData.Values)
			{
				form.cbsims.Items.Add(a);
			}
		}

		#endregion
	}
}
