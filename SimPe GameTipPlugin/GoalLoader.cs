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
using System.Collections;
using SimPe.Interfaces.Plugin;
using System.Windows.Forms;
using SimPe.Cache;


namespace SimPe.Plugin
{
	/// <summary>
	/// This basically is a Class describing the Goals (loaded from the Cache)
	/// </summary>
	public class GoalCacheInformation  : GoalInformation
	{
		/// <summary>
        /// Use GoalInformation::LoadGoal() to create a new Instance
		/// </summary>
        /// <param name="guid">The guid of the Goal</param>
		GoalCacheInformation(uint guid):base(guid)
		{
			name = "";
		}

		/// <summary>
        /// Use GoalInformation::LoadGoal() to create a new Instance
		/// </summary>
		GoalCacheInformation():base()
		{
			name = "";
		}

		/// <summary>
		/// Load Informations about a specific Goal
		/// </summary>
		/// <param name="guid">The GUID of the goal</param>
		/// <returns>A Goal Information Structure</returns>
		public static GoalCacheInformation LoadGoal(SimPe.Cache.GoalCacheItem wci) 
		{
			GoalCacheInformation ret = new GoalCacheInformation();
			ret.icon = wci.Icon;
			ret.name = wci.Name;
			ret.guid = wci.Guid;

			XGoal w = new XGoal();
			SimPe.PackedFiles.Wrapper.CpfItem i = new SimPe.PackedFiles.Wrapper.CpfItem(); i.Name = "id"; i.UIntegerValue = wci.Guid; w.AddItem(i, true);
			i = new SimPe.PackedFiles.Wrapper.CpfItem(); i.Name = "score"; i.IntegerValue = wci.Score; w.AddItem(i, true);
			i = new SimPe.PackedFiles.Wrapper.CpfItem(); i.Name = "influence"; i.IntegerValue = wci.Influence; w.AddItem(i, true);

			ret.wnt = w;
			
			return ret;
		}

		System.Drawing.Image icon;
		public override System.Drawing.Image Icon
		{
			get
			{
				return icon;
			}
		}

		string name;
		public override string Name
		{
			get
			{
				return name;
			}
		}
	}

	/// <summary>
	/// This basically is a Class describing the Goals
	/// </summary>
	public class GoalInformation 
	{
		protected XGoal wnt; // Fuick
		SimPe.PackedFiles.Wrapper.Str str;
		SimPe.PackedFiles.Wrapper.Picture primicon;
		protected uint guid;
		internal string prefix="";

		static Hashtable goalcache;

		/// <summary>
		/// Use GoalInformation::LoadGoal() to create a new Instance
		/// </summary>
		protected GoalInformation() 
		{
		}

		/// <summary>
        /// Use WGoalInformation::LoadGoal() to create a new Instance
		/// </summary>
		/// <param name="guid">The guid of the Goal</param>
		protected GoalInformation(uint guid)
		{
			this.guid = guid;
			
			wnt = GoalLoader.GetGoal(guid);
			str = GoalLoader.LoadText(wnt);
			primicon = GoalLoader.LoadIcon(wnt);				 				
		}

		#region Cache 
		static GoalCacheFile cachefile;
        static bool hasnew = false;

		static void LoadCache()
		{
			if (cachefile!=null) return;

            cachefile = new GoalCacheFile();
            if (!Helper.XmlRegistry.UseCache) return;
			try 
			{
				cachefile.Load(Helper.SimPeLanguageCache, true);
			} 
			catch (Exception ex)
			{
				Helper.ExceptionMessage("", ex);
			}
		}

		/// <summary>
		/// Save the Cache to the FileSystem
		/// </summary>
		public static void SaveCache()
		{
            if (!Helper.XmlRegistry.UseCache) return;
            if (cachefile == null || !hasnew) return;
			cachefile.Save(Helper.SimPeLanguageCache);
		}
		#endregion

		/// <summary>
		/// Load Informations about a specific Goal
		/// </summary>
		/// <param name="guid">The GUID of the goal</param>
		/// <returns>A Goal Information Structure</returns>
		public static GoalInformation LoadGoal(uint guid) 
		{
			LoadCache();
			if (goalcache == null) goalcache = cachefile.Map;

			if (goalcache.ContainsKey(guid)) 
			{
				object o = goalcache[guid];
				GoalInformation wf;
				if (o.GetType()==typeof(GoalInformation)) 
					 wf = (GoalInformation)o;
				else 
					wf = GoalCacheInformation.LoadGoal((GoalCacheItem)o);

				return wf;
			}  
			else 
			{
				GoalInformation wf = new GoalInformation(guid);
				goalcache[guid] = wf;
                cachefile.AddItem(wf);
                hasnew = true;
                return wf;
			}
		}

		/// <summary>
		/// Returns the XGoal File
		/// </summary>
		public XGoal XGoal
		{
			get {return wnt;}
			
		}

		/// <summary>
		/// Returns the Name of this Goal
		/// </summary>
		public virtual string Name 
		{
			get 
			{
                string stg;
				if (str==null) return "0x"+Helper.HexString(guid);
                stg = str.FallbackedLanguageItem(Helper.XmlRegistry.LanguageCode, 0).Title.Replace("$NeighborLocal:2", "True Love");
                return stg.Replace("$NeighborLocal:3", "Orangutan");
			}
		}

