/***************************************************************************
 *   Copyright (C) 2026 by GramzeSweatshop                                  *
 *   rhiamom@mac.com                                                        *
 *                                                                          *
 *   Built on JFade's Sims 2 Collection Creator (© 2006-2007 DJS Sims /     *
 *   The Sims Programming Group), used with permission of the original     *
 *   author (granted 2026-06-26).                                           *
 *                                                                          *
 *   This program is free software; you can redistribute it and/or modify   *
 *   it under the terms of the GNU General Public License as published by   *
 *   the Free Software Foundation; either version 2 of the License, or      *
 *   (at your option) any later version.                                    *
 ***************************************************************************/

using System;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using SimPe.Interfaces;
using SimPe.Interfaces.Plugin;

namespace SimPe.Plugin
{
    /// <summary>
    /// Editor form for Sims 2 collection packages. UI is laid out by hand
    /// in <see cref="InitializeComponent"/> (no Visual Studio .Designer.cs
    /// split — simpler to maintain for a single-author plugin). All real
    /// work delegates to <see cref="CollectionReader"/>,
    /// <see cref="CollectionWriter"/>, and <see cref="ObjectCatalog"/>;
    /// this file is purely UI + event glue.
    /// </summary>
    internal partial class CollectionCreatorForm : Form
    {
        // The collection currently being edited. Null when no collection is
        // loaded (initial state and after a New click is cancelled). Every
        // user action manipulates this object directly; saving serializes it.
        CollectionInfo current;

        // Path of the currently-open collection on disk, when it came from
        // Open Collection. Lets Save round-trip without re-prompting.
        string currentPath;

        // OpcodeProvider for Objd parsing — passed through from the tool's
        // factory. Null is acceptable; ObjectCatalog handles that.
        readonly IProviderRegistry providers;

        // Plugin-bundled data folder (data/) — resolved from the running DLL's
        // location once and cached. Used as starting directory for Sample
        // Icons picker and for the MaxisObjectList.txt lookup table.
        readonly string dataFolder;

        public CollectionCreatorForm(IProviderRegistry providers)
        {
            this.providers = providers;
            this.dataFolder = ResolveDataFolder();

            InitializeComponent();
            UpdateUIState();
        }

        /// <summary>Find the plugin's data folder next to the loaded DLL.</summary>
        static string ResolveDataFolder()
        {
            try
            {
                string dllPath = Assembly.GetExecutingAssembly().Location;
                string dllDir = Path.GetDirectoryName(dllPath) ?? "";
                // Directory.Build.targets copies data/ into bin/Release/data/
                // (next to the plugin DLL when it sits in Plugins\).
                string candidate = Path.Combine(dllDir, "data");
                if (Directory.Exists(candidate)) return candidate;
                // When launched from per-project bin, data/ also lives at
                // ../data relative to the DLL.
                candidate = Path.Combine(dllDir, "..", "data");
                if (Directory.Exists(candidate)) return Path.GetFullPath(candidate);
            }
            catch { }
            return null;
        }
    }
}
