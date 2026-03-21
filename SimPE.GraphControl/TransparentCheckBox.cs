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
 ***************************************************************************/

// Ported from WinForms.  The original class used a custom GDI+ rendering
// pipeline to simulate a transparent background for CheckBox on themed
// Windows controls.  Avalonia handles transparency natively, so the
// standard CheckBox is sufficient.

using Avalonia.Controls;

namespace Ambertation.Windows.Forms
{
    /// <summary>
    /// CheckBox with transparent background support.
    /// In Avalonia, transparency is handled natively; this is a thin alias.
    /// </summary>
    public class TransparentCheckBox : CheckBox
    {
        public TransparentCheckBox() { }
    }
}
