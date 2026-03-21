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
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Threading;
using Ambertation.Collections;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Avalonia.Threading;

namespace Ambertation.Windows.Forms
{
    /// <summary>
    /// A control that cycles through an Images collection on a timer.
    /// Ported from WinForms UserControl + System.Windows.Forms.Timer.
    /// Uses Avalonia UserControl + DispatcherTimer for the animation loop
    /// and GDI+ offscreen rendering to produce the frame.
    /// </summary>
    public class AnimatedImagelist : UserControl
    {
        readonly DispatcherTimer timer;

        public AnimatedImagelist()
        {
            index = 0;
            list  = new Images();

            timer = new DispatcherTimer();
            timer.Tick += timer_Tick;
        }

        #region public Properties
        public DispatcherTimer Timer => timer;

        bool doevents;
        public bool DoEvents
        {
            get => doevents;
            set => doevents = value;
        }

        int index;
        public int CurrentIndex
        {
            get => index;
            set
            {
                if (index != value)
                {
                    index = Math.Min(list.Count - 1, Math.Max(0, value));
                    InvalidateVisual();
                }
            }
        }

        Images list;
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public Images Images
        {
            get => list;
            set => list = value;
        }
        #endregion

        #region Rendering
        public override void Render(DrawingContext context)
        {
            base.Render(context);

            int w = (int)Bounds.Width;
            int h = (int)Bounds.Height;
            if (w <= 0 || h <= 0 || index < 0 || index >= list.Count) return;

            System.Drawing.Image src = list[index];
            if (src == null) return;

            // Scale GDI+ image to control size, then convert to Avalonia Bitmap.
            using var scaled = Ambertation.Drawing.GraphicRoutines.ScaleImage(src, w, h, true);
            using var bmp = new System.Drawing.Bitmap(w, h);
            using var g = System.Drawing.Graphics.FromImage(bmp);
            g.DrawImage(scaled, 0, 0, w, h);

            using var ms = new MemoryStream();
            bmp.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
            ms.Position = 0;
            using var avBmp = new Avalonia.Media.Imaging.Bitmap(ms);
            context.DrawImage(avBmp, new Rect(0, 0, w, h));
        }
        #endregion

        [Browsable(false)]
        public bool Running => timer.IsEnabled;

        public void Start()  { timer.IsEnabled = true; }
        public void Pause()  { timer.IsEnabled = false; }
        public void Stop()   { Pause(); index = 0; }

        private void timer_Tick(object sender, EventArgs e)
        {
            lock (timer)
            {
                if (index < 0) index = 0;
                else if (index >= list.Count - 1) index = 0;
                else index++;
                InvalidateVisual();
            }
        }
    }
}
