/***************************************************************************
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
using System.Drawing;
using System.Windows.Forms;

namespace SimPe.PackedFiles.UserInterface
{
    /// <summary>
    /// Minimal Sim Description panel for testing SDSC logic.
    /// This does NOT replicate the full original SDSC panel;
    /// it only provides core controls so SDesc can read/write
    /// data during testing.
    /// </summary>
    public class SdscPanel : UserControl
    {
        // BASIC SIM FIELDS
        internal TextBox tbsim;                 // Sim ID
        internal TextBox tbsimdescname;         // First name
        internal TextBox tbsimdescfamname;      // Last name
        internal TextBox tbfaminst;             // Family instance

        // GENDER
        internal RadioButton rbFemale;
        internal RadioButton rbMale;

        // LIFE SECTION / AGE
        internal ComboBox cblifesection;
        internal TextBox tbagedur;

        // SPECIES + ASPIRATION
        internal ComboBox cbspecies;
        internal ComboBox cbaspiration;

        // Commit button (optional wiring later)
        internal Button btcommit;

        public SdscPanel()
        {
            InitializeComponent();
            BuildLayout();
        }

        private void InitializeComponent()
        {
            this.tbsim = new System.Windows.Forms.TextBox();
            this.tbsimdescname = new System.Windows.Forms.TextBox();
            this.tbsimdescfamname = new System.Windows.Forms.TextBox();
            this.tbfaminst = new System.Windows.Forms.TextBox();
            this.rbFemale = new System.Windows.Forms.RadioButton();
            this.rbMale = new System.Windows.Forms.RadioButton();
            this.cblifesection = new System.Windows.Forms.ComboBox();
            this.tbagedur = new System.Windows.Forms.TextBox();
            this.cbspecies = new System.Windows.Forms.ComboBox();
            this.cbaspiration = new System.Windows.Forms.ComboBox();
            this.btcommit = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // tbsim
            // 
            this.tbsim.Location = new System.Drawing.Point(0, 0);
            this.tbsim.Name = "tbsim";
            this.tbsim.Size = new System.Drawing.Size(100, 20);
            this.tbsim.TabIndex = 0;
            // 
            // tbsimdescname
            // 
            this.tbsimdescname.Location = new System.Drawing.Point(0, 0);
            this.tbsimdescname.Name = "tbsimdescname";
            this.tbsimdescname.Size = new System.Drawing.Size(100, 20);
            this.tbsimdescname.TabIndex = 0;
            // 
            // tbsimdescfamname
            // 
            this.tbsimdescfamname.Location = new System.Drawing.Point(0, 0);
            this.tbsimdescfamname.Name = "tbsimdescfamname";
            this.tbsimdescfamname.Size = new System.Drawing.Size(100, 20);
            this.tbsimdescfamname.TabIndex = 0;
            // 
            // tbfaminst
            // 
            this.tbfaminst.Location = new System.Drawing.Point(0, 0);
            this.tbfaminst.Name = "tbfaminst";
            this.tbfaminst.Size = new System.Drawing.Size(100, 20);
            this.tbfaminst.TabIndex = 0;
            // 
            // rbFemale
            // 
            this.rbFemale.Location = new System.Drawing.Point(0, 0);
            this.rbFemale.Name = "rbFemale";
            this.rbFemale.Size = new System.Drawing.Size(104, 24);
            this.rbFemale.TabIndex = 0;
            // 
            // rbMale
            // 
            this.rbMale.Location = new System.Drawing.Point(0, 0);
            this.rbMale.Name = "rbMale";
            this.rbMale.Size = new System.Drawing.Size(104, 24);
            this.rbMale.TabIndex = 0;
            // 
            // cblifesection
            // 
            this.cblifesection.Location = new System.Drawing.Point(0, 0);
            this.cblifesection.Name = "cblifesection";
            this.cblifesection.Size = new System.Drawing.Size(121, 21);
            this.cblifesection.TabIndex = 0;
            // 
            // tbagedur
            // 
            this.tbagedur.Location = new System.Drawing.Point(0, 0);
            this.tbagedur.Name = "tbagedur";
            this.tbagedur.Size = new System.Drawing.Size(100, 20);
            this.tbagedur.TabIndex = 0;
            // 
            // cbspecies
            // 
            this.cbspecies.Location = new System.Drawing.Point(0, 0);
            this.cbspecies.Name = "cbspecies";
            this.cbspecies.Size = new System.Drawing.Size(121, 21);
            this.cbspecies.TabIndex = 0;
            // 
            // cbaspiration
            // 
            this.cbaspiration.Location = new System.Drawing.Point(0, 0);
            this.cbaspiration.Name = "cbaspiration";
            this.cbaspiration.Size = new System.Drawing.Size(121, 21);
            this.cbaspiration.TabIndex = 0;
            // 
            // btcommit
            // 
            this.btcommit.Location = new System.Drawing.Point(0, 0);
            this.btcommit.Name = "btcommit";
            this.btcommit.Size = new System.Drawing.Size(75, 23);
            this.btcommit.TabIndex = 0;
            // 
            // SdscPanel
            // 
            this.Name = "SdscPanel";
            this.Size = new System.Drawing.Size(865, 479);
            this.ResumeLayout(false);

        }

        private Label MakeLabel(string text, int y)
        {
            var lbl = new Label();
            lbl.Text = text;
            lbl.AutoSize = true;
            lbl.Location = new Point(8, y + 4);
            this.Controls.Add(lbl);
            return lbl;
        }
        private void BuildLayout()
        {
            this.Dock = DockStyle.Fill;
            this.Padding = new Padding(8);
            this.BackColor = SystemColors.Control;

            int left = 110;
            int top = 10;
            int rowHeight = 24;
            int spacing = 6;

            MakeLabel("Sim ID:", top);
            this.tbsim.Location = new Point(left, top);
            this.tbsim.Width = 180;
            this.Controls.Add(this.tbsim);
            top += rowHeight + spacing;

            MakeLabel("First name:", top);
            this.tbsimdescname.Location = new Point(left, top);
            this.tbsimdescname.Width = 180;
            this.Controls.Add(this.tbsimdescname);
            top += rowHeight + spacing;

            MakeLabel("Last name:", top);
            this.tbsimdescfamname.Location = new Point(left, top);
            this.tbsimdescfamname.Width = 180;
            this.Controls.Add(this.tbsimdescfamname);
            top += rowHeight + spacing;

            MakeLabel("Family instance:", top);
            this.tbfaminst.Location = new Point(left, top);
            this.tbfaminst.Width = 180;
            this.Controls.Add(this.tbfaminst);
            top += rowHeight + spacing;

            MakeLabel("Gender:", top);
            this.rbFemale.Text = "Female";
            this.rbFemale.AutoSize = true;
            this.rbFemale.Location = new Point(left, top + 2);
            this.Controls.Add(this.rbFemale);

            this.rbMale.Text = "Male";
            this.rbMale.AutoSize = true;
            this.rbMale.Location = new Point(left + 80, top + 2);
            this.Controls.Add(this.rbMale);
            top += rowHeight + spacing;

            MakeLabel("Life section:", top);
            this.cblifesection.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cblifesection.Location = new Point(left, top);
            this.cblifesection.Width = 140;
            this.Controls.Add(this.cblifesection);
            top += rowHeight + spacing;

            MakeLabel("Remaining days:", top);
            this.tbagedur.Location = new Point(left, top);
            this.tbagedur.Width = 60;
            this.Controls.Add(this.tbagedur);
            top += rowHeight + spacing;

            MakeLabel("Species:", top);
            this.cbspecies.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cbspecies.Location = new Point(left, top);
            this.cbspecies.Width = 140;
            this.Controls.Add(this.cbspecies);
            top += rowHeight + spacing;

            MakeLabel("Aspiration:", top);
            this.cbaspiration.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cbaspiration.Location = new Point(left, top);
            this.cbaspiration.Width = 180;
            this.Controls.Add(this.cbaspiration);
            top += rowHeight + spacing + 4;

            this.btcommit.Text = "Commit";
            this.btcommit.Location = new Point(left, top);
            this.btcommit.Width = 100;
            this.Controls.Add(this.btcommit);
        }

    }
}
