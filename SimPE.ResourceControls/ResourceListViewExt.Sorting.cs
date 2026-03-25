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
        ResourceViewManager.SortColumn sc;
        bool asc;
        int sortticket;
        ResoureNameSorter sortingthread;

        public ResourceViewManager.SortColumn SortedColumn
        {
            get { return sc; }
            set
            {
                sc = value;
                lock (names)
                {
                    SortResources();
                }
            }
        }

        void SortResources()
        {
            sortticket++;
            CancelThreads();

            if (sc == ResourceViewManager.SortColumn.Name)
            {
                if (Helper.XmlRegistry.AsynchronSort)
                    SimPe.Wait.SubStart(names.Count);
                SimPe.Wait.Message = SimPe.Localization.GetString("Loading embedded resource names...");
                sortingthread = new ResoureNameSorter(this, names, sortticket);
            }
            else
            {
                FinishSort();
            }
        }

        public void CancelThreads()
        {
            if (sortingthread != null)
            {
                sortingthread.Cancel();
                sortingthread = null;
            }
        }

        // Called by ResoureNameSorter when it finishes (passes the IntPtr handle it stored from parent.Handle)
        internal void SignalFinishedSort(IntPtr handle, int ticket)
        {
            SignalFinishedSort(ticket);
        }

        internal void SignalFinishedSort(int ticket)
        {
            Avalonia.Threading.Dispatcher.UIThread.Post(() =>
            {
                if (ticket == sortticket)
                {
                    sortingthread = null;
                    FinishSort();
                    Refresh();

                    if (Helper.XmlRegistry.AsynchronSort)
                        Wait.SubStop();
                }
            });
        }

        int noselectevent;
        SelectResourceEventArgs selresea;
        EventArgs resselchgea;

        void FinishSort()
        {
            BeginUpdate();
            ResourceViewManager.ResourceNameList oldsel = this.SelectedItems;
            names.SortByColumn(sc, asc);
            RebuildRows();
            object reselect = null;
            foreach (NamedPackedFileDescriptor pfd in oldsel)
                foreach (var row in rows)
                    if (row.Descriptor == pfd) { reselect = row; break; }
            if (reselect != null) { lv.SelectedItem = reselect; lv.ScrollIntoView(reselect, null); }
            EndUpdate(false);
        }

        private void lv_ColumnHeaderClick(Avalonia.Controls.DataGridColumn col)
        {
            ResourceViewManager.SortColumn tmp = ResourceViewManager.SortColumn.Offset;
            if      (col == clTNameCol)  tmp = ResourceViewManager.SortColumn.Name;
            else if (col == clTypeCol)   tmp = ResourceViewManager.SortColumn.Extension;
            else if (col == clGroupCol)  tmp = ResourceViewManager.SortColumn.Group;
            else if (col == clInstHiCol) tmp = ResourceViewManager.SortColumn.InstanceHi;
            else if (col == clInstCol)   tmp = ResourceViewManager.SortColumn.InstanceLo;
            else if (col == clSizeCol)   tmp = ResourceViewManager.SortColumn.Size;
            else if (col == clOffsetCol) tmp = ResourceViewManager.SortColumn.Offset;

            if (tmp == SortedColumn) asc = !asc;
            SortedColumn = tmp;
        }
    }
}
