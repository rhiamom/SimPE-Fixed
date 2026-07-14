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
            bool rootIsItselfABaseGame = Directory.Exists(rootTsData);
            if (rootIsItselfABaseGame)
            {
                packs.Add(new PackFolderInfo(
                    name: "Base Game",
                    fullPath: root,
                    isBaseGame: true,
                    hasTsData: true));
            }

            // Look at all immediate child directories under the root.
            // NEW: Walk the directory tree to find TSData at ANY reasonable depth.
            // This covers UC layouts like "Fun with Pets\\SP9\\TSData" etc.
            var seenPackPaths = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

            // We already handled root\\TSData above, but track it so we don't duplicate.
            if (rootIsItselfABaseGame)
            {
                seenPackPaths.Add(root);
            }

            // Two layouts need the chosen root's SIBLINGS scanned too, not
            // just its descendants:
            //
            // (1) Classic Origin / Mr DJ repack: each EP/SP sits as a
            //     sibling of the base game (e.g., "The Sims 2\\TSData",
            //     "The Sims 2 University\\TSData", ...). If the user picked
            //     the base-game folder itself, the BFS from there can't see
            //     the sibling packs. Triggered when the chosen root has
            //     TSData directly (rootIsItselfABaseGame).
            //
            // (2) Magipacks-style UC bundles: root contains bundle folders
            //     like "Double Deluxe", "Fun with Pets", "Best of Business",
            //     each of which contains "Base" / "EPn" / "SPn" subfolders
            //     with TSData. If the user picked one of the bundles
            //     directly (very natural — Double Deluxe visually feels
            //     like "the install"), the BFS from Double Deluxe finds
            //     Base/EP2/SP4 inside but never sees the sibling bundle
            //     folders. Triggered when root has a Base subfolder with
            //     TSData (rootLooksLikeUcBundle).
            //
            // Only these two narrow signals trigger sibling-widening so we
            // don't accidentally scan a user's whole Program Files tree.
            bool rootLooksLikeUcBundle = !rootIsItselfABaseGame &&
                Directory.Exists(Path.Combine(root, "Base", "TSData"));

            var rootsToWalk = new List<string> { root };
            if (rootIsItselfABaseGame || rootLooksLikeUcBundle)
            {
                string parent = null;
                try { parent = Path.GetDirectoryName(root); } catch { }
                if (!string.IsNullOrEmpty(parent) && Directory.Exists(parent))
                {
                    string[] siblings;
                    try { siblings = Directory.GetDirectories(parent); }
                    catch { siblings = new string[0]; }
                    foreach (string sib in siblings)
                    {
                        if (string.Equals(sib, root, StringComparison.OrdinalIgnoreCase)) continue;
                        // Include a sibling if it looks like ANY of:
                        //   (a) A single-pack folder — has TSData directly
                        //       (Magipacks "Apartment Life", "Bon Voyage",
                        //       "Free Time", "Glamour Life Stuff", "Seasons").
                        //   (b) A UC bundle folder — has Base/TSData child
                        //       (Magipacks "Double Deluxe").
                        //   (c) A UC bundle folder — has ANY EPn/SPn child
                        //       with TSData (Magipacks "Best of Business",
                        //       "Fun with Pets", "University Life" — none
                        //       have TSData directly and none contain Base).
                        //   (d) Has "Sims" in the name (classic disc layout).
                        // Otherwise skip — avoids paying enumeration cost
                        // for unrelated siblings on slow disks.
                        string sibName = Path.GetFileName(sib) ?? "";
                        bool include = false;
                        if (sibName.IndexOf("Sims", StringComparison.OrdinalIgnoreCase) >= 0) include = true;
                        else if (Directory.Exists(Path.Combine(sib, "TSData"))) include = true;
                        else if (Directory.Exists(Path.Combine(sib, "Base", "TSData"))) include = true;
                        else if (SiblingHasPackChild(sib)) include = true;

                        if (include) rootsToWalk.Add(sib);
                    }
                }
            }

            // Depth limit: prevents scanning huge directory trees if someone points at a big folder.
            // 4–5 is usually enough for Sims 2 layouts.
            const int maxDepth = 5;

            var queue = new Queue<Tuple<string, int>>();
            foreach (string r in rootsToWalk)
                queue.Enqueue(Tuple.Create(r, 0));

            while (queue.Count > 0)
            {
                var item = queue.Dequeue();
                string currentDir = item.Item1;
                int depth = item.Item2;

                if (depth > maxDepth)
                    continue;

                // If this folder contains TSData, it's a pack root.
                string tsDataPath = Path.Combine(currentDir, "TSData");
                if (Directory.Exists(tsDataPath))
                {
                    if (!seenPackPaths.Contains(currentDir))
                    {
                        string name = Path.GetFileName(currentDir) ?? currentDir;

                        // Treat both "Base" (Legacy-style) and "The Sims 2" (disc-style) as base game
                        bool isBaseChild =
                            string.Equals(name, "Base", StringComparison.OrdinalIgnoreCase) ||
                            string.Equals(name, "The Sims 2", StringComparison.OrdinalIgnoreCase) ||
                            string.Equals(currentDir, root, StringComparison.OrdinalIgnoreCase);

                        string displayName = isBaseChild ? "Base Game" : name;

                        packs.Add(new PackFolderInfo(
                            name: displayName,
                            fullPath: currentDir,
                            isBaseGame: isBaseChild,
                            hasTsData: true));

                        seenPackPaths.Add(currentDir);
                    }

                    // IMPORTANT: Don’t descend further once TSData is found here.
                    continue;
                }

                // Otherwise, enqueue child dirs
                string[] subDirs;
                try
                {
                    subDirs = Directory.GetDirectories(currentDir);
                }
                catch
                {
                    subDirs = new string[0]; // .NET 4.5.2 compatible
                }

                foreach (string sub in subDirs)
                {
                    string subName = Path.GetFileName(sub) ?? sub;

                    // Ignore EA patch / temp folders like TH14FF~1, THE3E9~1, etc.
                    if (subName.StartsWith("TH", StringComparison.OrdinalIgnoreCase) && subName.Contains("~"))
                        continue;

                    queue.Enqueue(Tuple.Create(sub, depth + 1));
                }
            }


            return new GameRootScanResult(
                rootFolder: root,
                packs: new ReadOnlyCollection<PackFolderInfo>(packs));
        }

        // Cheap one-level probe used by the sibling-widening filter above:
        // does this sibling contain at least one immediate child folder that
        // itself has TSData? Catches Magipacks bundle folders like "Best of
        // Business" / "Fun with Pets" / "University Life" — these have no
        // TSData directly and no "Base" child, only EPn/SPn children.
        static bool SiblingHasPackChild(string sib)
        {
            string[] children;
            try { children = Directory.GetDirectories(sib); }
            catch { return false; }

            foreach (string child in children)
            {
                try
                {
                    if (Directory.Exists(Path.Combine(child, "TSData")))
                        return true;
                }
                catch { }
            }
            return false;
        }
    }
}


