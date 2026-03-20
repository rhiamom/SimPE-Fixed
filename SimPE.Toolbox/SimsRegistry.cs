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

namespace SimPe.Plugin
{
	/// <summary>
	/// Registry Keys for the Object Workshop
	/// </summary>
	class SimsRegistry : System.IDisposable
	{
		XmlRegistryKey xrk;		
		Sims form;
		public SimsRegistry(Sims form)
		{
			this.form = form;
			xrk = Helper.XmlRegistry.PluginRegistryKey;

            form.ckbPlayable.Checked = this.ShowPlayable;
            form.ckbPlayable.CheckedChanged += new EventHandler(ckbPlayable_CheckedChanged);

            form.cbTownie.Checked = this.ShowTownies;
            form.cbTownie.CheckedChanged += new EventHandler(cbTownie_CheckedChanged);

            form.cbNpc.Checked = this.ShowNPCs;
			form.cbNpc.CheckedChanged += new EventHandler(cbNpc_CheckedChanged);

            form.ckbUnEditable.Checked = this.ShowUnEditable;
            form.ckbUnEditable.CheckedChanged += new EventHandler(ckbUnEditable_CheckedChanged);

            form.cbdetail.Checked = this.ShowDetails;
            form.cbdetail.CheckedChanged += new EventHandler(cbdetail_CheckedChanged);

            form.cbgals.Checked = this.JustGals;
            form.cbgals.CheckedChanged += new EventHandler(cbgals_CheckedChanged);
            form.cbmens.Enabled = !form.cbgals.Checked;

            form.cbadults.Checked = this.AdultsOnly;
            form.cbadults.CheckedChanged += new EventHandler(cbadults_CheckedChanged);

			form.sorter.CurrentColumn = this.SortedColumn;
			form.sorter.Sorting = this.SortOrder;
			form.sorter.Changed += new EventHandler(sorter_Changed);
		}

		#region Properties
        public bool ShowPlayable
        {
            get
            {
                XmlRegistryKey rkf = xrk.CreateSubKey("SimBrowser");
                object o = rkf.GetValue("ShowPlayable", true);
                return Convert.ToBoolean(o);
            }
            set
            {
                XmlRegistryKey rkf = xrk.CreateSubKey("SimBrowser");
                rkf.SetValue("ShowPlayable", value);
            }
        }

        public bool ShowTownies
        {
            get
            {
                XmlRegistryKey rkf = xrk.CreateSubKey("SimBrowser");
                object o = rkf.GetValue("ShowTownies", false);
                return Convert.ToBoolean(o);
            }
            set
            {
                XmlRegistryKey rkf = xrk.CreateSubKey("SimBrowser");
                rkf.SetValue("ShowTownies", value);
            }
        }

        public bool ShowNPCs
		{
			get 
			{
				XmlRegistryKey rkf = xrk.CreateSubKey("SimBrowser");
				object o = rkf.GetValue("ShowNPCs", false);
				return Convert.ToBoolean(o);
			}
			set
			{
				XmlRegistryKey rkf = xrk.CreateSubKey("SimBrowser");
				rkf.SetValue("ShowNPCs", value);
			}
		}

        public bool ShowUnEditable
        {
            get
            {
                XmlRegistryKey rkf = xrk.CreateSubKey("SimBrowser");
                object o = rkf.GetValue("ShowUnEditable", false);
                return Convert.ToBoolean(o);
            }
            set
            {
                XmlRegistryKey rkf = xrk.CreateSubKey("SimBrowser");
                rkf.SetValue("ShowUnEditable", value);
            }
        }

        public bool ShowDetails
		{
			get 
			{
				XmlRegistryKey rkf = xrk.CreateSubKey("SimBrowser");
				object o = rkf.GetValue("ShowDetails", true);
				return Convert.ToBoolean(o);
			}
			set
			{
				XmlRegistryKey rkf = xrk.CreateSubKey("SimBrowser");
				rkf.SetValue("ShowDetails", value);
			}
        }

