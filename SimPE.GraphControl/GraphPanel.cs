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
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.IO;
using Ambertation.Windows.Forms;
using Ambertation.Windows.Forms.Graph;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Media;
using Avalonia.Media.Imaging;

namespace Ambertation.Windows.Forms
{
    /// <summary>
    /// A custom-rendered panel that hosts GraphPanelElement items with
    /// drag-and-drop support.  All drawing is done offscreen via GDI+
    /// and composited into Avalonia's render pipeline.
    /// </summary>
    public class GraphPanel : Avalonia.Controls.Control
    {
        Ambertation.Collections.GraphElements li;
        internal Ambertation.Collections.GraphElements LinkItems => li;
        public  Ambertation.Collections.GraphElements Items      => li;

        public GraphPanel()
        {
            li = new Ambertation.Collections.GraphElements();
            li.ItemsChanged += li_ItemsChanged;
            lm = Ambertation.Windows.Forms.Graph.LinkControlLineMode.Bezier;
            quality   = true;
            savebound = true;
            minwd     = 0;
            minhg     = 0;
            lk        = false;
            update    = false;
            autosz    = false;
        }

        #region Properties
        bool lk;
        public bool LockItems
        {
            get => lk;
            set
            {
                if (lk != value) { lk = value; SetLocked(); }
            }
        }

        bool savebound;
        public virtual bool SaveBounds
        {
            get => savebound;
            set => savebound = value;
        }

        bool autosz;
        public bool AutoSizeToContent
        {
            get => autosz;
            set
            {
                autosz = value;
                li_ItemsChanged(li, null);
            }
        }

        Ambertation.Windows.Forms.Graph.LinkControlLineMode lm;
        public Ambertation.Windows.Forms.Graph.LinkControlLineMode LineMode
        {
            get => lm;
            set { lm = value; SetLinkLineMode(); }
        }

        bool quality;
        public bool Quality
        {
            get => quality;
            set { quality = value; SetLinkQuality(); }
        }

        int minwd, minhg;
        [Browsable(false)]
        public new int MinWidth
        {
            get => minwd;
            set { minwd = value; Width = Math.Max(Width, minwd); }
        }

        [Browsable(false)]
        public new int MinHeight
        {
            get => minhg;
            set { minhg = value; Height = Math.Max(Height, minhg); }
        }

        [Browsable(false)]
        public GraphPanelElement SelectedElement
        {
            get
            {
                foreach (GraphPanelElement gpe in li)
                    if (gpe is DragPanel dp && dp.Focused)
                        return gpe;
                return null;
            }
            set
            {
                if (value == null || !(value is DragPanel)) return;
                if (li.Contains(value))
                {
                    GraphPanelElement[] elements = new GraphPanelElement[li.Count];
                    li.CopyTo(elements);
                    foreach (GraphPanelElement gpe in elements)
                        if (gpe is DragPanel dp)
                            dp.SetFocus(gpe == value);
                }
            }
        }
        #endregion

        #region Avalonia rendering
        public override void Render(DrawingContext context)
        {
            if (update) return;
            base.Render(context);

            int w = (int)Bounds.Width;
            int h = (int)Bounds.Height;
            if (w <= 0 || h <= 0) return;

            // Draw all graph elements into an offscreen GDI+ bitmap.
            using var gdiBmp = new System.Drawing.Bitmap(w, h, System.Drawing.Imaging.PixelFormat.Format32bppPArgb);
            using var g = System.Drawing.Graphics.FromImage(gdiBmp);
            g.FillRectangle(new System.Drawing.SolidBrush(System.Drawing.SystemColors.ControlLightLight), 0, 0, w, h);
            GraphPanelElement.SetGraphicsMode(g, true);
            var clipRect = new System.Drawing.Rectangle(0, 0, w, h);
            foreach (GraphPanelElement c in li) c.OnPaint(g, clipRect);

            // Convert to Avalonia Bitmap and draw.
            using var ms = new MemoryStream();
            gdiBmp.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
            ms.Position = 0;
            using var avBmp = new Avalonia.Media.Imaging.Bitmap(ms);
            context.DrawImage(avBmp, new Rect(0, 0, w, h));
        }
        #endregion

