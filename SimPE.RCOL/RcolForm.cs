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
using Avalonia.Input;
using Avalonia.Layout;
using SimPe.Interfaces.Scenegraph;

namespace SimPe.Plugin
{
	/// <summary>
	/// Summary description for RcolForm.
	/// </summary>
	public class RcolForm : SimPe.Windows.Forms.WrapperBaseControl
	{
		// ── Controls ────────────────────────────────────────────────────────
		private Avalonia.Controls.Button     llfix;
		private Avalonia.Controls.Button     llhash;
		private Avalonia.Controls.TextBox    tbflname;
		private Avalonia.Controls.TextBlock  label2;
		internal Avalonia.Controls.ComboBox  cbitem;
		private Avalonia.Controls.TabItem    tabPage1;
		private Avalonia.Controls.TabItem    tabPage2;
		internal Avalonia.Controls.ListBox   lbref;
		private Avalonia.Controls.TabControl childtc;
		private Avalonia.Controls.Panel      pntypes;
		internal Avalonia.Controls.Button    lladd;
		internal Avalonia.Controls.Button    lldelete;
		internal Avalonia.Controls.TextBox   tbsubtype;
		internal Avalonia.Controls.TextBox   tbinstance;
		private Avalonia.Controls.TextBlock  label11;
		internal Avalonia.Controls.TextBox   tbtype;
		private Avalonia.Controls.TextBlock  label8;
		private Avalonia.Controls.TextBlock  label9;
		private Avalonia.Controls.TextBlock  label10;
		internal Avalonia.Controls.TextBox   tbgroup;
		internal Avalonia.Controls.ComboBox  cbtypes;
		private Avalonia.Controls.Button     btref;
		private Avalonia.Controls.TabItem    tabPage3;
		private Avalonia.Controls.ListBox    lbblocks;
		private Avalonia.Controls.Button     btup;
		private Avalonia.Controls.Button     btdown;
		private Avalonia.Controls.Button     btadd;
		private Avalonia.Controls.Button     btdel;
		private Avalonia.Controls.ComboBox   cbblocks;
		private Avalonia.Controls.TextBlock  label1;
		internal Avalonia.Controls.TabItem   tpref;
		internal Avalonia.Controls.TreeView  tv;
		private Avalonia.Controls.TextBlock  label3;
		private Avalonia.Controls.TextBlock  label4;
		private Avalonia.Controls.TextBox    tbrefgroup;
		private Avalonia.Controls.TextBox    tbrefinst;
		private Avalonia.Controls.TextBlock  label5;
		private Avalonia.Controls.TextBox    tbfile;
		private Avalonia.Controls.Button     linkLabel1;
		internal Avalonia.Controls.TabControl tbResource;

		public RcolForm() : base()
		{
			this.HeaderText = "Generic Rcol Editor";
			this.FontSize = 11;   // cascade to all child tabs and controls
			BuildLayout();

			foreach (Interfaces.IAlias alias in SimPe.Helper.TGILoader.FileTypes)
				cbtypes.Items.Add(alias);

			this.Commited += Commit;
		}

