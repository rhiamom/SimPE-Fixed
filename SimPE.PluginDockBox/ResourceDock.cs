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
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using Avalonia.Controls;
using Avalonia.Input;
using Ambertation.Windows.Forms;
using Compat = SimPe.Scenegraph.Compat;

namespace SimPe.Plugin.Tool.Dockable
{
    /// <summary>
    /// Summary description for ResourceDock.
    /// </summary>
    public class ResourceDock
    {
        private DockManager manager;
        internal Ambertation.Windows.Forms.DockPanel dcWrapper;
        internal Ambertation.Windows.Forms.DockPanel dcResource;
        private Avalonia.Controls.StackPanel xpGradientPanel1;
        private Avalonia.Controls.StackPanel xpGradientPanel2;
        internal Avalonia.Controls.StackPanel pntypes;
        internal Avalonia.Controls.TextBox tbinstance;
        private Avalonia.Controls.TextBlock label11;
        internal Avalonia.Controls.TextBox tbtype;
        private Avalonia.Controls.TextBlock label8;
        private Avalonia.Controls.TextBlock label9;
        private Avalonia.Controls.TextBlock label10;
        internal Avalonia.Controls.TextBox tbgroup;
        internal Avalonia.Controls.ComboBox cbtypes;
        internal Avalonia.Controls.TextBlock label3;
        internal Avalonia.Controls.ComboBox cbComp;
        internal Avalonia.Controls.TextBox tbinstance2;
        internal Avalonia.Controls.TextBlock lbName;
        internal Avalonia.Controls.TextBlock label1;
        internal Avalonia.Controls.TextBlock label2;
        internal Avalonia.Controls.TextBlock label5;
        internal Avalonia.Controls.TextBlock lbAuthor;
        internal Avalonia.Controls.TextBlock lbVersion;
        internal Avalonia.Controls.TextBlock lbDesc;
        internal Avalonia.Controls.TextBlock lbComp;
        internal Ambertation.Windows.Forms.DockPanel dcPackage;
        private Avalonia.Controls.StackPanel xpGradientPanel3;
        internal Compat.PropertyGridStub pgHead;
        internal Avalonia.Controls.TextBlock label4;
        internal Compat.ListView lv;
        private Compat.ColumnHeader clOffset;
        private Compat.ColumnHeader clSize;
        internal Ambertation.Windows.Forms.DockPanel dcConvert;
        private Avalonia.Controls.StackPanel xpGradientPanel4;
        private Avalonia.Controls.TextBox tbHex;
        private Avalonia.Controls.TextBox tbDec;
        internal Ambertation.Windows.Forms.DockPanel dcHex;
        internal Ambertation.Windows.Forms.HexViewControl hvc;
        private Avalonia.Controls.TextBox tbBin;
        internal Avalonia.Controls.Button button1;
        internal Avalonia.Controls.Button btcopie;
        private Ambertation.Windows.Forms.HexEditControl hexEditControl1;
        private Avalonia.Controls.Button linkLabel1;
        internal Compat.PictureBoxCompat pb;
        private Avalonia.Controls.StackPanel panel1;
        private Avalonia.Controls.TextBox tbFloat;
        private DockContainer dockBottom;
        private Avalonia.Controls.StackPanel gradientpanel1;
        private Avalonia.Controls.TextBlock label13;
        private Avalonia.Controls.TextBlock label12;
        private Avalonia.Controls.TextBlock label7;
        private Avalonia.Controls.TextBlock label6;
        private System.ComponentModel.IContainer components;

        public ResourceDock()
        {
            //
            // Required designer variable.
            //
            InitializeComponent();

            ThemeManager tm = ThemeManager.Global.CreateChild();
            tm.AddControl(this.xpGradientPanel1);
            tm.AddControl(this.xpGradientPanel2);
            tm.AddControl(this.xpGradientPanel3);
            tm.AddControl(this.xpGradientPanel4);

            this.lv.View = Compat.View.Details;
            foreach (SimPe.Data.TypeAlias a in SimPe.Helper.TGILoader.FileTypes)
                cbtypes.Items.Add(a);
            // cbtypes.Sorted = true; // no Sorted in Avalonia ComboBox
            tbFloat.Width = tbBin.Width;
        }

        #region Layout helpers

        static Avalonia.Controls.Border MakeHeaderBar(string title,
            Avalonia.Controls.Control extraContent = null)
        {
            var lbl = new Avalonia.Controls.TextBlock
            {
                Text = title,
                Foreground = Avalonia.Media.Brushes.White,
                FontWeight = Avalonia.Media.FontWeight.SemiBold,
                FontSize = 11,
                VerticalAlignment = Avalonia.Layout.VerticalAlignment.Center,
                Margin = new Avalonia.Thickness(8, 0),
            };
            Avalonia.Controls.Control headerContent;
            if (extraContent != null)
            {
                var dp = new Avalonia.Controls.DockPanel { LastChildFill = true };
                Avalonia.Controls.DockPanel.SetDock(extraContent, Avalonia.Controls.Dock.Right);
                dp.Children.Add(extraContent);
                dp.Children.Add(lbl);
                headerContent = dp;
            }
            else headerContent = lbl;

            return new Avalonia.Controls.Border
            {
                Background = new Avalonia.Media.LinearGradientBrush
                {
                    StartPoint = new Avalonia.RelativePoint(0, 0, Avalonia.RelativeUnit.Relative),
                    EndPoint   = new Avalonia.RelativePoint(0, 1, Avalonia.RelativeUnit.Relative),
                    GradientStops =
                    {
                        new Avalonia.Media.GradientStop(Avalonia.Media.Color.FromRgb(74, 84, 100), 0.0),
                        new Avalonia.Media.GradientStop(Avalonia.Media.Color.FromRgb(54, 64,  80), 1.0),
                    },
                },
                MinHeight = 28,
                Child = headerContent,
            };
        }