        #region Pointer event overrides
        protected override void OnPointerPressed(PointerPressedEventArgs e)
        {
            base.OnPointerPressed(e);
            var pos  = e.GetPosition(this);
            var pt   = e.GetCurrentPoint(this);
            bool isLeft = pt.Properties.IsLeftButtonPressed;
            var me = new MouseEventArgs(
                isLeft ? MouseButtons.Left : MouseButtons.Right,
                e.ClickCount, (int)pos.X, (int)pos.Y, 0);

            bool hit = false;
            GraphPanelElement[] elements = new GraphPanelElement[li.Count];
            li.CopyTo(elements);
            for (int i = elements.Length - 1; i >= 0; i--)
            {
                GraphPanelElement c = elements[i];
                if (c is DragPanel dp)
                {
                    if (!hit && dp.OnMouseDown(me))
                    {
                        if (isLeft) { hit = true; dp.SetFocus(true); continue; }
                    }
                    if (isLeft) dp.SetFocus(false);
                }
            }
        }

        protected override void OnPointerMoved(PointerEventArgs e)
        {
            base.OnPointerMoved(e);
            var pos = e.GetPosition(this);
            var pt  = e.GetCurrentPoint(this);
            bool isLeft = pt.Properties.IsLeftButtonPressed;
            var me = new MouseEventArgs(
                isLeft ? MouseButtons.Left : MouseButtons.None,
                0, (int)pos.X, (int)pos.Y, 0);

            for (int i = li.Count - 1; i >= 0; i--)
            {
                GraphPanelElement c = li[i];
                if (c is DragPanel dp && dp.OnMouseMove(me)) break;
            }
        }

        protected override void OnPointerReleased(PointerReleasedEventArgs e)
        {
            base.OnPointerReleased(e);
            var pos = e.GetPosition(this);
            var me = new MouseEventArgs(
                e.InitialPressMouseButton == MouseButton.Left ? MouseButtons.Left : MouseButtons.Right,
                0, (int)pos.X, (int)pos.Y, 0);

            for (int i = li.Count - 1; i >= 0; i--)
            {
                GraphPanelElement c = li[i];
                if (c is DragPanel dp && dp.OnMouseUp(me)) break;
            }
        }
        #endregion

        void SetLinkLineMode()  { foreach (GraphPanelElement gpe in li) gpe.ChangedParent(); }
        void SetLinkQuality()   { foreach (GraphPanelElement gpe in li) gpe.ChangedParent(); }
        void SetLocked()
        {
            foreach (GraphPanelElement gpe in li)
                if (gpe is DragPanel dp) dp.Movable = !lk;
        }

        private void li_ItemsChanged(object sender, EventArgs e)
        {
            if (!autosz) return;
            int mx = 0, my = 0;
            foreach (GraphPanelElement gpe in li)
            {
                mx = Math.Max(mx, gpe.Right);
                my = Math.Max(my, gpe.Bottom);
            }
            Width  = Math.Max(mx, minwd);
            Height = Math.Max(my, minhg);
        }

        bool update;
        public void BeginUpdate() { update = true; }
        public void EndUpdate()   { update = false; InvalidateVisual(); }

        public void Clear()
        {
            while (li.Count > 0)
            {
                GraphPanelElement l = li[0];
                li.RemoveAt(0);
                l.Clear();
                l.Parent = null;
            }
            InvalidateVisual();
        }

        /// <summary>
        /// Calculate the radius of a circle you can use to place items on.
        /// </summary>
        public static double GetPinCircleRadius(System.Drawing.Size centersize, System.Drawing.Size itemsize, int itemcount)
        {
            double alpha = Math.Max(0.01, Math.Min(Math.PI/2, 2*Math.PI / itemcount));
            double l     = Math.Max(itemsize.Width, itemsize.Height) * Math.Sqrt(2);
            double minl  = Math.Max(centersize.Width, centersize.Height) * Math.Sqrt(0.5) + l/2;
            return Math.Max(l/(2*Math.Sin(alpha)), minl);
        }

        public static System.Drawing.Point GetItemLocationOnPinCricle(System.Drawing.Point center, double r, int nr, int itemcount, System.Drawing.Size itemsize)
        {
            double alpha = (2*Math.PI / itemcount) * nr;
            return new System.Drawing.Point(
                center.X + (int)(Math.Cos(alpha)*r) - itemsize.Width/2,
                center.Y + (int)(Math.Sin(alpha)*r) - itemsize.Height/2);
        }

        public static System.Drawing.Point GetCenterLocationOnPinCircle(System.Drawing.Point center, double r, System.Drawing.Size itemsize)
        {
            return new System.Drawing.Point(center.X - itemsize.Width/2, center.Y - itemsize.Height/2);
        }
    }
}
