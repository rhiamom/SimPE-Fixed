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
using System.IO;

namespace SimPe.Plugin
{
    /// <summary>
    /// Per-user options for the Collection Creator (Collections directory,
    /// Thumbnails directory, plus the two checkboxes JFade exposed). Stored
    /// next to SimPE's own user config so it follows the user's profile and
    /// doesn't get blown away by a plugin reinstall.
    /// JFade's original wrote these to <c>Options.txt</c> next to the EXE
    /// but a long-standing bug stopped the save half from working — values
    /// were prompted-for on every launch. This rewrite actually persists.
    /// </summary>
    internal class CollectionOptions
    {
        public string CollectionsDir { get; set; } = "";
        public string ThumbnailsDir { get; set; } = "";
        public bool   WarningsOff    { get; set; }
        public bool   CompressOnSave { get; set; }

        /// <summary>
        /// Returns true if the user has at least set a Collections directory
        /// — the one path that can't be sensibly defaulted. Drives the
        /// first-run prompt on form load.
        /// </summary>
        public bool IsConfigured => !string.IsNullOrEmpty(CollectionsDir);

        // %APPDATA%\SimPe\Data\CollectionCreator.txt. Lives next to SimPE's
        // own simpe.xreg so a user wiping the SimPE profile clears this too.
        static string ConfigPath
        {
            get
            {
                try { return Path.Combine(Helper.SimPeDataPath, "CollectionCreator.txt"); }
                catch { return null; }
            }
        }

        /// <summary>Load options from disk, returning defaults if the file is
        /// missing or unreadable. Never throws.</summary>
        public static CollectionOptions Load()
        {
            var opts = new CollectionOptions();
            string path = ConfigPath;
            if (string.IsNullOrEmpty(path) || !File.Exists(path)) return opts;
            try
            {
                string[] lines = File.ReadAllLines(path);
                if (lines.Length > 0) opts.CollectionsDir = lines[0];
                if (lines.Length > 1) opts.ThumbnailsDir  = lines[1];
                if (lines.Length > 2) opts.WarningsOff    = lines[2].Trim() == "1";
                if (lines.Length > 3) opts.CompressOnSave = lines[3].Trim() == "1";
            }
            catch
            {
            }
            return opts;
        }

        /// <summary>Persist options to disk. Silently swallows IO errors —
        /// the user can re-set in Options if write fails.</summary>
        public void Save()
        {
            string path = ConfigPath;
            if (string.IsNullOrEmpty(path)) return;
            try
            {
                Directory.CreateDirectory(Path.GetDirectoryName(path));
                File.WriteAllLines(path, new[]
                {
                    CollectionsDir ?? "",
                    ThumbnailsDir  ?? "",
                    WarningsOff    ? "1" : "0",
                    CompressOnSave ? "1" : "0",
                });
            }
            catch
            {
            }
        }
    }
}
