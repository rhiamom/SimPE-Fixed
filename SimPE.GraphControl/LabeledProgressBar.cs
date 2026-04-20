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

using System;
using System.Drawing;
using System.Windows.Forms;

namespace Ambertation.Windows.Forms
{
    /// <summary>
    /// Stub implementation of LabeledProgressBar used by ExtSDescUI.
    /// Provides the same public surface as the original GraphControl version
    /// but without the advanced rendering. Enough for compatibility.
    /// </summary>
    public class LabeledProgressBar : Panel
    {
        private readonly Label captionLabel;
        private readonly TextBox valueBox;
        private readonly ProgressBar bar;
        private bool syncing; // re-entrancy guard between bar click and textbox edit

        public LabeledProgressBar()
        {
            captionLabel = new Label();
            valueBox = new TextBox();
            bar = new ProgressBar();

            captionLabel.AutoSize = true;
            captionLabel.Dock = DockStyle.Left;
            captionLabel.TextAlign = ContentAlignment.MiddleLeft;

            // Editable numeric display: user can type a value, Enter/Leave commits.
            valueBox.Dock = DockStyle.Right;
            valueBox.TextAlign = HorizontalAlignment.Right;
            valueBox.BorderStyle = BorderStyle.None;
            valueBox.Width = 40;
            valueBox.KeyDown += ValueBox_KeyDown;
            valueBox.Leave += ValueBox_Leave;

            // Click or drag on the bar to set the value by position.
            bar.Dock = DockStyle.Fill;
            bar.MouseDown += Bar_MouseDownOrMove;
            bar.MouseMove += Bar_MouseDownOrMove;

            // order matters with Dock: add Fill first, then the sides
            Controls.Add(bar);
            Controls.Add(valueBox);
            Controls.Add(captionLabel);

            // Default values
            DisplayOffset = 0;
            NumberScale = 1.0;
            TokenCount = 0;
            SelectedColor = SystemColors.Highlight;
            UnselectedColor = SystemColors.ControlDark;
            NumberFormat = "0";
            Style = ProgresBarStyle.Normal;
        }

        private void Bar_MouseDownOrMove(object sender, MouseEventArgs e)
        {
            if (!Enabled || (e.Button & MouseButtons.Left) != MouseButtons.Left) return;
            int w = Math.Max(1, bar.ClientSize.Width);
            int range = bar.Maximum - bar.Minimum;
            int x = Math.Max(0, Math.Min(w, e.X));
            int newRaw = bar.Minimum + (int)Math.Round((double)x / w * range);
            if (newRaw == bar.Value) return;
            Value = newRaw;
        }

        private void ValueBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter || e.KeyCode == Keys.Return)
            {
                CommitTypedValue();
                e.SuppressKeyPress = true;
            }
        }

        private void ValueBox_Leave(object sender, EventArgs e) => CommitTypedValue();

        private void CommitTypedValue()
        {
            if (syncing) return;
            if (!double.TryParse(valueBox.Text, out double typed))
            {
                UpdateLabelText(); // revert display
                return;
            }
            // Reverse the display transform: displayed = (raw + numberOffset)*scale + displayOffset
            double scale = numberScale == 0 ? 1 : numberScale;
            int rawUnclamped = (int)Math.Round((typed - displayOffset) / scale) - numberOffset;
            Value = rawUnclamped;
        }

        // ─────────────────────────────────────────────────────────
        //     Basic UI properties
        // ─────────────────────────────────────────────────────────

        public override string Text
        {
            get => base.Text;
            set
            {
                base.Text = value;
                captionLabel.Text = value;
            }
        }

        public string LabelText
        {
            get => captionLabel.Text;
            set => captionLabel.Text = value;
        }

        public int Value
        {
            get => bar.Value;
            set
            {
                int v = value;
                if (v < bar.Minimum) v = bar.Minimum;
                if (v > bar.Maximum) v = bar.Maximum;

                bar.Value = v;
                UpdateLabelText();
                Changed?.Invoke(this, EventArgs.Empty);
                ChangedValue?.Invoke(this, EventArgs.Empty);
            }
        }


        public int Maximum
        {
            get => bar.Maximum;
            set => bar.Maximum = value;
        }

        public int Minimum
        {
            get => bar.Minimum;
            set => bar.Minimum = value;
        }

        // ─────────────────────────────────────────────────────────
        //     Extended properties expected by ExtSDescUI
        // ─────────────────────────────────────────────────────────

        private double numberScale = 1.0;
        private int displayOffset = 0;
        private Color selectedColor = Color.Lime;
        private Color unselectedColor = Color.Black;
        private int tokenCount = 10;
        private string numberFormat = "{0}";
        private int numberOffset = 0;

        public double NumberScale
        {
            get => numberScale;
            set
            {
                numberScale = value;
                UpdateLabelText();
            }
        }

        public int NumberOffset
        {
            get => numberOffset;
            set
            {
                numberOffset = value;
                UpdateLabelText();
            }
        }
        public int DisplayOffset
        {
            get => displayOffset;
            set
            {
                displayOffset = value;
                UpdateLabelText();
            }
        }

        public Color SelectedColor
        {
            get => selectedColor;
            set
            {
                selectedColor = value;
                Invalidate(); // we’re not doing fancy painting yet, but this is future-proof
            }
        }

        public Color UnselectedColor
        {
            get => unselectedColor;
            set
            {
                unselectedColor = value;
                Invalidate();
            }
        }

        public int TokenCount
        {
            get => tokenCount;
            set
            {
                tokenCount = value;
                Invalidate();
            }
        }

        public string NumberFormat
        {
            get => numberFormat;
            set
            {
                numberFormat = string.IsNullOrEmpty(value) ? "{0}" : value;
                UpdateLabelText();
            }
        }

        public override Font Font
        {
            get => base.Font;
            set
            {
                base.Font = value;
                captionLabel.Font = value;
                valueBox.Font = value;
            }
        }

        public override Color ForeColor
        {
            get => base.ForeColor;
            set
            {
                base.ForeColor = value;
                captionLabel.ForeColor = value;
                valueBox.ForeColor = value;
            }
        }


        private ProgresBarStyle style = ProgresBarStyle.Normal;

        public ProgresBarStyle Style
        {
            get => style;
            set
            {
                style = value;
                Invalidate();
            }
        }

        // ─────────────────────────────────────────────────────────
        //     Events expected by ExtSDescUI
        // ─────────────────────────────────────────────────────────
        private void UpdateLabelText()
        {
            double scaled = (bar.Value + numberOffset) * numberScale + displayOffset;

            string display;
            // If numberFormat is "{0}" or "{0:N2}", treat it as a composite format string.
            if (!string.IsNullOrEmpty(numberFormat) && numberFormat.Contains("{0"))
                display = string.Format(numberFormat, scaled);
            else
                display = scaled.ToString(string.IsNullOrEmpty(numberFormat) ? "0" : numberFormat);

            // Don't clobber what the user is typing.
            if (valueBox.Focused) return;

            syncing = true;
            try { valueBox.Text = display; }
            finally { syncing = false; }
        }


        /// <summary>
        /// Raised when the bar appearance or value changes.
        /// </summary>
        public event EventHandler Changed;

        /// <summary>
        /// Raised specifically when the numeric value changes.
        /// </summary>
        public event EventHandler ChangedValue;

    }
}
