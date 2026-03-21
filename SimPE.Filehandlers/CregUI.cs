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
using Avalonia.Controls;
using SimPe.Interfaces.Plugin;
using SimPe.Interfaces;
using SimPe.PackedFiles.Wrapper.Supporting;
using SimPe.Data;
using SimPe.Windows.Forms;
using SimPe.PackedFiles.Wrapper;

namespace SimPe.PackedFiles.UserInterface
{
	/// <summary>
	/// Avalonia port of CregUI. Displays Creg Group3 data.
	/// </summary>
	public class CregUI :
		SimPe.Windows.Forms.WrapperBaseControl, SimPe.Interfaces.Plugin.IPackedFileUI
	{
		private SimPe.PackedFiles.Wrapper.Creg3UI creg3;

		public CregUI()
		{
			creg3 = new SimPe.PackedFiles.Wrapper.Creg3UI();
			creg3.IsVisible = false;
			Content = creg3;
		}

		public Creg Creg
		{
			get { return (Creg)Wrapper; }
		}

		public override void RefreshGUI()
		{
			this.creg3.IsVisible = false;
			if (Creg != null)
			{
				this.creg3.IsVisible = Creg.Group3 != null;
				creg3.Creg = Creg.Group3;
				this.IsEnabled = (creg3.Creg != null);
			}
		}

		public override void OnCommit()
		{
			Creg.SynchronizeUserData(true, false);
		}
	}
}
