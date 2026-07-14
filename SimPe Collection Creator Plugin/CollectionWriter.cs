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
using System.Drawing.Imaging;
using System.IO;
using SimPe.Data;
using SimPe.Interfaces.Files;
using SimPe.Packages;
using SimPe.PackedFiles.Wrapper;

namespace SimPe.Plugin
{
    /// <summary>
    /// Serializes a <see cref="CollectionInfo"/> to a standalone Sims 2
    /// collection <c>.package</c>. Replaces the entire
    /// <c>MakeHeader/MakeIndex/MakeBINX/Make3IDR/CombineDats/Reverse/Fill</c>
    /// family in JFade's original (~800 lines of byte-shuffling through hex
    /// strings) by delegating to SimPE's existing DBPF + wrapper machinery.
    /// </summary>
    public static class CollectionWriter
    {
        // COLL — Sims 2 collection metadata (cGZPropertySetString XML).
        // Not in SimPE.Helper.MetaData yet; the constant lives here because
        // it's collection-specific and only this plugin reads/writes it.
        const uint COLL = 0x6C4F359D;

        // The fixed sortindex Maxis stamps on the COLL itself. JFade copied
        // this from sample Maxis collections; cross-confirmed from real
        // Aspyr-port samples while writing the format spec.
        const uint CollSortIndex = 0;

        /// <summary>
        /// Build a brand-new collection <c>.package</c> at the given path.
        /// Overwrites if the file exists.
        /// </summary>
        public static void Write(CollectionInfo coll, string outputPath)
        {
            if (coll == null) throw new ArgumentNullException(nameof(coll));
            if (string.IsNullOrEmpty(outputPath)) throw new ArgumentException("Output path required.", nameof(outputPath));

            // Start with an empty DBPF and add each resource in turn. SimPE's
            // GeneratableFile handles the v1.1 container layout, 20-byte index
            // entries, and QFS compression — none of JFade's hand-rolled
            // CombineDats sequence is needed.
            GeneratableFile pkg = GeneratableFile.LoadFromFile((string)null);

            WriteColl(pkg, coll);
            WriteStr(pkg, coll);
            WriteImg(pkg, coll);
            WriteMembers(pkg, coll);

            pkg.Save(outputPath);
        }

        // The COLL resource holds metadata only — name pointer, icon
        // pointer, creator id, scope flags. It does NOT list members; those
        // live in the BINX+3IDR pairs written by WriteMembers.
        static void WriteColl(GeneratableFile pkg, CollectionInfo coll)
        {
            Cpf cpf = new Cpf();
            AddUInt(cpf, "creatorid", coll.CreatorId);
            AddUInt(cpf, "flags", (uint)coll.Scope);

            // Icon pointer — every collection has exactly one IMG thumbnail
            // sharing the collection's instance + group.
            AddUInt(cpf, "iconid", coll.Instance);
            AddUInt(cpf, "icongroupid", coll.Group);
            AddUInt(cpf, "iconrestypeid", MetaData.SIM_IMAGE_FILE);

            AddUInt(cpf, "sortindex", CollSortIndex);

            // Name pointer — the STR# resource holds the user-visible name.
            // stringindex picks which line of the STR# (always 0 for collections).
            AddUInt(cpf, "stringindex", 0);
            AddUInt(cpf, "stringsetid", coll.Instance);
            AddUInt(cpf, "stringsetgroupid", coll.Group);
            AddUInt(cpf, "stringsetrestypeid", MetaData.STRING_FILE);

            AddString(cpf, "type", "collection");

            IPackedFileDescriptor pfd = pkg.NewDescriptor(COLL, 0, coll.Group, coll.Instance);
            cpf.Save(pfd);
            pkg.Add(pfd, true);
        }

        // STR# holds the collection's display name. One line, English-default.
        static void WriteStr(GeneratableFile pkg, CollectionInfo coll)
        {
            Str str = new Str();
            str.Add(new StrToken(0, (byte)MetaData.Languages.English, coll.Name ?? string.Empty, string.Empty));

            IPackedFileDescriptor pfd = pkg.NewDescriptor(MetaData.STRING_FILE, 0, coll.Group, coll.Instance);
            str.Save(pfd);
            pkg.Add(pfd, true);
        }

