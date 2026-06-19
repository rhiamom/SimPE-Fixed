/***************************************************************************
 *   Copyright (C) 2026 by GramzeSweatshop                                 *
 *   rhiamom@mac.com                                                       *
 *                                                                         *
 *   This program is free software; you can redistribute it and/or modify  *
 *   it under the terms of the GNU General Public License as published by  *
 *   the Free Software Foundation; either version 2 of the License, or     *
 *   (at your option) any later version.                                   *
 ***************************************************************************/

using System.Drawing;
using System.Windows.Forms;

namespace SimPe.Plugin
{
    // Shim for Chris Hatch's `booby.GraphPanel` from GDF.dll (NSFW theme lib,
    // never distributed in source). The designer sets BarColour/Datas/etc on
    // each of the seven family-history graphs; we accept the setters and ignore
    // them so the designer compiles. Visual result: a plain panel where the
    // booby version would have drawn a bar/line graph. The underlying FAMH
    // data is still parsed and shown in the block table — only the summary
    // visualisation is lost.
    public class FamhGraphPanel : Panel
    {
        public Color BarColour { get; set; }
        public Color HighlightColour { get; set; }
        public float LineWidth { get; set; }
        public string Title { get; set; }
        public bool UseBars { get; set; }
        public int[] Datas { get; set; }
    }

    // Shim for Chris Hatch's `booby.linkyicon` — a LinkLabel-with-icon. Same
    // story: source unavailable. Accept Gap/Icon/Label setters as no-ops.
    public class FamhLinkLabel : LinkLabel
    {
        public int Gap { get; set; }
        public Image Icon { get; set; }
        public string Label { get; set; }
        public Color LinkColour { get => LinkColor; set => LinkColor = value; }
    }
}
