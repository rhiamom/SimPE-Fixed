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

// Ported from WinForms Form to Avalonia stub (Mac port).
// The dialog UI is stubbed; business logic in Execute() is preserved and
// will proceed with default values (import all meshes/bones).

using System;
using System.Collections.Generic;
using System.Windows.Forms;  // our compat stubs (ListViewEx, ListViewItem, ColumnHeader)

namespace SimPe.Plugin.Gmdc
{
    /// <summary>
    /// Avalonia-compatible stub for the WinForms GenericImportForm.
    /// The dialog is not shown in the Avalonia port; Execute() runs with default settings.
    /// </summary>
    class GenericImportForm : IDisposable
    {
        // ── Fields used by business logic ────────────────────────────────────────
        private System.Windows.Forms.ListViewEx lvmesh;
        private System.Windows.Forms.ListViewEx lvbones;
        private Avalonia.Controls.CheckBox cbClear;

        private System.Windows.Forms.ColumnHeader chMeshName;
        private System.Windows.Forms.ColumnHeader chMeshAction;
        private System.Windows.Forms.ColumnHeader chMeshTarget;
        private System.Windows.Forms.ColumnHeader chFaces;
        private System.Windows.Forms.ColumnHeader chVertices;
        private System.Windows.Forms.ColumnHeader chImportEnvelope;
        private System.Windows.Forms.ColumnHeader chJointCount;
        private System.Windows.Forms.ColumnHeader clBoneName;
        private System.Windows.Forms.ColumnHeader clBoneAction;
        private System.Windows.Forms.ColumnHeader clImportBone;
        private System.Windows.Forms.ColumnHeader clAssignedVertices;

        GenericImportForm()
        {
            lvmesh  = new System.Windows.Forms.ListViewEx();
            lvbones = new System.Windows.Forms.ListViewEx();
            cbClear = new Avalonia.Controls.CheckBox { Content = "Clear Meshgroups before Import" };

            chMeshName        = new System.Windows.Forms.ColumnHeader { Text = "Name",              Width = 106 };
            chMeshAction      = new System.Windows.Forms.ColumnHeader { Text = "",                   Width = 102 };
            chMeshTarget      = new System.Windows.Forms.ColumnHeader { Text = "Import as",          Width = 277 };
            chFaces           = new System.Windows.Forms.ColumnHeader { Text = "Faces",              Width = 67  };
            chVertices        = new System.Windows.Forms.ColumnHeader { Text = "Vertices",           Width = 67  };
            chImportEnvelope  = new System.Windows.Forms.ColumnHeader { Text = "Boneweight Import",  Width = 20  };
            chJointCount      = new System.Windows.Forms.ColumnHeader { Text = "Joint Count",        Width = 79  };
            clBoneName        = new System.Windows.Forms.ColumnHeader { Text = "Name",               Width = 106 };
            clBoneAction      = new System.Windows.Forms.ColumnHeader { Text = "",                   Width = 102 };
            clImportBone      = new System.Windows.Forms.ColumnHeader { Text = "Import as",          Width = 277 };
            clAssignedVertices= new System.Windows.Forms.ColumnHeader { Text = "Vertices",           Width = 67  };

            lvmesh.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
                chMeshName, chMeshAction, chMeshTarget, chFaces, chVertices, chImportEnvelope, chJointCount });

            lvbones.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
                clBoneName, clBoneAction, clImportBone, clAssignedVertices });
        }

        public static void Execute(GenericMeshImport gmi)
        {
            GenericImportForm f = new GenericImportForm();
            f.gmi = gmi;
            f.Setup();
            // In Avalonia port: dialog is not shown — proceed with defaults
            f.ApplyDefaults();
            f.Dispose();
        }

        GenericMeshImport gmi;
        MeshListViewItem.ActionChangedEvent chg;
        BoneListViewItem.ActionChangedEvent bonechg;

        void Setup()
        {
            cbClear.IsChecked = gmi.ClearGroupsOnImport;
            if (chg == null)
                chg = new MeshListViewItem.ActionChangedEvent(ActionChangedEvent);
            if (bonechg == null)
                bonechg = new BoneListViewItem.ActionChangedEvent(BoneActionChangedEvent);

            foreach (Ambertation.Scenes.Mesh m in gmi.Scene.MeshCollection)
                new MeshListViewItemExt(lvmesh, m, gmi, chg);

            foreach (Ambertation.Scenes.Joint j in gmi.Scene.JointCollection)
                new BoneListViewItemExt(lvbones, j, gmi, bonechg);
        }

        void ApplyDefaults()
        {
            // Apply the current state of the list views (with default values from Setup)
            var meshList = new List<MeshListViewItemExt>();
            for (int i = 0; i < lvmesh.Items.Count; i++)
                if (lvmesh.Items[i] is MeshListViewItemExt mx) meshList.Add(mx);
            gmi.SetMeshList(meshList.ToArray());

            var boneList = new List<BoneListViewItemExt>();
            for (int i = 0; i < lvbones.Items.Count; i++)
                if (lvbones.Items[i] is BoneListViewItemExt bx) boneList.Add(bx);
            gmi.SetBoneList(boneList.ToArray());

            gmi.ClearGroupsOnImport = cbClear.IsChecked ?? false;
        }

        bool ignore = false;
        void ActionChangedEvent(MeshListViewItem sender)
        {
            if (ignore) return;
            ignore = true;
            // no multi-select in stub — no-op
            ignore = false;
        }

        void BoneActionChangedEvent(BoneListViewItem sender)
        {
            if (ignore) return;
            ignore = true;
            // no multi-select in stub — no-op
            ignore = false;
        }

        public void Dispose() { }
    }
}
