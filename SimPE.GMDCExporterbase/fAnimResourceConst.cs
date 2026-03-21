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
// All business logic lives in cAnimResourceConst.cs.

using System;
using Avalonia.Controls;
using Avalonia.Layout;

namespace SimPe.Plugin.Anim
{
    /// <summary>
    /// Avalonia UserControl replacement for the WinForms fAnimResourceConst Form.
    /// Exposes only the fields accessed by cAnimResourceConst.
    /// </summary>
    public class fAnimResourceConst : Avalonia.Controls.UserControl, IDisposable
    {
        // ── TabItems (were WinForms TabPages) ───────────────────────────────────
        internal Avalonia.Controls.TabItem tMesh;
        internal Avalonia.Controls.TabItem tMisc;
        internal Avalonia.Controls.TabItem tAnimResourceConst;

        // ── Controls accessed by cAnimResourceConst.InitTabPage() ───────────────
        internal Avalonia.Controls.TreeView tv;
        internal Avalonia.Controls.TextBox  tb_arc_ver;
        internal SimPe.Plugin.Anim.AnimMeshBlockControl ambc;

        public fAnimResourceConst()
        {
            BuildControls();
            BuildLayout();
        }

        private void BuildControls()
        {
            tv         = new Avalonia.Controls.TreeView();
            tb_arc_ver = new Avalonia.Controls.TextBox { Text = "0x00000000" };
            tb_arc_ver.TextChanged += tb_arc_ver_TextChanged;

            ambc = new AnimMeshBlockControl();
            ambc.Changed += new EventHandler(ambc_Changed);

            tMesh              = new TabItem { Header = "Mesh Animations" };
            tMisc              = new TabItem { Header = "Misc." };
            tAnimResourceConst = new TabItem { Header = "Raw View" };
        }

        private void BuildLayout()
        {
            // ── tMesh ────────────────────────────────────────────────────────────
            tMesh.Content = ambc;

            // ── tMisc ────────────────────────────────────────────────────────────
            var miscPanel = new StackPanel { Spacing = 4 };
            miscPanel.Children.Add(new TextBlock { Text = "Version:" });
            miscPanel.Children.Add(tb_arc_ver);
            tMisc.Content = miscPanel;

            // ── tAnimResourceConst (raw view) ────────────────────────────────────
            tAnimResourceConst.Content = tv;

            // ── Outer TabControl ─────────────────────────────────────────────────
            var tc = new Avalonia.Controls.TabControl();
            tc.Items.Add(tMesh);
            tc.Items.Add(tMisc);
            tc.Items.Add(tAnimResourceConst);
            Content = tc;
        }

        // ── Event handlers ───────────────────────────────────────────────────────

        private void tb_arc_ver_TextChanged(object sender, Avalonia.Controls.TextChangedEventArgs e)
        {
            if (tb_arc_ver.Tag == null) return;
            try
            {
                // tAnimResourceConst.Tag holds the AnimResourceConst block
                var arb = tAnimResourceConst.Tag as AbstractRcolBlock;
                if (arb == null) return;
                arb.Version = Convert.ToUInt32(tb_arc_ver.Text, 16);
                arb.Changed = true;
            }
            catch { }
        }

        private void ambc_Changed(object sender, EventArgs e) { }

        // ── IDisposable ──────────────────────────────────────────────────────────
        public void Dispose() { }
    }
}