        static Avalonia.Controls.Border MakeGroupBox(Avalonia.Controls.Control inner)
            => new Avalonia.Controls.Border
            {
                Background      = new Avalonia.Media.SolidColorBrush(Avalonia.Media.Color.FromRgb(220, 228, 238)),
                BorderBrush     = new Avalonia.Media.SolidColorBrush(Avalonia.Media.Color.FromRgb(170, 185, 205)),
                BorderThickness = new Avalonia.Thickness(1),
                CornerRadius    = new Avalonia.CornerRadius(3),
                Margin          = new Avalonia.Thickness(0, 0, 0, 6),
                Child = inner,
            };

        static void StyleLabel(Avalonia.Controls.TextBlock tb)
        {
            tb.VerticalAlignment = Avalonia.Layout.VerticalAlignment.Center;
            tb.Margin = new Avalonia.Thickness(0, 0, 6, 0);
        }

        static void StyleTextBox(Avalonia.Controls.TextBox tb)
        {
            tb.Background = Avalonia.Media.Brushes.White;
            tb.MinHeight  = 0;
            tb.Padding    = new Avalonia.Thickness(4, 2);
        }

        static Avalonia.Controls.Button MakeLinkButton(string text) => new Avalonia.Controls.Button
        {
            Content         = text,
            Padding         = new Avalonia.Thickness(0),
            Background      = Avalonia.Media.Brushes.Transparent,
            BorderThickness = new Avalonia.Thickness(0),
            Foreground      = new Avalonia.Media.SolidColorBrush(Avalonia.Media.Color.FromRgb(0, 80, 180)),
            Cursor          = new Avalonia.Input.Cursor(Avalonia.Input.StandardCursorType.Hand),
        };

        // Two-column Grid: label (Auto) | control (Star), with a set of (label, control) row pairs.
        static Avalonia.Controls.Grid MakeTwoColumnGrid(
            params (Avalonia.Controls.Control label, Avalonia.Controls.Control control)[] rows)
        {
            var grid = new Avalonia.Controls.Grid { Margin = new Avalonia.Thickness(6) };
            grid.ColumnDefinitions.Add(new Avalonia.Controls.ColumnDefinition(Avalonia.Controls.GridLength.Auto));
            grid.ColumnDefinitions.Add(new Avalonia.Controls.ColumnDefinition(
                new Avalonia.Controls.GridLength(1, Avalonia.Controls.GridUnitType.Star)));
            for (int i = 0; i < rows.Length; i++)
            {
                grid.RowDefinitions.Add(new Avalonia.Controls.RowDefinition(Avalonia.Controls.GridLength.Auto));
                var (lbl, ctrl) = rows[i];
                lbl.Margin = new Avalonia.Thickness(0, i == 0 ? 0 : 4, 6, 0);
                Avalonia.Controls.Grid.SetRow(lbl,  i); Avalonia.Controls.Grid.SetColumn(lbl,  0);
                Avalonia.Controls.Grid.SetRow(ctrl, i); Avalonia.Controls.Grid.SetColumn(ctrl, 1);
                grid.Children.Add(lbl);
                grid.Children.Add(ctrl);
            }
            return grid;
        }

        #endregion

        #region Panel builders

        Avalonia.Controls.Control BuildPackagePanel()
        {
            label4.IsVisible  = false;
            label4.Text       = "Hole Index:";
            label4.FontWeight = Avalonia.Media.FontWeight.SemiBold;
            label4.Margin     = new Avalonia.Thickness(0, 6, 0, 2);
            lv.IsVisible      = false;
            lv.Height         = 120;
            clOffset.Text = "Offset"; clOffset.Width = 80;
            clSize.Text   = "Size";   clSize.Width   = 80;
            lv.Columns.Add(clOffset);
            lv.Columns.Add(clSize);

            // Property grid on the left, hole index on the right — two-column layout.
            var rightPanel = new Avalonia.Controls.StackPanel { Margin = new Avalonia.Thickness(8, 0, 6, 6) };
            rightPanel.Children.Add(label4);
            rightPanel.Children.Add(lv);

            var holeBox = new Avalonia.Controls.Border
            {
                Background      = new Avalonia.Media.SolidColorBrush(Avalonia.Media.Color.FromRgb(220, 228, 238)),
                BorderBrush     = new Avalonia.Media.SolidColorBrush(Avalonia.Media.Color.FromRgb(170, 185, 205)),
                BorderThickness = new Avalonia.Thickness(1),
                CornerRadius    = new Avalonia.CornerRadius(3),
                Margin          = new Avalonia.Thickness(6),
                Child           = rightPanel,
                IsVisible       = false,
            };
            // Keep holeBox visibility in sync with whether holes exist.
            label4.PropertyChanged += (_, _2) => holeBox.IsVisible = label4.IsVisible;

            var columns = new Avalonia.Controls.Grid { Margin = new Avalonia.Thickness(6) };
            columns.ColumnDefinitions.Add(new Avalonia.Controls.ColumnDefinition(
                new Avalonia.Controls.GridLength(1, Avalonia.Controls.GridUnitType.Star)));
            columns.ColumnDefinitions.Add(new Avalonia.Controls.ColumnDefinition(
                new Avalonia.Controls.GridLength(1.2, Avalonia.Controls.GridUnitType.Star)));
            Avalonia.Controls.Grid.SetColumn(pgHead,   0);
            Avalonia.Controls.Grid.SetColumn(holeBox,  1);
            columns.Children.Add(pgHead);
            columns.Children.Add(holeBox);

            var header = MakeHeaderBar("Package");
            var outer  = new Avalonia.Controls.DockPanel
            {
                LastChildFill = true,
                Background = new Avalonia.Media.SolidColorBrush(Avalonia.Media.Color.FromRgb(220, 228, 238)),
            };
            Avalonia.Controls.DockPanel.SetDock(header, Avalonia.Controls.Dock.Top);
            outer.Children.Add(header);
            outer.Children.Add(columns);
            return outer;
        }

