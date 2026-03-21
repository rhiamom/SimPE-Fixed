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
using Ambertation.Collections;
using Ambertation.Windows.Forms;
using Ambertation.Windows.Forms.Graph;
using Ambertation.Drawing;
using System.Collections;
using System.Collections.Generic;
using Avalonia.Controls;
using Avalonia.Input;

namespace SimPe.PackedFiles.Wrapper
{
	/// <summary>
	/// You can use this Control whenever you need to display a SimPool
	/// </summary>
	[System.ComponentModel.DefaultEvent("SelectedSimChanged")]
	public class SimPoolControl : Avalonia.Controls.UserControl
	{
		public SimPoolControl()
		{
            details = false;
            rightclicksel = false;
            gp = new SimPe.PackedFiles.Wrapper.SimListView();
            cbhousehold = new Avalonia.Controls.ComboBox();
		}

		protected SimListView gp;
        internal Avalonia.Controls.ComboBox cbhousehold;
        private System.ComponentModel.IContainer components;

		public SimPe.PackedFiles.Wrapper.SDesc SelectedElement
		{
			get {
				if (gp.SelectedItems.Count<1) return null;
				return (SimPe.PackedFiles.Wrapper.ExtSDesc)((XPListViewItem)gp.SelectedItems[0]).Tag;
			}
			set { FindItem(value); }
		}

		public SimPe.PackedFiles.Wrapper.ExtSDesc SelectedSim
		{
			get
			{
				if (gp.SelectedItems.Count<1) return null;
				return (SimPe.PackedFiles.Wrapper.ExtSDesc)((XPListViewItem)gp.SelectedItems[0]).Tag;
			}
			set { FindItem(value); }
		}

		SimPe.Interfaces.Files.IPackageFile pkg;
		public SimPe.Interfaces.Files.IPackageFile Package
		{
			get { return pkg;}
			set
			{
				if (pkg!=value)
				{
					if (value==null) pkg=value;
					else if (Helper.IsNeighborhoodFile(value.FileName)) pkg=value;
					else return;

					UpdateContent();
				}
			}
		}

        protected void UpdateContent()
        {
            SimPe.PackedFiles.Wrapper.ExtSDesc selectedSim = this.SelectedSim;
            string house = selectedSim == null ? "" : selectedSim.HouseholdName;

            this.cbhousehold.Items.Clear();
            this.cbhousehold.Items.Add(SimPe.Localization.GetString("[All Households]"));

            if (pkg == null)
            {
                cbhousehold.SelectedIndex = 0;
                return;
            }

            string chouse;
            List<string> names = new List<string>((string[])FileTable.ProviderRegistry.SimDescriptionProvider.GetHouseholdNames(out chouse).ToArray(typeof(String)));
            foreach (string n in names) this.cbhousehold.Items.Add(n);

            int index = names.IndexOf(house);
            if (index < 0) index = names.IndexOf(chouse);
            cbhousehold.SelectedIndex = index + 1;
            this.SelectedSim = selectedSim;
        }

        bool details;
        public bool SimDetails
        {
            get { return details; }
            set
            {
                if (details != value)
                {
                    details = value;
                    this.SetViewMode();
                }
            }
        }
        public class AddSimToPoolEventArgs : System.EventArgs
        {
            SimPe.PackedFiles.Wrapper.ExtSDesc sdsc;
            public SimPe.PackedFiles.Wrapper.ExtSDesc SimDescription
            {
                get { return sdsc; }
            }

            string name;
            public string Name
            {
                get { return name; }
            }

            string household;
            public string Household
            {
                get { return household; }
            }

            bool cancel;
            public bool Cancel
            {
                get { return cancel; }
                set { cancel = value; }
            }

            System.Drawing.Image img;
            public System.Drawing.Image Image
            {
                get { return img; }
            }

            int grpid;
            public int GroupIndex
            {
                get { return grpid; }
                set { grpid = value; }
            }

            internal AddSimToPoolEventArgs(SimPe.PackedFiles.Wrapper.ExtSDesc sdsc, string name, string household, System.Drawing.Image img, int groupindex)
            {
                this.sdsc = sdsc;
                this.name = name;
                this.img = img;
                this.household = household;
                this.grpid = groupindex;


                cancel = false;
            }

