/***************************************************************************
 *   Copyright (C) 2005 by Ambertation                                     *
 *   quaxi@ambertation.de                                                  *
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
using SimPe.Interfaces.Plugin;
using System.Runtime.InteropServices;
using System.Drawing.Imaging;

namespace SimPe.PackedFiles.Wrapper
{
	/// <summary>
	/// Represents a PacjedFile in JPEG Format
	/// </summary>
	public class Picture : AbstractWrapper, SimPe.Interfaces.Plugin.IFileWrapper, System.IDisposable
	{
		/// <summary>
		/// Stores the Image
		/// </summary>
		protected System.Drawing.Image image;

		/// <summary>
		/// Returns the Stored Image
		/// </summary>
		public System.Drawing.Image Image
		{
			get
			{
				return image;
			}
		}

		#region IWrapper Member
		protected override IWrapperInfo CreateWrapperInfo()
		{
			return new AbstractWrapperInfo(
				"Picture Wrapper",
				"Quaxi",
				"---",
				2,
				System.Drawing.Image.FromStream(this.GetType().Assembly.GetManifestResourceStream("SimPe.PackedFiles.Handlers.pic.png"))
				);
		}
		#endregion

		public static Image SetAlpha(Image img)
		{
			Bitmap bmp = new Bitmap(img.Size.Width, img.Size.Height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

			for (int y = 0; y<bmp.Size.Height; y++)
			{
				for (int x = 0; x<bmp.Size.Width; x++)
				{
					Color basecol = ((Bitmap)img).GetPixel(x, y);
					int a = 0xFF - ((basecol.R + basecol.G + basecol.B) / 3);
					if (a>0x10) a=0xff;

					Color col = Color.FromArgb(a, basecol);
					bmp.SetPixel(x, y, col);
				}
			}

			return bmp;
		}

        protected bool DoLoad(System.IO.BinaryReader reader, bool errmsg)
        {
            long pos = reader.BaseStream.Position;

            try
            {
                // First try normal GDI+ formats (PNG/JPG/BMP/etc.)
                image = System.Drawing.Image.FromStream(reader.BaseStream);
                return true;
            }
            catch (ArgumentException)
            {
                // Not a GDI+ format (often TGA). Rewind and try SOIL2.
                reader.BaseStream.Position = pos;

                byte[] bytes;
                using (var ms = new System.IO.MemoryStream())
                {
                    reader.BaseStream.CopyTo(ms);
                    bytes = ms.ToArray();
                }

                image = TryLoadWithSoil2(bytes);   // <-- THIS is the DLL fallback call
                return (image != null);
            }
            catch
            {
                // Anything else: treat as no image
                reader.BaseStream.Position = pos;
                image = null;
                return false;
            }
        }


        #region AbstractWrapper Member
        protected override IPackedFileUI CreateDefaultUIHandler()
		{
			return new SimPe.PackedFiles.UserInterface.Picture();
		}

		public Picture() : base() { }

		protected override void Unserialize(System.IO.BinaryReader reader)
		{
			if (!this.DoLoad(reader, false))
			{
				System.IO.BinaryReader br = new System.IO.BinaryReader(new System.IO.MemoryStream());
				System.IO.BinaryWriter bw = new System.IO.BinaryWriter(br.BaseStream);
				reader.BaseStream.Seek(0x40, System.IO.SeekOrigin.Begin);

				bw.Write(reader.ReadBytes((int)(reader.BaseStream.Length-0x40)));
				DoLoad(br, true);
			}

		}
		#endregion

		#region IPackedFileWrapper Member

		public uint[] AssignableTypes
		{
			get
			{
				uint[] Types = {
					0x0C7E9A76, //jpeg
					0x856DDBAC, //jpeg
					0x424D505F, //bitmap
					0x856DDBAC, //png
					0x856DDBAC,  //tga
					0xAC2950C1, //thumbnail
					0x4D533EDD,
					0xAC2950C1,
					0x2C30E040,
					0x2C43CBD4,
					0x2C488BCA,
					0x8C31125E,
					0x8C311262,
					0xCC30CDF8,
					0xCC44B5EC,
					0xCC489E46,
					0xCC48C51F,
					0x8C3CE95A,
					0xEC3126C4,
					0xF03D464C
							   };
				return Types;
			}
		}

		public Byte[] FileSignature
		{
			get
			{
				return new Byte[0];
			}
		}

		#endregion

		#region IDisposable Member

		public override void Dispose()
		{
			if (this.image!=null) this.image.Dispose();
			image = null;

			base.Dispose();
		}

        #endregion

        [DllImport("soil2.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr SOIL_load_image_from_memory(
			byte[] buffer, int bufferLength,
			out int width, out int height, out int channels,
			int forceChannels);

        [DllImport("soil2.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern void SOIL_free_image_data(IntPtr imgData);

        private const int SoilLoadRgba = 4; // Force RGBA output

        private static Image TryLoadWithSoil2(byte[] bytes)
        {
            System.Diagnostics.Debug.WriteLine("TryLoadWithSoil2 called");
            int w, h, ch;
            IntPtr img = SOIL_load_image_from_memory(bytes, bytes.Length, out w, out h, out ch, SoilLoadRgba);
            if (img == IntPtr.Zero || w <= 0 || h <= 0) return null;

            try
            {
                int srcStride = w * 4;
                byte[] rgba = new byte[srcStride * h];

                // Copy native RGBA buffer into managed array
                Marshal.Copy(img, rgba, 0, rgba.Length);

                Bitmap bmp = new Bitmap(w, h, PixelFormat.Format32bppArgb);
                Rectangle rect = new Rectangle(0, 0, w, h);
                BitmapData data = bmp.LockBits(rect, ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);

                try
                {
                    int dstStride = data.Stride;
                    byte[] bgra = new byte[dstStride * h];

                    for (int y = 0; y < h; y++)
                    {
                        int srcRow = y * srcStride;
                        int dstRow = y * dstStride;

                        for (int x = 0; x < w; x++)
                        {
                            int si = srcRow + (x * 4);
                            int di = dstRow + (x * 4);

                            // RGBA -> BGRA
                            bgra[di + 0] = rgba[si + 2]; // B
                            bgra[di + 1] = rgba[si + 1]; // G
                            bgra[di + 2] = rgba[si + 0]; // R
                            bgra[di + 3] = rgba[si + 3]; // A
                        }
                    }

                    Marshal.Copy(bgra, 0, data.Scan0, bgra.Length);
                }
                finally
                {
                    bmp.UnlockBits(data);
                }

                return bmp;
            }
            finally
            {
                SOIL_free_image_data(img);
            }
        }

    }
}
