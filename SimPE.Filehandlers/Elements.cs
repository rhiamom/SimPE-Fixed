/***************************************************************************
 *   Copyright (C) 2005 by Ambertation                                     *
 *   quaxi@ambertation.de                                                  *
 *                                                                         *
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
using System.Collections.Generic;
using System.Collections;
using Avalonia.Controls;
using SimPe.PackedFiles.Wrapper.Supporting;
using SimPe.Data;
using static SimPe.Data.LocalizedNeighbourhoodEP;

namespace SimPe.PackedFiles.UserInterface
{
	/// <summary>
	/// Avalonia port of the WinForms Elements form.
	/// Acts as a data-container/panel-host for UIBase-derived handlers.
	/// </summary>
	internal class Elements : UserControl
	{
		// ── JPEG / Picture panel ─────────────────────────────────────────────
		private Panel panel2 = new Panel();
		private TextBlock banner = new TextBlock();
		internal Image pb = new Image();
        internal Image pbImage = new Image();
        internal Panel JpegPanel = new Panel();
		internal Panel xmlPanel = new Panel();
		private Panel panel3 = new Panel();
		private TextBlock label1 = new TextBlock();
		internal TextBox rtb = new TextBox { AcceptsReturn = true };
        private Button visualStyleLinkLabel2 = new Button { Content = "Commit XML" };
        internal TextBox tbsimid = new TextBox();
		private TextBlock label8 = new TextBlock();
		private Panel panel6 = new Panel();
		private TextBlock label12 = new TextBlock();
		internal Panel objdPanel = new Panel();
		internal TextBox tbsimname = new TextBox();
		private TextBlock label9 = new TextBlock();
		private TabControl tabControl1 = new TabControl();
		private TabItem tabPage1 = new TabItem();
		internal Panel famiPanel = new Panel();
		internal TextBox tblotinst = new TextBox();
		internal TextBlock label15 = new TextBlock();
		private Button llFamiDeleteSim = new Button { Content = "Remove" };
		private Button llFamiAddSim = new Button { Content = "Add" };
		private Button button1 = new Button { Content = "Commit" };
		internal ComboBox cbsims = new ComboBox();
		internal ListBox lbmembers = new ListBox();
		internal TextBox tbname = new TextBox();
		private TextBlock label6 = new TextBlock();
		internal TextBox tbfamily = new TextBox();
		internal TextBox tbmoney = new TextBox();
		private TextBlock label5 = new TextBlock();
		private TextBlock label4 = new TextBlock();
		internal TextBlock label3 = new TextBlock();
		internal Panel panel4 = new Panel();
		private TextBlock label2 = new TextBlock();
		private TabItem tabPage3 = new TabItem();
		internal Panel realPanel = new Panel();
		private Panel panel7 = new Panel();
		private TextBlock label56 = new TextBlock();
		internal TextBox tblongterm = new TextBox();
		internal TextBox tbshortterm = new TextBox();
		private TextBlock label57 = new TextBlock();
		private TextBlock label58 = new TextBlock();
        private Button llrelcommit = new Button { Content = "Commit" };
		internal CheckBox cbmarried = new CheckBox { Content = "Married" };
		internal CheckBox cbengaged = new CheckBox { Content = "Engaged" };
		internal CheckBox cbsteady = new CheckBox { Content = "Steady" };
		internal CheckBox cblove = new CheckBox { Content = "Love" };
		internal CheckBox cbcrush = new CheckBox { Content = "Crush" };
		internal CheckBox cbenemy = new CheckBox { Content = "Enemy" };
		internal CheckBox cbbuddie = new CheckBox { Content = "Buddie" };
		internal CheckBox cbfriend = new CheckBox { Content = "Friend" };
		private TabItem tabPage4 = new TabItem();
		private TextBlock label64 = new TextBlock();
		private Panel panel8 = new Panel();
		private TextBlock label68 = new TextBlock();
		internal Panel familytiePanel = new Panel();
		private Button bttiecommit = new Button { Content = "Commit" };
		internal ComboBox cbtiesims = new ComboBox();
		internal ComboBox cbtietype = new ComboBox();
		internal Button btdeletetie = new Button { Content = "Delete" };
		internal Button btaddtie = new Button { Content = "Add" };
		internal ListBox lbties = new ListBox();
		internal ComboBox cballtieablesims = new ComboBox();
        private Button llcommitties = new Button { Content = "Commit" };
        internal Button btnewtie = new Button { Content = "New" };
		internal TextBox tblottype = new TextBox();
		private TextBlock label65 = new TextBlock();
        private Button llcommitobjd = new Button { Content = "Commit" };
        private Button llgetGUID = new Button { Content = "Get GUID" };
		internal Panel pnelements = new Panel();
        internal TextBlock lbtypename = new TextBlock();
		internal CheckBox cbfamily = new CheckBox { Content = "Family" };
		internal CheckBox cbbest = new CheckBox { Content = "Best" };
        private Button llsrelcommit = new Button { Content = "Commit" };
        internal ComboBox cbfamtype = new ComboBox();
        internal ComboBox cboutfamtype => cbfamtype;
        internal ComboBox cbinfamtype => cbfamtype;
		private TextBlock label91 = new TextBlock();
		internal TextBox tbflag = new TextBox();
		private TextBlock label92 = new TextBlock();
		internal TextBox tbalbum = new TextBox();
		private TextBlock label93 = new TextBlock();
		internal TextBox tborgguid = new TextBox();
		internal TextBox tbproxguid = new TextBox();
		private TextBlock label97 = new TextBlock();
		private TextBlock label63 = new TextBlock();
		private CheckBox cbphone = new CheckBox { Content = "Phone" };
		private CheckBox cbbaby = new CheckBox { Content = "Baby" };
		private CheckBox cbcomputer = new CheckBox { Content = "Computer" };
		private CheckBox cblot = new CheckBox { Content = "Lot" };
		private CheckBox cbupdate = new CheckBox { Content = "Update", IsChecked = true };
		internal TextBox tbsubhood = new TextBox();
        private TextBlock label89 = new TextBlock();
		private Button btPicExport = new Button { Content = "Export..." };
        internal TextBox tbvac = new TextBox();
        internal TextBlock label7 = new TextBlock();
        internal TextBox tbblot = new TextBox();
        internal TextBlock label14 = new TextBlock();
        internal TextBox tbbmoney = new TextBox();
        internal TextBlock label16 = new TextBlock();

        // GroupBox equivalents (Border wrappers — not used externally, just here for layout)
		private Border gbrelation = new Border();
		private Border gbelements = new Border();
		private Border groupBox4 = new Border();

        internal SimPe.Interfaces.Plugin.IFileWrapperSaveExtension wrapper = null;

		public Elements()
		{
			// Wire up events for the panels this class manages
			llFamiAddSim.Click += FamiSimAddClick;
			llFamiDeleteSim.Click += FamiDeleteSimClick;
			button1.Click += CommitFamiClick;
			cbsims.SelectionChanged += SimSelectionChange;
			lbmembers.SelectionChanged += FamiMemberSelectionClick;
			visualStyleLinkLabel2.Click += (s, e) => CommitXmlClick(s, e);
			llrelcommit.Click += (s, e) => RelationshipFileCommit(s, e);
			llcommitobjd.Click += (s, e) => CommitObjdClicked(s, e);
			llsrelcommit.Click += (s, e) => RelationshipFileCommit(s, e);
			bttiecommit.Click += CommitTieClick;
			llcommitties.Click += (s, e) => CommitSimTieClicked(s, e);
			btdeletetie.Click += DeleteTieClick;
			btaddtie.Click += AddTieClick;
			btnewtie.Click += AddSimToTiesClick;
			cbtiesims.SelectionChanged += FamilyTieSimIndexChanged;
			cballtieablesims.SelectionChanged += AllTieableSimsIndexChanged;
			lbties.SelectionChanged += TieIndexChanged;
			btPicExport.Click += btPicExport_Click;
			tbflag.TextChanged += FlagChanged;
			cbphone.IsCheckedChanged += ChangeFlags;
			cbcomputer.IsCheckedChanged += ChangeFlags;
			cbbaby.IsCheckedChanged += ChangeFlags;
			cblot.IsCheckedChanged += ChangeFlags;
		}

		public void Dispose() { }

		private void CommitFamiClick(object sender, Avalonia.Interactivity.RoutedEventArgs e)
		{
			if (wrapper != null)
			{
				try
				{
					SimPe.PackedFiles.Wrapper.Fami fami = (Wrapper.Fami)wrapper;
					fami.Money = Convert.ToInt32(tbmoney.Text);
					fami.Friends = Convert.ToUInt32(tbfamily.Text);
					fami.Flags = Convert.ToUInt32(tbflag.Text, 16);
					fami.AlbumGUID = Convert.ToUInt32(tbalbum.Text, 16);
					fami.SubHoodNumber = Convert.ToUInt32(tbsubhood.Text, 16);
                    fami.VacationLotInstance = Helper.StringToUInt32(tbvac.Text, fami.VacationLotInstance, 16);
                    fami.CurrentlyOnLotInstance = Helper.StringToUInt32(tbblot.Text, fami.CurrentlyOnLotInstance, 16);
                    fami.BusinessMoney = Helper.StringToInt32(this.tbbmoney.Text, fami.BusinessMoney, 10);

					uint[] members = new uint[lbmembers.ItemCount];
					for (int i = 0; i < members.Length; i++)
					{
						members[i] = ((SimPe.Interfaces.IAlias)lbmembers.Items[i]).Id;
						SimPe.PackedFiles.Wrapper.SDesc sdesc = fami.GetDescriptionFile(members[i]);
						sdesc.FamilyInstance = (ushort)fami.FileDescriptor.Instance;
						sdesc.SynchronizeUserData();
					}
					fami.Members = members;

					fami.LotInstance = Convert.ToUInt32(this.tblotinst.Text, 16);
					if (tbname.Text != fami.Name) fami.Name = tbname.Text;

					wrapper.SynchronizeUserData();
					SimPe.Message.Show(Localization.Manager.GetString("commited"), null, System.Windows.Forms.MessageBoxButtons.OK);
				}
				catch (Exception ex)
				{
					Helper.ExceptionMessage(Localization.Manager.GetString("cantcommitfamily"), ex);
				}
			}
		}

		private void CommitXmlClick(object sender, Avalonia.Interactivity.RoutedEventArgs e)
		{
			if (wrapper != null)
			{
				try
				{
					SimPe.PackedFiles.Wrapper.Xml xml = (Wrapper.Xml)wrapper;
					xml.Text = rtb.Text;
					wrapper.SynchronizeUserData();
					SimPe.Message.Show(Localization.Manager.GetString("commited"), null, System.Windows.Forms.MessageBoxButtons.OK);
				}
				catch (Exception) {}
			}
		}

		private void FamiSimAddClick(object sender, Avalonia.Interactivity.RoutedEventArgs e)
		{
			if (cbsims.SelectedIndex >= 0)
			{
				if (!this.lbmembers.Items.Contains(cbsims.Items[cbsims.SelectedIndex]))
					this.lbmembers.Items.Add(cbsims.Items[cbsims.SelectedIndex]);
			}
		}

		private void SimSelectionChange(object sender, Avalonia.Controls.SelectionChangedEventArgs e)
		{
			this.llFamiAddSim.IsEnabled = ((((ComboBox)sender).SelectedIndex >= 0) && (((ComboBox)sender).ItemCount > 0));
		}

		private void FamiMemberSelectionClick(object sender, Avalonia.Controls.SelectionChangedEventArgs e)
		{
			this.llFamiDeleteSim.IsEnabled = (((ListBox)sender).SelectedIndex >= 0);
		}

		private void FamiDeleteSimClick(object sender, Avalonia.Interactivity.RoutedEventArgs e)
		{
			if (lbmembers.SelectedIndex >= 0)
			{
				lbmembers.Items.Remove(lbmembers.Items[lbmembers.SelectedIndex]);
			}
		}

		#region FAMi ProgressBar Handling
		// ProgressBar logic is stubbed — Avalonia ProgressBar API differs significantly
		// and is not accessible from outside this class.
		internal void ProgressBarMaximize(Panel parent) { }
		internal void ProgressBarUpdate(Panel parent) { }
		#endregion

		#region Family Ties
		private void FamilyTieSimIndexChanged(object sender, Avalonia.Controls.SelectionChangedEventArgs e)
		{
			this.btdeletetie.IsEnabled = false;
			if (this.cbtiesims.SelectedIndex < 0) return;
			FamilyTieSim sim = (FamilyTieSim)cbtiesims.Items[cbtiesims.SelectedIndex];

			this.lbties.Items.Clear();
			foreach (FamilyTieItem tie in sim.Ties)
			{
				lbties.Items.Add(tie);
			}
		}

		private void AllTieableSimsIndexChanged(object sender, Avalonia.Controls.SelectionChangedEventArgs e)
		{
			this.btaddtie.IsEnabled = false;
			this.btnewtie.IsEnabled = false;
			if (this.cballtieablesims.SelectedIndex < 0) return;
			this.btnewtie.IsEnabled = true;
			if (this.cbtiesims.SelectedIndex < 0) return;
			this.btaddtie.IsEnabled = true;
		}

		private void DeleteTieClick(object sender, Avalonia.Interactivity.RoutedEventArgs e)
		{
			this.btaddtie.IsEnabled = false;
			if (this.lbties.SelectedIndex < 0) return;
			lbties.Items.Remove(lbties.Items[lbties.SelectedIndex]);
		}

		private void AddTieClick(object sender, Avalonia.Interactivity.RoutedEventArgs e)
		{
			if (this.cballtieablesims.SelectedIndex < 0) return;
			if (this.cbtietype.SelectedIndex < 0) return;

			try
			{
				SimPe.PackedFiles.Wrapper.FamilyTies famt = (Wrapper.FamilyTies)wrapper;
                LocalizedFamilyTieTypes selected = (LocalizedFamilyTieTypes)cbtietype.SelectedItem;
                MetaData.FamilyTieTypes ftt = selected;
                FamilyTieSim fts = (FamilyTieSim)this.cballtieablesims.Items[cballtieablesims.SelectedIndex];
				FamilyTieItem tie = new FamilyTieItem(ftt, fts.Instance, famt);
				this.lbties.Items.Add(tie);
			}
			catch (Exception ex)
			{
				Helper.ExceptionMessage(Localization.Manager.GetString("cantaddtie"), ex);
			}
		}

		private void CommitSimTieClicked(object sender, Avalonia.Interactivity.RoutedEventArgs e)
		{
			if (this.cbtiesims.SelectedIndex < 0) return;

			if (wrapper != null)
			{
				try
				{
					SimPe.PackedFiles.Wrapper.FamilyTies famt = (Wrapper.FamilyTies)wrapper;

					FamilyTieSim fts = (FamilyTieSim)cbtiesims.Items[cbtiesims.SelectedIndex];
					FamilyTieItem[] ftis = new FamilyTieItem[lbties.ItemCount];
					for (int i = 0; i < lbties.ItemCount; i++)
					{
						ftis[i] = (FamilyTieItem)lbties.Items[i];
					}
					fts.Ties = ftis;
				}
				catch (Exception ex)
				{
					Helper.ExceptionMessage(Localization.Manager.GetString("cantcommitfamt"), ex);
				}
			}
		}

		private void TieIndexChanged(object sender, Avalonia.Controls.SelectionChangedEventArgs e)
		{
			this.btdeletetie.IsEnabled = false;
			if (this.lbties.SelectedIndex < 0) return;
			this.btdeletetie.IsEnabled = true;
		}

		private void CommitTieClick(object sender, Avalonia.Interactivity.RoutedEventArgs e)
		{
			CommitSimTieClicked(null, null);
			if (wrapper != null)
			{
				try
				{
					SimPe.PackedFiles.Wrapper.FamilyTies famt = (Wrapper.FamilyTies)wrapper;

					FamilyTieSim[] sims = new FamilyTieSim[cbtiesims.ItemCount];
					for (int i = 0; i < sims.Length; i++)
					{
						sims[i] = (FamilyTieSim)cbtiesims.Items[i];
					}
					famt.Sims = sims;

					famt.SynchronizeUserData();
				}
				catch (Exception ex)
				{
					Helper.ExceptionMessage(Localization.Manager.GetString("cantcommittie"), ex);
				}
			}
		}

		private void AddSimToTiesClick(object sender, Avalonia.Interactivity.RoutedEventArgs e)
		{
			if (this.cballtieablesims.SelectedIndex < 0) return;
			FamilyTieSim sim = (FamilyTieSim)this.cballtieablesims.Items[cballtieablesims.SelectedIndex];
			sim.Ties = new FamilyTieItem[0];

			bool exists = false;
			foreach (FamilyTieSim exsim in cbtiesims.Items)
			{
				if (exsim.Instance == sim.Instance)
				{
					exists = true;
					break;
				}
			}

			if (!exists)
			{
				cbtiesims.Items.Add(sim);
			}
		}
		#endregion

		#region Relationships

		private void RelationshipFileCommit(object sender, Avalonia.Interactivity.RoutedEventArgs e)
		{
			if (wrapper != null)
			{
				try
				{
					SimPe.PackedFiles.Wrapper.SRel srel = (Wrapper.SRel)wrapper;
					srel.Shortterm = Convert.ToInt32(tbshortterm.Text);
					srel.Longterm = Convert.ToInt32(tblongterm.Text);

                    List<CheckBox> ltcb = new List<CheckBox>(new CheckBox[] {
                        cbcrush, cblove, cbengaged, cbmarried, cbfriend, cbbuddie, cbsteady, cbenemy,
                    });
                    Boolset bs1 = srel.RelationState.Value;
                    for (int i = 0; i < ltcb.Count; i++)
                        if (ltcb[i] != null) bs1[i] = ltcb[i].IsChecked == true;
                    srel.RelationState.Value = bs1;

					if (cbfamtype.SelectedIndex > 0)
						srel.FamilyRelation = (Data.LocalizedRelationshipTypes)cbfamtype.Items[cbfamtype.SelectedIndex];

					wrapper.SynchronizeUserData();
					SimPe.Message.Show(Localization.Manager.GetString("commited"), null, System.Windows.Forms.MessageBoxButtons.OK);
				}
				catch (Exception ex)
				{
					Helper.ExceptionMessage("Unable to Save Relationship Information!", ex);
				}
			}
		}
		#endregion

		private void CommitObjdClicked(object sender, Avalonia.Interactivity.RoutedEventArgs e)
		{
			if (wrapper != null)
			{
				try
				{
					SimPe.PackedFiles.Wrapper.Objd objd = (Wrapper.Objd)wrapper;

					foreach (Control c in pnelements.Children)
					{
						if (c is TextBox tb && tb.Tag != null)
						{
							string name = (string)tb.Tag;
							Wrapper.ObjdItem item = (Wrapper.ObjdItem)objd.Attributes[name];
							item.val = Convert.ToUInt16(tb.Text, 16);
							objd.Attributes[name] = item;
						}
					}

					objd.Type = (ushort)Helper.HexStringToUInt(tblottype.Text);
					objd.Guid = (uint)Helper.HexStringToUInt(tbsimid.Text);
					objd.FileName = tbsimname.Text;
					objd.OriginalGuid = (uint)Helper.HexStringToUInt(this.tborgguid.Text);
					objd.ProxyGuid = (uint)Helper.HexStringToUInt(this.tbproxguid.Text);

					objd.SynchronizeUserData();
					SimPe.Message.Show(Localization.Manager.GetString("commited"), null, System.Windows.Forms.MessageBoxButtons.OK);
				}
				catch (Exception ex)
				{
					Helper.ExceptionMessage(Localization.Manager.GetString("cantcommitobjd"), ex);
				}
			}
		}

		internal bool simnamechanged;
		private void SimNameChanged(object sender, Avalonia.Interactivity.RoutedEventArgs e)
		{
			simnamechanged = true;
		}

		private void FlagChanged(object sender, Avalonia.Controls.TextChangedEventArgs e)
		{
			if (tbflag.Tag != null) return;
			tbflag.Tag = true;
			try
			{
				uint flag = Convert.ToUInt32(tbflag.Text, 16);
				SimPe.PackedFiles.Wrapper.FamiFlags flags = new SimPe.PackedFiles.Wrapper.FamiFlags((ushort)flag);

				this.cbphone.IsChecked = flags.HasPhone;
				this.cbcomputer.IsChecked = flags.HasComputer;
				this.cbbaby.IsChecked = flags.HasBaby;
				this.cblot.IsChecked = flags.NewLot;
			}
			catch (Exception) {}
			finally
			{
				tbflag.Tag = null;
			}
		}

		private void ChangeFlags(object sender, Avalonia.Interactivity.RoutedEventArgs e)
		{
			if (tbflag.Tag != null) return;
			tbflag.Tag = true;
			try
			{
				uint flag = Convert.ToUInt32(tbflag.Text, 16) & 0xffff0000;

				SimPe.PackedFiles.Wrapper.FamiFlags flags = new SimPe.PackedFiles.Wrapper.FamiFlags(0);

				flags.HasPhone = this.cbphone.IsChecked == true;
				flags.HasComputer = this.cbcomputer.IsChecked == true;
				flags.HasBaby = this.cbbaby.IsChecked == true;
				flags.NewLot = this.cblot.IsChecked == true;

				flag = flag | flags.Value;
				tbflag.Text = "0x" + Helper.HexString(flag);
			}
			catch (Exception) {}
			finally
			{
				tbflag.Tag = null;
			}
		}

		internal SimPe.Interfaces.Plugin.IFileWrapper picwrapper;
		private async void btPicExport_Click(object sender, Avalonia.Interactivity.RoutedEventArgs e)
		{
			SimPe.PackedFiles.Wrapper.Picture wrp = (SimPe.PackedFiles.Wrapper.Picture)picwrapper;
			var topLevel = Avalonia.Controls.TopLevel.GetTopLevel(this);
			if (topLevel == null) return;
			var file = await topLevel.StorageProvider.SaveFilePickerAsync(new Avalonia.Platform.Storage.FilePickerSaveOptions
			{
				Title = "Export Image",
				SuggestedFileName = "image.png",
				FileTypeChoices = new[] { new Avalonia.Platform.Storage.FilePickerFileType("PNG Image") { Patterns = new[] { "*.png" } } }
			});
			if (file != null)
			{
				try
				{
					string path = file.Path.LocalPath;
					wrp.Image.Save(path, System.Drawing.Imaging.ImageFormat.Png);
				}
				catch (Exception ex)
				{
					Helper.ExceptionMessage(ex);
				}
			}
		}

		private void cboutfamtype_SelectedIndexChanged(object sender, Avalonia.Controls.SelectionChangedEventArgs e) { }
	}
}
