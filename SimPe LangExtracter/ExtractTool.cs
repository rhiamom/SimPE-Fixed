/***************************************************************************
 *   Copyright (C) 2005 by Ambertation                                     *
 *   quaxi@ambertation.de                                                  *
 *                                                                         *
 *   Copyright (C) 2025 by GramzeSweatshop                                 *
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
	/// <summary>
    /// Summary description for ExtractTool.
	/// </summary>
    public class ExtractTool : Interfaces.AbstractTool, Interfaces.ITool
	{
		/// <summary>
		/// Windows Registry Link
		/// </summary>
		static SimPe.Registry registry;
		internal static Registry WindowsRegistry 
		{
			get { return registry; }
		}

		IWrapperRegistry reg;
		IProviderRegistry prov;

		internal ExtractTool(IWrapperRegistry reg, IProviderRegistry prov) 
		{
			this.reg = reg;
			this.prov = prov;

			if (registry==null) registry = Helper.WindowsRegistry;
		}

		#region ITool Member

        public bool IsEnabled(SimPe.Interfaces.Files.IPackedFileDescriptor pfd, SimPe.Interfaces.Files.IPackageFile package)
        {
            if (package == null || package.FileName == null) return false;
            return true;
        }

        private bool IsReallyEnabled(SimPe.Interfaces.Files.IPackedFileDescriptor pfd, SimPe.Interfaces.Files.IPackageFile package)
        {
            if (package == null || package.FileName == null) return false;

            if (package.FindFiles(0x53545223).Length > 0) return true; //Strings (STR#)
            if (package.FindFiles(0x54544173).Length > 0) return true; //Pie String (TTAB)
            if (package.FindFiles(0x43545353).Length > 0) return true; //Catalogue Description (CTSS)

            System.Windows.Forms.MessageBox.Show("This package does not contain any Text Files.");
            return false;
        }

		public Interfaces.Plugin.IToolResult ShowDialog(ref SimPe.Interfaces.Files.IPackedFileDescriptor pfd, ref SimPe.Interfaces.Files.IPackageFile package)
		{
            if (!IsReallyEnabled(pfd, package)) return new SimPe.Plugin.ToolResult(false, false);
            
            LanguageExtrator languagextrator = new LanguageExtrator();
            return languagextrator.Execute(ref pfd, ref package, prov);
        }

		public override string ToString()
		{
            return "Object Tool\\Single Language Extractor...";
        }

        #endregion

        #region IToolExt Member
        public override System.Drawing.Image Icon
        {
            get
            {
                return Helper.LoadImage(this.GetType().Assembly.GetManifestResourceStream("SimPe.Plugin.Extractor.png"));
            }
        }
        public override System.Windows.Forms.Shortcut Shortcut
        {
            get
            {
                return System.Windows.Forms.Shortcut.None;
            }
        }
        #endregion
    }
}