        Avalonia.Controls.Control BuildResourcePanel()
        {
            foreach (var tb in new[] { tbtype, tbgroup, tbinstance, tbinstance2 })
                StyleTextBox(tb);

            label8.Text  = "Type:";      StyleLabel(label8);
            label9.Text  = "Group:";     StyleLabel(label9);
            label10.Text = "Instance:";  StyleLabel(label10);
            label11.Text = "Sub-Type:";  StyleLabel(label11);

            var typeRow = new Avalonia.Controls.Grid();
            typeRow.ColumnDefinitions.Add(new Avalonia.Controls.ColumnDefinition(
                new Avalonia.Controls.GridLength(1, Avalonia.Controls.GridUnitType.Star)));
            typeRow.ColumnDefinitions.Add(new Avalonia.Controls.ColumnDefinition(
                new Avalonia.Controls.GridLength(1, Avalonia.Controls.GridUnitType.Star)));
            Avalonia.Controls.Grid.SetColumn(tbtype,  0);
            Avalonia.Controls.Grid.SetColumn(cbtypes, 1);
            cbtypes.Margin = new Avalonia.Thickness(4, 0, 0, 0);
            typeRow.Children.Add(tbtype);
            typeRow.Children.Add(cbtypes);

            var tgiGrid = MakeTwoColumnGrid(
                (label8,  typeRow),
                (label9,  tbgroup),
                (label10, tbinstance),
                (label11, tbinstance2));

            pntypes.Children.Add(MakeGroupBox(tgiGrid));

            lbComp.Text = "Compression:"; StyleLabel(lbComp);
            cbComp.Items.Add("Uncompressed");
            cbComp.Items.Add("Compressed");
            cbComp.Items.Add("Mixed");
            cbComp.SelectedIndex = 0;

            var compRow = new Avalonia.Controls.StackPanel
            {
                Orientation = Avalonia.Layout.Orientation.Horizontal,
                Margin      = new Avalonia.Thickness(6),
                Spacing     = 6,
            };
            compRow.Children.Add(lbComp);
            compRow.Children.Add(cbComp);

            linkLabel1 = MakeLinkButton("update");
            linkLabel1.Margin = new Avalonia.Thickness(6, 2, 0, 0);
            linkLabel1.Click += (s, e) => linkLabel1_LinkClicked(s, null);

            var content = new Avalonia.Controls.StackPanel { Margin = new Avalonia.Thickness(6) };
            content.Children.Add(pntypes);
            content.Children.Add(MakeGroupBox(compRow));
            content.Children.Add(linkLabel1);

            var scroll = new Avalonia.Controls.ScrollViewer
            {
                VerticalScrollBarVisibility = Avalonia.Controls.Primitives.ScrollBarVisibility.Auto,
                Content = content,
            };
            var header = MakeHeaderBar("Resource");
            var outer  = new Avalonia.Controls.DockPanel { LastChildFill = true };
            Avalonia.Controls.DockPanel.SetDock(header, Avalonia.Controls.Dock.Top);
            outer.Children.Add(header);
            outer.Children.Add(scroll);
            return outer;
        }

        Avalonia.Controls.Control BuildWrapperPanel()
        {
            label1.Text = "Name:";        StyleLabel(label1);
            label2.Text = "Author:";      StyleLabel(label2);
            label5.Text = "Version:";     StyleLabel(label5);
            label3.Text = "Description:"; StyleLabel(label3);

            lbDesc.TextWrapping = Avalonia.Media.TextWrapping.Wrap;

            var infoGrid = MakeTwoColumnGrid(
                (label1, lbName),
                (label2, lbAuthor),
                (label5, lbVersion),
                (label3, lbDesc));

            pb.Height = 48;
            pb.Margin = new Avalonia.Thickness(0, 0, 0, 6);

            var content = new Avalonia.Controls.StackPanel { Margin = new Avalonia.Thickness(6) };
            content.Children.Add(pb);
            content.Children.Add(MakeGroupBox(infoGrid));

            var scroll = new Avalonia.Controls.ScrollViewer
            {
                VerticalScrollBarVisibility = Avalonia.Controls.Primitives.ScrollBarVisibility.Auto,
                Content = content,
            };
            var header = MakeHeaderBar("Wrapper");
            var outer  = new Avalonia.Controls.DockPanel { LastChildFill = true };
            Avalonia.Controls.DockPanel.SetDock(header, Avalonia.Controls.Dock.Top);
            outer.Children.Add(header);
            outer.Children.Add(scroll);
            return outer;
        }

        Avalonia.Controls.Control BuildConverterPanel()
        {
            foreach (var tb in new[] { tbHex, tbDec, tbBin, tbFloat })
                StyleTextBox(tb);

            label12.Text = "Hex:";    StyleLabel(label12);
            label13.Text = "Dec:";    StyleLabel(label13);
            label6.Text  = "Binary:"; StyleLabel(label6);
            label7.Text  = "Float:";  StyleLabel(label7);

            var convertGrid = MakeTwoColumnGrid(
                (label12, tbHex),
                (label13, tbDec),
                (label6,  tbBin),
                (label7,  tbFloat));

            var groupBox = MakeGroupBox(convertGrid);
            groupBox.Margin = new Avalonia.Thickness(6);

            var header = MakeHeaderBar("Converter");
            var outer  = new Avalonia.Controls.DockPanel { LastChildFill = true };
            Avalonia.Controls.DockPanel.SetDock(header, Avalonia.Controls.Dock.Top);
            outer.Children.Add(header);
            outer.Children.Add(groupBox);
            return outer;
        }