		private void BuildLayout()
		{
			// ── Reference Tab (tpref) ──────────────────────────────────────
			label3    = new Avalonia.Controls.TextBlock { Text = "Group:",    VerticalAlignment = Avalonia.Layout.VerticalAlignment.Center };
			label4    = new Avalonia.Controls.TextBlock { Text = "Instance:", VerticalAlignment = Avalonia.Layout.VerticalAlignment.Center };
			label5    = new Avalonia.Controls.TextBlock { Text = "File:",     VerticalAlignment = Avalonia.Layout.VerticalAlignment.Center };
			tbrefgroup = new Avalonia.Controls.TextBox { Text = "0x00000000", IsReadOnly = true, Background = Avalonia.Media.Brushes.White, MinHeight = 0, Padding = new Avalonia.Thickness(4, 2) };
			tbrefinst  = new Avalonia.Controls.TextBox { Text = "0x00000000", IsReadOnly = true, Background = Avalonia.Media.Brushes.White, MinHeight = 0, Padding = new Avalonia.Thickness(4, 2) };
			tbfile     = new Avalonia.Controls.TextBox { IsReadOnly = true, Background = Avalonia.Media.Brushes.White, MinHeight = 0, Padding = new Avalonia.Thickness(4, 2) };
			linkLabel1 = new Avalonia.Controls.Button { Content = "reload", Padding = new Avalonia.Thickness(0), Background = Avalonia.Media.Brushes.Transparent, BorderThickness = new Avalonia.Thickness(0), Foreground = new Avalonia.Media.SolidColorBrush(Avalonia.Media.Color.FromRgb(0, 80, 180)), Cursor = new Avalonia.Input.Cursor(Avalonia.Input.StandardCursorType.Hand) };
			linkLabel1.Click += linkLabel1_LinkClicked;

			tv = new Avalonia.Controls.TreeView();
			tv.SelectionChanged += SelectRefItem;
			DragDrop.SetAllowDrop(tv, false);

			// "Values" groupbox: 4-row grid inside a titled border
			//   row 0: Group:    | textbox
			//   row 1: Instance: | textbox
			//   row 2: File:     | reload link
			//   row 3: tbfile (spans both columns)
			var valuesGrid = new Grid { Margin = new Avalonia.Thickness(4) };
			valuesGrid.RowDefinitions.Add(new RowDefinition(GridLength.Auto));
			valuesGrid.RowDefinitions.Add(new RowDefinition(GridLength.Auto));
			valuesGrid.RowDefinitions.Add(new RowDefinition(GridLength.Auto));
			valuesGrid.RowDefinitions.Add(new RowDefinition(GridLength.Auto));
			valuesGrid.ColumnDefinitions.Add(new ColumnDefinition(GridLength.Auto));
			valuesGrid.ColumnDefinitions.Add(new ColumnDefinition(new GridLength(1, GridUnitType.Star)));

			label3.Margin = new Avalonia.Thickness(0, 0, 6, 3);
			label4.Margin = new Avalonia.Thickness(0, 3, 6, 3);
			label5.Margin = new Avalonia.Thickness(0, 3, 6, 0);

			Grid.SetRow(label3,    0); Grid.SetColumn(label3,    0);
			Grid.SetRow(tbrefgroup,0); Grid.SetColumn(tbrefgroup,1);
			Grid.SetRow(label4,    1); Grid.SetColumn(label4,    0);
			Grid.SetRow(tbrefinst, 1); Grid.SetColumn(tbrefinst, 1);
			Grid.SetRow(label5,    2); Grid.SetColumn(label5,    0);
			Grid.SetRow(linkLabel1,2); Grid.SetColumn(linkLabel1,1);
			Grid.SetRow(tbfile,    3); Grid.SetColumn(tbfile,    0); Grid.SetColumnSpan(tbfile, 2);
			tbfile.Margin = new Avalonia.Thickness(0, 3, 0, 0);

			valuesGrid.Children.Add(label3);
			valuesGrid.Children.Add(tbrefgroup);
			valuesGrid.Children.Add(label4);
			valuesGrid.Children.Add(tbrefinst);
			valuesGrid.Children.Add(label5);
			valuesGrid.Children.Add(linkLabel1);
			valuesGrid.Children.Add(tbfile);

			// "Values" groupbox — same style as Reference tab "Settings" box
			var valuesHeaderBorder = new Avalonia.Controls.Border {
				Background = new Avalonia.Media.LinearGradientBrush {
					StartPoint = new Avalonia.RelativePoint(0, 0.5, Avalonia.RelativeUnit.Relative),
					EndPoint   = new Avalonia.RelativePoint(1, 0.5, Avalonia.RelativeUnit.Relative),
					GradientStops = { new Avalonia.Media.GradientStop(Avalonia.Media.Color.FromArgb(220, 60, 60, 80), 0.0), new Avalonia.Media.GradientStop(Avalonia.Media.Color.FromArgb(200, 80, 80, 110), 1.0) }
				},
				Child = new Avalonia.Controls.TextBlock { Text = "Values", Foreground = Avalonia.Media.Brushes.White, FontSize = 11, FontWeight = Avalonia.Media.FontWeight.SemiBold, Margin = new Avalonia.Thickness(6, 3) }
			};
			var valuesBox = new Avalonia.Controls.Border {
				VerticalAlignment = Avalonia.Layout.VerticalAlignment.Top,
				Background      = new Avalonia.Media.SolidColorBrush(Avalonia.Media.Color.FromRgb(220, 228, 238)),
				BorderBrush     = new Avalonia.Media.SolidColorBrush(Avalonia.Media.Color.FromRgb(170, 185, 205)),
				BorderThickness = new Avalonia.Thickness(1),
				CornerRadius    = new Avalonia.CornerRadius(3),
				Margin          = new Avalonia.Thickness(4),
				ClipToBounds    = true,
				Child = new Avalonia.Controls.StackPanel { Children = { valuesHeaderBorder, valuesGrid } }
			};

			var refDetails = new Avalonia.Controls.StackPanel {
				VerticalAlignment = Avalonia.Layout.VerticalAlignment.Top,
				MinWidth = 240,
				Children = { valuesBox }
			};

			var refPanel = new Grid();
			refPanel.ColumnDefinitions.Add(new ColumnDefinition(new GridLength(1, GridUnitType.Star)));
			refPanel.ColumnDefinitions.Add(new ColumnDefinition(GridLength.Auto));
			Grid.SetColumn(tv,         0);
			Grid.SetColumn(refDetails, 1);
			refPanel.Children.Add(tv);
			refPanel.Children.Add(refDetails);

			tpref = new Avalonia.Controls.TabItem { Header = "All References", Content = refPanel };

			// ── Content Tab (tabPage1) ─────────────────────────────────────
			label2   = new Avalonia.Controls.TextBlock { Text = "Blocklist:" };
			label1   = new Avalonia.Controls.TextBlock { Text = "Filename:" };
			cbitem   = new Avalonia.Controls.ComboBox();
			cbitem.SelectionChanged += SelectRcolItem;
			cbitem.MinHeight = 0;
			cbitem.Padding   = new Avalonia.Thickness(6, 2);
			cbitem.Height    = 24;
			cbitem.Background = Avalonia.Media.Brushes.White;

			tbflname = new Avalonia.Controls.TextBox();
			tbflname.TextChanged += ChangeFileName;
			tbflname.MinHeight  = 0;
			tbflname.Padding    = new Avalonia.Thickness(6, 2);
			tbflname.Height     = 24;
			tbflname.Background = Avalonia.Media.Brushes.White;

			llhash = new Avalonia.Controls.Button { Content = "assign Hash", IsVisible = false };
			llhash.Click += BuildFilename;
			llfix  = new Avalonia.Controls.Button { Content = "fix TGI" };
			llfix.Click  += FixTGI;

			childtc = new Avalonia.Controls.TabControl();
			childtc.FontSize = 11;
			childtc.SelectionChanged += ChildTabPageChanged;

			// Header grid: row 0 = "Blocklist:" | cbitem
			//              row 1 = "Filename:"  | tbflname | [llhash] [llfix]
			//              row 2 = childtc (star — gives it a bounded height for scrolling)
			var contentPanel = new Grid();
			contentPanel.RowDefinitions.Add(new RowDefinition(GridLength.Auto));
			contentPanel.RowDefinitions.Add(new RowDefinition(GridLength.Auto));
			contentPanel.RowDefinitions.Add(new RowDefinition(new GridLength(1, GridUnitType.Star)));
			contentPanel.ColumnDefinitions.Add(new ColumnDefinition(GridLength.Auto));
			contentPanel.ColumnDefinitions.Add(new ColumnDefinition(new GridLength(1, GridUnitType.Star)));
			contentPanel.ColumnDefinitions.Add(new ColumnDefinition(GridLength.Auto));

			// Row 0 — Blocklist label + ComboBox (spans all 3 columns so the dropdown uses full width)
			label2.VerticalAlignment = Avalonia.Layout.VerticalAlignment.Center;
			label2.Margin = new Avalonia.Thickness(0, 0, 4, 0);
			Grid.SetRow(label2, 0); Grid.SetColumn(label2, 0);
			Grid.SetRow(cbitem, 0); Grid.SetColumn(cbitem, 1); Grid.SetColumnSpan(cbitem, 2);
			contentPanel.Children.Add(label2);
			contentPanel.Children.Add(cbitem);

			// Row 1 — Filename label + TextBox + buttons
			label1.VerticalAlignment = Avalonia.Layout.VerticalAlignment.Center;
			label1.Margin = new Avalonia.Thickness(0, 2, 4, 2);
			tbflname.Margin = new Avalonia.Thickness(0, 2, 0, 2);
			Grid.SetRow(label1,   1); Grid.SetColumn(label1,   0);
			Grid.SetRow(tbflname, 1); Grid.SetColumn(tbflname, 1);
			contentPanel.Children.Add(label1);
			contentPanel.Children.Add(tbflname);

			var fnButtons = new StackPanel { Orientation = Orientation.Horizontal, Spacing = 4,
				Margin = new Avalonia.Thickness(4, 2, 0, 2) };
			fnButtons.Children.Add(llhash);
			fnButtons.Children.Add(llfix);
			Grid.SetRow(fnButtons, 1); Grid.SetColumn(fnButtons, 2);
			contentPanel.Children.Add(fnButtons);

			// Row 2 — inner TabControl (gets all remaining height)
			Grid.SetRow(childtc, 2); Grid.SetColumnSpan(childtc, 3);
			contentPanel.Children.Add(childtc);

			tabPage1 = new Avalonia.Controls.TabItem { Header = "Content", Content = contentPanel };

			// ── Reference Tab (tabPage2) ───────────────────────────────────
			lbref = new Avalonia.Controls.ListBox();
			lbref.SelectionChanged += SelectReference;
			DragDrop.SetAllowDrop(lbref, true);
			lbref.AddHandler(DragDrop.DropEvent,     new EventHandler<Avalonia.Input.DragEventArgs>(PackageItemDrop));
			lbref.AddHandler(DragDrop.DragOverEvent, new EventHandler<Avalonia.Input.DragEventArgs>(PackageItemDragEnter));

			label8  = new Avalonia.Controls.TextBlock { Text = "File Type:" };
			label9  = new Avalonia.Controls.TextBlock { Text = "Group:" };
			label10 = new Avalonia.Controls.TextBlock { Text = "Sub Typ:" };
			label11 = new Avalonia.Controls.TextBlock { Text = "Instance:" };
			tbtype     = new Avalonia.Controls.TextBox { Background = Avalonia.Media.Brushes.White };
			tbtype.TextChanged += tbtype_TextChanged;
			tbsubtype  = new Avalonia.Controls.TextBox { Background = Avalonia.Media.Brushes.White };
			tbsubtype.TextChanged += AutoChangeReference;
			tbgroup    = new Avalonia.Controls.TextBox { Background = Avalonia.Media.Brushes.White };
			tbgroup.TextChanged += AutoChangeReference;
			tbinstance = new Avalonia.Controls.TextBox { Background = Avalonia.Media.Brushes.White };
			tbinstance.TextChanged += AutoChangeReference;
			cbtypes    = new Avalonia.Controls.ComboBox { Background = Avalonia.Media.Brushes.White };
			cbtypes.SelectionChanged += SelectType;
			// Compact textbox heights to match the header rows
			foreach (var tb in new[] { tbtype, tbsubtype, tbgroup, tbinstance })
			{
				tb.MinHeight = 0; tb.Padding = new Avalonia.Thickness(4, 2); tb.Height = 24;
			}
			cbtypes.MinHeight = 0; cbtypes.Padding = new Avalonia.Thickness(4, 2); cbtypes.Height = 24;
			cbtypes.HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Stretch;

			// "add" and "delete" styled as link-buttons (no border, accent foreground)
			lladd    = new Avalonia.Controls.Button { Content = "add",    Padding = new Avalonia.Thickness(0), Background = Avalonia.Media.Brushes.Transparent, BorderThickness = new Avalonia.Thickness(0), Foreground = new Avalonia.Media.SolidColorBrush(Avalonia.Media.Color.FromRgb(0, 80, 180)), Cursor = new Avalonia.Input.Cursor(Avalonia.Input.StandardCursorType.Hand) };
			lladd.Click += SRNItemsAAdd;
			lldelete = new Avalonia.Controls.Button { Content = "delete", Padding = new Avalonia.Thickness(0), Background = Avalonia.Media.Brushes.Transparent, BorderThickness = new Avalonia.Thickness(0), Foreground = new Avalonia.Media.SolidColorBrush(Avalonia.Media.Color.FromRgb(0, 80, 180)), Cursor = new Avalonia.Input.Cursor(Avalonia.Input.StandardCursorType.Hand) };
			lldelete.Click += SRNItemsADelete;
			btref    = new Avalonia.Controls.Button { Content = "...", Padding = new Avalonia.Thickness(4, 2), Height = 24, IsVisible = false };
			btref.Click += ShowPackageSelector;

			// Aligned grid: label col (Auto) | input col (*)
			var fieldsGrid = new Avalonia.Controls.Grid { Margin = new Avalonia.Thickness(4), RowDefinitions = { new Avalonia.Controls.RowDefinition(Avalonia.Controls.GridLength.Auto), new Avalonia.Controls.RowDefinition(Avalonia.Controls.GridLength.Auto), new Avalonia.Controls.RowDefinition(Avalonia.Controls.GridLength.Auto), new Avalonia.Controls.RowDefinition(Avalonia.Controls.GridLength.Auto), new Avalonia.Controls.RowDefinition(Avalonia.Controls.GridLength.Auto) }, ColumnDefinitions = { new Avalonia.Controls.ColumnDefinition(Avalonia.Controls.GridLength.Auto), new Avalonia.Controls.ColumnDefinition(new Avalonia.Controls.GridLength(120)), new Avalonia.Controls.ColumnDefinition(new Avalonia.Controls.GridLength(1, Avalonia.Controls.GridUnitType.Star)) } };

			// Labels vertical alignment
			foreach (var lbl in new[] { label8, label9, label10, label11 })
			{
				lbl.VerticalAlignment = Avalonia.Layout.VerticalAlignment.Center;
				lbl.Margin = new Avalonia.Thickness(0, 0, 4, 2);
			}

			// Row 0: File Type | tbtype | cbtypes
			Avalonia.Controls.Grid.SetRow(label8,  0); Avalonia.Controls.Grid.SetColumn(label8,  0);
			Avalonia.Controls.Grid.SetRow(tbtype,  0); Avalonia.Controls.Grid.SetColumn(tbtype,  1);
			Avalonia.Controls.Grid.SetRow(cbtypes, 0); Avalonia.Controls.Grid.SetColumn(cbtypes, 2);
			tbtype.Margin = new Avalonia.Thickness(0, 0, 2, 2);
			// Row 1: Sub Typ | tbsubtype (span 2 cols)
			Avalonia.Controls.Grid.SetRow(label10,   1); Avalonia.Controls.Grid.SetColumn(label10,   0);
			Avalonia.Controls.Grid.SetRow(tbsubtype, 1); Avalonia.Controls.Grid.SetColumn(tbsubtype, 1);
			tbsubtype.Margin = new Avalonia.Thickness(0, 0, 0, 2);
			// Row 2: Group | tbgroup (span 2)
			Avalonia.Controls.Grid.SetRow(label9,   2); Avalonia.Controls.Grid.SetColumn(label9,   0);
			Avalonia.Controls.Grid.SetRow(tbgroup,  2); Avalonia.Controls.Grid.SetColumn(tbgroup,  1);
			tbgroup.Margin = new Avalonia.Thickness(0, 0, 0, 2);
			// Row 3: Instance | tbinstance | btref
			Avalonia.Controls.Grid.SetRow(label11,    3); Avalonia.Controls.Grid.SetColumn(label11,    0);
			Avalonia.Controls.Grid.SetRow(tbinstance, 3); Avalonia.Controls.Grid.SetColumn(tbinstance, 1);
			Avalonia.Controls.Grid.SetRow(btref,      3); Avalonia.Controls.Grid.SetColumn(btref,      2);
			tbinstance.Margin = new Avalonia.Thickness(0, 0, 2, 2);
			// Row 4: add / delete links (right-aligned)
			var addDelRow = new Avalonia.Controls.StackPanel { Orientation = Avalonia.Layout.Orientation.Horizontal, Spacing = 8, HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Right };
			addDelRow.Children.Add(lladd);
			addDelRow.Children.Add(lldelete);
			Avalonia.Controls.Grid.SetRow(addDelRow, 4); Avalonia.Controls.Grid.SetColumnSpan(addDelRow, 3);

			fieldsGrid.Children.Add(label8);   fieldsGrid.Children.Add(tbtype);    fieldsGrid.Children.Add(cbtypes);
			fieldsGrid.Children.Add(label10);  fieldsGrid.Children.Add(tbsubtype);
			fieldsGrid.Children.Add(label9);   fieldsGrid.Children.Add(tbgroup);
			fieldsGrid.Children.Add(label11);  fieldsGrid.Children.Add(tbinstance); fieldsGrid.Children.Add(btref);
			fieldsGrid.Children.Add(addDelRow);

			// "Settings" group box with dark header bar matching the main editor header
			var settingsHeader = new Avalonia.Controls.Border
			{
				Background = new Avalonia.Media.LinearGradientBrush
				{
					StartPoint = new Avalonia.RelativePoint(0, 0.5, Avalonia.RelativeUnit.Relative),
					EndPoint   = new Avalonia.RelativePoint(1, 0.5, Avalonia.RelativeUnit.Relative),
					GradientStops = { new Avalonia.Media.GradientStop(Avalonia.Media.Color.FromArgb(220, 60, 60, 80), 0.0), new Avalonia.Media.GradientStop(Avalonia.Media.Color.FromArgb(200, 80, 80, 110), 1.0) }
				},
				Child = new Avalonia.Controls.TextBlock { Text = "Settings", Foreground = Avalonia.Media.Brushes.White, FontSize = 11, FontWeight = Avalonia.Media.FontWeight.SemiBold, Margin = new Avalonia.Thickness(6, 3) }
			};
			var settingsBox = new Avalonia.Controls.Border
			{
				VerticalAlignment = Avalonia.Layout.VerticalAlignment.Top,
				Background      = new Avalonia.Media.SolidColorBrush(Avalonia.Media.Color.FromRgb(220, 228, 238)),
				BorderBrush     = new Avalonia.Media.SolidColorBrush(Avalonia.Media.Color.FromRgb(170, 185, 205)),
				BorderThickness = new Avalonia.Thickness(1),
				CornerRadius    = new Avalonia.CornerRadius(3),
				Margin          = new Avalonia.Thickness(4),
				Child           = new Avalonia.Controls.StackPanel { Children = { settingsHeader, fieldsGrid } }
			};

			pntypes = new Avalonia.Controls.Panel();
			pntypes.Children.Add(settingsBox);

			var refTabPanel = new Grid();
			refTabPanel.ColumnDefinitions.Add(new ColumnDefinition(new GridLength(1, GridUnitType.Star)));
			refTabPanel.ColumnDefinitions.Add(new ColumnDefinition(new GridLength(1, GridUnitType.Star)));
			Grid.SetColumn(lbref,   0);
			Grid.SetColumn(pntypes, 1);
			refTabPanel.Children.Add(lbref);
			refTabPanel.Children.Add(pntypes);

			tabPage2 = new Avalonia.Controls.TabItem { Header = "Reference", Content = refTabPanel };

			// ── Edit Blocks Tab (tabPage3) ─────────────────────────────────
			lbblocks = new Avalonia.Controls.ListBox();
			lbblocks.SelectionChanged += lbblocks_SelectedIndexChanged;

			btup   = new Avalonia.Controls.Button { Content = "Up",     IsEnabled = false };
			btdown = new Avalonia.Controls.Button { Content = "Down",   IsEnabled = false };
			btdel  = new Avalonia.Controls.Button { Content = "Delete", IsEnabled = false };
			btadd  = new Avalonia.Controls.Button { Content = "Add" };
			btup.Click    += btup_Click;
			btdown.Click  += btdown_Click;
			btdel.Click   += btdel_Click;
			btadd.Click   += btadd_Click;

			cbblocks = new Avalonia.Controls.ComboBox();
			cbblocks.MinHeight = 0;
			cbblocks.Padding   = new Avalonia.Thickness(4, 2);
			cbblocks.Height    = 24;
			cbblocks.HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Stretch;

			// Button order: Up, Down, [cbblocks], Add, Delete
			var blockButtons = new StackPanel { Orientation = Orientation.Vertical, Spacing = 4 };
			blockButtons.Children.Add(btup);
			blockButtons.Children.Add(btdown);
			blockButtons.Children.Add(cbblocks);
			blockButtons.Children.Add(btadd);
			blockButtons.Children.Add(btdel);

			// List ~1/3 screen width (fixed 420px), buttons auto-sized beside it
			var blocksPanel = new Grid { HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Left };
			blocksPanel.ColumnDefinitions.Add(new ColumnDefinition(new Avalonia.Controls.GridLength(420)));
			blocksPanel.ColumnDefinitions.Add(new ColumnDefinition(Avalonia.Controls.GridLength.Auto));
			Grid.SetColumn(lbblocks,     0);
			Grid.SetColumn(blockButtons, 1);
			blocksPanel.Children.Add(lbblocks);
			blocksPanel.Children.Add(blockButtons);

			tabPage3 = new Avalonia.Controls.TabItem { Header = "Edit Blocks", Content = blocksPanel };

			// ── Main TabControl ────────────────────────────────────────────
			tbResource = new Avalonia.Controls.TabControl();
			tbResource.FontSize = 11;
			tbResource.Items.Add(tabPage1);
			tbResource.Items.Add(tabPage2);
			tbResource.Items.Add(tabPage3);
			// tpref is added/removed dynamically by RcolUI
			tbResource.SelectionChanged += tabControl1_SelectedIndexChanged;

			// ── Root layout: real header bar + main TabControl ───────────────
			// Row 0 (HeaderHeight px): Border with dark gradient, white label left,
			//   Commit button right.
			// Row 1 (*): tbResource fills remaining height.
			btCommit.HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Right;
			btCommit.VerticalAlignment   = Avalonia.Layout.VerticalAlignment.Center;
			btCommit.Margin     = new Avalonia.Thickness(0, 0, 6, 0);
			btCommit.Padding    = new Avalonia.Thickness(8, 2);
			btCommit.FontSize   = 11;
			btCommit.Background = Avalonia.Media.Brushes.White;
			btCommit.Foreground = Avalonia.Media.Brushes.Black;

			headerLabel = new Avalonia.Controls.TextBlock
			{
				Foreground          = Avalonia.Media.Brushes.White,
				FontSize            = 11,
				FontWeight          = Avalonia.Media.FontWeight.SemiBold,
				VerticalAlignment   = Avalonia.Layout.VerticalAlignment.Center,
				Margin              = new Avalonia.Thickness(6, 0, 0, 0),
				Text                = HeaderText ?? ""
			};

			var headerInner = new Avalonia.Controls.Grid();
			headerInner.ColumnDefinitions.Add(new Avalonia.Controls.ColumnDefinition(
				new Avalonia.Controls.GridLength(1, Avalonia.Controls.GridUnitType.Star)));
			headerInner.ColumnDefinitions.Add(new Avalonia.Controls.ColumnDefinition(
				Avalonia.Controls.GridLength.Auto));
			Avalonia.Controls.Grid.SetColumn(headerLabel, 0);
			Avalonia.Controls.Grid.SetColumn(btCommit,    1);
			headerInner.Children.Add(headerLabel);
			headerInner.Children.Add(btCommit);

			var headerBorder = new Avalonia.Controls.Border
			{
				Height     = HeaderHeight,
				Background = new Avalonia.Media.LinearGradientBrush
				{
					StartPoint = new Avalonia.RelativePoint(0, 0.5, Avalonia.RelativeUnit.Relative),
					EndPoint   = new Avalonia.RelativePoint(1, 0.5, Avalonia.RelativeUnit.Relative),
					GradientStops =
					{
						new Avalonia.Media.GradientStop(Avalonia.Media.Color.FromArgb(220, 60, 60, 80), 0.0),
						new Avalonia.Media.GradientStop(Avalonia.Media.Color.FromArgb(200, 80, 80, 110), 1.0),
					}
				},
				Child = headerInner
			};

			// Wrap tbResource in a light-gray groupbox border
			var contentBorder = new Avalonia.Controls.Border
			{
				Background      = new Avalonia.Media.SolidColorBrush(Avalonia.Media.Color.FromRgb(220, 228, 238)),
				BorderBrush     = new Avalonia.Media.SolidColorBrush(Avalonia.Media.Color.FromRgb(170, 185, 205)),
				BorderThickness = new Avalonia.Thickness(1),
				CornerRadius    = new Avalonia.CornerRadius(3),
				Margin          = new Avalonia.Thickness(4),
				Child           = tbResource
			};

			var rootGrid = new Avalonia.Controls.Grid();
			rootGrid.Background = Avalonia.Media.Brushes.White;
			rootGrid.RowDefinitions.Add(new Avalonia.Controls.RowDefinition(
				Avalonia.Controls.GridLength.Auto));
			rootGrid.RowDefinitions.Add(new Avalonia.Controls.RowDefinition(
				new Avalonia.Controls.GridLength(1, Avalonia.Controls.GridUnitType.Star)));
			Avalonia.Controls.Grid.SetRow(headerBorder,  0);
			Avalonia.Controls.Grid.SetRow(contentBorder, 1);
			rootGrid.Children.Add(headerBorder);
			rootGrid.Children.Add(contentBorder);

			this.Content = rootGrid;
		}

