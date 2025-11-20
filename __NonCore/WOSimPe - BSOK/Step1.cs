using System;

namespace SimPe.Wizards
{
	/// <summary>
    /// Summary description for Step3.
    /// </summary>
    public class Step1 : AWizardForm, IWizardEntry
	{
		static BsokWizardForm bwf;

		/// <summary>
		/// Returns the Main Form
		/// </summary>
		public static BsokWizardForm Form
		{
			get { 
				if (bwf==null) bwf = new BsokWizardForm();
				return bwf; 
			}
		}

		public Step1()
		{
			
		}

		#region IWizardEntry Member

		public string WizardDescription
		{
			get
			{
				return "Organize groups of your Sims2 outfits by Body Shape";
			}
		}

		public string WizardCaption
		{
			get
			{
				return "Bodyshape Organization Kit";
			}
		}

		public System.Drawing.Image WizardImage
		{
			get
			{
                return global::SimPe.Wizards.Properties.Resources.WizardIcon;
			}
		}

		#endregion

		#region IWizardForm Member

        public override System.Windows.Forms.Panel WizardWindow
		{
			get
			{
                Form.rtbAbout.Visible = false;
				return Form.pnwizard1;
			}
		}

		protected override bool Init()
        {
            if (Form.step1 == null) Form.step1 = this;
			return true;
		}

        public override string WizardMessage
        {
            get
            {
                return "Select a Folder of outfits that you want to configure";
            }
        }
		

		public override bool CanContinue
		{
			get
			{
                if (Form.floder != null)
                    return true;
                return false;
			}
		}

        public override int WizardStep
		{
			get
			{
				return 2;
			}
		}
        
		public override IWizardForm Next
		{
			get
			{
				if (Form.step2==null) Form.step2 = new Step2();
				return Form.step2;
			}
		}

		#endregion
	}
}
