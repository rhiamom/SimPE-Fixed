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
using SimPe.Interfaces.Files;
using SimPe.Packages;

namespace SimPe.Plugin
{
    /// <summary>
    /// Reads per-object preview JPEGs out of the user's
    /// <c>ObjectThumbnails.package</c> cache. Faithful to JFade's original
    /// (<c>PullThumbnail</c> / <c>HandleThumbnail</c> in his frmMain.cs):
    /// <list type="bullet">
    ///   <item>Instance = <c>0x7F000000 | CRC24(basename)</c> where basename
    ///     is the source <c>.package</c> filename lowercased, no extension.</item>
    ///   <item>Resource type = <c>0xAC2950C1</c> (JFade wrote this
    ///     byte-reversed as the string <c>"C15029AC"</c>).</item>
    ///   <item>Only <c>ObjectThumbnails.package</c> is probed — JFade's
    ///     tool was CC-only and never touched Maxis catalog thumbnails,
    ///     which live in a different cache format anyway.</item>
    /// </list>
    /// The package handle is cached for the editor's lifetime so
    /// selection-changed events don't re-open the multi-megabyte file.
    /// </summary>
    internal static class ObjectThumbnailLoader
    {
        const uint THUMB_TYPE = 0xAC2950C1;

        static GeneratableFile cached;
        static string cachedPath;

        public static Image GetThumbnail(string thumbnailsFolder, string sourceBasename)
        {
            return GetThumbnail(thumbnailsFolder, sourceBasename, out _);
        }

        /// <summary>
        /// Diagnostic overload that also names the pipeline step that
        /// produced a null result — used for status-bar feedback so the
        /// user can tell config problems from genuine cache misses.
        /// </summary>
        public static Image GetThumbnail(string thumbnailsFolder, string sourceBasename, out string diagnostic)
        {
            if (string.IsNullOrWhiteSpace(sourceBasename))
            {
                diagnostic = "no source filename recorded for this member (was it added from an existing collection?)";
                return null;
            }
            if (string.IsNullOrWhiteSpace(thumbnailsFolder))
            {
                diagnostic = "Options → Thumbnails Package Directory is blank";
                return null;
            }

            string pkgPath = Path.Combine(thumbnailsFolder, "ObjectThumbnails.package");
            if (!System.IO.File.Exists(pkgPath))
            {
                diagnostic = $"ObjectThumbnails.package not found at {pkgPath}";
                return null;
            }

            var pkg = GetPackage(pkgPath);
            if (pkg == null)
            {
                diagnostic = $"failed to load {pkgPath}";
                return null;
            }

            uint inst = 0x7F000000u | (Hashes.GetCrc24(sourceBasename.ToLowerInvariant()) & 0x00FFFFFFu);
            try
            {
                // JFade compared instance IDs as strings across whatever
                // groups the file happened to use — his lstInstance2 was
                // populated by a prior full-file scan. FindFile-by-instance
                // isn't available; we fall through to a full index scan
                // for any entry matching (THUMB_TYPE, inst) regardless of
                // group descriptor.
                IPackedFileDescriptor matched = null;
                foreach (var p in pkg.Index)
                {
                    if (p.Type == THUMB_TYPE && p.Instance == inst)
                    {
                        matched = p;
                        break;
                    }
                }

                if (matched == null)
                {
                    diagnostic = $"no cache entry for basename=\"{sourceBasename}\" (hash=0x{inst:X8}) — the game may not have processed this CC yet";
                    return null;
                }

                var pic = new SimPe.PackedFiles.Wrapper.Picture();
                pic.ProcessData(matched, pkg);
                if (pic.Image == null)
                {
                    diagnostic = $"Picture wrapper produced null image for hash=0x{inst:X8}";
                    return null;
                }
                diagnostic = null;
                return pic.Image;
            }
            catch (Exception ex)
            {
                diagnostic = $"exception during lookup: {ex.GetType().Name}: {ex.Message}";
                return null;
            }
        }

        static GeneratableFile GetPackage(string path)
        {
            if (cached != null && string.Equals(cachedPath, path, StringComparison.OrdinalIgnoreCase))
                return cached;
            try
            {
                cached = GeneratableFile.LoadFromFile(path);
                cachedPath = path;
                return cached;
            }
            catch
            {
                cached = null;
                cachedPath = null;
                return null;
            }
        }
    }
}