		public new void Dispose()
		{
			// no-op
		}

		internal Rcol wrapper = null;

        internal void BuildChildTabControl(AbstractRcolBlock rb)
        {
            childtc.Items.Clear();

            if (rb == null) return;
            if (rb.TabPage != null) rb.AddToTabControl(childtc);
        }

        private void SelectRcolItem(object sender, Avalonia.Controls.SelectionChangedEventArgs e)
		{
			if (cbitem.Tag != null) return;
			if (cbitem.SelectedIndex < 0) return;
			try
			{
				cbitem.Tag = true;
				SimPe.CountedListItem cli = (SimPe.CountedListItem)cbitem.Items[cbitem.SelectedIndex];
				AbstractRcolBlock rb = (AbstractRcolBlock)cli.Object;
				tbflname.IsEnabled = (rb.NameResource != null);
				llhash.IsEnabled   = tbflname.IsEnabled;
				llfix.IsEnabled    = tbflname.IsEnabled;

				if (rb.NameResource != null) tbflname.Text = rb.NameResource.FileName;
				else tbflname.Text = "";

				BuildChildTabControl(rb);
			}
			catch (Exception ex)
			{
				Helper.ExceptionMessage("", ex);
			}
			finally
			{
				cbitem.Tag = null;
			}
		}

