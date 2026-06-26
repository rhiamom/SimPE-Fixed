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
            cmdBatchAdd.Click    += (s, e) => EnterMode(UIMode.BatchAdd);

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
            cmdCloseOptions.Click += (s, e) => EnterMode(UIMode.Main);
            cmdFindCollDir.Click  += (s, e) => PickFolderInto(txtCollDir);
            cmdFindThumbDir.Click += (s, e) => PickFolderInto(txtThumbDir);

            // --- Batch Add mode buttons (placeholders for now) -----
            cmdFinishBatchAdd.Click += (s, e) => EnterMode(UIMode.Main);
            cmdCancelBatchAdd.Click += (s, e) => EnterMode(UIMode.Main);

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

            int added = 0, skipped = 0;
            foreach (string path in dlgAddObject.FileNames)
            {
                try
                {
                    var infos = ObjectCatalog.Read(path, nameTable);
                    if (infos.Count == 0) { skipped++; continue; }

                    var info = infos[0];
                    current.Members.Add(new CollectionMember
                    {
                        ObjectType = info.ObjectType,
                        ObjectGroup = info.ObjectGroup,
                        ObjectInstance = info.ObjectInstance,
                        ObjectInstanceHi = info.ObjectInstanceHi,
                        Guid = info.Guid,
                        DisplayName = info.DisplayName,
                    });
                    added++;
                }
                catch
                {
                    skipped++;
                }
            }

            RefreshMemberList();
            UpdateUIState();
            SetStatus(skipped == 0
                ? $"Added {added} object(s)."
                : $"Added {added} object(s); skipped {skipped} (no OBJD inside).");
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
