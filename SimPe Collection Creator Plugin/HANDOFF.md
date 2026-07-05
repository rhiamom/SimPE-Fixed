# Collection Creator → SimPE plugin — working handoff

Portable context for resuming the port on another machine (Mac laptop ⇄ Windows PC).
Generated from the Mac during the initial decompile/analysis session.

## Goal

JFade gave permission (2026-06-26) to decompile his **Sims 2 Collection Creator** and
rework it as a **plugin for SimPE**. Turn it into a Tool plugin inside the .NET 8 SimPE
fork (`rhiamom/simPE-Fixed`).

## Build target — DECIDED

**Native in-process SimPE Tool plugin**, built on the Windows PC.

- `rhiamom/simPE-Fixed` builds **`net8.0-windows` + WinForms** (`UseWindowsForms=true`) —
  .NET 8 but Windows-only, which is why dev happens on the PC.
- Collection Creator is *already* WinForms (`frmMain`), so its UI transplants almost
  directly into the plugin — far less work than an Avalonia rewrite.
- **Plugin contract** (unchanged from classic SimPE, confirmed in the fork's
  `SimPE.WorkSpaceHelper/ITool.cs` + `IToolFactory.cs`):
  - implement **`IToolFactory`** — exposes `KnownTools` (your `IToolPlugin[]`),
    receives `LinkedRegistry` / `LinkedProvider` (SimPE hands you its DBPF/wrapper/provider
    machinery), and `FileName`.
  - implement **`ITool`** — `IToolResult ShowDialog(ref IPackedFileDescriptor pfd, ref IPackageFile package)`
    (opens the tool window, returns whether the package changed) + `bool IsEnabled(pfd, package)`.
  - SimPE drop-loads the plugin DLL and wires the tool into its **Tools menu**.
- **Template to copy:** `SimPe Copyright Plugin/Plugin.Copyright.csproj` is the minimal
  tool-plugin project. It references `SimPE.Filehandlers`, `SimPE.Helper`,
  `SimPE.Interfaces`, `SimPE.RCOL`, `SimPE.WorkSpaceHelper`. The Collection Creator plugin
  will be a sibling project with the same refs (+ `SimPE.Packages` for DBPF I/O).

## SimPE-Fixed plugin specifics (from the repo's CLAUDE.md / ARCHITECTURE.md)

- **Plugin discovery:** `PluginManager.cs` scans `bin/Debug/Plugins/` for `*.plugin.dll`
  (tools/wrappers) and `*.wizard.dll`. A project whose **`AssemblyName` ends in `.plugin`**
  is auto-copied there by `Directory.Build.targets`. So name the assembly e.g.
  `simpe.collectioncreator.plugin`.
- **Tool plugin shape** (canonical example in ARCHITECTURE.md §5):
  ```csharp
  public class CollectionCreatorFactory : AbstractWrapperFactory, IToolFactory {
      public IToolPlugin[] KnownTools => new IToolPlugin[] { new CollectionCreatorTool() };
  }
  public class CollectionCreatorTool : AbstractTool, ITool {
      public bool IsEnabled(IPackedFileDescriptor pfd, IPackageFile package) => true;
      public IToolResult ShowDialog(ref IPackedFileDescriptor pfd, ref IPackageFile package) {
          new frmMain().ShowDialog();
          return new ToolResult(false, false);
      }
      public override string ToString() => "Object Tools\\Collection Creator…"; // \\ = submenu
  }
  ```
- **Reuse existing SimPE wrappers instead of CCB.* — big win:**
  - COLL `0x6C4F359D` and BINX `0x0C560F39` are both `cGZPropertySetString` → use the
    **GZPS / CPF** wrapper in `SimPE.Plugin`.
  - **3IDR** `0xAC506764` → existing **3IDR wrapper** in `SimPE.Plugin`.
  - **OBJD / STR# / CTSS** → wrappers in `SimPE.Filehandlers`.
  - DBPF read/write → `GeneratableFile` in `SimPE.Packages`; type constants in
    `SimPE.Helper/MetaData.cs`.
  So `CCB.IO` (DBPF+QFS) and `CCB.Math` (CRC) are dropped entirely; most of `frmMain`'s
  byte-shuffling collapses into wrapper calls.
- **Project refs** (mirror `SimPe Copyright Plugin/Plugin.Copyright.csproj`):
  `SimPE.Interfaces`, `SimPE.Helper`, `SimPE.WorkSpaceHelper`, `SimPE.Filehandlers`,
  `SimPE.RCOL`, plus `SimPE.Packages` and `SimPE.Plugin`. Target `net8.0-windows`,
  `UseWindowsForms=true`, `AssemblyName` ending `.plugin`.
