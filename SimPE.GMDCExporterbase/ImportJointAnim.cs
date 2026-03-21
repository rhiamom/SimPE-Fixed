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
// The dialog UI is not shown; Execute() performs all pre-dialog setup and
// returns true (proceed with import) by default.
// A future pass will implement a real Avalonia dialog.

using System;
using System.Collections;

namespace SimPe.Plugin.Anim
{
    /// <summary>
    /// Stub for the joint animation import dialog.
    /// In the Avalonia port the dialog is not shown; defaults are used.
    /// </summary>
    public class ImportJointAnim : IDisposable
    {
        bool ok = false;
        bool cbCorrect_value = false;

        ImportJointAnim() { }

        /// <summary>
        /// Matches imported frame blocks to their best-fit targets and proceeds with default settings.
        /// The dialog is not shown in the Avalonia port.
        /// </summary>
        /// <param name="amb">Imported frame blocks</param>
        /// <param name="gmdc">Geometry data container</param>
        /// <returns>true if the import should proceed (always true in stub)</returns>
        public static bool Execute(ImportedFrameBlocks amb, GeometryDataContainer gmdc)
        {
            ImportJointAnim f = new ImportJointAnim();
            f.cbCorrect_value = amb.AuskelCorrection;

            // ── Pre-dialog setup (preserved business logic) ──────────────────────
            // Build target list from animation mesh block
            var cbnames_items = new System.Collections.Generic.List<AnimationFrameBlock>();
            if (gmdc.LinkedAnimation != null)
                foreach (AnimationFrameBlock afb in gmdc.LinkedAnimation.Part2)
                    cbnames_items.Add(afb);

            // For each imported frame, find the best matching target if one isn't set
            foreach (ImportedFrameBlock ifb in amb)
            {
                if (ifb.Target == null)
                {
                    foreach (AnimationFrameBlock afb in cbnames_items)
                    {
                        if (afb.Name == ifb.ImportedName)
                        {
                            ifb.Target = afb;
                            break;
                        }
                    }
                }
            }

            // ── No dialog — proceed with defaults ────────────────────────────────
            f.ok = true;
            amb.AuskelCorrection = f.cbCorrect_value;
            return f.ok;
        }

        public void Dispose() { }
    }
}
