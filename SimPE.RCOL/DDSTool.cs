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
using System.Diagnostics;
using Pfim;
using Avalonia.Controls;
using BCnEncoder.Encoder;
using BCnEncoder.Shared;

namespace SimPe.Plugin
{
	/// <summary>
	/// Summary description for DDSTool.
	/// Avalonia UserControl replacement (was WinForms Form).
	/// </summary>
	public class DDSTool : Avalonia.Controls.UserControl
	{
		private Avalonia.Controls.Button linkLabel1;
		private Avalonia.Controls.Image pb;
		private Avalonia.Controls.ListBox cbfilter;
		private Avalonia.Controls.TextBox tblevel;
		private Avalonia.Controls.TextBox tbwidth;
		private Avalonia.Controls.TextBox tbheight;
		private Avalonia.Controls.ComboBox cbformat;
		private Avalonia.Controls.ComboBox cbsharpen;
		private Avalonia.Controls.Button button1;

		public DDSTool()
		{
			BuildLayout();

			cbformat.Items.Clear();
			cbformat.Items.Add(ImageLoader.TxtrFormats.DXT1Format);
			cbformat.Items.Add(ImageLoader.TxtrFormats.DXT3Format);
			cbformat.Items.Add(ImageLoader.TxtrFormats.DXT5Format);
		}

