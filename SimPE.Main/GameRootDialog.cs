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
using System.IO;
using System.Windows.Forms;

namespace SimPe
{
    public partial class GameRootDialog : Form
    {
        public string GameRootPath { get; private set; }

        public string SelectedEdition { get; private set; }

        public GameRootDialog()
        {
            InitializeComponent();

            // Choose a sensible default so at least one is always selected.
            rbLegacy.Checked = true;   // Change this if you prefer another default.
            UpdateDefaultGameRootPath();
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            using (var dlg = new FolderBrowserDialog())
            {
                dlg.Description = "Select the root folder where The Sims 2 is installed.";
                dlg.ShowNewFolderButton = false;

                if (Directory.Exists(txtGameRoot.Text))
                {
                    dlg.SelectedPath = txtGameRoot.Text;
                }

                if (dlg.ShowDialog(this) == DialogResult.OK)
                {
                    txtGameRoot.Text = dlg.SelectedPath;
                }
            }
        }

        private string GetSelectedEdition()
        {
            if (rbLegacy.Checked) return "Legacy";
            if (rbUC.Checked) return "Ultimate Collection";
            if (rbSteam.Checked) return "Steam";
            if (rbEpic.Checked) return "Epic";
            if (rbDisc.Checked) return "Disc";
            if (rbCustom.Checked) return "Custom";

            return string.Empty;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            // 1) Edition must be selected (in practice, one radio is always checked if you set a default)
            string edition = GetSelectedEdition();
            if (edition.Length == 0)
            {
                MessageBox.Show(
                    this,
                    "Please select which type of Sims 2 installation you have (Legacy, UC, Steam, Epic, Disc, or Custom).",
                    "Edition Required",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            // 2) Validate folder path
            string path = txtGameRoot.Text.Trim();

            if (path.Length == 0)
            {
                MessageBox.Show(
                    this,
                    "Please select the folder where The Sims 2 is installed.",
                    "Game Root Required",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            if (!Directory.Exists(path))
            {
                MessageBox.Show(
                    this,
                    "The selected folder does not exist. Please choose a valid folder.",
                    "Invalid Folder",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            // 3) Use our scanner to validate that this really looks like a TS2 install.
            GameRootScanResult scanResult;
            try
            {
                scanResult = GameRootAutoScanner.ScanRoot(path);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    this,
                    "An error occurred while scanning the selected folder:\n\n" + ex.Message,
                    "Scan Failed",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                return;
            }

            bool hasAnyPack = false;

            foreach (var pack in scanResult.Packs)
            {
                if (pack.HasTsData)
                {
                    hasAnyPack = true;
                }
            }

            if (!hasAnyPack)
            {
                MessageBox.Show(
                    this,
                    "No Sims 2 TSData folders were found under this folder.\n\n" +
                    "The edition has been set to Custom so you can browse to the correct folder.",
                    "No Packs Found",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                // Force manual correction
                rbCustom.Checked = true;
                txtGameRoot.Text = string.Empty;

                return;
            }

            // 4) Store values and close
            GameRootPath = path;
            SelectedEdition = edition;

            // Make it available globally for this run
            Helper.GameRootPath = GameRootPath;
            Helper.GameEdition  = SelectedEdition;

            // Persist them so we don't lose them after this run
            Helper.SaveGameRootToFile(GameRootPath, SelectedEdition);

            this.DialogResult = DialogResult.OK;
            this.Close();
        }
        private void UpdateDefaultGameRootPath()
        {
            string suggested = null;

            if (rbLegacy.Checked)
            {
                // Legacy can end up in Program Files or Program Files (x86),
                // with or without the "EA GAMES" folder.
                string l1 = @"C:\Program Files\EA GAMES\The Sims 2 Legacy";
                string l2 = @"C:\Program Files (x86)\EA GAMES\The Sims 2 Legacy";

                // Newer / custom EA App layouts without "EA GAMES"
                string l3 = @"C:\Program Files\The Sims 2 Legacy";
                string l4 = @"C:\Program Files (x86)\The Sims 2 Legacy";

                if (Directory.Exists(l1))
                    suggested = l1;
                else if (Directory.Exists(l2))
                    suggested = l2;
                else if (Directory.Exists(l3))
                    suggested = l3;
                else if (Directory.Exists(l4))
                    suggested = l4;
                else
                    suggested = string.Empty;   //act like custom was checked
            }

            else if (rbUC.Checked)
            {
                // Classic EA App / Origin installs
                string p1 = @"C:\Program Files (x86)\EA GAMES\The Sims 2 Ultimate Collection";
                string p2 = @"C:\Program Files\EA GAMES\The Sims 2 Ultimate Collection";

                // Newer EA App installs (no EA GAMES folder)
                string p3 = @"C:\Program Files\The Sims 2 Ultimate Collection";
                string p4 = @"C:\Program Files (x86)\The Sims 2 Ultimate Collection";

                if (Directory.Exists(p1)) suggested = p1;
                else if (Directory.Exists(p2)) suggested = p2;
                else if (Directory.Exists(p3)) suggested = p3;
                else if (Directory.Exists(p4)) suggested = p4;
                else
                    suggested = string.Empty;   //act like custom was checked
            }

            else if (rbDisc.Checked)
            {
                // Classic disc installs also usually live here
                suggested = @"C:\Program Files (x86)\EA GAMES\The Sims 2";
            }

            else if (rbSteam.Checked)
            {
                // Hypothetical Steam default – user can edit if wrong
                suggested = @"C:\Program Files (x86)\Steam\steamapps\common\The Sims 2";
            }

            else if (rbEpic.Checked)
            {
                // Epic hands off to EA App; it may install with or without "EA GAMES".
                string p1 = @"C:\Program Files (x86)\EA GAMES\The Sims 2 Legacy";
                string p2 = @"C:\Program Files\EA GAMES\The Sims 2 Legacy";

                string p3 = @"C:\Program Files\The Sims 2 Legacy";
                string p4 = @"C:\Program Files (x86)\The Sims 2 Legacy";

                if (Directory.Exists(p1))
                    suggested = p1;
                else if (Directory.Exists(p2))
                    suggested = p2;
                else if (Directory.Exists(p3))
                    suggested = p3;
                else if (Directory.Exists(p4))
                    suggested = p4;
                else
                    suggested = string.Empty;   //act like custom was checked
            }

            else if (rbCustom.Checked)
            {
                // Custom: leave it blank so the user *must* choose
                suggested = string.Empty;
            }
            // If you have a Mac radio button (rbMac), you can either leave this blank
            // or later set something like "/Applications/The Sims 2.app" in the Mac build.
            // else if (rbMac.Checked)
            // {
            //     suggested = string.Empty;
            // }

            if (suggested != null)
            {
                txtGameRoot.Text = suggested;
            }
        }


    }
}

