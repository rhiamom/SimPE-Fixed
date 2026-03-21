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

// Ported from WinForms Form to Avalonia UserControl stub (Mac port).
// All business logic lives in cGeometryDataContainer.cs — this file only
// provides the UI scaffold and exposes the named fields that cGeometryDataContainer
// accesses.  WinForms layout code (InitializeComponent / designer code) is removed;
// controls are created directly.

using System;
using System.IO;
using System.Globalization;
using System.Collections.Generic;
using Avalonia.Controls;
using Avalonia.Layout;
using SimPe.Plugin.Gmdc;
using SimPe.Geometry;

namespace SimPe.Plugin
{
    // ── Minimal CheckedListBox stub ─────────────────────────────────────────────
    // Avalonia has no built-in CheckedListBox; we provide a data-model stub.
    internal class CheckedListBox
    {
        public CheckedListBoxItemCollection Items { get; } = new CheckedListBoxItemCollection();
    }

    internal class CheckedListBoxItemCollection
    {
        private readonly List<(object Item, bool Checked)> _list = new();
        public int Count => _list.Count;
        public void Add(object item, bool isChecked) { _list.Add((item, isChecked)); }
        public void Add(object item)                  { _list.Add((item, false)); }
        public void Clear()                           { _list.Clear(); }
        public object this[int i] => _list[i].Item;
        public bool GetItemChecked(int i) => _list[i].Checked;
    }

    // ── fGeometryDataContainer ──────────────────────────────────────────────────
    /// <summary>
    /// Avalonia UserControl replacement for the WinForms fGeometryDataContainer Form.
    /// Exposes only the fields accessed by cGeometryDataContainer (all internal).
    /// </summary>
    internal class fGeometryDataContainer : Avalonia.Controls.UserControl, IDisposable
    {
        // ── TabItems (were WinForms TabPages) ───────────────────────────────────
        internal Avalonia.Controls.TabItem tMesh;
        internal Avalonia.Controls.TabItem tGeometryDataContainer;
        internal Avalonia.Controls.TabItem tGeometryDataContainer2;
        internal Avalonia.Controls.TabItem tGeometryDataContainer3;
        internal Avalonia.Controls.TabItem tModel;
        internal Avalonia.Controls.TabItem tSubset;
        internal Avalonia.Controls.TabItem tAdvncd;

        // ── Settings ────────────────────────────────────────────────────────────
        internal Avalonia.Controls.TextBox tb_ver;

        // ── Elements tab ────────────────────────────────────────────────────────
        internal Avalonia.Controls.TextBlock label_elements;
        internal Avalonia.Controls.ListBox   list_elements;
        internal Avalonia.Controls.TextBlock label_links;
        internal Avalonia.Controls.ListBox   list_links;
        internal Avalonia.Controls.TextBlock label_groups;
        internal Avalonia.Controls.ListBox   list_groups;
        internal Avalonia.Controls.TextBlock label_subsets;
        internal Avalonia.Controls.ListBox   list_subsets;

        // ── Element detail fields (tGeometryDataContainer) ──────────────────────
        internal Avalonia.Controls.ListBox lb_itemsa;
        internal Avalonia.Controls.ListBox lb_itemsa2;
        internal Avalonia.Controls.TextBox tb_itemsa2;
        internal Avalonia.Controls.TextBox tb_uk1;
        internal Avalonia.Controls.TextBox tb_uk5;
        internal Avalonia.Controls.TextBox tb_id;
        internal Avalonia.Controls.TextBox tb_mod1;
        internal Avalonia.Controls.TextBox tb_mod2;
        internal Avalonia.Controls.ListBox lb_itemsa1;
        internal Avalonia.Controls.ComboBox cbblock;
        internal Avalonia.Controls.ComboBox cbset;
        internal Avalonia.Controls.ComboBox cbid;

        // ── Group tab (tGeometryDataContainer3) ─────────────────────────────────
        internal Avalonia.Controls.ListBox lb_itemsc;
        internal Avalonia.Controls.TextBox tb_itemsc_name;
        internal Avalonia.Controls.TextBox tb_opacity;
        internal Avalonia.Controls.TextBox tb_uk2;
        internal Avalonia.Controls.TextBox tb_uk3;
        internal Avalonia.Controls.ListBox lb_itemsc2;
        internal Avalonia.Controls.TextBox tb_itemsc2;
        internal Avalonia.Controls.ListBox lb_itemsc3;
        internal Avalonia.Controls.TextBox tb_itemsc3;
        internal Avalonia.Controls.ComboBox cbGroupJoint;
        internal CheckedListBox lbmodel;
        internal Avalonia.Controls.TextBlock lb_models;

