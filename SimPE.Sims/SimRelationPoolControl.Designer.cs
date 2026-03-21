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

namespace SimPe.PackedFiles.Wrapper
{
    partial class SimRelationPoolControl
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Designer generated code

        private void InitializeComponent()
        {
            this.panel1 = new Avalonia.Controls.Panel();
            this.panel2 = new Avalonia.Controls.Panel();
            this.label1 = new Avalonia.Controls.TextBlock();
            this.cbNoRelation = new Avalonia.Controls.CheckBox();
            this.cbRelation = new Avalonia.Controls.CheckBox();

            this.cbNoRelation.IsCheckedChanged += new System.EventHandler<Avalonia.Interactivity.RoutedEventArgs>(
                (s, e) => cbNoRelation_CheckedChanged(s, e));
            this.cbRelation.IsCheckedChanged += new System.EventHandler<Avalonia.Interactivity.RoutedEventArgs>(
                (s, e) => cbRelation_CheckedChanged(s, e));
        }

        #endregion

        private Avalonia.Controls.Panel panel1;
        private Avalonia.Controls.CheckBox cbNoRelation;
        private Avalonia.Controls.CheckBox cbRelation;
        private Avalonia.Controls.TextBlock label1;
        private Avalonia.Controls.Panel panel2;
    }
}
