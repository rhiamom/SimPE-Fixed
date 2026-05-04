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
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace SimPe
{
    /// <summary>
    /// Small Add / Edit dialog for a custom FileTable entry. Lets the user
    /// pick a single .package file or a folder, and (folders only) toggle
    /// recursion.
    /// </summary>
    internal sealed class CustomFileTableItemDialog : Form
    {
        private TextBox tbPath;
        private Button btnBrowseFile;
        private Button btnBrowseFolder;
        private CheckBox cbRecursive;
        private Label lblPath;
        private Button btnOK;
        private Button btnCancel;

        public string SelectedPath { get; private set; }
        public bool IsFile { get; private set; }
        public bool IsRecursive { get { return cbRecursive.Checked; } }

        public CustomFileTableItemDialog(FileTableItem existing)
        {
            BuildLayout();

            if (existing != null)
            {
                tbPath.Text = existing.Name;
                IsFile = existing.IsFile;
                cbRecursive.Checked = existing.IsRecursive;
                cbRecursive.Enabled = !existing.IsFile;
            }
        }

        private void BuildLayout()
        {
            Text = "Custom FileTable Entry";
            FormBorderStyle = FormBorderStyle.FixedDialog;
            StartPosition = FormStartPosition.CenterParent;
            MinimizeBox = false;
            MaximizeBox = false;
            ClientSize = new Size(560, 150);
            Font = new Font("Tahoma", 9F);

            lblPath = new Label
            {
                Text = "Path:",
                Location = new Point(12, 14),
                AutoSize = true,
            };
            Controls.Add(lblPath);

            tbPath = new TextBox
            {
                Location = new Point(12, 32),
                Size = new Size(536, 22),
                ReadOnly = true,
                BackColor = SystemColors.Window,
            };
            Controls.Add(tbPath);

            btnBrowseFile = new Button
            {
                Text = "Browse File...",
                Location = new Point(12, 62),
                Size = new Size(120, 26),
            };
            btnBrowseFile.Click += BrowseFile_Click;
            Controls.Add(btnBrowseFile);

            btnBrowseFolder = new Button
            {
                Text = "Browse Folder...",
                Location = new Point(140, 62),
                Size = new Size(120, 26),
            };
            btnBrowseFolder.Click += BrowseFolder_Click;
            Controls.Add(btnBrowseFolder);

            cbRecursive = new CheckBox
            {
                Text = "Include subfolders (recursive)",
                Location = new Point(280, 65),
                AutoSize = true,
                Enabled = false,
            };
            Controls.Add(cbRecursive);

            btnOK = new Button
            {
                Text = "OK",
                DialogResult = DialogResult.OK,
                Location = new Point(380, 110),
                Size = new Size(80, 28),
            };
            btnOK.Click += OK_Click;
            Controls.Add(btnOK);

            btnCancel = new Button
            {
                Text = "Cancel",
                DialogResult = DialogResult.Cancel,
                Location = new Point(468, 110),
                Size = new Size(80, 28),
            };
            Controls.Add(btnCancel);

            AcceptButton = btnOK;
            CancelButton = btnCancel;
        }

        private void BrowseFile_Click(object sender, EventArgs e)
        {
            using (var ofd = new OpenFileDialog())
            {
                ofd.Filter = "Sims 2 packages (*.package)|*.package|All files (*.*)|*.*";
                ofd.CheckFileExists = true;
                if (!string.IsNullOrEmpty(tbPath.Text) && File.Exists(tbPath.Text))
                    ofd.FileName = tbPath.Text;
                else if (!string.IsNullOrEmpty(SimPe.PathProvider.SimSavegameFolder))
                    ofd.InitialDirectory = Path.Combine(SimPe.PathProvider.SimSavegameFolder, "Downloads");

                if (ofd.ShowDialog(this) == DialogResult.OK)
                {
                    tbPath.Text = ofd.FileName;
                    IsFile = true;
                    cbRecursive.Checked = false;
                    cbRecursive.Enabled = false;
                }
            }
        }

        private void BrowseFolder_Click(object sender, EventArgs e)
        {
            using (var fbd = new FolderBrowserDialog())
            {
                fbd.Description = "Pick a folder to scan for .package files.";
                if (Directory.Exists(tbPath.Text))
                    fbd.SelectedPath = tbPath.Text;
                else if (!string.IsNullOrEmpty(SimPe.PathProvider.SimSavegameFolder))
                    fbd.SelectedPath = Path.Combine(SimPe.PathProvider.SimSavegameFolder, "Downloads");

                if (fbd.ShowDialog(this) == DialogResult.OK)
                {
                    tbPath.Text = fbd.SelectedPath;
                    IsFile = false;
                    cbRecursive.Enabled = true;
                }
            }
        }

        private void OK_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(tbPath.Text))
            {
                MessageBox.Show(this, "Pick a file or folder first.", "No Path",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                DialogResult = DialogResult.None;
                return;
            }
            SelectedPath = tbPath.Text.Trim();
        }
    }
}
