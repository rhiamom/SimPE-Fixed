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
using System.IO;
using System.Collections;
using SimPe;
using SimPe.Plugin;


namespace SimPe.Cache
{
	/// <summary>
	/// Contains an Instance of a CacheFile
	/// </summary>
	public class MemoryCacheFile: CacheFile
	{
        static bool isBuilding = false;
        static MemoryCacheFile sharedCache = null;

        /// <summary>
        /// Updates and Loads the Memory Cache
        /// </summary>
        /// <returns></returns>
        public static MemoryCacheFile InitCacheFile()
		{
			// FileTable.FileIndex.Load() can scan thousands of expansion files on
			// first call; surface a message so the user knows what's happening
			// instead of seeing a frozen UI for several seconds.
			if (sharedCache == null && !isBuilding)
			{
				Wait.SubStart(100);
				Wait.Message = "Scanning game folders...";
				Wait.Progress = 0;
				try { FileTable.FileIndex.Load(); }
				finally { Wait.SubStop(); }
			}
			else
			{
				FileTable.FileIndex.Load();
			}
			return InitCacheFile(FileTable.FileIndex);
		}
        /// <summary>
        /// Updates and Loads the Memory Cache
        /// </summary>
        /// <returns></returns>
        public static MemoryCacheFile InitCacheFile(SimPe.Interfaces.Scenegraph.IScenegraphFileIndex fileindex)
        {
            if (sharedCache != null && !isBuilding)
                return sharedCache;

            if (isBuilding)
                return sharedCache ?? new MemoryCacheFile();

            isBuilding = true;
            sharedCache = new MemoryCacheFile();

            try
            {
                // Use SubStart(100) so the wait bar's progress strip is visible from
                // the very first tick — SubStart() with no arg leaves ShowProgress
                // false and the bar shows just a "Please Wait" text that's easy to
                // miss in the 22-pixel bottom status strip.
                Wait.SubStart(100);
                Wait.Message = "Loading Memory Cache...";
                Wait.Progress = 0;

                string cachePath = Helper.SimPeLanguageCache;
                System.Diagnostics.Debug.WriteLine("MemoryCache loading from: " + cachePath);

                bool missing = !File.Exists(cachePath);

                if (!missing)
                {
                    try
                    {
                        Wait.Message = "Reading Memory Cache...";
                        sharedCache.Load(cachePath, true);
                    }
                    catch (Exception ex)
                    {
                        long len = -1;
                        try { len = new FileInfo(cachePath).Length; } catch { }

                        //System.Diagnostics.Debug.WriteLine("MemoryCache: LOAD FAILED: " + ex.ToString());
                        //System.Diagnostics.Debug.WriteLine("MemoryCache: cachePath=" + cachePath + " length=" + len);

                        // Force rebuild, but keep evidence.
                        missing = true;

                        try
                        {
                            string badPath = cachePath + ".bad";
                            if (File.Exists(badPath)) File.Delete(badPath);
                            File.Move(cachePath, badPath);
                            System.Diagnostics.Debug.WriteLine("MemoryCache: moved bad cache to: " + badPath);
                        }
                        catch (Exception moveEx)
                        {
                            System.Diagnostics.Debug.WriteLine("MemoryCache: could not move bad cache: " + moveEx.ToString());
                            // do NOT delete
                        }

                        sharedCache = new MemoryCacheFile();
                    }
                }

                if (missing)
                {
                    System.Diagnostics.Debug.WriteLine("MemoryCache: rebuilding...");
                    Wait.Message = "Building Memory Cache (first run)...";
                    sharedCache.ReloadCache(fileindex, true);
                }

                Wait.Message = "Memory Cache ready";
                Wait.SubStop();
                return sharedCache;
            }
            finally
            {
                isBuilding = false;
            }
        }

        public void ReloadCache()
		{
			ReloadCache(true);
		}

		public void ReloadCache(bool save)
		{
			FileTable.FileIndex.Load();
			ReloadCache(FileTable.FileIndex, save);
		}