		private void BuildLayout()
		{
			// Image preview (was PictureBox / Ambertation ImagePanel)
			pb = new Avalonia.Controls.Image { Width = 128, Height = 128, Stretch = Avalonia.Media.Stretch.Uniform };

			// "open Image..." button (was LinkLabel)
			linkLabel1 = new Avalonia.Controls.Button { Content = "open Image..." };
			linkLabel1.Click += linkLabel1_Click;

			// Settings group controls
			var label1 = new Avalonia.Controls.TextBlock { Text = "Levels:" };
			tblevel = new Avalonia.Controls.TextBox { Text = "0" };

			var label2 = new Avalonia.Controls.TextBlock { Text = "Size:" };
			tbwidth  = new Avalonia.Controls.TextBox { Text = "0", IsReadOnly = true };
			var labelX = new Avalonia.Controls.TextBlock { Text = "x" };
			tbheight = new Avalonia.Controls.TextBox { Text = "0", IsReadOnly = true };

			var label3 = new Avalonia.Controls.TextBlock { Text = "Format:" };
			cbformat = new Avalonia.Controls.ComboBox();

			var label4 = new Avalonia.Controls.TextBlock { Text = "Sharpen:" };
			cbsharpen = new Avalonia.Controls.ComboBox();
			cbsharpen.Items.Add("None");
			cbsharpen.Items.Add("Negative");
			cbsharpen.Items.Add("Lighter");
			cbsharpen.Items.Add("Darker");
			cbsharpen.Items.Add("ContrastMore");
			cbsharpen.Items.Add("ContrastLess");
			cbsharpen.Items.Add("Smoothen");
			cbsharpen.Items.Add("SharpenSoft");
			cbsharpen.Items.Add("SharpenMedium");
			cbsharpen.Items.Add("SharpenStrong");
			cbsharpen.Items.Add("FindEdges");
			cbsharpen.Items.Add("Contour");
			cbsharpen.Items.Add("EdgeDetect");
			cbsharpen.Items.Add("EdgeDetectSoft");
			cbsharpen.Items.Add("Emboss");
			cbsharpen.Items.Add("MeanRemoval");

			var label5 = new Avalonia.Controls.TextBlock { Text = "Filter:" };
			cbfilter = new Avalonia.Controls.ListBox();
			cbfilter.Items.Add("dither");
			cbfilter.Items.Add("Point");
			cbfilter.Items.Add("Box");
			cbfilter.Items.Add("Triangle");
			cbfilter.Items.Add("Quadratic");
			cbfilter.Items.Add("Cubic");
			cbfilter.Items.Add("Catrom");
			cbfilter.Items.Add("Mitchell");
			cbfilter.Items.Add("Gaussian");
			cbfilter.Items.Add("Sinc");
			cbfilter.Items.Add("Bessel");
			cbfilter.Items.Add("Hanning");
			cbfilter.Items.Add("Hamming");
			cbfilter.Items.Add("Blackman");
			cbfilter.Items.Add("Kaiser");
			cbfilter.SelectionMode = Avalonia.Controls.SelectionMode.Multiple;

			// Settings grid
			var settingsGrid = new Avalonia.Controls.Grid();
			settingsGrid.ColumnDefinitions.Add(new Avalonia.Controls.ColumnDefinition(Avalonia.Controls.GridLength.Auto));
			settingsGrid.ColumnDefinitions.Add(new Avalonia.Controls.ColumnDefinition(new Avalonia.Controls.GridLength(1, Avalonia.Controls.GridUnitType.Star)));
			for (int r = 0; r < 5; r++)
				settingsGrid.RowDefinitions.Add(new Avalonia.Controls.RowDefinition(Avalonia.Controls.GridLength.Auto));

			// Row 0: Levels
			Avalonia.Controls.Grid.SetRow(label1, 0); Avalonia.Controls.Grid.SetColumn(label1, 0);
			Avalonia.Controls.Grid.SetRow(tblevel, 0); Avalonia.Controls.Grid.SetColumn(tblevel, 1);
			// Row 1: Size
			Avalonia.Controls.Grid.SetRow(label2, 1); Avalonia.Controls.Grid.SetColumn(label2, 0);
			var sizeRow = new Avalonia.Controls.StackPanel { Orientation = Avalonia.Layout.Orientation.Horizontal, Spacing = 4 };
			sizeRow.Children.Add(tbwidth);
			sizeRow.Children.Add(labelX);
			sizeRow.Children.Add(tbheight);
			Avalonia.Controls.Grid.SetRow(sizeRow, 1); Avalonia.Controls.Grid.SetColumn(sizeRow, 1);
			// Row 2: Format
			Avalonia.Controls.Grid.SetRow(label3, 2); Avalonia.Controls.Grid.SetColumn(label3, 0);
			Avalonia.Controls.Grid.SetRow(cbformat, 2); Avalonia.Controls.Grid.SetColumn(cbformat, 1);
			// Row 3: Sharpen
			Avalonia.Controls.Grid.SetRow(label4, 3); Avalonia.Controls.Grid.SetColumn(label4, 0);
			Avalonia.Controls.Grid.SetRow(cbsharpen, 3); Avalonia.Controls.Grid.SetColumn(cbsharpen, 1);
			// Row 4: Filter
			Avalonia.Controls.Grid.SetRow(label5, 4); Avalonia.Controls.Grid.SetColumn(label5, 0);
			Avalonia.Controls.Grid.SetRow(cbfilter, 4); Avalonia.Controls.Grid.SetColumn(cbfilter, 1);

			settingsGrid.Children.Add(label1);
			settingsGrid.Children.Add(tblevel);
			settingsGrid.Children.Add(label2);
			settingsGrid.Children.Add(sizeRow);
			settingsGrid.Children.Add(label3);
			settingsGrid.Children.Add(cbformat);
			settingsGrid.Children.Add(label4);
			settingsGrid.Children.Add(cbsharpen);
			settingsGrid.Children.Add(label5);
			settingsGrid.Children.Add(cbfilter);

			var settingsBorder = new Avalonia.Controls.Border
			{
				BorderThickness = new Avalonia.Thickness(1),
				Padding = new Avalonia.Thickness(4),
				Child = settingsGrid
			};

			// Build button
			button1 = new Avalonia.Controls.Button { Content = "Build", IsEnabled = false };
			button1.Click += Build;

			// Left column: image preview + open button
			var leftPanel = new Avalonia.Controls.StackPanel { Spacing = 8 };
			leftPanel.Children.Add(pb);
			leftPanel.Children.Add(linkLabel1);

			// Outer layout
			var outerGrid = new Avalonia.Controls.Grid();
			outerGrid.ColumnDefinitions.Add(new Avalonia.Controls.ColumnDefinition(Avalonia.Controls.GridLength.Auto));
			outerGrid.ColumnDefinitions.Add(new Avalonia.Controls.ColumnDefinition(new Avalonia.Controls.GridLength(1, Avalonia.Controls.GridUnitType.Star)));
			outerGrid.RowDefinitions.Add(new Avalonia.Controls.RowDefinition(new Avalonia.Controls.GridLength(1, Avalonia.Controls.GridUnitType.Star)));
			outerGrid.RowDefinitions.Add(new Avalonia.Controls.RowDefinition(Avalonia.Controls.GridLength.Auto));

			Avalonia.Controls.Grid.SetRow(leftPanel, 0); Avalonia.Controls.Grid.SetColumn(leftPanel, 0);
			Avalonia.Controls.Grid.SetRow(settingsBorder, 0); Avalonia.Controls.Grid.SetColumn(settingsBorder, 1);
			Avalonia.Controls.Grid.SetRow(button1, 1); Avalonia.Controls.Grid.SetColumn(button1, 1);
			Avalonia.Controls.Grid.SetColumnSpan(button1, 1);

			outerGrid.Children.Add(leftPanel);
			outerGrid.Children.Add(settingsBorder);
			outerGrid.Children.Add(button1);

			Content = outerGrid;
		}

