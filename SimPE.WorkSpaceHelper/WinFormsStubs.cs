/***************************************************************************
 *   Copyright (C) 2025 by GramzeSweatshop                                 *
 *   rhiamom@mac.com                                                       *
 *                                                                         *
 *   This program is free software; you can redistribute it and/or modify  *
 *   it under the terms of the GNU General Public License as published by  *
 *   the Free Software Foundation; either version 2 of the License, or     *
 *   (at your option) any later version.                                   *
 *                                                                         *
 *   This program is distributed in the hope that it will be useful,       *
 *   but WITHOUT ANY WARRANTY; without even the implied warranty of        *
 *   MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the         *
 *   GNU General Public License for more details.                          *
 *                                                                         *
 *   You should have received a copy of the GNU General Public License     *
 *   along with this program; if not, write to the                         *
 *   Free Software Foundation, Inc.,                                       *
 *   59 Temple Place - Suite 330, Boston, MA  02111-1307, USA.             *
 ***************************************************************************/

// Minimal WinForms stubs required to compile LoadFileWrappers.cs (Mac/Avalonia port).
// Only the members actually used by ToolMenuItem are provided.

using System;

namespace System.Windows.Forms
{
    [Flags]
    public enum AnchorStyles { None = 0, Top = 1, Bottom = 2, Left = 4, Right = 8 }

    public enum DockStyle { None, Top, Bottom, Left, Right, Fill }

    // MessageBoxButtons, MessageBoxIcon, MessageBoxDefaultButton — defined in SimPe namespace (Message.cs).

    /// <summary>Minimal stub for System.Windows.Forms.Padding.</summary>
    public struct Padding
    {
        public int Left, Top, Right, Bottom;
        public Padding(int all) { Left = Top = Right = Bottom = all; }
        public Padding(int left, int top, int right, int bottom)
        { Left = left; Top = top; Right = right; Bottom = bottom; }
    }

    /// <summary>
    /// Minimal stub for System.Windows.Forms.ToolStripItem.
    /// </summary>
    public class ToolStripItem
    {
        public string Text    { get; set; } = string.Empty;
        public bool   Enabled { get => IsEnabled; set => IsEnabled = value; }
        public bool   IsEnabled { get; set; } = true;
        public bool   Visible { get => IsVisible; set => IsVisible = value; }
        public bool   IsVisible { get; set; } = true;
        public object Tag     { get; set; }
        public object Image   { get; set; }
        public string Name    { get; set; } = "";
        public string ToolTipText { get; set; } = "";
        public System.Drawing.Size Size { get; set; }
        public event EventHandler Click;
        public event EventHandler EnabledChanged;
        protected virtual void OnClick(EventArgs e) => Click?.Invoke(this, e);
    }

    /// <summary>Minimal stub for System.Windows.Forms.ToolStrip.</summary>
    public class ToolStrip
    {
        public string Text { get; set; } = string.Empty;
        public bool Enabled { get; set; } = true;
        public bool Visible { get; set; } = true;
        public bool ShowItemToolTips { get; set; } = true;
        public int ImageScalingSize_Width { get; set; } = 16;
        public System.Drawing.Size ImageScalingSize { get; set; } = new System.Drawing.Size(16, 16);
        public System.Drawing.Point Location { get; set; }
        public System.Drawing.Size Size { get; set; }
        public int TabIndex { get; set; }
        public string Name { get; set; } = "";
        public ToolStripItemCollection Items { get; } = new ToolStripItemCollection();
        public void SuspendLayout() { }
        public void ResumeLayout(bool performLayout = false) { }
        public void PerformLayout() { }
    }

    /// <summary>Minimal stub for System.Windows.Forms.ToolStripButton.</summary>
    public class ToolStripButton : ToolStripItem
    {
        public ToolStripButton() { }
        public ToolStripButton(string text) { Text = text; }
        public bool Checked { get; set; }
        public System.Drawing.Font Font { get; set; }
        public object ImageScaling { get; set; }
        public object Overflow { get; set; }
        public bool Available { get; set; } = true;
        public System.Drawing.ContentAlignment ImageAlign { get; set; }
        public object Alignment { get; set; }
        public object DisplayStyle { get; set; }
        public object TextImageRelation { get; set; }
        public System.Drawing.Rectangle Bounds { get; set; }
        public void SetBounds(System.Drawing.Rectangle r) { }
        public event EventHandler VisibleChanged;
        public event EventHandler AvailableChanged;
        public event EventHandler CheckedChanged;
        protected virtual void OnCheckedChanged(EventArgs e) => CheckedChanged?.Invoke(this, e);
        protected virtual void OnClick(EventArgs e) => Click?.Invoke(this, e);
        public new event EventHandler Click;
    }

