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
using Avalonia.Input;
using Avalonia.Layout;
using SimPe.Interfaces.Scenegraph;

namespace SimPe.Plugin
{
	/// <summary>
	/// Summary description for RcolForm.
	/// </summary>
	public class RcolForm : SimPe.Windows.Forms.WrapperBaseControl
	{
		// ── Controls ────────────────────────────────────────────────────────
		private Avalonia.Controls.Button     llfix;
		private Avalonia.Controls.Button     llhash;
		private Avalonia.Controls.TextBox    tbflname;
		private Avalonia.Controls.TextBlock  label2;
		internal Avalonia.Controls.ComboBox  cbitem;
		private Avalonia.Controls.TabItem    tabPage1;
		private Avalonia.Controls.TabItem    tabPage2;
		internal Avalonia.Controls.ListBox   lbref;
		private Avalonia.Controls.TabControl childtc;
		private Avalonia.Controls.Panel      pntypes;
		internal Avalonia.Controls.Button    lladd;
		internal Avalonia.Controls.Button    lldelete;
		internal Avalonia.Controls.TextBox   tbsubtype;
		internal Avalonia.Controls.TextBox   tbinstance;
		private Avalonia.Controls.TextBlock  label11;
		internal Avalonia.Controls.TextBox   tbtype;
		private Avalonia.Controls.TextBlock  label8;
		private Avalonia.Controls.TextBlock  label9;
		private Avalonia.Controls.TextBlock  label10;
		internal Avalonia.Controls.TextBox   tbgroup;
		internal Avalonia.Controls.ComboBox  cbtypes;
		private Avalonia.Controls.Button     btref;
		private Avalonia.Controls.TabItem    tabPage3;
		private Avalonia.Controls.ListBox    lbblocks;
		private Avalonia.Controls.Button     btup;
		private Avalonia.Controls.Button     btdown;
		private Avalonia.Controls.Button     btadd;
		private Avalonia.Controls.Button     btdel;
		private Avalonia.Controls.ComboBox   cbblocks;
		private Avalonia.Controls.TextBlock  label1;
		internal Avalonia.Controls.TabItem   tpref;
		internal Avalonia.Controls.TreeView  tv;
		private Avalonia.Controls.TextBlock  label3;
		private Avalonia.Controls.TextBlock  label4;
		private Avalonia.Controls.TextBox    tbrefgroup;
		private Avalonia.Controls.TextBox    tbrefinst;
		private Avalonia.Controls.TextBlock  label5;
		private Avalonia.Controls.TextBox    tbfile;
		private Avalonia.Controls.Button     linkLabel1;
		internal Avalonia.Controls.TabControl tbResource;

		public RcolForm() : base()
		{
			BuildLayout();

			if (SimPe.Helper.XmlRegistry.UseBigIcons)
			{
				// Avalonia FontSize instead of System.Drawing.Font
				lbblocks.FontSize = 11;
				lbref.FontSize    = 11;
				tv.FontSize       = 11;
				tbflname.FontSize = 12;
			}

			foreach (Interfaces.IAlias alias in SimPe.Helper.TGILoader.FileTypes)
				cbtypes.Items.Add(alias);

			this.Commited += Commit;
		}

