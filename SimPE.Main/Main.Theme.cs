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

        private void StoreLayout()
        {
            // Layout serialization disabled — Ambertation serializer saves incorrect state.
            // Panels are positioned correctly by enforcement code in ReloadLayout().
            // ToFile(SimPeLayoutW) is intentionally omitted until save logic is fixed.

            MyButtonItem.SetLayoutInformations(this);

            Helper.WindowsRegistry.Layout.PluginActionBoxExpanded = true;
            Helper.WindowsRegistry.Layout.DefaultActionBoxExpanded = true;
            Helper.WindowsRegistry.Layout.ToolActionBoxExpanded = true;

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
    // Panel positions are enforced entirely by ReloadLayout() — no file/stream needed.

    Helper.WindowsRegistry.Layout.PluginActionBoxExpanded = false;
    Helper.WindowsRegistry.Layout.DefaultActionBoxExpanded = true;
    Helper.WindowsRegistry.Layout.ToolActionBoxExpanded = false;

    Helper.WindowsRegistry.Layout.TypeColumnWidth = 204;
    Helper.WindowsRegistry.Layout.GroupColumnWidth = 100;
    Helper.WindowsRegistry.Layout.InstanceHighColumnWidth = 100;
    Helper.WindowsRegistry.Layout.InstanceColumnWidth = 100;
    Helper.WindowsRegistry.Layout.OffsetColumnWidth = 100;
    Helper.WindowsRegistry.Layout.SizeColumnWidth = 100;
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

            // Delete any stale simpe.layout — saving is disabled so any existing file is
            // from a prior session and will produce wrong panel positions.
            try { System.IO.File.Delete(Helper.DataFolder.SimPeLayout); } catch { }
            try { System.IO.File.Delete(Helper.DataFolder.SimPeLayoutW); } catch { }

            // Enforce correct dock containers for fixed panels.
            // A stale/corrupt simpe.layout can move these panels to wrong containers
            // (e.g. everything ends up in dockBottom). These assignments are authoritative.
            if (dcResource.DockContainer != dockLeft)       dcResource.DockContainer = dockLeft;
            if (dcResourceList.DockContainer != manager)    dcResourceList.DockContainer = manager;
            if (dcAction.DockContainer != dockRight)        dcAction.DockContainer = dockRight;
            if (dcFilter.DockContainer != dockRight)        dcFilter.DockContainer = dockRight;
            if (dcPlugin.DockContainer != dockBottom)       dcPlugin.DockContainer = dockBottom;

            // Remove ghost DockPanels created by the deserializer for panel names that no
            // longer exist in the current session (e.g. stale ManagedDockPanel* entries).
            // These show up as blank tabs in the dock areas.
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

            // Default Object Workshop to dockRight (user preference).
            // Move it there whether it is floating or in another container.
            var owPanel = Ambertation.Windows.Forms.ManagerSingelton.Global
                .GetPanelWithName("dc.SimPe.Plugin.Tool.Dockable.ObectWorkshopDockTool");
            if (owPanel != null && owPanel.DockContainer != dockRight)
                owPanel.DockContainer = dockRight;

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
