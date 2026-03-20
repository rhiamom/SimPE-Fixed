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
using System.Collections;
using System.IO;
using System.Xml;
namespace SimPe
{
	/// <summary>
	/// Handles Application Settings stored in the Registry
	/// </summary>
	/// <remarks>You cannot create instance of this class, use the 
	/// <see cref="SimPe.Helper.XmlRegistry"/> Field to acces the Registry</remarks>
	public class Registry
	{
		#region Attributes
		public const byte RECENT_COUNT = 15;

		/// <summary>
		/// Contains the Registry
		/// </summary>
		XmlRegistry reg;

		/// <summary>
		/// The Root Registry Key for this Application
		/// </summary>
		XmlRegistryKey xrk;

        XmlRegistry mru;
        XmlRegistryKey mrk;

		LayoutRegistry lr;
		/// <summary>
		/// Returns the LayoutRegistry
		/// </summary>
		public LayoutRegistry Layout 
		{
			get {return lr;}
		}
        long pver;
        // int pep, pepct; long pver; - seem not to be used will comment all out
		#endregion

		#region Management
		/// <summary>
		/// Creates a new Instance
		/// </summary>
		internal Registry()
		{
			pver = this.GetPreviousVersion();
            // pep = -1;
			// pepct = this.GetPreviousEpCount();
			Reload();
			if (Helper.QARelease) this.WasQAUser=true;
		}

		/// <summary>
		/// Reload the SimPe Registry
		/// </summary>
		public void Reload()
		{
            reg = new XmlRegistry(Helper.DataFolder.SimPeXREG, Helper.DataFolder.SimPeXREGW, true);
			xrk = reg.CurrentUser.CreateSubKey(@"Software\Ambertation\SimPe");
			ReloadLayout();
            mru = new XmlRegistry(Helper.DataFolder.MRUXREG, Helper.DataFolder.MRUXREGW, true);
            mrk = mru.CurrentUser.CreateSubKey(@"Software\Ambertation\SimPe");
        }

		/// <summary>
		/// Reload the SimPe Registry
		/// </summary>
		public void ReloadLayout()
		{
            //lr = new LayoutRegistry(xrk.CreateSubKey("Layout"));
            lr = new LayoutRegistry(null);
		}

		/// <summary>
		/// Descturtor 
		/// </summary>
		/// <remarks>
		/// Will flsuh the XmlRegistry to the disk
		/// </remarks>
		~Registry()
		{			
			//Flush();
		}

		/// <summary>
		/// Write the Settings to the Disk
		/// </summary>
		public void Flush() 
		{
			if (lr!=null) lr.Flush();
			if (reg!=null) reg.Flush();
            if (mru != null) mru.Flush();
		}

		/// <summary>
		/// Returns the Registry Key you can use to store Optional Plugin Data
		/// </summary>
		public XmlRegistryKey PluginRegistryKey
		{
			get 
			{
				return xrk.CreateSubKey("PluginSettings");
			}
		}

		/// <summary>
		/// Returns the Base Registry Key
		/// </summary>
		public XmlRegistryKey RegistryKey
		{
			get 
			{
				return xrk;
			}
		}
		#endregion

		public void UpdateSimPEDirectory() { }

		public string PreviousDataFolder => "";

        public string GetPreviousData() => "";

        public long GetPreviousVersion() => 0;

		/// <summary>
		/// Returns the Version of the latest SimPe used so far
		/// </summary>
		public long PreviousVersion
		{
			get
			{
				return pver;
			}
		}

		#region EP Handler
		public bool FoundUnknownEP() => false;
		public string[] InstalledEPExecutables => new string[0];
		#endregion

		#region Settings
		public bool WasQAUser
		{
			get { XmlRegistryKey rkf = xrk.CreateSubKey("Settings"); return Convert.ToBoolean(rkf.GetValue("WasQAUser", false)); }
			set { XmlRegistryKey rkf = xrk.CreateSubKey("Settings"); rkf.SetValue("WasQAUser", value); }
		}

		public int LoadOnlySimsStory
		{
			get { XmlRegistryKey rkf = xrk.CreateSubKey("Settings"); return Convert.ToInt32(rkf.GetValue("LoadOnlySimsStory", 0)); }
			set { XmlRegistryKey rkf = xrk.CreateSubKey("Settings"); rkf.SetValue("LoadOnlySimsStory", value); }
		}

		public SimPe.Data.MetaData.Languages LanguageCode
		{
			get { XmlRegistryKey rkf = xrk.CreateSubKey("Settings"); return (SimPe.Data.MetaData.Languages)Convert.ToInt32(rkf.GetValue("LanguageCode", 0)); }
			set { XmlRegistryKey rkf = xrk.CreateSubKey("Settings"); rkf.SetValue("LanguageCode", (int)value); }
		}

		public bool HiddenMode
		{
			get { XmlRegistryKey rkf = xrk.CreateSubKey("Settings"); return Convert.ToBoolean(rkf.GetValue("HiddenMode", false)); }
			set { XmlRegistryKey rkf = xrk.CreateSubKey("Settings"); rkf.SetValue("HiddenMode", value); }
		}

		public bool AsynchronLoad
		{
			get { XmlRegistryKey rkf = xrk.CreateSubKey("Settings"); return Convert.ToBoolean(rkf.GetValue("AsynchronLoad", true)); }
			set { XmlRegistryKey rkf = xrk.CreateSubKey("Settings"); rkf.SetValue("AsynchronLoad", value); }
		}

		public bool WaitingScreen
		{
			get { XmlRegistryKey rkf = xrk.CreateSubKey("Settings"); return Convert.ToBoolean(rkf.GetValue("WaitingScreen", false)); }
			set { XmlRegistryKey rkf = xrk.CreateSubKey("Settings"); rkf.SetValue("WaitingScreen", value); }
		}

