/***************************************************************************
 *   Copyright (C) 2005 by Ambertation                                     *
 *   quaxi@ambertation.de                                                  *
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
using System.ComponentModel;
using System.Windows.Forms;

namespace SimPe.Plugin
{
	/// <summary>
	/// Zusammenfassung für Sims.
	/// </summary>
	public class Surgery : System.Windows.Forms.Form
	{
		private System.Windows.Forms.ImageList ilist;
		private System.Windows.Forms.ListView lv;
		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.GroupBox groupBox2;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.PictureBox pbpatient;
		private System.Windows.Forms.PictureBox pbarche;
		private System.Windows.Forms.LinkLabel llusepatient;
		private System.Windows.Forms.LinkLabel llusearche;
		private System.Windows.Forms.Label lbpatname;
		private System.Windows.Forms.Label lbpatlife;
		private System.Windows.Forms.Label lbarchlife;
		private System.Windows.Forms.Label lbarchname;
		private System.Windows.Forms.LinkLabel llexport;
		private System.Windows.Forms.SaveFileDialog sfd;
		private System.Windows.Forms.ToolTip toolTip1;
		private System.Windows.Forms.CheckBox cbskin;
		private System.Windows.Forms.GroupBox groupBox3;
		private System.Windows.Forms.ListView lvskin;
		private System.Windows.Forms.ImageList iskin;
		private System.Windows.Forms.CheckBox cbface;
		private System.Windows.Forms.CheckBox cbmakeup;
		private System.Windows.Forms.CheckBox cbeye;
		private System.ComponentModel.IContainer components;

		public Surgery()
		{
			//
			// Erforderlich für die Windows Form-Designerunterstützung
			//
			InitializeComponent();

			LoadSkins();
		}

		/// <summary>
		/// Die verwendeten Ressourcen bereinigen.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Vom Windows Form-Designer generierter Code
		/// <summary>
		/// Erforderliche Methode für die Designerunterstützung. 
		/// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
		/// </summary>
		private void InitializeComponent()
		{
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Surgery));
            this.ilist = new System.Windows.Forms.ImageList(this.components);
            this.lv = new System.Windows.Forms.ListView();
            this.button1 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label2 = new System.Windows.Forms.Label();
            this.cbeye = new System.Windows.Forms.CheckBox();
            this.cbmakeup = new System.Windows.Forms.CheckBox();
            this.llexport = new System.Windows.Forms.LinkLabel();
            this.lbpatlife = new System.Windows.Forms.Label();
            this.lbpatname = new System.Windows.Forms.Label();
            this.pbpatient = new System.Windows.Forms.PictureBox();
            this.llusepatient = new System.Windows.Forms.LinkLabel();
            this.cbface = new System.Windows.Forms.CheckBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.lbarchlife = new System.Windows.Forms.Label();
            this.lbarchname = new System.Windows.Forms.Label();
            this.llusearche = new System.Windows.Forms.LinkLabel();
            this.pbarche = new System.Windows.Forms.PictureBox();
            this.label3 = new System.Windows.Forms.Label();
            this.sfd = new System.Windows.Forms.SaveFileDialog();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.cbskin = new System.Windows.Forms.CheckBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.lvskin = new System.Windows.Forms.ListView();
            this.iskin = new System.Windows.Forms.ImageList(this.components);
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbpatient)).BeginInit();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbarche)).BeginInit();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // ilist
            // 
            this.ilist.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit;
            resources.ApplyResources(this.ilist, "ilist");
            this.ilist.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // lv
            // 
            resources.ApplyResources(this.lv, "lv");
            this.lv.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.lv.HideSelection = false;
            this.lv.LargeImageList = this.ilist;
            this.lv.MultiSelect = false;
            this.lv.Name = "lv";
            this.lv.SmallImageList = this.ilist;
            this.lv.Sorting = System.Windows.Forms.SortOrder.Ascending;
            this.lv.StateImageList = this.ilist;
            this.toolTip1.SetToolTip(this.lv, resources.GetString("lv.ToolTip"));
            this.lv.UseCompatibleStateImageBehavior = false;
            this.lv.SelectedIndexChanged += new System.EventHandler(this.SelectSim);
            this.lv.DoubleClick += new System.EventHandler(this.Open);
            // 
            // button1
            // 
            resources.ApplyResources(this.button1, "button1");
            this.button1.Name = "button1";
            this.toolTip1.SetToolTip(this.button1, resources.GetString("button1.ToolTip"));
            this.button1.Click += new System.EventHandler(this.Open);
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // groupBox1
            // 
            resources.ApplyResources(this.groupBox1, "groupBox1");
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.cbeye);
            this.groupBox1.Controls.Add(this.cbmakeup);
            this.groupBox1.Controls.Add(this.llexport);
            this.groupBox1.Controls.Add(this.lbpatlife);
            this.groupBox1.Controls.Add(this.lbpatname);
            this.groupBox1.Controls.Add(this.pbpatient);
            this.groupBox1.Controls.Add(this.llusepatient);
            this.groupBox1.Controls.Add(this.cbface);
            this.groupBox1.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.TabStop = false;
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.label2.Name = "label2";
            // 
            // cbeye
            // 
            resources.ApplyResources(this.cbeye, "cbeye");
            this.cbeye.Name = "cbeye";
            this.toolTip1.SetToolTip(this.cbeye, resources.GetString("cbeye.ToolTip"));
            this.cbeye.CheckedChanged += new System.EventHandler(this.cbskin_CheckedChanged);
            // 
            // cbmakeup
            // 
            resources.ApplyResources(this.cbmakeup, "cbmakeup");
            this.cbmakeup.Name = "cbmakeup";
            this.toolTip1.SetToolTip(this.cbmakeup, resources.GetString("cbmakeup.ToolTip"));
            this.cbmakeup.CheckedChanged += new System.EventHandler(this.cbskin_CheckedChanged);
            // 
            // llexport
            // 
            resources.ApplyResources(this.llexport, "llexport");
            this.llexport.Name = "llexport";
            this.llexport.TabStop = true;
            this.toolTip1.SetToolTip(this.llexport, resources.GetString("llexport.ToolTip"));
            this.llexport.UseCompatibleTextRendering = true;
            this.llexport.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.Export);
            // 
            // lbpatlife
            // 
            resources.ApplyResources(this.lbpatlife, "lbpatlife");
            this.lbpatlife.Name = "lbpatlife";
            // 
            // lbpatname
            // 
            resources.ApplyResources(this.lbpatname, "lbpatname");
            this.lbpatname.Name = "lbpatname";
            // 
            // pbpatient
            // 
            this.pbpatient.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            resources.ApplyResources(this.pbpatient, "pbpatient");
            this.pbpatient.Name = "pbpatient";
            this.pbpatient.TabStop = false;
            // 
            // llusepatient
            // 
            resources.ApplyResources(this.llusepatient, "llusepatient");
            this.llusepatient.Name = "llusepatient";
            this.llusepatient.TabStop = true;
            this.toolTip1.SetToolTip(this.llusepatient, resources.GetString("llusepatient.ToolTip"));
            this.llusepatient.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.UsePatient);
            // 
            // cbface
            // 
            resources.ApplyResources(this.cbface, "cbface");
            this.cbface.Name = "cbface";
            this.toolTip1.SetToolTip(this.cbface, resources.GetString("cbface.ToolTip"));
            this.cbface.CheckedChanged += new System.EventHandler(this.cbskin_CheckedChanged);
            // 
            // groupBox2
            // 
            resources.ApplyResources(this.groupBox2, "groupBox2");
            this.groupBox2.Controls.Add(this.lbarchlife);
            this.groupBox2.Controls.Add(this.lbarchname);
            this.groupBox2.Controls.Add(this.llusearche);
            this.groupBox2.Controls.Add(this.pbarche);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.TabStop = false;
            // 
            // lbarchlife
            // 
            resources.ApplyResources(this.lbarchlife, "lbarchlife");
            this.lbarchlife.Name = "lbarchlife";
            // 
            // lbarchname
            // 
            resources.ApplyResources(this.lbarchname, "lbarchname");
            this.lbarchname.Name = "lbarchname";
            // 
            // llusearche
            // 
            resources.ApplyResources(this.llusearche, "llusearche");
            this.llusearche.Name = "llusearche";
            this.llusearche.TabStop = true;
            this.toolTip1.SetToolTip(this.llusearche, resources.GetString("llusearche.ToolTip"));
            this.llusearche.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.UseArchetype);
            // 
            // pbarche
            // 
            this.pbarche.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            resources.ApplyResources(this.pbarche, "pbarche");
            this.pbarche.Name = "pbarche";
            this.pbarche.TabStop = false;
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.label3.Name = "label3";
            // 
            // sfd
            // 
            resources.ApplyResources(this.sfd, "sfd");
            // 
            // toolTip1
            // 
            this.toolTip1.AutoPopDelay = 30000;
            this.toolTip1.InitialDelay = 500;
            this.toolTip1.ReshowDelay = 100;
            // 
            // cbskin
            // 
            resources.ApplyResources(this.cbskin, "cbskin");
            this.cbskin.Name = "cbskin";
            this.toolTip1.SetToolTip(this.cbskin, resources.GetString("cbskin.ToolTip"));
            this.cbskin.CheckedChanged += new System.EventHandler(this.cbskin_CheckedChanged);
            // 
            // groupBox3
            // 
            resources.ApplyResources(this.groupBox3, "groupBox3");
            this.groupBox3.Controls.Add(this.cbskin);
            this.groupBox3.Controls.Add(this.lvskin);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.TabStop = false;
            // 
            // lvskin
            // 
            resources.ApplyResources(this.lvskin, "lvskin");
            this.lvskin.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.lvskin.HideSelection = false;
            this.lvskin.LargeImageList = this.iskin;
            this.lvskin.MultiSelect = false;
            this.lvskin.Name = "lvskin";
            this.lvskin.UseCompatibleStateImageBehavior = false;
            this.lvskin.SelectedIndexChanged += new System.EventHandler(this.lvskin_SelectedIndexChanged);
            // 
            // iskin
            // 
            this.iskin.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit;
            resources.ApplyResources(this.iskin, "iskin");
            this.iskin.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // Surgery
            // 
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.lv);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.groupBox3);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Name = "Surgery";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbpatient)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbarche)).EndInit();
            this.groupBox3.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		#endregion

		protected void AddImage(SimPe.PackedFiles.Wrapper.ExtSDesc sdesc) 
		{
			if (sdesc.Image!=null) 
			{
				if ((sdesc.Unlinked!=0x00) || (!sdesc.AvailableCharacterData) || sdesc.IsNPC)
				{
					Image img = (Image)sdesc.Image.Clone();
					System.Drawing.Graphics g = Graphics.FromImage(img);
					g.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
					g.CompositingMode = System.Drawing.Drawing2D.CompositingMode.SourceOver;

					Pen pen = new Pen(Data.MetaData.SpecialSimColor, 3);

					g.FillRectangle(pen.Brush, 0, 0, img.Width, img.Height);

					int pos = 2;
					if (sdesc.Unlinked!=0x00) 
					{
						g.FillRectangle(new SolidBrush(Data.MetaData.UnlinkedSim), pos, 2, 25, 25);
						pos += 28;
					}
					if (!sdesc.AvailableCharacterData) 
					{
						g.FillRectangle(new SolidBrush(Data.MetaData.InactiveSim), pos, 2, 25, 25);
						pos += 28;
					}
					if (sdesc.IsNPC) 
					{
						g.FillRectangle(new SolidBrush(Data.MetaData.NPCSim), pos, 2, 25, 25);
						pos += 28;
					}

					this.ilist.Images.Add(img);
				} 
				else 
				{
					this.ilist.Images.Add(sdesc.Image);
				}
			} 
			else 
			{
				this.ilist.Images.Add(new Bitmap(this.GetType().Assembly.GetManifestResourceStream("SimPe.Plugin.Network.png")));
			}
		}

		protected void AddSim(SimPe.PackedFiles.Wrapper.ExtSDesc sdesc) 
		{
			//if (!sdesc.HasImage) return;
			if (!sdesc.AvailableCharacterData) return;
#if DEBUG
#else
			if (sdesc.IsNPC) return;
#endif
			

			AddImage(sdesc);
			ListViewItem lvi = new ListViewItem();
			lvi.Text = sdesc.SimName +" "+sdesc.SimFamilyName;
			lvi.ImageIndex = ilist.Images.Count -1;
			lvi.Tag = sdesc;

			lv.Items.Add(lvi);
		}

		Hashtable skinfiles;
		void LoadSkins()
		{
			WaitingScreen.Wait();
			try 
			{
				skinfiles = new Hashtable();
				ArrayList tones = new ArrayList();
				iskin.Images.Add(new Bitmap(iskin.ImageSize.Width, iskin.ImageSize.Height));
				ListViewItem lvia = new ListViewItem("* from Archetype");
				lvia.ImageIndex = 0;
				this.lvskin.Items.Add(lvia);
				lvia.Selected = true;

				FileTable.FileIndex.Load();
				SimPe.Interfaces.Scenegraph.IScenegraphFileIndexItem[] items = FileTable.FileIndex.FindFile(Data.MetaData.GZPS, true);				
				foreach (SimPe.Interfaces.Scenegraph.IScenegraphFileIndexItem item in items)
				{
					SimPe.PackedFiles.Wrapper.Cpf skin = new SimPe.PackedFiles.Wrapper.Cpf();
					skin.ProcessData(item);

					//Maintain a List of all availabe SkinsFiles per skintone
					ArrayList files = null;
					string st = skin.GetSaveItem("skintone").StringValue;
					if (skinfiles.ContainsKey(st)) 
					{
						files = (ArrayList)skinfiles[st];
					}
					else 
					{
						files = new ArrayList();
						skinfiles[st] = files;
					}
					files.Add(skin);

					if ((skin.GetSaveItem("override0subset").StringValue=="top") && (skin.GetSaveItem("type").StringValue=="skin") && ((skin.GetSaveItem("category").UIntegerValue&(uint)Data.SkinCategories.Skin)==(uint)Data.SkinCategories.Skin))
					{
						WaitingScreen.UpdateMessage(skin.GetSaveItem("name").StringValue);

						if (tones.Contains(st)) continue;
						else tones.Add(st);

						SimPe.Interfaces.Scenegraph.IScenegraphFileIndexItem[] idr = FileTable.FileIndex.FindFile(0xAC506764, item.FileDescriptor.Group, item.FileDescriptor.LongInstance, null);
						if (idr.Length>0) 
						{
							SimPe.Plugin.RefFile reffile = new RefFile();
							reffile.ProcessData(idr[0]);

							ListViewItem lvi = new ListViewItem(skin.GetSaveItem("name").StringValue);
							if (Helper.DebugMode) lvi.Text += " ("+skin.GetSaveItem("skintone").StringValue+")";
							lvi.Tag = skin.GetSaveItem("skintone").StringValue;
							foreach (Interfaces.Files.IPackedFileDescriptor pfd in reffile.Items) 
							{								
								if (pfd.Type == Data.MetaData.TXMT) 
								{
									SimPe.Interfaces.Scenegraph.IScenegraphFileIndexItem[] txmts = FileTable.FileIndex.FindFile(pfd, null);
									if (txmts.Length>0) 
									{
										SimPe.Plugin.Rcol rcol = new GenericRcol(null, false);
										rcol.ProcessData(txmts[0]);

										MaterialDefinition md = (MaterialDefinition)rcol.Blocks[0];
										string txtrname = md.FindProperty("stdMatBaseTextureName").Value+"_txtr";

										SimPe.Interfaces.Scenegraph.IScenegraphFileIndexItem txtri = FileTable.FileIndex.FindFileByName(txtrname, Data.MetaData.TXTR, Data.MetaData.LOCAL_GROUP, true);
										if (txtri!=null) 
										{
											rcol = new GenericRcol(null, false);
											rcol.ProcessData(txtri);

											ImageData id = (ImageData)rcol.Blocks[0];
											MipMap mm = id.GetLargestTexture(iskin.ImageSize);

											if (mm!=null) 
											{
												iskin.Images.Add(ImageLoader.Preview(mm.Texture, iskin.ImageSize));
												lvi.ImageIndex = iskin.Images.Count-1;
											}
										}
									
									}
								}
							} //foreach reffile.Items
							
							lvskin.Items.Add(lvi);
						} //if idr
					}
				} //foreach items
			} 
			finally 
			{
				WaitingScreen.Stop();
			}
		}

		SimPe.Interfaces.Files.IPackedFileDescriptor pfd;
		Interfaces.IProviderRegistry prov;
		SimPe.Interfaces.Files.IPackageFile ngbh;
		public Interfaces.Plugin.IToolResult Execute(ref SimPe.Interfaces.Files.IPackedFileDescriptor pfd, ref SimPe.Interfaces.Files.IPackageFile package, Interfaces.IProviderRegistry prov) 
		{
			this.Cursor = Cursors.WaitCursor;
			
			this.pfd = null;
			this.prov = prov;
			this.ngbh = package;

			this.pbarche.Image = null;
			this.pbpatient.Image = null;

			this.lbpatlife.Text = "Lifestage";
			this.lbpatname.Text = "Name";

			this.lbarchlife.Text = "Lifestage";
			this.lbarchname.Text = "Name";

			this.spatient = null;
			this.sarche = null;

			button1.Enabled = CanDo();

			ilist.Images.Clear();
			lv.Items.Clear();

			

			Interfaces.Files.IPackedFileDescriptor[] pfds = package.FindFiles(Data.MetaData.SIM_DESCRIPTION_FILE);
			WaitingScreen.Wait();
            try
            {
                foreach (Interfaces.Files.IPackedFileDescriptor spfd in pfds)
                {

                    PackedFiles.Wrapper.ExtSDesc sdesc = new SimPe.PackedFiles.Wrapper.ExtSDesc();
                    sdesc.ProcessData(spfd, package);

                    //WaitingScreen.UpdateImage(ImageLoader.Preview(sdesc.Image, new Size(64, 64)));
                    AddSim(sdesc);
                }

                this.Cursor = Cursors.Default;
                this.llusearche.Enabled = false;
                this.llusepatient.Enabled = false;
                this.llexport.Enabled = false;
                button1.Enabled = false;
                if (lv.Items.Count > 0) lv.Items[0].Selected = true;



            }
            finally { WaitingScreen.Stop(this); }
			RemoteControl.ShowSubForm(this);

			if (this.pfd!=null) pfd = this.pfd;
			return new Plugin.ToolResult((this.pfd!=null), false);
		}

		private void Open(object sender, System.EventArgs e)
		{
			if (!CanDo())  return;
			

			SimPe.Packages.File patient = SimPe.Packages.File.LoadFromFile(spatient.CharacterFileName);
			SimPe.Packages.File archetype = null;
			if (sarche!=null) archetype = SimPe.Packages.File.LoadFromFile(sarche.CharacterFileName);
			else archetype = SimPe.Packages.File.LoadFromFile(null);

			SimPe.Packages.GeneratableFile newpackage = null;
			PlasticSurgery ps = new PlasticSurgery(ngbh, patient, spatient, archetype, sarche);

			if (!this.cbskin.Checked && !this.cbface.Checked && !this.cbmakeup.Checked && !this.cbeye.Checked) newpackage = ps.CloneSim();

			if (this.cbskin.Checked) 
			{
				if (lvskin.SelectedItems.Count==0) return;
				string skin = (string)lvskin.SelectedItems[0].Tag;
				if (skin==null) newpackage = ps.CloneSkinTone(skinfiles);
				else newpackage = ps.CloneSkinTone(skin, skinfiles);
			}

			if (this.cbface.Checked) 
			{
				if (this.cbskin.Checked) ps = new PlasticSurgery(ngbh, newpackage, spatient, archetype, sarche);
				newpackage = ps.CloneFace();
			}

			if (this.cbmakeup.Checked) 
			{
				if ((this.cbskin.Checked) || (this.cbface.Checked)) ps = new PlasticSurgery(ngbh, newpackage, spatient, archetype, sarche);
				newpackage = ps.CloneMakeup(false, true);
			}

			if (this.cbeye.Checked) 
			{
				if ((this.cbskin.Checked) || (this.cbface.Checked) || (this.cbmakeup.Checked)) ps = new PlasticSurgery(ngbh, newpackage, spatient, archetype, sarche);
				newpackage = ps.CloneMakeup(true, false);
			}
			

			if (newpackage != null) 
			{
				newpackage.Save(spatient.CharacterFileName);
				prov.SimNameProvider.StoredData = null;
				Close();
			}
		}

		private void SelectSim(object sender, System.EventArgs e)
		{
			this.llusearche.Enabled = false;
			this.llusepatient.Enabled = false;
			if (lv.SelectedItems.Count==0) return;
			this.llusearche.Enabled = true;


			this.llusepatient.Enabled = !((SimPe.PackedFiles.Wrapper.ExtSDesc)lv.SelectedItems[0].Tag).IsNPC;
		}

		SimPe.PackedFiles.Wrapper.SDesc spatient = null;
		SimPe.PackedFiles.Wrapper.SDesc sarche = null;
		private void UsePatient(object sender, System.Windows.Forms.LinkLabelLinkClickedEventArgs e)
		{
			this.llexport.Enabled = (spatient!=null);
			if (lv.SelectedItems.Count==0) return;
			if (lv.SelectedItems[0].ImageIndex>=0) pbpatient.Image = ilist.Images[lv.SelectedItems[0].ImageIndex];

			this.lbpatname.Text = lv.SelectedItems[0].Text;
			
			spatient = (SimPe.PackedFiles.Wrapper.SDesc)lv.SelectedItems[0].Tag;
			lbpatlife.Text = spatient.CharacterDescription.LifeSection.ToString();
			lbpatlife.Text += ", " + spatient.CharacterDescription.Gender.ToString();

			button1.Enabled =  (pbpatient.Image!=null) && (pbarche.Image!=null);
			pfd = (SimPe.Interfaces.Files.IPackedFileDescriptor)spatient.FileDescriptor;
			this.llexport.Enabled = (spatient!=null);
		}

		private void UseArchetype(object sender, System.Windows.Forms.LinkLabelLinkClickedEventArgs e)
		{
			if (lv.SelectedItems.Count==0) return;
			if (lv.SelectedItems[0].ImageIndex>=0) this.pbarche.Image = ilist.Images[lv.SelectedItems[0].ImageIndex];

			iskin.Images[0] = ImageLoader.Preview(pbarche.Image, iskin.ImageSize);
			lvskin.Refresh();

			this.lbarchname.Text = lv.SelectedItems[0].Text;
			
			sarche = (SimPe.PackedFiles.Wrapper.SDesc)lv.SelectedItems[0].Tag;
			lbarchlife.Text = sarche.CharacterDescription.LifeSection.ToString();
			lbarchlife.Text += ", " + sarche.CharacterDescription.Gender.ToString();

			button1.Enabled =  (pbpatient.Image!=null) && (pbarche.Image!=null);
		}

		protected void FaceSurgery()
		{
			try 
			{
				SimPe.Packages.GeneratableFile patient = SimPe.Packages.GeneratableFile.LoadFromFile(spatient.CharacterFileName);
				SimPe.Packages.File archetype = SimPe.Packages.File.LoadFromFile(sarche.CharacterFileName);

				//Load Facial Data
				Interfaces.Files.IPackedFileDescriptor[] apfds = archetype.FindFiles(0xCCCEF852); //LxNR, Face
				if (apfds.Length==0) return;
				Interfaces.Files.IPackedFile file = archetype.Read(apfds[0]);

				Interfaces.Files.IPackedFileDescriptor[] ppfds = patient.FindFiles(0xCCCEF852); //LxNR, Face
				if (ppfds.Length==0) return;

				ppfds[0].UserData = file.UncompressedData;

#if DEBUG
				//Load Shape Data
				/*apfds = archetype.FindFiles(0xFC6EB1F7); //SHPE
				if (apfds.Length==0) return;
				file = archetype.Read(apfds[0]);

				ppfds = patient.FindFiles(0xFC6EB1F7); //SHPE
				if (ppfds.Length==0) return;

				ppfds[0].UserData = file.UncompressedData;*/
