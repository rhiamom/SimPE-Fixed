/***************************************************************************
 *   Copyright (C) 2005 by Ambertation                                     *
 *   quaxi@ambertation.de                                                  *
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
using System.Media;

namespace SimPe.Plugin.Tool.Action
{
	/// <summary>
	/// The Intrigued Neighbourhood Action
	/// </summary>
	public class ActionIntriguedNeighborhood : SimPe.Interfaces.IToolAction
	{
		
		#region IToolAction Member

		public virtual bool ChangeEnabledStateEventHandler(object sender, SimPe.Events.ResourceEventArgs es)
		{
            if (es.Loaded && Helper.IsNeighborhoodFile(es.LoadedPackage.FileName)) return true;

			return false;
		}

        private bool RealChangeEnabledStateEventHandler(object sender, SimPe.Events.ResourceEventArgs es)
        {
            if (!es.Loaded) return false;

            return es.LoadedPackage.Package.FindFiles(Data.MetaData.SIM_DESCRIPTION_FILE).Length > 0;
        }

        public void ExecuteEventHandler(object sender, SimPe.Events.ResourceEventArgs e)
        {
            // Keep the original safety/context check
            if (!RealChangeEnabledStateEventHandler(null, e))
            {
                System.Windows.Forms.MessageBox.Show(
                    SimPe.Localization.GetString("This is not an appropriate context in which to use this tool"),
                    this.ToString()
                );
                return;
            }

            if (e == null || e.LoadedPackage == null || e.LoadedPackage.Package == null)
                return;

            // Find all Sim Description resources in the package
            SimPe.Interfaces.Files.IPackedFileDescriptor[] pfds =
                e.LoadedPackage.Package.FindFiles(Data.MetaData.SIM_DESCRIPTION_FILE);

            if (pfds == null || pfds.Length == 0)
                return;

            // Use a single SDesc wrapper instance to process each descriptor
            SimPe.PackedFiles.Wrapper.SDesc sdesc =
                new SimPe.PackedFiles.Wrapper.SDesc(null, null, null);

            Random rng = new Random();

            foreach (SimPe.Interfaces.Files.IPackedFileDescriptor pfd in pfds)
            {
                // Load the Sim Description
                sdesc.ProcessData(pfd, e.LoadedPackage.Package);

                // Skip non-human species and special Castaway subspecies
                if (sdesc.Nightlife != null && sdesc.Nightlife.Species != 0)
                {
                    continue;
                }

                // Neutral interest randomization (no CH sex-pack logic)
                sdesc.Interests.Animals      = (ushort)rng.Next(400, 600);
                sdesc.Interests.Crime        = (ushort)rng.Next(400, 600);
                sdesc.Interests.Culture      = (ushort)rng.Next(400, 600);
                sdesc.Interests.Entertainment= (ushort)rng.Next(600, 1000);
                sdesc.Interests.Environment  = (ushort)rng.Next(400, 600);
                sdesc.Interests.Fashion      = (ushort)rng.Next(700, 1000);
                sdesc.Interests.Food         = (ushort)rng.Next(400, 600);
                sdesc.Interests.Health       = (ushort)rng.Next(400, 600);
                sdesc.Interests.Money        = (ushort)rng.Next(400, 600);
                sdesc.Interests.Paranormal   = 10;
                sdesc.Interests.Politics     = (ushort)rng.Next(400, 600);
                sdesc.Interests.School       = (ushort)rng.Next(400, 600);
                sdesc.Interests.Scifi        = (ushort)rng.Next(0, 100);
                sdesc.Interests.Sports       = (ushort)rng.Next(400, 600);
                sdesc.Interests.Toys         = (ushort)rng.Next(400, 600);
                sdesc.Interests.Travel       = (ushort)rng.Next(600, 1000);
                sdesc.Interests.Weather      = (ushort)rng.Next(400, 600);
                sdesc.Interests.Work         = (ushort)rng.Next(400, 600);

                // Write changes back to the underlying resource
                sdesc.SynchronizeUserData();
            }
        }


        #endregion


        #region IToolPlugin Member
        public override string ToString()
        {
            return "Randomize Hood Interests";
        }
        #endregion

        #region IToolExt Member
        public System.Windows.Forms.Shortcut Shortcut
		{
			get
			{
				return System.Windows.Forms.Shortcut.None;
			}
		}

		public System.Drawing.Image Icon
		{
			get
			{
				return System.Drawing.Image.FromStream(this.GetType().Assembly.GetManifestResourceStream("SimPe.Plugin.emoticon.png"));
			}
		}

		public virtual bool Visible 
		{
			get {return true;}
		}

		#endregion
	}
}
