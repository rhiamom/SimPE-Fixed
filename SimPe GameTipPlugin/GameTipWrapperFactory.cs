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
using SimPe.Interfaces;

namespace SimPe.Plugin
{
    public class GametipWrapperFactory : SimPe.Interfaces.Plugin.AbstractWrapperFactory, SimPe.Interfaces.Plugin.IHelpFactory
    {
        #region AbstractWrapperFactory Member
        public override SimPe.Interfaces.IWrapper[] KnownWrappers
		{
			get
            {
                if (Helper.SimPeVersionLong < 330717003793) // requires updated simpe.workspace and GDF
                {
                    return new IWrapper[0];
                }
                else if (Helper.StartedGui == Executable.Classic)
                {
                    IWrapper[] wrappers = { new XGoal() };
                    return wrappers;
                }
                else
                {
                    IWrapper[] wrappers = {
										  new GametipPackedFileWrapper()
										  ,new LastEPusePackedFileWrapper()
										  ,new GWInvPackedFileWrapper()
									     };
                    return wrappers;
                }
			}
		}
        #endregion

        #region IHelpFactory Members

        class simmyHelp : IHelp
        {
            public System.Drawing.Image Icon { get { return null; } }
            public override string ToString() { return "Sims2 Beginners Guide"; }
            public void ShowHelp(ShowHelpEventArgs e) { SimPe.RemoteControl.ShowHelp("file://" + SimPe.Helper.SimPePath + "/Doc/BeginnerGuide.htm"); }
        }

        public IHelp[] KnownHelpTopics
        {
            get
            {
                if (Helper.StartedGui == Executable.Classic || Helper.XmlRegistry.HiddenMode)
                {
                    return new IHelp[0];
                }
                else
                {
                    IHelp[] helpTopics = { new simmyHelp() };
                    return helpTopics;
                }
            }
        }
        #endregion
    }
}
