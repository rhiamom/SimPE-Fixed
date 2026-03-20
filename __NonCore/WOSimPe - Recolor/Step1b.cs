/***************************************************************************
 *   Copyright (C) 2005 by Ambertation                                     *
 *   quaxi@ambertation.de                                                  *
 *                                                                         *
 *   Copyright (C) 2025 by GramzeSweatShop                                 *
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
 ***************************************************************************/

using System;

namespace SimPe.Wizards
{
	/// <summary>
	/// Zusammenfassung f�r Step1.
	/// </summary>
	public class Step1b : AWizardForm
	{
		

		public Step1b()
		{
			
		}

		#region IWizardForm Member

		public override Avalonia.Controls.Panel WizardWindow
		{
			get
			{
				return null; // TODO: replace WinForms pnwizard1b with Avalonia panel
			}
		}

		protected override bool Init()
        {
            if (Helper.XmlRegistry.UseBigIcons)
            SimPe.Plugin.SubsetSelectForm.ImageSize = new System.Drawing.Size(120, 120);
            else
            SimPe.Plugin.SubsetSelectForm.ImageSize = new System.Drawing.Size(60, 60);
			return Step1.Form.Recolor();
		}

		public override string WizardMessage
		{
			get
			{
				return "Select the Subsets you want to Recolour and the Basetexture for each Subset";
			}
		}

		public override bool CanContinue
		{
			get
			{
				if (Step1.Form.ssf==null) return false;
				return Step1.Form.ssf.button1.Enabled;
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
				if (Step1.Form.step2==null) Step1.Form.step2 = new Step2();
				return Step1.Form.step2;
			}
		}

		#endregion
	}
}
