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

namespace pjse.BhavOperandWizards.Wiz0x007c
{

    internal class UI : System.Windows.Forms.Form, iBhavOperandWizForm
    {
        #region Form variables

        internal System.Windows.Forms.Panel pnWiz0x007c;
        private Label lblType;
        private Label lblWantName;
        private Label lblTargetSim;
        private Label lblSubjectSim;
        private Label lblLevel;
        private Label lblWant;
        private TextBox textDataValue1;
        private TextBox textDataValue2;
        private TextBox textDataValue3;
        private ComboBox comboDataPicker1;
        private ComboBox comboDataPicker2;
        private ComboBox comboType;
        private ComboBox comboDataPicker3;
        private TextBox textGUID;
        private ComboBox comboDataOwner1;
        private ComboBox comboDataOwner2;
        private ComboBox comboDataOwner3;
        private PictureBox WantIcon;
        private CheckBox checkDecimal;
        private CheckBox checkAttrPicker;

        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.Container components = null;
        #endregion

        public UI()
        {
            InitializeComponent();
        }

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
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
        private DataOwnerControl doTargetSim = null;
        private DataOwnerControl doSubjectSim = null;
        private DataOwnerControl doLevel = null;

        #region iBhavOperandWizForm
        public Panel WizPanel { get { return this.pnWiz0x007c; } }
        public void Execute(Instruction inst)
        {
            this.inst = inst;

            wrappedByteArray reserved1 = inst.Reserved1;
            wrappedByteArray ops = inst.Operands;

            textGUID.Text = "0x" + SimPe.Helper.HexString(ops[3] | (ops[4] << 8) | (ops[5] << 16) | (ops[6] << 24));
            doTargetSim = new DataOwnerControl(inst, this.comboDataOwner1, this.comboDataPicker1, this.textDataValue1, this.checkDecimal, this.checkAttrPicker, null, ops[0], BhavWiz.ToShort(ops[1], ops[2]));
            doSubjectSim = new DataOwnerControl(inst, this.comboDataOwner2, this.comboDataPicker2, this.textDataValue2, this.checkDecimal, this.checkAttrPicker, null, ops[7], BhavWiz.ToShort(reserved1[0], reserved1[1]));
            doLevel = new DataOwnerControl(inst, this.comboDataOwner3, this.comboDataPicker3, this.textDataValue3, this.checkDecimal, this.checkAttrPicker, null, reserved1[2], BhavWiz.ToShort(reserved1[3], reserved1[4]));
            if ( (int)reserved1[3] < comboType.Items.Count) comboType.SelectedIndex = (int)reserved1[3];

            UpdateWantName();
        }

        public Instruction Write(Instruction inst)
        {
            if (inst != null)
            {
                wrappedByteArray ops = inst.Operands;
                wrappedByteArray reserved1 = inst.Reserved1;;
                uint uint32 = Convert.ToUInt32(textGUID.Text, 16);

                ops[0] = doTargetSim.DataOwner;
                ops[1] = (byte)doTargetSim.Value;
                ops[2] = (byte)(doTargetSim.Value >> 8);
                ops[3] = (byte)(uint32 & (uint)byte.MaxValue);
                ops[4] = (byte)(uint32 >> 8 & (uint)byte.MaxValue);
                ops[5] = (byte)(uint32 >> 16 & (uint)byte.MaxValue);
                ops[6] = (byte)(uint32 >> 24 & (uint)byte.MaxValue);
                ops[7] = doSubjectSim.DataOwner;

                reserved1[0] = (byte)doSubjectSim.Value;
                reserved1[1] = (byte)(doSubjectSim.Value >> 8);
                reserved1[2] = doLevel.DataOwner;
                reserved1[3] = (byte)doLevel.Value;
                reserved1[4] = (byte)(doLevel.Value >> 8);
                reserved1[5] = (byte)comboType.SelectedIndex;
            }
            return inst;
        }

