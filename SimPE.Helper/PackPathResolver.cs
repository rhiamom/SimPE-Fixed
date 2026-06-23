/***************************************************************************
 *   Copyright (C) 2026 by GramzeSweatshop                                 *
 *   rhiamom@mac.com                                                       *
 *                                                                         *
 *   This program is free software; you can redistribute it and/or modify  *
 *   it under the terms of the GNU General Public License as published by  *
 *   the Free Software Foundation; either version 2 of the License, or     *
 *   (at your option) any later version.                                   *
 ***************************************************************************/

using System;
using System.Collections.Generic;

namespace SimPe
{
    /// <summary>
    /// Maps GameRootScanner pack folder names to Expansions enum values so
    /// PathProvider can resolve install folders for editions where the EA/Origin
    /// installer didn't write the App Paths registry keys. Without this bridge,
    /// only the base game gets detected (via Helper.BaseGamePath fallback) and
    /// every EP/SP shows up as missing — breaking SDSC labels, BHAV name
    /// resolution, want/fear data, and FileTable indexing for affected installs.
    /// </summary>
    public static class PackPathResolver
    {
        // Case-insensitive leaf-folder-name -> Expansions map. Covers:
        //  - UC nested layout (EPn/SPn/Base inside container folders like
        //    "Double Deluxe", "Fun with Pets", "Best of Business")
        //  - Origin descriptive layout (Apartment Life, Bon Voyage, Free Time,
        //    Seasons, Glamour Life Stuff sitting at the root)
        //  - Disc/Legacy installs ("The Sims 2 University" etc.)
        // Volume-pack bonus folders (VPn) and Support/__Installer folders are
        // intentionally absent — they aren't real EPs/SPs.
        static readonly Dictionary<string, Expansions> Map = new Dictionary<string, Expansions>(StringComparer.OrdinalIgnoreCase)
        {
            // UC numeric codes (leaves of container folders)
            { "Base",  Expansions.BaseGame    },
            { "EP1",   Expansions.University  },
            { "EP2",   Expansions.Nightlife   },
            { "EP3",   Expansions.Business    },
            { "EP4",   Expansions.Pets        },
            { "EP5",   Expansions.Seasons     },
            { "EP6",   Expansions.Voyage      },
            { "EP7",   Expansions.FreeTime    },
            { "EP8",   Expansions.Apartments  },
            { "SP1",   Expansions.FamilyFun   },
            { "SP2",   Expansions.Glamour     },
            { "SP4",   Expansions.Celebrations},
            { "SP5",   Expansions.Fashion     },
            { "SP6",   Expansions.Teen        },
            { "SP7",   Expansions.Kitchens    },
            { "SP8",   Expansions.IKEA        },
            { "SP9",   Expansions.Mansions    },

            // Origin descriptive names sitting at the UC root
            { "The Sims 2",         Expansions.BaseGame    },
            { "University",         Expansions.University  },
            { "Nightlife",          Expansions.Nightlife   },
            { "Open for Business",  Expansions.Business    },
            { "Pets",               Expansions.Pets        },
            { "Seasons",            Expansions.Seasons     },
            { "Bon Voyage",         Expansions.Voyage      },
            { "FreeTime",           Expansions.FreeTime    },
            { "Free Time",          Expansions.FreeTime    },
            { "Apartment Life",     Expansions.Apartments  },
            { "Family Fun Stuff",                          Expansions.FamilyFun    },
            { "Glamour Life Stuff",                        Expansions.Glamour      },
            { "Celebration Stuff",                         Expansions.Celebrations },
            { "Celebrations Stuff",                        Expansions.Celebrations },
            { "H&M Fashion Stuff",                         Expansions.Fashion      },
            { "Teen Style Stuff",                          Expansions.Teen         },
            { "Kitchen and Bath Interior Design Stuff",    Expansions.Kitchens     },
            { "IKEA Home Stuff",                           Expansions.IKEA         },
            { "Mansion and Garden Stuff",                  Expansions.Mansions     },

            // Disc/Legacy classic names (e.g. "The Sims 2 University" sitting
            // as siblings under EA Games/)
            { "The Sims 2 University",        Expansions.University },
            { "The Sims 2 Nightlife",         Expansions.Nightlife  },
            { "The Sims 2 Open for Business", Expansions.Business   },
            { "The Sims 2 Pets",              Expansions.Pets       },
            { "The Sims 2 Seasons",           Expansions.Seasons    },
            { "The Sims 2 Bon Voyage",        Expansions.Voyage     },
            { "The Sims 2 FreeTime",          Expansions.FreeTime   },
            { "The Sims 2 Free Time",         Expansions.FreeTime   },
            { "The Sims 2 Apartment Life",    Expansions.Apartments },
        };

        /// <summary>
        /// Returns the Expansions value matching a scan-found leaf folder name,
        /// or null if the name is unrecognized (volume packs, support folders,
        /// custom repacks).
        /// </summary>
        public static Expansions? Resolve(string leafName)
        {
            if (string.IsNullOrEmpty(leafName)) return null;
            Expansions v;
            return Map.TryGetValue(leafName.Trim(), out v) ? v : (Expansions?)null;
        }

        /// <summary>
        /// Builds a version -> install-folder map from a GameRootScanner result.
        /// Unknown folders are skipped.
        /// </summary>
        public static Dictionary<Expansions, string> BuildMap(GameRootScanResult scan)
        {
            var map = new Dictionary<Expansions, string>();
            if (scan == null) return map;
            foreach (var pack in scan.Packs)
            {
                if (!pack.HasTsData) continue;

                // Base game shortcut. The scanner sets pack.Name = "Base Game"
                // (a display label) whenever it identifies a folder as
                // isBaseGame=true — folder name "Base", "The Sims 2", or the
                // root itself. Our resolver map only has the literal "Base"
                // key, so Resolve("Base Game") returns null and the base game
                // gets silently dropped from PackInstallFolders. Special-case
                // it before falling through to the name lookup.
                if (pack.IsBaseGame)
                {
                    if (!map.ContainsKey(Expansions.BaseGame))
                        map[Expansions.BaseGame] = pack.FullPath;
                    continue;
                }

                Expansions? exp = Resolve(pack.Name);
                if (exp == null) continue;
                // First-write wins. The scanner walks BFS so shallower (more
                // canonical) layouts get registered before duplicates from
                // deeper folders.
                if (!map.ContainsKey(exp.Value))
                    map[exp.Value] = pack.FullPath;
            }
            return map;
        }
    }
}
