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
	/// Summary description for AWizardForm.
	/// </summary>
	public abstract class AWizardForm : IWizardForm
	{
		/// <summary>
		/// The Update delegate
		/// </summary>
		protected SimPe.Wizards.ChangedContent update;
		

		/// <summary>
		/// Triggers the Update Delegate
		/// </summary>
		public void Update()
		{
			Update(false);
		}

		/// <summary>
		/// Triggers the Update Delegate
		/// </summary>
		public void Update(bool autonext)
		{
			if (update!=null) update(this, autonext);
		}

		/// <summary>
		/// Called in the normal Init method
		/// </summary>
		protected abstract bool Init();
		#region IWizardForm Member

		public abstract Avalonia.Controls.Panel WizardWindow
		{
			get;
		}

		public abstract string WizardMessage
		{
			get;
		}

		public abstract int WizardStep
		{
			get;
		}

		public bool Init(SimPe.Wizards.ChangedContent fkt)
		{
			update = fkt;
			return Init();
		}

		public abstract IWizardForm Next
		{
			get;
		}

		public abstract bool CanContinue
		{
			get;
		}

		#endregion
	}
}