		private void BuildLayout()
		{
			// ── Reference Tab (tpref) ──────────────────────────────────────
			label3    = new Avalonia.Controls.TextBlock { Text = "Group:" };
			label4    = new Avalonia.Controls.TextBlock { Text = "Instance:" };
			label5    = new Avalonia.Controls.TextBlock { Text = "File:" };
			tbrefgroup = new Avalonia.Controls.TextBox { Text = "0x00000000", IsReadOnly = true };
			tbrefinst  = new Avalonia.Controls.TextBox { Text = "0x00000000", IsReadOnly = true };
			tbfile     = new Avalonia.Controls.TextBox { IsReadOnly = true };
			linkLabel1 = new Avalonia.Controls.Button { Content = "reload" };
			linkLabel1.Click += linkLabel1_LinkClicked;

			tv = new Avalonia.Controls.TreeView();
			tv.SelectionChanged += SelectRefItem;
			DragDrop.SetAllowDrop(tv, false);

			var refDetails = new StackPanel { Orientation = Orientation.Vertical, Spacing = 4 };
			refDetails.Children.Add(label3);
			refDetails.Children.Add(tbrefgroup);
			refDetails.Children.Add(label4);
			refDetails.Children.Add(tbrefinst);
			refDetails.Children.Add(label5);
			refDetails.Children.Add(linkLabel1);
			refDetails.Children.Add(tbfile);

			var refPanel = new Grid();
			refPanel.ColumnDefinitions.Add(new ColumnDefinition(new GridLength(1, GridUnitType.Star)));
			refPanel.ColumnDefinitions.Add(new ColumnDefinition(new GridLength(1, GridUnitType.Star)));
			Grid.SetColumn(tv,         0);
			Grid.SetColumn(refDetails, 1);
			refPanel.Children.Add(tv);
			refPanel.Children.Add(refDetails);

			tpref = new Avalonia.Controls.TabItem { Header = "All References", Content = refPanel };

			// ── Content Tab (tabPage1) ─────────────────────────────────────
			label2   = new Avalonia.Controls.TextBlock { Text = "Blocklist:" };
			label1   = new Avalonia.Controls.TextBlock { Text = "Filename:" };
			cbitem   = new Avalonia.Controls.ComboBox();
			cbitem.SelectionChanged += SelectRcolItem;

			tbflname = new Avalonia.Controls.TextBox();
			tbflname.TextChanged += ChangeFileName;

			llhash = new Avalonia.Controls.Button { Content = "assign Hash", IsVisible = false };
			llhash.Click += BuildFilename;
			llfix  = new Avalonia.Controls.Button { Content = "fix TGI" };
			llfix.Click  += FixTGI;

			childtc = new Avalonia.Controls.TabControl();
			childtc.SelectionChanged += ChildTabPageChanged;

			var fnRow = new StackPanel { Orientation = Orientation.Horizontal, Spacing = 4 };
			fnRow.Children.Add(label1);
			fnRow.Children.Add(tbflname);
			fnRow.Children.Add(llhash);
			fnRow.Children.Add(llfix);

			var contentPanel = new StackPanel { Orientation = Orientation.Vertical, Spacing = 4 };
			contentPanel.Children.Add(label2);
			contentPanel.Children.Add(cbitem);
			contentPanel.Children.Add(fnRow);
			contentPanel.Children.Add(childtc);

			tabPage1 = new Avalonia.Controls.TabItem { Header = "Content", Content = contentPanel };

			// ── Reference Tab (tabPage2) ───────────────────────────────────
			lbref = new Avalonia.Controls.ListBox();
			lbref.SelectionChanged += SelectReference;
			DragDrop.SetAllowDrop(lbref, true);
			lbref.AddHandler(DragDrop.DropEvent,     new EventHandler<Avalonia.Input.DragEventArgs>(PackageItemDrop));
			lbref.AddHandler(DragDrop.DragOverEvent, new EventHandler<Avalonia.Input.DragEventArgs>(PackageItemDragEnter));

			label8  = new Avalonia.Controls.TextBlock { Text = "File Type:" };
			label9  = new Avalonia.Controls.TextBlock { Text = "Group:" };
			label10 = new Avalonia.Controls.TextBlock { Text = "Sub Typ:" };
			label11 = new Avalonia.Controls.TextBlock { Text = "Instance:" };
			tbtype     = new Avalonia.Controls.TextBox();
			tbtype.TextChanged += tbtype_TextChanged;
			tbsubtype  = new Avalonia.Controls.TextBox();
			tbsubtype.TextChanged += AutoChangeReference;
			tbgroup    = new Avalonia.Controls.TextBox();
			tbgroup.TextChanged += AutoChangeReference;
			tbinstance = new Avalonia.Controls.TextBox();
			tbinstance.TextChanged += AutoChangeReference;
			cbtypes    = new Avalonia.Controls.ComboBox();
			cbtypes.SelectionChanged += SelectType;
			lladd      = new Avalonia.Controls.Button { Content = "add" };
			lladd.Click    += SRNItemsAAdd;
			lldelete   = new Avalonia.Controls.Button { Content = "delete" };
			lldelete.Click += SRNItemsADelete;
			btref      = new Avalonia.Controls.Button { Content = "u" };
			btref.Click    += ShowPackageSelector;

			pntypes = new Avalonia.Controls.Panel();
			var pntypesStack = new StackPanel { Orientation = Orientation.Vertical, Spacing = 2 };
			pntypesStack.Children.Add(new StackPanel { Orientation = Orientation.Horizontal, Spacing = 4, Children = { label8,  tbtype,  cbtypes } });
			pntypesStack.Children.Add(new StackPanel { Orientation = Orientation.Horizontal, Spacing = 4, Children = { label10, tbsubtype } });
			pntypesStack.Children.Add(new StackPanel { Orientation = Orientation.Horizontal, Spacing = 4, Children = { label9,  tbgroup } });
			pntypesStack.Children.Add(new StackPanel { Orientation = Orientation.Horizontal, Spacing = 4, Children = { label11, tbinstance } });
			pntypesStack.Children.Add(new StackPanel { Orientation = Orientation.Horizontal, Spacing = 4, Children = { lladd, lldelete, btref } });
			pntypes.Children.Add(pntypesStack);

			var refTabPanel = new Grid();
			refTabPanel.ColumnDefinitions.Add(new ColumnDefinition(new GridLength(1, GridUnitType.Star)));
			refTabPanel.ColumnDefinitions.Add(new ColumnDefinition(new GridLength(1, GridUnitType.Star)));
			Grid.SetColumn(lbref,   0);
			Grid.SetColumn(pntypes, 1);
			refTabPanel.Children.Add(lbref);
			refTabPanel.Children.Add(pntypes);

			tabPage2 = new Avalonia.Controls.TabItem { Header = "Reference", Content = refTabPanel };

			// ── Edit Blocks Tab (tabPage3) ─────────────────────────────────
			lbblocks = new Avalonia.Controls.ListBox();
			lbblocks.SelectionChanged += lbblocks_SelectedIndexChanged;

			btup   = new Avalonia.Controls.Button { Content = "Up",     IsEnabled = false };
			btdown = new Avalonia.Controls.Button { Content = "Down",   IsEnabled = false };
			btdel  = new Avalonia.Controls.Button { Content = "Delete", IsEnabled = false };
			btadd  = new Avalonia.Controls.Button { Content = "Add" };
			btup.Click    += btup_Click;
			btdown.Click  += btdown_Click;
			btdel.Click   += btdel_Click;
			btadd.Click   += btadd_Click;

			cbblocks = new Avalonia.Controls.ComboBox();

			var blockButtons = new StackPanel { Orientation = Orientation.Vertical, Spacing = 4 };
			blockButtons.Children.Add(btup);
			blockButtons.Children.Add(btdown);
			blockButtons.Children.Add(btdel);
			blockButtons.Children.Add(btadd);
			blockButtons.Children.Add(cbblocks);

			var blocksPanel = new Grid();
			blocksPanel.ColumnDefinitions.Add(new ColumnDefinition(new GridLength(1, GridUnitType.Star)));
			blocksPanel.ColumnDefinitions.Add(new ColumnDefinition(GridLength.Auto));
			Grid.SetColumn(lbblocks,     0);
			Grid.SetColumn(blockButtons, 1);
			blocksPanel.Children.Add(lbblocks);
			blocksPanel.Children.Add(blockButtons);

			tabPage3 = new Avalonia.Controls.TabItem { Header = "Edit Blocks", Content = blocksPanel };

			// ── Main TabControl ────────────────────────────────────────────
			tbResource = new Avalonia.Controls.TabControl();
			tbResource.Items.Add(tabPage1);
			tbResource.Items.Add(tabPage2);
			tbResource.Items.Add(tabPage3);
			// tpref is added/removed dynamically by RcolUI
			tbResource.SelectionChanged += tabControl1_SelectedIndexChanged;

			this.Content = tbResource;
		}

