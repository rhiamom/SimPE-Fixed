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
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using GdiSize                = System.Drawing.Size;
using GdiPoint               = System.Drawing.Point;
using AvSize                 = Avalonia.Size;
using GdiPen                 = System.Drawing.Pen;
using GdiLinearGradientBrush = System.Drawing.Drawing2D.LinearGradientBrush;

namespace Ambertation.Windows.Forms
{
    /// <summary>
    /// A styled panel with a gradient header bar and body area.
    /// Ported from the WinForms version; GDI+ rendering is kept for the
    /// offscreen cache bitmap which is then composited via Avalonia.
    /// </summary>
    [DesignTimeVisible(true)]
    public class XPTaskBoxSimple : Avalonia.Controls.Control
    {
        // Methods
        public XPTaskBoxSimple()
        {
            headerh         = 22;
            mstrHeaderText  = "";
            bc              = System.Drawing.SystemColors.Window;
            lhc             = System.Drawing.SystemColors.InactiveCaption;
            rhc             = System.Drawing.SystemColors.Highlight;
            bodc            = System.Drawing.SystemColors.InactiveCaptionText;
            htc             = System.Drawing.SystemColors.ActiveCaptionText;
            font            = new Font("Arial", 9f, System.Drawing.FontStyle.Bold, GraphicsUnit.Point);
            icsz            = new GdiSize(32, 32);
            icpt            = new GdiPoint(4, 12);
        }

        System.Drawing.Color lhc, rhc, bc, bodc, htc;
        public System.Drawing.Color LeftHeaderColor
        {
            get => lhc;
            set { if (lhc != value) { lhc = value; InvalidateCanvas(); } }
        }

        public System.Drawing.Color RightHeaderColor
        {
            get => rhc;
            set { if (rhc != value) { rhc = value; InvalidateCanvas(); } }
        }

        public System.Drawing.Color BorderColor
        {
            get => bc;
            set { if (bc != value) { bc = value; InvalidateCanvas(); } }
        }

        public System.Drawing.Color HeaderTextColor
        {
            get => htc;
            set { if (htc != value) { htc = value; InvalidateCanvas(); } }
        }

        public System.Drawing.Color BodyColor
        {
            get => bodc;
            set { if (bodc != value) { bodc = value; InvalidateCanvas(); } }
        }

        Font font;
        public Font HeaderFont
        {
            get => font;
            set { if (font != value) { font = value; InvalidateCanvas(); } }
        }

        System.Drawing.Bitmap canvas;
        int canvasW, canvasH;

        void InvalidateCanvas() { canvas = null; InvalidateVisual(); }

        protected void RebuildCanvas(int w, int h)
        {
            if (canvas != null) canvas.Dispose();
            canvas = null;
            canvasW = w;
            canvasH = h;

            if (w <= 7 || h <= headerh + 21) return;

            canvas = new System.Drawing.Bitmap(w, h);
            System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(canvas);
            Rectangle ef3  = new Rectangle(0, 16, w - 1, headerh);
            Rectangle ef3b = new Rectangle(3, ef3.Bottom, w - 7, h - ef3.Bottom - 4);
            Rectangle ef1  = new Rectangle(0, 16, w - 1, h - 0x11);
            GraphicsPath path     = new GraphicsPath();
            GdiLinearGradientBrush brush1  = new GdiLinearGradientBrush(ef3, LeftHeaderColor, RightHeaderColor, LinearGradientMode.Horizontal);
            GdiPen borderpen = new GdiPen(BorderColor, 1f);
            StringFormat format1  = new StringFormat
            {
                Alignment     = StringAlignment.Near,
                LineAlignment = StringAlignment.Center,
                Trimming      = StringTrimming.EllipsisCharacter,
                FormatFlags   = StringFormatFlags.NoWrap,
            };
            borderpen.Alignment = PenAlignment.Inset;
            g.SmoothingMode = SmoothingMode.HighQuality;

            path = Ambertation.Drawing.GraphicRoutines.GethRoundRectPath(ef1, 7);
            g.FillPath(brush1, path);

            path = Ambertation.Drawing.GraphicRoutines.GethRoundRectPath(ef3b, 7);
            g.FillPath(new SolidBrush(BodyColor), path);

            path = Ambertation.Drawing.GraphicRoutines.GethRoundRectPath(ef1, 7);
            g.DrawPath(borderpen, path);

            Rectangle ef4;
            if (mIcon != null)
            {
                GdiSize isize = mIcon.Size;
                g.DrawImage(mIcon, new Rectangle(IconLocation, isize),
                            new Rectangle(0, 0, mIcon.Width, mIcon.Height), GraphicsUnit.Pixel);
                ef4 = new Rectangle(8 + isize.Width + IconLocation.X, 16, w - (isize.Width + IconLocation.X), headerh);
            }
            else
            {
                ef4 = new Rectangle(8, 16, w - 0x18, headerh);
            }
            g.DrawString(mstrHeaderText, HeaderFont, new SolidBrush(HeaderTextColor), ef4, format1);

            path.Dispose();
            brush1.Dispose();
            borderpen.Dispose();
            format1.Dispose();
            g.Dispose();
        }

        public override void Render(DrawingContext context)
        {
            int w = (int)Bounds.Width;
            int h = (int)Bounds.Height;
            if (canvas == null || canvasW != w || canvasH != h)
                RebuildCanvas(w, h);
            if (canvas == null) { base.Render(context); return; }

            using var ms = new MemoryStream();
            canvas.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
            ms.Position = 0;
            using var avBmp = new Avalonia.Media.Imaging.Bitmap(ms);
            context.DrawImage(avBmp, new Rect(0, 0, w, h));
        }

        protected override AvSize MeasureOverride(AvSize availableSize) => availableSize;

        protected override AvSize ArrangeOverride(AvSize finalSize)
        {
            canvas = null; // size changed — rebuild on next render
            return base.ArrangeOverride(finalSize);
        }

        // Properties
        [Category("Appearance"), DefaultValue("Title"), Localizable(true), Description("Caption text.")]
        public string HeaderText
        {
            get => mstrHeaderText;
            set { mstrHeaderText = value; InvalidateCanvas(); }
        }

        [Localizable(true), Description("Icon"), Category("Appearance")]
        public System.Drawing.Image Icon
        {
            get => mIcon;
            set { mIcon = value; InvalidateCanvas(); }
        }

        int headerh;
        [Localizable(true), Description("Height of the Headline"), Category("Appearance"), DefaultValue(22)]
        public int HeaderHeight
        {
            get => headerh;
            set { headerh = value; InvalidateCanvas(); }
        }

        GdiSize icsz;
        public GdiSize IconSize
        {
            get => icsz;
            set { if (icsz != value) { icsz = value; InvalidateCanvas(); } }
        }

        GdiPoint icpt;
        public GdiPoint IconLocation
        {
            get => icpt;
            set { if (icpt != value) { icpt = value; InvalidateCanvas(); } }
        }

        [Browsable(false)]
        internal Rectangle WorkspaceRect => new Rectangle(3, 0x29, (int)Bounds.Width - 7, (int)Bounds.Height - 40 - 4);

        private System.Drawing.Image mIcon;
        private string mstrHeaderText;
    }
}
