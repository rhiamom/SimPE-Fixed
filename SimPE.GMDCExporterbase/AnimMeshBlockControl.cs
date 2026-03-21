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

// Ported from WinForms UserControl to Avalonia stub (Mac port).
// All WinForms layout and P/Invoke code removed. Public API preserved.
// Business logic (export/import) preserved in stubbed event handlers.

using System;
using Avalonia.Controls;
using Avalonia.Layout;

namespace SimPe.Plugin.Anim
{
    /// <summary>
    /// Avalonia UserControl replacement for the WinForms AnimMeshBlockControl.
    /// </summary>
    public class AnimMeshBlockControl : Avalonia.Controls.UserControl
    {
        // ── Internal controls accessed by fAnimResourceConst ─────────────────────
        internal Avalonia.Controls.ComboBox cbJoint;
        internal Avalonia.Controls.ComboBox cbSubMesh;
        internal Avalonia.Controls.CheckBox cbCorrect;

        // ── Private controls ─────────────────────────────────────────────────────
        private Avalonia.Controls.Button llExport;
        private Avalonia.Controls.Button llImport;
        private Avalonia.Controls.Button miAdd;
        private Avalonia.Controls.Button miRem;
        private AnimFrameBlockControl pnFrames;

        // ── Events ───────────────────────────────────────────────────────────────
        public event EventHandler Changed;

        public AnimMeshBlockControl()
        {
            BuildLayout();
            Clear();
            cbCorrect.IsChecked = Helper.XmlRegistry.CorrectJointDefinitionOnExport;
        }

        private void BuildLayout()
        {
            cbJoint   = new ComboBox();
            cbSubMesh = new ComboBox();
            cbCorrect = new CheckBox { Content = "Correct Joint Definition" };
            llExport  = new Button { Content = "Export", IsEnabled = false };
            llImport  = new Button { Content = "Import", IsEnabled = false };
            miAdd     = new Button { Content = "Add Joint" };
            miRem     = new Button { Content = "Remove Joint" };
            pnFrames  = new AnimFrameBlockControl();

            cbSubMesh.SelectionChanged += cbSubMesh_SelectionChanged;
            cbJoint.SelectionChanged   += cbJoint_SelectionChanged;
            llExport.Click             += llExport_Click;
            llImport.Click             += llImport_Click;
            miAdd.Click                += miAdd_Click;
            miRem.Click                += miRem_Click;
            cbCorrect.IsCheckedChanged += cbCorrect_CheckedChanged;
            pnFrames.Changed           += pnFrames_Changed;

            var topRow = new StackPanel { Orientation = Orientation.Horizontal, Spacing = 8 };
            topRow.Children.Add(new TextBlock { Text = "SubMesh:" });
            topRow.Children.Add(cbSubMesh);
            topRow.Children.Add(new TextBlock { Text = "Joint:" });
            topRow.Children.Add(cbJoint);
            topRow.Children.Add(cbCorrect);
            topRow.Children.Add(llExport);
            topRow.Children.Add(llImport);
            topRow.Children.Add(miAdd);
            topRow.Children.Add(miRem);

            var root = new StackPanel { Spacing = 4 };
            root.Children.Add(topRow);
            root.Children.Add(pnFrames);
            Content = root;
        }

        // ── Public Properties ────────────────────────────────────────────────────

        AnimationMeshBlock[] ambs;
        public AnimationMeshBlock[] MeshBlocks
        {
            get { return ambs; }
            set
            {
                ambs = value;
                RefreshData();
            }
        }

        AnimationMeshBlock amb;
        public AnimationMeshBlock MeshBlock
        {
            get { return amb; }
            set
            {
                amb = value;
                RefreshData(amb);
            }
        }

        // ── Methods ──────────────────────────────────────────────────────────────

        internal void ClearJoint()
        {
            cbJoint.Items.Clear();
            pnFrames.Clear();
        }

        internal void Clear()
        {
            ClearJoint();
            cbSubMesh.Items.Clear();
            llImport.IsEnabled = false;
            llExport.IsEnabled = false;
        }

        public void RefreshData()
        {
            Clear();
            if (ambs != null)
            {
                foreach (AnimationMeshBlock a in ambs)
                    cbSubMesh.Items.Add(a);
                if (cbSubMesh.Items.Count > 0) cbSubMesh.SelectedIndex = 0;
            }
        }

