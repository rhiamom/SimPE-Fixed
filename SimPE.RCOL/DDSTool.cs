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
using System.Threading.Tasks;
using Pfim;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Layout;
using BCnEncoder.Encoder;
using BCnEncoder.Shared;
using SkiaSharp;

namespace SimPe.Plugin
{
	/// <summary>
	/// Summary description for DDSTool.
	/// Modal dialog: user opens a PNG, configures DXT settings, clicks Build.
	/// </summary>
	public class DDSTool : Avalonia.Controls.Window
	{
		private Avalonia.Controls.Button linkLabel1;
		private Avalonia.Controls.Image pb;
		private Avalonia.Controls.TextBox tblevel;
		private Avalonia.Controls.TextBlock tbwidth;
		private Avalonia.Controls.TextBlock tbheight;
		private Avalonia.Controls.ComboBox cbformat;
		private Avalonia.Controls.ComboBox cbsharpen;
		private Avalonia.Controls.Button button1;

		public DDSTool()
		{
			Title = "Build DXT Texture";
			Width = 500;
			Height = 370;
			CanResize = false;
			WindowStartupLocation = Avalonia.Controls.WindowStartupLocation.CenterOwner;
			BuildLayout();

			cbformat.Items.Clear();
			cbformat.Items.Add(ImageLoader.TxtrFormats.DXT1Format);
			cbformat.Items.Add(ImageLoader.TxtrFormats.DXT3Format);
			cbformat.Items.Add(ImageLoader.TxtrFormats.DXT5Format);
		}

