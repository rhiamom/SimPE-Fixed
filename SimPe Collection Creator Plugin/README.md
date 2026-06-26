# SimPe Collection Creator Plugin (work in progress)

Porting JFade's Sims 2 Collection Creator into a SimPE Tool plugin, with his permission.

**Start here: [HANDOFF.md](HANDOFF.md)** — status, build-target decision, the reverse-confirmed
collection format (COLL/BINX/3IDR), how it maps onto this fork's plugin system, and next steps.

- `decompiled-source/` — ILSpy decompile of the original tool. **Reference/spec only** — not
  added to the solution and not meant to build (it targets .NET 1.1 / VB-runtime). The real
  plugin will be a new `net8.0-windows` project in this folder reusing SimPE's own wrappers.
- `reference/` — collection-format spec + project notes.
