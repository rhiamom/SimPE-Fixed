/***************************************************************************
 *   Copyright (C) 2026 by GramzeSweatshop                                 *
 *   rhiamom@mac.com                                                       *
 *                                                                         *
 *   This program is free software; you can redistribute it and/or modify  *
 *   it under the terms of the GNU General Public License as published by  *
 *   the Free Software Foundation; either version 2 of the License, or     *
 *   (at your option) any later version.                                   *
 ***************************************************************************/

using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace SimPe
{
    /// <summary>
    /// "File Table" preferences tab — lets users add custom .package files,
    /// folders, or recursive folders to the FileTable so PJSE can resolve
    /// their mods' BHAVs and SDSC labels can find their wantSimulator XMLs.
    ///
    /// Built programmatically rather than via Designer.cs to keep the
    /// change scoped to one file.
    /// </summary>
    public partial class OptionForm
    {
        private TabPage tpFileTable;
        private CheckedListBox lbFileTable;
        private Button btnFtAdd;
        private Button btnFtAddDownloads;
        private Button btnFtEdit;
        private Button btnFtDelete;

        // Backing list mirrors the visible CheckedListBox 1:1.
        private List<FileTableItem> ftItems = new List<FileTableItem>();

        private void BuildFileTableTab()
        {
            tpFileTable = new TabPage("File Table")
            {
                UseVisualStyleBackColor = true,
                Padding = new Padding(8),
            };

            lbFileTable = new CheckedListBox
            {
                Dock = DockStyle.Fill,
                Font = new Font("Tahoma", 9F),
                IntegralHeight = false,
                CheckOnClick = true,
            };
            lbFileTable.ItemCheck += LbFileTable_ItemCheck;
            lbFileTable.DoubleClick += (s, e) => EditSelected();

            var bottom = new FlowLayoutPanel
            {
                Dock = DockStyle.Bottom,
                FlowDirection = FlowDirection.LeftToRight,
                Height = 38,
                Padding = new Padding(0, 6, 0, 0),
            };

            btnFtAdd = MakeButton("Add", BtnFtAdd_Click);
            btnFtAddDownloads = MakeButton("Add Downloads Folder", BtnFtAddDownloads_Click, 170);
            btnFtEdit = MakeButton("Change", BtnFtEdit_Click);
            btnFtDelete = MakeButton("Delete", BtnFtDelete_Click);

            bottom.Controls.Add(btnFtAdd);
            bottom.Controls.Add(btnFtAddDownloads);
            bottom.Controls.Add(btnFtEdit);
            bottom.Controls.Add(btnFtDelete);

            var hint = new Label
            {
                Dock = DockStyle.Top,
                AutoSize = false,
                Height = 38,
                Text = "Add files, folders, or recursive folders to be loaded into SimPE's FileTable. " +
                       "Use this to make your mods' globals visible to PJSE and the SDSC viewer.",
                Font = new Font("Tahoma", 8.5F, FontStyle.Italic),
                ForeColor = SystemColors.GrayText,
                TextAlign = ContentAlignment.MiddleLeft,
            };

            tpFileTable.Controls.Add(lbFileTable);
            tpFileTable.Controls.Add(bottom);
            tpFileTable.Controls.Add(hint);

            tc.Controls.Add(tpFileTable);
        }

        private static Button MakeButton(string text, EventHandler handler, int width = 90)
        {
            var b = new Button
            {
                Text = text,
                Width = width,
                Height = 28,
                Margin = new Padding(0, 0, 6, 0),
                UseVisualStyleBackColor = true,
            };
            b.Click += handler;
            return b;
        }

        // Called from OptionForm.Execute() once the rest of the form has
        // populated. Reads the user's saved customfolders.xreg and fills the
        // listbox; checked = enabled (Ignore == false).
        private void LoadCustomFileTableItems()
        {
            ftItems = CustomFolderStore.Load() ?? new List<FileTableItem>();
            RefreshList();
        }

        // Called from SaveOptionsClick to persist before the form closes.
        private void SaveCustomFileTableItems()
        {
            CustomFolderStore.Save(ftItems);
        }

        private void RefreshList()
        {
            if (lbFileTable == null) return;

            // ItemCheck fires during Items.Clear() in some .NET versions; guard.
            lbFileTable.ItemCheck -= LbFileTable_ItemCheck;
            try
            {
                lbFileTable.Items.Clear();
                for (int i = 0; i < ftItems.Count; i++)
                {
                    lbFileTable.Items.Add(ftItems[i].ToString(), !ftItems[i].Ignore);
                }
            }
            finally
            {
                lbFileTable.ItemCheck += LbFileTable_ItemCheck;
            }
        }

        private void LbFileTable_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            if (e.Index < 0 || e.Index >= ftItems.Count) return;
            ftItems[e.Index].Ignore = (e.NewValue != CheckState.Checked);
        }

        private void BtnFtAdd_Click(object sender, EventArgs e)
        {
            using (var dlg = new CustomFileTableItemDialog(null))
            {
                if (dlg.ShowDialog(this) != DialogResult.OK) return;
                AddItem(dlg.SelectedPath, dlg.IsFile, dlg.IsRecursive);
            }
        }

        private void BtnFtAddDownloads_Click(object sender, EventArgs e)
        {
            string sg = SimPe.PathProvider.SimSavegameFolder;
            if (string.IsNullOrEmpty(sg))
            {
                MessageBox.Show(this,
                    "SimPE doesn't know where your save game folder is. " +
                    "Set Game Locations first.",
                    "Save Game Folder Not Found",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
                return;
            }

            string dl = Path.Combine(sg, "Downloads");
            if (!Directory.Exists(dl))
            {
                MessageBox.Show(this,
                    "No Downloads folder was found at:\n\n" + dl,
                    "Downloads Folder Not Found",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
                return;
            }

            // Skip duplicate.
            foreach (var existing in ftItems)
            {
                if (!existing.IsFile && existing.IsRecursive &&
                    string.Equals(existing.Name, dl, StringComparison.OrdinalIgnoreCase))
                {
                    MessageBox.Show(this,
                        "Your Downloads folder is already in the list.",
                        "Already Added",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                    return;
                }
            }

            AddItem(dl, false, true);
        }

        private void AddItem(string fullPath, bool isFile, bool recursive)
        {
            // FileTableItem ctor with FileTablePaths.Absolute then SetName auto-rebases
            // onto whichever well-known root applies (SaveGameFolder, an EP folder, etc.).
            var fti = new FileTableItem(fullPath, FileTablePaths.Absolute, recursive, isFile, -1, false);
            fti.Name = fullPath; // triggers SetName -> rebase
            ftItems.Add(fti);
            RefreshList();
            if (lbFileTable.Items.Count > 0)
                lbFileTable.SelectedIndex = lbFileTable.Items.Count - 1;
        }

        private void BtnFtEdit_Click(object sender, EventArgs e)
        {
            EditSelected();
        }

        private void EditSelected()
        {
            int idx = lbFileTable.SelectedIndex;
            if (idx < 0 || idx >= ftItems.Count) return;

            using (var dlg = new CustomFileTableItemDialog(ftItems[idx]))
            {
                if (dlg.ShowDialog(this) != DialogResult.OK) return;
                bool oldIgnore = ftItems[idx].Ignore;
                var fti = new FileTableItem(dlg.SelectedPath, FileTablePaths.Absolute, dlg.IsRecursive, dlg.IsFile, -1, oldIgnore);
                fti.Name = dlg.SelectedPath;
                ftItems[idx] = fti;
                RefreshList();
                lbFileTable.SelectedIndex = idx;
            }
        }

        private void BtnFtDelete_Click(object sender, EventArgs e)
        {
            int idx = lbFileTable.SelectedIndex;
            if (idx < 0 || idx >= ftItems.Count) return;
            ftItems.RemoveAt(idx);
            RefreshList();
            if (lbFileTable.Items.Count > 0)
                lbFileTable.SelectedIndex = Math.Min(idx, lbFileTable.Items.Count - 1);
        }
    }
}
