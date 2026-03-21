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
using System.Text;
using Avalonia.Controls;

namespace SimPe.PackedFiles.Wrapper.SCOR
{
    /// <summary>
    /// Avalonia port of LearnedBahaviourComboBox.
    /// </summary>
    public partial class LearnedBahaviourComboBox : Avalonia.Controls.ComboBox
    {
        public LearnedBahaviourComboBox()
        {
            try
            {
                foreach (ExtObjd objd in BehaviourObjds)
                {
                    this.Items.Add(new ContainerItem(objd));
                }
            }
            catch { }
        }

        class ContainerItem
        {
            ExtObjd objd;
            public ContainerItem(uint guid)
            {
                objd = new ExtObjd();
                objd.Guid = guid;
                objd.FileName = "0x" + Helper.HexString(guid);
            }

            public ContainerItem(ExtObjd objd)
            {
                this.objd = objd;
            }

            public uint Guid
            {
                get { return objd.Guid; }
            }

            public override string ToString()
            {
                return objd.FileName;
            }
        }

        #region Behaviours
        static List<ExtObjd> objds;
        internal static List<ExtObjd> BehaviourObjds
        {
            get
            {
                if (objds == null) GetPetBehaviours();
                return objds;
            }
        }

        private static void GetPetBehaviours()
        {
            if (objds != null) return;
            objds = new List<ExtObjd>();

            FileTable.FileIndex.Load();
            SimPe.Interfaces.Scenegraph.IScenegraphFileIndexItem[] objs = FileTable.FileIndex.FindFileDiscardingGroup(Data.MetaData.OBJD_FILE, 0x41a7);
            Wait.Start(objs.Length);
            Wait.Message = "Loading Behaviours...";
            int ct = 0;
            foreach (SimPe.Interfaces.Scenegraph.IScenegraphFileIndexItem ofii in objs)
            {
                Wait.Progress = ct++;
                ExtObjd obj = new ExtObjd();
                obj.ProcessData(ofii);
                if (obj.FileName.StartsWith("Learned Behavior"))
                {
                    objds.Add(obj);
                }
            }
            Wait.Stop();
        }
        #endregion

        [System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Hidden), Browsable(false)]
        public uint SelectedGuid
        {
            get
            {
                ContainerItem ci = SelectedItem as ContainerItem;
                if (ci == null) return 0;
                return ci.Guid;
            }

            set
            {
                SelectedIndex = -1;
                for (int i = 0; i < ItemCount; i++)
                {
                    ContainerItem ci = Items[i] as ContainerItem;
                    if (ci != null && ci.Guid == value)
                    {
                        this.SelectedIndex = i;
                        return;
                    }
                }

                this.Items.Add(new ContainerItem(value));
            }
        }
    }
}
