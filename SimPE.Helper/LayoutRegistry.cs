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
using Microsoft.Win32;


namespace SimPe
{
	/// <summary>
	/// Handles Layout Settings for the Application
	/// </summary>
	/// <remarks>You cannot create instance of this class, use the 
	/// <see cref="SimPe.Helper.WindowsRegistry.Layout"/> Field to access the LayoutRegistry</remarks>
	public class LayoutRegistry
	{
		#region Attributes		

		/// <summary>
		/// The Root Registry Key for this Application
		/// </summary>
		XmlRegistryKey xrk;
		#endregion

		#region Management
		XmlRegistry reg;
		/// <summary>
		/// Creates a new Instance
		/// </summary>
		/// <param name="layoutkey">Key to the Layout</param>
        internal LayoutRegistry(XmlRegistryKey layoutkey)
        {
            reg = new XmlRegistry(Helper.DataFolder.Layout2XREG, Helper.DataFolder.Layout2XREGW, true);
            xrk = reg.CurrentUser.CreateSubKey(@"Software\Ambertation\SimPe\Layout");
        }

		/// <summary>
		/// Returns the Registry Key you can use to store Optional Plugin Data
		/// </summary>
		public XmlRegistryKey LayoutRegistryKey
		{
			get 
			{
				return xrk.CreateSubKey("PluginLayout");
			}
		}

		/// <summary>
        /// Descturtor -(Whats a Descturtor?)
		/// </summary>
		/// <remarks>
        /// Will flsuh the XmlRegistry to the disk -(Whats a flsuh?)
		/// </remarks>
		~LayoutRegistry()
		{
			//Flush();
		}

		/// <summary>
		/// Write the Settings to the Disk
		/// </summary>
		public void Flush() 
		{
			if (reg!=null) reg.Flush();
		}

        #endregion

        /// <summary>
        /// true if the Plugin action box should be presented expanded.
        /// Defaults to true on fresh installs to match prior behavior.
        /// </summary>
        public bool PluginActionBoxExpanded
        {
            get { return Convert.ToBoolean(xrk.GetValue("ActionPlugExpanded", true)); }
            set { xrk.SetValue("ActionPlugExpanded", value); }
        }

        /// <summary>
        /// true if the Tool action box should be presented expanded.
        /// </summary>
        public bool ToolActionBoxExpanded
        {
            get { return Convert.ToBoolean(xrk.GetValue("ActionToolExpanded", true)); }
            set { xrk.SetValue("ActionToolExpanded", value); }
        }

        /// <summary>
        /// true if the Default action box should be presented expanded.
        /// </summary>
        public bool DefaultActionBoxExpanded
        {
            get { return Convert.ToBoolean(xrk.GetValue("ActionDefExpanded", true)); }
            set { xrk.SetValue("ActionDefExpanded", value); }
        }
        // Returns null when the user has never saved a customization, so
        // MyButtonItem.GetLayoutInformations can distinguish "no saved state"
        // (default-show all) from an explicitly-saved empty list.
        public ArrayList VisibleToolbarButtons
        {
            get
            {
                object o = xrk.GetValue("TBButtons");
                return o as ArrayList;
            }
            set
            {
                xrk.SetValue("TBButtons", value);
            }
        }

        /// <summary>
        /// gets / sets the Theme for SimPe
        /// </summary>
        /// <remarks>Math.Min caps the maximum theme to 10 to prevent errors, must be increased to add another theme</remarks>
        public byte SelectedTheme
        {
            get
            {
                // Classic preset: no theming
                if (Helper.WindowsRegistry.Layout.IsClassicPreset)
                    return 0;

                // Read the stored theme id from the XML registry
                object o = xrk.GetValue("ThemeID", 0);

                int n = Convert.ToInt32(o);

                // Clamp to [0, 10] as in the original logic
                if (n < 0) n = 0;
                if (n > 10) n = 10;

                return (byte)n;
            }
            set
            {
                xrk.SetValue("ThemeID", (int)value);
            }
        }


        /// <summary>
        /// true if classic pre-set has been launched
        /// </summary>
        public bool IsClassicPreset
        {
            get
            {
                object o = xrk.GetValue("IsClassic", false);
                return Convert.ToBoolean(o);
            }
            set
            {
                xrk.SetValue("IsClassic", value);
            }
        }

        /// <summary>
        /// true if the Layout should be stored on exit
        /// </summary>
        public bool AutoStoreLayout
        {
            get
            {
                object o = xrk.GetValue("AutoLayout", true);
                return Convert.ToBoolean(o);
            }
            set
            {
                xrk.SetValue("AutoLayout", value);
            }
        }

