using System;
using System.Collections.Generic;
using System.Text;
using SimPe.Wizards;
using SimPe.Plugin.UI;

namespace SimPe.Plugin
{
    public class Step1 : StepBase, IWizardEntry
    {
        NeighborhoodBrowser form;

        #region IWizardEntry Members

        public string WizardCaption
        {
            get { return "Wardrobe Cleaner"; }
        }

        public string WizardDescription
        {
            get { return "Deletes clothing entries from households in a neighbourhood."; }
        }

        public System.Drawing.Image WizardImage
        {
            get { return global::SimPe.Wizards.Properties.Resources.WizardIcon; }
            // get { return SetKeyName(0, "WizardIcon.png"); }
        }

        private System.Drawing.Image SetKeyName(int p, string p_2)
        {
           throw new Exception("The method or operation is not implemented.");
        }

        #endregion

        public override bool CanContinue
        {
            get
            {
                if (this.NeighborhoodPackage != null)
                    return true;
                return false;
            }
        }

        protected override bool Init()
        {
            this.NeighborhoodPackage = null;

            if (this.form != null)
            {
                this.form.UpdateList();
                this.form.PackageChanged += delegate(object sender, EventArgs e)
                {
                    this.NeighborhoodPackage = this.form.NeighborhoodPackage;
                    this.FamilyInstances.Clear();
                    this.Update();
                };
            }
            return true;
        }

        public override IWizardForm Next
        {
            get
            {
                if (!this.CanContinue)
                    return null;
                return new Step2();
            }
        }

        public override string WizardMessage
        {
            get { return "Select Neighbourhood"; }
        }

        public override int WizardStep
        {
            get { return 2; }
        }

        public override System.Windows.Forms.Panel WizardWindow
        {
            get
            {
                if (this.form == null)
                    this.form = new NeighborhoodBrowser();
                return this.form;
            }
        }
    }
}
