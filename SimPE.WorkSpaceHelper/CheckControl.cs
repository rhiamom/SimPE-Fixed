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

// Ported from WinForms UserControl to Avalonia UserControl.
// Layout built programmatically; no AXAML file required.
// WinForms MessageBox calls replaced with SimPe.Message.Show().
// Cursor changes removed (not needed for Avalonia).
// Control-collection iteration replaced with direct field access.

using System;
using System.Windows.Forms;
using GdiImage = System.Drawing.Image;  // disambiguate from Avalonia.Controls.Image
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Layout;

namespace SimPe
{
    public enum CheckItemState { Unknown, Ok, Fail, Warning }

    /// <summary>
    /// Hosts three CheckItem rows (Sims folder, Cache, File table) plus a Run
    /// button.  All business-logic check/fix handlers are preserved verbatim.
    /// Ported to Avalonia; WinForms bits replaced.
    /// </summary>
    public class CheckControl : Avalonia.Controls.UserControl
    {
        // ── Static status images (loaded from embedded resources) ─────────
        static GdiImage LoadFromResource(string name)
        {
            name = "SimPe." + name + ".png";
            System.IO.Stream s = typeof(CheckControl).Assembly.GetManifestResourceStream(name);
            if (s == null) return new System.Drawing.Bitmap(1, 1);
            return Helper.LoadImage(s);
        }

        static GdiImage iok, ifail, iunk, iwarn;

        public static GdiImage OKImage
        {
            get { if (iok   == null) iok   = LoadFromResource("ok");   return iok; }
        }
        public static GdiImage FailImage
        {
            get { if (ifail == null) ifail = LoadFromResource("fail"); return ifail; }
        }
        public static GdiImage UnknownImage
        {
            get { if (iunk  == null) iunk  = LoadFromResource("unk");  return iunk; }
        }
        public static GdiImage WarnImage
        {
            get { if (iwarn == null) iwarn = LoadFromResource("warn"); return iwarn; }
        }

        // ── Controls ──────────────────────────────────────────────────────
        CheckItem chkSimFolder;
        CheckItem chkCache;
        CheckItem chkFileTable;
        Button    button1;

        public CheckControl()
        {
            chkSimFolder = new CheckItem { CanFix = true, Caption = "Sims Folder" };
            chkCache     = new CheckItem { CanFix = true, Caption = "Cache" };
            chkFileTable = new CheckItem { CanFix = true, Caption = "File Table" };
            button1      = new Button { Content = "Run Check" };

            chkSimFolder.CalledCheck += chkSimFolder_CalledCheck;
            chkSimFolder.ClickedFix  += chkSimFolder_ClickedFix;
            chkCache.CalledCheck     += chkCache_CalledCheck;
            chkCache.ClickedFix      += chkCache_ClickedFix;
            chkFileTable.CalledCheck += chkFileTable_CalledCheck;
            chkFileTable.ClickedFix  += chkFileTable_ClickedFix;
            button1.Click            += button1_Click;

            Content = new StackPanel
            {
                Children = { chkSimFolder, chkCache, chkFileTable, button1 }
            };
        }

        public void Reset()
        {
            chkSimFolder.Reset();
            chkCache.Reset();
            chkFileTable.Reset();
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            Reset();
            chkSimFolder.Check();
            chkCache.Check();
            chkFileTable.Check();
        }

        public static void ClearCache()
        {
            string[] files = System.IO.Directory.GetFiles(Helper.SimPeDataPath, "*.simpepkg");
            foreach (string file in files)
            {
                try
                {
                    System.IO.File.Delete(file);
                }
                catch (Exception ex)
                {
                    Helper.ExceptionMessage("", ex);
                }
            }
            Message.Show(SimPe.Localization.GetString("cache_cleared"), "Information", MessageBoxButtons.OK);
        }

        public event System.EventHandler FixedFileTable;

        #region Sims Path Test
        private SimPe.CheckItemState chkSimFolder_CalledCheck(object sender, SimPe.CheckItemState isok)
        {
            isok = CheckItemState.Ok;
            CheckItem ci = sender as CheckItem;
            try
            {
                string test, path;
                foreach (ExpansionItem ei in PathProvider.Global.Expansions)
                {
                    if (!ei.Exists) continue;
                    path = ei.InstallFolder;
                    string name = ei.ExeName;

                    test = System.IO.Path.Combine(path, "TSBin" + Helper.PATH_SEP + name);
                    if (!System.IO.File.Exists(test))
                    {
                        isok = CheckItemState.Fail;
                        ci.Details += SimPe.Localization.GetString("Check: Folder not found").Replace("{name}", ei.Name) + Helper.lbr;
                        ci.Details += "    " + SimPe.Localization.GetString("Check: Unable to locate").Replace("{name}", test) + Helper.lbr + Helper.lbr;
                        continue;
                    }

                    test = System.IO.Path.Combine(path, "TSData" + Helper.PATH_SEP + "Res" + Helper.PATH_SEP + "Objects" + Helper.PATH_SEP + "objects.package");
                    if (!System.IO.File.Exists(test))
                    {
                        isok = CheckItemState.Fail;
                        ci.Details += SimPe.Localization.GetString("Check: Folder not found").Replace("{name}", ei.Name) + Helper.lbr;
                        ci.Details += "    " + SimPe.Localization.GetString("Check: Unable to locate").Replace("{name}", test) + Helper.lbr + Helper.lbr;
                        continue;
                    }
                }

                path = PathProvider.SimSavegameFolder;
                test = System.IO.Path.Combine(path, "Neighborhoods");
                if (!System.IO.Directory.Exists(test))
                {
                    isok = CheckItemState.Fail;
                    ci.Details += SimPe.Localization.GetString("Check: Folder not found").Replace("{name}", SimPe.Localization.GetString("Savegames")) + Helper.lbr;
                    ci.Details += "    " + SimPe.Localization.GetString("Check: Unable to locate").Replace("{name}", test) + Helper.lbr + Helper.lbr;
                }

                if (isok == CheckItemState.Ok)
                {
                    test = Data.MetaData.GMND_PACKAGE;
                    if (!System.IO.File.Exists(test))
                    {
                        isok = CheckItemState.Warning;
                        ci.Details += SimPe.Localization.GetString("Check: CEP not found") + Helper.lbr;
                        ci.Details += "    " + SimPe.Localization.GetString("Check: Unable to locate").Replace("{name}", test) + Helper.lbr + Helper.lbr;
                    }

                    test = Data.MetaData.MMAT_PACKAGE;
                    if (!System.IO.File.Exists(test))
                    {
                        isok = CheckItemState.Warning;
                        ci.Details += SimPe.Localization.GetString("Check: CEP not found") + Helper.lbr;
                        ci.Details += "    " + SimPe.Localization.GetString("Check: Unable to locate").Replace("{name}", test) + Helper.lbr + Helper.lbr;
                    }
                }
            }
            catch (Exception ex)
            {
                isok = CheckItemState.Fail;
                ci.Details = ex.Message;
            }

            return isok;
        }

