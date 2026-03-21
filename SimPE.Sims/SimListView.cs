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
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;

namespace SimPe.PackedFiles.Wrapper
{
	/// <summary>
	/// Summary description for SimListView.
	/// </summary>
	public class SimListView : Avalonia.Controls.ListBox
	{
        static Size ICON_SIZE = new Size(64, 64);

		public SimListView()
		{
		}

		public void BeginUpdate() { /* no-op: batch update not needed for Avalonia ListBox */ }
		public void EndUpdate() { /* no-op */ }
		public new void Refresh() { base.InvalidateVisual(); }
		public void EnsureVisible(int index) { /* no-op in Avalonia */ }

		public new void Clear()
		{
			Items.Clear();
		}

        public XPListViewItem Add(SimPe.PackedFiles.Wrapper.ExtSDesc sdesc)
        {
            return Add(sdesc, SimPe.PackedFiles.Wrapper.SimPoolControl.GetImagePanelColor(sdesc));
        }

        public Image GetSimIcon(SimPe.PackedFiles.Wrapper.ExtSDesc sdesc, Color bgcol)
        {
            return BuildSimPreviewImage(bgcol, sdesc.Image, sdesc.SimId, sdesc);
        }

		public XPListViewItem Add(SimPe.PackedFiles.Wrapper.ExtSDesc sdesc, Color bgcol)
		{
            Image imgbig = BuildSimPreviewImage(bgcol, sdesc.Image, sdesc.SimId, sdesc);
            return Add(sdesc, imgbig);
        }

        public XPListViewItem Add(SimPe.PackedFiles.Wrapper.ExtSDesc sdesc, Image imgbig)
        {
			XPListViewItem lvi = new XPListViewItem();

			lvi.Text = " "+sdesc.SimName+" "+sdesc.SimFamilyName;
			lvi.SubItems.Add("    " + sdesc.HouseholdName);
			lvi.SubItems.Add("    0x"+Helper.HexString(sdesc.SimId));
			lvi.SubItems.Add("    0x"+Helper.HexString((ushort)sdesc.FileDescriptor.Instance));
            if (sdesc.University.OnCampus == 0x1)
                lvi.SubItems.Add("    " + Localization.Manager.GetString("YoungAdult"));
            else
                lvi.SubItems.Add("    " + new Data.LocalizedLifeSections(sdesc.CharacterDescription.LifeSection).ToString());

			this.Items.Add(lvi);
			return lvi;
		}

        static System.Collections.Generic.Dictionary<uint, Image> simicons = new System.Collections.Generic.Dictionary<uint, Image>();

        public static Image BuildSimPreviewImage(SimPe.PackedFiles.Wrapper.ExtSDesc sdesc, Color bgcol)
        {
            return BuildSimPreviewImage(bgcol, sdesc.Image, sdesc.SimId, sdesc);
        }
        protected static Image BuildSimPreviewImage(Color bgcol, Image imgbig, uint guid, SimPe.PackedFiles.Wrapper.ExtSDesc sdesc)
        {
            if (simicons.ContainsKey(guid))
            {
                Image img = simicons[guid];
                if (img != null) return (Image)img.Clone();
            }

            if (imgbig != null)
                if (imgbig.Width < 16)
                    imgbig = null;

			if (imgbig != null) imgbig = Ambertation.Drawing.GraphicRoutines.KnockoutImage(imgbig, new Point(0, 0), Color.Magenta);
			else
			{
				var asm = typeof(SimPe.Helper).Assembly;
				imgbig = Helper.LoadImage(asm.GetManifestResourceStream("SimPe.IconXmlResources.noone.png"));
			}
            imgbig = Ambertation.Windows.Forms.Graph.ImagePanel.CreateThumbnail(
                imgbig,
                ICON_SIZE,
                8,
                Color.FromArgb(90, Color.Black),
                bgcol,
                Color.White,
                Color.FromArgb(80, Color.White),
                true,
                0,
                0
                );

            simicons[guid] = imgbig;
            return (Image)imgbig.Clone();
        }
	}

	/// <summary>
	/// Lightweight replacement for SteepValley.Windows.Forms.XPListViewItem.
	/// </summary>
	public class XPListViewItem
	{
		public object Tag { get; set; }
		public string Text { get; set; }
		public int ImageIndex { get; set; }
		public bool Selected { get; set; }
		public int GroupIndex { get; set; }
		public int Index { get; set; }
		public bool IsChecked { get; set; }

		private readonly List<string> _subItems = new List<string>();
		public SubItemCollection SubItems { get; }

		public XPListViewItem()
		{
			SubItems = new SubItemCollection(_subItems);
		}

		public void EnsureVisible() { /* no-op in Avalonia */ }

		public class SubItemCollection
		{
			private List<string> _items;
			public SubItemCollection(List<string> items) { _items = items; }
			public void Add(string text) { _items.Add(text); }
			public int Count => _items.Count;
			public string this[int i] => _items[i];
		}
	}
}
