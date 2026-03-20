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

namespace SimPe.Plugin.Tool.Dockable
{
	/// <summary>
	/// Registry Keys for the Object Workshop
	/// </summary>
	class ObjectWorkshopRegistry : System.IDisposable
	{
		XmlRegistryKey xrk;		
		dcObjectWorkshop dock;
		public ObjectWorkshopRegistry(dcObjectWorkshop dock)
		{
			this.dock = dock;
			xrk = Helper.XmlRegistry.PluginRegistryKey;
			
			try { dock.cbTask.SelectedIndex = LastOWAction;	} 
			catch {	dock.cbTask.SelectedIndex = 0;}
			dock.cbTask.SelectedIndexChanged += new EventHandler(cbTask_SelectedIndexChanged);

			dock.cbDesc.Checked = ChangeDescription;
			dock.cbDesc.CheckedChanged += new EventHandler(cbDesc_CheckedChanged);

			dock.cbgid.Checked = SetCustomGroup;
			dock.cbgid.CheckedChanged += new EventHandler(cbgid_CheckedChanged);

            dock.cbfix.Checked = FixCloned;
			dock.cbfix.CheckedChanged += new EventHandler(cbfix_CheckedChanged);

            dock.cbclean.Checked = FixCloned;
			dock.cbclean.CheckedChanged += new EventHandler(cbclean_CheckedChanged);

            dock.cbRemTxt.Checked = RemoveNoneDefaultLangaugeStrings;
			dock.cbRemTxt.CheckedChanged += new EventHandler(cbRemTxt_CheckedChanged);

            dock.cbparent.Checked = CreateStandAlone;
			dock.cbparent.CheckedChanged += new EventHandler(cbparent_CheckedChanged);

            dock.cbdefault.Checked = PullDefaultColorOnly;
			dock.cbdefault.CheckedChanged += new EventHandler(cbdefault_CheckedChanged);

            dock.cbwallmask.Checked = PullWallmasks;
			dock.cbwallmask.CheckedChanged += new EventHandler(cbwallmask_CheckedChanged);

            dock.cbanim.Checked = PullAnimations;
			dock.cbanim.CheckedChanged += new EventHandler(cbanim_CheckedChanged);

            dock.cbstrlink.Checked = PullStrLinkedResources;
			dock.cbstrlink.CheckedChanged += new EventHandler(cbstrlink_CheckedChanged);

            dock.cbOrgGmdc.Checked = ReferenceOriginalMesh;
			dock.cbOrgGmdc.CheckedChanged += new EventHandler(cbOrgGmdc_CheckedChanged);
		}

        public void SetDefaults()
        {
            dock.cbDesc.Checked = true;
            dock.cbgid.Checked = true;
            dock.cbfix.Checked = true;
            dock.cbclean.Checked = true;
            dock.cbRemTxt.Checked = true;
            dock.cbparent.Checked = false;
            dock.cbdefault.Checked = true;
            dock.cbwallmask.Checked = true;
            dock.cbanim.Checked = false;
            dock.cbstrlink.Checked = true;
            dock.cbOrgGmdc.Checked = false;			
        }

		#region Properties
		/// <summary>
		/// true, if user wants to show the OBJD Filenames in OW
		/// </summary>
		public  int LastOWAction
		{
			get 
			{
				XmlRegistryKey rkf = xrk.CreateSubKey("ObjectWorkshop");
				object o = rkf.GetValue("LastOWAction", 0);
				return Convert.ToInt32(o);
			}
			set
			{
				XmlRegistryKey rkf = xrk.CreateSubKey("ObjectWorkshop");
				rkf.SetValue("LastOWAction", value);
			}
		}

		public bool ChangeDescription 
		{
			get 
			{
				XmlRegistryKey rkf = xrk.CreateSubKey("ObjectWorkshop");
				object o = rkf.GetValue("ChangeDescription", true);
				return Convert.ToBoolean(o);
			}
			set
			{
				XmlRegistryKey rkf = xrk.CreateSubKey("ObjectWorkshop");
				rkf.SetValue("ChangeDescription", value);
			}
		}		

