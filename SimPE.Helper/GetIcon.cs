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

using System.Drawing;

namespace SimPe
{
    /// <summary>
    /// Provides small toolbar / browser icons.
    /// This is a clean replacement for Chris Hatch’s GetIcon helper.
    /// All images are neutral placeholders that you can replace later.
    /// </summary>
    public static class GetIcon
    {
        private static Image simBrowser;
        private static Image folder;
        private static Image package;
        private static Image texture;
        private static Image sim;
        private static Image material;

        public static Image Fail => GetImage.Fail;

        /// <summary>
        /// Icon used by Sim Browser buttons.
        /// </summary>
        public static Image SimBrowser
        {
            get
            {
                if (simBrowser == null)
                    simBrowser = CreateIconPlaceholder("Sim");

                return simBrowser;
            }
        }

        /// <summary>
        /// Generic folder icon.
        /// </summary>
        public static Image Folder
        {
            get
            {
                if (folder == null)
                    folder = CreateIconPlaceholder("Fldr");

                return folder;
            }
        }

        /// <summary>
        /// Generic package icon.
        /// </summary>
        public static Image Package
        {
            get
            {
                if (package == null)
                    package = CreateIconPlaceholder("Pkg");

                return package;
            }
        }

        /// <summary>
        /// Generic texture icon.
        /// </summary>
        public static Image Texture
        {
            get
            {
                if (texture == null)
                    texture = CreateIconPlaceholder("Txtr");

                return texture;
            }
        }

        /// <summary>
        /// Generic sim icon.
        /// </summary>
        public static Image Sim
        {
            get
            {
                if (sim == null)
                    sim = CreateIconPlaceholder("Sim");

                return sim;
            }
        }

        /// <summary>
        /// Generic material (TXMT) icon.
        /// </summary>
        public static Image Material
        {
            get
            {
                if (material == null)
                    material = CreateIconPlaceholder("Mat");

                return material;
            }
        }

        /// <summary>
        /// Creates a simple 16x16 placeholder icon with text.
        /// </summary>
        private static Image CreateIconPlaceholder(string label)
        {
            Bitmap bmp = new Bitmap(16, 16);

            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.Clear(Color.Gray);
                using (Pen p = new Pen(Color.Black))
                    g.DrawRectangle(p, 0, 0, 15, 15);

                using (Font f = new Font(FontFamily.GenericSansSerif, 6f))
                using (Brush b = new SolidBrush(Color.White))
                    g.DrawString(label, f, b, 1, 3);
            }

            return bmp;
        }
    }
}
