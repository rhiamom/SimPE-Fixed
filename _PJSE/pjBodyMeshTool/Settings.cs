/***************************************************************************
 *   Copyright (C) 2008 by Peter L Jones                                   *
 *   peter@users.sf.net                                                    *
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
using System.Collections;
using System.Resources;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.ComponentModel;

namespace pj
{
    class Settings : SimPe.GlobalizedObject
    {
        const string BASENAME = "PJSE\\BMtool";
        SimPe.XmlRegistryKey xrk = SimPe.Helper.XmlRegistry.PluginRegistryKey;
        SimPe.XmlRegistryKey rkf = SimPe.Helper.XmlRegistry.PluginRegistryKey.CreateSubKey(BASENAME);
        public Settings() : base(new ResourceManager(typeof(Settings))) { }

        private static Settings settings = new Settings();

        [Category("PJSE")]
        public static bool BodyMeshExtractUseCres
        {
            get
            {
                SimPe.XmlRegistryKey rkf = SimPe.Helper.XmlRegistry.PluginRegistryKey.CreateSubKey(BASENAME);
                object o = rkf.GetValue("meshexttractusecres", false);
                return Convert.ToBoolean(o);
            }

            set
            {
                SimPe.XmlRegistryKey rkf = SimPe.Helper.XmlRegistry.PluginRegistryKey.CreateSubKey(BASENAME);
                rkf.SetValue("meshexttractusecres", value);
            }
        }
    }
}
