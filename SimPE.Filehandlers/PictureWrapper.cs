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
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using Pfim;
using SimPe.Interfaces.Plugin;

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
        private static bool IsDdsHeader(byte[] bytes)
        {
            // DDS files start with "DDS " (0x44445320)
            return bytes.Length >= 4 &&
                   bytes[0] == 0x44 && bytes[1] == 0x44 &&
                   bytes[2] == 0x53 && bytes[3] == 0x20;
        }

        private static bool IsGdiPlusFormat(byte[] bytes)
        {
            if (bytes.Length < 4) return false;

            // JPEG: FF D8
            if (bytes[0] == 0xFF && bytes[1] == 0xD8) return true;
            // PNG: 89 50 4E 47
            if (bytes[0] == 0x89 && bytes[1] == 0x50 && bytes[2] == 0x4E && bytes[3] == 0x47) return true;
            // BMP: 42 4D
            if (bytes[0] == 0x42 && bytes[1] == 0x4D) return true;
            // GIF: 47 49 46
            if (bytes[0] == 0x47 && bytes[1] == 0x49 && bytes[2] == 0x46) return true;

            return false;
        }

        protected bool DoLoad(System.IO.BinaryReader reader, bool errmsg)
        {
            long pos = reader.BaseStream.Position;

            try
            {
                byte[] bytes;
                using (var ms = new System.IO.MemoryStream())
                {
                    reader.BaseStream.Position = pos;
                    reader.BaseStream.CopyTo(ms);
                    bytes = ms.ToArray();
                }

                if (bytes.Length == 0)
                {
                    image = null;
                    return false;
                }

                // Route to Pfim directly for DDS/DXT - never attempt GDI+ on these
                if (IsDdsHeader(bytes))
                {
                    image = TryLoadWithPfim(bytes);
                    return (image != null);
                }

                // For known GDI+ formats, try GDI+ only - never attempt Pfim
                if (IsGdiPlusFormat(bytes))
                {
                    image = LoadViaGdiPlus(bytes);
                    return image != null;
                }

                // Unknown format (likely TGA) - try Pfim first, then GDI+ as fallback
                image = TryLoadWithPfim(bytes);
                if (image != null) return true;

                image = LoadViaGdiPlus(bytes);
                return image != null;
            }
            catch
            {
                image = null;
                return false;
            }
            finally
            {
                reader.BaseStream.Position = pos;
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

        // GDI+ Image.FromStream returns an Image that holds a reference to the
        // source stream — disposing the stream leaves the Image in a state where
        // Save() and pixel access throw "A generic error occurred in GDI+".
        // Clone into a stream-independent Bitmap so the cache writer can re-encode
        // it later without surfacing that error.
        private static Image LoadViaGdiPlus(byte[] bytes)
        {
            try
            {
                using (var ims = new System.IO.MemoryStream(bytes))
                using (var loaded = System.Drawing.Image.FromStream(ims))
                {
                    return new Bitmap(loaded);
                }
            }
            catch
            {
                return null;
            }
        }

        // Prevent infinite retry loops when image decode fails repeatedly.
        static readonly object pfimFailLock = new object();
        static readonly System.Collections.Generic.HashSet<int> pfimFailSigs =
            new System.Collections.Generic.HashSet<int>();

        static int GetPfimFailSig(byte[] bytes)
        {
            if (bytes == null) return 0;
            unchecked
            {
                int h = bytes.Length;
                int n = Math.Min(bytes.Length, 64);
                for (int i = 0; i < n; i++)
                    h = (h * 31) + bytes[i];
                return h;
            }
        }

        private static Image TryLoadWithPfim(byte[] bytes)
        {
            int sig = GetPfimFailSig(bytes);
            lock (pfimFailLock)
            {
                if (pfimFailSigs.Contains(sig)) return null;
            }

            try
            {
                using var ms = new System.IO.MemoryStream(bytes);
                IImage pfimImage = IsDdsHeader(bytes)
                    ? (IImage)Dds.Create(ms, new PfimConfig())
                    : (IImage)Targa.Create(ms, new PfimConfig());
                using (pfimImage)
                    return PfimToBitmap(pfimImage);
            }
            catch
            {
                lock (pfimFailLock) { pfimFailSigs.Add(sig); }
                return null;
            }
        }

        // Pfim 0.11.4 ImageFormat enum: Rgb8=0, R5g5b5=1, R5g6b5=2, R5g5b5a1=3,
        // Rgba16=4, Rgb24=5, Rgba32=6, R16f=7, R32f=8.
        // IMPORTANT: despite the names, Pfim's Rgba32/Rgb24 actually return data
        // in BGR(A) byte order — they pass through the file's native bytes (TGA
        // and DDS are both BGR-on-disk). GDI+ Format32bppArgb is also BGRA in
        // memory, so for 32/24-bit we do a straight copy with no channel swap.
        // The 16-bit packed formats use the standard TGA/DDS bit layout
        // (B in low bits, R in high bits) regardless of byte order.
        private static Bitmap PfimToBitmap(IImage pfimImage)
        {
            if (pfimImage == null || pfimImage.Width <= 0 || pfimImage.Height <= 0) return null;
            if (pfimImage.Width > 4096 || pfimImage.Height > 4096) return null;

            int w = pfimImage.Width;
            int h = pfimImage.Height;
            byte[] src = pfimImage.Data;
            int srcStride = pfimImage.Stride;
            var fmt = pfimImage.Format;
            int srcBpp;
            switch (fmt)
            {
                case Pfim.ImageFormat.Rgba32:    srcBpp = 4; break;
                case Pfim.ImageFormat.Rgb24:     srcBpp = 3; break;
                case Pfim.ImageFormat.Rgba16:    srcBpp = 2; break;
                case Pfim.ImageFormat.R5g5b5a1:  srcBpp = 2; break;
                case Pfim.ImageFormat.R5g6b5:    srcBpp = 2; break;
                case Pfim.ImageFormat.R5g5b5:    srcBpp = 2; break;
                case Pfim.ImageFormat.Rgb8:      srcBpp = 1; break;
                default: return null; // R16f / R32f (HDR float) — not used by The Sims 2
            }

            Bitmap bmp = new Bitmap(w, h, PixelFormat.Format32bppArgb);
            BitmapData bmpData = bmp.LockBits(new Rectangle(0, 0, w, h),
                ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);
            try
            {
                int dstStride = Math.Abs(bmpData.Stride);
                byte[] dst = new byte[dstStride * h];

                for (int y = 0; y < h; y++)
                {
                    int srcRow = y * srcStride;
                    int dstRow = y * dstStride;
                    for (int x = 0; x < w; x++)
                    {
                        int si = srcRow + x * srcBpp;
                        int di = dstRow + x * 4;
                        switch (fmt)
                        {
                            case Pfim.ImageFormat.Rgba32: // really BGRA in memory
                                dst[di]     = src[si];     // B
                                dst[di + 1] = src[si + 1]; // G
                                dst[di + 2] = src[si + 2]; // R
                                dst[di + 3] = src[si + 3]; // A
                                break;
                            case Pfim.ImageFormat.Rgb24:  // really BGR in memory
                                dst[di]     = src[si];     // B
                                dst[di + 1] = src[si + 1]; // G
                                dst[di + 2] = src[si + 2]; // R
                                dst[di + 3] = 0xFF;
                                break;
                            case Pfim.ImageFormat.Rgb8:
                            {
                                byte g = src[si];
                                dst[di] = g; dst[di + 1] = g; dst[di + 2] = g;
                                dst[di + 3] = 0xFF;
                                break;
                            }
                            case Pfim.ImageFormat.R5g5b5: // 0RRRRRGG GGGBBBBB (little-endian)
                            {
                                ushort v = (ushort)(src[si] | (src[si + 1] << 8));
                                dst[di]     = (byte)((v & 0x1F) << 3);
                                dst[di + 1] = (byte)(((v >> 5) & 0x1F) << 3);
                                dst[di + 2] = (byte)(((v >> 10) & 0x1F) << 3);
                                dst[di + 3] = 0xFF;
                                break;
                            }
                            case Pfim.ImageFormat.R5g6b5: // RRRRRGGG GGGBBBBB
                            {
                                ushort v = (ushort)(src[si] | (src[si + 1] << 8));
                                dst[di]     = (byte)((v & 0x1F) << 3);
                                dst[di + 1] = (byte)(((v >> 5) & 0x3F) << 2);
                                dst[di + 2] = (byte)(((v >> 11) & 0x1F) << 3);
                                dst[di + 3] = 0xFF;
                                break;
                            }
                            case Pfim.ImageFormat.R5g5b5a1: // ARRRRRGG GGGBBBBB
                            {
                                ushort v = (ushort)(src[si] | (src[si + 1] << 8));
                                dst[di]     = (byte)((v & 0x1F) << 3);
                                dst[di + 1] = (byte)(((v >> 5) & 0x1F) << 3);
                                dst[di + 2] = (byte)(((v >> 10) & 0x1F) << 3);
                                dst[di + 3] = (byte)(((v >> 15) & 0x1) * 0xFF);
                                break;
                            }
                            case Pfim.ImageFormat.Rgba16: // 4 bits per channel
                            {
                                ushort v = (ushort)(src[si] | (src[si + 1] << 8));
                                dst[di]     = (byte)(((v >> 4) & 0xF) * 0x11);
                                dst[di + 1] = (byte)(((v >> 8) & 0xF) * 0x11);
                                dst[di + 2] = (byte)(((v >> 12) & 0xF) * 0x11);
                                dst[di + 3] = (byte)((v & 0xF) * 0x11);
                                break;
                            }
                        }
                    }
                }

                Marshal.Copy(dst, 0, bmpData.Scan0, dst.Length);
            }
            finally
            {
                bmp.UnlockBits(bmpData);
            }
            return bmp;
        }
    }
}
