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
using SimPe.Interfaces.Plugin;
using SimPe.Interfaces;
using SimPe.PackedFiles.Wrapper.Supporting;
using SimPe.Data;
using Avalonia.Controls;

namespace SimPe.PackedFiles.UserInterface
{
    /// <summary>
    /// Avalonia port of ExtSrelUI.
    /// </summary>
    public class ExtSrel : SimPe.Windows.Forms.WrapperBaseControl, IPackedFileUI
    {
        private TextBlock label1 = new TextBlock();
        private TextBlock lbsims = new TextBlock();
        private SimPe.PackedFiles.UserInterface.CommonSrel sc;
        private Image pb = new Image();

        public ExtSrel()
        {
            HeaderText = SimPe.Localization.GetString("Sim Relation Editor");

            sc = new CommonSrel();
            // sc.ChangedContent += new EventHandler(this.ExtSrel_Commited);

            this.Commited += new System.EventHandler(this.ExtSrel_Commited);
        }

        public SimPe.PackedFiles.Wrapper.ExtSrel Srel
        {
            get { return (SimPe.PackedFiles.Wrapper.ExtSrel)Wrapper; }
        }

        public override void RefreshGUI()
        {
            base.RefreshGUI();

            if (this.Srel == null)
            {
                if (sc != null) sc.Srel = null;
                this.lbsims.Text = "";
                this.pb.Source = null;
                return;
            }

            sc.Srel = this.Srel;

            this.lbsims.Text = sc.SourceSimName + " " + SimPe.Localization.GetString("towards") + " " + sc.TargetSimName;

            System.Drawing.Image img = sc.Image;
            if (img != null)
            {
                img = Ambertation.Drawing.GraphicRoutines.ScaleImage(img, 64, 64, true);
                pb.Source = SimPe.Helper.ToAvaloniaBitmap(img);
            }
            else
            {
                pb.Source = null;
            }
        }

        private void ExtSrel_Commited(object sender, System.EventArgs e)
        {
            Srel.SynchronizeUserData();
        }
    }
}
