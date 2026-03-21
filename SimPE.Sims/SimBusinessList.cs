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
    /// Avalonia port of SimBusinessList.
    /// </summary>
    public class SimBusinessList : Avalonia.Controls.UserControl
    {
        private ComboBox cb = new ComboBox();

        public SimBusinessList()
        {
            cb.SelectionChanged += cb_SelectedIndexChanged;

            if (!IsInitialized) SetContent();
        }

        public void Dispose() { }

        bool loaded;
        LinkedSDesc sdsc;
        public SimPe.Interfaces.Wrapper.ISDesc SimDescription
        {
            get { return sdsc; }
            set
            {
                sdsc = value as LinkedSDesc;
                SetContent();
            }
        }

        void SetContent()
        {
            loaded = IsVisible;
            if (!loaded) return;
            cb.Items.Clear();
            if (sdsc != null)
            {
                SimPe.Interfaces.Providers.ILotItem[] items = sdsc.BusinessList;
                foreach (SimPe.Interfaces.Providers.ILotItem item in items)
                    cb.Items.Add(item);

                if (cb.ItemCount > 0) cb.SelectedIndex = 0;
                else if (SelectedBusinessChanged != null) SelectedBusinessChanged(this, new EventArgs());
            }
        }

        public event System.EventHandler SelectedBusinessChanged;

        public SimPe.Interfaces.Providers.ILotItem SelectedBusiness
        {
            get { return cb.SelectedItem as SimPe.Interfaces.Providers.ILotItem; }
        }

        private void cb_SelectedIndexChanged(object sender, SelectionChangedEventArgs e)
        {
            if (SelectedBusinessChanged != null) SelectedBusinessChanged(this, new EventArgs());
        }
    }
}
