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
using System.Collections;
using System.ComponentModel;
using SimPe.PackedFiles.Wrapper;
using Avalonia.Controls;

namespace SimPe.PackedFiles.UserInterface
{
    /// <summary>
    /// Avalonia port of ScorUI.
    /// </summary>
    public class ScorUI :
        SimPe.Windows.Forms.WrapperBaseControl, SimPe.Interfaces.Plugin.IPackedFileUI
    {
        private TextBlock label1 = new TextBlock { Text = "Unknown 1:" };
        private TextBox tbunk1 = new TextBox { IsReadOnly = true };
        private TextBox tbunk2 = new TextBox { IsReadOnly = true };
        private TextBlock label2 = new TextBlock { Text = "Unknown 2:" };
        private ListBox lb = new ListBox();
        private Panel pnContainer = new Panel();
        private Button btAdd = new Button { Content = "Add" };
        private Button btRem = new Button { Content = "Remove" };
        private ComboBox cbType = new ComboBox();

        public ScorUI()
        {
            btAdd.IsEnabled = false;
            btRem.IsEnabled = false;
            UpdateTypeSelector();

            this.HeaderText = "Sim Scores";
            this.Commited += new EventHandler(ScorUI_Commited);
            this.CanCommit = Helper.XmlRegistry.HiddenMode;

            lb.SelectionChanged += lb_SelectedIndexChanged;
            cbType.SelectionChanged += comboBox1_SelectedIndexChanged;
            btAdd.Click += btAdd_Click;
            btRem.Click += btRem_Click;
        }

        #region IPackedFileUI Member

        protected override void OnWrapperChanged(SimPe.Windows.Forms.WrapperBaseControl.WrapperChangedEventArgs e)
        {
            if (e.OldWrapper != null)
            {
                (e.OldWrapper as Scor).AddedItem -= new Scor.ChangedListHandler(ScorUI_AddedItem);
                (e.OldWrapper as Scor).RemovedItem -= new Scor.ChangedListHandler(ScorUI_RemovedItem);
            }
            if (e.NewWrapper != null)
            {
                (e.NewWrapper as Scor).AddedItem += new Scor.ChangedListHandler(ScorUI_AddedItem);
                (e.NewWrapper as Scor).RemovedItem += new Scor.ChangedListHandler(ScorUI_RemovedItem);
            }
        }

        void ScorUI_RemovedItem(Scor sender, Scor.ChangedListEventArgs e)
        {
            int index = Math.Max(0, lb.SelectedIndex);
            lb.Items.Remove(e.Item);
            index = Math.Min(lb.ItemCount - 1, index);
            if (lb.ItemCount > index) lb.SelectedIndex = index;
        }

        void ScorUI_AddedItem(Scor sender, Scor.ChangedListEventArgs e)
        {
           lb.Items.Add(e.Item);
           lb.SelectedIndex = lb.ItemCount - 1;
        }

        public Wrapper.Scor Scor
        {
            get { return (SimPe.PackedFiles.Wrapper.Scor)Wrapper; }
        }

        public override void RefreshGUI()
        {
            pnContainer.Children.Clear();
            this.tbunk1.Text = Helper.HexString(Scor.Unknown1);
            this.tbunk2.Text = Helper.HexString(Scor.Unknown2);

            btRem.IsEnabled = false;
            lb.Items.Clear();
            foreach (Wrapper.ScorItem si in Scor)
                lb.Items.Add(si);

            if (lb.ItemCount > 0) lb.SelectedIndex = 0;
        }

        #endregion

        void UpdateTypeSelector()
        {
            this.cbType.Items.Clear();
            foreach (string s in ScorItem.GuiElements.Keys)
                cbType.Items.Add(s);

            if (cbType.ItemCount > 0) cbType.SelectedIndex = 0;
        }

        private void ScorUI_Commited(object sender, EventArgs e)
        {
            Scor.SynchronizeUserData();
        }

        private void lb_SelectedIndexChanged(object sender, SelectionChangedEventArgs e)
        {
            Wrapper.ScorItem si = lb.SelectedItem as Wrapper.ScorItem;
            pnContainer.Children.Clear();
            if (si != null)
            {
                if (si.Gui != null)
                {
                    pnContainer.Children.Add(si.Gui);
                }
            }
            btRem.IsEnabled = lb.SelectedItem != null && Helper.XmlRegistry.HiddenMode;
        }

        private void comboBox1_SelectedIndexChanged(object sender, SelectionChangedEventArgs e)
        {
             btAdd.IsEnabled = cbType.SelectedItem != null && Helper.XmlRegistry.HiddenMode;
        }

        private void btAdd_Click(object sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            if (Scor != null)
                if (cbType.SelectedItem != null)
                    Scor.Add(cbType.SelectedItem.ToString());
        }

        private void btRem_Click(object sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            if (Scor != null)
                Scor.Remove(lb.SelectedItem as ScorItem);
        }
    }
}