		public void Dispose()
		{
			// no-op
		}

		internal Rcol wrapper = null;

        internal void BuildChildTabControl(AbstractRcolBlock rb)
        {
            childtc.Items.Clear();

            if (rb == null) return;
            if (rb.TabPage != null) rb.AddToTabControl(childtc);
        }

        private void SelectRcolItem(object sender, Avalonia.Controls.SelectionChangedEventArgs e)
		{
			if (cbitem.Tag != null) return;
			if (cbitem.SelectedIndex < 0) return;
			try
			{
				cbitem.Tag = true;
				SimPe.CountedListItem cli = (SimPe.CountedListItem)cbitem.Items[cbitem.SelectedIndex];
				AbstractRcolBlock rb = (AbstractRcolBlock)cli.Object;
				tbflname.IsEnabled = (rb.NameResource != null);
				llhash.IsEnabled   = tbflname.IsEnabled;
				llfix.IsEnabled    = tbflname.IsEnabled;

				if (rb.NameResource != null) tbflname.Text = rb.NameResource.FileName;
				else tbflname.Text = "";

				BuildChildTabControl(rb);
			}
			catch (Exception ex)
			{
				Helper.ExceptionMessage("", ex);
			}
			finally
			{
				cbitem.Tag = null;
			}
		}

