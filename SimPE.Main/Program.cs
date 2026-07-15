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
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using SimPe.Events;
using Ambertation.Windows.Forms;
using System.Diagnostics;
using System.Runtime.ExceptionServices;

namespace SimPe
{
    partial class MainForm
    {
        public static MainForm Global;
        /// <summary>
        /// Der Haupteinstiegspunkt f�r die Anwendung.
        /// </summary>
        static void LogCrash(Exception ex, bool isTerminating)
        {
            try
            {
                string logPath = System.IO.Path.Combine(
                    System.IO.Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath),
                    "crash.log");
                string msg = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] Terminating={isTerminating}\r\n{ex}\r\n\r\n";
                System.IO.File.AppendAllText(logPath, msg);
            }
            catch { }
        }

        [STAThread]
        static void Main(string[] args)
        {
            // Lock DPI mode before any control is created. DpiUnaware lets Windows
            // bitmap-scale the window so the 96-DPI resx-based form layouts (designed
            // pre-.NET 8) render without clipping or overlap.
            Application.SetHighDpiMode(HighDpiMode.DpiUnaware);

            // Follow the Windows system light/dark setting. .NET 10 made this a
            // stable API; the framework themes most standard controls. Custom-
            // drawn surfaces (BHAV connectors, resource icons) and third-party
            // controls (WeifenLuo docking, OpenTK) don't auto-follow and may
            // need per-site work if this spike proves worth continuing.
            Application.SetColorMode(SystemColorMode.System);

            AppDomain.CurrentDomain.UnhandledException += (s, e) =>
                LogCrash(e.ExceptionObject as Exception ?? new Exception(e.ExceptionObject?.ToString()), e.IsTerminating);
            System.Windows.Forms.Application.ThreadException += (s, e) =>
                LogCrash(e.Exception, false);

            if (System.Environment.Version.Major < 2)
            {
                Message.Show(SimPe.Localization.GetString("NoDotNet").Replace("{VERSION}", System.Environment.Version.ToString()));
                return;
            }

            List<string> argv = new List<string>(args);

            if (Commandline.PreSplash(argv)) return;

            Commandline.CheckFiles();
            //if (!Commandline.ImportOldData()) return;

            // Fresh install: seed default layout and XML data files if they don't exist yet
            if (!System.IO.File.Exists(Helper.DataFolder.SimPeLayout))
                Commandline.ForceModernLayout();
            Helper.ExtractDefaultDataFiles();

            try
            {
                SimPe.Splash.Screen.SetMessage(SimPe.Localization.GetString("Starting SimPE..."));

                Application.DoEvents();
                Application.Idle += new EventHandler(Application_Idle);

                Helper.WindowsRegistry.UpdateSimPEDirectory();
                Helper.LoadGameRootFromFile();
                Global = new MainForm();

                if (!Commandline.FullEnvStart(argv))
                {
                    //load Files passed on the commandline
                    //SimPe.Splash.Screen.SetMessage(SimPe.Localization.GetString("Load or Import Files"));
                    //Global.package.LoadOrImportFiles(argv.ToArray(), true);
                    //Application.Run(Global);

                    try
                    {
                        SimPe.Splash.Screen.SetMessage(SimPe.Localization.GetString("Load or Import Files"));
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine("Crash in Splash.Screen.SetMessage:");
                        Debug.WriteLine(ex.ToString());
                        throw;
                    }

                    try
                    {
                        Global.package.LoadOrImportFiles(argv.ToArray(), true);
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine("Crash in Global.package.LoadOrImportFiles:");
                        Debug.WriteLine(ex.ToString());
                        throw;
                    }

                    //Application.Run(Global);
                    try
                    {
                        Application.Run(Global);
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine("=== Exception escaped Application.Run ===");
                        Debug.WriteLine(ex.ToString());
                        throw;
                    }
                }


                Helper.WindowsRegistry.Flush();
                Helper.WindowsRegistry.Layout.Flush();
                //ExpansionLoader.Global.Flush(); SimPE should not edit this File!

            }
#if !DEBUG
            catch (Exception ex)
            {
                try
                {
                    SimPe.Splash.Screen.Stop();
                    Helper.ExceptionMessage("SimPE will shutdown due to an unhandled Exception.", ex);
                }
                catch (Exception ex2)
                {
                    MessageBox.Show("SimPE will shutdown due to an unhandled Exception.\n\nMessage: " + ex2.Message);
                }
            }
#endif
            finally
            {
                if (SimPe.Splash.Running) SimPe.Splash.Screen.ShutDown();
            }

            try
            {
                SimPe.Packages.StreamFactory.UnlockAll();
                SimPe.Packages.StreamFactory.CloseAll(true);
                SimPe.Packages.StreamFactory.CleanupTeleport();
            }
            catch { }

            // Force process exit — plugin threads and stream handles can otherwise
            // keep the process alive after the main window closes, locking DLL files.
            Environment.Exit(0);
        }
    }
}
