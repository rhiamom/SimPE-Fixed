namespace SimPe.Plugin.Tool.Dockable.Finder
{
    partial class FindInSG
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FindInSG));
            this.label3 = new System.Windows.Forms.Label();
            this.cbtypes = new System.Windows.Forms.ComboBox();
            this.content.SuspendLayout();
            this.SuspendLayout();
            // 
            // content
            // 
            this.content.Controls.Add(this.label3);
            this.content.Controls.Add(this.cbtypes);
            resources.ApplyResources(this.content, "content");
            this.content.Controls.SetChildIndex(this.cbtypes, 0);
            this.content.Controls.SetChildIndex(this.label3, 0);
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // cbtypes
            // 
            resources.ApplyResources(this.cbtypes, "cbtypes");
            this.cbtypes.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbtypes.FormattingEnabled = true;
            this.cbtypes.Name = "cbtypes";
            // 
            // FindInSG
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Name = "FindInSG";
            this.content.ResumeLayout(false);
            this.content.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox cbtypes;
    }
}
