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
using System.Text;

namespace SimPe.Windows.Forms
{
    class ResoureNameSorter
    {         
        Stack<NamedPackedFileDescriptor> names;
        int started;
        int counter;
        ResourceListViewExt parent;
        IntPtr handle;
        int ticket;
        public ResoureNameSorter(ResourceListViewExt parent, ResourceViewManager.ResourceNameList names, int ticket)
        {
            int numberofthreads = Helper.WindowsRegistry.SortProcessCount;
            handle = parent.Handle;
            this.parent = parent;
            this.ticket = ticket;
            this.names = new Stack<NamedPackedFileDescriptor>();
            foreach (NamedPackedFileDescriptor pfd in names)
                this.names.Push(pfd);
            

            counter = 0;
            if (Helper.WindowsRegistry.AsynchronSort)
            {
                started = numberofthreads;                
                for (int i = 0; i < numberofthreads; i++)
                {
                    System.Threading.Thread t = new System.Threading.Thread(new System.Threading.ThreadStart(ReadNames));
                    t.Name = "Resource Sorting Thread " + i + "." + Helper.HexString(ticket);
                    t.Start();
                   
                }
            }
            else
            {
                started = 1;
                ReadNames();
            }
        }

        public void Cancel()
        {
            lock (names)
            {
                names.Clear();
                started = 0;
            }
        }

        void ReadNames()
        {
            while (names.Count > 0)
            {
                NamedPackedFileDescriptor pfd = null;
                lock (names)
                {
                    if (names.Count == 0) break;
                    pfd = names.Pop();
                    if (Helper.WindowsRegistry.AsynchronSort)
                        SimPe.Wait.Progress = counter++;
                }
                pfd.GetRealName();
            }

            lock (names)
            {
                started--;
                if (started == 0)
                {
                    parent.SignalFinishedSort(handle, ticket);
                }
            }
            
        }
    }
}
