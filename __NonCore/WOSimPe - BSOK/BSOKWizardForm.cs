/**************************************************************************
 *   Copyright (C) 2023 by Chris Hatch                                    *
 *   (original author, BSOK Wizard)                                       *
 *                                                                         *
 *   Copyright (C) 2025 by GramzeSweatShop                                *
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
 **************************************************************************/
using System;
using System.Collections.Generic;
using System.IO;
using SimPe.Scenegraph.Compat;

namespace SimPe.Wizards
{
    /// <summary>
    /// Summary description for BsokWizardForm.
    /// </summary>
    public class BsokWizardForm : Avalonia.Controls.Window
    {
        private Avalonia.Controls.TabControl tabControl1 = new Avalonia.Controls.TabControl();
        private Avalonia.Controls.TabItem tabPage1 = new Avalonia.Controls.TabItem();
        private Avalonia.Controls.TabItem tabPage2 = new Avalonia.Controls.TabItem();
        internal Avalonia.Controls.Panel pnwizard1 = new Avalonia.Controls.Panel();
        internal Avalonia.Controls.Panel pnwizard2 = new Avalonia.Controls.Panel();
        private Avalonia.Controls.Label label1 = new Avalonia.Controls.Label();
        private Avalonia.Controls.Label label2 = new Avalonia.Controls.Label();
        private Avalonia.Controls.Label lboops = new Avalonia.Controls.Label();
        private Avalonia.Controls.Label lbPath = new Avalonia.Controls.Label();
        internal Avalonia.Controls.Label lbDone = new Avalonia.Controls.Label();
        internal Avalonia.Controls.Button linkLabel1 = new Avalonia.Controls.Button();
        private Avalonia.Controls.Button linkyicon1 = new Avalonia.Controls.Button();
        private Avalonia.Controls.Button button1 = new Avalonia.Controls.Button();
        private SimPe.Scenegraph.Compat.PictureBox pbicon = new SimPe.Scenegraph.Compat.PictureBox();
        private Avalonia.Controls.TextBox rtb = new Avalonia.Controls.TextBox();
        internal Avalonia.Controls.TextBox rtbAbout = new Avalonia.Controls.TextBox();
        internal Avalonia.Controls.ComboBox cbShapes = new Avalonia.Controls.ComboBox();
        internal SimPe.Scenegraph.Compat.ListView lvpackages = new SimPe.Scenegraph.Compat.ListView();
        internal System.Windows.Forms.FolderBrowserDialog fbd1 = new System.Windows.Forms.FolderBrowserDialog();
        internal string floder = null;
        internal Step1 step1;
        internal Step2 step2;
        internal Step3 step3;
        internal SimPe.Packages.File pak;
        private bool foun = false;
        internal Dictionary<uint, string> BodyShapeIds = new Dictionary<uint, string>();

        public BsokWizardForm()
        {
            Title = "BsokWizardForm";
            Width = 660;
            Height = 260;

            BuildLayout();

            if (SimPe.Helper.SimPeVersionLong < 330717003777) // the first 77 version
            {
                button1.IsEnabled = false;
                lboops.IsVisible = true;
                linkyicon1.IsVisible = label1.IsVisible = false;
                lboops.Content = "This Version of SimPe is too old";
            }
            else
            {
                InitializeBodyShapes();
                LoadHelpFile();

                foreach (KeyValuePair<uint, string> kvp in BodyShapeIds)
                    cbShapes.Items.Add(kvp.Value);

                pak = SimPe.Packages.File.LoadFromFile(System.IO.Path.Combine(PathProvider.Global.Latest.InstallFolder, "TSData\\Res\\UI\\ui.package"));
                if (System.IO.Directory.Exists(System.IO.Path.Combine(PathProvider.SimSavegameFolder, "Downloads")))
                {
                    string[] files = System.IO.Directory.GetFiles(System.IO.Path.Combine(PathProvider.SimSavegameFolder, "Downloads"), "Bodyshape Icons.package", SearchOption.AllDirectories);
                    if (files.Length > 0) pak = SimPe.Packages.File.LoadFromFile(files[0]);
                }
            }
        }

