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
using Avalonia.Controls;
using SimPe.Interfaces.Plugin;

namespace SimPe.Plugin.TabPage
{
	/// <summary>
	/// Summary description for MatdForm.
	/// </summary>
	public class MaterialDefinitionCategories : Avalonia.Controls.TabItem
	{
		protected override Type StyleKeyOverride => typeof(Avalonia.Controls.TabItem);
		private PropertyGridControl pg;

		public MaterialDefinitionCategories()
		{
			this.Header = "Categorized Properties";
			this.FontSize = 11;

			pg = new SimPe.Plugin.TabPage.PropertyGridControl();
			Content = pg;
		}

		Ambertation.PropertyObjectBuilderExt pob;
		internal void SetupGrid(SimPe.Plugin.MaterialDefinition md)
		{
			pg.SelectedObject = null;

			//Build the table for the current MMAT
			System.Collections.Hashtable ht = new System.Collections.Hashtable();

			foreach (MaterialDefinitionProperty mdp in md.Properties)
			{
				if (SimPe.Plugin.MaterialDefinition.PropertyParser.Properties.ContainsKey(mdp.Name))
				{
					Ambertation.PropertyDescription pd = ((Ambertation.PropertyDescription)SimPe.Plugin.MaterialDefinition.PropertyParser.Properties[mdp.Name]).Clone();
					pd.Property = mdp.Value;
					ht[mdp.Name] = pd;
				}
				else
				{
					ht[mdp.Name] = mdp.Value;
				}
			}

			pob = new Ambertation.PropertyObjectBuilderExt(ht);
			pg.SelectedObject = pob.Instance;
		}

		private void pg_PropertyValueChanged(object s, System.EventArgs e)
		{
			// PropertyGrid stub - no-op
		}

		internal void TxmtChangeTab(object sender, System.EventArgs e)
		{
			if (this.Tag==null) return;
			SimPe.Plugin.MaterialDefinition md = (SimPe.Plugin.MaterialDefinition)this.Tag;
			if (Parent==null) return;
			if (((Avalonia.Controls.TabControl)Parent).SelectedItem == this)
			{
				md.Refresh();
			}
		}
	}
}
