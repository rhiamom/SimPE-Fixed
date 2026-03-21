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
using System.Collections.Generic;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using SimPe.Interfaces.Plugin;
using SimPe.Interfaces;
using SimPe.PackedFiles.Wrapper.Supporting;
using SimPe.Data;
using SimPe;
using Avalonia.Controls;

namespace SimPe.PackedFiles.UserInterface
{
	/// <summary>
	/// Avalonia port of CommonSrel — shows relationship details for a single Sim relation.
	/// </summary>
	public class CommonSrel : Avalonia.Controls.UserControl
    {
        #region Form fields
        private StackPanel flowLayoutPanel2 = new StackPanel();
        private StackPanel flowLayoutPanel1 = new StackPanel();
        private TextBlock label91 = new TextBlock { Text = "Family type:" };
        private ComboBox cbfamtype = new ComboBox();
        private TextBox tbRel = new TextBox();
        private ProgressBar pbDay = new ProgressBar { Minimum = 0, Maximum = 200 };
        private ProgressBar pbLife = new ProgressBar { Minimum = 0, Maximum = 200 };
        private Grid tableLayoutPanel1 = new Grid();
        private CheckBox cblove = new CheckBox { Content = "Love" };
        private CheckBox cbcrush = new CheckBox { Content = "Crush" };
        private CheckBox cbengaged = new CheckBox { Content = "Engaged" };
        private CheckBox cbmarried = new CheckBox { Content = "Married" };
        private CheckBox cbbuddie = new CheckBox { Content = "Buddie" };
        private CheckBox cbfriend = new CheckBox { Content = "Friend" };
        private CheckBox cbsteady = new CheckBox { Content = "Steady" };
        private CheckBox cbenemy = new CheckBox { Content = "Enemy" };
        private CheckBox cbfamily = new CheckBox { Content = "Family" };
        private CheckBox cbbest = new CheckBox { Content = "Best" };
        private CheckBox cbBFF = new CheckBox { Content = "BFF" };
        private CheckBox cbsecret = new CheckBox { Content = "Secret" };
        private CheckBox cbplatonic = new CheckBox { Content = "Platonic" };
        #endregion

        public CommonSrel()
        {
            InitComboBox();

            ltcb = new List<CheckBox>(new CheckBox[] {
                cbcrush, cblove, cbengaged, cbmarried, cbfriend, cbbuddie, cbsteady, cbenemy,
                null, null, null, null, null, null, cbfamily, cbbest,
                cbBFF, null, null, null,
                null, null, null, null, null, null, null, null,
            });

            // Wire events
            cbfamtype.SelectionChanged += ChangedRelation;
            tbRel.TextChanged += ChangedRelationText;
            foreach (var cb in ltcb)
                if (cb != null) cb.IsCheckedChanged += ChangedState;
        }

        public void Dispose() { }

        SimPe.PackedFiles.Wrapper.ExtSrel srel;
        public SimPe.PackedFiles.Wrapper.ExtSrel Srel
        {
            get { return srel; }
            set
            {
                srel = value;
                UpdateContent();
            }
        }

        public event EventHandler ChangedContent;

        protected void InitComboBox()
        {
            this.cbfamtype.Items.Clear();
            this.cbfamtype.Items.Add(new LocalizedRelationshipTypes(Data.MetaData.RelationshipTypes.Unset_Unknown));
            this.cbfamtype.Items.Add(new LocalizedRelationshipTypes(Data.MetaData.RelationshipTypes.Aunt));
            this.cbfamtype.Items.Add(new LocalizedRelationshipTypes(Data.MetaData.RelationshipTypes.Child));
            this.cbfamtype.Items.Add(new LocalizedRelationshipTypes(Data.MetaData.RelationshipTypes.Cousin));
            this.cbfamtype.Items.Add(new LocalizedRelationshipTypes(Data.MetaData.RelationshipTypes.Grandchild));
            this.cbfamtype.Items.Add(new LocalizedRelationshipTypes(Data.MetaData.RelationshipTypes.Gradparent));
            this.cbfamtype.Items.Add(new LocalizedRelationshipTypes(Data.MetaData.RelationshipTypes.Nice_Nephew));
            this.cbfamtype.Items.Add(new LocalizedRelationshipTypes(Data.MetaData.RelationshipTypes.Parent));
            this.cbfamtype.Items.Add(new LocalizedRelationshipTypes(Data.MetaData.RelationshipTypes.Sibling));
            this.cbfamtype.Items.Add(new LocalizedRelationshipTypes(Data.MetaData.RelationshipTypes.Spouses));
            this.cbfamtype.Items.Add(new LocalizedRelationshipTypes(Data.MetaData.RelationshipTypes.Child_Inlaw));
            this.cbfamtype.Items.Add(new LocalizedRelationshipTypes(Data.MetaData.RelationshipTypes.Parent_Inlaw));
            this.cbfamtype.Items.Add(new LocalizedRelationshipTypes(Data.MetaData.RelationshipTypes.Sibling_Inlaw));
        }

