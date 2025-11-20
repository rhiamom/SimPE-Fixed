using System;
using System.Drawing;
using SimPe.Interfaces.Plugin;

namespace SimPe.Plugin
{
    public partial class SimmyListPackedFileUI : SimPe.Windows.Forms.WrapperBaseControl, IPackedFileUI
    {
        protected new SimmyListPackedFileWrapper Wrapper
        {
            get { return base.Wrapper as SimmyListPackedFileWrapper; }
        }
        public SimmyListPackedFileWrapper TPFW
        {
            get { return (SimmyListPackedFileWrapper)Wrapper; }
        }

        #region WrapperBaseControl Member

        public SimmyListPackedFileUI()
        {
            InitializeComponent();
            if (booby.ThemeManager.ThemedForms)
            {
                booby.ThemeManager.Global.AddControl(this.TBsting);
                if (booby.ThemeManager.savedTheme == 4 || booby.ThemeManager.savedTheme == 7)
                    this.TBsting.Font = new System.Drawing.Font("Comic Sans MS", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            }
            if (booby.PrettyGirls.PervyMode)
            {
                label2.Visible = true;
                this.HeaderText = "Boobies";
                if (!booby.Infos.IsFontinstalled("Blackadder ITC"))
                    label2.Font = new System.Drawing.Font("Comic Sans MS", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            }
        }

        protected override void RefreshGUI()
        {
            base.RefreshGUI();
            if (booby.ThemeManager.savedTheme == 8) this.BackgroundImage = booby.PrettyGirls.HippyGirl;
            else this.BackgroundImage = booby.PrettyGirls.RandomGirl;

            this.checkBox1.Checked = false;
            this.TBsting.Text = Wrapper.Strung;
        }

        public override void OnCommit()
        {
        }
        #endregion

        #region IPackedFileUI Member
        System.Windows.Forms.Control IPackedFileUI.GUIHandle
        {
            get { return this; }
        }
        #endregion

        #region IDisposable Member

        void IDisposable.Dispose()
        {
            this.TPFW.Dispose();
        }
        #endregion

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked == true) this.TBsting.Text = Wrapper.Twine;
            else this.TBsting.Text = Wrapper.Strung;
            this.TBsting.Refresh();
        }
    }
}
