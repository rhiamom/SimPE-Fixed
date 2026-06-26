/***************************************************************************
 *   Copyright (C) 2026 by GramzeSweatshop                                  *
 *   rhiamom@mac.com                                                        *
 *                                                                          *
 *   Built on JFade's Sims 2 Collection Creator (© 2006-2007 DJS Sims /     *
 *   The Sims Programming Group), used with permission of the original     *
 *   author (granted 2026-06-26). The decompiled reference source lives    *
 *   under decompiled-source/; this project rewrites that mechanism        *
 *   against SimPE's own DBPF / wrapper / hash libraries.                   *
 *                                                                          *
 *   This program is free software; you can redistribute it and/or modify   *
 *   it under the terms of the GNU General Public License as published by   *
 *   the Free Software Foundation; either version 2 of the License, or      *
 *   (at your option) any later version.                                    *
 ***************************************************************************/

using SimPe.Interfaces;
using SimPe.Interfaces.Plugin;

namespace SimPe.Plugin
{
    /// <summary>
    /// Exposes the Collection Creator tool to SimPE's plugin loader.
    /// PluginManager discovers this factory via the AbstractWrapperFactory
    /// base + IToolFactory contract, then surfaces each KnownTools entry
    /// in the Tools menu using its ToString() path.
    /// </summary>
    public class CollectionCreatorFactory : AbstractWrapperFactory, IToolFactory
    {
        public CollectionCreatorFactory() { }

        // No file-format wrappers shipped by this plugin — SimPE's existing
        // GZPS/3IDR/STR# wrappers already handle every resource type a
        // collection .package contains.
        public override IWrapper[] KnownWrappers => new IWrapper[0];

        public IToolPlugin[] KnownTools
        {
            get
            {
                return new IToolPlugin[]
                {
                    new CollectionCreatorTool(this.LinkedRegistry, this.LinkedProvider)
                };
            }
        }
    }
}
