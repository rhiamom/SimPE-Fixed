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
using System.Collections;
using System.Drawing;
using System.IO;
using System.Linq;
using SimPe.Data;
using SimPe.Interfaces.Files;
using SimPe.Packages;
using SimPe.PackedFiles.Wrapper;

namespace SimPe.Plugin
{
    /// <summary>
    /// Loads an existing collection <c>.package</c> back into a
    /// <see cref="CollectionInfo"/> so the editor can modify it. Replaces
    /// JFade's <c>FindResource/Handle3IDR/HandleCOLLFile/HandleSTRFile/HandleJPGImage</c>
    /// family — same logic, but driven by SimPE's DBPF index and wrappers
    /// instead of byte-offset string slicing.
    /// </summary>
    public static class CollectionReader
    {
        // Mirrored from CollectionWriter for self-contained type matching.
        const uint COLL = 0x6C4F359D;

        /// <summary>
        /// Opens <paramref name="packagePath"/> as a collection. Returns null
        /// if the file isn't a collection (no COLL resource of the expected
        /// type). Members' DisplayName/Guid/Thumbnail fields are left blank —
        /// <see cref="ObjectCatalog"/> populates those in a second pass.
        /// </summary>
        public static CollectionInfo Read(string packagePath)
        {
            if (string.IsNullOrEmpty(packagePath))
                throw new ArgumentException("Package path required.", nameof(packagePath));

            GeneratableFile pkg = GeneratableFile.LoadFromFile(packagePath);

            IPackedFileDescriptor collPfd = pkg.Index.FirstOrDefault(p => p.Type == COLL);
            if (collPfd == null) return null;

            CollectionInfo info = new CollectionInfo
            {
                Instance = collPfd.Instance,
                Group = collPfd.Group,
            };

            ReadColl(pkg, collPfd, info);
            ReadStr(pkg, info);
            ReadImg(pkg, info);
            ReadMembers(pkg, info);

            return info;
        }

        static void ReadColl(IPackageFile pkg, IPackedFileDescriptor pfd, CollectionInfo info)
        {
            Cpf cpf = new Cpf();
            cpf.ProcessData(pfd, pkg);

            info.CreatorId = GetUInt(cpf, "creatorid");
            info.Scope = (CollectionScope)GetUInt(cpf, "flags");
        }

        // STR# pairs with COLL by sharing the collection's instance ID. Take
        // English line 0 if present, otherwise the first language we find —
        // works for unusual locale-stamped collections in the same way.
        static void ReadStr(IPackageFile pkg, CollectionInfo info)
        {
            IPackedFileDescriptor pfd = pkg.Index.FirstOrDefault(p =>
                p.Type == MetaData.STRING_FILE && p.Instance == info.Instance);
            if (pfd == null) return;

            Str str = new Str();
            str.ProcessData(pfd, pkg);

            // Try English first; fall back to any available language's line 0.
            StrItemList list = (StrItemList)str.Lines[(byte)MetaData.Languages.English];
            if (list == null || list.Count == 0)
            {
                foreach (DictionaryEntry de in str.Lines)
                {
                    var l = (StrItemList)de.Value;
                    if (l != null && l.Count > 0) { list = l; break; }
                }
            }
            if (list != null && list.Count > 0)
                info.Name = ((StrToken)list[0]).Title ?? string.Empty;
        }

        // IMG = thumbnail bytes (PNG when our writer produced it; could be
        // JPG/BMP/TGA on collections produced by JFade's original).
        // System.Drawing's Image.FromStream handles all three.
        static void ReadImg(IPackageFile pkg, CollectionInfo info)
        {
            IPackedFileDescriptor pfd = pkg.Index.FirstOrDefault(p =>
                p.Type == MetaData.SIM_IMAGE_FILE && p.Instance == info.Instance);
            if (pfd == null || pfd.UserData == null || pfd.UserData.Length == 0) return;

            try
            {
                using (var ms = new MemoryStream(pfd.UserData))
                    info.Thumbnail = Image.FromStream(ms);
            }
            catch
            {
                // Corrupted or unrecognised image format — leave thumbnail null.
                info.Thumbnail = null;
            }
        }

        // For each BINX, find the 3IDR with the same instance and read its
        // first reference array entry — that's the member object's TGI.
        // Order by Instance so the resulting member list matches the order
        // CollectionWriter assigns (Instance+1, Instance+2, ...).
        static void ReadMembers(IPackageFile pkg, CollectionInfo info)
        {
            var binxes = pkg.Index
                .Where(p => p.Type == MetaData.BINX)
                .OrderBy(p => p.Instance);

            foreach (var binxPfd in binxes)
            {
                IPackedFileDescriptor refPfd = pkg.Index.FirstOrDefault(p =>
                    p.Type == MetaData.REF_FILE && p.Instance == binxPfd.Instance);
                if (refPfd == null) continue;

                CollectionMember m = ParseFirst3IdrEntry(refPfd);
                if (m != null) info.Members.Add(m);
            }
        }

        // Parse the 3IDR header + first reference entry. Format (from
        // reference/collection-format-spec.md):
        //   uint32 magic = 0xDEADBEEF
        //   uint32 version (2)
        //   uint32 count
        //   N × (Type, Group, Instance, InstanceHi)  — 16 bytes each
        // We only care about entry 0; that's where BINX.objectidx points.
        static CollectionMember ParseFirst3IdrEntry(IPackedFileDescriptor pfd)
        {
            byte[] data = pfd.UserData;
            if (data == null || data.Length < 12 + 16) return null;

            using (var br = new BinaryReader(new MemoryStream(data)))
            {
                uint magic = br.ReadUInt32();
                br.ReadUInt32(); // version
                uint count = br.ReadUInt32();

                if (magic != 0xDEADBEEF || count == 0) return null;

                return new CollectionMember
                {
                    ObjectType = br.ReadUInt32(),
                    ObjectGroup = br.ReadUInt32(),
                    ObjectInstance = br.ReadUInt32(),
                    ObjectInstanceHi = br.ReadUInt32(),
                };
            }
        }

        static uint GetUInt(Cpf cpf, string name)
        {
            CpfItem item = cpf.GetItem(name);
            return item == null ? 0u : item.UIntegerValue;
        }
    }
}
