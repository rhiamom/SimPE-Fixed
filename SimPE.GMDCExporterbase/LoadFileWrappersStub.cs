/***************************************************************************
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

// Stub for SimPe.LoadFileWrappers used by ExporterLoader.cs.
// SimPE.WorkSpaceHelper/LoadFileWrappers.cs is currently excluded from the
// WorkSpaceHelper build because it depends on LoadFileWrappersExt (WinForms).
// This stub provides only the static LoadPlugins(string, Type) overload that
// ExporterLoader.cs calls, using the same reflection logic as the original.

using System;
using System.IO;
using System.Reflection;
using System.Collections;

namespace SimPe
{
    /// <summary>
    /// Stub for SimPe.LoadFileWrappers — provides only the static LoadPlugins
    /// overloads needed by ExporterLoader.  The full class lives in
    /// SimPE.WorkSpaceHelper/LoadFileWrappers.cs but is excluded until
    /// LoadFileWrappersExt (WinForms-heavy) is ported.
    /// </summary>
    public class LoadFileWrappers
    {
        /// <summary>
        /// Loads all public types from <paramref name="file"/> that implement
        /// <paramref name="interfaceType"/> and returns them as instances.
        /// Returns an empty array if the file cannot be loaded.
        /// </summary>
        public static object[] LoadPlugins(string file, Type interfaceType)
        {
            return LoadPlugins(file, interfaceType, new object[0]);
        }

        /// <summary>
        /// Loads all public types from <paramref name="file"/> that implement
        /// <paramref name="interfaceType"/>, constructing each with <paramref name="args"/>.
        /// Returns an empty array if the file cannot be loaded.
        /// </summary>
        public static object[] LoadPlugins(string file, Type interfaceType, object[] args)
        {
            if (!File.Exists(file)) return new object[0];
            try
            {
                Assembly a = Assembly.LoadFrom(file);
                return LoadPlugins(a, interfaceType, args);
            }
            catch
            {
                return new object[0];
            }
        }

        /// <summary>
        /// Loads all public types from assembly <paramref name="a"/> that implement
        /// <paramref name="interfaceType"/>, constructing each with <paramref name="args"/>.
        /// </summary>
        public static object[] LoadPlugins(Assembly a, Type interfaceType, object[] args)
        {
            ArrayList list = new ArrayList();
            try
            {
                foreach (Type t in a.GetTypes())
                {
                    if (!t.IsClass || t.IsAbstract) continue;
                    if (!interfaceType.IsAssignableFrom(t)) continue;
                    try
                    {
                        object instance = Activator.CreateInstance(t, args);
                        if (instance != null) list.Add(instance);
                    }
                    catch { }
                }
            }
            catch { }

            return list.ToArray();
        }
    }
}