		public uint CachedUserId
		{
			get { XmlRegistryKey rkf = xrk.CreateSubKey("Settings"); return Convert.ToUInt32(rkf.GetValue("CachedUserId", (uint)0)); }
			set { XmlRegistryKey rkf = xrk.CreateSubKey("Settings"); rkf.SetValue("CachedUserId", value); }
		}

		public string Username
		{
			get { XmlRegistryKey rkf = xrk.CreateSubKey("Settings"); return (string)rkf.GetValue("Username", ""); }
			set { XmlRegistryKey rkf = xrk.CreateSubKey("Settings"); rkf.SetValue("Username", value); }
		}

		public string Password
		{
			get { XmlRegistryKey rkf = xrk.CreateSubKey("Settings"); return (string)rkf.GetValue("Password", ""); }
			set { XmlRegistryKey rkf = xrk.CreateSubKey("Settings"); rkf.SetValue("Password", value); }
		}
		#endregion

		#region Wrappers
		/// <summary>
		/// Returns the Priority for the Wrapper identified with the given UID
		/// </summary>
		/// <param name="uid">uique id of the Wrapper</param>
		/// <returns>Priority for the Wrapper</returns>
		public int GetWrapperPriority(ulong uid)
		{
			XmlRegistryKey  rkf = xrk.CreateSubKey("Priorities");
			object o = rkf.GetValue(Helper.HexString(uid));
			if (o==null) return 0x00000000;
			else return Convert.ToInt32(o);
		}

		/// <summary>
		/// Stores the Priority of a Wrapper
		/// </summary>
		/// <param name="uid">uique id of the Wrapper</param>
		/// <param name="priority">the new Priority</param>
		public void SetWrapperPriority(ulong uid, int priority) 
		{
			XmlRegistryKey rkf = xrk.CreateSubKey("Priorities");
			rkf.SetValue(Helper.HexString(uid), priority);
		}
		#endregion

		#region recent Files
		public void ClearRecentFileList()
		{
			XmlRegistryKey rkf = mrk.CreateSubKey("Listings");
			rkf.SetValue("RecentFiles", new Ambertation.CaseInvariantArrayList());
			mru.Flush();
		}

		/// <summary>
		/// Returns a List of recently opened Files
		/// </summary>
		/// <returns>List of Filenames</returns>
		public string[] GetRecentFiles() 
		{
			XmlRegistryKey  rkf = mrk.CreateSubKey("Listings");
			Ambertation.CaseInvariantArrayList al = (Ambertation.CaseInvariantArrayList)rkf.GetValue("RecentFiles", new Ambertation.CaseInvariantArrayList());			

			string[] res = new string[al.Count];
			al.CopyTo(res);
			return res;
		}

		/// <summary>
		/// Adds a File to the List of recently opened Files
		/// </summary>
		/// <param name="filename">The Filename</param>
		public void AddRecentFile(string filename) 
		{
			if (filename==null) return;
			if (filename.Trim()=="") return;
			if (!System.IO.File.Exists(filename)) return;
			
			filename = filename.Trim();
			XmlRegistryKey rkf = mrk.CreateSubKey("Listings");
			
			Ambertation.CaseInvariantArrayList al = (Ambertation.CaseInvariantArrayList)rkf.GetValue("RecentFiles", new Ambertation.CaseInvariantArrayList());	
			if (al.Contains(filename)) 
				al.Remove(filename);
			
			al.Insert(0, filename);			
			while (al.Count>RECENT_COUNT) al.RemoveAt(al.Count-1);
			rkf.SetValue("RecentFiles", al);
			mru.Flush();
		}
		#endregion		

		#region Starup Cheat File
		/// <summary>
		/// Returns true if the Game will start in Debug Mode
		/// </summary>
		public bool GameDebug 
		{
			get 
			{
				if (!System.IO.File.Exists(PathProvider.Global.StartupCheatFile)) return false;

				try 
				{
                    System.IO.TextReader fs = System.IO.File.OpenText(PathProvider.Global.StartupCheatFile);
					string cont = fs.ReadToEnd();
					fs.Close();
					fs.Dispose();
					fs = null;
					string[] lines = cont.Split("\n".ToCharArray());

					foreach (string line in lines) 
					{
						string pline = line.ToLower().Trim();
						while (pline.IndexOf("  ")!=-1) pline = pline.Replace("  ", " ");
						string[] tokens = pline.Split(" ".ToCharArray());

						if (tokens.Length==3) 
						{
							if ( (tokens[0]=="boolprop") &&
								(tokens[1]=="testingcheatsenabled") &&
								(tokens[2]=="true") 
								) return true;
						}
					}
				} 
				catch (Exception) {}

				return false;
			}

			set 
			{
                if (!System.IO.Directory.Exists(System.IO.Path.GetDirectoryName(PathProvider.Global.StartupCheatFile))) return;
				try 
				{
					string newcont = "";
					bool found = false;
                    if (System.IO.File.Exists(PathProvider.Global.StartupCheatFile)) 
					{
                        System.IO.TextReader fs = System.IO.File.OpenText(PathProvider.Global.StartupCheatFile);
						string cont = fs.ReadToEnd();
						fs.Close();
						fs.Dispose();
						fs = null;
						
						string[] lines = cont.Split("\n".ToCharArray());						

						foreach (string line in lines) 
						{
							string pline = line.ToLower().Trim();
							while (pline.IndexOf("  ")!=-1) pline = pline.Replace("  ", " ");
							string[] tokens = pline.Split(" ".ToCharArray());

							if (tokens.Length==3) 
							{
								if ( (tokens[0]=="boolprop") &&
									(tokens[1]=="testingcheatsenabled") 
									) 
								{
									if (!found) 
									{
										newcont += "boolProp testingCheatsEnabled ";
										if (value) newcont += "true"; else newcont += "false";
										newcont += Helper.lbr;
										found = true;
									}
									continue;
								}
							}
							newcont += line.Trim();
							newcont += Helper.lbr;
						}

                        System.IO.File.Delete(PathProvider.Global.StartupCheatFile);
					}

					if (!found) 
					{
						newcont += "boolProp testingCheatsEnabled ";
						if (value) newcont += "true"; else newcont += "false";
						newcont += Helper.lbr;
					}

                    System.IO.TextWriter fw = System.IO.File.CreateText(PathProvider.Global.StartupCheatFile);
					fw.Write(newcont.Trim());
					fw.Close();
					fw.Dispose();
					fw = null;
				}
				catch (Exception) {}
			}
		}		
		#endregion