		public bool SetCustomGroup 
		{
			get 
			{
				XmlRegistryKey rkf = xrk.CreateSubKey("ObjectWorkshop");
				object o = rkf.GetValue("SetCustomGroup", true);
				return Convert.ToBoolean(o);
			}
			set
			{
				XmlRegistryKey rkf = xrk.CreateSubKey("ObjectWorkshop");
				rkf.SetValue("SetCustomGroup", value);
			}
		}

		public bool FixCloned 
		{
			get 
			{
				XmlRegistryKey rkf = xrk.CreateSubKey("ObjectWorkshop");
				object o = rkf.GetValue("FixCloned", true);
				return Convert.ToBoolean(o);
			}
			set
			{
				XmlRegistryKey rkf = xrk.CreateSubKey("ObjectWorkshop");
				rkf.SetValue("FixCloned", value);
			}
		}

		public bool Cleanup 
		{
			get 
			{
				XmlRegistryKey rkf = xrk.CreateSubKey("ObjectWorkshop");
				object o = rkf.GetValue("Cleanup", true);
				return Convert.ToBoolean(o);
			}
			set
			{
				XmlRegistryKey rkf = xrk.CreateSubKey("ObjectWorkshop");
				rkf.SetValue("Cleanup", value);
			}
		}

		public bool RemoveNoneDefaultLangaugeStrings 
		{
			get 
			{
				XmlRegistryKey rkf = xrk.CreateSubKey("ObjectWorkshop");
				object o = rkf.GetValue("RemoveNoneDefaultLangaugeStrings", true);
				return Convert.ToBoolean(o);
			}
			set
			{
				XmlRegistryKey rkf = xrk.CreateSubKey("ObjectWorkshop");
				rkf.SetValue("RemoveNoneDefaultLangaugeStrings", value);
			}
		}

		public bool CreateStandAlone 
		{
			get 
			{
				XmlRegistryKey rkf = xrk.CreateSubKey("ObjectWorkshop");
				object o = rkf.GetValue("CreateStandAlone", false);
				return Convert.ToBoolean(o);
			}
			set
			{
				XmlRegistryKey rkf = xrk.CreateSubKey("ObjectWorkshop");
				rkf.SetValue("CreateStandAlone", value);
			}
		}

		public bool PullDefaultColorOnly 
		{
			get 
			{
				XmlRegistryKey rkf = xrk.CreateSubKey("ObjectWorkshop");
				object o = rkf.GetValue("PullDefaultColorOnly", true);
				return Convert.ToBoolean(o);
			}
			set
			{
				XmlRegistryKey rkf = xrk.CreateSubKey("ObjectWorkshop");
				rkf.SetValue("PullDefaultColorOnly", value);
			}
		}

		public bool PullWallmasks
		{
			get 
			{
				XmlRegistryKey rkf = xrk.CreateSubKey("ObjectWorkshop");
				object o = rkf.GetValue("PullWallmaks", true);
				return Convert.ToBoolean(o);
			}
			set
			{
				XmlRegistryKey rkf = xrk.CreateSubKey("ObjectWorkshop");
				rkf.SetValue("PullWallmaks", value);
			}
		}

		public bool PullAnimations
		{
			get 
			{
				XmlRegistryKey rkf = xrk.CreateSubKey("ObjectWorkshop");
				object o = rkf.GetValue("PullAnimations", false);
				return Convert.ToBoolean(o);
			}
			set
			{
				XmlRegistryKey rkf = xrk.CreateSubKey("ObjectWorkshop");
				rkf.SetValue("PullAnimations", value);
			}
		}

		public bool PullStrLinkedResources
		{
			get 
			{
				XmlRegistryKey rkf = xrk.CreateSubKey("ObjectWorkshop");
				object o = rkf.GetValue("PullStrLinkedResources", true);
				return Convert.ToBoolean(o);
			}
			set
			{
				XmlRegistryKey rkf = xrk.CreateSubKey("ObjectWorkshop");
				rkf.SetValue("PullStrLinkedResources", value);
			}
		}

		public bool ReferenceOriginalMesh
		{
			get 
			{
				XmlRegistryKey rkf = xrk.CreateSubKey("ObjectWorkshop");
				object o = rkf.GetValue("ReferenceOriginalMesh", false);
				return Convert.ToBoolean(o);
			}
			set
			{
				XmlRegistryKey rkf = xrk.CreateSubKey("ObjectWorkshop");
				rkf.SetValue("ReferenceOriginalMesh", value);
			}
		}

