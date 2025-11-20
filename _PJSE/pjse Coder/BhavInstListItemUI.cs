/***************************************************************************
 *   Copyright (C) 2005 by Peter L Jones                                   *
 *   pljones@users.sf.net                                                  *
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
using SimPe.PackedFiles.Wrapper;

namespace SimPe.PackedFiles.UserInterface
{
	/// <summary>
	/// Summary description for BhavInstListItemUI.
	/// </summary>
	public class BhavInstListItemUI : System.Windows.Forms.UserControl
    {
        #region Control variables
        private System.Windows.Forms.Label instrText;
		private System.Windows.Forms.LinkLabel trueTarget;
		private System.Windows.Forms.LinkLabel falseTarget;
        private System.Windows.Forms.TextBox bhavInstListItem;
        internal ToolTip toolTip1;
        #endregion
        private IContainer components;

        public BhavInstListItemUI()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();

			// TODO: Add any initialization after the InitializeComponent call
			this.Height = rowHeight;
			MakeUnselected();
			pjse.FileTable.GFT.FiletableRefresh += new EventHandler(FiletableRefresh);

            if (strTrue == null) strTrue = this.trueTarget.Text;
            if (strFalse == null) strFalse = this.falseTarget.Text;
            if (SimPe.Helper.WindowsRegistry.UseBigIcons && Screen.PrimaryScreen.WorkingArea.Width > 1600)
            {
                bhavInstListItem.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F); // 12 works well like this but it is a bit close, tried 11.25F was OK
                trueTarget.Location = new System.Drawing.Point(476, 9); // up 10 back 40  - for 10F up 4 back 20 - try down 2 forward 5
                falseTarget.Location = new System.Drawing.Point(550, 9); // up 10 back 20 - back 10
            }
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

			pjse.FileTable.GFT.FiletableRefresh -= new EventHandler(FiletableRefresh);
			Index = -1;
			Wrapper = null;
		}


		#region BhavInstListItemUI
		public const int rowHeight = 32;
		public event EventHandler Selected;
		public event EventHandler Unselected;
		public event LinkLabelLinkClickedEventHandler TargetClick;
		public event EventHandler MoveUp;
		public event EventHandler MoveDown;
		protected virtual void OnSelected(EventArgs e) { if (Selected != null) { Selected(this, e); } }
		protected virtual void OnUnselected(EventArgs e) { if (Unselected != null) { Unselected(this, e); } }
		protected virtual void OnTargetClick(LinkLabelLinkClickedEventArgs e) { if (TargetClick != null) { TargetClick(this, e); } }
		protected virtual void OnMoveUp(EventArgs e) { if (MoveUp != null) { MoveUp(this, e); } }
		protected virtual void OnMoveDown(EventArgs e) { if (MoveDown != null) { MoveDown(this, e); } }


		private Bhav wrapper = null;
		private int index = -1;

        private static String strTrue  = null;
        private static String strFalse = null;

		public Bhav Wrapper
		{
			set
			{
				if (wrapper != value)
				{
					if (wrapper != null)
						wrapper.WrapperChanged -= new EventHandler(WrapperChanged);
					wrapper = value;
					if (wrapper != null)
					{
						if (index != -1)
							this.WrapperChanged(wrapper[index], null);
						wrapper.WrapperChanged += new EventHandler(WrapperChanged);
					}
				}
			}
		}

		public int Index
		{
			set
			{
				if (index != value)
				{
					index = value;
					if (wrapper != null && index != -1)
						this.WrapperChanged(wrapper[index], null);
				}
			}
            get
            {
                return index;
            }
		}

        public void SetComment(string tip)
        {
            this.toolTip1.RemoveAll();
            if (tip != "" && tip != null)
                this.toolTip1.SetToolTip(this.instrText, tip);
        }

		public void MakeSelected()
        {
            if (booby.ThemeManager.ThemedForms)
            {
                if (booby.ThemeManager.Global.CurrentTheme == booby.GuiTheme.DeepPurple)
                    this.bhavInstListItem.BackColor = booby.ThemeManager.Global.ThemeColorLighter;
                else
                    this.bhavInstListItem.BackColor = booby.ThemeManager.Global.ThemeColorMild;
            }
            else this.BackColor = this.bhavInstListItem.BackColor = System.Drawing.Color.LightGray;// .PowderBlue;
		}

		public void MakeUnselected()
		{
			this.BackColor = this.bhavInstListItem.BackColor = System.Drawing.Color.White;
		}

        private static string fmt = "0x{0} ({1}): {2}";
        private static string Content(int index, pjse.BhavWiz inst)
        {
            return Format(fmt, index.ToString("X"), index.ToString(), cleanup(inst.ShortName));
        }
        private static string Format(string res, params string[] args)
        {
            for (int i = 0; i < args.Length; i++)
                res = res.Replace("{" + i.ToString() + "}", args[i]);
            return res;
        }
        private static string cleanup(string str)
        {
            for (char c = System.Convert.ToChar(1); c < ' '; c++) str = str.Replace(c, ' ');
            return str;
        }

        private void WrapperChanged(object sender, System.EventArgs e)
		{
			if (wrapper == null || index == -1) return;

			if (!(sender is Instruction) || wrapper.IndexOf((Instruction)sender) != index) return;
			Instruction inst = (Instruction)sender;

			bhavInstListItem.Text = "";
			instrText.Text = Content(index, inst);//LongName;

			trueTarget.Text = strTrue + ": "+inst.Target1.ToString("X");
			trueTarget.LinkArea = new LinkArea(0, 0);
			if (inst.Target1 < wrapper.Count)
				trueTarget.Links.Add(6, trueTarget.Text.Length-6, inst.Target1);

            falseTarget.Text = strFalse + ": " + inst.Target2.ToString("X");
			falseTarget.LinkArea = new LinkArea(0, 0);
			if (inst.Target2 < wrapper.Count)
				falseTarget.Links.Add(7, falseTarget.Text.Length-7, inst.Target2);
		}

		private void FiletableRefresh(object sender, System.EventArgs e)
		{
			if (wrapper == null || index == -1) return;
            instrText.Text = Content(index, wrapper[index]);//LongName;
        }
		#endregion

		#region Component Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.components = new System.ComponentModel.Container();
            this.instrText = new System.Windows.Forms.Label();
            this.trueTarget = new System.Windows.Forms.LinkLabel();
            this.falseTarget = new System.Windows.Forms.LinkLabel();
            this.bhavInstListItem = new System.Windows.Forms.TextBox();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.bhavInstListItem.SuspendLayout();
            this.SuspendLayout();
            // 
            // instrText
            // 
            this.instrText.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.instrText.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.instrText.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.instrText.Location = new System.Drawing.Point(0, 0);
            this.instrText.Name = "instrText";
            this.instrText.Size = new System.Drawing.Size(640, 32);
            this.instrText.TabIndex = 2;
            this.instrText.Text = "instrText";
            this.instrText.UseMnemonic = false;
            this.instrText.Click += new System.EventHandler(this.Control_Click);
            // 
            // trueTarget
            // 
            this.trueTarget.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.trueTarget.AutoSize = true;
            this.trueTarget.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.trueTarget.LinkArea = new System.Windows.Forms.LinkArea(0, 0);
            this.trueTarget.Location = new System.Drawing.Point(496, 13);
            this.trueTarget.Name = "trueTarget";
            this.trueTarget.Size = new System.Drawing.Size(25, 13);
            this.trueTarget.TabIndex = 3;
            this.trueTarget.Text = "true";
            this.trueTarget.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            this.trueTarget.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.Target_LinkClicked);
            this.trueTarget.Click += new System.EventHandler(this.Control_Click);
            // 
            // falseTarget
            // 
            this.falseTarget.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.falseTarget.AutoSize = true;
            this.falseTarget.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.falseTarget.LinkArea = new System.Windows.Forms.LinkArea(0, 0);
            this.falseTarget.Location = new System.Drawing.Point(560, 13);
            this.falseTarget.Name = "falseTarget";
            this.falseTarget.Size = new System.Drawing.Size(29, 13);
            this.falseTarget.TabIndex = 4;
            this.falseTarget.Text = "false";
            this.falseTarget.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            this.falseTarget.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.Target_LinkClicked);
            this.falseTarget.Click += new System.EventHandler(this.Control_Click);
            // 
            // bhavInstListItem
            // 
            this.bhavInstListItem.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.bhavInstListItem.BackColor = System.Drawing.Color.White;
            this.bhavInstListItem.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.bhavInstListItem.Controls.Add(this.falseTarget);
            this.bhavInstListItem.Controls.Add(this.trueTarget);
            this.bhavInstListItem.Controls.Add(this.instrText);
            this.bhavInstListItem.Cursor = System.Windows.Forms.Cursors.Default;
            this.bhavInstListItem.Location = new System.Drawing.Point(0, 0);
            this.bhavInstListItem.Multiline = true;
            this.bhavInstListItem.Name = "bhavInstListItem";
            this.bhavInstListItem.Size = new System.Drawing.Size(640, 32);
            this.bhavInstListItem.TabIndex = 1;
            this.bhavInstListItem.Text = "bhavInstListItem";
            this.bhavInstListItem.KeyDown += new System.Windows.Forms.KeyEventHandler(this.bhavInstListItem_KeyDown);
            // 
            // toolTip1
            // 
            this.toolTip1.AutoPopDelay = 8000;
            this.toolTip1.InitialDelay = 500;
            this.toolTip1.IsBalloon = true;
            this.toolTip1.ReshowDelay = 100;
            this.toolTip1.ToolTipTitle = "Comment";
            // 
            // BhavInstListItemUI
            // 
            this.BackColor = System.Drawing.SystemColors.Control;
            this.Controls.Add(this.bhavInstListItem);
            this.Name = "BhavInstListItemUI";
            this.Size = new System.Drawing.Size(640, 32);
            this.Leave += new System.EventHandler(this.bhavInstListItemUI_Leave);
            this.Enter += new System.EventHandler(this.bhavInstListItemUI_Enter);
            this.bhavInstListItem.ResumeLayout(false);
            this.bhavInstListItem.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		#endregion

		private void bhavInstListItemUI_Enter(object sender, System.EventArgs e)
		{
            //MakeSelected();
            if (booby.ThemeManager.ThemedForms)
            {
                if (booby.ThemeManager.Global.CurrentTheme == booby.GuiTheme.DeepPurple)
                    this.bhavInstListItem.BackColor = booby.ThemeManager.Global.ThemeColorLighter;
                else
                    this.bhavInstListItem.BackColor = booby.ThemeManager.Global.ThemeColorMild;
            }
			else this.BackColor = this.bhavInstListItem.BackColor = System.Drawing.Color.PowderBlue;
			OnSelected(e);
		}

		private void bhavInstListItemUI_Leave(object sender, System.EventArgs e)
        {
            if (booby.ThemeManager.ThemedForms)
            {
                if (booby.ThemeManager.Global.CurrentTheme == booby.GuiTheme.SoftLilac)
                    this.BackColor = this.bhavInstListItem.BackColor = booby.ThemeManager.Global.ThemeColorMild;
                else
                    this.BackColor = this.bhavInstListItem.BackColor = booby.ThemeManager.Global.ThemeColorLight;
            }
            else
                this.BackColor = this.bhavInstListItem.BackColor = System.Drawing.Color.LightGray;
			OnUnselected(e);
		}

		private void bhavInstListItem_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
		{
			this.OnKeyDown(e);
		}

		private void Control_Click(object sender, System.EventArgs e)
		{
			this.Focus();
		}

		private void Target_LinkClicked(object sender, System.Windows.Forms.LinkLabelLinkClickedEventArgs e)
		{
			OnTargetClick(e);
		}

		private void moveUp_LinkClicked(object sender, System.Windows.Forms.LinkLabelLinkClickedEventArgs e)
		{
			OnMoveUp(e);
		}

		private void moveDown_LinkClicked(object sender, System.Windows.Forms.LinkLabelLinkClickedEventArgs e)
		{
			OnMoveDown(e);
		}

	}
}