		#region Censor Patch
		/// <summary>
		/// Returns true if the Game runs properly
		/// </summary>
        [System.ComponentModel.ReadOnly(true)]
		public bool BlurNudity 
		{
			get { return PathProvider.Global.BlurNudity; }
			set { PathProvider.Global.BlurNudity = value; }
		}

		public void BlurNudityUpdate()
		{
            PathProvider.Global.BlurNudityUpdate();
		}
		#endregion
        /*
		#region Obsolete
        
		/// <summary>
        /// Returns the latest number of the Expansion used so far - seems not to be used ever
		/// </summary>
		public int PreviousEpCount
		{
			get
			{
                if (pep == -1) pep = this.GetPreviousEp();
				return pep;
			}
		}
		protected int EPCount
		{
			get 
			{
                int cints = 0;
                string[] cinst = InstalledEPExecutables;
                if (cinst.Length == 0) return 0;
                foreach (string csi in cinst)
                {
                    if (csi != "") cints += 1;
                }
                return cints;
			}
		}

		protected int GetPreviousEpCount()
		{
			XmlRegistryKey rkf = xrk.CreateSubKey("Settings");
			int res = Convert.ToInt32(rkf.GetValue("LastEPCount", this.EPCount));
			rkf.SetValue("LastEPCount", this.EPCount);
			return res;
		}

		protected int GetPreviousEp()
		{
			XmlRegistryKey rkf = xrk.CreateSubKey("Settings");
			int res = Convert.ToInt32(rkf.GetValue("LatestEP", 0));
			rkf.SetValue("LatestEP", PathProvider.Global.GameVersion);
			return res;
		}
        
        private string scramble(string rey)
        {
            byte[] b = Helper.ToBytes(rey);
            return Helper.BytesToHexList(b);
        }

        private string descramble(string rey)
        {
            string ret = "";
            byte[] b = Helper.HexListToBytes(rey);
            foreach (byte f in b) ret += (char)f;
            return ret;
        }
        /// <summary>
        /// oboslete??? True, if you want to see items added to the resoruceList at once (ie. no BeginUpdate)
        /// </summary>
        private bool ShowResourceListContentAtOnce
        {
            get
            {
                XmlRegistryKey rkf = xrk.CreateSubKey("Settings");
                object o = rkf.GetValue("ShowResourceListContentAtOnce", false);
                return Convert.ToBoolean(o);
            }
            set
            {
                XmlRegistryKey rkf = xrk.CreateSubKey("Settings");
                rkf.SetValue("ShowResourceListContentAtOnce", value);
            }
        }

		/// <summary>
		/// true, if user wants to activate the Cache
		/// </summary>
		public  bool XPStyle
		{
			get 
			{
				XmlRegistryKey  rkf = xrk.CreateSubKey("Settings");
				object o = rkf.GetValue("XPStyle", true);
				return Convert.ToBoolean(o);
			}
			set
			{
				XmlRegistryKey rkf = xrk.CreateSubKey("Settings");
				rkf.SetValue("XPStyle", value);
			}
		}
		/// <summary>
		/// true, if the user wanted to use the HexViewer
		/// </summary>
		public  bool HexViewState 
		{
			get 
			{
				XmlRegistryKey  rkf = xrk.CreateSubKey("Settings");
				object o = rkf.GetValue("HexViewEnabled", false);
				return Convert.ToBoolean(o);
			}
			set
			{
				XmlRegistryKey rkf = xrk.CreateSubKey("Settings");
				rkf.SetValue("HexViewEnabled", value);
			}
		}        
		/// <summary>
		/// Obsolete, Since there is no updates will always get/set false
		/// </summary>
		public bool CheckForUpdates 
		{
			get 
			{
                return false;
			}
			set
			{
				XmlRegistryKey rkf = xrk.CreateSubKey("Settings");
				rkf.SetValue("CheckForUpdates", false);
			}
		}        
		/// <summary>
		/// When did whe perform the last UpdateCheck? Obsolete always returns the default
		/// </summary>
		public DateTime LastUpdateCheck
		{
			get 
			{
				XmlRegistryKey  rkf = xrk.CreateSubKey("Settings");
				object o = rkf.GetValue("LastUpdateCheck", DateTime.Now.Subtract(new TimeSpan(2, 0, 0, 0, 0)));
				return Convert.ToDateTime(o);
			}
			set
			{
				XmlRegistryKey rkf = xrk.CreateSubKey("Settings");
				rkf.SetValue("LastUpdateCheck", value);
			}
        }        
        public class ObsoleteWarning : Warning
        {
            internal ObsoleteWarning(string message, string detail) : base(message, detail) { }
        }

        protected static void WarnObsolete()
        {
            // if (Helper.DebugMode)
                throw new SimPe.Registry.ObsoleteWarning("This call is obsolete!", "The Call to this method is obsolete.\n\n Please use the matching version in SimPe.PathProvider.Global, or see http://www.modthesims2.com/index.php? for details.");
        }
		/// <summary>
		/// Obsolete! 
		/// </summary>
		public string RealEP1GamePath 
		{
			get 
			{
                WarnObsolete();
                return SimPe.PathProvider.Global[Expansions.University].RealInstallFolder;
			}
		}
		/// <summary>
		/// Obsolete! 
		/// </summary>
		public string RealEP2GamePath 
		{
			get 
			{
                WarnObsolete();
                return SimPe.PathProvider.Global[Expansions.Nightlife].RealInstallFolder;
			}
		}
		/// <summary>
		/// Obsolete! 
		/// </summary>
		public string RealEP3GamePath 
		{
			get 
			{
                WarnObsolete();
                return SimPe.PathProvider.Global[Expansions.Business].RealInstallFolder;
			}
		}
		/// <summary>
		/// Obsolete!
		/// </summary>
		public string RealSP1GamePath  
		{
			get 
			{
                WarnObsolete();
                return SimPe.PathProvider.Global[Expansions.FamilyFun].RealInstallFolder;
			}
		}
        /// <summary>
        /// Obsolete!
        /// </summary>
        public string RealSP2GamePath 
        {
            get
            {
                WarnObsolete();
                return SimPe.PathProvider.Global[Expansions.Glamour].RealInstallFolder;
            }
        }
        /// <summary>
        /// Obsolete!
        /// </summary>
		public int InstalledVersions
		{
			get
            {
                WarnObsolete();       
				int ret = EPInstalled;
				ret |= SPInstalled<<16;
				return ret;
			}
		}
        /// <summary>
        /// Obsolete!
        /// </summary>
		public int GameVersion 
		{
			get 
			{
                WarnObsolete();
                return SimPe.PathProvider.Global.GameVersion;			
			}
		}
		/// <summary>
		/// Obsolete!
		/// </summary>
		public int EPInstalled 
		{
			get
            {
                WarnObsolete();
                return SimPe.PathProvider.Global.EPInstalled;
			}
		}
		/// <summary>
		/// Obsolete!
		/// </summary>
		public int SPInstalled 
		{
			get 
			{
                WarnObsolete();
                return PathProvider.Global.SPInstalled;
			}
        }
        /// <summary>
        /// Obsolete!
        /// </summary>
        public int STInstalled
        {
            get
            {
                WarnObsolete();
                return PathProvider.Global.STInstalled;
            }
        }
        /// <summary>
        /// Obsolete!
        /// </summary>
        public string RealSavegamePath 
		{
			get 
			{
                WarnObsolete();
				return SimPe.PathProvider.RealSavegamePath;
			}
		}         
		/// <summary>
		/// Obsolete!
		/// </summary>
		public string RealGamePath 
		{
			get 
			{
                WarnObsolete();
				return SimPe.PathProvider.Global[Expansions.BaseGame].RealInstallFolder;
			}
		}
        /// <summary>
        /// Obsolete!
        /// </summary>
        public string SimsPath
        {
            get
            {
                WarnObsolete();
                return SimPe.PathProvider.Global[Expansions.BaseGame].InstallFolder;
            }
            set
            {
                 WarnObsolete();
                SimPe.PathProvider.Global[Expansions.BaseGame].InstallFolder = value;
            }
        }
		/// <summary>
		/// Obsolete!
		/// </summary>
		public string NvidiaDDSTool 
		{
			get 
			{
                WarnObsolete();
				return PathProvider.Global.NvidiaDDSTool;
			}
		}
		/// <summary>
		/// Obsolete!
		/// </summary>
		public string StartupCheatFile 
		{
			get 
			{
                WarnObsolete();
				return PathProvider.Global.StartupCheatFile;
			}
		}
		/// <summary>
		/// Obsolete!
		/// </summary>
		public string NeighborhoodFolder 
		{
			get
            {
                WarnObsolete();
				return PathProvider.Global.NeighborhoodFolder;
			}
		}
		/// <summary>
		/// Obsolete!
		/// </summary>
		public string BackupFolder 
		{
			get 
			{
                WarnObsolete();
                return PathProvider.Global.BackupFolder;
			}
		}
        /// <summary>
		/// Obsolete!
		/// </summary>
		public string NvidiaDDSPath 
		{
			get 
			{
                WarnObsolete();
                return PathProvider.Global.NvidiaDDSPath;
			}
			set
			{
                WarnObsolete();
                PathProvider.Global.NvidiaDDSPath = value;
			}
		}		
		/// <summary>
		/// Obsolete!
		/// </summary>
		public string SimsEP1Path 
		{
			get 
			{
                WarnObsolete();
                return PathProvider.Global[Expansions.University].InstallFolder;
			}
			set
			{
                WarnObsolete();
                PathProvider.Global[Expansions.University].InstallFolder = value;
			}
		}
		/// <summary>
		/// Obsolete!
		/// </summary>
		public string SimsEP2Path 
		{
			get 
			{
                WarnObsolete();
                return PathProvider.Global[Expansions.Nightlife].InstallFolder;
			}
			set
			{
                WarnObsolete();
                PathProvider.Global[Expansions.Nightlife].InstallFolder = value;
			}
		}
		/// <summary>
		/// Obsolete!
		/// </summary>
		public string SimsEP3Path 
		{
			get 
			{
                WarnObsolete();
                return PathProvider.Global[Expansions.Business].InstallFolder;
			}
			set
			{
                WarnObsolete();
                PathProvider.Global[Expansions.Business].InstallFolder = value;
			}
		}
		/// <summary>
		///Obsolete !
		/// </summary>
		public string SimsSP1Path 
		{
			get 
			{
                WarnObsolete();
                return PathProvider.Global[Expansions.FamilyFun].InstallFolder;
			}
			set
			{
                WarnObsolete();
                PathProvider.Global[Expansions.FamilyFun].InstallFolder = value;
			}
		}
        /// <summary>
        /// Obsolete!
        /// </summary>
        public string SimsSP2Path 
        {
            get 
			{
                WarnObsolete();
                return PathProvider.Global[Expansions.Glamour].InstallFolder;
			}
			set
			{
                WarnObsolete();
                PathProvider.Global[Expansions.Glamour].InstallFolder = value;
			}  
        }
		protected static int GetVersion(int index) {
            if ((index & 0xFFFF0000) == 0x00020000) return 5;
			if ((index & 0xFFFF0000) == 0x00010000) return 4;
			if ((index & 0x0000FFFF) == 0x00000003) return 3;			
			if ((index & 0x0000FFFF) == 0x00000002) return 2;
			if ((index & 0x0000FFFF) == 0x00000001) return 1;
            return 0;
        }		
        /// <summary>
        /// Obsolete!
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
		public static string GetEpName(int index)
		{

            WarnObsolete();
            return PathProvider.Global[GetVersion(index)].Name;			
		}
        /// <summary>
        /// Obsolete!
        /// </summary>
		public string CurrentEPName 
		{
            get
            {
                WarnObsolete();
                return SimPe.PathProvider.Global.Latest.DisplayName;
            }
		}
        /// <summary>
        /// Obsolete!
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
		public static string GetExecutableName(int index)
		{
            WarnObsolete();
            return PathProvider.Global[GetVersion(index)].ExeName;	
		}		 
        /// <summary>
        /// Obsolete!
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
		public string GetExecutableFolder(int index)
		{
            WarnObsolete();
            return PathProvider.Global[GetVersion(index)].InstallFolder;
		}
		/// <summary>
		/// Obsolete!
		/// </summary>
		public string SimsApplication 
		{
			get 
			{
                WarnObsolete();
                return PathProvider.Global.SimsApplication;
			}
			
		}
		/// <summary>
		/// Obsolete!
		/// </summary>
		public string SimSavegameFolder
		{
			get 
			{
                WarnObsolete();
                return PathProvider.SimSavegameFolder;
			}
			set 
			{
                WarnObsolete();
                PathProvider.SimSavegameFolder = value;
			}
		}
		#endregion
         */

