/***************************************************************************
 *   Copyright (C) 2005 by Ambertation                                     *
 *   quaxi@ambertation.de                                                  *
 *                                                                         *
 *   Copyright (C) 2025 by GramzeSweatshop                                 *
 *   rhiamom@mac.com                                                       *
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

namespace SimPe
{
    /// <summary>
    /// Provides standard SimPE images (placeholder implementation).
    /// This replaces the Chris Hatch GetImage helper with a clean version.
    /// </summary>
    public static class GetImage
    {
        private static Image noOne;
        private static Image network;
        private static Image demo;

        /// <summary>
        /// Generic "something went wrong" image.
        /// For now we just use the system error icon.
        /// </summary>
        public static Image Fail => SystemIcons.Error.ToBitmap();

        /// <summary>
        /// Generic "no sim" placeholder image.
        /// </summary>
        public static Image NoOne
        {
            get
            {
                if (noOne == null) noOne = CreatePlaceholderImage();
                return noOne;
            }
        }

        /// <summary>
        /// Generic "network/lot" placeholder image.
        /// </summary>
        public static Image Network
        {
            get
            {
                if (network == null) network = CreatePlaceholderImage();
                return network;
            }
        }

        /// <summary>
        /// Generic "demo" placeholder image.
        /// </summary>
        public static Image Demo
        {
            get
            {
                if (demo == null) demo = CreatePlaceholderImage();
                return demo;
            }
        }

        /// <summary>
        /// Returns a logo image for an expansion pack.
        /// For now we just return Demo; you can swap in real logos later.
        /// </summary>
        public static Image GetExpansionLogo(int expansionId)
        {
            // TODO (later, when you care about nice visuals): map IDs to real logos.
            return Demo;
        }

        /// <summary>
        /// Returns a small icon-style image for an expansion pack.
        /// For now we also just return Demo.
        /// </summary>
        public static Image GetExpansionIcon(int expansionId)
        {
            // Same placeholder as logo for now.
            return Demo;
        }

        /// <summary>
        /// Original Chris method signature used in a few places:
        /// Load expansion images into a PictureBox based on version.
        /// </summary>
        public static void Loadimges(PictureBox pictureBox, int version)
        {
            if (pictureBox == null) return;
            pictureBox.Image = GetExpansionLogo(version);
        }

        private static Image CreatePlaceholderImage()
        {
            const int size = 64;

            Bitmap bmp = new Bitmap(size, size);
            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.Clear(Color.DarkGray);
                using (Pen pen = new Pen(Color.Black))
                {
                    g.DrawRectangle(pen, 0, 0, bmp.Width - 1, bmp.Height - 1);
                }

                using (Font font = new Font(FontFamily.GenericSansSerif, 10f))
                using (Brush brush = new SolidBrush(Color.White))
                using (StringFormat fmt = new StringFormat
                {
                    Alignment = StringAlignment.Center,
                    LineAlignment = StringAlignment.Center
                })
                {
                    g.DrawString("SimPE", font, brush,
                        new RectangleF(0, 0, bmp.Width, bmp.Height), fmt);
                }
            }

            return bmp;
        }
    }
}

    /// <summary>
    /// Provides icon-sized images (also placeholders for now).
    /// </summary>
    
