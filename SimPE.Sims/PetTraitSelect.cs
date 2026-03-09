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
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace SimPe.PackedFiles.Wrapper
{
    public partial class PetTraitSelect : UserControl
    {
        public enum Levels { High, Normal, Low };
        
        public PetTraitSelect()
        {            
            InitializeComponent();

            Level = Levels.Normal;
        }

        public Levels Level
        {
            get
            {
                if (rb1.Checked) return Levels.High;
                if (rb3.Checked) return Levels.Low;
                return Levels.Normal;
            }
            set
            {
                if (value == Levels.High) rb1.Checked = true;
                else if (value == Levels.Low) rb3.Checked = true;
                else rb2.Checked = true;
            }
        }

        public event EventHandler LevelChanged;
        private void CheckedChanged(object sender, EventArgs e)
        {
            if (LevelChanged != null) LevelChanged(this, new EventArgs());
        }

        public void UpdateTraits(int high, int low, PetTraits traits)
        {
            if (traits == null) return;
            Levels lv = Level;
            traits.SetTrait(high, false);
            traits.SetTrait(low, false);
            if (lv == Levels.High) traits.SetTrait(high, true);
            if (lv == Levels.Low) traits.SetTrait(low, true);
        }

        public void SetTraitLevel(int high, int low, PetTraits traits)
        {
            if (traits == null) return;
            if (traits.GetTrait(high)) Level = Levels.High;
            else if (traits.GetTrait(low)) Level = Levels.Low;
            else Level = Levels.Normal;
        }
    }
}