		public void ReloadCache(SimPe.Interfaces.Scenegraph.IScenegraphFileIndex fileindex, bool save)
		{
            Interfaces.Scenegraph.IScenegraphFileIndexItem[] items = fileindex.FindFile(Data.MetaData.OBJD_FILE, true);

            bool added = false;
            Wait.MaxProgress = items.Length;
            Wait.Message = "Updating Cache";
            int ct = 0;
            // Updating Wait.Progress / Wait.Message every iteration is what made
            // the cache build take 10 minutes — each setter calls Application.DoEvents()
            // which pumps the whole Windows message queue. Pump only once per batch.
            int progressStep = System.Math.Max(1, items.Length / 100);
			foreach (SimPe.Interfaces.Scenegraph.IScenegraphFileIndexItem item in items)
			{
				Interfaces.Scenegraph.IScenegraphFileIndexItem[] citems = this.FileIndex.FindFile(item.GetLocalFileDescriptor(), null);
				if (citems.Length==0)
				{

					SimPe.PackedFiles.Wrapper.ExtObjd objd = new SimPe.PackedFiles.Wrapper.ExtObjd();
					objd.ProcessData(item);

					this.AddItem(objd);
					added = true;
				}
                ct++;
                if (ct % progressStep == 0) Wait.Progress = ct;
			}
            Wait.Progress = items.Length;
            if (added) 
			{
				this.map = null;
                Wait.Message = "Saving Cache";
                if (save) this.Save(Helper.SimPeLanguageCache);
                this.LoadMemTable();
				this.LoadMemList();
			}			
		}

		/// <summary>
		/// Creaet a new Instance for an empty File
		/// </summary>
		public MemoryCacheFile() : base()
		{
			DEFAULT_TYPE = ContainerType.Memory;
		}		

		/// <summary>
		/// Add a MaterialOverride to the Cache
		/// </summary>
		/// <param name="objd">The Object Data File</param>
		public MemoryCacheItem AddItem(SimPe.PackedFiles.Wrapper.ExtObjd objd) 
		{
			CacheContainer mycc = this.UseConatiner(ContainerType.Memory, objd.Package.FileName);
			
			MemoryCacheItem mci = new MemoryCacheItem();			
			mci.FileDescriptor = objd.FileDescriptor;
			mci.Guid = objd.Guid;			
			mci.ObjectType = objd.Type;		
			mci.ObjdName = objd.FileName;
			mci.ParentCacheContainer = mycc;

			try
			{
                Interfaces.Scenegraph.IScenegraphFileIndexItem[] sitems = FileTable.FileIndex.FindFile(Data.MetaData.CTSS_FILE, objd.FileDescriptor.Group, objd.CTSSInstance + (ulong)1, null);
                if (sitems.Length == 0)
                    sitems = FileTable.FileIndex.FindFile(Data.MetaData.CTSS_FILE, objd.FileDescriptor.Group, objd.CTSSInstance, null);
                if (sitems.Length>0)
				{
					SimPe.PackedFiles.Wrapper.Str str = new SimPe.PackedFiles.Wrapper.Str();
					str.ProcessData(sitems[0]);
					SimPe.PackedFiles.Wrapper.StrItemList strs = str.LanguageItems(Helper.WindowsRegistry.LanguageCode);																
					if (strs.Length>0) mci.Name = strs[0].Title;
								
					//not found?
					if (mci.Name== "") 
					{
						strs = str.LanguageItems(1);																
						if (strs.Length>0) mci.Name = strs[0].Title;
					}							
				}
            }
			catch (Exception) {}

			try 
			{
				Interfaces.Scenegraph.IScenegraphFileIndexItem[] sitems = FileTable.FileIndex.FindFile(Data.MetaData.STRING_FILE, objd.FileDescriptor.Group, 0x100, null);
				if (sitems.Length>0) 
				{
					SimPe.PackedFiles.Wrapper.Str str = new SimPe.PackedFiles.Wrapper.Str();
					str.ProcessData(sitems[0]);
					SimPe.PackedFiles.Wrapper.StrItemList strs = str.LanguageItems(Data.MetaData.Languages.English);																
					string[] res = new string[strs.Count];
					for (int i=0; i<res.Length; i++)					
						res[i] = strs[i].Title;
					mci.ValueNames = res;
				}
            }
			catch (Exception) {}
			
			//still no name?
			if (mci.Name == "") mci.Name = objd.FileName;
            //having an icon?
			SimPe.PackedFiles.Wrapper.Picture pic = new SimPe.PackedFiles.Wrapper.Picture();
            Interfaces.Scenegraph.IScenegraphFileIndexItem[] iitems;
            if (mci.IsBadge)
                iitems = FileTable.FileIndex.FindFile(Data.MetaData.SIM_IMAGE_FILE, objd.FileDescriptor.Group, 3, null);
            else
                iitems = FileTable.FileIndex.FindFile(Data.MetaData.SIM_IMAGE_FILE, objd.FileDescriptor.Group, 1, null);	
			if (iitems.Length>0)
			{
				pic.ProcessData(iitems[0]);
				mci.Icon = pic.Image;
			}

			// Per-item Wait.Message / Wait.Image were here too — both setters call
			// Application.DoEvents() in WaitingBar, so updating them per OBJD pumped
			// the message queue thousands of times during cache build.
			mycc.Items.Add(mci);

			return mci;
		}

