# GMDC 3D Preview — Fixes to Replicate in the Mac/Avalonia Port

This document lists every change that made the 3D mesh preview work in the Windows/.NET 8 build. Use it as a checklist when porting the preview pipeline to Mac/Linux via Avalonia.

The work spans four commits plus uncommitted tuning:
- `0381a09` — 3D mesh now working (initial breakthrough)
- `f3a7913` — GMDC preview code fix (resilient GL init)
- `17fad23` — GMDC preview code fix (camera/aspect/scale tuning)
- *uncommitted* — matrix order, vertex colors, joint z-test, SceneToMesh transform propagation, diagnostics

Git status as of 2026-04-17: uncommitted tuning still in the working tree.

---

## Files Touched

All paths are relative to repo root.

| File | Role |
|---|---|
| [SimPE.Main/Program.cs](SimPE.Main/Program.cs) | DPI lock at process startup |
| [Ambertation.3D.Gl.Binding/Ambertation/Graphics/DirectXPanel.cs](Ambertation.3D.Gl.Binding/Ambertation/Graphics/DirectXPanel.cs) | OpenTK GL panel: matrix uniforms, aspect, viewport, axis mesh, vertex color path |
| [Ambertation.3D.Gl.Binding/Ambertation/Graphics/GlMesh.cs](Ambertation.3D.Gl.Binding/Ambertation/Graphics/GlMesh.cs) | Mesh VBO: `HasVertexColors` flag |
| [Ambertation.3D.Gl.Binding/Ambertation/Graphics/RenderSelection.cs](Ambertation.3D.Gl.Binding/Ambertation/Graphics/RenderSelection.cs) | Joint-list → scene refresh; event-suppression flag |
| [Ambertation.3D.Gl.Binding/Ambertation/Graphics/ViewportSetting.cs](Ambertation.3D.Gl.Binding/Ambertation/Graphics/ViewportSetting.cs) | Reset defaults: line scale, cam offset |
| [Ambertation.3D.Gl.Binding/Ambertation/Graphics/ViewportSettingBasic.cs](Ambertation.3D.Gl.Binding/Ambertation/Graphics/ViewportSettingBasic.cs) | Reset defaults: joint sphere scale |
| [Ambertation.3D.Gl.Binding/Ambertation/Scenes/SceneToMesh.cs](Ambertation.3D.Gl.Binding/Ambertation/Scenes/SceneToMesh.cs) | Copy per-mesh transform to scene copy; disable Z-test on joint meshes; scale joint bone thickness |
| [SimPE.GMDCExporterbase/fGeometryDataContainer.cs](SimPE.GMDCExporterbase/fGeometryDataContainer.cs) | GMDC editor form: resilient panel creation, preview handler, diagnostic dump |

---

## 1. DPI Lock (Session 1 — commit `0381a09`)

**Problem:** OpenTK `GLControl` changes the process DPI awareness the first time it is created, which reflows every WinForms layout and leaves the app unusable.

**Fix:** Set DPI mode **before any control exists**, in `Main()`.

```csharp
// SimPE.Main/Program.cs, inside Main(), BEFORE any other UI code
Application.SetHighDpiMode(HighDpiMode.SystemAware);
```

**Avalonia note:** Avalonia handles DPI differently (per-window, not per-process). This specific fix is WinForms/OpenTK-only, but the *principle* — initialize GL with a known DPI/scaling context before building layout — still applies. On Mac the equivalent hazard is the NSOpenGLContext pixel scaling on Retina; verify `DrawingContextImpl` / the chosen GL view honours `RenderScaling` consistently.

---

## 2. Matrix Uniform Transpose (Session 1 — commit `0381a09`)

**Problem:** OpenTK matrices are row-major in .NET memory layout, but the original MDX binding treated them as column-major when uploading. Passing `transpose: true` to `GL.UniformMatrix4` caused every mesh to render in the wrong orientation — looked like noise or an empty scene.

**Fix:** Every `GL.UniformMatrix4` call must pass `false` as the transpose argument (matches row-major layout OpenTK already uses).

