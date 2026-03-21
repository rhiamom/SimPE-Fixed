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
    /// Avalonia UserControl replacement for the WinForms AnimAxisTransformControl.
    /// </summary>
    public class AnimAxisTransformControl : Avalonia.Controls.UserControl
    {
        // ── Public properties ────────────────────────────────────────────────────
        AnimationAxisTransform aat;
        public AnimationAxisTransform AxisTransform
        {
            get { return aat; }
            set
            {
                aat = value;
                RefreshData();
            }
        }

        private Avalonia.Controls.Button llAdd;
        public bool CanCreate
        {
            get { return llAdd?.IsVisible ?? false; }
            set { if (llAdd != null) llAdd.IsVisible = value; }
        }

        // ── Events ───────────────────────────────────────────────────────────────
        public event EventHandler Deleted;
        public event EventHandler Changed;
        public event EventHandler Cleared;
        public event EventHandler CreateClicked;

        public AnimAxisTransformControl()
        {
            BuildLayout();
            CanCreate = false;
            Clear();
        }

        private void BuildLayout()
        {
            llAdd = new Avalonia.Controls.Button { Content = "Add", IsVisible = false };
            llAdd.Click += (s, e) => CreateClicked?.Invoke(this, EventArgs.Empty);

            var deleteBtn = new Avalonia.Controls.Button { Content = "Delete" };
            deleteBtn.Click += (s, e) => Deleted?.Invoke(this, EventArgs.Empty);

            var panel = new StackPanel { Spacing = 4 };
            panel.Children.Add(llAdd);
            panel.Children.Add(deleteBtn);
            Content = panel;
        }

        public void Clear()
        {
            aat = null;
            Cleared?.Invoke(this, EventArgs.Empty);
        }

        private void RefreshData()
        {
            // no-op stub — data binding will be done via Avalonia MVVM in a future pass
        }
    }
}
