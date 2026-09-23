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

## Sim names render wrong — language detection (fixed 2026-09-23)

Symptom: every Sim in Sim Description (and anywhere names render) shows a name
that isn't theirs — typically an untouched Maxis placeholder such as
"Adrian Bui" where the hood actually has a renamed Sim like "John Tester" —
while family and lot data look correct. Reported on macOS and present on Linux
identically. Native Windows installs usually escape it because the EA registry
lookup in step 1 resolves and short-circuits first — but **non-registry Windows
repacks hit it too**, so this is a canonical bug, not a Wine-only one.

A Sim's name is not one string. It lives in the Sim's character package in the
CTSS STR# (instance `0x7D0`), with **one name pair per language**:

    lang 0x01   John    Tester     <- English (US): what the player sees in game
    lang 0x02   Adrian  Bui        <- English UK: untouched placeholder
    lang 0x03   Adrien  ...        <- French

The game only writes the slot for the language it is running in, so every other
slot keeps template/placeholder data. Choose the wrong slot and every renamed
Sim displays a stale name. SimPE chose slot 2 because of **two stacked defects**
in `Helper.GetMatchingLanguage()`:

1. `PathProvider.InGameLang` reads the EA `App Paths` registry key for the
   game's language. **When that key was missing it returned the literal string
   `"English"`** — which `GetMatchingLanguage` reads as a Maxis language *name*,
   and in that table plain `"English"` means **UK English** (`"US English"` is
   the US one). Under Wine there is no EA registry at all, so this fired every
   time and short-circuited the culture detection below it. Fixed by returning
   `""` so no case matches and detection continues (`c5e141e`).

2. The culture fallback tested Windows three-letter codes (`ENU`/`ENG`/`ESP`/
   `CHS`) against `ThreeLetterISOLanguageName`, which returns `"eng"` for
   **every** English culture — so `en-US` matched `case "ENG"` (English UK) and
   never reached `"ENU"`. The switch **mixes conventions** (`POR` is ISO 639-2,
   not the Windows `PTG`/`PTB`), so swapping the property outright breaks
   Portuguese. The cases moved into `MatchLanguageCode(string)`, tried with the
   **Windows name first and the ISO name second** (`b9336bd`).

Fixing only #2 accomplishes nothing, because #1 returns before it is reached.

### Gotcha when testing this

A stored `<int name="Language">` in `Data/simpe.xreg` **shadows detection
entirely** — `Registry.LanguageCode` only calls `GetMatchingLanguage()` when the
value is absent:

    object o = rkf.GetValue("Language");
    if (o == null) return Helper.GetMatchingLanguage();
    else           return (Languages)Convert.ToByte(o);

A wrong value written by an older build therefore survives upgrades and makes a
correct fix look broken. To re-test detection, quit SimPE and delete that key —
SimPE rewrites the file on clean exit, so edits made while it is running are
lost.

### Verify, do not assume

Three builds shipped against this bug before it was found, each fixing a real
defect that was not the one in play, because a plausible code path was assumed
to execute without checking. Cheap checks that settle it:

- Is the EA registry actually present?
  `grep -ic "app paths.*sims2" "$WINEPREFIX/system.reg"` (it is 0 under Wine).
- What does the runtime really report? A one-file .NET probe printing
  `CultureInfo.CurrentCulture.ThreeLetterISOLanguageName` next to
  `ThreeLetterWindowsLanguageName`, run under Wine: en-US gives `ENG` vs `ENU`.
- **Do not grep `.package` files for Sim names.** DBPF resources are
  QFS-compressed, so names survive only as fragments ("Adr" + "ian") and greps
  yield false negatives. Parse the DBPF index and decompress the CTSS instead.

## Cross-machine notes

This project is developed across multiple machines. Authoritative state lives on GitHub — local clones on any one machine may lag behind pushed work. When orienting at the start of a session, prefer `git fetch` + `gh api` checks over trusting the local working tree.

## Currently outstanding

- `"FRE"` and `"DUT"` in `GetMatchingLanguage`'s culture switch match neither
  property (.NET returns `fra` and `nld`), so French and Dutch fall through to
  the English default. Pre-existing, untested, left alone deliberately.
