/***************************************************************************
 *   Copyright (C) 2005 by Ambertation                                     *
 *   quaxi@ambertation.de                                                  *
 *                                                                         *
 *   Copyright (C) 2008 by Peter L Jones                                   *
 *   pljones@users.sf.net                                                  *
 *                                                                         *
 *   Copyright (C) 2008 by GramzeSweatShop                                 *
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
using System.Drawing;       // Size is a cross-platform value type — safe
using System.Threading;
using Avalonia.Controls;
using AvBitmap = Avalonia.Media.Imaging.Bitmap;

namespace SimPe
{
    public class WaitingScreen
    {
        /// <summary>Display a new WaitingScreen image.</summary>
        public static void UpdateImage(AvBitmap? image) { Screen.doUpdate(image); }
        /// <summary>The WaitingScreen image.</summary>
        public static AvBitmap? Image { get { return scr == null ? null : scr.prevImage; } set { Screen.doUpdate(value); } }
        /// <summary>Display a new WaitingScreen message.</summary>
        public static void UpdateMessage(string msg) { Screen.doUpdate(msg); }
        /// <summary>The WaitingScreen message.</summary>
        public static string Message { get { return scr == null ? "" : scr.prevMessage; } set { Screen.doUpdate(value); } }
        /// <summary>Display a new WaitingScreen image and message.</summary>
        public static void Update(AvBitmap? image, string msg) { Screen.doUpdate(image, msg); }
        /// <summary>Show the WaitingScreen for a specific window.</summary>
        public static void Wait(Window form) { Screen.doWait(form); }
        /// <summary>Show the WaitingScreen.</summary>
        public static void Wait() { Screen.doWait(null); }
        /// <summary>Stop the WaitingScreen and activate the given window.</summary>
        public static void Stop(Window form) { Stop(); form.Activate(); }
        /// <summary>Stop the WaitingScreen.</summary>
        public static void Stop() { if (Running) Screen.doStop(); }
        /// <summary>True if the WaitingScreen is displayed.</summary>
        public static bool Running { get { return count > 0; } }
        /// <summary>Returns the Size of the displayed image.</summary>
        public static Size ImageSize { get { return new Size(64, 64); } }


        static WaitingScreen scr;
        static object lockFrm = new object();
        static object lockScr = new object();
        static uint count = 0;
        static WaitingScreen Screen
        {
            get
            {
                System.Diagnostics.Trace.WriteLine("SimPe.WaitingScreen.getScreen: " + count);
                lock (lockScr)
                {
                    if (scr == null)
                        scr = new WaitingScreen();
                }
                return scr;
            }
        }


        AvBitmap? prevImage = null;
        string prevMessage = "";
        SimPe.WaitingForm frm;

        Window? parent = null;
        void doUpdate(AvBitmap? image) { System.Diagnostics.Trace.WriteLine("SimPe.WaitingScreen.doUpdate(image): " + count); lock (lockFrm) { prevImage = image; if (frm != null) frm.SetImage(image); } }
        void doUpdate(string msg) { System.Diagnostics.Trace.WriteLine("SimPe.WaitingScreen.doUpdate(message): " + msg + ", " + count); lock (lockFrm) { prevMessage = msg; if (frm != null) frm.SetMessage(msg); } }
        void doUpdate(AvBitmap? image, string msg) { doUpdate(image); doUpdate(msg); }
        void doWait(Window? form)
        {
            System.Diagnostics.Trace.WriteLine("SimPe.WaitingScreen.doWait(...): "); ++count;
            if (count > 1) return;

            if (!Helper.XmlRegistry.WaitingScreen) return;
            lock (lockFrm)
            {
                if (parent != form)
                {
                    if (parent != null) parent.Activated -= parent_Activated;
                    parent = form;
                    if (parent != null) parent.Activated += parent_Activated;
                }
                parent_Activated(null, EventArgs.Empty);
            }
        }

        void doStop()
        {
            System.Diagnostics.Trace.WriteLine("SimPe.WaitingScreen.doStop(): ");
            count--;
            if (parent != null && count == 0) parent.Activate();
            lock (lockFrm) { if (frm != null) frm.StopSplash(); }
        }

        void parent_Activated(object? sender, EventArgs e) { if (frm != null && count > 0) { frm.StartSplash(); } }

        private WaitingScreen()
        {
            System.Diagnostics.Trace.WriteLine("SimPe.WaitingScreen..ctor(): " + count);
            if (Helper.XmlRegistry.WaitingScreen)
            {
                lock (lockFrm)
                {
                    frm = new SimPe.WaitingForm();
                    System.Diagnostics.Trace.WriteLine("SimPe.WaitingScreen..ctor() - created new SimPe.WaitingForm()");
                    frm.Closed += frm_Closed;
                    prevImage = frm.Image;
                    prevMessage = frm.Message;
                    doUpdate(prevImage, prevMessage);
                }
            }
        }

        void frm_Closed(object? sender, EventArgs e)
        {
            System.Diagnostics.Trace.WriteLine("SimPe.WaitingScreen.frm_Closed(...)");
            lock (lockFrm)
            {
                frm = null;
                lock (lockScr)
                {
                    scr = null;
                }
            }
            count = 0;
        }
    }
}
