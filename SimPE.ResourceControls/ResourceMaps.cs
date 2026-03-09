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

using System;
using System.Collections.Generic;
using System.Text;

namespace SimPe.Windows.Forms
{
    public class ResourceMaps
    {
        public class IntMap : Dictionary<uint, ResourceViewManager.ResourceNameList> { }
        public class LongMap : Dictionary<ulong, ResourceViewManager.ResourceNameList> { }

        public ResourceMaps()
        {
            all = new ResourceViewManager.ResourceNameList();
            typemap = new IntMap();
            groupmap = new IntMap();
            instmap = new LongMap();
        }

        IntMap typemap, groupmap;
        private LongMap instmap;
        private ResourceViewManager.ResourceNameList all;

        public ResourceViewManager.ResourceNameList Everything
        {
            get { return all; }
        }

        internal IntMap ByGroup
        {
            get { return groupmap; }
        }
        internal IntMap ByType
        {
            get { return typemap; }
        }

        public LongMap ByInstance
        {
            get { return instmap; }
        }

        public void Clear()
        {
            Clear(true);
        }

        public void Clear(bool call)
        {
            typemap.Clear();
            groupmap.Clear();
            instmap.Clear();
            if (call) all.Clear();
        }
    }
}
