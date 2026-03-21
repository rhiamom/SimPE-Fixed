/***************************************************************************
 *   Copyright (C) 2005 by Ambertation                                     *
 *   quaxi@ambertation.de                                                  *
 *                                                                         *
 *   Copyright (C) 2026 by GramzeSweatshop                                 *
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
using System.ComponentModel;

namespace SimPe
{
    /// <summary>
    /// This is used to display Paths in the Options Dialog
    /// </summary>
    public class PathSettings : SimPe.GlobalizedObject
    {
        Registry r;

        protected PathSettings(Registry r)
        {
            this.r = r;
        }

        protected string GetPath(ExpansionItem ei)
        {
            if (ei.InstallFolder == null) return ei.RealInstallFolder;
            if (ei.InstallFolder.Trim() == "") return ei.RealInstallFolder;
            return ei.InstallFolder;
        }

        protected string GetPath(string userpath, string defpath)
        {
            if (userpath == null) userpath = "";
            if (userpath.Trim() == "") return defpath;
            return userpath;
        }

        [Category("BaseGame")]
        public string SaveGamePath
        {
            get { return GetPath(PathProvider.SimSavegameFolder, PathProvider.RealSavegamePath); }
            set { PathProvider.SimSavegameFolder = value; }
        }
    }
}