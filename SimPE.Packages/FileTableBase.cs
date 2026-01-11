/***************************************************************************
 *   Copyright (C) 2005-2008 by Peter L Jones                              *
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
using System.Collections;
using System.Xml;
using SimPe.Interfaces.Wrapper;

namespace SimPe
{
	/// <summary>
	/// Do not use this class direct, use <see cref="SimPe.FileTable"/> instead!
	/// </summary>
	public class FileTableBase
	{
		static Interfaces.Scenegraph.IScenegraphFileIndex fileindex;

        /// <summary>
        /// Returns the FileIndex
        /// </summary>
        /// <remarks>This will be initialized by the RCOL Factory</remarks>
        public static Interfaces.Scenegraph.IScenegraphFileIndex FileIndex
        {
            get { return fileindex; }
            set
            {
                fileindex = value;
                System.Diagnostics.Debug.WriteLine(
                    "[FileTable] FileIndex set to " +
                    (fileindex == null ? "null" : fileindex.GetType().FullName));
            }
        }

        /// <summary>
        /// Returns a List of all Folders, even those the User doesn't want to scan for Content
        /// </summary>
        public static ArrayList DefaultFolders
        {
            get
            {
                try
                {
                    ArrayList folders = new ArrayList();

                    // Use the game root chosen via GameRootDialog
                    string root = Helper.GameRootPath;
                    if (string.IsNullOrEmpty(root) || !System.IO.Directory.Exists(root))
                        return folders;   // empty ? SimPE loads, but no game data

                    // Scan the actual installation (this replaces the old .xreg/registry logic)
                    var scan = GameRootAutoScanner.ScanRoot(root);

                    // Build the file table from each detected pack's TSData path
                    foreach (var pack in scan.Packs)
                    {
                        string tsData = System.IO.Path.Combine(pack.FullPath, "TSData");

                        folders.Add(new FileTableItem(System.IO.Path.Combine(tsData, "Res\\Objects"), false, false));
                        folders.Add(new FileTableItem(System.IO.Path.Combine(tsData, "Res\\Overrides"), false, false));
                        folders.Add(new FileTableItem(System.IO.Path.Combine(tsData, "Res\\StuffPack\\Objects"), false, false));

                        // Materials may be under both Catalog and Res
                        folders.Add(new FileTableItem(System.IO.Path.Combine(tsData, "Res\\Materials"), false, false));
                        folders.Add(new FileTableItem(System.IO.Path.Combine(tsData, "Res\\Catalog\\Materials"), false, false));

                        // Base game uses Sims3D; EPs/SPs use 3D
                        folders.Add(new FileTableItem(System.IO.Path.Combine(tsData, pack.IsBaseGame ? "Res\\Sims3D" : "Res\\3D"), false, false));

                        folders.Add(new FileTableItem(System.IO.Path.Combine(tsData, "Res\\UI"), false, false));

                        // Catalog contains buy/build entries – recurse here
                        folders.Add(new FileTableItem(System.IO.Path.Combine(tsData, "Res\\Catalog"), true, false));
                    }

                    return folders;
                }
                catch (Exception ex)
                {
                    System.Windows.Forms.MessageBox.Show(
                        "SimPE could not build the FileTable from the game installation.\n\n" + ex.Message,
                        "FileTable Error",
                        System.Windows.Forms.MessageBoxButtons.OK,
                        System.Windows.Forms.MessageBoxIcon.Error);

                    // Preserve original behavior: fall back to MetaFolder
                    // (you can change this to 'return new ArrayList();' if you prefer)
                    return new ArrayList();
                }
            }
        }


        /// <summary>
        /// Creates a default Folder xml
        /// </summary>
        public static void BuildFolderXml()
        {
            try
            {
                XmlWriterSettings xws = new XmlWriterSettings();
                xws.CloseOutput = true;
                xws.Indent = true;
                xws.Encoding = System.Text.Encoding.UTF8;
                XmlWriter xw = XmlWriter.Create(Helper.DataFolder.FoldersXREGW, xws);

                try
                {
                    xw.WriteStartElement("folders");
                    xw.WriteStartElement("filetable");
                    if (SimPe.PathProvider.Global.GameVersion < 18)
                    {
                        xw.WriteStartElement("file");
                        xw.WriteAttributeString("root", "save");
                        xw.WriteAttributeString("ignore", "1");
                        xw.WriteValue("Downloads" + Helper.PATH_SEP + "_EnableColorOptionsGMND.package");
                        xw.WriteEndElement();

                        xw.WriteStartElement("file");
                        xw.WriteAttributeString("root", "game");
                        xw.WriteAttributeString("ignore", "1");
                        xw.WriteValue("TSData" + Helper.PATH_SEP + "Res" + Helper.PATH_SEP + "Sims3D" + Helper.PATH_SEP + "_EnableColorOptionsMMAT.package");
                        xw.WriteEndElement();

                        xw.WriteStartElement("path");
                        xw.WriteAttributeString("root", "save");
                        xw.WriteAttributeString("recursive", "1");
                        xw.WriteAttributeString("ignore", "1");
                        xw.WriteValue("zCEP-EXTRA");
                        xw.WriteEndElement();

                        xw.WriteStartElement("path");
                        xw.WriteAttributeString("root", "game");
                        xw.WriteAttributeString("recursive", "1");
                        xw.WriteAttributeString("ignore", "1");
                        xw.WriteValue("TSData" + Helper.PATH_SEP + "Res" + Helper.PATH_SEP + "Catalog" + Helper.PATH_SEP + "zCEP-EXTRA");
                        xw.WriteEndElement();
                    }

                    for (int i = PathProvider.Global.Expansions.Count - 1; i >= 0; i--)
                    {
                        ExpansionItem ei = PathProvider.Global.Expansions[i];
                        string s = ei.ShortId.ToLower();
                        {
                            foreach (string folder in ei.PreObjectFileTableFolders)
                                writenode(xw, shouldignor(ei, folder), s, null, folder);

                            if (ei.Flag.Class == ExpansionItem.Classes.Story || !ei.Flag.FullObjectsPackage)
                                writenode(xw, shouldignor(ei, ei.ObjectsSubFolder), s, null, ei.ObjectsSubFolder);
                            else
                                writenode(xw, shouldignor(ei, ei.ObjectsSubFolder), s, ei.Version.ToString(), ei.ObjectsSubFolder);

                            foreach (string folder in ei.FileTableFolders)
                                writenode(xw, shouldignor(ei, folder), s, null, folder);
                        }
                    }

                    xw.WriteEndElement();
                    xw.WriteEndElement();
                }
                finally
                {
                    xw.Close();
                    xw = null;
                }
            }
            catch (Exception ex)
            {
                Helper.ExceptionMessage("Unable to create default Folder File!", ex);
            }
        }

        private static bool shouldignor(ExpansionItem ei, string folder)
        {
            if (PathProvider.Global.GameVersion == 19 && (ei.Version == 18 || ei.Version == 17)) return true;
            if (PathProvider.Global.GameVersion == 18 && ei.Version == 17) return true;
            if ((PathProvider.Global.GameVersion < 21 && ei.Flag.SimStory) || (!ei.Exists && ei.InstallFolder == "")) return true;
            if (ei.Version == PathProvider.Global.GameVersion && (folder.EndsWith("\\Objects") || folder.EndsWith("\\Overrides") || folder.EndsWith("\\UI") || folder.EndsWith("\\Wants"))) return false;
            if (folder.EndsWith("\\3D") || folder.EndsWith("\\Sims3D") || folder.EndsWith("\\Stuffpack\\Objects") || folder.EndsWith("\\Materials")) return false;
            return true;
        }

        /// <summary>
        /// Write folders.xreg
        /// </summary>
        /// <param name="lfti">A <typeparamref name="List&lt;&gt;"/> of <typeparamref name="FileTableItem"/> entries</param>
        public static void StoreFoldersXml(System.Collections.Generic.List<FileTableItem> lfti)
        {
            try
            {
                XmlWriterSettings xws = new XmlWriterSettings();
                xws.CloseOutput = true;
                xws.Indent = true;
                xws.Encoding = System.Text.Encoding.UTF8;
                XmlWriter xw = XmlWriter.Create(Helper.DataFolder.FoldersXREGW, xws);

                try
                {
                    xw.WriteStartElement("folders");
                    xw.WriteStartElement("filetable");
                    foreach (FileTableItem fti in lfti)
                    {
                        xw.WriteStartElement(fti.IsFile ? "file" : "path");

                        if (fti.Type != FileTablePaths.Absolute)
                        {
                            bool ok = false;
                            foreach (ExpansionItem ei in PathProvider.Global.Expansions)
                            {
                                if (fti.Type == ei.Expansion)
                                {
                                    xw.WriteAttributeString("root", ei.ShortId.ToLower());
                                    ok = true;
                                    break;
                                }
                            }
                            if (!ok)
                            {
                                switch (fti.Type.AsUint)
                                {
                                    case (uint)FileTablePaths.SaveGameFolder:
                                        {
                                            xw.WriteAttributeString("root", "save");
                                            break;
                                        }
                                    case (uint)FileTablePaths.SimPEFolder:
                                        {
                                            xw.WriteAttributeString("root", "simpe");
                                            break;
                                        }
                                    case (uint)FileTablePaths.SimPEDataFolder:
                                        {
                                            xw.WriteAttributeString("root", "simpeData");
                                            break;
                                        }
                                    case (uint)FileTablePaths.SimPEPluginFolder:
                                        {
                                            xw.WriteAttributeString("root", "simpePlugin");
                                            break;
                                        }
                                } //switch
                            }
                        }

                        if (fti.IsRecursive) xw.WriteAttributeString("recursive", "1");
                        if (fti.EpVersion >= 0) xw.WriteAttributeString("version", fti.EpVersion.ToString());
                        if (fti.Ignore) xw.WriteAttributeString("ignore", "1");

                        xw.WriteValue(fti.RelativePath);
                        xw.WriteEndElement();
                    }
                    xw.WriteEndElement();
                    xw.WriteEndElement();
                }
                finally
                {
                    xw.Close();
                    xw = null;
                }
            }
            catch (Exception ex)
            {
                Helper.ExceptionMessage("", ex);
            }
        }

        private static void writenode(XmlWriter xw, bool ign, string root, string version, string folder)
        {
            xw.WriteStartElement("path");
            if (ign)
                xw.WriteAttributeString("ignore", "1");
            xw.WriteAttributeString("root", root);
            if (version != null)
                xw.WriteAttributeString("version", version);
            xw.WriteValue(folder);
            xw.WriteEndElement();
        }

		static SimPe.Interfaces.IWrapperRegistry wreg;
		/// <summary>
		/// Returns/Sets a WrapperRegistry (can be null)
		/// </summary>
		public static SimPe.Interfaces.IWrapperRegistry WrapperRegistry
		{
			get { return wreg; }
			set { wreg = value;}
		}

		static SimPe.Interfaces.IProviderRegistry preg;
		/// <summary>
		/// Returns/Sets a ProviderRegistry (can be null)
		/// </summary>
		public static SimPe.Interfaces.IProviderRegistry ProviderRegistry
		{
			get { return preg; }
			set { preg = value;}
		}

		static IGroupCache gc;
		/// <summary>
		/// Returns The Group Cache used to determin local Groups
		/// </summary>
		public static IGroupCache GroupCache
		{
			get { return gc; }
			set { gc = value;}
		}
	}
}