		public bool UseBigIcons
		{
			get { XmlRegistryKey rkf = xrk.CreateSubKey("Settings"); return Convert.ToBoolean(rkf.GetValue("UseBigIcons", false)); }
			set { XmlRegistryKey rkf = xrk.CreateSubKey("Settings"); rkf.SetValue("UseBigIcons", value); }
		}

		public bool ShowStartupSplash
		{
			get { XmlRegistryKey rkf = xrk.CreateSubKey("Settings"); return Convert.ToBoolean(rkf.GetValue("ShowStartupSplash", true)); }
			set { XmlRegistryKey rkf = xrk.CreateSubKey("Settings"); rkf.SetValue("ShowStartupSplash", value); }
		}

		public enum ResourceListFormats { JustLongType = 0, LongTypeNames = 1, ShortTypeNames = 2, JustNames = 3 }

		public enum ResourceListUnnamedFormats { FullTGI = 0, Instance = 1, TypeGroup = 2 }

		public ResourceListFormats ResourceListFormat
		{
			get { XmlRegistryKey rkf = xrk.CreateSubKey("Settings"); return (ResourceListFormats)Convert.ToInt32(rkf.GetValue("ResourceListFormat", (int)ResourceListFormats.LongTypeNames)); }
			set { XmlRegistryKey rkf = xrk.CreateSubKey("Settings"); rkf.SetValue("ResourceListFormat", (int)value); }
		}

