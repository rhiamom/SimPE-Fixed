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
