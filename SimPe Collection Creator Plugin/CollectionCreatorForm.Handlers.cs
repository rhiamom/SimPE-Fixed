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
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace SimPe.Plugin
{
    // Event handlers partial. Control names match JFade's original
    // (Command1, cmdMakeNewColl, etc.) so anyone cross-referencing the
    // decompiled source can map this file to the corresponding click
    // method in frmMain.cs. Actual work delegates into CollectionWriter /
    // CollectionReader / ObjectCatalog from Pass 1.
    internal partial class CollectionCreatorForm
    {
        // Re-entry guard for programmatic field updates so TextChanged
        // handlers don't bounce values back into `current` mid-load.
        bool loadingUI;

        void WireHandlers()
        {
            // --- Top row (main mode) -------------------------------
            cmdMakeNewColl.Click += CmdMakeNewColl_Click;
            cmdEditColl.Click    += CmdEditColl_Click;
            cmdBackUpColl.Click  += CmdBackUpColl_Click;
            cmdAlphaSort.Click   += CmdAlphaSort_Click;
            cmdOptions.Click     += (s, e) => EnterMode(UIMode.Options);
            cmdAbout.Click       += CmdAbout_Click;

            // --- Bottom row + add/save -----------------------------
            cmdExit.Click        += (s, e) => Close();
            cmdSaveColl.Click    += CmdSaveColl_Click;
            Command1.Click       += Command1_Click;    // Add Object
            cmdBatchAdd.Click    += CmdBatchAdd_Click;

            // --- Metadata edits ------------------------------------
            cmdLoadPic.Click     += CmdLoadPic_Click;
            txtCollName.TextChanged       += TxtCollName_TextChanged;
            cmbCollType.SelectedIndexChanged += CmbCollType_Changed;

            // --- Item list reorder ---------------------------------
            lstListOfItems.SelectedIndexChanged += (s, e) => UpdateUIState();
            cmdMoveUp.Click      += CmdMoveUp_Click;
            cmdMoveDown.Click    += CmdMoveDown_Click;
            cmdRemoveItem.Click  += CmdRemoveItem_Click;

            // --- Options mode buttons ------------------------------
            cmdCloseOptions.Click += CmdCloseOptions_Click;
            cmdFindCollDir.Click  += (s, e) => PickFolderInto(txtCollDir);
            cmdFindThumbDir.Click += (s, e) => PickFolderInto(txtThumbDir);

            // --- Batch Add mode buttons ----------------------------
            cmdBatchAddUp.Click     += CmdBatchAddUp_Click;
            cmdBatchAddDown.Click   += CmdBatchAddDown_Click;
            cmdBatchAddRemove.Click += CmdBatchAddRemove_Click;
            cmdFinishBatchAdd.Click += CmdFinishBatchAdd_Click;
            cmdCancelBatchAdd.Click += CmdCancelBatchAdd_Click;
            lstBatchAdd.SelectedIndexChanged += LstBatchAdd_SelectedIndexChanged;

            // --- Add Item Details mode buttons (placeholders) ------
            cmdAddItem.Click += (s, e) => EnterMode(UIMode.Main);
            cmdCancel.Click  += (s, e) => EnterMode(UIMode.Main);
        }

        // --- Top-level actions: New / Open / Save / Backup / Sort --

        void CmdMakeNewColl_Click(object sender, EventArgs e)
        {
            if (!ConfirmDiscardChanges()) return;

            var info = new CollectionInfo
            {
                Instance = (uint)new Random().Next(0x1000, 0xFFFE),
                Name = "Untitled collection",
            };
            currentPath = null;
            SetCurrent(info);
            SetStatus("New collection — add objects, set a name, then Save.");
        }

        void CmdEditColl_Click(object sender, EventArgs e)
        {
            if (!ConfirmDiscardChanges()) return;
            if (dlgOpenCollection.ShowDialog(this) != DialogResult.OK) return;

            try
            {
                var info = CollectionReader.Read(dlgOpenCollection.FileName);
                if (info == null)
                {
                    ShowError("Not a collection",
                        "The selected file doesn't contain a COLL resource. Pick a collection from your " +
                        "Documents\\EA Games\\The Sims 2…\\Collections folder, not a plain object package.");
                    return;
                }
                currentPath = dlgOpenCollection.FileName;
                SetCurrent(info);
                SetStatus($"Loaded {Path.GetFileName(currentPath)} — {info.Members.Count} member(s).");
            }
            catch (Exception ex)
            {
                ShowError("Couldn't read collection", ex.Message);
            }
        }

        void CmdSaveColl_Click(object sender, EventArgs e)
        {
            if (current == null) return;
            ReadCollectionFromUI();

            string outputPath = currentPath;
            if (outputPath == null)
            {
                dlgSaveCollection.FileName = SuggestFilename(current.Name);
                if (!string.IsNullOrEmpty(txtCollDir.Text) && Directory.Exists(txtCollDir.Text))
                    dlgSaveCollection.InitialDirectory = txtCollDir.Text;
                if (dlgSaveCollection.ShowDialog(this) != DialogResult.OK) return;
                outputPath = dlgSaveCollection.FileName;
            }

            try
            {
                CollectionWriter.Write(current, outputPath);
                currentPath = outputPath;
                SetStatus($"Saved {Path.GetFileName(outputPath)}.");
            }
            catch (Exception ex)
            {
                ShowError("Couldn't save collection", ex.Message);
            }
        }

        void CmdBackUpColl_Click(object sender, EventArgs e)
        {
            if (currentPath == null)
            {
                ShowError("Nothing to back up", "Open a saved collection first, then click Backup.");
                return;
            }
            string backup = currentPath + ".bak";
            try
            {
                File.Copy(currentPath, backup, overwrite: true);
                SetStatus($"Backed up to {Path.GetFileName(backup)}.");
            }
            catch (Exception ex)
            {
                ShowError("Couldn't make backup", ex.Message);
            }
        }

        void CmdAlphaSort_Click(object sender, EventArgs e)
        {
            if (current == null || current.Members.Count < 2) return;
            var sorted = current.Members
                .OrderBy(m => string.IsNullOrEmpty(m.DisplayName) ? "￿" : m.DisplayName,
                         StringComparer.OrdinalIgnoreCase)
                .ToList();
            current.Members.Clear();
            current.Members.AddRange(sorted);
            RefreshMemberList();
            SetStatus("Sorted A–Z.");
        }

        // --- About ------------------------------------------------

        void CmdAbout_Click(object sender, EventArgs e)
        {
            string text =
                "Sims 2 Collection Creator\r\n\r\n" +
                "Originally written by JFade — © 2006-2007 DJS Sims / The Sims Programming Group.\r\n\r\n" +
                "Ported as a SimPE plugin by GramzeSweatshop, 2026, with the original author's " +
                "permission (granted 2026-06-26).\r\n\r\n" +
                "JFade's original user manual ships in this plugin's data folder " +
                "(CollectionCreatorManual.pdf).\r\n\r\n" +
                "Plugin source: github.com/rhiamom/SimPE-Fixed";

            MessageBox.Show(this, text, "About JFade's Collection Creator",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        // --- Add Object (Command1) ---------------------------------
        // Pass 2 keeps this as a direct add (file picker → ObjectCatalog
        // → append to list) rather than entering GroupBox4 AddItem-preview
        // mode. AddItem-preview UX is reserved for Pass 4 polish.
        void Command1_Click(object sender, EventArgs e)
        {
            if (current == null || dlgAddObject.ShowDialog(this) != DialogResult.OK) return;

            string nameTable = dataFolder != null
                ? Path.Combine(dataFolder, "MaxisObjectList.txt")
                : null;

            int added = 0;
            var recolors = new System.Collections.Generic.List<string>();
            var unknowns = new System.Collections.Generic.List<string>();

            foreach (string path in dlgAddObject.FileNames)
            {
                try
                {
                    var infos = ObjectCatalog.Read(path, nameTable);
                    if (infos.Count > 0)
                    {
                        // Add every OBJD the package carries — a multi-tile
                        // window/door set is often 3-8 OBJDs, and the Maxis
                        // catalog merges (globalCatbundle.package etc) carry
                        // thousands. Dropping all-but-the-first would silently
                        // throw away most of the file's contents.
                        string basename = Path.GetFileNameWithoutExtension(path).ToLowerInvariant();
                        foreach (var info in infos)
                        {
                            current.Members.Add(new CollectionMember
                            {
                                ObjectType = info.ObjectType,
                                ObjectGroup = info.ObjectGroup,
                                ObjectInstance = info.ObjectInstance,
                                ObjectInstanceHi = info.ObjectInstanceHi,
                                Guid = info.Guid,
                                DisplayName = info.DisplayName,
                                SourceBasename = basename,
                            });
                            added++;
                        }
                    }
                    else
                    {
                        // Sort the no-OBJD skips into recolors vs unknowns so
                        // the user gets a meaningful explanation instead of a
                        // bare "no OBJD" — recolors are the common case and
                        // the constraint is the game's, not the tool's.
                        switch (ObjectCatalog.Classify(path))
                        {
                            case PackageKind.Recolor: recolors.Add(Path.GetFileName(path)); break;
                            default:                  unknowns.Add(Path.GetFileName(path)); break;
                        }
                    }
                }
                catch
                {
                    unknowns.Add(Path.GetFileName(path));
                }
            }

            RefreshMemberList();
            UpdateUIState();

            // Friendly explanation for the recolor case — the constraint catches
            // people often enough that a status-bar line isn't enough. Pop a
            // dialog the first time it happens in a multi-select; status bar
            // still gets a summary.
            if (recolors.Count > 0)
            {
                string list = string.Join("\r\n  • ",
                    new[] { "" }.Concat(recolors.Take(8)).ToArray());
                if (recolors.Count > 8) list += $"\r\n  …and {recolors.Count - 8} more";

                MessageBox.Show(this,
                    $"{recolors.Count} of the files you picked are recolors, not objects:" + list + "\r\n\r\n" +
                    "Sims 2 catalog collections are keyed by the original object's GUID — the game's catalog " +
                    "tile shows ONE object, and the recolor swatches under it are picked at runtime, not stored " +
                    "in the collection.\r\n\r\n" +
                    "To include the brown variant of (e.g.) a sofa, add the ORIGINAL sofa package; the collection " +
                    "will show it, and players pick the brown recolor from its in-game recolor swatches.\r\n\r\n" +
                    "If you want a tile dedicated to the brown recolor specifically, you'd need to clone the " +
                    "object in SimPE's Object Workshop (giving it a new GUID) and bind the brown recolor to the " +
                    "clone — that's a separate workflow, not something this tool can do at collection-build time.",
                    "Recolors can't be added directly",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            string status = $"Added {added} object(s)";
            if (recolors.Count > 0) status += $"; skipped {recolors.Count} recolor(s)";
            if (unknowns.Count > 0) status += $"; skipped {unknowns.Count} unrecognised";
            SetStatus(status + ".");
        }

        // --- Item list reorder -------------------------------------

        void CmdRemoveItem_Click(object sender, EventArgs e)
        {
            int i = lstListOfItems.SelectedIndex;
            if (current == null || i < 0) return;
            current.Members.RemoveAt(i);
            RefreshMemberList();
            if (current.Members.Count > 0)
                lstListOfItems.SelectedIndex = Math.Min(i, current.Members.Count - 1);
            UpdateUIState();
        }

        void CmdMoveUp_Click(object sender, EventArgs e)
        {
            int i = lstListOfItems.SelectedIndex;
            if (current == null || i <= 0) return;
            (current.Members[i - 1], current.Members[i]) = (current.Members[i], current.Members[i - 1]);
            RefreshMemberList();
            lstListOfItems.SelectedIndex = i - 1;
        }

        void CmdMoveDown_Click(object sender, EventArgs e)
        {
            int i = lstListOfItems.SelectedIndex;
            if (current == null || i < 0 || i >= current.Members.Count - 1) return;
            (current.Members[i + 1], current.Members[i]) = (current.Members[i], current.Members[i + 1]);
            RefreshMemberList();
            lstListOfItems.SelectedIndex = i + 1;
        }

        // --- Batch Add ---------------------------------------------
        // JFade's flow: pick a folder, recursively scan every .package
        // under it, stage one row per OBJD in a preview list, let the user
        // prune/reorder, then commit the whole batch into the collection.

        readonly List<CollectionMember> pendingBatchAdd = new List<CollectionMember>();

        void CmdBatchAdd_Click(object sender, EventArgs e)
        {
            if (current == null) return;

            dlgPickFolder.Description = "Choose a folder of .package files to batch-add";
            if (dlgPickFolder.ShowDialog(this) != DialogResult.OK) return;
            string folder = dlgPickFolder.SelectedPath;
            if (string.IsNullOrEmpty(folder) || !Directory.Exists(folder)) return;

            string nameTable = dataFolder != null
                ? Path.Combine(dataFolder, "MaxisObjectList.txt")
                : null;

            pendingBatchAdd.Clear();
            int objects = 0, recolors = 0, unknowns = 0, seen = 0;

            // SearchOption.AllDirectories throws and bails the entire walk
            // on the first inaccessible subfolder it hits — Windows Downloads
            // often has leftover OneDrive metadata, junctions, or restricted
            // system folders that would silently truncate the recursion to
            // zero results. EnumerationOptions with IgnoreInaccessible skips
            // those and keeps walking; AttributesToSkip = 0 also picks up
            // hidden/system folders that the default would have excluded.
            var enumOpts = new EnumerationOptions
            {
                RecurseSubdirectories = true,
                IgnoreInaccessible    = true,
                AttributesToSkip      = 0,
            };

            Cursor.Current = Cursors.WaitCursor;
            try
            {
                foreach (string path in Directory.EnumerateFiles(folder, "*.package", enumOpts))
                {
                    seen++;
                    try
                    {
                        var infos = ObjectCatalog.Read(path, nameTable);
                        if (infos.Count > 0)
                        {
                            string basename = Path.GetFileNameWithoutExtension(path).ToLowerInvariant();
                            foreach (var info in infos)
                            {
                                pendingBatchAdd.Add(new CollectionMember
                                {
                                    ObjectType = info.ObjectType,
                                    ObjectGroup = info.ObjectGroup,
                                    ObjectInstance = info.ObjectInstance,
                                    ObjectInstanceHi = info.ObjectInstanceHi,
                                    Guid = info.Guid,
                                    DisplayName = info.DisplayName,
                                    SourceBasename = basename,
                                });
                                objects++;
                            }
                        }
                        else if (ObjectCatalog.Classify(path) == PackageKind.Recolor) recolors++;
                        else unknowns++;
                    }
                    catch
                    {
                        unknowns++;
                    }
                }
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }

            RefreshBatchList();
            EnterMode(UIMode.BatchAdd);
            SetStatus($"Scanned {folder} — saw {seen} .package file(s): {objects} object(s) queued, {recolors} recolor(s), {unknowns} unrecognised.");
        }

        void CmdBatchAddRemove_Click(object sender, EventArgs e)
        {
            int i = lstBatchAdd.SelectedIndex;
            if (i < 0 || i >= pendingBatchAdd.Count) return;
            pendingBatchAdd.RemoveAt(i);
            RefreshBatchList();
            if (pendingBatchAdd.Count > 0)
                lstBatchAdd.SelectedIndex = Math.Min(i, pendingBatchAdd.Count - 1);
        }

        void CmdBatchAddUp_Click(object sender, EventArgs e)
        {
            int i = lstBatchAdd.SelectedIndex;
            if (i <= 0) return;
            (pendingBatchAdd[i - 1], pendingBatchAdd[i]) = (pendingBatchAdd[i], pendingBatchAdd[i - 1]);
            RefreshBatchList();
            lstBatchAdd.SelectedIndex = i - 1;
        }

        void CmdBatchAddDown_Click(object sender, EventArgs e)
        {
            int i = lstBatchAdd.SelectedIndex;
            if (i < 0 || i >= pendingBatchAdd.Count - 1) return;
            (pendingBatchAdd[i + 1], pendingBatchAdd[i]) = (pendingBatchAdd[i], pendingBatchAdd[i + 1]);
            RefreshBatchList();
            lstBatchAdd.SelectedIndex = i + 1;
        }

        void CmdFinishBatchAdd_Click(object sender, EventArgs e)
        {
            if (current != null && pendingBatchAdd.Count > 0)
            {
                int n = pendingBatchAdd.Count;
                current.Members.AddRange(pendingBatchAdd);
                pendingBatchAdd.Clear();
                RefreshMemberList();
                SetStatus($"Added {n} object(s) from batch.");
            }
            EnterMode(UIMode.Main);
        }

        void CmdCancelBatchAdd_Click(object sender, EventArgs e)
        {
            pendingBatchAdd.Clear();
            EnterMode(UIMode.Main);
            SetStatus("Batch add canceled.");
        }

        void RefreshBatchList()
        {
            lstBatchAdd.BeginUpdate();
            try
            {
                lstBatchAdd.Items.Clear();
                foreach (var m in pendingBatchAdd)
                {
                    string label = !string.IsNullOrEmpty(m.DisplayName)
                        ? m.DisplayName
                        : $"GUID 0x{m.Guid:X8}";
                    lstBatchAdd.Items.Add(label);
                }
            }
            finally
            {
                lstBatchAdd.EndUpdate();
            }
            if (lblBatchAddTotal != null)
                lblBatchAddTotal.Text = $"Total Items: {pendingBatchAdd.Count}";
        }

        // Selection → preview thumbnail of the highlighted pending item.
        // Caches the per-member Image on first successful load so flipping
        // back and forth doesn't pay the ObjectThumbnails.package lookup
        // cost more than once per item. Empty PictureBox when the item
        // has no modelname or the lookup returns nothing.
        void LstBatchAdd_SelectedIndexChanged(object sender, EventArgs e)
        {
            int i = lstBatchAdd.SelectedIndex;
            if (i < 0 || i >= pendingBatchAdd.Count) { PictureBox2.Image = null; return; }

            var member = pendingBatchAdd[i];
            if (member.Thumbnail == null)
            {
                member.Thumbnail = ObjectThumbnailLoader.GetThumbnail(
                    txtThumbDir?.Text, member.SourceBasename,
                    out string diag);

                // Surface the specific failure step in the status bar so
                // the user can tell whether it's a config problem (folder
                // unset, package missing) or a genuine cache miss.
                if (member.Thumbnail == null && !string.IsNullOrWhiteSpace(diag))
                    SetStatus($"No thumbnail: {diag}");
            }
            PictureBox2.Image = member.Thumbnail;
        }

        // --- Metadata edits ----------------------------------------

        void TxtCollName_TextChanged(object sender, EventArgs e)
        {
            if (loadingUI || current == null) return;
            current.Name = txtCollName.Text;
        }

        void CmbCollType_Changed(object sender, EventArgs e)
        {
            if (loadingUI || current == null) return;
            current.Scope = (CollectionScope)cmbCollType.SelectedIndex;
        }

        void CmdLoadPic_Click(object sender, EventArgs e)
        {
            if (current == null || dlgPickThumbnail.ShowDialog(this) != DialogResult.OK) return;
            try
            {
                using (var fs = File.OpenRead(dlgPickThumbnail.FileName))
                {
                    var ms = new MemoryStream();
                    fs.CopyTo(ms);
                    ms.Position = 0;
                    current.Thumbnail?.Dispose();
                    current.Thumbnail = Image.FromStream(ms);
                }
                Picture1.Image = current.Thumbnail;
                txtImgPath.Text = dlgPickThumbnail.FileName;
            }
            catch (Exception ex)
            {
                ShowError("Couldn't load thumbnail", ex.Message);
            }
        }

        // --- Options helpers ---------------------------------------

        // Pulled in from a persisted file on form load and pushed back out
        // when the user leaves Options mode. JFade's original prompted on
        // first run but never actually saved (Options.txt write was broken)
        // — we save for real so the prompt is genuinely first-run only.
        void OnFormLoadedFirstTime()
        {
            var opts = CollectionOptions.Load();
            txtCollDir.Text    = opts.CollectionsDir;
            txtThumbDir.Text   = opts.ThumbnailsDir;
            chkWarningOff.Checked = opts.WarningsOff;
            chkCompression.Checked = opts.CompressOnSave;

            if (!opts.IsConfigured)
            {
                EnterMode(UIMode.Options);
                SetStatus("Welcome — set your Collections folder, then click OK.");
            }
        }

        void CmdCloseOptions_Click(object sender, EventArgs e)
        {
            new CollectionOptions
            {
                CollectionsDir = txtCollDir.Text   ?? "",
                ThumbnailsDir  = txtThumbDir.Text  ?? "",
                WarningsOff    = chkWarningOff.Checked,
                CompressOnSave = chkCompression.Checked,
            }.Save();
            EnterMode(UIMode.Main);
        }

        void PickFolderInto(TextBox target)
        {
            if (!string.IsNullOrEmpty(target.Text) && Directory.Exists(target.Text))
                dlgPickFolder.SelectedPath = target.Text;
            if (dlgPickFolder.ShowDialog(this) == DialogResult.OK)
                target.Text = dlgPickFolder.SelectedPath;
        }

        // --- UI sync helpers ---------------------------------------

        void SetCurrent(CollectionInfo info)
        {
            current = info;
            LoadCollectionIntoUI();
            UpdateUIState();
        }

        void LoadCollectionIntoUI()
        {
            loadingUI = true;
            try
            {
                if (current == null)
                {
                    txtCollName.Text = "";
                    txtCollID.Text = "";
                    cmbCollType.SelectedIndex = 0;
                    Picture1.Image = null;
                    txtImgPath.Text = "";
                    lstListOfItems.Items.Clear();
                    return;
                }

                txtCollName.Text = current.Name ?? "";
                txtCollID.Text   = "0x" + current.Instance.ToString("X4");
                cmbCollType.SelectedIndex = (int)current.Scope;
                Picture1.Image = current.Thumbnail;
                txtImgPath.Text = currentPath ?? "";
                RefreshMemberList();
            }
            finally
            {
                loadingUI = false;
            }
        }

        void ReadCollectionFromUI()
        {
            if (current == null) return;
            current.Name = txtCollName.Text;
            current.Scope = (CollectionScope)cmbCollType.SelectedIndex;
        }

        void RefreshMemberList()
        {
            lstListOfItems.BeginUpdate();
            try
            {
                lstListOfItems.Items.Clear();
                if (current == null) return;
                foreach (var m in current.Members)
                {
                    string label = !string.IsNullOrEmpty(m.DisplayName)
                        ? m.DisplayName
                        : $"GUID 0x{m.Guid:X8}";
                    lstListOfItems.Items.Add(label);
                }
            }
            finally
            {
                lstListOfItems.EndUpdate();
            }
        }

        // --- Small helpers -----------------------------------------

        bool ConfirmDiscardChanges()
        {
            if (current == null) return true;
            var result = MessageBox.Show(this,
                "Discard the current collection? Unsaved changes will be lost.",
                "Discard?",
                MessageBoxButtons.OKCancel,
                MessageBoxIcon.Question);
            return result == DialogResult.OK;
        }

        static string SuggestFilename(string name)
        {
            string clean = new string((name ?? "Collection").Select(c =>
                char.IsLetterOrDigit(c) || c == ' ' || c == '_' || c == '-' ? c : '_').ToArray()).Trim();
            return string.IsNullOrEmpty(clean) ? "Collection.package" : clean + ".package";
        }

        void SetStatus(string text)
        {
            if (Panel1 != null) Panel1.Text = text ?? "";
        }

        void ShowError(string title, string body)
        {
            MessageBox.Show(this, body ?? "", title ?? "Error",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
            SetStatus(title);
        }
    }
}
