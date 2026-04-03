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
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Platform.Storage;
using SimPe.Interfaces.Plugin;
using SimPe.Scenegraph.Compat;
using SimPe.Interfaces.Scenegraph;
using GdiImage = System.Drawing.Image;
using SkiaSharp;

namespace SimPe.Plugin
{
	/// <summary>
	/// Summary description for LifoForm.
	/// </summary>
	public class LifoForm : Avalonia.Controls.Window
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		public LifoForm()
		{
            InitializeComponent();
            if (Helper.XmlRegistry.UseBigIcons)
            {
                if (tbflname != null)
                {
                    tbflname.FontSize = 14;
                }
            }
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void OnClosed(EventArgs e)
		{
			base.OnClosed(e);
		}

		#region Avalonia AXAML layout placeholder
		private void InitializeComponent()
		{
			// TODO: Avalonia AXAML layout
			LifoPanel = new Avalonia.Controls.Panel();
			panel2 = new Avalonia.Controls.Panel();
			panel1 = new Avalonia.Controls.Panel();
			pb = new PictureBoxCompat();
			btim = new Avalonia.Controls.Button();
			btex = new Avalonia.Controls.Button();
			cbitem = new Avalonia.Controls.ComboBox();
			label2 = new Avalonia.Controls.TextBlock();
			tbflname = new Avalonia.Controls.TextBox();
			cbformats = new Avalonia.Controls.ComboBox();
			label3 = new Avalonia.Controls.TextBlock();
			label4 = new Avalonia.Controls.TextBlock();
			tbwidth = new Avalonia.Controls.TextBox();
			tbheight = new Avalonia.Controls.TextBox();
			label5 = new Avalonia.Controls.TextBlock();
			tbz = new Avalonia.Controls.TextBox();
			label1 = new Avalonia.Controls.TextBlock();
			contextMenu1 = new Avalonia.Controls.ContextMenu();
			menuItem1 = new Avalonia.Controls.MenuItem();
			menuItem4 = new Avalonia.Controls.MenuItem();
			menuItem2 = new Avalonia.Controls.MenuItem();
			menuItem5 = new Avalonia.Controls.MenuItem();
			linkLabel1 = new Avalonia.Controls.TextBlock();
			linkLabel2 = new Avalonia.Controls.TextBlock();
			menuItem3 = new Avalonia.Controls.MenuItem();
			menuItem6 = new Avalonia.Controls.MenuItem();
			menuItem7 = new Avalonia.Controls.MenuItem();
		}
		#endregion

        internal Avalonia.Controls.Panel LifoPanel;
        private Avalonia.Controls.Panel panel2;
		private Avalonia.Controls.Panel panel1;
        private PictureBoxCompat pb;
		private Avalonia.Controls.Button btim;
		internal Avalonia.Controls.Button btex;
		internal Avalonia.Controls.ComboBox cbitem;
		private Avalonia.Controls.TextBlock label2;
		private Avalonia.Controls.TextBox tbflname;
		internal Avalonia.Controls.ComboBox cbformats;
		private Avalonia.Controls.TextBlock label3;
		private Avalonia.Controls.TextBlock label4;
		private Avalonia.Controls.TextBox tbwidth;
		private Avalonia.Controls.TextBox tbheight;
		private Avalonia.Controls.TextBlock label5;
		private Avalonia.Controls.TextBox tbz;
		private Avalonia.Controls.TextBlock label1;
		private Avalonia.Controls.ContextMenu contextMenu1;
		private Avalonia.Controls.MenuItem menuItem1;
		private Avalonia.Controls.MenuItem menuItem4;
		private Avalonia.Controls.MenuItem menuItem2;
		private Avalonia.Controls.MenuItem menuItem5;
		private Avalonia.Controls.TextBlock linkLabel1;
		private Avalonia.Controls.TextBlock linkLabel2;
		private Avalonia.Controls.MenuItem menuItem3;
		private Avalonia.Controls.MenuItem menuItem6;
		private Avalonia.Controls.MenuItem menuItem7;

		internal Lifo wrapper = null;

		private void btcommit_Click(object sender, System.EventArgs e)
		{
			try
			{
				Lifo wrp = (Lifo)wrapper;
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

			var topLevel = TopLevel.GetTopLevel(this);
			if (topLevel == null) return;
			var file = await topLevel.StorageProvider.SaveFilePickerAsync(new FilePickerSaveOptions
			{
				Title = "Export Texture",
				SuggestedFileName = tbflname.Text + "_" + tbwidth.Text + "x" + tbheight.Text + ".png",
				FileTypeChoices = new[] { new FilePickerFileType("PNG Image") { Patterns = new[] { "*.png" } } }
			});
			if (file != null)
			{
				try
				{
					string savePath = file.Path.LocalPath;
					using var skImage = SKImage.FromBitmap(pb.Image);
					using var encoded = skImage.Encode(SKEncodedImageFormat.Png, 100);
					using var fs = System.IO.File.OpenWrite(savePath);
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
			var topLevel = TopLevel.GetTopLevel(this);
			if (topLevel == null) return;
			var files = await topLevel.StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions
			{
				Title = "Import Texture",
				AllowMultiple = false,
				FileTypeFilter = new[] { new FilePickerFileType("Image Files") { Patterns = new[] { "*.png", "*.bmp", "*.gif", "*.jpg", "*.jpeg" } } }
			});
			if (files.Count > 0)
			{
				try
				{
					LevelInfo id = (LevelInfo)cbitem.Items[cbitem.SelectedIndex];
					SKBitmap img = Helper.LoadSKBitmap(System.IO.File.OpenRead(files[0].Path.LocalPath));
					img = this.CropImage(id, img);
					if (img==null) return;

					id.Texture = img;
					pb.Image = img;
				}
				catch (Exception ex)
				{
					Helper.ExceptionMessage(Localization.Manager.GetString("erropenfile"), ex);
				}
			}
		}

		private void SelectItem(object sender, System.EventArgs e)
		{
			if (cbitem.Tag!=null) return;
			if (cbitem.SelectedIndex<0) return;
			try
			{
				cbitem.Tag = true;
				LevelInfo selecteditem = (LevelInfo)cbitem.Items[cbitem.SelectedIndex];

				this.tbflname.Text = selecteditem.NameResource.FileName;
				this.tbwidth.Text = selecteditem.TextureSize.Width.ToString();
				this.tbheight.Text = selecteditem.TextureSize.Height.ToString();
				this.tbz.Text = selecteditem.ZLevel.ToString();

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

				pb.Image = selecteditem.Texture;
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
				LevelInfo selecteditem = (LevelInfo)cbitem.Items[cbitem.SelectedIndex];
				selecteditem.NameResource.FileName = tbflname.Text;
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

		private void ChangeFormat(object sender, System.EventArgs e)
		{
			if (cbitem.Tag!=null) return;
			if (cbitem.SelectedIndex<0) return;
			if (cbformats.SelectedIndex<1) return;
			try
			{
				cbitem.Tag = true;
				LevelInfo selecteditem = (LevelInfo)cbitem.Items[cbitem.SelectedIndex];
				selecteditem.Format = (ImageLoader.TxtrFormats)cbformats.Items[cbformats.SelectedIndex];
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


		protected LevelInfo SelectedLevelInfo()
		{
			//add a MipMapBlock if it doesnt already exist
			LevelInfo li = null;
			if (cbitem.SelectedIndex<0)
			{
				Lifo wrp = (Lifo)wrapper;
				li = new LevelInfo(wrp);
				li.NameResource.FileName = "Unknown";

				IRcolBlock[] irc = new IRcolBlock[wrp.Blocks.Length+1];
				wrp.Blocks.CopyTo(irc, 0);
				irc[irc.Length-1] = li;
				wrp.Blocks = irc;
				cbitem.Items.Add(li);
				cbitem.SelectedIndex = cbitem.Items.Count-1;
			}
			else
			{
				li = (LevelInfo)cbitem.Items[cbitem.SelectedIndex];
			}

			return li;
		}

		private void ChangeZLevel(object sender, System.EventArgs e)
		{
			try
			{
				LevelInfo li = this.SelectedLevelInfo();
				li.ZLevel = Convert.ToInt32(this.tbz.Text);
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

		protected SKBitmap CropImage(LevelInfo id, SKBitmap img)
		{
			double ratio = (double)id.TextureSize.Width / (double)id.TextureSize.Height;
			double newratio = (double)img.Width / (double)img.Height;

			if (ratio != newratio)
			{
				if (SimPe.Message.Show("The File you want to import does not have the correct aspect Ration!\n\nDo you want SimPe to crop the Image?", "Warning", MessageBoxButtons.YesNo) == SimPe.DialogResult.Yes)
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

			var topLevel = TopLevel.GetTopLevel(this);
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
					SKBitmap bm = GetAlpha(pb.Image);
					using var skImg = SKImage.FromBitmap(bm);
					using var encoded = skImg.Encode(SKEncodedImageFormat.Png, 100);
					using var saveFs = System.IO.File.OpenWrite(file.Path.LocalPath);
					encoded.SaveTo(saveFs);
				}
				catch (Exception ex)
				{
					Helper.ExceptionMessage(Localization.Manager.GetString("errwritingfile"), ex);
				}
			}
		}

		private async void ImportAlpha(object sender, System.EventArgs e)
		{
			var topLevel = TopLevel.GetTopLevel(this);
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
					LevelInfo id = (LevelInfo)cbitem.Items[cbitem.SelectedIndex];
					SKBitmap img = Helper.LoadSKBitmap(System.IO.File.OpenRead(files[0].Path.LocalPath));
					img = this.CropImage(id, img);
					if (img==null) return;

					id.Texture = this.ChangeAlpha(id.Texture, img);
					pb.Image = id.Texture;
				}
				catch (Exception ex)
				{
					Helper.ExceptionMessage(Localization.Manager.GetString("erropenfile"), ex);
				}
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

		private async void BuildDXT(object sender, System.EventArgs e)
		{
			LevelInfo id = SelectedImageData();
			DDSData[] dds = await DDSTool.Execute(1, id.TextureSize, id.Format);
			if (dds != null && dds.Length > 0)
				LoadDDS(dds);
		}


		private async void ImportDDS(object sender, System.EventArgs e)
		{
			var topLevel = TopLevel.GetTopLevel(this);
			if (topLevel == null) return;
			var files = await topLevel.StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions
			{
				Title = "Import DDS Texture",
				AllowMultiple = false,
				FileTypeFilter = new[] { new FilePickerFileType("NVIDIA DDS File") { Patterns = new[] { "*.dds" } } }
			});
			if (files.Count > 0)
			{
				try
				{
					cbitem.Tag = true;
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


		void LoadDDS(DDSData[] data)
		{
			if (data == null) return;
			if (data.Length > 0)
			{
				try
				{
					cbitem.Tag = true;

					LevelInfo id = SelectedImageData();
					id.Format = data[0].Format;
					id.Data = data[0].Data;

					pb.Image = data[0].Texture;

					tbwidth.Text = id.TextureSize.Width.ToString();
					tbheight.Text = id.TextureSize.Height.ToString();

					this.cbformats.SelectedIndex = 0;
					for (int i = 0; i < cbformats.Items.Count; i++)
					{
						if ((ImageLoader.TxtrFormats)cbformats.Items[i] == id.Format)
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
		}

		protected LevelInfo SelectedImageData()
		{
			//add a MipMapBlock if it doesnt already exist
			LevelInfo id = null;
			if (cbitem.SelectedIndex < 0)
			{
				Lifo wrp = (Lifo)wrapper;
				id = new LevelInfo(wrp);
				id.NameResource.FileName = "Unknown";
				id.Format = (ImageLoader.TxtrFormats)cbformats.SelectedItem;

				IRcolBlock[] irc = new IRcolBlock[wrp.Blocks.Length + 1];
				wrp.Blocks.CopyTo(irc, 0);
				irc[irc.Length - 1] = id;
				wrp.Blocks = irc;
				cbitem.Items.Add(id);
				cbitem.SelectedIndex = cbitem.Items.Count - 1;
			}
			else
			{
				id = (LevelInfo)cbitem.Items[cbitem.SelectedIndex];
			}

			return id;
		}

	}
}