		#endregion
		

		#region IDisposable Member

		public void Dispose()
		{
			dock.cbTask.SelectedIndexChanged -= new EventHandler(cbTask_SelectedIndexChanged);
			dock.cbDesc.CheckedChanged -= new EventHandler(cbDesc_CheckedChanged);
			dock.cbgid.CheckedChanged -= new EventHandler(cbgid_CheckedChanged);
			dock.cbfix.CheckedChanged -= new EventHandler(cbfix_CheckedChanged);
			dock.cbclean.CheckedChanged -= new EventHandler(cbclean_CheckedChanged);
			dock.cbRemTxt.CheckedChanged -= new EventHandler(cbRemTxt_CheckedChanged);
			dock.cbparent.CheckedChanged -= new EventHandler(cbparent_CheckedChanged);
			dock.cbdefault.CheckedChanged -= new EventHandler(cbdefault_CheckedChanged);
			dock.cbwallmask.CheckedChanged -= new EventHandler(cbwallmask_CheckedChanged);
			dock.cbanim.CheckedChanged -= new EventHandler(cbanim_CheckedChanged);
			dock.cbstrlink.CheckedChanged -= new EventHandler(cbstrlink_CheckedChanged);
			dock.cbOrgGmdc.CheckedChanged -= new EventHandler(cbOrgGmdc_CheckedChanged);

			dock = null;
			xrk = null;
		}

		#endregion

		
		#region Events
		private void cbDesc_CheckedChanged(object sender, EventArgs e)
		{
			System.Windows.Forms.CheckBox cb = sender as System.Windows.Forms.CheckBox;
			ChangeDescription = cb.Checked;
		}

		private void cbgid_CheckedChanged(object sender, EventArgs e)
		{
			System.Windows.Forms.CheckBox cb = sender as System.Windows.Forms.CheckBox;
			SetCustomGroup = cb.Checked;
		}

		private void cbTask_SelectedIndexChanged(object sender, EventArgs e)
		{
            System.Windows.Forms.ComboBox cb = sender as System.Windows.Forms.ComboBox;
			LastOWAction = cb.SelectedIndex;
		}

		private void cbfix_CheckedChanged(object sender, EventArgs e)
		{
			System.Windows.Forms.CheckBox cb = sender as System.Windows.Forms.CheckBox;
			FixCloned = cb.Checked;
		}

		private void cbclean_CheckedChanged(object sender, EventArgs e)
		{
			System.Windows.Forms.CheckBox cb = sender as System.Windows.Forms.CheckBox;
			Cleanup = cb.Checked;
		}

		private void cbRemTxt_CheckedChanged(object sender, EventArgs e)
		{
			System.Windows.Forms.CheckBox cb = sender as System.Windows.Forms.CheckBox;
			RemoveNoneDefaultLangaugeStrings = cb.Checked;
		}

		private void cbparent_CheckedChanged(object sender, EventArgs e)
		{
			System.Windows.Forms.CheckBox cb = sender as System.Windows.Forms.CheckBox;
			CreateStandAlone = cb.Checked;
		}

		private void cbdefault_CheckedChanged(object sender, EventArgs e)
		{
			System.Windows.Forms.CheckBox cb = sender as System.Windows.Forms.CheckBox;
			PullDefaultColorOnly = cb.Checked;
		}

		private void cbwallmask_CheckedChanged(object sender, EventArgs e)
		{
			System.Windows.Forms.CheckBox cb = sender as System.Windows.Forms.CheckBox;
			PullWallmasks = cb.Checked;
		}

		private void cbanim_CheckedChanged(object sender, EventArgs e)
		{
			System.Windows.Forms.CheckBox cb = sender as System.Windows.Forms.CheckBox;
			PullAnimations = cb.Checked;
		}

		private void cbstrlink_CheckedChanged(object sender, EventArgs e)
		{
			System.Windows.Forms.CheckBox cb = sender as System.Windows.Forms.CheckBox;
			PullStrLinkedResources = cb.Checked;
		}

		private void cbOrgGmdc_CheckedChanged(object sender, EventArgs e)
		{
			System.Windows.Forms.CheckBox cb = sender as System.Windows.Forms.CheckBox;
			ReferenceOriginalMesh = cb.Checked;
		}
		#endregion
	}
}
