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
using Avalonia.Input;
using Avalonia.Media;
using GdiPoint              = System.Drawing.Point;
using AvSize                = Avalonia.Size;
using GdiPen                = System.Drawing.Pen;
using GdiLinearGradientBrush = System.Drawing.Drawing2D.LinearGradientBrush;

namespace Ambertation.Windows.Forms
{
    /// <summary>
    /// A tab-selector panel with bezier-curved tab buttons along the bottom.
    /// Ported from WinForms; GDI+ rendering is kept for the offscreen cache,
    /// composited into Avalonia's render pipeline.
    /// </summary>
    [DesignTimeVisible(true)]
    public class XPTaskBoxSelector : Avalonia.Controls.Control
    {
        // Methods
        public XPTaskBoxSelector()
        {
            bc   = System.Drawing.SystemColors.Window;
            lhc  = System.Drawing.SystemColors.InactiveCaption;
            rhc  = System.Drawing.SystemColors.Highlight;
            bodc = System.Drawing.SystemColors.InactiveCaptionText;
            htc  = System.Drawing.SystemColors.ActiveCaptionText;
            font = new Font("Arial", 9f, System.Drawing.FontStyle.Bold, GraphicsUnit.Point);

            pages = new ArrayList();
            pages.Add(new SelectorItem(this, "Hello"));
            pages.Add(new SelectorItem(this, "Test"));
            pages.Add(new SelectorItem(this, "Frank"));
            pages.Add(new SelectorItem(this, "Bauer"));
        }

        ArrayList pages;

        #region Public Properties
        int selid;
        public int SelectedIndex
        {
            get => selid;
            set
            {
                if (value != selid && value >= 0 && value < pages.Count)
                {
                    mousesel = ((SelectorItem)pages[value]).BoundingRectangle.Location;
                    DrawComplete();
                    InvalidateVisual();
                }
            }
        }

        System.Drawing.Color lhc, rhc, bc, bodc, htc;
        public System.Drawing.Color LeftHeaderColor
        {
            get => lhc;
            set { if (lhc != value) { lhc = value; DrawComplete(); InvalidateVisual(); } }
        }

        public System.Drawing.Color RightHeaderColor
        {
            get => rhc;
            set { if (rhc != value) { rhc = value; DrawComplete(); InvalidateVisual(); } }
        }

        public System.Drawing.Color BorderColor
        {
            get => bc;
            set { if (bc != value) { bc = value; DrawComplete(); InvalidateVisual(); } }
        }

        public System.Drawing.Color HeaderTextColor
        {
            get => htc;
            set { if (htc != value) { htc = value; DrawComplete(); InvalidateVisual(); } }
        }

        public System.Drawing.Color BodyColor
        {
            get => bodc;
            set { if (bodc != value) { bodc = value; DrawComplete(); InvalidateVisual(); } }
        }

        Font font;
        public Font HeaderFont
        {
            get => font;
            set { if (font != value) { font = value; DrawComplete(); InvalidateVisual(); } }
        }
        #endregion

        protected Rectangle SelectionRect
        {
            get
            {
                int h = (int)Bounds.Height;
                int w = (int)Bounds.Width;
                return new Rectangle(0, h - 25, w - 1, 22);
            }
        }

        protected void DrawSelection(System.Drawing.Graphics g, Rectangle ef3)
        {
            int minleft = ef3.Right;
            for (int i = 0; i < pages.Count; i++)
            {
                minleft -= ((SelectorItem)pages[i]).MinWidth;
                minleft -= ef3.Height;
            }

            selid = -1;
            for (int i = 0; i < pages.Count; i++)
            {
                SelectorItem item = (SelectorItem)pages[i];
                Rectangle rec = new Rectangle(minleft, ef3.Top, item.MinWidth + ef3.Height, ef3.Height);
                item.DrawButton(g, rec, i == pages.Count - 1, rec.Contains(mouseloc), rec.Contains(mousesel));
                if (rec.Contains(mousesel)) selid = i;
                minleft += rec.Width;
            }
        }

        internal void UpdateSelection(SelectorItem sender)
        {
            DrawComplete();
            InvalidateVisual();
        }

        System.Drawing.Bitmap cachedimg;
        int cachedW, cachedH;

