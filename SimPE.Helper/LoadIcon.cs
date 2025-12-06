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
using System.Collections.Generic;
using System.Drawing;
using System.IO;

namespace SimPe
{
    public static class LoadIcon
    {
        // Cache of filename -> manifest resource name
        static readonly Dictionary<string, string> resourceCache = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

        // All resource names in this assembly
        static readonly string[] resourceNames = typeof(LoadIcon).Assembly.GetManifestResourceNames();

        // Fallback icon file name
        const string defaultIconFileName = "unk.png";

        public static Image load(string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
            {
                fileName = defaultIconFileName;
            }

            Image img = loadInternal(fileName);

            if (img != null)
            {
                return img;
            }

            // Try fallback icon if the requested icon was not found
            if (!fileName.Equals(defaultIconFileName, StringComparison.OrdinalIgnoreCase))
            {
                img = loadInternal(defaultIconFileName);
                if (img != null)
                {
                    return img;
                }
            }

            // Last resort: return a blank 16x16 image so callers never get null
            return new Bitmap(16, 16);
        }

        static Image loadInternal(string fileName)
        {
            string key = fileName.ToLowerInvariant();
            string resourceName;

            if (!resourceCache.TryGetValue(key, out resourceName))
            {
                resourceName = findResourceName(fileName);
                resourceCache[key] = resourceName;
            }

            if (string.IsNullOrEmpty(resourceName))
            {
                return null;
            }

            using (Stream stream = typeof(LoadIcon).Assembly.GetManifestResourceStream(resourceName))
            {
                if (stream == null)
                {
                    return null;
                }

                return Image.FromStream(stream);
            }
        }

        static string findResourceName(string fileName)
        {
            if (resourceNames == null || resourceNames.Length == 0)
            {
                return null;
            }

            string suffix = "." + fileName;

            foreach (string name in resourceNames)
            {
                if (name.EndsWith(suffix, StringComparison.OrdinalIgnoreCase))
                {
                    return name;
                }
            }

            return null;
        }
    }
}
