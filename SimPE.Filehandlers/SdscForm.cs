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
        }

        private void InitializeComponent()
        {
            this.tbsim = new TextBox();
            this.tbsimdescname = new TextBox();
            this.tbsimdescfamname = new TextBox();
            this.tbfaminst = new TextBox();

            this.rbFemale = new RadioButton();
            this.rbMale = new RadioButton();

            this.cblifesection = new ComboBox();
            this.tbagedur = new TextBox();

            this.cbspecies = new ComboBox();
            this.cbaspiration = new ComboBox();

            this.btcommit = new Button();

            // 
            // SdscPanel
            // 
            this.Dock = DockStyle.Fill;
            this.Padding = new Padding(8);
            this.BackColor = SystemColors.Control;

            int left = 110;
            int top = 10;
            int rowHeight = 24;
            int spacing = 6;

            // Helper function for labels
            Label MakeLabel(string text, int y)
            {
                var lbl = new Label();
                lbl.Text = text;
                lbl.AutoSize = true;
                lbl.Location = new Point(8, y + 4);
                this.Controls.Add(lbl);
                return lbl;
            }

            // Sim ID
            MakeLabel("Sim ID:", top);
            this.tbsim.Location = new Point(left, top);
            this.tbsim.Width = 180;
            this.Controls.Add(this.tbsim);
            top += rowHeight + spacing;

            // First name
            MakeLabel("First name:", top);
            this.tbsimdescname.Location = new Point(left, top);
            this.tbsimdescname.Width = 180;
            this.Controls.Add(this.tbsimdescname);
            top += rowHeight + spacing;

            // Last name
            MakeLabel("Last name:", top);
            this.tbsimdescfamname.Location = new Point(left, top);
            this.tbsimdescfamname.Width = 180;
            this.Controls.Add(this.tbsimdescfamname);
            top += rowHeight + spacing;

            // Family instance
            MakeLabel("Family instance:", top);
            this.tbfaminst.Location = new Point(left, top);
            this.tbfaminst.Width = 180;
            this.Controls.Add(this.tbfaminst);
            top += rowHeight + spacing;

            // Gender
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

            // Life section
            MakeLabel("Life section:", top);
            this.cblifesection.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cblifesection.Location = new Point(left, top);
            this.cblifesection.Width = 140;
            this.Controls.Add(this.cblifesection);
            top += rowHeight + spacing;

            // Remaining days
            MakeLabel("Remaining days:", top);
            this.tbagedur.Location = new Point(left, top);
            this.tbagedur.Width = 60;
            this.Controls.Add(this.tbagedur);
            top += rowHeight + spacing;

            // Species
            MakeLabel("Species:", top);
            this.cbspecies.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cbspecies.Location = new Point(left, top);
            this.cbspecies.Width = 140;
            this.Controls.Add(this.cbspecies);
            top += rowHeight + spacing;

            // Aspiration
            MakeLabel("Aspiration:", top);
            this.cbaspiration.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cbaspiration.Location = new Point(left, top);
            this.cbaspiration.Width = 180;
            this.Controls.Add(this.cbaspiration);
            top += rowHeight + spacing + 4;

            // Commit button
            this.btcommit.Text = "Commit";
            this.btcommit.Location = new Point(left, top);
            this.btcommit.Width = 100;
            this.Controls.Add(this.btcommit);
        }
    }
}