        // ── Link tab (tGeometryDataContainer2) ──────────────────────────────────
        internal Avalonia.Controls.ListBox lb_itemsb;
        internal Avalonia.Controls.ListBox lb_itemsb2;
        internal Avalonia.Controls.ListBox lb_itemsb3;
        internal Avalonia.Controls.ListBox lb_itemsb4;
        internal Avalonia.Controls.ListBox lb_itemsb5;
        internal Avalonia.Controls.TextBox tb_itemsb2;
        internal Avalonia.Controls.TextBox tb_itemsb3;
        internal Avalonia.Controls.TextBox tb_itemsb4;
        internal Avalonia.Controls.TextBox tb_itemsb5;
        internal Avalonia.Controls.TextBox tb_uk4;
        internal Avalonia.Controls.TextBox tb_uk6;

        // ── Subset tab ──────────────────────────────────────────────────────────
        internal Avalonia.Controls.ListBox lb_subsets;
        internal Avalonia.Controls.ListBox lb_sub_items;
        internal Avalonia.Controls.ListBox lb_sub_faces;

        // ── Model tab ───────────────────────────────────────────────────────────
        internal Avalonia.Controls.ListBox lb_model_trans;
        internal Avalonia.Controls.ListBox lb_model_names;
        internal Avalonia.Controls.ListBox lb_model_faces;
        internal Avalonia.Controls.ListBox lb_model_items;

        // ── Advanced tab ────────────────────────────────────────────────────────
        internal Avalonia.Controls.CheckBox cbCorrect;
        internal Avalonia.Controls.ComboBox cbaxis;

        // ── RenderSelection stub ─────────────────────────────────────────────────
        private Ambertation.Graphics.RenderSelection scenesel;

        // ── static default axis index ────────────────────────────────────────────
        internal static int DefaultSelectedAxisIndex = -1;

        internal fGeometryDataContainer()
        {
            BuildControls();
            BuildLayout();
        }

        private void BuildControls()
        {
            // TabItems
            tMesh                   = new TabItem { Header = "Mesh" };
            tGeometryDataContainer  = new TabItem { Header = "Elements" };
            tGeometryDataContainer2 = new TabItem { Header = "Links" };
            tGeometryDataContainer3 = new TabItem { Header = "Groups" };
            tModel                  = new TabItem { Header = "Model" };
            tSubset                 = new TabItem { Header = "Subsets" };
            tAdvncd                 = new TabItem { Header = "Advanced" };

            // Settings
            tb_ver = new TextBox { Text = "0x00000000" };
            tb_ver.TextChanged += SettingsChange;

            // Elements tab
            label_elements = new TextBlock { Text = "Elements:" };
            list_elements  = new ListBox();
            label_links    = new TextBlock { Text = "Links:" };
            list_links     = new ListBox();
            label_groups   = new TextBlock { Text = "Groups:" };
            list_groups    = new ListBox();
            label_subsets  = new TextBlock { Text = "Joints:" };
            list_subsets   = new ListBox();

            // Element detail
            lb_itemsa  = new ListBox(); lb_itemsa.SelectionChanged  += SelectItemsA;
            lb_itemsa2 = new ListBox(); lb_itemsa2.SelectionChanged += SelectItemsA2;
            tb_itemsa2 = new TextBox { Text = "0x00000000", IsReadOnly = true };
            tb_uk1     = new TextBox { IsReadOnly = true };
            tb_uk5     = new TextBox { IsReadOnly = true };
            tb_id      = new TextBox { IsReadOnly = true };
            tb_mod1    = new TextBox { IsReadOnly = true };
            tb_mod2    = new TextBox { IsReadOnly = true };
            lb_itemsa1 = new ListBox();
            cbblock    = new ComboBox();
            cbset      = new ComboBox();
            cbid       = new ComboBox();

            // Group tab
            lb_itemsc      = new ListBox(); lb_itemsc.SelectionChanged += SelectItemsC;
            tb_itemsc_name = new TextBox();
            tb_opacity     = new TextBox();
            tb_uk2         = new TextBox();
            tb_uk3         = new TextBox();
            lb_itemsc2     = new ListBox(); lb_itemsc2.SelectionChanged += SelectItemsC2;
            tb_itemsc2     = new TextBox { IsReadOnly = true };
            lb_itemsc3     = new ListBox(); lb_itemsc3.SelectionChanged += SelectItemsC3;
            tb_itemsc3     = new TextBox { IsReadOnly = true };
            cbGroupJoint   = new ComboBox();
            lbmodel        = new CheckedListBox();
            lb_models      = new TextBlock { Text = "Models:" };

            // Link tab
            lb_itemsb  = new ListBox(); lb_itemsb.SelectionChanged  += SelectItemsB;
            lb_itemsb2 = new ListBox(); lb_itemsb2.SelectionChanged += SelectItemsB2;
            lb_itemsb3 = new ListBox(); lb_itemsb3.SelectionChanged += SelectItemsB3;
            lb_itemsb4 = new ListBox(); lb_itemsb4.SelectionChanged += SelectItemsB4;
            lb_itemsb5 = new ListBox(); lb_itemsb5.SelectionChanged += SelectItemsB5;
            tb_itemsb2 = new TextBox { IsReadOnly = true };
            tb_itemsb3 = new TextBox { IsReadOnly = true };
            tb_itemsb4 = new TextBox { IsReadOnly = true };
            tb_itemsb5 = new TextBox { IsReadOnly = true };
            tb_uk4     = new TextBox();
            tb_uk6     = new TextBox();

            // Subset tab
            lb_subsets    = new ListBox(); lb_subsets.SelectionChanged    += SelectSubset;
            lb_sub_items  = new ListBox();
            lb_sub_faces  = new ListBox();

            // Model tab
            lb_model_trans = new ListBox();
            lb_model_names = new ListBox();
            lb_model_faces = new ListBox();
            lb_model_items = new ListBox();

            // Advanced tab
            cbCorrect = new CheckBox { Content = "Correct Joint Definition on Export" };
            cbCorrect.IsCheckedChanged += cbCorrect_CheckedChanged;
            cbaxis = new ComboBox();
            cbaxis.SelectionChanged += cbaxis_SelectionChanged;

            // RenderSelection stub
            scenesel = new Ambertation.Graphics.RenderSelection();
        }

