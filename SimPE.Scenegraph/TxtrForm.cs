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
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Layout;
using SimPe.Interfaces.Plugin;
using GdiSize  = System.Drawing.Size;
using GdiPoint = System.Drawing.Point;
using SimPe.Interfaces.Scenegraph;
using SimPe.Windows.Forms;
using Avalonia.Platform.Storage;
using SimPe.Scenegraph.Compat;
using SkiaSharp;

namespace SimPe.Plugin
{
	/// <summary>
	/// Summary description for TxtrForm.
	/// </summary>
	public class TxtrForm : Avalonia.Controls.Window
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		public TxtrForm()
		{
            InitializeComponent();
            if (Helper.XmlRegistry.UseBigIcons)
            {
                if (lbimg != null) lbimg.FontSize = 13;
                if (tblifo != null) tblifo.FontSize = 14;
                if (tbflname != null) tbflname.FontSize = 14;
            }

			tbwidth.IsReadOnly = true;
			tbheight.IsReadOnly = true;
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void OnClosed(EventArgs e)
		{
			base.OnClosed(e);
		}

		#region Avalonia layout

		static Button MakeHyperlink(string text) => new Button
		{
			Content = text,
			Background = Avalonia.Media.Brushes.Transparent,
			BorderThickness = new Thickness(0),
			Foreground = new Avalonia.Media.SolidColorBrush(Avalonia.Media.Color.FromRgb(0, 102, 204)),
			FontSize = 11,
			Padding = new Thickness(0),
			Cursor = new Avalonia.Input.Cursor(Avalonia.Input.StandardCursorType.Hand),
		};

		static Button MakeHeaderButton(string text) => new Button
		{
			Content = text,
			FontSize = 11,
			Padding = new Thickness(8, 2),
			Margin = new Thickness(2, 0),
		};

		private void InitializeComponent()
		{
			// ── field instantiation ─────────────────────────────────────────
			txtrPanel  = new Panel();
			cbitem     = new ComboBox { HorizontalAlignment = HorizontalAlignment.Stretch, FontSize = 11 };
			tbflname   = new TextBox  { Background = Avalonia.Media.Brushes.White, FontSize = 11 };
			cbformats  = new ComboBox { HorizontalAlignment = HorizontalAlignment.Stretch, FontSize = 11 };
			tbwidth    = new TextBox  { Width = 52, Background = Avalonia.Media.Brushes.White, FontSize = 11 };
			tbheight   = new TextBox  { Width = 52, Background = Avalonia.Media.Brushes.White, FontSize = 11 };
			tblevel    = new TextBox  { Width = 44, Background = Avalonia.Media.Brushes.White, FontSize = 11 };
			cbmipmaps  = new ComboBox { HorizontalAlignment = HorizontalAlignment.Stretch, FontSize = 11 };
			lbimg      = new ListBox  { FontSize = 11 };
			tblifo     = new TextBox  { Background = Avalonia.Media.Brushes.White, FontSize = 11 };
			pb         = new PictureBoxCompat { Stretch = Avalonia.Media.Stretch.None };
			panel1     = new Panel();
			panel2     = new WrapperBaseControl();

			label1 = new TextBlock { Text = "Filename:",     VerticalAlignment = VerticalAlignment.Center, FontSize = 11, Margin = new Thickness(0,0,4,0) };
			label2 = new TextBlock { Text = "Format:",       VerticalAlignment = VerticalAlignment.Center, FontSize = 11, Margin = new Thickness(0,0,4,0) };
			label3 = new TextBlock { Text = "Size:",         VerticalAlignment = VerticalAlignment.Center, FontSize = 11, Margin = new Thickness(0,0,4,0) };
			label4 = new TextBlock { Text = "x",             VerticalAlignment = VerticalAlignment.Center, FontSize = 11, Margin = new Thickness(4,0) };
			label5 = new TextBlock { Text = "MipMap Level:", VerticalAlignment = VerticalAlignment.Center, FontSize = 11, Margin = new Thickness(8,0,4,0) };
			label6 = new TextBlock { Text = "LIFO Reference:", VerticalAlignment = VerticalAlignment.Center, FontSize = 11, Margin = new Thickness(0,0,4,0) };
			label7 = new TextBlock { Text = "Blocks:",       VerticalAlignment = VerticalAlignment.Center, FontSize = 11, Margin = new Thickness(0,0,4,0) };
			label8 = new TextBlock { Text = "" };

			linkLabel1 = MakeHyperlink("add");
			linkLabel2 = MakeHyperlink("fix TGI");
			linkLabel3 = MakeHyperlink("build default MipMap");
			linkLabel4 = MakeHyperlink("build filename");
			lldel      = MakeHyperlink("delete");

			btim = MakeHeaderButton("Import...");
			btex = MakeHeaderButton("Export...");
			var btcommit = MakeHeaderButton("Commit");

			// context menu on the image preview
			menuItem1 = new MenuItem { Header = "Import..." };
			menuItem2 = new MenuItem { Header = "Export Texture" };
			menuItem3 = new MenuItem { Header = "Import Alpha..." };
			menuItem4 = new MenuItem { Header = "Export Alpha..." };
			menuItem5 = new MenuItem { Header = "Import DDS..." };
			menuItem6 = new MenuItem { Header = "Update All Sizes" };
			menuItem7 = new MenuItem { Header = "-" };
			milifo    = new MenuItem { Header = "Import from LIFO" };
			mibuild   = new MenuItem { Header = "Build DXT" };
			contextMenu1 = new ContextMenu();
			contextMenu1.Items.Add(menuItem1);
			contextMenu1.Items.Add(menuItem2);
			contextMenu1.Items.Add(new MenuItem { Header = "-" });
			contextMenu1.Items.Add(menuItem3);
			contextMenu1.Items.Add(menuItem4);
			contextMenu1.Items.Add(new MenuItem { Header = "-" });
			contextMenu1.Items.Add(menuItem5);
			contextMenu1.Items.Add(menuItem6);
			contextMenu1.Items.Add(menuItem7);
			contextMenu1.Items.Add(milifo);
			contextMenu1.Items.Add(mibuild);

			// ── event wiring ────────────────────────────────────────────────
			// Use lambdas where handler signatures use base EventArgs but Avalonia
			// events supply a derived type (SelectionChangedEventArgs, etc.)
			cbitem.SelectionChanged    += (s, e) => SelectItem(s, e);
			cbmipmaps.SelectionChanged += (s, e) => SelectMipMapBlock(s, e);
			lbimg.SelectionChanged     += (s, e) => PictureSelect(s, e);
			cbformats.SelectionChanged += (s, e) => ChangeFormat(s, e);
			tbflname.TextChanged       += (s, e) => FileNameChanged(s, e);
			tblevel.LostFocus          += (s, e) => Changedlevel(s, e);
			tblifo.LostFocus           += (s, e) => SetLifo(s, e);
			btim.Click     += (s, e) => btim_Click(s, e);
			btex.Click     += (s, e) => btex_Click(s, e);
			btcommit.Click += (s, e) => btcommit_Click(s, e);
			linkLabel1.Click += (s, e) => Add(s, e);
			linkLabel2.Click += (s, e) => FixTGI(s, e);
			linkLabel3.Click += (s, e) => BuildMipMap(s, e);
			lldel.Click      += (s, e) => Delete(s, e);
			menuItem1.Click  += (s, e) => btim_Click(s, e);
			menuItem2.Click  += (s, e) => btex_Click(s, e);
			menuItem3.Click  += (s, e) => ImportAlpha(s, e);
			menuItem4.Click  += (s, e) => ExportAlpha(s, e);
			menuItem5.Click  += (s, e) => ImportDDS(s, e);
			menuItem6.Click  += (s, e) => UpdateAllSizes(s, e);
			milifo.Click     += (s, e) => ImportLifo(s, e);
			mibuild.Click    += (s, e) => BuildDXT(s, e);
			contextMenu1.Opening += ContextPopUp;
			pb.ContextMenu = contextMenu1;

			// ── layout ──────────────────────────────────────────────────────

			// Header bar
			var headerLabel = new TextBlock
			{
				Text = "TXTR Editor",
				Foreground = Avalonia.Media.Brushes.White,
				FontSize = 11,
				FontWeight = Avalonia.Media.FontWeight.SemiBold,
				VerticalAlignment = VerticalAlignment.Center,
				Margin = new Thickness(8, 0),
			};
			var headerButtons = new StackPanel
			{
				Orientation = Orientation.Horizontal,
				VerticalAlignment = VerticalAlignment.Center,
				Margin = new Thickness(4, 3),
				Spacing = 2,
			};
			headerButtons.Children.Add(btim);
			headerButtons.Children.Add(btex);
			headerButtons.Children.Add(btcommit);
			var headerContent = new DockPanel { LastChildFill = true };
			DockPanel.SetDock(headerButtons, Dock.Right);
			headerContent.Children.Add(headerButtons);
			headerContent.Children.Add(headerLabel);
			var headerBar = new Border
			{
				Background = new Avalonia.Media.LinearGradientBrush
				{
					StartPoint = new RelativePoint(0, 0, RelativeUnit.Relative),
					EndPoint   = new RelativePoint(0, 1, RelativeUnit.Relative),
					GradientStops =
					{
						new Avalonia.Media.GradientStop(Avalonia.Media.Color.FromRgb(74, 84, 100), 0.0),
						new Avalonia.Media.GradientStop(Avalonia.Media.Color.FromRgb(54, 64,  80), 1.0),
					},
				},
				MinHeight = 28,
				Child = headerContent,
			};

			// ── Left column ────────────────────────────────────────────────
			// Filename row
			var filenameRow = new Grid { Margin = new Thickness(0, 2) };
			filenameRow.ColumnDefinitions.Add(new ColumnDefinition(GridLength.Auto));
			filenameRow.ColumnDefinitions.Add(new ColumnDefinition(new GridLength(1, GridUnitType.Star)));
			Grid.SetColumn(label1, 0); filenameRow.Children.Add(label1);
			Grid.SetColumn(cbitem,  1); filenameRow.Children.Add(cbitem);

			// Filename text (second row) + fix TGI
			tbflname.Margin = new Thickness(0, 2);
			var fixTgiRow = new DockPanel { Margin = new Thickness(0, 0, 0, 4) };
			DockPanel.SetDock(linkLabel2, Dock.Right);
			fixTgiRow.Children.Add(linkLabel2);

			// Format row
			var formatRow = new Grid { Margin = new Thickness(0, 2) };
			formatRow.ColumnDefinitions.Add(new ColumnDefinition(GridLength.Auto));
			formatRow.ColumnDefinitions.Add(new ColumnDefinition(new GridLength(1, GridUnitType.Star)));
			Grid.SetColumn(label2,    0); formatRow.Children.Add(label2);
			Grid.SetColumn(cbformats, 1); formatRow.Children.Add(cbformats);

			// Size row
			var sizeRow = new StackPanel { Orientation = Orientation.Horizontal, Margin = new Thickness(0, 2), Spacing = 0 };
			sizeRow.Children.Add(label3);
			sizeRow.Children.Add(tbwidth);
			sizeRow.Children.Add(label4);
			sizeRow.Children.Add(tbheight);
			sizeRow.Children.Add(label5);
			sizeRow.Children.Add(tblevel);

			// Blocks row
			var blocksRow = new Grid { Margin = new Thickness(0, 2) };
			blocksRow.ColumnDefinitions.Add(new ColumnDefinition(GridLength.Auto));
			blocksRow.ColumnDefinitions.Add(new ColumnDefinition(new GridLength(1, GridUnitType.Star)));
			Grid.SetColumn(label7,    0); blocksRow.Children.Add(label7);
			Grid.SetColumn(cbmipmaps, 1); blocksRow.Children.Add(cbmipmaps);

			// Fields stack
			var fieldsStack = new StackPanel { Margin = new Thickness(4, 4, 4, 0) };
			fieldsStack.Children.Add(filenameRow);
			fieldsStack.Children.Add(tbflname);
			fieldsStack.Children.Add(fixTgiRow);
			fieldsStack.Children.Add(formatRow);
			fieldsStack.Children.Add(sizeRow);
			fieldsStack.Children.Add(blocksRow);

			// Links row at bottom of left column
			var linksRow = new StackPanel
			{
				Orientation = Orientation.Horizontal,
				Margin = new Thickness(4, 2),
				Spacing = 8,
			};
			linksRow.Children.Add(linkLabel3);
			linksRow.Children.Add(linkLabel1);
			linksRow.Children.Add(lldel);

			// Left DockPanel: fields on top, links on bottom, listbox fills rest
			var leftDock = new DockPanel { LastChildFill = true, Margin = new Thickness(0, 0, 4, 0) };
			DockPanel.SetDock(fieldsStack, Dock.Top);
			DockPanel.SetDock(linksRow,    Dock.Bottom);
			leftDock.Children.Add(fieldsStack);
			leftDock.Children.Add(linksRow);
			leftDock.Children.Add(lbimg);

			// ── Right column ───────────────────────────────────────────────
			// LIFO reference row at bottom
			var lifoRow = new Grid { Margin = new Thickness(0, 4, 0, 0) };
			lifoRow.ColumnDefinitions.Add(new ColumnDefinition(GridLength.Auto));
			lifoRow.ColumnDefinitions.Add(new ColumnDefinition(new GridLength(1, GridUnitType.Star)));
			Grid.SetColumn(label6, 0); lifoRow.Children.Add(label6);
			Grid.SetColumn(tblifo, 1); lifoRow.Children.Add(tblifo);

			// Image fills remaining space at actual size (scrollable); LIFO docked to bottom
			var pbScroll = new ScrollViewer
			{
				HorizontalScrollBarVisibility = Avalonia.Controls.Primitives.ScrollBarVisibility.Auto,
				VerticalScrollBarVisibility   = Avalonia.Controls.Primitives.ScrollBarVisibility.Auto,
				Content = pb,
			};
			var rightDock = new DockPanel { LastChildFill = true, Margin = new Thickness(0, 4) };
			DockPanel.SetDock(lifoRow, Dock.Bottom);
			rightDock.Children.Add(lifoRow);
			rightDock.Children.Add(pbScroll);

			// ── Main 2-column grid ─────────────────────────────────────────
			var mainGrid = new Grid { Margin = new Thickness(4) };
			mainGrid.ColumnDefinitions.Add(new ColumnDefinition(new GridLength(380)));
			mainGrid.ColumnDefinitions.Add(new ColumnDefinition(new GridLength(1, GridUnitType.Star)));
			Grid.SetColumn(leftDock,  0); mainGrid.Children.Add(leftDock);
			Grid.SetColumn(rightDock, 1); mainGrid.Children.Add(rightDock);

			// ── Outer DockPanel ────────────────────────────────────────────
			var outerDock = new DockPanel { LastChildFill = true };
			DockPanel.SetDock(headerBar, Dock.Top);
			outerDock.Children.Add(headerBar);
			outerDock.Children.Add(mainGrid);
			txtrPanel.Children.Add(outerDock);
		}
		#endregion

        internal Avalonia.Controls.Panel txtrPanel;
		internal Avalonia.Controls.ListBox lbimg;
        private SimPe.Windows.Forms.WrapperBaseControl panel2;
		private Avalonia.Controls.Panel panel1;
        private PictureBoxCompat pb;
		private Avalonia.Controls.Button btim;
		internal Avalonia.Controls.Button btex;
		private Avalonia.Controls.TextBlock label1;
		internal Avalonia.Controls.ComboBox cbmipmaps;
		internal Avalonia.Controls.ComboBox cbitem;
		private Avalonia.Controls.TextBlock label2;
		private Avalonia.Controls.TextBox tbflname;
		internal Avalonia.Controls.ComboBox cbformats;
		private Avalonia.Controls.TextBlock label3;
		private Avalonia.Controls.TextBlock label4;
		private Avalonia.Controls.TextBox tbwidth;
		private Avalonia.Controls.TextBox tbheight;
		private Avalonia.Controls.TextBlock label5;
		private Avalonia.Controls.TextBox tblifo;
		private Avalonia.Controls.TextBlock label6;
		private Avalonia.Controls.Button linkLabel2;
		internal Avalonia.Controls.Button lldel;
		private Avalonia.Controls.ContextMenu contextMenu1;
		private Avalonia.Controls.MenuItem menuItem1;
		private Avalonia.Controls.MenuItem menuItem2;
		private Avalonia.Controls.MenuItem menuItem3;
		private Avalonia.Controls.MenuItem menuItem4;
		private Avalonia.Controls.MenuItem menuItem5;
		private Avalonia.Controls.MenuItem menuItem6;
		private Avalonia.Controls.TextBlock label7;
		private Avalonia.Controls.TextBox tblevel;
		private Avalonia.Controls.TextBlock label8;
		private Avalonia.Controls.Button linkLabel1;
		private Avalonia.Controls.Button linkLabel3;
		private Avalonia.Controls.MenuItem milifo;
		private Avalonia.Controls.Button linkLabel4;
		private Avalonia.Controls.MenuItem menuItem7;
		private Avalonia.Controls.MenuItem mibuild;

		internal Txtr wrapper = null;

		private void PictureSelect(object sender, System.EventArgs e)
		{
			pb.Image = null;
			btex.IsEnabled = false;
			lldel.IsEnabled = false;
			try
			{
				lbimg.Tag = true;
				MipMap mm = (MipMap)lbimg.Items[lbimg.SelectedIndex];
				pb.Image = mm.Texture;
				if (mm.Texture==null) tblifo.Text = mm.LifoFile;
				else tblifo.Text = "";

				btex.IsEnabled = (pb.Image!=null);
				lldel.IsEnabled = true;
			}
			catch (Exception) {}
			finally
			{
				lbimg.Tag = null;
			}
		}

		private void btcommit_Click(object sender, System.EventArgs e)
		{
			try
			{
				Txtr wrp = (Txtr)wrapper;
				wrp.SynchronizeUserData();
				SimPe.Message.Show(Localization.Manager.GetString("commited"));
			}
			catch (Exception ex)
			{
				Helper.ExceptionMessage(Localization.Manager.GetString("errwritingfile"), ex);
			}
		}

		private async void btex_Click(object sender, System.EventArgs e)
		{
			if (pb.Image == null) return;

			var topLevel = TopLevel.GetTopLevel(txtrPanel);
			if (topLevel == null) return;
			var file = await topLevel.StorageProvider.SaveFilePickerAsync(new FilePickerSaveOptions
			{
				Title = "Export Texture",
				SuggestedFileName = tbflname.Text + "_" + pb.Image.Width + "x" + pb.Image.Height + ".png",
				FileTypeChoices = new[] { new FilePickerFileType("PNG Image") { Patterns = new[] { "*.png" } } }
			});
			if (file != null)
			{
				try
				{
					string path = file.Path.LocalPath;
					using var image = SKImage.FromBitmap(pb.Image);
					var fmt = path.ToLower().EndsWith(".png") ? SKEncodedImageFormat.Png : SKEncodedImageFormat.Jpeg;
					using var encoded = image.Encode(fmt, 100);
					using var fs = System.IO.File.OpenWrite(path);
					encoded.SaveTo(fs);
				}
				catch (Exception ex)
				{
					Helper.ExceptionMessage(Localization.Manager.GetString("errwritingfile"), ex);
				}
			}
		}

		private async void btim_Click(object sender, System.EventArgs e)
		{
			if (lbimg.SelectedIndex < 0) return;

			var topLevel = TopLevel.GetTopLevel(txtrPanel);
			if (topLevel == null) return;
			var files = await topLevel.StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions
			{
				Title = "Import Texture",
				AllowMultiple = false,
				FileTypeFilter = new[] { new FilePickerFileType("Image Files") { Patterns = new[] { "*.png", "*.bmp", "*.gif", "*.jpg", "*.jpeg", "*.tif", "*.tiff" } } }
			});
			if (files.Count > 0)
			{
				try
				{
					ImageData id = (ImageData)cbitem.Items[cbitem.SelectedIndex];
					using var s = System.IO.File.OpenRead(files[0].Path.LocalPath);
					SKBitmap img = Helper.LoadSKBitmap(s);
					img = this.CropImage(id, img);
					if (img == null) return;

					lbimg.Tag = true;
					MipMap mm = (MipMap)lbimg.Items[lbimg.SelectedIndex];
					mm.LifoFile = "";
					mm.Texture = img;
					pb.Image = img;
					lbimg.Items[lbimg.SelectedIndex] = mm;
				}
				catch (Exception ex)
				{
					Helper.ExceptionMessage(Localization.Manager.GetString("erropenfile"), ex);
				}
				finally
				{
					lbimg.Tag = null;
				}
			}
		}