		// ── Helper: convert System.Drawing.Image to Avalonia Bitmap ──────────
		private static Avalonia.Media.Imaging.Bitmap ToAvaloniaBitmap(System.Drawing.Image image)
		{
			if (image == null) return null;
			try
			{
				using (var ms = new System.IO.MemoryStream())
				{
					image.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
					ms.Position = 0;
					return new Avalonia.Media.Imaging.Bitmap(ms);
				}
			}
			catch { return null; }
		}

		System.Drawing.Image img;
		string imgname;
		DDSData[] dds;

		public DDSData[] Execute(int level, System.Drawing.Size size, ImageLoader.TxtrFormats format)
		{
			pb.Source = null;
			img  = null;
			dds  = null;

			this.cbsharpen.SelectedIndex = 0;
			this.tblevel.Text   = level.ToString();
			this.tbwidth.Text   = size.Width.ToString();
			this.tbheight.Text  = size.Height.ToString();

			cbformat.SelectedIndex = 2;
			for (int i = 0; i < cbformat.Items.Count; i++)
			{
				ImageLoader.TxtrFormats fr = (ImageLoader.TxtrFormats)cbformat.Items[i];
				if (fr == format)
				{
					cbformat.SelectedIndex = i;
					break;
				}
			}

			this.button1.IsEnabled = false;
			// ShowDialog() is not available for UserControl — caller is responsible for display
			return dds;
		}

		private async void linkLabel1_Click(object sender, Avalonia.Interactivity.RoutedEventArgs e)
		{
			var topLevel = Avalonia.Controls.TopLevel.GetTopLevel(this);
			if (topLevel == null) return;

			var files = await topLevel.StorageProvider.OpenFilePickerAsync(new Avalonia.Platform.Storage.FilePickerOpenOptions
			{
				Title = "Open Image",
				AllowMultiple = false,
				FileTypeFilter = new[]
				{
					new Avalonia.Platform.Storage.FilePickerFileType("All Image Files")
					{
						Patterns = new[] { "*.jpg", "*.bmp", "*.gif", "*.png" }
					},
					new Avalonia.Platform.Storage.FilePickerFileType("All Files")
					{
						Patterns = new[] { "*.*" }
					}
				}
			});

			if (files != null && files.Count > 0)
			{
				imgname = files[0].Path.LocalPath;
				img = System.Drawing.Image.FromFile(imgname);

				pb.Source = ToAvaloniaBitmap(ImageLoader.Preview(img, new System.Drawing.Size(128, 128)));

				tbwidth.Text  = img.Width.ToString();
				tbheight.Text = img.Height.ToString();
				button1.IsEnabled = (img != null);
			}
		}

        private static byte[] LoadFileAsRgba(string filename, out int w, out int h)
        {
            using var pfimImage = Pfimage.FromFile(filename);
            w = pfimImage.Width;
            h = pfimImage.Height;
            byte[] src = pfimImage.Data;
            int stride = pfimImage.Stride;
            byte[] rgba = new byte[w * h * 4];
            for (int y = 0; y < h; y++)
            {
                for (int x = 0; x < w; x++)
                {
                    int si = y * stride + x * 4;
                    int di = (y * w + x) * 4;
                    if (pfimImage.Format == Pfim.ImageFormat.Rgba32)
                    {
                        rgba[di]     = src[si];     // R
                        rgba[di + 1] = src[si + 1]; // G
                        rgba[di + 2] = src[si + 2]; // B
                        rgba[di + 3] = src[si + 3]; // A
                    }
                    else // Bgra32
                    {
                        rgba[di]     = src[si + 2]; // R
                        rgba[di + 1] = src[si + 1]; // G
                        rgba[di + 2] = src[si];     // B
                        rgba[di + 3] = src[si + 3]; // A
                    }
                }
            }
            return rgba;
        }

