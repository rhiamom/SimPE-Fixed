using System;
using System.Collections.Generic;
using System.Text;
using SimPe.Wizards;
using SimPe.Plugin.UI;
using SimPe.Interfaces.Files;
using System.Windows.Forms;

namespace SimPe.Plugin
{
    public class Step2 : StepBase, IWizardFinish
    {
        HouseholdBrowser form;

        public override bool CanContinue
        {
            get
            {
                return (
              this.NeighborhoodPackage != null &&
              this.FamilyInstances.Count > 0
              );
            }
        }

        protected override bool Init()
        {
            if (this.form != null)
            {
                this.form.NeighborhoodPackage = this.NeighborhoodPackage;
                this.form.HouseholdSelectionChanged += delegate(object sender, EventArgs e)
                {
                    this.FamilyInstances.Clear();
                    this.FamilyInstances.AddRange(this.form.GetCheckedFamilyInstances());
                    this.Update();
                };
            }
            return true;
        }

        public override IWizardForm Next
        {
            get { return null; }
        }

        public override string WizardMessage
        {
            get { return "Choose Households to clean"; }
        }

        public override int WizardStep
        {
            get { return 3; }
        }

        public override System.Windows.Forms.Panel WizardWindow
        {
            get
            {
                if (this.form == null)
                    this.form = new HouseholdBrowser();
                return this.form;
            }
        }

        #region IWizardFinish Members

        public void Finit()
        {
            WaitingScreen.Wait();
            try
            {
                this.Controller.DeleteClothingEntries();
                this.Controller.CommitChanges();
                MessageBox.Show("The changes have been saved.");
            }
            catch (Exception x)
            {
                MessageBox.Show(x.ToString());
            }
            WaitingScreen.Stop();


        }

        #endregion



    }
}