        Avalonia.Controls.Control BuildHexPanel()
        {
            // "Copy as Text" stays in the header bar.
            btcopie.Content = "Copy as Text";
            btcopie.Padding = new Avalonia.Thickness(6, 2);
            btcopie.Click  += (s, e) => btcopie_Click(s, null);
            var headerButtons = new Avalonia.Controls.StackPanel
            {
                Orientation       = Avalonia.Layout.Orientation.Horizontal,
                VerticalAlignment = Avalonia.Layout.VerticalAlignment.Center,
                Margin            = new Avalonia.Thickness(0, 0, 4, 0),
            };
            headerButtons.Children.Add(btcopie);

            // "Commit" button lives in the Values section (bottom-right).
            button1.Content = "Commit";
            button1.Padding = new Avalonia.Thickness(8, 3);
            // Override the Fluent theme's ButtonBackground resource so the
            // normal-state fill is white (local resource wins over theme resource).
            button1.Resources["ButtonBackground"]              = Avalonia.Media.Brushes.White;
            button1.Resources["ButtonBackgroundPointerOver"]   = new Avalonia.Media.SolidColorBrush(Avalonia.Media.Color.FromRgb(240, 240, 240));
            button1.Resources["ButtonBackgroundPressed"]       = new Avalonia.Media.SolidColorBrush(Avalonia.Media.Color.FromRgb(220, 220, 220));
            button1.Click += (s, e) => button1_Click(s, null);

            var header   = MakeHeaderBar("Hex", headerButtons);
            var valuesSection = BuildHexValuesSection();

            var outer = new Avalonia.Controls.DockPanel { LastChildFill = true };
            Avalonia.Controls.DockPanel.SetDock(header,        Avalonia.Controls.Dock.Top);
            Avalonia.Controls.DockPanel.SetDock(valuesSection, Avalonia.Controls.Dock.Bottom);
            outer.Children.Add(header);
            outer.Children.Add(valuesSection);
            outer.Children.Add(hvc);   // fills remaining space
            return outer;
        }

