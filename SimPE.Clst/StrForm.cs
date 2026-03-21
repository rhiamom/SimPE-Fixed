/***************************************************************************
 *   Copyright (C) 2005 by Peter L Jones                                   *
 *   pljones@users.sf.net                                                  *
 *                                                                         *
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
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Layout;
using Avalonia.Media;
using SimPe.Interfaces.Files;
using SimPe.Interfaces.Wrapper;
using SimPe.Data;
using SimPe.PackedFiles.Wrapper;
using SimPe.Interfaces.Plugin;

namespace SimPe.PackedFiles.UserInterface
{
    /// <summary>
    /// Edits a Str (string table) resource.
    /// Ported from WinForms Form to Avalonia UserControl.
    /// All business logic preserved; only UI construction changed.
    /// </summary>
    public class StrForm : UserControl, IPackedFileUI
    {
        #region UI controls
        private Button   button1;      // "Clear"
        private Button   btcommit;     // "Commit to File"
        private ComboBox   cblanguage;
        private Border     gbstr;
        private TextBlock  gbstrHeader;
        private Button   lldelall;
        private Button   lldelete;
        private Button   llchangeall;
        private Button   lladdall;
        private Button   lladd;
        private Button   llcommit;
        private TextBox  rtbdesc;
        private TextBox  rtbvalue;
        private ListBox  lbtexts;
        private TextBox  tbformat;
        private Button   llcreate;
        private Button   linkLabel1;
        private Button   button2;      // "Clean File"
        #endregion

        public StrForm()
        {
            BuildUI();
            llcreate.IsVisible = Helper.XmlRegistry.HiddenMode;
        }

        private void BuildUI()
        {
            // ── Outer Grid ─────────────────────────────────────────────────
            var outerGrid = new Grid();
            outerGrid.RowDefinitions.Add(new RowDefinition(30, GridUnitType.Pixel));  // header bar
            outerGrid.RowDefinitions.Add(new RowDefinition(GridLength.Auto));         // toolbar
            outerGrid.RowDefinitions.Add(new RowDefinition(GridLength.Auto));         // link row
            outerGrid.RowDefinitions.Add(new RowDefinition(GridLength.Star));         // main content

            // header bar
            var header = new Border { Background = new SolidColorBrush(Colors.Gray), Height = 30 };
            Grid.SetRow(header, 0);

            // toolbar row: Format: | tbformat | Language: | cblanguage | btcommit | button2
            tbformat    = new TextBox   { IsReadOnly = true, MinWidth = 80, Margin = new Thickness(4, 2, 4, 2) };
            cblanguage  = new ComboBox  { MinWidth = 120, Margin = new Thickness(4, 2, 4, 2) };
            btcommit    = new Button    { Content = "Commit to File", IsEnabled = false, Margin = new Thickness(4, 2, 4, 2) };
            button2     = new Button    { Content = "Clean File",     Margin = new Thickness(4, 2, 4, 2) };

            var toolBar = new StackPanel
            {
                Orientation = Orientation.Horizontal,
                Margin = new Thickness(4, 2, 4, 2)
            };
            toolBar.Children.Add(new TextBlock { Text = "Format:", VerticalAlignment = VerticalAlignment.Center, Margin = new Thickness(0, 0, 4, 0) });
            toolBar.Children.Add(tbformat);
            toolBar.Children.Add(new TextBlock { Text = "Language:", VerticalAlignment = VerticalAlignment.Center, Margin = new Thickness(8, 0, 4, 0) });
            toolBar.Children.Add(cblanguage);
            toolBar.Children.Add(btcommit);
            toolBar.Children.Add(button2);
            Grid.SetRow(toolBar, 1);

            // link row
            llcreate   = new Button { Content = "Create Text File", Margin = new Thickness(4, 0, 4, 2) };
            linkLabel1 = new Button { Content = "Add All Languages", Margin = new Thickness(4, 0, 4, 2) };
            button1    = new Button { Content = "Clear",             Margin = new Thickness(4, 0, 4, 2) };
            var linkRow = new StackPanel { Orientation = Orientation.Horizontal };
            linkRow.Children.Add(llcreate);
            linkRow.Children.Add(linkLabel1);
            linkRow.Children.Add(button1);
            Grid.SetRow(linkRow, 2);

            // ── Main content: list | splitter | editor ──────────────────────
            lbtexts = new ListBox { MinWidth = 180 };

            rtbvalue = new TextBox { AcceptsReturn = true, TextWrapping = TextWrapping.Wrap, MinHeight = 60 };
            rtbdesc  = new TextBox { AcceptsReturn = true, TextWrapping = TextWrapping.Wrap, MinHeight = 60 };

            lladd       = new Button { Content = "Add",        IsEnabled = false, Margin = new Thickness(2) };
            lladdall    = new Button { Content = "Add to All", IsEnabled = false, Margin = new Thickness(2) };
            lldelete    = new Button { Content = "Delete",     IsEnabled = false, Margin = new Thickness(2) };
            lldelall    = new Button { Content = "Del in All", IsEnabled = false, Margin = new Thickness(2) };
            llchangeall = new Button { Content = "Change All", IsEnabled = false, Margin = new Thickness(2) };
            llcommit    = new Button { Content = "Commit",     IsEnabled = false, Margin = new Thickness(2) };

            var actionRow = new StackPanel { Orientation = Orientation.Horizontal };
            actionRow.Children.Add(lladd);
            actionRow.Children.Add(lladdall);
            actionRow.Children.Add(lldelete);
            actionRow.Children.Add(lldelall);
            actionRow.Children.Add(llchangeall);
            actionRow.Children.Add(llcommit);

            var editorGrid = new Grid();
            editorGrid.RowDefinitions.Add(new RowDefinition(GridLength.Auto));
            editorGrid.RowDefinitions.Add(new RowDefinition(GridLength.Star));
            editorGrid.RowDefinitions.Add(new RowDefinition(GridLength.Auto));
            editorGrid.RowDefinitions.Add(new RowDefinition(GridLength.Star));
            editorGrid.RowDefinitions.Add(new RowDefinition(GridLength.Auto));

            Grid.SetRow(new TextBlock { Text = "Value:" },       0); editorGrid.Children.Add(new TextBlock { Text = "Value:",       Margin = new Thickness(4, 2, 4, 0) });
            Grid.SetRow(rtbvalue, 1); editorGrid.Children.Add(rtbvalue);
            var descLabel = new TextBlock { Text = "Description:", Margin = new Thickness(4, 2, 4, 0) };
            Grid.SetRow(descLabel, 2); editorGrid.Children.Add(descLabel);
            Grid.SetRow(rtbdesc, 3); editorGrid.Children.Add(rtbdesc);
            Grid.SetRow(actionRow, 4); editorGrid.Children.Add(actionRow);

            // Fix row assignment — redo properly
            editorGrid.Children.Clear();
            var valueLabel = new TextBlock { Text = "Value:",       Margin = new Thickness(4, 2, 4, 0) };
            Grid.SetRow(valueLabel,   0); editorGrid.Children.Add(valueLabel);
            Grid.SetRow(rtbvalue,     1); editorGrid.Children.Add(rtbvalue);
            Grid.SetRow(descLabel,    2); editorGrid.Children.Add(descLabel);
            Grid.SetRow(rtbdesc,      3); editorGrid.Children.Add(rtbdesc);
            Grid.SetRow(actionRow,    4); editorGrid.Children.Add(actionRow);

            gbstrHeader = new TextBlock
            {
                Text       = "String Item",
                FontWeight = Avalonia.Media.FontWeight.SemiBold,
                Margin     = new Thickness(4, 2, 4, 0)
            };
            var gbstrInner = new DockPanel();
            DockPanel.SetDock(gbstrHeader, Dock.Top);
            gbstrInner.Children.Add(gbstrHeader);
            gbstrInner.Children.Add(editorGrid);
            gbstr = new Border
            {
                BorderBrush     = new SolidColorBrush(Colors.Gray),
                BorderThickness = new Thickness(1),
                Margin          = new Thickness(4),
                Child           = gbstrInner
            };

            var mainGrid = new Grid();
            mainGrid.ColumnDefinitions.Add(new ColumnDefinition(200, GridUnitType.Pixel));
            mainGrid.ColumnDefinitions.Add(new ColumnDefinition(4,   GridUnitType.Pixel));
            mainGrid.ColumnDefinitions.Add(new ColumnDefinition(GridLength.Star));

            var splitter = new GridSplitter
            {
                Background = new SolidColorBrush(Colors.LightGray),
                Width      = 4,
                HorizontalAlignment = HorizontalAlignment.Stretch
            };

            Grid.SetColumn(lbtexts,  0);
            Grid.SetColumn(splitter, 1);
            Grid.SetColumn(gbstr,    2);
            mainGrid.Children.Add(lbtexts);
            mainGrid.Children.Add(splitter);
            mainGrid.Children.Add(gbstr);

            var mainGroupBox = new Border { Child = mainGrid, Margin = new Thickness(4) };
            Grid.SetRow(mainGroupBox, 3);

            outerGrid.Children.Add(header);
            outerGrid.Children.Add(toolBar);
            outerGrid.Children.Add(linkRow);
            outerGrid.Children.Add(mainGroupBox);

            Content = outerGrid;

            // ── Wire events ────────────────────────────────────────────────
            btcommit.Click      += (s, e) => CommitStr(s, e);
            button2.Click       += (s, e) => CleanFile(s, e);
            button1.Click       += (s, e) => ClearStr(s, e);
            llcreate.Click      += (s, e) => CreateTextFile(s, e);
            linkLabel1.Click    += (s, e) => linkLabel1_LinkClicked(s, e);
            lladd.Click         += (s, e) => StrAdd(s, e);
            lladdall.Click      += (s, e) => AddToAll(s, e);
            lldelete.Click      += (s, e) => StrDelete(s, e);
            lldelall.Click      += (s, e) => DelInAll(s, e);
            llchangeall.Click   += (s, e) => ChangeInAllLanguages(s, e);
            llcommit.Click      += (s, e) => CommitChanges(s, e);

            cblanguage.SelectionChanged += (s, e) => LanguageChanged(s, e);
            lbtexts.SelectionChanged    += (s, e) => StringSelected(s, e);
            rtbvalue.TextChanged        += (s, e) => ChangeText(s, e);
            rtbdesc.TextChanged         += (s, e) => ChangeText(s, e);
        }

        #region Str
        internal Str wrapper;
        #endregion

        #region IPackedFileUI Member
        public Avalonia.Controls.Control GUIHandle => this;

        public void UpdateGUI(IFileWrapper wrp)
        {
            wrapper = (Str)wrp;
            Tag = true;

            tbformat.Text = "0x" + Helper.HexString((ushort)wrapper.Format);

            lbtexts.Items.Clear();

            llcommit.IsEnabled    = false;
            llchangeall.IsEnabled = false;
            btcommit.IsEnabled    = false;

            rtbvalue.Text = "";
            rtbdesc.Text  = "";
            gbstrHeader.Text = "String Item";

            cblanguage.Items.Clear();

            foreach (StrLanguage s in wrapper.Languages)
                cblanguage.Items.Add(s);

            if (cblanguage.Items.Count > 0) cblanguage.SelectedIndex = 0;

            Tag = null;
        }

        public void Dispose() { }
        #endregion

        private void StrDelete(object sender, RoutedEventArgs e)
        {
            if (lbtexts.SelectedIndex < 0) return;

            try
            {
                Str wrp = (Str)wrapper;
                StrToken item = (StrToken)lbtexts.Items[lbtexts.SelectedIndex];
                wrp.Remove(item);

                lbtexts.Items.Remove(item);
                LanguageChanged(null, null);
                btcommit.IsEnabled = true;

                wrp.Changed = true;
            }
            catch (Exception ex)
            {
                Helper.ExceptionMessage(Localization.Manager.GetString("errconvert"), ex);
            }
        }

        private void DelInAll(object sender, RoutedEventArgs e)
        {
            if (lbtexts.SelectedIndex < 0) return;

            try
            {
                Str wrp = (Str)wrapper;

                foreach (StrItemList list in wrp.Lines.Values)
                {
                    if (list.Count > lbtexts.SelectedIndex)
                        list.RemoveAt(lbtexts.SelectedIndex);
                }

                lbtexts.Items.Remove(lbtexts.Items[lbtexts.SelectedIndex]);
                LanguageChanged(null, null);
                btcommit.IsEnabled = true;
                wrp.Changed = true;
            }
            catch (Exception ex)
            {
                Helper.ExceptionMessage(Localization.Manager.GetString("errconvert"), ex);
            }
        }

        private void CreateTextFile(object sender, RoutedEventArgs e)
        {
            try
            {
                string list = "";
                for (int i = 0; i < lbtexts.Items.Count; i++)
                {
                    StrToken item = (StrToken)lbtexts.Items[i];
                    list += "0x" + i.ToString("X") + ": " + item.Title + " (" + item.Description + ")" + Helper.lbr;
                }

                btcommit.IsEnabled = true;
                _ = TopLevel.GetTopLevel(this)?.Clipboard?.SetTextAsync(list);
            }
            catch (Exception) { }
        }

        private void ClearStr(object sender, RoutedEventArgs e)
        {
            try
            {
                Str wrp = (Str)wrapper;
                wrp.Items = new StrItemList();
                cblanguage.Items.Clear();

                StrLanguageList lngs = new StrLanguageList();
                for (int i = 1; i < 45; i++)
                {
                    StrLanguage lng = new StrLanguage((byte)i);
                    cblanguage.Items.Add(lng);
                    lngs.Add(lng);
                }
                wrp.Languages = lngs;
                cblanguage.SelectedIndex = 0;
                btcommit.IsEnabled = true;
                LanguageChanged(null, null);
            }
            catch (Exception) { }
        }

        private void CleanFile(object sender, RoutedEventArgs e)
        {
            try
            {
                StrLanguageList ls = new StrLanguageList();
                cblanguage.Items.Clear();
                for (byte i = 1; i < 45; i++)
                {
                    ls.Add(new StrLanguage(i));
                    cblanguage.Items.Add(ls[ls.Count - 1]);
                }
                Str wrp = (Str)wrapper;
                wrp.Languages = ls;
                wrp.Changed = true;

                for (int i = 0; i < cblanguage.Items.Count; i++)
                {
                    cblanguage.SelectedIndex = i;
                    foreach (StrToken s in wrp.LanguageItems((StrLanguage)cblanguage.Items[cblanguage.SelectedIndex]))
                        lbtexts.Items.Add(s);
                    if (i < 11 || i == 12 || i == 13 || i == 14 || i == 15 || i == 16 || i == 17 || i == 18 || i == 19 || i == 25 || i == 27 || i == 34)
                    {
                        for (int f = 0; f < lbtexts.Items.Count; f++)
                        {
                            lbtexts.SelectedIndex = f;
                            StrToken b = (StrToken)lbtexts.Items[lbtexts.SelectedIndex];
                            b.Description = null;
                        }
                    }
                    else
                    {
                        for (int f = 0; f < lbtexts.Items.Count; f++)
                        {
                            lbtexts.SelectedIndex = f;
                            StrToken b = (StrToken)lbtexts.Items[lbtexts.SelectedIndex];
                            b.Title = null;
                            b.Description = null;
                        }
                    }
                }
                cblanguage.SelectedIndex = 0;
                lbtexts.SelectedIndex = 0;
                CommitStr(null, null);
            }
            catch (Exception) { }
        }

        private void LanguageChanged(object sender, EventArgs e)
        {
            llcommit.IsEnabled = false;
            lbtexts.Items.Clear();
            if (cblanguage.SelectedIndex < 0) return;

            try
            {
                Str wrp = (Str)wrapper;
                foreach (StrToken s in wrp.LanguageItems((StrLanguage)cblanguage.Items[cblanguage.SelectedIndex]))
                    lbtexts.Items.Add(s);
            }
            catch (Exception ex)
            {
                Helper.ExceptionMessage(Localization.Manager.GetString("errconvert"), ex);
            }
        }

        private void StringSelected(object sender, EventArgs e)
        {
            if (Tag != null) return;
            llcommit.IsEnabled    = false;
            llchangeall.IsEnabled = false;
            lldelete.IsEnabled    = false;
            lldelall.IsEnabled    = false;
            if (lbtexts.SelectedIndex < 0) return;

            Tag = true;
            try
            {
                StrToken s = (StrToken)lbtexts.Items[lbtexts.SelectedIndex];

                rtbvalue.Text = s.Title;
                rtbdesc.Text  = s.Description;
                llcommit.IsEnabled    = true;
                llchangeall.IsEnabled = true;
                lldelete.IsEnabled    = true;
                lldelall.IsEnabled    = true;
                btcommit.IsEnabled    = true;

                gbstrHeader.Text = "0x" + Helper.HexString((ushort)lbtexts.SelectedIndex);
            }
            catch (Exception ex)
            {
                Helper.ExceptionMessage(Localization.Manager.GetString("errconvert"), ex);
            }
            finally
            {
                Tag = null;
            }
        }

        private void StrAdd(object sender, RoutedEventArgs e)
        {
            lbtexts.SelectedIndex = -1;
            CommitChanges(null, null);
            lbtexts.SelectedIndex = lbtexts.Items.Count - 1;
            btcommit.IsEnabled = true;
        }

        private void CommitChanges(object sender, RoutedEventArgs e)
        {
            if (Tag != null) return;

            llcommit.IsEnabled = (lbtexts.SelectedIndex < 0);
            if (cblanguage.SelectedIndex < 0) return;

            Tag = true;
            try
            {
                Str wrp = (Str)wrapper;

                if (lbtexts.SelectedIndex < 0)
                {
                    StrToken s = new StrToken(lbtexts.Items.Count,
                        (StrLanguage)cblanguage.Items[cblanguage.SelectedIndex],
                        rtbvalue.Text,
                        rtbdesc.Text);
                    wrp.Add(s);
                    lbtexts.Items.Add(s);
                }
                else
                {
                    StrToken s = (StrToken)lbtexts.Items[lbtexts.SelectedIndex];
                    s.Title       = rtbvalue.Text;
                    s.Description = rtbdesc.Text;
                    lbtexts.Items[lbtexts.SelectedIndex] = s;
                }

                wrp.Changed = true;
            }
            catch (Exception ex)
            {
                Helper.ExceptionMessage(Localization.Manager.GetString("errconvert"), ex);
            }
            finally
            {
                Tag = null;
                btcommit.IsEnabled = false;
            }
        }

        private void CommitStr(object sender, RoutedEventArgs e)
        {
            try
            {
                if (lbtexts.SelectedIndex >= 0) CommitChanges(null, null);
                Str wrp = (Str)wrapper;
                wrp.SynchronizeUserData();
                // TODO: Show "committed" notification via Avalonia dialog
                System.Diagnostics.Trace.WriteLine(Localization.Manager.GetString("commited"));
            }
            catch (Exception ex)
            {
                Helper.ExceptionMessage(Localization.Manager.GetString("errwritingfile"), ex);
            }
        }

        private void ChangeInAllLanguages(object sender, RoutedEventArgs e)
        {
            int curIndex = lbtexts.SelectedIndex;
            if (curIndex < 0) return;
            try
            {
                Str wrp = (Str)wrapper;
                StrToken s = (StrToken)lbtexts.Items[curIndex];

                s.Title       = rtbvalue.Text;
                s.Description = rtbdesc.Text;

                foreach (StrLanguage lng in wrp.Languages)
                {
                    if (lng == null) continue;

                    while (wrp.LanguageItems(lng).Length <= curIndex)
                        wrp.Add(new StrToken(wrp.LanguageItems(lng).Length, lng, "", ""));

                    StrItemList sis = wrp.LanguageItems(lng);
                    sis[curIndex].Title       = s.Title;
                    sis[curIndex].Description = s.Description;
                }

                LanguageChanged(null, null);
                btcommit.IsEnabled = true;
                wrp.Changed = true;
            }
            catch (Exception ex)
            {
                Helper.ExceptionMessage(Localization.Manager.GetString("errconvert"), ex);
            }
        }

        private void AddToAll(object sender, RoutedEventArgs e)
        {
            try
            {
                Str wrp = (Str)wrapper;

                int count = 0;
                foreach (StrLanguage lng in wrp.Languages)
                    count = Math.Max(count, wrp.LanguageItems(lng).Length);

                foreach (StrLanguage lng in wrp.Languages)
                {
                    if (lng == null) continue;
                    while (wrp.LanguageItems(lng).Length < count)
                        wrp.Add(new StrToken(wrp.LanguageItems(lng).Length, lng, "", ""));

                    wrp.Add(new StrToken(wrp.LanguageItems(lng).Length, lng, rtbvalue.Text, rtbdesc.Text));
                }

                LanguageChanged(null, null);
                btcommit.IsEnabled = true;
                wrp.Changed = true;
            }
            catch (Exception ex)
            {
                Helper.ExceptionMessage(Localization.Manager.GetString("errconvert"), ex);
            }
        }

        private void linkLabel1_LinkClicked(object sender, RoutedEventArgs e)
        {
            StrLanguageList ls = new StrLanguageList();
            cblanguage.Items.Clear();
            for (byte i = 1; i < 45; i++)
            {
                ls.Add(new StrLanguage(i));
                cblanguage.Items.Add(ls[ls.Count - 1]);
            }

            Str wrp = (Str)wrapper;
            wrp.Languages = ls;
            wrp.Changed   = true;

            if (cblanguage.Items.Count > 0) cblanguage.SelectedIndex = 0;
        }

        private void ChangeText(object sender, EventArgs e)
        {
            if (lbtexts.SelectedIndex < 0) return;
            CommitChanges(null, null);
            btcommit.IsEnabled = true;
        }
    }
}
