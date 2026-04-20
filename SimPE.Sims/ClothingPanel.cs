/***************************************************************************
 *   Copyright (C) 2005 by Ambertation                                     *
 *   quaxi@ambertation.de                                                  *
 *                                                                         *
 *   Copyright (C) 2025 by GramzeSweatshop                                 *
 *   rhiamom@mac.com                                                       *
 *                                                                         *
 *   This program is free software; you can redistribute it and/or modify  *
 *   it under the terms of the GNU General Public License as published by  *
 *   the Free Software Foundation; either version 2 of the License, or     *
 *   (at your option) any later version.                                   *
 ***************************************************************************/

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using SimPe.Data;
using SimPe.Interfaces.Files;
using SimPe.PackedFiles.Wrapper;
using SimPe.Plugin;

namespace SimPe.WardrobeViewer
{
    /// <summary>
    /// Read-only viewer for the wardrobe (clothing) that a sim's family owns. Embedded
    /// inside Sim Description as a sidebar tab (no longer a popup). Call SetSim(...) to
    /// (re)bind it to a sim; the heavy neighborhood scan only runs when the panel
    /// becomes visible. See memory/project_clothing_viewer.md for the data model.
    /// </summary>
    public sealed class ClothingPanel : UserControl
    {
        private const uint TYPE_3IDR = 0xAC506764;       // REF_FILE
        private const uint TYPE_COLL = 0x6C4F359D;       // Collection (family link)
        private const uint TYPE_GZPS = 0xEBCF3E27;       // Property Set (clothing)
        private const uint TYPE_XHTN = 0x8C1580B5;       // Hairtone
        private const uint TYPE_XMOL = 0x0C1FE246;       // Mesh Overlay (accessories)
        private const uint TYPE_BINX = 0x0C560F39;       // Binary Index

        private ExtSDesc sdesc;
        private bool dirty = true;                       // needs (re)scan on next visibility
        private readonly List<WardrobeEntry> entries = new();
        private readonly TreeView tree;
        private readonly ListView list;
        private readonly Label header;
        private readonly Label status;

        public ClothingPanel()
        {
            Dock = DockStyle.Fill;

            header = new Label
            {
                Dock = DockStyle.Top,
                Height = 44,
                Padding = new Padding(12, 8, 12, 4),
                Font = new System.Drawing.Font(Font.FontFamily, 10f, System.Drawing.FontStyle.Bold),
                AutoSize = false,
                TextAlign = System.Drawing.ContentAlignment.MiddleLeft
            };

            // Age-filter toolbar — defaults ON: family wardrobes are shared, but the
            // user almost always wants to see just what THIS sim can wear.
            cbAgeFilter = new CheckBox
            {
                Dock = DockStyle.Top,
                Height = 28,
                Padding = new Padding(12, 4, 12, 4),
                Checked = true,
                Text = "Show only outfits for this sim's age",
                AutoSize = false
            };
            cbAgeFilter.CheckedChanged += (s, e) => RefreshTreeAndList();

            cbGenderFilter = new CheckBox
            {
                Dock = DockStyle.Top,
                Height = 28,
                Padding = new Padding(12, 4, 12, 4),
                Checked = true,
                Text = "Show only outfits for this sim's gender",
                AutoSize = false
            };
            cbGenderFilter.CheckedChanged += (s, e) => RefreshTreeAndList();

            status = new Label
            {
                Dock = DockStyle.Bottom,
                Height = 24,
                Padding = new Padding(12, 4, 12, 4),
                TextAlign = System.Drawing.ContentAlignment.MiddleLeft,
                BorderStyle = BorderStyle.Fixed3D
            };

            var split = new SplitContainer
            {
                Dock = DockStyle.Fill,
                SplitterDistance = 240,
                FixedPanel = FixedPanel.Panel1
            };

            tree = new TreeView
            {
                Dock = DockStyle.Fill,
                HideSelection = false,
                ShowLines = true,
                ShowPlusMinus = true,
                ShowRootLines = true
            };
            tree.AfterSelect += Tree_AfterSelect;
            split.Panel1.Controls.Add(tree);

            list = new ListView
            {
                Dock = DockStyle.Fill,
                View = View.Details,
                FullRowSelect = true,
                GridLines = true,
                MultiSelect = false
            };
            list.Columns.Add("Name", 280);
            list.Columns.Add("Category", 110);
            list.Columns.Add("Age", 110);
            list.Columns.Add("Gender", 70);
            list.Columns.Add("GZPS Instance", 110);
            list.Columns.Add("Source package", 240);
            split.Panel2.Controls.Add(list);

            Controls.Add(split);
            Controls.Add(status);
            // Docked top controls stack in REVERSE order of Controls.Add, so add
            // bottom-first: gender (lowest) → age → header (top).
            Controls.Add(cbGenderFilter);
            Controls.Add(cbAgeFilter);
            Controls.Add(header);

            // Lazy load: only scan the neighborhood package when the user actually
            // switches to this tab (and only once per sim).
            VisibleChanged += (s, e) =>
            {
                if (Visible && dirty && sdesc != null)
                {
                    dirty = false;
                    Populate();
                }
            };
        }