        Avalonia.Controls.Control BuildHexValuesSection()
        {
            // ── Textbox factory ──────────────────────────────────────────────
            Avalonia.Controls.TextBox MakeTb(bool readOnly = true)
            {
                var tb = new Avalonia.Controls.TextBox
                {
                    Background          = Avalonia.Media.Brushes.White,
                    Padding             = new Avalonia.Thickness(3, 1),
                    MinHeight           = 0,
                    FontSize            = 11,
                    IsReadOnly          = readOnly,
                    BorderThickness     = new Avalonia.Thickness(1),
                    Margin              = new Avalonia.Thickness(0, 2, 8, 2),
                    HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Left,
                };
                return tb;
            }
            Avalonia.Controls.TextBlock MakeLbl(string text) => new Avalonia.Controls.TextBlock
            {
                Text              = text,
                FontSize          = 11,
                VerticalAlignment = Avalonia.Layout.VerticalAlignment.Center,
                Margin            = new Avalonia.Thickness(0, 0, 4, 0),
            };
            void Place(Avalonia.Controls.Grid g, Avalonia.Controls.Control c, int col, int row)
            {
                Avalonia.Controls.Grid.SetColumn(c, col);
                Avalonia.Controls.Grid.SetRow(c, row);
                g.Children.Add(c);
            }

            // ── Main form Grid: 4 columns, 4 rows (rows 0-3) ────────────────
            // All columns Auto so textboxes keep their fixed widths.
            // Col 0: left label   Col 1: left textbox (fixed width)
            // Col 2: right label  Col 3: right textbox (fixed width)
            var form = new Avalonia.Controls.Grid { Margin = new Avalonia.Thickness(6, 4, 6, 0) };
            var Auto = Avalonia.Controls.GridLength.Auto;
            form.ColumnDefinitions.Add(new Avalonia.Controls.ColumnDefinition(Auto));
            form.ColumnDefinitions.Add(new Avalonia.Controls.ColumnDefinition(Auto));
            form.ColumnDefinitions.Add(new Avalonia.Controls.ColumnDefinition(Auto));
            form.ColumnDefinitions.Add(new Avalonia.Controls.ColumnDefinition(Auto));
            for (int r = 0; r < 4; r++)
                form.RowDefinitions.Add(new Avalonia.Controls.RowDefinition(Auto));

            // Row 0: Byte | [tb 80] | Int | [tb 110]
            var tbByte  = MakeTb(); tbByte.Width  = 80;
            var tbInt   = MakeTb(); tbInt.Width   = 110;
            Place(form, MakeLbl("Byte:"),  0, 0); Place(form, tbByte, 1, 0);
            Place(form, MakeLbl("Int:"),   2, 0); Place(form, tbInt,  3, 0);

            // Row 1: Short | [tb 80] | Long | [tb 150]
            var tbShort = MakeTb(); tbShort.Width = 80;
            var tbLong  = MakeTb(); tbLong.Width  = 150;
            Place(form, MakeLbl("Short:"), 0, 1); Place(form, tbShort, 1, 1);
            Place(form, MakeLbl("Long:"),  2, 1); Place(form, tbLong,  3, 1);

            // Row 2: Single | [tb 100] | Double | [tb 180]
            var tbSingle = MakeTb(); tbSingle.Width = 100;
            var tbDouble = MakeTb(); tbDouble.Width = 180;
            Place(form, MakeLbl("Single:"), 0, 2); Place(form, tbSingle, 1, 2);
            Place(form, MakeLbl("Double:"), 2, 2); Place(form, tbDouble, 3, 2);

            // Row 3: Binary | [tb1 72] [tb2 72] [tb3 72] [tb4 72]  spans cols 1-3
            var tbBin1 = MakeTb(); var tbBin2 = MakeTb();
            var tbBin3 = MakeTb(); var tbBin4 = MakeTb();
            foreach (var tb in new[] { tbBin1, tbBin2, tbBin3, tbBin4 })
            {
                tb.Width  = 72;
                tb.Margin = new Avalonia.Thickness(0, 2, 4, 2);
            }
            var binBoxes = new Avalonia.Controls.StackPanel { Orientation = Avalonia.Layout.Orientation.Horizontal };
            binBoxes.Children.Add(tbBin1); binBoxes.Children.Add(tbBin2);
            binBoxes.Children.Add(tbBin3); binBoxes.Children.Add(tbBin4);
            Avalonia.Controls.Grid.SetColumnSpan(binBoxes, 3);
            Place(form, MakeLbl("Binary:"), 0, 3); Place(form, binBoxes, 1, 3);

            // Row 4 (Offset / Highlight / Commit): a DockPanel so Highlight fills remaining width.
            var tbOffset = MakeTb(); tbOffset.Width = 100;
            var tbHigh   = MakeTb(false);  // no fixed Width — fills remaining space
            tbHigh.Margin              = new Avalonia.Thickness(0, 2, 8, 2);
            tbHigh.HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Stretch;

            button1.VerticalAlignment   = Avalonia.Layout.VerticalAlignment.Center;
            button1.HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Right;
            button1.Margin              = new Avalonia.Thickness(0, 2, 0, 2);

            var lbOff  = MakeLbl("Offset:");

            // "Highlight:" is a blue hyperlink that searches for the typed hex bytes
            // and selects the first occurrence in the hex view.
            var lbHigh = new Avalonia.Controls.Button
            {
                Content             = "Highlight:",
                FontSize            = 11,
                Foreground          = Avalonia.Media.Brushes.Blue,
                Background          = Avalonia.Media.Brushes.Transparent,
                BorderThickness     = new Avalonia.Thickness(0),
                Padding             = new Avalonia.Thickness(0, 0, 4, 0),
                VerticalAlignment   = Avalonia.Layout.VerticalAlignment.Center,
                Cursor              = new Avalonia.Input.Cursor(Avalonia.Input.StandardCursorType.Hand),
            };
            lbHigh.Click += (s, e) =>
            {
                string raw = tbHigh.Text?.Trim() ?? "";
                // Parse hex bytes in any spacing/dash format
                raw = raw.Replace("-", " ");
                while (raw.Contains("  ")) raw = raw.Replace("  ", " ");
                if (raw.Length > 0 && !raw.Contains(" "))
                {
                    // No spaces — insert them every 2 chars
                    var sb = new System.Text.StringBuilder();
                    for (int i = 0; i < raw.Length; i += 2)
                    {
                        if (sb.Length > 0) sb.Append(' ');
                        sb.Append(raw.Length - i >= 2 ? raw.Substring(i, 2) : raw.Substring(i));
                    }
                    raw = sb.ToString();
                }
                string[] parts = raw.Trim().Split(' ', System.StringSplitOptions.RemoveEmptyEntries);
                if (parts.Length == 0) { hvc.Highlight(System.Array.Empty<byte>()); return; }
                var data = new byte[parts.Length];
                for (int i = 0; i < parts.Length; i++)
                    try { data[i] = System.Convert.ToByte(parts[i], 16); } catch { data[i] = 0; }
                // Normalize back to "FF 00 AB" format in the textbox
                tbHigh.Text = System.BitConverter.ToString(data).Replace("-", " ");
                hvc.Highlight(data);
            };

            Avalonia.Controls.DockPanel.SetDock(lbOff,    Avalonia.Controls.Dock.Left);
            Avalonia.Controls.DockPanel.SetDock(tbOffset, Avalonia.Controls.Dock.Left);
            Avalonia.Controls.DockPanel.SetDock(lbHigh,   Avalonia.Controls.Dock.Left);
            Avalonia.Controls.DockPanel.SetDock(button1,  Avalonia.Controls.Dock.Right);
            var bottomRow = new Avalonia.Controls.DockPanel
            {
                LastChildFill = true,
                Margin        = new Avalonia.Thickness(6, 0, 6, 4),
            };
            bottomRow.Children.Add(lbOff);
            bottomRow.Children.Add(tbOffset);
            bottomRow.Children.Add(lbHigh);
            bottomRow.Children.Add(button1);
            bottomRow.Children.Add(tbHigh);  // last child fills remaining space

            // ── Right column: radio buttons + checkboxes ─────────────────────
            var rbHex      = new Avalonia.Controls.RadioButton { Content = "Hex",           IsChecked = true, GroupName = "hvcView", FontSize = 11, Margin = new Avalonia.Thickness(0, 0, 0, 2) };
            var rbSigned   = new Avalonia.Controls.RadioButton { Content = "signed Dec.",    GroupName = "hvcView", FontSize = 11, Margin = new Avalonia.Thickness(0, 0, 0, 2) };
            var rbUnsigned = new Avalonia.Controls.RadioButton { Content = "unsigned Dec.",  GroupName = "hvcView", FontSize = 11, Margin = new Avalonia.Thickness(0, 0, 0, 4) };
            var cbHL       = new Avalonia.Controls.CheckBox    { Content = "Highlight Zeros", FontSize = 11, Margin = new Avalonia.Thickness(0, 0, 0, 2) };
            var cbGrid     = new Avalonia.Controls.CheckBox    { Content = "Show Grid", IsChecked = true, FontSize = 11 };

            var rightCol = new Avalonia.Controls.StackPanel
            {
                Margin            = new Avalonia.Thickness(12, 4, 8, 4),
                VerticalAlignment = Avalonia.Layout.VerticalAlignment.Top,
            };
            rightCol.Children.Add(rbHex); rightCol.Children.Add(rbSigned);
            rightCol.Children.Add(rbUnsigned); rightCol.Children.Add(cbHL); rightCol.Children.Add(cbGrid);

            // ── Outer layout: form (Star) + right column (Auto) ──────────────
            var outer = new Avalonia.Controls.Grid();
            outer.ColumnDefinitions.Add(new Avalonia.Controls.ColumnDefinition(new Avalonia.Controls.GridLength(1, Avalonia.Controls.GridUnitType.Star)));
            outer.ColumnDefinitions.Add(new Avalonia.Controls.ColumnDefinition(Auto));
            Avalonia.Controls.Grid.SetColumn(form,     0);
            Avalonia.Controls.Grid.SetColumn(rightCol, 1);
            outer.Children.Add(form); outer.Children.Add(rightCol);

            var title = new Avalonia.Controls.TextBlock
            {
                Text       = "Values / Navigation:",
                FontWeight = Avalonia.Media.FontWeight.SemiBold,
                FontSize   = 11,
                Margin     = new Avalonia.Thickness(6, 4, 0, 0),
            };
            var inner = new Avalonia.Controls.StackPanel();
            inner.Children.Add(title);
            inner.Children.Add(outer);
            inner.Children.Add(bottomRow);

            return new Avalonia.Controls.Border
            {
                BorderBrush     = new Avalonia.Media.SolidColorBrush(Avalonia.Media.Color.FromRgb(170, 185, 205)),
                BorderThickness = new Avalonia.Thickness(0, 1, 0, 0),
                Background      = new Avalonia.Media.SolidColorBrush(Avalonia.Media.Color.FromRgb(220, 228, 238)),
                Child           = inner,
            };
        }

