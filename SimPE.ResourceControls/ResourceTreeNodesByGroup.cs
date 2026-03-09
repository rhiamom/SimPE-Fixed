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
using System.Windows.Forms;

namespace SimPe.Windows.Forms
{
    class ResourceTreeNodesByGroup : AResourceTreeNodeBuilder
    {
        #region IResourceTreeNodeBuilder Member

        public override ResourceTreeNodeExt BuildNodes(ResourceMaps maps)
        {
            ResourceTreeNodeExt tn = new ResourceTreeNodeExt(0, maps.Everything, SimPe.Localization.GetString("AllRes"));

            AddGroups(maps.ByGroup, tn, true, true);

            tn.ImageIndex = 0;
            return tn;
        }      
        #endregion

        public static void AddGroups(ResourceMaps.IntMap map, ResourceTreeNodeExt tn, bool type, bool inst)
        {
            List<ResourceTreeNodeExt> nodelist = new List<ResourceTreeNodeExt>();
            foreach (uint group in map.Keys)
            {
                ResourceViewManager.ResourceNameList list = map[group];
                ResourceTreeNodeExt node = new ResourceTreeNodeExt(group, list, "0x" + Helper.HexString(group));
                if (type)
                {
                    ResourceTreeNodeExt typenode = new ResourceTreeNodeExt(group, list, "Types");
                    AddSubNodesForTypes(typenode, list);
                    node.Nodes.Add(typenode);
                }

                if (inst)
                {
                    ResourceTreeNodeExt instnode = new ResourceTreeNodeExt(group, list, "Instances");
                    AddSubNodesForInstances(instnode, list);
                    node.Nodes.Add(instnode);
                }

                nodelist.Add(node);
            }

            nodelist.Sort();
            foreach (ResourceTreeNodeExt node in nodelist)
                tn.Nodes.Add(node);
        }

        public static void AddSubNodesForTypes(ResourceTreeNodeExt node, ResourceViewManager.ResourceNameList resources)
        {
            ResourceMaps.IntMap map = new ResourceMaps.IntMap();
            foreach (NamedPackedFileDescriptor pfd in resources)
            {
                ResourceViewManager.ResourceNameList list;
                if (!map.ContainsKey(pfd.Descriptor.Type))
                {
                    list = new ResourceViewManager.ResourceNameList();
                    map.Add(pfd.Descriptor.Type, list);
                }
                else list = map[pfd.Descriptor.Type];

                list.Add(pfd);
            }

            ResourceTreeNodesByType.AddType(map, node);
        }

        public static void AddSubNodesForInstances(ResourceTreeNodeExt node, ResourceViewManager.ResourceNameList resources)
        {
            ResourceMaps.LongMap map = new ResourceMaps.LongMap();
            foreach (NamedPackedFileDescriptor pfd in resources)
            {
                ResourceViewManager.ResourceNameList list;
                if (!map.ContainsKey(pfd.Descriptor.LongInstance))
                {
                    list = new ResourceViewManager.ResourceNameList();
                    map.Add(pfd.Descriptor.LongInstance, list);
                }
                else list = map[pfd.Descriptor.LongInstance];

                list.Add(pfd);
            }

            ResourceTreeNodesByInstance.AddInstances(map, node, false, false);
        }
    }
}