		private void ChangeFileName(object sender, System.EventArgs e)
		{
			if (cbitem.Tag != null) return;
			if (cbitem.SelectedIndex < 0) return;
			try
			{
				cbitem.Tag = true;
				SimPe.CountedListItem cli = (SimPe.CountedListItem)cbitem.Items[cbitem.SelectedIndex];
				AbstractRcolBlock rb = (AbstractRcolBlock)cli.Object;
				if (rb.NameResource != null)
				{
					rb.NameResource.FileName = tbflname.Text;
					cbitem.Items[cbitem.SelectedIndex] = cli;
					// No cbitem.Text in Avalonia – selection label is handled by ComboBox itself
				}
			}
			catch (Exception ex)
			{
				Helper.ExceptionMessage(Localization.Manager.GetString("erropenfile"), ex);
			}
			finally
			{
				cbitem.Tag = null;
			}
		}

		private void BuildFilename(object sender, Avalonia.Interactivity.RoutedEventArgs e)
		{
			string fl = Hashes.StripHashFromName(this.tbflname.Text);
			this.tbflname.Text = Hashes.AssembleHashedFileName(wrapper.Package.FileGroupHash, fl);
		}

		private void FixTGI(object sender, Avalonia.Interactivity.RoutedEventArgs e)
		{
			string fl = Hashes.StripHashFromName(this.tbflname.Text);
			if (wrapper != null)
				if (wrapper.FileDescriptor != null)
				{
					wrapper.FileDescriptor.Instance = Hashes.InstanceHash(fl);
					wrapper.FileDescriptor.SubType  = Hashes.SubTypeHash(fl);
				}
		}