		private void ChangeFileName(object sender, System.EventArgs e)
		{
			if (cbitem.Tag != null) return;
			if (cbitem.SelectedIndex < 0) return;
			try
			{
				cbitem.Tag = true;
				SimPe.CountedListItem cli = (SimPe.CountedListItem)cbitem.Items[cbitem.SelectedIndex];
				AbstractRcolBlock rb = (AbstractRcolBlock)cli.Object;
				if (rb.NameResource != null)
				{
					rb.NameResource.FileName = tbflname.Text;
					cbitem.Items[cbitem.SelectedIndex] = cli;
					// No cbitem.Text in Avalonia – selection label is handled by ComboBox itself
				}
			}
			catch (Exception ex)
			{
				Helper.ExceptionMessage(Localization.Manager.GetString("erropenfile"), ex);
			}
			finally
			{
				cbitem.Tag = null;
			}
		}

		private void BuildFilename(object sender, Avalonia.Interactivity.RoutedEventArgs e)
		{
			string fl = Hashes.StripHashFromName(this.tbflname.Text);
			this.tbflname.Text = Hashes.AssembleHashedFileName(wrapper.Package.FileGroupHash, fl);
		}

		private void FixTGI(object sender, Avalonia.Interactivity.RoutedEventArgs e)
		{
			string fl = Hashes.StripHashFromName(this.tbflname.Text);
			if (wrapper != null)
				if (wrapper.FileDescriptor != null)
				{
					wrapper.FileDescriptor.Instance = Hashes.InstanceHash(fl);
					wrapper.FileDescriptor.SubType  = Hashes.SubTypeHash(fl);
				}
		}

		private void Commit(object sender, System.EventArgs e)
		{
			try
			{
				wrapper.SynchronizeUserData();
				Message.Show(Localization.Manager.GetString("commited"), "Commit", MessageBoxButtons.OK);
			}
			catch (Exception ex)
			{
				Helper.ExceptionMessage(Localization.Manager.GetString("errwritingfile"), ex);
			}
		}

		private void SelectType(object sender, Avalonia.Controls.SelectionChangedEventArgs e)
		{
			if (cbtypes.Tag != null) return;
			tbtype.Text = "0x" + Helper.HexString(((SimPe.Data.TypeAlias)cbtypes.Items[cbtypes.SelectedIndex]).Id);
		}

		protected void Change()
		{
			if (lbref.Tag != null) return;
			if (lbref.SelectedIndex < 0) return;
			try
			{
				lbref.Tag = true;
				Interfaces.Files.IPackedFileDescriptor pfd = (Interfaces.Files.IPackedFileDescriptor)this.lbref.Items[lbref.SelectedIndex];

				pfd.Type     = Convert.ToUInt32(this.tbtype.Text, 16);
				pfd.SubType  = Convert.ToUInt32(this.tbsubtype.Text, 16);
				pfd.Group    = Convert.ToUInt32(this.tbgroup.Text, 16);
				pfd.Instance = Convert.ToUInt32(this.tbinstance.Text, 16);

				lbref.Items[lbref.SelectedIndex] = pfd;
			}
			catch (Exception)
			{
				// suppress
			}
			finally
			{
				lbref.Tag = null;
			}
		}

