/***************************************************************************
 *   Copyright (C) 2005 by Ambertation                                     *
 *   quaxi@ambertation.de                                                  *
 *                                                                         *
 *   Copyright (C) 2026 by GramzeSweatshop                                 *
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
using System.Collections.Generic;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace SimPe
{
    /// <summary>
    /// Zusammenfassung f�r OptionForm.
    /// </summary>
    public partial class OptionForm : System.Windows.Forms.Form
    {
        

        public OptionForm()
        {
            Application.UseWaitCursor = true;
            try
            {
                Application.DoEvents();
                //
                // Erforderlich f�r die Windows Form-Designerunterst�tzung
                //
                InitializeComponent();

                this.cbRLExt.ResourceManager = SimPe.Localization.Manager;
                this.cbRLNames.ResourceManager = SimPe.Localization.Manager;
                this.cbRLTGI.ResourceManager = SimPe.Localization.Manager;

                this.cbRLExt.Enum = typeof(SimPe.Registry.ResourceListExtensionFormats);
                this.cbRLNames.Enum = typeof(SimPe.Registry.ResourceListFormats);
                this.cbRLTGI.Enum = typeof(SimPe.Registry.ResourceListUnnamedFormats);
                //this.pgPaths.SelectedObject = SimPe.PathSettings.Global;


                for (byte i = 1; i < 0x44; i++) this.cblang.Items.Add(new SimPe.PackedFiles.Wrapper.StrLanguage(i));
                tc.SelectedTab = tpSettings;

                SimPe.GuiTheme[] gts = (SimPe.GuiTheme[])System.Enum.GetValues(typeof(SimPe.GuiTheme));
                foreach (SimPe.GuiTheme gt in gts) cbThemes.Items.Add(gt);
                cbThemes.SelectedIndex = 0;

                SimPe.Registry.ReportFormats[] rfs = (SimPe.Registry.ReportFormats[])System.Enum.GetValues(typeof(SimPe.Registry.ReportFormats));
                foreach (SimPe.Registry.ReportFormats rf in rfs) cbReport.Items.Add(rf);
                cbReport.SelectedIndex = 0;

                foreach (SimPe.Interfaces.ISettings settings in FileTable.SettingsRegistry.Settings)
                    this.cbCustom.Items.Add(settings);
                if (cbCustom.Items.Count > 0) cbCustom.SelectedIndex = 0;

            }
            finally { Application.UseWaitCursor = false; }
        }


        void Execute()
        {
            this.Tag = true;
            //linkLabel3.Enabled = (Helper.XmlRegistry.EPInstalled>=1);
            tbgame.Text = PathProvider.Global[Expansions.BaseGame].InstallFolder;
            tbep1.Text = PathProvider.Global[Expansions.University].InstallFolder;
            tbep2.Text = PathProvider.Global[Expansions.Business].InstallFolder;
            //tbep1.Text = Helper.XmlRegistry.RealEP1GamePath;
            tbsavegame.Text = PathProvider.SimSavegameFolder;
            tbdds.Text = PathProvider.Global.NvidiaDDSPath;
            this.cbdebug.Checked = Helper.XmlRegistry.GameDebug;
            cbautobak.Checked = Helper.XmlRegistry.AutoBackup;
            cbblur.Checked = Helper.XmlRegistry.BlurNudity;
            cbsound.Checked = Helper.XmlRegistry.EnableSound;
            cbwait.Checked = Helper.XmlRegistry.WaitingScreen;
            cbow.Checked = Helper.XmlRegistry.LoadOWFast;
            cbsilent.Checked = Helper.XmlRegistry.Silent;
            cbcache.Checked = Helper.XmlRegistry.UseCache;
            cbshowobjd.Checked = Helper.XmlRegistry.ShowObjdNames;
            cbhidden.Checked = Helper.XmlRegistry.HiddenMode;
            cbjointname.Checked = Helper.XmlRegistry.ShowJointNames;
            tbthumb.Text = Helper.XmlRegistry.OWThumbSize.ToString();
            tbscale.Text = Helper.XmlRegistry.ImportExportScaleFactor.ToString();
            //cbupdate.Checked = Helper.XmlRegistry.CheckForUpdates;
            cbpkgmaint.Checked = Helper.XmlRegistry.UsePackageMaintainer;
            cbmulti.Checked = Helper.XmlRegistry.MultipleFiles;
            cbSimple.Checked = Helper.XmlRegistry.SimpleResourceSelect;
            cbFirefox.Checked = Helper.XmlRegistry.FirefoxTabbing;
            cbDeep.Checked = Helper.XmlRegistry.DeepSimScan;
            cbSimTemp.Checked = Helper.XmlRegistry.DeepSimTemplateScan;
            cbAsync.Checked = Helper.XmlRegistry.AsynchronLoad;

            cbLock.Checked = Helper.XmlRegistry.LockDocks;
            cbsplash.Checked = Helper.XmlRegistry.ShowStartupSplash;
            cbAsyncSort.Checked = Helper.XmlRegistry.AsynchronSort;
            cbRLTGI.SelectedValue = Helper.XmlRegistry.ResourceListUnknownDescriptionFormat;
            cbRLNames.SelectedValue = Helper.XmlRegistry.ResourceListFormat;
            cbRLExt.SelectedValue = Helper.XmlRegistry.ResourceListExtensionFormat;

            this.cbLock_CheckedChanged(cbLock, null);

            /*this.tbUserId.Text = "0x" + Helper.HexString(Helper.XmlRegistry.CachedUserId);
            this.tbUsername.Text = Helper.XmlRegistry.Username;
            this.tbPassword.Text = Helper.XmlRegistry.Password;*/

            this.tbep1.ReadOnly = (PathProvider.Global.EPInstalled < 1);
            this.tbep2.ReadOnly = (PathProvider.Global.EPInstalled < 2);
            this.button5.Enabled = (PathProvider.Global.EPInstalled >= 1);
            this.btNightlife.Enabled = (PathProvider.Global.EPInstalled >= 2);
            llsetep1.Enabled = button5.Enabled;
            llNightlife.Enabled = btNightlife.Enabled;

            if (((byte)Helper.XmlRegistry.LanguageCode <= cblang.Items.Count) && ((byte)Helper.XmlRegistry.LanguageCode > 0))
            {
                cblang.SelectedIndex = (byte)Helper.XmlRegistry.LanguageCode - 1;
            }

            //Favorite Theme
            GuiTheme gt = (GuiTheme)Helper.XmlRegistry.Layout.SelectedTheme;
            for (int i = 0; i < cbThemes.Items.Count; i++)
                if ((GuiTheme)cbThemes.Items[i] == gt)
                    cbThemes.SelectedIndex = i;

            //Report Format
            SimPe.Registry.ReportFormats rf = (SimPe.Registry.ReportFormats)Helper.XmlRegistry.ReportFormat;
            for (int i = 0; i < cbReport.Items.Count; i++)
                if ((SimPe.Registry.ReportFormats)cbReport.Items[i] == rf)
                    cbReport.SelectedIndex = i;

            //state
            cbSimTemp.Enabled = cbDeep.Checked;

            this.Tag = null;
            this.ShowDialog();
        }

        private void SaveOptionsClick(object sender, System.EventArgs e)
        {
            /*Helper.XmlRegistry.SimsPath = this.tbgame.Text;
            Helper.XmlRegistry.SimsEP1Path = this.tbep1.Text;
            Helper.XmlRegistry.SimsEP2Path = this.tbep2.Text;
            Helper.XmlRegistry.SimSavegameFolder = this.tbsavegame.Text;*/
            PathProvider.Global.NvidiaDDSPath = tbdds.Text;
            Helper.XmlRegistry.LanguageCode = (Data.MetaData.Languages)cblang.SelectedIndex + 1;
            Helper.XmlRegistry.GameDebug = cbdebug.Checked;
            Helper.XmlRegistry.AutoBackup = cbautobak.Checked;
            //Helper.XmlRegistry.BlurNudity = cbblur.Checked;
            Helper.XmlRegistry.EnableSound = cbsound.Checked;
            Helper.XmlRegistry.WaitingScreen = cbwait.Checked;
            Helper.XmlRegistry.LoadOWFast = cbow.Checked;
            Helper.XmlRegistry.Silent = cbsilent.Checked;
            Helper.XmlRegistry.UseCache = cbcache.Checked;
            Helper.XmlRegistry.ShowObjdNames = cbshowobjd.Checked;
            Helper.XmlRegistry.HiddenMode = cbhidden.Checked;
            Helper.XmlRegistry.ShowJointNames = cbjointname.Checked;
            //Helper.XmlRegistry.CheckForUpdates = cbupdate.Checked;
            Helper.XmlRegistry.UsePackageMaintainer = cbpkgmaint.Checked;
            Helper.XmlRegistry.MultipleFiles = cbmulti.Checked;
            Helper.XmlRegistry.Layout.SelectedTheme = (byte)cbThemes.Items[cbThemes.SelectedIndex];
            Helper.XmlRegistry.SimpleResourceSelect = cbSimple.Checked;
            Helper.XmlRegistry.FirefoxTabbing = cbFirefox.Checked;
            Helper.XmlRegistry.DeepSimScan = cbDeep.Checked;
            Helper.XmlRegistry.DeepSimTemplateScan = cbSimTemp.Checked;
            Helper.XmlRegistry.AsynchronLoad = cbAsync.Checked;
            Helper.XmlRegistry.ReportFormat = (Registry.ReportFormats)cbReport.SelectedItem;
            Helper.XmlRegistry.LockDocks = cbLock.Checked;
            Helper.XmlRegistry.ShowStartupSplash = cbsplash.Checked;

            Helper.XmlRegistry.AsynchronSort = cbAsyncSort.Checked;
            Helper.XmlRegistry.ResourceListExtensionFormat = (Registry.ResourceListExtensionFormats)cbRLExt.SelectedValue;
            Helper.XmlRegistry.ResourceListFormat = (Registry.ResourceListFormats)cbRLNames.SelectedValue;
            Helper.XmlRegistry.ResourceListUnknownDescriptionFormat = (Registry.ResourceListUnnamedFormats)cbRLTGI.SelectedValue;

            //Helper.XmlRegistry.Username = tbUsername.Text;
            //Helper.XmlRegistry.Password = tbPassword.Text;
            //Helper.XmlRegistry.CachedUserId = Helper.StringToUInt32(tbUserId.Text, 0, 16);

            try
            {
                Helper.XmlRegistry.OWThumbSize = Convert.ToInt32(tbthumb.Text);
                Helper.XmlRegistry.ImportExportScaleFactor = Convert.ToSingle(tbscale.Text);
            }
            catch { }

            Helper.XmlRegistry.Flush();

            Close();
        }

        private void button2_Click(object sender, System.EventArgs e)
        {
            if (System.IO.Directory.Exists(tbgame.Text)) fbd.SelectedPath = tbgame.Text;
            if (fbd.ShowDialog() == DialogResult.OK) this.tbgame.Text = fbd.SelectedPath;
        }

        private void button3_Click(object sender, System.EventArgs e)
        {
            if (System.IO.Directory.Exists(tbsavegame.Text)) fbd.SelectedPath = tbsavegame.Text;
            if (fbd.ShowDialog() == DialogResult.OK) this.tbsavegame.Text = fbd.SelectedPath;
        }

        private void button4_Click(object sender, System.EventArgs e)
        {
            if (System.IO.Directory.Exists(tbdds.Text)) ofd.InitialDirectory = tbdds.Text;
            if (ofd.ShowDialog() == DialogResult.OK) this.tbdds.Text = System.IO.Path.GetDirectoryName(ofd.FileName);
        }

        private void label2_LinkClicked(object sender, System.Windows.Forms.LinkLabelLinkClickedEventArgs e)
        {
            tbgame.Text = PathProvider.Global[Expansions.BaseGame].RealInstallFolder;
        }

        private void linkLabel3_LinkClicked(object sender, System.Windows.Forms.LinkLabelLinkClickedEventArgs e)
        {
            tbep1.Text = PathProvider.Global[Expansions.University].RealInstallFolder;
        }

        private void linkLabel4_LinkClicked(object sender, System.Windows.Forms.LinkLabelLinkClickedEventArgs e)
        {
            tbsavegame.Text = PathProvider.RealSavegamePath;
        }

        private void button5_Click(object sender, System.EventArgs e)
        {
            if (System.IO.Directory.Exists(tbep1.Text)) fbd.SelectedPath = tbep1.Text;
            if (fbd.ShowDialog() == DialogResult.OK) this.tbep1.Text = fbd.SelectedPath;
        }

        private void ClearCaches(object sender, System.EventArgs e)
        {
            SimPe.CheckControl.ClearCache();
        }

        private void DDSChanged(object sender, System.EventArgs e)
        {
            string name = System.IO.Path.Combine(this.tbdds.Text, "nvdxt.exe");
            lldds.Visible = (!System.IO.File.Exists(name));
            lldds2.Visible = lldds.Visible;
        }

        private void LoadDDS(object sender, System.Windows.Forms.LinkLabelLinkClickedEventArgs e)
        {
            System.Windows.Forms.Help.ShowHelp(this, "http://developer.nvidia.com/object/nv_texture_tools.html");
        }

        private void visualStyleLinkLabel1_LinkClicked(object sender, System.Windows.Forms.LinkLabelLinkClickedEventArgs e)
        {
            tbep1.Text = SimPe.PathProvider.Global[Expansions.University].RealInstallFolder;
        }

        void EnablePanel(TabPage page)
        {
            tc.SelectedTab = page;
        }

        private void SelectCategory(object sender, System.EventArgs e)
        {
            // No longer needed — TabControl handles selection natively
            // Kept as stub in case something calls it
        }

        private void ChangedThemeHandler(object sender, System.EventArgs e)
        {
            if (NewTheme != null) NewTheme((SimPe.GuiTheme)cbThemes.Items[cbThemes.SelectedIndex]);
        }

        private void ResetLayoutClick(object sender, System.EventArgs e)
        {
            if (ResetLayout != null) ResetLayout(this, e);
        }

        private void LockDocksClick(object sender, System.EventArgs e)
        {
            if (LockDocks != null) LockDocks(this, e);
        }

        private void UnlockDocksClick(object sender, System.EventArgs e)
        {
            if (UnlockDocks != null) UnlockDocks(this, e);
        }

        #region Events
        public event SimPe.Events.ChangedThemeEvent NewTheme;
        public event System.EventHandler ResetLayout;
        public event System.EventHandler LockDocks;
        public event System.EventHandler UnlockDocks;
        #endregion

        #region Plugins
        public Image GetImage(SimPe.Interfaces.IWrapper wrapper)
        {
            if (uids.Contains(wrapper.WrapperDescription.UID))
                return Helper.LoadImage(typeof(SimPe.Helper).Assembly.GetManifestResourceStream("SimPe.IconXmlResources.error.png"));

            if (wrapper.Priority >= 0)
                return Helper.LoadImage(typeof(SimPe.Helper).Assembly.GetManifestResourceStream("SimPe.IconXmlResources.enabled.png"));

            return Helper.LoadImage(typeof(SimPe.Helper).Assembly.GetManifestResourceStream("SimPe.IconXmlResources.disabled.png"));
        }

        internal void SetPanel(SimPe.Interfaces.IWrapper wrapper, PluginPanel pn)
        {


            if (wrapper.Priority < 0)
            {
                pn.Text = "(" + wrapper.WrapperDescription.Name + ")";
                pn.ForeColor = SystemColors.ControlDarkDark;
            }
            else
            {
                pn.Text = wrapper.WrapperDescription.Name;
                pn.ForeColor = SystemColors.ControlText;
            }
            pn.Text = "     " + pn.Text;

        }

        internal Image GetShrinkImage(PluginPanel pn)
        {
            if (pn.Height == pn.DisplayRectangle.Top + 1)
            {
                return Helper.LoadImage(typeof(SimPe.Helper).Assembly.GetManifestResourceStream("SimPe.IconXmlResources.expand.png"));
            }
            else
            {
                return Helper.LoadImage(typeof(SimPe.Helper).Assembly.GetManifestResourceStream("SimPe.IconXmlResources.shrink.png"));
            }

        }

        public bool ThumbnailCallback()
        {
            return false;
        }


        System.Collections.ArrayList uids;
        System.Collections.ArrayList wrappers;
