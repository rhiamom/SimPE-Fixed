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
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using SimPe.Events;

namespace SimPe
{
    partial class MainForm
    {
        void InitTheme()
        {
            this.dcResourceList.Visible = true;
            this.dcResource.Visible = true;

            // WeifenLuo DockPanelSuite requires a theme before content can be shown
            this.dc.Theme = new WeifenLuo.WinFormsUI.Docking.VS2015LightTheme();

            //setup the Theme Manager

            ThemeManager.Global.AddControl(this.manager);
            ThemeManager.Global.AddControl(this.xpGradientPanel1);
            ThemeManager.Global.AddControl(this.xpGradientPanel2);
            ThemeManager.Global.AddControl(this.xpGradientPanel3);
            ThemeManager.Global.AddControl(this.menuBar1);
            ThemeManager.Global.AddControl(this.miAction);

            ThemeManager.Global.AddControl(tbAction);
            ThemeManager.Global.AddControl(tbTools);
            ThemeManager.Global.AddControl(tbWindow);
            ThemeManager.Global.AddControl(toolBar1);
            ThemeManager.Global.AddControl(tbContainer);
        }

        // Map a DockContainer reference to its persistence name and back.
        // "Manager" is the central document area, "Floating" means the panel
        // is detached. Any unrecognized container falls through to null on
        // restore (we leave it where it is rather than guess wrong).
        string NameForContainer(Ambertation.Windows.Forms.DockContainer dc)
        {
            if (dc == null)        return "Floating";
            if (dc == dockLeft)    return "Left";
            if (dc == dockRight)   return "Right";
            if (dc == dockBottom)  return "Bottom";
            if (dc == manager)     return "Manager";
            return null;
        }

        Ambertation.Windows.Forms.DockContainer ContainerForName(string name)
        {
            switch (name)
            {
                case "Left":    return dockLeft;
                case "Right":   return dockRight;
                case "Bottom":  return dockBottom;
                case "Manager": return manager;
                default:        return null;   // Floating or unknown
            }
        }

        private void StoreLayout()
        {
            // Save everything we'll need on next launch into Layout2XREG via
            // LayoutRegistry. Replaces the old Ambertation binary serializer,
            // which left panels hidden on restore after the .NET 8 port.

            MyButtonItem.SetLayoutInformations(this);

            // Window bounds — RestoreBounds preserves the un-maximized rect
            // when maximized so unmaximizing next session goes back correctly.
            bool maximized = this.WindowState == FormWindowState.Maximized;
            System.Drawing.Rectangle r = maximized ? this.RestoreBounds : this.Bounds;
            if (r.Width > 0 && r.Height > 0)
            {
                Helper.WindowsRegistry.Layout.WindowX = r.X;
                Helper.WindowsRegistry.Layout.WindowY = r.Y;
                Helper.WindowsRegistry.Layout.WindowWidth = r.Width;
                Helper.WindowsRegistry.Layout.WindowHeight = r.Height;
            }
            Helper.WindowsRegistry.Layout.WindowMaximized = maximized;

            // Dock-area split widths (where the user dragged the splitters
            // between the main work area and the side panel strips).
            if (dockLeft   != null && dockLeft.Width    > 0) Helper.WindowsRegistry.Layout.DockLeftWidth    = dockLeft.Width;
            if (dockRight  != null && dockRight.Width   > 0) Helper.WindowsRegistry.Layout.DockRightWidth   = dockRight.Width;
            if (dockBottom != null && dockBottom.Height > 0) Helper.WindowsRegistry.Layout.DockBottomHeight = dockBottom.Height;

            // Every registered DockPanel: its container, open/closed state,
            // size, and (if floating) its on-screen position. Skip auto-named
            // ghost panels — they have no stable identity across sessions.
            foreach (Ambertation.Windows.Forms.DockPanel dp in
                     Ambertation.Windows.Forms.ManagerSingelton.Global.KnownPanels)
            {
                if (dp == null) continue;
                if (string.IsNullOrEmpty(dp.Name)) continue;
                if (dp.Name.StartsWith("ManagedDockPanel")) continue;

                var st = new LayoutRegistry.PanelState
                {
                    Name      = dp.Name,
                    Container = dp.IsFloating ? "Floating" : NameForContainer(dp.DockContainer),
                    IsOpen    = dp.IsOpen,
                    Width     = dp.Width,
                    Height    = dp.Height,
                };
                if (dp.IsFloating && dp.ParentForm != null)
                {
                    st.FloatingX = dp.ParentForm.Location.X;
                    st.FloatingY = dp.ParentForm.Location.Y;
                }
                if (st.Container != null) Helper.WindowsRegistry.Layout.SetPanelState(st);
            }

            // Keep the OW special case in sync — older code paths still read it.
            var owPanel = Ambertation.Windows.Forms.ManagerSingelton.Global
                .GetPanelWithName("dc.SimPe.Plugin.Tool.Dockable.ObectWorkshopDockTool");
            if (owPanel != null)
            {
                string container = owPanel.IsFloating ? "Floating" : NameForContainer(owPanel.DockContainer);
                if (container != null) Helper.WindowsRegistry.Layout.OWDockContainer = container;
            }

            resourceViewManager1.StoreLayout();
        }


