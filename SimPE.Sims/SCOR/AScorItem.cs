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
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace SimPe.PackedFiles.Wrapper.SCOR
{
    [System.ComponentModel.ToolboxItem(false)]
    public  partial  class AScorItem : UserControl
    {
        ScorItem parent;
        string name;
        public string TokenName
        {
            get { return name; }
        }
        public ScorItem ParentItem
        {
            get { return parent; }
        }

        [DesignerSerializationVisibility( DesignerSerializationVisibility.Hidden), Browsable(false)]
        public bool Changed
        {
            get { 
                if (parent!=null)
                    if (parent.Parent !=null)
                        return parent.Parent.Changed;

                return false;
            }
            set {
                if (parent != null)
                    if (parent.Parent != null)                        
                        parent.Parent.Changed = value; 
            }
        }

        internal AScorItem()
            : this(new ScorItem(null))
        {
            name = "";
        }

        internal AScorItem(ScorItem si)
        {
            this.parent = si;
            InitializeComponent();
        }

        internal void SetData(string name, System.IO.BinaryReader reader)
        {
            this.name = name;
            if (reader != null) DoSetData(name, reader);
        }

        protected virtual void DoSetData(string name, System.IO.BinaryReader reader)
        {
        }

        internal virtual void Serialize(System.IO.BinaryWriter writer, bool last)
        {
            StreamHelper.WriteString(writer, name);
        }
    }
}