		private void Commit(object sender, System.EventArgs e)
		{
			try
			{
				wrapper.SynchronizeUserData();
				Message.Show(Localization.Manager.GetString("commited"), "Commit", MessageBoxButtons.OK);
			}
			catch (Exception ex)
			{
				Helper.ExceptionMessage(Localization.Manager.GetString("errwritingfile"), ex);
			}
		}

		private void SelectType(object sender, Avalonia.Controls.SelectionChangedEventArgs e)
		{
			if (cbtypes.Tag != null) return;
			tbtype.Text = "0x" + Helper.HexString(((SimPe.Data.TypeAlias)cbtypes.Items[cbtypes.SelectedIndex]).Id);
		}

		protected void Change()
		{
			if (lbref.Tag != null) return;
			if (lbref.SelectedIndex < 0) return;
			try
			{
				lbref.Tag = true;
				Interfaces.Files.IPackedFileDescriptor pfd = (Interfaces.Files.IPackedFileDescriptor)this.lbref.Items[lbref.SelectedIndex];

				pfd.Type     = Convert.ToUInt32(this.tbtype.Text, 16);
				pfd.SubType  = Convert.ToUInt32(this.tbsubtype.Text, 16);
				pfd.Group    = Convert.ToUInt32(this.tbgroup.Text, 16);
				pfd.Instance = Convert.ToUInt32(this.tbinstance.Text, 16);

				lbref.Items[lbref.SelectedIndex] = pfd;
			}
			catch (Exception)
			{
				// suppress
			}
			finally
			{
				lbref.Tag = null;
			}
		}

