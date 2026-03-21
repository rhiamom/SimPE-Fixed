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
using Avalonia.Controls;
using Avalonia.Layout;
using Avalonia.Interactivity;

namespace SimPe.Plugin
{
	/// <summary>
	/// Avalonia UserControl replacement for fAnimResourceConst (was WinForms Form).
	/// </summary>
	public class fAnimResourceConst : Avalonia.Controls.UserControl
	{
		// Externally-accessed fields — keep same names and types
		internal Avalonia.Controls.TabItem tAnimResourceConst;
		internal Avalonia.Controls.TextBox tb_arc_ver;
		internal Avalonia.Controls.TreeView tv;

		// Internal UI fields
		private SimPe.Plugin.TabPage.PropertyGridStub pg;
		private Avalonia.Controls.Button llAdd;
		private Avalonia.Controls.Button llExport;

		public fAnimResourceConst()
		{
			BuildLayout();

			llAdd.IsVisible = Helper.XmlRegistry.HiddenMode;
		}

		private void BuildLayout()
		{
			// --- PropertyGrid stub ---
			pg = new SimPe.Plugin.TabPage.PropertyGridStub();

			// --- TreeView ---
			tv = new Avalonia.Controls.TreeView();

			// --- tb_arc_ver ---
			tb_arc_ver = new Avalonia.Controls.TextBox { Text = "0x00000000" };
			tb_arc_ver.TextChanged += tb_arc_ver_TextChanged;

			// --- label30 ---
			var label30 = new Avalonia.Controls.TextBlock { Text = "Version:" };

			// --- llAdd / llExport buttons (were LinkLabels) ---
			llAdd = new Avalonia.Controls.Button { Content = "Add Frame", IsEnabled = false };
			llAdd.Click += llAdd_Click;

			llExport = new Avalonia.Controls.Button { Content = "Export", IsEnabled = false };
			llExport.Click += llExport_Click;

			// --- Settings group (was groupBox12) ---
			var settingsPanel = new Avalonia.Controls.StackPanel { Spacing = 4 };
			settingsPanel.Children.Add(label30);
			settingsPanel.Children.Add(tb_arc_ver);
			settingsPanel.Children.Add(llAdd);
			settingsPanel.Children.Add(llExport);

			var settingsBorder = new Avalonia.Controls.Border
			{
				BorderThickness = new Avalonia.Thickness(1),
				Padding = new Avalonia.Thickness(4),
				Child = settingsPanel
			};

			// --- Content group: TreeView on the left, pg on the right ---
			var contentGrid = new Avalonia.Controls.Grid();
			contentGrid.ColumnDefinitions.Add(new Avalonia.Controls.ColumnDefinition(new Avalonia.Controls.GridLength(1, Avalonia.Controls.GridUnitType.Star)));
			contentGrid.ColumnDefinitions.Add(new Avalonia.Controls.ColumnDefinition(new Avalonia.Controls.GridLength(1, Avalonia.Controls.GridUnitType.Star)));

			Avalonia.Controls.Grid.SetColumn(tv, 0);
			// pg is a plain object (stub), not a Control — omit from visual tree
			contentGrid.Children.Add(tv);

			var contentBorder = new Avalonia.Controls.Border
			{
				BorderThickness = new Avalonia.Thickness(1),
				Padding = new Avalonia.Thickness(4),
				Child = contentGrid
			};
			Avalonia.Controls.Grid.SetColumn(contentBorder, 1);

			// Outer layout for tAnimResourceConst tab content
			var outerGrid = new Avalonia.Controls.Grid();
			outerGrid.ColumnDefinitions.Add(new Avalonia.Controls.ColumnDefinition(new Avalonia.Controls.GridLength(140)));
			outerGrid.ColumnDefinitions.Add(new Avalonia.Controls.ColumnDefinition(new Avalonia.Controls.GridLength(1, Avalonia.Controls.GridUnitType.Star)));

			Avalonia.Controls.Grid.SetColumn(settingsBorder, 0);
			Avalonia.Controls.Grid.SetColumn(contentBorder, 1);
			outerGrid.Children.Add(settingsBorder);
			outerGrid.Children.Add(contentBorder);

			// --- TabItem (was TabPage tAnimResourceConst) ---
			tAnimResourceConst = new Avalonia.Controls.TabItem
			{
				Header = "AnimResourceConst",
				Content = outerGrid
			};

			// --- TabControl ---
			var tabControl1 = new Avalonia.Controls.TabControl();
			tabControl1.Items.Add(tAnimResourceConst);

			// Wire selection event
			tv.SelectionChanged += tv_SelectionChanged;

			Content = tabControl1;
		}