        /// <summary>
        /// (Re)bind the panel to a sim. The actual neighborhood scan is deferred until
        /// the panel becomes visible.
        /// </summary>
        public void SetSim(ExtSDesc newSdesc)
        {
            sdesc = newSdesc;
            dirty = true;
            entries.Clear();
            tree.Nodes.Clear();
            list.Items.Clear();
            header.Text = "";
            status.Text = "";

            // If we're already visible (user is currently on this tab), repopulate now.
            if (Visible && sdesc != null)
            {
                dirty = false;
                Populate();
            }
        }

        // ------------------------------------------------------------
        //   Data model
        // ------------------------------------------------------------
        private sealed class WardrobeEntry
        {
            public OutfitCats Category;
            public string CategoryName = "";
            public string Name = "";
            public uint AgeBits;                 // raw GZPS "age" bitmask
            public string Age = "";
            public uint GenderBits;              // raw GZPS "gender" bitmask (Sex enum)
            public string Gender = "";
            public uint GzpsInstance;
            public string SourcePackage = "";
        }

        private uint simAgeBit;                  // 0 = no filter
        private uint simGenderBit;               // 0 = no filter
        private CheckBox cbAgeFilter;
        private CheckBox cbGenderFilter;

        // ------------------------------------------------------------
        //   Population
        // ------------------------------------------------------------
        private void Populate()
        {
            try
            {
                // Sim's life section -> Ages bitmask. Used to filter the family-shared
                // wardrobe down to outfits this sim can actually wear.
                try
                {
                    var lifeSection = sdesc.CharacterDescription.LifeSection;
                    simAgeBit = (uint)Data.MetaData.AgeTranslation(lifeSection);
                }
                catch { simAgeBit = 0; }

                cbAgeFilter.Text = simAgeBit != 0
                    ? "Show only outfits for this sim's age (" + ((Ages)simAgeBit) + ")"
                    : "Show only outfits for this sim's age (unknown)";
                cbAgeFilter.Enabled = simAgeBit != 0;

                // Sim's gender -> GZPS "gender" bitmask (Sex enum, different from the
                // SDesc MetaData.Gender that's Male=0 / Female=1). Outfits tagged with
                // gender=0 are unisex (common for child/toddler) and stay visible.
                try
                {
                    var g = sdesc.CharacterDescription.Gender;
                    simGenderBit = g == Data.MetaData.Gender.Female
                        ? (uint)Sex.Female   // 0x01
                        : (uint)Sex.Male;    // 0x02
                }
                catch { simGenderBit = 0; }

                cbGenderFilter.Text = simGenderBit != 0
                    ? "Show only outfits for this sim's gender (" + ((Sex)simGenderBit) + ")"
                    : "Show only outfits for this sim's gender (unknown)";
                cbGenderFilter.Enabled = simGenderBit != 0;

                string hoodPath = TryFindNeighborhoodPackage(sdesc);
                header.Text = "Family #0x" + Helper.HexString(sdesc.FamilyInstance)
                    + (hoodPath != null ? "   (" + Path.GetFileName(hoodPath) + ")" : "");

                if (hoodPath == null)
                {
                    status.Text = "Neighborhood package not found — cannot scan wardrobe.";
                    return;
                }

                SimPe.Packages.GeneratableFile hood;
                try { hood = SimPe.Packages.GeneratableFile.LoadFromFile(hoodPath); }
                catch (Exception ex)
                {
                    status.Text = "Could not open neighborhood package: " + ex.Message;
                    return;
                }

                ScanWardrobe(hood, sdesc.FamilyInstance, entries);

                if (entries.Count == 0)
                {
                    status.Text = "No wardrobe entries found for this family.";
                    return;
                }

                RefreshTreeAndList();
            }
            catch (Exception ex)
            {
                status.Text = "Error: " + ex.Message;
            }
        }

