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

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;

namespace SimPe
{
    /// <summary>
    /// Represents a single pack folder under a user-chosen Game Root.
    /// This can be the base game (root itself) or any child folder that has TSData.
    /// </summary>
    public sealed class PackFolderInfo
    {
        /// <summary>
        /// The display name we use for this pack. For base game this is "Base Game",
        /// for other packs this is the directory name (e.g. "EP1", "SP9", "Mansion and Garden Stuff").
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Full path to the folder that contains TSData.
        /// For base game this is the root; for packs this is the immediate child directory.
        /// </summary>
        public string FullPath { get; }

        /// <summary>
        /// True if this entry represents the base game at the root.
        /// </summary>
        public bool IsBaseGame { get; }

        /// <summary>
        /// True if a TSData subfolder actually exists under FullPath.
        /// </summary>
        public bool HasTsData { get; }

        internal PackFolderInfo(string name, string fullPath, bool isBaseGame, bool hasTsData)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            FullPath = fullPath ?? throw new ArgumentNullException(nameof(fullPath));
            IsBaseGame = isBaseGame;
            HasTsData = hasTsData;
        }

        public override string ToString()
        {
            return $"{(IsBaseGame ? "[Base]" : "[Pack]")} {Name} - " +
                   (HasTsData ? FullPath : "No TSData found");
        }
    }

    /// <summary>
    /// Result of scanning a Game Root folder.
    /// </summary>
    public sealed class GameRootScanResult
    {
        /// <summary>
        /// The normalized root folder that was scanned.
        /// </summary>
        public string RootFolder { get; }

        /// <summary>
        /// All pack folders discovered under this root, including the base game.
        /// Only entries with HasTsData == true are "real" packs to use.
        /// </summary>
        public ReadOnlyCollection<PackFolderInfo> Packs { get; }

        internal GameRootScanResult(string rootFolder, ReadOnlyCollection<PackFolderInfo> packs)
        {
            RootFolder = rootFolder ?? throw new ArgumentNullException(nameof(rootFolder));
            Packs = packs ?? throw new ArgumentNullException(nameof(packs));
        }
    }

    /// <summary>
    /// Scans a user-chosen Game Root and discovers all packs by looking for TSData.
    /// 
    /// Rules (v1):
    ///  - Base game is at: root\TSData
    ///  - Each immediate child directory of root is examined:
    ///        root\[child]\TSData
    ///    If TSData exists there, it is considered a pack.
    /// 
    /// We intentionally do NOT assume anything about the child folder names.
    /// They could be EP1, EP2, Best of Business, random repack names, etc.
    /// </summary>
    public static class GameRootAutoScanner
    {
        public static GameRootScanResult ScanRoot(string gameRootFolder)
        {
            if (string.IsNullOrWhiteSpace(gameRootFolder))
                throw new ArgumentException("Game root folder must not be empty.", nameof(gameRootFolder));

            string root;
            try
            {
                root = Path.GetFullPath(gameRootFolder);
            }
            catch
            {
                // If GetFullPath fails for any reason, just use the raw string.
                root = gameRootFolder;
            }

            var packs = new List<PackFolderInfo>();

            string rootTsData = Path.Combine(root, "TSData");
            if (Directory.Exists(rootTsData))
            {
                packs.Add(new PackFolderInfo(
                    name: "Base Game",
                    fullPath: root,
                    isBaseGame: true,
                    hasTsData: true));
            }

            // Look at all immediate child directories under the root.
            string[] childDirs;
            try
            {
                childDirs = Directory.GetDirectories(root);
            }
            catch
            {
                childDirs = new string[0];   // .NET 4.5.2 compatible
            }

            foreach (string dir in childDirs)
            {
                string name = Path.GetFileName(dir) ?? dir;

                // Ignore EA patch / temp folders like TH14FF~1, THE3E9~1, etc.
                if (name.StartsWith("TH", StringComparison.OrdinalIgnoreCase) && name.Contains("~"))
                    continue;

                string tsDataPath = Path.Combine(dir, "TSData");
                bool hasTsData = Directory.Exists(tsDataPath);

                if (hasTsData)
                {
                    // Treat both "Base" (Legacy-style) and "The Sims 2" (disc-style) as base game
                    bool isBaseChild =
                        string.Equals(name, "Base", StringComparison.OrdinalIgnoreCase) ||
                        string.Equals(name, "The Sims 2", StringComparison.OrdinalIgnoreCase);

                    string displayName = isBaseChild ? "Base Game" : name;

                    packs.Add(new PackFolderInfo(
                        name: displayName,
                        fullPath: dir,
                        isBaseGame: isBaseChild,
                        hasTsData: true));
                }
                else
                {
                    // NEW: look one level deeper (dir\[grandchild]\TSData)
                    string[] grandChildDirs;
                    try
                    {
                        grandChildDirs = Directory.GetDirectories(dir);
                    }
                    catch
                    {
                        grandChildDirs = new string[0];   // .NET 4.5.2 compatible
                    }

                    foreach (string gdir in grandChildDirs)
                    {
                        string gname = Path.GetFileName(gdir) ?? gdir;

                        // Ignore EA patch / temp folders like TH14FF~1, THE3E9~1, etc.
                        if (gname.StartsWith("TH", StringComparison.OrdinalIgnoreCase) && gname.Contains("~"))
                            continue;

                        string gTsData = Path.Combine(gdir, "TSData");
                        if (!Directory.Exists(gTsData))
                            continue;

                        bool isBaseGrandChild =
                            string.Equals(gname, "Base", StringComparison.OrdinalIgnoreCase) ||
                            string.Equals(gname, "The Sims 2", StringComparison.OrdinalIgnoreCase);

                        string gDisplayName = isBaseGrandChild ? "Base Game" : gname;

                        packs.Add(new PackFolderInfo(
                            name: gDisplayName,
                            fullPath: gdir,
                            isBaseGame: isBaseGrandChild,
                            hasTsData: true));
                    }
                }

            }

            return new GameRootScanResult(
                rootFolder: root,
                packs: new ReadOnlyCollection<PackFolderInfo>(packs));
        }
    }
}