            public override string ToString()
            {
                return Name;
            }
        }

        public delegate void AddSimToPoolEvent(object sender, AddSimToPoolEventArgs e);
        public event AddSimToPoolEvent AddSimToPool;


        protected virtual void OnAddSimToPool(AddSimToPoolEventArgs e)
        {
        }

        AddSimToPoolEventArgs DoAddSimToPool(SimPe.PackedFiles.Wrapper.ExtSDesc sdsc, string name, string household, System.Drawing.Image img)
        {
            AddSimToPoolEventArgs e = new AddSimToPoolEventArgs(sdsc, name, household, img, 0);
            OnAddSimToPool(e);
            if (AddSimToPool != null) AddSimToPool(this, e);
            return e;
        }

        protected virtual System.Drawing.Color GetBackgroundColor(SimPe.PackedFiles.Wrapper.ExtSDesc sdsc)
        {
            return GetImagePanelColor(sdsc);
        }

		void UpdateSimList(string household)
		{
            SimPe.PackedFiles.Wrapper.ExtSDesc selectedSim = this.SelectedSim;
            if (household != null && selectedSim != null && selectedSim.HouseholdName != household) selectedSim = null;

            gp.BeginUpdate();
			gp.Clear();
            lastsel = null;

            Hashtable ht = FileTable.ProviderRegistry.SimDescriptionProvider.SimInstance;
			Wait.SubStart(ht.Count);
			int ct=0;

			System.Collections.SortedList map = new System.Collections.SortedList();

			foreach(SimPe.PackedFiles.Wrapper.ExtSDesc sdsc in ht.Values)
			{
				if (household != null)
					if (household != sdsc.HouseholdName)
						continue;

				string name = sdsc.SimName+" "+sdsc.SimFamilyName;
                System.Drawing.Image simimg = gp.GetSimIcon(sdsc, GetBackgroundColor(sdsc));
                AddSimToPoolEventArgs ret = DoAddSimToPool(sdsc, name, household, simimg);

                if (!ret.Cancel)
                {
                    XPListViewItem eip = gp.Add(sdsc, simimg);
                    eip.Tag = sdsc;
                    eip.GroupIndex = ret.GroupIndex;


                    if (map.ContainsKey(name)) name += " (" + sdsc.FileDescriptor.Instance.ToString() + ")";
                    map[name] = eip;
                    Wait.Message = eip.Text;
                }

				Wait.Progress = ct++;

			}

            SetViewMode();

			if (gp.Items.Count>0)
			{
                if (selectedSim != null) SelectedSim = selectedSim;
                else ((XPListViewItem)gp.Items[0]).Selected = true;
                try
                {
                    if (SelectedSimChanged!=null)
                        SelectedSimChanged(this, ((SimPe.PackedFiles.Wrapper.ExtSDesc)((XPListViewItem)gp.Items[0]).Tag).Image, (Wrapper.SDesc)((SimPe.PackedFiles.Wrapper.ExtSDesc)((XPListViewItem)gp.Items[0]).Tag));
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
			}

			gp.EndUpdate();
			Wait.SubStop();
		}

        private void SetViewMode()
        {
            // TileSize, TileColumns, SetColumnStyle are XPListView-specific — no-op in Avalonia ListBox
        }

		public static System.Drawing.Color GetImagePanelColor(SDesc sdesc)
		{
            if (sdesc.Unlinked != 0)
            {
                if (!sdesc.AvailableCharacterData)
                    return System.Drawing.Color.FromArgb(72,0,72);
                else
                    return System.Drawing.Color.DarkBlue;
            }
            else if (!sdesc.AvailableCharacterData)
                return System.Drawing.Color.DarkRed;
            else if (System.IO.Path.GetFileNameWithoutExtension(sdesc.CharacterFileName) == "objects")
                return System.Drawing.Color.DarkGoldenrod;
            else if (sdesc.CharacterDescription.GhostFlag.IsGhost && sdesc.FamilyInstance == 0)
                return System.Drawing.Color.Black;

		    return System.Drawing.SystemColors.ControlDarkDark;
		}

        internal static void CreateItem(ImagePanel eip, SDesc sdesc)
        {
            eip.ImagePanelColor = System.Drawing.Color.Black;
            eip.Fade = 0.5f;
            eip.FadeColor = System.Drawing.Color.Transparent;

            eip.Tag = sdesc;

            try
            {
                eip.Text = sdesc.SimName + " " + sdesc.SimFamilyName;

                System.Drawing.Image img = sdesc.Image;

                // Reject invalid thumbnails
                if (img == null || img.Width < 8)
                {
                    // Fallback: use generic "no sim" icon
                    img = Helper.LoadImage(
                        typeof(SDesc).Assembly.GetManifestResourceStream(
                            "SimPe.PackedFiles.Wrapper.noone.png"
                        )
                    );
                }
                else
                {
                    // Knockout transparency for Sims with real thumbnails
                    if (Helper.XmlRegistry.GraphQuality)
                    {
                        img = Ambertation.Drawing.GraphicRoutines.KnockoutImage(
                            img,
                            new System.Drawing.Point(0, 0),
                            System.Drawing.Color.Magenta
                        );
                    }
                }

                // Scale to preview size
                eip.Image = Ambertation.Drawing.GraphicRoutines.ScaleImage(
                    img,
                    48,
                    48,
                    Helper.XmlRegistry.GraphQuality
                );

                // Original logic for choosing the panel color (non-Chris)
                eip.ImagePanelColor = GetImagePanelColor(sdesc);
            }
            catch
            {
                // Swallow any preview errors; item will just show without an image
            }

            if (sdesc.CharacterDescription.Gender == Data.MetaData.Gender.Female)
                eip.PanelColor = System.Drawing.Color.LightPink;
            else
                eip.PanelColor = System.Drawing.Color.PowderBlue;

        }



        public static ExtendedImagePanel CreateItem(Wrapper.SDesc sdesc)
		{
			ExtendedImagePanel eip = new ExtendedImagePanel();
			eip.SetBounds(0, 0, 216, 80);
			eip.BeginUpdate();
			PrepareItem(eip, sdesc);
			eip.EndUpdate();

			return eip;
		}

		static void PrepareItem(ExtendedImagePanel eip, Wrapper.SDesc sdesc)
		{
			eip.ImagePanelColor = System.Drawing.Color.Black;
			eip.Fade = 0.5f;
			eip.FadeColor = System.Drawing.Color.Transparent;

			eip.Tag = sdesc;
			try
			{
				eip.Properties["GUID"].Value = "0x"+Helper.HexString(sdesc.SimId);
                eip.Properties["Instance"].Value = "0x" + Helper.HexString(sdesc.FileDescriptor.Instance);
				eip.Properties["Household"].Value = sdesc.HouseholdName;

			}
			catch (Exception ex)
			{
				eip.Properties["Error"].Value = ex.Message;
			}

			CreateItem(eip, sdesc);
		}

		protected ExtendedImagePanel CreateItem(Interfaces.Files.IPackedFileDescriptor pfd, int left, int top)
		{
			ExtendedImagePanel eip = new ExtendedImagePanel();
			eip.BeginUpdate();
			eip.SetBounds(left, top, 216, 80);
            Wrapper.SDesc sdesc = new SDesc();
			try
			{
				sdesc.ProcessData(pfd, pkg);

				PrepareItem(eip, sdesc);
			}
			catch (Exception ex)
			{
				eip.Properties["Error"].Value = ex.Message;
			}
			return eip;
		}

		#region Events
		public delegate void SelectedSimHandler(object sender, System.Drawing.Image thumb, Wrapper.SDesc sdesc);
		public event SelectedSimHandler SelectedSimChanged;
		public event SelectedSimHandler ClickOverSim;
		public event SelectedSimHandler DoubleClickSim;
		#endregion

		private void gp_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			if (SelectedSimChanged!=null && gp.SelectedItems.Count>0)
			{
				//SelectedSimChanged(this, gp.LargeImageList.Images[gp.SelectedItems[0].ImageIndex], (Wrapper.SDesc)((SimPe.PackedFiles.Wrapper.ExtSDesc)gp.SelectedItems[0].Tag));
			}
		}

