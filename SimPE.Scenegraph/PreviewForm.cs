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

namespace SimPe.Plugin
{
	/// <summary>
	/// Zusammenfassung f�r PreviewForm.
	/// </summary>
	public class PreviewForm : Avalonia.Controls.UserControl
	{
		public PreviewForm()
		{
		}

		public void ShowDialog() { }

		static void Exception()
		{
			throw new SimPe.Warning("This Item can't be previewed!", "SimPE was unable to build the Scenegraph.");
		}

        public static Ambertation.Scenes.Scene BuildScene(SimPe.PackedFiles.Wrapper.Cpf cmmat, SimPe.Interfaces.Files.IPackageFile package)
        {
            // 3D scene preview is not supported in this build yet.
            return null;
        }

        public static void Execute(SimPe.PackedFiles.Wrapper.Cpf cmmat, SimPe.Interfaces.Files.IPackageFile package)
		{
			if (!(cmmat is MmatWrapper)) return;

			MmatWrapper mmat = cmmat as MmatWrapper;
			Wait.Start();
			SimPe.Interfaces.Scenegraph.IScenegraphFileIndex fii = FileTable.FileIndex.AddNewChild();
			try
			{
				FileTable.FileIndex.Load();
				fii.AddIndexFromPackage(package);
				if (System.IO.File.Exists(package.SaveFileName))
					fii.AddIndexFromFolder(System.IO.Path.GetDirectoryName(package.SaveFileName));

				GenericRcol rcol = mmat.GMDC;
				if (rcol != null)
				{
					GeometryDataContainerExt gmdcext = new GeometryDataContainerExt(rcol.Blocks[0] as GeometryDataContainer);
					gmdcext.UserTxmtMap[mmat.SubsetName] = mmat.TXMT;
					gmdcext.UserTxtrMap[mmat.SubsetName] = mmat.TXTR;
					Ambertation.Scenes.Scene scene = gmdcext.GetScene(new SimPe.Plugin.Gmdc.ElementOrder(Gmdc.ElementSorting.Preview));

					PreviewForm f = new PreviewForm();
					f.ShowDialog();
				}
				else Exception();

			}
			catch (System.IO.FileNotFoundException)
			{
				Wait.Stop();
				return;
			}
			catch (Exception ex)
			{
				Wait.Stop();
				Helper.ExceptionMessage(ex);
			}
			finally
			{
				FileTable.FileIndex.RemoveChild(fii);
				fii.Clear();
			}
			Wait.Stop();
		}
	}
}
