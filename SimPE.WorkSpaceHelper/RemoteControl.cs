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
using Avalonia.Controls;
using SimPe.Interfaces;

namespace SimPe
{
	/// <summary>
	/// This class can be used to control SimPe from a Plugin.
	/// </summary>
	public class RemoteControl
	{
		public class ControlEventArgs : System.EventArgs
		{
			object[] data;
			uint target;
			public ControlEventArgs(uint target) : this(target, new object[0]) {}
			public ControlEventArgs(uint target, object data) : this(target, new object[]{data}) {}
			public ControlEventArgs(uint target, object[] data)
			{
				if (data==null) data = new object[0];
				this.data = data;

				this.target = target;
			}

			public uint TargetType
			{
				get {return target;}
			}

			public object Item
			{
				get
				{
					if (data.Length==0) return null;
					else return data[0];
				}
			}

			public object Items
			{
				get
				{
					return data;
				}
			}
		}

		struct MessageQueueItemInfo
		{
			public uint target;
			public ControlEvent fkt;
		}

		public delegate void ControlEvent(object sender, ControlEventArgs e);
		static System.Collections.ArrayList events = new System.Collections.ArrayList();

		public static void HookToMessageQueue(uint type, ControlEvent fkt)
		{
			MessageQueueItemInfo mqi = new MessageQueueItemInfo();
			mqi.target = type;
			mqi.fkt = fkt;

			events.Add(mqi);
		}

		public static void UnhookFromMessageQueue(uint type, ControlEvent fkt)
		{
			for (int i=events.Count-1; i>=0; i--)
			{
				MessageQueueItemInfo mqi = (MessageQueueItemInfo)events[i];
				if (mqi.target == type)
					if (mqi.fkt == fkt)
						events.RemoveAt(i);
			}
		}

		public static void AddMessage(object sender, ControlEventArgs e)
		{
			foreach (MessageQueueItemInfo mqi in events)
			{
				if (mqi.target == e.TargetType || mqi.target==0xffffffff || e.TargetType==0xffffffff)
					mqi.fkt(sender, e);
			}
		}

		/// <summary>
		/// Delegate you have to implement for the remote Package opener
		/// </summary>
		public delegate bool OpenPackageDelegate(string filename);

		/// <summary>
		/// Delegate you have to implement for the remote Package opener
		/// </summary>
		public delegate bool OpenMemPackageDelegate(SimPe.Interfaces.Files.IPackageFile pkg);

		/// <summary>
		/// Delegate you have to implement for the Remote PackedFile Opener
		/// </summary>
		public delegate bool OpenPackedFileDelegate(SimPe.Interfaces.Scenegraph.IScenegraphFileIndexItem fii);

		/// <summary>
		/// Used to show/hide a Dock
		/// </summary>
        public delegate void ShowDockDelegate(Ambertation.Windows.Forms.DockPanel doc, bool hide);

		#region Application Window
		static Window appform;
		/// <summary>
		/// Returns the Main Application Window
		/// </summary>
		public static Window ApplicationForm
		{
			get { return appform; }
			set {
				appform = value;
				if (appform != null)
					appstate = appform.WindowState;
				else
					appstate = WindowState.Maximized;
			}
		}

		static bool VisibleForm(Window form)
		{
			return form.ShowInTaskbar;
		}

		public static void ShowSubForm(Window form)
		{
			bool hide = VisibleForm(form);
			if (hide) HideApplicationForm();
			form.Show();
			if (hide) ShowApplicationForm();
		}

		public static void HideApplicationForm()
		{
			if (ApplicationForm == null) return;
			if (ApplicationForm.IsVisible)
				ApplicationForm.Hide();
		}

		public static void ShowApplicationForm()
		{
			if (ApplicationForm == null) return;
			if (!ApplicationForm.IsVisible)
			{
				ApplicationForm.Show();
				ApplicationForm.ShowInTaskbar = true;
			}
		}

		static WindowState appstate;
		public static void MinimizeApplicationForm()
		{
			if (ApplicationForm == null) return;
			if (ApplicationForm.WindowState != WindowState.Minimized)
			{
				appstate = ApplicationForm.WindowState;
				ApplicationForm.WindowState = WindowState.Minimized;
			}
		}

		public static void RestoreApplicationForm()
		{
			if (ApplicationForm == null) return;
			if (ApplicationForm.WindowState == WindowState.Minimized)
				ApplicationForm.WindowState = appstate;
		}
		#endregion