        bool intern;
        List<CheckBox> ltcb;

        protected void UpdateContent()
        {
            if (Srel == null)
            {
                intern = true;
                this.pbDay.Value = this.pbLife.Value = 0;
                this.cbfamtype.SelectedIndex = 0;
                this.IsEnabled = false;
                intern = false;
                return;
            }

            this.IsEnabled = true;
            intern = true;

            // -100..100 -> 0..200
            this.pbDay.Value  = Srel.Shortterm + 100;
            this.pbLife.Value = Srel.Longterm  + 100;

            Boolset bs = Srel.RelationState.Value;
            for (int i = 0; i < bs.Length; i++)
                if (ltcb[i] != null) ltcb[i].IsChecked = bs[i];

            if (Srel.RelationState2 != null)
            {
                bs = Srel.RelationState2.Value;
                for (int i = 0; i < bs.Length; i++)
                {
                    int idx = i + 16;
                    if (idx >= ltcb.Count) break;

                    if (ltcb[idx] != null)
                    {
                        ltcb[idx].IsEnabled = true;
                        ltcb[idx].IsChecked = bs[i];
                    }
                }
            }
            else
            {
                for (int i = 0; i < bs.Length; i++)
                {
                    int idx = i + 16;
                    if (idx >= ltcb.Count) break;

                    if (ltcb[idx] != null)
                        ltcb[idx].IsEnabled = false;
                }
            }

            this.cbfamtype.SelectedIndex = 0;
            for (int i = 1; i < this.cbfamtype.ItemCount; i++)
                if (this.cbfamtype.Items[i] == new Data.LocalizedRelationshipTypes(srel.FamilyRelation))
                {
                    this.cbfamtype.SelectedIndex = i;
                    break;
                }

            this.tbRel.Text = "0x" + Helper.HexString((uint)srel.FamilyRelation);

            intern = false;

            if (ChangedContent != null) ChangedContent(this, EventArgs.Empty);
        }

        private void ChangedLife(object sender, EventArgs e)
        {
            int life = (int)pbLife.Value - 100;
            if (intern) return;
            Srel.Longterm = life;
            Srel.Changed = true;
        }

        private void ChangedDay(object sender, EventArgs e)
        {
            int day = (int)pbDay.Value - 100;
            if (intern) return;
            Srel.Shortterm = day;
            Srel.Changed = true;
        }

        private void ChangedRelation(object sender, SelectionChangedEventArgs e)
        {
            if (intern) return;
            if (this.cbfamtype.SelectedIndex >= 0)
                this.tbRel.Text = "0x" + Helper.HexString((uint)((Data.MetaData.RelationshipTypes)((Data.LocalizedRelationshipTypes)cbfamtype.SelectedItem)));
        }

        private void ChangedRelationText(object sender, Avalonia.Controls.TextChangedEventArgs e)
        {
            if (intern) return;
            Srel.FamilyRelation = (Data.LocalizedRelationshipTypes)Helper.StringToUInt32(this.tbRel.Text, (uint)Srel.FamilyRelation, 16);
            Srel.Changed = true;
        }

        private void ChangedState(object sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            if (intern) return;

            int i = ltcb.IndexOf((CheckBox)sender);
            if (i >= 0)
            {
                Boolset val;

                if (i < 16)
                {
                    val = Srel.RelationState.Value;
                }
                else
                {
                    if (Srel.RelationState2 == null) return;
                    val = Srel.RelationState2.Value;
                }

                val[i & 0x0f] = ((CheckBox)sender).IsChecked == true;

                if (i < 16) Srel.RelationState.Value = val;
                else Srel.RelationState2.Value = val;

                Srel.Changed = true;
            }

            if (Srel.RelationState2 != null)
            {
                this.cbsecret.IsEnabled = (cbmarried.IsChecked != true && cbengaged.IsChecked != true && cbsteady.IsChecked != true);
                this.cbplatonic.IsEnabled = (cbcrush.IsChecked != true && cblove.IsChecked != true);
            }
        }

        public SimPe.PackedFiles.Wrapper.ExtSDesc SourceSim
        {
            get
            {
                if (Srel == null) return null;
                return Srel.SourceSim;
            }
        }

        public SimPe.PackedFiles.Wrapper.ExtSDesc TargetSim
        {
            get
            {
                if (Srel == null) return null;
                return Srel.TargetSim;
            }
        }

        public string SourceSimName
        {
            get
            {
                if (Srel == null) return SimPe.Localization.GetString("Unknown");
                return Srel.SourceSimName;
            }
        }

        public string TargetSimName
        {
            get
            {
                if (Srel == null) return SimPe.Localization.GetString("Unknown");
                return Srel.TargetSimName;
            }
        }

        public System.Drawing.Image Image
        {
            get
            {
                if (Srel == null) return null;
                return Srel.Image;
            }
        }
    }
}