		private void gp_DoubleClick(object sender, System.EventArgs e)
		{
			if (DoubleClickSim!=null && gp.SelectedItems.Count>0)
			{
                var item = (SimPe.PackedFiles.Wrapper.XPListViewItem)gp.SelectedItems[0];
                DoubleClickSim(this, null, (Wrapper.SDesc)((SimPe.PackedFiles.Wrapper.ExtSDesc)item.Tag));
			}
		}

		XPListViewItem lastsel;
        bool rightclicksel;
        public bool RightClickSelect
        {
            get { return rightclicksel; }
            set { rightclicksel = value; }
        }

		private void gp_MouseDown(object sender, Avalonia.Input.PointerPressedEventArgs e)
		{
            XPListViewItem item = null; // GetItemAt not available in Avalonia
			if (ClickOverSim!=null && item!=null)
			{
				ClickOverSim(this, ((SimPe.PackedFiles.Wrapper.ExtSDesc)item.Tag).Image, (Wrapper.SDesc)((SimPe.PackedFiles.Wrapper.ExtSDesc)item.Tag));
			}

            bool isLeft = e.GetCurrentPoint(null).Properties.IsLeftButtonPressed;
            bool isRight = e.GetCurrentPoint(null).Properties.IsRightButtonPressed;

            if (SelectedSimChanged != null && item != null && (isLeft || (isRight && rightclicksel)))
			{
				gp.SelectedItems.Clear();
				item.Selected = true;
				lastsel = item;
				SelectedSimChanged(this, ((SimPe.PackedFiles.Wrapper.ExtSDesc)item.Tag).Image, (Wrapper.SDesc)((SimPe.PackedFiles.Wrapper.ExtSDesc)item.Tag));
			}
		}