#if DEBUG
        const int height = 148;
#else
		const int height = 116;
#endif
        public Control BuildPanel(SimPe.Interfaces.IWrapper wrapper, ThemeManager tm, int index)
        {
            if (uids == null) uids = new ArrayList();
            if (wrappers == null) wrappers = new ArrayList();

            if (wrapper.Priority >= 0) wrapper.Priority = index + 1;
            else wrapper.Priority = -1 * (index + 1);



            const int imgwidth = 22;
            int top = 4 + index * (height + 4);
            PluginPanel pn = new PluginPanel();
            pn.Parent = cnt;
            pn.Top = top;
            pn.Left = 4;
            pn.Width = cnt.Width - System.Windows.Forms.SystemInformation.VerticalScrollBarWidth - 2 - 2 * pn.Left;
            pn.Height = height;
            pn.Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Right;
            pn.Click += new EventHandler(pn_Click);
            pn.LostFocus += new EventHandler(pn_LostFocus);
            pn.GotFocus += new EventHandler(pn_Focused);
            pn.Enter += new EventHandler(pn_Focused);
            pn.Leave += new EventHandler(pn_LostFocus);
            pn_LostFocus(pn, null);
            SetPanel(wrapper, pn);
            pn.Tag = wrapper;
            pn.Dock = DockStyle.Top;

            wrappers.Add(pn);

            #region Author
            Label lbauthor = new Label();
            lbauthor.Parent = pn;
            lbauthor.Top = pn.DisplayRectangle.Top + 8;
            lbauthor.Left = 8;
            lbauthor.Text = "Author:";
            lbauthor.Width = 85;
            lbauthor.Font = new Font(cnt.Font.Name, cnt.Font.Size, FontStyle.Bold, cnt.Font.Unit);
            lbauthor.Height = (int)lbauthor.Font.SizeInPoints * 2;
            lbauthor.ForeColor = SystemColors.ControlDarkDark;
            lbauthor.TextAlign = ContentAlignment.TopRight;
            lbauthor.Click += new EventHandler(pn_Click);

            Label lb = new Label();
            lb.Parent = pn;
            lb.Top = lbauthor.Top;
            lb.Left = lbauthor.Right + 4;
            lb.AutoSize = true;
            lb.Text = wrapper.WrapperDescription.Author;
            lb.Font = new Font(cnt.Font.Name, cnt.Font.Size, FontStyle.Regular, cnt.Font.Unit);
            lb.Height = lbauthor.Height;
            lb.ForeColor = lbauthor.ForeColor;
            lb.Click += new EventHandler(pn_Click);
            #endregion

            #region Version
            Label lbversion = new Label();
            lbversion.Parent = pn;
            lbversion.Top = lbauthor.Top;
            lbversion.Left = lb.Right + 16;
            lbversion.Width = lbauthor.Width;
            lbversion.Height = lbauthor.Height;
            lbversion.Text = "Version:";
            lbversion.Font = lbauthor.Font;
            lbversion.ForeColor = lbauthor.ForeColor;
            lbversion.TextAlign = lbauthor.TextAlign;
            lbversion.Click += new EventHandler(pn_Click);

            lb = new Label();
            lb.Parent = pn;
            lb.Top = lbversion.Top;
            lb.Left = lbversion.Right + 4;
            lb.AutoSize = true;
            lb.Text = wrapper.WrapperDescription.Version.ToString();
            lb.Font = new Font(cnt.Font.Name, cnt.Font.Size, FontStyle.Regular, cnt.Font.Unit);
            lb.Height = lbauthor.Height;
            lb.ForeColor = lbauthor.ForeColor;
            lb.Click += new EventHandler(pn_Click);
            #endregion

            #region FileName
            Label lbfile = new Label();
            lbfile.Parent = pn;
            lbfile.Top = lbversion.Bottom;
            lbfile.Left = lbauthor.Left;
            lbfile.Width = lbauthor.Width;
            lbfile.Height = lbauthor.Height;
            lbfile.Text = "Filename:";
            lbfile.Font = lbauthor.Font;
            lbfile.ForeColor = lbauthor.ForeColor;
            lbfile.TextAlign = lbauthor.TextAlign;
            lbfile.Click += new EventHandler(pn_Click);

            lb = new Label();
            lb.Parent = pn;
            lb.Top = lbfile.Top;
            lb.Left = lbfile.Right + 4;
            lb.AutoSize = true;
            lb.Text = wrapper.WrapperFileName;
            lb.Font = new Font(cnt.Font.Name, cnt.Font.Size, FontStyle.Regular, cnt.Font.Unit);
            lb.Height = lbauthor.Height;
            lb.ForeColor = lbauthor.ForeColor;
            lb.Click += new EventHandler(pn_Click);
            #endregion

            #region UID
            Label lbui = new Label();
            lbui.Parent = pn;
            lbui.Top = lbfile.Bottom;
            lbui.Left = lbauthor.Left;
            lbui.Width = lbauthor.Width;
            lbui.Height = lbauthor.Height;
            lbui.Text = "UID:";
            lbui.Font = lbauthor.Font;
            lbui.ForeColor = lbauthor.ForeColor;
            lbui.TextAlign = lbauthor.TextAlign;
            lbui.Click += new EventHandler(pn_Click);

            lb = new Label();
            lb.Parent = pn;
            lb.Top = lbui.Top;
            lb.Left = lbui.Right + 4;
            lb.AutoSize = true;
            lb.Text = "0x" + Helper.HexString(wrapper.WrapperDescription.UID);
            lb.Font = new Font(cnt.Font.Name, cnt.Font.Size, FontStyle.Regular, cnt.Font.Unit);
            lb.Height = lbauthor.Height;
            lb.ForeColor = lbauthor.ForeColor;
            lb.Click += new EventHandler(pn_Click);
            #endregion

            #region Description
            Label lbdesc = new Label();
            lbdesc.Parent = pn;
            lbdesc.Top = lbui.Bottom;
            lbdesc.Left = lbauthor.Left;
            lbdesc.Width = lbauthor.Width;
            lbdesc.Height = lbauthor.Height;
            lbdesc.Text = "Description:";
            lbdesc.Font = lbauthor.Font;
            lbdesc.ForeColor = lbauthor.ForeColor;
            lbdesc.TextAlign = lbauthor.TextAlign;
            lbdesc.Click += new EventHandler(pn_Click);

            TextBox tb = new TextBox();
            tb.Parent = pn;
            tb.Top = lbdesc.Top;
            tb.Left = lbdesc.Right + 4;
            tb.Width = pn.Width - lb.Left - imgwidth - 12;
            tb.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Right;
            tb.Text = wrapper.WrapperDescription.Description;
#if DEBUG
            tb.Text += Helper.lbr + wrapper.GetType().FullName + " " + wrapper.GetType().Assembly.FullName;
#endif
            tb.Multiline = true;
            tb.WordWrap = true;
            tb.ScrollBars = ScrollBars.Vertical;
            tb.BorderStyle = BorderStyle.None;
            tb.BackColor = pn.BackColor;

            tb.Font = new Font(cnt.Font.Name, cnt.Font.Size, FontStyle.Regular, cnt.Font.Unit);
            tb.Height = pn.Height - tb.Top - 8;
            tb.ForeColor = lbauthor.ForeColor;
            tb.GotFocus += new EventHandler(tb_GotFocus);
            tb.Enter += new EventHandler(tb_GotFocus);
            tb.ReadOnly = true;
            #endregion

            PictureBox pb = null;

            if (wrapper.AllowMultipleInstances)
            {
                pb = new PictureBox();
                pb.Parent = pn;
                pb.Width = imgwidth;
                pb.Height = imgwidth;
                pb.Left = pn.Width - 2 * pb.Width - 16;
                pb.Top = pn.DisplayRectangle.Top + 4; //pn.DisplayRectangle.Top + 4 + pb.Height + 4; //pn.Height - 2*pb.Height -16;
                pb.Anchor = AnchorStyles.Top | AnchorStyles.Right;
                var stream = typeof(SimPe.Helper).Assembly.GetManifestResourceStream("SimPe.IconXmlResources.multienabled.png");
                if (stream != null)
                    pb.Image = Helper.LoadImage(stream);
                pb.Click += new EventHandler(pn_Click);
                this.toolTip1.SetToolTip(pb, "Allows Multiple instance");


                pb = new PictureBox();
                pb.Parent = pn;
                pb.Width = pn.DisplayRectangle.Top + 1;
                pb.Height = pn.DisplayRectangle.Top;
                pb.SizeMode = PictureBoxSizeMode.CenterImage;
                pb.Top = (pn.DisplayRectangle.Top + 1 - pb.Height) / 2;
                pb.Left = pn.Width - 3 * pb.Width - pb.Top;
                pb.Anchor = AnchorStyles.Top | AnchorStyles.Right;
                var asm = System.Reflection.Assembly.Load("simpe.helper");
                asm.GetManifestResourceStream("SimPe.IconXml.multienabled.png");
                if (stream != null)
                    pb.Image = Helper.LoadImage(stream);
                pb.BackColor = Color.Transparent;

                this.toolTip1.SetToolTip(pb, "Allows Multiple instance.");
            }

            if (wrapper is SimPe.PackedFiles.Wrapper.ErrorWrapper)
            {
                pb = new PictureBox();
                pb.Parent = pn;
                pb.Width = pn.DisplayRectangle.Top + 1;
                pb.Height = pn.DisplayRectangle.Top;
                pb.SizeMode = PictureBoxSizeMode.CenterImage;
                pb.Top = (pn.DisplayRectangle.Top + 1 - pb.Height) / 2;
                if (wrapper.AllowMultipleInstances) pb.Left = pn.Width - 4 * pb.Width - pb.Top;
                else pb.Left = pn.Width - 3 * pb.Width - pb.Top;
                pb.Anchor = AnchorStyles.Top | AnchorStyles.Right;
                pb.Image = Helper.LoadImage(this.GetType().Assembly.GetManifestResourceStream("SimPe.IconXml.error.png")).GetThumbnailImage(16, 16, new Image.GetThumbnailImageAbort(ThumbnailCallback), IntPtr.Zero); ;
                pb.BackColor = Color.Transparent;
                this.toolTip1.SetToolTip(pb, "This wrapper caused an Error while loading.");
            }

            PictureBox ipb = new PictureBox();
            ipb.Parent = pn;
            ipb.Left = 2;
            ipb.Top = 1;
            ipb.BackColor = Color.Transparent;
            ipb.SizeMode = PictureBoxSizeMode.StretchImage;
            if (wrapper.WrapperDescription.Icon != null)
            {
                ipb.Height = Math.Min(wrapper.WrapperDescription.Icon.Height, pn.DisplayRectangle.Top - 2);
                ipb.Width = Math.Min(wrapper.WrapperDescription.Icon.Width, pn.DisplayRectangle.Top - 2);
                ipb.Image = wrapper.WrapperDescription.Icon;
            }
            else
            {
                ipb.Height = 16;
                ipb.Width = 16;
                //				ipb.Image = FileTable.WrapperRegistry.WrapperImageList.Images[1];
            }

            PictureBox pbe = new PictureBox();
            pbe.Parent = pn;
            pbe.Width = pn.DisplayRectangle.Top + 1;
            pbe.Height = pn.DisplayRectangle.Top;
            pbe.SizeMode = PictureBoxSizeMode.CenterImage;
            pbe.Top = (pn.DisplayRectangle.Top + 1 - pbe.Height) / 2;
            pbe.Left = pn.Width - pbe.Width - pbe.Top;
            pbe.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            pbe.Image = GetShrinkImage(pn);
            pbe.Tag = pn;
            pbe.BackColor = Color.Transparent;
            pbe.Click += new EventHandler(pb_ExpandClick);
            this.toolTip1.SetToolTip(pbe, "Collapse/Expand");

            pb = new PictureBox();
            pb.Parent = pn;
            pb.Width = imgwidth;
            pb.Height = imgwidth;
            pb.Left = pn.Width - pb.Width - 8;
            pb.Top = pn.DisplayRectangle.Top + 4; //pn.Height - pb.Height -8;
            pb.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            pb.Image = GetImage(wrapper);
            pb.Tag = pn;
            pb.BackColor = Color.Transparent;
            pb.Click += new EventHandler(pb_Click);
            this.toolTip1.SetToolTip(pb, "Enable/Disable");

            pb = new PictureBox();
            pb.Parent = pn;
            pb.Width = pn.DisplayRectangle.Top + 1;
            pb.Height = pn.DisplayRectangle.Top;
            pb.SizeMode = PictureBoxSizeMode.CenterImage;
            pb.Top = (pn.DisplayRectangle.Top + 1 - pb.Height) / 2;
            pb.Left = pn.Width - 2 * pb.Width - pb.Top;
            pb.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            pb.Image = GetImage(wrapper).GetThumbnailImage(16, 16, new Image.GetThumbnailImageAbort(ThumbnailCallback), IntPtr.Zero);
            pb.BackColor = Color.Transparent;
            pb.Tag = pn;
            pb.Click += new EventHandler(pb_Click);
            this.toolTip1.SetToolTip(pb, "Enable/Disable");

            Panel pan = new Panel();
            pan.BackColor = this.cnt.BackColor;
            pan.Parent = pn;
            pan.Height = 4;
            pan.Dock = DockStyle.Bottom;

            uids.Add(wrapper.WrapperDescription.UID);
            pb_ExpandClick(pbe, null);
            return pn;
        }

        public void Execute(Icon icon)
        {
            ThemeManager tm = new ThemeManager(ThemeManager.Global.CurrentTheme);
            tm.Parent = ThemeManager.Global;

            OptionForm f = this;
            if (icon != null) f.Icon = icon;
            SimPe.Interfaces.IWrapper[] wrappers = FileTable.WrapperRegistry.AllWrappers;

            for (int ct = wrappers.Length - 1; ct >= 0; ct--)
            {
                SimPe.Interfaces.IWrapper wrapper = wrappers[ct];
                f.cnt.Controls.Add(f.BuildPanel(wrapper, tm, ct));
            }

            f.uids.Clear();
            if (f.cnt.Controls.Count > 0) f.cnt.Controls[0].Focus();

            f.Execute();

            foreach (SimPe.Interfaces.IWrapper wrapper in wrappers)
            {
                if (!(wrapper is SimPe.PackedFiles.Wrapper.ErrorWrapper))
                    Helper.XmlRegistry.SetWrapperPriority(wrapper.WrapperDescription.UID, wrapper.Priority);
            }
        }

        private void pn_Click(object sender, EventArgs e)
        {
            if (sender is PluginPanel)
            {
                PluginPanel pn = (PluginPanel)sender;
                pn.Focus();
            }
            else if (sender is Control)
            {
                PluginPanel pn = (PluginPanel)((Control)sender).Parent;
                pn.Focus();
            }
        }

        PluginPanel lastpn;
        private void pn_Focused(object sender, EventArgs e)
        {
            PluginPanel pn = (PluginPanel)sender;
            pn.BackColor = SystemColors.Window;
            pn.Font = new Font(pn.Font.Name, pn.Font.Size, FontStyle.Bold, pn.Font.Unit);

            btpup.Enabled = wrappers[0] != pn;
            btpdown.Enabled = wrappers[wrappers.Count - 1] != pn;

            lastpn = pn;
            if (pn.Controls.Count > 9) pn.Controls[9].BackColor = pn.BackColor;
        }

        private void pn_LostFocus(object sender, EventArgs e)
        {
            PluginPanel pn = (PluginPanel)sender;
            pn.BackColor = SystemColors.ControlLight;
            pn.Font = new Font(pn.Font.Name, pn.Font.Size, FontStyle.Regular, pn.Font.Unit);
            if (pn.Controls.Count > 9) pn.Controls[9].BackColor = pn.BackColor;
        }

        private void pb_Click(object sender, EventArgs e)
        {
            PictureBox pb = (PictureBox)sender;
            PluginPanel pn = (PluginPanel)pb.Tag;
            SimPe.Interfaces.IWrapper wrapper = (SimPe.Interfaces.IWrapper)pn.Tag;

            wrapper.Priority *= -1;
            SetPanel(wrapper, pn);

            Image i = this.GetImage(wrapper);

            SetBackgroundColor(pn.Controls[pn.Controls.Count - 2], i, true);
            SetBackgroundColor(pn.Controls[pn.Controls.Count - 3], i, false);
        }

        int FindPanelIndex(PluginPanel pn)
        {
            for (int i = 0; i < wrappers.Count; i++)
            {
                if (wrappers[i] == pn) return i;
            }

            return -1;
        }

        void Exchange(int index, int o)
        {
            PluginPanel pn1 = (PluginPanel)wrappers[index];
            PluginPanel pn2 = (PluginPanel)wrappers[index + o];

            int d = pn1.Top;
            pn1.Top = pn2.Top;
            pn2.Top = d;
            SimPe.Interfaces.IWrapper w1 = (SimPe.Interfaces.IWrapper)pn1.Tag;
            SimPe.Interfaces.IWrapper w2 = (SimPe.Interfaces.IWrapper)pn2.Tag;

            int p1 = w1.Priority;
            int p2 = w2.Priority;

            if (p1 >= 0) w1.Priority = Math.Abs(p2);
            else w1.Priority = -1 * Math.Abs(p2);

            if (p2 >= 0) w2.Priority = Math.Abs(p1);
            else w2.Priority = -1 * Math.Abs(p1);

            wrappers[index] = pn2;
            wrappers[index + o] = pn1;

            cnt.Controls.SetChildIndex(pn1, index + o);
        }

        private void btpup_Click(object sender, System.EventArgs e)
        {
            if (lastpn == null) return;

            int index = FindPanelIndex(lastpn);
            if (index < 1) return;

            Exchange(index, -1);

            lastpn.Focus();
        }

        private void btpdown_Click(object sender, System.EventArgs e)
        {
            if (lastpn == null) return;

            int index = FindPanelIndex(lastpn);
            if (index < 0) return;
            if (index >= wrappers.Count - 1) return;

            Exchange(index, 1);

            lastpn.Focus();
        }

        void SetBackgroundColor(object sender, Image i, bool small)
        {
            PictureBox pb = (PictureBox)sender;
            if (small) pb.Image = i.GetThumbnailImage(16, 16, new Image.GetThumbnailImageAbort(ThumbnailCallback), IntPtr.Zero);
            else pb.Image = i;
            /*if (wrapper.Priority<0) pb.BackColor = Color.FromArgb(0x70FF5B60);
            else pb.BackColor = Color.FromArgb(0x7087D300);*/
        }

        private void pb_ExpandClick(object sender, EventArgs e)
        {
            PictureBox pb = (PictureBox)sender;
            PluginPanel pn = (PluginPanel)pb.Tag;

            if (pn.Height == pn.DisplayRectangle.Top + 1)
            {
                pn.Controls[pn.Controls.Count - 1].Visible = true;
                pn.Height = height;
            }
            else
            {
                pn.Controls[pn.Controls.Count - 1].Visible = false;
                pn.Height = pn.DisplayRectangle.Top + 1;
            }


            pb.Image = GetShrinkImage(pn);
            SimPe.Interfaces.IWrapper wrapper = (SimPe.Interfaces.IWrapper)pn.Tag;
            //SetBackgroundColor(pb, wrapper);
        }


        private void tb_GotFocus(object sender, EventArgs e)
        {
            if (lastpn != null)
            {
                this.pn_Focused(lastpn, null);
            }
        }

        #endregion

        private void cbmulti_CheckedChanged(object sender, System.EventArgs e)
        {
            cbFirefox.Enabled = cbmulti.Checked;
            cbFirefox.Refresh();
        }

        private void button8_Click(object sender, System.EventArgs e)
        {
            Helper.XmlRegistry.ClearRecentFileList();
        }

        private void tbPassword_Leave(object sender, System.EventArgs e)
        {
            tbUserId_TextChanged(null, null);
            if (this.Tag != null) return;

            // Old SimPE online account system removed.
            // Generate a stable UID based on username + password instead.
            uint uid = UserVerification.GenerateUserId(0, tbUsername.Text, tbPassword.Text);

            tbUserId.Text = "0x" + Helper.HexString(uid);
            tbUserId_TextChanged(null, null);
        }

        private void linkLabel3_LinkClicked_1(object sender, System.Windows.Forms.LinkLabelLinkClickedEventArgs e)
        {
            tbPassword_Leave(null, null);
        }

        private void tbUserId_TextChanged(object sender, System.EventArgs e)
        {
            uint i = Helper.StringToUInt32(tbUserId.Text, 0, 16);
            cbValid.Checked = UserVerification.ValidUserId(i, tbUsername.Text, tbPassword.Text);
        }

        private void linkLabel5_LinkClicked(object sender, System.Windows.Forms.LinkLabelLinkClickedEventArgs e)
        {
            tbUserId.Text = "0x" + Helper.HexString(0);
        }

        private void llNightlife_LinkClicked(object sender, System.Windows.Forms.LinkLabelLinkClickedEventArgs e)
        {
            tbep2.Text = PathProvider.Global[Expansions.Nightlife].RealInstallFolder;
        }

        private void btNightlife_Click(object sender, System.EventArgs e)
        {
            if (System.IO.Directory.Exists(tbep2.Text)) fbd.SelectedPath = tbep2.Text;
            if (fbd.ShowDialog() == DialogResult.OK) this.tbep2.Text = fbd.SelectedPath;
        }

        private void cbblur_CheckedChanged(object sender, System.EventArgs e)
        {
            Helper.XmlRegistry.BlurNudity = cbblur.Checked;
            cbblur.Checked = Helper.XmlRegistry.BlurNudity;
        }

        private void cbDeep_CheckedChanged(object sender, System.EventArgs e)
        {
            cbSimTemp.Enabled = cbDeep.Checked;
        }





        private void cbLock_CheckedChanged(object sender, System.EventArgs e)
        {
            CheckBox cb = sender as CheckBox;

            if (cb.Checked) this.LockDocksClick(sender, e);
            else this.UnlockDocksClick(sender, e);
        }

        private void cbCustom_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            this.pgcustom.SelectedObject = cbCustom.SelectedItem;
        }

        void cbautobak_CheckedChanged(object sender, EventArgs e)
        {
            if (cbautobak.CheckState == CheckState.Checked && Helper.XmlRegistry.AutoBackup == false)
                MessageBox.Show(Localization.GetString("cbautobak_CheckedChanged"), this.Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        class MyPropertyGrid : PropertyGrid
        {

        }
    }

    /// <summary>
    /// Replacement for TD.Eyefinder.HeaderControl used in plugin list panels.
    /// Draws its own header strip and overrides DisplayRectangle to match HeaderControl behavior.
    /// </summary>
    internal sealed class PluginPanel : Panel
    {
        public const int HeaderHeight = 22;

        // Override DisplayRectangle so existing code using pn.DisplayRectangle.Top still works
        public override Rectangle DisplayRectangle =>
            new Rectangle(0, HeaderHeight, ClientSize.Width, Math.Max(0, ClientSize.Height - HeaderHeight));

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            var g = e.Graphics;
            // Header background
            using (var brush = new SolidBrush(BackColor))
                g.FillRectangle(brush, 0, 0, Width, HeaderHeight);
            // Header text
            if (!string.IsNullOrEmpty(Text))
                using (var brush = new SolidBrush(ForeColor))
                using (var sf = new StringFormat { LineAlignment = StringAlignment.Center })
                    g.DrawString(Text, Font, brush, new RectangleF(4, 0, Width - 8, HeaderHeight), sf);
            // Separator line
            using (var pen = new Pen(SystemColors.ControlDark))
                g.DrawLine(pen, 0, HeaderHeight - 1, Width - 1, HeaderHeight - 1);
        }

        protected override void OnBackColorChanged(EventArgs e) { base.OnBackColorChanged(e); Invalidate(); }
        protected override void OnForeColorChanged(EventArgs e) { base.OnForeColorChanged(e); Invalidate(); }
        protected override void OnFontChanged(EventArgs e) { base.OnFontChanged(e); Invalidate(); }
        protected override void OnTextChanged(EventArgs e) { base.OnTextChanged(e); Invalidate(); }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            if (e.Y < HeaderHeight) Focus();
        }
    }
}
