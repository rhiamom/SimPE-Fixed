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
// GDI+ painting is preserved via offscreen System.Drawing.Bitmap bridge.

using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using GdiLinearGradientBrush = System.Drawing.Drawing2D.LinearGradientBrush;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using Avalonia.Media.Imaging;

namespace SimPe.Windows.Forms
{
    /// <summary>
    /// Base control for SimPE plugin wrapper UIs.
    /// Provides a gradient background and a branded header bar.
    /// Ported to Avalonia; GDI+ painting preserved via offscreen bitmap.
    /// </summary>
    public class WrapperBaseControl : UserControl, SimPe.Interfaces.Plugin.IPackedFileUI
    {
        /// <summary>
        /// Determines the anchor location of the background image.
        /// </summary>
        public enum ImageLayout
        {
            TopLeft = 0, TopRight = 1, BottomLeft = 2, BottomRight = 3,
            Centered = 4, CenterLeft = 5, CenterRight = 6, CenterTop = 7, CenterBottom = 8
        }

        // ── WinForms layout stub properties (ignored at runtime) ──────────────
        public System.Drawing.Point  Location  { get; set; }
        public System.Drawing.Size   Size      { get; set; }
        public int                   TabIndex  { get; set; }
        public System.Windows.Forms.Padding Margin { get; set; }
        public System.Windows.Forms.AnchorStyles Anchor { get; set; }
        public WFControlCollection   Controls  { get; } = new WFControlCollection();
        public void SuspendLayout()  { }
        public void ResumeLayout(bool performLayout = true) { }
        public void PerformLayout()  { }
        public class WFControlCollection { public void Add(object c) { } }

        // ── Avalonia Commit button ──────────────────────────────────────────
        private Button btCommit;

        public WrapperBaseControl()
        {
            try
            {
                headfont = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold);

                headcol    = System.Drawing.Color.FromArgb(120, 0, 0, 0);
                headend    = System.Drawing.Color.FromArgb(120, 0, 0, 0);
                headforecol = System.Drawing.Color.White;
                headfont   = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold);

                mGradient  = LinearGradientMode.ForwardDiagonal;
                BackgroundColor = System.Drawing.Color.FromArgb(240, 236, 255);
                midcol     = System.Drawing.Color.FromArgb(192, 192, 255);
                gradcol    = System.Drawing.Color.FromArgb(252, 248, 255);
                mCentre    = 0.7f;
                mPicloc    = new System.Drawing.Point(0, 0);
                mPicZoom   = 1.0f;
                mPicOpacity = 1.0f;
                mPicFit    = false;
                bklayout   = ImageLayout.TopLeft;

                txt = "";
                cc  = true;

                // Commit button (docked to bottom-right)
                btCommit = new Button { Content = "Commit", IsVisible = true };
                btCommit.Click += (s, e) => { Commited?.Invoke(this, EventArgs.Empty); OnCommit(); };

                Content = btCommit;

