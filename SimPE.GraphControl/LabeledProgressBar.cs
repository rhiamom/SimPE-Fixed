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
 ***************************************************************************/

using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Layout;
using Avalonia.Media;

namespace Ambertation.Windows.Forms
{
    /// <summary>
    /// A progress bar with a caption label on the left and a value label on
    /// the right.  Ported from the WinForms Panel-based version; uses an
    /// Avalonia Grid for layout.
    /// </summary>
    public class LabeledProgressBar : UserControl
    {
        readonly TextBlock captionLabel;
        readonly TextBlock valueLabel;
        readonly ProgressBar bar;
        readonly Grid grid;

        public LabeledProgressBar()
        {
            captionLabel = new TextBlock { VerticalAlignment = VerticalAlignment.Center };
            valueLabel   = new TextBlock { VerticalAlignment = VerticalAlignment.Center, TextAlignment = Avalonia.Media.TextAlignment.Right };
            bar          = new ProgressBar { VerticalAlignment = VerticalAlignment.Center };

            grid = new Grid();
            grid.ColumnDefinitions.Add(new ColumnDefinition(GridLength.Auto));
            grid.ColumnDefinitions.Add(new ColumnDefinition(GridLength.Star));
            grid.ColumnDefinitions.Add(new ColumnDefinition(GridLength.Auto));

            Grid.SetColumn(captionLabel, 0);
            Grid.SetColumn(bar,          1);
            Grid.SetColumn(valueLabel,   2);

            grid.Children.Add(captionLabel);
            grid.Children.Add(bar);
            grid.Children.Add(valueLabel);

            Content = grid;

            // Defaults
            DisplayOffset   = 0;
            NumberScale     = 1.0;
            TokenCount      = 0;
            NumberFormat    = "0";
            Style_          = ProgresBarStyle.Normal;
        }

        // ─────────────────────────────────────────────────────────
        //     Basic UI properties
        // ─────────────────────────────────────────────────────────

        public string LabelText
        {
            get => captionLabel.Text;
            set => captionLabel.Text = value;
        }

        public int Value
        {
            get => (int)bar.Value;
            set
            {
                double v = Math.Max(bar.Minimum, Math.Min(bar.Maximum, value));
                bar.Value = v;
                UpdateLabelText();
                Changed?.Invoke(this, EventArgs.Empty);
                ChangedValue?.Invoke(this, EventArgs.Empty);
            }
        }

        public double Maximum
        {
            get => bar.Maximum;
            set => bar.Maximum = value;
        }

        public double Minimum
        {
            get => bar.Minimum;
            set => bar.Minimum = value;
        }

        // ─────────────────────────────────────────────────────────
        //     Extended properties
        // ─────────────────────────────────────────────────────────

        double numberScale = 1.0;
        int displayOffset = 0;
        int tokenCount = 10;
        string numberFormat = "{0}";
        int numberOffset = 0;
        System.Drawing.Color selectedColor   = System.Drawing.Color.Lime;
        System.Drawing.Color unselectedColor = System.Drawing.Color.Black;

        public double NumberScale
        {
            get => numberScale;
            set { numberScale = value; UpdateLabelText(); }
        }

        public int NumberOffset
        {
            get => numberOffset;
            set { numberOffset = value; UpdateLabelText(); }
        }

        public int DisplayOffset
        {
            get => displayOffset;
            set { displayOffset = value; UpdateLabelText(); }
        }

        public System.Drawing.Color SelectedColor
        {
            get => selectedColor;
            set { selectedColor = value; InvalidateVisual(); }
        }

        public System.Drawing.Color UnselectedColor
        {
            get => unselectedColor;
            set { unselectedColor = value; InvalidateVisual(); }
        }

        public int TokenCount
        {
            get => tokenCount;
            set { tokenCount = value; InvalidateVisual(); }
        }

        public string NumberFormat
        {
            get => numberFormat;
            set { numberFormat = string.IsNullOrEmpty(value) ? "{0}" : value; UpdateLabelText(); }
        }

        ProgresBarStyle style_ = ProgresBarStyle.Normal;
        public ProgresBarStyle Style_
        {
            get => style_;
            set { style_ = value; InvalidateVisual(); }
        }

        // ─────────────────────────────────────────────────────────
        //     Events
        // ─────────────────────────────────────────────────────────
        private void UpdateLabelText()
        {
            double scaled = ((int)bar.Value + numberOffset) * numberScale + displayOffset;
            if (!string.IsNullOrEmpty(numberFormat) && numberFormat.Contains("{0"))
                valueLabel.Text = string.Format(numberFormat, scaled);
            else
                valueLabel.Text = scaled.ToString(string.IsNullOrEmpty(numberFormat) ? "0" : numberFormat);
        }

        public event EventHandler Changed;
        public event EventHandler ChangedValue;
    }
}