		private void BuildLayout()
		{
			// ── image preview in a bordered box ──────────────────────────────
			pb = new Avalonia.Controls.Image
			{
				Width = 128, Height = 128,
				Stretch = Avalonia.Media.Stretch.Uniform,
			};
			var imageBorder = new Avalonia.Controls.Border
			{
				BorderBrush     = new Avalonia.Media.SolidColorBrush(Avalonia.Media.Color.FromRgb(140, 140, 140)),
				BorderThickness = new Avalonia.Thickness(1),
				Background      = Avalonia.Media.Brushes.White,
				Width = 132, Height = 132,
				HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Left,
				Child = pb,
			};

			linkLabel1 = new Avalonia.Controls.Button
			{
				Content = "open Image...",
				HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Stretch,
				Margin = new Avalonia.Thickness(0, 6, 0, 0),
			};
			linkLabel1.Click += linkLabel1_Click;

			var leftPanel = new Avalonia.Controls.StackPanel
			{
				Width = 136,
				VerticalAlignment = Avalonia.Layout.VerticalAlignment.Top,
				Margin = new Avalonia.Thickness(0, 0, 8, 0),
			};
			leftPanel.Children.Add(imageBorder);
			leftPanel.Children.Add(linkLabel1);

			// ── settings ──────────────────────────────────────────────────────
			const double dropW = 240;
			const double rowH  = 28;

			var label1 = new Avalonia.Controls.TextBlock { Text = "Levels:",  VerticalAlignment = Avalonia.Layout.VerticalAlignment.Center };
			tblevel    = new Avalonia.Controls.TextBox   { Width = dropW,     HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Left };

			var label2 = new Avalonia.Controls.TextBlock { Text = "Size:",    VerticalAlignment = Avalonia.Layout.VerticalAlignment.Center };
			tbwidth    = new Avalonia.Controls.TextBlock { Text = "0",        VerticalAlignment = Avalonia.Layout.VerticalAlignment.Center };
			tbheight   = new Avalonia.Controls.TextBlock { Text = "0",        VerticalAlignment = Avalonia.Layout.VerticalAlignment.Center };
			var labelX = new Avalonia.Controls.TextBlock { Text = " x ",      VerticalAlignment = Avalonia.Layout.VerticalAlignment.Center };
			var sizeRow = new Avalonia.Controls.StackPanel { Orientation = Avalonia.Layout.Orientation.Horizontal };
			sizeRow.Children.Add(tbwidth);
			sizeRow.Children.Add(labelX);
			sizeRow.Children.Add(tbheight);

			var label3 = new Avalonia.Controls.TextBlock { Text = "Format:",  VerticalAlignment = Avalonia.Layout.VerticalAlignment.Center };
			cbformat   = new Avalonia.Controls.ComboBox  { Width = dropW,     HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Left };

			var label4 = new Avalonia.Controls.TextBlock { Text = "Sharpen:", VerticalAlignment = Avalonia.Layout.VerticalAlignment.Center };
			cbsharpen  = new Avalonia.Controls.ComboBox  { Width = dropW,     HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Left };
			cbsharpen.Items.Add("None");        cbsharpen.Items.Add("Negative");
			cbsharpen.Items.Add("Lighter");     cbsharpen.Items.Add("Darker");
			cbsharpen.Items.Add("ContrastMore");cbsharpen.Items.Add("ContrastLess");
			cbsharpen.Items.Add("Smoothen");    cbsharpen.Items.Add("SharpenSoft");
			cbsharpen.Items.Add("SharpenMedium");cbsharpen.Items.Add("SharpenStrong");
			cbsharpen.Items.Add("FindEdges");   cbsharpen.Items.Add("Contour");
			cbsharpen.Items.Add("EdgeDetect");  cbsharpen.Items.Add("EdgeDetectSoft");
			cbsharpen.Items.Add("Emboss");      cbsharpen.Items.Add("MeanRemoval");

			var label5 = new Avalonia.Controls.TextBlock
			{
				Text = "Filter:",
				VerticalAlignment = Avalonia.Layout.VerticalAlignment.Top,
				Margin = new Avalonia.Thickness(0, 4, 0, 0),
			};
			var filterStack = new Avalonia.Controls.StackPanel { Spacing = 0 };
			foreach (var fname in new[] { "dither","Point","Box","Triangle","Quadratic","Cubic","Catrom","Mitchell","Gaussian","Sinc","Bessel","Hanning","Hamming","Blackman","Kaiser" })
				filterStack.Children.Add(new Avalonia.Controls.CheckBox { Content = fname, Padding = new Avalonia.Thickness(4, 1), MinHeight = 0 });
			var filterScroll = new Avalonia.Controls.ScrollViewer
			{
				Height  = 122,
				VerticalScrollBarVisibility   = Avalonia.Controls.Primitives.ScrollBarVisibility.Auto,
				HorizontalScrollBarVisibility = Avalonia.Controls.Primitives.ScrollBarVisibility.Disabled,
				Content = filterStack,
			};
			var filterBorder = new Avalonia.Controls.Border
			{
				BorderBrush     = new Avalonia.Media.SolidColorBrush(Avalonia.Media.Color.FromRgb(160, 160, 160)),
				BorderThickness = new Avalonia.Thickness(1),
				Width           = dropW,
				HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Left,
				Child = filterScroll,
			};

			// settings grid: label col (Auto) | control col (Star)
			var sg = new Avalonia.Controls.Grid();
			sg.ColumnDefinitions.Add(new Avalonia.Controls.ColumnDefinition(Avalonia.Controls.GridLength.Auto));
			sg.ColumnDefinitions.Add(new Avalonia.Controls.ColumnDefinition(new Avalonia.Controls.GridLength(1, Avalonia.Controls.GridUnitType.Star)));
			for (int r = 0; r < 5; r++)
				sg.RowDefinitions.Add(new Avalonia.Controls.RowDefinition(Avalonia.Controls.GridLength.Auto) { MinHeight = rowH });

			void Place(Avalonia.Controls.Control c, int row, int col, Avalonia.Layout.VerticalAlignment va = Avalonia.Layout.VerticalAlignment.Center)
			{
				c.VerticalAlignment = va;
				Avalonia.Controls.Grid.SetRow(c, row);
				Avalonia.Controls.Grid.SetColumn(c, col);
				sg.Children.Add(c);
			}

			Place(label1,       0, 0);  Place(tblevel,      0, 1);
			Place(label2,       1, 0);  Place(sizeRow,      1, 1);
			Place(label3,       2, 0);  Place(cbformat,     2, 1);
			Place(label4,       3, 0);  Place(cbsharpen,    3, 1);
			Place(label5,       4, 0, Avalonia.Layout.VerticalAlignment.Top);
			Place(filterBorder, 4, 1, Avalonia.Layout.VerticalAlignment.Top);

			// ── build button ──────────────────────────────────────────────────
			button1 = new Avalonia.Controls.Button
			{
				Content = "Build",
				IsEnabled = false,
				HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Right,
				Margin = new Avalonia.Thickness(0, 8, 0, 0),
			};
			button1.Click += Build;

			// ── outer layout ──────────────────────────────────────────────────
			var settingsBox = MakeGroupBox("Settings", sg);

			var outerGrid = new Avalonia.Controls.Grid { Margin = new Avalonia.Thickness(8) };
			outerGrid.ColumnDefinitions.Add(new Avalonia.Controls.ColumnDefinition(Avalonia.Controls.GridLength.Auto));
			outerGrid.ColumnDefinitions.Add(new Avalonia.Controls.ColumnDefinition(new Avalonia.Controls.GridLength(1, Avalonia.Controls.GridUnitType.Star)));
			outerGrid.RowDefinitions.Add(new Avalonia.Controls.RowDefinition(new Avalonia.Controls.GridLength(1, Avalonia.Controls.GridUnitType.Star)));
			outerGrid.RowDefinitions.Add(new Avalonia.Controls.RowDefinition(Avalonia.Controls.GridLength.Auto));

			Avalonia.Controls.Grid.SetRow(leftPanel,   0); Avalonia.Controls.Grid.SetColumn(leftPanel,   0);
			Avalonia.Controls.Grid.SetRow(settingsBox, 0); Avalonia.Controls.Grid.SetColumn(settingsBox, 1);
			Avalonia.Controls.Grid.SetRow(button1,     1); Avalonia.Controls.Grid.SetColumn(button1,     1);

			outerGrid.Children.Add(leftPanel);
			outerGrid.Children.Add(settingsBox);
			outerGrid.Children.Add(button1);

			Content = outerGrid;
		}