		private void tbtype_TextChanged(object sender, System.EventArgs e)
		{
			Change();

			cbtypes.Tag = true;
			Data.TypeAlias a = Data.MetaData.FindTypeAlias(Helper.HexStringToUInt(tbtype.Text));

			int ct = 0;
			foreach (Data.TypeAlias i in cbtypes.Items)
			{
				if (i == a)
				{
					cbtypes.SelectedIndex = ct;
					cbtypes.Tag = null;
					return;
				}
				ct++;
			}

			cbtypes.SelectedIndex = -1;
			cbtypes.Tag = null;
		}

		private void SelectReference(object sender, Avalonia.Controls.SelectionChangedEventArgs e)
		{
			if (lbref.Tag != null) return;
			if (lbref.SelectedIndex < 0) return;
			try
			{
				lbref.Tag = true;
				Interfaces.Files.IPackedFileDescriptor pfd = (Interfaces.Files.IPackedFileDescriptor)this.lbref.Items[lbref.SelectedIndex];
				this.tbtype.Text     = "0x" + Helper.HexString(pfd.Type);
				this.tbsubtype.Text  = "0x" + Helper.HexString(pfd.SubType);
				this.tbgroup.Text    = "0x" + Helper.HexString(pfd.Group);
				this.tbinstance.Text = "0x" + Helper.HexString(pfd.Instance);
			}
			catch (Exception ex)
			{
				Helper.ExceptionMessage("", ex);
			}
			finally
			{
				lbref.Tag = null;
			}
		}

		private void AutoChangeReference(object sender, System.EventArgs e)
		{
			Change();
		}

		private void SRNItemsAAdd(object sender, Avalonia.Interactivity.RoutedEventArgs e)
		{
			try
			{
				Interfaces.Files.IPackedFileDescriptor pfd = new SimPe.Packages.PackedFileDescriptor();

				pfd.Type     = Convert.ToUInt32(this.tbtype.Text, 16);
				pfd.SubType  = Convert.ToUInt32(this.tbsubtype.Text, 16);
				pfd.Group    = Convert.ToUInt32(this.tbgroup.Text, 16);
				pfd.Instance = Convert.ToUInt32(this.tbinstance.Text, 16);

				wrapper.ReferencedFiles = (Interfaces.Files.IPackedFileDescriptor[])Helper.Add(wrapper.ReferencedFiles, pfd);
				this.lbref.Items.Add(pfd);

				wrapper.Changed = true;
			}
			catch (Exception ex)
			{
				Helper.ExceptionMessage("", ex);
			}
		}

		private void SRNItemsADelete(object sender, Avalonia.Interactivity.RoutedEventArgs e)
		{
			if (lbref.SelectedIndex < 0) return;
			try
			{
				Interfaces.Files.IPackedFileDescriptor pfd = (Interfaces.Files.IPackedFileDescriptor)this.lbref.Items[lbref.SelectedIndex];

				wrapper.ReferencedFiles = (Interfaces.Files.IPackedFileDescriptor[])Helper.Delete(wrapper.ReferencedFiles, pfd);
				lbref.Items.Remove(pfd);

				wrapper.Changed = true;

				btup.IsEnabled = btdown.IsEnabled = btdel.IsEnabled = false;
			}
			catch (Exception ex)
			{
				Helper.ExceptionMessage("", ex);
			}
		}

		#region Package Selector
		private void ShowPackageSelector(object sender, Avalonia.Interactivity.RoutedEventArgs e)
		{
			SimPe.PackageSelectorForm form = new SimPe.PackageSelectorForm();
			form.Execute(wrapper.Package);
		}

		private void PackageItemDragEnter(object sender, Avalonia.Input.DragEventArgs e)
		{
			if (e.Data.Contains(typeof(SimPe.Packages.PackedFileDescriptor).FullName))
			{
				e.DragEffects = Avalonia.Input.DragDropEffects.Copy;
			}
			else
			{
				e.DragEffects = Avalonia.Input.DragDropEffects.None;
			}
		}

