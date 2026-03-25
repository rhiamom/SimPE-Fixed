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

namespace SimPe
{
	/// <summary>
	/// This class can be used to interface the StatusBar of the Main GUI, which will display
	/// something like the WaitingScreen
	/// </summary>
	public class WaitBarControl : IWaitingBarControl
	{
		WaitControl wc;
        public WaitBarControl(WaitControl wc)
		{
			this.wc = wc;
			ShowProgress(false);
		}

		#region Visible Control
		protected void ShowMain(bool visible)
		{
            wc.Waiting = visible;
		}

		protected void ShowAnimation(bool visible)
		{
            wc.ShowAnimation = visible;
		}

		protected void ShowProgress(bool visible)
		{
            wc.ShowProgress = visible;
		}

		protected void ShowImage(bool visible)
		{

		}

		protected void ShowDescription(bool visible)
		{
            wc.ShowText = visible;
		}
		#endregion

		#region Setters
		protected void SetMessage(string text)
		{
            string t = "";
            if (text != null) t = text;
            wc.Message = t;
		}

		protected void SetImage(object img)
		{

		}

		protected void SetProgress(int val)
		{
            wc.Progress = val;
		}

		protected void SetMaxProgress(int val)
		{
            wc.MaxProgress = val;
		}

		protected void StartAnimation(bool b)
		{

		}
		#endregion

		public bool Running
		{
			get { return wc.Waiting; }
		}

		public string Message
		{
			get { return wc.Message; }
			set
			{
				Avalonia.Threading.Dispatcher.UIThread.Post(() => SetMessage(" " + value));
			}
		}

		public Avalonia.Media.Imaging.Bitmap Image
		{
			get { return null; }
			set { }
		}

		public int Progress
		{
			get { return wc.Value; }
			set
			{
				var _v = value;
				Avalonia.Threading.Dispatcher.UIThread.Post(() => SetProgress(_v));
			}
		}

		public int MaxProgress
		{
			get { return wc.Maximum; }
			set
			{
				var _v = value;
				Avalonia.Threading.Dispatcher.UIThread.Post(() => ShowProgress(true));
				Avalonia.Threading.Dispatcher.UIThread.Post(() => SetMaxProgress(_v));
			}
		}

        bool IWaitingBarControl.ShowProgress { get => ((IWaitingBarControl)wc).ShowProgress; set => ((IWaitingBarControl)wc).ShowProgress=value; }

        protected void StartWait()
		{
			Avalonia.Threading.Dispatcher.UIThread.Post(() => ShowDescription(true));
			Avalonia.Threading.Dispatcher.UIThread.Post(() => ShowAnimation(true));

			Message = SimPe.Localization.GetString("Please Wait");
			Image = null;
			Avalonia.Threading.Dispatcher.UIThread.Post(() => ShowMain(true));
		}

		public void Wait()
		{
			StartWait();
		}

		public void Wait(int max)
		{
			Progress=0;
			StartWait();
			MaxProgress = max;
		}

		public void Stop()
		{
			try
			{
				Avalonia.Threading.Dispatcher.UIThread.Post(() => ShowMain(false));
				Avalonia.Threading.Dispatcher.UIThread.Post(() => ShowProgress(false));
			}
			catch {}
		}
	}
}
