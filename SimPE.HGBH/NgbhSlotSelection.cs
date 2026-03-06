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
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;

namespace SimPe.Plugin
{
	/// <summary>
	/// Summary description for NgbhSlotSelection.
	/// </summary>
	[System.ComponentModel.DefaultEvent("SelectedSlotChanged")]
	public class NgbhSlotSelection : System.Windows.Forms.UserControl
	{
		private SimPe.Plugin.NgbhSlotListView lv;
		private Ambertation.Windows.Forms.EnumComboBox cb;
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public NgbhSlotSelection()
		{
			SetStyle(
				ControlStyles.SupportsTransparentBackColor |
				ControlStyles.AllPaintingInWmPaint |
				//ControlStyles.Opaque |
				ControlStyles.UserPaint |
				ControlStyles.ResizeRedraw 
				| ControlStyles.DoubleBuffer
				,true);

			// Required designer variable.
			InitializeComponent();

			cb.Enum = typeof(Data.NeighborhoodSlots);
			cb.ResourceManager = SimPe.Localization.Manager;
			cb.SelectedIndex = 0;
		}

		/// <summary> 
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(NgbhSlotSelection));
            this.lv = new SimPe.Plugin.NgbhSlotListView();
            this.cb = new Ambertation.Windows.Forms.EnumComboBox();
            this.SuspendLayout();
            // 
            // lv
            // 
            resources.ApplyResources(this.lv, "lv");
            this.lv.Name = "lv";
            this.lv.NgbhResource = null;
            this.lv.Slot = null;
            this.lv.Slots = null;
            this.lv.SlotType = SimPe.Data.NeighborhoodSlots.LotsIntern;
            this.lv.SelectedSlotChanged += new System.EventHandler(this.lv_SelectedSlotChanged);
            // 
            // cb
            // 
            resources.ApplyResources(this.cb, "cb");
            this.cb.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cb.Enum = null;
            this.cb.ForeColor = System.Drawing.SystemColors.ControlText;
            this.cb.Name = "cb";
            this.cb.ResourceManager = null;
            this.cb.SelectedIndexChanged += new System.EventHandler(this.cb_SelectedIndexChanged);
            // 
            // NgbhSlotSelection
            // 
            this.Controls.Add(this.cb);
            this.Controls.Add(this.lv);
            resources.ApplyResources(this, "$this");
            this.Name = "NgbhSlotSelection";
            this.ResumeLayout(false);

		}
		#endregion

		private void cb_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			if (cb.SelectedIndex>=0)
			{
				lv.SlotType = SlotType;
				SetContent();
			}
		}

		Ngbh ngbh;
		[System.ComponentModel.Browsable(false)]
		public Ngbh NgbhResource
		{
			get {return ngbh;}
			set 
			{
				ngbh = value;
				lv.NgbhResource = ngbh;			
			}
		}

		void SetContent()
		{			
			lv.SlotType = SlotType;
		}

		private void lv_SelectedSlotChanged(object sender, System.EventArgs e)
		{
			if (SelectedSlotChanged!=null) SelectedSlotChanged(this, e);
		}

					

		public NgbhSlot SelectedSlot
		{
			get 
			{
				return lv.SelectedSlot;
			}
		}

		public Data.NeighborhoodSlots SlotType 
		{
			get {
				if (cb.SelectedIndex<0) return Data.NeighborhoodSlots.Lots;
				return (Data.NeighborhoodSlots)cb.SelectedValue;
			}
		}

		public event EventHandler SelectedSlotChanged;

		
	}
}
