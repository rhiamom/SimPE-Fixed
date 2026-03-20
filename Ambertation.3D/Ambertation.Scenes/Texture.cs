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
using System.Drawing; // Size — plain struct, no GDI+ rendering
using System.IO;
using Avalonia.Media.Imaging;
using Bitmap = Avalonia.Media.Imaging.Bitmap;

namespace Ambertation.Scenes;

public class Texture : IDisposable
{
	private string flname;

	private Size sz;

	private Bitmap img;

	private object tag;

	public bool Available => flname != null;

	public string FileName
	{
		get => flname;
		set => flname = value;
	}

	public Size Size
	{
		get => sz;
		set => sz = value;
	}

	/// <summary>Loaded texture image, as an Avalonia Bitmap.</summary>
	public Bitmap TextureImage
	{
		get => img;
		set
		{
			img = value;
			if (img != null)
				Size = new Size(img.PixelSize.Width, img.PixelSize.Height);
		}
	}

	public object Tag
	{
		get => tag;
		set => tag = value;
	}

	internal Texture(string filename, Size sz)
	{
		flname = filename;
		this.sz = sz;
	}

	public void ImportTextureImage()
	{
		LoadTexture(flname);
	}

	public void ExportTextureImage()
	{
		ExportTextureImage(img, flname);
	}

	public override string ToString()
	{
		if (Available)
			return flname + " (" + sz.ToString() + ")";
		return GetType().Name;
	}

	protected bool ExportTextureImage(Bitmap img, string flname)
	{
		if (img == null || flname == null) return false;
		try
		{
			string dir = Path.GetDirectoryName(flname);
			if (!string.IsNullOrEmpty(dir) && !Directory.Exists(dir))
				Directory.CreateDirectory(dir);
			// Avalonia Bitmap.Save writes PNG — suitable for all texture export paths
			using var stream = File.OpenWrite(flname);
			img.Save(stream);
			return true;
		}
		catch
		{
			return false;
		}
	}

	protected void LoadTexture(string flname)
	{
		if (flname == null) return;
		if (!File.Exists(flname))
			flname = Path.GetFileName(flname);
		if (!File.Exists(flname)) return;
		img = null;
		try
		{
			img = new Bitmap(flname);
			if (img != null)
				sz = new Size(img.PixelSize.Width, img.PixelSize.Height);
		}
		catch { }
	}

	public void Dispose()
	{
		img?.Dispose();
		img = null;
		flname = null;
	}
}
