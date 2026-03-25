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
using System.ComponentModel;
using Avalonia.Controls;

namespace SimPe.Plugin.Gmdc
{

	class MeshListViewItem : System.IDisposable
	{
		protected Ambertation.Scenes.Mesh mesh;
		protected GenericMeshImport gmi;
		Avalonia.Controls.ComboBox cbact, cbgroup;
		Avalonia.Controls.CheckBox cbenv;
		public delegate void ActionChangedEvent(MeshListViewItem sender);
		ActionChangedEvent fkt;

		public MeshListViewItem(Ambertation.Scenes.Mesh mesh, GenericMeshImport gmi, ActionChangedEvent fkt)
		{
			this.fkt = fkt;
			this.mesh = mesh;
			this.gmi = gmi;

			cbact = new Avalonia.Controls.ComboBox();
			cbact.SelectionChanged += cbact_SelectionChanged;
			GenericMeshImport.ImportAction[] acts = (GenericMeshImport.ImportAction[])Enum.GetValues(typeof(GenericMeshImport.ImportAction));
			foreach (GenericMeshImport.ImportAction a in acts)
				cbact.Items.Add(a);
			cbact.SelectedItem = GenericMeshImport.ImportAction.Add;

			cbgroup = new Avalonia.Controls.ComboBox();
			cbgroup.Items.Add("["+SimPe.Localization.GetString("none")+"]");
			foreach (GmdcGroup g in gmi.Gmdc.Groups)
				cbgroup.Items.Add(g);
			cbgroup.SelectedIndex = 0;

			cbenv = new Avalonia.Controls.CheckBox();
			cbenv.IsChecked = mesh.Envelopes.Count > 0;

			int i = gmi.Gmdc.FindGroupByName(mesh.Name);
			if (i>=0)
			{
				Group = gmi.Gmdc.Groups[i];
				Action = GenericMeshImport.ImportAction.Replace;
			}
		}

		~MeshListViewItem()
		{
			Dispose();
		}

		public bool ImportEnvelope
		{
			get {return cbenv.IsChecked ?? false;}
			set {cbenv.IsChecked = value;}
		}

		public bool Shadow
		{
			get {return false;}
			set {}
		}

		public GenericMeshImport.ImportAction Action
		{
			get {return (GenericMeshImport.ImportAction)cbact.SelectedItem;}
			set {cbact.SelectedItem = value;}
		}

		public new GmdcGroup Group
		{
			get
			{
				if (cbgroup.SelectedItem==null) return null;
				if (!(cbgroup.SelectedItem is GmdcGroup)) return null;
				return cbgroup.SelectedItem as GmdcGroup;
			}
			set
			{
				if (value==null) cbgroup.SelectedIndex=0;
				else cbgroup.SelectedItem = value;

				if (cbgroup.SelectedIndex<0) cbgroup.SelectedIndex=0;
			}
		}

		#region IDisposable Member

		public virtual void Dispose()
		{
			if (cbact!=null)
			{
				cbact.SelectionChanged -= cbact_SelectionChanged;
			}
			cbact = null;
			cbgroup = null;
			cbenv = null;
			mesh = null;
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
