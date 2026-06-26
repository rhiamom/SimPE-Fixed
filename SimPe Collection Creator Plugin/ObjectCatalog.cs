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
using System.Collections.Generic;
using System.IO;
using System.Linq;
using SimPe.Data;
using SimPe.Interfaces.Files;
using SimPe.Packages;
using SimPe.PackedFiles.Wrapper;

namespace SimPe.Plugin
{
    /// <summary>
    /// Metadata pulled out of an object <c>.package</c>'s OBJD resource —
    /// just enough to (a) populate a <see cref="CollectionMember"/> for
    /// inclusion in a collection and (b) show a friendly name in the
    /// editor's list. Per-OBJD record (an object package can contain
    /// multiple OBJDs; <see cref="ObjectCatalog"/> returns all of them
    /// and lets the caller decide).
    /// </summary>
    public class ObjectInfo
    {
        /// <summary>The OBJD resource's TGI — goes into the collection's 3IDR.</summary>
        public uint ObjectType { get; set; } = MetaData.OBJD_FILE;
        public uint ObjectGroup { get; set; }
        public uint ObjectInstance { get; set; }
        public uint ObjectInstanceHi { get; set; }

        /// <summary>Object GUID extracted from the OBJD body.</summary>
        public uint Guid { get; set; }

        /// <summary>Filename embedded in the OBJD — used as fallback name.</summary>
        public string ObjdName { get; set; } = string.Empty;

        /// <summary>
        /// User-friendly name resolved by <see cref="MaxisObjectList.Lookup"/>
        /// against the GUID. Falls back to <see cref="ObjdName"/> if no match.
        /// </summary>
        public string DisplayName { get; set; } = string.Empty;
    }

    /// <summary>
    /// Inspects an object <c>.package</c> and yields one
    /// <see cref="ObjectInfo"/> per OBJD found. Replaces JFade's
    /// <c>HandleOBJD/Pull3IDRInfo</c> family with SimPE's OBJD wrapper.
    /// </summary>
    public static class ObjectCatalog
    {
        /// <summary>
        /// Open <paramref name="objectPackagePath"/> and return all OBJDs
        /// inside it. Empty list if the package has none (not a catalog
        /// object). Friendly names are resolved using
        /// <paramref name="nameTablePath"/> if provided — usually the
        /// MaxisObjectList.txt shipped in this plugin's data/ folder.
        /// </summary>
        public static IList<ObjectInfo> Read(string objectPackagePath, string nameTablePath = null)
        {
            if (string.IsNullOrEmpty(objectPackagePath))
                throw new ArgumentException("Package path required.", nameof(objectPackagePath));

            GeneratableFile pkg = GeneratableFile.LoadFromFile(objectPackagePath);
            var results = new List<ObjectInfo>();

            foreach (IPackedFileDescriptor pfd in pkg.Index.Where(p => p.Type == MetaData.OBJD_FILE))
            {
                // Objd needs an OpcodeProvider for full BHAV-context parsing,
                // but we only read the GUID/FileName/TGI fields, none of which
                // touch opcodes. Passing null is safe for this read-only use.
                Objd objd = new Objd(null);
                objd.ProcessData(pfd, pkg);

                var info = new ObjectInfo
                {
                    ObjectType = pfd.Type,
                    ObjectGroup = pfd.Group,
                    ObjectInstance = pfd.Instance,
                    ObjectInstanceHi = pfd.SubType,
                    Guid = objd.Guid,
                    ObjdName = objd.FileName ?? string.Empty,
                };

                // Try the lookup table for a nicer name; fall back to the
                // FileName field inside the OBJD itself if no match.
                info.DisplayName = MaxisObjectList.Lookup(info.Guid, nameTablePath) ?? info.ObjdName;

                results.Add(info);
            }

            return results;
        }
    }

    /// <summary>
    /// CSV lookup for GUID → friendly name. JFade's
    /// <c>MaxisObjectList.txt</c> is a 2-column comma-separated file
    /// (GUID hex, name) shipped in <c>data/</c>. <c>UserObjectList.txt</c>
    /// in the same folder lets users add their own mappings — checked
    /// first so user overrides win.
    /// </summary>
    public static class MaxisObjectList
    {
        /// <summary>
        /// Look up <paramref name="guid"/> in the GUID→name table at
        /// <paramref name="tablePath"/>. Returns null if not found or
        /// the file doesn't exist. Tries <c>UserObjectList.txt</c> in
        /// the same directory first so user customisations take priority.
        /// </summary>
        public static string Lookup(uint guid, string tablePath)
        {
            if (string.IsNullOrEmpty(tablePath) || !System.IO.File.Exists(tablePath)) return null;

            string userTable = Path.Combine(Path.GetDirectoryName(tablePath) ?? string.Empty, "UserObjectList.txt");

            string hit = LookupInFile(guid, userTable);
            if (hit != null) return hit;

            return LookupInFile(guid, tablePath);
        }

        // Linear scan of a comma-separated GUID,name file. The tables are
        // small enough (~5,000 lines) that the overhead doesn't matter
        // when called per-Add-Item; if it ever does, swap for a cached
        // Dictionary keyed by GUID hex.
        static string LookupInFile(uint guid, string path)
        {
            if (string.IsNullOrEmpty(path) || !System.IO.File.Exists(path)) return null;

            string needle = guid.ToString("X8");
            foreach (string raw in System.IO.File.ReadLines(path))
            {
                string line = raw.Trim();
                if (line.Length == 0 || line.StartsWith("#")) continue;

                int comma = line.IndexOf(',');
                if (comma <= 0) continue;

                string key = line.Substring(0, comma).Trim();
                // JFade's files store GUIDs both as bare hex and as "0xHHHH";
                // normalize before compare.
                if (key.StartsWith("0x", StringComparison.OrdinalIgnoreCase))
                    key = key.Substring(2);

                if (string.Equals(key, needle, StringComparison.OrdinalIgnoreCase))
                    return line.Substring(comma + 1).Trim();
            }
            return null;
        }
    }
}
