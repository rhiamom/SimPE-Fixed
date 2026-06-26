/***************************************************************************
 *   Copyright (C) 2026 by GramzeSweatshop                                  *
 *   rhiamom@mac.com                                                        *
 *                                                                          *
 *   Built on JFade's Sims 2 Collection Creator (© 2006-2007 DJS Sims /     *
 *   The Sims Programming Group), used with permission of the original     *
 *   author (granted 2026-06-26). Real UI port of frmMain (~6,900 lines)   *
 *   pending — see HANDOFF.md.                                              *
 *                                                                          *
 *   This program is free software; you can redistribute it and/or modify   *
 *   it under the terms of the GNU General Public License as published by   *
 *   the Free Software Foundation; either version 2 of the License, or      *
 *   (at your option) any later version.                                    *
 ***************************************************************************/

using System.Drawing;
using System.Windows.Forms;

namespace SimPe.Plugin
{
    /// <summary>
    /// Placeholder shell. Confirms the plugin loads, the IToolFactory wires
    /// into the Tools menu, and ShowDialog opens. Real port of JFade's
    /// frmMain replaces the body of this form.
    /// </summary>
    internal class CollectionCreatorForm : Form
    {
        public CollectionCreatorForm()
        {
            Text = "Collection Creator (work in progress)";
            ClientSize = new Size(420, 140);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            StartPosition = FormStartPosition.CenterParent;

            var label = new Label
            {
                Text = "Collection Creator port is scaffolded — frmMain UI " +
                       "transplant not yet done.\r\n\r\n" +
                       "See SimPe Collection Creator Plugin/HANDOFF.md for " +
                       "remaining work.",
                Location = new Point(12, 12),
                Size = new Size(396, 70),
                AutoSize = false,
            };

            var btnClose = new Button
            {
                Text = "Close",
                DialogResult = DialogResult.Cancel,
                Location = new Point(333, 95),
                Size = new Size(75, 30),
            };

            Controls.Add(label);
            Controls.Add(btnClose);
            CancelButton = btnClose;
            AcceptButton = btnClose;
        }
    }
}