        private IEnumerable<WardrobeEntry> VisibleEntries()
        {
            IEnumerable<WardrobeEntry> q = entries;
            if (cbAgeFilter != null && cbAgeFilter.Checked && simAgeBit != 0)
                q = q.Where(e => e.AgeBits == 0 || (e.AgeBits & simAgeBit) != 0);
            if (cbGenderFilter != null && cbGenderFilter.Checked && simGenderBit != 0)
                q = q.Where(e => e.GenderBits == 0 || (e.GenderBits & simGenderBit) != 0);
            return q;
        }

        private void RefreshTreeAndList()
        {
            var visible = VisibleEntries().ToList();

            // Remember the previously selected category so the user doesn't lose context
            // when toggling the age filter.
            string prevCat = tree.SelectedNode?.Tag as string;

            var byCat = visible
                .GroupBy(e => e.CategoryName)
                .OrderBy(g => g.Key)
                .ToList();

            tree.BeginUpdate();
            tree.Nodes.Clear();
            var all = new TreeNode("All (" + visible.Count + ")") { Tag = null };
            tree.Nodes.Add(all);
            TreeNode toSelect = all;
            foreach (var grp in byCat)
            {
                var node = new TreeNode(grp.Key + " (" + grp.Count() + ")") { Tag = grp.Key };
                tree.Nodes.Add(node);
                if (grp.Key == prevCat) toSelect = node;
            }
            tree.SelectedNode = toSelect;
            tree.EndUpdate();

            ShowCategory(toSelect.Tag as string);

            var filterParts = new List<string>();
            if (cbAgeFilter.Checked && simAgeBit != 0)
                filterParts.Add(((Ages)simAgeBit).ToString());
            if (cbGenderFilter.Checked && simGenderBit != 0)
                filterParts.Add(((Sex)simGenderBit).ToString());
            string filterNote = filterParts.Count > 0
                ? " (filtered to " + string.Join(" + ", filterParts) + ")"
                : "";
            int totalShown = visible.Count;
            int totalAll = entries.Count;
            string countSuffix = totalShown == totalAll
                ? totalAll + " outfit(s)"
                : totalShown + " of " + totalAll + " outfit(s)";
            status.Text = countSuffix + filterNote + ". "
                + byCat.Count + " categor" + (byCat.Count == 1 ? "y" : "ies") + ".";
        }

        private void Tree_AfterSelect(object sender, TreeViewEventArgs e)
        {
            ShowCategory(e.Node?.Tag as string);
        }