		static Avalonia.Controls.Border MakeGroupBox(string header, Avalonia.Controls.Control content)
		{
			var title = new Avalonia.Controls.TextBlock
			{
				Text = header,
				FontWeight = Avalonia.Media.FontWeight.SemiBold,
				Margin = new Avalonia.Thickness(0, 0, 0, 6),
			};
			var inner = new Avalonia.Controls.DockPanel();
			Avalonia.Controls.DockPanel.SetDock(title, Avalonia.Controls.Dock.Top);
			inner.Children.Add(title);
			inner.Children.Add(content);
			return new Avalonia.Controls.Border
			{
				BorderBrush     = new Avalonia.Media.SolidColorBrush(Avalonia.Media.Color.FromRgb(160, 160, 160)),
				BorderThickness = new Avalonia.Thickness(1),
				CornerRadius    = new Avalonia.CornerRadius(3),
				Padding         = new Avalonia.Thickness(8),
				Child = inner,
			};
		}

		static Avalonia.Media.Imaging.Bitmap ToAvaloniaBitmap(SKBitmap bm)
		{
			if (bm == null) return null;
			try
			{
				using var skImg = SKImage.FromBitmap(bm);
				using var encoded = skImg.Encode(SKEncodedImageFormat.Png, 100);
				using var ms = new System.IO.MemoryStream();
				encoded.SaveTo(ms);
				ms.Position = 0;
				return new Avalonia.Media.Imaging.Bitmap(ms);
			}
			catch { return null; }
		}

		SKBitmap _img;
		string imgname;
		DDSData[] _dds;

		/// <summary>
		/// Show as modal dialog. Returns the built DDSData[] or null if cancelled.
		/// </summary>
		public static async Task<DDSData[]> Execute(int level, System.Drawing.Size size, ImageLoader.TxtrFormats format)
		{
			var dlg = new DDSTool();
			dlg.cbsharpen.SelectedIndex = 0;
			dlg.tblevel.Text   = level.ToString();
			dlg.tbwidth.Text   = size.Width.ToString();
			dlg.tbheight.Text  = size.Height.ToString();

			dlg.cbformat.SelectedIndex = 2;
			for (int i = 0; i < dlg.cbformat.Items.Count; i++)
			{
				if ((ImageLoader.TxtrFormats)dlg.cbformat.Items[i] == format)
				{
					dlg.cbformat.SelectedIndex = i;
					break;
				}
			}
			dlg.button1.IsEnabled = false;

			await dlg.ShowDialog(SimPe.RemoteControl.ApplicationForm);
			return dlg._dds;
		}

		private async void linkLabel1_Click(object sender, RoutedEventArgs e)
		{
			var files = await StorageProvider.OpenFilePickerAsync(new Avalonia.Platform.Storage.FilePickerOpenOptions
			{
				Title = "Open Image",
				AllowMultiple = false,
				FileTypeFilter = new[]
				{
					new Avalonia.Platform.Storage.FilePickerFileType("Image Files")
					{
						Patterns = new[] { "*.png", "*.jpg", "*.jpeg", "*.bmp", "*.gif" }
					},
				}
			});

			if (files != null && files.Count > 0)
			{
				imgname = files[0].Path.LocalPath;
				using var stream = System.IO.File.OpenRead(imgname);
				_img = SKBitmap.Decode(stream);
				if (_img == null) return;

				pb.Source = ToAvaloniaBitmap(ImageLoader.Preview(_img, new System.Drawing.Size(128, 128)));

				tbwidth.Text  = _img.Width.ToString();
				tbheight.Text = _img.Height.ToString();
				button1.IsEnabled = true;
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

        public static DDSData[] BuildDDS(SKBitmap bm, int levels, ImageLoader.TxtrFormats format, string parameters)
        {
            string tmpfile = System.IO.Path.GetTempFileName() + ".png";
            try
            {
                using var image = SKImage.FromBitmap(bm);
                using var encoded = image.Encode(SKEncodedImageFormat.Png, 100);
                using (var fs = System.IO.File.Create(tmpfile))
                    encoded.SaveTo(fs);
                return BuildDDS(tmpfile, levels, format, parameters);
            }
            finally
            {
                if (System.IO.File.Exists(tmpfile)) System.IO.File.Delete(tmpfile);
            }
        }

        public static void AddDDsData(ImageData id, DDSData[] data)
		{
			id.TextureSize = data[0].ParentSize;
			id.Format = data[0].Format;
			id.MipMapLevels = (uint)data.Length;

			id.MipMapBlocks[0].AddDDSData(data);
		}

		private void Build(object sender, RoutedEventArgs e)
		{
			if (imgname == null || _img == null) return;
			try
			{
				_dds = BuildDDS(imgname, Convert.ToInt32(tblevel.Text),
					(ImageLoader.TxtrFormats)cbformat.Items[cbformat.SelectedIndex], "");
			}
			catch (Exception ex)
			{
				Helper.ExceptionMessage(ex);
			}
			Close();
		}
	}
}
