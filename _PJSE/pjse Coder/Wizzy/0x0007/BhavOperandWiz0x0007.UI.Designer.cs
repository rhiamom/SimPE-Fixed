/*
 * SimpePrimitiveWizards - additional primitive wizards for SimPe
 *                       - see https://www.picknmixmods.com/Sims2/Notes/SimpePrimitiveWizards/SimpePrimitiveWizards.html
 *
 * William Howard - 2023-2023
 *
 * Permission granted to use this code in any way, except to claim it as your own or sell it
 *
 * NOTE: Code should not be "using Simpe;" or "using pjse;" but fully qualifying classes in those high level namespaces
 *
 */

namespace whse.PrimitiveWizards.Wiz0x0007
{
    partial class UI
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            panelMain = new System.Windows.Forms.Panel();
            comboWhat = new System.Windows.Forms.ComboBox();
            lblWhat = new System.Windows.Forms.Label();
            iconPnM = new System.Windows.Forms.PictureBox();
            comboWho = new System.Windows.Forms.ComboBox();
            lblWho = new System.Windows.Forms.Label();
            toolTip = new System.Windows.Forms.ToolTip(components);
            panelMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)iconPnM).BeginInit();
            SuspendLayout();
            // 
            // panelMain
            // 
            panelMain.Controls.Add(comboWhat);
            panelMain.Controls.Add(lblWhat);
            panelMain.Controls.Add(iconPnM);
            panelMain.Controls.Add(comboWho);
            panelMain.Controls.Add(lblWho);
            panelMain.Location = new System.Drawing.Point(0, 0);
            panelMain.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            panelMain.Name = "panelMain";
            panelMain.Size = new System.Drawing.Size(350, 81);
            panelMain.TabIndex = 0;
            // 
            // comboWhat
            // 
            comboWhat.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            comboWhat.FormattingEnabled = true;
            comboWhat.Items.AddRange(new object[] { "Graphic", "Lighting Contribution", "Room Score Contribution" });
            comboWhat.Location = new System.Drawing.Point(117, 36);
            comboWhat.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            comboWhat.Name = "comboWhat";
            comboWhat.Size = new System.Drawing.Size(174, 23);
            comboWhat.TabIndex = 1;
            // 
            // lblWhat
            // 
            lblWhat.Location = new System.Drawing.Point(6, 39);
            lblWhat.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            lblWhat.Name = "lblWhat";
            lblWhat.Size = new System.Drawing.Size(111, 15);
            lblWhat.TabIndex = 18;
            lblWhat.Text = "What:";
            lblWhat.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // iconPnM
            // 
            iconPnM.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
            iconPnM.Image = pjse.Properties.Resources.MinionWithNotebook;
            iconPnM.Location = new System.Drawing.Point(309, 40);
            iconPnM.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            iconPnM.Name = "iconPnM";
            iconPnM.Size = new System.Drawing.Size(37, 37);
            iconPnM.TabIndex = 17;
            iconPnM.TabStop = false;
            toolTip.SetToolTip(iconPnM, "Primitive wizard by Pick'n'Mix (whoward69)\r\nhttps://www.picknmixmods.com/Sims2/");
            // 
            // comboWho
            // 
            comboWho.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            comboWho.FormattingEnabled = true;
            comboWho.Items.AddRange(new object[] { "My", "Stack Object's" });
            comboWho.Location = new System.Drawing.Point(117, 5);
            comboWho.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            comboWho.Name = "comboWho";
            comboWho.Size = new System.Drawing.Size(174, 23);
            comboWho.TabIndex = 0;
            // 
            // lblWho
            // 
            lblWho.Location = new System.Drawing.Point(6, 8);
            lblWho.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            lblWho.Name = "lblWho";
            lblWho.Size = new System.Drawing.Size(111, 15);
            lblWho.TabIndex = 2;
            lblWho.Text = "Refresh:";
            lblWho.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // UI
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            Controls.Add(panelMain);
            Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            Name = "UI";
            Size = new System.Drawing.Size(350, 81);
            panelMain.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)iconPnM).EndInit();
            ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelMain;
        private System.Windows.Forms.Label lblWho;
        private System.Windows.Forms.ComboBox comboWho;
        private System.Windows.Forms.PictureBox iconPnM;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.ComboBox comboWhat;
        private System.Windows.Forms.Label lblWhat;
    }
}
