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
using System.Windows.Forms;
using System.Drawing;
using SimPe.Interfaces.Plugin;
using SimPe.Interfaces;

namespace SimPe.PackedFiles.UserInterface 
{
	/// <summary>
	/// handles Packed Jpeg Files
	/// </summary>
	public class Picture : UIBase, IPackedFileUI
	{
		
		#region IPackedFileUI Member
		public Control GUIHandle
		{
			get 
			{
				return form.JpegPanel;
			}
		}

		public void UpdateGUI(SimPe.Interfaces.Plugin.IFileWrapper wrapper)
		{
			form.picwrapper = wrapper;
			PictureBox pb = form.pb;
			Image img = ((SimPe.PackedFiles.Wrapper.Picture)wrapper).Image;
			pb.Image = img == null ? null : ScaleNearest(img, 1.5f);
		}

		// Scale by an integer-friendly factor with nearest-neighbour interpolation
		// so small Sims 2 thumbnails (often 64×64 or 128×128) stay crisp instead of
		// looking blurry from PictureBox's default bilinear stretch.
		private static Image ScaleNearest(Image src, float factor)
		{
			int w = Math.Max(1, (int)Math.Round(src.Width * factor));
			int h = Math.Max(1, (int)Math.Round(src.Height * factor));
			Bitmap dst = new Bitmap(w, h, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
			using (Graphics g = Graphics.FromImage(dst))
			{
				g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
				g.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.Half;
				g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.None;
				g.DrawImage(src, new Rectangle(0, 0, w, h));
			}
			return dst;
		}

		

		#endregion
	}
}
