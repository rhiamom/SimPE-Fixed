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
using System.Collections.Generic;

namespace SimPe.Plugin.Gmdc
{
    /// <summary>
    /// Headless replacement for the WinForms GenericImportForm.
    /// The dialog is never shown; Execute() populates lists with default values and applies them.
    /// </summary>
    class GenericImportForm : IDisposable
    {
        private List<MeshListViewItemExt> meshItems = new List<MeshListViewItemExt>();
        private List<BoneListViewItemExt> boneItems = new List<BoneListViewItemExt>();
        private Avalonia.Controls.CheckBox cbClear;

        GenericImportForm()
        {
            cbClear = new Avalonia.Controls.CheckBox { Content = "Clear Meshgroups before Import" };
        }

        public static void Execute(GenericMeshImport gmi)
        {
            GenericImportForm f = new GenericImportForm();
            f.gmi = gmi;
            f.Setup();
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
                meshItems.Add(new MeshListViewItemExt(m, gmi, chg));

            foreach (Ambertation.Scenes.Joint j in gmi.Scene.JointCollection)
                boneItems.Add(new BoneListViewItemExt(j, gmi, bonechg));
        }

        void ApplyDefaults()
        {
            gmi.SetMeshList(meshItems.ToArray());
            gmi.SetBoneList(boneItems.ToArray());
            gmi.ClearGroupsOnImport = cbClear.IsChecked ?? false;
        }

        bool ignore = false;
        void ActionChangedEvent(MeshListViewItem sender)
        {
            if (ignore) return;
            ignore = true;
            ignore = false;
        }

        void BoneActionChangedEvent(BoneListViewItem sender)
        {
            if (ignore) return;
            ignore = true;
            ignore = false;
        }

        public void Dispose() { }
    }
}