        public bool JustGals
        {
            get
            {
                XmlRegistryKey rkf = xrk.CreateSubKey("SimBrowser");
                object o = rkf.GetValue("JustGals", false);
                return Convert.ToBoolean(o);
            }
            set
            {
                XmlRegistryKey rkf = xrk.CreateSubKey("SimBrowser");
                rkf.SetValue("JustGals", value);
            }
        }

        public bool AdultsOnly
        {
            get
            {
                XmlRegistryKey rkf = xrk.CreateSubKey("SimBrowser");
                object o = rkf.GetValue("AdultsOnly", false);
                return Convert.ToBoolean(o);
            }
            set
            {
                XmlRegistryKey rkf = xrk.CreateSubKey("SimBrowser");
                rkf.SetValue("AdultsOnly", value);
            }
        }

		public int SortedColumn
		{
			get 
			{
				XmlRegistryKey rkf = xrk.CreateSubKey("SimBrowser");
				object o = rkf.GetValue("SortedColumn", 3);
				return Convert.ToInt32(o);
			}
			set
			{
				XmlRegistryKey rkf = xrk.CreateSubKey("SimBrowser");
				rkf.SetValue("SortedColumn", value);
			}
		}

		public System.Windows.Forms.SortOrder SortOrder
		{
			get 
			{
				XmlRegistryKey rkf = xrk.CreateSubKey("SimBrowser");
				object o = rkf.GetValue("SortOrder", (int)System.Windows.Forms.SortOrder.Ascending);
				return (System.Windows.Forms.SortOrder)Convert.ToInt32(o);
			}
			set
			{
				XmlRegistryKey rkf = xrk.CreateSubKey("SimBrowser");
				rkf.SetValue("SortOrder", (int)value);
			}
		}

		#endregion
		

		#region IDisposable Member

		public void Dispose()
		{
			

			form = null;
			xrk = null;
		}

		#endregion

        private void ckbPlayable_CheckedChanged(object sender, EventArgs e)
        {
            System.Windows.Forms.CheckBox cb = sender as System.Windows.Forms.CheckBox;
            this.ShowPlayable = cb.Checked;
        }

        private void cbTownie_CheckedChanged(object sender, EventArgs e)
        {
            System.Windows.Forms.CheckBox cb = sender as System.Windows.Forms.CheckBox;
            this.ShowTownies = cb.Checked;
        }

        private void cbNpc_CheckedChanged(object sender, EventArgs e)
        {
            System.Windows.Forms.CheckBox cb = sender as System.Windows.Forms.CheckBox;
            this.ShowNPCs = cb.Checked;
        }

        private void ckbUnEditable_CheckedChanged(object sender, EventArgs e)
		{
			System.Windows.Forms.CheckBox cb = sender as System.Windows.Forms.CheckBox;
            this.ShowUnEditable = cb.Checked;
		}

		private void cbdetail_CheckedChanged(object sender, EventArgs e)
		{
			System.Windows.Forms.CheckBox cb = sender as System.Windows.Forms.CheckBox;
			this.ShowDetails = cb.Checked;
        }

        private void cbgals_CheckedChanged(object sender, EventArgs e)
        {
            System.Windows.Forms.CheckBox cb = sender as System.Windows.Forms.CheckBox;
            this.JustGals = cb.Checked;
        }

        private void cbadults_CheckedChanged(object sender, EventArgs e)
        {
            System.Windows.Forms.CheckBox cb = sender as System.Windows.Forms.CheckBox;
            this.AdultsOnly = cb.Checked;
        }

		private void sorter_Changed(object sender, EventArgs e)
		{
			SimPe.ColumnSorter cs = sender as SimPe.ColumnSorter;
			this.SortedColumn = cs.CurrentColumn;
			this.SortOrder = cs.Sorting;
		}
	}
}