- **Wine caveat** (for the downstream Mac/Linux forks): follow the repo's text-clipping fix
  pattern — `Label`/`LinkLabel` with `TextAlign=BottomLeft`+`AutoSize` clip under Wine; use
  `MiddleLeft` + symmetric `Padding(0,2,0,2)`.

## Porting strategy

**Treat the decompiled source as the SPEC, not the codebase.** Reimplement the mechanism
against SimPE's own libraries.

- JFade wrote it in **VB.NET** at age 16; ILSpy lowered it to C# (full of
  `Microsoft.VisualBasic` runtime calls — `LongType.FromObject`, `StringType.FromObject`,
  `ObjectType.ObjTst`). Everything routes through hex *strings* (read bytes → reverse →
  format hex → parse to decimal → math → re-encode). Functional but naive.
- **Delete and replace** `CCB.IO` (hand-rolled DBPF + QFS) and `CCB.Math`/`Hashes` (CRC) —
  SimPE already has robust DBPF read/write, QFS, and hashing. Reuse SimPE's.
- **Keep / port** the Collection-Creator-specific logic: the COLL/BINX/3IDR assembly,
  category translation (`OBJD.EN.txt`, `MaxisObjectList.txt`), thumbnail handling, and the
  WinForms UI (`frmMain`).

## What a Sims 2 collection actually is (reverse-confirmed)

See `reference/collection-format-spec.md` for the full spec. Confirmed against real Maxis
files in `~/Library/Application Support/Aspyr/The Sims 2/Collections/` (Mac).

- DBPF v1.1, **20-byte** index entries (Type, Group, Instance, Offset, Size).
- **COLL `0x6C4F359D`** — metadata only, a QFS-compressed `cGZPropertySetString` XML:
  icon pointer (`iconid`/`icongroupid`/`iconrestypeid`), name pointer (`stringset*` → STR#),
  `creatorid`, `type="collection"`. *Does not list members.*
- **IMG `0x856DDBAC`** — collection thumbnail. **STR# `0x53545223`** — collection name.
- **N × (BINX `0x0C560F39` + 3IDR `0xAC506764`) pairs** — one per member object, paired by
  instance ID. The BINX (XML) has `binidx`/`objectidx`/`iconidx`/`sortindex` = indices into
  its 3IDR's TGI array; `objectidx` resolves to the real object's TGI. The 3IDR =
  header `0xDEADBEEF`, version 2, count, then count × 16-byte (Type, Group, Instance, InstanceHi).
- **Open detail:** exact type-id of the object-reference entry (`0xE9DA450E` in the sample)
  still to be pinned down.

## Where things are

- **Decompiled C# source:** `decompiled-source/` in this repo. Six assemblies:
  `Sims2CollectionMaker` (WinForms `frmMain` ~6.9k lines = all orchestration), `CCB.IO`,
  `CCB.OBJD`, `CCB.Math`, `CCB.Common`, `CCB.IOTools`.
- **Original binaries (Mac only):** `~/Library/Developer/Sims2CollectionCreator2/` — the
  `.exe`, `CCB.*.dll`, plus data files `OBJD.EN.txt`, `MaxisObjectList.txt`, `Templates/`,
  `Sample Icons/`, `CollectionCreatorManual.pdf`. **Copy these to the PC** — the port needs
  the data/template files even though source is here.
- **SimPE fork:** `rhiamom/simPE-Fixed` (branch `master`), interfaces in
  `SimPE.WorkSpaceHelper/`.
- **Decompiler (if re-running on the Mac):** `ilspycmd` 8.2.0.7535 global tool; needs
  `DOTNET_ROLL_FORWARD=LatestMajor ~/.dotnet/tools/ilspycmd <asm> -p -o <dir>` (only .NET 8
  installed; the 9.x nupkgs are broken).

## Next steps

1. On the PC: clone/refresh `simPE-Fixed`, copy the CC data files from the Mac
   `Sims2CollectionCreator2/` folder.
2. Read `Plugin.Copyright`'s `IToolFactory`/`ITool` implementation as the skeleton.
3. Scaffold a new `Plugin.CollectionCreator` project (`net8.0-windows`, WinForms) in the
   solution, referencing the same SimPE projects (+ `SimPE.Packages`).
4. Transplant `frmMain` UI; rewire it to open via `ITool.ShowDialog`.
5. Replace `CCB.IO`/`CCB.Math` calls with SimPE's package/RCOL/hash APIs.
6. Build the COLL/BINX/3IDR writer against the confirmed format spec; test by round-tripping
   a real collection package.