        static string[] colNames = new string[] { "Name", "Type", "Group", "InstHi", "Inst", "Offset", "Size" };
        public System.Collections.Generic.List<string> ColumnOrder
        {
            get
            {
                string[] s = xrk.GetValue("ColumnOrder", String.Join(",", colNames)).ToString().Split(',');
                System.Collections.Generic.List<string> ls = new System.Collections.Generic.List<string>(s);
                System.Collections.Generic.List<string> lc = new System.Collections.Generic.List<string>(colNames);
                foreach (string v in s) if (!lc.Contains(v)) ls.Remove(v);
                foreach (string v in colNames) if (!ls.Contains(v)) ls.Add(v);
                return ls;
            }
            set
            {
                string[] s = value.ToArray();
                System.Collections.Generic.List<string> ls = new System.Collections.Generic.List<string>(s);
                System.Collections.Generic.List<string> lc = new System.Collections.Generic.List<string>(colNames);
                foreach (string v in s) if (!lc.Contains(v)) ls.Remove(v);
                foreach (string v in colNames) if (!ls.Contains(v)) ls.Add(v);
                xrk.SetValue("ColumnOrder", String.Join(",", ls.ToArray()));
            }
        }

		/// <summary>
		/// Width of the Column in the main Window
		/// </summary>
		public int NameColumnWidth
		{
			get 
			{
                object o = xrk.GetValue("NameColumnWidth", (int)280);
				return Convert.ToInt32(o);
			}
			set
			{
                xrk.SetValue("NameColumnWidth", value);
			}
        }

        /// <summary>
        /// Width of the Column in the main Window
        /// </summary>
        public int TypeColumnWidth
        {
            get
            {
                object o = xrk.GetValue("TypeColumnWidth", (int)70);
                return Convert.ToInt32(o);
            }
            set
            {
                xrk.SetValue("TypeColumnWidth", value);
            }
        }

		/// <summary>
		/// Width of the Column in the main Window
		/// </summary>
		public int GroupColumnWidth
		{
			get 
			{
                object o = xrk.GetValue("GroupColumnWidth", (int)120);
				return Convert.ToInt32(o);
			}
			set
			{
				xrk.SetValue("GroupColumnWidth", value);
			}
		}

		/// <summary>
		/// Width of the Column in the main Window
		/// </summary>
		public int InstanceHighColumnWidth
		{
			get 
			{
                object o = xrk.GetValue("InstanceHighColumnWidth", (int)120);
				return Convert.ToInt32(o);
			}
			set
			{
				xrk.SetValue("InstanceHighColumnWidth", value);
			}
		}

		/// <summary>
		/// Width of the Column in the main Window
		/// </summary>
		public int InstanceColumnWidth
		{
			get 
			{
                object o = xrk.GetValue("InstanceColumnWidth", (int)160);
				return Convert.ToInt32(o);
			}
			set
			{
				xrk.SetValue("InstanceColumnWidth", value);
			}
		}

		/// <summary>
		/// Width of the Column in the main Window
		/// </summary>
		public int OffsetColumnWidth
		{
			get 
			{
                object o = xrk.GetValue("OffsetColumnWidth", (int)120);
				return Convert.ToInt32(o);
			}
			set
			{
				xrk.SetValue("OffsetColumnWidth", value);
			}
		}

		/// <summary>
		/// Width of the Column in the main Window
		/// </summary>
		public int SizeColumnWidth
		{
			get 
			{
                object o = xrk.GetValue("SizeColumnWidth", (int)140);
				return Convert.ToInt32(o);
			}
			set
			{
				xrk.SetValue("SizeColumnWidth", value);
			}
		}        

        /// <summary>
        /// Which view mode the Resource Tree was last set to via its TGI
        /// buttons. One of: "Type" (default), "Group", "Instance". Persists
        /// across sessions so the user's chosen view sticks.
        /// </summary>
        public string ResourceTreeBuilder
        {
            get
            {
                object o = xrk.GetValue("ResourceTreeBuilder", "Type");
                return o == null ? "Type" : o.ToString();
            }
            set
            {
                xrk.SetValue("ResourceTreeBuilder", value ?? "Type");
            }
        }

        /// <summary>
        /// Which dock container the Object Workshop panel was last placed in.
        /// One of: "Bottom" (default — sibling tab of Plugin View), "Left", "Right", "Floating".
        /// </summary>
        public string OWDockContainer
        {
            get
            {
                object o = xrk.GetValue("OWDockContainer", "Bottom");
                return o == null ? "Bottom" : o.ToString();
            }
            set
            {
                xrk.SetValue("OWDockContainer", value ?? "Bottom");
            }
        }

        #region Window bounds

        // Stored as four ints under the "Window" subkey so the main form can
        // come back at the user's last position/size. Sentinel value -1 on the
        // getters means "no stored bounds — caller should leave the form alone."

        XmlRegistryKey WindowKey { get { return xrk.CreateSubKey("Window"); } }

        public int WindowX      { get { return Convert.ToInt32(WindowKey.GetValue("X",      -1)); } set { WindowKey.SetValue("X",      value); } }
        public int WindowY      { get { return Convert.ToInt32(WindowKey.GetValue("Y",      -1)); } set { WindowKey.SetValue("Y",      value); } }
        public int WindowWidth  { get { return Convert.ToInt32(WindowKey.GetValue("Width",  -1)); } set { WindowKey.SetValue("Width",  value); } }
        public int WindowHeight { get { return Convert.ToInt32(WindowKey.GetValue("Height", -1)); } set { WindowKey.SetValue("Height", value); } }
        public bool WindowMaximized { get { return Convert.ToBoolean(WindowKey.GetValue("Maximized", false)); } set { WindowKey.SetValue("Maximized", value); } }

