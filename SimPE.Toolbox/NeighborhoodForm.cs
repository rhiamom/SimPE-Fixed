/***************************************************************************
 *   Copyright (C) 2005 by Ambertation                                     *
 *   quaxi@ambertation.de                                                  *
 *                                                                         *
 *   Copyright (C) 2025 by GramzeSweatShop                                 *
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
using System.Drawing;
using System.Collections;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using SimPe.Interfaces.Plugin;
using Avalonia.Controls.Templates;
using Avalonia.Media;

namespace SimPe.Plugin
{
    // ── View-model item for each neighborhood shown in the ListBox ────────────
    class NeighborhoodItem
    {
        public Avalonia.Media.Imaging.Bitmap Image { get; set; }
        public string Text    { get; set; }
        public string FilePath { get; set; }   // formerly SubItems[1]
        public string Name    { get; set; }    // formerly SubItems[2]
        public string Label   { get; set; }    // formerly SubItems[3]
        public string ToolTip { get; set; }
    }

    /// <summary>
    /// Summary description for NeighborhoodForm.
    /// </summary>
    public class NeighborhoodForm : Avalonia.Controls.Window
    {
        // ── Avalonia controls ────────────────────────────────────────────────
        private Avalonia.Controls.ListBox          lv;
        private Avalonia.Controls.ComboBox         cbtypes;
        private Avalonia.Controls.TextBlock        label1;
        private Avalonia.Controls.Button           btnOpen;
        private Avalonia.Controls.Button           button2;
        private Avalonia.Controls.Button           button3;
        private Avalonia.Controls.Button           btnClose;
        private Avalonia.Controls.Image            pbox;
        private Avalonia.Controls.StackPanel       pnBackup;
        private Avalonia.Controls.StackPanel       pnOptions;
        private Avalonia.Controls.Grid             pnBoPeep;
        ThemeManager tm;

        // ── Backing collections ──────────────────────────────────────────────
        private ObservableCollection<NeighborhoodItem> lvItems = new ObservableCollection<NeighborhoodItem>();
        private ObservableCollection<NgbhType>         cbItems = new ObservableCollection<NgbhType>();

        // ── Modal result ─────────────────────────────────────────────────────
        public System.Windows.Forms.DialogResult DialogResult { get; set; }

        public NeighborhoodForm()
        {
            InitializeComponent();
            tm = ThemeManager.Global.CreateChild();
            // ThemeManager.AddControl expects WinForms Control — skip for Avalonia panels
        }

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (tm != null)
                {
                    tm.Clear();
                    tm.Parent = null;
                    tm = null;
                }
                // Dispose Avalonia bitmaps held by list items
                foreach (var item in lvItems)
                    item.Image?.Dispose();
            }
        }

        #region Windows Form Designer generated code
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.Title  = "Neighborhood Browser";
            this.Width  = 660;
            this.Height = 520;
            this.MinWidth  = 400;
            this.MinHeight = 320;
            this.WindowStartupLocation = Avalonia.Controls.WindowStartupLocation.CenterOwner;
            this.CanResize = true;

            // ── ListBox (replaces WinForms ListView with LargeImageList) ─────
            lv = new Avalonia.Controls.ListBox();
            lv.ItemsSource   = lvItems;
            lv.SelectionMode = Avalonia.Controls.SelectionMode.Single;
            lv.ItemsPanel    = new FuncTemplate<Avalonia.Controls.Panel>(() =>
                new Avalonia.Controls.WrapPanel { Orientation = Avalonia.Layout.Orientation.Horizontal });
            lv.ItemTemplate  = new FuncDataTemplate<NeighborhoodItem>((item, _) =>
            {
                if (item == null) return new Avalonia.Controls.TextBlock { Text = string.Empty };

                var imgCtrl = new Avalonia.Controls.Image
                {
                    Width   = 120,
                    Height  = 90,
                    Stretch = Avalonia.Media.Stretch.Uniform,
                    Source  = item.Image
                };

                var tb = new Avalonia.Controls.TextBlock
                {
                    Text           = item.Text,
                    TextWrapping   = Avalonia.Media.TextWrapping.Wrap,
                    TextAlignment  = Avalonia.Media.TextAlignment.Center,
                    MaxWidth       = 130
                };

                var sp = new Avalonia.Controls.StackPanel
                {
                    Orientation = Avalonia.Layout.Orientation.Vertical,
                    Margin      = new Avalonia.Thickness(4),
                    Width       = 134
                };
                if (item.ToolTip != null)
                    Avalonia.Controls.ToolTip.SetTip(sp, item.ToolTip);

                sp.Children.Add(imgCtrl);
                sp.Children.Add(tb);
                return sp;
            }, false);
            lv.SelectionChanged += (s, e) => NgbSelect(s, EventArgs.Empty);

            // ── ComboBox (replaces cbtypes) ──────────────────────────────────
            cbtypes = new Avalonia.Controls.ComboBox();
            cbtypes.ItemsSource = cbItems;
            cbtypes.MinWidth    = 200;
            cbtypes.SelectionChanged += (s, e) => cbtypes_SelectedIndexChanged(s, EventArgs.Empty);

            // ── Label ────────────────────────────────────────────────────────
            label1 = new Avalonia.Controls.TextBlock();
            label1.Text = SimPe.Localization.GetString("Type") + ":";
            label1.VerticalAlignment = Avalonia.Layout.VerticalAlignment.Center;
            label1.Margin = new Avalonia.Thickness(0, 0, 6, 0);

            // ── Buttons ──────────────────────────────────────────────────────
            btnOpen = new Avalonia.Controls.Button();
            btnOpen.Content  = SimPe.Localization.GetString("Open");
            btnOpen.MinWidth = 80;
            btnOpen.IsEnabled = false;
            btnOpen.Click += (s, e) => NgbOpen(s, EventArgs.Empty);

            button2 = new Avalonia.Controls.Button();
            button2.Content  = SimPe.Localization.GetString("Backup");
            button2.MinWidth = 80;
            button2.IsEnabled = false;
            button2.Click += (s, e) => NgbBackup(s, EventArgs.Empty);

            button3 = new Avalonia.Controls.Button();
            button3.Content  = SimPe.Localization.GetString("Restore Backup");
            button3.MinWidth = 100;
            button3.IsEnabled = false;
            button3.Click += (s, e) => NgbRestoreBackup(s, EventArgs.Empty);

            btnClose = new Avalonia.Controls.Button();
            btnClose.Content  = SimPe.Localization.GetString("Close");
            btnClose.MinWidth = 80;
            btnClose.Click += (s, e) =>
            {
                DialogResult = System.Windows.Forms.DialogResult.Cancel;
                Close();
            };

            // ── Smiley icon image (replaces PictureBox pbox) ─────────────────
            pbox        = new Avalonia.Controls.Image();
            pbox.Width  = 32;
            pbox.Height = 32;
            pbox.VerticalAlignment = Avalonia.Layout.VerticalAlignment.Center;

            // ── pnOptions: label + cbtypes (type selector row) ───────────────
            pnOptions = new Avalonia.Controls.StackPanel();
            pnOptions.Orientation = Avalonia.Layout.Orientation.Horizontal;
            pnOptions.Margin      = new Avalonia.Thickness(4, 2, 4, 2);
            pnOptions.Children.Add(label1);
            pnOptions.Children.Add(cbtypes);

            // ── pnBackup: backup/restore buttons + smiley ────────────────────
            pnBackup = new Avalonia.Controls.StackPanel();
            pnBackup.Orientation = Avalonia.Layout.Orientation.Horizontal;
            pnBackup.Spacing     = 4;
            pnBackup.Margin      = new Avalonia.Thickness(4, 2, 4, 2);
            pnBackup.Children.Add(button2);
            pnBackup.Children.Add(button3);
            pnBackup.Children.Add(pbox);

            // ── Button bar at the bottom ──────────────────────────────────────
            var buttonBar = new Avalonia.Controls.StackPanel();
            buttonBar.Orientation         = Avalonia.Layout.Orientation.Horizontal;
            buttonBar.HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Right;
            buttonBar.Spacing             = 4;
            buttonBar.Margin              = new Avalonia.Thickness(4, 4, 4, 4);
            buttonBar.Children.Add(btnOpen);
            buttonBar.Children.Add(btnClose);

            // ── Bottom area: type selector, backup bar, button row ────────────
            var bottomArea = new Avalonia.Controls.StackPanel();
            bottomArea.Orientation = Avalonia.Layout.Orientation.Vertical;
            bottomArea.Children.Add(pnOptions);
            bottomArea.Children.Add(pnBackup);
            bottomArea.Children.Add(buttonBar);

            // ── Main grid: lv fills top, bottomArea at bottom ─────────────────
            pnBoPeep = new Avalonia.Controls.Grid();
            pnBoPeep.RowDefinitions.Add(new Avalonia.Controls.RowDefinition(1, Avalonia.Controls.GridUnitType.Star));
            pnBoPeep.RowDefinitions.Add(new Avalonia.Controls.RowDefinition(Avalonia.Controls.GridLength.Auto));
            Avalonia.Controls.Grid.SetRow(lv,         0);
            Avalonia.Controls.Grid.SetRow(bottomArea, 1);
            pnBoPeep.Children.Add(lv);
            pnBoPeep.Children.Add(bottomArea);

            this.Content = pnBoPeep;
        }
        #endregion


        bool lodesubs = true;
        public bool ShowSubHoods      { get { return lodesubs;    } set { lodesubs    = value; } }

        bool ngbhBUMgr = true;
        public bool ShowBackupManager { get { return ngbhBUMgr;   } set { ngbhBUMgr   = value; } }

        bool loadNgbh = true;
        public bool LoadNgbh          { get { return loadNgbh;    } set { loadNgbh    = value; } }

        NgbhType ngbh = null;
        public string SelectedNgbh    { get { return ngbh == null ? null : ngbh.FileName; } }

        SimPe.Packages.GeneratableFile package;
        SimPe.Packages.File source_package;
        Interfaces.IProviderRegistry prov;
        bool changed;

        // ── Helper: convert System.Drawing.Image to Avalonia Bitmap ──────────
        private static Avalonia.Media.Imaging.Bitmap ToAvaloniaBitmap(System.Drawing.Image img)
        {
            if (img == null) return null;
            try
            {
                using (var ms = new System.IO.MemoryStream())
                {
                    img.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                    ms.Position = 0;
                    return new Avalonia.Media.Imaging.Bitmap(ms);
                }
            }
            catch { return null; }
        }

        protected Avalonia.Media.Imaging.Bitmap AddImage(string path)
        {
            string name = System.IO.Path.Combine(
                System.IO.Path.GetDirectoryName(path),
                System.IO.Path.GetFileNameWithoutExtension(path) + ".png");

            if (System.IO.File.Exists(name))
            {
                try
                {
                    System.IO.Stream st = System.IO.File.OpenRead(name);
                    System.Drawing.Image img = Helper.LoadImage(st);
                    st.Close();
                    st.Dispose();
                    st = null;
                    if (WaitingScreen.Running)
                        WaitingScreen.UpdateImage(ImageLoader.Preview(img, WaitingScreen.ImageSize));
                    var bmp = ToAvaloniaBitmap(img);
                    img.Dispose();
                    return bmp;
                }
                catch (System.ArgumentException) { }
            }

            using (var fallback = new System.Drawing.Bitmap(SimPe.GetImage.Network))
                return ToAvaloniaBitmap(fallback);
        }

        protected void AddNeighborhood(ExpansionItem.NeighborhoodPath np, string path)
        {
            AddNeighborhood(np, path, "_Neighborhood.package");
        }

        protected string NeighborhoodIdentifier(string flname)
        {
            return System.IO.Path.GetFileNameWithoutExtension(flname).Replace("_Neighborhood", "");
        }

        protected bool AddNeighborhood(ExpansionItem.NeighborhoodPath np, string path, string filename)
        {
            Application.DoEvents();
            string flname = System.IO.Path.Combine(
                System.IO.Path.GetDirectoryName(path),
                System.IO.Path.Combine(
                    System.IO.Path.GetFileName(path),
                    System.IO.Path.GetFileName(path) + filename));
            if (!System.IO.File.Exists(flname)) return false;

            var bmp    = AddImage(flname);
            flname     = System.IO.Path.Combine(path, flname);
            string name    = flname;
            string actime  = "";
            bool ret       = false;

            if (System.IO.File.Exists(name))
            {
                actime  = " (" + System.IO.File.GetLastWriteTime(name).ToString() + ") ";
                actime += NeighborhoodIdentifier(flname);
                ret     = true;
                try
                {
                    SimPe.Packages.File pk = SimPe.Packages.File.LoadFromFile(name);
                    NeighbourhoodTipe t;
                    name = LoadLabel(pk, out t);
                }
                catch (Exception) { }
            }

            string displayText = name + actime;
            if (np.Lable != "") displayText = np.Lable + ": " + displayText;

            var item = new NeighborhoodItem
            {
                Image    = bmp,
                Text     = displayText,
                FilePath = flname,
                Name     = name,
                Label    = np.Lable,
                ToolTip  = UserVerification.HaveUserId ? flname : null
            };
            lvItems.Add(item);

            return ret;
        }

        private static string LoadLabel(SimPe.Packages.File pk, out NeighbourhoodTipe type)
        {
            string name = SimPe.Localization.GetString("Unknown");
            type = NeighbourhoodTipe.Normal;
            try
            {
                SimPe.Interfaces.Files.IPackedFileDescriptor pfd = pk.FindFile(0x43545353, 0, 0xffffffff, 1);
                if (pfd != null)
                {
                    SimPe.PackedFiles.Wrapper.Str str = new SimPe.PackedFiles.Wrapper.Str();
                    str.ProcessData(pfd, pk);
                    name = str.FallbackedLanguageItem(Helper.XmlRegistry.LanguageCode, 0).Title;
                }
                else
                    if (pk.FileName.Contains("Tutorial")) name = "Tutorial"; // CJH

                pfd = pk.FindFile(0xAC8A7A2E, 0, 0xffffffff, 1);
                if (pfd != null)
                {
                    SimPe.Plugin.Idno idno = new Idno();
                    idno.ProcessData(pfd, pk);
                    type = idno.Tipe;
                }
                else
                    if (pk.FileName.Contains("Tutorial")) type = NeighbourhoodTipe.Tutorial;
            }
            finally { }
            return name;
        }

        protected void UpdateList()
        {
            WaitingScreen.Wait();
            Application.DoEvents();

            try
            {
                // Dispose and clear existing thumbnail bitmaps
                foreach (var old in lvItems)
                    old.Image?.Dispose();
                lvItems.Clear();

                ExpansionItem.NeighborhoodPaths paths = PathProvider.Global.GetNeighborhoodsForGroup(PathProvider.Global.CurrentGroup);
                foreach (ExpansionItem.NeighborhoodPath path in paths)
                {
                    string sourcepath = path.Path;
                    string[] dirs = System.IO.Directory.GetDirectories(sourcepath, "*"); // CJH - removes the 4 char limit
                    foreach (string dir in dirs)
                        if (!dir.Contains("Tutorial"))
                            AddNeighborhood(path, dir);
                }
                if (Helper.XmlRegistry.LoadAllNeighbourhoods && loadNgbh)
                {
                    if (PathProvider.Global.GetExpansion(SimPe.Expansions.IslandStories).Exists)
                    {
                        paths = PathProvider.Global.GetNeighborhoodsForGroup(8);
                        foreach (ExpansionItem.NeighborhoodPath path in paths)
                        {
                            string sourcepath = path.Path;
                            string[] dirs = System.IO.Directory.GetDirectories(sourcepath, "*");
                            foreach (string dir in dirs)
                                if (!dir.Contains("Tutorial"))
                                    AddNeighborhood(path, dir);
                        }
                    }
                    if (PathProvider.Global.GetExpansion(SimPe.Expansions.PetStories).Exists)
                    {
                        paths = PathProvider.Global.GetNeighborhoodsForGroup(4);
                        foreach (ExpansionItem.NeighborhoodPath path in paths)
                        {
                            string sourcepath = path.Path;
                            string[] dirs = System.IO.Directory.GetDirectories(sourcepath, "*");
                            foreach (string dir in dirs)
                                if (!dir.Contains("Tutorial"))
                                    AddNeighborhood(path, dir);
                        }
                    }
                    if (PathProvider.Global.GetExpansion(SimPe.Expansions.LifeStories).Exists)
                    {
                        paths = PathProvider.Global.GetNeighborhoodsForGroup(2);
                        foreach (ExpansionItem.NeighborhoodPath path in paths)
                        {
                            string sourcepath = path.Path;
                            string[] dirs = System.IO.Directory.GetDirectories(sourcepath, "*");
                            foreach (string dir in dirs)
                                if (!dir.Contains("Tutorial"))
                                    AddNeighborhood(path, dir);
                        }
                    }
                }
            }
            finally
            {
                WaitingScreen.UpdateImage(null);
                WaitingScreen.Stop(this);
            }
        }


        public IToolResult Execute(ref SimPe.Interfaces.Files.IPackageFile package, Interfaces.IProviderRegistry prov)
        {
            this.Cursor     = new Avalonia.Input.Cursor(Avalonia.Input.StandardCursorType.Wait);
            this.package    = null;
            this.prov       = prov;
            source_package  = (SimPe.Packages.File)package;
            changed         = false;
            UpdateList();
            this.Cursor     = null;
            pnBackup.IsVisible  = ngbhBUMgr;
            pnOptions.IsVisible = lodesubs;
            RemoteControl.ShowSubForm(this);
            if (this.package != null) package = this.package;
            return new Plugin.ToolResult(false, ((this.package != null) || (changed)));
        }

        class NgbhType
        {
            string name, file; NeighbourhoodTipe type;

            public string FileName { get { return file; } }

            public NgbhType(string file, string name, NeighbourhoodTipe type)
            {
                this.name = name;
                this.type = type;
                this.file = file;
            }

            public override string ToString()
            {
                return type.ToString() + ": " + name;
            }
        }

        private void NgbSelect(object sender, System.EventArgs e)
        {
            var selected = lv.SelectedItem as NeighborhoodItem;
            button2.IsEnabled = selected != null;
            button3.IsEnabled = selected != null;

            cbItems.Clear();
            if (selected != null)
            {
                string path  = System.IO.Path.GetDirectoryName(selected.FilePath);
                string[] files = System.IO.Directory.GetFiles(path, "*.package");

                foreach (string file in files)
                {
                    SimPe.Packages.File pk = SimPe.Packages.File.LoadFromFile(file);
                    NeighbourhoodTipe type;
                    string name = LoadLabel(pk, out type);
                    NgbhType nt = new NgbhType(file, name, type);

                    cbItems.Add(nt);
                    if (Helper.EqualFileName(file, selected.FilePath))
                        cbtypes.SelectedIndex = cbItems.Count - 1;
                }
                if (cbtypes.SelectedIndex < 0 && cbItems.Count > 0)
                    cbtypes.SelectedIndex = 0;
            }
            SetSmilyIcon("none");
        }

        private void NgbOpen(object sender, System.EventArgs e)
        {
            if (lv.SelectedItem == null) return;

            ngbh = cbtypes.SelectedItem as NgbhType;
            if (ngbh != null)
            {
                if (loadNgbh) package = SimPe.Packages.GeneratableFile.LoadFromFile(ngbh.FileName);
                DialogResult = System.Windows.Forms.DialogResult.OK;
                Close();
            }
        }

        protected void CloseIfOpened(string path)
        {
            if (source_package != null)
            {
                if (source_package.SaveFileName.Trim().ToLower().StartsWith(path.ToLower()))
                {
                    if (source_package.Reader != null)
                    {
                        changed = true;
                    }
                }
            }
        }

        private void NgbBackup(object sender, System.EventArgs e)
        {
            var selected = lv.SelectedItem as NeighborhoodItem;
            if (selected == null) return;

            SimPe.Packages.StreamFactory.CloseAll();
            string path  = System.IO.Path.GetDirectoryName(selected.FilePath).Trim();
            string lable = selected.Label;

            //if a File in the current Neighborhood is opened - close it!
            CloseIfOpened(path);
            try
            {
                //create a Backup Folder
                string name = System.IO.Path.GetFileName(path);
                if (lable != "") name = lable + "_" + name;
                long grp = PathProvider.Global.SaveGamePathProvidedByGroup(path);
                if (grp > 1) name = grp.ToString() + "_" + name;

                string backuppath = System.IO.Path.Combine(PathProvider.Global.BackupFolder, name);
                string subname    = DateTime.Now.ToString();
                backuppath        = System.IO.Path.Combine(backuppath, subname.Replace("\\", "-").Replace("/", "-").Replace(":", "-"));
                if (!System.IO.Directory.Exists(backuppath)) System.IO.Directory.CreateDirectory(backuppath);

                Helper.CopyDirectory(path, backuppath, true);
                SetSmilyIcon("happy");
            }
            catch (Exception ex)
            {
                Helper.ExceptionMessage("", ex);
                SetSmilyIcon("sad");
            }
        }

        private void NgbRestoreBackup(object sender, System.EventArgs e)
        {
            var selected = lv.SelectedItem as NeighborhoodItem;
            if (selected == null) return;

            string path = System.IO.Path.GetDirectoryName(selected.FilePath).Trim();

            //if a File in the current Neighborhood is opened - close it!
            CloseIfOpened(path);

            NgbBackup nb = new NgbBackup();
            nb.Text += " (";
            if (selected.Label != "") nb.Text += selected.Label + ": ";
            nb.Text += selected.Name.Trim() + ")";
            if (UserVerification.HaveUserId) nb.Text += " " + NeighborhoodIdentifier(path);
            nb.Execute(path, package, prov, selected.Label);
            UpdateList();
            SetSmilyIcon("none");
        }

        private void cbtypes_SelectedIndexChanged(object sender, EventArgs e)
        {
            btnOpen.IsEnabled = cbtypes.SelectedItem != null;

            if (!lodesubs)
                return;

            System.Drawing.Image newImg = null;

            if (cbtypes.SelectedItem != null)
            {
                ngbh = cbtypes.SelectedItem as NgbhType;
                if (ngbh != null)
                {
                    string name = System.IO.Path.Combine(
                        System.IO.Path.GetDirectoryName(ngbh.FileName),
                        System.IO.Path.GetFileNameWithoutExtension(ngbh.FileName) + ".png"
                    );

                    if (System.IO.File.Exists(name))
                    {
                        try
                        {
                            // Optional: quick sanity check on file size (previews should not be enormous)
                            var fi = new System.IO.FileInfo(name);
                            if (fi.Length > 20 * 1024 * 1024) // 20MB cap
                                throw new Exception("Preview PNG is unexpectedly large: " + fi.Length);

                            using (System.IO.Stream st = System.IO.File.OpenRead(name))
                            using (System.Drawing.Image tmp = Helper.LoadImage(st, true, true))
                            using (System.Drawing.Bitmap bmp = new System.Drawing.Bitmap(tmp))
                            {
                                // Sanity check on dimensions
                                if (bmp.Width > 4096 || bmp.Height > 4096 || bmp.Width <= 0 || bmp.Height <= 0)
                                    throw new Exception($"Preview PNG has invalid size {bmp.Width}x{bmp.Height}");

                                newImg = new System.Drawing.Bitmap(bmp);
                            }
                        }
                        catch
                        {
                            newImg = null;
                        }
                    }
                }
            }

            // Background image not supported on Avalonia controls — dispose preview
            if (newImg != null) newImg.Dispose();
        }

        private void SetSmilyIcon(string hapy)
        {
            uint inst = 0xABBA2585;
            if      (hapy == "none")  { pbox.Source = null; return; }
            else if (hapy == "happy") inst = 0xABBA2575;
            else if (hapy == "sad")   inst = 0xABBA2591;

            SimPe.Packages.File pkg = SimPe.Packages.File.LoadFromFile(
                System.IO.Path.Combine(PathProvider.Global.Latest.InstallFolder, "TSData\\Res\\UI\\ui.package"));
            if (pkg != null)
            {
                SimPe.Interfaces.Files.IPackedFileDescriptor pfd = pkg.FindFile(0x856DDBAC, 0, 0x499DB772, inst);
                if (pfd != null)
                {
                    SimPe.PackedFiles.Wrapper.Picture pic = new SimPe.PackedFiles.Wrapper.Picture();
                    pic.ProcessData(pfd, pkg);
                    pbox.Source = ToAvaloniaBitmap(pic.Image);
                }
                else pbox.Source = null;
            }
            else pbox.Source = null;
        }
    }
}