#endif
				
				//System.IO.MemoryStream ms = patient.Build();
				//patient.Reader.Close();
				patient.Save(spatient.CharacterFileName);
			} 
			catch (Exception ex)
			{
				Helper.ExceptionMessage("Unable to update the new Character Package.", ex);
			}
		}

		private void Export(object sender, System.Windows.Forms.LinkLabelLinkClickedEventArgs e)
		{
			if (spatient==null) return;


			try 
			{
				//list of all Files top copy from the Archetype
				ArrayList list = new ArrayList();
				list.Add((uint)0xAC506764); //3IDR
				list.Add((uint)0xE519C933); //CRES
				list.Add((uint)0xEBCF3E27); //GZPS, Property Set
				list.Add((uint)0xAC598EAC); //AGED
				list.Add((uint)0xCCCEF852); //LxNR, Face
				list.Add((uint)0x0C560F39); //BINX
				list.Add((uint)0xAC4F8687); //GMDC
				list.Add((uint)0x7BA3838C); //GMND				
				list.Add((uint)0x49596978); //MATD
				list.Add((uint)0xFC6EB1F7); //SHPE

				System.IO.BinaryReader br1 = new System.IO.BinaryReader(this.GetType().Assembly.GetManifestResourceStream("SimPe.Plugin.3d.simpe"));
				System.IO.BinaryReader br2 = new System.IO.BinaryReader(this.GetType().Assembly.GetManifestResourceStream("SimPe.Plugin.bin.simpe"));

				SimPe.Packages.PackedFileDescriptor pfd1 = new SimPe.Packages.PackedFileDescriptor();
				pfd1.Group = 0xffffffff; pfd1.SubType = 0x00000000; pfd1.Instance = 0xFF123456; pfd1.Type = 0xAC506764; //3IDR
				pfd1.UserData = br1.ReadBytes((int)br1.BaseStream.Length);

				SimPe.Packages.PackedFileDescriptor pfd2 = new SimPe.Packages.PackedFileDescriptor();
				pfd2.Group = 0xffffffff; pfd2.SubType = 0x00000000; pfd2.Instance = 0xFF123456; pfd2.Type = 0x0C560F39; //BINX
				pfd2.UserData = br2.ReadBytes((int)br2.BaseStream.Length);

                sfd.InitialDirectory = System.IO.Path.Combine(PathProvider.SimSavegameFolder, "SavedSims");
				sfd.FileName = System.IO.Path.GetFileNameWithoutExtension(spatient.CharacterFileName);

				SimPe.Packages.GeneratableFile source = SimPe.Packages.GeneratableFile.LoadFromFile(spatient.CharacterFileName);
				if (sfd.ShowDialog()==DialogResult.OK) 
				{
					SimPe.Packages.GeneratableFile patient = SimPe.Packages.GeneratableFile.LoadFromStream((System.IO.BinaryReader)null);
					patient.FileName = "";
					patient.Add(pfd1);
					patient.Add(pfd2);

					foreach (Interfaces.Files.IPackedFileDescriptor pfd in source.Index) 
					{
						if (list.Contains(pfd.Type)) 
						{
							Interfaces.Files.IPackedFile file = source.Read(pfd);
							pfd.UserData = file.UncompressedData;
							patient.Add(pfd);

							if ((pfd.Type == Data.MetaData.GZPS) || (pfd.Type == 0xAC598EAC)) //property set and 3IDR
							{
								SimPe.PackedFiles.Wrapper.Cpf cpf = new SimPe.PackedFiles.Wrapper.Cpf();
								cpf.ProcessData(pfd, patient);

								SimPe.PackedFiles.Wrapper.CpfItem ci = new SimPe.PackedFiles.Wrapper.CpfItem();
								ci.Name = "product";
								ci.UIntegerValue = 0;
								cpf.AddItem(ci, false);

								ci = cpf.GetItem("version");
								if (ci==null) 
								{
									ci = new SimPe.PackedFiles.Wrapper.CpfItem();
									ci.Name = "version";
									if ((cpf.GetSaveItem("age").UIntegerValue&(uint)Data.Ages.YoungAdult)!=0) ci.UIntegerValue = 2;
									else ci.UIntegerValue = 1;
									cpf.AddItem(ci);
								}
								

								cpf.SynchronizeUserData();
							}
						}
					}

					patient.Save(sfd.FileName);
				}
			} 
			catch (Exception ex)
			{
				Helper.ExceptionMessage("", ex);
			}
			
		}

		bool CanDo()
		{
			if (spatient == null) return false;

			bool ret = true;
			if (cbskin.Checked)
			{
				ret = (lvskin.SelectedItems.Count==1);
				if (ret) if (lv.Items[0].Selected && (sarche == null)) ret=false;
				
			} 
			
			if (!cbskin.Checked || cbface.Checked || cbmakeup.Checked || cbeye.Checked)
			{
				ret = ret && (sarche != null);
			}

			return ret;
		}

		private void cbskin_CheckedChanged(object sender, System.EventArgs e)
		{
			lvskin.Enabled = this.cbskin.Checked;
			lvskin_SelectedIndexChanged(null, null);
			button1.Enabled = CanDo();
		}

		private void lvskin_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			button1.Enabled = CanDo();
		}
	}
}
