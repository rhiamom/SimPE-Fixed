using System;

namespace SimPe.Wizards
{
	/// <summary>
    /// Summary description for Step1.
	/// </summary>
	public class Step3 :IWizardFinish
	{
		public Step3()
		{
			
		}
		#region IWizardFinish Member
		public void Finit()
        {
		}
		#endregion

		#region IWizardForm Member

		public System.Windows.Forms.Panel WizardWindow
		{
			get
            {
                Step1.Form.DoTheWork();
				return Step1.Form.pnwizard2;
			}
		}

		public bool Init(SimPe.Wizards.ChangedContent fkt)
		{
			return true;
		}

		public string WizardMessage
		{
			get
			{
				return "All Done !";
			}
		}

		public bool CanContinue
		{
			get
			{
				return true;
			}
		}

		public int WizardStep
		{
			get
			{
				return 4;
			}
		}

		public IWizardForm Next
		{
			get
            {
				return null;
			}
		}

		#endregion
	}
}
