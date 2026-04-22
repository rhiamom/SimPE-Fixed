/***************************************************************************
 *   Copyright (C) 2005 by Ambertation                                     *
 *   quaxi@ambertation.de                                                  *
 *                                                                         *
 *   Copyright (C) 2008 Peter L Jones                                      *
 *   pljones@users.sf.net                                                  *
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
// Restored from SimPe 0.75f layout (decompiled from simpe.ngbh.plugin.dll)
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Ambertation.Windows.Forms;
using SimPe.Interfaces.Files;
using SimPe.Interfaces.Plugin;

namespace SimPe.Plugin
{

public class LtxtForm : Form
{
	internal Panel ltxtPanel;

	private Button button1;

	private Panel panel2;

	private Label label27;

	private Label label1;

	private Label label2;

	private Label label3;

	private Label label4;

	private Label label5;

	private Label label7;

	internal TextBox tblotname;

	internal TextBox tbdesc;

	internal TextBox tbRoads;

	internal ComboBox cbtype;

	internal TextBox tbrotation;

	internal TextBox tbtype;

	internal TextBox tbver;

	private Label label10;

	internal TextBox tbsubver;

	private Label label11;

	internal TextBox tbhg;

	internal TextBox tbwd;

	private Label label9;

	private Label label8;

	internal TextBox tbtop;

	internal TextBox tbleft;

	private Label label12;

	private Label label13;

	private Label label14;

	internal EnumComboBox cborient;

	internal TextBox tbinst;

	private Label label15;

	internal TextBox tbu2;

	private Label label18;

	internal ListBox lb;

	private Label label19;

	private Label label20;

	internal TextBox tbz;

	internal TextBox tbData;

	internal TextBox tbu0;

	private Label label21;

	internal PictureBox pb;

	internal TextBox tbu4;

	private Label label22;

	internal TextBox tbu3;

	private Label label16;

	internal TextBox tbTexture;

	private Label label6;

	private Label label24;

	private Label label25;

	internal TextBox tbowner;

	internal TextBox tbu5;

	internal TextBox tbApBase;

	private Label label23;

	internal TextBox tbu6;

	private Label label26;

	internal ListBox lbApts;

	internal TextBox tbElevationAt;

	internal TextBox tbApartment;

	internal GroupBox gbApartment;

	internal TextBox tbSAu3;

	private Label label31;

	internal TextBox tbSAu2;

	private Label label30;

	internal TextBox tbSAFamily;

	private TableLayoutPanel tableLayoutPanel1;

	internal ListBox lbu7;

	private Label label32;

	internal TextBox tbu7;

	private TableLayoutPanel tableLayoutPanel2;

	private FlowLayoutPanel flowLayoutPanel1;

	private TableLayoutPanel tableLayoutPanel3;

	private FlowLayoutPanel flowLayoutPanel2;

	private TableLayoutPanel tableLayoutPanel4;

	private TableLayoutPanel tableLayoutPanel5;

	private TableLayoutPanel tableLayoutPanel6;

	private FlowLayoutPanel flowLayoutPanel3;

	private TableLayoutPanel tableLayoutPanel7;

	private TableLayoutPanel tableLayoutPanel8;

	internal LinkLabel llFamily;

	internal LinkLabel llSubLot;

	internal LinkLabel llAptBase;

	private FlowLayoutPanel flowLayoutPanel4;

	internal Button btnDelApt;

	private Button btnAddApt;

	internal FlowLayoutPanel flpAptBtns;

	private Container components;

	internal Ltxt wrapper;

	public LtxtForm()
	{
		InitializeComponent();
		wrapper = null;
		cborient.ResourceManager = Localization.Manager;
		cborient.Enum = typeof(LotOrientation);
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing && components != null)
		{
			components.Dispose();
		}
		base.Dispose(disposing);
	}

	private void InitializeComponent()
	{
		//IL_02a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_02af: Expected O, but got Unknown
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SimPe.Plugin.LtxtForm));
		this.ltxtPanel = new System.Windows.Forms.Panel();
		this.tableLayoutPanel7 = new System.Windows.Forms.TableLayoutPanel();
		this.label19 = new System.Windows.Forms.Label();
		this.tbData = new System.Windows.Forms.TextBox();
		this.tableLayoutPanel6 = new System.Windows.Forms.TableLayoutPanel();
		this.flowLayoutPanel3 = new System.Windows.Forms.FlowLayoutPanel();
		this.label32 = new System.Windows.Forms.Label();
		this.tbu7 = new System.Windows.Forms.TextBox();
		this.lbu7 = new System.Windows.Forms.ListBox();
		this.tableLayoutPanel5 = new System.Windows.Forms.TableLayoutPanel();
		this.flowLayoutPanel4 = new System.Windows.Forms.FlowLayoutPanel();
		this.label26 = new System.Windows.Forms.Label();
		this.flpAptBtns = new System.Windows.Forms.FlowLayoutPanel();
		this.btnAddApt = new System.Windows.Forms.Button();
		this.btnDelApt = new System.Windows.Forms.Button();
		this.lbApts = new System.Windows.Forms.ListBox();
		this.gbApartment = new System.Windows.Forms.GroupBox();
		this.tableLayoutPanel8 = new System.Windows.Forms.TableLayoutPanel();
		this.llFamily = new System.Windows.Forms.LinkLabel();
		this.llSubLot = new System.Windows.Forms.LinkLabel();
		this.tbSAu3 = new System.Windows.Forms.TextBox();
		this.tbSAFamily = new System.Windows.Forms.TextBox();
		this.tbApartment = new System.Windows.Forms.TextBox();
		this.label31 = new System.Windows.Forms.Label();
		this.tbSAu2 = new System.Windows.Forms.TextBox();
		this.label30 = new System.Windows.Forms.Label();
		this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
		this.label18 = new System.Windows.Forms.Label();
		this.llAptBase = new System.Windows.Forms.LinkLabel();
		this.tbu2 = new System.Windows.Forms.TextBox();
		this.label25 = new System.Windows.Forms.Label();
		this.tbowner = new System.Windows.Forms.TextBox();
		this.tbApBase = new System.Windows.Forms.TextBox();
		this.label23 = new System.Windows.Forms.Label();
		this.tbu6 = new System.Windows.Forms.TextBox();
		this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
		this.lb = new System.Windows.Forms.ListBox();
		this.flowLayoutPanel2 = new System.Windows.Forms.FlowLayoutPanel();
		this.label7 = new System.Windows.Forms.Label();
		this.tbElevationAt = new System.Windows.Forms.TextBox();
		this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
		this.label10 = new System.Windows.Forms.Label();
		this.tbver = new System.Windows.Forms.TextBox();
		this.tbsubver = new System.Windows.Forms.TextBox();
		this.label1 = new System.Windows.Forms.Label();
		this.cbtype = new System.Windows.Forms.ComboBox();
		this.tbtype = new System.Windows.Forms.TextBox();
		this.label2 = new System.Windows.Forms.Label();
		this.tbRoads = new System.Windows.Forms.TextBox();
		this.label8 = new System.Windows.Forms.Label();
		this.tbwd = new System.Windows.Forms.TextBox();
		this.label9 = new System.Windows.Forms.Label();
		this.tbhg = new System.Windows.Forms.TextBox();
		this.label12 = new System.Windows.Forms.Label();
		this.tbtop = new System.Windows.Forms.TextBox();
		this.label13 = new System.Windows.Forms.Label();
		this.tbleft = new System.Windows.Forms.TextBox();
		this.label20 = new System.Windows.Forms.Label();
		this.tbz = new System.Windows.Forms.TextBox();
		this.label14 = new System.Windows.Forms.Label();
		this.cborient = new EnumComboBox();
		this.label3 = new System.Windows.Forms.Label();
		this.tbrotation = new System.Windows.Forms.TextBox();
		this.label21 = new System.Windows.Forms.Label();
		this.tbu0 = new System.Windows.Forms.TextBox();
		this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
		this.tbu5 = new System.Windows.Forms.TextBox();
		this.label24 = new System.Windows.Forms.Label();
		this.tbu4 = new System.Windows.Forms.TextBox();
		this.label22 = new System.Windows.Forms.Label();
		this.tbu3 = new System.Windows.Forms.TextBox();
		this.label16 = new System.Windows.Forms.Label();
		this.tbinst = new System.Windows.Forms.TextBox();
		this.label15 = new System.Windows.Forms.Label();
		this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
		this.label4 = new System.Windows.Forms.Label();
		this.label5 = new System.Windows.Forms.Label();
		this.tblotname = new System.Windows.Forms.TextBox();
		this.tbdesc = new System.Windows.Forms.TextBox();
		this.label6 = new System.Windows.Forms.Label();
		this.tbTexture = new System.Windows.Forms.TextBox();
		this.label11 = new System.Windows.Forms.Label();
		this.button1 = new System.Windows.Forms.Button();
		this.panel2 = new System.Windows.Forms.Panel();
		this.label27 = new System.Windows.Forms.Label();
		this.pb = new System.Windows.Forms.PictureBox();
		this.ltxtPanel.SuspendLayout();
		this.tableLayoutPanel7.SuspendLayout();
		this.tableLayoutPanel6.SuspendLayout();
		this.flowLayoutPanel3.SuspendLayout();
		this.tableLayoutPanel5.SuspendLayout();
		this.flowLayoutPanel4.SuspendLayout();
		this.flpAptBtns.SuspendLayout();
		this.gbApartment.SuspendLayout();
		this.tableLayoutPanel8.SuspendLayout();
		this.tableLayoutPanel4.SuspendLayout();
		this.tableLayoutPanel3.SuspendLayout();
		this.flowLayoutPanel2.SuspendLayout();
		this.flowLayoutPanel1.SuspendLayout();
		this.tableLayoutPanel2.SuspendLayout();
		this.tableLayoutPanel1.SuspendLayout();
		this.panel2.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.pb).BeginInit();
		base.SuspendLayout();
		resources.ApplyResources(this.ltxtPanel, "ltxtPanel");
		this.ltxtPanel.Controls.Add(this.tableLayoutPanel7);
		this.ltxtPanel.Controls.Add(this.tableLayoutPanel6);
		this.ltxtPanel.Controls.Add(this.tableLayoutPanel5);
		this.ltxtPanel.Controls.Add(this.tableLayoutPanel4);
		this.ltxtPanel.Controls.Add(this.tableLayoutPanel3);
		this.ltxtPanel.Controls.Add(this.flowLayoutPanel1);
		this.ltxtPanel.Controls.Add(this.tableLayoutPanel2);
		this.ltxtPanel.Controls.Add(this.tableLayoutPanel1);
		this.ltxtPanel.Controls.Add(this.label11);
		this.ltxtPanel.Controls.Add(this.button1);
		this.ltxtPanel.Controls.Add(this.panel2);
		this.ltxtPanel.Controls.Add(this.pb);
		this.ltxtPanel.Name = "ltxtPanel";
		resources.ApplyResources(this.tableLayoutPanel7, "tableLayoutPanel7");
		this.tableLayoutPanel7.Controls.Add(this.label19, 0, 0);
		this.tableLayoutPanel7.Controls.Add(this.tbData, 1, 0);
		this.tableLayoutPanel7.Name = "tableLayoutPanel7";
		resources.ApplyResources(this.label19, "label19");
		this.label19.Name = "label19";
		resources.ApplyResources(this.tbData, "tbData");
		this.tbData.Name = "tbData";
		this.tbData.TextChanged += new System.EventHandler(ChangeData);
		resources.ApplyResources(this.tableLayoutPanel6, "tableLayoutPanel6");
		this.tableLayoutPanel6.Controls.Add(this.flowLayoutPanel3, 0, 0);
		this.tableLayoutPanel6.Controls.Add(this.lbu7, 1, 0);
		this.tableLayoutPanel6.Name = "tableLayoutPanel6";
		resources.ApplyResources(this.flowLayoutPanel3, "flowLayoutPanel3");
		this.flowLayoutPanel3.Controls.Add(this.label32);
		this.flowLayoutPanel3.Controls.Add(this.tbu7);
		this.flowLayoutPanel3.Name = "flowLayoutPanel3";
		resources.ApplyResources(this.label32, "label32");
		this.label32.Name = "label32";
		resources.ApplyResources(this.tbu7, "tbu7");
		this.tbu7.Name = "tbu7";
		this.tbu7.TextChanged += new System.EventHandler(tbu7_TextChanged);
		resources.ApplyResources(this.lbu7, "lbu7");
		this.lbu7.Items.AddRange(new object[8]
		{
			resources.GetString("lbu7.Items"),
			resources.GetString("lbu7.Items1"),
			resources.GetString("lbu7.Items2"),
			resources.GetString("lbu7.Items3"),
			resources.GetString("lbu7.Items4"),
			resources.GetString("lbu7.Items5"),
			resources.GetString("lbu7.Items6"),
			resources.GetString("lbu7.Items7")
		});
		this.lbu7.MultiColumn = true;
		this.lbu7.Name = "lbu7";
		this.lbu7.SelectedIndexChanged += new System.EventHandler(lbu7_SelectedIndexChanged);
		resources.ApplyResources(this.tableLayoutPanel5, "tableLayoutPanel5");
		this.tableLayoutPanel5.Controls.Add(this.flowLayoutPanel4, 0, 0);
		this.tableLayoutPanel5.Controls.Add(this.lbApts, 1, 0);
		this.tableLayoutPanel5.Controls.Add(this.gbApartment, 2, 0);
		this.tableLayoutPanel5.Name = "tableLayoutPanel5";
		resources.ApplyResources(this.flowLayoutPanel4, "flowLayoutPanel4");
		this.flowLayoutPanel4.Controls.Add(this.label26);
		this.flowLayoutPanel4.Controls.Add(this.flpAptBtns);
		this.flowLayoutPanel4.Name = "flowLayoutPanel4";
		resources.ApplyResources(this.label26, "label26");
		this.flowLayoutPanel4.SetFlowBreak(this.label26, true);
		this.label26.Name = "label26";
		resources.ApplyResources(this.flpAptBtns, "flpAptBtns");
		this.flpAptBtns.Controls.Add(this.btnAddApt);
		this.flpAptBtns.Controls.Add(this.btnDelApt);
		this.flpAptBtns.Name = "flpAptBtns";
		resources.ApplyResources(this.btnAddApt, "btnAddApt");
		this.btnAddApt.Name = "btnAddApt";
		this.btnAddApt.UseVisualStyleBackColor = true;
		this.btnAddApt.Click += new System.EventHandler(btnAddApt_Click);
		resources.ApplyResources(this.btnDelApt, "btnDelApt");
		this.btnDelApt.Name = "btnDelApt";
		this.btnDelApt.UseVisualStyleBackColor = true;
		this.btnDelApt.Click += new System.EventHandler(btnDelApt_Click);
		resources.ApplyResources(this.lbApts, "lbApts");
		this.lbApts.Items.AddRange(new object[8]
		{
			resources.GetString("lbApts.Items"),
			resources.GetString("lbApts.Items1"),
			resources.GetString("lbApts.Items2"),
			resources.GetString("lbApts.Items3"),
			resources.GetString("lbApts.Items4"),
			resources.GetString("lbApts.Items5"),
			resources.GetString("lbApts.Items6"),
			resources.GetString("lbApts.Items7")
		});
		this.lbApts.MultiColumn = true;
		this.lbApts.Name = "lbApts";
		this.lbApts.SelectedIndexChanged += new System.EventHandler(lbApts_SelectedIndexChanged);
		resources.ApplyResources(this.gbApartment, "gbApartment");
		this.gbApartment.Controls.Add(this.tableLayoutPanel8);
		this.gbApartment.Name = "gbApartment";
		this.gbApartment.TabStop = false;
		resources.ApplyResources(this.tableLayoutPanel8, "tableLayoutPanel8");
		this.tableLayoutPanel8.Controls.Add(this.llFamily, 0, 1);
		this.tableLayoutPanel8.Controls.Add(this.llSubLot, 0, 0);
		this.tableLayoutPanel8.Controls.Add(this.tbSAu3, 3, 1);
		this.tableLayoutPanel8.Controls.Add(this.tbSAFamily, 1, 1);
		this.tableLayoutPanel8.Controls.Add(this.tbApartment, 1, 0);
		this.tableLayoutPanel8.Controls.Add(this.label31, 2, 1);
		this.tableLayoutPanel8.Controls.Add(this.tbSAu2, 3, 0);
		this.tableLayoutPanel8.Controls.Add(this.label30, 2, 0);
		this.tableLayoutPanel8.Name = "tableLayoutPanel8";
		resources.ApplyResources(this.llFamily, "llFamily");
		this.llFamily.Name = "llFamily";
		this.llFamily.TabStop = true;
		this.llFamily.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(ll_Click);
		resources.ApplyResources(this.llSubLot, "llSubLot");
		this.llSubLot.Name = "llSubLot";
		this.llSubLot.TabStop = true;
		this.llSubLot.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(ll_Click);
		resources.ApplyResources(this.tbSAu3, "tbSAu3");
		this.tbSAu3.Name = "tbSAu3";
		this.tbSAu3.TextChanged += new System.EventHandler(SAChange);
		resources.ApplyResources(this.tbSAFamily, "tbSAFamily");
		this.tbSAFamily.Name = "tbSAFamily";
		this.tbSAFamily.TextChanged += new System.EventHandler(SAChange);
		resources.ApplyResources(this.tbApartment, "tbApartment");
		this.tbApartment.Name = "tbApartment";
		this.tbApartment.TextChanged += new System.EventHandler(SAChange);
		resources.ApplyResources(this.label31, "label31");
		this.label31.Name = "label31";
		resources.ApplyResources(this.tbSAu2, "tbSAu2");
		this.tbSAu2.Name = "tbSAu2";
		this.tbSAu2.TextChanged += new System.EventHandler(SAChange);
		resources.ApplyResources(this.label30, "label30");
		this.label30.Name = "label30";
		resources.ApplyResources(this.tableLayoutPanel4, "tableLayoutPanel4");
		this.tableLayoutPanel4.Controls.Add(this.label18, 0, 0);
		this.tableLayoutPanel4.Controls.Add(this.llAptBase, 4, 0);
		this.tableLayoutPanel4.Controls.Add(this.tbu2, 1, 0);
		this.tableLayoutPanel4.Controls.Add(this.label25, 2, 0);
		this.tableLayoutPanel4.Controls.Add(this.tbowner, 3, 0);
		this.tableLayoutPanel4.Controls.Add(this.tbApBase, 5, 0);
		this.tableLayoutPanel4.Controls.Add(this.label23, 6, 0);
		this.tableLayoutPanel4.Controls.Add(this.tbu6, 7, 0);
		this.tableLayoutPanel4.Name = "tableLayoutPanel4";
		resources.ApplyResources(this.label18, "label18");
		this.label18.Name = "label18";
		resources.ApplyResources(this.llAptBase, "llAptBase");
		this.llAptBase.Name = "llAptBase";
		this.llAptBase.TabStop = true;
		this.llAptBase.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(ll_Click);
		resources.ApplyResources(this.tbu2, "tbu2");
		this.tbu2.Name = "tbu2";
		this.tbu2.TextChanged += new System.EventHandler(CommonChange);
		resources.ApplyResources(this.label25, "label25");
		this.label25.Name = "label25";
		resources.ApplyResources(this.tbowner, "tbowner");
		this.tbowner.Name = "tbowner";
		this.tbowner.TextChanged += new System.EventHandler(CommonChange);
		resources.ApplyResources(this.tbApBase, "tbApBase");
		this.tbApBase.Name = "tbApBase";
		this.tbApBase.TextChanged += new System.EventHandler(tbApBase_TextChanged);
		resources.ApplyResources(this.label23, "label23");
		this.label23.Name = "label23";
		resources.ApplyResources(this.tbu6, "tbu6");
		this.tbu6.Name = "tbu6";
		this.tbu6.TextChanged += new System.EventHandler(ChangeData);
		resources.ApplyResources(this.tableLayoutPanel3, "tableLayoutPanel3");
		this.tableLayoutPanel3.Controls.Add(this.lb, 1, 0);
		this.tableLayoutPanel3.Controls.Add(this.flowLayoutPanel2, 0, 0);
		this.tableLayoutPanel3.Name = "tableLayoutPanel3";
		resources.ApplyResources(this.lb, "lb");
		this.lb.Items.AddRange(new object[49]
		{
			resources.GetString("lb.Items"),
			resources.GetString("lb.Items1"),
			resources.GetString("lb.Items2"),
			resources.GetString("lb.Items3"),
			resources.GetString("lb.Items4"),
			resources.GetString("lb.Items5"),
			resources.GetString("lb.Items6"),
			resources.GetString("lb.Items7"),
			resources.GetString("lb.Items8"),
			resources.GetString("lb.Items9"),
			resources.GetString("lb.Items10"),
			resources.GetString("lb.Items11"),
			resources.GetString("lb.Items12"),
			resources.GetString("lb.Items13"),
			resources.GetString("lb.Items14"),
			resources.GetString("lb.Items15"),
			resources.GetString("lb.Items16"),
			resources.GetString("lb.Items17"),
			resources.GetString("lb.Items18"),
			resources.GetString("lb.Items19"),
			resources.GetString("lb.Items20"),
			resources.GetString("lb.Items21"),
			resources.GetString("lb.Items22"),
			resources.GetString("lb.Items23"),
			resources.GetString("lb.Items24"),
			resources.GetString("lb.Items25"),
			resources.GetString("lb.Items26"),
			resources.GetString("lb.Items27"),
			resources.GetString("lb.Items28"),
			resources.GetString("lb.Items29"),
			resources.GetString("lb.Items30"),
			resources.GetString("lb.Items31"),
			resources.GetString("lb.Items32"),
			resources.GetString("lb.Items33"),
			resources.GetString("lb.Items34"),
			resources.GetString("lb.Items35"),
			resources.GetString("lb.Items36"),
			resources.GetString("lb.Items37"),
			resources.GetString("lb.Items38"),
			resources.GetString("lb.Items39"),
			resources.GetString("lb.Items40"),
			resources.GetString("lb.Items41"),
			resources.GetString("lb.Items42"),
			resources.GetString("lb.Items43"),
			resources.GetString("lb.Items44"),
			resources.GetString("lb.Items45"),
			resources.GetString("lb.Items46"),
			resources.GetString("lb.Items47"),
			resources.GetString("lb.Items48")
		});
		this.lb.MultiColumn = true;
		this.lb.Name = "lb";
		this.lb.SelectedIndexChanged += new System.EventHandler(lb_SelectedIndexChanged);
		resources.ApplyResources(this.flowLayoutPanel2, "flowLayoutPanel2");
		this.flowLayoutPanel2.Controls.Add(this.label7);
		this.flowLayoutPanel2.Controls.Add(this.tbElevationAt);
		this.flowLayoutPanel2.Name = "flowLayoutPanel2";
		resources.ApplyResources(this.label7, "label7");
		this.label7.Name = "label7";
		resources.ApplyResources(this.tbElevationAt, "tbElevationAt");
		this.tbElevationAt.Name = "tbElevationAt";
		this.tbElevationAt.TextChanged += new System.EventHandler(tbElevationAt_TextChanged);
		resources.ApplyResources(this.flowLayoutPanel1, "flowLayoutPanel1");
		this.flowLayoutPanel1.Controls.Add(this.label10);
		this.flowLayoutPanel1.Controls.Add(this.tbver);
		this.flowLayoutPanel1.Controls.Add(this.tbsubver);
		this.flowLayoutPanel1.Controls.Add(this.label1);
		this.flowLayoutPanel1.Controls.Add(this.cbtype);
		this.flowLayoutPanel1.Controls.Add(this.tbtype);
		this.flowLayoutPanel1.Controls.Add(this.label2);
		this.flowLayoutPanel1.Controls.Add(this.tbRoads);
		this.flowLayoutPanel1.Controls.Add(this.label8);
		this.flowLayoutPanel1.Controls.Add(this.tbwd);
		this.flowLayoutPanel1.Controls.Add(this.label9);
		this.flowLayoutPanel1.Controls.Add(this.tbhg);
		this.flowLayoutPanel1.Controls.Add(this.label12);
		this.flowLayoutPanel1.Controls.Add(this.tbtop);
		this.flowLayoutPanel1.Controls.Add(this.label13);
		this.flowLayoutPanel1.Controls.Add(this.tbleft);
		this.flowLayoutPanel1.Controls.Add(this.label20);
		this.flowLayoutPanel1.Controls.Add(this.tbz);
		this.flowLayoutPanel1.Controls.Add(this.label14);
		this.flowLayoutPanel1.Controls.Add((System.Windows.Forms.Control)(object)this.cborient);
		this.flowLayoutPanel1.Controls.Add(this.label3);
		this.flowLayoutPanel1.Controls.Add(this.tbrotation);
		this.flowLayoutPanel1.Controls.Add(this.label21);
		this.flowLayoutPanel1.Controls.Add(this.tbu0);
		this.flowLayoutPanel1.Name = "flowLayoutPanel1";
		resources.ApplyResources(this.label10, "label10");
		this.label10.Name = "label10";
		resources.ApplyResources(this.tbver, "tbver");
		this.tbver.Name = "tbver";
		this.tbver.ReadOnly = true;
		resources.ApplyResources(this.tbsubver, "tbsubver");
		this.tbsubver.Name = "tbsubver";
		this.tbsubver.ReadOnly = true;
		resources.ApplyResources(this.label1, "label1");
		this.label1.Name = "label1";
		resources.ApplyResources(this.cbtype, "cbtype");
		this.cbtype.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
		this.cbtype.Name = "cbtype";
		this.cbtype.SelectedIndexChanged += new System.EventHandler(SelectType);
		resources.ApplyResources(this.tbtype, "tbtype");
		this.tbtype.Name = "tbtype";
		this.tbtype.ReadOnly = true;
		resources.ApplyResources(this.label2, "label2");
		this.label2.Name = "label2";
		resources.ApplyResources(this.tbRoads, "tbRoads");
		this.flowLayoutPanel1.SetFlowBreak(this.tbRoads, true);
		this.tbRoads.Name = "tbRoads";
		this.tbRoads.TextChanged += new System.EventHandler(CommonChange);
		resources.ApplyResources(this.label8, "label8");
		this.label8.Name = "label8";
		resources.ApplyResources(this.tbwd, "tbwd");
		this.tbwd.Name = "tbwd";
		this.tbwd.TextChanged += new System.EventHandler(CommonChange);
		resources.ApplyResources(this.label9, "label9");
		this.label9.Name = "label9";
		resources.ApplyResources(this.tbhg, "tbhg");
		this.tbhg.Name = "tbhg";
		this.tbhg.TextChanged += new System.EventHandler(CommonChange);
		resources.ApplyResources(this.label12, "label12");
		this.label12.Name = "label12";
		resources.ApplyResources(this.tbtop, "tbtop");
		this.tbtop.Name = "tbtop";
		this.tbtop.TextChanged += new System.EventHandler(CommonChange);
		resources.ApplyResources(this.label13, "label13");
		this.label13.Name = "label13";
		resources.ApplyResources(this.tbleft, "tbleft");
		this.tbleft.Name = "tbleft";
		this.tbleft.TextChanged += new System.EventHandler(CommonChange);
		resources.ApplyResources(this.label20, "label20");
		this.label20.Name = "label20";
		resources.ApplyResources(this.tbz, "tbz");
		this.flowLayoutPanel1.SetFlowBreak(this.tbz, true);
		this.tbz.Name = "tbz";
		this.tbz.TextChanged += new System.EventHandler(CommonChange);
		resources.ApplyResources(this.label14, "label14");
		this.label14.Name = "label14";
		resources.ApplyResources(this.cborient, "cborient");
		((System.Windows.Forms.ComboBox)(object)this.cborient).DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
		this.cborient.Enum = null;
		((System.Windows.Forms.Control)(object)this.cborient).Name = "cborient";
		this.cborient.ResourceManager = null;
		((System.Windows.Forms.ComboBox)(object)this.cborient).SelectedIndexChanged += new System.EventHandler(CommonChange);
		resources.ApplyResources(this.label3, "label3");
		this.label3.Name = "label3";
		resources.ApplyResources(this.tbrotation, "tbrotation");
		this.tbrotation.Name = "tbrotation";
		this.tbrotation.TextChanged += new System.EventHandler(CommonChange);
		resources.ApplyResources(this.label21, "label21");
		this.label21.Name = "label21";
		resources.ApplyResources(this.tbu0, "tbu0");
		this.tbu0.Name = "tbu0";
		this.tbu0.TextChanged += new System.EventHandler(CommonChange);
		resources.ApplyResources(this.tableLayoutPanel2, "tableLayoutPanel2");
		this.tableLayoutPanel2.Controls.Add(this.tbu5, 7, 0);
		this.tableLayoutPanel2.Controls.Add(this.label24, 6, 0);
		this.tableLayoutPanel2.Controls.Add(this.tbu4, 5, 0);
		this.tableLayoutPanel2.Controls.Add(this.label22, 4, 0);
		this.tableLayoutPanel2.Controls.Add(this.tbu3, 3, 0);
		this.tableLayoutPanel2.Controls.Add(this.label16, 2, 0);
		this.tableLayoutPanel2.Controls.Add(this.tbinst, 1, 0);
		this.tableLayoutPanel2.Controls.Add(this.label15, 0, 0);
		this.tableLayoutPanel2.Name = "tableLayoutPanel2";
		resources.ApplyResources(this.tbu5, "tbu5");
		this.tbu5.Name = "tbu5";
		this.tbu5.TextChanged += new System.EventHandler(ChangeData);
		resources.ApplyResources(this.label24, "label24");
		this.label24.Name = "label24";
		resources.ApplyResources(this.tbu4, "tbu4");
		this.tbu4.Name = "tbu4";
		this.tbu4.TextChanged += new System.EventHandler(CommonChange);
		resources.ApplyResources(this.label22, "label22");
		this.label22.Name = "label22";
		resources.ApplyResources(this.tbu3, "tbu3");
		this.tbu3.Name = "tbu3";
		this.tbu3.TextChanged += new System.EventHandler(CommonChange);
		resources.ApplyResources(this.label16, "label16");
		this.label16.Name = "label16";
		resources.ApplyResources(this.tbinst, "tbinst");
		this.tbinst.Name = "tbinst";
		this.tbinst.TextChanged += new System.EventHandler(CommonChange);
		resources.ApplyResources(this.label15, "label15");
		this.label15.Name = "label15";
		resources.ApplyResources(this.tableLayoutPanel1, "tableLayoutPanel1");
		this.tableLayoutPanel1.Controls.Add(this.label4, 0, 0);
		this.tableLayoutPanel1.Controls.Add(this.label5, 0, 1);
		this.tableLayoutPanel1.Controls.Add(this.tblotname, 1, 0);
		this.tableLayoutPanel1.Controls.Add(this.tbdesc, 1, 1);
		this.tableLayoutPanel1.Controls.Add(this.label6, 2, 0);
		this.tableLayoutPanel1.Controls.Add(this.tbTexture, 3, 0);
		this.tableLayoutPanel1.Name = "tableLayoutPanel1";
		resources.ApplyResources(this.label4, "label4");
		this.label4.Name = "label4";
		resources.ApplyResources(this.label5, "label5");
		this.label5.Name = "label5";
		resources.ApplyResources(this.tblotname, "tblotname");
		this.tblotname.Name = "tblotname";
		this.tblotname.TextChanged += new System.EventHandler(CommonChange);
		resources.ApplyResources(this.tbdesc, "tbdesc");
		this.tableLayoutPanel1.SetColumnSpan(this.tbdesc, 3);
		this.tbdesc.Name = "tbdesc";
		this.tbdesc.TextChanged += new System.EventHandler(CommonChange);
		resources.ApplyResources(this.label6, "label6");
		this.label6.Name = "label6";
		resources.ApplyResources(this.tbTexture, "tbTexture");
		this.tbTexture.Name = "tbTexture";
		this.tbTexture.TextChanged += new System.EventHandler(CommonChange);
		resources.ApplyResources(this.label11, "label11");
		this.label11.Name = "label11";
		resources.ApplyResources(this.button1, "button1");
		this.button1.Name = "button1";
		this.button1.Click += new System.EventHandler(Commit);
		resources.ApplyResources(this.panel2, "panel2");
		this.panel2.BackColor = System.Drawing.SystemColors.AppWorkspace;
		this.panel2.Controls.Add(this.label27);
		this.panel2.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
		this.panel2.Name = "panel2";
		resources.ApplyResources(this.label27, "label27");
		this.label27.Name = "label27";
		resources.ApplyResources(this.pb, "pb");
		this.pb.Name = "pb";
		this.pb.TabStop = false;
		resources.ApplyResources(this, "$this");
		base.Controls.Add(this.ltxtPanel);
		base.Name = "LtxtForm";
		this.ltxtPanel.ResumeLayout(false);
		this.ltxtPanel.PerformLayout();
		this.tableLayoutPanel7.ResumeLayout(false);
		this.tableLayoutPanel7.PerformLayout();
		this.tableLayoutPanel6.ResumeLayout(false);
		this.tableLayoutPanel6.PerformLayout();
		this.flowLayoutPanel3.ResumeLayout(false);
		this.flowLayoutPanel3.PerformLayout();
		this.tableLayoutPanel5.ResumeLayout(false);
		this.tableLayoutPanel5.PerformLayout();
		this.flowLayoutPanel4.ResumeLayout(false);
		this.flowLayoutPanel4.PerformLayout();
		this.flpAptBtns.ResumeLayout(false);
		this.flpAptBtns.PerformLayout();
		this.gbApartment.ResumeLayout(false);
		this.gbApartment.PerformLayout();
		this.tableLayoutPanel8.ResumeLayout(false);
		this.tableLayoutPanel8.PerformLayout();
		this.tableLayoutPanel4.ResumeLayout(false);
		this.tableLayoutPanel4.PerformLayout();
		this.tableLayoutPanel3.ResumeLayout(false);
		this.tableLayoutPanel3.PerformLayout();
		this.flowLayoutPanel2.ResumeLayout(false);
		this.flowLayoutPanel2.PerformLayout();
		this.flowLayoutPanel1.ResumeLayout(false);
		this.flowLayoutPanel1.PerformLayout();
		this.tableLayoutPanel2.ResumeLayout(false);
		this.tableLayoutPanel2.PerformLayout();
		this.tableLayoutPanel1.ResumeLayout(false);
		this.tableLayoutPanel1.PerformLayout();
		this.panel2.ResumeLayout(false);
		this.panel2.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.pb).EndInit();
		base.ResumeLayout(false);
	}

	private void SelectType(object sender, EventArgs e)
	{
		if (wrapper != null)
		{
			if (Enum.IsDefined(typeof(Ltxt.LotType), cbtype.SelectedItem))
			{
				wrapper.Type = (Ltxt.LotType)cbtype.SelectedItem;
			}
			else
			{
				wrapper.Type = Ltxt.LotType.Unknown;
			}
			tbtype.Text = "0x" + Helper.HexString((byte)wrapper.Type);
			flpAptBtns.Enabled = wrapper.Type == Ltxt.LotType.ApartmentBase;
			((AbstractWrapper)wrapper).Changed = true;
		}
	}

	private void Commit(object sender, EventArgs e)
	{
		if (wrapper == null)
		{
			return;
		}
		try
		{
			((AbstractWrapper)wrapper).SynchronizeUserData();
			MessageBox.Show(Localization.Manager.GetString("commited"));
		}
		catch (Exception ex)
		{
			Helper.ExceptionMessage(Localization.Manager.GetString("errwritingfile"), ex);
		}
	}

	private void CommonChange(object sender, EventArgs e)
	{
		if (wrapper == null)
		{
			return;
		}
		try
		{
			wrapper.LotRoads = Convert.ToByte(tbRoads.Text, 16);
			wrapper.LotSize = new Size(Helper.StringToInt32(tbwd.Text, wrapper.LotSize.Width, (byte)10), Helper.StringToInt32(tbhg.Text, wrapper.LotSize.Height, (byte)10));
			wrapper.LotPosition = new Point(Helper.StringToInt32(tbleft.Text, wrapper.LotPosition.X, (byte)10), Helper.StringToInt32(tbtop.Text, wrapper.LotPosition.Y, (byte)10));
			wrapper.LotElevation = Helper.StringToFloat(tbz.Text, wrapper.LotElevation);
			wrapper.Orientation = (LotOrientation)cborient.SelectedValue;
			wrapper.LotRotation = Convert.ToByte(tbrotation.Text, 16);
			wrapper.Unknown0 = Helper.StringToUInt32(tbu0.Text, wrapper.Unknown0, (byte)16);
			wrapper.LotName = tblotname.Text;
			wrapper.Texture = tbTexture.Text;
			wrapper.LotDesc = tbdesc.Text;
			wrapper.LotInstance = Helper.StringToUInt32(tbinst.Text, wrapper.LotInstance, (byte)16);
			wrapper.Unknown3 = Helper.StringToFloat(tbu3.Text, wrapper.Unknown3);
			wrapper.Unknown4 = Helper.StringToUInt32(tbu4.Text, wrapper.Unknown4, (byte)16);
			wrapper.Unknown2 = (byte)Helper.StringToUInt16(tbu2.Text, (ushort)wrapper.Unknown2, (byte)16);
			wrapper.OwnerInstance = Helper.StringToUInt32(tbowner.Text, wrapper.OwnerInstance, (byte)16);
			((AbstractWrapper)wrapper).Changed = true;
		}
		catch (Exception ex)
		{
			Helper.ExceptionMessage("", ex);
		}
	}

	private void ChangeData(object sender, EventArgs e)
	{
		if (wrapper == null)
		{
			return;
		}
		try
		{
			wrapper.Unknown5 = Helper.SetLength(Helper.HexListToBytes(tbu5.Text), 14);
			wrapper.Unknown6 = Helper.SetLength(Helper.HexListToBytes(tbu6.Text), 9);
			wrapper.Followup = Helper.SetLength(Helper.HexListToBytes(tbData.Text), 0);
			((AbstractWrapper)wrapper).Changed = true;
		}
		catch (Exception ex)
		{
			Helper.ExceptionMessage("", ex);
		}
	}

	private void lb_SelectedIndexChanged(object sender, EventArgs e)
	{
		if (wrapper != null)
		{
			Ltxt ltxt = wrapper;
			wrapper = null;
			if (lb.SelectedIndex < 0)
			{
				tbElevationAt.Text = "";
			}
			else
			{
				tbElevationAt.Text = ltxt.Unknown1[lb.SelectedIndex].ToString();
			}
			wrapper = ltxt;
		}
	}

	private void tbElevationAt_TextChanged(object sender, EventArgs e)
	{
		if (wrapper == null || lb.SelectedIndex < 0)
		{
			return;
		}
		Ltxt ltxt = wrapper;
		wrapper = null;
		try
		{
			ltxt.Unknown1[lb.SelectedIndex] = Helper.StringToFloat(tbElevationAt.Text, ltxt.Unknown1[lb.SelectedIndex]);
			int num = Convert.ToInt32(lb.SelectedIndex / ltxt.LotSize.Height);
			int num2 = lb.SelectedIndex - num * ltxt.LotSize.Height;
			lb.Items[lb.SelectedIndex] = "(" + num2 + "," + num + ") " + ltxt.Unknown1[lb.SelectedIndex];
			((AbstractWrapper)ltxt).Changed = true;
		}
		catch (Exception ex)
		{
			Helper.ExceptionMessage("", ex);
		}
		finally
		{
			wrapper = ltxt;
		}
	}

	private void lbApts_SelectedIndexChanged(object sender, EventArgs e)
	{
		if (wrapper != null)
		{
			Ltxt ltxt = wrapper;
			wrapper = null;
			if (lbApts.SelectedIndex < 0)
			{
				TextBox textBox = tbApartment;
				TextBox textBox2 = tbSAFamily;
				TextBox textBox3 = tbSAu2;
				string text = (tbSAu3.Text = "");
				string text3 = (textBox3.Text = text);
				string text5 = (textBox2.Text = text3);
				textBox.Text = text5;
				Button button = btnDelApt;
				LinkLabel linkLabel = llFamily;
				bool flag = (llSubLot.Enabled = false);
				bool enabled = (linkLabel.Enabled = flag);
				button.Enabled = enabled;
			}
			else
			{
				Ltxt.SubLot subLot = ltxt.SubLots[lbApts.SelectedIndex];
				tbApartment.Text = (string)lbApts.SelectedItem;
				tbSAFamily.Text = "0x" + Helper.HexString(subLot.Family);
				tbSAu2.Text = "0x" + Helper.HexString(subLot.Unknown2);
				tbSAu3.Text = "0x" + Helper.HexString(subLot.Unknown3);
				Button button2 = btnDelApt;
				LinkLabel linkLabel2 = llFamily;
				bool flag = (llSubLot.Enabled = true);
				bool enabled = (linkLabel2.Enabled = flag);
				button2.Enabled = enabled;
			}
			wrapper = ltxt;
		}
	}

	private void SAChange(object sender, EventArgs e)
	{
		if (wrapper == null || lbApts.SelectedIndex < 0)
		{
			return;
		}
		Ltxt ltxt = wrapper;
		wrapper = null;
		try
		{
			Ltxt.SubLot subLot = ltxt.SubLots[lbApts.SelectedIndex];
			subLot.ApartmentSublot = Helper.StringToUInt32(tbApartment.Text, subLot.ApartmentSublot, (byte)16);
			subLot.Family = Helper.StringToUInt32(tbSAFamily.Text, subLot.Family, (byte)16);
			subLot.Unknown2 = Helper.StringToUInt32(tbSAu2.Text, subLot.Unknown2, (byte)16);
			subLot.Unknown3 = Helper.StringToUInt32(tbSAu3.Text, subLot.Unknown3, (byte)16);
			lbApts.Items[lbApts.SelectedIndex] = "0x" + Helper.HexString(subLot.ApartmentSublot);
			((AbstractWrapper)ltxt).Changed = true;
		}
		catch (Exception ex)
		{
			Helper.ExceptionMessage("", ex);
		}
		finally
		{
			wrapper = ltxt;
		}
	}

	private void lbu7_SelectedIndexChanged(object sender, EventArgs e)
	{
		if (wrapper != null)
		{
			Ltxt ltxt = wrapper;
			wrapper = null;
			if (lbu7.SelectedIndex < 0)
			{
				tbu7.Text = "";
			}
			else
			{
				tbu7.Text = (string)lbu7.SelectedItem;
			}
			wrapper = ltxt;
		}
	}

	private void tbu7_TextChanged(object sender, EventArgs e)
	{
		if (wrapper == null || lbu7.SelectedIndex < 0)
		{
			return;
		}
		Ltxt ltxt = wrapper;
		wrapper = null;
		try
		{
			ltxt.Unknown7[lbu7.SelectedIndex] = Helper.StringToUInt32(tbu7.Text, ltxt.Unknown7[lbu7.SelectedIndex], (byte)16);
			lbu7.Items[lb.SelectedIndex] = "0x" + Helper.HexString(ltxt.Unknown7[lb.SelectedIndex]);
			((AbstractWrapper)ltxt).Changed = true;
		}
		catch (Exception ex)
		{
			Helper.ExceptionMessage("", ex);
		}
		finally
		{
			wrapper = ltxt;
		}
	}

	private void ll_Click(object sender, LinkLabelLinkClickedEventArgs e)
	{
		uint num;
		uint num2;
		switch (new List<LinkLabel>(new LinkLabel[3] { llAptBase, llSubLot, llFamily }).IndexOf((LinkLabel)sender))
		{
		default:
			return;
		case 0:
			num = 200907239u;
			num2 = wrapper.ApartmentBase;
			break;
		case 1:
			num = 200907239u;
			num2 = wrapper.SubLots[lbApts.SelectedIndex].ApartmentSublot;
			break;
		case 2:
			num = 1178684745u;
			num2 = wrapper.SubLots[lbApts.SelectedIndex].Family;
			break;
		}
		IPackedFileDescriptor val = ((AbstractWrapper)wrapper).Package.NewDescriptor(num, ((IPackedFileDescriptorBasic)((AbstractWrapper)wrapper).FileDescriptor).SubType, ((IPackedFileDescriptorBasic)((AbstractWrapper)wrapper).FileDescriptor).Group, num2);
		val = ((AbstractWrapper)wrapper).Package.FindFile(val);
		if (val != null)
		{
			RemoteControl.OpenPackedFile(val, ((AbstractWrapper)wrapper).Package);
		}
	}

	private void btnAddApt_Click(object sender, EventArgs e)
	{
		wrapper.SubLots.Add(new Ltxt.SubLot());
		lbApts.Items.Add("0x" + Helper.HexString(wrapper.SubLots[wrapper.SubLots.Count - 1].ApartmentSublot));
		lbApts.SelectedIndex = wrapper.SubLots.Count - 1;
		((AbstractWrapper)wrapper).Changed = true;
	}

	private void btnDelApt_Click(object sender, EventArgs e)
	{
		int num = lbApts.SelectedIndex;
		lbApts.BeginUpdate();
		lbApts.SelectedIndex = -1;
		wrapper.SubLots.RemoveAt(num);
		lbApts.Items.RemoveAt(num);
		if (num > 0)
		{
			num--;
		}
		else if (lbApts.Items.Count == 0)
		{
			num = -1;
		}
		lbApts.SelectedIndex = num;
		lbApts.EndUpdate();
		((AbstractWrapper)wrapper).Changed = true;
	}

	private void tbApBase_TextChanged(object sender, EventArgs e)
	{
		if (wrapper != null)
		{
			wrapper.ApartmentBase = Helper.StringToUInt32(tbApBase.Text, wrapper.ApartmentBase, (byte)16);
			llAptBase.Enabled = wrapper.ApartmentBase != 0;
		}
	}
}
}
