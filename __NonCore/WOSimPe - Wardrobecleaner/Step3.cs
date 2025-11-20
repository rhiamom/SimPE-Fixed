using System;
using System.Collections.Generic;
using System.Text;
using SimPe.Wizards;
using SimPe.Interfaces.Files;

namespace SimPe.Plugin
{

    public abstract class StepBase : AWizardForm
    {
        public WizardController Controller
        {
            get { return WizardController.Instance; }
        }

        public IPackageFile NeighborhoodPackage
        {
            get { return this.Controller.NeighborhoodPackage; }
            set { this.Controller.NeighborhoodPackage = value; }
        }

        public List<uint> FamilyInstances
        {
            get { return this.Controller.FamilyInstances; }
        }

        public StepBase()
        {
        }

    }

}