		public bool AutoBackup
		{
			get { XmlRegistryKey rkf = xrk.CreateSubKey("Settings"); return Convert.ToBoolean(rkf.GetValue("AutoBackup", false)); }
			set { XmlRegistryKey rkf = xrk.CreateSubKey("Settings"); rkf.SetValue("AutoBackup", value); }
		}

		public bool UsePackageMaintainer
		{
			get { XmlRegistryKey rkf = xrk.CreateSubKey("Settings"); return Convert.ToBoolean(rkf.GetValue("UsePackageMaintainer", true)); }
			set { XmlRegistryKey rkf = xrk.CreateSubKey("Settings"); rkf.SetValue("UsePackageMaintainer", value); }
		}

		public int BigPackageResourceCount
		{
			get { XmlRegistryKey rkf = xrk.CreateSubKey("Settings"); return Convert.ToInt32(rkf.GetValue("BigPackageResourceCount", 200)); }
			set { XmlRegistryKey rkf = xrk.CreateSubKey("Settings"); rkf.SetValue("BigPackageResourceCount", value); }
		}

		public ResourceListUnnamedFormats ResourceListUnknownDescriptionFormat
		{
			get { XmlRegistryKey rkf = xrk.CreateSubKey("Settings"); return (ResourceListUnnamedFormats)Convert.ToInt32(rkf.GetValue("ResourceListUnknownDescriptionFormat", (int)ResourceListUnnamedFormats.FullTGI)); }
			set { XmlRegistryKey rkf = xrk.CreateSubKey("Settings"); rkf.SetValue("ResourceListUnknownDescriptionFormat", (int)value); }
		}