        private static void WriteDDSFile(string path, int width, int height, string fourCC, byte[] data)
        {
            using var bw = new System.IO.BinaryWriter(System.IO.File.Create(path));
            bw.Write(new byte[] { 0x44, 0x44, 0x53, 0x20 }); // "DDS "
            bw.Write(124);          // header size
            bw.Write(0x00001007);   // flags: CAPS|HEIGHT|WIDTH|PIXELFORMAT|LINEARSIZE
            bw.Write(height);
            bw.Write(width);
            bw.Write(data.Length);  // linear size
            bw.Write(0);            // depth
            bw.Write(1);            // mipmap count
            for (int i = 0; i < 11; i++) bw.Write(0); // reserved
                                                      // Pixel format
            bw.Write(32);           // pixel format size
            bw.Write(4);            // DDPF_FOURCC
            bw.Write(System.Text.Encoding.ASCII.GetBytes(fourCC)); // "DXT1" or "DXT5"
            bw.Write(0); bw.Write(0); bw.Write(0); bw.Write(0); bw.Write(0);
            // Caps
            bw.Write(0x1000);       // DDSCAPS_TEXTURE
            bw.Write(0); bw.Write(0); bw.Write(0); bw.Write(0);
            // Pixel data
            bw.Write(data);
        }

        public static DDSData[] BuildDDS(string imgname, int levels, ImageLoader.TxtrFormats format, string parameters)
        {
            int w, h;
            byte[] rgba;
            try { rgba = LoadFileAsRgba(imgname, out w, out h); }
            catch { return new DDSData[0]; }

            string ddsfile = System.IO.Path.GetTempFileName() + ".dds";
            try
            {
                BCnEncoder.Shared.CompressionFormat bcFormat;
                if (format == ImageLoader.TxtrFormats.DXT1Format)
                    bcFormat = BCnEncoder.Shared.CompressionFormat.Bc1;
                else if (format == ImageLoader.TxtrFormats.DXT3Format)
                    bcFormat = BCnEncoder.Shared.CompressionFormat.Bc2;
                else
                    bcFormat = BCnEncoder.Shared.CompressionFormat.Bc3;

                var encoder = new BCnEncoder.Encoder.BcEncoder(bcFormat);
                encoder.OutputOptions.GenerateMipMaps = false;
                encoder.OutputOptions.FileFormat = BCnEncoder.Shared.OutputFileFormat.Dds;

                var ddsData = encoder.EncodeToDds(rgba, w, h, BCnEncoder.Encoder.PixelFormat.Rgba32);

                using (var fs = System.IO.File.Create(ddsfile))
                {
                    ddsData.Write(fs);
                }

                return ImageLoader.ParesDDS(ddsfile);
            }
            catch (Exception ex)
            {
                Helper.ExceptionMessage("", ex);
                return new DDSData[0];
            }
            finally
            {
                if (System.IO.File.Exists(ddsfile)) System.IO.File.Delete(ddsfile);
            }
        }

        public static DDSData[] BuildDDS(System.Drawing.Image img, int levels, ImageLoader.TxtrFormats format, string parameters)
        {
            string imgname = System.IO.Path.GetTempFileName() + ".png";
            img.Save(imgname, System.Drawing.Imaging.ImageFormat.Png);
            try
            {
                return BuildDDS(imgname, levels, format, parameters);
            }
            finally
            {
                if (System.IO.File.Exists(imgname)) System.IO.File.Delete(imgname);
            }
        }

        public static void AddDDsData(ImageData id, DDSData[] data)
		{
			id.TextureSize = data[0].ParentSize;
			id.Format = data[0].Format;
			id.MipMapLevels = (uint)data.Length;

			id.MipMapBlocks[0].AddDDSData(data);
		}

		private void Build(object sender, Avalonia.Interactivity.RoutedEventArgs e)
		{
			string arg = "-sharpenMethod " + (cbsharpen.SelectedItem?.ToString() ?? "") + " ";
			foreach (var item in cbfilter.SelectedItems)
			{
				arg += "-" + item.ToString() + " ";
			}

			try
			{
				dds = BuildDDS(img, Convert.ToInt32(tblevel.Text), (ImageLoader.TxtrFormats)cbformat.Items[cbformat.SelectedIndex], arg);
			}
			catch (Exception ex)
			{
				Helper.ExceptionMessage(ex);
			}
		}
	}
}
