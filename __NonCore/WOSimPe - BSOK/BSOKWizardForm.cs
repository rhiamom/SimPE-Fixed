using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Collections.Generic;
using System.IO;
using System.Data;

namespace SimPe.Wizards
{
	/// <summary>
    /// Summary description for BsokWizardForm.
	/// </summary>
	public class BsokWizardForm : System.Windows.Forms.Form
	{
		private TabControl tabControl1;
		private TabPage tabPage1;
		private TabPage tabPage2;
		internal Panel pnwizard1;
		internal Panel pnwizard2;
		private Label label1;
        private Label label2;
        private Label lboops;
        private Label lbPath;
        internal Label lbDone;
        internal LinkLabel linkLabel1;
        private booby.linkyicon linkyicon1;
        private Button button1;
        private PictureBox pbicon;
        private RichTextBox rtb;
        internal RichTextBox rtbAbout;
        internal ComboBox cbShapes;
        internal ListView lvpackages;
        internal FolderBrowserDialog fbd1;
		private System.ComponentModel.Container components = null;
        internal string floder = null;
        internal Step1 step1;
        internal Step2 step2;
        internal Step3 step3;
        internal SimPe.Packages.File pak;
        private bool foun = false;
        internal Dictionary<uint, string> BodyShapeIds = new Dictionary<uint, string>();

