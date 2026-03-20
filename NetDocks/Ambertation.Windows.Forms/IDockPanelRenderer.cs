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
using System.Windows.Forms;

namespace Ambertation.Windows.Forms;

public interface IDockPanelRenderer
{
	BaseDockPanelRenderer.Dimensions Dimension { get; }

	event DockAnimationEventHandler FinishedAnimation;

	DockPanelButtonManager ConstructButtonData(IButtonContainer cnt, NCPaintEventArgs e);

	Rectangle GetButtonsRectangle(ButtonOrientation orient, NCPaintEventArgs e, DockContainer dc);

	Rectangle GetPanelClientRectangle(DockPanel dp, ButtonOrientation orient);

	Rectangle GetPanelClientRectangle(DockContainer dc, NCPaintEventArgs e, ButtonOrientation orient);

	Padding GetGripSize(DockStyle dock);

	Rectangle GetGripRectangle(NCPaintEventArgs e, DockStyle dock);

	void RenderGrip(DockContainer dc, NCPaintEventArgs e, Rectangle r);

	void RenderResizePanel(DockContainer dc, RubberBandHelper rbh, PaintEventArgs e);

	Padding GetPanelBorderSize(DockContainer dc, DockPanel dp, ButtonOrientation orient);

	Padding GetBarBorderSize(ButtonOrientation orient);

	Padding GetBorderSize(IButtonContainer c);

	Size GetButtonSize(DockPanel dp);

	Size GetButtonSize(DockPanel dp, ButtonOrientation orient);

	string GetFittingString(Font font, string caption, ButtonOrientation orient, Size maxsz);

	Rectangle GetCaptionRect(DockPanel dp);

	Rectangle GetCaptionRect(DockPanel dp, ButtonOrientation orient);

	Rectangle GetCloseButtonRect(DockPanel dp, Rectangle caprect);

	Rectangle GetCollapseButtonRect(DockPanel dp, Rectangle caprect);

	Rectangle GetCaptionTextRect(DockPanel dp, Rectangle caprect);

	void RenderButtonBarBackground(NCPaintEventArgs e, Rectangle r, ButtonOrientation orient);

	void RenderButtonBackground(DockPanel dp, NCPaintEventArgs e);

	void RenderButton(Graphics g, Rectangle r, string caption, Image img, ButtonOrientation orient, ButtonState state, bool renderbackgroundbar);

	void RenderCaption(DockPanel dp, NCPaintEventArgs e);

	void RenderCaptionButton(DockPanel dp, DockPanelCaptionButton but, NCPaintEventArgs e);

	void RenderBorder(DockPanel dp, NCPaintEventArgs e);

	void Animate(DockAnimationEventArgs e);
}
