/***************************************************************************
 *   Copyright (C) 2005 by Peter L Jones                                   *
 *   peter@users.sf.net                                                    *
 *                                                                         *
 *   Copyright (C) 2025 by GramzeSweatShop                                 *
 *   Rhiamom@mac.com                                                       *
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
using System.Collections;
using System.IO;
using SimPe.Interfaces;
using SimPe.Interfaces.Scenegraph;
using SimPe.Interfaces.Files;
using SimPe;
using SimPe.Scenegraph.Compat;
using MessageBoxButtons = SimPe.Scenegraph.Compat.MessageBoxButtons;
using MessageBoxIcon = SimPe.Scenegraph.Compat.MessageBoxIcon;

namespace pj
{
    class BodyMeshLinker : SimPe.Interfaces.AbstractTool, ITool
    {
        private IPackageFile currentPackage = null;
        private IPackedFileDescriptor refFilePFD = null;

        private String getFilename()
        {
            OpenFileDialogCompat ofd = new OpenFileDialogCompat();
            ofd.AddExtension = true;
            ofd.CheckFileExists = true;
            ofd.CheckPathExists = true;
            ofd.DefaultExt = ".package";
            ofd.DereferenceLinks = true;
            ofd.FileName = "";
            ofd.Filter = L.Get("pkgFilter");
            ofd.FilterIndex = 0;
            ofd.InitialDirectory = System.IO.Path.Combine(SimPe.PathProvider.SimSavegameFolder, "SavedSims");
            ofd.Multiselect = false;
            ofd.ReadOnlyChecked = true;
            ofd.ShowHelp = ofd.ShowReadOnly = false;
            ofd.Title = L.Get("selectPkgMesh");
            ofd.ValidateNames = true;
            SimPe.Scenegraph.Compat.DialogResult dr = ofd.ShowDialog();
            if (SimPe.Scenegraph.Compat.DialogResult.OK.Equals(dr))
                return ofd.FileName;
            return null;
        }

        private void Main()
        {
            if (!SimPe.Scenegraph.Compat.MessageBox.ShowAsync(L.Get("pjSMLbegin"),
                L.Get("pjSML"), MessageBoxButtons.OKCancel, MessageBoxIcon.Information).GetAwaiter().GetResult()
                .Equals(SimPe.Scenegraph.Compat.DialogResult.OK))
                return;

            SimPe.Plugin.RefFile refFile = new SimPe.Plugin.RefFile();
            refFile.ProcessData(refFilePFD, currentPackage);

            if (LinkBodyMesh(refFile))
            {
                refFile.SynchronizeUserData();
                SimPe.Scenegraph.Compat.MessageBox.ShowAsync(L.Get("pjSMLdone"),
                    L.Get("pjSML"), MessageBoxButtons.OK, MessageBoxIcon.Information).GetAwaiter().GetResult();
            }
        }

        public bool LinkBodyMesh(SimPe.Plugin.RefFile refFile)
        {
            if (refFile.Items[0].Type != SimPe.Data.MetaData.CRES
                || refFile.Items[1].Type != SimPe.Data.MetaData.SHPE)
            {
                SimPe.Scenegraph.Compat.MessageBox.ShowAsync(L.Get("noCRESSHPE"),
                    L.Get("pjSML"), MessageBoxButtons.OK, MessageBoxIcon.Error).GetAwaiter().GetResult();
                return false;
            }

            String meshPackage = getFilename();
            if (meshPackage == null || meshPackage.Length == 0)
                return false;

            IPackageFile p = null;
            try { p = SimPe.Packages.File.LoadFromFile(meshPackage); }
            catch { p = null; }
            if (p == null)
            {
                SimPe.Scenegraph.Compat.MessageBox.ShowAsync(L.Get("didNotOpen") + "\r\n" + meshPackage,
                    L.Get("pjSML"), MessageBoxButtons.OK, MessageBoxIcon.Error).GetAwaiter().GetResult();
                return false;
            }

            IPackedFileDescriptor[] pfa = p.FindFiles(SimPe.Data.MetaData.CRES);
            IPackedFileDescriptor[] pfb = p.FindFiles(SimPe.Data.MetaData.SHPE);
            if (pfa == null || pfa.Length != 1 || pfb == null || pfb.Length != 1)
            {
                SimPe.Scenegraph.Compat.MessageBox.ShowAsync(L.Get("badMeshPackage") + "\r\n" + meshPackage,
                    L.Get("pjSML"), MessageBoxButtons.OK, MessageBoxIcon.Error).GetAwaiter().GetResult();
                return false;
            }

            refFile.Items[0].Group = pfa[0].Group;
            refFile.Items[0].SubType = pfa[0].SubType;
            refFile.Items[0].Instance = pfa[0].Instance;
            refFile.Items[1].Group = pfb[0].Group;
            refFile.Items[1].SubType = pfb[0].SubType;
            refFile.Items[1].Instance = pfb[0].Instance;

            return true;
        }

        #region ITool Members

        public bool IsEnabled(IPackedFileDescriptor pfd, IPackageFile package)
        {
            return (package != null);
        }

        private bool IsReallyEnabled(IPackedFileDescriptor pfd, IPackageFile package)
        {
            currentPackage = package;
            if (pfd != null && pfd.Type == SimPe.Data.MetaData.REF_FILE)
                refFilePFD = pfd;
            else
                refFilePFD = null;
            return package != null && refFilePFD != null;
        }

        public SimPe.Interfaces.Plugin.IToolResult ShowDialog(ref SimPe.Interfaces.Files.IPackedFileDescriptor pfd, ref SimPe.Interfaces.Files.IPackageFile package)
        {
            if (!IsReallyEnabled(pfd, package))
            {
                SimPe.Scenegraph.Compat.MessageBox.ShowAsync(SimPe.Localization.GetString("This is not an appropriate context in which to use this tool"),
                    L.Get("pjSML")).GetAwaiter().GetResult();
                return new SimPe.Plugin.ToolResult(false, false);
            }
            Main();
            return new SimPe.Plugin.ToolResult(false, false);
        }


        #region IToolPlugin Members

        public override string ToString()
        {
            return L.Get("pjBMTLink");
        }

        #endregion
        #endregion

        #region IToolExt Member
        public override System.Drawing.Image Icon
        {
            get
            {
                return LoadIcon.load("BMLinker");
            }
        }
        #endregion
    }
}
