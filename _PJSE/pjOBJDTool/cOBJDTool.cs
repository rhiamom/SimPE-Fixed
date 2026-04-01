/***************************************************************************
 *   Copyright (C) 2008 by Peter L Jones                                   *
 *   peter@users.sf.net                                                    *
 *                                                                         *
 *   Copyright (C) 2025 by GramzeSweatShop                                 *
 *   Rhiamom@mac.com                                                       *
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
using SimPe.Interfaces;
using SimPe.Interfaces.Plugin;
using SimPe.Scenegraph.Compat;
using pjse.BhavOperandWizards;
using MessageBoxButtons = SimPe.Scenegraph.Compat.MessageBoxButtons;
using MessageBoxIcon = SimPe.Scenegraph.Compat.MessageBoxIcon;

namespace pjOBJDTool
{
    public class cOBJDTool : Avalonia.Controls.Window
    {
        #region Fields (formerly in Designer / Stubs)

        private Avalonia.Controls.Label label1 = new Avalonia.Controls.Label();
        private Avalonia.Controls.TextBox tbOBJDName = new Avalonia.Controls.TextBox();
        private Avalonia.Controls.Label label2 = new Avalonia.Controls.Label();
        private Avalonia.Controls.TextBox tbOBJDGroup = new Avalonia.Controls.TextBox();
        private Avalonia.Controls.Label label3 = new Avalonia.Controls.Label();
        private Avalonia.Controls.TextBox tbOBJDInstance = new Avalonia.Controls.TextBox();
        private Avalonia.Controls.Label label4 = new Avalonia.Controls.Label();
        private Avalonia.Controls.Label label5 = new Avalonia.Controls.Label();
        private Avalonia.Controls.Label label6 = new Avalonia.Controls.Label();
        private Avalonia.Controls.Label label21 = new Avalonia.Controls.Label();
        private TextBoxCompat tbCTSSInstance = new TextBoxCompat();
        private Avalonia.Controls.TabControl tabControl1 = new Avalonia.Controls.TabControl();
        private Avalonia.Controls.TabItem tabPage1 = new Avalonia.Controls.TabItem();
        private Avalonia.Controls.WrapPanel flpTabPage1 = new Avalonia.Controls.WrapPanel();
        private GroupBox gbValue = new GroupBox();
        private Avalonia.Controls.Grid tlpValue = new Avalonia.Controls.Grid();
        private Avalonia.Controls.Label label7 = new Avalonia.Controls.Label();
        private Avalonia.Controls.Label label8 = new Avalonia.Controls.Label();
        private Avalonia.Controls.Label label9 = new Avalonia.Controls.Label();
        private Avalonia.Controls.Label label10 = new Avalonia.Controls.Label();
        private Avalonia.Controls.Label label11 = new Avalonia.Controls.Label();
        private Avalonia.Controls.Label label12 = new Avalonia.Controls.Label();
        private TextBoxCompat tbPrice = new TextBoxCompat();
        private TextBoxCompat tbSalePrice = new TextBoxCompat();
        private TextBoxCompat tbInitialDep = new TextBoxCompat();
        private TextBoxCompat tbDailyDep = new TextBoxCompat();
        private TextBoxCompat tbDepLimit = new TextBoxCompat();
        private Avalonia.Controls.CheckBox ckbSelfDep = new Avalonia.Controls.CheckBox();
        private GroupBox gbMotiveRs = new GroupBox();
        private Avalonia.Controls.Grid tlpMotiveRs = new Avalonia.Controls.Grid();
        private Avalonia.Controls.Label label15 = new Avalonia.Controls.Label();
        private TextBoxCompat tbMotive1 = new TextBoxCompat();
        private TextBoxCompat tbMotive2 = new TextBoxCompat();
        private TextBoxCompat tbMotive3 = new TextBoxCompat();
        private TextBoxCompat tbMotive4 = new TextBoxCompat();
        private TextBoxCompat tbMotive5 = new TextBoxCompat();
        private TextBoxCompat tbMotive6 = new TextBoxCompat();
        private Avalonia.Controls.Label label16 = new Avalonia.Controls.Label();
        private Avalonia.Controls.Label label17 = new Avalonia.Controls.Label();
        private Avalonia.Controls.Label label18 = new Avalonia.Controls.Label();
        private Avalonia.Controls.Label label19 = new Avalonia.Controls.Label();
        private Avalonia.Controls.Label label20 = new Avalonia.Controls.Label();
        private Avalonia.Controls.Label label13 = new Avalonia.Controls.Label();
        private Avalonia.Controls.Label label14 = new Avalonia.Controls.Label();
        private TextBoxCompat tbMotive7 = new TextBoxCompat();
        private TextBoxCompat tbMotive8 = new TextBoxCompat();
        private Avalonia.Controls.Label label22 = new Avalonia.Controls.Label();
        private Avalonia.Controls.Label label23 = new Avalonia.Controls.Label();
        private TextBoxCompat tbMotive9 = new TextBoxCompat();
        private TextBoxCompat tbMotiveA = new TextBoxCompat();
        private GroupBox gbValidEPs1 = new GroupBox();
        private LabelledBoolsetControl lbcValidEPs1 = new LabelledBoolsetControl();
        private GroupBox gbValidEPs2 = new GroupBox();
        private LabelledBoolsetControl lbcValidEPs2 = new LabelledBoolsetControl();
        private Avalonia.Controls.TabItem tabPage2 = new Avalonia.Controls.TabItem();
        private Avalonia.Controls.WrapPanel flpTabPage2 = new Avalonia.Controls.WrapPanel();
        private GroupBox gbRoomSort = new GroupBox();
        private LabelledBoolsetControl lbcRoom = new LabelledBoolsetControl();
        private GroupBox gbFuncSort = new GroupBox();
        private LabelledBoolsetControl lbcFunction = new LabelledBoolsetControl();
        private Avalonia.Controls.ComboBox cbFunction = new Avalonia.Controls.ComboBox();
        private GroupBox gbBuildSort = new GroupBox();
        private LabelledBoolsetControl lbcBuild = new LabelledBoolsetControl();
        private Avalonia.Controls.ComboBox cbBuild = new Avalonia.Controls.ComboBox();
        private GroupBox gbCommSort = new GroupBox();
        private LabelledBoolsetControl lbcCommunity = new LabelledBoolsetControl();
        private Avalonia.Controls.Grid tlpOBJDCTSS = new Avalonia.Controls.Grid();
        private Avalonia.Controls.TextBox tbCTSSName = new Avalonia.Controls.TextBox();
        private Avalonia.Controls.TextBox tbCTSSDesc = new Avalonia.Controls.TextBox();
        private Avalonia.Controls.Button btnCommit = new Avalonia.Controls.Button();
        private Avalonia.Controls.Button btnSelectOBJD = new Avalonia.Controls.Button();
        private Avalonia.Controls.ComboBox cbOBJDvsn = new Avalonia.Controls.ComboBox();

        #endregion

        bool initialised = false;

        public cOBJDTool()
        {
            this.Title = "PJSE OBJD Tool";
            this.Width = 700;
            this.Height = 500;

            // Wire up buttons
            btnCommit.Content = "Commit";
            btnCommit.Click += (s, e) => btnCommit_Click(s, e);
            btnSelectOBJD.Content = "Select OBJD";
            btnSelectOBJD.Click += (s, e) => btnSelectOBJD_Click(s, e);

            // cbOBJDvsn items from original resx
            cbOBJDvsn.Items.Add("0x008b");
            cbOBJDvsn.Items.Add("0x008c");
            cbOBJDvsn.Items.Add("0x008d");
            cbOBJDvsn.SelectionChanged += (s, e) => cbOBJDvsn_SelectedIndexChanged(s, EventArgs.Empty);

            // Build layout
            BuildLayout();

            this.Closing += (s, e) =>
            {
                CurrentOBJD = null;
                e.Cancel = CurrentOBJD != null;
            };

            initialised = false;
        }

        private void BuildLayout()
        {
            // Header row: OBJD name/group/instance + CTSS
            label1.Content = "OBJD Name:";
            label2.Content = "Group:";
            label3.Content = "Instance:";
            label21.Content = "CTSS Instance:";
            tbOBJDName.IsReadOnly = true;
            tbOBJDGroup.IsReadOnly = true;
            tbOBJDInstance.IsReadOnly = true;

            var headerRow = new Avalonia.Controls.StackPanel { Orientation = Avalonia.Layout.Orientation.Horizontal, Margin = new Avalonia.Thickness(4, 4, 4, 2) };
            headerRow.Children.Add(label1); headerRow.Children.Add(tbOBJDName);
            headerRow.Children.Add(label2); headerRow.Children.Add(tbOBJDGroup);
            headerRow.Children.Add(label3); headerRow.Children.Add(tbOBJDInstance);
            headerRow.Children.Add(label21); headerRow.Children.Add(tbCTSSInstance);
            headerRow.Children.Add(label4); // Version label
            headerRow.Children.Add(cbOBJDvsn);
            headerRow.Children.Add(btnSelectOBJD);
            headerRow.Children.Add(btnCommit);

            // Tab 1: Value + Motives + EP flags
            label7.Content = "Price:"; label8.Content = "Sale price:"; label9.Content = "Initial dep:";
            label10.Content = "Daily dep:"; label11.Content = "Dep limit:"; label12.Content = "Self dep:";
            ckbSelfDep.Content = "Self Depreciation";

            gbValue.Text = "Value";
            var valuePanel = new Avalonia.Controls.StackPanel();
            void addValueRow(Avalonia.Controls.Label lbl, Avalonia.Controls.Control ctrl) {
                var row = new Avalonia.Controls.StackPanel { Orientation = Avalonia.Layout.Orientation.Horizontal };
                row.Children.Add(lbl); row.Children.Add(ctrl);
                valuePanel.Children.Add(row);
            }
            addValueRow(label7, tbPrice); addValueRow(label8, tbSalePrice);
            addValueRow(label9, tbInitialDep); addValueRow(label10, tbDailyDep);
            addValueRow(label11, tbDepLimit);
            valuePanel.Children.Add(ckbSelfDep);
            gbValue.Content = valuePanel;

            label15.Content = "Motive ratings:";
            gbMotiveRs.Text = "Motive Ratings";
            var motivePanel = new Avalonia.Controls.WrapPanel();
            foreach (var tb in new TextBoxCompat[] { tbMotive1, tbMotive2, tbMotive3, tbMotive4, tbMotive5, tbMotive6, tbMotive7, tbMotive8, tbMotive9, tbMotiveA })
                motivePanel.Children.Add(tb);
            gbMotiveRs.Content = motivePanel;

            gbValidEPs1.Text = "Valid EPs 1";
            gbValidEPs1.Content = lbcValidEPs1;
            gbValidEPs2.Text = "Valid EPs 2";
            gbValidEPs2.Content = lbcValidEPs2;

            flpTabPage1.Children.Add(gbValue);
            flpTabPage1.Children.Add(gbMotiveRs);
            flpTabPage1.Children.Add(gbValidEPs1);
            flpTabPage1.Children.Add(gbValidEPs2);
            tabPage1.Header = "Object Info";
            tabPage1.Content = flpTabPage1;

            // Tab 2: Sorting
            gbRoomSort.Text = "Room Sort"; gbRoomSort.Content = lbcRoom;
            gbCommSort.Text = "Community Sort"; gbCommSort.Content = lbcCommunity;
            gbFuncSort.Text = "Function Sort";
            var funcPanel = new Avalonia.Controls.StackPanel();
            funcPanel.Children.Add(cbFunction); funcPanel.Children.Add(lbcFunction);
            gbFuncSort.Content = funcPanel;
            gbBuildSort.Text = "Build Sort";
            var buildPanel = new Avalonia.Controls.StackPanel();
            buildPanel.Children.Add(cbBuild); buildPanel.Children.Add(lbcBuild);
            gbBuildSort.Content = buildPanel;

            flpTabPage2.Children.Add(gbRoomSort);
            flpTabPage2.Children.Add(gbFuncSort);
            flpTabPage2.Children.Add(gbBuildSort);
            flpTabPage2.Children.Add(gbCommSort);
            tabPage2.Header = "Sorting";
            tabPage2.Content = flpTabPage2;

            tabControl1.Items.Add(tabPage1);
            tabControl1.Items.Add(tabPage2);

            // CTSS name/desc
            label5.Content = "CTSS Name:"; label6.Content = "CTSS Desc:";
            var ctssRow = new Avalonia.Controls.StackPanel { Orientation = Avalonia.Layout.Orientation.Horizontal, Margin = new Avalonia.Thickness(4, 2, 4, 2) };
            ctssRow.Children.Add(label5); ctssRow.Children.Add(tbCTSSName);
            ctssRow.Children.Add(label6); ctssRow.Children.Add(tbCTSSDesc);

            var mainPanel = new Avalonia.Controls.StackPanel();
            mainPanel.Children.Add(headerRow);
            mainPanel.Children.Add(ctssRow);
            mainPanel.Children.Add(tabControl1);

            this.Content = mainPanel;
        }

        private void InitializeForm()
        {
            initialised = true;

            SimPe.Wait.Start(6);

            docCTSSInstance = new DataOwnerControl(null, null, null, tbCTSSInstance, null, null, null, 7, (ushort)0);
            docCTSSInstance.Decimal = false;
            docCTSSInstance.Use0xPrefix = true;
            docCTSSInstance.DataOwnerControlChanged += new EventHandler(docCTSSInstance_DataOwnerControlChanged);
            SimPe.Wait.Progress++;

            InitializeValueMotive();
            SimPe.Wait.Progress++;
            InitializeValue();
            SimPe.Wait.Progress++;
            InitializeEPs();
            SimPe.Wait.Progress++;
            InitializeRoomComm();
            SimPe.Wait.Progress++;
            InitializeFuncBuild();
            SimPe.Wait.Progress++;

            SimPe.Wait.Stop();
        }

        DataOwnerControl docCTSSInstance = null;
        void docCTSSInstance_DataOwnerControlChanged(object sender, EventArgs e)
        {
            if (wrapper == null) return;

            pjse.FileTable.Entry[] actss = pjse.FileTable.GFT[SimPe.Data.MetaData.CTSS_FILE,
                wrapper.FileDescriptor.Group, docCTSSInstance.Value];
            wrapper[0x29] = docCTSSInstance.Value;

            SimPe.PackedFiles.Wrapper.StrWrapper ctss = new SimPe.PackedFiles.Wrapper.StrWrapper();
            if (actss.Length > 0)
            {
                ctss.ProcessData(actss[0].PFD, actss[0].Package);
                tbCTSSName.Text = ((SimPe.PackedFiles.Wrapper.StrItem)ctss[1, 0]).Title;
                tbCTSSDesc.Text = ((SimPe.PackedFiles.Wrapper.StrItem)ctss[1, 1]).Title;
            }
            else
                tbCTSSName.Text = tbCTSSDesc.Text = "";
        }

        #region ValueMotive shorts
        List<DataOwnerControl> adocValueMotive = null;
        short[] asValueMotive = new short[] {
            0x12, 0x22, 0x23, 0x24, 0x26,
            0x52, 0x53, 0x54, 0x55, 0x56, 0x57, 0x58, 0x59,
            0x67, 0x68
        };
        void InitializeValueMotive()
        {
            atbValueMotive = new TextBoxCompat[] {
                tbPrice, tbSalePrice, tbInitialDep, tbDailyDep, tbDepLimit,
                tbMotive1, tbMotive2, tbMotive3, tbMotive4, tbMotive5, tbMotive6, tbMotive7, tbMotive8,
                tbMotive9, tbMotiveA,
            };
            adocValueMotive = new List<DataOwnerControl>();
            foreach (TextBoxCompat tb in atbValueMotive)
                adocValueMotive.Add(new DataOwnerControl(null, null, null, tb, null, null, null, 7, (ushort)0));
            foreach (DataOwnerControl doc in adocValueMotive)
            {
                doc.Decimal = true;
                doc.DataOwnerControlChanged += new EventHandler(adocValueMotive_DataOwnerControlChanged);
            }
        }
        TextBoxCompat[] atbValueMotive = null;
        void adocValueMotive_DataOwnerControlChanged(object sender, EventArgs e)
        {
            if (wrapper == null) return;

            int i = adocValueMotive.IndexOf((DataOwnerControl)sender);
            if (i < 0) return;
            wrapper[asValueMotive[i]] = adocValueMotive[i].Value;
        }
        #endregion

        #region Value bool
        List<Avalonia.Controls.CheckBox> ackbValue = null;
        short[] abValue = new short[] { 0x25, };
        void InitializeValue()
        {
            ackbValue = new List<Avalonia.Controls.CheckBox>(new Avalonia.Controls.CheckBox[] { ckbSelfDep, });
            foreach (Avalonia.Controls.CheckBox ckb in ackbValue)
                ckb.IsCheckedChanged += (s, e) => ackbValue_CheckedChanged(s, e);
        }
        void ackbValue_CheckedChanged(object sender, EventArgs e)
        {
            if (wrapper == null) return;

            int i = ackbValue.IndexOf((Avalonia.Controls.CheckBox)sender);
            if (i < 0) return;
            wrapper[abValue[i]] = (ushort)(ackbValue[i].IsChecked == true ? 1 : 0);
        }
        #endregion

        #region EPs flags
        List<LabelledBoolsetControl> albcValidEPs = null;
        pjse.GS.BhavStr[] abhsEPs = new pjse.GS.BhavStr[]
        {
            pjse.GS.BhavStr.ValidEPFlags1,
            pjse.GS.BhavStr.ValidEPFlags2,
        };
        short[] absEPs = new short[] { 0x40, 0x41, };

        void InitializeEPs()
        {
            albcValidEPs = new List<LabelledBoolsetControl>(new LabelledBoolsetControl[] {
                lbcValidEPs1, lbcValidEPs2,
            });
            foreach (LabelledBoolsetControl lbc in albcValidEPs)
                lbc.ValueChanged += new EventHandler(rb_CheckedChanged);

            List<string> l = new List<string>();
            for (int i = 0; i < abhsEPs.Length; i++)
            {
                l = pjse.BhavWiz.readStr(abhsEPs[i]);
                while (l.Count < 16)
                    l.Add("-");
                albcValidEPs[i].Labels = l;
            }
        }

        void rb_CheckedChanged(object sender, EventArgs e)
        {
            if (wrapper == null) return;

            int i = albcValidEPs.IndexOf((LabelledBoolsetControl)sender);
            if (i < 0) return;
            wrapper[absEPs[i]] = albcValidEPs[i].Value;
        }
        #endregion

        #region RoomComm boolsets
        List<LabelledBoolsetControl> albcRoomCommm = null;
        pjse.GS.BhavStr[] abhsRoomComm = new pjse.GS.BhavStr[]
        {
            pjse.GS.BhavStr.RoomSortFlags,
            pjse.GS.BhavStr.CommunitySortFlags,
        };
        short[] absRoomComm = new short[] { 0x27, 0x64, };
        void InitializeRoomComm()
        {
            albcRoomCommm = new List<LabelledBoolsetControl>(new LabelledBoolsetControl[] {
                lbcRoom, lbcCommunity,
            });
            foreach (LabelledBoolsetControl lbc in albcRoomCommm)
                lbc.ValueChanged += new EventHandler(albcRoomCommm_ValueChanged);

            List<string> l = new List<string>();
            for (int i = 0; i < abhsRoomComm.Length; i++)
            {
                l = pjse.BhavWiz.readStr(abhsRoomComm[i]);
                while (l.Count < 16)
                    l.Add("-");
                albcRoomCommm[i].Labels = l;
            }
        }
        void albcRoomCommm_ValueChanged(object sender, EventArgs e)
        {
            if (wrapper == null) return;

            int i = albcRoomCommm.IndexOf((LabelledBoolsetControl)sender);
            if (i < 0) return;
            wrapper[absRoomComm[i]] = albcRoomCommm[i].Value;
        }
        #endregion

        #region FuncBuild Flags
        List<Avalonia.Controls.ComboBox> acbFuncBuild = null;
        pjse.GS.BhavStr[] abhsFuncBuild = new pjse.GS.BhavStr[] { pjse.GS.BhavStr.FunctionSortFlags, pjse.GS.BhavStr.BuildModeType, };
        short[] afFuncBuild = new short[] { 0x28, 0x45, };

        List<LabelledBoolsetControl> albcFuncBuild = null;
        short[] absFuncBuild = new short[] { 0x5e, 0x4a, };

        void InitializeFuncBuild()
        {
            acbFuncBuild = new List<Avalonia.Controls.ComboBox>(new Avalonia.Controls.ComboBox[] { cbFunction, cbBuild, });
            for (int i = 0; i < acbFuncBuild.Count; i++)
            {
                acbFuncBuild[i].Items.Add("None");
                foreach (string label in pjse.BhavWiz.readStr(abhsFuncBuild[i]))
                    acbFuncBuild[i].Items.Add(acbFuncBuild[i].Items.Count.ToString() + ". " + label);
                while (acbFuncBuild[i].Items.Count < 16)
                    acbFuncBuild[i].Items.Add(acbFuncBuild[i].Items.Count.ToString() + ".");
                acbFuncBuild[i].SelectionChanged += (s, e) => acbFuncBuild_SelectedIndexChanged(s, EventArgs.Empty);
            }

            albcFuncBuild = new List<LabelledBoolsetControl>(new LabelledBoolsetControl[] { lbcFunction, lbcBuild, });
            for (int i = 0; i < albcFuncBuild.Count; i++)
                albcFuncBuild[i].ValueChanged += new EventHandler(albcFuncBuild_ValueChanged);
        }
        void acbFuncBuild_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (wrapper == null) return;

            int i = acbFuncBuild.IndexOf((Avalonia.Controls.ComboBox)sender);
            if (i < 0 || acbFuncBuild[i].SelectedIndex < 0) return;

            List<string> l = new List<string>();
            if (acbFuncBuild[i].SelectedIndex == 0)
            {
                wrapper[afFuncBuild[i]] = 0;
                while (l.Count < 16)
                    l.Add("-");
                albcFuncBuild[i].Labels = l;
                return;
            }

            int j = acbFuncBuild[i].SelectedIndex - 1;
            wrapper[afFuncBuild[i]] = (ushort)(1 << j);
            l = pjse.BhavWiz.readStr((pjse.GS.BhavStr)(0x110 + 16 * i + j));
            while (l.Count < 16)
                l.Add("-");
            albcFuncBuild[i].Labels = l;
        }
        void albcFuncBuild_ValueChanged(object sender, EventArgs e)
        {
            if (wrapper == null) return;

            int i = albcFuncBuild.IndexOf((LabelledBoolsetControl)sender);
            if (i < 0) return;
            wrapper[absFuncBuild[i]] = albcFuncBuild[i].Value;
        }
        #endregion


        private bool changed = false;
        SimPe.Interfaces.Files.IPackedFileDescriptor pfd;
        private pfOBJD wrapper = null;

        pfOBJD CurrentOBJD
        {
            get { return wrapper; }
            set
            {
                if (wrapper == value) return;
                if (!SaveAbandonCancel()) return;

                if (wrapper != null)
                    wrapper.WrapperChanged -= new System.EventHandler(this.WrapperChanged);
                wrapper = value;

                LoadOBJD();
            }
        }

        private bool SaveAbandonCancel()
        {
            if (wrapper == null || !wrapper.Changed) return true;
            SimPe.Scenegraph.Compat.DialogResult dr = SimPe.Scenegraph.Compat.MessageBox.ShowAsync(
                "You have uncommitted changes to the Object Definition you are editing. Do you want to save these changes?",
                "PJ OBJD Tool", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question).GetAwaiter().GetResult();
            switch (dr)
            {
                case SimPe.Scenegraph.Compat.DialogResult.Yes: SaveOBJD(); return true;
                case SimPe.Scenegraph.Compat.DialogResult.No: wrapper.Changed = false; return true;
                default: return false;
            }
        }

        private void LoadOBJD()
        {
            if (wrapper == null)
            {
                tbOBJDName.Text = tbOBJDGroup.Text = tbOBJDInstance.Text =
                    tbCTSSInstance.Text = tbCTSSName.Text = tbCTSSDesc.Text = "";
                cbOBJDvsn.SelectedIndex = -1;
                for (int i = 0; i < adocValueMotive.Count; i++) atbValueMotive[i].Text = "";
                for (int i = 0; i < ackbValue.Count; i++) ackbValue[i].IsChecked = null;
                for (int i = 0; i < albcRoomCommm.Count; i++) albcRoomCommm[i].Value = 0;
                for (int i = 0; i < acbFuncBuild.Count; i++) acbFuncBuild[i].SelectedIndex = -1;
                for (int i = 0; i < albcFuncBuild.Count; i++) albcFuncBuild[i].Value = 0;
                cbOBJDvsn.IsEnabled = tbCTSSInstance.IsEnabled =
                    gbValue.IsEnabled = gbMotiveRs.IsEnabled =
                    gbValidEPs1.IsEnabled = gbValidEPs2.IsEnabled =
                    gbRoomSort.IsEnabled = gbCommSort.IsEnabled = gbFuncSort.IsEnabled = gbBuildSort.IsEnabled =
                        false;
            }
            else
            {
                wrapper.WrapperChanged += new System.EventHandler(this.WrapperChanged);
                cbOBJDvsn.IsEnabled = tbCTSSInstance.IsEnabled =
                    gbValue.IsEnabled = gbMotiveRs.IsEnabled =
                    gbValidEPs1.IsEnabled = gbValidEPs2.IsEnabled =
                    gbRoomSort.IsEnabled = gbCommSort.IsEnabled = gbFuncSort.IsEnabled = gbBuildSort.IsEnabled =
                        true;

                tbOBJDName.Text = wrapper.Filename;
                tbOBJDGroup.Text = "0x" + SimPe.Helper.HexString(wrapper.FileDescriptor.Group);
                tbOBJDInstance.Text = "0x" + SimPe.Helper.HexString(wrapper.FileDescriptor.Instance);

                cbOBJDvsn.SelectedIndex = wrapper[0x00] <= 0x8b ? 0 : wrapper[0x00] >= 0x8d ? 2 : 1;
                cbOBJDvsn_SelectedIndexChanged(null, null);

                tbCTSSInstance.Text = "0x" + SimPe.Helper.HexString(wrapper[0x29]);
                docCTSSInstance_DataOwnerControlChanged(null, null);

                for (int i = 0; i < adocValueMotive.Count; i++)
                    atbValueMotive[i].Text = wrapper[asValueMotive[i]].ToString();

                for (int i = 0; i < ackbValue.Count; i++)
                    ackbValue[i].IsChecked = wrapper[abValue[i]] != 0;

                for (int i = 0; i < albcRoomCommm.Count; i++)
                    albcRoomCommm[i].Value = (ushort)wrapper[absRoomComm[i]];

                for (int i = 0; i < albcValidEPs.Count; i++)
                    albcValidEPs[i].Value = (ushort)wrapper[absEPs[i]];

                for (int i = 0; i < acbFuncBuild.Count; i++)
                {
                    ushort j = wrapper[afFuncBuild[i]];
                    int k = ((String)(Boolset)j).IndexOf("1");
                    acbFuncBuild[i].SelectedIndex = k < 0 ? 0 : 16 - k;
                }
                for (int i = 0; i < albcFuncBuild.Count; i++)
                    albcFuncBuild[i].Value = (ushort)wrapper[absFuncBuild[i]];
            }
        }

        private void SaveOBJD()
        {
            if (wrapper == null) return;
            changed |= pfd != null && wrapper.FileDescriptor != null && pfd == wrapper.FileDescriptor && wrapper.Changed;

            wrapper.SynchronizeUserData();
            btnCommit.IsEnabled = wrapper.Changed;
        }

        private void WrapperChanged(object sender, System.EventArgs e)
        {
            this.btnCommit.IsEnabled = wrapper.Changed;
        }

        private List<pfOBJD> availableOBJDs = null;
        private List<pfOBJD> AvailableOBJDs
        {
            get
            {
                if (availableOBJDs != null) return availableOBJDs;

                List<pfOBJD> lpfo = new List<pfOBJD>();
                pjse.FileTable.Entry[] items = pjse.FileTable.GFT[SimPe.Data.MetaData.OBJD_FILE, pjse.FileTable.Source.Local];
                SimPe.Wait.Start(items.Length);
                foreach (pjse.FileTable.Entry item in items)
                {
                    pfOBJD pfo = new pfOBJD();
                    pfo.ProcessData(item.PFD, item.Package);
                    lpfo.Add(pfo);
                    SimPe.Wait.Progress++;
                }
                SimPe.Wait.Stop();

                availableOBJDs = lpfo;
                return lpfo;
            }
        }

        #region ITool Members

        internal IToolResult Execute(ref SimPe.Interfaces.Files.IPackedFileDescriptor pfd, ref SimPe.Interfaces.Files.IPackageFile package, IProviderRegistry prov)
        {
            SimPe.RemoteControl.ApplicationForm.Cursor = new Avalonia.Input.Cursor(Avalonia.Input.StandardCursorType.Wait);

            if (!initialised) InitializeForm();
            availableOBJDs = null;

            this.pfd = pfd;
            changed = false;

            List<pfOBJD> apfs = AvailableOBJDs;
            btnSelectOBJD.IsEnabled = apfs.Count > 1;

            SimPe.RemoteControl.ApplicationForm.Cursor = null;

            if (apfs.Count > 1 || apfs.Count == 0) btnSelectOBJD_Click(null, null);
            else CurrentOBJD = apfs[0];

            if (wrapper != null)
            {
                btnCommit.IsEnabled = wrapper.Changed;
                this.ShowDialog(null).GetAwaiter().GetResult();
            }

            return new SimPe.Plugin.ToolResult(changed, false);
        }

        #endregion

        private void btnSelectOBJD_Click(object sender, EventArgs e)
        {
            cOBJDChooser coc = new cOBJDChooser();
            coc.Execute(AvailableOBJDs);
            if (coc.DialogAccepted && coc.Value != null)
                CurrentOBJD = coc.Value;
        }

        private void btnCommit_Click(object sender, EventArgs e)
        {
            SaveOBJD();
        }

        private void cbOBJDvsn_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (wrapper == null) return;

            ushort value = (ushort)(cbOBJDvsn.SelectedIndex < 1 ? 0x8b : cbOBJDvsn.SelectedIndex > 1 ? 0x8d : 0x8c);
            wrapper[0x00] = value;

            value = wrapper[absEPs[1]];
            if (wrapper[0x00] < 0x8c) { wrapper[absEPs[1]] = 0; gbValidEPs2.IsEnabled = false; if (value != 0) wrapper[absEPs[0]] = 0x01; }
            else
                gbValidEPs2.IsEnabled = true;
        }
    }
}
