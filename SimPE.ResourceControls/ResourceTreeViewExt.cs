/***************************************************************************
 *   Copyright (C) 2005 by Ambertation                                     *
 *   quaxi@ambertation.de                                                  *
 *                                                                         *
 *   Copyright (C) 2025 by GramzeSweatShop                                 *
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
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace SimPe.Windows.Forms
{
    public partial class ResourceTreeViewExt : UserControl
    {
        ResourceTreeNodesByType typebuilder;
        ResourceTreeNodesByGroup groupbuilder;
        ResourceTreeNodesByInstance instbuilder;
        ResourceViewManager manager;
        IResourceTreeNodeBuilder builder;
        public ResourceTreeViewExt()
        {
            allowselectevent = true;
            InitializeComponent();

            tv.Font = new System.Drawing.Font("Tahoma", 9.0F);

            // tv.HideSelection = false keeps the selected node visibly
            // marked even after focus moves elsewhere (e.g. clicking into
            // the Resource List) — deliberate, so the user keeps their
            // place. But Windows/Wine render that "selected, not focused"
            // state using hardcoded system colors our ThemeManager can't
            // reach, which clashes badly under the Dark theme: the node's
            // own (correctly light, for readability against a dark
            // background) text color stays as-is, but the background
            // becomes system-default light gray/white — light text on a
            // light background, unreadable. OwnerDrawText scopes our
            // control to just that text/background painting (icons,
            // expand glyphs, indentation all stay natively drawn), so we
            // only override the one state that's actually broken and let
            // Windows/Wine handle every other state exactly as before.
            tv.DrawMode = System.Windows.Forms.TreeViewDrawMode.OwnerDrawText;
            tv.DrawNode += new System.Windows.Forms.DrawTreeNodeEventHandler(tv_DrawNode);

            typebuilder = new ResourceTreeNodesByType();
            groupbuilder = new ResourceTreeNodesByGroup();
            instbuilder = new ResourceTreeNodesByInstance();

            ThemeManager.Global.AddControl(this.toolStrip1);
            builder = typebuilder;
            tbType.Checked = true;
            last = null;
        }

        ~ResourceTreeViewExt()
        {
            ThemeManager.Global.RemoveControl(this.toolStrip1);
        }

        private void tv_DrawNode(object sender, System.Windows.Forms.DrawTreeNodeEventArgs e)
        {
            bool selectedButUnfocused = e.Node == tv.SelectedNode && !tv.Focused;
            if (!selectedButUnfocused)
            {
                e.DrawDefault = true;
                return;
            }

            Color back = ThemeManager.ExtendedTheme ? ThemeManager.Global.ThemeColorLighter : System.Drawing.SystemColors.Control;
            Color fore = ThemeManager.ExtendedTheme ? ThemeManager.Global.ThemeTextColor : System.Drawing.SystemColors.ControlText;

            using (System.Drawing.SolidBrush brush = new System.Drawing.SolidBrush(back))
                e.Graphics.FillRectangle(brush, e.Bounds);

            System.Windows.Forms.TextRenderer.DrawText(
                e.Graphics, e.Node.Text, tv.Font, e.Bounds, fore, back,
                System.Windows.Forms.TextFormatFlags.VerticalCenter | System.Windows.Forms.TextFormatFlags.Left);
        }

        internal void SetManager(ResourceViewManager manager)
        {
            last = null;
            if (this.manager != manager)
            {
                this.manager = manager;
            }
        }  

        public void Clear()
        {
            tv.Nodes.Clear();
        }

        ResourceMaps last;
        void SetResourceMaps(bool nosave)
        {
            tv.Nodes.Clear();
            if (last != null) SetResourceMaps(last, true, nosave);
        }

        bool allowselectevent;
        TreeNode firstnode;
        public bool SetResourceMaps(ResourceMaps maps, bool selectevent, bool dontselect)
        {
            return SetResourceMaps(maps, selectevent, dontselect, false);
        }
        protected bool SetResourceMaps(ResourceMaps maps, bool selectevent, bool dontselect, bool nosave)
        {
            last = maps;
            if (FileTable.WrapperRegistry != null)
            {
                tv.ImageList = FileTable.WrapperRegistry.WrapperImageList;
                tv.StateImageList = tv.ImageList;
            }
            if (!nosave) SaveLastSelection();

            this.Clear();
            firstnode = builder.BuildNodes(maps);
            tv.Nodes.Add(firstnode);
            firstnode.Expand();

            allowselectevent = selectevent;
            if (!dontselect && (maps.Everything.Count <= Helper.WindowsRegistry.BigPackageResourceCount || Helper.WindowsRegistry.ResoruceTreeAllwaysAutoselect))
            {
                if (!SelectID(firstnode, builder.LastSelectedId))
                {
                    SelectAll();
                    allowselectevent = true;
                    return false;
                }
            }
            else if (dontselect)
            {
                foreach (ResourceTreeNodeExt node in firstnode.Nodes)
                {
                    if (node.ID == 0x46414D49) { tv.SelectedNode = node; break; }
                }
            }

            allowselectevent = true;
            return true;
        }

        private void SaveLastSelection()
        {
            ResourceTreeNodeExt node = tv.SelectedNode as ResourceTreeNodeExt;
            if (node != null) builder.LastSelectedId = node.ID;
            else builder.LastSelectedId = 0;
        }

        protected bool SelectID(TreeNode node, ulong id)
        {
            ResourceTreeNodeExt rn = node as ResourceTreeNodeExt;
            if (rn != null)
            {
                if (rn.ID == id)
                {
                    tv.SelectedNode = rn;
                    rn.EnsureVisible();
                    return true;
                }
            }

            foreach (TreeNode sub in node.Nodes)
                if (SelectID(sub, id)) return true;

            return false;
        }

        public void SelectAll()
        {
            if (firstnode!=null)
                tv.SelectedNode = firstnode;
        }

        private void tv_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (!allowselectevent) return;
            if (e.Node == null) return;
            ResourceTreeNodeExt node = e.Node as ResourceTreeNodeExt;
            if (node != null)
            {
                if (this.manager != null)
                {
                    if (manager.ListView != null)
                    {
                        manager.ListView.SetResources(node.Resources);
                    }
                }
            }
        }

        private void SelectTreeBuilder(object sender, EventArgs e)
        {
            tbType.Checked = sender == tbType;
            tbGroup.Checked = sender == tbGroup;
            tbInst.Checked = sender == tbInst;

            SaveLastSelection();

            IResourceTreeNodeBuilder old = builder;
            string persistName;
            if (sender == tbInst) { builder = instbuilder; persistName = "Instance"; }
            else if (sender == tbGroup) { builder = groupbuilder; persistName = "Group"; }
            else { builder = typebuilder; persistName = "Type"; }

            // Persist the choice so the next session picks it up via RestoreLayout.
            try { Helper.WindowsRegistry.Layout.ResourceTreeBuilder = persistName; }
            catch { /* registry hiccup shouldn't kill the click */ }

            if (old != builder) SetResourceMaps(true);
        }

        internal void RestoreLayout()
        {
            // Restore the TGI button the user last picked. Defaults to tbType
            // (the LayoutRegistry getter falls back to "Type" if nothing saved).
            string saved = Helper.WindowsRegistry.Layout.ResourceTreeBuilder;
            ToolStripButton btn;
            switch (saved)
            {
                case "Group":    btn = tbGroup; break;
                case "Instance": btn = tbInst;  break;
                default:         btn = tbType;  break;
            }
            SelectTreeBuilder(btn, null);
        }
    }
}