		Hashtable map;
		/// <summary>
		/// Return the FileIndex represented by the Cached Files
		/// </summary>
		public Hashtable Map 
		{
			get { 
				if (map==null) LoadMem();
				return map; 
			}
		}

		/// <summary>
		/// Creates the Map
		/// </summary>
		/// <returns>the FileIndex</returns>
		/// <remarks>
		/// The Tags of the FileDescriptions contain the MMATCachItem Object, 
		/// the FileNames of the FileDescriptions contain the Name of the package File
		/// </remarks>
		public void LoadMem()
		{
			map = new Hashtable();
			

			foreach (CacheContainer cc in Containers) 
			{
				if (cc.Type==ContainerType.Memory && cc.Valid) 
				{
					foreach (MemoryCacheItem mci in cc.Items) 
					{
						map[mci.Guid] = mci;
					}
				}
			}//foreach
		}	

		ArrayList list;
		/// <summary>
		/// Return a List of all cached Memory Items
		/// </summary>
		public ArrayList List
		{
			get 
			{ 
				if (list==null) LoadMemList();
				return list; 
			}
		}

		/// <summary>
		/// Creates the List
		/// </summary>
		/// <returns>the FileIndex</returns>
		/// <remarks>
		/// The Tags of the FileDescriptions contain the MMATCachItem Object, 
		/// the FileNames of the FileDescriptions contain the Name of the package File
		/// </remarks>
		public void LoadMemList()
		{
			list = new ArrayList();
			

			foreach (CacheContainer cc in Containers) 
			{
				if (cc.Type==ContainerType.Memory && cc.Valid) 
				{
					foreach (MemoryCacheItem mci in cc.Items) 
					{						
						list.Add(mci);
					}
				}
			}//foreach
		}
	
		FileIndex fi;
		/// <summary>
		/// Return the FileIndex represented by the Cached Files
		/// </summary>
		public FileIndex FileIndex 
		{
			get 
			{ 
				if (fi==null) LoadMemTable();
				return fi; 
			}
		}

		/// <summary>
		/// Creates a FileIndex with all available MMAT Files
		/// </summary>
		/// <returns>the FileIndex</returns>
		/// <remarks>
		/// The Tags of the FileDescriptions contain the MMATCachItem Object, 
		/// the FileNames of the FileDescriptions contain the Name of the package File
		/// </remarks>
		public void LoadMemTable()
		{
			fi = new FileIndex(new ArrayList());
			fi.Duplicates = false;
			
			foreach (CacheContainer cc in Containers) 
			{
				if (cc.Type==ContainerType.Memory && cc.Valid) 
				{
					foreach (MemoryCacheItem mci in cc.Items) 
					{
						Interfaces.Files.IPackedFileDescriptor pfd = mci.FileDescriptor;
						pfd.Filename = cc.FileName;
						fi.AddIndexFromPfd(pfd, null, FileIndex.GetLocalGroup(pfd.Filename));
					}
				}
			}//foreach
		}

		/// <summary>
		/// Returns an Alias for the given Guid
		/// </summary>
		/// <param name="guid"></param>
		/// <returns></returns>
		public MemoryCacheItem FindItem(uint guid)
		{
			MemoryCacheItem mci = (MemoryCacheItem)Map[guid];
			return mci;
		}

		/// <summary>
		/// Returns an Alias for the given Guid
		/// </summary>
		/// <param name="guid"></param>
		/// <returns></returns>
		public SimPe.Interfaces.IAlias FindObject(uint guid)
		{
			MemoryCacheItem mci = FindItem(guid);
			SimPe.Data.Alias a;
			if (mci==null)
			     a = new SimPe.Data.Alias(guid, Localization.Manager.GetString("Unknown"));
			else
				 a = new SimPe.Data.Alias(guid, mci.Name);

			object[] o = new object[3];
			o[0] = mci.FileDescriptor;
			o[1] = mci.ObjectType;
			o[2] = mci.Icon;
			a.Tag = o;

			return a;
		}		
	}
}