        private void ShowCategory(string categoryFilter)
        {
            list.BeginUpdate();
            list.Items.Clear();
            IEnumerable<WardrobeEntry> filtered = VisibleEntries();
            if (categoryFilter != null)
                filtered = filtered.Where(x => x.CategoryName == categoryFilter);
            foreach (var entry in filtered.OrderBy(x => x.Name))
            {
                var lvi = new ListViewItem(entry.Name);
                lvi.SubItems.Add(entry.CategoryName);
                lvi.SubItems.Add(entry.Age);
                lvi.SubItems.Add(entry.Gender);
                lvi.SubItems.Add("0x" + Helper.HexString(entry.GzpsInstance));
                lvi.SubItems.Add(entry.SourcePackage);
                list.Items.Add(lvi);
            }
            list.EndUpdate();
        }

        // ------------------------------------------------------------
        //   Neighborhood locator
        // ------------------------------------------------------------
        /// <summary>
        /// From a character-file path, walk up to find the Nxxx_Neighborhood.package.
        /// Character files live in {hood}/Characters/{hood}_UserNNNNN.package, so one
        /// directory up and look for a file ending in "_Neighborhood.package".
        /// </summary>
        private static string TryFindNeighborhoodPackage(ExtSDesc sdesc)
        {
            // If the loaded Sdesc package IS the neighborhood, use it.
            string simPkgPath = sdesc.Package?.FileName;
            if (!string.IsNullOrEmpty(simPkgPath)
                && simPkgPath.EndsWith("_Neighborhood.package", StringComparison.OrdinalIgnoreCase))
                return simPkgPath;

            // Otherwise look at the sim's character file location.
            string charPath = sdesc.CharacterFileName;
            if (string.IsNullOrEmpty(charPath) || !File.Exists(charPath))
                charPath = simPkgPath; // best-effort fallback

            if (string.IsNullOrEmpty(charPath)) return null;

            string charDir = Path.GetDirectoryName(charPath);
            if (string.IsNullOrEmpty(charDir)) return null;

            // Typical layout: <hood>\Characters\<hood>_User00005.package
            // Look in charDir itself first (rare), then parent directory.
            string candidate = FindHoodInDir(charDir);
            if (candidate != null) return candidate;

            var parent = Directory.GetParent(charDir);
            if (parent != null)
            {
                candidate = FindHoodInDir(parent.FullName);
                if (candidate != null) return candidate;
            }

            return null;
        }

        private static string FindHoodInDir(string dir)
        {
            if (!Directory.Exists(dir)) return null;
            foreach (var f in Directory.EnumerateFiles(dir, "*_Neighborhood.package"))
                return f;
            return null;
        }

        // ------------------------------------------------------------
        //   Wardrobe scan (ported from WOSimPe WardrobeCleaner)
        // ------------------------------------------------------------
        private static void ScanWardrobe(IPackageFile hood, uint familyInstance, List<WardrobeEntry> outList)
        {
            outList.Clear();

            IPackedFileDescriptor[] idrs = hood.FindFiles(TYPE_3IDR);
            foreach (var pfd in idrs)
            {
                // User-owned outfits live at instance >= 0x7fff; lower is system/reserved.
                if (pfd.Instance < 0x7fff) continue;

                RefFile idr = new RefFile();
                try { idr.ProcessData(pfd, hood, false); }
                catch { continue; }

                if (idr.Items == null || idr.Items.Length != 3) continue;

                // Items[1] must be the family-collection ref matching this sim's family.
                var coll = idr.Items[1];
                if (coll == null || coll.Type != TYPE_COLL) continue;
                if (coll.Instance != familyInstance) continue;

                // Only include 3IDRs that point to a GZPS Property Set (clothing).
                // Hair (XHTN), accessories (XMOL), makeup (XTOL), and skin (XSTN)
                // are intentionally excluded — that's Bodyshop territory.
                IPackedFileDescriptor meta = null;
                foreach (var itm in idr.Items)
                {
                    if (itm == null) continue;
                    if (itm.Type == TYPE_GZPS) { meta = itm; break; }
                }
                if (meta == null) continue;

                WardrobeEntry entry = new WardrobeEntry
                {
                    GzpsInstance = meta.Instance
                };

                Cpf cpf = TryLoadCpf(meta, hood, out string sourcePath);
                if (cpf != null)
                {
                    entry.Name = cpf.GetSaveItem("name").StringValue ?? "";
                    uint catBits = SafeUInt(cpf.GetSaveItem("category"));
                    entry.Category = (OutfitCats)catBits;
                    entry.CategoryName = FormatCategories(catBits);
                    uint ageBits = SafeUInt(cpf.GetSaveItem("age"));
                    entry.AgeBits = ageBits;
                    entry.Age = FormatAges(ageBits);
                    uint genderBits = SafeUInt(cpf.GetSaveItem("gender"));
                    entry.GenderBits = genderBits;
                    entry.Gender = FormatGender(genderBits);
                    entry.SourcePackage = sourcePath ?? "";
                }
                else
                {
                    entry.Name = "(unknown — clothing package not installed)";
                    entry.CategoryName = "Unknown";
                }

                outList.Add(entry);
            }
        }

