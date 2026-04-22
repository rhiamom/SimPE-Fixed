/***************************************************************************
 *   Copyright (C) 2005 by Ambertation                                     *
 *   quaxi@ambertation.de                                                  *
 *                                                                         *
 *   Copyright (C) 2008 Peter L Jones                                      *
 *   pljones@users.sf.net                                                  *
 *                                                                         *
 *   Copyright (C) 2026 by GramzeSweatshop                                 *
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
// Restored from SimPe 0.75f layout (decompiled from simpe.ngbh.plugin.dll)
// EP-gated cbtype population preserved from prior SimPE-Fixed build.
using System;
using System.Windows.Forms;
using SimPe.Interfaces.Plugin;

namespace SimPe.Plugin
{
	public class LtxtUI : IPackedFileUI, IDisposable
	{
		internal LtxtForm form;

		public Control GUIHandle => form.ltxtPanel;

		public LtxtUI()
		{
			form = new LtxtForm();

			form.cbtype.Items.Clear();
			form.cbtype.Items.Add(Ltxt.LotType.Unknown);
			form.cbtype.Items.Add(Ltxt.LotType.Residential);
			form.cbtype.Items.Add(Ltxt.LotType.Community);
			if (PathProvider.Global.EPInstalled > 0)
			{
				form.cbtype.Items.Add(Ltxt.LotType.Dorm);
				form.cbtype.Items.Add(Ltxt.LotType.GreekHouse);
				form.cbtype.Items.Add(Ltxt.LotType.SecretSociety);
			}
			if (PathProvider.Global.EPInstalled > 9)
			{
				form.cbtype.Items.Add(Ltxt.LotType.Hotel);
				form.cbtype.Items.Add(Ltxt.LotType.SecretHoliday);
			}
			if (PathProvider.Global.EPInstalled > 11)
			{
				form.cbtype.Items.Add(Ltxt.LotType.Hobby);
			}
			if (PathProvider.Global.EPInstalled > 15)
			{
				form.cbtype.Items.Add(Ltxt.LotType.ApartmentBase);
				form.cbtype.Items.Add(Ltxt.LotType.ApartmentSublot);
				form.cbtype.Items.Add(Ltxt.LotType.Witches);
			}
		}

		public void UpdateGUI(IFileWrapper wrapper)
		{
			Ltxt ltxt = (Ltxt)wrapper;
			form.wrapper = null;
			if (ltxt.LotDescription != null) form.pb.Image = ltxt.LotDescription.Image;
			form.tbver.Text = ltxt.Version.ToString();
			form.tbsubver.Text = ltxt.SubVersion.ToString();
			if (form.cbtype.Items.Contains(ltxt.Type))
				form.cbtype.SelectedIndex = form.cbtype.Items.IndexOf(ltxt.Type);
			else
				form.cbtype.SelectedIndex = 0;
			form.tbtype.Text = "0x" + Helper.HexString((byte)ltxt.Type);
			form.flpAptBtns.Enabled = ltxt.Type == Ltxt.LotType.ApartmentBase;
			form.tbRoads.Text = "0x" + Helper.HexString(ltxt.LotRoads);
			form.tbwd.Text = ltxt.LotSize.Width.ToString();
			form.tbhg.Text = ltxt.LotSize.Height.ToString();
			form.tbtop.Text = ltxt.LotPosition.Y.ToString();
			form.tbleft.Text = ltxt.LotPosition.X.ToString();
			form.tbz.Text = ltxt.LotElevation.ToString();
			form.cborient.SelectedValue = ltxt.Orientation;
			form.tbrotation.Text = "0x" + Helper.HexString(ltxt.LotRotation);
			form.tbu0.Text = "0x" + Helper.HexString(ltxt.Unknown0);
			form.tblotname.Text = ltxt.LotName;
			form.tbTexture.Text = ltxt.Texture;
			form.tbdesc.Text = ltxt.LotDesc;
			form.tbinst.Text = "0x" + Helper.HexString(ltxt.LotInstance);
			form.tbu3.Text = ltxt.Unknown3.ToString();
			form.tbu4.Text = "0x" + Helper.HexString(ltxt.Unknown4);
			form.tbu5.Text = Helper.BytesToHexList(ltxt.Unknown5);

			form.lb.Items.Clear();
			int x = 0, y = 0;
			foreach (float elev in ltxt.Unknown1)
			{
				form.lb.Items.Add("(" + x + "," + y + ") " + elev);
				x++;
				if ((y + 1) * (ltxt.LotSize.Width + 1) == form.lb.Items.Count)
				{
					y++;
					x = 0;
				}
			}
			form.tbElevationAt.Text = "";

			form.tbu2.Text = "0x" + Helper.HexString(ltxt.Unknown2);
			form.tbowner.Text = "0x" + Helper.HexString(ltxt.OwnerInstance);
			form.tbApBase.Text = "0x" + Helper.HexString(ltxt.ApartmentBase);
			form.tbu6.Text = Helper.BytesToHexList(ltxt.Unknown6);

			form.lbApts.Items.Clear();
			foreach (Ltxt.SubLot sl in ltxt.SubLots)
				form.lbApts.Items.Add("0x" + Helper.HexString(sl.ApartmentSublot));
			form.tbSAu3.Text = "";
			form.tbSAu2.Text = "";
			form.tbSAFamily.Text = "";
			form.tbApartment.Text = "";

			form.lbu7.Items.Clear();
			foreach (uint u in ltxt.Unknown7)
				form.lbu7.Items.Add("0x" + Helper.HexString(u));
			form.tbu7.Text = "";
			form.tbData.Text = Helper.BytesToHexList(ltxt.Followup);

			form.tbowner.ReadOnly = ltxt.Version < LtxtVersion.Business;
			form.tbu3.ReadOnly = ltxt.SubVersion < LtxtSubVersion.Voyage;
			form.tbu4.ReadOnly = ltxt.SubVersion < LtxtSubVersion.Freetime;

			bool isApartmentVer = ltxt.Version >= LtxtVersion.Apartment || ltxt.SubVersion >= LtxtSubVersion.Apartment;
			form.lbu7.Enabled = isApartmentVer;
			form.gbApartment.Enabled = isApartmentVer;
			form.lbApts.Enabled = isApartmentVer;
			form.tbu5.ReadOnly = !isApartmentVer;
			form.tbApBase.ReadOnly = !isApartmentVer;
			form.tbu6.ReadOnly = !isApartmentVer;
			form.tbu7.ReadOnly = !isApartmentVer;

			form.llAptBase.Enabled = ltxt.ApartmentBase != 0;
			form.flpAptBtns.Visible = isApartmentVer && Helper.WindowsRegistry.HiddenMode;
			form.flpAptBtns.Enabled = ltxt.Type == Ltxt.LotType.ApartmentBase;
			form.btnDelApt.Enabled = false;
			form.llFamily.Enabled = false;
			form.llSubLot.Enabled = false;

			form.wrapper = ltxt;
		}

		public void Dispose()
		{
			form.Dispose();
		}
	}
}
