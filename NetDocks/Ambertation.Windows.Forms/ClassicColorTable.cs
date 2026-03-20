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
 
 using System.Drawing;

namespace Ambertation.Windows.Forms;

public class ClassicColorTable : IColorTable
{
	public Color DockBorderColor => SystemColors.ActiveBorder;

	public Color DockBackgroundColor => SystemColors.Control;

	public Color DockHintHightlightColor => SystemColors.MenuHighlight;

	public Color DockHintOverlayColor => Color.FromArgb(128, Color.White);

	public Color DockButtonBorderColorOuter => SystemColors.InactiveBorder;

	public Color DockButtonBorderColorInner => Color.Transparent;

	public Color DockButtonHighlightBorderColorOuter => SystemColors.ActiveBorder;

	public Color DockButtonHighlightBorderColorInner => Color.Transparent;

	public Color DockButtonBarBackgroundTop => SystemColors.ControlDarkDark;

	public Color DockButtonBarBackgroundBottom => DockButtonBarBackgroundTop;

	public Color DockButtonBackgroundTop => SystemColors.Control;

	public Color DockButtonBackgroundBottom => DockButtonBackgroundTop;

	public Color DockButtonHighlightBackgroundTop => SystemColors.ControlLightLight;

	public Color DockButtonHighlightBackgroundBottom => DockButtonHighlightBackgroundTop;

	public Color DockButtonTextColor => SystemColors.GrayText;

	public Color DockButtonHighlightTextColor => SystemColors.ControlText;

	public Color DockCaptionColorTop => SystemColors.InactiveCaption;

	public Color DockCaptionColorBottom => DockCaptionColorTop;

	public Color DockCaptionFocusColorTop => SystemColors.ActiveCaption;

	public Color DockCaptionFocusColorBottom => DockCaptionFocusColorTop;

	public Color DockCaptionTextColor => SystemColors.InactiveCaptionText;

	public Color DockCaptionFocusTextColor => SystemColors.ActiveCaptionText;

	public Color DockGripColor => SystemColors.ControlDark;

	public Color DockReSizeBackgroundColor => SystemColors.AppWorkspace;

	public Color DockReSizeGripColor => SystemColors.ButtonShadow;
}