        public bool HasStoredWindowBounds
        {
            get { return WindowWidth > 0 && WindowHeight > 0; }
        }

        #endregion

        #region Dock area split sizes

        // The three side dock containers' widths/heights — i.e. where the
        // user dragged the splitters between the main work area and the
        // left/right/bottom panel strips. Stored under a "DockAreas" subkey
        // with sentinel -1 = "no stored value, use designer default."

        XmlRegistryKey DockAreasKey { get { return xrk.CreateSubKey("DockAreas"); } }

        public int DockLeftWidth    { get { return Convert.ToInt32(DockAreasKey.GetValue("LeftWidth",    -1)); } set { DockAreasKey.SetValue("LeftWidth",    value); } }
        public int DockRightWidth   { get { return Convert.ToInt32(DockAreasKey.GetValue("RightWidth",   -1)); } set { DockAreasKey.SetValue("RightWidth",   value); } }
        public int DockBottomHeight { get { return Convert.ToInt32(DockAreasKey.GetValue("BottomHeight", -1)); } set { DockAreasKey.SetValue("BottomHeight", value); } }

        #endregion

        #region Per-panel layout state

        /// <summary>
        /// Snapshot of one DockPanel's saveable state. `Container` is one of
        /// "Left" / "Right" / "Bottom" / "Manager" / "Floating" / null (not
        /// stored). FloatingX/Y are only meaningful when Container=="Floating".
        /// </summary>
        public class PanelState
        {
            public string Name;
            public string Container;
            public bool IsOpen;
            public int Width;
            public int Height;
            public int FloatingX;
            public int FloatingY;
        }

        XmlRegistryKey PanelsKey { get { return xrk.CreateSubKey("Panels"); } }

        /// <summary>
        /// Returns the saved state for a panel by name, or null if none stored.
        /// </summary>
        public PanelState GetPanelState(string name)
        {
            if (string.IsNullOrEmpty(name)) return null;
            XmlRegistryKey k = PanelsKey.OpenSubKey(name, false);
            if (k == null) return null;
            string container = k.GetValue("Container") as string;
            if (string.IsNullOrEmpty(container)) return null;
            return new PanelState
            {
                Name      = name,
                Container = container,
                IsOpen    = Convert.ToBoolean(k.GetValue("IsOpen",    true)),
                Width     = Convert.ToInt32  (k.GetValue("Width",     -1)),
                Height    = Convert.ToInt32  (k.GetValue("Height",    -1)),
                FloatingX = Convert.ToInt32  (k.GetValue("FloatingX", 0)),
                FloatingY = Convert.ToInt32  (k.GetValue("FloatingY", 0)),
            };
        }

        public void SetPanelState(PanelState s)
        {
            if (s == null || string.IsNullOrEmpty(s.Name)) return;
            XmlRegistryKey k = PanelsKey.CreateSubKey(s.Name);
            k.SetValue("Container", s.Container ?? "");
            k.SetValue("IsOpen",    s.IsOpen);
            k.SetValue("Width",     s.Width);
            k.SetValue("Height",    s.Height);
            k.SetValue("FloatingX", s.FloatingX);
            k.SetValue("FloatingY", s.FloatingY);
        }

        public string[] StoredPanelNames
        {
            get { return PanelsKey.GetSubKeyNames(); }
        }

        /// <summary>
        /// Wipes every persisted bit of UI layout state — window bounds,
        /// dock-area splitter sizes, and every per-panel state entry. Called
        /// from Reset Layout so the next ReloadLayout starts from the
        /// designer defaults rather than the user's prior arrangement.
        /// </summary>
        public void ClearLayoutState()
        {
            try { xrk.DeleteSubKey("Panels",    false); } catch { }
            try { xrk.DeleteSubKey("Window",    false); } catch { }
            try { xrk.DeleteSubKey("DockAreas", false); } catch { }
        }

        #endregion

        /*
		#region Obsolete
		/// <summary>
		/// true if the taskBox should be presented expanded
		/// </summary>
		public bool DefaultActionBoxExpanded
		{
			get 
			{
				object o = xrk.GetValue("ActionDefExpanded", true);
				return Convert.ToBoolean(o);
			}
			set
			{
				xrk.SetValue("ActionDefExpanded", value);
			}
		}

		/// <summary>
		/// true if the taskBox should be presented expanded
		/// </summary>
		public bool ToolActionBoxExpanded
		{
			get 
			{
				object o = xrk.GetValue("ActionToolExpanded", false);
				return Convert.ToBoolean(o);
			}
			set
			{
				xrk.SetValue("ActionToolExpanded", value);
			}
		}

		/// <summary>
		/// true if the taskBox should be presented expanded
		/// </summary>
		public bool PluginActionBoxExpanded
		{
			get 
			{
				object o = xrk.GetValue("ActionPlugExpanded", false);
				return Convert.ToBoolean(o);
			}
			set
			{
				xrk.SetValue("ActionPlugExpanded", value);
			}
        }
        */
	}
}