        protected void RefreshData(AnimationMeshBlock a)
        {
            ClearJoint();
            if (a != null)
            {
                foreach (AnimationFrameBlock afb in a.Part2)
                    cbJoint.Items.Add(afb);
                if (cbJoint.Items.Count > 0) cbJoint.SelectedIndex = 0;
            }
        }

        // ── Event handlers ───────────────────────────────────────────────────────

        private void cbSubMesh_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            RefreshData((AnimationMeshBlock)cbSubMesh.SelectedItem);
            llImport.IsEnabled = cbSubMesh.SelectedItem != null;
            llExport.IsEnabled = llImport.IsEnabled;
            miAdd.IsEnabled = cbJoint.Items.Count > 0;
            miRem.IsEnabled = cbJoint.Items.Count > 0;
        }

        private void cbJoint_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            pnFrames.FrameBlock = cbJoint.SelectedItem as AnimationFrameBlock;
        }

        private void pnFrames_Changed(object sender, EventArgs e)
        {
            Changed?.Invoke(this, e);
        }

        private void llExport_Click(object sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            AnimationMeshBlock ab1 = (AnimationMeshBlock)cbSubMesh.SelectedItem;
            if (ab1 == null) return;
            GenericRcol gmdc = ab1.FindUsedGMDC(ab1.FindDefiningCRES());
            if (gmdc != null)
            {
                GeometryDataContainer gdc = (GeometryDataContainer)gmdc.Blocks[0];
                gdc.LinkedAnimation = ab1;
                fGeometryDataContainer.StartExport(gdc, ".txt", gdc.Groups,
                    (SimPe.Plugin.Gmdc.ElementSorting)fGeometryDataContainer.DefaultSelectedAxisIndex,
                    cbCorrect.IsChecked ?? false);
            }
            else
            {
                Helper.ExceptionMessage(new Warning(
                    "Unable to Find Model File for \"" + ab1.Name + "\".",
                    "SimPe was not able to Find the Model File that defines the specified Hirarchy. The Animation will not get exported!"));
            }
        }

        private void llImport_Click(object sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            AnimationMeshBlock ab1 = (AnimationMeshBlock)cbSubMesh.SelectedItem;
            if (ab1 == null) return;
            GenericRcol gmdc = ab1.FindUsedGMDC(ab1.FindDefiningCRES());
            if (gmdc != null)
            {
                GeometryDataContainer gdc = (GeometryDataContainer)gmdc.Blocks[0];
                gdc.LinkedAnimation = ab1;
                fGeometryDataContainer.StartImport(gdc, ".txt",
                    (SimPe.Plugin.Gmdc.ElementSorting)fGeometryDataContainer.DefaultSelectedAxisIndex, true);
                ab1.Parent.Changed = true;
                this.RefreshData();
            }
            else
            {
                Helper.ExceptionMessage(new Warning(
                    "Unable to Find Model File for \"" + ab1.Name + "\".",
                    "SimPe was not able to Find the Model File that defines the specified Hirarchy. The Animation will not get imported!"));
            }
        }

        private void miAdd_Click(object sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            AnimationMeshBlock ab1 = (AnimationMeshBlock)cbSubMesh.SelectedItem;
            if (ab1 != null)
            {
                AnimationFrameBlock afb = new AnimationFrameBlock(ab1);
                afb.Name = "SimPE Dummy";
                afb.TransformationType = FrameType.Rotation;
                afb.CreateBaseAxisSet();
                ab1.Part2 = (AnimationFrameBlock[])Helper.Add(ab1.Part2, afb);
                cbJoint.SelectedIndex = -1;
                cbJoint.Items.Add(afb);
                cbJoint.SelectedIndex = cbJoint.Items.Count - 1;
            }
        }

        private void miRem_Click(object sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            AnimationMeshBlock ab1 = (AnimationMeshBlock)cbSubMesh.SelectedItem;
            AnimationFrameBlock afb = (AnimationFrameBlock)cbJoint.SelectedItem;
            if (ab1 != null && afb != null)
            {
                ab1.Part2 = (AnimationFrameBlock[])Helper.Delete(ab1.Part2, afb);
                int sel = cbJoint.SelectedIndex + 1;
                if (sel >= cbJoint.Items.Count) sel = cbJoint.Items.Count - 1;
                cbJoint.Items.Remove(afb);
                cbJoint.SelectedIndex = sel;
            }
        }

        private void cbCorrect_CheckedChanged(object sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            Helper.XmlRegistry.CorrectJointDefinitionOnExport = cbCorrect.IsChecked ?? false;
        }
    }
}
