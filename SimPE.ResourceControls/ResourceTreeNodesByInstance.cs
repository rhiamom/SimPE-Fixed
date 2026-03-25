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
    class ResourceTreeNodesByInstance : AResourceTreeNodeBuilder
    {
        #region IResourceTreeNodeBuilder Member

        public override ResourceTreeNodeExt BuildNodes(ResourceMaps maps)
        {
            ResourceTreeNodeExt tn = new ResourceTreeNodeExt(0, maps.Everything, SimPe.Localization.GetString("AllRes"));

            AddInstances(maps.ByInstance, tn, true, true);

            tn.ImageIndex = 0;
            return tn;
        }      
        #endregion

        public static void AddInstances(ResourceMaps.LongMap map, ResourceTreeNodeExt tn, bool group, bool type)
        {
            List<ResourceTreeNodeExt> nodelist = new List<ResourceTreeNodeExt>();
            foreach (ulong inst in map.Keys)
            {
                ResourceViewManager.ResourceNameList list = map[inst];
                ResourceTreeNodeExt node = new ResourceTreeNodeExt(inst, list, "0x" + Helper.HexString(inst));
                if (group)
                {
                    ResourceTreeNodeExt groupnode = new ResourceTreeNodeExt(inst, list, "Groups");
                    AddSubNodesForGroups(groupnode, list);
                    node.Nodes.Add(groupnode);
                }
                if (type)
                {
                    ResourceTreeNodeExt typenode = new ResourceTreeNodeExt(inst, list, "Types");
                    ResourceTreeNodesByGroup.AddSubNodesForTypes(typenode, list);
                    node.Nodes.Add(typenode);
                }

                nodelist.Add(node);
            }

            nodelist.Sort();
            foreach (ResourceTreeNodeExt node in nodelist)
                tn.Nodes.Add(node);
        }

        static void AddSubNodesForGroups(ResourceTreeNodeExt node, ResourceViewManager.ResourceNameList resources)
        {
            ResourceMaps.IntMap map = new ResourceMaps.IntMap();
            foreach (NamedPackedFileDescriptor pfd in resources)
            {
                ResourceViewManager.ResourceNameList list;
                if (!map.ContainsKey(pfd.Descriptor.Group))
                {
                    list = new ResourceViewManager.ResourceNameList();
                    map.Add(pfd.Descriptor.Group, list);
                }
                else list = map[pfd.Descriptor.Group];

                list.Add(pfd);
            }

            ResourceTreeNodesByGroup.AddGroups(map, node, false, false);
        }        
    }
}