        void ChangedTheme(GuiTheme gt)
        {
            ThemeManager.Global.CurrentTheme = gt;
        }
        
        /// <summary>
        /// Wrapper needed to call the Layout Change through an Event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void ResetLayout(object sender, EventArgs e)
        {
            // 1. Wipe all stored layout state so subsequent ReloadLayout doesn't
            //    re-apply the user's prior arrangement on top of our reset.
            Helper.WindowsRegistry.Layout.ClearLayoutState();
            Helper.WindowsRegistry.Layout.OWDockContainer = "Bottom";

            // 2. Reset the column widths and action-box defaults.
            Helper.WindowsRegistry.Layout.PluginActionBoxExpanded = false;
            Helper.WindowsRegistry.Layout.DefaultActionBoxExpanded = true;
            Helper.WindowsRegistry.Layout.ToolActionBoxExpanded = false;

            Helper.WindowsRegistry.Layout.TypeColumnWidth = 204;
            Helper.WindowsRegistry.Layout.GroupColumnWidth = 100;
            Helper.WindowsRegistry.Layout.InstanceHighColumnWidth = 100;
            Helper.WindowsRegistry.Layout.InstanceColumnWidth = 100;
            Helper.WindowsRegistry.Layout.OffsetColumnWidth = 100;
            Helper.WindowsRegistry.Layout.SizeColumnWidth = 100;

            // 3. Explicitly reassert the designer-intended dock containers for
            //    the fixed side panels. ReloadLayout no longer does this — it
            //    just consumes stored state — so Reset has to do it here.
            //    (Mirrors the old enforcement block from pre-persistence days.)
            try
            {
                if (dcResource     != null && dcResource.DockContainer     != dockLeft)   dcResource.DockContainer     = dockLeft;
                if (dcResourceList != null && dcResourceList.DockContainer != manager)    dcResourceList.DockContainer = manager;
                if (dcAction       != null && dcAction.DockContainer       != dockRight)  dcAction.DockContainer       = dockRight;
                if (dcFilter       != null && dcFilter.DockContainer       != dockRight)  dcFilter.DockContainer       = dockRight;
                if (dcPlugin       != null && dcPlugin.DockContainer       != dockBottom) dcPlugin.DockContainer       = dockBottom;

                // OW back to its default sibling tab in the bottom container.
                var owPanel = Ambertation.Windows.Forms.ManagerSingelton.Global
                    .GetPanelWithName("dc.SimPe.Plugin.Tool.Dockable.ObectWorkshopDockTool");
                if (owPanel != null && owPanel.DockContainer != dockBottom)
                {
                    try { owPanel.DockContainer = dockBottom; } catch { }
                }
            }
            catch { /* dock library can throw on transient state; ignore */ }

            FixVisibleState(tbTools);
            FixVisibleState(tbAction);
            FixVisibleState(toolBar1);

            ReloadLayout();

            tbTools.Visible = true;
            tbAction.Visible = true;
            toolBar1.Visible = true;

            tbWindow.Visible = false;
            this.dcResourceList.Visible = true;
        }



