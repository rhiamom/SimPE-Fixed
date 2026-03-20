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

public interface IColorTable
{
	Color DockBorderColor { get; }

	Color DockBackgroundColor { get; }

	Color DockHintHightlightColor { get; }

	Color DockHintOverlayColor { get; }

	Color DockButtonBarBackgroundTop { get; }

	Color DockButtonBarBackgroundBottom { get; }

	Color DockButtonBorderColorOuter { get; }

	Color DockButtonBorderColorInner { get; }

	Color DockButtonHighlightBorderColorOuter { get; }

	Color DockButtonHighlightBorderColorInner { get; }

	Color DockButtonBackgroundTop { get; }

	Color DockButtonBackgroundBottom { get; }

	Color DockButtonHighlightBackgroundTop { get; }

	Color DockButtonHighlightBackgroundBottom { get; }

	Color DockButtonTextColor { get; }

	Color DockButtonHighlightTextColor { get; }

	Color DockCaptionColorTop { get; }

	Color DockCaptionFocusColorTop { get; }

	Color DockCaptionColorBottom { get; }

	Color DockCaptionFocusColorBottom { get; }

	Color DockCaptionTextColor { get; }

	Color DockCaptionFocusTextColor { get; }

	Color DockGripColor { get; }

	Color DockReSizeBackgroundColor { get; }

	Color DockReSizeGripColor { get; }
}
