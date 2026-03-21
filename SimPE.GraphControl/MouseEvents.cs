/***************************************************************************
 *   Copyright (C) 2025 by GramzeSweatshop                                 *
 *   rhiamom@mac.com                                                       *
 *                                                                         *
 *   This program is free software; you can redistribute it and/or modify  *
 *   it under the terms of the GNU General Public License as published by  *
 *   the Free Software Foundation; either version 2 of the License, or     *
 *   (at your option) any later version.                                   *
 ***************************************************************************/

// Local replacements for System.Windows.Forms mouse event infrastructure.
// The Ambertation graph panel hierarchy uses these types internally;
// callers that previously used System.Windows.Forms equivalents must be
// updated to reference these when porting.

namespace Ambertation.Windows.Forms
{
    /// <summary>Mouse button flags, matching the WinForms MouseButtons values.</summary>
    public enum MouseButtons
    {
        None   = 0,
        Left   = 0x00100000,
        Right  = 0x00200000,
        Middle = 0x00400000,
        XButton1 = 0x00800000,
        XButton2 = 0x01000000,
    }

    /// <summary>Mouse event data passed through the graph panel element hierarchy.</summary>
    public class MouseEventArgs : System.EventArgs
    {
        public MouseButtons Button { get; }
        public int Clicks { get; }
        public int X { get; }
        public int Y { get; }
        public int Delta { get; }

        public MouseEventArgs(MouseButtons button, int clicks, int x, int y, int delta)
        {
            Button = button;
            Clicks = clicks;
            X = x;
            Y = y;
            Delta = delta;
        }
    }

    /// <summary>Delegate for graph element mouse events.</summary>
    public delegate void MouseEventHandler(object sender, MouseEventArgs e);
}