		private void PackageItemDrop(object sender, Avalonia.Input.DragEventArgs e)
		{
			try
			{
				lbref.Tag = true;
				Interfaces.Files.IPackedFileDescriptor pfd = null;
				pfd = e.Data.Get(typeof(SimPe.Packages.PackedFileDescriptor).FullName) as Interfaces.Files.IPackedFileDescriptor;

				wrapper.ReferencedFiles = (Interfaces.Files.IPackedFileDescriptor[])Helper.Add(wrapper.ReferencedFiles, pfd);
				this.lbref.Items.Add(pfd);

				wrapper.Changed = true;
			}
			catch (Exception ex)
			{
				Helper.ExceptionMessage("", ex);
			}
			finally
			{
				lbref.Tag = null;
			}
		}
		#endregion

		protected void UpdateComboBox()
		{
			this.cbitem.Items.Clear();

			this.tbflname.Text = "";
			this.childtc.Items.Clear();
			foreach (SimPe.CountedListItem o in this.lbblocks.Items) cbitem.Items.Add(o);

			if (cbitem.Items.Count > 0) cbitem.SelectedIndex = 0;
		}

		private void tabControl1_SelectedIndexChanged(object sender, Avalonia.Controls.SelectionChangedEventArgs e)
		{
			// Display the Block Editor when tabPage3 is selected
			if (tbResource.SelectedIndex >= 0 && tbResource.Items[tbResource.SelectedIndex] == this.tabPage3)
			{
				this.lbblocks.Items.Clear();
				foreach (IRcolBlock irb in wrapper.Blocks) SimPe.CountedListItem.AddHex(lbblocks, irb);

				this.cbblocks.Items.Clear();
				foreach (string s in Rcol.Tokens.Keys)
				{
					try
					{
						Type t = (Type)Rcol.Tokens[s];
						IRcolBlock irb = AbstractRcolBlock.Create(t, null);
						cbblocks.Items.Add(irb);
					}
					catch (Exception ex)
					{
						Helper.ExceptionMessage("Error in Block " + s, ex);
					}
				}
				if (cbblocks.Items.Count > 0) cbblocks.SelectedIndex = 0;
			}
		}

		private void btup_Click(object sender, Avalonia.Interactivity.RoutedEventArgs e)
		{
			if (lbblocks.SelectedIndex < 1) return;
			try
			{
				object o = lbblocks.Items[lbblocks.SelectedIndex - 1];
				lbblocks.Items[lbblocks.SelectedIndex - 1] = lbblocks.Items[lbblocks.SelectedIndex];
				lbblocks.Items[lbblocks.SelectedIndex]     = o;

				wrapper.Blocks[lbblocks.SelectedIndex]     = (AbstractRcolBlock)((SimPe.CountedListItem)lbblocks.Items[lbblocks.SelectedIndex]).Object;
				wrapper.Blocks[lbblocks.SelectedIndex - 1] = (AbstractRcolBlock)((SimPe.CountedListItem)lbblocks.Items[lbblocks.SelectedIndex - 1]).Object;
				lbblocks.SelectedIndex--;

				UpdateComboBox();
			}
			catch (Exception ex)
			{
				Helper.ExceptionMessage("", ex);
			}
		}

		private void btdown_Click(object sender, Avalonia.Interactivity.RoutedEventArgs e)
		{
			if (lbblocks.SelectedIndex < 0) return;
			if (lbblocks.SelectedIndex > lbblocks.Items.Count - 2) return;
			try
			{
				object o = lbblocks.Items[lbblocks.SelectedIndex + 1];
				lbblocks.Items[lbblocks.SelectedIndex + 1] = lbblocks.Items[lbblocks.SelectedIndex];
				lbblocks.Items[lbblocks.SelectedIndex]     = o;
				wrapper.Blocks[lbblocks.SelectedIndex]     = (AbstractRcolBlock)((SimPe.CountedListItem)lbblocks.Items[lbblocks.SelectedIndex]).Object;
				wrapper.Blocks[lbblocks.SelectedIndex + 1] = (AbstractRcolBlock)((SimPe.CountedListItem)lbblocks.Items[lbblocks.SelectedIndex + 1]).Object;
				lbblocks.SelectedIndex++;

				UpdateComboBox();
			}
			catch (Exception ex)
			{
				Helper.ExceptionMessage("", ex);
			}
		}

