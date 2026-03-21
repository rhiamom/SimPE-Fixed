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

using Avalonia.Media.Imaging;

namespace SimPe
{
    /// <summary>
    /// Provides standard SimPE images.
    /// GDI+ rendering is not available cross-platform; placeholder images return null.
    /// Real images can be loaded later from embedded resources via Avalonia asset loading.
    /// </summary>
    public static class GetImage
    {
        /// <summary>Generic "something went wrong" image (null placeholder).</summary>
        public static Bitmap? Fail => null;

        /// <summary>Generic "no sim" placeholder image (null placeholder).</summary>
        public static Bitmap? NoOne => null;

        /// <summary>Generic "network/lot" placeholder image (null placeholder).</summary>
        public static Bitmap? Network => null;

        /// <summary>Generic "demo" placeholder image (null placeholder).</summary>
        public static Bitmap? Demo => null;

        /// <summary>Returns a logo image for an expansion pack (null placeholder).</summary>
        public static Bitmap? GetExpansionLogo(int expansionId) => null;

        /// <summary>Returns an icon-sized image for an expansion pack (null placeholder).</summary>
        public static Bitmap? GetExpansionIcon(int expansionId) => null;
    }
}