		private void tbtype_TextChanged(object sender, System.EventArgs e)
		{
			Change();

			cbtypes.Tag = true;
			Data.TypeAlias a = Data.MetaData.FindTypeAlias(Helper.HexStringToUInt(tbtype.Text));

			int ct = 0;
			foreach (Data.TypeAlias i in cbtypes.Items)
			{
				if (i == a)
				{
					cbtypes.SelectedIndex = ct;
					cbtypes.Tag = null;
					return;
				}
				ct++;
			}

			cbtypes.SelectedIndex = -1;
			cbtypes.Tag = null;
		}

		private void SelectReference(object sender, Avalonia.Controls.SelectionChangedEventArgs e)
		{
			if (lbref.Tag != null) return;
			if (lbref.SelectedIndex < 0) return;
			try
			{
				lbref.Tag = true;
				Interfaces.Files.IPackedFileDescriptor pfd = (Interfaces.Files.IPackedFileDescriptor)this.lbref.Items[lbref.SelectedIndex];
				this.tbtype.Text     = "0x" + Helper.HexString(pfd.Type);
				this.tbsubtype.Text  = "0x" + Helper.HexString(pfd.SubType);
				this.tbgroup.Text    = "0x" + Helper.HexString(pfd.Group);
				this.tbinstance.Text = "0x" + Helper.HexString(pfd.Instance);
			}
			catch (Exception ex)
			{
				Helper.ExceptionMessage("", ex);
			}
			finally
			{
				lbref.Tag = null;
			}
		}

