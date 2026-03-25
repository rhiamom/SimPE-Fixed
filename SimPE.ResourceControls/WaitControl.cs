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
using System.ComponentModel;

namespace SimPe
{
    public partial class WaitControl : Avalonia.Controls.UserControl, IWaitingBarControl
    {
        Avalonia.Controls.ProgressBar pb;
        Avalonia.Controls.TextBlock tbPercent;
        Avalonia.Controls.TextBlock tbInfo;

        public WaitControl()
        {
            pb = new Avalonia.Controls.ProgressBar
            {
                IsVisible = false,
                MinWidth = 100,
                VerticalAlignment = Avalonia.Layout.VerticalAlignment.Center,
            };
            tbPercent = new Avalonia.Controls.TextBlock
            {
                Text = "0%",
                IsVisible = false,
                VerticalAlignment = Avalonia.Layout.VerticalAlignment.Center,
                Margin = new Avalonia.Thickness(4, 0),
            };
            tbInfo = new Avalonia.Controls.TextBlock
            {
                Text = "---",
                VerticalAlignment = Avalonia.Layout.VerticalAlignment.Center,
                Margin = new Avalonia.Thickness(4, 0),
            };

            var panel = new Avalonia.Controls.StackPanel
            {
                Orientation = Avalonia.Layout.Orientation.Horizontal,
                VerticalAlignment = Avalonia.Layout.VerticalAlignment.Center,
            };
            panel.Children.Add(pb);
            panel.Children.Add(tbPercent);
            panel.Children.Add(tbInfo);
            Content = panel;

            msg = "";
            MaxProgress = 0;
            Waiting = false;
            ShowProgress = false;
            ShowAnimation = true;
            ShowText = true;
            nowp = -1;
        }

        string msg;
        public string Message
        {
            get { lock (this) { return msg; } }
            set
            {
                lock (this) { msg = value; }
                var _m = msg;
                Avalonia.Threading.Dispatcher.UIThread.Post(() => { tbInfo.Text = _m; });
            }
        }

        int max;
        public int MaxProgress
        {
            get { lock (this) { return max; } }
            set
            {
                lock (this) { max = value; }
                var _mp = value;
                Avalonia.Threading.Dispatcher.UIThread.Post(() =>
                {
                    pb.Value = pb.Minimum;
                    pb.Maximum = Math.Max(Math.Max(1, pb.Minimum), (double)_mp);
                    DoShowProgress(pb.Maximum > 1);
                });
            }
        }

        int val;
        int nowp;
        public int Progress
        {
            get { return val; }
            set
            {
                var _pv = value;
                Avalonia.Threading.Dispatcher.UIThread.Post(() => SetProgress(_pv));
            }
        }

        private void SetProgress(int value)
        {
            val = (int)Math.Min(pb.Maximum, (double)value);
            pb.Value = val;

            int perc = pb.Maximum > 0 ? (int)((val / pb.Maximum) * 100) : 0;
            int diff = Math.Abs(nowp - perc);
            if (diff > 0)
                tbPercent.Text = perc.ToString("N0") + "%";

            if (diff >= 10)
            {
                nowp = perc;
            }
        }

        bool wait;
        public bool Waiting
        {
            get { return wait; }
            set
            {
                var _sw = value;
                Avalonia.Threading.Dispatcher.UIThread.Post(() => SetWaiting(_sw));
            }
        }

        private void SetWaiting(bool value)
        {
            wait = value;
            if (wait)
            {
                this.IsVisible = true;
            }
            else
            {
                if (!Helper.XmlRegistry.ShowWaitBarPermanent) this.IsVisible = false;
                this.Message = "";
                this.Progress = 0;
                this.ShowProgress = false;
            }
        }

        bool spb;
        public bool ShowProgress
        {
            get { return spb; }
            set
            {
                var _sp = value;
                Avalonia.Threading.Dispatcher.UIThread.Post(() => DoShowProgress(_sp));
            }
        }

        void DoShowProgress(bool value)
        {
            spb = value;
            pb.IsVisible = spb;
            tbPercent.IsVisible = spb;
        }

        bool sanim;
        public bool ShowAnimation
        {
            get { return sanim; }
            set { sanim = value; }
        }

        bool stxt;
        public bool ShowText
        {
            get { return stxt; }
            set
            {
                var _st = value;
                Avalonia.Threading.Dispatcher.UIThread.Post(() => DoShowText(_st));
            }
        }

        private void DoShowText(bool value)
        {
            stxt = value;
            tbInfo.IsVisible = stxt;
        }

        #region IWaitingBarControl Member

        public bool Running => Waiting;

        public Avalonia.Media.Imaging.Bitmap Image
        {
            get { return null; }
            set { }
        }

        public int Value { get; internal set; }
        public int Maximum { get; internal set; }

        public void Wait()
        {
            Message = SimPe.Localization.GetString("Please Wait");
            Waiting = true;
        }

        public void Wait(int max)
        {
            ShowProgress = true;
            Message = SimPe.Localization.GetString("Please Wait");
            MaxProgress = max;
            Waiting = true;
        }

        public void Stop()
        {
            ShowProgress = false;
            Waiting = false;
        }

        #endregion
    }
}
