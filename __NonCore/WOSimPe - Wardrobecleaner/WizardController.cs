using System;
using System.Collections.Generic;
using System.Text;
using SimPe.Interfaces.Files;
using SimPe.Interfaces;
using SimPe.PackedFiles;

namespace SimPe.Plugin
{

	public sealed class WizardController
	{
		IProviderRegistry prov;
		IPackageFile package;
		List<uint> familyInstances;
		List<RefFile> refList;

		public IProviderRegistry ProviderRegistry
		{
			get { return prov; }
		}

		public List<uint> FamilyInstances
		{
			get
			{
				if (this.familyInstances == null)
					this.familyInstances = new List<uint>();
				return familyInstances;
			}
		}

		public IPackageFile NeighborhoodPackage
		{
			get { return package; }
			set
			{
				package = value;
				this.FamilyInstances.Clear();
				if (this.refList != null)
					this.refList.Clear();
				this.refList = null;
			}
		}

		WizardController()
		{
			this.prov = new TypeRegistry();
		}


		public List<RefFile> BuildWardrobes(List<uint> familyInstances)
		{
			List<RefFile> ret = new List<RefFile>();

			if (this.refList == null)
			{
				this.refList = new List<RefFile>();

				/*
				 * Find all 3IDR resources
				 */
				IPackedFileDescriptor[] pfds = this.package.FindFiles(SimPe.Data.MetaData.REF_FILE);
				foreach (IPackedFileDescriptor pfd in pfds)
				{
					RefFile idr = new RefFile();
                    try
                    {
                        idr.ProcessData(pfd, package, false);
                        refList.Add(idr);
                    }
                    catch // if it's corrupt then lets lose it
                    {
                        pfd.MarkForDelete = true;
                        IPackedFileDescriptor binx = this.package.FindFile(0x0C560F39u, pfd.SubType, pfd.Group, pfd.Instance);
                        if (binx != null)
                            binx.MarkForDelete = true;
                    }
				}

			}


			/*
			 * Find collection/family instances
			 */
			foreach (RefFile idr in refList)
			{
				// reserved?
				if (idr.FileDescriptor.Instance < 0x7fff)
					continue;

				// poo
				if (idr.Items.Length != 3)
					continue;

				IPackedFileDescriptor pfdColl = idr.Items[1];
				if (
					pfdColl.Type == 0x6C4F359Du // collection?
			&& familyInstances.Contains(pfdColl.Instance) // match family?
					)
					ret.Add(idr);

			} //foreach

			return ret;
		}

		public void DeleteClothingEntries()
		{

			if (this.FamilyInstances.Count > 0)
			{
				List<RefFile> refList = this.BuildWardrobes(this.familyInstances);

				foreach (RefFile idr in refList)
				{
					IPackedFileDescriptor pfd = idr.FileDescriptor;
					pfd.MarkForDelete = true;

					/*
					 * Don't forget the BINX too!
					 */
					IPackedFileDescriptor binx = this.package.FindFile(0x0C560F39u, pfd.SubType, pfd.Group, pfd.Instance);
					if (binx != null)
						binx.MarkForDelete = true;
				}

			}

		}

		public void CommitChanges()
		{
			if (this.NeighborhoodPackage != null)
			{
				this.NeighborhoodPackage.Save();
				this.NeighborhoodPackage.Close();
			}

		}

		#region Singleton

		static WizardController instance = new WizardController();

		public static WizardController Instance
		{
			get { return WizardController.instance; }
		}

		#endregion

	}
}
