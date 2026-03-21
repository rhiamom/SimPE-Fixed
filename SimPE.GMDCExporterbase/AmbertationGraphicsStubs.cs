// AmbertationGraphicsStubs.cs — Stubs for Ambertation.Graphics DirectX rendering types.
// The DirectX panel/selection will be replaced with Avalonia OpenGL in a future pass.

using System;

namespace Ambertation.Graphics
{
    /// <summary>Stub for DirectX render panel — will be replaced with Avalonia OpenGL.</summary>
    public class DirectXPanel : Avalonia.Controls.Panel
    {
        public event EventHandler SceneUpdated;
    }

    /// <summary>Stub for render selection helper.</summary>
    public class RenderSelection : Avalonia.Controls.Control
    {
        public IDisposable Scene { get; set; }
    }
}