        private void BuildLayout()
        {
            // Tab 1: Step 1 — Browse for folder
            label1.Content = "Select the Browse button to choose a folder of outfits to configure";
            lboops.Content = "There is no outfits there";
            lboops.IsVisible = false;
            button1.Content = "Browse..";
            button1.Click += button1_Click;
            linkyicon1.Content = "About...";
            linkyicon1.Click += linkyicon1_Click;
            rtbAbout.IsVisible = false;
            rtbAbout.AcceptsReturn = true;
            rtbAbout.IsReadOnly = true;
            rtb.IsVisible = false;
            rtb.AcceptsReturn = true;

            var step1Stack = new Avalonia.Controls.StackPanel { Margin = new Avalonia.Thickness(4) };
            var step1TopRow = new Avalonia.Controls.StackPanel { Orientation = Avalonia.Layout.Orientation.Horizontal, HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Right };
            step1TopRow.Children.Add(linkyicon1);
            step1Stack.Children.Add(step1TopRow);
            step1Stack.Children.Add(label1);
            step1Stack.Children.Add(button1);
            step1Stack.Children.Add(lboops);
            step1Stack.Children.Add(lbPath);
            step1Stack.Children.Add(rtbAbout);
            pnwizard1.Children.Add(step1Stack);
            tabPage1.Header = "Step 1";
            tabPage1.Content = pnwizard1;

            // Tab 2: Step 2 — Select body shape
            label2.Content = "Select a Body Shape for the selected outfits";
            lbDone.Content = "The selected files have been BSOK'd, unselected files were not altered";
            lbDone.IsVisible = false;
            linkLabel1.Content = "Sort by Creator";
            linkLabel1.Click += linkLabel1_Click;
            cbShapes.SelectionChanged += cbShapes_SelectedIndexChanged;

            var step2Stack = new Avalonia.Controls.StackPanel { Margin = new Avalonia.Thickness(4) };
            step2Stack.Children.Add(label2);
            var step2Row = new Avalonia.Controls.StackPanel { Orientation = Avalonia.Layout.Orientation.Horizontal, Spacing = 8 };
            step2Row.Children.Add(cbShapes);
            step2Row.Children.Add(linkLabel1);
            step2Stack.Children.Add(step2Row);
            step2Stack.Children.Add(lbDone);
            pnwizard2.Children.Add(step2Stack);
            tabPage2.Header = "Step 2";
            tabPage2.Content = pnwizard2;

            tabControl1.Items.Add(tabPage1);
            tabControl1.Items.Add(tabPage2);

            Content = tabControl1;
        }

        #region 1 Find a Folder
        void LoadHelpFile()
        {
            Stream s;

            if (SimPe.Helper.SimPeVersionLong >= 330717003790 && File.Exists(Path.Combine(Helper.SimPeDataPath, "additional_skins.xml")))
                s = this.GetType().Assembly.GetManifestResourceStream("SimPe.Wizards.About.rtf");
            else
                s = this.GetType().Assembly.GetManifestResourceStream("SimPe.Wizards.About2.rtf");
            if (s != null)
            {
                StreamReader sr = new StreamReader(s);
                rtbAbout.Text = sr.ReadToEnd();
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
                    rtb.Text = sr.ReadToEnd();
                }
                finally
                {
                    sr.Close();
                    sr.Dispose();
                }
            }
            else
                rtb.Text = "<?xml version=\"1.0\" encoding=\"UTF-8\" ?>\r\n<alias>\r\n</alias>";
        }

        private void button1_Click(object sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            if (fbd1.ShowDialog() == SimPe.DialogResult.OK)
            {
                string[] stuff = Directory.GetFiles(fbd1.SelectedPath, "*.package");
                if (stuff.Length > 0)
                {
                    floder = fbd1.SelectedPath;
                    lboops.IsVisible = false;
                    PopulatFileList();
                }
                else
                {
                    floder = null;
                    lboops.IsVisible = true;
                }
                lbPath.Content = fbd1.SelectedPath;
            }
            else
            {
                floder = null;
                lboops.IsVisible = false;
                lbPath.Content = "";
            }
            if (step1 != null) step1.Update();
        }

        private void linkyicon1_Click(object sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            rtbAbout.IsVisible = !rtbAbout.IsVisible;
        }
        #endregion

