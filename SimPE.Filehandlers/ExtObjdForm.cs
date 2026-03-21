/***************************************************************************
 *   Copyright (C) 2005 by Peter L Jones                                   *
 *   pljones@users.sf.net                                                  *
 *                                                                         *
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
using System.Collections.Generic;
using System.Collections;
using Avalonia.Controls;
using Avalonia.Layout;
using SimPe.Interfaces.Plugin;
using SimPe.PackedFiles.Wrapper;
using SimPe.Interfaces.Files;

namespace SimPe.PackedFiles.UserInterface
{
    /// <summary>
    /// Avalonia port of ExtObjdForm.
    /// PropertyGrid replaced with read-only TextBox dump.
    /// EnumComboBox replaced with standard ComboBox (manual SelectedValue tracking).
    /// LinkLabel replaced with Button.
    /// ShowDialog pattern removed — GUIHandle returns the pnobjd panel directly.
    /// </summary>
    internal class ExtObjdForm : Avalonia.Controls.UserControl, IPackedFileUI
    {
        #region Fields
        private Button btnUpdateMMAT = new Button { Content = "Update", IsVisible = false };
        private TextBlock label2 = new TextBlock { Text = "MMATs and commit", IsVisible = false };
        // PropertyGrid -> read-only TextBox dump
        internal TextBox pg = new TextBox { AcceptsReturn = true, IsReadOnly = true };
        internal TabControl tc = new TabControl();
        internal TabItem tpcatalogsort = new TabItem { Header = "Catalogue Sort" };
        private TabItem tpraw = new TabItem { Header = "RAW Data" };
        private TabItem tpreqeps = new TabItem { Header = "Required Ep" };
        internal CheckBox cbhobby = new CheckBox { Content = "Hobbies" };
        internal CheckBox cbaspiration = new CheckBox { Content = "Aspiration" };
        internal CheckBox cbcareer = new CheckBox { Content = "Career Reward" };
        internal CheckBox cbkids = new CheckBox { Content = "Kids" };
        private Panel panel1 = new Panel();
        private RadioButton rbbin = new RadioButton { Content = "Binary", GroupName = "digitbase" };
        private RadioButton rbdec = new RadioButton { Content = "Decimal", GroupName = "digitbase" };
        private RadioButton rbhex = new RadioButton { Content = "Hexadecimal", GroupName = "digitbase" };
        private CheckBox cball = new CheckBox { Content = "update all MMATs", IsVisible = false };
        internal TextBlock lbIsOk = new TextBlock { Text = "Please commit!", IsVisible = false };
        private TextBlock label1 = new TextBlock { Text = "Overall Sort:" };
        // EnumComboBox -> standard ComboBox; SelectedValue tracked manually
        internal ComboBox cbsort = new ComboBox();
        private TextBlock label63 = new TextBlock { Text = "Orig. GUID" };
        internal TextBox tbproxguid = new TextBox { Text = "0xDDDDDDDD" };
        private TextBlock label97 = new TextBlock { Text = "Fallback GUID" };
        internal TextBox tborgguid = new TextBox { Text = "0xDDDDDDDD" };
        // LinkLabel -> Button
        private Button llgetGUID = new Button { Content = "make GUID" };
        private Button lladdgooee = new Button { Content = "Add To pjse Guid Index", IsVisible = false };
        private TextBlock label65 = new TextBlock { Text = "Object Type" };
        private TextBlock label9 = new TextBlock { Text = "Filename" };
        private TextBlock label8 = new TextBlock { Text = "GUID" };
        private TextBlock label3 = new TextBlock { Text = "Diagonal GUID" };
        private TextBlock label4 = new TextBlock { Text = "Grid Align GUID" };

        internal TextBox tbflname = new TextBox();
        internal TextBox tbguid = new TextBox { Text = "0xDDDDDDDD" };
        internal ComboBox cbtype = new ComboBox();
        internal TextBox tbtype = new TextBox { IsReadOnly = true, Text = "0xDDDD" };

        internal CheckBox cbbathroom = new CheckBox { Content = "Bathroom" };
        internal CheckBox cbbedroom = new CheckBox { Content = "Bedroom" };
        internal CheckBox cbdinigroom = new CheckBox { Content = "Diningroom" };
        internal CheckBox cbkitchen = new CheckBox { Content = "Kitchen" };
        internal CheckBox cbstudy = new CheckBox { Content = "Study" };
        internal CheckBox cblivingroom = new CheckBox { Content = "Livingroom" };
        internal CheckBox cboutside = new CheckBox { Content = "Outside" };
        internal CheckBox cbmisc = new CheckBox { Content = "Misc." };
        internal CheckBox cbgeneral = new CheckBox { Content = "General" };
        internal CheckBox cbelectronics = new CheckBox { Content = "Electronics" };
        internal CheckBox cbdecorative = new CheckBox { Content = "Decorative" };
        internal CheckBox cbappliances = new CheckBox { Content = "Appliances" };
        internal CheckBox cbsurfaces = new CheckBox { Content = "Surfaces" };
        internal CheckBox cbseating = new CheckBox { Content = "Seating" };
        internal CheckBox cbplumbing = new CheckBox { Content = "Plumbing" };
        internal CheckBox cblightning = new CheckBox { Content = "Lights" };

        internal TextBox tbdiag = new TextBox { Text = "0xDDDDDDDD" };
        internal TextBox tbgrid = new TextBox { Text = "0xDDDDDDDD" };

        // pnobjd is the panel returned by GUIHandle
        private Panel pnobjd = new Panel();
        private Panel panel6 = new Panel();
        private Button btnCommit = new Button { Content = "Commit", IsVisible = false };
        private ComboBox cbBuildSort = new ComboBox();
        private TextBlock label5 = new TextBlock { Text = "Build Mode Sort:" };

        private CheckBox cbcMisc = new CheckBox { Content = "Misc." };
        private CheckBox cbcStreet = new CheckBox { Content = "Street" };
        private CheckBox cbcOuts = new CheckBox { Content = "Outdoors" };
        private CheckBox cbcShop = new CheckBox { Content = "Shopping" };
        private CheckBox cbcDine = new CheckBox { Content = "Dining" };
        private Panel tbreqeps = new Panel();

        private TextBlock lbepnote = new TextBlock { Text = "These Flags are 'OR' If you set two EPs then either EP is required, not both" };
        private TextBlock lbgamef2 = new TextBlock { Text = "Game Edition Flags 2" };
        private CheckBox cbStoreEd = new CheckBox { Content = "Store Edition (new)" };
        private CheckBox cbMansion = new CheckBox { Content = "Mansion + Garden" };
        private CheckBox cbApartments = new CheckBox { Content = "Apartment Life" };
        private CheckBox cbIkeaHome = new CheckBox { Content = "IKEA Home" };
        private CheckBox cbKitchens = new CheckBox { Content = "Kitchen + Bathroom" };
        private CheckBox cbFreeTime = new CheckBox { Content = "Free Time" };
        private CheckBox cbExtras = new CheckBox { Content = "Store Edition (old)" };
        private CheckBox cbTeenStyle = new CheckBox { Content = "Teen Style" };
        private CheckBox cbBonVoyage = new CheckBox { Content = "Bon Voyage" };
        private CheckBox cbFashion = new CheckBox { Content = "HM Fashion Stuff" };
        private CheckBox cbCelebrations = new CheckBox { Content = "Celebration!" };
        private CheckBox cbSeasons = new CheckBox { Content = "Seasons" };
        private CheckBox cbPets = new CheckBox { Content = "Pets" };
        private CheckBox cbGlamour = new CheckBox { Content = "Glamour Life" };
        private CheckBox cbFamilyFun = new CheckBox { Content = "Family Fun" };
        private CheckBox cbBusiness = new CheckBox { Content = "Open for Business" };
        private CheckBox cbNightlife = new CheckBox { Content = "Nightlife" };
        private CheckBox cbUniversity = new CheckBox { Content = "University" };
        private CheckBox cbBase = new CheckBox { Content = "Base Game" };

        private TextBlock lbprise = new TextBlock { Text = "Price:" };
        internal TextBox tbPrice = new TextBox { Text = "0" };
        #endregion

        public ExtObjdForm()
        {
            // Populate cbtype items
            cbtype.Items.Add(Data.ObjectTypes.Unknown);
            cbtype.Items.Add(Data.ObjectTypes.ArchitecturalSupport);
            cbtype.Items.Add(Data.ObjectTypes.Door);
            cbtype.Items.Add(Data.ObjectTypes.Memory);
            cbtype.Items.Add(Data.ObjectTypes.ModularStairs);
            cbtype.Items.Add(Data.ObjectTypes.ModularStairsPortal);
            cbtype.Items.Add(Data.ObjectTypes.Normal);
            cbtype.Items.Add(Data.ObjectTypes.Outfit);
            cbtype.Items.Add(Data.ObjectTypes.Person);
            cbtype.Items.Add(Data.ObjectTypes.SimType);
            cbtype.Items.Add(Data.ObjectTypes.Stairs);
            cbtype.Items.Add(Data.ObjectTypes.Template);
            cbtype.Items.Add(Data.ObjectTypes.Vehicle);
            cbtype.Items.Add(Data.ObjectTypes.Window);
            cbtype.Items.Add(Data.ObjectTypes.UnlinkedSim);
            cbtype.Items.Add(Data.ObjectTypes.Tiles);

            // Populate cbBuildSort items
            cbBuildSort.Items.Add(new SimPe.Data.LocalizedBuildSubSort(Data.BuildFunctionSubSort.none));
            cbBuildSort.Items.Add(new SimPe.Data.LocalizedBuildSubSort(Data.BuildFunctionSubSort.General_Columns));
            cbBuildSort.Items.Add(new SimPe.Data.LocalizedBuildSubSort(Data.BuildFunctionSubSort.General_Stairs));
            cbBuildSort.Items.Add(new SimPe.Data.LocalizedBuildSubSort(Data.BuildFunctionSubSort.General_Pool));
            cbBuildSort.Items.Add(new SimPe.Data.LocalizedBuildSubSort(Data.BuildFunctionSubSort.General_TallColumns));
            cbBuildSort.Items.Add(new SimPe.Data.LocalizedBuildSubSort(Data.BuildFunctionSubSort.General_Arch));
            cbBuildSort.Items.Add(new SimPe.Data.LocalizedBuildSubSort(Data.BuildFunctionSubSort.General_Driveway));
            cbBuildSort.Items.Add(new SimPe.Data.LocalizedBuildSubSort(Data.BuildFunctionSubSort.General_Elevator));
            cbBuildSort.Items.Add(new SimPe.Data.LocalizedBuildSubSort(Data.BuildFunctionSubSort.General_Architectural));
            cbBuildSort.Items.Add(new SimPe.Data.LocalizedBuildSubSort(Data.BuildFunctionSubSort.Garden_Trees));
            cbBuildSort.Items.Add(new SimPe.Data.LocalizedBuildSubSort(Data.BuildFunctionSubSort.Garden_Shrubs));
            cbBuildSort.Items.Add(new SimPe.Data.LocalizedBuildSubSort(Data.BuildFunctionSubSort.Garden_Flowers));
            cbBuildSort.Items.Add(new SimPe.Data.LocalizedBuildSubSort(Data.BuildFunctionSubSort.Garden_Objects));
            cbBuildSort.Items.Add(new SimPe.Data.LocalizedBuildSubSort(Data.BuildFunctionSubSort.Openings_Door));
            cbBuildSort.Items.Add(new SimPe.Data.LocalizedBuildSubSort(Data.BuildFunctionSubSort.Openings_TallWindow));
            cbBuildSort.Items.Add(new SimPe.Data.LocalizedBuildSubSort(Data.BuildFunctionSubSort.Openings_Window));
            cbBuildSort.Items.Add(new SimPe.Data.LocalizedBuildSubSort(Data.BuildFunctionSubSort.Openings_Gate));
            cbBuildSort.Items.Add(new SimPe.Data.LocalizedBuildSubSort(Data.BuildFunctionSubSort.Openings_Arch));
            cbBuildSort.Items.Add(new SimPe.Data.LocalizedBuildSubSort(Data.BuildFunctionSubSort.Openings_TallDoor));
            if (Helper.XmlRegistry.HiddenMode)
                cbBuildSort.Items.Add(new SimPe.Data.LocalizedBuildSubSort(Data.BuildFunctionSubSort.unknown));

            // Populate cbsort with ObjFunctionSubSort enum values
            foreach (Data.ObjFunctionSubSort val in Enum.GetValues(typeof(Data.ObjFunctionSubSort)))
                cbsort.Items.Add(val);

            // Wire events
            cbtype.SelectionChanged += ChangeType;
            cbBuildSort.SelectionChanged += cbBuildSort_SelectedIndexChanged;
            cbsort.SelectionChanged += cbsort_SelectedIndexChanged;
            tbflname.TextChanged += SetFlName;
            tbguid.TextChanged += SetGuide;
            tbproxguid.TextChanged += SetGuid;
            tborgguid.TextChanged += SetGuid;
            tbdiag.TextChanged += SetGuid;
            tbgrid.TextChanged += SetGuid;
            tbPrice.TextChanged += tbPrice_TextChanged;
            llgetGUID.Click += GetGuid;
            lladdgooee.Click += lladdgooee_LinkClicked;
            btnUpdateMMAT.Click += btnUpdateMMAT_Click;
            btnCommit.Click += OnCommit;
            tc.SelectionChanged += CangedTab;
            rbhex.IsCheckedChanged += DigitChanged;
            rbdec.IsCheckedChanged += DigitChanged;
            rbbin.IsCheckedChanged += DigitChanged;

            // Room sort checkboxes
            cbbathroom.IsCheckedChanged += SetRoomFlags;
            cbbedroom.IsCheckedChanged += SetRoomFlags;
            cbdinigroom.IsCheckedChanged += SetRoomFlags;
            cbkitchen.IsCheckedChanged += SetRoomFlags;
            cblivingroom.IsCheckedChanged += SetRoomFlags;
            cbmisc.IsCheckedChanged += SetRoomFlags;
            cboutside.IsCheckedChanged += SetRoomFlags;
            cbstudy.IsCheckedChanged += SetRoomFlags;
            cbkids.IsCheckedChanged += SetRoomFlags;

            // Comm sort checkboxes
            cbcDine.IsCheckedChanged += SetCommFlags;
            cbcShop.IsCheckedChanged += SetCommFlags;
            cbcOuts.IsCheckedChanged += SetCommFlags;
            cbcStreet.IsCheckedChanged += SetCommFlags;
            cbcMisc.IsCheckedChanged += SetCommFlags;

            // Function sort checkboxes
            cbappliances.IsCheckedChanged += SetFunctionFlags;
            cbdecorative.IsCheckedChanged += SetFunctionFlags;
            cbelectronics.IsCheckedChanged += SetFunctionFlags;
            cbgeneral.IsCheckedChanged += SetFunctionFlags;
            cblightning.IsCheckedChanged += SetFunctionFlags;
            cbplumbing.IsCheckedChanged += SetFunctionFlags;
            cbseating.IsCheckedChanged += SetFunctionFlags;
            cbsurfaces.IsCheckedChanged += SetFunctionFlags;
            cbhobby.IsCheckedChanged += SetFunctionFlags;
            cbaspiration.IsCheckedChanged += SetFunctionFlags;
            cbcareer.IsCheckedChanged += SetFunctionFlags;

            // Expansion checkboxes
            cbBase.IsCheckedChanged += SetExpansionFlags;
            cbUniversity.IsCheckedChanged += SetExpansionFlags;
            cbNightlife.IsCheckedChanged += SetExpansionFlags;
            cbBusiness.IsCheckedChanged += SetExpansionFlags;
            cbFamilyFun.IsCheckedChanged += SetExpansionFlags;
            cbGlamour.IsCheckedChanged += SetExpansionFlags;
            cbPets.IsCheckedChanged += SetExpansionFlags;
            cbSeasons.IsCheckedChanged += SetExpansionFlags;
            cbCelebrations.IsCheckedChanged += SetExpansionFlags;
            cbFashion.IsCheckedChanged += SetExpansionFlags;
            cbBonVoyage.IsCheckedChanged += SetExpansionFlags;
            cbTeenStyle.IsCheckedChanged += SetExpansionFlags;
            cbExtras.IsCheckedChanged += SetExpansionFlags;
            cbFreeTime.IsCheckedChanged += SetExpansionFlags;
            cbKitchens.IsCheckedChanged += SetExpansionFlags;
            cbIkeaHome.IsCheckedChanged += SetExpansionFlags;
            cbApartments.IsCheckedChanged += SetExpansionFlags;
            cbMansion.IsCheckedChanged += SetExpansionFlags;
            cbStoreEd.IsCheckedChanged += SetExpansionFlags;

            // Build tpcatalogsort tab content
            // groupBox1: Room sort + price
            var groupBox1 = new StackPanel { Orientation = Orientation.Vertical };
            groupBox1.Children.Add(new TextBlock { Text = "Room Sort" });
            var roomRow1 = new StackPanel { Orientation = Orientation.Horizontal };
            roomRow1.Children.Add(cbbathroom); roomRow1.Children.Add(cbmisc);
            var roomRow2 = new StackPanel { Orientation = Orientation.Horizontal };
            roomRow2.Children.Add(cbbedroom); roomRow2.Children.Add(cboutside);
            var roomRow3 = new StackPanel { Orientation = Orientation.Horizontal };
            roomRow3.Children.Add(cbdinigroom); roomRow3.Children.Add(cbkids);
            var roomRow4 = new StackPanel { Orientation = Orientation.Horizontal };
            roomRow4.Children.Add(cbkitchen); roomRow4.Children.Add(cbstudy);
            groupBox1.Children.Add(roomRow1);
            groupBox1.Children.Add(roomRow2);
            groupBox1.Children.Add(roomRow3);
            groupBox1.Children.Add(roomRow4);
            groupBox1.Children.Add(cblivingroom);
            var priceRow = new StackPanel { Orientation = Orientation.Horizontal };
            priceRow.Children.Add(lbprise); priceRow.Children.Add(tbPrice);
            groupBox1.Children.Add(priceRow);

            // groupBox2: Function sort
            var groupBox2 = new StackPanel { Orientation = Orientation.Vertical };
            groupBox2.Children.Add(new TextBlock { Text = "Function Sort" });
            var funcRow1 = new StackPanel { Orientation = Orientation.Horizontal };
            funcRow1.Children.Add(cbappliances); funcRow1.Children.Add(cbplumbing);
            var funcRow2 = new StackPanel { Orientation = Orientation.Horizontal };
            funcRow2.Children.Add(cbdecorative); funcRow2.Children.Add(cbseating);
            var funcRow3 = new StackPanel { Orientation = Orientation.Horizontal };
            funcRow3.Children.Add(cbelectronics); funcRow3.Children.Add(cbsurfaces);
            var funcRow4 = new StackPanel { Orientation = Orientation.Horizontal };
            funcRow4.Children.Add(cbgeneral); funcRow4.Children.Add(cbhobby);
            var funcRow5 = new StackPanel { Orientation = Orientation.Horizontal };
            funcRow5.Children.Add(cblightning); funcRow5.Children.Add(cbaspiration);
            groupBox2.Children.Add(funcRow1);
            groupBox2.Children.Add(funcRow2);
            groupBox2.Children.Add(funcRow3);
            groupBox2.Children.Add(funcRow4);
            groupBox2.Children.Add(funcRow5);
            groupBox2.Children.Add(cbcareer);
            var sortRow = new StackPanel { Orientation = Orientation.Horizontal };
            sortRow.Children.Add(label1); sortRow.Children.Add(cbsort);
            groupBox2.Children.Add(sortRow);
            var buildSortRow = new StackPanel { Orientation = Orientation.Horizontal };
            buildSortRow.Children.Add(label5); buildSortRow.Children.Add(cbBuildSort);
            groupBox2.Children.Add(buildSortRow);

            // taskBox1: Comm sort
            var taskBox1 = new StackPanel { Orientation = Orientation.Vertical };
            taskBox1.Children.Add(new TextBlock { Text = "Community Sort" });
            taskBox1.Children.Add(cbcDine);
            taskBox1.Children.Add(cbcShop);
            taskBox1.Children.Add(cbcOuts);
            taskBox1.Children.Add(cbcStreet);
            taskBox1.Children.Add(cbcMisc);

            var catalogueRow = new StackPanel { Orientation = Orientation.Horizontal };
            catalogueRow.Children.Add(groupBox1);
            catalogueRow.Children.Add(groupBox2);
            catalogueRow.Children.Add(taskBox1);
            tpcatalogsort.Content = catalogueRow;

            // Build tpreqeps tab content
            var epCol1 = new StackPanel { Orientation = Orientation.Vertical };
            epCol1.Children.Add(cbBase);
            epCol1.Children.Add(cbUniversity);
            epCol1.Children.Add(cbNightlife);
            epCol1.Children.Add(cbBusiness);
            epCol1.Children.Add(cbFamilyFun);
            epCol1.Children.Add(cbGlamour);
            epCol1.Children.Add(cbPets);

            var epCol2 = new StackPanel { Orientation = Orientation.Vertical };
            epCol2.Children.Add(cbSeasons);
            epCol2.Children.Add(cbCelebrations);
            epCol2.Children.Add(cbFashion);
            epCol2.Children.Add(cbBonVoyage);
            epCol2.Children.Add(cbTeenStyle);
            epCol2.Children.Add(cbExtras);
            epCol2.Children.Add(cbFreeTime);

            var epCol3 = new StackPanel { Orientation = Orientation.Vertical };
            epCol3.Children.Add(lbgamef2);
            epCol3.Children.Add(cbKitchens);
            epCol3.Children.Add(cbIkeaHome);
            epCol3.Children.Add(cbApartments);
            epCol3.Children.Add(cbMansion);
            epCol3.Children.Add(cbStoreEd);

            var epRow = new StackPanel { Orientation = Orientation.Horizontal };
            epRow.Children.Add(epCol1);
            epRow.Children.Add(epCol2);
            epRow.Children.Add(epCol3);

            tbreqeps.Children.Add(lbepnote);
            tbreqeps.Children.Add(epRow);
            tpreqeps.Content = tbreqeps;

            // Build tpraw tab content
            var digitRow = new StackPanel { Orientation = Orientation.Horizontal };
            digitRow.Children.Add(rbbin);
            digitRow.Children.Add(rbdec);
            digitRow.Children.Add(rbhex);
            var rawGrid = new Grid();
            rawGrid.RowDefinitions.Add(new RowDefinition(GridLength.Auto));
            rawGrid.RowDefinitions.Add(new RowDefinition(GridLength.Star));
            Grid.SetRow(digitRow, 0); rawGrid.Children.Add(digitRow);
            Grid.SetRow(pg, 1); rawGrid.Children.Add(pg);
            tpraw.Content = rawGrid;

            // Add tabs to TabControl
            tc.Items.Add(tpcatalogsort);
            tc.Items.Add(tpreqeps);
            tc.Items.Add(tpraw);

            if (!UserVerification.HaveUserId || SimPe.PathProvider.Global.EPInstalled <= 1)
                tc.Items.Remove(tpreqeps);

            // Build the header panel (panel6) with commit button
            panel6.Children.Add(btnCommit);

            // Build the main pnobjd panel layout
            var mainGrid = new Grid();
            mainGrid.RowDefinitions.Add(new RowDefinition(GridLength.Auto)); // row 0: header panel
            mainGrid.RowDefinitions.Add(new RowDefinition(GridLength.Auto)); // row 1: top fields
            mainGrid.RowDefinitions.Add(new RowDefinition(GridLength.Auto)); // row 2: status/buttons
            mainGrid.RowDefinitions.Add(new RowDefinition(GridLength.Star)); // row 3: tab control
            mainGrid.RowDefinitions.Add(new RowDefinition(GridLength.Auto)); // row 4: bottom guid fields

            Grid.SetRow(panel6, 0); mainGrid.Children.Add(panel6);

            // Row 1: filename + type
            var row1 = new StackPanel { Orientation = Orientation.Horizontal };
            row1.Children.Add(label9);
            row1.Children.Add(tbflname);
            row1.Children.Add(label65);
            row1.Children.Add(cbtype);
            row1.Children.Add(tbtype);
            Grid.SetRow(row1, 1); mainGrid.Children.Add(row1);

            // Row 2: GUID + status
            var row2 = new StackPanel { Orientation = Orientation.Horizontal };
            row2.Children.Add(label8);
            row2.Children.Add(tbguid);
            row2.Children.Add(llgetGUID);
            row2.Children.Add(lbIsOk);
            row2.Children.Add(label2);
            row2.Children.Add(btnUpdateMMAT);
            row2.Children.Add(cball);
            Grid.SetRow(row2, 2); mainGrid.Children.Add(row2);

            // Row 3: TabControl
            Grid.SetRow(tc, 3); mainGrid.Children.Add(tc);

            // Row 4: extra GUIDs
            var row4 = new StackPanel { Orientation = Orientation.Vertical };
            var origRow = new StackPanel { Orientation = Orientation.Horizontal };
            origRow.Children.Add(label63); origRow.Children.Add(tborgguid);
            var fallbackRow = new StackPanel { Orientation = Orientation.Horizontal };
            fallbackRow.Children.Add(label97); fallbackRow.Children.Add(tbproxguid);
            var diagRow = new StackPanel { Orientation = Orientation.Horizontal };
            diagRow.Children.Add(label3); diagRow.Children.Add(tbdiag);
            var gridRow = new StackPanel { Orientation = Orientation.Horizontal };
            gridRow.Children.Add(label4); gridRow.Children.Add(tbgrid);
            gridRow.Children.Add(lladdgooee);
            row4.Children.Add(origRow);
            row4.Children.Add(fallbackRow);
            row4.Children.Add(diagRow);
            row4.Children.Add(gridRow);
            Grid.SetRow(row4, 4); mainGrid.Children.Add(row4);

            pnobjd.Children.Add(mainGrid);
            Content = pnobjd;
        }

        public void Dispose() { wrapper = null; }

        #region ExtObjdForm
        internal ExtObjd wrapper = null;
        internal uint initialguid;
        Ambertation.PropertyObjectBuilderExt pob;
        bool propchanged;
        // Track the selected sort value since we replaced EnumComboBox
        private Data.ObjFunctionSubSort _selectedSortValue;

        void ShowData()
        {
            propchanged = false;

            Hashtable ht = new Hashtable();
            for (int i = 0; i < wrapper.Data.Length; i++)
            {
                Ambertation.PropertyDescription pf = ExtObjd.PropertyParser.GetDescriptor((ushort)i);
                if (pf == null)
                    pf = new Ambertation.PropertyDescription("Unknown", null, wrapper.Data[i]);
                else
                    pf.Property = wrapper.Data[i];

                ht[GetName(i)] = pf;
            }

            pob = new Ambertation.PropertyObjectBuilderExt(ht);

            // Render properties as text (no PropertyGrid in Avalonia)
            var sb = new System.Text.StringBuilder();
            foreach (DictionaryEntry entry in ht)
            {
                var pf = entry.Value as Ambertation.PropertyDescription;
                sb.AppendLine($"{entry.Key} = {(pf != null ? pf.Property?.ToString() : entry.Value?.ToString())}");
            }
            pg.Text = sb.ToString();
        }

        void UpdateData()
        {
            if (!propchanged) return;
            propchanged = false;

            try
            {
                Hashtable ht = pob.Properties;

                for (int i = 0; i < wrapper.Data.Length; i++)
                {
                    string name = GetName(i);
                    try
                    {
                        if (ht.Contains(name))
                        {
                            object o = ht[name];
                            if (o is SimPe.FlagBase)
                                wrapper.Data[i] = ((SimPe.FlagBase)ht[name]);
                            else
                                wrapper.Data[i] = Convert.ToInt16(ht[name]);
                        }
                    }
                    catch (Exception ex)
                    {
                        if (Helper.XmlRegistry.HiddenMode) Helper.ExceptionMessage("Error converting " + name, ex);
                    }
                }

                wrapper.Changed = true;
                wrapper.UpdateFlags();
                wrapper.RefreshUI();
            }
            catch (Exception ex)
            {
                Helper.ExceptionMessage("", ex);
            }
        }

        private static Dictionary<int, string> names = null;
        private string GetName(int i)
        {
            string name = null;
            if (names == null) readPJSEGlobalStringObjDef();
            if (names == null) readGLUAObjDef();
            if (names == null || names[i] == null)
            {
                Ambertation.PropertyDescription pf = ExtObjd.PropertyParser.GetDescriptor((ushort)i);
                name = pf == null ? null : pf.Description;
            }
            else
                name = names[i];
            return "0x" + Helper.HexString((ushort)i) + ((name != null) ? ": " + name : "") + " ";
        }

        private static void readGLUAObjDef()
        {
            names = null;
            string objDefGLUAFile = System.IO.Path.Combine(
                SimPe.PathProvider.Global.Latest.InstallFolder,
                "TSData\\Res\\ObjectScripts\\ObjectScripts.package");
            if (!System.IO.File.Exists(objDefGLUAFile)) return;
            IPackageFile glua = SimPe.Packages.File.LoadFromFile(objDefGLUAFile);
            if (glua == null) return;
            IPackedFileDescriptor objDefPFD = glua.FindFile(Data.MetaData.GLUA, 0x49fa1f15, 0xffffffff, 0xff89f911);
            if (objDefPFD == null) return;
            SimPe.PackedFiles.Wrapper.ObjLua objDef = new SimPe.PackedFiles.Wrapper.ObjLua();
            objDef.ProcessData(objDefPFD, glua);

            List<ObjLuaConstant> loc = new List<ObjLuaConstant>((ObjLuaConstant[])objDef.Root.Constants.ToArray(typeof(ObjLuaConstant)));
            if (loc[0].String != "ObjDef") return;
            loc.RemoveAt(0);

            names = new Dictionary<int, string>();
            bool started = false;
            while (loc.Count > 0)
            {
                string value = loc[0].String;
                loc.RemoveAt(0);
                int key = Convert.ToInt32(loc[0].Value);
                loc.RemoveAt(0);
                if (started)
                    names[key] = value;
                else if (key == 0)
                {
                    started = true;
                    names[key] = value;
                }
            }
        }

        private static void readPJSEGlobalStringObjDef()
        {
            names = null;
            string pjseGlobalStringFile = System.IO.Path.Combine(SimPe.Helper.SimPePluginPath, "pjse.coder.plugin\\GlobalStrings.package");
            if (!System.IO.File.Exists(pjseGlobalStringFile)) return;
            IPackageFile gs = SimPe.Packages.File.LoadFromFile(pjseGlobalStringFile);
            if (gs == null) return;
            IPackedFileDescriptor objDefPFD = gs.FindFile(0x53545223, 0, 0xfeedf00d, 0xcc);
            if (objDefPFD == null) return;
            Str objDef = new SimPe.PackedFiles.Wrapper.Str();
            objDef.ProcessData(objDefPFD, gs);
            if (objDef.LanguageItems(1) == null) return;

            List<StrToken> lST = new List<StrToken>((StrToken[])objDef.LanguageItems(1).ToArray(typeof(StrToken)));
            names = new Dictionary<int, string>();
            for (int i = 0; i < lST.Count; i++)
                names[i] = lST[i].Title;
        }

        internal void SetFunctionCb(Wrapper.ExtObjd objd)
        {
            this.cbappliances.IsChecked = objd.FunctionSort.InAppliances;
            this.cbdecorative.IsChecked = objd.FunctionSort.InDecorative;
            this.cbelectronics.IsChecked = objd.FunctionSort.InElectronics;
            this.cbgeneral.IsChecked = objd.FunctionSort.InGeneral;
            this.cblightning.IsChecked = objd.FunctionSort.InLighting;
            this.cbplumbing.IsChecked = objd.FunctionSort.InPlumbing;
            this.cbseating.IsChecked = objd.FunctionSort.InSeating;
            this.cbsurfaces.IsChecked = objd.FunctionSort.InSurfaces;
            this.cbhobby.IsChecked = objd.FunctionSort.InHobbies;
            this.cbaspiration.IsChecked = objd.FunctionSort.InAspirationRewards;
            this.cbcareer.IsChecked = objd.FunctionSort.InCareerRewards;
        }

        internal void SetExpansionsCb(Wrapper.ExtObjd objd)
        {
            this.cbBase.IsChecked = objd.EpRequired1.Basegame;
            this.cbUniversity.IsChecked = objd.EpRequired1.University;
            this.cbNightlife.IsChecked = objd.EpRequired1.Nightlife;
            this.cbBusiness.IsChecked = objd.EpRequired1.Business;
            this.cbFamilyFun.IsChecked = objd.EpRequired1.FamilyFun;
            this.cbGlamour.IsChecked = objd.EpRequired1.GlamourLife;
            this.cbSeasons.IsChecked = objd.EpRequired1.Seasons;
            this.cbCelebrations.IsChecked = objd.EpRequired1.Celebration;
            this.cbFashion.IsChecked = objd.EpRequired1.Fashion;
            this.cbBonVoyage.IsChecked = objd.EpRequired1.BonVoyage;
            this.cbTeenStyle.IsChecked = objd.EpRequired1.TeenStyle;
            this.cbExtras.IsChecked = objd.EpRequired1.StoreEdition_old;
            this.cbFreeTime.IsChecked = objd.EpRequired1.Freetime;
            this.cbKitchens.IsChecked = objd.EpRequired1.KitchenBath;
            this.cbIkeaHome.IsChecked = objd.EpRequired1.IkeaHome;
            this.cbApartments.IsChecked = objd.EpRequired2.ApartmentLife;
            this.cbMansion.IsChecked = objd.EpRequired2.MansionGarden;
            this.cbStoreEd.IsChecked = objd.EpRequired2.StoreEdition;
        }

        static string subKey = "ExtObdjForm";
        private int InitialTab
        {
            get
            {
                XmlRegistryKey rkf = Helper.XmlRegistry.RegistryKey.CreateSubKey(subKey);
                object o = rkf.GetValue("initialTab", 0);
                return Convert.ToInt16(o);
            }
            set
            {
                XmlRegistryKey rkf = Helper.XmlRegistry.RegistryKey.CreateSubKey(subKey);
                rkf.SetValue("initialTab", value);
            }
        }
        #endregion

        #region IPackedFileUI Member

        public Avalonia.Controls.Control GUIHandle => this.pnobjd;

        public void UpdateGUI(SimPe.Interfaces.Plugin.IFileWrapper wrapper)
        {
            Wrapper.ExtObjd objd = (Wrapper.ExtObjd)wrapper;
            this.wrapper = objd;
            this.initialguid = objd.Guid;
            this.Tag = true;

            try
            {
                if (objd.Ok != Wrapper.ObjdHealth.Ok)
                {
                    this.lbIsOk.Text = "Please commit! (" + objd.Ok.ToString() + ")";
                    this.lbIsOk.IsVisible = true;
                }
                else
                {
                    this.lbIsOk.Text = "Please commit!";
                    this.lbIsOk.IsVisible = false;
                }

                pg.Text = string.Empty;
                this.tc.SelectedIndex = InitialTab;
                this.cbtype.SelectedIndex = 0;
                for (int i = 0; i < this.cbtype.ItemCount; i++)
                {
                    Data.ObjectTypes ot = (Data.ObjectTypes)this.cbtype.Items[i];
                    if (ot == objd.Type)
                    {
                        this.cbtype.SelectedIndex = i;
                        break;
                    }
                }

                this.tbtype.Text = "0x" + Helper.HexString((ushort)(objd.Type));
                this.tbguid.Text = "0x" + Helper.HexString(objd.Guid);
                this.tbproxguid.Text = "0x" + Helper.HexString(objd.ProxyGuid);
                this.tborgguid.Text = "0x" + Helper.HexString(objd.OriginalGuid);
                this.tbdiag.Text = "0x" + Helper.HexString(objd.DiagonalGuid);
                this.tbgrid.Text = "0x" + Helper.HexString(objd.GridAlignedGuid);
                this.tbflname.Text = objd.FileName;

                this.cbbathroom.IsChecked = objd.RoomSort.InBathroom;
                this.cbbedroom.IsChecked = objd.RoomSort.InBedroom;
                this.cbdinigroom.IsChecked = objd.RoomSort.InDiningRoom;
                this.cbkitchen.IsChecked = objd.RoomSort.InKitchen;
                this.cblivingroom.IsChecked = objd.RoomSort.InLivingRoom;
                this.cbmisc.IsChecked = objd.RoomSort.InMisc;
                this.cboutside.IsChecked = objd.RoomSort.InOutside;
                this.cbstudy.IsChecked = objd.RoomSort.InStudy;
                this.cbkids.IsChecked = objd.RoomSort.InKids;

                this.cbcDine.IsChecked = objd.CommSort.InDining;
                this.cbcShop.IsChecked = objd.CommSort.InShopping;
                this.cbcOuts.IsChecked = objd.CommSort.InOutdoors;
                this.cbcStreet.IsChecked = objd.CommSort.InStreet;
                this.cbcMisc.IsChecked = objd.CommSort.InMiscel;

                tbPrice.Text = "\u00a7" + Convert.ToString(objd.Price);

                this.tbreqeps.IsEnabled = (objd.Version > 0x008b);
                this.SetExpansionsCb(objd);
                this.SetFunctionCb(objd);

                // Set cbsort selection by matching enum value
                _selectedSortValue = objd.FunctionSubSort;
                for (int i = 0; i < cbsort.ItemCount; i++)
                {
                    if (cbsort.Items[i] is Data.ObjFunctionSubSort v && v == objd.FunctionSubSort)
                    {
                        cbsort.SelectedIndex = i;
                        break;
                    }
                }

                this.cbBuildSort.SelectedIndex = 0;
                if (objd.BuildType.Value != 0)
                {
                    if (Helper.XmlRegistry.HiddenMode) this.cbBuildSort.SelectedIndex = 19;
                    for (int i = 0; i < this.cbBuildSort.ItemCount; i++)
                    {
                        object o = this.cbBuildSort.Items[i];
                        Data.BuildFunctionSubSort at;
                        if (o.GetType() == typeof(SimPe.Data.Alias)) at = (Data.LocalizedBuildSubSort)((uint)((SimPe.Data.Alias)o).Id);
                        else at = (Data.LocalizedBuildSubSort)o;
                        if (at == objd.BuildSubSort)
                        {
                            this.cbBuildSort.SelectedIndex = i;
                            break;
                        }
                    }
                }

                this.llgetGUID.IsVisible = (
                    UserVerification.HaveUserId &&
                    objd.Type != SimPe.Data.ObjectTypes.Person &&
                    objd.Type != SimPe.Data.ObjectTypes.UnlinkedSim
                );
                this.lladdgooee.IsVisible = false;
            }
            finally
            {
                this.Tag = null;
            }
        }

        #endregion

        private void ChangeType(object sender, SelectionChangedEventArgs e)
        {
            if (this.Tag != null) return;
            this.Tag = true;
            try
            {
                if (cbtype.SelectedIndex < 0) return;
                Data.ObjectTypes ot = (Data.ObjectTypes)cbtype.Items[cbtype.SelectedIndex];
                tbtype.Text = "0x" + Helper.HexString((ushort)ot);

                wrapper.Type = ot;
                wrapper.Changed = true;
                this.btnUpdateMMAT.IsVisible = this.label2.IsVisible = this.cball.IsVisible = this.lbIsOk.IsVisible = false;
                this.llgetGUID.IsVisible = (UserVerification.HaveUserId && wrapper.Type != SimPe.Data.ObjectTypes.Person && wrapper.Type != SimPe.Data.ObjectTypes.UnlinkedSim);
            }
            finally
            {
                this.Tag = null;
            }
        }

        private void OnCommit(object sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            this.lbIsOk.IsVisible = false;
            if (pg.Text?.Length > 0) UpdateData();
            wrapper.SynchronizeUserData();
        }

        private void SetRoomFlags(object sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            if (this.Tag != null) return;
            this.Tag = true;
            try
            {
                wrapper.RoomSort.InBathroom = cbbathroom.IsChecked == true;
                wrapper.RoomSort.InBedroom = cbbedroom.IsChecked == true;
                wrapper.RoomSort.InDiningRoom = cbdinigroom.IsChecked == true;
                wrapper.RoomSort.InKitchen = cbkitchen.IsChecked == true;
                wrapper.RoomSort.InLivingRoom = cblivingroom.IsChecked == true;
                wrapper.RoomSort.InMisc = cbmisc.IsChecked == true;
                wrapper.RoomSort.InOutside = cboutside.IsChecked == true;
                wrapper.RoomSort.InStudy = cbstudy.IsChecked == true;
                wrapper.RoomSort.InKids = cbkids.IsChecked == true;
                wrapper.Changed = true;
            }
            finally
            {
                this.Tag = null;
            }
        }

        private void SetCommFlags(object sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            if (this.Tag != null) return;
            this.Tag = true;
            try
            {
                wrapper.CommSort.InDining = cbcDine.IsChecked == true;
                wrapper.CommSort.InShopping = cbcShop.IsChecked == true;
                wrapper.CommSort.InOutdoors = cbcOuts.IsChecked == true;
                wrapper.CommSort.InStreet = cbcStreet.IsChecked == true;
                wrapper.CommSort.InMiscel = cbcMisc.IsChecked == true;
                wrapper.Changed = true;
            }
            finally
            {
                this.Tag = null;
            }
        }

        private void SetFunctionFlags(object sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            if (this.Tag != null) return;
            this.Tag = true;
            try
            {
                wrapper.FunctionSort.InAppliances = this.cbappliances.IsChecked == true;
                wrapper.FunctionSort.InDecorative = this.cbdecorative.IsChecked == true;
                wrapper.FunctionSort.InElectronics = this.cbelectronics.IsChecked == true;
                wrapper.FunctionSort.InGeneral = this.cbgeneral.IsChecked == true;
                wrapper.FunctionSort.InLighting = this.cblightning.IsChecked == true;
                wrapper.FunctionSort.InPlumbing = this.cbplumbing.IsChecked == true;
                wrapper.FunctionSort.InSeating = this.cbseating.IsChecked == true;
                wrapper.FunctionSort.InSurfaces = this.cbsurfaces.IsChecked == true;
                wrapper.FunctionSort.InHobbies = this.cbhobby.IsChecked == true;
                wrapper.FunctionSort.InAspirationRewards = this.cbaspiration.IsChecked == true;
                wrapper.FunctionSort.InCareerRewards = this.cbcareer.IsChecked == true;
                wrapper.Changed = true;
            }
            finally
            {
                this.Tag = null;
            }
        }

        private void SetExpansionFlags(object sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            if (this.Tag != null) return;
            this.Tag = true;
            try
            {
                if (cbBase.IsChecked == true)
                {
                    cbUniversity.IsChecked = cbNightlife.IsChecked = cbBusiness.IsChecked = cbFamilyFun.IsChecked = cbGlamour.IsChecked = cbSeasons.IsChecked = false;
                    cbCelebrations.IsChecked = cbFashion.IsChecked = cbBonVoyage.IsChecked = cbTeenStyle.IsChecked = cbExtras.IsChecked = cbFreeTime.IsChecked = false;
                    cbPets.IsChecked = cbKitchens.IsChecked = cbIkeaHome.IsChecked = cbApartments.IsChecked = cbMansion.IsChecked = cbStoreEd.IsChecked = false;
                }

                wrapper.EpRequired1.Basegame = cbBase.IsChecked == true;
                wrapper.EpRequired1.University = cbUniversity.IsChecked == true;
                wrapper.EpRequired1.Nightlife = cbNightlife.IsChecked == true;
                wrapper.EpRequired1.Business = cbBusiness.IsChecked == true;
                wrapper.EpRequired1.FamilyFun = cbFamilyFun.IsChecked == true;
                wrapper.EpRequired1.GlamourLife = cbGlamour.IsChecked == true;
                wrapper.EpRequired1.Pets = cbPets.IsChecked == true;
                wrapper.EpRequired1.Seasons = cbSeasons.IsChecked == true;
                wrapper.EpRequired1.Celebration = cbCelebrations.IsChecked == true;
                wrapper.EpRequired1.Fashion = cbFashion.IsChecked == true;
                wrapper.EpRequired1.BonVoyage = cbBonVoyage.IsChecked == true;
                wrapper.EpRequired1.TeenStyle = cbTeenStyle.IsChecked == true;
                wrapper.EpRequired1.StoreEdition_old = cbExtras.IsChecked == true;
                wrapper.EpRequired1.Freetime = cbFreeTime.IsChecked == true;
                wrapper.EpRequired1.KitchenBath = cbKitchens.IsChecked == true;
                wrapper.EpRequired1.IkeaHome = cbIkeaHome.IsChecked == true;
                wrapper.EpRequired2.ApartmentLife = cbApartments.IsChecked == true;
                wrapper.EpRequired2.MansionGarden = cbMansion.IsChecked == true;
                wrapper.EpRequired2.StoreEdition = cbStoreEd.IsChecked == true;

                wrapper.Changed = true;
            }
            finally
            {
                this.Tag = null;
            }
        }

        private void SetGuide(object sender, Avalonia.Controls.TextChangedEventArgs e)
        {
            if (this.Tag != null) return;
            this.Tag = true;
            try
            {
                wrapper.Guid = Convert.ToUInt32(tbguid.Text, 16);
                wrapper.Changed = true;
            }
            catch (Exception) { }
            finally
            {
                if (wrapper.Type != SimPe.Data.ObjectTypes.Person && wrapper.Type != SimPe.Data.ObjectTypes.UnlinkedSim)
                    this.btnUpdateMMAT.IsVisible = this.label2.IsVisible = this.cball.IsVisible = this.lbIsOk.IsVisible = true;
                this.Tag = null;
            }
        }

        private void SetGuid(object sender, Avalonia.Controls.TextChangedEventArgs e)
        {
            if (this.Tag != null) return;
            this.Tag = true;
            try
            {
                wrapper.ProxyGuid = Convert.ToUInt32(this.tbproxguid.Text, 16);
                wrapper.OriginalGuid = Convert.ToUInt32(this.tborgguid.Text, 16);
                wrapper.DiagonalGuid = Convert.ToUInt32(this.tbdiag.Text, 16);
                wrapper.GridAlignedGuid = Convert.ToUInt32(this.tbgrid.Text, 16);
                wrapper.Changed = true;
            }
            catch (Exception) { }
            finally
            {
                this.lbIsOk.IsVisible = true;
                this.Tag = null;
            }
        }

        private void GetGuid(object sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            uint gooy = wrapper.createguid;
            if (gooy != 0)
            {
                this.tbguid.Text = "0x" + Helper.HexString(gooy);
                llgetGUID.IsEnabled = true;
            }
            else
                llgetGUID.IsEnabled = false;
        }

        private void lladdgooee_LinkClicked(object sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            // Subhoods.GuidAdd removed — not available in this build.
            lladdgooee.IsEnabled = false;
        }

        private void btnUpdateMMAT_Click(object sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            if ((wrapper.Guid != initialguid) || (cball.IsChecked == true))
            {
                SimPe.Plugin.FixGuid fg = new SimPe.Plugin.FixGuid(wrapper.Package);
                if (cball.IsChecked == true)
                {
                    fg.FixGuids(wrapper.Guid);
                }
                else
                {
                    ArrayList al = new ArrayList();
                    SimPe.Plugin.GuidSet gs = new SimPe.Plugin.GuidSet();
                    gs.oldguid = initialguid;
                    gs.guid = wrapper.Guid;
                    al.Add(gs);
                    fg.FixGuids(al);
                }
                initialguid = wrapper.Guid;
            }
            this.lbIsOk.IsVisible = false;
            wrapper.SynchronizeUserData();
        }

        private void CangedTab(object sender, SelectionChangedEventArgs e)
        {
            InitialTab = tc.SelectedIndex;
            // tpraw is at index 2 (or 1 if tpreqeps was removed)
            bool isRawTab = tc.SelectedItem == tpraw;
            if (isRawTab)
            {
                rbhex.IsChecked = (Ambertation.BaseChangeableNumber.DigitBase == 16);
                rbbin.IsChecked = (Ambertation.BaseChangeableNumber.DigitBase == 2);
                rbdec.IsChecked = (!rbhex.IsChecked == true && !rbbin.IsChecked == true);
                ShowData();
            }
            else
            {
                if (pg.Text?.Length > 0) UpdateData();
                pg.Text = string.Empty;
            }
        }

        private void PropChanged()
        {
            propchanged = true;
        }

        private void SetFlName(object sender, Avalonia.Controls.TextChangedEventArgs e)
        {
            if (this.Tag != null) return;
            wrapper.FileName = tbflname.Text;
            wrapper.Changed = true;
        }

        private void DigitChanged(object sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            if (rbhex.IsChecked == true) Ambertation.BaseChangeableNumber.DigitBase = 16;
            else if (rbbin.IsChecked == true) Ambertation.BaseChangeableNumber.DigitBase = 2;
            else Ambertation.BaseChangeableNumber.DigitBase = 10;

            // Re-render the raw data with new base
            if (pob != null && wrapper != null) ShowData();
        }

        private void cbsort_SelectedIndexChanged(object sender, SelectionChangedEventArgs e)
        {
            if (Tag != null) return;
            this.Tag = true;
            if (cbsort.SelectedIndex >= 0 && cbsort.Items[cbsort.SelectedIndex] is Data.ObjFunctionSubSort val)
            {
                _selectedSortValue = val;
                wrapper.FunctionSubSort = val;
                wrapper.Changed = true;
                this.SetFunctionCb(wrapper);
            }
            this.Tag = null;
        }

        private void cbBuildSort_SelectedIndexChanged(object sender, SelectionChangedEventArgs e)
        {
            if (Tag != null) return;
            this.Tag = true;

            if (cbBuildSort.SelectedIndex < 0) { this.Tag = null; return; }
            bool skippy = false;
            int idx = cbBuildSort.SelectedIndex;
            if (idx == 0)  { wrapper.BuildSubSort = 0; wrapper.Type = SimPe.Data.ObjectTypes.Normal; }
            else if (idx == 1)  { wrapper.BuildSubSort = Data.BuildFunctionSubSort.General_Columns; wrapper.Type = SimPe.Data.ObjectTypes.ArchitecturalSupport; }
            else if (idx == 2)  { wrapper.BuildSubSort = Data.BuildFunctionSubSort.General_Stairs; wrapper.Type = SimPe.Data.ObjectTypes.Stairs; }
            else if (idx == 3)  { wrapper.BuildSubSort = Data.BuildFunctionSubSort.General_Pool; skippy = true; }
            else if (idx == 4)  { wrapper.BuildSubSort = Data.BuildFunctionSubSort.General_TallColumns; wrapper.Type = SimPe.Data.ObjectTypes.ArchitecturalSupport; }
            else if (idx == 5)  { wrapper.BuildSubSort = Data.BuildFunctionSubSort.General_Arch; wrapper.Type = SimPe.Data.ObjectTypes.ArchitecturalSupport; }
            else if (idx == 6)  { wrapper.BuildSubSort = Data.BuildFunctionSubSort.General_Driveway; skippy = true; }
            else if (idx == 7)  { wrapper.BuildSubSort = Data.BuildFunctionSubSort.General_Elevator; skippy = true; }
            else if (idx == 8)  { wrapper.BuildSubSort = Data.BuildFunctionSubSort.General_Architectural; wrapper.Type = SimPe.Data.ObjectTypes.Normal; }
            else if (idx == 9)  { wrapper.BuildSubSort = Data.BuildFunctionSubSort.Garden_Trees; wrapper.Type = SimPe.Data.ObjectTypes.Normal; }
            else if (idx == 10) { wrapper.BuildSubSort = Data.BuildFunctionSubSort.Garden_Shrubs; wrapper.Type = SimPe.Data.ObjectTypes.Normal; }
            else if (idx == 11) { wrapper.BuildSubSort = Data.BuildFunctionSubSort.Garden_Flowers; wrapper.Type = SimPe.Data.ObjectTypes.Normal; }
            else if (idx == 12) { wrapper.BuildSubSort = Data.BuildFunctionSubSort.Garden_Objects; wrapper.Type = SimPe.Data.ObjectTypes.Normal; }
            else if (idx == 13) { wrapper.BuildSubSort = Data.BuildFunctionSubSort.Openings_Door; wrapper.Type = SimPe.Data.ObjectTypes.Door; }
            else if (idx == 14) { wrapper.BuildSubSort = Data.BuildFunctionSubSort.Openings_TallWindow; wrapper.Type = SimPe.Data.ObjectTypes.Window; }
            else if (idx == 15) { wrapper.BuildSubSort = Data.BuildFunctionSubSort.Openings_Window; wrapper.Type = SimPe.Data.ObjectTypes.Window; }
            else if (idx == 16) { wrapper.BuildSubSort = Data.BuildFunctionSubSort.Openings_Gate; wrapper.Type = SimPe.Data.ObjectTypes.Door; }
            else if (idx == 17) { wrapper.BuildSubSort = Data.BuildFunctionSubSort.Openings_Arch; wrapper.Type = SimPe.Data.ObjectTypes.Door; }
            else if (idx == 18) { wrapper.BuildSubSort = Data.BuildFunctionSubSort.Openings_TallDoor; wrapper.Type = SimPe.Data.ObjectTypes.Door; }
            else if (idx == 19) skippy = true; // Unknown

            if (!skippy)
            {
                this.cbtype.SelectedIndex = 0;
                for (int i = 0; i < this.cbtype.ItemCount; i++)
                {
                    Data.ObjectTypes ot = (Data.ObjectTypes)this.cbtype.Items[i];
                    if (ot == wrapper.Type)
                    {
                        this.cbtype.SelectedIndex = i;
                        break;
                    }
                }
                this.tbtype.Text = "0x" + Helper.HexString((ushort)(wrapper.Type));
            }
            else
                this.cbtype.Focus();

            this.Tag = null;
            wrapper.Changed = true;
        }

        private void tbPrice_TextChanged(object sender, Avalonia.Controls.TextChangedEventArgs e)
        {
            if (this.Tag != null) return;
            try
            {
                string prise = this.tbPrice.Text;
                if (prise != null && prise.StartsWith("\u00a7")) prise = prise.Remove(0, 1);
                wrapper.Price = Convert.ToInt16(prise);
            }
            catch { }
        }
    }
}
