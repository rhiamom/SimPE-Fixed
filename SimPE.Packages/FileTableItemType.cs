/***************************************************************************
 *   Copyright (C) 2005 by Ambertation                                     *
 *   quaxi@ambertation.de                                                  *
 *                                                                         *
 *   Copyright (C) 2025 by GramzeSweatShop                                 *
 *   Rhiamom@mac.com                                                       *
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

namespace SimPe
{
    public enum FileTablePaths : uint
    {
        Absolute = 0x8FFF0000,
        SaveGameFolder = 0x8FFF0001,
        SimPEFolder = 0x8FFE0000,
        SimPEDataFolder = 0x8FFE0001,
        SimPEPluginFolder = 0x8FFE0002,
    }

    public class FileTableItemType : IComparable
    {
        uint val = 0;
        public FileTableItemType(Expansions e)
            : this((uint)e)
        {
        }

        public FileTableItemType(FileTablePaths f)
            : this((uint)f)
        {
        }

        public FileTableItemType(int i)
        {
            val = (uint)i;
        }

        public FileTableItemType(uint i)
        {
            val = i;
        }

        public static implicit operator FileTableItemType(Expansions e)
        {
            return new FileTableItemType(e);
        }

        public static implicit operator FileTableItemType(FileTablePaths f)
        {
            return new FileTableItemType(f);
        }

        public static implicit operator FileTableItemType(int i)
        {
            return new FileTableItemType(i);
        }

        public static implicit operator FileTableItemType(uint i)
        {
            return new FileTableItemType(i);
        }

        public Expansions AsExpansions
        {
            get
            {
                if (val <= 0x80000000)
                    return (Expansions)val;
                else return Expansions.Custom;
            }
        }

        public FileTablePaths AsFileTablePaths
        {
            get
            {
                if (val > 0x80000000)
                    return (FileTablePaths)val;
                else return FileTablePaths.Absolute;
            }
        }

        public uint AsUint
        {
            get { return val; }
        }

        public string GetRoot()
        {
            string ret = null;
            if (val <= 0x80000000) ret = PathProvider.Global.GetExpansion(AsExpansions).InstallFolder;
            else if (this == FileTablePaths.SaveGameFolder) ret = PathProvider.SimSavegameFolder;
            else if (this == FileTablePaths.SimPEDataFolder) ret = Helper.SimPeDataPath;
            else if (this == FileTablePaths.SimPEFolder) ret = Helper.SimPePath;
            else if (this == FileTablePaths.SimPEPluginFolder) ret = Helper.SimPePluginPath;
//            else ret = PathProvider.Global.GetExpansion(AsExpansions).InstallFolder;

            return ret;
        }

        public int GetEPVersion()
        {
            if (val > 0x80000000) return -1;
            ExpansionItem ei = PathProvider.Global[AsExpansions];
            if (ei.Flag.Class == ExpansionItem.Classes.Story) return -1;
            return ei.Version;

            //return -1;
        }

        #region IComparable Member

        public int CompareTo(object obj)
        {
            if (obj is Expansions) return val.CompareTo((uint)((Expansions)obj));
            if (obj is FileTablePaths) return val.CompareTo((uint)((FileTablePaths)obj));
            if (obj is int) return val.CompareTo((uint)((int)obj));
            if (obj is uint) return val.CompareTo((uint)obj);
            if (obj is FileTableItemType) return val.CompareTo(((FileTableItemType)obj).val);

            return ((object)val) == obj ? 0 : 1; // random objects are not equal
        }

        #endregion

        public override bool Equals(object obj)
        {
            int res = CompareTo(obj);
            return (res == 0);
            //return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return val.GetHashCode();
        }

        public static bool operator == (FileTableItemType a, Expansions b){
            return a.Equals(b);
        }

        public static bool operator !=(FileTableItemType a, Expansions b)
        {
            return !a.Equals(b);
        }

        public static bool operator == (FileTableItemType a, FileTableItemType b){
            return a.Equals(b);
        }

        public static bool operator !=(FileTableItemType a, FileTableItemType b)
        {
            return !a.Equals(b);
        }

        public static bool operator == (FileTableItemType a, FileTablePaths b){
            return a.Equals(b);
        }

        public static bool operator !=(FileTableItemType a, FileTablePaths b)
        {
            return !a.Equals(b);
        }

        public override string ToString()
        {
            if (val > 0x80000000) return AsFileTablePaths.ToString();
            else return AsExpansions.ToString();
        }

    }

}
