---
name: collection-creator-simpe-plugin
description: JFade granted permission to decompile his Collection Creator and turn it into a SimPE plugin
metadata: 
  node_type: memory
  type: project
  originSessionId: 39149156-5aff-4883-80e7-897ad35d3327
---

JFade has given permission (as of 2026-06-26) to decompile his **Collection Creator** and rework it into a **plugin for SimPE**. This is the next tool project after [[ccmerger-for-mac]].

**Why:** Explicit creator permission to decompile is the legal/ethical green light — same clean footing the user insists on for [[ccmerger-for-mac]] (awaiting Lazy Duchess's OK).

**How to apply:** Collection Creator builds Sims 2 collection files (the .package collections that group CAS/Build/Buy catalog items). SimPE is GPL/open-source .NET with a plugin architecture, so the end form is likely a SimPE plugin DLL rather than a standalone port. The user ([[user-sims2-creator]]) wrote SimPE-adjacent tooling and works on Mac, so cross-platform (.NET/Mono/Avalonia) concerns will matter as with [[ccmerger-for-mac]].

**Source location & form:** Binaries at `~/Library/Developer/Sims2CollectionCreator2/` (note: `Library/Developer`, not `~/Developer`). It's a **.NET Framework 1.1** app from 2007, six managed assemblies: `Sims2CollectionMaker.exe` (WinForms `frmMain`, ~6.9k lines, all orchestration), `CCB.IO` (hand-rolled DBPF reader/writer + QFS/refpack decompressor), `CCB.OBJD` (OBJD field reads at fixed offsets — GUID@92, buy-cat@144, subcat@252), `CCB.Math`/`Hashes` (CRC24/CRC32 via `Classless.Hasher`), `CCB.Common`/`CCB.IOTools` (CSV translate + file helpers). Native helpers `dbpf-recompress.exe` + `msvcr71.dll` are NOT managed. COLL resource type id = `0x6C4F359D`, written by `frmMain.HandleCOLLFile`; package assembled by `MakeHeader`/`MakeIndex`/`CombineDats`.

**Originally VB.NET, not C#:** decompiled code is full of `Microsoft.VisualBasic` runtime calls (`LongType.FromObject`, `StringType.FromObject`, `ObjectType.ObjTst`) — JFade wrote it in VB.NET; the ILSpy C# is the cleanest form available, there's no C# source to recover. Coded by JFade at age 16: everything routes through hex *strings* (read bytes → reverse → format hex → parse to decimal → math → re-encode), `MakeByteArray` copy-pasted into 3 assemblies, magic-number DBPF offsets. Functional but naive. **Strategy: treat decompiled source as the format SPEC, reimplement the mechanism against SimPE's existing DBPF/QFS/hash/OBJD libraries** — replace `CCB.IO`/`CCB.Math` wholesale, port only Collection-specific logic.

**Decompile tooling (reproducible):** `ilspycmd` 8.2.0.7535 is installed as a global dotnet tool but needs the .NET 6 runtime (only .NET 8 present); the 9.x nupkgs are broken (missing `DotnetToolSettings.xml`). Working invocation: `DOTNET_ROLL_FORWARD=LatestMajor ~/.dotnet/tools/ilspycmd <asm> -p -o <dir>`.

**BUILD TARGET DECIDED — native .NET 8 in-process plugin.** The user's **SimPE 0.8.3 targets .NET 8** (a modernized fork), so the plugin is built natively with the CC-Merger toolchain (.NET 8, no Wine, no old Framework) AND still loads in-process into SimPE. Best of both paths. The **0.8.3 source is on GitHub** (get URL/owner from user) — that is the authoritative plugin-interface reference, NOT the old `~/Desktop/SimPe_0_77_69-Source/` (whose `SimPe Workspace Helper/ITool.cs` + `IToolFactory.cs` are the 2005-era contract: a Tool plugin implements `IToolFactory` (exposes `KnownTools`, receives `LinkedRegistry`/`LinkedProvider`) + `ITool.ShowDialog(ref pfd, ref package)`/`IsEnabled(...)`; SimPE drop-loads plugin DLLs and wires them into the Tools menu). The 0.8.3 interfaces may differ — read them before building.

**Multi-machine:** user works across a Mac laptop (where decompile happened) and a Windows PC (where the SimPE 0.8.3 dev/GitHub clone lives). Memory carries the decompiled source notes + [[sims2-collection-format]] spec across both.
