/***************************************************************************
 *   Copyright (C) 2026 by GramzeSweatshop                                 *
 *   rhiamom@mac.com                                                       *
 *                                                                         *
 *   This program is free software; you can redistribute it and/or modify  *
 *   it under the terms of the GNU General Public License as published by  *
 *   the Free Software Foundation; either version 2 of the License, or     *
 *   (at your option) any later version.                                   *
 ***************************************************************************/

using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace SimPe
{
    /// <summary>
    /// Persists the user-managed list of FileTable entries (added via
    /// Preferences -> File Table) to customfolders.xreg next to folders.xreg,
    /// and reads them back so FileTableBase.DefaultFolders can include them.
    /// </summary>
    public static class CustomFolderStore
    {
        public static string ConfigPath
        {
            get { return Path.Combine(Helper.SimPeDataPath, "customfolders.xreg"); }
        }

        public static List<FileTableItem> Load()
        {
            var items = new List<FileTableItem>();
            try
            {
                string path = ConfigPath;
                if (!File.Exists(path)) return items;

                var doc = new XmlDocument();
                doc.Load(path);

                XmlNodeList nodes = doc.SelectNodes("//file | //path");
                if (nodes == null) return items;

                foreach (XmlNode n in nodes)
                {
                    bool isFile = string.Equals(n.Name, "file", StringComparison.OrdinalIgnoreCase);

                    string root = AttrOrEmpty(n, "root");
                    string recAttr = AttrOrEmpty(n, "recursive");
                    string ignAttr = AttrOrEmpty(n, "ignore");
                    string verAttr = AttrOrEmpty(n, "version");
                    string relPath = (n.InnerText ?? "").Trim();
                    if (relPath.Length == 0) continue;

                    bool recursive = (recAttr == "1" || recAttr.Equals("true", StringComparison.OrdinalIgnoreCase));
                    bool ignore = (ignAttr == "1" || ignAttr.Equals("true", StringComparison.OrdinalIgnoreCase));
                    int ver = -1;
                    int.TryParse(verAttr, out ver);

                    FileTableItemType type = ResolveRoot(root);

                    var fti = new FileTableItem(relPath, type, recursive, isFile, ver, ignore);
                    items.Add(fti);
                }
            }
            catch
            {
                // Treat any read failure as "no custom items" rather than blocking startup.
            }
            return items;
        }

        public static void Save(IList<FileTableItem> items)
        {
            try
            {
                var xws = new XmlWriterSettings
                {
                    CloseOutput = true,
                    Indent = true,
                    Encoding = System.Text.Encoding.UTF8,
                };
                using (XmlWriter xw = XmlWriter.Create(ConfigPath, xws))
                {
                    xw.WriteStartElement("folders");
                    xw.WriteStartElement("filetable");

                    if (items != null)
                    {
                        foreach (FileTableItem fti in items)
                        {
                            if (fti == null) continue;
                            xw.WriteStartElement(fti.IsFile ? "file" : "path");

                            string rootKey = RootAttr(fti.Type);
                            if (rootKey != null) xw.WriteAttributeString("root", rootKey);
                            if (fti.IsRecursive) xw.WriteAttributeString("recursive", "1");
                            if (fti.EpVersion >= 0) xw.WriteAttributeString("version", fti.EpVersion.ToString());
                            if (fti.Ignore) xw.WriteAttributeString("ignore", "1");

                            xw.WriteValue(fti.RelativePath);
                            xw.WriteEndElement();
                        }
                    }

                    xw.WriteEndElement();
                    xw.WriteEndElement();
                }
            }
            catch
            {
                // Silent — Preferences caller can show its own error if it wants to.
            }
        }

        // Maps the "root" XML attribute back onto a FileTableItemType. Mirrors the
        // strings written by FileTableBase.StoreFoldersXml so files can be shared
        // with the existing folder-XML format.
        private static FileTableItemType ResolveRoot(string root)
        {
            if (string.IsNullOrEmpty(root)) return FileTablePaths.Absolute;

            switch (root.ToLowerInvariant())
            {
                case "save":        return FileTablePaths.SaveGameFolder;
                case "simpe":       return FileTablePaths.SimPEFolder;
                case "simpedata":   return FileTablePaths.SimPEDataFolder;
                case "simpeplugin": return FileTablePaths.SimPEPluginFolder;
            }

            // Try to match an EP shortid (e.g. "bg", "ep1", "sp9").
            foreach (ExpansionItem ei in PathProvider.Global.Expansions)
            {
                if (string.Equals(ei.ShortId, root, StringComparison.OrdinalIgnoreCase))
                    return ei.Expansion;
            }

            return FileTablePaths.Absolute;
        }

        private static string RootAttr(FileTableItemType type)
        {
            if (type == FileTablePaths.SaveGameFolder) return "save";
            if (type == FileTablePaths.SimPEFolder) return "simpe";
            if (type == FileTablePaths.SimPEDataFolder) return "simpeData";
            if (type == FileTablePaths.SimPEPluginFolder) return "simpePlugin";
            if (type == FileTablePaths.Absolute) return null;

            foreach (ExpansionItem ei in PathProvider.Global.Expansions)
            {
                if (type == ei.Expansion) return ei.ShortId.ToLowerInvariant();
            }
            return null;
        }

        private static string AttrOrEmpty(XmlNode n, string name)
        {
            if (n.Attributes == null) return "";
            var a = n.Attributes[name];
            return a == null ? "" : (a.Value ?? "").Trim();
        }
    }
}
