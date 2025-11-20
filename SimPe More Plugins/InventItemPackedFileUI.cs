using System;
using SimPe.Interfaces.Plugin;

namespace SimPe.Plugin
{
	/// <summary>
	/// This class is used to fill the UI for this FileType with Data
	/// </summary>
    public partial class InventItemPackedFileUI : SimPe.Windows.Forms.WrapperBaseControl, IPackedFileUI
    {
        protected new InventItemPackedFileWrapper Wrapper
        {
            get { return base.Wrapper as InventItemPackedFileWrapper; }
        }
        public InventItemPackedFileWrapper TPFW
        {
            get { return (InventItemPackedFileWrapper)Wrapper; }
        }

        #region WrapperBaseControl Member

        public InventItemPackedFileUI()
        {
            InitializeComponent();
            if (booby.ThemeManager.ThemedForms)
            {
                if (booby.ThemeManager.savedTheme == 4 || booby.ThemeManager.savedTheme == 7)
                    this.lbdisp.Font = new System.Drawing.Font("Comic Sans MS", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            }
            if (booby.PrettyGirls.PervyMode)
            {
                this.HeaderText = "Boobies";
                this.label1.Visible = true;
                if (!booby.Infos.IsFontinstalled("Blackadder ITC"))
                    this.label1.Font = new System.Drawing.Font("Comic Sans MS", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            }
        }

        protected override void RefreshGUI()
        {
            base.RefreshGUI();
            this.lbdisp.Text = Wrapper.DispLabel;
            if (booby.ThemeManager.savedTheme == 8) this.BackgroundImage = booby.PrettyGirls.HippyGirl;
            else this.BackgroundImage = booby.PrettyGirls.RandomGirl;
        }

        public override void OnCommit()
        {
            // base.OnCommit();
            // TPFW.SynchronizeUserData(true, false);
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
    }
}
