/**************************************************************************
 *   Copyright (C) 2023 by Chris Hatch                                    *
 *   (original author, BSOK Wizard)                                       *
 *                                                                         *
 *   Copyright (C) 2025 by GramzeSweatShop                                *
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
 **************************************************************************/
using System;
using SimPe.Interfaces;
using SimPe.Plugin;

namespace SimPe.Wizards
{
    public class BsokTool : Interfaces.AbstractTool, Interfaces.ITool
    {
        public bool IsEnabled(SimPe.Interfaces.Files.IPackedFileDescriptor pfd, SimPe.Interfaces.Files.IPackageFile package)
        {
            return true;
        }

        public Interfaces.Plugin.IToolResult ShowDialog(ref SimPe.Interfaces.Files.IPackedFileDescriptor pfd, ref SimPe.Interfaces.Files.IPackageFile package)
        {
            var form = new BsokWizardForm();
            form.ShowDialog(null).GetAwaiter().GetResult();
            return new ToolResult(false, false);
        }

        public override string ToString()
        {
            return "Wizards\\BSOK Wizard...";
        }

        public override int Shortcut
        {
            get { return 0; }
        }
    }
}
