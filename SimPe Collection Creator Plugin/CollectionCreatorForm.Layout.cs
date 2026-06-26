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
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace SimPe.Plugin
{
    // Form layout — faithful recreation of JFade's original frmMain at the
    // same 640×550 ClientSize and control coordinates so users who know the
    // original tool immediately recognise the UI. Four GroupBoxes act as
    // mode panes (Options / Batch Add / Progress / Add Item Details) hidden
    // by default; HideUI(n) / ShowUI(n) in the handlers swap modes.
    internal partial class CollectionCreatorForm
    {
        // --- Always-visible main controls ---
        Button cmdMakeNewColl, cmdEditColl, cmdBackUpColl;
        Button cmdAlphaSort, cmdOptions, cmdAbout;
        Button cmdExit, cmdSaveColl;
        Button Command1;        // "Add Object"
        Button cmdBatchAdd;
        Button cmdLoadPic;

        Label lblName, lblPicture, lblScope;
        TextBox txtCollName, txtCollID, txtImgPath;
        ComboBox cmbCollType;
        PictureBox Picture1;

        ListBox lstListOfItems;
        Button cmdMoveUp, cmdMoveDown, cmdRemoveItem;

        // JFade used StatusBar/StatusBarPanel in 2007; both were removed
        // from modern WinForms. StatusStrip + ToolStripStatusLabel is the
        // direct equivalent — same role, same visual.
        StatusStrip StatusBar1;
        ToolStripStatusLabel Panel1;

        // --- GroupBox1: Options mode ---
        GroupBox GroupBox1;
        Label lblOptDefaultDir, lblOptCompression, lblOptWarnings, lblOptThumbDir;
        TextBox txtCollDir, txtThumbDir;
        Button cmdFindCollDir, cmdFindThumbDir, cmdCloseOptions;
        CheckBox chkCompression, chkWarningOff;

        // --- GroupBox2: Batch Add mode ---
        GroupBox GroupBox2;
        Label lblBatchAddTotal, lblBatchCategories, lblBatchPreview;
        ListBox lstBatchAdd, lstBatchCategories;
        PictureBox PictureBox2;
        Button cmdBatchAddUp, cmdBatchAddDown, cmdBatchAddRemove;
        Button cmdFinishBatchAdd, cmdCancelBatchAdd;

        // --- GroupBox3: Progress mode ---
        GroupBox GroupBox3;
        Label lblProgressDesc, lblProgress;
        ProgressBar ProgressBar1;

        // --- GroupBox4: Add Item Details mode ---
        GroupBox GroupBox4;
        Label lblItemGuid, lblItemGroup, lblItemFilename, lblItemDesc, lblItemPreview, lblItemCategories;
        TextBox txtGUID, txtGroup, txtFileName, txtCTSSName, txtCTSSDesc;
        PictureBox PictureBox1;
        ListBox lstCategories;
        Button cmdAddItem, cmdCancel;

        // --- ToolTip ---
        ToolTip ToolTip1;

        // --- Dialogs ---
        OpenFileDialog dlgOpenCollection;
        OpenFileDialog dlgAddObject;
        OpenFileDialog dlgPickThumbnail;
        SaveFileDialog dlgSaveCollection;
        FolderBrowserDialog dlgPickFolder;

        // Big-button (toolbar) size, small-button (list/dialog action) size —
        // both taken straight from JFade's original layout.
        static readonly Size BigBtn = new Size(67, 67);
        static readonly Size SmallBtn = new Size(44, 44);

        // Font used for the three Comic Sans labels on the main form
        // (Name / Picture / Collection Type) — JFade's branding choice.
        static readonly Font ComicSans12 = new Font("Comic Sans MS", 12F);

        void InitializeComponent()
        {
            SuspendLayout();

            // --- Form chrome ---------------------------------------------
            Text = "Sims 2 Collection Creator - All Your Sim Are Belong To Us!";
            ClientSize = new Size(640, 550);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            StartPosition = FormStartPosition.CenterParent;
            ShowInTaskbar = false;
            // JFade's original background is fairly dark — combined with
            // flat (borderless) icon buttons, low-contrast button edges hurt
            // visibility. Lighten the image at load time by blending white
            // over it; the icons stay sharp because they're separate Image
            // properties on each Button.
            BackgroundImage = LightenImage(LoadImage("_this.BackgroundImage.png"), 0.5f);
            BackgroundImageLayout = ImageLayout.None;
            Icon = LoadIcon("_this.Icon.ico");

            BuildMainBar();
            BuildMetadataBlock();
            BuildItemList();
            BuildBottomButtons();
            BuildStatusBar();

            BuildOptionsGroup();    // GroupBox1
            BuildBatchAddGroup();   // GroupBox2
            BuildProgressGroup();   // GroupBox3
            BuildAddItemGroup();    // GroupBox4

            BuildDialogs();
            BuildToolTips();
            WireHandlers();
            UpdateUIState();

            ResumeLayout(false);
            PerformLayout();
        }

        // --- Top row: 6 big buttons across the top ---------------------
        void BuildMainBar()
        {
            cmdMakeNewColl = NewImageButton("cmdMakeNewColl", 8,   7, BigBtn);
            cmdEditColl    = NewImageButton("cmdEditColl",    120, 7, BigBtn);
            cmdBackUpColl  = NewImageButton("cmdBackUpColl",  232, 7, BigBtn);
            cmdAlphaSort   = NewImageButton("cmdAlphaSort",   344, 7, BigBtn);
            cmdOptions     = NewImageButton("cmdOptions",     456, 7, BigBtn);
            cmdAbout       = NewImageButton("cmdAbout",       560, 7, BigBtn);

            Controls.Add(cmdMakeNewColl);
            Controls.Add(cmdEditColl);
            Controls.Add(cmdBackUpColl);
            Controls.Add(cmdAlphaSort);
            Controls.Add(cmdOptions);
            Controls.Add(cmdAbout);
        }

        // --- Metadata block: Name / Picture / Scope --------------------
        void BuildMetadataBlock()
        {
            lblName    = new Label { Text = "Name Of Collection:", Location = new Point(88, 79),  Size = new Size(160, 25), Font = ComicSans12, BackColor = Color.Transparent };
            lblPicture = new Label { Text = "Picture:",            Location = new Point(88, 103), Size = new Size(145, 25), Font = ComicSans12, BackColor = Color.Transparent };
            lblScope   = new Label { Text = "Collection Type:",    Location = new Point(88, 135), Size = new Size(153, 25), Font = ComicSans12, BackColor = Color.Transparent };

            txtCollName = new TextBox { Location = new Point(248, 80),  Size = new Size(289, 19) };
            Picture1    = new PictureBox { Location = new Point(248, 103), Size = new Size(33, 32), BorderStyle = BorderStyle.Fixed3D, SizeMode = PictureBoxSizeMode.Zoom, BackColor = Color.White };
            cmdLoadPic  = new Button { Text = "...", Location = new Point(288, 103), Size = new Size(25, 25) };
            txtImgPath  = new TextBox { Location = new Point(328, 111), Size = new Size(208, 20), ReadOnly = true };

            cmbCollType = new ComboBox { Location = new Point(248, 143), Size = new Size(153, 22), DropDownStyle = ComboBoxStyle.DropDownList };
            cmbCollType.Items.AddRange(new object[] { "Residential", "Community", "Both (PETS ONLY)" });
            cmbCollType.SelectedIndex = 0;

            txtCollID = new TextBox { Location = new Point(416, 143), Size = new Size(120, 20), ReadOnly = true };

            Controls.Add(lblName); Controls.Add(lblPicture); Controls.Add(lblScope);
            Controls.Add(txtCollName); Controls.Add(Picture1); Controls.Add(cmdLoadPic);
            Controls.Add(txtImgPath); Controls.Add(cmbCollType); Controls.Add(txtCollID);
        }

        // --- Item list + side reorder buttons --------------------------
        void BuildItemList()
        {
            lstListOfItems = new ListBox
            {
                Location = new Point(96, 167),
                Size = new Size(441, 214),
                IntegralHeight = false,
            };

            cmdMoveUp     = NewImageButton("cmdMoveUp",     544, 199, SmallBtn);
            cmdRemoveItem = NewImageButton("cmdRemoveItem", 544, 255, SmallBtn);
            cmdMoveDown   = NewImageButton("cmdMoveDown",   544, 311, SmallBtn);

            Controls.Add(lstListOfItems);
            Controls.Add(cmdMoveUp);
            Controls.Add(cmdRemoveItem);
            Controls.Add(cmdMoveDown);
        }

        // --- Bottom row: Exit / Add Object / Batch Add / Save ----------
        void BuildBottomButtons()
        {
            cmdExit     = NewImageButton("cmdExit",     8,   455, BigBtn);
            Command1    = NewImageButton("Command1",    104, 383, BigBtn);  // Add Object
            cmdBatchAdd = NewImageButton("cmdBatchAdd", 464, 383, BigBtn);
            cmdSaveColl = NewImageButton("cmdSaveColl", 560, 455, BigBtn);

            Controls.Add(cmdExit);
            Controls.Add(Command1);
            Controls.Add(cmdBatchAdd);
            Controls.Add(cmdSaveColl);
        }

        void BuildStatusBar()
        {
            Panel1 = new ToolStripStatusLabel("Ready.")
            {
                Spring = true,                                  // fill the bar
                TextAlign = ContentAlignment.MiddleRight,       // match JFade's right-aligned panel
            };
            StatusBar1 = new StatusStrip();
            StatusBar1.Items.Add(Panel1);
            Controls.Add(StatusBar1);
        }

        // --- GroupBox1: Options mode (Location 88,96 — overlays metadata) ---
        void BuildOptionsGroup()
        {
            GroupBox1 = new GroupBox { Text = "Options", Location = new Point(88, 96), Size = new Size(456, 304), Visible = false };

            lblOptDefaultDir = new Label { Text = "Default Collections Directory:", Location = new Point(16, 24),  Size = new Size(200, 48) };
            txtCollDir       = new TextBox { Location = new Point(224, 24), Size = new Size(192, 20) };
            cmdFindCollDir   = new Button  { Text = "...", Location = new Point(424, 24), Size = new Size(24, 24) };

            lblOptWarnings   = new Label    { Text = "Turn Off Warning Dialogs:",   Location = new Point(16, 80),  Size = new Size(200, 32) };
            chkWarningOff    = new CheckBox { Text = "Yes",                          Location = new Point(224, 88), Size = new Size(104, 16) };

            lblOptCompression= new Label    { Text = "Enable Package Compression:", Location = new Point(16, 120), Size = new Size(200, 64) };
            chkCompression   = new CheckBox { Text = "Yes",                          Location = new Point(224, 144), Size = new Size(104, 16) };

            lblOptThumbDir   = new Label   { Text = "Thumbnail Packages Directory:", Location = new Point(16, 192), Size = new Size(200, 64) };
            txtThumbDir      = new TextBox { Location = new Point(224, 208), Size = new Size(192, 20) };
            cmdFindThumbDir  = new Button  { Text = "...", Location = new Point(424, 208), Size = new Size(24, 24) };

            cmdCloseOptions  = new Button  { Text = "OK", Location = new Point(184, 272), Size = new Size(104, 24) };

            GroupBox1.Controls.AddRange(new Control[]
            {
                lblOptDefaultDir, txtCollDir, cmdFindCollDir,
                lblOptWarnings, chkWarningOff,
                lblOptCompression, chkCompression,
                lblOptThumbDir, txtThumbDir, cmdFindThumbDir,
                cmdCloseOptions,
            });
            Controls.Add(GroupBox1);
        }

        // --- GroupBox2: Batch Add mode -------------------------------
        void BuildBatchAddGroup()
        {
            GroupBox2 = new GroupBox { Text = "Batch Add", Location = new Point(16, 80), Size = new Size(608, 376), Visible = false };

            lblBatchAddTotal    = new Label   { Text = "Total Items: 0",        Location = new Point(8, 16),    Size = new Size(456, 16) };
            lstBatchAdd         = new ListBox { Location = new Point(8, 32),    Size = new Size(448, 290), IntegralHeight = false };

            cmdBatchAddUp       = NewImageButton("cmdBatchAddUp",     472, 16, SmallBtn);
            cmdBatchAddDown     = NewImageButton("cmdBatchAddDown",   472, 64, SmallBtn);
            cmdBatchAddRemove   = NewImageButton("cmdBatchAddRemove", 520, 40, SmallBtn);

            lblBatchCategories  = new Label   { Text = "Categories of Objects:", Location = new Point(472, 112), Size = new Size(128, 16) };
            lstBatchCategories  = new ListBox { Location = new Point(472, 128), Size = new Size(128, 95), Sorted = true };

            lblBatchPreview     = new Label   { Text = "Object Preview:",        Location = new Point(472, 224), Size = new Size(128, 16) };
            PictureBox2         = new PictureBox { Location = new Point(472, 240), Size = new Size(128, 128), BorderStyle = BorderStyle.FixedSingle, SizeMode = PictureBoxSizeMode.Zoom };

            cmdFinishBatchAdd   = NewImageButton("cmdFinishBatchAdd", 224, 328, SmallBtn);
            cmdCancelBatchAdd   = NewImageButton("cmdCancelBatchAdd", 280, 328, SmallBtn);

            GroupBox2.Controls.AddRange(new Control[]
            {
                lblBatchAddTotal, lstBatchAdd,
                cmdBatchAddUp, cmdBatchAddDown, cmdBatchAddRemove,
                lblBatchCategories, lstBatchCategories,
                lblBatchPreview, PictureBox2,
                cmdFinishBatchAdd, cmdCancelBatchAdd,
            });
            Controls.Add(GroupBox2);
        }

        // --- GroupBox3: Progress mode -------------------------------
        void BuildProgressGroup()
        {
            GroupBox3 = new GroupBox { Text = "Working…", Location = new Point(192, 0), Size = new Size(256, 88), Visible = false };

            lblProgressDesc = new Label       { Text = "Now Reading Resource:", Location = new Point(8, 16),  Size = new Size(240, 16) };
            ProgressBar1    = new ProgressBar { Location = new Point(8, 56),  Size = new Size(240, 24) };
            lblProgress     = new Label       { Text = "",                       Location = new Point(80, 32), Size = new Size(100, 16) };

            GroupBox3.Controls.AddRange(new Control[] { lblProgressDesc, ProgressBar1, lblProgress });
            Controls.Add(GroupBox3);
        }

        // --- GroupBox4: Add Item Details mode -----------------------
        void BuildAddItemGroup()
        {
            GroupBox4 = new GroupBox { Text = "Add Item", Location = new Point(88, 80), Size = new Size(457, 352), Visible = false, BackColor = SystemColors.InactiveCaptionText };

            lblItemGuid     = new Label   { Text = "GUID:",                Location = new Point(8, 16),    Size = new Size(137, 17) };
            txtGUID         = new TextBox { Location = new Point(8, 32),   Size = new Size(136, 19), ReadOnly = true };
            lblItemGroup    = new Label   { Text = "Group:",               Location = new Point(152, 16), Size = new Size(136, 17) };
            txtGroup        = new TextBox { Location = new Point(152, 32), Size = new Size(136, 19), ReadOnly = true };
            lblItemFilename = new Label   { Text = "Filename:",            Location = new Point(8, 56),   Size = new Size(280, 17) };
            txtFileName     = new TextBox { Location = new Point(8, 72),   Size = new Size(280, 19), ReadOnly = true };
            lblItemDesc     = new Label   { Text = "Catalog Description:", Location = new Point(8, 96),   Size = new Size(280, 17) };
            txtCTSSName     = new TextBox { Location = new Point(8, 112),  Size = new Size(281, 19), ReadOnly = true };
            txtCTSSDesc     = new TextBox { Location = new Point(8, 136),  Size = new Size(281, 153), ReadOnly = true, Multiline = true, ScrollBars = ScrollBars.Vertical };

            lblItemPreview  = new Label      { Text = "Preview:",       Location = new Point(296, 8),   Size = new Size(137, 17) };
            PictureBox1     = new PictureBox { Location = new Point(296, 24),  Size = new Size(152, 152), BorderStyle = BorderStyle.FixedSingle, SizeMode = PictureBoxSizeMode.Zoom };
            lblItemCategories = new Label    { Text = "Catalog Sorts:", Location = new Point(296, 176), Size = new Size(137, 17) };
            lstCategories   = new ListBox    { Location = new Point(296, 200), Size = new Size(152, 88), Sorted = true };

            cmdAddItem      = NewImageButton("cmdAddItem", 168, 296, SmallBtn);
            cmdCancel       = NewImageButton("cmdCancel",  240, 296, SmallBtn);

            GroupBox4.Controls.AddRange(new Control[]
            {
                lblItemGuid, txtGUID, lblItemGroup, txtGroup,
                lblItemFilename, txtFileName,
                lblItemDesc, txtCTSSName, txtCTSSDesc,
                lblItemPreview, PictureBox1,
                lblItemCategories, lstCategories,
                cmdAddItem, cmdCancel,
            });
            Controls.Add(GroupBox4);
        }

        // JFade had a ToolTip1 in his original; the strings here are
        // reconstructed from what each button does (his original tooltip
        // text isn't preserved in the decompile beyond the field decl).
        void BuildToolTips()
        {
            ToolTip1 = new ToolTip
            {
                AutoPopDelay = 8000,
                InitialDelay = 400,
                ReshowDelay = 200,
                ShowAlways = true,
            };

            // Main mode — top toolbar
            ToolTip1.SetToolTip(cmdMakeNewColl, "Start a new collection");
            ToolTip1.SetToolTip(cmdEditColl,    "Open an existing collection to edit");
            ToolTip1.SetToolTip(cmdBackUpColl,  "Make a .bak copy of the currently-open collection");
            ToolTip1.SetToolTip(cmdAlphaSort,   "Sort the item list A–Z");
            ToolTip1.SetToolTip(cmdOptions,     "Options");
            ToolTip1.SetToolTip(cmdAbout,       "About JFade's Collection Creator");

            // Main mode — metadata + list controls
            ToolTip1.SetToolTip(cmdLoadPic,     "Pick a thumbnail image for the collection");
            ToolTip1.SetToolTip(cmdMoveUp,      "Move the selected item up");
            ToolTip1.SetToolTip(cmdMoveDown,    "Move the selected item down");
            ToolTip1.SetToolTip(cmdRemoveItem,  "Remove the selected item from the collection");

            // Main mode — bottom row
            ToolTip1.SetToolTip(cmdExit,        "Close Collection Creator");
            ToolTip1.SetToolTip(Command1,       "Add an object to the collection");
            ToolTip1.SetToolTip(cmdBatchAdd,    "Add multiple objects at once (batch mode)");
            ToolTip1.SetToolTip(cmdSaveColl,    "Save the collection to a .package file");

            // Options mode
            ToolTip1.SetToolTip(cmdFindCollDir,   "Browse for the default Collections folder");
            ToolTip1.SetToolTip(cmdFindThumbDir,  "Browse for the Thumbnails folder");
            ToolTip1.SetToolTip(cmdCloseOptions,  "Save options and return");

            // Batch Add mode
            ToolTip1.SetToolTip(cmdBatchAddUp,     "Move the selected batch item up");
            ToolTip1.SetToolTip(cmdBatchAddDown,   "Move the selected batch item down");
            ToolTip1.SetToolTip(cmdBatchAddRemove, "Remove the selected batch item");
            ToolTip1.SetToolTip(cmdFinishBatchAdd, "Add every listed item to the collection");
            ToolTip1.SetToolTip(cmdCancelBatchAdd, "Cancel batch add and discard the list");

            // Add Item Details mode
            ToolTip1.SetToolTip(cmdAddItem, "Add this item to the collection");
            ToolTip1.SetToolTip(cmdCancel,  "Cancel and discard");
        }

        void BuildDialogs()
        {
            dlgOpenCollection = new OpenFileDialog
            {
                Title = "Open Collection package",
                Filter = "Sims 2 package (*.package)|*.package|All files (*.*)|*.*",
            };
            dlgAddObject = new OpenFileDialog
            {
                Title = "Pick an object package to add",
                Filter = "Sims 2 package (*.package)|*.package|All files (*.*)|*.*",
                Multiselect = true,
            };
            dlgPickThumbnail = new OpenFileDialog
            {
                Title = "Pick a collection thumbnail",
                Filter = "Image files|*.png;*.jpg;*.jpeg;*.bmp;*.gif|All files (*.*)|*.*",
                InitialDirectory = dataFolder != null ? Path.Combine(dataFolder, "Sample Icons") : "",
            };
            dlgSaveCollection = new SaveFileDialog
            {
                Title = "Save Collection package",
                Filter = "Sims 2 package (*.package)|*.package",
                DefaultExt = "package",
                AddExtension = true,
                OverwritePrompt = true,
            };
            dlgPickFolder = new FolderBrowserDialog { ShowNewFolderButton = true };
        }

        // --- Mode swapping --------------------------------------------
        // JFade's HideUI(n) / ShowUI(n) pattern: when entering a "mode"
        // (Options / Batch Add / Add Item Details), the main controls
        // hide and the corresponding GroupBox shows. ShowUI(n) reverses it.
        // Progress (GroupBox3) is a status overlay — main controls stay
        // visible underneath.

        enum UIMode { Main, Options, BatchAdd, AddItem }

        UIMode currentMode = UIMode.Main;

        void EnterMode(UIMode mode)
        {
            currentMode = mode;
            bool main = mode == UIMode.Main;
            // Main controls hide whenever we're inside a modal sub-mode
            // (Options / BatchAdd / AddItem) — Progress overlay keeps them.
            SetMainControlsVisible(main);
            GroupBox1.Visible = mode == UIMode.Options;
            GroupBox2.Visible = mode == UIMode.BatchAdd;
            GroupBox4.Visible = mode == UIMode.AddItem;
            UpdateUIState();
        }

        void SetMainControlsVisible(bool v)
        {
            foreach (Control c in new Control[]
            {
                cmdMakeNewColl, cmdEditColl, cmdBackUpColl, cmdAlphaSort, cmdOptions, cmdAbout,
                cmdExit, cmdSaveColl, Command1, cmdBatchAdd,
                lblName, lblPicture, lblScope,
                txtCollName, Picture1, cmdLoadPic, txtImgPath,
                cmbCollType, txtCollID,
                lstListOfItems, cmdMoveUp, cmdMoveDown, cmdRemoveItem,
            })
            {
                c.Visible = v;
            }
        }

        // --- Helpers --------------------------------------------------

        // Build a Button styled like JFade's tile buttons: image fills the
        // whole button, FlatStyle=Flat with no border, transparent
        // background so the form's BackgroundImage shows through.
        Button NewImageButton(string imageStem, int x, int y, Size size)
        {
            var btn = new Button
            {
                Location = new Point(x, y),
                Size = size,
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.Transparent,
                Image = LoadImage(imageStem + ".Image.png"),
                ImageAlign = ContentAlignment.MiddleCenter,
            };
            btn.FlatAppearance.BorderSize = 0;
            return btn;
        }

        // Blend white over the source image at the given opacity to brighten it.
        // amount 0 = unchanged, 1 = pure white. 0.5 = halfway lighter, leaving
        // ~50% of the original detail visible — enough to keep JFade's branding
        // but bright enough that dark borderless icon buttons read as buttons.
        static Image LightenImage(Image src, float amount)
        {
            if (src == null) return null;
            amount = Math.Max(0f, Math.Min(1f, amount));
            var result = new Bitmap(src.Width, src.Height);
            using (var g = Graphics.FromImage(result))
            {
                g.DrawImage(src, 0, 0, src.Width, src.Height);
                int alpha = (int)Math.Round(255 * amount);
                using (var brush = new SolidBrush(Color.FromArgb(alpha, Color.White)))
                    g.FillRectangle(brush, 0, 0, src.Width, src.Height);
            }
            return result;
        }

        Image LoadImage(string filename)
        {
            if (dataFolder == null) return null;
            string path = Path.Combine(dataFolder, "Images", filename);
            if (!File.Exists(path)) return null;
            try
            {
                // Load via FileStream + MemoryStream so the file handle
                // doesn't stay open for the lifetime of the Image (which
                // would block hot-reloading the data folder during dev).
                using (var fs = File.OpenRead(path))
                {
                    var ms = new MemoryStream();
                    fs.CopyTo(ms);
                    ms.Position = 0;
                    return Image.FromStream(ms);
                }
            }
            catch { return null; }
        }

        Icon LoadIcon(string filename)
        {
            if (dataFolder == null) return null;
            string path = Path.Combine(dataFolder, "Images", filename);
            if (!File.Exists(path)) return null;
            try
            {
                using (var fs = File.OpenRead(path))
                    return new Icon(fs);
            }
            catch { return null; }
        }

        // State-driven button enabling. Called after every action that
        // touches `current` or the member-list selection.
        void UpdateUIState()
        {
            bool hasCollection = current != null;
            bool hasMembers    = hasCollection && current.Members.Count > 0;
            int selIdx         = lstListOfItems?.SelectedIndex ?? -1;
            bool hasSelection  = hasCollection && selIdx >= 0;

            if (cmdSaveColl   != null) cmdSaveColl.Enabled   = hasMembers;
            if (cmdAlphaSort  != null) cmdAlphaSort.Enabled  = hasMembers && current.Members.Count > 1;
            if (Command1      != null) Command1.Enabled      = hasCollection;
            if (cmdBatchAdd   != null) cmdBatchAdd.Enabled   = hasCollection;
            if (cmdLoadPic    != null) cmdLoadPic.Enabled    = hasCollection;
            if (txtCollName   != null) txtCollName.ReadOnly  = !hasCollection;
            if (cmbCollType   != null) cmbCollType.Enabled   = hasCollection;

            if (cmdRemoveItem != null) cmdRemoveItem.Enabled = hasSelection;
            if (cmdMoveUp     != null) cmdMoveUp.Enabled     = hasSelection && selIdx > 0;
            if (cmdMoveDown   != null) cmdMoveDown.Enabled   = hasSelection && selIdx < current.Members.Count - 1;
        }
    }
}
