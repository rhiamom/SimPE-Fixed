using System;
using System.Drawing;
using SimPe.Interfaces.Plugin;

namespace SimPe.Plugin
{
    public partial class AudioRefPackedFileUI : SimPe.Windows.Forms.WrapperBaseControl, IPackedFileUI
    {
        protected new AudioRefPackedFileWrapper Wrapper
        {
            get { return base.Wrapper as AudioRefPackedFileWrapper; }
        }
        public AudioRefPackedFileWrapper TPFW
        {
            get { return (AudioRefPackedFileWrapper)Wrapper; }
        }

        #region WrapperBaseControl Member

        public AudioRefPackedFileUI()
        {
            InitializeComponent();
            if (booby.ThemeManager.ThemedForms)
            {
                booby.ThemeManager.Global.AddControl(this.TBsting);
                if (booby.ThemeManager.savedTheme == 4 || booby.ThemeManager.savedTheme == 7)
                {
                    this.TBsting.Font = new System.Drawing.Font("Comic Sans MS", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                    this.lbnote.Font = new System.Drawing.Font("Comic Sans MS", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                }
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

            this.TBsting.Text = Wrapper.Strung;
        }

        public override void OnCommit()
        {
            base.OnCommit();
            TPFW.SynchronizeUserData(true, false);
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

        private void TBsting_TextChanged(object sender, EventArgs e)
        {
            Wrapper.Strung = TBsting.Text;
        }
    }
}