        public BsokWizardForm()
        {
            InitializeComponent();
            if (SimPe.Helper.SimPeVersionLong < 330717003777) // the first 77 version
            {
                this.button1.Enabled = false;
                this.lboops.Visible = true;
                this.linkyicon1.Visible = this.label1.Visible = false;
                this.lboops.Text = "This Version of SimPe is too old";
            }
            else
            {
                InitializeBodyShapes();
                LoadHelpFile();
                if (SimPe.Helper.SimPeVersionLong < 330717003783) // the first version with larger form
                {
                    this.lbDone.Font = new System.Drawing.Font("Verdana", 9.75F);
                    this.rtbAbout.ZoomFactor = 0.7F; // comprimize 0.6 fits but is too small to read
                }

                foreach (KeyValuePair<uint, string> kvp in BodyShapeIds)
                    this.cbShapes.Items.Add(kvp.Value);

                this.pbicon.BackgroundImage = global::SimPe.Wizards.Properties.Resources.WizardIcon;
                pak = SimPe.Packages.File.LoadFromFile(System.IO.Path.Combine(PathProvider.Global.Latest.InstallFolder, "TSData\\Res\\UI\\ui.package"));
                if (System.IO.Directory.Exists(System.IO.Path.Combine(PathProvider.SimSavegameFolder, "Downloads")))
                {
                    string[] files = System.IO.Directory.GetFiles(System.IO.Path.Combine(PathProvider.SimSavegameFolder, "Downloads"), "Bodyshape Icons.package", SearchOption.AllDirectories);
                    if (files.Length > 0) pak = SimPe.Packages.File.LoadFromFile(files[0]);
                }
            }
        }

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if (components != null) 
				{
					components.Dispose();
				}
                if (pak != null) pak.Close();
			}
			base.Dispose( disposing );
        }

        #region Windows Form Designer generated code
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.pnwizard1 = new System.Windows.Forms.Panel();
            this.rtb = new System.Windows.Forms.RichTextBox();
            this.rtbAbout = new System.Windows.Forms.RichTextBox();
            this.linkyicon1 = new booby.linkyicon();
            this.lbPath = new System.Windows.Forms.Label();
            this.lboops = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.pnwizard2 = new System.Windows.Forms.Panel();
            this.linkLabel1 = new System.Windows.Forms.LinkLabel();
            this.pbicon = new System.Windows.Forms.PictureBox();
            this.lbDone = new System.Windows.Forms.Label();
            this.lvpackages = new System.Windows.Forms.ListView();
            this.cbShapes = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.fbd1 = new System.Windows.Forms.FolderBrowserDialog();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.pnwizard1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.pnwizard2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbicon)).BeginInit();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(8, 8);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(632, 214);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.pnwizard1);
            this.tabPage1.Location = new System.Drawing.Point(4, 23);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Size = new System.Drawing.Size(624, 187);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Step 1";
            // 
            // pnwizard1
            // 
            this.pnwizard1.BackColor = System.Drawing.Color.White;
            this.pnwizard1.Controls.Add(this.rtbAbout);
            this.pnwizard1.Controls.Add(this.linkyicon1);
            this.pnwizard1.Controls.Add(this.lbPath);
            this.pnwizard1.Controls.Add(this.lboops);
            this.pnwizard1.Controls.Add(this.button1);
            this.pnwizard1.Controls.Add(this.label1);
            this.pnwizard1.Location = new System.Drawing.Point(0, 0);
            this.pnwizard1.Name = "pnwizard1";
            this.pnwizard1.Size = new System.Drawing.Size(624, 184);
            this.pnwizard1.TabIndex = 10;
            // 
            // rtb
            // 
            this.rtb.Name = "rtb";
            this.rtb.Size = new System.Drawing.Size(824, 156);
            this.rtb.TabIndex = 8;
            this.rtb.Text = "";
            this.rtb.Visible = false;
            // 
            // rtbAbout
            // 
            this.rtbAbout.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.rtbAbout.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.rtbAbout.Font = new System.Drawing.Font("Times New Roman", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rtbAbout.Location = new System.Drawing.Point(0, 28);
            this.rtbAbout.Name = "rtbAbout";
            this.rtbAbout.Size = new System.Drawing.Size(624, 156);
            this.rtbAbout.TabIndex = 5;
            this.rtbAbout.Text = "";
            this.rtbAbout.Visible = false;
            this.rtbAbout.LinkClicked += new System.Windows.Forms.LinkClickedEventHandler(this.rtbAbout_LinkClicked);
            // 
            // linkyicon1
            // 
            this.linkyicon1.ActiveLinkColour = System.Drawing.Color.Red;
            this.linkyicon1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.linkyicon1.BackColor = System.Drawing.Color.Transparent;
            this.linkyicon1.DisabledLinkColour = System.Drawing.SystemColors.ControlDarkDark;
            this.linkyicon1.Font = new System.Drawing.Font("Verdana", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.linkyicon1.ForeColor = System.Drawing.Color.Black;
            this.linkyicon1.Gap = 0;
            this.linkyicon1.Label = "About...";
            this.linkyicon1.LinkColour = System.Drawing.Color.Red;
            this.linkyicon1.Location = new System.Drawing.Point(517, 4);
            this.linkyicon1.Margin = new System.Windows.Forms.Padding(0);
            this.linkyicon1.Name = "linkyicon1";
            this.linkyicon1.Size = new System.Drawing.Size(92, 18);
            this.linkyicon1.TabIndex = 4;
            this.linkyicon1.VisitedLinkColour = System.Drawing.Color.Maroon;
            this.linkyicon1.LinkClicked += new booby.linkyicon.EventHandler(this.linkyicon1_LinkClicked);
            // 
            // lbPath
            // 
            this.lbPath.AutoSize = true;
            this.lbPath.Font = new System.Drawing.Font("Verdana", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbPath.ForeColor = System.Drawing.SystemColors.ControlDark;
            this.lbPath.Location = new System.Drawing.Point(4, 148);
            this.lbPath.Name = "lbPath";
            this.lbPath.Size = new System.Drawing.Size(0, 18);
            this.lbPath.TabIndex = 3;
            // 
            // lboops
            // 
            this.lboops.AutoSize = true;
            this.lboops.Font = new System.Drawing.Font("Verdana", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lboops.ForeColor = System.Drawing.Color.Maroon;
            this.lboops.Location = new System.Drawing.Point(111, 116);
            this.lboops.Name = "lboops";
            this.lboops.Size = new System.Drawing.Size(274, 23);
            this.lboops.TabIndex = 2;
            this.lboops.Text = "There is no outfits there";
            this.lboops.Visible = false;
            // 
            // button1
            // 
            this.button1.Font = new System.Drawing.Font("Verdana", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button1.Location = new System.Drawing.Point(200, 72);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(96, 33);
            this.button1.TabIndex = 1;
            this.button1.Text = "Browse.. ";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Verdana", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(8, 28);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(557, 18);
            this.label1.TabIndex = 0;
            this.label1.Text = "Select the Browse button to choose a folder of outfits to configure";
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.pnwizard2);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Size = new System.Drawing.Size(624, 188);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Step 2";
            // 
            // pnwizard2
            // 
            this.pnwizard2.BackColor = System.Drawing.Color.White;
            this.pnwizard2.Controls.Add(this.linkLabel1);
            this.pnwizard2.Controls.Add(this.pbicon);
            this.pnwizard2.Controls.Add(this.lbDone);
            this.pnwizard2.Controls.Add(this.lvpackages);
            this.pnwizard2.Controls.Add(this.cbShapes);
            this.pnwizard2.Controls.Add(this.label2);
            this.pnwizard2.Location = new System.Drawing.Point(0, 2);
            this.pnwizard2.Name = "pnwizard2";
            this.pnwizard2.Size = new System.Drawing.Size(624, 184);
            this.pnwizard2.TabIndex = 10;
            // 
            // linkLabel1
            // 
            this.linkLabel1.AutoSize = true;
            this.linkLabel1.Font = new System.Drawing.Font("Verdana", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.linkLabel1.LinkColor = System.Drawing.Color.Red;
            this.linkLabel1.Location = new System.Drawing.Point(444, 29);
            this.linkLabel1.Margin = new System.Windows.Forms.Padding(0, 0, 3, 0);
            this.linkLabel1.Name = "linkLabel1";
            this.linkLabel1.Size = new System.Drawing.Size(132, 18);
            this.linkLabel1.TabIndex = 5;
            this.linkLabel1.TabStop = true;
            this.linkLabel1.Text = "Sort by Creator";
            this.linkLabel1.VisitedLinkColor = System.Drawing.Color.Maroon;
            this.linkLabel1.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel1_LinkClicked);
            // 
            // pbicon
            // 
            this.pbicon.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.pbicon.Location = new System.Drawing.Point(378, 0);
            this.pbicon.Name = "pbicon";
            this.pbicon.Size = new System.Drawing.Size(66, 66);
            this.pbicon.TabIndex = 2;
            this.pbicon.TabStop = false;
            // 
            // lbDone
            // 
            this.lbDone.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lbDone.AutoSize = true;
            this.lbDone.Font = new System.Drawing.Font("Verdana", 12F);
            this.lbDone.Location = new System.Drawing.Point(4, 164);
            this.lbDone.Name = "lbDone";
            this.lbDone.Size = new System.Drawing.Size(594, 18);
            this.lbDone.TabIndex = 4;
            this.lbDone.Text = "The selected files have been BSOK\'d, unselected files were not altered";
            this.lbDone.Visible = false;
            // 
            // lvpackages
            // 
            this.lvpackages.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lvpackages.CheckBoxes = true;
            this.lvpackages.FullRowSelect = true;
            this.lvpackages.Location = new System.Drawing.Point(4, 66);
            this.lvpackages.Name = "lvpackages";
            this.lvpackages.Size = new System.Drawing.Size(614, 94);
            this.lvpackages.TabIndex = 3;
            this.lvpackages.UseCompatibleStateImageBehavior = false;
            this.lvpackages.View = System.Windows.Forms.View.List;
            // 
            // cbShapes
            // 
            this.cbShapes.Font = new System.Drawing.Font("Verdana", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbShapes.FormattingEnabled = true;
            this.cbShapes.Location = new System.Drawing.Point(4, 25);
            this.cbShapes.Name = "cbShapes";
            this.cbShapes.Size = new System.Drawing.Size(372, 26);
            this.cbShapes.TabIndex = 1;
            this.cbShapes.SelectedIndexChanged += new System.EventHandler(this.cbShapes_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Verdana", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(4, 4);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(372, 18);
            this.label2.TabIndex = 0;
            this.label2.Text = "Select a Body Shape for the selected outfits";
            // 
            // fbd1
            // 
            this.fbd1.RootFolder = System.Environment.SpecialFolder.MyComputer;
            // 
            // BsokWizardForm
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(7, 15);
            this.ClientSize = new System.Drawing.Size(648, 230);
            this.Controls.Add(this.tabControl1);
            this.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "BsokWizardForm";
            this.Padding = new System.Windows.Forms.Padding(8);
            this.Text = "BsokWizardForm";
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.pnwizard1.ResumeLayout(false);
            this.pnwizard1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.pnwizard2.ResumeLayout(false);
            this.pnwizard2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbicon)).EndInit();
            this.ResumeLayout(false);

		}
		#endregion

        /// <summary>
        /// The main entry point for the program.
        /// </summary>
		[STAThread]
		static void Main() 
		{
			Application.Run(new BsokWizardForm());
		}

        #region 1 Find a Folder
        void LoadHelpFile()
        {
            this.linkyicon1.Icon = SimPe.GetIcon.Support;
            Stream s;

            if (SimPe.Helper.SimPeVersionLong >= 330717003790 && File.Exists(Path.Combine(Helper.SimPeDataPath, "additional_skins.xml")))
                s = this.GetType().Assembly.GetManifestResourceStream("SimPe.Wizards.About.rtf");
            else
                s = this.GetType().Assembly.GetManifestResourceStream("SimPe.Wizards.About2.rtf");
            if (s != null)
            {
                StreamReader sr = new StreamReader(s);
                rtbAbout.Rtf = sr.ReadToEnd();
                sr.Close();
                sr.Dispose();
                s.Close();
            }
            else
            {
                rtbAbout.Text = "Error: Unable to load the Instructions\r\n\r\nRe-Click the About link to close this";
            }

            if (File.Exists(Path.Combine(Helper.SimPeDataPath, "additional_skins.xml")))
            {
                StreamReader sr = File.OpenText(Path.Combine(Helper.SimPeDataPath, "additional_skins.xml"));
                try
                {
                    this.rtb.Text = sr.ReadToEnd();
                }
                finally
                {
                    sr.Close();
                    sr.Dispose();
                    sr = null;
                }
            }
            else
                this.rtb.Text = "<?xml version=\"1.0\" encoding=\"UTF-8\" ?>\r\n<alias>\r\n</alias>";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (fbd1.ShowDialog() == DialogResult.OK)
            {
                string[] stuff = Directory.GetFiles(fbd1.SelectedPath, "*.package");
                if (stuff.Length > 0)
                {
                    floder = fbd1.SelectedPath;
                    this.lboops.Visible = false;
                    PopulatFileList();
                }
                else
                {
                    floder = null;
                    this.lboops.Visible = true;
                }
                lbPath.Text = fbd1.SelectedPath;
            }
            else
            {
                floder = null;
                this.lboops.Visible = false;
                lbPath.Text = "";
            }
            if (step1 != null) step1.Update();
        }

        private void linkyicon1_LinkClicked(object sender, EventArgs e)
        {
            this.linkyicon1.Links[0].Visited = true;
            this.rtbAbout.Visible = !this.rtbAbout.Visible;
        }

        private void rtbAbout_LinkClicked(object sender, System.Windows.Forms.LinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start(e.LinkText);
        }
        #endregion

        #region 2 Select a BodyShape
        void PopulatFileList()
        {
            this.lvpackages.Items.Clear();
            if (floder == null) return;
            string[] stuff = Directory.GetFiles(floder, "*.package");

            if (stuff.Length > 0)
            {
                try
                {
                    foreach (string file in stuff)
                    {
                        ListViewItem li = new ListViewItem();
                        li.Text = Path.GetFileNameWithoutExtension(file);
                        li.Tag = file;
                        li.Checked = true;
                        this.lvpackages.Items.Add(li);
                    }
                }
                catch { }
            }
        }

        private void cbShapes_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (GetBodyShapeId(cbShapes.SelectedItem) > 0)
                this.pbicon.Image = GetBodyIcon(Convert.ToByte(GetBodyShapeId(cbShapes.SelectedItem) - 1));
            else this.pbicon.Image = null;
            if (step2 != null) step2.Update();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.linkLabel1.LinkVisited = true;
            this.linkLabel1.Links[0].Enabled = false;
            this.cbShapes.Sorted = true;
            this.cbShapes.SelectedItem = -1;
            this.pbicon.Image = null;
            if (step2 != null) step2.Update();
        }
        #endregion

        internal void DoTheWork()
        {
            Interfaces.Files.IPackedFileDescriptor[] pfds;
            foreach (ListViewItem li in this.lvpackages.CheckedItems)
            {
                SimPe.Packages.GeneratableFile file = SimPe.Packages.GeneratableFile.LoadFromFile((string)li.Tag);
                pfds = file.FindFiles(0x4C158081); // is a Skin File
                if (pfds.Length > 0) { AddImIn(pfds, file); li.Checked = false; continue; } // is a skin, Sub Add Im In, don't BSOK
                string creat;
                if (GetBodyShapeId(cbShapes.SelectedItem) == 0) // User has opted to re-customize outfit(s)
                    creat = "243b4ac8-43ec-ccf8-c358-7f86f0bdfaff";
                else
                    creat = "00000000-0000-0000-0000-000000000000";
                pfds = file.FindFiles(0xEBCF3E27);
                if (pfds.Length > 0)
                {
                    foreach (Interfaces.Files.IPackedFileDescriptor pfd in pfds)
                    {
                        SimPe.PackedFiles.Wrapper.Cpf cpf = new SimPe.PackedFiles.Wrapper.Cpf();
                        cpf.ProcessData(pfd, file);
                        SimPe.PackedFiles.Wrapper.CpfItem cr = cpf.GetSaveItem("creator");
                        SimPe.PackedFiles.Wrapper.CpfItem pr = cpf.GetSaveItem("product");

                        if (cr == null)
                        {
                            cr = new SimPe.PackedFiles.Wrapper.CpfItem();
                            cr.Name = "creator";
                        }
                        if (pr == null)
                        {
                            pr = new SimPe.PackedFiles.Wrapper.CpfItem();
                            pr.Name = "product";
                        }
                        cr.StringValue = creat;
                        pr.UIntegerValue = GetBodyShapeId(cbShapes.SelectedItem);

                        cpf.AddItem(cr, false);
                        cpf.AddItem(pr, false);
                        cpf.SynchronizeUserData(true, true);
                    }
                    file.Save((string)li.Tag);
                }
                else li.Checked = false;
            }
            this.lbDone.Visible = true;
            this.linkLabel1.Visible = false;
            this.lvpackages.Enabled = this.cbShapes.Enabled = false;
            if (foun && SimPe.Helper.SimPeVersionLong >= 330717003790)
            {
                StreamWriter sw = File.CreateText(Path.Combine(Helper.SimPeDataPath, "additional_skins.xml"));
                try
                {
                    string titty = "";
                    foreach (string boob in rtb.Lines) titty += boob + "\r\n";
                    sw.Write(titty);
                }
                finally
                {
                    sw.Flush();
                    sw.Close();
                    sw.Dispose();
                    sw = null;
                }
            }
        }

        private void AddImIn(Interfaces.Files.IPackedFileDescriptor[] pfds, SimPe.Packages.GeneratableFile file)
        {
            foun = true;
            string titty;
            foreach (Interfaces.Files.IPackedFileDescriptor pfd in pfds)
            {
                titty = "";
                SimPe.PackedFiles.Wrapper.Cpf cpf = new SimPe.PackedFiles.Wrapper.Cpf();
                cpf.ProcessData(pfd, file);
                SimPe.PackedFiles.Wrapper.CpfItem fr = cpf.GetSaveItem("family");

                foreach (string boob in this.rtb.Lines)
                {
                    if (!boob.Contains("</alias>") && !boob.Contains(fr.StringValue) && boob != "")
                        titty += (boob + "\r\n");
                }
                titty += "<item value=\"" + "0x" + Helper.HexString(GetBodyShapeId(cbShapes.SelectedItem)) + "\">" + fr.StringValue + "</item>\r\n</alias>";
                this.rtb.Text = titty;
            }
        }

        Image GetBodyIcon(byte bs)
        {
            if (SimPe.Helper.SimPeVersionLong >= 330717003790)
                return SimPe.GetImage.GetExpansionIcon(bs);

            uint inst = 0xABBB0000 + bs;
            if (pak != null)
            {
                SimPe.Interfaces.Files.IPackedFileDescriptor pfd = pak.FindFile(0x856DDBAC, 0, 0x499DB772, inst);
                if (pfd != null)
                {
                    SimPe.PackedFiles.Wrapper.Picture pic = new SimPe.PackedFiles.Wrapper.Picture();
                    pic.ProcessData(pfd, pak);
                    return pic.Image;
                }
            }
            return null;
        }

        uint GetBodyShapeId(object ob)
        {
            string val = Convert.ToString(ob);
            if (BodyShapeIds.ContainsValue(val))
                foreach (KeyValuePair<uint, string> kvp in BodyShapeIds)
                    if (kvp.Value == val) return kvp.Key;
            return 0;
        }

        void InitializeBodyShapes()
        {
            BodyShapeIds.Clear();
            BodyShapeIds.Add(0x00, " Default : Remove Icon");
            if (booby.PrettyGirls.IsTitsInstalled() || booby.PrettyGirls.IsAngelsInstalled())
            {
                BodyShapeIds.Add(0x13, "Chris H : Tiny Sim");
                BodyShapeIds.Add(0x14, "Chris H : Fashion Model Natural");
                BodyShapeIds.Add(0x15, "Maxis : Elder");
            }
            BodyShapeIds.Add(0x16, "Not a Bodyshape : Gold Star");
            BodyShapeIds.Add(0x17, "Not a Bodyshape : Silver Star");
            BodyShapeIds.Add(0x1e, "Maxis : Maxis");
            // BodyShapeIds.Add(0x1f, "Holiday");
            BodyShapeIds.Add(0x20, "SITES : Goth");
            // BodyShapeIds.Add(0x21, "SteamPunk");
            BodyShapeIds.Add(0x22, "SITES : Medieval");
            // BodyShapeIds.Add(0x23, "StoneAge");
            BodyShapeIds.Add(0x24, "SITES : Pirates");
            BodyShapeIds.Add(0x26, "SITES : Grungy");
            if (booby.PrettyGirls.IsTitsInstalled() || booby.PrettyGirls.IsAngelsInstalled())
                BodyShapeIds.Add(0x27, "Maxis : Castaway");
            BodyShapeIds.Add(0x29, "SITES : Super Heros");
            //BodyShapeIds.Add(0x2a, "Futuristic");
            BodyShapeIds.Add(0x2c, "Various : Various");
            BodyShapeIds.Add(0x2d, "Synaptic Sim : Werewolves");
            BodyShapeIds.Add(0x2f, "Creatures : Satyrs");
            BodyShapeIds.Add(0x30, "Creatures : Centaurs");
            BodyShapeIds.Add(0x31, "Creatures : Mermaid");
            BodyShapeIds.Add(0x33, "Synaptic Sim : Huge Body Builder Beast");
            if (booby.PrettyGirls.IsTitsInstalled() || booby.PrettyGirls.IsAngelsInstalled())
                BodyShapeIds.Add(0x35, "Chris H : Fannystein");
            else
                BodyShapeIds.Add(0x35, "Synaptic Sim : Nightcrawler - Nocturne");
            BodyShapeIds.Add(0x36, "Cynnix : Quarians");
            BodyShapeIds.Add(0x37, "MartaXL : Martaxlm");
            BodyShapeIds.Add(0x38, "DarkPsyFox : Fat Dark PsyFox");
            BodyShapeIds.Add(0x39, "Melodie9 : Fat Family Male");
            BodyShapeIds.Add(0x3a, "Netra : Chubby Guy");
            BodyShapeIds.Add(0x3b, "Consort : Consort's Fat Male");
            BodyShapeIds.Add(0x3d, "Synaptic Sim : Massive Body Builder");
            BodyShapeIds.Add(0x3f, "Montoto : Bear Body Builder");
            BodyShapeIds.Add(0x40, "Boesboxyboy-Marvine : Super Hero");
            BodyShapeIds.Add(0x41, "Boesboxyboy-Marvine : Huge Body Builder");
            BodyShapeIds.Add(0x43, "Boesboxyboy-Marvine : Body Body Builder");
            BodyShapeIds.Add(0x45, "Boesboxyboy-Marvine : Slim Body Builder");
            BodyShapeIds.Add(0x47, "Bloom : Neanderthal");
            BodyShapeIds.Add(0x48, "Zenman : Fit");
            BodyShapeIds.Add(0x49, "Boesboxyboy-Marvine : Athlete");
            BodyShapeIds.Add(0x4b, "Synaptic Sim : Lean Body Builder");
            BodyShapeIds.Add(0x4c, "Transgender(BCup) : Transgender B-Cup");
            BodyShapeIds.Add(0x4d, "Corrine : PunkJunkie");
            BodyShapeIds.Add(0x4e, "July77 : Slim Male");
            BodyShapeIds.Add(0x4f, "Melodie9 : Slim Family Male");
            if (booby.PrettyGirls.IsTitsInstalled() || booby.PrettyGirls.IsAngelsInstalled())
                BodyShapeIds.Add(0x52, "Chris H : Transgender");
            BodyShapeIds.Add(0x5c, "Bloom : Monster Jugs");
            BodyShapeIds.Add(0x5d, "Bloom : Hyper Busty");
            BodyShapeIds.Add(0x5f, "MartaXL : Martaxl");
            BodyShapeIds.Add(0x60, "Netra : Big Girl");
            BodyShapeIds.Add(0x61, "Melodie9 : Fat Family Female");
            BodyShapeIds.Add(0x62, "Netra : Thick Madame");
            BodyShapeIds.Add(0x63, "Franny Sims : Momma Lisa");
            BodyShapeIds.Add(0x64, "Faeriegurl : Fat Faerie gurl");
            BodyShapeIds.Add(0x65, "Warlokk : Booty Gal");
            BodyShapeIds.Add(0x66, "Telfin : Mountain Girl");
            BodyShapeIds.Add(0x67, "Mia Studios : Booty Cutie");
            BodyShapeIds.Add(0x68, "Netra : Curvy Mama");
            BodyShapeIds.Add(0x69, "Warlokk : Renaissance Gal");
            BodyShapeIds.Add(0x6a, "Jessica : Gypsy Rose Lee");
            BodyShapeIds.Add(0x6b, "Pierre : Teresa Queen");
            BodyShapeIds.Add(0x6c, "Cynnix : Buxum Wench");
            BodyShapeIds.Add(0x6d, "Warlokk : Voluptuous");
            BodyShapeIds.Add(0x6f, "Dr. Pixel : Well Rounded");
            BodyShapeIds.Add(0x70, "Netra : Curvy Girl");
            BodyShapeIds.Add(0x71, "Zenman : Big");
            BodyShapeIds.Add(0x72, "Warlokk : Power Gal");
            BodyShapeIds.Add(0x74, "Warlokk : Xenos Heroine");
            BodyShapeIds.Add(0x75, "Chris H : Body Builder Girl D");
            BodyShapeIds.Add(0x76, "Boesboxyboy-Marvine : Body Builder Girl");
            BodyShapeIds.Add(0x77, "Starangel : Curvy GirlS");
            BodyShapeIds.Add(0x79, "Pierre : Nichon Queen");
            BodyShapeIds.Add(0x7b, "Pierre : Divine Queen");
            BodyShapeIds.Add(0x7d, "Warlokk : Classic Pinup Gal");
            BodyShapeIds.Add(0x7f, "Pierre : Amour Queen");
            BodyShapeIds.Add(0x80, "Zenman : Young Elder");
            BodyShapeIds.Add(0x81, "Pierre : Beaute Queen");
            BodyShapeIds.Add(0x82, "Lipje : Round DCup");
            BodyShapeIds.Add(0x83, "Pierre : Cherie Queen");
            BodyShapeIds.Add(0x84, "Cylais : Swimsuit");
            BodyShapeIds.Add(0x86, "Vanity DeMise : Farmer Daughter");
            BodyShapeIds.Add(0x87, "Zenman : Curvier");
            BodyShapeIds.Add(0x88, "Oph3lia : SC");
            BodyShapeIds.Add(0x89, "Pierre : Olympe Queen");
            BodyShapeIds.Add(0x8b, "Boesboxyboy-Marvine : Athletic Girl");
            BodyShapeIds.Add(0x8e, "Jaccirocker : Statuesque");
            BodyShapeIds.Add(0x90, "Franny Sims : Kurvy K");
            BodyShapeIds.Add(0x92, "Warlokk : Toon Gal");
            BodyShapeIds.Add(0x94, "Bobby TH : Girl Next Door");
            BodyShapeIds.Add(0x95, "Chris H : Naughty Girl");
            BodyShapeIds.Add(0x96, "Warlokk : Rio Girl");
            BodyShapeIds.Add(0x97, "Wooden Bear : Hollywood");
            BodyShapeIds.Add(0x98, "Bloom : Ruben");
            BodyShapeIds.Add(0x99, "Bobby TH : BootyLicious G");
            BodyShapeIds.Add(0x9a, "Sussi : Sussi");
            BodyShapeIds.Add(0x9b, "Bobby TH : BootyLicious DD");
            BodyShapeIds.Add(0x9c, "DL Mulsow : HourGlass");
            BodyShapeIds.Add(0x9d, "Bobby TH : BootyLicious");
            BodyShapeIds.Add(0x9e, "Bobby TH : BootyLicious C");
            BodyShapeIds.Add(0x9f, "Bobby TH : Made Of Dreams");
            BodyShapeIds.Add(0xa3, "Rising Sun : Fantasy Girl");
            BodyShapeIds.Add(0xa6, "Pierre : Modele Queen");
            BodyShapeIds.Add(0xa8, "Pierre : Poupee Queen");
            BodyShapeIds.Add(0xaa, "Pierre : Chaton Queen");
            BodyShapeIds.Add(0xad, "Pierre : Darling Queen");
            BodyShapeIds.Add(0xaf, "Rising Sun : Dream Girl");
            BodyShapeIds.Add(0xb1, "Poppeboy : Fit Chick");
            BodyShapeIds.Add(0xb2, "Nemesis : Natural Beauty");
            BodyShapeIds.Add(0xb4, "Pierre : Petite Queen");
            BodyShapeIds.Add(0xb7, "Inebriant : SexyBum");
            BodyShapeIds.Add(0xb8, "Chris H : Fashion Model D-36");
            BodyShapeIds.Add(0xba, "Warlokk : Fashion Model");
            BodyShapeIds.Add(0xbc, "Gothplague : Androgyny");
            BodyShapeIds.Add(0xbe, "Warlokk : Faerie Gal");
            BodyShapeIds.Add(0xc0, "Gothplague : Miana");
            BodyShapeIds.Add(0xc1, "Melodie9 : SlimFamily Female");
            BodyShapeIds.Add(0xc3, "Warlokk : (teen) D X-Large");
            BodyShapeIds.Add(0xc4, "Warlokk : (teen) D Large");
            BodyShapeIds.Add(0xc5, "Warlokk : (teen) D Medium");
            BodyShapeIds.Add(0xc6, "Warlokk : (teen) C X-Large");
            BodyShapeIds.Add(0xc7, "Warlokk : (teen) C Large");
            BodyShapeIds.Add(0xc8, "Warlokk : (teen) C Medium");
            BodyShapeIds.Add(0xc9, "Warlokk : (teen) C Small");
            BodyShapeIds.Add(0xca, "Warlokk : (teen) B X-Large");
            BodyShapeIds.Add(0xcb, "Warlokk : (teen) B Large");
            BodyShapeIds.Add(0xcc, "Warlokk : (teen) B Small");
            BodyShapeIds.Add(0xcd, "Warlokk : (teen) A X-Large");
            BodyShapeIds.Add(0xce, "Warlokk : (teen) A Large");
            BodyShapeIds.Add(0xcf, "Warlokk : (teen) A Medium");
            BodyShapeIds.Add(0xd0, "Warlokk : (teen) A Small");
            BodyShapeIds.Add(0xd2, "Warlokk : DDD-40");
            BodyShapeIds.Add(0xd3, "Warlokk : DDD-38");
            BodyShapeIds.Add(0xd4, "Warlokk : DDD-36");
            BodyShapeIds.Add(0xd5, "Warlokk : DDD-34");
            BodyShapeIds.Add(0xd6, "Warlokk : DD-40");
            BodyShapeIds.Add(0xd7, "Warlokk : DD-38");
            BodyShapeIds.Add(0xd8, "Warlokk : DD-36");
            BodyShapeIds.Add(0xd9, "Warlokk : DD-34");
            BodyShapeIds.Add(0xda, "Warlokk : D-40");
            BodyShapeIds.Add(0xdb, "Warlokk : D-38");
            BodyShapeIds.Add(0xdc, "Warlokk : D-36");
            BodyShapeIds.Add(0xdd, "Warlokk : D-34");
            BodyShapeIds.Add(0xde, "Warlokk : D-32");
            BodyShapeIds.Add(0xdf, "Warlokk : C-40");
            BodyShapeIds.Add(0xe0, "Warlokk : C-38");
            BodyShapeIds.Add(0xe1, "Warlokk : C-36");
            BodyShapeIds.Add(0xe2, "Warlokk : C-34");
            BodyShapeIds.Add(0xe3, "Warlokk : C-32");
            BodyShapeIds.Add(0xe4, "Warlokk : B-40");
            BodyShapeIds.Add(0xe5, "Warlokk : B-38");
            BodyShapeIds.Add(0xe6, "Warlokk : B-36");
            BodyShapeIds.Add(0xe7, "Warlokk : B-32");
            BodyShapeIds.Add(0xe8, "Warlokk : A-40");
            BodyShapeIds.Add(0xe9, "Warlokk : A-38");
            BodyShapeIds.Add(0xea, "Warlokk : A-36");
            BodyShapeIds.Add(0xeb, "Warlokk : A-34");
            BodyShapeIds.Add(0xec, "Warlokk : A-32");
            BodyShapeIds.Add(0xee, "Warlokk : (tf Bottom) X-Large");
            BodyShapeIds.Add(0xef, "Warlokk : (tf Bottom) Large");
            BodyShapeIds.Add(0xf0, "Warlokk : (tf Bottom) Small");
            BodyShapeIds.Add(0xf2, "Warlokk : (AF Bottom) 40");
            BodyShapeIds.Add(0xf3, "Warlokk : (AF Bottom) 38");
            BodyShapeIds.Add(0xf4, "Warlokk : (AF Bottom) 36");
            BodyShapeIds.Add(0xf5, "Warlokk : (AF Bottom) 32");
            BodyShapeIds.Add(0xf7, "Warlokk : (Top) DDD");
            BodyShapeIds.Add(0xf8, "Warlokk : (Top) DD");
            BodyShapeIds.Add(0xf9, "Warlokk : (Top) D");
            BodyShapeIds.Add(0xfa, "Warlokk : (Top) C");
            BodyShapeIds.Add(0xfb, "Warlokk : (Top) MJ");
            BodyShapeIds.Add(0xfc, "Warlokk : (Top) A");
        }
	}
}