        protected virtual void DrawComplete()
        {
            int w = (int)Bounds.Width;
            int h = (int)Bounds.Height;
            if (w <= 0 || h <= 0) return;

            if (cachedimg != null) cachedimg.Dispose();
            cachedimg = new System.Drawing.Bitmap(w, h);
            cachedW = w;
            cachedH = h;

            System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(cachedimg);
            Ambertation.Windows.Forms.Graph.GraphPanelElement.SetGraphicsMode(g, false);

            Rectangle ef3  = SelectionRect;
            Rectangle ef3b = new Rectangle(3, 3, w - 7, h - ef3.Height - 6);
            Rectangle ef1  = new Rectangle(0, 0, w - 1, h - 1);

            GraphicsPath path = new GraphicsPath();
            using var brush1 = new GdiLinearGradientBrush(ef3.IsEmpty ? new Rectangle(0,0,1,1) : ef3, LeftHeaderColor, RightHeaderColor, LinearGradientMode.Horizontal);
            using var borderpen = new GdiPen(BorderColor, 1f);
            using var format1 = new StringFormat
            {
                Alignment     = StringAlignment.Near,
                LineAlignment = StringAlignment.Center,
                Trimming      = StringTrimming.EllipsisCharacter,
                FormatFlags   = StringFormatFlags.NoWrap,
            };
            borderpen.Alignment = PenAlignment.Inset;

            path = Ambertation.Drawing.GraphicRoutines.GethRoundRectPath(ef1, 7);
            g.FillPath(brush1, path);

            DrawSelection(g, ef3);

            path = Ambertation.Drawing.GraphicRoutines.GethRoundRectPath(ef3b, 7);
            g.FillPath(new SolidBrush(BodyColor), path);

            path = Ambertation.Drawing.GraphicRoutines.GethRoundRectPath(ef1, 7);
            g.DrawPath(borderpen, path);

            path.Dispose();
            Ambertation.Windows.Forms.Graph.GraphPanelElement.SetGraphicsMode(g, true);
            g.Dispose();
        }

        public override void Render(DrawingContext context)
        {
            int w = (int)Bounds.Width;
            int h = (int)Bounds.Height;
            if (cachedimg == null || cachedW != w || cachedH != h)
                DrawComplete();

            if (cachedimg != null)
            {
                using var ms = new MemoryStream();
                cachedimg.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                ms.Position = 0;
                using var avBmp = new Avalonia.Media.Imaging.Bitmap(ms);
                context.DrawImage(avBmp, new Rect(0, 0, w, h));
            }

            base.Render(context);
        }

        protected override AvSize ArrangeOverride(AvSize finalSize)
        {
            cachedimg = null; // size changed — rebuild on next render
            return base.ArrangeOverride(finalSize);
        }

        [Browsable(false)]
        internal Rectangle WorkspaceRect
            => new Rectangle(3, 3, (int)Bounds.Width - 7, (int)Bounds.Height - 22 - 4);

        #region Pointer event overrides
        GdiPoint mouseloc;
        GdiPoint mousesel;
        bool hadhover;

        protected override void OnPointerMoved(PointerEventArgs e)
        {
            base.OnPointerMoved(e);
            var pos = e.GetPosition(this);
            GdiPoint pt = new GdiPoint((int)pos.X, (int)pos.Y);

            for (int i = 0; i < pages.Count; i++)
            {
                SelectorItem item = (SelectorItem)pages[i];
                if (item.BoundingRectangle.Contains(pt))
                {
                    hadhover = true;
                    mouseloc = pt;
                    DrawComplete();
                    InvalidateVisual();
                    return;
                }
            }
            mouseloc = pt;
            if (hadhover) { DrawComplete(); InvalidateVisual(); }
            hadhover = false;
        }

        protected override void OnPointerPressed(PointerPressedEventArgs e)
        {
            base.OnPointerPressed(e);
            if (!e.GetCurrentPoint(this).Properties.IsLeftButtonPressed) return;

            var pos = e.GetPosition(this);
            GdiPoint pt = new GdiPoint((int)pos.X, (int)pos.Y);
            for (int i = 0; i < pages.Count; i++)
            {
                SelectorItem item = (SelectorItem)pages[i];
                if (item.BoundingRectangle.Contains(pt))
                {
                    mousesel = pt;
                    DrawComplete();
                    InvalidateVisual();
                    return;
                }
            }
        }

        protected override void OnPointerExited(PointerEventArgs e)
        {
            base.OnPointerExited(e);
            mouseloc = new GdiPoint(0, 0);
            DrawComplete();
            InvalidateVisual();
        }
        #endregion
    }
}
