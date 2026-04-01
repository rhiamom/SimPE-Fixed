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
using System.Collections.Generic;
using System.Text;
using SimPe.Wizards;
using SimPe.Plugin.UI;
using SimPe.Interfaces.Files;
using SimPe.Scenegraph.Compat;

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

        public override Avalonia.Controls.Panel WizardWindow
        {
            get { return null; /* TODO: replace HouseholdBrowser with Avalonia panel */ }
        }

        #region IWizardFinish Members

        public void Finit()
        {
            WaitingScreen.Wait();
            try
            {
                this.Controller.DeleteClothingEntries();
                this.Controller.CommitChanges();
                SimPe.Scenegraph.Compat.MessageBox.ShowAsync("The changes have been saved.").GetAwaiter().GetResult();
            }
            catch (Exception x)
            {
                SimPe.Scenegraph.Compat.MessageBox.ShowAsync(x.ToString()).GetAwaiter().GetResult();
            }
            WaitingScreen.Stop();


        }

        #endregion



    }
}