        #endregion

        #region Windows Form Designer generated code
        private void InitializeComponent()
        {
            // ── field instantiation ───────────────────────────────────────────
            this.manager         = new Ambertation.Windows.Forms.DockManager();
            this.dockBottom      = new Ambertation.Windows.Forms.DockContainer();
            this.dcConvert       = new Ambertation.Windows.Forms.DockPanel();
            this.dcPackage       = new Ambertation.Windows.Forms.DockPanel();
            this.dcWrapper       = new Ambertation.Windows.Forms.DockPanel();
            this.dcResource      = new Ambertation.Windows.Forms.DockPanel();
            this.dcHex           = new Ambertation.Windows.Forms.DockPanel();
            this.xpGradientPanel1 = new Avalonia.Controls.StackPanel();
            this.xpGradientPanel2 = new Avalonia.Controls.StackPanel();
            this.xpGradientPanel3 = new Avalonia.Controls.StackPanel();
            this.xpGradientPanel4 = new Avalonia.Controls.StackPanel();
            this.gradientpanel1  = new Avalonia.Controls.StackPanel();
            this.panel1          = new Avalonia.Controls.StackPanel();
            this.pntypes         = new Avalonia.Controls.StackPanel();
            this.hvc             = new Ambertation.Windows.Forms.HexViewControl();
            this.hexEditControl1 = new Ambertation.Windows.Forms.HexEditControl();
            this.pgHead          = new Compat.PropertyGridStub();
            this.lv              = new Compat.ListView();
            this.clOffset        = new Compat.ColumnHeader();
            this.clSize          = new Compat.ColumnHeader();
            this.tbinstance      = new Avalonia.Controls.TextBox();
            this.tbtype          = new Avalonia.Controls.TextBox();
            this.tbgroup         = new Avalonia.Controls.TextBox();
            this.tbinstance2     = new Avalonia.Controls.TextBox();
            this.tbHex           = new Avalonia.Controls.TextBox();
            this.tbDec           = new Avalonia.Controls.TextBox();
            this.tbBin           = new Avalonia.Controls.TextBox();
            this.tbFloat         = new Avalonia.Controls.TextBox();
            this.cbtypes         = new Avalonia.Controls.ComboBox();
            this.cbComp          = new Avalonia.Controls.ComboBox();
            this.button1         = new Avalonia.Controls.Button();
            this.btcopie         = new Avalonia.Controls.Button();
            this.linkLabel1      = new Avalonia.Controls.Button();
            this.pb              = new Compat.PictureBoxCompat();
            this.label1          = new Avalonia.Controls.TextBlock();
            this.label2          = new Avalonia.Controls.TextBlock();
            this.label3          = new Avalonia.Controls.TextBlock();
            this.label4          = new Avalonia.Controls.TextBlock();
            this.label5          = new Avalonia.Controls.TextBlock();
            this.label6          = new Avalonia.Controls.TextBlock();
            this.label7          = new Avalonia.Controls.TextBlock();
            this.label8          = new Avalonia.Controls.TextBlock();
            this.label9          = new Avalonia.Controls.TextBlock();
            this.label10         = new Avalonia.Controls.TextBlock();
            this.label11         = new Avalonia.Controls.TextBlock();
            this.label12         = new Avalonia.Controls.TextBlock();
            this.label13         = new Avalonia.Controls.TextBlock();
            this.lbName          = new Avalonia.Controls.TextBlock();
            this.lbAuthor        = new Avalonia.Controls.TextBlock();
            this.lbVersion       = new Avalonia.Controls.TextBlock();
            this.lbDesc          = new Avalonia.Controls.TextBlock();
            this.lbComp          = new Avalonia.Controls.TextBlock();

            // ── event wiring ──────────────────────────────────────────────────
            this.tbHex.TextChanged      += (s, e) => this.HexChanged(s, null);
            this.tbDec.TextChanged      += (s, e) => this.DecChanged(s, null);
            this.tbBin.TextChanged      += (s, e) => this.BinChanged(s, null);
            this.tbBin.SizeChanged      += (s, e) => this.tbBin_SizeChanged(s, null);
            this.tbFloat.TextChanged    += (s, e) => this.FloatChanged(s, null);
            this.tbtype.TextChanged     += (s, e) => this.tbtype_TextChanged(s, null);
            this.tbgroup.TextChanged    += (s, e) => this.TextChanged(s, null);
            this.tbinstance.TextChanged += (s, e) => this.TextChanged(s, null);
            this.tbinstance2.TextChanged += (s, e) => this.TextChanged(s, null);
            this.cbtypes.SelectionChanged += (s, e) => cbtypes_SelectedIndexChanged(s, e);
            this.cbComp.SelectionChanged  += (s, e) => cbComp_SelectedIndexChanged(s, e);
            this.dcHex.VisibleChanged     += new System.EventHandler(this.dcHex_VisibleChanged);

            // ── Tab labels and icons ──────────────────────────────────────────
            this.dcHex.TabText      = "Hex";
            this.dcPackage.TabText  = "Package";
            this.dcWrapper.TabText  = "Wrapper";
            this.dcResource.TabText = "Resource";
            this.dcConvert.TabText  = "Converter";

            this.dcHex.TabIconBitmap      = SimPe.LoadIcon.LoadAvaloniaBitmap("ResourceDock_dcHex.TabImage.png");
            this.dcPackage.TabIconBitmap  = SimPe.LoadIcon.LoadAvaloniaBitmap("ResourceDock_dcPackage.TabImage.png");
            this.dcWrapper.TabIconBitmap  = SimPe.LoadIcon.LoadAvaloniaBitmap("ResourceDock_dcWrapper.TabImage.png");
            this.dcResource.TabIconBitmap = SimPe.LoadIcon.LoadAvaloniaBitmap("ResourceDock_dcResource.TabImage.png");
            this.dcConvert.TabIconBitmap  = SimPe.LoadIcon.LoadAvaloniaBitmap("ResourceDock_dcConvert.TabImage.png");

            // ── Build and attach panel layouts ────────────────────────────────
            dcPackage.AvaloniaContent = BuildPackagePanel();   dcPackage.Controls.Add(dcPackage.AvaloniaContent);
            dcResource.AvaloniaContent = BuildResourcePanel(); dcResource.Controls.Add(dcResource.AvaloniaContent);
            dcWrapper.AvaloniaContent = BuildWrapperPanel();   dcWrapper.Controls.Add(dcWrapper.AvaloniaContent);
            dcConvert.AvaloniaContent = BuildConverterPanel(); dcConvert.Controls.Add(dcConvert.AvaloniaContent);
            dcHex.AvaloniaContent = BuildHexPanel();           dcHex.Controls.Add(dcHex.AvaloniaContent);
        }
        #endregion

