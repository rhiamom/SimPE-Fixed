---
name: sims2-collection-format
description: "Binary layout of a Sims 2 collection .package (COLL/BINX/3IDR), reverse-confirmed from real Maxis files"
metadata: 
  node_type: memory
  type: reference
  originSessionId: 39149156-5aff-4883-80e7-897ad35d3327
---

Reverse-confirmed format of a Sims 2 collection `.package`, the target format for the [[collection-creator-simpe-plugin]] port. Real samples live in `~/Library/Application Support/Aspyr/The Sims 2/Collections/` (sibling of the Sims 2 Downloads folder), named `0x<hash>.package`.

**Container:** DBPF v1.1, index minor version 1, **20-byte** index entries (5×uint32: Type, Group, Instance, Offset, Size — NO instance-high field). COLL resource is QFS/refpack compressed (sig `0xFB10`, 3-byte big-endian uncompressed size at bytes 6–8 of the resource).

**Resources in a collection:**
- **1× COLL `0x6C4F359D`** — metadata ONLY, stored as `cGZPropertySetString` XML. Keys: `creatorid`, `flags`, `iconid`/`icongroupid`/`iconrestypeid` (thumbnail pointer), `sortindex`, `stringindex`/`stringsetid`/`stringsetgroupid`/`stringsetrestypeid` (name STR# pointer), `type="collection"`. Does NOT list member objects.
- **1× IMG `0x856DDBAC`** — collection thumbnail.
- **1× STR#** (`0x53545223`) — collection display name.
- **N× (BINX `0x0C560F39` + 3IDR `0xAC506764`) pairs** — one pair per member object, the two sharing an instance ID. This is where membership lives.

**Membership encoding:** the BINX is `cGZPropertySetString` XML with uint keys `binidx` / `objectidx` / `iconidx` / `sortindex` — each is an INDEX into its paired 3IDR's reference array. The 3IDR = header `0xDEADBEEF`, uint32 index-version (2), uint32 count, then count × 16-byte entries in order **(Type, Group, Instance, InstanceHi)**. `objectidx` resolves to the real catalog object's TGI; other indices point at the COLL self-ref / a null icon slot. So adding an object = append a BINX+3IDR pair whose 3IDR references that object.

Open detail: exact type-id of the object reference entry (`0xE9DA450E` in the sample) still to be pinned down. Maps directly onto JFade's `MakeBINX`/`HandleOBJD`/`CombineDats`/`HandleSTRFile`/`PullThumbnail` methods.