		private void tv_SelectionChanged(object sender, Avalonia.Controls.SelectionChangedEventArgs e)
		{
			llAdd.IsEnabled = false;
			llExport.IsEnabled = false;
			pg.SelectedObject = null;

			var selected = tv.SelectedItem as Avalonia.Controls.TreeViewItem;
			if (selected == null) return;
			if (selected.Tag == null) return;

			pg.SelectedObject = selected.Tag;

			if (selected.Tag is AnimBlock1)
			{
				llExport.IsEnabled = true;
			}
			if (selected.Tag.GetType() == typeof(AnimationFrame[]))
			{
				llAdd.IsEnabled = true;
#if DEBUG
				AnimationFrame[] afs = (AnimationFrame[])selected.Tag;

				System.IO.StreamWriter sw = System.IO.File.CreateText(@"G:\anim.txt");
				try
				{
					sw.WriteLine(afs.Length.ToString());
					for (int i = 0; i < afs.Length; i++)
					{
						if (afs[0].Type == FrameType.Translation)
							sw.WriteLine((i + 1).ToString() + " " +
								(afs[i].Float_X * -1 * Helper.XmlRegistry.ImportExportScaleFactor).ToString("N12", System.Globalization.CultureInfo.InvariantCulture) + " " +
								(afs[i].Float_Z * Helper.XmlRegistry.ImportExportScaleFactor).ToString("N12", System.Globalization.CultureInfo.InvariantCulture) + " " +
								(afs[i].Float_Y * Helper.XmlRegistry.ImportExportScaleFactor).ToString("N12", System.Globalization.CultureInfo.InvariantCulture));
						else
							sw.WriteLine((i + 1).ToString() + " " +
								(afs[i].Float_X * -1).ToString("N12", System.Globalization.CultureInfo.InvariantCulture) + " " +
								(afs[i].Float_Z).ToString("N12", System.Globalization.CultureInfo.InvariantCulture) + " " +
								(afs[i].Float_Y).ToString("N12", System.Globalization.CultureInfo.InvariantCulture));
					}
				}
				finally
				{
					sw.Close();
					sw.Dispose();
					sw = null;
				}
#endif
			}
		}

		private void tb_arc_ver_TextChanged(object sender, Avalonia.Controls.TextChangedEventArgs e)
		{
			if (this.tb_arc_ver.Tag == null) return;
			try
			{
				AbstractRcolBlock arb = (AbstractRcolBlock)this.tAnimResourceConst.Tag;

				arb.Version = Convert.ToUInt32(tb_arc_ver.Text, 16);
				arb.Changed = true;
			}
			catch (Exception)
			{
				//Helper.ExceptionMessage("", ex);
			}
		}

		private void llAdd_Click(object sender, Avalonia.Interactivity.RoutedEventArgs e)
		{
			var selectedItem = tv.SelectedItem as Avalonia.Controls.TreeViewItem;
			if (selectedItem?.Parent is Avalonia.Controls.TreeViewItem parentItem)
			{
				AnimBlock2 ab2 = (AnimBlock2)parentItem.Tag;

				if (ab2.Part3Count != 3) return;

				for (int i = 0; i < ab2.Part3Count; i++)
				{
					AnimBlock3 b = ab2.Part3[i];

					short[] s = new short[b.AddonTokenSize];
					s[0] = -1;
					b.AddData(s);
				}

				AnimResourceConst arc = (AnimResourceConst)tAnimResourceConst.Tag;
				arc.Refresh();
			}
		}

		private void llExport_Click(object sender, Avalonia.Interactivity.RoutedEventArgs e)
		{
			var selectedItem = tv.SelectedItem as Avalonia.Controls.TreeViewItem;
			if (selectedItem == null) return;
			AnimBlock1 ab1 = (AnimBlock1)selectedItem.Tag;
			GenericRcol gmdc = ab1.FindUsedGMDC(ab1.FindDefiningCRES());
			if (gmdc != null)
			{

			}
		}
	}
}