        internal SimPe.Events.ResourceEventArgs items;
        internal LoadedPackage guipackage;

        private void ResourceDock_Load(object sender, System.EventArgs e)
        {

        }

        private void cbtypes_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            if (cbtypes.Tag != null) return;
            tbtype.Text = "0x" + Helper.HexString(((SimPe.Data.TypeAlias)cbtypes.Items[cbtypes.SelectedIndex]).Id);
            this.tbtype.Tag = true;
            tbtype_TextChanged2(this.tbtype, e);
        }

        private void tbtype_TextChanged(object sender, System.EventArgs e)
        {
            cbtypes.Tag = true;
            Data.TypeAlias a = Data.MetaData.FindTypeAlias(Helper.HexStringToUInt(tbtype.Text));

            int ct = 0;
            foreach (Data.TypeAlias i in cbtypes.Items)
            {
                if (i == a)
                {
                    cbtypes.SelectedIndex = ct;
                    cbtypes.Tag = null;
                    return;
                }
                ct++;
            }

            cbtypes.SelectedIndex = -1;
            cbtypes.Tag = null;
            TextChanged(sender, null);
        }

        private void tbtype_TextChanged2(object sender, System.EventArgs ea)
        {
            if (items == null || ((TextBox)sender).Tag == null) return;
            ((TextBox)sender).Tag = null;
            guipackage.PauseIndexChangedEvents();
            foreach (SimPe.Events.ResourceContainer e in items)
            {
                if (!e.HasFileDescriptor) continue;
                try
                {
                    e.Resource.FileDescriptor.Type = Convert.ToUInt32(tbtype.Text, 16);

                    e.Resource.FileDescriptor.Changed = true;
                }
                catch { }
            }
            guipackage.PauseIndexChangedEvents();
            guipackage.RestartIndexChangedEvents();
        }

        private void tbgroup_TextChanged(object sender, System.EventArgs ea)
        {
            if (items == null || ((TextBox)sender).Tag == null) return;
            ((TextBox)sender).Tag = null;

            guipackage.PauseIndexChangedEvents();
            foreach (SimPe.Events.ResourceContainer e in items)
            {
                if (!e.HasFileDescriptor) continue;
                try
                {
                    e.Resource.FileDescriptor.Group = Convert.ToUInt32(tbgroup.Text, 16);

                    e.Resource.FileDescriptor.Changed = true;
                }
                catch { }
            }
            guipackage.PauseIndexChangedEvents();
            guipackage.RestartIndexChangedEvents();
        }

        private void tbinstance_TextChanged(object sender, System.EventArgs ea)
        {
            if (items == null || ((TextBox)sender).Tag == null) return;
            ((TextBox)sender).Tag = null;

            guipackage.PauseIndexChangedEvents();
            foreach (SimPe.Events.ResourceContainer e in items)
            {
                if (!e.HasFileDescriptor) continue;


                try
                {
                    e.Resource.FileDescriptor.Instance = Convert.ToUInt32(tbinstance.Text, 16);

                    e.Resource.FileDescriptor.Changed = true;
                }
                catch { }
            }

            guipackage.PauseIndexChangedEvents();
            guipackage.RestartIndexChangedEvents();

        }

        private void tbinstance2_TextChanged(object sender, System.EventArgs ea)
        {
            if (items == null || ((TextBox)sender).Tag == null) return;
            ((TextBox)sender).Tag = null;

            guipackage.PauseIndexChangedEvents();
            foreach (SimPe.Events.ResourceContainer e in items)
            {
                if (!e.HasFileDescriptor) continue;


                try
                {
                    e.Resource.FileDescriptor.SubType = Convert.ToUInt32(tbinstance2.Text, 16);
                    e.Resource.FileDescriptor.Changed = true;
                }
                catch { }
            }
            guipackage.PauseIndexChangedEvents();
            guipackage.RestartIndexChangedEvents();
        }


        private void cbComp_SelectedIndexChanged(object sender, System.EventArgs ea)
        {
            if (this.cbComp.SelectedIndex < 0) return;
            if (this.cbComp.SelectedIndex > 1) return;
            if (items == null) return;

            guipackage.PauseIndexChangedEvents();
            foreach (SimPe.Events.ResourceContainer e in items)
            {
                if (!e.HasFileDescriptor) continue;

                try
                {
                    e.Resource.FileDescriptor.MarkForReCompress = (cbComp.SelectedIndex == 1);
                    if (!e.Resource.FileDescriptor.MarkForReCompress && e.Resource.FileDescriptor.WasCompressed)
                    {
                        e.Resource.FileDescriptor.UserData = e.Resource.Package.Read(e.Resource.FileDescriptor).UncompressedData;
                    }
                    e.Resource.FileDescriptor.Changed = true;
                }
                catch { }
            }
            guipackage.PauseIndexChangedEvents();
            guipackage.RestartIndexChangedEvents();
        }