        #region 2 Select a BodyShape
        void PopulatFileList()
        {
            lvpackages.Items.Clear();
            if (floder == null) return;
            string[] stuff = Directory.GetFiles(floder, "*.package");

            if (stuff.Length > 0)
            {
                try
                {
                    foreach (string file in stuff)
                    {
                        SimPe.Scenegraph.Compat.ListViewItem li = new SimPe.Scenegraph.Compat.ListViewItem();
                        li.Text = Path.GetFileNameWithoutExtension(file);
                        li.Tag = file;
                        li.Checked = true;
                        lvpackages.Items.Add(li);
                    }
                }
                catch { }
            }
        }

        private void cbShapes_SelectedIndexChanged(object sender, Avalonia.Controls.SelectionChangedEventArgs e)
        {
            if (GetBodyShapeId(cbShapes.SelectedItem) > 0)
                pbicon.Image = GetBodyIcon(Convert.ToByte(GetBodyShapeId(cbShapes.SelectedItem) - 1));
            else pbicon.Image = null;
            if (step2 != null) step2.Update();
        }

        private void linkLabel1_Click(object sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            cbShapes.SelectedIndex = -1;
            pbicon.Image = null;
            if (step2 != null) step2.Update();
        }
        #endregion

        internal void DoTheWork()
        {
            Interfaces.Files.IPackedFileDescriptor[] pfds;
            foreach (SimPe.Scenegraph.Compat.ListViewItem li in lvpackages.CheckedItems)
            {
                SimPe.Packages.GeneratableFile file = SimPe.Packages.GeneratableFile.LoadFromFile((string)li.Tag);
                pfds = file.FindFiles(0x4C158081);
                if (pfds.Length > 0) { AddImIn(pfds, file); li.Checked = false; continue; }
                string creat;
                if (GetBodyShapeId(cbShapes.SelectedItem) == 0)
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
            lbDone.IsVisible = true;
            linkLabel1.IsVisible = false;
            lvpackages.Enabled = cbShapes.IsEnabled = false;
            if (foun && SimPe.Helper.SimPeVersionLong >= 330717003790)
            {
                StreamWriter sw = File.CreateText(Path.Combine(Helper.SimPeDataPath, "additional_skins.xml"));
                try
                {
                    string titty = "";
                    foreach (string boob in rtb.Text.Split('\n')) titty += boob + "\r\n";
                    sw.Write(titty);
                }
                finally
                {
                    sw.Flush();
                    sw.Close();
                    sw.Dispose();
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

                foreach (string boob in rtb.Text.Split('\n'))
                {
                    if (!boob.Contains("</alias>") && !boob.Contains(fr.StringValue) && boob != "")
                        titty += (boob + "\r\n");
                }
                titty += "<item value=\"" + "0x" + Helper.HexString(GetBodyShapeId(cbShapes.SelectedItem)) + "\">" + fr.StringValue + "</item>\r\n</alias>";
                rtb.Text = titty;
            }
        }

        System.Drawing.Image GetBodyIcon(byte bs)
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
            BodyShapeIds.Add(0x16, "Not a Bodyshape : Gold Star");
            BodyShapeIds.Add(0x17, "Not a Bodyshape : Silver Star");
            BodyShapeIds.Add(0x1e, "Maxis : Maxis");
            BodyShapeIds.Add(0x20, "SITES : Goth");
            BodyShapeIds.Add(0x22, "SITES : Medieval");
            BodyShapeIds.Add(0x24, "SITES : Pirates");
            BodyShapeIds.Add(0x26, "SITES : Grungy");
            BodyShapeIds.Add(0x29, "SITES : Super Heros");
            BodyShapeIds.Add(0x2c, "Various : Various");
            BodyShapeIds.Add(0x2d, "Synaptic Sim : Werewolves");
            BodyShapeIds.Add(0x2f, "Creatures : Satyrs");
            BodyShapeIds.Add(0x30, "Creatures : Centaurs");
            BodyShapeIds.Add(0x31, "Creatures : Mermaid");
            BodyShapeIds.Add(0x33, "Synaptic Sim : Huge Body Builder Beast");
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
        }
    }
}
