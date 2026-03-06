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
using System.Text;
using System.Xml;

namespace SimPe.Data
{

    public class SemiGlobalListing : System.Collections.Generic.List<SemiGlobalAlias>
    {
        string flname;
        public SemiGlobalListing() : this(Helper.SimPeSemiGlobalFile) { }

        public SemiGlobalListing(string flname)
        {
            this.flname = flname;
            LoadXML();
        }

        void LoadXML()
        {
            //read XML File
            System.Xml.XmlDocument xmlfile = new XmlDocument();
            xmlfile.Load(flname);

            //seek Root Node
            XmlNodeList XMLData = xmlfile.GetElementsByTagName("semiglobals");

            //Process all Root Node Entries
            for (int i = 0; i < XMLData.Count; i++)
            {
                XmlNode node = XMLData.Item(i);
                foreach (XmlNode subnode in node.ChildNodes)
                    ProcessItem(subnode);
                
            }
        }

        void ProcessItem(XmlNode node)
        {
            bool known = false;
            uint group = 0;
            string name = "";
            foreach (XmlNode subnode in node)
            {
                if (subnode.Name == "known")
                {
                    known = true;
                }
                else if (subnode.Name == "group")
                {
                    group = Helper.StringToUInt32(subnode.InnerText, 0, 16);
                }
                else if (subnode.Name == "name")
                {
                    name = subnode.InnerText.Trim();
                }
            }

            if (name!="" && group !=0)
                this.Add(new SemiGlobalAlias(known, group, name));
        }
    }
}
