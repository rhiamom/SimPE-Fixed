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
using SimPe.Plugin;

namespace SimPe.Windows.Forms
{
    public class NamedPackedFileDescriptor
    {
        SimPe.Interfaces.Files.IPackedFileDescriptor pfd;
        SimPe.Interfaces.Files.IPackageFile pkg;
        SimPe.Plugin.FileIndexItem fii;
        string realname;
        public NamedPackedFileDescriptor(SimPe.Interfaces.Files.IPackedFileDescriptor pfd, SimPe.Interfaces.Files.IPackageFile pkg)
        {
            this.pfd = pfd;
            this.pkg = pkg;
            this.fii = new SimPe.Plugin.FileIndexItem(pfd, pkg);
            realname = null;
        }

        public SimPe.Plugin.FileIndexItem Resource
        {
            get { return fii; }
        }

        public SimPe.Interfaces.Files.IPackedFileDescriptor Descriptor
        {
            get { return pfd; }
        }

        public bool RealNameLoaded
        {
            get { return realname != null; }
        }

        public void ResetRealName()
        {
            realname = null;
        }

        public string GetRealName()
        {
            if (realname == null)
            {
                if (Helper.WindowsRegistry.DecodeFilenamesState)
                {                    
                    SimPe.Interfaces.Plugin.Internal.IPackedFileWrapper wrp = FileTable.WrapperRegistry.FindHandler(pfd.Type);
                    if (wrp != null)
                    {      
                        lock (wrp)
                        {
                            //System.Diagnostics.Debug.WriteLine("Processing " + pfd.Type.ToString("X")+" "+pfd.Offset.ToString("X"));
                            SimPe.Interfaces.Files.IPackedFileDescriptor bakpfd = null;
                            SimPe.Interfaces.Files.IPackageFile bakpkg = null;
                            if (wrp is SimPe.Interfaces.Plugin.AbstractWrapper)
                            {
                                SimPe.Interfaces.Plugin.AbstractWrapper awrp = (SimPe.Interfaces.Plugin.AbstractWrapper)wrp;
                                if (!awrp.AllowMultipleInstances)
                                {
                                    bakpfd = awrp.FileDescriptor;
                                    bakpkg = awrp.Package;
                                }

                                awrp.FileDescriptor = pfd;
                                awrp.Package = pkg;
                            }
                            try
                            {
                                realname = wrp.ResourceName;                                
                            }
                            catch{ realname = pfd.ToResListString();}
                            finally
                            {
                                if (bakpfd != null || bakpkg != null)
                                    if (wrp is SimPe.Interfaces.Plugin.AbstractWrapper)
                                    {
                                        SimPe.Interfaces.Plugin.AbstractWrapper awrp = (SimPe.Interfaces.Plugin.AbstractWrapper)wrp;
                                        if (!awrp.AllowMultipleInstances)
                                        {
                                            awrp.FileDescriptor = bakpfd;
                                            awrp.Package = bakpkg;
                                        }
                                    }
                            }
                        } //lock
                    }

                }
                if (realname == null) realname = pfd.ToResListString();
            }

            return realname;
        }
    }
}