		private void btadd_Click(object sender, Avalonia.Interactivity.RoutedEventArgs e)
		{
			try
			{
				IRcolBlock irb = ((IRcolBlock)cbblocks.Items[cbblocks.SelectedIndex]).Create();
				if (irb is AbstractRcolBlock) ((AbstractRcolBlock)irb).Parent = wrapper;
				SimPe.CountedListItem.AddHex(this.lbblocks, irb);
				wrapper.Blocks = (IRcolBlock[])Helper.Add(wrapper.Blocks, irb, typeof(IRcolBlock));
				UpdateComboBox();
			}
			catch (Exception ex)
			{
				Helper.ExceptionMessage("", ex);
			}
		}

		private void lbblocks_SelectedIndexChanged(object sender, Avalonia.Controls.SelectionChangedEventArgs e)
		{
			btup.IsEnabled = btdown.IsEnabled = btdel.IsEnabled = false;
			if (lbblocks.SelectedIndex < 0) return;
			btup.IsEnabled = btdown.IsEnabled = btdel.IsEnabled = true;
		}

		private void btdel_Click(object sender, Avalonia.Interactivity.RoutedEventArgs e)
		{
			if (lbblocks.SelectedIndex < 0) return;
			try
			{
				SimPe.CountedListItem cli = (SimPe.CountedListItem)lbblocks.Items[lbblocks.SelectedIndex];
				IRcolBlock irb = ((IRcolBlock)cli.Object);
				this.lbblocks.Items.Remove(cli);
				wrapper.Blocks = (IRcolBlock[])Helper.Delete(wrapper.Blocks, irb, typeof(IRcolBlock));

				UpdateComboBox();
			}
			catch (Exception ex)
			{
				Helper.ExceptionMessage("", ex);
			}
		}

		private void SelectRefItem(object sender, Avalonia.Controls.SelectionChangedEventArgs e)
		{
			Avalonia.Controls.TreeViewItem node = (Avalonia.Controls.TreeViewItem)tv.SelectedItem;
			if (node == null) return;
			if (node.Tag != null)
			{
				Interfaces.Files.IPackedFileDescriptor pfd = (Interfaces.Files.IPackedFileDescriptor)node.Tag;
				tbrefgroup.Text = "0x" + Helper.HexString(pfd.Group);
				tbrefinst.Text  = "0x" + Helper.HexString(pfd.Instance);

				SimPe.FileTable.FileIndex.Load();
				SimPe.Interfaces.Scenegraph.IScenegraphFileIndexItem[] items = FileTable.FileIndex.FindFile(pfd, null);
				if (items.Length == 0)
				{
					SimPe.Interfaces.Scenegraph.IScenegraphFileIndexItem item = FileTable.FileIndex.FindFileByName(pfd.Filename, pfd.Type, pfd.Group, true);
					if (item != null)
					{
						items = new SimPe.Interfaces.Scenegraph.IScenegraphFileIndexItem[1];
						items[0] = item;
					}
				}
				if (items.Length == 0)
				{
					Interfaces.Files.IPackedFileDescriptor npfd = (Interfaces.Files.IPackedFileDescriptor)pfd.Clone();
					npfd.SubType = 0;
					items = FileTable.FileIndex.FindFile(npfd, null);
				}

				if (items.Length > 0)
				{
					tbfile.Text = items[0].Package.FileName;
				}
				else
				{
					tbfile.Text = "[unreferenced]";
				}
			}
		}

		private void linkLabel1_LinkClicked(object sender, Avalonia.Interactivity.RoutedEventArgs e)
		{
			WaitingScreen.Wait();
			try   { SimPe.FileTable.FileIndex.ForceReload(); }
			finally { WaitingScreen.Stop(); }
		}

		private void ChildTabPageChanged(object sender, Avalonia.Controls.SelectionChangedEventArgs e)
		{
			wrapper.ChildTabPageChanged(this, e);
		}

		internal void ClearControlTags()
		{
			// In Avalonia we do not iterate WinForms Control.Controls;
			// tags are cleared on individual controls as needed.
			cbitem.Tag   = null;
			lbref.Tag    = null;
			cbtypes.Tag  = null;
		}
	}
}
