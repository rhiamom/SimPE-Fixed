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
    /// Summary description for Step2.
	/// </summary>
	public class Step2 : AWizardForm
	{
        public Step2()
		{
			
		}

		#region IWizardForm Member

		public override Avalonia.Controls.Panel WizardWindow
		{
			get
            {
                Step1.Form.lbDone.IsVisible = false;
                Step1.Form.linkLabel1.IsVisible = true;
                Step1.Form.lvpackages.IsEnabled = Step1.Form.cbShapes.IsEnabled = true;
                return null; // TODO: return Avalonia Panel
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
