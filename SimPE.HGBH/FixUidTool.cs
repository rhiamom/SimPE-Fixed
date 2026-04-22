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
using System.Windows.Forms;
using SimPe.Interfaces;
using SimPe.Interfaces.Files;
using SimPe.Interfaces.Plugin;

namespace SimPe.Plugin
{
    public class FixUidTool : ITool
    {
        internal FixUidTool() { }

        public bool IsEnabled(IPackedFileDescriptor pfd, IPackageFile package)
        {
            return true;
        }

        public IToolResult ShowDialog(ref IPackedFileDescriptor pfd, ref IPackageFile package)
        {
            DialogResult dr = Message.Show(
                "Using this tool can seriously mess up all of your Neighbourhoods and Neighbourhood Stories, and it may achieve nothing useful.\n\n"
                + "Make sure you have a backup of ALL your Neighbourhoods before starting this tool!\n\n"
                + "Do you want to start this tool?",
                "Confirmation",
                MessageBoxButtons.YesNo);

            if (dr != DialogResult.Yes) return new ToolResult(false, false);

            try
            {
                System.Collections.Hashtable ht = Idno.FindUids(PathProvider.SimSavegameFolder, false);
                string[] files = new string[ht.Count];
                int i = 0;
                foreach (string file in ht.Keys) { files[i] = file; i++; }

                Wait.Start(files.Length);
                foreach (string file in files)
                {
                    SimPe.Packages.GeneratableFile fl = SimPe.Packages.GeneratableFile.LoadFromFile(file);
                    IPackedFileDescriptor[] pfds = fl.FindFiles(Data.MetaData.IDNO);
                    foreach (IPackedFileDescriptor spfd in pfds)
                    {
                        Idno idno = new Idno();
                        idno.ProcessData(spfd, fl);
                        idno.MakeUnique(ht);
                        idno.SynchronizeUserData();
                    }
                    fl.Save();
                    Wait.Progress++;
                }
            }
            catch (Exception ex)
            {
                Helper.ExceptionMessage("", ex);
            }
            finally
            {
                Wait.Stop(true);
            }

            return new ToolResult(false, false);
        }

        public override string ToString()
        {
            return "Neighbourhood\\Chris Hatch's FixUID Tool";
        }
    }
}