        private SimPe.CheckItemState chkSimFolder_ClickedFix(object sender, SimPe.CheckItemState isok)
        {
            isok = CheckItemState.Unknown;
            PathProvider.Global.SetDefaultPaths();
            if (Helper.Profile.Length > 0)
                Message.Show("You will need to re-save profile " + Helper.Profile, "Fix", MessageBoxButtons.OK);
            return isok;
        }
        #endregion

        #region Cache Test
        private SimPe.CheckItemState chkCache_CalledCheck(object sender, SimPe.CheckItemState isok)
        {
            isok = CheckItemState.Ok;
            CheckItem ci = sender as CheckItem;
            try
            {
                SimPe.Cache.CacheFile cf = new SimPe.Cache.CacheFile();
                string path = Helper.SimPeCache;
                try
                {
                    cf.Load(path);
                }
                catch (Exception ex)
                {
                    ci.Details += SimPe.Localization.GetString("Check: Unable to load cache") + Helper.lbr;
                    ci.Details += "    " + SimPe.Localization.GetString("Check: Error while load").Replace("{name}", path) + Helper.lbr;
                    ci.Details += "    " + ex.Message + Helper.lbr + Helper.lbr;
                }

                path = Helper.SimPeLanguageCache;
                try
                {
                    cf.Load(path);
                }
                catch (Exception ex)
                {
                    ci.Details += SimPe.Localization.GetString("Check: Unable to load cache") + Helper.lbr;
                    ci.Details += "    " + SimPe.Localization.GetString("Check: Error while load").Replace("{name}", path) + Helper.lbr;
                    ci.Details += "    " + ex.Message + Helper.lbr + Helper.lbr;
                }
            }
            catch (Exception ex)
            {
                isok = CheckItemState.Fail;
                ci.Details = ex.Message;
            }

            return isok;
        }

        private SimPe.CheckItemState chkCache_ClickedFix(object sender, SimPe.CheckItemState isok)
        {
            isok = CheckItemState.Unknown;
            ClearCache();
            return isok;
        }
        #endregion

        #region Filetable Test
        private SimPe.CheckItemState chkFileTable_CalledCheck(object sender, SimPe.CheckItemState isok)
        {
            isok = CheckItemState.Ok;
            FileTable.FileIndex.Load();
            CheckItem ci = sender as CheckItem;
            try
            {
                SimPe.Interfaces.Scenegraph.IScenegraphFileIndexItem[] items = FileTable.FileIndex.FindFile(Data.MetaData.OBJD_FILE, true);
                if (items.Length < 3000)
                {
                    ci.Details += SimPe.Localization.GetString("Check: No Objects") + Helper.lbr;
                    isok = CheckItemState.Fail;
                }
                else
                {
                    items = FileTable.FileIndex.FindFile(Data.MetaData.OBJD_FILE, 0x7F94AFE8, 0x000041AB, null);
                    if (items.Length == 0)
                    {
                        ci.Details += SimPe.Localization.GetString("Check: No Objects") + Helper.lbr;
                        isok = CheckItemState.Fail;
                    }
                }

                items = FileTable.FileIndex.FindFile(Data.MetaData.TXMT, true);
                if (items.Length < 100)
                {
                    ci.Details += SimPe.Localization.GetString("Check: No Textures") + Helper.lbr;
                    isok = CheckItemState.Fail;
                }
            }
            catch (Exception ex)
            {
                isok = CheckItemState.Fail;
                ci.Details = ex.Message;
            }

            return isok;
        }

        private SimPe.CheckItemState chkFileTable_ClickedFix(object sender, SimPe.CheckItemState isok)
        {
            isok = CheckItemState.Unknown;
            try
            {
                string msg = "your file table folder settings will be reset";
                if (Helper.Profile.Length > 0) msg += " and you will need to re-save profile " + Helper.Profile;
                if (Message.Show(
                        "The File table settings file was not correct and you have asked to fix it.\n" +
                        Helper.DataFolder.FoldersXREG + "\n" +
                        "SimPE can generate a new one (" + msg + ").\n\n" +
                        "Should SimPe delete the File table settings File?",
                        "Fix",
                        MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    System.IO.File.Delete(Helper.DataFolder.FoldersXREG);
                    FileTable.Reload();
                    if (FixedFileTable != null) FixedFileTable(this, new EventArgs());
                }
            }
            catch
            {
                isok = CheckItemState.Fail;
            }
            return isok;
        }
        #endregion
    }
}
