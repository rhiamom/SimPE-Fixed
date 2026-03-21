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

// Ported from WinForms UserControl to Avalonia UserControl.
// Layout built programmatically; no AXAML file required.
// PictureBox replaced with TextBlock showing state symbol.

using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Layout;
using Avalonia.Media;

namespace SimPe
{
    /// <summary>
    /// Displays a single environment-check row with status icon, caption,
    /// Fix link, Details toggle, and a collapsible detail text panel.
    /// Ported to Avalonia; layout built in code.
    /// </summary>
    [System.ComponentModel.DefaultEvent("ClickedFix")]
    public class CheckItem : Avalonia.Controls.UserControl
    {
        public delegate CheckItemState FixEventHandler(object sender, CheckItemState isok);

        // ── State ────────────────────────────────────────────────────────
        bool cf;
        string txt;
        CheckItemState cs;
        string det;

        // ── Controls ─────────────────────────────────────────────────────
        TextBlock lbCaption;    // was Label lb
        TextBlock pbState;      // was PictureBox pb — shows "?", "OK", "!!", "?!"
        Button btnFix;          // was LinkLabel llfix
        Button btnDetails;      // was LinkLabel lldet
        Button btnHideDetails;  // was LinkLabel linkLabel1
        Border pnDetails;       // was Panel pnDetails
        TextBox rtb;            // was RichTextBox rtb

        public CheckItem()
        {
            cs  = CheckItemState.Unknown;
            txt = "--";
            cf  = false;
            det = "";

            lbCaption = new TextBlock
            {
                VerticalAlignment = VerticalAlignment.Center,
                Margin = new Thickness(4, 0)
            };

            pbState = new TextBlock
            {
                Text = "?",
                Width = 28,
                TextAlignment = TextAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                FontWeight = FontWeight.Bold
            };

            btnFix = new Button
            {
                Content = "Fix",
                IsVisible = false,
                Margin = new Thickness(4, 0)
            };

            btnDetails = new Button
            {
                Content = "Details »",
                IsVisible = false,
                Margin = new Thickness(4, 0)
            };

            btnHideDetails = new Button
            {
                Content = "« Hide",
                Margin = new Thickness(0, 2)
            };

            rtb = new TextBox
            {
                IsReadOnly = true,
                AcceptsReturn = true,
                TextWrapping = TextWrapping.Wrap,
                MinHeight = 60
            };

            pnDetails = new Border
            {
                IsVisible = false,
                Padding = new Thickness(4),
                Child = new StackPanel
                {
                    Children = { rtb, btnHideDetails }
                }
            };

            btnFix.Click += (s, e) =>
            {
                CheckItemState res = cs;
                OnFix();
                if (ClickedFix != null) res = ClickedFix(this, res);
                CheckState = res;
            };

            btnDetails.Click      += (s, e) => { pnDetails.IsVisible = true; };
            btnHideDetails.Click  += (s, e) => { pnDetails.IsVisible = false; };

            var topRow = new StackPanel
            {
                Orientation = Orientation.Horizontal,
                Children    = { pbState, lbCaption, btnFix, btnDetails }
            };

            Content = new StackPanel
            {
                Children = { topRow, pnDetails }
            };

            SetContent();
        }

        // ── Public properties ─────────────────────────────────────────────
        public bool CanFix
        {
            get => cf;
            set { if (cf != value) { cf = value; SetContent(); } }
        }

        [System.ComponentModel.Localizable(true)]
        public string Caption
        {
            get => txt;
            set { txt = value; SetContent(); }
        }

        public CheckItemState CheckState
        {
            get => cs;
            set
            {
                cs = value;
                pbState.Text = cs == CheckItemState.Ok      ? "OK"
                             : cs == CheckItemState.Fail    ? "!!"
                             : cs == CheckItemState.Warning ? "?!"
                             : "?";
                SetContent();
            }
        }

        public string Details
        {
            get => det;
            set { det = value; SetContent(); }
        }

        // ── Internal helpers ──────────────────────────────────────────────
        protected virtual void SetContent()
        {
            rtb.Text          = det;
            btnDetails.IsVisible = !string.IsNullOrWhiteSpace(det);
            lbCaption.Text    = txt;
            btnFix.IsVisible  = cs == CheckItemState.Fail && cf;
        }

        protected virtual void OnFix() { }

        public void Reset()
        {
            pnDetails.IsVisible = false;
            CheckState = CheckItemState.Unknown;
            det = "";
            SetContent();
        }

        // ── Events ────────────────────────────────────────────────────────
        public event FixEventHandler CalledCheck;

        protected virtual CheckItemState OnCheck() => CheckItemState.Ok;

        public void Check()
        {
            CheckItemState res = OnCheck();
            if (CalledCheck != null) res = CalledCheck(this, res);
            CheckState = res;
        }

        public event FixEventHandler ClickedFix;
    }
}