    /// <summary>Minimal stub for System.Windows.Forms.ToolTip.</summary>
    public class ToolTip : IDisposable
    {
        public ToolTip() { }
        public ToolTip(System.ComponentModel.IContainer container) { }
        public void SetToolTip(object control, string caption) { }
        public void Dispose() { }
    }

    /// <summary>Minimal stub for System.Windows.Forms.LinkLabel (maps to Button in Avalonia).</summary>
    public class LinkLabel
    {
        public string Text { get; set; } = string.Empty;
        public bool Enabled { get; set; } = true;
        public bool Visible { get; set; } = true;
        public bool TabStop { get; set; }
        public string Name { get; set; } = "";
        public object Tag { get; set; }
        public event LinkLabelLinkClickedEventHandler LinkClicked;
        protected virtual void OnLinkClicked(LinkLabelLinkClickedEventArgs e) => LinkClicked?.Invoke(this, e);
    }

    public class LinkLabelLinkClickedEventArgs : EventArgs
    {
        public LinkLabelLinkClickedEventArgs() { }
    }
    public delegate void LinkLabelLinkClickedEventHandler(object sender, LinkLabelLinkClickedEventArgs e);

    // ToolStripItemCollection and ToolStripMenuItem are NOT in GMDCExporterbase, so define them here.

    /// <summary>Minimal stub for System.Windows.Forms.ToolStripItemCollection.</summary>
    public class ToolStripItemCollection : System.Collections.IEnumerable
    {
        private readonly System.Collections.Generic.List<ToolStripItem> _items = new();
        public int Count => _items.Count;
        public ToolStripItem this[int i] => _items[i];
        public void Add(ToolStripItem item) { _items.Add(item); }
        public void Insert(int index, ToolStripItem item) { _items.Insert(index, item); }
        public void Remove(ToolStripItem item) { _items.Remove(item); }
        public void Clear() { _items.Clear(); }
        public System.Collections.IEnumerator GetEnumerator() => _items.GetEnumerator();
    }

    /// <summary>Minimal stub for System.Windows.Forms.ToolStripMenuItem.</summary>
    public class ToolStripMenuItem : ToolStripItem
    {
        public ToolStripMenuItem() { }
        public ToolStripMenuItem(string text) { Text = text; }
        public bool Checked { get; set; }
        public object ShortcutKeys { get; set; }
        public ToolStripItemCollection DropDownItems { get; } = new ToolStripItemCollection();
        public event EventHandler CheckedChanged;
        public void PerformClick() { }
    }

    // Control and ControlCollection are defined in SimPE.GMDCExporterbase (ListViewEx.cs).
    // WorkSpaceHelper now references GMDCExporterbase via Wizardbase, so they must not be
    // redefined here to avoid CS0433 ambiguity.

    /// <summary>Minimal stub for System.Windows.Forms.FormClosingEventArgs.</summary>
    public class FormClosingEventArgs : EventArgs
    {
        public bool Cancel { get; set; }
        public CloseReason CloseReason { get; }
    }
    public delegate void FormClosingEventHandler(object sender, FormClosingEventArgs e);

    public enum CloseReason { None, WindowsShutDown, MdiFormClosing, UserClosing, TaskManagerClosing, FormOwnerClosing, ApplicationExitCall }

    // Shortcut is defined in SimPE.GMDCExporterbase (ListViewEx.cs) — do not redefine here.

    /// <summary>Minimal stub for System.Windows.Forms.SaveFileDialog.</summary>
    public class SaveFileDialog : IDisposable
    {
        public string FileName { get; set; } = string.Empty;
        public string Filter { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string InitialDirectory { get; set; } = string.Empty;
        public bool OverwritePrompt { get; set; } = true;
        public SimPe.DialogResult ShowDialog() => SimPe.DialogResult.Cancel;
        public void Dispose() { }
    }

    /// <summary>Minimal stub for System.Windows.Forms.FolderBrowserDialog.</summary>
    public class FolderBrowserDialog : IDisposable
    {
        public string SelectedPath { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public System.Environment.SpecialFolder RootFolder { get; set; }
        public SimPe.DialogResult ShowDialog() => SimPe.DialogResult.Cancel;
        public void Dispose() { }
    }

    /// <summary>Minimal stub for System.Windows.Forms.OpenFileDialog.</summary>
    public class OpenFileDialog : IDisposable
    {
        public string FileName { get; set; } = string.Empty;
        public string Filter { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public SimPe.DialogResult ShowDialog() => SimPe.DialogResult.Cancel;
        public void Dispose() { }
    }

    /// <summary>Minimal stub for System.Windows.Forms.Application.</summary>
    public static class Application
    {
        public static void Run(object form) { }
        public static void EnableVisualStyles() { }
        public static void DoEvents() { }
    }
}
