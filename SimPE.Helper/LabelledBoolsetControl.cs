/***************************************************************************
 *   Copyright (C) 2008 by Ambertation                                     *
 *   pljones@users.sf.net                                                  *
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

using System;
using System.Collections.Generic;
using System.ComponentModel;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Layout;

namespace SimPe
{
    /// <summary>
    /// Avalonia UserControl replacing the WinForms CheckedListBox-based boolset editor.
    /// Moved from namespace System.Windows.Forms to namespace SimPe.
    /// CheckedListBox replaced with a StackPanel of CheckBox controls inside a ScrollViewer.
    /// </summary>
    public class LabelledBoolsetControl : UserControl
    {
        Boolset boolset = (ushort)0;
        List<string> labels = new List<string>();

        readonly StackPanel checkStack = new StackPanel();
        readonly List<CheckBox> checkBoxes = new List<CheckBox>();
        readonly Button btnAll;
        readonly Button btnNone;

        bool _suppressEvents = false;

        public LabelledBoolsetControl()
        {
            btnAll = new Button { Content = "All", MinWidth = 45, Margin = new Thickness(2) };
            btnNone = new Button { Content = "None", MinWidth = 45, Margin = new Thickness(2) };
            btnAll.Click += BtnAll_Click;
            btnNone.Click += BtnNone_Click;

            var buttonRow = new StackPanel
            {
                Orientation = Orientation.Horizontal,
                Children = { btnAll, btnNone }
            };

            var scrollViewer = new ScrollViewer
            {
                Content = checkStack,
                HorizontalScrollBarVisibility = Avalonia.Controls.Primitives.ScrollBarVisibility.Disabled,
                VerticalScrollBarVisibility = Avalonia.Controls.Primitives.ScrollBarVisibility.Auto
            };

            var root = new DockPanel { LastChildFill = true };
            DockPanel.SetDock(buttonRow, Dock.Top);
            root.Children.Add(buttonRow);
            root.Children.Add(scrollViewer);

            Content = root;
        }

        void BtnAll_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            _suppressEvents = true;
            foreach (var cb in checkBoxes) cb.IsChecked = true;
            _suppressEvents = false;
            ushort old = boolset;
            boolset = (ushort)0xffff;
            if (old != (ushort)boolset) OnValueChanged(this, EventArgs.Empty);
        }

        void BtnNone_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            _suppressEvents = true;
            foreach (var cb in checkBoxes) cb.IsChecked = false;
            _suppressEvents = false;
            ushort old = boolset;
            boolset = (ushort)0;
            if (old != (ushort)boolset) OnValueChanged(this, EventArgs.Empty);
        }

        [Browsable(true)]
        [Description("Show or Hide the All/None buttons")]
        public bool ButtonsVisible
        {
            get => btnAll.IsVisible && btnNone.IsVisible;
            set => btnAll.IsVisible = btnNone.IsVisible = value;
        }

        [Browsable(true)]
        [Description("The unsigned short value representing the bit set to be edited")]
        public ushort Value
        {
            get => (ushort)boolset;
            set
            {
                ushort oldvalue = boolset;
                boolset = value;
                RebuildCheckBoxes();
                if (oldvalue != (ushort)boolset)
                    OnValueChanged(this, EventArgs.Empty);
            }
        }

        /// <summary>Indicates the Value changed.</summary>
        [Description("Indicates the Value changed")]
        public event EventHandler? ValueChanged;

        public virtual void OnValueChanged(object sender, EventArgs e)
        {
            ValueChanged?.Invoke(sender, e);
        }

        [Browsable(true)]
        [Description("The collection representing the labels for the bits")]
        public List<string> Labels
        {
            get => labels;
            set
            {
                labels = value;
                RebuildCheckBoxes();
            }
        }

        void RebuildCheckBoxes()
        {
            while (labels.Count < boolset.Length) labels.Add(labels.Count.ToString());

            _suppressEvents = true;
            checkStack.Children.Clear();
            checkBoxes.Clear();

            for (int i = 0; i < boolset.Length; i++)
            {
                string label = i < labels.Count ? labels[i] : i.ToString();
                var cb = new CheckBox
                {
                    Content = label,
                    IsChecked = boolset[i],
                    Margin = new Thickness(2, 1, 2, 1)
                };
                int idx = i; // capture
                cb.IsCheckedChanged += (_, _) =>
                {
                    if (_suppressEvents) return;
                    ushort old = boolset;
                    boolset[idx] = cb.IsChecked == true;
                    if (old != (ushort)boolset)
                        OnValueChanged(this, EventArgs.Empty);
                };
                checkBoxes.Add(cb);
                checkStack.Children.Add(cb);
            }

            _suppressEvents = false;
        }
    }
}
