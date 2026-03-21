/***************************************************************************
 *   Copyright (C) 2005 by Ambertation                                     *
 *   quaxi@ambertation.de                                                  *
 *                                                                         *
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

// Ported from WinForms UserControl to Avalonia stub (Mac port).
// All WinForms layout and designer code removed. Public API preserved.

using System;
using Avalonia.Controls;
using Avalonia.Layout;

namespace SimPe.Plugin.Anim
{
    /// <summary>
    /// Avalonia UserControl replacement for the WinForms AnimFrameBlockControl.
    /// </summary>
    public class AnimFrameBlockControl : Avalonia.Controls.UserControl
    {
        // ── Public properties ────────────────────────────────────────────────────
        AnimationFrameBlock afb;
        public AnimationFrameBlock FrameBlock
        {
            get { return afb; }
            set
            {
                afb = value;
                RefreshData();
            }
        }

        // ── Events ───────────────────────────────────────────────────────────────
        public event EventHandler Changed;

        public AnimFrameBlockControl()
        {
            BuildLayout();
            Clear();
        }

        private void BuildLayout()
        {
            var panel = new StackPanel { Spacing = 4 };
            panel.Children.Add(new TextBlock { Text = "Animation Frame Block" });
            Content = panel;
        }

        public void Clear()
        {
            afb = null;
        }

        private void RefreshData()
        {
            // no-op stub — data binding will be done via Avalonia MVVM in a future pass
        }
    }
}