		private void SelectItem(object sender, System.EventArgs e)
		{
			if (cbitem.Tag!=null) return;
			this.cbmipmaps.Items.Clear();
			this.lbimg.Items.Clear();
			if (cbitem.SelectedIndex<0) return;
			try
			{
				cbitem.Tag = true;
				ImageData selecteditem = (ImageData)cbitem.Items[cbitem.SelectedIndex];
				foreach (MipMapBlock mmp in selecteditem.MipMapBlocks)
				{
					this.cbmipmaps.Items.Add(mmp);
				}

				if (cbmipmaps.Items.Count>0) cbmipmaps.SelectedIndex = 0;
				this.tbflname.Text = selecteditem.NameResource.FileName;
				this.tbwidth.Text = selecteditem.TextureSize.Width.ToString();
				this.tbheight.Text = selecteditem.TextureSize.Height.ToString();
				this.tblevel.Text = selecteditem.MipMapLevels.ToString();

				this.cbformats.SelectedIndex = 0;
				for (int i=0; i<cbformats.Items.Count; i++)
				{
					ImageLoader.TxtrFormats f = (ImageLoader.TxtrFormats)cbformats.Items[i];
					if (f==selecteditem.Format)
					{
						cbformats.SelectedIndex = i;
						break;
					}
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

		private void FileNameChanged(object sender, System.EventArgs e)
		{
			if (cbitem.Tag!=null) return;
			if (cbitem.SelectedIndex<0) return;
			try
			{
				cbitem.Tag = true;
				ImageData selecteditem = (ImageData)cbitem.Items[cbitem.SelectedIndex];
				selecteditem.NameResource.FileName = tbflname.Text.Trim();
				if (tbflname.Text.ToLower().EndsWith("_txtr"))
				{
					selecteditem.FileNameRepeat = selecteditem.NameResource.FileName.Substring(0, selecteditem.NameResource.FileName.Length - 5);
				}
				cbitem.Items[cbitem.SelectedIndex] = selecteditem;
				// cbitem.Text not available in Avalonia ComboBox — display updates via selected item
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

		private void SelectMipMapBlock(object sender, System.EventArgs e)
		{
			if (cbmipmaps.Tag!=null) return;
			this.lbimg.Items.Clear();
			if (cbmipmaps.SelectedIndex<0) return;
			try
			{
				cbmipmaps.Tag = true;
				MipMapBlock mmp = (MipMapBlock)cbmipmaps.Items[cbmipmaps.SelectedIndex];
				int minindex = -1;
				for (int i=0; i<mmp.MipMaps.Length; i++)
				{
					MipMap mm = mmp.MipMaps[i];
					mm.ReloadTexture();
					this.lbimg.Items.Add(mm);
					if (mm.Texture != null) minindex = i;
				}

				lbimg.SelectedIndex = minindex;
			}
			catch (Exception ex)
			{
				Helper.ExceptionMessage(Localization.Manager.GetString("erropenfile"), ex);
			}
			finally
			{
				cbmipmaps.Tag = null;
			}
		}

		private void ChangeFormat(object sender, System.EventArgs e)
		{
			if (cbitem.Tag!=null) return;
			if (cbitem.SelectedIndex<0) return;
			if (cbformats.SelectedIndex<1) return;
			try
			{
				cbitem.Tag = true;
				ImageData selecteditem = (ImageData)cbitem.Items[cbitem.SelectedIndex];
				selecteditem.Format = (ImageLoader.TxtrFormats)cbformats.Items[cbformats.SelectedIndex];

				//make sure images are resaved when the Format was changed!
				foreach (MipMapBlock mmp in selecteditem.MipMapBlocks)
				{
					foreach (MipMap mm in mmp.MipMaps)
					{
						mm.Data = null;
					}
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

		private void SetLifo(object sender, System.EventArgs e)
		{
			if (lbimg.Tag !=null) return;
			try
			{
				MipMap mm = (MipMap)lbimg.Items[lbimg.SelectedIndex];
				pb.Image = null;
				mm.Texture = null;
				mm.LifoFile = tblifo.Text;
				lbimg.Items[lbimg.SelectedIndex] = mm;
			}
			catch (Exception ex)
			{
				Helper.ExceptionMessage(Localization.Manager.GetString("erropenfile"), ex);
			}
		}

		private void Delete(object sender, EventArgs e)
		{
			if (lbimg.SelectedIndex<0) return;
			lbimg.Items.Remove(lbimg.Items[lbimg.SelectedIndex]);
			UpdateMimMaps();
		}

		private void Add(object sender, EventArgs e)
		{
			MipMap mm = new MipMap(SelectedImageData());
			mm.LifoFile = null;
			mm.Texture = new SKBitmap(512, 256);
			lbimg.Items.Add(mm);
			UpdateMimMaps();
		}

		protected ImageData SelectedImageData()
		{
			//add a MipMapBlock if it doesnt already exist
			ImageData id = null;
			if (cbitem.SelectedIndex<0)
			{
				Txtr wrp = (Txtr)wrapper;
				id = new ImageData(wrp);
				id.NameResource.FileName = "Unknown";

				IRcolBlock[] irc = new IRcolBlock[wrp.Blocks.Length+1];
				wrp.Blocks.CopyTo(irc, 0);
				irc[irc.Length-1] = id;
				wrp.Blocks = irc;
				cbitem.Items.Add(id);
				cbitem.SelectedIndex = cbitem.Items.Count-1;
			}
			else
			{
				id = (ImageData)cbitem.Items[cbitem.SelectedIndex];
			}

			return id;
		}

		protected MipMapBlock SelectedMipMapBlock(ImageData id)
		{
			//add a MipMapBlock if it doesnt already exist
			if (this.cbmipmaps.SelectedIndex<0)
			{
				MipMapBlock[] mmp = new MipMapBlock[id.MipMapBlocks.Length+1];
				id.MipMapBlocks.CopyTo(mmp, 0);
				mmp[mmp.Length-1] = new MipMapBlock(id);
				id.MipMapBlocks = mmp;
				cbmipmaps.Items.Add(mmp[mmp.Length-1]);
				cbmipmaps.SelectedIndex = cbmipmaps.Items.Count-1;

				return mmp[mmp.Length-1];
			}
			else
			{
				object o = cbmipmaps.SelectedItem;
				if (o is MipMapBlock) return o as MipMapBlock;

				try
				{
					MipMapBlock[] mmb = o as MipMapBlock[];
					return mmb[mmb.Length-1];
				}
				catch
				{
					return new MipMapBlock(id);
				}
			}
		}

		protected void UpdateMimMaps()
		{
			ImageData id = SelectedImageData();
			MipMapBlock mmp = SelectedMipMapBlock(id);

			MipMap[] mm = new MipMap[lbimg.Items.Count];
			for (int i=0; i<mm.Length; i++)
			{
				mm[i] = (MipMap)lbimg.Items[i];
			}
			mmp.MipMaps = mm;
			id.MipMapLevels = (uint)mm.Length;
			tblevel.Text = id.MipMapLevels.ToString();
		}

		private void UpdateAllSizes(object sender, System.EventArgs e)
		{
			try
			{
				lbimg.Tag = true;
				MipMap map = null;
				GdiSize sz = new GdiSize(0, 0);

				//Find biggest Texture
				for (int i=0; i< lbimg.Items.Count; i++)
				{
					MipMap mm = (MipMap)lbimg.Items[i];

					if (mm.Texture!=null)
					{
						if (mm.Texture.Width > sz.Width)
						{
							sz = new GdiSize(mm.Texture.Width, mm.Texture.Height);
							map = mm;
						}
					}
				} // for i

				if (map==null) return;

				//create a Scaled Version for each testure
				for (int i=0; i< lbimg.Items.Count; i++)
				{
					MipMap mm = (MipMap)lbimg.Items[i];

					if (mm.Texture!=null)
					{
						//don't change the original Picture
						if (mm != map)
						{
							SKBitmap bm = new SKBitmap(mm.Texture.Width, mm.Texture.Height);
							using var gr = new SKCanvas(bm);
							var destRect = new SKRect(0, 0, mm.Texture.Width, mm.Texture.Height);
							using var scalePaint = new SKPaint { FilterQuality = SKFilterQuality.High };
							gr.DrawBitmap(map.Texture, destRect, scalePaint);
							mm.Texture = bm;
						}
					}
				} // for i
			}
			catch (Exception ex)
			{
				Helper.ExceptionMessage(Localization.Manager.GetString("errconvert"), ex);
			}
		}

		protected SKBitmap GetAlpha(SKBitmap img)
		{
			SKBitmap bm = new SKBitmap(pb.Image.Width, pb.Image.Height);

			for (int y=0; y<bm.Height; y++)
			{
				for (int x=0; x<bm.Width; x++)
				{
					byte a = img.GetPixel(x, y).Alpha;
					bm.SetPixel(x, y, new SKColor(a, a, a));
				} // for x
			} //for y

			return bm;
		}

		protected SKBitmap ChangeAlpha(SKBitmap img, SKBitmap alpha)
		{
			SKBitmap bm = new SKBitmap(pb.Image.Width, pb.Image.Height, SKColorType.Bgra8888, SKAlphaType.Unpremul);

			for (int y=0; y<bm.Height; y++)
			{
				for (int x=0; x<bm.Width; x++)
				{
					byte a = alpha.GetPixel(x, y).Red;
					SKColor cl = img.GetPixel(x, y);
					bm.SetPixel(x, y, new SKColor(cl.Red, cl.Green, cl.Blue, a));
				} // for x
			} //for y

			return bm;
		}

		protected SKBitmap CropImage(ImageData id, SKBitmap img)
		{
			double ratio = (double)id.TextureSize.Width / (double)id.TextureSize.Height;
			double newratio = (double)img.Width / (double)img.Height;

			if (ratio != newratio)
			{
				if (SimPe.Message.Show("The File you want to import does not have the correct aspect Ratio!\n\nDo you want SimPe to crop the Image?", "Warning", MessageBoxButtons.YesNo) == SimPe.DialogResult.Yes)
				{
					int w = Convert.ToInt32(img.Height * ratio);
					int h = img.Height;
					if (w>img.Width)
					{
						w = img.Width;
						h = Convert.ToInt32(img.Width / ratio);
					}

					SKBitmap img2 = new SKBitmap(w, h);
					using var canvas = new SKCanvas(img2);
					canvas.DrawBitmap(img, 0, 0);
					img = img2;
				}
				else
				{
					return null;
				}
			}

			return img;
		}

		private async void ExportAlpha(object sender, System.EventArgs e)
		{
			if (pb.Image == null) return;

			var topLevel = TopLevel.GetTopLevel(txtrPanel);
			if (topLevel == null) return;
			var file = await topLevel.StorageProvider.SaveFilePickerAsync(new FilePickerSaveOptions
			{
				Title = "Export Alpha Channel",
				SuggestedFileName = tbflname.Text + "_alpha_" + pb.Image.Width + "x" + pb.Image.Height + ".png",
				FileTypeChoices = new[] { new FilePickerFileType("PNG Image") { Patterns = new[] { "*.png" } } }
			});
			if (file != null)
			{
				try
				{
					string path = file.Path.LocalPath;
					SKBitmap bm = GetAlpha(pb.Image);
					using var image = SKImage.FromBitmap(bm);
					var fmt = path.ToLower().EndsWith(".png") ? SKEncodedImageFormat.Png : SKEncodedImageFormat.Jpeg;
					using var encoded = image.Encode(fmt, 100);
					using var fs = System.IO.File.OpenWrite(path);
					encoded.SaveTo(fs);
				}
				catch (Exception ex)
				{
					Helper.ExceptionMessage(Localization.Manager.GetString("errwritingfile"), ex);
				}
			}
		}

		private async void ImportAlpha(object sender, System.EventArgs e)
		{
			if (lbimg.SelectedIndex<0) return;

			var topLevel = TopLevel.GetTopLevel(txtrPanel);
			if (topLevel == null) return;
			var files = await topLevel.StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions
			{
				Title = "Import Alpha Channel",
				AllowMultiple = false,
				FileTypeFilter = new[] { new FilePickerFileType("Image Files") { Patterns = new[] { "*.png", "*.bmp", "*.gif", "*.jpg" } } }
			});
			if (files.Count > 0)
			{
				try
				{
					ImageData id = (ImageData)cbitem.Items[cbitem.SelectedIndex];
					System.IO.Stream s = System.IO.File.OpenRead(files[0].Path.LocalPath);
					SKBitmap img = Helper.LoadSKBitmap(s);
					s.Close();
					s.Dispose();
					s = null;

					img = this.CropImage(id, img);
					if (img==null) return;

					lbimg.Tag = true;
					MipMap mm = (MipMap)lbimg.Items[lbimg.SelectedIndex];
					mm.LifoFile = "";
					mm.Texture = this.ChangeAlpha(mm.Texture, img);
					pb.Image = mm.Texture;
					lbimg.Items[lbimg.SelectedIndex] = mm;
				}
				catch (Exception ex)
				{
					Helper.ExceptionMessage(Localization.Manager.GetString("erropenfile"), ex);
				}
				finally
				{
					lbimg.Tag = null;
				}
			}
		}

		private void Changedlevel(object sender, System.EventArgs e)
		{
			if (cbitem.Tag!=null) return;
			if (cbitem.SelectedIndex<0) return;
			try
			{
				cbitem.Tag = true;
				ImageData selecteditem = (ImageData)cbitem.Items[cbitem.SelectedIndex];
				selecteditem.MipMapLevels = Convert.ToUInt32(tblevel.Text);
				cbitem.Items[cbitem.SelectedIndex] = selecteditem;
				// cbitem.Text not available in Avalonia ComboBox — display updates via selected item
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

		private void BuildFilename(object sender, EventArgs e)
		{
			string fl = Hashes.StripHashFromName(this.tbflname.Text);
			this.tbflname.Text = Hashes.AssembleHashedFileName(wrapper.Package.FileGroupHash, fl);
		}

		private void FixTGI(object sender, EventArgs e)
		{
			string fl = Hashes.StripHashFromName(this.tbflname.Text);
			wrapper.FileDescriptor.Instance = Hashes.InstanceHash(fl);
			wrapper.FileDescriptor.SubType = Hashes.SubTypeHash(fl);
		}

		protected Interfaces.Files.IPackedFileDescriptor GetLocalLifo(MipMap mm)
		{
			if (mm.Texture==null)
			{
				uint st = Hashes.SubTypeHash(mm.LifoFile);
				uint inst = Hashes.InstanceHash(mm.LifoFile);

				Interfaces.Files.IPackedFileDescriptor pfd = wrapper.Package.FindFile(0xED534136, st, wrapper.FileDescriptor.Group, inst);
				return pfd;
			}

			return null;
		}

		private void ContextPopUp(object sender, System.ComponentModel.CancelEventArgs e)
		{
			milifo.IsEnabled = false;
			mibuild.IsEnabled = true;
			if (lbimg.SelectedIndex<0) return;
			try
			{
				if (lbimg.SelectedIndex>=0)
				{
					MipMap mm = (MipMap)lbimg.Items[lbimg.SelectedIndex];
					Interfaces.Files.IPackedFileDescriptor pfd = GetLocalLifo(mm);
					milifo.IsEnabled = (pfd != null);
				}
				else
				{
					milifo.IsEnabled = false;
				}
				mibuild.IsEnabled = true;
			}
			catch (Exception ex)
			{
				Helper.ExceptionMessage("", ex);
			}
		}

		private void ImportLifo(object sender, System.EventArgs e)
		{
			if (lbimg.SelectedIndex<0) return;
			try
			{
				cbitem.Tag = true;
				MipMap mm = (MipMap)lbimg.Items[lbimg.SelectedIndex];
				Interfaces.Files.IPackedFileDescriptor pfd = GetLocalLifo(mm);
				Lifo lifo = new Lifo(null, false);
				lifo.ProcessData(pfd, wrapper.Package);
				mm.Texture = null;
				mm.Data = ((LevelInfo)lifo.Blocks[0]).Data;
				pb.Image = mm.Texture;
				lbimg.Items[lbimg.SelectedIndex] = mm;
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

		private async void BuildMipMap(object sender, EventArgs e)
		{
			try
			{
				GdiSize sz = await SimPe.Plugin.ImageSize.Execute(SelectedImageData().TextureSize);
				cbitem.Tag = true;
				lbimg.Items.Clear();
				int wd = 1;
				int hg = 1;

				int levels = Convert.ToInt32(tblevel.Text);
				for (int i=0; i<levels; i++)
				{
					MipMap mm = new MipMap(SelectedImageData());
					mm.Texture = new SKBitmap(wd, hg);

					if (i==levels-1)
					{
						SelectedImageData().TextureSize = new GdiSize(wd, hg);
					}

					if ((wd==hg) && (wd==1))
					{
						wd =  Math.Max(1, (sz.Width / Math.Max(1, sz.Height)));
						hg =  Math.Max(1, (sz.Height / Math.Max(1, sz.Width)));

						if ((wd==hg) && (wd==1))
						{
							wd *= 2; hg *= 2;
						}
					}
					else
					{
						wd *= 2; hg *= 2;
					}

					lbimg.Items.Add(mm);
				}

				UpdateMimMaps();
				if (cbitem.Tag==null)
				{
					tbwidth.Text = SelectedImageData().TextureSize.Width.ToString();
					tbheight.Text = SelectedImageData().TextureSize.Height.ToString();
					lbimg.SelectedIndex = lbimg.Items.Count-1;
				}
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

		void LoadDDS(DDSData[] data)
		{
			if (data==null) return;
			if (data.Length>0)
			{
				try
				{
					cbitem.Tag = true;
					ImageData id = SelectedImageData();

					id.TextureSize = data[0].ParentSize;
					id.Format = data[0].Format;
					id.MipMapLevels = (uint)data.Length;

					this.lbimg.Items.Clear();
					for (int i=data.Length-1; i>=0; i--)
					{
						DDSData item = data[i];
						MipMap mm = new MipMap(id);
						mm.Texture = item.Texture;
						mm.Data = item.Data;

						lbimg.Items.Add(mm);
					}

					tbwidth.Text = id.TextureSize.Width.ToString();
					tbheight.Text = id.TextureSize.Height.ToString();

					this.cbformats.SelectedIndex = 0;
					for (int i=0; i<cbformats.Items.Count; i++)
					{
						if ((ImageLoader.TxtrFormats)cbformats.Items[i]==id.Format)
						{
							cbformats.SelectedIndex = i;
							break;
						}
					}
				}
				finally
				{
					cbitem.Tag = null;
				}
			}

			UpdateMimMaps();
			lbimg.SelectedIndex = lbimg.Items.Count-1;
		}

		private async void ImportDDS(object sender, System.EventArgs e)
		{
			var topLevel = TopLevel.GetTopLevel(txtrPanel);
			if (topLevel == null) return;
			var files = await topLevel.StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions
			{
				Title = "Import DDS Texture",
				AllowMultiple = false,
				FileTypeFilter = new[] { new FilePickerFileType("DDS Texture") { Patterns = new[] { "*.dds" } } }
			});
			if (files.Count > 0)
			{
				try
				{
					cbitem.Tag = true;
					ImageData id = SelectedImageData();
					DDSData[] data = ImageLoader.ParesDDS(files[0].Path.LocalPath);

					LoadDDS(data);
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
		}

		private void ChangedSize(object sender, System.EventArgs e)
		{
			if (cbitem.Tag!=null) return;
			if (cbitem.SelectedIndex<0) return;
			try
			{
				cbitem.Tag = true;
				ImageData id = (ImageData)cbitem.Items[cbitem.SelectedIndex];
				id.TextureSize = new GdiSize(Convert.ToInt32(tbwidth.Text), Convert.ToInt32(tbheight.Text));

				BuildMipMap(null, null);
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

		private async void BuildDXT(object sender, System.EventArgs e)
		{
			ImageData id = SelectedImageData();
			DDSData[] dds = await DDSTool.Execute(
				Convert.ToInt32(this.tblevel.Text), id.TextureSize, id.Format);
			if (dds != null && dds.Length > 0)
				LoadDDS(dds);
			id.Refresh();
		}
	}
}