        #endregion


        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.pnWiz0x007c = new System.Windows.Forms.Panel();
            this.lblType = new System.Windows.Forms.Label();
            this.lblWantName = new System.Windows.Forms.Label();
            this.lblTargetSim = new System.Windows.Forms.Label();
            this.lblSubjectSim = new System.Windows.Forms.Label();
            this.lblLevel = new System.Windows.Forms.Label();
            this.lblWant = new System.Windows.Forms.Label();
            this.textDataValue1 = new System.Windows.Forms.TextBox();
            this.textDataValue2 = new System.Windows.Forms.TextBox();
            this.textDataValue3 = new System.Windows.Forms.TextBox();
            this.comboDataPicker1 = new System.Windows.Forms.ComboBox();
            this.comboDataPicker2 = new System.Windows.Forms.ComboBox();
            this.comboType = new System.Windows.Forms.ComboBox();
            this.comboDataPicker3 = new System.Windows.Forms.ComboBox();
            this.textGUID = new System.Windows.Forms.TextBox();
            this.comboDataOwner1 = new System.Windows.Forms.ComboBox();
            this.comboDataOwner2 = new System.Windows.Forms.ComboBox();
            this.comboDataOwner3 = new System.Windows.Forms.ComboBox();
            this.WantIcon = new System.Windows.Forms.PictureBox();
            this.checkDecimal = new System.Windows.Forms.CheckBox();
            this.checkAttrPicker = new System.Windows.Forms.CheckBox();
            this.pnWiz0x007c.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.WantIcon)).BeginInit();
            this.SuspendLayout();
            // 
            // pnWiz0x007c
            // 
            this.pnWiz0x007c.BackColor = System.Drawing.Color.Transparent;
            this.pnWiz0x007c.Controls.Add(this.lblType);
            this.pnWiz0x007c.Controls.Add(this.lblWantName);
            this.pnWiz0x007c.Controls.Add(this.lblTargetSim);
            this.pnWiz0x007c.Controls.Add(this.lblSubjectSim);
            this.pnWiz0x007c.Controls.Add(this.lblLevel);
            this.pnWiz0x007c.Controls.Add(this.lblWant);
            this.pnWiz0x007c.Controls.Add(this.textDataValue1);
            this.pnWiz0x007c.Controls.Add(this.textDataValue2);
            this.pnWiz0x007c.Controls.Add(this.textDataValue3);
            this.pnWiz0x007c.Controls.Add(this.comboDataPicker1);
            this.pnWiz0x007c.Controls.Add(this.comboDataPicker2);
            this.pnWiz0x007c.Controls.Add(this.comboType);
            this.pnWiz0x007c.Controls.Add(this.comboDataPicker3);
            this.pnWiz0x007c.Controls.Add(this.textGUID);
            this.pnWiz0x007c.Controls.Add(this.comboDataOwner1);
            this.pnWiz0x007c.Controls.Add(this.comboDataOwner2);
            this.pnWiz0x007c.Controls.Add(this.comboDataOwner3);
            this.pnWiz0x007c.Controls.Add(this.WantIcon);
            this.pnWiz0x007c.Controls.Add(this.checkDecimal);
            this.pnWiz0x007c.Controls.Add(this.checkAttrPicker);
            this.pnWiz0x007c.Location = new System.Drawing.Point(0, 0);
            this.pnWiz0x007c.Margin = new System.Windows.Forms.Padding(2);
            this.pnWiz0x007c.Name = "pnWiz0x007c";
            this.pnWiz0x007c.Size = new System.Drawing.Size(438, 208);
            this.pnWiz0x007c.TabIndex = 0;
            // 
            // lblType
            // 
            this.lblType.Location = new System.Drawing.Point(4, 158);
            this.lblType.Name = "lblType";
            this.lblType.Size = new System.Drawing.Size(95, 13);
            this.lblType.TabIndex = 47;
            this.lblType.Text = "Type:";
            this.lblType.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // lblWantName
            // 
            this.lblWantName.AutoSize = true;
            this.lblWantName.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblWantName.Location = new System.Drawing.Point(4, 8);
            this.lblWantName.Name = "lblWantName";
            this.lblWantName.Size = new System.Drawing.Size(87, 20);
            this.lblWantName.TabIndex = 46;
            this.lblWantName.Text = "want name";
            // 
            // lblTargetSim
            // 
            this.lblTargetSim.Location = new System.Drawing.Point(4, 77);
            this.lblTargetSim.Name = "lblTargetSim";
            this.lblTargetSim.Size = new System.Drawing.Size(95, 13);
            this.lblTargetSim.TabIndex = 33;
            this.lblTargetSim.Text = "Target Sim:";
            this.lblTargetSim.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // lblSubjectSim
            // 
            this.lblSubjectSim.Location = new System.Drawing.Point(4, 104);
            this.lblSubjectSim.Name = "lblSubjectSim";
            this.lblSubjectSim.Size = new System.Drawing.Size(95, 13);
            this.lblSubjectSim.TabIndex = 44;
            this.lblSubjectSim.Text = "Target:";
            this.lblSubjectSim.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // lblLevel
            // 
            this.lblLevel.Location = new System.Drawing.Point(4, 131);
            this.lblLevel.Name = "lblLevel";
            this.lblLevel.Size = new System.Drawing.Size(95, 13);
            this.lblLevel.TabIndex = 45;
            this.lblLevel.Text = "(optional) level:";
            this.lblLevel.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // lblWant
            // 
            this.lblWant.Location = new System.Drawing.Point(4, 49);
            this.lblWant.Name = "lblWant";
            this.lblWant.Size = new System.Drawing.Size(95, 13);
            this.lblWant.TabIndex = 39;
            this.lblWant.Text = "Want:";
            this.lblWant.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // textDataValue1
            // 
            this.textDataValue1.Location = new System.Drawing.Point(309, 73);
            this.textDataValue1.Name = "textDataValue1";
            this.textDataValue1.Size = new System.Drawing.Size(120, 20);
            this.textDataValue1.TabIndex = 31;
            // 
            // textDataValue2
            // 
            this.textDataValue2.Location = new System.Drawing.Point(309, 100);
            this.textDataValue2.Name = "textDataValue2";
            this.textDataValue2.Size = new System.Drawing.Size(120, 20);
            this.textDataValue2.TabIndex = 35;
            // 
            // textDataValue3
            // 
            this.textDataValue3.Location = new System.Drawing.Point(309, 127);
            this.textDataValue3.Name = "textDataValue3";
            this.textDataValue3.Size = new System.Drawing.Size(120, 20);
            this.textDataValue3.TabIndex = 38;
            // 
            // comboDataPicker1
            // 
            this.comboDataPicker1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboDataPicker1.FormattingEnabled = true;
            this.comboDataPicker1.Location = new System.Drawing.Point(309, 73);
            this.comboDataPicker1.Name = "comboDataPicker1";
            this.comboDataPicker1.Size = new System.Drawing.Size(120, 21);
            this.comboDataPicker1.TabIndex = 30;
            // 
            // comboDataPicker2
            // 
            this.comboDataPicker2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboDataPicker2.FormattingEnabled = true;
            this.comboDataPicker2.Location = new System.Drawing.Point(309, 100);
            this.comboDataPicker2.Name = "comboDataPicker2";
            this.comboDataPicker2.Size = new System.Drawing.Size(120, 21);
            this.comboDataPicker2.TabIndex = 34;
            // 
            // comboType
            // 
            this.comboType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboType.FormattingEnabled = true;
            this.comboType.Items.AddRange(new object[] {
            "Generic",
            "Sim",
            "Object",
            "{unused}",
            "Skill",
            "Career"});
            this.comboType.Location = new System.Drawing.Point(99, 154);
            this.comboType.Name = "comboType";
            this.comboType.Size = new System.Drawing.Size(90, 21);
            this.comboType.TabIndex = 40;
            this.comboType.SelectedIndexChanged += new System.EventHandler(this.comboType_SelectedIndexChanged);
            // 
            // comboDataPicker3
            // 
            this.comboDataPicker3.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboDataPicker3.FormattingEnabled = true;
            this.comboDataPicker3.Location = new System.Drawing.Point(309, 127);
            this.comboDataPicker3.Name = "comboDataPicker3";
            this.comboDataPicker3.Size = new System.Drawing.Size(120, 21);
            this.comboDataPicker3.TabIndex = 37;
            // 
            // textGUID
            // 
            this.textGUID.Location = new System.Drawing.Point(99, 45);
            this.textGUID.Name = "textGUID";
            this.textGUID.Size = new System.Drawing.Size(120, 20);
            this.textGUID.TabIndex = 28;
            this.textGUID.TextChanged += new System.EventHandler(this.textGUID_TextChanged);
            // 
            // comboDataOwner1
            // 
            this.comboDataOwner1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboDataOwner1.FormattingEnabled = true;
            this.comboDataOwner1.Location = new System.Drawing.Point(99, 73);
            this.comboDataOwner1.Name = "comboDataOwner1";
            this.comboDataOwner1.Size = new System.Drawing.Size(200, 21);
            this.comboDataOwner1.TabIndex = 29;
            // 
            // comboDataOwner2
            // 
            this.comboDataOwner2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboDataOwner2.FormattingEnabled = true;
            this.comboDataOwner2.Location = new System.Drawing.Point(99, 100);
            this.comboDataOwner2.Name = "comboDataOwner2";
            this.comboDataOwner2.Size = new System.Drawing.Size(200, 21);
            this.comboDataOwner2.TabIndex = 32;
            // 
            // comboDataOwner3
            // 
            this.comboDataOwner3.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboDataOwner3.FormattingEnabled = true;
            this.comboDataOwner3.Location = new System.Drawing.Point(99, 127);
            this.comboDataOwner3.Name = "comboDataOwner3";
            this.comboDataOwner3.Size = new System.Drawing.Size(200, 21);
            this.comboDataOwner3.TabIndex = 36;
            // 
            // WantIcon
            // 
            this.WantIcon.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.WantIcon.BackColor = System.Drawing.Color.Transparent;
            this.WantIcon.Location = new System.Drawing.Point(362, 0);
            this.WantIcon.Name = "WantIcon";
            this.WantIcon.Size = new System.Drawing.Size(72, 72);
            this.WantIcon.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.WantIcon.TabIndex = 43;
            this.WantIcon.TabStop = false;
            // 
            // checkDecimal
            // 
            this.checkDecimal.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.checkDecimal.AutoSize = true;
            this.checkDecimal.Location = new System.Drawing.Point(144, 188);
            this.checkDecimal.Name = "checkDecimal";
            this.checkDecimal.Size = new System.Drawing.Size(140, 17);
            this.checkDecimal.TabIndex = 41;
            this.checkDecimal.Text = "Decimal (except Consts)";
            this.checkDecimal.UseVisualStyleBackColor = true;
            // 
            // checkAttrPicker
            // 
            this.checkAttrPicker.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.checkAttrPicker.AutoSize = true;
            this.checkAttrPicker.Location = new System.Drawing.Point(290, 188);
            this.checkAttrPicker.Name = "checkAttrPicker";
            this.checkAttrPicker.Size = new System.Drawing.Size(117, 17);
            this.checkAttrPicker.TabIndex = 42;
            this.checkAttrPicker.Text = "use Attribute picker";
            this.checkAttrPicker.UseVisualStyleBackColor = true;
            // 
            // UI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(500, 217);
            this.Controls.Add(this.pnWiz0x007c);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "UI";
            this.Text = "UI";
            this.pnWiz0x007c.ResumeLayout(false);
            this.pnWiz0x007c.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.WantIcon)).EndInit();
            this.ResumeLayout(false);

        }
        #endregion

        private void comboType_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateWantName();
        }

        private void textGUID_TextChanged(object sender, EventArgs e)
        {
            if (!hex32_IsValid(sender)) return;
            UpdateWantName();
        }

        private bool hex32_IsValid(object sender)
        {
            try { Convert.ToUInt32(((TextBox)sender).Text, 16); }
            catch (Exception) { return false; }
            return true;
        }

        private void UpdateWantName()
        {
            try
            {
                SimPe.Plugin.WantInformation wantim = SimPe.Plugin.WantInformation.LoadWant(Convert.ToUInt32(textGUID.Text, 16));
                if (wantim != null) { lblWantName.Text = wantim.Name; WantIcon.Image = wantim.Icon; }
                else { lblWantName.Text = textGUID.Text; WantIcon.Image = null; }
            }
            catch { lblWantName.Text = textGUID.Text; WantIcon.Image = null; }
        }
    }
}

namespace pjse.BhavOperandWizards
{
	public class BhavOperandWiz0x007c : pjse.ABhavOperandWiz
	{
		public BhavOperandWiz0x007c(Instruction i) : base(i) { myForm = new Wiz0x007c.UI(); }

		#region IDisposable Members
		public override void Dispose()
		{
			if (myForm != null) myForm = null;
		}
		#endregion
	}
}