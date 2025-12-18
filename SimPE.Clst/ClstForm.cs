/***************************************************************************
 *   Copyright (C) 2005 by Peter L Jones                                   *
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
using SimPe.Interfaces.Plugin;
using SimPe.PackedFiles.Wrapper;

namespace SimPe.PackedFiles.UserInterface
{
    /// <summary>
    /// Summary description for BconForm.
    /// </summary>
    public class ClstForm : Form, IPackedFileUI
    {
        #region Form elements
        private Label lbformat;
        private Label label9;
        private ListBox lbclst;

        // Replacements for Chris Hatch.panelheader and Chris Hatch.gradientpanel
        private Panel panel4;         
        private Panel clstPanel;      

        private System.ComponentModel.Container components = null;
        #endregion

        public ClstForm()
        {
            InitializeComponent();

            // No ThemeManager here anymore
            if (Helper.WindowsRegistry.UseBigIcons)
                this.lbclst.Font = new System.Drawing.Font("Verdana", 11F);
        }

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                    components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region CompressedFileList
        private CompressedFileList wrapper;
        #endregion

        #region IPackedFileUI Member

        /// <summary>
        /// Returns the Panel that will be displayed within SimPe
        /// </summary>
        public Control GUIHandle
        {
            get { return clstPanel; }
        }

        /// <summary>
        /// Updates the GUI from the wrapper
        /// </summary>
        public void UpdateGUI(IFileWrapper wrp)
        {
            wrapper = (CompressedFileList)wrp;

            lbformat.Text = wrapper.IndexType.ToString();

            lbclst.Items.Clear();
            foreach (ClstItem i in wrapper.Items)
            {
                if (i != null) lbclst.Items.Add(i);
                else lbclst.Items.Add("Error");
            }
        }

        #endregion

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.clstPanel = new Panel();
            this.lbformat = new Label();
            this.label9 = new Label();
            this.lbclst = new ListBox();
            this.panel4 = new Panel();

            this.SuspendLayout();

            // clstPanel (replaces Chris Hatch.gradientpanel)
            this.clstPanel.BackColor = System.Drawing.Color.Transparent;
            this.clstPanel.Controls.Add(this.lbformat);
            this.clstPanel.Controls.Add(this.label9);
            this.clstPanel.Controls.Add(this.lbclst);
            this.clstPanel.Controls.Add(this.panel4);
            this.clstPanel.Dock = DockStyle.Fill;
            this.clstPanel.Name = "clstPanel";

            // lbformat
            this.lbformat.BackColor = System.Drawing.Color.Transparent;
            this.lbformat.Location = new System.Drawing.Point(10, 10);
            this.lbformat.Name = "lbformat";
            this.lbformat.Text = "";

            // label9
            this.label9.BackColor = System.Drawing.Color.Transparent;
            this.label9.Location = new System.Drawing.Point(10, 30);
            this.label9.Name = "label9";
            this.label9.Text = "Format:";

            // lbclst
            this.lbclst.Location = new System.Drawing.Point(10, 50);
            this.lbclst.Name = "lbclst";
            this.lbclst.Sorted = true;
            this.lbclst.Size = new System.Drawing.Size(300, 300);

            // panel4 (replaces Chris Hatch.panelheader)
            this.panel4.BackColor = System.Drawing.SystemColors.ControlDark;
            this.panel4.Dock = DockStyle.Top;
            this.panel4.Height = 30;
            this.panel4.Name = "panel4";

            // ClstForm
            this.Controls.Add(this.clstPanel);
            this.Name = "ClstForm";
            this.Text = "Compressed File List";

            this.ResumeLayout(false);
        }

        #endregion
    }
}
