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
    /// Tells the caller why a package couldn't be added to a collection
    /// — useful for explaining the OBJD requirement to users who picked
    /// a recolor by mistake.
    /// </summary>
    public enum PackageKind
    {
        /// <summary>Has at least one OBJD — addable to a collection.</summary>
        Object,

        /// <summary>No OBJD but has recolor content (MMAT/TXMT/TXTR).
        /// Caller should explain to the user that Sims 2 catalog collections
        /// are object-keyed; the original object has to go in first.</summary>
        Recolor,

        /// <summary>No OBJD and no recolor markers — unknown structure
        /// (broken package, neighborhood resource, etc.).</summary>
        Unknown,
    }

    /// <summary>
    /// Inspects an object <c>.package</c> and yields one
    /// <see cref="ObjectInfo"/> per OBJD found. Replaces JFade's
    /// <c>HandleOBJD/Pull3IDRInfo</c> family with SimPE's OBJD wrapper.
    /// </summary>
    public static class ObjectCatalog
    {
        // Recolor packages always carry a Material Override (MMAT) entry
        // pointing at the parent object's GUID. That's a much stronger
        // signal than "has TXMT/TXTR" alone (object packages have those
        // too). MMAT presence + no OBJD = definitively a recolor.
        const uint MMAT_TYPE = 0x4C697E5A;

        /// <summary>
        /// Quick classification of <paramref name="packagePath"/> so the
        /// caller can tell the user why a package was rejected — recolor
        /// vs broken vs not-recognised.
        /// </summary>
        public static PackageKind Classify(string packagePath)
        {
            if (string.IsNullOrEmpty(packagePath) || !System.IO.File.Exists(packagePath))
                return PackageKind.Unknown;

            try
            {
                GeneratableFile pkg = GeneratableFile.LoadFromFile(packagePath);
                bool hasObjd = false, hasMmat = false;
                foreach (var p in pkg.Index)
                {
                    if (p.Type == MetaData.OBJD_FILE) { hasObjd = true; break; }
                    if (p.Type == MMAT_TYPE) hasMmat = true;
                }
                if (hasObjd) return PackageKind.Object;
                if (hasMmat) return PackageKind.Recolor;
                return PackageKind.Unknown;
            }
            catch
            {
                return PackageKind.Unknown;
            }
        }
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
                // Per-OBJD try/catch so one malformed entry can't take out
                // the whole bundle — merged catalogs like globalCatbundle.package
                // carry thousands of OBJDs and a single odd-format one would
                // otherwise drop the entire file to "unrecognised".
                try
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
                catch
                {
                }
            }

            return results;
        }
    }

    /// <summary>
    /// GUID → friendly-name lookup. JFade's <c>MaxisObjectList.txt</c>
    /// is a 5-column semicolon-separated file with a header row:
    /// <c>Group ID;GUID;CTSS Name;OBJD Name;Game Version</c>.
    /// We key on column 2 (GUID, stored as <c>0xHHHHHHHH</c>) and return
    /// column 3 (CTSS Name — the human-readable catalog name).
    /// <c>UserObjectList.txt</c> in the same folder uses the same format
    /// and is checked first so user customisations win.
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

        // Linear scan of a semicolon-separated 5-column file. The table
        // is ~1,270 lines today; lookup cost is negligible per Add-Item.
        // If it ever matters, swap for a cached Dictionary<uint, string>.
        // Header row ("Group ID;GUID;...") is skipped automatically because
        // its second column doesn't parse as a hex GUID.
        static string LookupInFile(uint guid, string path)
        {
            if (string.IsNullOrEmpty(path) || !System.IO.File.Exists(path)) return null;

            string needle = guid.ToString("X8");
            foreach (string raw in System.IO.File.ReadLines(path))
            {
                string line = raw.Trim();
                if (line.Length == 0 || line.StartsWith("#")) continue;

                // Columns: 0=Group, 1=GUID, 2=CTSS Name, 3=OBJD Name, 4=Pack
                string[] cols = line.Split(';');
                if (cols.Length < 3) continue;

                string key = cols[1].Trim();
                if (key.StartsWith("0x", StringComparison.OrdinalIgnoreCase))
                    key = key.Substring(2);

                if (string.Equals(key, needle, StringComparison.OrdinalIgnoreCase))
                    return cols[2].Trim();
            }
            return null;
        }
    }
}