		private void AutoChangeReference(object sender, System.EventArgs e)
		{
			Change();
		}

		private void SRNItemsAAdd(object sender, Avalonia.Interactivity.RoutedEventArgs e)
		{
			try
			{
				Interfaces.Files.IPackedFileDescriptor pfd = new SimPe.Packages.PackedFileDescriptor();

				pfd.Type     = Convert.ToUInt32(this.tbtype.Text, 16);
				pfd.SubType  = Convert.ToUInt32(this.tbsubtype.Text, 16);
				pfd.Group    = Convert.ToUInt32(this.tbgroup.Text, 16);
				pfd.Instance = Convert.ToUInt32(this.tbinstance.Text, 16);

				wrapper.ReferencedFiles = (Interfaces.Files.IPackedFileDescriptor[])Helper.Add(wrapper.ReferencedFiles, pfd);
				this.lbref.Items.Add(pfd);

				wrapper.Changed = true;
			}
			catch (Exception ex)
			{
				Helper.ExceptionMessage("", ex);
			}
		}

		private void SRNItemsADelete(object sender, Avalonia.Interactivity.RoutedEventArgs e)
		{
			if (lbref.SelectedIndex < 0) return;
			try
			{
				Interfaces.Files.IPackedFileDescriptor pfd = (Interfaces.Files.IPackedFileDescriptor)this.lbref.Items[lbref.SelectedIndex];

				wrapper.ReferencedFiles = (Interfaces.Files.IPackedFileDescriptor[])Helper.Delete(wrapper.ReferencedFiles, pfd);
				lbref.Items.Remove(pfd);

				wrapper.Changed = true;

				btup.IsEnabled = btdown.IsEnabled = btdel.IsEnabled = false;
			}
			catch (Exception ex)
			{
				Helper.ExceptionMessage("", ex);
			}
		}

		#region Package Selector
		private void ShowPackageSelector(object sender, Avalonia.Interactivity.RoutedEventArgs e)
		{
			SimPe.PackageSelectorForm form = new SimPe.PackageSelectorForm();
			form.Execute(wrapper.Package);
		}

		private void PackageItemDragEnter(object sender, Avalonia.Input.DragEventArgs e)
		{
			if (e.Data.Contains(typeof(SimPe.Packages.PackedFileDescriptor).FullName))
			{
				e.DragEffects = Avalonia.Input.DragDropEffects.Copy;
			}
			else
			{
				e.DragEffects = Avalonia.Input.DragDropEffects.None;
			}
		}

		private void PackageItemDrop(object sender, Avalonia.Input.DragEventArgs e)
		{
			try
			{
				lbref.Tag = true;
				Interfaces.Files.IPackedFileDescriptor pfd = null;
				pfd = e.Data.Get(typeof(SimPe.Packages.PackedFileDescriptor).FullName) as Interfaces.Files.IPackedFileDescriptor;

				wrapper.ReferencedFiles = (Interfaces.Files.IPackedFileDescriptor[])Helper.Add(wrapper.ReferencedFiles, pfd);
				this.lbref.Items.Add(pfd);

				wrapper.Changed = true;
			}
			catch (Exception ex)
			{
				Helper.ExceptionMessage("", ex);
			}
			finally
			{
				lbref.Tag = null;
			}
		}
		#endregion

		protected void UpdateComboBox()
		{
			this.cbitem.Items.Clear();

			this.tbflname.Text = "";
			this.childtc.Items.Clear();
			foreach (SimPe.CountedListItem o in this.lbblocks.Items) cbitem.Items.Add(o);

			if (cbitem.Items.Count > 0) cbitem.SelectedIndex = 0;
		}

		private void tabControl1_SelectedIndexChanged(object sender, Avalonia.Controls.SelectionChangedEventArgs e)
		{
			// Ignore events that bubbled up from child controls (e.g. cbblocks, lbblocks).
			// In Avalonia, SelectionChanged is a routed event that bubbles; WinForms never did this.
			if (!ReferenceEquals(e.Source, tbResource)) return;

			// Display the Block Editor when tabPage3 is selected
			if (tbResource.SelectedIndex >= 0 && tbResource.Items[tbResource.SelectedIndex] == this.tabPage3)
			{
				this.lbblocks.Items.Clear();
				foreach (IRcolBlock irb in wrapper.Blocks) SimPe.CountedListItem.AddHex(lbblocks, irb);

				this.cbblocks.Items.Clear();
				foreach (string s in Rcol.Tokens.Keys)
				{
					try
					{
						Type t = (Type)Rcol.Tokens[s];
						IRcolBlock irb = AbstractRcolBlock.Create(t, null);
						cbblocks.Items.Add(irb);
					}
					catch (Exception ex)
					{
						Helper.ExceptionMessage("Error in Block " + s, ex);
					}
				}
				if (cbblocks.Items.Count > 0) cbblocks.SelectedIndex = 0;
			}
		}