                SimPe.ThemeManager.Global.AddControl(this);
            }
            catch { }
        }

        ~WrapperBaseControl()
        {
            if (wrp != null) SetWrapper(null);
        }

        // ── Public properties ───────────────────────────────────────────────
        string txt;
        public string HeaderText
        {
            get => txt;
            set { if (txt != value) { txt = value; InvalidateVisual(); } }
        }

        System.Drawing.Color headcol, headend, headforecol;
        System.Drawing.Font headfont;

        public System.Drawing.Color HeadBackColor
        {
            get => headcol;
            set { if (value != headcol) { headcol = value; InvalidateVisual(); } }
        }

        public System.Drawing.Color HeadEndColor
        {
            get => headend;
            set { if (value != headend) { headend = value; InvalidateVisual(); } }
        }

        public System.Drawing.Color HeadForeColor
        {
            get => headforecol;
            set { if (value != headforecol) { headforecol = value; InvalidateVisual(); } }
        }

        public System.Drawing.Font HeadFont
        {
            get => headfont;
            set { if (value != headfont) { headfont = value; InvalidateVisual(); } }
        }

        public int HeaderHeight => 24;

        LinearGradientMode mGradient;
        System.Drawing.Color gradcol, midcol;
        float mCentre;
        System.Drawing.Point mPicloc;
        float mPicZoom, mPicOpacity;
        bool mPicFit;
        ImageLayout bklayout;

        // Called by ThemeManager instead of BackColor (Avalonia Background is a brush)
        public System.Drawing.Color BackgroundColor
        {
            get => _backColor;
            set { _backColor = value; InvalidateVisual(); }
        }
        System.Drawing.Color _backColor = System.Drawing.Color.FromArgb(240, 236, 255);

        public System.Drawing.Color GradientColor
        {
            get => gradcol;
            set { if (value != gradcol) { gradcol = value; InvalidateVisual(); } }
        }

        public System.Drawing.Color MiddleColor
        {
            get => midcol;
            set { if (value != midcol) { midcol = value; InvalidateVisual(); } }
        }

        public float GradCentre
        {
            get => mCentre;
            set { mCentre = value; InvalidateVisual(); }
        }

        public LinearGradientMode Gradient
        {
            get => mGradient;
            set => mGradient = value;
        }

        bool cc;
        public bool CanCommit
        {
            get => cc;
            set { cc = value; if (btCommit != null) btCommit.IsVisible = cc; }
        }

        public bool BackgroundImageZoomToFit { get => mPicFit; set { mPicFit = value; InvalidateVisual(); } }
        public float BackgroundImageScale { get => mPicZoom; set { if (!mPicFit) { mPicZoom = value; InvalidateVisual(); } } }
        public System.Drawing.Point BackgroundImageLocation { get => mPicloc; set { if (bklayout != ImageLayout.Centered) { mPicloc = value; InvalidateVisual(); } } }
        public ImageLayout BackgroundImageAnchor { get => bklayout; set { bklayout = value; InvalidateVisual(); } }
        public float BackgroundImageOpacity { get => mPicOpacity; set { mPicOpacity = value; InvalidateVisual(); } }

        // ── Rendering (GDI+ offscreen bridge) ──────────────────────────────
        public override void Render(DrawingContext context)
        {
            int w = (int)Bounds.Width;
            int h = (int)Bounds.Height;
            if (w <= 0 || h <= 0) { base.Render(context); return; }

            using var bmp = new System.Drawing.Bitmap(w, h);
            using var g = System.Drawing.Graphics.FromImage(bmp);

            if (h - HeaderHeight > 0)
            {
                float centre = Math.Max(0.02f, Math.Min(0.98f, GradCentre));
                var rec = new System.Drawing.Rectangle(0, HeaderHeight, w, h - HeaderHeight);
                using var b = new GdiLinearGradientBrush(rec, _backColor, midcol, mGradient);
                var cb = new ColorBlend(3);
                cb.Colors    = new[] { _backColor, midcol, gradcol };
                cb.Positions = new[] { 0f, centre, 1f };
                b.InterpolationColors = cb;
                g.FillRectangle(b, rec);
            }

            // Gradient header bar
            var hrec = new System.Drawing.Rectangle(0, 0, w, HeaderHeight);
            if (HeaderHeight > 0)
            {
                using var bg = new GdiLinearGradientBrush(hrec, headcol, headend, LinearGradientMode.Horizontal);
                g.FillRectangle(bg, hrec);
                if (!string.IsNullOrEmpty(txt))
                {
                    using var fb = new System.Drawing.SolidBrush(headforecol);
                    var sz  = g.MeasureString(txt, headfont);
                    int dist = (int)((HeaderHeight - sz.Height) / 2);
                    g.DrawString(txt, headfont, fb, dist, dist);
                }
            }

            // Write to Avalonia bitmap
            using var ms = new System.IO.MemoryStream();
            bmp.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
            ms.Seek(0, System.IO.SeekOrigin.Begin);
            using var avBmp = new Avalonia.Media.Imaging.Bitmap(ms);
            context.DrawImage(avBmp, new Avalonia.Rect(0, 0, w, h));

            base.Render(context);
        }

        // ── Events ──────────────────────────────────────────────────────────
        public event EventHandler Commited;

        public virtual void OnCommit() { }

        // ── IPackedFileUI ────────────────────────────────────────────────────
        public Avalonia.Controls.Control GUIHandle => this;

        public class WrapperChangedEventArgs : EventArgs
        {
            public WrapperChangedEventArgs(SimPe.Interfaces.Plugin.IFileWrapper o, SimPe.Interfaces.Plugin.IFileWrapper n)
            { OldWrapper = o; NewWrapper = n; }
            public SimPe.Interfaces.Plugin.IFileWrapper OldWrapper { get; }
            public SimPe.Interfaces.Plugin.IFileWrapper NewWrapper { get; }
        }
        public delegate void WrapperChangedHandle(object sender, WrapperChangedEventArgs e);
        public event WrapperChangedHandle WrapperChanged;

        SimPe.Interfaces.Plugin.IFileWrapper wrp;
        public SimPe.Interfaces.Plugin.IFileWrapper Wrapper => wrp;

        private void SetWrapper(SimPe.Interfaces.Plugin.IFileWrapper newWrp)
        {
            var old = wrp;
            wrp = newWrp;
            var e = new WrapperChangedEventArgs(old, newWrp);
            OnWrapperChanged(e);
            WrapperChanged?.Invoke(this, e);
        }

        protected virtual void OnWrapperChanged(WrapperChangedEventArgs e) { }

        public virtual void UpdateGUI(SimPe.Interfaces.Plugin.IFileWrapper wrapper)
        {
            SetWrapper(wrapper);
            RefreshGUI();
        }

        public virtual void RefreshGUI() { }

        // ── IDisposable (required by IPackedFileUI) ──────────────────────
        public void Dispose()
        {
            if (wrp != null) SetWrapper(null);
        }
    }
}
