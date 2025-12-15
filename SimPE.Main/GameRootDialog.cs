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
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

namespace SimPe
{
    public partial class GameRootDialog : Form
    {
        private const string CepDownloadUrl = "https://modthesims.info/d/92541/color-enable-package.html";

        public string GameRootPath { get; private set; }

        public string SelectedEdition { get; private set; }

        public string BaseGamePath { get; private set; }

        public string DownloadsPath { get; private set; }

        private bool cepHasGmnd;
        private bool cepHasMmat;
        private bool cepHasZcepFolder;
        private bool cepHasZcepExtraFolder;
        private bool IsCepComplete()
        {
            return
                cepHasGmnd &&
                cepHasZcepFolder &&
                cepHasMmat &&
                cepHasZcepExtraFolder;
        }

        public GameRootDialog()
        {
            InitializeComponent();

            // Choose a sensible default so at least one is always selected.
            rbLegacy.Checked = true;   // Change this if you prefer another default.
            UpdateDefaultGameRootPath();
            UpdateDefaultDownloadsPath();

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
                    UpdateCepStatus();
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

        private void EditionRadio_CheckedChanged(object sender, EventArgs e)
        {
            // Only act when a radio button becomes checked
            if (!(sender is RadioButton rb) || !rb.Checked)
                return;

            UpdateDefaultGameRootPath();
            UpdateDefaultDownloadsPath();
            UpdateCepStatus();
        }

        private static string ResolveBaseGamePath(string edition, string rootPath, GameRootScanResult scanResult)
        {
            if (scanResult == null) return null;

            // Helper local function to match child folder names strictly.
            bool PackIsNamed(PackFolderInfo p, string expectedFolderName)
            {
                if (p == null) return false;
                string folderName = Path.GetFileName(p.FullPath.TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar));
                return string.Equals(folderName, expectedFolderName, StringComparison.OrdinalIgnoreCase);
            }

            // 1) Legacy / Steam / Epic -> base folder must be "Base" under the wrapper root.
            if (edition == "Legacy" || edition == "Steam" || edition == "Epic")
            {
                foreach (var p in scanResult.Packs)
                {
                    if (p.HasTsData && PackIsNamed(p, "Base"))
                    {
                        return p.FullPath;
                    }
                }
                return null;
            }

            // 2) Ultimate Collection -> base folder is "Double Deluxe\\Base"
            if (edition == "Ultimate Collection")
            {
                string ddBase = Path.Combine(rootPath, "Double Deluxe", "Base");
                if (Directory.Exists(Path.Combine(ddBase, "TSData")))
                {
                    return ddBase;
                }

                // Fallback: if user selected Double Deluxe directly as root
                string directBase = Path.Combine(rootPath, "Base");
                if (Directory.Exists(Path.Combine(directBase, "TSData")))
                {
                    return directBase;
                }
                return null;
            }

            // 3) Disc / Custom:
            //    - If the chosen root itself has TSData, treat it as base (user pointed directly at base game folder).
            //    - Otherwise we do NOT guess among child packs; we require user correction in the dialog.
            if (edition == "Disc" || edition == "Custom")
                {
                    string rootTsData = Path.Combine(rootPath, "TSData");
                    if (Directory.Exists(rootTsData))
                    {
                        return rootPath;
                    }

                    // Some people might choose the parent of "The Sims 2" (disc-style),
                    // but we still keep this strict: check only the canonical disc folder name.
                    string theSims2 = Path.Combine(rootPath, "The Sims 2");
                    if (Directory.Exists(Path.Combine(theSims2, "TSData")))
                    {
                        return theSims2;
                    }

                    return null;
                }

                // Unknown edition
                return null;
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

            string downloads = txtDownloads.Text.Trim();
            if (downloads.Length > 0 && !Directory.Exists(downloads))
            {
                MessageBox.Show(
                    this,
                    "The Downloads folder you entered does not exist.\n\n" +
                    "This is OK if you have not run the game yet, but CEP and custom content won't be detected until it exists.\n\n" +
                    "You can continue, or click Cancel and choose a different folder.",
                    "Downloads Folder Not Found",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
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

            // 4) Resolve base game folder path (strict edition rules)
            string basePath = ResolveBaseGamePath(edition, scanResult.RootFolder, scanResult);

            if (string.IsNullOrEmpty(basePath))
            {
                MessageBox.Show(
                    this,
                    "The Sims 2 base game folder could not be found where it is expected for the selected edition.\n\n" +
                    "Please use Custom and browse directly to your base game folder (the folder that contains TSData).\n\n" +
                    "Examples:\n" +
                    "  - Legacy/Steam/Epic: ...\\The Sims 2 Legacy Collection\\Base\n" +
                    "  - Ultimate Collection: ...\\The Sims 2 Ultimate Collection\\Double Deluxe\\Base\n" +
                    "  - Disc/Custom base folder: ...\\The Sims 2",
                    "Base Game Folder Not Found",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                rbCustom.Checked = true;
                return;
            }

            // 5) Store values and close
            GameRootPath = path;
            SelectedEdition = edition;
            BaseGamePath = basePath;
            DownloadsPath = downloads;
            UpdateCepStatus();

            if (!IsCepComplete())
            {
                MessageBox.Show(
                    this,
                    "CEP is required.\n\n" +
                    "SimPE cannot run without the Color Enable Package (CEP).\n\n" +
                    "Without CEP, custom content and recolors will not appear in-game.\n\n" +
                    "Please download and install CEP, then return here.",
                    "CEP Required",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                return;
            }

            // Make it available globally for this run
            Helper.GameRootPath = GameRootPath;
            Helper.GameEdition  = SelectedEdition;
            Helper.BaseGamePath = BaseGamePath;
            Helper.DownloadsPath  = DownloadsPath;

            // Persist them so we don't lose them after this run
            Helper.SaveGameRootToFile(GameRootPath, SelectedEdition, BaseGamePath, DownloadsPath);

            Helper.LocalMode = false;
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

        private void UpdateDefaultDownloadsPath()
        {
            string suggested = null;

            string documents = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            string eaGames = Path.Combine(documents, "EA Games");

            if (rbLegacy.Checked || rbSteam.Checked || rbEpic.Checked)
            {
                // Steam uses the same Documents folder name as Legacy
                suggested = Path.Combine(eaGames, "The Sims 2 Legacy", "Downloads");
            }
            else if (rbUC.Checked)
            {
                suggested = Path.Combine(eaGames, "The Sims™ 2 Ultimate Collection", "Downloads");
            }
            else if (rbDisc.Checked)
            {
                suggested = Path.Combine(eaGames, "The Sims 2", "Downloads");
            }
            else if (rbCustom.Checked)
            {
                // Custom: leave blank so user must choose if needed
                suggested = string.Empty;
            }

            if (suggested != null)
            {
                txtDownloads.Text = suggested;
            }
        }

        private void UpdateCepStatus()
        {
            // Reset
            cepHasGmnd = false;
            cepHasMmat = false;
            cepHasZcepFolder = false;
            cepHasZcepExtraFolder = false;

            string baseGamePath = BaseGamePath;

            string downloadsPath = txtDownloads.Text.Trim();
            if (string.IsNullOrEmpty(baseGamePath))
            {
                string edition = GetSelectedEdition();
                string root = txtGameRoot.Text.Trim();

                if (!string.IsNullOrEmpty(edition) &&
                    !string.IsNullOrEmpty(root) &&
                    Directory.Exists(root))
                {
                    try
                    {
                        var scan = GameRootAutoScanner.ScanRoot(root);
                        baseGamePath = ResolveBaseGamePath(edition, scan.RootFolder, scan);
                    }
                    catch
                    {
                        // ignore; we'll just show missing
                    }
                }
            }

            // --- User-side CEP (Downloads) ---
            if (!string.IsNullOrEmpty(downloadsPath))
            {
                string gmndPath = Path.Combine(downloadsPath, "_EnableColorOptionsGMND.package");
                string zcepFolderPath = Path.Combine(downloadsPath, "zCEP-EXTRA");

                cepHasGmnd = File.Exists(gmndPath);
                cepHasZcepFolder = Directory.Exists(zcepFolderPath);
            }

            // --- Program-side CEP (Base game folder) ---
            if (!string.IsNullOrEmpty(baseGamePath))
            {
                string mmatPath = Path.Combine(baseGamePath, "TSData", "Res", "Sims3D", "_EnableColorOptionsMMAT.package");
                string zcepExtraFolderPath = Path.Combine(baseGamePath, "TSData", "Res", "Catalog", "zCEP-EXTRA");

                cepHasMmat = File.Exists(mmatPath);
                cepHasZcepExtraFolder = Directory.Exists(zcepExtraFolderPath);
            }

            // --- Display ---
            if (lblCepStatus != null)
            {
                txtCepStatus.Text =
                    "CEP status:\r\n" +
                    $"  Downloads: GMND {(cepHasGmnd ? "OK" : "Missing")}, zCEP {(cepHasZcepFolder ? "OK" : "Missing")}\r\n" +
                    $"  Base game: MMAT {(cepHasMmat ? "OK" : "Missing")}, zCEP-EXTRA {(cepHasZcepExtraFolder ? "OK" : "Missing")}\r\n" +
                        (cepHasGmnd && cepHasZcepFolder && cepHasMmat && cepHasZcepExtraFolder
        ? "  CEP is fully installed. Maxis object recolors will work."
        : "  CEP is incomplete or missing. Maxis object recolors will NOT work.");
                btnDownloadCep.Enabled = !IsCepComplete();
            }
        }

        private void btnBrowseDownloads_Click(object sender, EventArgs e)
        {
            using (var dlg = new FolderBrowserDialog())
            {
                dlg.Description = "Select your Sims 2 Downloads folder (in Documents\\EA Games\\...).";
                dlg.ShowNewFolderButton = true;

                if (Directory.Exists(txtDownloads.Text))
                {
                    dlg.SelectedPath = txtDownloads.Text;
                }

                if (dlg.ShowDialog(this) == DialogResult.OK)
                {
                    txtDownloads.Text = dlg.SelectedPath;
                    UpdateCepStatus();
                }
            }
        }

        private void btnDownloadCep_Click(object sender, EventArgs e)
        {
            try
            {
                System.Diagnostics.Process.Start(new ProcessStartInfo
                {
                    FileName = CepDownloadUrl,
                    UseShellExecute = true
                });
            }
            catch
            {
                MessageBox.Show(
                    this,
                    "Unable to open the CEP download page.\n\n" +
                    "Please search for \"Sims 2 Color Enable Package (CEP)\" on ModTheSims.",
                    "Error Opening Browser",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        protected override void OnActivated(EventArgs e)
        {
            base.OnActivated(e);
            UpdateCepStatus();
        }

    }
}