        private void BuildLayout()
        {
            // ── tGeometryDataContainer (Elements) ───────────────────────────────
            var elemPanel = new StackPanel { Spacing = 4 };
            elemPanel.Children.Add(tb_ver);
            elemPanel.Children.Add(label_elements); elemPanel.Children.Add(list_elements);
            elemPanel.Children.Add(label_links);    elemPanel.Children.Add(list_links);
            elemPanel.Children.Add(label_groups);   elemPanel.Children.Add(list_groups);
            elemPanel.Children.Add(label_subsets);  elemPanel.Children.Add(list_subsets);
            tGeometryDataContainer.Content = elemPanel;

            // ── tGeometryDataContainer2 (Links) ─────────────────────────────────
            var linkPanel = new StackPanel { Spacing = 4 };
            linkPanel.Children.Add(lb_itemsb);
            linkPanel.Children.Add(lb_itemsb2); linkPanel.Children.Add(tb_itemsb2);
            linkPanel.Children.Add(lb_itemsb3); linkPanel.Children.Add(tb_itemsb3);
            linkPanel.Children.Add(lb_itemsb4); linkPanel.Children.Add(tb_itemsb4);
            linkPanel.Children.Add(lb_itemsb5); linkPanel.Children.Add(tb_itemsb5);
            tGeometryDataContainer2.Content = linkPanel;

            // ── tGeometryDataContainer3 (Groups) ────────────────────────────────
            var grpPanel = new StackPanel { Spacing = 4 };
            grpPanel.Children.Add(lb_models);
            grpPanel.Children.Add(lb_itemsc);
            grpPanel.Children.Add(tb_itemsc_name);
            grpPanel.Children.Add(lb_itemsc2); grpPanel.Children.Add(tb_itemsc2);
            grpPanel.Children.Add(lb_itemsc3); grpPanel.Children.Add(tb_itemsc3);
            grpPanel.Children.Add(cbGroupJoint);
            tGeometryDataContainer3.Content = grpPanel;

            // ── tModel ──────────────────────────────────────────────────────────
            var modelPanel = new StackPanel { Spacing = 4 };
            modelPanel.Children.Add(lb_model_trans);
            modelPanel.Children.Add(lb_model_names);
            modelPanel.Children.Add(lb_model_faces);
            modelPanel.Children.Add(lb_model_items);
            tModel.Content = modelPanel;

            // ── tSubset ─────────────────────────────────────────────────────────
            var subsetPanel = new StackPanel { Spacing = 4 };
            subsetPanel.Children.Add(lb_subsets);
            subsetPanel.Children.Add(lb_sub_items);
            subsetPanel.Children.Add(lb_sub_faces);
            tSubset.Content = subsetPanel;

            // ── tAdvncd ─────────────────────────────────────────────────────────
            var advPanel = new StackPanel { Spacing = 4 };
            advPanel.Children.Add(lb_itemsa);
            advPanel.Children.Add(lb_itemsa2);
            advPanel.Children.Add(tb_itemsa2);
            advPanel.Children.Add(lb_itemsa1);
            advPanel.Children.Add(cbblock); advPanel.Children.Add(cbset); advPanel.Children.Add(cbid);
            advPanel.Children.Add(cbCorrect);
            advPanel.Children.Add(cbaxis);
            tAdvncd.Content = advPanel;

            // ── tMesh (primary TabItem returned by TabPage property) ─────────────
            var meshPanel = new StackPanel { Spacing = 4 };
            meshPanel.Children.Add(new TextBlock { Text = "Mesh View" });
            tMesh.Content = meshPanel;

            // ── Outer TabControl ─────────────────────────────────────────────────
            var tc = new TabControl();
            tc.Items.Add(tMesh);
            tc.Items.Add(tGeometryDataContainer);
            tc.Items.Add(tGeometryDataContainer2);
            tc.Items.Add(tGeometryDataContainer3);
            tc.Items.Add(tModel);
            tc.Items.Add(tSubset);
            tc.Items.Add(tAdvncd);
            Content = tc;
        }

