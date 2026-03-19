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
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;


namespace SimPe.Plugin.Tool.Window
{
	/// <summary>
	/// Tool that should automatically repair corrupted packages
	/// </summary>
	public class PackageRepairTool : SimPe.Interfaces.IToolPlus
	{		
		public PackageRepairTool()
		{
		}
		#region IToolPlus Member

		public void Execute(object sender, SimPe.Events.ResourceEventArgs e)
		{
			PackageRepairForm f = new PackageRepairForm();
			if (e.Loaded) 
			{
				string flname = e.LoadedPackage.Package.SaveFileName;
				e.LoadedPackage.Package.Close(true);
				f.Setup(flname);
			}
			RemoteControl.ShowSubForm(f);
		}

		public bool ChangeEnabledStateEventHandler(object sender, SimPe.Events.ResourceEventArgs e)
		{
            // return !e.Loaded;
            return true;
		}

		#endregion

		#region IToolExt Member

		public System.Windows.Forms.Shortcut Shortcut
		{
			get
			{				
				return System.Windows.Forms.Shortcut.None;
			}
		}

		public bool Visible
		{
			get
			{
				return true;
			}
		}

		public Image Icon
		{
			get
			{
				return Helper.LoadImage(this.GetType().Assembly.GetManifestResourceStream("SimPe.Plugin.Tool.Dockable.repair.png"));
			}
		}

		#endregion

		#region IToolPlugin Member

		public override string ToString()
		{
            return "Object Tools\\Repair Package Index...";
		}

		#endregion
	}
}