		private void btup_Click(object sender, Avalonia.Interactivity.RoutedEventArgs e)
		{
			if (lbblocks.SelectedIndex < 1) return;
			try
			{
				object o = lbblocks.Items[lbblocks.SelectedIndex - 1];
				lbblocks.Items[lbblocks.SelectedIndex - 1] = lbblocks.Items[lbblocks.SelectedIndex];
				lbblocks.Items[lbblocks.SelectedIndex]     = o;

				wrapper.Blocks[lbblocks.SelectedIndex]     = (AbstractRcolBlock)((SimPe.CountedListItem)lbblocks.Items[lbblocks.SelectedIndex]).Object;
				wrapper.Blocks[lbblocks.SelectedIndex - 1] = (AbstractRcolBlock)((SimPe.CountedListItem)lbblocks.Items[lbblocks.SelectedIndex - 1]).Object;
				lbblocks.SelectedIndex--;

				UpdateComboBox();
			}
			catch (Exception ex)
			{
				Helper.ExceptionMessage("", ex);
			}
		}

		private void btdown_Click(object sender, Avalonia.Interactivity.RoutedEventArgs e)
		{
			if (lbblocks.SelectedIndex < 0) return;
			if (lbblocks.SelectedIndex > lbblocks.Items.Count - 2) return;
			try
			{
				object o = lbblocks.Items[lbblocks.SelectedIndex + 1];
				lbblocks.Items[lbblocks.SelectedIndex + 1] = lbblocks.Items[lbblocks.SelectedIndex];
				lbblocks.Items[lbblocks.SelectedIndex]     = o;
				wrapper.Blocks[lbblocks.SelectedIndex]     = (AbstractRcolBlock)((SimPe.CountedListItem)lbblocks.Items[lbblocks.SelectedIndex]).Object;
				wrapper.Blocks[lbblocks.SelectedIndex + 1] = (AbstractRcolBlock)((SimPe.CountedListItem)lbblocks.Items[lbblocks.SelectedIndex + 1]).Object;
				lbblocks.SelectedIndex++;

				UpdateComboBox();
			}
			catch (Exception ex)
			{
				Helper.ExceptionMessage("", ex);
			}
		}

		private void btadd_Click(object sender, Avalonia.Interactivity.RoutedEventArgs e)
		{
			try
			{
				IRcolBlock irb = ((IRcolBlock)cbblocks.Items[cbblocks.SelectedIndex]).Create();
				if (irb is AbstractRcolBlock) ((AbstractRcolBlock)irb).Parent = wrapper;
				SimPe.CountedListItem.AddHex(this.lbblocks, irb);
				wrapper.Blocks = (IRcolBlock[])Helper.Add(wrapper.Blocks, irb, typeof(IRcolBlock));
				UpdateComboBox();
			}
			catch (Exception ex)
			{
				Helper.ExceptionMessage("", ex);
			}
		}

		private void lbblocks_SelectedIndexChanged(object sender, Avalonia.Controls.SelectionChangedEventArgs e)
		{
			btup.IsEnabled = btdown.IsEnabled = btdel.IsEnabled = false;
			if (lbblocks.SelectedIndex < 0) return;
			btup.IsEnabled = btdown.IsEnabled = btdel.IsEnabled = true;
		}

		private void btdel_Click(object sender, Avalonia.Interactivity.RoutedEventArgs e)
		{
			if (lbblocks.SelectedIndex < 0) return;
			try
			{
				SimPe.CountedListItem cli = (SimPe.CountedListItem)lbblocks.Items[lbblocks.SelectedIndex];
				IRcolBlock irb = ((IRcolBlock)cli.Object);
				this.lbblocks.Items.Remove(cli);
				wrapper.Blocks = (IRcolBlock[])Helper.Delete(wrapper.Blocks, irb, typeof(IRcolBlock));

				UpdateComboBox();
			}
			catch (Exception ex)
			{
				Helper.ExceptionMessage("", ex);
			}
		}

		private void SelectRefItem(object sender, Avalonia.Controls.SelectionChangedEventArgs e)
		{
			Avalonia.Controls.TreeViewItem node = (Avalonia.Controls.TreeViewItem)tv.SelectedItem;
			if (node == null) return;
			if (node.Tag != null)
			{
				Interfaces.Files.IPackedFileDescriptor pfd = (Interfaces.Files.IPackedFileDescriptor)node.Tag;
				tbrefgroup.Text = "0x" + Helper.HexString(pfd.Group);
				tbrefinst.Text  = "0x" + Helper.HexString(pfd.Instance);

				SimPe.FileTable.FileIndex.Load();
				SimPe.Interfaces.Scenegraph.IScenegraphFileIndexItem[] items = FileTable.FileIndex.FindFile(pfd, null);
				if (items.Length == 0)
				{
					SimPe.Interfaces.Scenegraph.IScenegraphFileIndexItem item = FileTable.FileIndex.FindFileByName(pfd.Filename, pfd.Type, pfd.Group, true);
					if (item != null)
					{
						items = new SimPe.Interfaces.Scenegraph.IScenegraphFileIndexItem[1];
						items[0] = item;
					}
				}
				if (items.Length == 0)
				{
					Interfaces.Files.IPackedFileDescriptor npfd = (Interfaces.Files.IPackedFileDescriptor)pfd.Clone();
					npfd.SubType = 0;
					items = FileTable.FileIndex.FindFile(npfd, null);
				}

				if (items.Length > 0)
				{
					tbfile.Text = items[0].Package.FileName;
				}
				else
				{
					tbfile.Text = "[unreferenced]";
				}
			}
		}

		private void linkLabel1_LinkClicked(object sender, Avalonia.Interactivity.RoutedEventArgs e)
		{
			WaitingScreen.Wait();
			try   { SimPe.FileTable.FileIndex.ForceReload(); }
			finally { WaitingScreen.Stop(); }
		}

		private void ChildTabPageChanged(object sender, Avalonia.Controls.SelectionChangedEventArgs e)
		{
			wrapper.ChildTabPageChanged(this, e);
		}

		internal void ClearControlTags()
		{
			// In Avalonia we do not iterate WinForms Control.Controls;
			// tags are cleared on individual controls as needed.
			cbitem.Tag   = null;
			lbref.Tag    = null;
			cbtypes.Tag  = null;
		}
	}
}