        /// <summary>
        /// Reload the Layout from the Registry
        /// </summary>
        void ReloadLayout()
        {
            this.SuspendLayout();

            // Restore main window bounds and maximized state — only if we have a
            // saved size, otherwise leave the designer / start-position default.
            if (Helper.WindowsRegistry.Layout.HasStoredWindowBounds)
            {
                int x = Helper.WindowsRegistry.Layout.WindowX;
                int y = Helper.WindowsRegistry.Layout.WindowY;
                int w = Helper.WindowsRegistry.Layout.WindowWidth;
                int h = Helper.WindowsRegistry.Layout.WindowHeight;
                this.StartPosition = FormStartPosition.Manual;
                this.Bounds = new System.Drawing.Rectangle(x, y, w, h);
                if (Helper.WindowsRegistry.Layout.WindowMaximized)
                    this.WindowState = FormWindowState.Maximized;
            }

            // Remove ghost DockPanels created by historical sessions for names
            // that no longer exist (e.g. uninstalled plugin's ManagedDockPanel*
            // entries). These show up as blank tabs in the dock areas.
            var dockContainers = new System.Windows.Forms.Control[] { dockBottom, dockLeft, dockRight };
            foreach (var container in dockContainers)
            {
                var toRemove = new System.Collections.Generic.List<Ambertation.Windows.Forms.DockPanel>();
                foreach (System.Windows.Forms.Control c in container.Controls)
                {
                    var dp = c as Ambertation.Windows.Forms.DockPanel;
                    if (dp != null && dp.Name.StartsWith("ManagedDockPanel"))
                        toRemove.Add(dp);
                }
                foreach (var dp in toRemove)
                    dp.Close();
            }

            // Apply each saved panel state. Panels with no stored state stay
            // wherever the designer/plugin put them.
            foreach (Ambertation.Windows.Forms.DockPanel dp in
                     Ambertation.Windows.Forms.ManagerSingelton.Global.KnownPanels)
            {
                if (dp == null || string.IsNullOrEmpty(dp.Name)) continue;
                if (dp.Name.StartsWith("ManagedDockPanel")) continue;

                var st = Helper.WindowsRegistry.Layout.GetPanelState(dp.Name);
                if (st == null) continue;

                try
                {
                    if (st.Container == "Floating")
                    {
                        // Best-effort: leave docked panels alone (the dock library
                        // throws if we try to detach in some transitional states).
                        // If already floating, restore the floating window position.
                        if (dp.IsFloating && dp.ParentForm != null)
                            dp.ParentForm.Location = new System.Drawing.Point(st.FloatingX, st.FloatingY);
                    }
                    else
                    {
                        var target = ContainerForName(st.Container);
                        if (target != null && dp.DockContainer != target)
                            dp.DockContainer = target;
                    }

                    // Apply open/closed last so it doesn't fight the container change.
                    if (st.IsOpen && !dp.IsOpen)       dp.Open();
                    else if (!st.IsOpen && dp.IsOpen)  dp.Close();
                }
                catch (System.Exception)
                {
                    // The dock library can throw if a panel is mid-transition.
                    // Leave that panel as-is rather than killing the whole restore.
                }
            }

            // Restore dock-area splitter sizes AFTER panel placement so the
            // containers have something in them when we resize.
            try
            {
                int lw = Helper.WindowsRegistry.Layout.DockLeftWidth;
                int rw = Helper.WindowsRegistry.Layout.DockRightWidth;
                int bh = Helper.WindowsRegistry.Layout.DockBottomHeight;
                if (lw > 0 && dockLeft   != null) dockLeft.Width    = lw;
                if (rw > 0 && dockRight  != null) dockRight.Width   = rw;
                if (bh > 0 && dockBottom != null) dockBottom.Height = bh;
            }
            catch { }

            // OW fallback for installs that have OWDockContainer set but no
            // per-panel state for it yet (upgrade path from the old code that
            // ONLY persisted the OW container).
            var owPanel = Ambertation.Windows.Forms.ManagerSingelton.Global
                .GetPanelWithName("dc.SimPe.Plugin.Tool.Dockable.ObectWorkshopDockTool");
            if (owPanel != null
                && Helper.WindowsRegistry.Layout.GetPanelState(owPanel.Name) == null)
            {
                Ambertation.Windows.Forms.DockContainer target =
                    ContainerForName(Helper.WindowsRegistry.Layout.OWDockContainer)
                    ?? dockBottom;
                if (owPanel.DockContainer != target)
                {
                    try { owPanel.DockContainer = target; }
                    catch { try { owPanel.DockContainer = dockBottom; } catch { } }
                }
            }

            resourceViewManager1.RestoreLayout();

            UpdateDockMenus();
            MyButtonItem.GetLayoutInformations(this);

            FixCheckedState(tbTools);
            FixCheckedState(toolBar1);

            foreach (ToolStripItem tsi in miWindow.DropDownItems)
            {
                ToolStripMenuItem tsmi = tsi as ToolStripMenuItem;
                if (tsmi == null) continue;
                if (tsmi.Tag == null) continue;

                Ambertation.Windows.Forms.DockPanel dp = tsmi.Tag as Ambertation.Windows.Forms.DockPanel;
                if (dp != null)
                    tsmi.Checked = dp.IsOpen;
            }
            this.ResumeLayout();

            // For layout resets triggered after the form is already shown (e.g. Reset Layout
            // menu action), BeginInvoke is still needed to let dock events settle first.
            this.BeginInvoke((Action)EnsureKeyPanelsVisible);
        }

