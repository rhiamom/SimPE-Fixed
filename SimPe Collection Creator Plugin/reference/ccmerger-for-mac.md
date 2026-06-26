---
name: ccmerger-for-mac
description: "CC-Merger for Mac project — status, location, and licensing of the Avalonia port of Lazy Duchess's CCMerger"
metadata: 
  node_type: memory
  type: project
  originSessionId: 576fcd5b-8933-462f-b498-5cafb1205189
---

**CC-Merger for Mac** — a native macOS port of Lazy Duchess's CCMerger (Sims 2 `.package` merger), built by the user (Rhiamom).

- **Stack:** Avalonia / .NET 8 (net8.0). Same pattern as [[clean-installer-sim-install]].
- **Location:** `~/Library/Developer/CC-Merger for Mac` (note the spaces in the folder name). Original Windows source kept at `~/Library/Developer/CC-Merger-1.4.1`.
- **GitHub:** `rhiamom/CC-Merger-for-Mac` (public, GPLv3). Pushed initial port 2026-06-21.
- **What changed from the Windows original:** WinForms UI → Avalonia; `Delimon.Win32.IO` → `System.IO` (no MAX_PATH limit on macOS); Windows taskbar progress removed; in-window progress reworked to climb 0→100 honestly (original 1.4.1 had overshoot/clamp bugs); per-entry `GC.Collect()` dropped. DBPF merge engine in `Engine/` carried over unchanged → same `.package` output. Downloads picker defaults to the Aspyr Super Collection path (App Store container first, non-sandboxed fallback — see [[clean-installer-locations]]).
- **Licensing nuance:** repo is GPLv3, but the `Engine/` files retain their original **MPL-2.0** headers (CCMerger/FreeSO lineage). MPL-2.0 is GPL-compatible, so this is correct; keep the MPL headers intact. The user initially thought CCMerger was GPL — it's actually MPL-2.0.
- **Verification done:** engine round-trip on 40 real packages → 5 merged, all 2315 entries + sampled bytes intact, progress monotonic to 100.

**Status (2026-06-21):** Port complete & pushed. User sent a courtesy message to Lazy Duchess at MTS asking if she's good with the port (license already permits it; this is etiquette). **Waiting on her reply** before announcing/sharing.

**Next task (tentative, 2026-06-22):** port **Mootilda's Hood Checker** (another Sims 2 tool) to Mac.
