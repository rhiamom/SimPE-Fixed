/**************************************************************************
 *   Copyright (C) 2023 by Chris Hatch                                    *
 *   (original author, BSOK Wizard)                                       *
 *                                                                         *
 *   Copyright (C) 2025 by GramzeSweatShop                                *
 *   rhiamom@mac.com                                                       *
 *                                                                         *
 *   This program is free software; you can redistribute it and/or modify  *
 *   it under the terms of the GNU General Public License as published by  *
 *   the Free Software Foundation; either version 2 of the License, or     *
 *   (at your option) any later version.                                   *
 *                                                                         *
 *   This program is distributed in the hope that it will be useful,       *
 *   but WITHOUT ANY WARRANTY; without even the implied warranty of        *
 *   MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the         *
 *   GNU General Public License for more details.                          *
 *                                                                         *
 *   You should have received a copy of the GNU General Public License     *
 *   along with this program; if not, write to the                         *
 *   Free Software Foundation, Inc.,                                       *
 *   59 Temple Place - Suite 330, Boston, MA  02111-1307, USA.             *
 **************************************************************************/
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
