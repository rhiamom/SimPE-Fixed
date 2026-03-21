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
using SimPe.Interfaces.Plugin;
using Avalonia.Controls;

namespace SimPe.PackedFiles.UserInterface
{
	/// <summary>
	/// Avalonia port of SimDNAUI.
	/// PropertyGrid replaced with read-only multi-line TextBox.
	/// </summary>
	public class SimDNAUI : SimPe.Windows.Forms.WrapperBaseControl, IPackedFileUI
	{
		private TextBlock label1 = new TextBlock { Text = "Dominant Gene:" };
		// PropertyGrid -> read-only TextBox (key=value dump)
		private TextBox pbDom = new TextBox { IsReadOnly = true, AcceptsReturn = true };
		private TextBlock label2 = new TextBlock { Text = "Recessive Gene:" };
		private TextBox pbRec = new TextBox { IsReadOnly = true, AcceptsReturn = true };
        private TextBlock lbbody = new TextBlock { Text = "Unknown", IsVisible = false };
        private ListBox lbcpf = new ListBox { IsVisible = false };

		public SimDNAUI()
		{
			this.HeaderText = "Sim DNA";
			this.Commited += new EventHandler(SimDNAUI_Commited);
		}

		#region IPackedFileUI Member

		public Wrapper.SimDNA Sdna
		{
			get { return (SimPe.PackedFiles.Wrapper.SimDNA)Wrapper; }
		}

        private SimPe.PackedFiles.Wrapper.Cpf wrp
        {
            get { return (SimPe.PackedFiles.Wrapper.Cpf)Wrapper; }
        }

        public override void RefreshGUI()
        {
            if (Sdna.Dominant.Skintone != "" || Sdna.Dominant.Hair != "")
            {
                // Show the DNA panel
                label2.IsVisible = pbRec.IsVisible = label1.IsVisible = pbDom.IsVisible = true;
                lbcpf.IsVisible = false;

                // Render DNA as key=value text (no PropertyGrid in Avalonia)
                pbDom.Text = FormatObject(Sdna.Dominant);
                pbRec.Text = FormatObject(Sdna.Recessive);

                lbbody.IsVisible = false;
            }
            else
            {
                // No DNA present — fall back to CPF list view
                label2.IsVisible = pbRec.IsVisible = lbbody.IsVisible = label1.IsVisible = pbDom.IsVisible = false;
                lbcpf.IsVisible = true;
                lbcpf.Items.Clear();

                foreach (SimPe.PackedFiles.Wrapper.CpfItem item in wrp.Items)
                    lbcpf.Items.Add(item);
            }
        }

        static string FormatObject(object obj)
        {
            if (obj == null) return "";
            var sb = new System.Text.StringBuilder();
            foreach (var prop in obj.GetType().GetProperties())
            {
                try { sb.AppendLine(prop.Name + " = " + prop.GetValue(obj)); }
                catch { }
            }
            return sb.ToString();
        }

		#endregion

        private void SimDNAUI_Commited(object sender, EventArgs e)
		{
			Sdna.SynchronizeUserData();
            RefreshGUI();
		}
	}
}
