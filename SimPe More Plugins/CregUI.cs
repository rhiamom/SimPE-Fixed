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
using SimPe.Interfaces.Plugin;

namespace SimPe.Plugin
{
	/// <summary>
	/// This class is used to fill the UI for this FileType with Data
	/// </summary>
    public partial class CregPackedFileUI : SimPe.Windows.Forms.WrapperBaseControl, IPackedFileUI
    {
        protected new CregPackedFileWrapper Wrapper
        {
            get { return base.Wrapper as CregPackedFileWrapper; }
        }
        public CregPackedFileWrapper TPFW
        {
            get { return (CregPackedFileWrapper)Wrapper; }
        }

        #region WrapperBaseControl Member

        bool intern;

        public CregPackedFileUI()
        {
            InitializeComponent();
        }

        public override void RefreshGUI()
        {
            base.RefreshGUI();
            intern = true;
            if (Helper.XmlRegistry.UseBigIcons)
            {
                this.rtbContent.Size = new System.Drawing.Size(530, this.rtbContent.Size.Height);
                this.rtbContent.Font = new System.Drawing.Font(this.rtbContent.Font.FontFamily, 12F);
            }

            this.rtbContent.Text = "";
            this.tbGuid.Text = Wrapper.GooiVal;
            this.tbCrc.Text = Wrapper.CRCVal;
            this.tbVer.Text = Wrapper.VersVal;

            if (Wrapper.Vesion == 1)
            {
                this.CanCommit = false;
                this.rtbContent.Visible = true;
                for (int i = 0; i < Wrapper.Qunty; i++)
                {
                    this.rtbContent.Text += Wrapper.Conent[i] + "\r\n";
                }
            }
            else
            {
                this.CanCommit = true;
                this.rtbContent.Visible = false;
            }
            intern = false;
        }

        public override void OnCommit()
        {
            base.OnCommit();
            TPFW.SynchronizeUserData(true, false);
        }
        #endregion

        #region IPackedFileUI Member
        System.Windows.Forms.Control IPackedFileUI.GUIHandle
        {
            get { return this; }
        }
        #endregion

        #region IDisposable Member

        void IDisposable.Dispose()
        {
            this.TPFW.Dispose();
        }
        #endregion

        private void tbVer_TextChanged(object sender, EventArgs e)
        {
            if (!intern) Wrapper.VersVal = this.tbVer.Text;
        }

    }
}
