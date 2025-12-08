/***************************************************************************
 *   Copyright (C) 2005 by Ambertation                                     *
 *   quaxi@ambertation.de                                                  *
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
            Ambertation.Windows.Forms.Serializer.Global.ToFile(Helper.DataFolder.SimPeLayoutW);
            
            MyButtonItem.SetLayoutInformations(this);

            Helper.WindowsRegistry.Layout.PluginActionBoxExpanded = this.tbPlugAction.IsExpanded;
            Helper.WindowsRegistry.Layout.DefaultActionBoxExpanded = this.tbDefaultAction.IsExpanded;
            Helper.WindowsRegistry.Layout.ToolActionBoxExpanded = this.tbExtAction.IsExpanded;

            resourceViewManager1.StoreLayout();
        }


        void ChangedTheme(GuiTheme gt)
        {
            ThemeManager.Global.CurrentTheme = gt;
        }
        
        System.IO.Stream defaultlayout;
        /// <summary>
        /// Wrapper needed to call the Layout Change through an Event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void ResetLayout(object sender, EventArgs e)
{
    // First try to load the shipped default layout from the app's Data folder
    try
    {
        string installedLayout = System.IO.Path.Combine(
            System.Windows.Forms.Application.StartupPath,
            "Data",
            "simpe.layout");

        if (System.IO.File.Exists(installedLayout))
        {
            Ambertation.Windows.Forms.Serializer.Global.FromFile(installedLayout);
            // Save it as the user layout too, so ReloadLayout has something to work with
            Ambertation.Windows.Forms.Serializer.Global.ToFile(Helper.DataFolder.SimPeLayout);
        }
        else if (defaultlayout != null)
        {
            // Fallback to any in-memory default layout, if someone initialized it
            Ambertation.Windows.Forms.Serializer.Global.FromStream(defaultlayout);
            Ambertation.Windows.Forms.Serializer.Global.ToFile(Helper.DataFolder.SimPeLayout);
        }
    }
    catch (Exception ex)
    {
        Helper.ExceptionMessage(ex);
    }

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
            //store defaults            
            if (defaultlayout == null) 
                defaultlayout = Ambertation.Windows.Forms.Serializer.Global.ToStream();

            try
            {
                Ambertation.Windows.Forms.Serializer.Global.FromFile(Helper.DataFolder.SimPeLayout);                                
            }
            catch (Exception ex)
            {
                Helper.ExceptionMessage(ex);
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
