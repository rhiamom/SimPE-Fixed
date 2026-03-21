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

// Ported from WinForms.  The original used CreateParams / WS_EX_TRANSPARENT
// to get a see-through background on themed GroupBox.  In Avalonia, a
// HeaderedContentControl (or GroupBox from Avalonia.Controls) serves the
// same layout purpose; transparency is handled by the theme.

using Avalonia.Controls;

namespace Ambertation.Windows.Forms
{
    /// <summary>
    /// GroupBox with transparent-background support.
    /// Uses Avalonia's ContentControl as the base (GroupBox equivalent).
    /// </summary>
    public class TransparentGroupBox : ContentControl
    {
        public TransparentGroupBox() { }
    }
}
