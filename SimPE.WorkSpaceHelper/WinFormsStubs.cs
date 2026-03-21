/***************************************************************************
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

// Minimal WinForms stubs required to compile LoadFileWrappers.cs (Mac/Avalonia port).
// Only the members actually used by ToolMenuItem are provided.

using System;

namespace System.Windows.Forms
{
    [Flags]
    public enum AnchorStyles { None = 0, Top = 1, Bottom = 2, Left = 4, Right = 8 }

    /// <summary>Minimal stub for System.Windows.Forms.MessageBoxButtons.</summary>
    public enum MessageBoxButtons { OK, OKCancel, YesNo, YesNoCancel, AbortRetryIgnore, RetryCancel }

    /// <summary>Minimal stub for System.Windows.Forms.Padding.</summary>
    public struct Padding
    {
        public int Left, Top, Right, Bottom;
        public Padding(int all) { Left = Top = Right = Bottom = all; }
        public Padding(int left, int top, int right, int bottom)
        { Left = left; Top = top; Right = right; Bottom = bottom; }
    }

    /// <summary>
    /// Minimal stub for System.Windows.Forms.ToolStripItem.
    /// </summary>
    public class ToolStripItem
    {
        public string Text    { get; set; } = string.Empty;
        public bool   Enabled { get; set; } = true;
        public bool   Visible { get; set; } = true;
        public object Tag     { get; set; }
        public object Image   { get; set; }
        public event EventHandler Click;
        protected virtual void OnClick(EventArgs e) => Click?.Invoke(this, e);
    }

    /// <summary>
    /// Minimal stub for System.Windows.Forms.ToolStripMenuItem.
    /// Only the members used by SimPe.ToolMenuItem are implemented.
    /// </summary>
    public class ToolStripMenuItem : ToolStripItem
    {
    }
}
