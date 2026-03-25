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
    public class ResourceTreeTypeNodeExt : ResourceTreeNodeExt
    {
        uint type;
        public ResourceTreeTypeNodeExt(ResourceViewManager.ResourceNameList list, uint type)
            : base(type, list, "")
        {
            this.type = type;
            this.ImageIndex = ResourceViewManager.GetIndexForResourceType(type);
            this.SelectedImageIndex = this.ImageIndex;
            SimPe.Data.TypeAlias ta = Data.MetaData.FindTypeAlias(type);
            this.Text = ta.Name + " ("+ta.shortname+") (" + list.Count + ")";
        }       

        public uint Type
        {
            get { return type; }
        }

       


        #region IComparable<ResResourceTreeNodeExt> Member

        public new int CompareTo(ResourceTreeNodeExt other)
        {
            return this.Text.CompareTo(other.Text);
        }

        #endregion
    }
}