        /// <summary>
        /// Forces the three primary panels to be the active (visible) panel in their
        /// respective dock containers. Called both from Shown (on first load) and via
        /// BeginInvoke from ReloadLayout (on subsequent layout resets).
        /// </summary>
        void EnsureKeyPanelsVisible()
        {
            dcResource.EnsureVisible();
            dcPlugin.EnsureVisible();
            var ow = Ambertation.Windows.Forms.ManagerSingelton.Global
                .GetPanelWithName("dc.SimPe.Plugin.Tool.Dockable.ObectWorkshopDockTool");
            if (ow != null) ow.EnsureVisible();
        }

        void MainForm_FirstShown(object sender, EventArgs e)
        {
            this.Shown -= MainForm_FirstShown;
            EnsureKeyPanelsVisible();
        }

        private void FixCheckedState(System.Windows.Forms.ToolStrip ts)
        {
            foreach (System.Windows.Forms.ToolStripItem tsi in ts.Items)
            {
                System.Windows.Forms.ToolStripButton tsb = tsi as System.Windows.Forms.ToolStripButton;
                if (tsb == null) continue;
                if (tsb.Overflow != System.Windows.Forms.ToolStripItemOverflow.Always)
                    tsb.Checked = false;
            }
        }

        private void FixVisibleState(System.Windows.Forms.ToolStrip ts)
        {
            foreach (System.Windows.Forms.ToolStripItem tsi in ts.Items)
            {
                System.Windows.Forms.ToolStripButton tsb = tsi as System.Windows.Forms.ToolStripButton;
                if (tsb == null) continue;
                if (tsb.Image!=null && tsb!=biUpdate) tsb.Visible = true;
            }
        }
    }
}
