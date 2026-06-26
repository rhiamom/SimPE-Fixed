/***************************************************************************
 *   Copyright (C) 2026 by GramzeSweatshop                                  *
 *   rhiamom@mac.com                                                        *
 *                                                                          *
 *   Built on JFade's Sims 2 Collection Creator (© 2006-2007 DJS Sims /     *
 *   The Sims Programming Group), used with permission of the original     *
 *   author (granted 2026-06-26).                                           *
 *                                                                          *
 *   This program is free software; you can redistribute it and/or modify   *
 *   it under the terms of the GNU General Public License as published by   *
 *   the Free Software Foundation; either version 2 of the License, or      *
 *   (at your option) any later version.                                    *
 ***************************************************************************/

using SimPe.Interfaces;
using SimPe.Interfaces.Files;
using SimPe.Interfaces.Plugin;

namespace SimPe.Plugin
{
    /// <summary>
    /// SimPE Tool entry-point for Collection Creator. PluginManager calls
    /// ShowDialog when the user picks the menu entry; we open the editor
    /// form and return whether the package was changed (so SimPE knows
    /// whether to mark the active document dirty).
    /// </summary>
    public class CollectionCreatorTool : ITool
    {
        readonly IWrapperRegistry reg;
        readonly IProviderRegistry prov;

        internal CollectionCreatorTool(IWrapperRegistry reg, IProviderRegistry prov)
        {
            this.reg = reg;
            this.prov = prov;
        }

        // Collection editing doesn't require an open package — the tool
        // creates new collection .packages from scratch. Always enabled.
        public bool IsEnabled(IPackedFileDescriptor pfd, IPackageFile package) => true;

        public IToolResult ShowDialog(ref IPackedFileDescriptor pfd, ref IPackageFile package)
        {
            using (var form = new CollectionCreatorForm(prov))
            {
                form.ShowDialog();
            }
            // changedPackage=false, changedResource=false — until the port
            // wires save-into-current-package behaviour, the tool only
            // writes standalone collection .package files via its own
            // SaveFileDialog.
            return new ToolResult(false, false);
        }

        // "Object Creation" matches the existing Tools-menu submenu used by
        // other third-party creator tools (Bidou's Career Editor, Theo's Color
        // Binning Tool). PluginManager.LoadMenuItems parses backslash-separated
        // segments into menu levels — picking an existing submenu name puts
        // the entry there instead of creating a parallel one that gets lost.
        public override string ToString() => "Object Creation\\JFade's Collection Creator";
    }
}
