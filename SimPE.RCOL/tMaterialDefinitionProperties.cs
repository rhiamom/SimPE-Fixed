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
	public class MatdForm : Avalonia.Controls.TabItem
	{
		internal Avalonia.Controls.ListBox lbprop;
		internal Avalonia.Controls.Border gbprop;
		private Avalonia.Controls.TextBlock label1;
		private Avalonia.Controls.TextBlock label2;
		internal Avalonia.Controls.TextBox tbname;
		private Avalonia.Controls.TextBox tbval;
		private Avalonia.Controls.Button lladd;
		internal Avalonia.Controls.Button lldel;
		private Avalonia.Controls.Button linkLabel1;
		private Avalonia.Controls.Button btnImport;
		private Avalonia.Controls.Button btnExport;
		private Avalonia.Controls.Button btnMerge;

		public MatdForm()
		{
			this.Header = "Properties";

			lbprop = new Avalonia.Controls.ListBox();
			lbprop.SelectionChanged += new EventHandler<Avalonia.Controls.SelectionChangedEventArgs>(this.SelectItem);
			gbprop = new Avalonia.Controls.Border();
			lldel = new Avalonia.Controls.Button { Content = "delete" };
			lldel.Click += new EventHandler<Avalonia.Interactivity.RoutedEventArgs>(this.DeletItem);
			lladd = new Avalonia.Controls.Button { Content = "add" };
			lladd.Click += new EventHandler<Avalonia.Interactivity.RoutedEventArgs>(this.AddItem);
			tbval = new Avalonia.Controls.TextBox();
			tbval.TextChanged += new EventHandler<Avalonia.Controls.TextChangedEventArgs>(this.AutoChange);
			tbname = new Avalonia.Controls.TextBox();
			tbname.TextChanged += new EventHandler<Avalonia.Controls.TextChangedEventArgs>(this.AutoChange);
			label2 = new Avalonia.Controls.TextBlock { Text = "Value:" };
			label1 = new Avalonia.Controls.TextBlock { Text = "Name:" };
			linkLabel1 = new Avalonia.Controls.Button { Content = "sort List" };
			linkLabel1.Click += new EventHandler<Avalonia.Interactivity.RoutedEventArgs>(this.linkLabel1_LinkClicked);
			btnImport = new Avalonia.Controls.Button { Content = "Import..." };
			btnImport.Click += new EventHandler<Avalonia.Interactivity.RoutedEventArgs>(this.btnImport_Click);
			btnExport = new Avalonia.Controls.Button { Content = "Export..." };
			btnExport.Click += new EventHandler<Avalonia.Interactivity.RoutedEventArgs>(this.btnExport_Click);
			btnMerge = new Avalonia.Controls.Button { Content = "Merge..." };
			btnMerge.Click += new EventHandler<Avalonia.Interactivity.RoutedEventArgs>(this.btnMerge_Click);

			Content = new Avalonia.Controls.StackPanel { Children = {
				lbprop, label1, tbname, label2, tbval,
				lladd, lldel, linkLabel1, btnExport, btnImport, btnMerge
			}};
		}

		protected void Change()
		{
			if (this.Tag==null) return;
			if (this.lbprop.SelectedIndex<0) return;
			try
			{
				tbname.Tag = true;
				SimPe.Plugin.MaterialDefinitionProperty prop = (SimPe.Plugin.MaterialDefinitionProperty)lbprop.Items[lbprop.SelectedIndex];

				prop.Name = tbname.Text;
				prop.Value = tbval.Text;

				lbprop.Items[lbprop.SelectedIndex] = prop;

				SimPe.Plugin.MaterialDefinition md = (SimPe.Plugin.MaterialDefinition)this.Tag;
				md.Changed = true;
			}
			catch (Exception ex)
			{
				Helper.ExceptionMessage(Localization.Manager.GetString("errconvert"), ex);
			}
			finally
			{
				tbname.Tag = null;
			}
		}

		private void linkLabel1_LinkClicked(object sender, Avalonia.Interactivity.RoutedEventArgs e)
		{
			if (this.Tag==null) return;

			SimPe.Plugin.MaterialDefinition md = (SimPe.Plugin.MaterialDefinition)this.Tag;
			md.Sort();
			md.Refresh();
		}

		private void AutoChange(object sender, System.EventArgs e)
		{
			if (tbname.Tag!=null) return;
			if (this.lbprop.SelectedIndex>=0) Change();
		}

		private void SelectItem(object sender, System.EventArgs e)
		{
			lldel.IsEnabled = false;
			if (lbprop.SelectedIndex<0) return;
			lldel.IsEnabled = true;

			try
			{
				tbname.Tag = true;
				SimPe.Plugin.MaterialDefinitionProperty prop = (SimPe.Plugin.MaterialDefinitionProperty)lbprop.Items[lbprop.SelectedIndex];
				this.tbname.Text = prop.Name;
				this.tbval.Text = prop.Value;
			}
			catch (Exception ex)
			{
				Helper.ExceptionMessage(Localization.Manager.GetString("errconvert"), ex);
			}
			finally
			{
				tbname.Tag = null;
			}
		}

		private void AddItem(object sender, Avalonia.Interactivity.RoutedEventArgs e)
		{
			if (this.Tag==null) return;
			SimPe.Plugin.MaterialDefinitionProperty prop = new MaterialDefinitionProperty();
			lbprop.Items.Add(prop);

			SimPe.Plugin.MaterialDefinition md = (SimPe.Plugin.MaterialDefinition)this.Tag;
			md.Properties = (MaterialDefinitionProperty[])Helper.Add(md.Properties, prop);

			md.Changed = true;
		}

		private void DeletItem(object sender, Avalonia.Interactivity.RoutedEventArgs e)
		{
			if (this.Tag==null) return;
			if (lbprop.SelectedIndex<0) return;

			SimPe.Plugin.MaterialDefinition md = (SimPe.Plugin.MaterialDefinition)this.Tag;
			md.Properties = (MaterialDefinitionProperty[])Helper.Delete(md.Properties, lbprop.Items[lbprop.SelectedIndex]);
			md.Changed = true;
			lbprop.Items.Remove(lbprop.Items[lbprop.SelectedIndex]);
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

		private void btnExport_Click(object sender, Avalonia.Interactivity.RoutedEventArgs e)
		{
			if (this.Tag == null) return;

			// File dialogs not available in headless mode; no-op
		}

		private void btnImport_Click(object sender, Avalonia.Interactivity.RoutedEventArgs e)
		{
			do_imp_mrg(true);
		}

		private void btnMerge_Click(object sender, Avalonia.Interactivity.RoutedEventArgs e)
		{
			do_imp_mrg(false);
		}

		private void do_imp_mrg(bool imp)
		{
			if (this.Tag == null) return;
			// File dialogs not available in headless mode; no-op
		}
	}
}
