/***************************************************************************
 *   Copyright (C) 2007 by Peter L Jones                                   *
 *   pljones@users.sf.net                                                  *
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
using SimPe.PackedFiles.Wrapper;

namespace pjse.BhavOperandWizards.Wiz0x0079
{
	/// <summary>
	/// Summary description for BhavInstruction.
	/// </summary>
    internal class UI : System.Windows.Forms.Form, iBhavOperandWizForm
    {
        #region Form variables

        internal System.Windows.Forms.Panel pnWiz0x0079;
        private CheckBox checkClear;
        private CheckBox checkRebuild;
        private CheckBox checkSave;
        private CheckBox checkAttrPicker;
        private TextBox textDataValue1;
        private Label lblTarget;
        private ComboBox comboDataPicker1;
        private ComboBox comboDataOwner1;
        private ComboBox comboSource;
        private CheckBox checkDecimal;
        private CheckBox checkOutfitVariable;
        private ComboBox comboOutfitType;
        private Label lblOutfitType;
        private Label lblGuid;
        private TextBox textGUID;
        private Label lblSource;
        private ComboBox comboDataOwner2;
        private Panel panelVariable;
        private Label lblVariable;
        private ComboBox comboDataPicker2;
        private TextBox textDataValue2;
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.Container components = null;
        #endregion

        public UI()
        {
            //
            // Required designer variable.
            //
            InitializeComponent();
        }

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
            }
            base.Dispose(disposing);

            inst = null;
        }

        private Instruction inst = null;
        private DataOwnerControl doVariable = null;
        private DataOwnerControl doTarget = null;

        #region iBhavOperandWizForm
        public Panel WizPanel { get { return this.pnWiz0x0079; } }

        public void Execute(Instruction inst)
        {
            this.inst = inst;
            this.comboOutfitType.Items.Clear();
            this.comboOutfitType.Items.AddRange(BhavWiz.readStr(GS.BhavStr.PersonOutfits).ToArray());

            wrappedByteArray ops1 = inst.Operands;
            wrappedByteArray ops2 = inst.Reserved1;
            Boolset boolset0 = (Boolset)ops1[0];

            comboSource.SelectedIndex = (boolset0[0] ? 3 : (boolset0[1] ? 1 : (boolset0[6] ? 2 : 0)));
            checkOutfitVariable.Checked = boolset0[2];
            checkSave.Checked = boolset0[3];
            checkRebuild.Checked = boolset0[4];
            checkClear.Checked = boolset0[5];

            doVariable = new DataOwnerControl(inst, this.comboDataOwner2, this.comboDataPicker2, this.textDataValue2, this.checkDecimal, this.checkAttrPicker, null, ops1[1], BhavWiz.ToShort(ops1[2], ops1[3]));
            doTarget = new DataOwnerControl(inst, this.comboDataOwner1, this.comboDataPicker1, this.textDataValue1, this.checkDecimal, this.checkAttrPicker, null, ops2[1], BhavWiz.ToShort(ops2[2], ops2[3]));
            textGUID.Text = "0x" + SimPe.Helper.HexString(ops1[4] | (ops1[5] << 8) | (ops1[6] << 16) | (ops1[7] << 24));
            if ((int)ops2[0] < comboOutfitType.Items.Count) comboOutfitType.SelectedIndex = (int)ops2[0];

            UpdatePanelState();
        }

        public Instruction Write(Instruction inst)
        {
            if (inst != null)
            {
                wrappedByteArray ops1 = inst.Operands;
                wrappedByteArray ops2 = inst.Reserved1;

                uint uint32 = Convert.ToUInt32(textGUID.Text, 16);
                Boolset boolset0 = (Boolset)ops1[0];
                boolset0[0] = (comboSource.SelectedIndex == 3);
                boolset0[1] = (comboSource.SelectedIndex == 1);
                boolset0[2] = checkOutfitVariable.Checked;
                boolset0[3] = checkSave.Checked;
                boolset0[4] = checkRebuild.Checked;
                boolset0[5] = checkClear.Checked;
                boolset0[6] = (comboSource.SelectedIndex == 2);
                ops1[0] = (byte)boolset0;
                ops1[1] = doVariable.DataOwner;
                ops1[2] = (byte)doVariable.Value;
                ops1[3] = (byte)(doVariable.Value >> 8);
                ops1[4] = (byte)(uint32 & (uint)byte.MaxValue);
                ops1[5] = (byte)(uint32 >> 8 & (uint)byte.MaxValue);
                ops1[6] = (byte)(uint32 >> 16 & (uint)byte.MaxValue);
                ops1[7] = (byte)(uint32 >> 24 & (uint)byte.MaxValue);

                ops2[0] = (byte)comboOutfitType.SelectedIndex;
                ops2[1] = doTarget.DataOwner;
                ops2[2] = (byte)doTarget.Value;
                ops2[3] = (byte)(doTarget.Value >> 8);
            }
            return inst;
        }

        #endregion

        #region Windows Form Designer generated code
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.pnWiz0x0079 = new System.Windows.Forms.Panel();
            this.checkClear = new System.Windows.Forms.CheckBox();
            this.checkRebuild = new System.Windows.Forms.CheckBox();
            this.checkSave = new System.Windows.Forms.CheckBox();
            this.checkAttrPicker = new System.Windows.Forms.CheckBox();
            this.textDataValue1 = new System.Windows.Forms.TextBox();
            this.lblTarget = new System.Windows.Forms.Label();
            this.comboDataPicker1 = new System.Windows.Forms.ComboBox();
            this.comboDataOwner1 = new System.Windows.Forms.ComboBox();
            this.comboSource = new System.Windows.Forms.ComboBox();
            this.checkDecimal = new System.Windows.Forms.CheckBox();
            this.checkOutfitVariable = new System.Windows.Forms.CheckBox();
            this.comboOutfitType = new System.Windows.Forms.ComboBox();
            this.panelVariable = new System.Windows.Forms.Panel();
            this.comboDataOwner2 = new System.Windows.Forms.ComboBox();
            this.lblVariable = new System.Windows.Forms.Label();
            this.comboDataPicker2 = new System.Windows.Forms.ComboBox();
            this.textDataValue2 = new System.Windows.Forms.TextBox();
            this.lblOutfitType = new System.Windows.Forms.Label();
            this.lblGuid = new System.Windows.Forms.Label();
            this.textGUID = new System.Windows.Forms.TextBox();
            this.lblSource = new System.Windows.Forms.Label();
            this.pnWiz0x0079.SuspendLayout();
            this.panelVariable.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnWiz0x0079
            // 
            this.pnWiz0x0079.AutoSize = true;
            this.pnWiz0x0079.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.pnWiz0x0079.BackColor = System.Drawing.Color.Transparent;
            this.pnWiz0x0079.Controls.Add(this.checkClear);
            this.pnWiz0x0079.Controls.Add(this.checkRebuild);
            this.pnWiz0x0079.Controls.Add(this.checkSave);
            this.pnWiz0x0079.Controls.Add(this.checkAttrPicker);
            this.pnWiz0x0079.Controls.Add(this.textDataValue1);
            this.pnWiz0x0079.Controls.Add(this.lblTarget);
            this.pnWiz0x0079.Controls.Add(this.comboDataPicker1);
            this.pnWiz0x0079.Controls.Add(this.comboDataOwner1);
            this.pnWiz0x0079.Controls.Add(this.comboSource);
            this.pnWiz0x0079.Controls.Add(this.checkDecimal);
            this.pnWiz0x0079.Controls.Add(this.checkOutfitVariable);
            this.pnWiz0x0079.Controls.Add(this.comboOutfitType);
            this.pnWiz0x0079.Controls.Add(this.panelVariable);
            this.pnWiz0x0079.Controls.Add(this.lblOutfitType);
            this.pnWiz0x0079.Controls.Add(this.lblGuid);
            this.pnWiz0x0079.Controls.Add(this.textGUID);
            this.pnWiz0x0079.Controls.Add(this.lblSource);
            this.pnWiz0x0079.Location = new System.Drawing.Point(0, 0);
            this.pnWiz0x0079.Margin = new System.Windows.Forms.Padding(2);
            this.pnWiz0x0079.Name = "pnWiz0x0079";
            this.pnWiz0x0079.Size = new System.Drawing.Size(446, 240);
            this.pnWiz0x0079.TabIndex = 0;
            // 
            // checkClear
            // 
            this.checkClear.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.checkClear.Location = new System.Drawing.Point(12, 193);
            this.checkClear.Name = "checkClear";
            this.checkClear.Size = new System.Drawing.Size(231, 17);
            this.checkClear.TabIndex = 70;
            this.checkClear.Text = "Clear GUID pointers in person data fields:";
            this.checkClear.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.checkClear.UseVisualStyleBackColor = true;
            // 
            // checkRebuild
            // 
            this.checkRebuild.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.checkRebuild.Location = new System.Drawing.Point(12, 171);
            this.checkRebuild.Name = "checkRebuild";
            this.checkRebuild.Size = new System.Drawing.Size(231, 17);
            this.checkRebuild.TabIndex = 69;
            this.checkRebuild.Text = "Rebuild:";
            this.checkRebuild.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.checkRebuild.UseVisualStyleBackColor = true;
            // 
            // checkSave
            // 
            this.checkSave.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.checkSave.Location = new System.Drawing.Point(12, 149);
            this.checkSave.Name = "checkSave";
            this.checkSave.Size = new System.Drawing.Size(231, 17);
            this.checkSave.TabIndex = 68;
            this.checkSave.Text = "Save change:";
            this.checkSave.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.checkSave.UseVisualStyleBackColor = true;
            // 
            // checkAttrPicker
            // 
            this.checkAttrPicker.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.checkAttrPicker.AutoSize = true;
            this.checkAttrPicker.Location = new System.Drawing.Point(317, 218);
            this.checkAttrPicker.Name = "checkAttrPicker";
            this.checkAttrPicker.Size = new System.Drawing.Size(117, 17);
            this.checkAttrPicker.TabIndex = 72;
            this.checkAttrPicker.Text = "use Attribute picker";
            this.checkAttrPicker.UseVisualStyleBackColor = true;
            // 
            // textDataValue1
            // 
            this.textDataValue1.Location = new System.Drawing.Point(314, 6);
            this.textDataValue1.Name = "textDataValue1";
            this.textDataValue1.Size = new System.Drawing.Size(120, 20);
            this.textDataValue1.TabIndex = 59;
            // 
            // lblTarget
            // 
            this.lblTarget.Location = new System.Drawing.Point(5, 9);
            this.lblTarget.Name = "lblTarget";
            this.lblTarget.Size = new System.Drawing.Size(95, 13);
            this.lblTarget.TabIndex = 74;
            this.lblTarget.Text = "Target:";
            this.lblTarget.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // comboDataPicker1
            // 
            this.comboDataPicker1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboDataPicker1.FormattingEnabled = true;
            this.comboDataPicker1.Location = new System.Drawing.Point(314, 6);
            this.comboDataPicker1.Name = "comboDataPicker1";
            this.comboDataPicker1.Size = new System.Drawing.Size(120, 21);
            this.comboDataPicker1.TabIndex = 60;
            // 
            // comboDataOwner1
            // 
            this.comboDataOwner1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboDataOwner1.FormattingEnabled = true;
            this.comboDataOwner1.Location = new System.Drawing.Point(108, 6);
            this.comboDataOwner1.Name = "comboDataOwner1";
            this.comboDataOwner1.Size = new System.Drawing.Size(200, 21);
            this.comboDataOwner1.TabIndex = 58;
            // 
            // comboSource
            // 
            this.comboSource.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboSource.FormattingEnabled = true;
            this.comboSource.Items.AddRange(new object[] {
            "the sim\'s outfits",
            "specific GUID",
            "GUID [Temp 0x0000,1]",
            "Stack Object"});
            this.comboSource.Location = new System.Drawing.Point(108, 33);
            this.comboSource.Name = "comboSource";
            this.comboSource.Size = new System.Drawing.Size(167, 21);
            this.comboSource.TabIndex = 62;
            this.comboSource.SelectedIndexChanged += new System.EventHandler(this.comboSource_SelectedIndexChanged);
            // 
            // checkDecimal
            // 
            this.checkDecimal.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.checkDecimal.AutoSize = true;
            this.checkDecimal.Location = new System.Drawing.Point(171, 218);
            this.checkDecimal.Name = "checkDecimal";
            this.checkDecimal.Size = new System.Drawing.Size(140, 17);
            this.checkDecimal.TabIndex = 71;
            this.checkDecimal.Text = "Decimal (except Consts)";
            this.checkDecimal.UseVisualStyleBackColor = true;
            // 
            // checkOutfitVariable
            // 
            this.checkOutfitVariable.AutoSize = true;
            this.checkOutfitVariable.Location = new System.Drawing.Point(282, 91);
            this.checkOutfitVariable.Name = "checkOutfitVariable";
            this.checkOutfitVariable.Size = new System.Drawing.Size(86, 17);
            this.checkOutfitVariable.TabIndex = 66;
            this.checkOutfitVariable.Text = "Use Variable";
            this.checkOutfitVariable.UseVisualStyleBackColor = true;
            this.checkOutfitVariable.CheckedChanged += new System.EventHandler(this.checkOutfitVariable_CheckedChanged);
            // 
            // comboOutfitType
            // 
            this.comboOutfitType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboOutfitType.FormattingEnabled = true;
            this.comboOutfitType.Location = new System.Drawing.Point(108, 89);
            this.comboOutfitType.Name = "comboOutfitType";
            this.comboOutfitType.Size = new System.Drawing.Size(167, 21);
            this.comboOutfitType.TabIndex = 64;
            // 
            // panelVariable
            // 
            this.panelVariable.Controls.Add(this.comboDataOwner2);
            this.panelVariable.Controls.Add(this.lblVariable);
            this.panelVariable.Controls.Add(this.comboDataPicker2);
            this.panelVariable.Controls.Add(this.textDataValue2);
            this.panelVariable.Location = new System.Drawing.Point(0, 118);
            this.panelVariable.Name = "panelVariable";
            this.panelVariable.Size = new System.Drawing.Size(442, 24);
            this.panelVariable.TabIndex = 7;
            // 
            // comboDataOwner2
            // 
            this.comboDataOwner2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboDataOwner2.FormattingEnabled = true;
            this.comboDataOwner2.Location = new System.Drawing.Point(108, 0);
            this.comboDataOwner2.Name = "comboDataOwner2";
            this.comboDataOwner2.Size = new System.Drawing.Size(200, 21);
            this.comboDataOwner2.TabIndex = 0;
            // 
            // lblVariable
            // 
            this.lblVariable.Location = new System.Drawing.Point(5, 4);
            this.lblVariable.Name = "lblVariable";
            this.lblVariable.Size = new System.Drawing.Size(95, 13);
            this.lblVariable.TabIndex = 43;
            this.lblVariable.Text = "Variable:";
            this.lblVariable.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // comboDataPicker2
            // 
            this.comboDataPicker2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboDataPicker2.FormattingEnabled = true;
            this.comboDataPicker2.Location = new System.Drawing.Point(315, 0);
            this.comboDataPicker2.Name = "comboDataPicker2";
            this.comboDataPicker2.Size = new System.Drawing.Size(120, 21);
            this.comboDataPicker2.TabIndex = 1;
            // 
            // textDataValue2
            // 
            this.textDataValue2.Location = new System.Drawing.Point(317, 0);
            this.textDataValue2.Name = "textDataValue2";
            this.textDataValue2.Size = new System.Drawing.Size(113, 20);
            this.textDataValue2.TabIndex = 2;
            this.textDataValue2.Visible = false;
            // 
            // lblOutfitType
            // 
            this.lblOutfitType.Location = new System.Drawing.Point(5, 92);
            this.lblOutfitType.Name = "lblOutfitType";
            this.lblOutfitType.Size = new System.Drawing.Size(95, 13);
            this.lblOutfitType.TabIndex = 65;
            this.lblOutfitType.Text = "Outfit index:";
            this.lblOutfitType.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // lblGuid
            // 
            this.lblGuid.AutoSize = true;
            this.lblGuid.Location = new System.Drawing.Point(97, 62);
            this.lblGuid.Name = "lblGuid";
            this.lblGuid.Size = new System.Drawing.Size(34, 13);
            this.lblGuid.TabIndex = 61;
            this.lblGuid.Text = "GUID";
            // 
            // textGUID
            // 
            this.textGUID.Location = new System.Drawing.Point(284, 33);
            this.textGUID.Name = "textGUID";
            this.textGUID.Size = new System.Drawing.Size(150, 20);
            this.textGUID.TabIndex = 63;
            this.textGUID.TextChanged += new System.EventHandler(this.textGUID_TextChanged);
            // 
            // lblSource
            // 
            this.lblSource.Location = new System.Drawing.Point(5, 36);
            this.lblSource.Name = "lblSource";
            this.lblSource.Size = new System.Drawing.Size(95, 13);
            this.lblSource.TabIndex = 57;
            this.lblSource.Text = "Source:";
            this.lblSource.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // UI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(464, 274);
            this.Controls.Add(this.pnWiz0x0079);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "UI";
            this.Text = "UI";
            this.pnWiz0x0079.ResumeLayout(false);
            this.pnWiz0x0079.PerformLayout();
            this.panelVariable.ResumeLayout(false);
            this.panelVariable.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion

        private void textGUID_TextChanged(object sender, EventArgs e)
        {
            uint guid = 0;
            try { guid = Convert.ToUInt32(((Control)sender).Text, 16); }
            catch { return; }
            lblGuid.Text = pjse.BhavWiz.FormatGUID(true, guid);
        }

        private void checkOutfitVariable_CheckedChanged(object sender, EventArgs e)
        {
            UpdatePanelState();
        }

        private void comboSource_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdatePanelState();
        }

        private void UpdatePanelState()
        {
            textGUID.Visible = lblGuid.Visible = (comboSource.SelectedIndex == 1);
            comboOutfitType.Visible = !checkOutfitVariable.Checked;
            panelVariable.Visible = checkOutfitVariable.Checked;
        }
    }
}

namespace pjse.BhavOperandWizards
{
	public class BhavOperandWiz0x0079 : pjse.ABhavOperandWiz
	{
		public BhavOperandWiz0x0079(Instruction i) : base(i) { myForm = new Wiz0x0079.UI(); }

		#region IDisposable Members
		public override void Dispose()
		{
			if (myForm != null) myForm = null;
		}
		#endregion

	}

}