		/// <summary>
        /// Returns Icon for this Goal or null
		/// </summary>
		public virtual System.Drawing.Image Icon
		{
			get 
			{
				if (primicon==null) return null;
				return primicon.Image;
			}
		}

		/// <summary>
		/// The guid of the current Goal
		/// </summary>
		public uint Guid 
		{
			get { return guid; }
		}

		public override string ToString()
		{
			return prefix+Name;
		}
	}

	/// <summary>
	/// This class is used to fill the UI for this FileType with Data
	/// </summary>
	public class GoalLoader
	{
		static Hashtable goals = null;
		static SimPe.Packages.File txtpkg = null;

		/// <summary>
        /// Returns a Hashtable of all available Goals
		/// </summary>
        /// <remarks>key is the Goal GUID, value is a XGoal object</remarks>
        public static Hashtable Goals 
		{
			get 
			{
                if (goals == null) LoadGoals();
				return goals;
			}
		}

		/// <summary>
		/// Loads the Text Package File
		/// </summary>
		static void LoadTextPackage() // fuck
		{
            txtpkg = SimPe.Packages.File.LoadFromFile(System.IO.Path.Combine(PathProvider.Global.GetExpansion(SimPe.Expansions.IslandStories).InstallFolder, "TSData\\Res\\Text\\Wants.package"));
            if (txtpkg == null) txtpkg = SimPe.Packages.File.LoadFromFile(System.IO.Path.Combine(SimPe.PathProvider.Global.Latest.InstallFolder, "TSData\\Res\\Text\\Wants.package"));
		}

		/// <summary>
        /// Load the availableGoals
		/// </summary>
        static void LoadGoals()
        {
            if (PathProvider.Global.GetExpansion(SimPe.Expansions.IslandStories).Exists && Helper.XmlRegistry.LoadOnlySimsStory != 28)
            {
                string gly = (System.IO.Path.Combine(PathProvider.Global.GetExpansion(SimPe.Expansions.IslandStories).InstallFolder, "TSData\\Res\\Wants\\Goals.package"));
                FileTable.FileIndex.AddIndexFromPackage(gly);
                gly = (System.IO.Path.Combine(PathProvider.Global.GetExpansion(SimPe.Expansions.IslandStories).InstallFolder, "TSData\\Res\\UI\\ui.package"));
                FileTable.FileIndex.AddIndexFromPackage(gly);
            }
			goals = new Hashtable();

			FileTable.FileIndex.Load();
            SimPe.Interfaces.Scenegraph.IScenegraphFileIndexItem[] wtss = FileTable.FileIndex.FindFile(0xBEEF7B4D, true);

			foreach (SimPe.Interfaces.Scenegraph.IScenegraphFileIndexItem wts in wtss)
			{
				goals[wts.FileDescriptor.Instance] = wts;
			}
		}

		/// <summary>
        /// Returns a XGoal File for this Item or null
		/// </summary>
        /// <param name="guid">The GUID of the Goal</param>
        /// <returns>The XGoal Object representing That Goal (or null if not found)</returns>
        public static XGoal GetGoal(uint guid)
		{
            if (goals == null) LoadGoals();

			SimPe.Interfaces.Scenegraph.IScenegraphFileIndexItem wts = (SimPe.Interfaces.Scenegraph.IScenegraphFileIndexItem)goals[guid];
			if (wts!=null) 
			{
				XGoal xwnt = new XGoal();
				wts.FileDescriptor.UserData = wts.Package.Read(wts.FileDescriptor).UncompressedData;
				xwnt.ProcessData(wts);

				return xwnt;
			}

			return null;
		}

		/// <summary>
        /// Returns the String File describing that Goal
		/// </summary>
        /// <param name="wnt">The Goal File</param>
		/// <returns>The Str File or null if none was found</returns>
		public static SimPe.PackedFiles.Wrapper.Str LoadText(XGoal wnt)
		{
			if (wnt==null) return null;
			if (txtpkg==null) LoadTextPackage();

			Interfaces.Files.IPackedFileDescriptor[] pfds = txtpkg.FindFile(Data.MetaData.STRING_FILE, 0, wnt.StringInstance);
			if (pfds.Length>0) 
			{
				SimPe.PackedFiles.Wrapper.Str str = new SimPe.PackedFiles.Wrapper.Str();
				pfds[0].UserData = txtpkg.Read(pfds[0]).UncompressedData;
				str.ProcessData(pfds[0], txtpkg);

				return str;
			}

			return null;
		}	
	
		/// <summary>
        /// Returns the Icon File for the passed Goal
		/// </summary>
        /// <param name="wnt">The Goal File</param>
		/// <returns>The Picture File or null if none was found</returns>
		public static SimPe.PackedFiles.Wrapper.Picture LoadIcon(XGoal wnt)
		{
			if (wnt==null) return null;
			if (txtpkg==null) LoadTextPackage();

			Interfaces.Scenegraph.IScenegraphFileIndexItem[] items = FileTable.FileIndex.FindFile(wnt.IconFileDescriptor, null);
			if (items.Length>0) 
			{
				SimPe.PackedFiles.Wrapper.Picture pic = new SimPe.PackedFiles.Wrapper.Picture();
				items[0].FileDescriptor.UserData = items[0].Package.Read(items[0].FileDescriptor).UncompressedData;
				pic.ProcessData(items[0]);

				return pic;
			}
			return null;
		}
	}

}
