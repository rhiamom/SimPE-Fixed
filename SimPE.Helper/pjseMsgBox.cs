/***************************************************************************
 *   Copyright (C) 2007 by Ambertation                                     *
 *   pljones@users.sf.net                                                  *
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
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Layout;
using Avalonia.Media;
using Avalonia.Threading;

namespace SimPe
{
    /// <summary>
    /// Custom message box with up to 3 configurable buttons.
    /// Moved from namespace System.Windows.Forms to namespace SimPe.
    /// Callers that used the old WinForms version must be updated to use ShowAsync().
    /// </summary>
    public class pjseMsgBox : Window
    {
        DialogResult _result = DialogResult.None;

        readonly Button button1;
        readonly Button button2;
        readonly Button button3;

        pjseMsgBox(string text, string caption,
            Boolset buttonsVisible, Boolset bsBtns, string[] sBtns, DialogResult[] adr)
        {
            if (buttonsVisible.Length < 3)
                throw new ArgumentException("need three (or more) flags", "buttonsVisible");

            Title = caption;
            CanResize = false;
            ShowInTaskbar = false;
            SizeToContent = SizeToContent.WidthAndHeight;
            SystemDecorations = SystemDecorations.BorderOnly;
            MinWidth = 300;

            var tbMessage = new TextBox
            {
                Text = text,
                IsReadOnly = true,
                BorderThickness = new Thickness(1),
                Margin = new Thickness(8),
                TextWrapping = TextWrapping.Wrap,
                MinHeight = 60
            };

            button1 = MakeButton("OK",     DialogResult.OK,     adr, bsBtns, sBtns, 0);
            button2 = MakeButton("Cancel", DialogResult.Cancel,  adr, bsBtns, sBtns, 1);
            button3 = MakeButton("",       DialogResult.None,    adr, bsBtns, sBtns, 2);

            if (!buttonsVisible[0]) button1.IsVisible = false;
            if (!buttonsVisible[1]) button2.IsVisible = false;
            if (!buttonsVisible[2]) button3.IsVisible = false;

            button1.Click += (_, _) => { _result = (DialogResult)button1.Tag!; Close(); };
            button2.Click += (_, _) => { _result = (DialogResult)button2.Tag!; Close(); };
            button3.Click += (_, _) => { _result = (DialogResult)button3.Tag!; Close(); };

            var buttonRow = new StackPanel
            {
                Orientation = Orientation.Horizontal,
                HorizontalAlignment = HorizontalAlignment.Center,
                Spacing = 4,
                Margin = new Thickness(8),
                Children = { button1, button2, button3 }
            };

            var root = new StackPanel { Children = { tbMessage, buttonRow } };
            Content = root;
        }

        Button MakeButton(string defaultText, DialogResult defaultResult,
            DialogResult[] adr, Boolset bsBtns, string[] sBtns, int index)
        {
            string text = defaultText;
            DialogResult result = defaultResult;

            if (bsBtns != null && bsBtns.Length > index && bsBtns[index])
            {
                if (sBtns != null && sBtns.Length > index) text = sBtns[index];
                if (adr != null && adr.Length > index) result = adr[index];
            }
            else if (adr != null && adr.Length > index)
            {
                result = adr[index];
            }

            var btn = new Button { Content = text, Tag = result, MinWidth = 75 };
            return btn;
        }

        // ── Static Show helpers ──────────────────────────────────────────────
        // Callers that previously used System.Windows.Forms.pjseMsgBox must be
        // updated to await these async methods.

        public static Task<DialogResult> ShowAsync(Window owner, string text)
            => ShowAsync(owner, text, "", "001", null, null, null);

        public static Task<DialogResult> ShowAsync(Window owner, string text, string caption)
            => ShowAsync(owner, text, caption, "001", null, null, null);

        public static Task<DialogResult> ShowAsync(Window owner, string text, string caption, Boolset buttonsVisible)
            => ShowAsync(owner, text, caption, buttonsVisible, null, null, null);

        public static Task<DialogResult> ShowAsync(Window owner, string text, string caption,
            Boolset buttonsVisible, Boolset buttonsOverride, string[] buttons)
            => ShowAsync(owner, text, caption, buttonsVisible, buttonsOverride, buttons, null);

        public static async Task<DialogResult> ShowAsync(Window owner, string text, string caption,
            Boolset buttonsVisible, Boolset buttonsOverride, string[] buttons, DialogResult[] resultSet)
        {
            var dlg = new pjseMsgBox(text, caption, buttonsVisible, buttonsOverride, buttons, resultSet);
            if (owner != null)
                await dlg.ShowDialog(owner);
            else
                dlg.Show();
            return dlg._result;
        }

        // Ownerless variants (for callers that don't have a window reference yet)
        public static Task<DialogResult> ShowAsync(string text)
            => ShowAsync(null, text, "", "001", null, null, null);

        public static Task<DialogResult> ShowAsync(string text, string caption)
            => ShowAsync(null, text, caption, "001", null, null, null);

        public static Task<DialogResult> ShowAsync(string text, string caption, Boolset buttonsVisible)
            => ShowAsync(null, text, caption, buttonsVisible, null, null, null);

        public static Task<DialogResult> ShowAsync(string text, string caption,
            Boolset buttonsVisible, Boolset buttonsOverride, string[] buttons)
            => ShowAsync(null, text, caption, buttonsVisible, buttonsOverride, buttons, null);

        public static Task<DialogResult> ShowAsync(string text, string caption,
            Boolset buttonsVisible, Boolset buttonsOverride, string[] buttons, DialogResult[] resultSet)
            => ShowAsync(null, text, caption, buttonsVisible, buttonsOverride, buttons, resultSet);
    }
}
