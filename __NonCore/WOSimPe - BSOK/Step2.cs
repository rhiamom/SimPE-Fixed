using System;

namespace SimPe.Wizards
{
	/// <summary>
    /// Summary description for Step2.
	/// </summary>
	public class Step2 : AWizardForm
	{
        public Step2()
		{
			
		}

		#region IWizardForm Member

		public override System.Windows.Forms.Panel WizardWindow
		{
			get
            {
                Step1.Form.lbDone.Visible = false;
                Step1.Form.linkLabel1.Visible = true;
                Step1.Form.lvpackages.Enabled = Step1.Form.cbShapes.Enabled = true;
                return Step1.Form.pnwizard2;
			}
        }

		protected override bool Init()
        {
            return true;
		}

		public override string WizardMessage
		{
			get
			{
				return "Select a Body Shape to BSOK the outfits to";
			}
		}

		public override bool CanContinue
		{
			get
			{
                if (Step1.Form.cbShapes.SelectedIndex < 0) return false;
				return true;
			}
		}

		public override int WizardStep
		{
			get
			{
				return 3;
			}
		}

		public override IWizardForm Next
        {
            get
            {
                if (Step1.Form.step3 == null) Step1.Form.step3 = new Step3();
                return Step1.Form.step3;
            }
		}

		#endregion
	}
}
