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
using Avalonia.Controls;

namespace SimPe.PackedFiles.Wrapper
{
    /// <summary>
    /// Avalonia port of SimComboBox — a combo box filled with Sim names from the provider.
    /// </summary>
    [System.ComponentModel.DefaultEvent("SelectedSimChanged")]
    public class SimComboBox : Avalonia.Controls.UserControl
    {
        private ComboBox cb = new ComboBox();

        public SimComboBox()
        {
            cb.SelectionChanged += cb_SelectedIndexChanged;

            try
            {
                if (!IsInitialized)
                    SimPe.FileTable.ProviderRegistry.SimDescriptionProvider.ChangedPackage += new EventHandler(SimDescriptionProvider_ChangedPackage);
                needreload = true;
            }
            catch { }
        }

        public void Dispose()
        {
            try
            {
                SimPe.FileTable.ProviderRegistry.SimDescriptionProvider.ChangedPackage -= new EventHandler(SimDescriptionProvider_ChangedPackage);
            }
            catch { }
        }

        void SetContent()
        {
            cb.Items.Clear();
            foreach (SimPe.PackedFiles.Wrapper.ExtSDesc sdsc in FileTable.ProviderRegistry.SimDescriptionProvider.SimInstance.Values)
            {
                SimPe.Interfaces.IAlias a = new SimPe.Data.StaticAlias(sdsc.SimId, sdsc.SimName + " " + sdsc.SimFamilyName, new object[] { sdsc });
                cb.Items.Add(a);
            }
        }

        public ushort SelectedSimInstance
        {
            get
            {
                SimPe.PackedFiles.Wrapper.ExtSDesc sdsc = SelectedSim;
                if (sdsc != null) return sdsc.Instance;
                return 0xffff;
            }
            set
            {
                int id = -1;
                int ct = 0;
                foreach (SimPe.Interfaces.IAlias a in cb.Items)
                {
                    SimPe.PackedFiles.Wrapper.ExtSDesc s = a.Tag[0] as SimPe.PackedFiles.Wrapper.ExtSDesc;
                    if (s.Instance == value)
                    {
                        id = ct;
                        break;
                    }
                    ct++;
                }
                cb.SelectedIndex = id;
            }
        }

        public uint SelectedSimId
        {
            get
            {
                SimPe.PackedFiles.Wrapper.ExtSDesc sdsc = SelectedSim;
                if (sdsc != null) return sdsc.SimId;
                return 0xffffffff;
            }
            set
            {
                int id = -1;
                int ct = 0;
                foreach (SimPe.Interfaces.IAlias a in cb.Items)
                {
                    SimPe.PackedFiles.Wrapper.ExtSDesc s = a.Tag[0] as SimPe.PackedFiles.Wrapper.ExtSDesc;
                    if (s.SimId == value)
                    {
                        id = ct;
                        break;
                    }
                    ct++;
                }
                cb.SelectedIndex = id;
            }
        }

        public SimPe.PackedFiles.Wrapper.ExtSDesc SelectedSim
        {
            get
            {
                if (cb.SelectedItem == null) return null;
                SimPe.Interfaces.IAlias a = cb.SelectedItem as SimPe.Interfaces.IAlias;
                return a.Tag[0] as SimPe.PackedFiles.Wrapper.ExtSDesc;
            }
            set
            {
                int id = -1;
                if (value != null)
                {
                    int ct = 0;
                    foreach (SimPe.Interfaces.IAlias a in cb.Items)
                    {
                        SimPe.PackedFiles.Wrapper.ExtSDesc s = a.Tag[0] as SimPe.PackedFiles.Wrapper.ExtSDesc;
                        if (s.Instance == value.Instance)
                        {
                            id = ct;
                            break;
                        }
                        ct++;
                    }
                }
                cb.SelectedIndex = id;
            }
        }

        public void Reload()
        {
            needreload = false;
            SetContent();
        }

        public event EventHandler SelectedSimChanged;
        private void cb_SelectedIndexChanged(object sender, SelectionChangedEventArgs e)
        {
            if (SelectedSimChanged != null) SelectedSimChanged(this, new EventArgs());
        }

        bool needreload;
        private void SimDescriptionProvider_ChangedPackage(object sender, EventArgs e)
        {
            needreload = true;
            if (this.IsVisible) Reload();
        }
    }
}
