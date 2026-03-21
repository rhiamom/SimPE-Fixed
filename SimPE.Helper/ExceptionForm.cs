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
using System.Runtime.InteropServices;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Layout;
using Avalonia.Media;
using Avalonia.Threading;

namespace SimPe
{
    /// <summary>
    /// Shows exception details in an Avalonia window.
    /// Execute() may be called from any thread; the window is posted to the UI thread
    /// asynchronously (fire-and-forget).
    /// </summary>
    public class ExceptionForm : Window
    {
        readonly TextBlock lberr;
        readonly TextBox rtb;           // details (plain text — RTF formatting is stripped)
        readonly Button lldetail;       // "Show details"
        readonly StackPanel gbdetail;   // details panel (initially hidden)

        ExceptionForm(string message, string details, bool isWarning, string supportUrl)
        {
            Title = isWarning ? "Warning" : "SimPE Error";
            Width = 500;
            Height = 280;
            CanResize = true;
            SystemDecorations = SystemDecorations.Full;

            lberr = new TextBlock
            {
                Text = message,
                Foreground = Brushes.Black,
                FontWeight = FontWeight.Bold,
                TextWrapping = TextWrapping.Wrap,
                Margin = new Thickness(8, 8, 8, 4)
            };

            rtb = new TextBox
            {
                Text = details,
                IsReadOnly = true,
                AcceptsReturn = true,
                FontSize = 11,
                Foreground = new SolidColorBrush(Color.FromRgb(64, 64, 64)),
                Margin = new Thickness(4)
            };

            gbdetail = new StackPanel
            {
                IsVisible = false,
                Children = { new ScrollViewer { Content = rtb, Height = 200 } }
            };

            lldetail = new Button
            {
                Content = "Show details...",
                HorizontalAlignment = HorizontalAlignment.Left,
                Margin = new Thickness(8, 0, 8, 4)
            };
            lldetail.Click += (_, _) =>
            {
                lldetail.IsVisible = false;
                gbdetail.IsVisible = true;
                Height = 480;
            };

            var copyBtn = new Button { Content = "Copy to Clipboard", Margin = new Thickness(4) };
            copyBtn.Click += async (_, _) =>
            {
                var clipboard = TopLevel.GetTopLevel(this)?.Clipboard;
                if (clipboard != null)
                    await clipboard.SetTextAsync(details);
            };

            var closeBtn = new Button { Content = "Close", Margin = new Thickness(4) };
            closeBtn.Click += (_, _) => Close();

            var buttonRow = new StackPanel
            {
                Orientation = Orientation.Horizontal,
                HorizontalAlignment = HorizontalAlignment.Right,
                Margin = new Thickness(8),
                Children = { copyBtn, closeBtn }
            };

            if (!string.IsNullOrEmpty(supportUrl))
            {
                var supportBtn = new Button { Content = "Support", Margin = new Thickness(4) };
                supportBtn.Click += (_, _) =>
                {
                    try { System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo(supportUrl) { UseShellExecute = true }); }
                    catch { }
                };
                buttonRow.Children.Insert(0, supportBtn);
            }

            var root = new DockPanel { LastChildFill = true };
            DockPanel.SetDock(lberr, Dock.Top);
            DockPanel.SetDock(lldetail, Dock.Top);
            DockPanel.SetDock(buttonRow, Dock.Bottom);
            root.Children.Add(lberr);
            root.Children.Add(lldetail);
            root.Children.Add(buttonRow);
            root.Children.Add(gbdetail);

            Content = root;
        }

        /// <summary>Show an Exception Message (single-arg overload).</summary>
        public static void Execute(Exception ex)
        {
            Execute(ex.Message, ex);
        }

        /// <summary>
        /// Show an exception message window.
        /// May be called from any thread; the window is shown asynchronously on the UI thread.
        /// </summary>
        public static void Execute(string message, Exception ex)
        {
            if (Helper.NoErrors) return;

            if (message == null) message = "";
            if (message.Trim() == "") message = ex?.Message ?? "";

            if (message.Contains("Microsoft.DirectX"))
            {
                ex = new Warning("You need to install MANAGED DirectX",
                    "In order to perform some Operations, you need to install Managed DirectX.\n\nPlease visit the SimPE support site for more details.", ex);
                message = ex.Message;
            }

            // Build plain-text details (previously RTF)
            var sb = new System.Text.StringBuilder();

            bool isWarning = ex?.GetType() == typeof(Warning);
            string supportUrl = "";

            if (isWarning)
            {
                message = "Warning: " + message;
                sb.AppendLine("This is just a Warning. It is supposed to keep you informed about a Problem.");
                sb.AppendLine("Most of the time this is not a Bug!");
                sb.AppendLine();
                sb.AppendLine(((Warning)ex).Details?.Trim());
            }
            else
            {
                sb.AppendLine("Message:");
                sb.AppendLine(message.Trim());
                sb.AppendLine();

                try
                {
                    sb.AppendLine("SimPE Version:");
                    sb.AppendLine(Helper.StartedGui.ToString() +
                        " (" + Helper.SimPeVersion.ProductMajorPart + "." +
                        Helper.SimPeVersion.ProductMinorPart + "." +
                        Helper.SimPeVersion.ProductBuildPart + "." +
                        Helper.SimPeVersion.ProductPrivatePart + ")");
                    sb.AppendLine();
                }
                catch { }

                sb.AppendLine("Exception Stack:");
                var myex = ex;
                while (myex != null)
                {
                    sb.AppendLine(myex.ToString());
                    myex = myex.InnerException;
                }

                if (ex?.Source != null)
                {
                    sb.AppendLine();
                    sb.AppendLine("Source:");
                    sb.AppendLine(ex.Source.Trim());
                }

                if (ex?.StackTrace != null)
                {
                    sb.AppendLine();
                    sb.AppendLine("Execution Stack:");
                    sb.AppendLine(ex.StackTrace.Trim());
                }
            }

            try
            {
                sb.AppendLine();
                sb.AppendLine("OS Version:");
                sb.AppendLine(RuntimeInformation.OSDescription);
            }
            catch { }

            try
            {
                sb.AppendLine();
                sb.AppendLine(".NET Version:");
                sb.AppendLine(Environment.Version.ToString());
            }
            catch { }

            string details = sb.ToString();

            // Post to UI thread — fire-and-forget, does not block caller
            Dispatcher.UIThread.Post(async () =>
            {
                var frm = new ExceptionForm(message.Trim(), details, isWarning, supportUrl);
                frm.Show();
            });
        }
    }
}
