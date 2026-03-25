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
    public class ResourceTreeNodeExt : System.Windows.Forms.TreeNode, IComparable<ResourceTreeNodeExt>
    {
        ResourceViewManager.ResourceNameList list;
        ulong id;
        public ResourceTreeNodeExt(ulong id, ResourceViewManager.ResourceNameList list, string text)
            : base()
        {
            this.id = id;
            this.list = list;

            this.ImageIndex = 0;
            this.Text = text + " ("+list.Count+")";
            this.SelectedImageIndex = this.ImageIndex;
        }

        public ResourceViewManager.ResourceNameList Resources
        {
            get { return list; }
        }

        public virtual ulong ID
        {
            get { return id; }
        }


        #region IComparable<ResResourceTreeNodeExt> Member

        public int CompareTo(ResourceTreeNodeExt other)
        {
            return this.Text.CompareTo(other.Text);
        }

        #endregion
    }
}
