# CLAUDE.md

Context for Claude Code (and humans) working in this repo. Project-specific information that isn't obvious from reading the source.

## What this is

**SimPE-Fixed** is the canonical, maintained source for SimPE — the Sims 2 package editor originally by Peter L Jones et al. This fork (rhiamom/SimPE-Fixed) is a .NET 8 / WinForms port that targets Windows natively and is also the upstream source for two cross-platform forks (see below).

Build: `SimPE-Fixed.sln` with `dotnet build`. The main project is `SimPE.Main`. Plugins are loaded dynamically from sibling project DLLs by `PluginManager.cs`.

## Repo family

Three downstream repos depend on this one. Changes here flow downstream:

- **`rhiamom/SimPE-Fixed`** *(this repo, canonical)* — Windows source, default branch `master`.
- **`rhiamom/SimPE-Fixed-Linux`** — fork with patches fixing WinForms/Wine incompatibilities so the app runs under Wine on Linux.
- **`rhiamom/SimPE-Fixed-Mac`** — fork built **on top of** `SimPE-Fixed-Linux` (inherits its Wine fixes), plus Mac-specific additions.
- **`rhiamom/SimPE-Mac-Wine`** — Wineskin wrapper that bundles SimPE-Fixed as a `win-x64` self-contained .NET publish inside a Wine prefix. Vendors this repo as a git submodule at `vendor/simpe-fixed`. Builds → `dist/SimPE-macOS.dmg` (signed, notarized, stapled for outside-store distribution).

When the Wine wrapper releases, the typical sequence is:
1. Land the change here (master), push.
2. In `SimPE-Mac-Wine`, bump `vendor/simpe-fixed` to the new SHA, retire any patches/ entries that became redundant.
3. Build + sign + notarize → DMG.

## Wine compatibility — text clipping pattern

WinForms `Label` / `LinkLabel` controls that use `TextAlign = BottomLeft` together with `AutoSize = true` clip the bottom of their text under Wine. Wine's GDI+ text-metric calculation under-estimates glyph descent for many fonts, so glyphs render below the control's reported bounds and are clipped by the parent.

**Fix pattern** — canonical example is the `trueTarget` / `falseTarget` LinkLabels in `_PJSE/pjse Coder/BhavInstListItemUI.cs` (commit `ca4d496`):

- `TextAlign` → `MiddleLeft` (text centers in the auto-sized box; over-paint is balanced top/bottom instead of clipped at the bottom).
- Add symmetric `Padding(0, 2, 0, 2)` — `GetPreferredSize` includes padding when `AutoSize = true`, so the control grows by 4px and gives a buffer.
- Let the font inherit from the parent rather than pinning small. Small fonts mask the symptom but compromise readability.
- Don't reach for `rowHeight` increases — invasive and changes data density.

Apply this same pattern to any other UI control that exhibits bottom-edge text clipping under Wine.

## Cross-machine notes

This project is developed across multiple machines. Authoritative state lives on GitHub — local clones on any one machine may lag behind pushed work. When orienting at the start of a session, prefer `git fetch` + `gh api` checks over trusting the local working tree.

## Currently outstanding

- Nothing tracked right now.

The `rhiamom/SimPE-Fixed-Linux` fork carries all the Wine-compat fixes: the BHAV row clip fix (`ca4d496`, propagated via a "Merge windows master"), the legacy-file-dialog fix (`AutoUpgradeEnabled = false` on all Open/Save sites — a Wine-only fix that intentionally lives in the Linux/Mac forks, NOT here, since the Vista dialog is the better UX on native Windows), and the FileIndex skip-foreign-files fix (`6d601fe`, cherry-picked). Mac builds pick these up via the vendor bump in `SimPE-Mac-Wine`.
