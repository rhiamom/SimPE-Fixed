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
using Avalonia;
using Avalonia.Controls;

namespace Ambertation.Windows.Forms
{
    /// <summary>
    /// One tab/page inside an XPTaskBoxSelector.
    /// Ported from WinForms — the child Panel is now an Avalonia Panel
    /// added to the parent's Children collection.
    /// </summary>
    public class SelectorItem
    {
        XPTaskBoxSelector parent;
        Avalonia.Controls.Panel pn;
        Rectangle lastrect;

        internal SelectorItem(XPTaskBoxSelector parent, string txt)
        {
            this.parent = parent;

            pn = new Avalonia.Controls.Panel();
            pn.IsVisible = false;
            // Layout of content panels into XPTaskBoxSelector's visual tree
            // is deferred until proper Avalonia tab-container implementation.
            // For now pn tracks visibility state only.

            wd = 0;
            lastrect = new Rectangle(0, 0, 0, 0);
            Text = txt;
        }

        string txt;
        int wd;

        public string Text
        {
            get => txt;
            set
            {
                if (txt != value)
                {
                    txt = value;
                    parent.UpdateSelection(this);

                    // Measure text width using a 1×1 offscreen GDI+ bitmap.
                    using var bmp1 = new System.Drawing.Bitmap(1, 1);
                    using var g = System.Drawing.Graphics.FromImage(bmp1);
                    SizeF sz = g.MeasureString(Text, parent.HeaderFont);
                    wd = (int)Math.Ceiling(sz.Width);
                }
            }
        }

        public int MinWidth => wd;

        public Rectangle BoundingRectangle => lastrect;

        public bool IsVisible
        {
            get => pn.IsVisible;
            set => pn.IsVisible = value;
        }

        void AddBezier(GraphicsPath path, int left, int top, int height)
        {
            path.AddBezier(
                left, top,
                left - Math.Abs(height / 3), top + height / 2,
                left - Math.Abs(height / 3), top + height / 2,
                left, top + height);
        }

        internal virtual Rectangle DrawButton(System.Drawing.Graphics g, Rectangle rect,
                                              bool last, bool hover, bool selected)
        {
            pn.IsVisible = selected;
            lastrect = rect;
            SizeF sz = g.MeasureString(Text, parent.HeaderFont);

            GraphicsPath path = new GraphicsPath();
            AddBezier(path, rect.Left, rect.Top, rect.Height);
            path.AddLine(rect.Left, rect.Bottom, rect.Right, rect.Bottom);
            if (last) path.AddLine(rect.Right, rect.Bottom, rect.Right, rect.Top);
            else      AddBezier(path, rect.Right, rect.Bottom, -rect.Height);
            path.CloseFigure();

            if (selected) g.FillPath(new SolidBrush(Color.Black), path);
            if (hover)    g.FillPath(new SolidBrush(Color.FromArgb(100, Color.YellowGreen)), path);

            Rectangle grec = new Rectangle(rect.Left - rect.Height / 3, rect.Top, rect.Width + rect.Height / 3 + 2, rect.Height);
            using var lgb = new LinearGradientBrush(grec, Color.FromArgb(70, Color.White), Color.Transparent, LinearGradientMode.ForwardDiagonal);
            g.FillPath(lgb, path);

            path = new GraphicsPath();
            AddBezier(path, rect.Left + 2, rect.Top, rect.Height);
            AddBezier(path, rect.Left + 2, rect.Bottom, -rect.Height);
            g.DrawPath(new Pen(Color.FromArgb(20, Color.White)), path);

            path = new GraphicsPath();
            AddBezier(path, rect.Left + 1, rect.Top, rect.Height);
            AddBezier(path, rect.Left + 1, rect.Bottom, -rect.Height);
            g.DrawPath(new Pen(Color.FromArgb(40, Color.White)), path);

            path = new GraphicsPath();
            AddBezier(path, rect.Left, rect.Top, rect.Height);
            AddBezier(path, rect.Left, rect.Bottom, -rect.Height);
            g.DrawPath(new Pen(Color.FromArgb(150, Color.Black), 1), path);

            path = new GraphicsPath();
            AddBezier(path, rect.Left - 1, rect.Top, rect.Height);
            AddBezier(path, rect.Left - 1, rect.Bottom, -rect.Height);
            g.DrawPath(new Pen(Color.FromArgb(40, Color.Black), 1), path);

            g.DrawString(Text, parent.HeaderFont, new SolidBrush(parent.HeaderTextColor),
                         rect.Left + 4, (rect.Height - sz.Height) / 2 + rect.Top);

            return rect;
        }
    }
}
