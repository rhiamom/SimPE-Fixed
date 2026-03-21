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

// Ported from WinForms to Avalonia (Mac port).
// ComboBox → Avalonia.Controls.ComboBox; Dispose() no-ops since Avalonia controls
// do not implement IDisposable.

using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;   // our compat stubs (ListViewItem, ListViewEx)
using Avalonia.Controls;      // real Avalonia ComboBox

namespace SimPe.Plugin.Gmdc
{

	class BoneListViewItem : System.Windows.Forms.ListViewItem, System.IDisposable
	{
		protected Ambertation.Scenes.Joint joint;
		protected GenericMeshImport gmi;
		System.Windows.Forms.ListViewEx parent;
		Avalonia.Controls.ComboBox cbact, cbgroup;

		public delegate void ActionChangedEvent(BoneListViewItem sender);
		ActionChangedEvent fkt;
		public BoneListViewItem(System.Windows.Forms.ListViewEx lv, Ambertation.Scenes.Joint joint, GenericMeshImport gmi, ActionChangedEvent fkt) : base()
		{
			this.fkt = fkt;
			parent = lv;
			this.joint = joint;
			this.gmi = gmi;

			cbact = new Avalonia.Controls.ComboBox();
			cbact.SelectionChanged += cbact_SelectionChanged;
			GenericMeshImport.JointImportAction[] acts = (GenericMeshImport.JointImportAction[])Enum.GetValues(typeof(GenericMeshImport.JointImportAction));
			foreach (GenericMeshImport.JointImportAction a in acts)
				cbact.Items.Add(a);
			cbact.SelectedItem = GenericMeshImport.JointImportAction.Ignore;

			cbgroup = new Avalonia.Controls.ComboBox();
			cbgroup.Items.Add("["+SimPe.Localization.GetString("none")+"]");
			foreach (GmdcJoint  j in gmi.Gmdc.Joints)
				cbgroup.Items.Add(j);
			cbgroup.SelectedIndex = 0;


			int i = gmi.Gmdc.FindJointByName(joint.Name);
			if (i>=0)
			{
				Joint = gmi.Gmdc.Joints[i];
				Action = GenericMeshImport.JointImportAction.Update;
			}

			Setup();
			parent.Items.Add(this);
			parent.AddEmbeddedControl(cbact, 1, parent.Items.Count-1);
			parent.AddEmbeddedControl(cbgroup, 2, parent.Items.Count-1);
		}

		~BoneListViewItem()
		{
			Dispose();
		}



		public GenericMeshImport.JointImportAction Action
		{
			get {return (GenericMeshImport.JointImportAction)cbact.SelectedItem;}
			set {cbact.SelectedItem = value;}
		}

		public GmdcJoint Joint
		{
			get
			{
				if (cbgroup.SelectedItem==null) return null;
				if (!(cbgroup.SelectedItem is GmdcJoint)) return null;
				return cbgroup.SelectedItem as GmdcJoint;
			}
			set
			{
				if (value==null) cbgroup.SelectedIndex=0;
				else cbgroup.SelectedItem = value;

				if (cbgroup.SelectedIndex<0) cbgroup.SelectedIndex=0;
			}
		}

		void Setup()
		{
			this.SubItems.Clear();
			this.Text = joint.Name;
			this.SubItems.Add(Action.ToString()); //action
			if (Joint!=null) this.SubItems.Add(Joint.Name); //target
			else this.SubItems.Add("["+SimPe.Localization.GetString("none")+"]");
			this.SubItems.Add(joint.GetAssignedVertexCount().ToString());

			this.ForeColor = MyColor();
		}

		Color MyColor()
		{
			return Color.Black;
		}

		#region IDisposable Member

		public virtual void Dispose()
		{
			if (cbact!=null)
			{
				cbact.SelectionChanged -= cbact_SelectionChanged;
				// Avalonia controls are not IDisposable — no Dispose() call needed
			}
			cbact = null;

			if (cbgroup!=null)
			{
				// Avalonia controls are not IDisposable — no Dispose() call needed
			}
			cbgroup = null;

			parent = null;
			joint = null;
			gmi = null;
			fkt = null;
		}

		#endregion


		private void cbact_SelectionChanged(object sender, Avalonia.Controls.SelectionChangedEventArgs e)
		{
			if (fkt != null) fkt(this);
		}
	}
}
