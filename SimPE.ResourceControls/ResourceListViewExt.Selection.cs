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

namespace SimPe.Windows.Forms
{
    partial class ResourceListViewExt
    {
        const int WAIT_SELECT = 400;
        System.Threading.Timer seltimer;

        private void lv_SelectionChanged(object sender, Avalonia.Controls.SelectionChangedEventArgs e)
        {
            SignalSelectionChanged();
        }

        protected void SignalSelectionChanged()
        {
            if (noselectevent > 0) FireSelectionChangedOnUIThread();
            else seltimer.Change(WAIT_SELECT, System.Threading.Timeout.Infinite);
        }

        void SelectionTimerCallback(object state)
        {
            Avalonia.Threading.Dispatcher.UIThread.Post(OnResourceSelectionChanged);
        }

        public ResourceViewManager.ResourceNameList SelectedItems
        {
            get
            {
                lock (names)
                {
                    ResourceViewManager.ResourceNameList ret = new ResourceViewManager.ResourceNameList();
                    foreach (var item in lv.SelectedItems)
                        if (item is ResourceListItemExt ri) ret.Add(ri.Descriptor);
                    return ret;
                }
            }
        }

        protected virtual void OnResourceSelectionChanged()
        {
            resselchgea = new EventArgs();
            if (SelectionChanged != null && noselectevent == 0) SelectionChanged(this, resselchgea);
            if (noselectevent == 0) resselchgea = null;
        }

        private void lv_DoubleTapped(object sender, Avalonia.Input.TappedEventArgs e)
        {
            if (!Helper.XmlRegistry.SimpleResourceSelect) OnSelectResource();
        }

        private void lv_PointerReleased(object sender, Avalonia.Input.PointerReleasedEventArgs e)
        {
            if (e.InitialPressMouseButton == Avalonia.Input.MouseButton.Middle)
            {
                bool old = ctrldown;
                ctrldown = true;
                OnSelectResource();
                ctrldown = old;
            }
            else if (Helper.XmlRegistry.SimpleResourceSelect)
            {
                OnSelectResource();
            }
        }

        bool ctrldown = false;
        private void lv_KeyUp_Handler(object sender, Avalonia.Input.KeyEventArgs e)
        {
            ctrldown = e.KeyModifiers.HasFlag(Avalonia.Input.KeyModifiers.Alt);

            if (!ctrldown && (e.Key == Avalonia.Input.Key.Up
                || e.Key == Avalonia.Input.Key.Down
                || e.Key == Avalonia.Input.Key.PageDown
                || e.Key == Avalonia.Input.Key.PageUp
                || e.Key == Avalonia.Input.Key.Home
                || e.Key == Avalonia.Input.Key.End)) OnSelectResource();

            if (e.Key == Avalonia.Input.Key.Enter) OnSelectResource();
            if (e.Key == Avalonia.Input.Key.A && e.KeyModifiers.HasFlag(Avalonia.Input.KeyModifiers.Control)) SelectAll();

            if (ListViewKeyUp != null) ListViewKeyUp(this, e);
        }

        public void SelectAll()
        {
            lock (names)
            {
                BeginUpdate();
                lv.SelectAll();
                EndUpdate();
            }
        }

        protected void OnSelectResource()
        {
            bool rctrl = ctrldown;
            if (!Helper.XmlRegistry.FirefoxTabbing) rctrl = false;

            selresea = new SelectResourceEventArgs(rctrl);
            if (SelectedResource != null && noselectevent == 0)
                SelectedResource(this, selresea);
            if (noselectevent == 0) selresea = null;
        }

        public class SelectResourceEventArgs : EventArgs
        {
            bool ctrldn;
            public bool CtrlDown => ctrldn;
            internal SelectResourceEventArgs(bool ctrldn) : base() { this.ctrldn = ctrldn; }
        }
        public delegate void SelectResourceHandler(ResourceListViewExt sender, SelectResourceEventArgs e);
        public event SelectResourceHandler SelectedResource;
        public event EventHandler<Avalonia.Input.KeyEventArgs> ListViewKeyUp;

        public SimPe.Plugin.FileIndexItem SelectedItem
        {
            get
            {
                lock (names)
                {
                    if (lv.SelectedItem is ResourceListItemExt ri) return ri.Descriptor.Resource;
                    return null;
                }
            }
        }

        public bool SelectResource(SimPe.Interfaces.Scenegraph.IScenegraphFileIndexItem resource)
        {
            lock (names)
            {
                foreach (var row in rows)
                {
                    if (row.Descriptor.Resource.FileDescriptor.Equals(resource.FileDescriptor))
                    {
                        BeginUpdate();
                        lv.SelectedItem = row;
                        lv.ScrollIntoView(row, null);
                        EndUpdate();
                        return true;
                    }
                }
            }
            return false;
        }
    }
}
