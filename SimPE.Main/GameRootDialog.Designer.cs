namespace SimPe
{
    partial class GameRootDialog
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.rbLegacy = new System.Windows.Forms.RadioButton();
            this.rbUC = new System.Windows.Forms.RadioButton();
            this.rbSteam = new System.Windows.Forms.RadioButton();
            this.rbDisc = new System.Windows.Forms.RadioButton();
            this.rbCustom = new System.Windows.Forms.RadioButton();
            this.rbMac = new System.Windows.Forms.RadioButton();
            this.rbEpic = new System.Windows.Forms.RadioButton();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.txtGameRoot = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.btnBrowse = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.labelDownloads = new System.Windows.Forms.Label();
            this.txtDownloads = new System.Windows.Forms.TextBox();
            this.btnBrowseDownloads = new System.Windows.Forms.Button();
            this.lblCepStatus = new System.Windows.Forms.Label();
            this.txtCepStatus = new System.Windows.Forms.TextBox();
            this.btnDownloadCep = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(60, 19);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(216, 32);
            this.label1.TabIndex = 0;
            this.label1.Text = "Game Edition :";
            // 
            // rbLegacy
            // 
            this.rbLegacy.AutoSize = true;
            this.rbLegacy.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rbLegacy.Location = new System.Drawing.Point(39, 59);
            this.rbLegacy.Margin = new System.Windows.Forms.Padding(4);
            this.rbLegacy.Name = "rbLegacy";
            this.rbLegacy.Size = new System.Drawing.Size(107, 29);
            this.rbLegacy.TabIndex = 1;
            this.rbLegacy.TabStop = true;
            this.rbLegacy.Text = "Legacy";
            this.rbLegacy.UseVisualStyleBackColor = true;
            this.rbLegacy.CheckedChanged += new System.EventHandler(this.EditionRadio_CheckedChanged);
            // 
            // rbUC
            // 
            this.rbUC.AutoSize = true;
            this.rbUC.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rbUC.Location = new System.Drawing.Point(39, 96);
            this.rbUC.Margin = new System.Windows.Forms.Padding(4);
            this.rbUC.Name = "rbUC";
            this.rbUC.Size = new System.Drawing.Size(217, 29);
            this.rbUC.TabIndex = 2;
            this.rbUC.TabStop = true;
            this.rbUC.Text = "Ultimate Collection";
            this.rbUC.UseVisualStyleBackColor = true;
            this.rbUC.CheckedChanged += new System.EventHandler(this.EditionRadio_CheckedChanged);
            // 
            // rbSteam
            // 
            this.rbSteam.AutoSize = true;
            this.rbSteam.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rbSteam.Location = new System.Drawing.Point(39, 133);
            this.rbSteam.Margin = new System.Windows.Forms.Padding(4);
            this.rbSteam.Name = "rbSteam";
            this.rbSteam.Size = new System.Drawing.Size(99, 29);
            this.rbSteam.TabIndex = 3;
            this.rbSteam.TabStop = true;
            this.rbSteam.Text = "Steam";
            this.rbSteam.UseVisualStyleBackColor = true;
            this.rbSteam.CheckedChanged += new System.EventHandler(this.EditionRadio_CheckedChanged);
            // 
            // rbDisc
            // 
            this.rbDisc.AutoSize = true;
            this.rbDisc.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rbDisc.Location = new System.Drawing.Point(347, 59);
            this.rbDisc.Margin = new System.Windows.Forms.Padding(4);
            this.rbDisc.Name = "rbDisc";
            this.rbDisc.Size = new System.Drawing.Size(120, 29);
            this.rbDisc.TabIndex = 4;
            this.rbDisc.TabStop = true;
            this.rbDisc.Text = "CD/DVD";
            this.rbDisc.UseVisualStyleBackColor = true;
            this.rbDisc.CheckedChanged += new System.EventHandler(this.EditionRadio_CheckedChanged);
            // 
            // rbCustom
            // 
            this.rbCustom.AutoSize = true;
            this.rbCustom.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rbCustom.Location = new System.Drawing.Point(347, 96);
            this.rbCustom.Margin = new System.Windows.Forms.Padding(4);
            this.rbCustom.Name = "rbCustom";
            this.rbCustom.Size = new System.Drawing.Size(111, 29);
            this.rbCustom.TabIndex = 5;
            this.rbCustom.TabStop = true;
            this.rbCustom.Text = "Custom";
            this.rbCustom.UseVisualStyleBackColor = true;
            this.rbCustom.CheckedChanged += new System.EventHandler(this.EditionRadio_CheckedChanged);
            // 
            // rbMac
            // 
            this.rbMac.AutoSize = true;
            this.rbMac.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rbMac.Location = new System.Drawing.Point(347, 133);
            this.rbMac.Margin = new System.Windows.Forms.Padding(4);
            this.rbMac.Name = "rbMac";
            this.rbMac.Size = new System.Drawing.Size(222, 29);
            this.rbMac.TabIndex = 6;
            this.rbMac.TabStop = true;
            this.rbMac.Text = "Mac (coming soon)";
            this.rbMac.UseVisualStyleBackColor = true;
            this.rbMac.CheckedChanged += new System.EventHandler(this.EditionRadio_CheckedChanged);
            // 
            // rbEpic
            // 
            this.rbEpic.AutoSize = true;
            this.rbEpic.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rbEpic.Location = new System.Drawing.Point(39, 170);
            this.rbEpic.Margin = new System.Windows.Forms.Padding(4);
            this.rbEpic.Name = "rbEpic";
            this.rbEpic.Size = new System.Drawing.Size(79, 29);
            this.rbEpic.TabIndex = 7;
            this.rbEpic.TabStop = true;
            this.rbEpic.Text = "Epic";
            this.rbEpic.UseVisualStyleBackColor = true;
            this.rbEpic.CheckedChanged += new System.EventHandler(this.EditionRadio_CheckedChanged);
            // 
            // textBox1
            // 
            this.textBox1.BackColor = System.Drawing.SystemColors.Control;
            this.textBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox1.Location = new System.Drawing.Point(392, 19);
            this.textBox1.Margin = new System.Windows.Forms.Padding(4);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.ReadOnly = true;
            this.textBox1.Size = new System.Drawing.Size(538, 81);
            this.textBox1.TabIndex = 8;
            this.textBox1.Text = "Choose which version of The Sims 2 you play.\r\n Please choose Custom if you did no" +
    "t install the game into the installer\'s default path.\r\n";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.rbEpic);
            this.groupBox1.Controls.Add(this.rbMac);
            this.groupBox1.Controls.Add(this.rbCustom);
            this.groupBox1.Controls.Add(this.rbDisc);
            this.groupBox1.Controls.Add(this.rbSteam);
            this.groupBox1.Controls.Add(this.rbUC);
            this.groupBox1.Controls.Add(this.rbLegacy);
            this.groupBox1.Location = new System.Drawing.Point(28, 101);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox1.Size = new System.Drawing.Size(585, 219);
            this.groupBox1.TabIndex = 10;
            this.groupBox1.TabStop = false;
            // 
            // txtGameRoot
            // 
            this.txtGameRoot.Location = new System.Drawing.Point(234, 328);
            this.txtGameRoot.Margin = new System.Windows.Forms.Padding(4);
            this.txtGameRoot.Name = "txtGameRoot";
            this.txtGameRoot.Size = new System.Drawing.Size(601, 30);
            this.txtGameRoot.TabIndex = 8;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(23, 331);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(203, 25);
            this.label2.TabIndex = 11;
            this.label2.Text = "Sims 2 folder location:";
            // 
            // btnBrowse
            // 
            this.btnBrowse.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnBrowse.Location = new System.Drawing.Point(834, 327);
            this.btnBrowse.Margin = new System.Windows.Forms.Padding(4);
            this.btnBrowse.Name = "btnBrowse";
            this.btnBrowse.Size = new System.Drawing.Size(96, 35);
            this.btnBrowse.TabIndex = 12;
            this.btnBrowse.Text = "Browse...";
            this.btnBrowse.UseVisualStyleBackColor = true;
            this.btnBrowse.Click += new System.EventHandler(this.btnBrowse_Click);
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnOK.Location = new System.Drawing.Point(819, 561);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(111, 44);
            this.btnOK.TabIndex = 13;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // labelDownloads
            // 
            this.labelDownloads.AutoSize = true;
            this.labelDownloads.Location = new System.Drawing.Point(23, 383);
            this.labelDownloads.Name = "labelDownloads";
            this.labelDownloads.Size = new System.Drawing.Size(233, 25);
            this.labelDownloads.TabIndex = 14;
            this.labelDownloads.Text = "Sims 2 Downloads folder:";
            // 
            // txtDownloads
            // 
            this.txtDownloads.Location = new System.Drawing.Point(256, 383);
            this.txtDownloads.Name = "txtDownloads";
            this.txtDownloads.Size = new System.Drawing.Size(579, 30);
            this.txtDownloads.TabIndex = 15;
            // 
            // btnBrowseDownloads
            // 
            this.btnBrowseDownloads.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnBrowseDownloads.Location = new System.Drawing.Point(834, 378);
            this.btnBrowseDownloads.Name = "btnBrowseDownloads";
            this.btnBrowseDownloads.Size = new System.Drawing.Size(96, 35);
            this.btnBrowseDownloads.TabIndex = 16;
            this.btnBrowseDownloads.Text = "Browse...";
            this.btnBrowseDownloads.UseVisualStyleBackColor = true;
            this.btnBrowseDownloads.Click += new System.EventHandler(this.btnBrowseDownloads_Click);
            // 
            // lblCepStatus
            // 
            this.lblCepStatus.Location = new System.Drawing.Point(23, 441);
            this.lblCepStatus.Name = "lblCepStatus";
            this.lblCepStatus.Size = new System.Drawing.Size(177, 27);
            this.lblCepStatus.TabIndex = 17;
            this.lblCepStatus.Text = "CEP Status:";
            // 
            // txtCepStatus
            // 
            this.txtCepStatus.Location = new System.Drawing.Point(159, 441);
            this.txtCepStatus.Multiline = true;
            this.txtCepStatus.Name = "txtCepStatus";
            this.txtCepStatus.Size = new System.Drawing.Size(676, 89);
            this.txtCepStatus.TabIndex = 18;
            this.txtCepStatus.Text = "(not checked yet)";
            // 
            // btnDownloadCep
            // 
            this.btnDownloadCep.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDownloadCep.Location = new System.Drawing.Point(28, 562);
            this.btnDownloadCep.Name = "btnDownloadCep";
            this.btnDownloadCep.Size = new System.Drawing.Size(204, 44);
            this.btnDownloadCep.TabIndex = 19;
            this.btnDownloadCep.Text = "Download CEP";
            this.btnDownloadCep.UseVisualStyleBackColor = true;
            this.btnDownloadCep.Click += new System.EventHandler(this.btnDownloadCep_Click);
            // 
            // GameRootDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(953, 630);
            this.Controls.Add(this.btnDownloadCep);
            this.Controls.Add(this.txtCepStatus);
            this.Controls.Add(this.lblCepStatus);
            this.Controls.Add(this.btnBrowseDownloads);
            this.Controls.Add(this.txtDownloads);
            this.Controls.Add(this.labelDownloads);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.btnBrowse);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtGameRoot);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.label1);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "GameRootDialog";
            this.Text = "GameRootDialog";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.RadioButton rbLegacy;
        private System.Windows.Forms.RadioButton rbUC;
        private System.Windows.Forms.RadioButton rbSteam;
        private System.Windows.Forms.RadioButton rbDisc;
        private System.Windows.Forms.RadioButton rbCustom;
        private System.Windows.Forms.RadioButton rbMac;
        private System.Windows.Forms.RadioButton rbEpic;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox txtGameRoot;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnBrowse;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Label labelDownloads;
        private System.Windows.Forms.TextBox txtDownloads;
        private System.Windows.Forms.Button btnBrowseDownloads;
        private System.Windows.Forms.Label lblCepStatus;
        private System.Windows.Forms.TextBox txtCepStatus;
        private System.Windows.Forms.Button btnDownloadCep;
    }
}