Grep for `GL.UniformMatrix4` in [DirectXPanel.cs](Ambertation.3D.Gl.Binding/Ambertation/Graphics/DirectXPanel.cs) — every call was flipped from `true` → `false`.

**Avalonia note:** If the Mac port uses Silk.NET or OpenTK again, same rule applies. If it uses a Metal backend via SkiaSharp/`Vulkan`, the transpose behaviour of the chosen math library must be verified against the shader's matrix layout. Document which is which at the top of the panel file.

---

## 3. Matrix Multiplication Order (uncommitted)

**Problem:** `MatrixStack.MultiplyMatrixLocal` multiplied in the wrong order. A "local" multiply means the new matrix is applied in the current frame — i.e. `new * top`, not `top * new`.

**Fix:** In [DirectXPanel.cs:21](Ambertation.3D.Gl.Binding/Ambertation/Graphics/DirectXPanel.cs#L21):

```csharp
// BEFORE
public void MultiplyMatrixLocal(Matrix4 m) { var top = stack.Pop(); stack.Push(Matrix4.Mult(top, m)); }
// AFTER
public void MultiplyMatrixLocal(Matrix4 m) { var top = stack.Pop(); stack.Push(Matrix4.Mult(m, top)); }
```

**Avalonia note:** Porting this is trivial but easy to miss — make the new port reuse the corrected helper, not a re-typed copy.

---

## 4. Resilient GL Panel Creation (Session 2 — commit `f3a7913`)

**Problem:** Original code created the `DirectXPanel` inside `InitializeComponent()` inside a `try/catch (FileNotFoundException)`. When GL init failed with any other exception, the whole form crashed and the editor (buttons, model list, joint list) was unusable.

**Fix in [fGeometryDataContainer.cs](SimPE.GMDCExporterbase/fGeometryDataContainer.cs) constructor:**

1. Call `InitializeComponent()` unconditionally so the rest of the form exists.
2. Remove `dxprev` from the designer-generated code (deleted lines in `InitializeComponent`).
3. Build `dxprev` in a separate `try/catch` in the constructor. On failure, set `dxprev = null` and show a warning.
4. Every later use of `dxprev` must be null-guarded: `dxprev?.Invalidate()`, `if (dxprev != null) { ... }` around layout code, `if (!weak && dxprev != null)` in `ResetPreviewCamera`.

This is the pattern to copy in Avalonia: the editor view-model and non-3D controls must remain functional if GL/Metal init fails.

---

## 5. Camera / Aspect / Viewport Correctness (commit `17fad23` + uncommitted)

### 5a. ResetPreviewCamera — remove stale aspect swap

The original code wrote `Aspect = Height/Width`, called `ResetDefaultViewport`, then wrote `Aspect = Width/Height` again. The middle `ResetDefaultViewport` observed the wrong aspect. In [fGeometryDataContainer.cs:2237-2244](SimPE.GMDCExporterbase/fGeometryDataContainer.cs#L2237-L2244):

```csharp
internal void ResetPreviewCamera(bool weak)
{
    if (!weak && dxprev != null)
    {
        dxprev.Settings.Aspect = (float)dxprev.Width / (float)dxprev.Height;
        dxprev.ResetDefaultViewport();
    }
}
```

### 5b. Compute aspect fresh in ProjectionMatrix

`vp.Aspect` stored a stale value that could be set before the GLControl had real dimensions. Compute from the actual control size per frame, and fall back to the stored value only if control height is zero. In [DirectXPanel.cs:133-136](Ambertation.3D.Gl.Binding/Ambertation/Graphics/DirectXPanel.cs#L133-L136):

```csharp
float fov = vp.FoV, near = vp.NearPlane, far = vp.FarPlane;
float aspect = vp.Aspect > 0f ? vp.Aspect : 1f;
return Matrix4.CreatePerspectiveFieldOfView(fov, aspect, near, far);
```

### 5c. Set GL viewport and aspect at init and on resize

In `OnResetDevice` and `OnResize` in [DirectXPanel.cs](Ambertation.3D.Gl.Binding/Ambertation/Graphics/DirectXPanel.cs):

```csharp
// init path
GL.Viewport(0, 0, Math.Max(1, base.Width), Math.Max(1, base.Height));
vp.Aspect = (float)Math.Max(1, base.Width) / (float)Math.Max(1, base.Height);

// resize path
vp.Aspect = (float)base.Width / (float)base.Height;
```

### 5d. Reset camera whenever the Preview button runs

In [fGeometryDataContainer.cs:2262](SimPE.GMDCExporterbase/fGeometryDataContainer.cs#L2262) — `ResetPreviewCamera(false)` was commented out; re-enabled so the camera refits after the mesh changes.

### 5e. Default camera offset

[ViewportSetting.cs:68](Ambertation.3D.Gl.Binding/Ambertation/Graphics/ViewportSetting.cs#L68): `camoffset` now `1.0f` (started at `1.2f`, briefly tuned to `0.7f` in `17fad23`, settled on `1.0f` in the uncommitted diff).

### 5f. Line and joint-sphere scales

- `lscale` (bone line thickness): `0.1f` → `0.01f` in `ViewportSetting.Reset()`
- `jsz` (joint sphere size): `10f` → `15f` in `ViewportSettingBasic.Reset()` (started at `10f`, went to `1f` in `17fad23`, then `15f` in uncommitted).

These values feed into `AddJointMesh` in [SceneToMesh.cs](Ambertation.3D.Gl.Binding/Ambertation/Scenes/SceneToMesh.cs) and the axis mesh path in [DirectXPanel.cs](Ambertation.3D.Gl.Binding/Ambertation/Graphics/DirectXPanel.cs). When porting, seed the defaults to match, then expose in Settings so users can re-tune for Retina/HiDPI.

**Known issue carried over:** preview panel aspect in the GMDC tab is ~3:1 because the layout is wider than the original 0.75 tool. Skinned clothing meshes render tall/thin. Fix would require layout work on the tab itself, not the GL panel. Flag this when porting the UI.

---

## 6. Joint Rendering Toggle + Z-test (commit `0381a09`, `17fad23`, uncommitted)

Joint spheres/bones overwhelm small body meshes, so default is `RenderJoints = false`. [fGeometryDataContainer.cs:191-192](SimPE.GMDCExporterbase/fGeometryDataContainer.cs#L191-L192):

```csharp
dxprev.Settings.AddAxis = true;
dxprev.Settings.RenderJoints = false;
```

When the user selects a joint in the RenderSelection list, rendering flips on. In [RenderSelection.cs:105-112](Ambertation.3D.Gl.Binding/Ambertation/Graphics/RenderSelection.cs#L105-L112):

```csharp
bool jointSelected = lb.SelectedItem is Joint;
directXPanel.Settings.RenderJoints = jointSelected;
if (!jointSelected)
    directXPanel.Meshes.AddRange(stm.ConvertToDx());
```

Joint meshes render with `ZTest = false` so the skeleton is visible through the body — in [SceneToMesh.cs:62](Ambertation.3D.Gl.Binding/Ambertation/Scenes/SceneToMesh.cs#L62) and the line-mesh block below it:

```csharp
meshBox.Wire = false; meshBox.JointMesh = true; meshBox.ZTest = false;
// ...
foreach (MeshBox mb in arr) { mb.JointMesh = true; mb.ZTest = false; }
```

Bone thickness is scaled to joint size: `linewd: num * 0.2`.

---

## 7. Event-suppression During Scene Setup (commit `0381a09`)

**Problem:** Populating the joint listbox fires `SelectedIndexChanged`, which calls `dx.Reset()`, which clears the meshes you just built.

**Fix in [RenderSelection.cs](Ambertation.3D.Gl.Binding/Ambertation/Graphics/RenderSelection.cs):**

```csharp
private bool suppressLbEvents;

private void SetContent()
{
    // ...
    suppressLbEvents = true;
    try {
        stm = new SceneToMesh(scn, dx);
        lb.Items.Add("--- [Display Mesh] ---");
        foreach (Joint item in scn.JointCollection) lb.Items.Add(item);
        dx.Reset();
        dx.ResetDefaultViewport();
    }
    finally { suppressLbEvents = false; }
}

private void lb_SelectedIndexChanged(object sender, EventArgs e)
{
    if (dx != null && !suppressLbEvents) dx.Reset();
}
```

Also: `dx_ResetDevice` must early-return when `stm == null` so we don't clear meshes before the scene is built.

Also in `ResetDefaultViewport` — set `ignorechangeevent = true` around `ResetView()` so the resize event doesn't fire a second `OnResetDevice` during the reset.

**Avalonia note:** The WinForms `SelectedIndexChanged` chain maps to Avalonia's `SelectionChanged`. Same anti-reentrancy pattern applies.

---

## 8. SceneToMesh — Copy Mesh Transform, Build Joints After (uncommitted)

**Problem:** `GetScene(...)` created a new scene tree but didn't copy per-mesh Translation/Rotation/Scaling, so any GMDC with a mesh transform rendered at identity. Also, joint meshes were being built before mesh conversion, which doesn't matter for rendering but does matter for the joint-meshes-when-selected code path.

**Fix in [SceneToMesh.cs](Ambertation.3D.Gl.Binding/Ambertation/Scenes/SceneToMesh.cs):**

Inside both places that call `scene.CreateMesh(item.Name)` (the plain path and the envelope-filtered path), add:

```csharp
Mesh dst = scene.CreateMesh(item.Name);
dst.Translation = item.Translation;
dst.Rotation = item.Rotation;
dst.Scaling = item.Scaling;
```

In the envelope-filtered overload, move joint-mesh construction to happen **after** `ConvertToDx()` so the meshes are complete before joint visualization is appended.

---

## 9. Vertex-color Material Selection (uncommitted)

**Problem:** `DirectXPanel` always set `useVertexColor = false`, so GMDCs with per-vertex color (most of them) rendered with only the material color.

**Fix in [DirectXPanel.cs:367](Ambertation.3D.Gl.Binding/Ambertation/Graphics/DirectXPanel.cs#L367):**

```csharp
bool useVertexColor = box.Mesh != null && box.Mesh.HasVertexColors;
```

And [GlMesh.cs:37](Ambertation.3D.Gl.Binding/Ambertation/Graphics/GlMesh.cs#L37) gains a `HasVertexColors` property, set to `true` in the two factory methods that accept an `argbColors` array. Private ctor picks up a `bool hasColors = false` parameter.

---

## 10. Diagnostic Dump (uncommitted, optional)

A non-essential `DumpPreviewDiagnostic(GeometryDataContainer)` at [fGeometryDataContainer.cs:2291](SimPE.GMDCExporterbase/fGeometryDataContainer.cs#L2291) writes panel size, camera state, mesh bounding boxes, and MeshBox transforms to `~/Desktop/simpe_preview_diag.txt` after each Preview click. Keep or strip when porting. Useful when the Mac preview looks wrong.

---

## 11. "Correct Joint definition" checkbox is export-only (not a bug)

The `cbCorrect` checkbox on the GMDC form only writes to `Helper.WindowsRegistry.CorrectJointDefinitionOnExport` and is read **only** during export ([fGeometryDataContainer.cs:2920](SimPE.GMDCExporterbase/fGeometryDataContainer.cs#L2920)). It does not affect preview rendering. This was intentional in the original MDX code; label is misleading but not broken. Consider moving/renaming it in the Avalonia port to make that clearer.

---

## Porting Order Suggestion

1. DPI / scaling init before any controls (§1) and the equivalent of the matrix-transpose / mul-order fixes (§2, §3) — without these, nothing else can be verified.
2. Resilient panel creation (§4) so the rest of the form is testable even if GL/Metal isn't ready.
3. Viewport + aspect + reset-camera plumbing (§5).
4. SceneToMesh transforms and vertex-color flag (§8, §9) — these determine whether the mesh matches the Windows output.
5. Joint rendering and event suppression (§6, §7) — last, because the scene has to render correctly before the skeleton overlay is worth debugging.
6. Keep the diagnostic dump (§10) available during porting, remove or gate it before release.

## Reference Commits

```
17fad23 GMDC preview code fix
f3a7913 GMDC preview code fix
0381a09 3D mesh now working  The key fixes were: ...
```

Run `git show <hash>` for the full context on any of these.