		public bool UseExpansions2
		{
			get { XmlRegistryKey rkf = xrk.CreateSubKey("Settings"); return Convert.ToBoolean(rkf.GetValue("UseExpansions2", true)); }
			set { XmlRegistryKey rkf = xrk.CreateSubKey("Settings"); rkf.SetValue("UseExpansions2", value); }
		}

		public bool LoadMetaInfo
		{
			get { XmlRegistryKey rkf = xrk.CreateSubKey("Settings"); return Convert.ToBoolean(rkf.GetValue("LoadMetaInfo", true)); }
			set { XmlRegistryKey rkf = xrk.CreateSubKey("Settings"); rkf.SetValue("LoadMetaInfo", value); }
		}

		public bool MultipleFiles
		{
			get { XmlRegistryKey rkf = xrk.CreateSubKey("Settings"); return Convert.ToBoolean(rkf.GetValue("MultipleFiles", false)); }
			set { XmlRegistryKey rkf = xrk.CreateSubKey("Settings"); rkf.SetValue("MultipleFiles", value); }
		}

		public bool Silent
		{
			get { XmlRegistryKey rkf = xrk.CreateSubKey("Settings"); return Convert.ToBoolean(rkf.GetValue("Silent", false)); }
			set { XmlRegistryKey rkf = xrk.CreateSubKey("Settings"); rkf.SetValue("Silent", value); }
		}

