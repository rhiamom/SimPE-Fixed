/***************************************************************************
 *   Copyright (C) 2005 by Ambertation                                     *
 *   quaxi@ambertation.de                                                  *
 *                                                                         *
 *   Copyright (C) 2008 by Peter L Jones                                   *
 *   pljones@users.sf.net                                                  *
 *                                                                         *
 *   Copyright (C) 2008 by GramzeSweatShop                                 *
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
using Avalonia;
using Avalonia.Controls;
using Avalonia.Layout;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Avalonia.Threading;

namespace SimPe
{
    /// <summary>
    /// Borderless splash overlay shown during long operations.
    /// Thread-safe: SetImage/SetMessage may be called from any thread.
    /// Replaces WinForms WndProc message passing with Dispatcher.UIThread.InvokeAsync.
    /// </summary>
    internal class WaitingForm : Window
    {
        // Background: #666699 (original WinForms BackColor = Color.FromArgb(102,102,153))
        static readonly IBrush BackgroundBrush = new SolidColorBrush(Color.FromRgb(102, 102, 153));

        readonly Avalonia.Controls.Image pb;        // custom image
        readonly Avalonia.Controls.Image pbsimpe;   // simpe logo placeholder
        readonly TextBlock lbwait;         // "Please wait..."
        readonly TextBlock lbmsg;          // status message

        Bitmap? _image;
        string _message = "";
        readonly object _lock = new object();

        public WaitingForm()
        {
            System.Diagnostics.Trace.WriteLine("SimPe.WaitingForm..ctor()");

            SystemDecorations = SystemDecorations.None;
            Background = BackgroundBrush;
            ShowInTaskbar = false;
            CanResize = false;
            Width = 220;
            Height = 80;
            Topmost = true;

            pb = new Avalonia.Controls.Image
            {
                Width = 64, Height = 64,
                Stretch = Stretch.Uniform,
                IsVisible = false
            };
            pbsimpe = new Avalonia.Controls.Image
            {
                Width = 64, Height = 64,
                Stretch = Stretch.Uniform,
                IsVisible = true
            };
            lbwait = new TextBlock
            {
                Text = "Please wait...",
                Foreground = Brushes.Gray,
                FontSize = 12,
                Margin = new Thickness(4, 0, 0, 2)
            };
            lbmsg = new TextBlock
            {
                Text = "",
                Foreground = new SolidColorBrush(Color.FromRgb(204, 211, 213)),
                FontSize = 11,
                TextWrapping = TextWrapping.Wrap,
                Margin = new Thickness(4, 0, 0, 0)
            };

            var labelStack = new StackPanel
            {
                VerticalAlignment = VerticalAlignment.Center,
                Children = { lbwait, lbmsg }
            };

            var imageStack = new StackPanel
            {
                Orientation = Avalonia.Layout.Orientation.Horizontal,
                Children = { pb, pbsimpe }
            };

            var root = new DockPanel { LastChildFill = true };
            DockPanel.SetDock(imageStack, Dock.Left);
            root.Children.Add(imageStack);
            root.Children.Add(labelStack);

            Content = root;
        }

        public void SetImage(Bitmap? image)
        {
            System.Diagnostics.Trace.WriteLine("SimPe.WaitingForm.SetImage()");
            lock (_lock)
            {
                if (_image == image) return;
                _image = image;
            }
            Dispatcher.UIThread.InvokeAsync(() =>
            {
                pb.Source = _image;
                pb.IsVisible = _image != null;
                pbsimpe.IsVisible = _image == null;
            });
        }

        public Bitmap? Image => _image;

        public void SetMessage(string message)
        {
            System.Diagnostics.Trace.WriteLine("SimPe.WaitingForm.SetMessage(): " + message);
            lock (_lock)
            {
                if (_message == message) return;
                _message = message;
            }
            Dispatcher.UIThread.InvokeAsync(() => lbmsg.Text = _message);
        }

        public string Message => _message;

        public void StartSplash()
        {
            System.Diagnostics.Trace.WriteLine("SimPe.WaitingForm.StartSplash()");
            Dispatcher.UIThread.InvokeAsync(() => Show());
        }

        public void StopSplash()
        {
            System.Diagnostics.Trace.WriteLine("SimPe.WaitingForm.StopSplash()");
            Dispatcher.UIThread.InvokeAsync(() => Hide());
        }
    }
}