        private static uint SafeUInt(CpfItem item)
        {
            try { return item?.UIntegerValue ?? 0; }
            catch { return 0; }
        }

        private static Cpf TryLoadCpf(IPackedFileDescriptor target, IPackageFile hood, out string sourcePath)
        {
            sourcePath = null;
            // First try the hood itself.
            var local = hood.FindFile(target.Type, target.SubType, target.Group, target.Instance);
            if (local != null)
            {
                try
                {
                    Cpf c = new Cpf();
                    c.ProcessData(local, hood, false);
                    sourcePath = Path.GetFileName(hood.FileName ?? "");
                    return c;
                }
                catch { }
            }

            // Then fall back to SimPe.FileTable.FileIndex so we can resolve outfits that
            // live in EP / Downloads packages (the same index PJSE's FileTable wraps).
            try
            {
                var hits = SimPe.FileTable.FileIndex.FindFileByGroupAndInstance(
                    target.Group, target.LongInstance);
                if (hits != null)
                {
                    foreach (var fii in hits)
                    {
                        var pf = fii.FileDescriptor;
                        if (pf.Type != target.Type) continue;
                        try
                        {
                            Cpf c = new Cpf();
                            c.ProcessData(pf, fii.Package, false);
                            sourcePath = Path.GetFileName(fii.Package?.FileName ?? "");
                            return c;
                        }
                        catch { }
                    }
                }
            }
            catch { }

            return null;
        }

        // ------------------------------------------------------------
        //   Pretty-printers
        // ------------------------------------------------------------
        private static string FormatCategories(uint bits)
        {
            if (bits == 0) return "Unknown";
            var parts = new List<string>();
            foreach (OutfitCats c in Enum.GetValues(typeof(OutfitCats)))
            {
                uint v = (uint)c;
                if (v != 0 && (bits & v) == v) parts.Add(c.ToString());
            }
            return parts.Count == 0 ? ("0x" + Helper.HexString(bits)) : string.Join(", ", parts);
        }

        private static string FormatAges(uint bits)
        {
            if (bits == 0) return "";
            var parts = new List<string>();
            foreach (Ages a in Enum.GetValues(typeof(Ages)))
            {
                uint v = (uint)a;
                if (v != 0 && (bits & v) == v) parts.Add(a.ToString());
            }
            return parts.Count == 0 ? ("0x" + Helper.HexString(bits)) : string.Join(", ", parts);
        }

        private static string FormatGender(uint bits)
        {
            if (bits == 0) return "";
            var parts = new List<string>();
            foreach (Sex s in Enum.GetValues(typeof(Sex)))
            {
                uint v = (uint)s;
                if (v != 0 && (bits & v) == v) parts.Add(s.ToString());
            }
            return parts.Count == 0 ? ("0x" + Helper.HexString(bits)) : string.Join(", ", parts);
        }
    }
}
