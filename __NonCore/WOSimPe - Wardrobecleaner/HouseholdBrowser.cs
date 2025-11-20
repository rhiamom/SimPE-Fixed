using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using SimPe.Interfaces.Files;
using SimPe.Interfaces;
using SimPe.PackedFiles.Wrapper;
using SimPe.PackedFiles;

namespace SimPe.Plugin.UI
{

    public partial class HouseholdBrowser : Panel // UserControl
	{
		IPackageFile package;
		internal bool suppressEvents;

		public IPackageFile NeighborhoodPackage
		{
			get { return package; }
			set
			{
				package = value;
				PopulateHouseholdList();
			}
		}

		public event EventHandler HouseholdSelectionChanged;

		public HouseholdBrowser()
		{
			InitializeComponent();
            if (Helper.WindowsRegistry.UseBigIcons)
            {
                this.label1.Size = new System.Drawing.Size(256, 18);
                this.label2.Size = new System.Drawing.Size(283, 18);
                this.cbCheckAll.Location = new System.Drawing.Point(5, 344);
                this.splitContainer1.SplitterDistance = 286;
                this.columnHeader2.Width = 100;
            }
		}

		protected virtual void OnHouseholdSelectionChanged(EventArgs e)
		{
			if (this.HouseholdSelectionChanged != null)
				this.HouseholdSelectionChanged(this, e);
		}


		void PopulateHouseholdList()
		{
			this.lvFam.Items.Clear();

			if (this.package != null)
			{
				IPackedFileDescriptor[] pfds = this.package.FindFiles(0x46414D49u); // FAMI
				foreach (IPackedFileDescriptor pfd in pfds)
				{
					Fami fam = new Fami(WizardController.Instance.ProviderRegistry.SimNameProvider);
					fam.ProcessData(pfd, package, false);

					ListViewItem li = new ListViewItem();
					li.ImageIndex = 0;
					li.Text = fam.Name;
					li.SubItems.Add(String.Format("0x{0:X4}", fam.FileDescriptor.Instance));

					li.Tag = fam;

					this.lvFam.Items.Add(li);
				}

			}
		}


		void PopulateItemList()
		{
			this.SuspendLayout();
			this.lvItems.BeginUpdate();

			this.lvItems.Items.Clear();

			this.Cursor = Cursors.WaitCursor;

			if (this.lvFam.SelectedItems.Count > 0)
			{
				/*
				 * Build selected family instances list
				 */
				List<uint> famInstances = this.GetSelectedFamilyInstances();

				List<RefFile> refList = WizardController.Instance.BuildWardrobes(famInstances);
				foreach (RefFile idr in refList)
				{
					ListViewItem li = new ListViewItem();
					li.ImageIndex = 0;
					li.Text = String.Format("{0:X8}-{1:X8}", idr.FileDescriptor.SubType, idr.FileDescriptor.Instance);

					if (idr.FileDescriptor.MarkForDelete)
						li.Font = new Font(li.Font, FontStyle.Strikeout);

					this.lvItems.Items.Add(li);
				}

			}

			this.Cursor = Cursors.Default;

			this.lvItems.EndUpdate();
			this.ResumeLayout();
		}

		List<uint> GetSelectedFamilyInstances()
		{
			/*
			 * Build selected family instances list
			 */
			List<uint> famInstances = new List<uint>();
			foreach (ListViewItem li in lvFam.SelectedItems)
			{
				Fami fam = li.Tag as Fami;
				if (fam != null) // If all goes well...
					famInstances.Add(fam.FileDescriptor.Instance);
			}
			return famInstances;
		}


		public List<uint> GetCheckedFamilyInstances()
		{
			/*
			 * Build selected family instances list
			 */
			List<uint> famInstances = new List<uint>();
			foreach (ListViewItem li in lvFam.CheckedItems)
			{
				Fami fam = li.Tag as Fami;
				famInstances.Add(fam.FileDescriptor.Instance);
			}
			return famInstances;
		}

		private void lvFam_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (!suppressEvents)
			{
				this.OnHouseholdSelectionChanged(e);
				this.PopulateItemList();
			}
		}

		private void cbCheckAll_CheckedChanged(object sender, EventArgs e)
		{
			bool check = this.cbCheckAll.Checked;

			this.suppressEvents = true;
			foreach (ListViewItem li in this.lvFam.Items)
				li.Checked = check;
			this.suppressEvents = false;

			this.OnHouseholdSelectionChanged(e);
		}

		private void lvFam_ItemChecked(object sender, ItemCheckedEventArgs e)
		{
			if (!suppressEvents)
				this.OnHouseholdSelectionChanged(e);
		}

	
	}
}
