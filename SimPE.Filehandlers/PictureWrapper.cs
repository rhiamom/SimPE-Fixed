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
                Helper.LoadImage(this.GetType().Assembly.GetManifestResourceStream("SimPe.PackedFiles.Handlers.pic.png"))
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
                    try
                    {
                        using (var ims = new System.IO.MemoryStream(bytes))
                        {
                            image = Helper.LoadImage(ims);
                            return true;
                        }
                    }
                    catch
                    {
                        image = null;
                        return false;
                    }
                }

                // Unknown format (likely TGA) - try Pfim first, then GDI+ as fallback
                image = TryLoadWithPfim(bytes);
                if (image != null) return true;

                try
                {
                    using (var ims = new System.IO.MemoryStream(bytes))
                    {
                        image = Helper.LoadImage(ims);
                        return true;
                    }
                }
                catch
                {
                    image = null;
                    return false;
                }
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

        private static Bitmap PfimToBitmap(IImage pfimImage)
        {
            if (pfimImage == null || pfimImage.Width <= 0 || pfimImage.Height <= 0) return null;
            if (pfimImage.Width > 4096 || pfimImage.Height > 4096) return null;

            int w = pfimImage.Width;
            int h = pfimImage.Height;
            byte[] src = pfimImage.Data;
            int srcStride = pfimImage.Stride;

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
                        int si = srcRow + x * 4;
                        int di = dstRow + x * 4;
                        if (pfimImage.Format == Pfim.ImageFormat.Rgba32)
                        {
                            dst[di]     = src[si + 2]; // B
                            dst[di + 1] = src[si + 1]; // G
                            dst[di + 2] = src[si];     // R
                            dst[di + 3] = src[si + 3]; // A
                        }
                        else // Bgra32 and similar
                        {
                            dst[di]     = src[si];     // B
                            dst[di + 1] = src[si + 1]; // G
                            dst[di + 2] = src[si + 2]; // R
                            dst[di + 3] = src[si + 3]; // A
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