		// ── Additional settings ──────────────────────────────────────────
		public bool AllowChangeOfSecondaryAspiration
		{
			get { XmlRegistryKey rkf = xrk.CreateSubKey("Settings"); return Convert.ToBoolean(rkf.GetValue("AllowChangeOfSecondaryAspiration", false)); }
			set { XmlRegistryKey rkf = xrk.CreateSubKey("Settings"); rkf.SetValue("AllowChangeOfSecondaryAspiration", value); }
		}
		public bool AllowLotZero
		{
			get { XmlRegistryKey rkf = xrk.CreateSubKey("Settings"); return Convert.ToBoolean(rkf.GetValue("AllowLotZero", false)); }
			set { XmlRegistryKey rkf = xrk.CreateSubKey("Settings"); rkf.SetValue("AllowLotZero", value); }
		}
		public bool AsynchronSort
		{
			get { XmlRegistryKey rkf = xrk.CreateSubKey("Settings"); return Convert.ToBoolean(rkf.GetValue("AsynchronSort", true)); }
			set { XmlRegistryKey rkf = xrk.CreateSubKey("Settings"); rkf.SetValue("AsynchronSort", value); }
		}
		public bool CorrectJointDefinitionOnExport
		{
			get { XmlRegistryKey rkf = xrk.CreateSubKey("Settings"); return Convert.ToBoolean(rkf.GetValue("CorrectJointDefinitionOnExport", true)); }
			set { XmlRegistryKey rkf = xrk.CreateSubKey("Settings"); rkf.SetValue("CorrectJointDefinitionOnExport", value); }
		}
		public bool CresPrioritize
		{
			get { XmlRegistryKey rkf = xrk.CreateSubKey("Settings"); return Convert.ToBoolean(rkf.GetValue("CresPrioritize", false)); }
			set { XmlRegistryKey rkf = xrk.CreateSubKey("Settings"); rkf.SetValue("CresPrioritize", value); }
		}
		public bool DecodeFilenamesState
		{
			get { XmlRegistryKey rkf = xrk.CreateSubKey("Settings"); return Convert.ToBoolean(rkf.GetValue("DecodeFilenamesState", true)); }
			set { XmlRegistryKey rkf = xrk.CreateSubKey("Settings"); rkf.SetValue("DecodeFilenamesState", value); }
		}
		public bool DeepSimScan
		{
			get { XmlRegistryKey rkf = xrk.CreateSubKey("Settings"); return Convert.ToBoolean(rkf.GetValue("DeepSimScan", false)); }
			set { XmlRegistryKey rkf = xrk.CreateSubKey("Settings"); rkf.SetValue("DeepSimScan", value); }
		}
		public bool DeepSimTemplateScan
		{
			get { XmlRegistryKey rkf = xrk.CreateSubKey("Settings"); return Convert.ToBoolean(rkf.GetValue("DeepSimTemplateScan", false)); }
			set { XmlRegistryKey rkf = xrk.CreateSubKey("Settings"); rkf.SetValue("DeepSimTemplateScan", value); }
		}
		public bool EnableSound
		{
			get { XmlRegistryKey rkf = xrk.CreateSubKey("Settings"); return Convert.ToBoolean(rkf.GetValue("EnableSound", true)); }
			set { XmlRegistryKey rkf = xrk.CreateSubKey("Settings"); rkf.SetValue("EnableSound", value); }
		}
		public bool FirefoxTabbing
		{
			get { XmlRegistryKey rkf = xrk.CreateSubKey("Settings"); return Convert.ToBoolean(rkf.GetValue("FirefoxTabbing", false)); }
			set { XmlRegistryKey rkf = xrk.CreateSubKey("Settings"); rkf.SetValue("FirefoxTabbing", value); }
		}
		public string GmdcExtension
		{
			get { XmlRegistryKey rkf = xrk.CreateSubKey("Settings"); return (string)rkf.GetValue("GmdcExtension", "obj"); }
			set { XmlRegistryKey rkf = xrk.CreateSubKey("Settings"); rkf.SetValue("GmdcExtension", value); }
		}
		public int GraphLineMode
		{
			get { XmlRegistryKey rkf = xrk.CreateSubKey("Settings"); return Convert.ToInt32(rkf.GetValue("GraphLineMode", 0)); }
			set { XmlRegistryKey rkf = xrk.CreateSubKey("Settings"); rkf.SetValue("GraphLineMode", value); }
		}
		public bool GraphQuality
		{
			get { XmlRegistryKey rkf = xrk.CreateSubKey("Settings"); return Convert.ToBoolean(rkf.GetValue("GraphQuality", true)); }
			set { XmlRegistryKey rkf = xrk.CreateSubKey("Settings"); rkf.SetValue("GraphQuality", value); }
		}
		public double ImportExportScaleFactor
		{
			get { XmlRegistryKey rkf = xrk.CreateSubKey("Settings"); return Convert.ToDouble(rkf.GetValue("ImportExportScaleFactor", 1.0)); }
			set { XmlRegistryKey rkf = xrk.CreateSubKey("Settings"); rkf.SetValue("ImportExportScaleFactor", value); }
		}
		public int LayoutVersion
		{
			get { XmlRegistryKey rkf = xrk.CreateSubKey("Settings"); return Convert.ToInt32(rkf.GetValue("LayoutVersion", 0)); }
			set { XmlRegistryKey rkf = xrk.CreateSubKey("Settings"); rkf.SetValue("LayoutVersion", value); }
		}
		public bool LoadAllNeighbourhoods
		{
			get { XmlRegistryKey rkf = xrk.CreateSubKey("Settings"); return Convert.ToBoolean(rkf.GetValue("LoadAllNeighbourhoods", false)); }
			set { XmlRegistryKey rkf = xrk.CreateSubKey("Settings"); rkf.SetValue("LoadAllNeighbourhoods", value); }
		}
		public bool LoadOWFast
		{
			get { XmlRegistryKey rkf = xrk.CreateSubKey("Settings"); return Convert.ToBoolean(rkf.GetValue("LoadOWFast", true)); }
			set { XmlRegistryKey rkf = xrk.CreateSubKey("Settings"); rkf.SetValue("LoadOWFast", value); }
		}
		public bool LoadTableAtStartup
		{
			get { XmlRegistryKey rkf = xrk.CreateSubKey("Settings"); return Convert.ToBoolean(rkf.GetValue("LoadTableAtStartup", false)); }
			set { XmlRegistryKey rkf = xrk.CreateSubKey("Settings"); rkf.SetValue("LoadTableAtStartup", value); }
		}
		public bool LockDocks
		{
			get { XmlRegistryKey rkf = xrk.CreateSubKey("Settings"); return Convert.ToBoolean(rkf.GetValue("LockDocks", false)); }
			set { XmlRegistryKey rkf = xrk.CreateSubKey("Settings"); rkf.SetValue("LockDocks", value); }
		}
		public int MaxSearchResults
		{
			get { XmlRegistryKey rkf = xrk.CreateSubKey("Settings"); return Convert.ToInt32(rkf.GetValue("MaxSearchResults", 100)); }
			set { XmlRegistryKey rkf = xrk.CreateSubKey("Settings"); rkf.SetValue("MaxSearchResults", value); }
		}
		public int OWThumbSize
		{
			get { XmlRegistryKey rkf = xrk.CreateSubKey("Settings"); return Convert.ToInt32(rkf.GetValue("OWThumbSize", 64)); }
			set { XmlRegistryKey rkf = xrk.CreateSubKey("Settings"); rkf.SetValue("OWThumbSize", value); }
		}
		public bool OWincludewalls
		{
			get { XmlRegistryKey rkf = xrk.CreateSubKey("Settings"); return Convert.ToBoolean(rkf.GetValue("OWincludewalls", false)); }
			set { XmlRegistryKey rkf = xrk.CreateSubKey("Settings"); rkf.SetValue("OWincludewalls", value); }
		}
		public bool OWtrimnames
		{
			get { XmlRegistryKey rkf = xrk.CreateSubKey("Settings"); return Convert.ToBoolean(rkf.GetValue("OWtrimnames", true)); }
			set { XmlRegistryKey rkf = xrk.CreateSubKey("Settings"); rkf.SetValue("OWtrimnames", value); }
		}
		public enum ReportFormats { Html = 0, Csv = 1, CSV = 1, Text = 2 }
		public ReportFormats ReportFormat
		{
			get { XmlRegistryKey rkf = xrk.CreateSubKey("Settings"); return (ReportFormats)Convert.ToInt32(rkf.GetValue("ReportFormat", (int)ReportFormats.Html)); }
			set { XmlRegistryKey rkf = xrk.CreateSubKey("Settings"); rkf.SetValue("ReportFormat", (int)value); }
		}
		public bool ResoruceTreeAllwaysAutoselect
		{
			get { XmlRegistryKey rkf = xrk.CreateSubKey("Settings"); return Convert.ToBoolean(rkf.GetValue("ResoruceTreeAllwaysAutoselect", false)); }
			set { XmlRegistryKey rkf = xrk.CreateSubKey("Settings"); rkf.SetValue("ResoruceTreeAllwaysAutoselect", value); }
		}
		public enum ResourceListExtensionFormats { Short = 0, Long = 1, Hex = 2 }
		public ResourceListExtensionFormats ResourceListExtensionFormat
		{
			get { XmlRegistryKey rkf = xrk.CreateSubKey("Settings"); return (ResourceListExtensionFormats)Convert.ToInt32(rkf.GetValue("ResourceListExtensionFormat", (int)ResourceListExtensionFormats.Short)); }
			set { XmlRegistryKey rkf = xrk.CreateSubKey("Settings"); rkf.SetValue("ResourceListExtensionFormat", (int)value); }
		}
		public bool ResourceListInstanceFormatDecOnly
		{
			get { XmlRegistryKey rkf = xrk.CreateSubKey("Settings"); return Convert.ToBoolean(rkf.GetValue("ResourceListInstanceFormatDecOnly", false)); }
			set { XmlRegistryKey rkf = xrk.CreateSubKey("Settings"); rkf.SetValue("ResourceListInstanceFormatDecOnly", value); }
		}
		public bool ResourceListInstanceFormatHexOnly
		{
			get { XmlRegistryKey rkf = xrk.CreateSubKey("Settings"); return Convert.ToBoolean(rkf.GetValue("ResourceListInstanceFormatHexOnly", false)); }
			set { XmlRegistryKey rkf = xrk.CreateSubKey("Settings"); rkf.SetValue("ResourceListInstanceFormatHexOnly", value); }
		}
		public bool ResourceListShowExtensions
		{
			get { XmlRegistryKey rkf = xrk.CreateSubKey("Settings"); return Convert.ToBoolean(rkf.GetValue("ResourceListShowExtensions", true)); }
			set { XmlRegistryKey rkf = xrk.CreateSubKey("Settings"); rkf.SetValue("ResourceListShowExtensions", value); }
		}
		public bool ShowJointNames
		{
			get { XmlRegistryKey rkf = xrk.CreateSubKey("Settings"); return Convert.ToBoolean(rkf.GetValue("ShowJointNames", false)); }
			set { XmlRegistryKey rkf = xrk.CreateSubKey("Settings"); rkf.SetValue("ShowJointNames", value); }
		}
		public bool ShowMoreSkills
		{
			get { XmlRegistryKey rkf = xrk.CreateSubKey("Settings"); return Convert.ToBoolean(rkf.GetValue("ShowMoreSkills", false)); }
			set { XmlRegistryKey rkf = xrk.CreateSubKey("Settings"); rkf.SetValue("ShowMoreSkills", value); }
		}
		public bool ShowObjdNames
		{
			get { XmlRegistryKey rkf = xrk.CreateSubKey("Settings"); return Convert.ToBoolean(rkf.GetValue("ShowObjdNames", true)); }
			set { XmlRegistryKey rkf = xrk.CreateSubKey("Settings"); rkf.SetValue("ShowObjdNames", value); }
		}
		public bool ShowProgressWhenPackageLoads
		{
			get { XmlRegistryKey rkf = xrk.CreateSubKey("Settings"); return Convert.ToBoolean(rkf.GetValue("ShowProgressWhenPackageLoads", false)); }
			set { XmlRegistryKey rkf = xrk.CreateSubKey("Settings"); rkf.SetValue("ShowProgressWhenPackageLoads", value); }
		}
		public bool ShowWaitBarPermanent
		{
			get { XmlRegistryKey rkf = xrk.CreateSubKey("Settings"); return Convert.ToBoolean(rkf.GetValue("ShowWaitBarPermanent", false)); }
			set { XmlRegistryKey rkf = xrk.CreateSubKey("Settings"); rkf.SetValue("ShowWaitBarPermanent", value); }
		}
		public bool ShowWelcomeOnStartup
		{
			get { XmlRegistryKey rkf = xrk.CreateSubKey("Settings"); return Convert.ToBoolean(rkf.GetValue("ShowWelcomeOnStartup", true)); }
			set { XmlRegistryKey rkf = xrk.CreateSubKey("Settings"); rkf.SetValue("ShowWelcomeOnStartup", value); }
		}
		public bool SimpleResourceSelect
		{
			get { XmlRegistryKey rkf = xrk.CreateSubKey("Settings"); return Convert.ToBoolean(rkf.GetValue("SimpleResourceSelect", false)); }
			set { XmlRegistryKey rkf = xrk.CreateSubKey("Settings"); rkf.SetValue("SimpleResourceSelect", value); }
		}
		public int SortProcessCount
		{
			get { XmlRegistryKey rkf = xrk.CreateSubKey("Settings"); return Convert.ToInt32(rkf.GetValue("SortProcessCount", 1)); }
			set { XmlRegistryKey rkf = xrk.CreateSubKey("Settings"); rkf.SetValue("SortProcessCount", value); }
		}
		public bool UpdateResourceListWhenTGIChanges
		{
			get { XmlRegistryKey rkf = xrk.CreateSubKey("Settings"); return Convert.ToBoolean(rkf.GetValue("UpdateResourceListWhenTGIChanges", true)); }
			set { XmlRegistryKey rkf = xrk.CreateSubKey("Settings"); rkf.SetValue("UpdateResourceListWhenTGIChanges", value); }
		}
		public bool UseCache
		{
			get { XmlRegistryKey rkf = xrk.CreateSubKey("Settings"); return Convert.ToBoolean(rkf.GetValue("UseCache", true)); }
			set { XmlRegistryKey rkf = xrk.CreateSubKey("Settings"); rkf.SetValue("UseCache", value); }
		}
		public bool UseMaxisGroupsCache
		{
			get { XmlRegistryKey rkf = xrk.CreateSubKey("Settings"); return Convert.ToBoolean(rkf.GetValue("UseMaxisGroupsCache", true)); }
			set { XmlRegistryKey rkf = xrk.CreateSubKey("Settings"); rkf.SetValue("UseMaxisGroupsCache", value); }
		}
		public bool WaitingScreenTopMost
		{
			get { XmlRegistryKey rkf = xrk.CreateSubKey("Settings"); return Convert.ToBoolean(rkf.GetValue("WaitingScreenTopMost", false)); }
			set { XmlRegistryKey rkf = xrk.CreateSubKey("Settings"); rkf.SetValue("WaitingScreenTopMost", value); }
		}
	}
}