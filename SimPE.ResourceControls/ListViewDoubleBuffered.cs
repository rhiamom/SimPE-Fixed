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
    public class ListViewDoubleBuffered : System.Windows.Forms.ListView
    {
        public ListViewDoubleBuffered()
            :base()
        {
            SetStyle(System.Windows.Forms.ControlStyles.OptimizedDoubleBuffer, true);
        }

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        static extern IntPtr SendMessage(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam);
        const int WM_LBUTTONDOWN = 0x0201, WM_RBUTTONDOWN = 0x0204;
        const int LVM_GETTOPINDEX = 0x1027, LVM_SCROLL = 0x1014;

        int TopIndex { get { return (int)SendMessage(Handle, LVM_GETTOPINDEX, IntPtr.Zero, IntPtr.Zero); } }

        // Wine: when the list gets keyboard focus from a mouse click, Wine's
        // listview scrolls back to the focused item (row 0 after the list is
        // filled) BEFORE hit-testing the click. If the list was scrolled with
        // the mouse wheel without ever having focus, the first click therefore
        // selects the row at the same screen position counted from the top.
        // Take focus ourselves first and undo any scroll it caused, so the
        // click is hit-tested against the rows the user is looking at.
        // No-op on Windows (focus doesn't move the scroll position there).
        void FocusKeepingScroll()
        {
            if (Focused || !IsHandleCreated || Items.Count == 0) return;
            int top = TopIndex;
            Focus();
            int moved = TopIndex - top;
            if (moved != 0)
            {
                int rowHeight = GetItemRect(TopIndex).Height;
                SendMessage(Handle, LVM_SCROLL, IntPtr.Zero, (IntPtr)(-moved * rowHeight));
            }
        }

        protected override void WndProc(ref System.Windows.Forms.Message m)
        {
            if (m.Msg == WM_LBUTTONDOWN || m.Msg == WM_RBUTTONDOWN) FocusKeepingScroll();
            base.WndProc(ref m);
        }
    }
}