        // ── Public methods called by cGeometryDataContainer ─────────────────────

        internal void ResetPreview()
        {
            // no-op stub — 3D preview not yet implemented in Avalonia port
        }

        // ── Event handlers (no-op stubs — business logic is in cGeometryDataContainer) ──

        private void SettingsChange(object sender, Avalonia.Controls.TextChangedEventArgs e) { }
        private void SelectItemsA(object sender, Avalonia.Controls.SelectionChangedEventArgs e) { }
        private void SelectItemsA2(object sender, Avalonia.Controls.SelectionChangedEventArgs e) { }
        private void SelectItemsB(object sender, Avalonia.Controls.SelectionChangedEventArgs e) { }
        private void SelectItemsB2(object sender, Avalonia.Controls.SelectionChangedEventArgs e) { }
        private void SelectItemsB3(object sender, Avalonia.Controls.SelectionChangedEventArgs e) { }
        private void SelectItemsB4(object sender, Avalonia.Controls.SelectionChangedEventArgs e) { }
        private void SelectItemsB5(object sender, Avalonia.Controls.SelectionChangedEventArgs e) { }
        private void SelectItemsC(object sender, Avalonia.Controls.SelectionChangedEventArgs e) { }
        private void SelectItemsC2(object sender, Avalonia.Controls.SelectionChangedEventArgs e) { }
        private void SelectItemsC3(object sender, Avalonia.Controls.SelectionChangedEventArgs e) { }
        private void SelectSubset(object sender, Avalonia.Controls.SelectionChangedEventArgs e) { }
        private void cbCorrect_CheckedChanged(object sender, Avalonia.Interactivity.RoutedEventArgs e) { }
        private void cbaxis_SelectionChanged(object sender, Avalonia.Controls.SelectionChangedEventArgs e) { }

        // ── Static export/import stubs ───────────────────────────────────────────
        // Called from AnimMeshBlockControl. File dialogs are no-ops in Avalonia port.
        internal static void StartExport(GeometryDataContainer gdc, string ext,
            GmdcGroups groups, SimPe.Plugin.Gmdc.ElementSorting sorting, bool correct)
        {
            // TODO: implement async file dialog when Avalonia StorageProvider is wired up
            System.Diagnostics.Trace.TraceInformation("[fGeometryDataContainer] StartExport (stub)");
        }

        internal static void StartImport(GeometryDataContainer gdc, string ext,
            SimPe.Plugin.Gmdc.ElementSorting sorting, bool correct)
        {
            // TODO: implement async file dialog when Avalonia StorageProvider is wired up
            System.Diagnostics.Trace.TraceInformation("[fGeometryDataContainer] StartImport (stub)");
        }

        // ── IDisposable ──────────────────────────────────────────────────────────
        public void Dispose() { }
    }
}
