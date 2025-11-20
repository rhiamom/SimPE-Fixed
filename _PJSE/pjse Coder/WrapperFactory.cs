/***************************************************************************
 *   Copyright (C) 2005 by Peter L Jones                                   *
 *   pljones@users.sf.net                                                  *
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
using SimPe.Interfaces;
using SimPe.Interfaces.Plugin;
using SimPe.PackedFiles.UserInterface;
using SimPe.PackedFiles.Wrapper;

namespace SimPe.Plugin
{
    /// <summary>
    /// Lists all Plugins (=FileType Wrappers) available in this Package
    /// </summary>
    /// <remarks>
    /// GetWrappers() has to return a list of all Plugins provided by this Library.
    /// If a Plugin isn't returned, SimPe won't recognize it!
    /// </remarks>
    public class WrapperFactory : AbstractWrapperFactory, IToolFactory, IHelpFactory, ISettingsFactory
    {
        /// <summary>
        /// Returns a List of all available Plugins in this Package
        /// </summary>
        /// <returns>A List of all provided Plugins (=FileType Wrappers)</returns>
        public override SimPe.Interfaces.IWrapper[] KnownWrappers
        {
            get
            {
                if (Helper.NoPlugins)
                {
                    return new IWrapper[0];
                }
                else if (Helper.StartedGui == Executable.Classic)
                {
                    return new IWrapper[] {
										   new Bcon()
										  ,new Bhav()
										  ,new Objf()
										  ,new TPRP()
										  ,new Trcn()
										  ,new Ttab()
									  };
                }
                else if (!UserVerification.HaveValidUserId || Helper.WindowsRegistry.HiddenMode)
                {
                    return new IWrapper[] {
										   new Bcon()
										  ,new Bhav()
										  ,new Objf()
										  ,new StrWrapper()
										  ,new TPRP()
										  ,new Trcn()
										  ,new Ttab()
									  };
                }
                else
                {
                    return new IWrapper[] {
										   new Bcon()
										  ,new Bhav()
										  ,new Objf()
										  ,new StrWrapper()
										  ,new TPRP()
										  ,new Trcn()
										  ,new Ttab()
										  ,new TreesPackedFileWrapper()
									  };
                }
            }
        }

        #region IToolFactory Members

        public SimPe.Interfaces.IToolPlugin[] KnownTools
        {
            get
            {
                if (Helper.NoPlugins)
                {
                    return new IToolPlugin[0];
                }
                else if (Helper.StartedGui == Executable.Classic)
                {
                    return new IToolPlugin[] {
										  new pjse.FileTableTool()
								      };
                }
                else return new IToolPlugin[] {
										  new pjse.guidtool.GUIDTool()
										 ,new pjse.FileTableTool()
								      };
            }
        }

        #endregion

        #region IHelpFactory Members

        class helpContents : IHelp
        {
            #region IHelp Members
            public System.Drawing.Image Icon { get { return null; } }
            public override string ToString() { return pjse.Localization.GetString("helpPJSE"); }
            public void ShowHelp(ShowHelpEventArgs e) { pjse.HelpHelper.Help("Contents"); }
            #endregion
        }

        public IHelp[] KnownHelpTopics
        {
            get
            {
                IHelp[] helpTopics = {
					new helpContents()
				};
                return helpTopics;
            }
        }


        #endregion

        #region ISettingsFactory Members

        public ISettings[] KnownSettings
        {
            get { return new ISettings[] { pjse.Settings.PJSE, pjse.FileTableSettings.FTS }; }
        }
        #endregion
    }
}
