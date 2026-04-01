/***************************************************************************
 *   Copyright (C) 2025 by GramzeSweatShop                                 *
 *   Rhiamom@mac.com                                                       *
 *                                                                         *
 *   This program is free software; you can redistribute it and/or modify  *
 *   it under the terms of the GNU General Public License as published by  *
 *   the Free Software Foundation; either version 2 of the License, or     *
 *   (at your option) any later version.                                   *
 ***************************************************************************/

using System;
using System.Collections.Generic;

namespace pjOBJDTool
{
    /// <summary>
    /// Avalonia UserControl that displays 16 labelled checkboxes representing a ushort bitmask.
    /// Bit 0 is the first (leftmost/topmost) checkbox.
    /// </summary>
    public class LabelledBoolsetControl : Avalonia.Controls.UserControl
    {
        private ushort _value = 0;
        private bool _updating = false;
        private readonly List<Avalonia.Controls.CheckBox> _checkBoxes = new List<Avalonia.Controls.CheckBox>();
        private readonly Avalonia.Controls.WrapPanel _panel;

        public event EventHandler ValueChanged;

        public LabelledBoolsetControl()
        {
            _panel = new Avalonia.Controls.WrapPanel();
            this.Content = _panel;
        }

        public ushort Value
        {
            get => _value;
            set
            {
                if (_value == value) return;
                _value = value;
                SyncCheckBoxes();
            }
        }

        public List<string> Labels
        {
            set
            {
                _panel.Children.Clear();
                _checkBoxes.Clear();
                for (int i = 0; i < value.Count && i < 16; i++)
                {
                    var cb = new Avalonia.Controls.CheckBox { Content = value[i], Margin = new Avalonia.Thickness(2) };
                    cb.IsCheckedChanged += CheckBox_Changed;
                    _checkBoxes.Add(cb);
                    _panel.Children.Add(cb);
                }
                SyncCheckBoxes();
            }
        }

        private void SyncCheckBoxes()
        {
            _updating = true;
            for (int i = 0; i < _checkBoxes.Count; i++)
                _checkBoxes[i].IsChecked = (_value & (1 << i)) != 0;
            _updating = false;
        }

        private void CheckBox_Changed(object sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            if (_updating) return;
            ushort newValue = 0;
            for (int i = 0; i < _checkBoxes.Count; i++)
                if (_checkBoxes[i].IsChecked == true)
                    newValue |= (ushort)(1 << i);
            _value = newValue;
            ValueChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