		/// <summary>
		/// Returns the <see cref="ImagePanel"/> that contains the passed Sim
		/// </summary>
		/// <param name="sdsc"></param>
		/// <returns></returns>
		public void FindItem(Wrapper.SDesc sdsc)
		{
			if (sdsc==null)
			{
				gp.SelectedItems.Clear();
				return;
			}

			foreach (XPListViewItem gpe in gp.Items)
			{
				if (gpe.Tag is Wrapper.SDesc)
				{
					if (sdsc.Equals((Wrapper.SDesc)gpe.Tag))
					{
						gpe.Selected = true;
						gpe.EnsureVisible();
						SelectedSimChanged(this, ((Wrapper.SDesc)gpe.Tag).Image, ((Wrapper.SDesc)gpe.Tag));
					}
					else
						gpe.Selected = false;
				}
			}
		}

        /// <summary>
        /// Refresh the LIst of displayed Sims
        /// </summary>
        public void UpdateSimList()
        {
            if (this.cbhousehold.SelectedIndex > 0)
                this.UpdateSimList(this.cbhousehold.SelectedItem?.ToString());
            else
                this.UpdateSimList(null);
        }

		private void cbhousehold_SelectedIndexChanged(object sender, System.EventArgs e)
		{
            UpdateSimList();
		}


		public void SelectHousehold(string name)
		{
			int index=0;
			for (int i=1; i<this.cbhousehold.Items.Count; i++)
			{
				if (this.cbhousehold.Items[i].ToString()==name)
				{
					index=i;
					break;
				}
			}
			this.cbhousehold.SelectedIndex = index;
		}

		public new void Refresh()
		{
			Refresh(true);
		}

		public void Refresh(bool full)
		{
			if (full) this.UpdateContent();
			base.InvalidateVisual();
		}

        internal Avalonia.Controls.ItemCollection Items
        {
            get { return this.gp.Items; }
        }

        internal System.Collections.IList SelectedIndices
        {
            get { return new System.Collections.ArrayList(); }
        }

        internal System.Collections.IList SelectedItems
        {
            get { return gp.SelectedItems; }
        }

        internal void Sort()
        {
        }

        internal XPListViewItem Add(PackedFiles.Wrapper.ExtSDesc o)
        {
            return gp.Add(o);
        }

        public void SetColumnStyle(int column, System.Drawing.Font font, System.Drawing.Color cl)
        {
            // SetColumnStyle is XPListView-specific — no-op in Avalonia ListBox
        }

        public int[] TileColumns
        {
            get { return new int[0]; }
            set { /* no-op */ }
        }

        public void EnsureVisible(int index)
        {
            // no-op in Avalonia ListBox
        }
	}
}
