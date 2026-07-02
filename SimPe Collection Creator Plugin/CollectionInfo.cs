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

using System.Collections.Generic;
using System.Drawing;

namespace SimPe.Plugin
{
    /// <summary>
    /// Where a collection appears in the catalog. Maps to the size/shape of
    /// the cGZPropertySetString flags JFade's original computed by file size
    /// after the COLL resource was already written — we set it directly here.
    /// </summary>
    public enum CollectionScope
    {
        /// <summary>Residential lots only (Home buy/build catalogs).</summary>
        Residential = 0,

        /// <summary>Community lots only.</summary>
        Community = 1,

        /// <summary>Both Residential and Community.</summary>
        Both = 2,
    }

    /// <summary>
    /// In-memory model of a Sims 2 collection — what the editor manipulates
    /// and what <see cref="CollectionWriter"/> serializes to a .package.
    /// Decoupled from on-disk DBPF resources so the UI can stay engine-agnostic.
    /// </summary>
    public class CollectionInfo
    {
        /// <summary>
        /// Instance ID of the collection's COLL resource. Doubles as the
        /// shared instance ID across the COLL / IMG / STR# resources so the
        /// game can pair them. JFade randomized this on "Make New Collection".
        /// </summary>
        public uint Instance { get; set; }

        /// <summary>Group ID for every resource in this collection package.</summary>
        public uint Group { get; set; } = 0x1F8B219F;

        /// <summary>User-visible collection name (goes into the STR# resource).</summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>Thumbnail icon (goes into the IMG resource). Optional.</summary>
        public Image Thumbnail { get; set; }

        /// <summary>Where in the catalog the collection should appear.</summary>
        public CollectionScope Scope { get; set; } = CollectionScope.Residential;

        /// <summary>
        /// Numeric creator GUID written into the COLL's "creatorid" property.
        /// JFade asked the user to enter this in txtGUID.
        /// </summary>
        public uint CreatorId { get; set; }

        /// <summary>Member objects, in display order.</summary>
        public List<CollectionMember> Members { get; } = new List<CollectionMember>();
    }

    /// <summary>
    /// One member object inside a collection. The four uint TGI fields are
    /// the catalog object's real resource pointer — they go into the 3IDR's
    /// reference array, and the BINX's <c>objectidx</c> property points at
    /// the index of this entry within that array.
    /// </summary>
    public class CollectionMember
    {
        public uint ObjectType { get; set; }
        public uint ObjectGroup { get; set; }
        public uint ObjectInstance { get; set; }
        public uint ObjectInstanceHi { get; set; }

        /// <summary>
        /// Display name shown in the editor's list. Not written to the
        /// .package — purely UI state, populated by <c>CollectionReader</c>
        /// when reopening an existing collection.
        /// </summary>
        public string DisplayName { get; set; } = string.Empty;

        /// <summary>
        /// Object GUID (from the OBJD), used for thumbnail lookup and
        /// MaxisObjectList.txt name resolution. Same caveat as DisplayName.
        /// </summary>
        public uint Guid { get; set; }

        /// <summary>
        /// Basename (no extension, lowercase) of the .package this member
        /// was added from. Feeds JFade's thumbnail lookup — the game
        /// caches CC previews in <c>ObjectThumbnails.package</c> keyed by
        /// <c>instance = 0x7F000000 | CRC24(basename)</c>. Populated at
        /// Add/Batch-Add time; empty for members loaded from an existing
        /// collection package (we no longer know the source file).
        /// </summary>
        public string SourceBasename { get; set; } = string.Empty;

        /// <summary>Optional per-member thumbnail (display only).</summary>
        public Image Thumbnail { get; set; }
    }
}
