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
                //if (sc != value)
                {
                    sc = value;
                    lock (names)
                    {
                        SortResources();
                        Refresh();
                    }
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
                SignalFinishedSort(sortticket);
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

        internal void SignalFinishedSort(int ticket)
        {
            SignalFinishedSort(Handle, ticket);
        }

        internal void SignalFinishedSort(IntPtr handle, int ticket)
        {
            SendMessage(handle, WM_USER_SORTED_RESOURCES, ticket, 0);
        }

        int noselectevent;
        SelectResourceEventArgs selresea;
        EventArgs resselchgea;
        void DoTheSorting()
        {
            BeginUpdate();

            ResourceViewManager.ResourceNameList oldsel = this.SelectedItems;
            
            lv.SelectedIndices.Clear();
            names.SortByColumn(sc, asc);
            bool first = true;
            foreach (NamedPackedFileDescriptor pfd in oldsel)
            {
                int i = names.IndexOf(pfd);
                if (i > 0)
                {
                    if (first) lv.EnsureVisible(i);
                    lv.SelectedIndices.Add(i);
                    first = false;
                }
            }
            EndUpdate(false);
        }

        private void lv_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            ResourceViewManager.SortColumn tmp = (ResourceViewManager.SortColumn.Offset);
            if (lv.Columns[e.Column] == clTName) tmp = ResourceViewManager.SortColumn.Name;
            else if (lv.Columns[e.Column] == clType) tmp = ResourceViewManager.SortColumn.Extension;
            else if (lv.Columns[e.Column] == clGroup) tmp = ResourceViewManager.SortColumn.Group;
            else if (lv.Columns[e.Column] == clInstHi) tmp = ResourceViewManager.SortColumn.InstanceHi;
            else if (lv.Columns[e.Column] == clInst) tmp = ResourceViewManager.SortColumn.InstanceLo;
            else if (lv.Columns[e.Column] == clSize) tmp = ResourceViewManager.SortColumn.Size;
            else if (lv.Columns[e.Column] == clOffset) tmp = ResourceViewManager.SortColumn.Offset;

            if (tmp == SortedColumn) asc = !asc;
            SortedColumn = tmp;
        }

    }
}