		static ShowDockDelegate sdd;
		/// <summary>
		/// Returns/Sets the ShowDock Delegate
		/// </summary>
		public static ShowDockDelegate ShowDockFkt
		{
			get { return sdd; }
			set { sdd = value; }
		}

		static OpenPackedFileDelegate opf;
		/// <summary>
		/// Returns/Sets the Function that should be called if you want to open a PackedFile
		/// </summary>
		public static OpenPackedFileDelegate OpenPackedFileFkt
		{
			get { return opf; }
			set { opf = value; }
		}

		static OpenPackageDelegate op;
		/// <summary>
		/// Returns/Sets the Function that should be called if you want to open a PackedFile
		/// </summary>
		public static OpenPackageDelegate OpenPackageFkt
		{
			get { return op; }
			set { op = value; }
		}

		/// <summary>
		/// Show/Hide a given Dock
		/// </summary>
		public static void ShowDock(Ambertation.Windows.Forms.DockPanel doc, bool hide)
		{
			if (sdd==null) return;
			sdd(doc, hide);
		}

		/// <summary>
		/// Open a Package in the main SimPe Gui
		/// </summary>
		public static bool OpenPackage(string filename)
		{
			if (op==null) return false;

			try
			{
				return op(filename);
			}
			catch (Exception ex)
			{
				Helper.ExceptionMessage("Unable to open a Package in the SimPe GUI. (file="+filename+")", ex);
			}
			return false;
		}

		static OpenMemPackageDelegate omp;
		/// <summary>
		/// Returns/Sets the Function that should be called if you want to open a PackedFile
		/// </summary>
		public static OpenMemPackageDelegate OpenMemoryPackageFkt
		{
			get { return omp; }
			set { omp = value; }
		}

		/// <summary>
		/// Open a Package in the main SimPe Gui
		/// </summary>
		public static bool OpenMemoryPackage(SimPe.Interfaces.Files.IPackageFile pkg)
		{
			if (omp==null) return false;

			try
			{
				return omp(pkg);
			}
			catch (Exception ex)
			{
				Helper.ExceptionMessage("Unable to open a Package in the SimPe GUI. (package="+pkg.ToString()+")", ex);
			}
			return false;
		}

		/// <summary>
		/// Open a Package in the main SimPe Gui
		/// </summary>
		public static bool OpenPackedFile(SimPe.Interfaces.Files.IPackedFileDescriptor pfd, SimPe.Interfaces.Files.IPackageFile pkg)
		{
			return OpenPackedFile(FileTable.FileIndex.CreateFileIndexItem(pfd, pkg));
		}

		/// <summary>
		/// Open a Package in the main SimPe Gui
		/// </summary>
		public static bool OpenPackedFile(SimPe.Interfaces.Scenegraph.IScenegraphFileIndexItem fii)
		{
			if (opf==null) return false;

			try
			{
				return opf(fii);
			}
			catch (Exception ex)
			{
				Helper.ExceptionMessage("Unable to open a resource in the SimPe GUI. ("+fii.ToString()+")", ex);
			}
			return false;
		}

		/// <summary>
		/// Displays a certain Help Topic
		/// </summary>
		public static void ShowHelp(string url)
        {
            try
            {
                System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
                {
                    FileName = url,
                    UseShellExecute = true
                });
            }
            catch { }
		}

		/// <summary>
		/// Displays a certain Help Topic
		/// </summary>
		public static void ShowHelp(string url, string topic)
		{
            ShowHelp(string.IsNullOrEmpty(topic) ? url : url + "#" + topic);
		}

		/// <summary>
		/// Displays a Form with the passed Custom Settings.
		/// TODO: replace with Avalonia settings dialog (no PropertyGrid in Avalonia).
		/// </summary>
		public static void ShowCustomSettings(SimPe.Interfaces.ISettings settings)
		{
		}

        public delegate void ResourceListSelectionChangedHandler(object sender, SimPe.Events.ResourceEventArgs e);
        public static void FireResourceListSelectionChangedHandler(object sender, SimPe.Events.ResourceEventArgs e)
        {
            if (ResourceListSelectionChanged != null) ResourceListSelectionChanged(sender, e);
        }
        public static event ResourceListSelectionChangedHandler ResourceListSelectionChanged;
	}
}