        // IMG = the collection's thumbnail. Optional; skip the resource if
        // the user didn't pick one (the catalog will fall back to a default).
        static void WriteImg(GeneratableFile pkg, CollectionInfo coll)
        {
            if (coll.Thumbnail == null) return;

            byte[] png;
            using (var ms = new MemoryStream())
            {
                coll.Thumbnail.Save(ms, ImageFormat.Png);
                png = ms.ToArray();
            }

            IPackedFileDescriptor pfd = pkg.NewDescriptor(MetaData.SIM_IMAGE_FILE, 0, coll.Group, coll.Instance);
            pfd.UserData = png;
            pkg.Add(pfd, true);
        }

        // One BINX + 3IDR pair per member. The two share an instance ID
        // (assigned sequentially starting at the collection's instance + 1).
        // The 3IDR's reference[0] is the real catalog object's TGI; the
        // BINX's "objectidx" property is 0 (pointing at index 0 of that
        // single-entry array). Icon/sort indices on the BINX are placeholder
        // zeros — JFade's original tool used the same approach.
        static void WriteMembers(GeneratableFile pkg, CollectionInfo coll)
        {
            uint memberInstance = coll.Instance + 1;
            for (int i = 0; i < coll.Members.Count; i++)
            {
                CollectionMember m = coll.Members[i];

                Write3IDR(pkg, coll.Group, memberInstance, m);
                WriteBinx(pkg, coll.Group, memberInstance, i);

                memberInstance++;
            }
        }

        // 3IDR (REF_FILE / 0xAC506764) — one entry pointing at the member
        // object's TGI. SimPE has a RefFile wrapper (SimPe.Plugin.RefFile)
        // but constructing one from scratch and forcing it to serialize a
        // single-entry array is more code than just emitting the binary
        // directly. The format is fixed and trivial:
        //   uint32 magic   = 0xDEADBEEF
        //   uint32 version = 2
        //   uint32 count   = N
        //   then N × 16 bytes (Type, Group, Instance, InstanceHi)
        static void Write3IDR(GeneratableFile pkg, uint group, uint instance, CollectionMember m)
        {
            byte[] data;
            using (var ms = new MemoryStream())
            using (var bw = new BinaryWriter(ms))
            {
                bw.Write((uint)0xDEADBEEF);
                bw.Write((uint)2);    // index version
                bw.Write((uint)1);    // entry count

                bw.Write(m.ObjectType);
                bw.Write(m.ObjectGroup);
                bw.Write(m.ObjectInstance);
                bw.Write(m.ObjectInstanceHi);

                data = ms.ToArray();
            }

            IPackedFileDescriptor pfd = pkg.NewDescriptor(MetaData.REF_FILE, 0, group, instance);
            pfd.UserData = data;
            pkg.Add(pfd, true);
        }

        // BINX (0x0C560F39) — cGZPropertySetString with the four uint index
        // fields that point into the paired 3IDR's reference array.
        static void WriteBinx(GeneratableFile pkg, uint group, uint instance, int memberIndex)
        {
            Cpf cpf = new Cpf();
            AddUInt(cpf, "binidx", 0);              // points at this BINX's slot
            AddUInt(cpf, "objectidx", 0);           // points at 3IDR[0] — the real object
            AddUInt(cpf, "iconidx", 0);             // no per-member icon
            AddUInt(cpf, "sortindex", (uint)memberIndex);

            IPackedFileDescriptor pfd = pkg.NewDescriptor(MetaData.BINX, 0, group, instance);
            cpf.Save(pfd);
            pkg.Add(pfd, true);
        }

        // --- small helpers ---------------------------------------------------

        static void AddUInt(Cpf cpf, string name, uint value)
        {
            CpfItem item = new CpfItem();
            item.Name = name;
            item.UIntegerValue = value;
            cpf.AddItem(item);
        }

        static void AddString(Cpf cpf, string name, string value)
        {
            CpfItem item = new CpfItem();
            item.Name = name;
            item.StringValue = value ?? string.Empty;
            cpf.AddItem(item);
        }
    }
}