        private void tbtype_KeyUp(object sender, Avalonia.Input.KeyEventArgs e)
        {
            if (e.Key == Avalonia.Input.Key.Return)
            {
                TextChanged(sender, null);
                this.tbtype_TextChanged2(sender, null);
            }
        }

        private void tbgroup_KeyUp(object sender, Avalonia.Input.KeyEventArgs e)
        {
            if (e.Key == Avalonia.Input.Key.Return)
            {
                TextChanged(sender, null);
                this.tbgroup_TextChanged(sender, null);
            }
        }

        private void tbinstance_KeyUp(object sender, Avalonia.Input.KeyEventArgs e)
        {
            if (e.Key == Avalonia.Input.Key.Return)
            {
                TextChanged(sender, null);
                this.tbinstance_TextChanged(sender, null);
            }
        }

        private void tbinstance2_KeyUp(object sender, Avalonia.Input.KeyEventArgs e)
        {
            if (e.Key == Avalonia.Input.Key.Return)
            {
                TextChanged(sender, null);
                this.tbinstance2_TextChanged(sender, null);
            }
        }

        #region Hex <-> Dec Converter
        bool sysupdate = false;
        void SetConverted(object exclude, long val)
        {
            if (exclude != this.tbDec) this.tbDec.Text = val.ToString();
            if (exclude != this.tbHex) this.tbHex.Text = Helper.HexString(val);
            if (exclude != this.tbBin) this.tbBin.Text = Convert.ToString(val, 2); ;
            if (exclude != this.tbFloat) this.tbFloat.Text = BitConverter.ToSingle(BitConverter.GetBytes((int)val), 0).ToString();
        }
        void ClearConverted(object exclude)
        {
            if (exclude != this.tbDec) this.tbDec.Text = "";
            if (exclude != this.tbHex) this.tbHex.Text = "";
            if (exclude != this.tbBin) this.tbBin.Text = "";
            if (exclude != this.tbFloat) this.tbFloat.Text = "";
        }
        private void FloatChanged(object sender, System.EventArgs e)
        {
            if (sysupdate) return;
            sysupdate = true;
            try
            {
                float f = Convert.ToSingle(tbFloat.Text);
                long val = BitConverter.ToInt32(BitConverter.GetBytes(f), 0);
                SetConverted(this.tbFloat, val);
            }
            catch
            {
                ClearConverted(this.tbFloat);
            }
            sysupdate = false;
        }
        private void BinChanged(object sender, System.EventArgs e)
        {
            if (sysupdate) return;
            sysupdate = true;
            try
            {
                long val = Convert.ToInt64(tbBin.Text.Replace(" ", ""), 2);
                SetConverted(this.tbBin, val);
            }
            catch
            {
                ClearConverted(this.tbBin);
            }
            sysupdate = false;
        }
        private void HexChanged(object sender, System.EventArgs e)
        {
            if (sysupdate) return;
            sysupdate = true;
            try
            {
                long val = Convert.ToInt64(tbHex.Text.Replace(" ", ""), 16);
                SetConverted(this.tbHex, val);
            }
            catch
            {
                ClearConverted(this.tbHex);
            }
            sysupdate = false;
        }

        private void DecChanged(object sender, System.EventArgs e)
        {
            if (sysupdate) return;
            sysupdate = true;
            try
            {
                long val = Convert.ToInt64(tbDec.Text);
                SetConverted(this.tbDec, val);
            }
            catch (Exception)
            {
                ClearConverted(this.tbDec);
            }
            sysupdate = false;
        }
        #endregion

        internal SimPe.Interfaces.Files.IPackedFileDescriptor hexpfd;
        private void TextChanged(object sender, System.EventArgs e)
        {
            if (items == null) return;
            ((TextBox)sender).Tag = true;
        }


        private void btcopie_Click(object sender, System.EventArgs e)
        {
            int i = 1;
            string s = "";
            string d;
            foreach (byte b in hvc.Data)
            {

                d = b.ToString("X");
                if (d.Length == 1) d = "0" + d;
                s += d;
                if (i == 24) { s += " \r\n"; i = 0; }
                else s += " ";
                i++;
            }
            // TODO: await TopLevel.GetTopLevel(this)?.Clipboard?.SetTextAsync(s);
        }

        private void button1_Click(object sender, System.EventArgs e)
        {
            hexpfd.UserData = hvc.Data;
        }

        private void dcHex_VisibleChanged(object sender, System.EventArgs e)
        {
            this.hvc.IsVisible = dcHex.Visible;
            hvc.Refresh(true);
        }

        private void linkLabel1_LinkClicked(object sender, System.EventArgs ev)
        {
            if (items == null) return;
            guipackage.PauseIndexChangedEvents();
            foreach (SimPe.Events.ResourceContainer e in items)
            {
                if (!e.HasFileDescriptor) continue;
                try
                {
                    e.Resource.FileDescriptor.Type = Convert.ToUInt32(tbtype.Text, 16);
                    e.Resource.FileDescriptor.Group = Convert.ToUInt32(tbgroup.Text, 16);
                    e.Resource.FileDescriptor.Instance = Convert.ToUInt32(tbinstance.Text, 16);
                    e.Resource.FileDescriptor.SubType = Convert.ToUInt32(tbinstance2.Text, 16);
                    e.Resource.FileDescriptor.MarkForReCompress = (cbComp.SelectedIndex == 1 && !e.Resource.FileDescriptor.WasCompressed);

                    e.Resource.FileDescriptor.Changed = true;
                }
                catch { }
            }
            guipackage.RestartIndexChangedEvents();
        }

        private void tbBin_SizeChanged(object sender, System.EventArgs e)
        {
            tbFloat.Width = tbBin.Width;
        }
    }
}
