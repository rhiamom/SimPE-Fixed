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
// The dialog UI is not shown; Execute() performs all pre-dialog setup (business logic)
// and returns ImportOptions with DialogResult.OK and default (false) checkbox values.
// A future pass will implement a real Avalonia dialog.

using System;
using System.Collections;
using System.Collections.Generic;

namespace SimPe.Plugin.Gmdc
{
    /// <summary>
    /// Stub for the import dialog that lets the user choose how GMDC groups/bones are imported.
    /// In the Avalonia port the dialog is not shown; defaults are used.
    /// </summary>
    internal class ImportGmdcGroupsForm : IDisposable
    {
        // ── Internal state used by Execute() ────────────────────────────────────
        SimPe.Plugin.GeometryDataContainer gmdc;
        bool ok = false;
        bool cbcleangrp_value = false;
        bool cbcleanbn_value  = false;
        bool cbupdatecres_value = false;
        bool cbBMesh_enabled = true;

        // ── Dummy collections so the build-time item-construction code compiles ──
        private List<string> cbnames_items = new List<string>();
        private List<string> cbbones_items = new List<string>();

        // ── Public API ───────────────────────────────────────────────────────────

        /// <summary>
        /// Sets up default import actions for all groups and bones, and returns ImportOptions.
        /// The dialog is not shown in the Avalonia port.
        /// </summary>
        /// <param name="gmdc">The target GMDC</param>
        /// <param name="actions">Imported group descriptors</param>
        /// <param name="joints">Imported bone descriptors</param>
        /// <returns>ImportOptions with DialogResult.OK and default settings</returns>
        public static ImportOptions Execute(SimPe.Plugin.GeometryDataContainer gmdc, ImportedGroups actions, ImportedBones joints)
        {
            ImportGmdcGroupsForm f = new ImportGmdcGroupsForm();
            f.gmdc = gmdc;

            // ── Pre-dialog setup (preserved business logic) ──────────────────────
            foreach (GmdcGroup g in gmdc.Groups) f.cbnames_items.Add(g.Name);
            for (int i = 0; i < gmdc.Joints.Length; i++) f.cbbones_items.Add(f.BuildBoneName(i));

            f.cbBMesh_enabled = (joints.Count == 0);

            foreach (ImportedGroup a in actions)
            {
                if (a.Group.Name.ToLower().Trim().IndexOf("shadow") > -1)
                    a.Group.Opacity = (uint)MeshOpacity.Shadow;
                if (a.Group.Opacity > 0x10 && f.cbBMesh_enabled) a.UseInBoundingMesh = true;

                if (a.Target.Name == "")
                {
                    a.Target.Name  = a.Group.Name;
                    a.Target.Index = gmdc.FindGroupByName(a.Target.Name);
                }
            }

            int ct = 0;
            foreach (ImportedBone a in joints)
            {
                a.Action = GmdcImporterAction.Update;
                a.FindBestFitJoint(gmdc);
                if (ct < gmdc.Joints.Length && a.TargetIndex == -1)
                    a.TargetIndex = ct;
                ct++;
            }

            // ── Return with OK and default checkbox values (no dialog shown) ──────
            f.ok = true;
            return new ImportOptions(SimPe.DialogResult.OK, f.cbcleangrp_value, f.cbcleanbn_value, f.cbupdatecres_value);
        }

        // ── Helper methods ───────────────────────────────────────────────────────

        string BuildBoneName(int index)
        {
            if (index < 0 || index >= gmdc.Joints.Length) return "---";
            return gmdc.Joints[index].Name;
        }

        string BuildBoneName(ImportedBone a)
        {
            if (a.TargetIndex < 0) return "---";
            return BuildBoneName(a.TargetIndex);
        }

        public void Dispose() { }
    }
}
