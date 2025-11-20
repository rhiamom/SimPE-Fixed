namespace SimPe.Interfaces
{
    partial class AFinderTool
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
            if (tm != null) tm.Clear();
            tm= null;
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AFinderTool));
            this.grp = new booby.TaskBox();
            this.btStart = new System.Windows.Forms.Button();
            this.content = new System.Windows.Forms.Panel();
            this.grp.SuspendLayout();
            this.SuspendLayout();
            // 
            // grp
            // 
            this.grp.BackColor = System.Drawing.Color.Transparent;
            this.grp.BodyColor = System.Drawing.SystemColors.ControlLight;
            this.grp.BorderColor = System.Drawing.SystemColors.ControlDarkDark;
            this.grp.Controls.Add(this.btStart);
            this.grp.Controls.Add(this.content);
            resources.ApplyResources(this.grp, "grp");
            this.grp.HeaderFont = new System.Drawing.Font("Microsoft Sans Serif", 10.25F, System.Drawing.FontStyle.Bold);
            this.grp.HeaderTextColor = System.Drawing.SystemColors.ControlText;
            this.grp.IconLocation = new System.Drawing.Point(4, 0);
            this.grp.IconSize = new System.Drawing.Size(32, 32);
            this.grp.LeftHeaderColor = System.Drawing.SystemColors.ControlDark;
            this.grp.Name = "grp";
            this.grp.RightHeaderColor = System.Drawing.SystemColors.ControlDark;
            this.grp.TopGap = 6;
            // 
            // btStart
            // 
            resources.ApplyResources(this.btStart, "btStart");
            this.btStart.Name = "btStart";
            this.btStart.Click += new System.EventHandler(this.btStart_Click);
            // 
            // content
            // 
            resources.ApplyResources(this.content, "content");
            this.content.Name = "content";
            // 
            // AFinderTool
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.grp);
            this.Name = "AFinderTool";
            this.grp.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btStart;
        protected System.Windows.Forms.Panel content;
        private booby.TaskBox grp;
    }
}
