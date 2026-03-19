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
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using SimPe.Data;
using SimPe.Interfaces.Files;
using SimPe.Interfaces;

namespace SimPe.Plugin.UI
{
	public partial class NeighborhoodBrowser : Panel //UserControl
	{
		IPackageFile package;
		string fileName;

		public event EventHandler PackageChanged;

		public IPackageFile NeighborhoodPackage
		{
			get { return this.package; }
			set {
				string newFilename = null;
				if (value != null)
					newFilename = value.FileName;

				if (fileName != newFilename)
				{
					if (this.package != null)
						this.package.Close();

					this.package = value;
					fileName = newFilename;
					this.OnPackageChanged(EventArgs.Empty);
				}

			}
		}

		protected virtual void OnPackageChanged(EventArgs eventArgs)
		{
			if (this.PackageChanged != null)
				this.PackageChanged(this, eventArgs);
		}

		public NeighborhoodBrowser()
		{
			InitializeComponent();
		}


		protected void AddImage(string path)
		{
			string name = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(path),
                System.IO.Path.GetFileNameWithoutExtension(path) + ".png");
			//name = System.IO.Path.Combine(path, name);
            if (System.IO.File.Exists(name))
			{
				System.IO.Stream st = System.IO.File.OpenRead(name);
				Image img = Helper.LoadImage(st);
				st.Close();
				WaitingScreen.UpdateImage(ImageLoader.Preview(img, WaitingScreen.ImageSize));
				this.ilist.Images.Add(img);
			}
            /* Unable to get SimPe.Plugin.Network.png; may want to use something else.
            else
            {
                this.ilist.Images.Add(new Bitmap(this.GetType().Assembly.GetManifestResourceStream("SimPe.Plugin.Network.png")));
            }
            */
		}

		protected void AddNeighborhood(string path)
		{
			AddNeighborhood(path, "_Neighborhood.package");
			/*int i=1;
			while (AddNeighborhood(path, "_University"+Helper.MinStrLength(i.ToString(), 3)+".package")) 
			{
				i++;
			}*/
		}

		protected bool AddNeighborhood(string path, string filename)
		{
			string flname = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(path),
                System.IO.Path.Combine(System.IO.Path.GetFileName(path),
                System.IO.Path.GetFileName(path) + filename));
			if (!System.IO.File.Exists(flname)) return false;

			AddImage(flname);
			flname = System.IO.Path.Combine(path, flname);
			string name = flname;
			string actime = "";
			bool ret = false;
			ListViewItem lvi = new ListViewItem();

			if (System.IO.File.Exists(name))
			{
				actime = " (" + System.IO.File.GetLastWriteTime(name).ToString() + ")";
				ret = true;
				try
				{
					SimPe.Packages.File pk = SimPe.Packages.File.LoadFromFile(name);
					lvi.Tag = name; // filename

					name = LoadLabel(pk);
				}
				catch (Exception) { }

			}
			
			lvi.Text = name + actime;
			lvi.ImageIndex = ilist.Images.Count - 1;
			lvi.SubItems.Add(flname);
			lvi.SubItems.Add(name);

			lv.Items.Add(lvi);

			return ret;
		}

		private static string LoadLabel(SimPe.Packages.File pk)
		{
			string name = "Unknown";
			try
			{
				SimPe.Interfaces.Files.IPackedFileDescriptor pfd = pk.FindFile(0x43545353, 0, 0xffffffff, 1);
				if (pfd != null)
				{
					using (SimPe.PackedFiles.Wrapper.Str str = new SimPe.PackedFiles.Wrapper.Str())
					{
						str.ProcessData(pfd, pk);
						name = str.LanguageItems(MetaData.Languages.English)[0].Title;
					}
				}

			}
			finally
			{
				//pk.Reader.Close();
			}
			return name;
		}

		public void UpdateList()
		{
			WaitingScreen.Wait();

			lv.Items.Clear();
			ilist.Images.Clear();
            string path = PathProvider.SimSavegameFolder;
            string sourcepath = System.IO.Path.Combine(path, "Neighborhoods");
            string[] dirs = System.IO.Directory.GetDirectories(sourcepath, "*");
            foreach (string dir in dirs)
                if (dir.IndexOf("Tutorial") == -1)
                AddNeighborhood(dir);

			WaitingScreen.Stop();
		}


		public void NgbOpen(object sender, System.EventArgs e)
		{
			if (lv.SelectedItems.Count <= 0) return;

			string filename = Convert.ToString(lv.SelectedItems[0].Tag);
			if (filename != null && filename != this.fileName)
			{
				this.NeighborhoodPackage = SimPe.Packages.File.LoadFromFile(filename);
			}
		}


	}
}
