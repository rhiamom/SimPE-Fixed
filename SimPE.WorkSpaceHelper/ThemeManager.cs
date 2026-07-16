/***************************************************************************
 *   Copyright (C) 2005 by Ambertation                                     *
 *   quaxi@ambertation.de                                                  *
 *                                                                         *
 *   Copyright (C) 2010 by Chris Hatch                                     *
 *   Ten-theme palette (Everett, Office2003, Whidbey, Glossy, SoftPink,    *
 *   GreenGlossy, DeepPurple, SoftLilac, Psychedelic, Coolblue, Golden),   *
 *   the six-color scheme (Base / Light / Dark / Xdark / Lighter / Mild),  *
 *   and the "extended theme" concept originally shipped in his booby      *
 *   theme system inside GDF.dll (SimPE 0.77 line). Reimplemented here     *
 *   without the GDF.dll dependency; credit for the design is his.         *
 *                                                                         *
 *   Copyright (C) 2025 by GramzeSweatShop                                 *
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

using System;
using System.Drawing;

namespace SimPe.Events
{
    /// <summary>
    /// This Event is called, when the Theme should be changed
    /// </summary>
    public delegate void ChangedThemeEvent(GuiTheme gt);
}

namespace SimPe
{
    /// <summary>
    /// Available Themes. Values 0-3 match the historical 0.77 order so
    /// existing user preference bytes stored in the registry still resolve
    /// to the same theme after upgrade. Values 4-10 were previously provided
    /// by GDF.dll's booby.GuiTheme; reimplemented here without the DLL
    /// dependency.
    /// </summary>
    public enum GuiTheme : byte
    {
        /// <summary>Classic Flat Win2K/Office 2002 Look</summary>
        Everett = 0,
        /// <summary>Office 2003 Look</summary>
        Office2003 = 1,
        /// <summary>VS 2005 Whidbey look</summary>
        Whidbey = 2,
        /// <summary>Glossy blue-grey chrome</summary>
        Glossy = 3,
        /// <summary>Soft pink pastel</summary>
        SoftPink = 4,
        /// <summary>Bright green glossy</summary>
        GreenGlossy = 5,
        /// <summary>Deep purple</summary>
        DeepPurple = 6,
        /// <summary>Soft lilac pastel</summary>
        SoftLilac = 7,
        /// <summary>High-contrast yellow/pink/red</summary>
        Psychedelic = 8,
        /// <summary>Cool blue</summary>
        Coolblue = 9,
        /// <summary>Golden mustard tones</summary>
        Golden = 10,
        /// <summary>Modern dark theme — dark backgrounds, light text.
        /// Not part of the historical 0.77 palette; introduced 2026-07-16.</summary>
        Dark = 11
    }

    /// <summary>
    /// Classes used to manage the Theme of our GUI
    /// </summary>
    public class ThemeManager : System.IDisposable
    {
        #region Fields, Properties, Constructors
        GuiTheme ctheme;
        System.Collections.ArrayList ctrls;

        public GuiTheme CurrentTheme
        {
            get { return ctheme; }
            set
            {
                if (ctheme!=value)
                {
                    ctheme = value;
                    SetTheme();
                    if (ChangedTheme!=null) ChangedTheme(value);
                }
            }
        }

        Color clight, c, cdark;
        System.Windows.Forms.ToolStripRenderer whidbey;
        System.Windows.Forms.ToolStripRenderer whidbeysquare;
        System.Windows.Forms.ToolStripRenderer square;
        MediaPlayerRenderer mediaplayer;
        MediaPlayerRenderer mediaplayerwhidbey;
        ToolStripColorTable colortable;
        MediaPlayerToolStripColorTable mpcolortable;

        Ambertation.Renderer.GlossyRenderer glossy;
        Ambertation.Renderer.GlossyRenderer glossysquare;
        public ThemeManager(GuiTheme t)
        {
            colortable = new ToolStripColorTable();
            mpcolortable = new MediaPlayerToolStripColorTable();

            mediaplayer = new MediaPlayerRenderer();
            mediaplayerwhidbey = new MediaPlayerRenderer(mpcolortable);
            whidbey = new Ambertation.Renderer.AdvancedToolStripProfessionalRenderer(colortable);
            whidbeysquare = new ToolStripProfessionalSquareRenderer(colortable);
            square = new ToolStripProfessionalSquareRenderer();

            glossysquare = new Ambertation.Renderer.GlossyRenderer();
            glossy = new Ambertation.Renderer.GlossyRenderer();
            glossy.RenderRoundedEdges = true;

            ctheme = t;
            parent = null;
            ctrls = new System.Collections.ArrayList();

            Ambertation.Windows.Forms.WhidbeyColorTable rend = new Ambertation.Windows.Forms.WhidbeyColorTable();

            clight = rend.DockButtonHighlightBackgroundBottom;
            c = Ambertation.Drawing.GraphicRoutines.InterpolateColors(rend.DockButtonBackgroundBottom, rend.DockBorderColor, 0.5f); ;
            cdark = rend.DockBorderColor;
        }

        ~ThemeManager()
        {
            try
            {
                this.Dispose();
            }
            catch { }
        }

        /// <summary>
        /// Creates a Child Theme Manager and returns it
        /// </summary>
        /// <returns></returns>
        public ThemeManager CreateChild()
        {
            ThemeManager tm = new ThemeManager(this.ctheme);
            tm.Parent = this;
            return tm;
        }
        #endregion

        #region Apply Themes
        void SetTheme(System.Windows.Forms.ToolStrip sdm)
        {
            if (sdm.Parent is System.Windows.Forms.ToolStripContainer)
            {
                if (ctheme == GuiTheme.Everett) sdm.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
                else if (ctheme == GuiTheme.Office2003) sdm.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional;
                else if (ctheme == GuiTheme.Glossy) sdm.Renderer = glossy;
                else sdm.Renderer = whidbey;
            }
            else
            {
                if (sdm.Renderer is MediaPlayerRenderer)
                {
                    if (ctheme == GuiTheme.Everett) sdm.Renderer = mediaplayerwhidbey;
                    else if (ctheme == GuiTheme.Office2003) sdm.Renderer = mediaplayer;
                    else if (ctheme == GuiTheme.Glossy) sdm.Renderer = glossy;
                    else sdm.Renderer = mediaplayerwhidbey;
                }
                else if (ctheme == GuiTheme.Everett) sdm.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
                else if (ctheme == GuiTheme.Office2003) sdm.Renderer = square;
                else if (ctheme == GuiTheme.Glossy) sdm.Renderer = glossysquare;
                else sdm.Renderer = whidbeysquare;
            }
        }

        void SetTheme(Ambertation.Windows.Forms.DockManager mng)
        {
            if (ctheme == GuiTheme.Everett) mng.Renderer = new Ambertation.Windows.Forms.ClassicRenderer();
            else if (ctheme == GuiTheme.Glossy) mng.Renderer = new Ambertation.Windows.Forms.GlossyRenderer();
            else mng.Renderer = new Ambertation.Windows.Forms.WhidbeyRenderer();
        }

        void SetTheme(System.Windows.Forms.ToolStripContainer sdm)
        {
            SetTheme(sdm.TopToolStripPanel);
            SetTheme(sdm.RightToolStripPanel);
            SetTheme(sdm.BottomToolStripPanel);
            SetTheme(sdm.LeftToolStripPanel);
        }

        void SetTheme(System.Windows.Forms.ToolStripPanel sdm)
        {
            if (sdm.Parent is System.Windows.Forms.ToolStripContainer)
            {
                if (ctheme == GuiTheme.Everett) sdm.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
                else if (ctheme == GuiTheme.Office2003) sdm.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional;
                else if (ctheme == GuiTheme.Glossy) sdm.Renderer = glossy;
                else sdm.Renderer = whidbey;
            }
            else
            {
                if (sdm.Renderer is MediaPlayerRenderer)
                {
                    if (ctheme == GuiTheme.Everett) sdm.Renderer = mediaplayerwhidbey;
                    else if (ctheme == GuiTheme.Office2003) sdm.Renderer = mediaplayer;
                    else if (ctheme == GuiTheme.Glossy) sdm.Renderer = glossy;
                    else sdm.Renderer = mediaplayerwhidbey;
                }
                else if (ctheme == GuiTheme.Everett) sdm.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
                else if (ctheme == GuiTheme.Office2003) sdm.Renderer = square;
                else if (ctheme == GuiTheme.Glossy) sdm.Renderer = glossysquare;
                else sdm.Renderer = whidbeysquare;
            }
        }

        void SetTheme(System.Windows.Forms.MenuStrip sdm)
        {
            if (ctheme == GuiTheme.Everett) sdm.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            else if (ctheme == GuiTheme.Office2003) sdm.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional;
            else if (ctheme == GuiTheme.Glossy) sdm.Renderer = glossy;
            else sdm.Renderer = whidbey;
        }

        void SetTheme(System.Windows.Forms.ContextMenuStrip sdm)
        {
            if (ctheme == GuiTheme.Everett) sdm.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            else if (ctheme == GuiTheme.Office2003) sdm.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional;
            else if (ctheme == GuiTheme.Glossy) sdm.Renderer = glossy;
            else sdm.Renderer = whidbey;
        }


        void SetTheme(System.Windows.Forms.Splitter tb)
        {
            tb.BackColor = ThemeColorDark;
        }

        void SetTheme(System.Windows.Forms.Control c)
        {
            c.BackColor = ThemeColorLight;
            c.ForeColor = ThemeTextColor;
        }

        // LinkLabel exposes a distinct LinkColor (default hardcoded blue)
        // that is NOT the same as ForeColor. On the Dark theme those
        // default blue links become dark-blue-on-dark-bg — unreadable.
        // Reuse ThemeHighlightColor for links so they stay accent-visible
        // on any background.
        void SetTheme(System.Windows.Forms.LinkLabel link)
        {
            link.BackColor = ThemeColorLight;
            link.ForeColor = ThemeTextColor;
            link.LinkColor = ThemeHighlightColor;
            link.ActiveLinkColor = ThemeHighlightColor;
            link.VisitedLinkColor = ThemeHighlightColor;
            link.DisabledLinkColor = ThemeColorMild;
        }

        // Button styling for ExtendedTheme mode. On modern Windows a plain
        // BackColor change (as 0.77 did) is invisible on Buttons because
        // FlatStyle=Standard defers to the OS visual style. Flip to
        // FlatStyle=Flat so BackColor/border/hover values are actually
        // painted, then apply the theme's palette.
        //
        // BackColor is ThemeColorMild (not Light) so buttons stand out
        // from surrounding containers, which use Light — otherwise button
        // and panel blur together on the Dark theme in particular.
        void SetTheme(System.Windows.Forms.Button btn)
        {
            btn.UseVisualStyleBackColor = false;
            btn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btn.BackColor = ThemeColorMild;
            btn.ForeColor = ThemeTextColor;
            btn.FlatAppearance.BorderColor = ThemeColorDark;
            btn.FlatAppearance.BorderSize = 1;
            btn.FlatAppearance.MouseOverBackColor = ThemeColorLighter;
            btn.FlatAppearance.MouseDownBackColor = ThemeColor;
        }

        void SetTheme(System.Windows.Forms.Panel gp)
        {
            gp.BackColor = ThemeColorLight;
            gp.ForeColor = ThemeTextColor;
        }

        void SetTheme(SimPe.Windows.Forms.WrapperBaseControl gp)
        {
            gp.BackColor = ThemeColorLight;
            gp.ForeColor = ThemeTextColor;
            gp.GradientColor = ThemeColor;
        }

        /// <summary>
        /// Apply a Theme to the passed object
        /// </summary>
        /// <param name="o"></param>
        public void Theme(object o)
        {
            if (o is Ambertation.Windows.Forms.DockManager) SetTheme((Ambertation.Windows.Forms.DockManager)o);
            else if (o is System.Windows.Forms.Button) SetTheme((System.Windows.Forms.Button)o);
            // LinkLabel MUST come before Panel/Label/Control checks below —
            // LinkLabel is-a Label so the generic Control handler would
            // otherwise swallow it and skip the LinkColor properties.
            else if (o is System.Windows.Forms.LinkLabel) SetTheme((System.Windows.Forms.LinkLabel)o);
            else if (o is System.Windows.Forms.Panel) SetTheme((System.Windows.Forms.Panel)o);
            else if (o is SimPe.Windows.Forms.WrapperBaseControl) SetTheme((SimPe.Windows.Forms.WrapperBaseControl)o);
            else if (o is System.Windows.Forms.Splitter) SetTheme((System.Windows.Forms.Splitter)o);
            else if (o is System.Windows.Forms.ContextMenuStrip) SetTheme((System.Windows.Forms.ContextMenuStrip)o);
            else if (o is System.Windows.Forms.MenuStrip) SetTheme((System.Windows.Forms.MenuStrip)o);
            else if (o is System.Windows.Forms.ToolStrip) SetTheme((System.Windows.Forms.ToolStrip)o);
            else if (o is System.Windows.Forms.ToolStripContainer) SetTheme((System.Windows.Forms.ToolStripContainer)o);
            else if (o is System.Windows.Forms.Control) SetTheme((System.Windows.Forms.Control)o);
        }

        /// <summary>
        /// Walk the descendant tree of <paramref name="root"/> and apply
        /// the current theme to every themable control (Button, Panel,
        /// Splitter, WrapperBaseControl) found — but only when
        /// <see cref="ExtendedTheme"/> is enabled. A no-op when off, so
        /// safe to always call.
        /// </summary>
        /// <remarks>
        /// Usually you don't need to call this manually — the global idle
        /// hook installed by <see cref="Global"/> takes care of every form
        /// as it opens. Call this explicitly only for controls that live
        /// outside a Form (e.g. a docked plugin surface added dynamically
        /// after the form is already themed).
        /// </remarks>
        public static void ApplyExtendedThemeToButtons(System.Windows.Forms.Control root)
        {
            if (!ExtendedTheme) return;
            if (root == null) return;
            WalkAndTheme(root, Global);
        }

        static void WalkAndTheme(System.Windows.Forms.Control c, ThemeManager mgr)
        {
            // Theme buttons + common container types (whose BackColor shows
            // through to the user). Skip leaf controls like Label / TextBox /
            // CheckBox / ComboBox / ListView — tinting those uniformly with
            // the theme color usually looks wrong; they render on top of a
            // themed container anyway.
            if (c is System.Windows.Forms.Button
                || c is SimPe.Windows.Forms.WrapperBaseControl
                || c is System.Windows.Forms.Panel
                || c is System.Windows.Forms.Splitter
                || c is System.Windows.Forms.Form
                || c is System.Windows.Forms.UserControl
                || c is System.Windows.Forms.TabControl
                || c is System.Windows.Forms.TabPage
                || c is System.Windows.Forms.GroupBox
                || c is System.Windows.Forms.TreeView
                || c is System.Windows.Forms.ListView
                || c is System.Windows.Forms.ListBox
                || c is System.Windows.Forms.TextBoxBase   // TextBox, RichTextBox, MaskedTextBox
                || c is System.Windows.Forms.ComboBox
                || c is System.Windows.Forms.NumericUpDown
                || c is System.Windows.Forms.Label         // includes LinkLabel
                || c is System.Windows.Forms.CheckBox
                || c is System.Windows.Forms.RadioButton)
            {
                mgr.Theme(c);
            }
            foreach (System.Windows.Forms.Control child in c.Controls)
                WalkAndTheme(child, mgr);
        }

        // Global "theme every form as it opens" hook.
        // Application.OpenForms only tracks forms that have been Shown, so
        // an Idle-time walk over that collection reliably catches every
        // new form shortly after it appears — without requiring each form
        // to opt in manually. We track already-processed handles in a
        // HashSet, and remove entries when the form closes so the set
        // doesn't grow forever in a long session.
        private static readonly System.Collections.Generic.HashSet<System.IntPtr> _themedFormHandles
            = new System.Collections.Generic.HashSet<System.IntPtr>();
        private static bool _idleHookInstalled;

        static void EnsureGlobalIdleHook()
        {
            if (_idleHookInstalled) return;
            _idleHookInstalled = true;
            System.Windows.Forms.Application.Idle += OnAppIdle;
        }

        static void OnAppIdle(object sender, System.EventArgs e)
        {
            if (!ExtendedTheme) return;
            foreach (System.Windows.Forms.Form f in System.Windows.Forms.Application.OpenForms)
            {
                if (f == null || !f.IsHandleCreated) continue;
                System.IntPtr h = f.Handle;
                if (_themedFormHandles.Add(h))
                {
                    ApplyExtendedThemeToButtons(f);
                    HookControlAdded(f);
                    // Prune on close so the set doesn't accumulate stale handles.
                    f.FormClosed += (s2, e2) => _themedFormHandles.Remove(h);
                }
            }
        }

        // Recursively subscribe to ControlAdded on every existing descendant
        // so plugin editors and other dynamically-appearing subtrees get
        // themed at the moment they're inserted into an already-themed
        // form (e.g. opening a BHAV/BCON/GLOB wrapper in the docking pane
        // long after MainForm was first walked).
        static void HookControlAdded(System.Windows.Forms.Control parent)
        {
            parent.ControlAdded += OnControlAdded;
            foreach (System.Windows.Forms.Control child in parent.Controls)
                HookControlAdded(child);
        }

        static void OnControlAdded(object sender, System.Windows.Forms.ControlEventArgs e)
        {
            if (!ExtendedTheme || e.Control == null) return;
            WalkAndTheme(e.Control, Global);
            HookControlAdded(e.Control);
        }
        #endregion

        #region Default Colors
        // Palette hex values for themes 4-10 come from disassembling
        // GDF.dll's booby.ThemeManager (0.77 source's __Release folder).
        // Themes 0-3 keep the historical SystemColors-based behavior so
        // existing user setups don't shift visually on upgrade.
        public Color ThemeColor
        {
            get
            {
                switch (ctheme)
                {
                    case GuiTheme.Office2003: return SystemColors.InactiveCaption;
                    case GuiTheme.Everett: return SystemColors.ControlDark;
                    case GuiTheme.Glossy: return Color.FromArgb(0xAD, 0xBC, 0xCE);
                    case GuiTheme.SoftPink: return Color.FromArgb(0xE2, 0xB2, 0xD8);
                    case GuiTheme.GreenGlossy: return Color.FromArgb(0x98, 0xE2, 0x98);
                    case GuiTheme.DeepPurple: return Color.FromArgb(0x64, 0x32, 0xA0);
                    case GuiTheme.SoftLilac: return Color.FromArgb(0xC0, 0xC0, 0xFF);
                    case GuiTheme.Psychedelic: return Color.FromArgb(0xF0, 0xB4, 0x18);
                    case GuiTheme.Coolblue: return Color.FromArgb(0xA2, 0xC8, 0xFF);
                    case GuiTheme.Golden: return Color.FromArgb(0xC8, 0x96, 0x18);
                    case GuiTheme.Dark: return Color.FromArgb(0x3C, 0x3C, 0x3C);
                    default: return c; // Whidbey — initialized from WhidbeyColorTable in ctor
                }
            }
        }

        public Color ThemeColorLight
        {
            get
            {
                switch (ctheme)
                {
                    case GuiTheme.Office2003: return SystemColors.ControlLight;
                    case GuiTheme.Everett: return SystemColors.ControlLight;
                    case GuiTheme.Glossy: return Color.FromArgb(0xDB, 0xE4, 0xEE);
                    case GuiTheme.SoftPink: return Color.FromArgb(0xFF, 0xF2, 0xFC);
                    case GuiTheme.GreenGlossy: return Color.FromArgb(0xFF, 0xFF, 0xD8);
                    case GuiTheme.DeepPurple: return Color.FromArgb(0xF5, 0xE2, 0xFF);
                    case GuiTheme.SoftLilac: return Color.FromArgb(0xFC, 0xF8, 0xFF);
                    case GuiTheme.Psychedelic: return Color.FromArgb(0xFF, 0x60, 0xA0);
                    case GuiTheme.Coolblue: return Color.FromArgb(0xE4, 0xF0, 0xFF);
                    case GuiTheme.Golden: return Color.FromArgb(0xF0, 0xE6, 0xC8);
                    case GuiTheme.Dark: return Color.FromArgb(0x2D, 0x2D, 0x2D);
                    default: return clight;
                }
            }
        }

        public Color ThemeColorDark
        {
            get
            {
                switch (ctheme)
                {
                    case GuiTheme.Office2003: return SystemColors.Highlight;
                    case GuiTheme.Everett: return SystemColors.ControlDarkDark;
                    case GuiTheme.Glossy: return Color.FromArgb(0x75, 0x84, 0x97);
                    case GuiTheme.SoftPink: return Color.FromArgb(0xBE, 0x6C, 0xA0);
                    case GuiTheme.GreenGlossy: return Color.FromArgb(0x08, 0x80, 0x40);
                    case GuiTheme.DeepPurple: return Color.FromArgb(0x32, 0x14, 0x6E);
                    case GuiTheme.SoftLilac: return Color.FromArgb(0x96, 0x80, 0xBE);
                    case GuiTheme.Psychedelic: return Color.FromArgb(0xC8, 0x08, 0x10);
                    case GuiTheme.Coolblue: return Color.FromArgb(0x50, 0x8C, 0xF0);
                    case GuiTheme.Golden: return Color.FromArgb(0x8C, 0x64, 0x0A);
                    case GuiTheme.Dark: return Color.FromArgb(0x1E, 0x1E, 0x1E);
                    default: return cdark;
                }
            }
        }

        /// <summary>Extra-dark shade for heavy accents. New in themes 4-10.</summary>
        public Color ThemeColourXdark
        {
            get
            {
                switch (ctheme)
                {
                    case GuiTheme.Everett: return Color.FromArgb(0x32, 0x32, 0x32);
                    case GuiTheme.Office2003: return Color.FromArgb(0x00, 0x78, 0xD7);
                    case GuiTheme.Whidbey: return Color.FromArgb(0x64, 0x64, 0x64);
                    case GuiTheme.Glossy: return Color.FromArgb(0x2C, 0x3C, 0x4B);
                    case GuiTheme.SoftPink: return Color.FromArgb(0x60, 0x18, 0x3C);
                    case GuiTheme.GreenGlossy: return Color.FromArgb(0x04, 0x40, 0x20);
                    case GuiTheme.DeepPurple: return Color.FromArgb(0x20, 0x04, 0x40);
                    case GuiTheme.SoftLilac: return Color.FromArgb(0x36, 0x18, 0x40);
                    case GuiTheme.Psychedelic: return Color.FromArgb(0x60, 0x04, 0x04);
                    case GuiTheme.Coolblue: return Color.FromArgb(0x18, 0x32, 0x70);
                    case GuiTheme.Golden: return Color.FromArgb(0x50, 0x28, 0x00);
                    case GuiTheme.Dark: return Color.FromArgb(0x0F, 0x0F, 0x0F);
                    default: return cdark;
                }
            }
        }

        /// <summary>Lighter tint used for hover / soft backgrounds. New in themes 4-10.</summary>
        public Color ThemeColorLighter
        {
            get
            {
                switch (ctheme)
                {
                    case GuiTheme.Everett: return Color.FromArgb(0xEE, 0xEE, 0xEE);
                    case GuiTheme.Office2003: return Color.FromArgb(0xDD, 0xEC, 0xF9);
                    case GuiTheme.Whidbey: return Color.FromArgb(0xEE, 0xEE, 0xEE);
                    case GuiTheme.Glossy: return Color.FromArgb(0xEE, 0xF8, 0xFF);
                    case GuiTheme.SoftPink: return Color.FromArgb(0xFF, 0xFA, 0xFF);
                    case GuiTheme.GreenGlossy: return Color.FromArgb(0xE2, 0xFF, 0xF0);
                    case GuiTheme.DeepPurple: return Color.FromArgb(0xFF, 0xEC, 0xFF);
                    case GuiTheme.SoftLilac: return Color.FromArgb(0xF0, 0xEC, 0xFF);
                    case GuiTheme.Psychedelic: return Color.FromArgb(0xFF, 0xFF, 0xC8);
                    case GuiTheme.Coolblue: return Color.FromArgb(0xEE, 0xF8, 0xFF);
                    case GuiTheme.Golden: return Color.FromArgb(0xFF, 0xFA, 0xE9);
                    case GuiTheme.Dark: return Color.FromArgb(0x4A, 0x4A, 0x4A);
                    default: return clight;
                }
            }
        }

        /// <summary>Mid-tone between base and light. New in themes 4-10.</summary>
        public Color ThemeColorMild
        {
            get
            {
                switch (ctheme)
                {
                    case GuiTheme.Everett: return Color.FromArgb(0xA0, 0xA0, 0xA0);
                    case GuiTheme.Office2003: return Color.FromArgb(0xA1, 0xAF, 0xBD);
                    case GuiTheme.Whidbey: return Color.FromArgb(0xDE, 0xDC, 0xD4);
                    case GuiTheme.Glossy: return Color.FromArgb(0xC4, 0xD0, 0xE8);
                    case GuiTheme.SoftPink: return Color.FromArgb(0xEC, 0xCA, 0xEC);
                    case GuiTheme.GreenGlossy: return Color.FromArgb(0xD0, 0xF1, 0xB8);
                    case GuiTheme.DeepPurple: return Color.FromArgb(0xAE, 0x8A, 0xD0);
                    case GuiTheme.SoftLilac: return Color.FromArgb(0xF0, 0xF0, 0xFF);
                    case GuiTheme.Psychedelic: return Color.FromArgb(0xFF, 0xA0, 0x50);
                    case GuiTheme.Coolblue: return Color.FromArgb(0xC8, 0xDC, 0xFF);
                    case GuiTheme.Golden: return Color.FromArgb(0xDC, 0xB4, 0x40);
                    case GuiTheme.Dark: return Color.FromArgb(0x3C, 0x3C, 0x3C);
                    default: return c;
                }
            }
        }

        /// <summary>
        /// Semantic text color for readable text on this theme's
        /// backgrounds. Dark themes flip to a light text color; light
        /// themes return the existing dark accent. Use this instead of
        /// hardcoding ThemeColourXdark in control ForeColor assignments
        /// so future dark themes render legibly without per-theme code.
        /// </summary>
        public Color ThemeTextColor
        {
            get
            {
                switch (ctheme)
                {
                    case GuiTheme.Dark: return Color.FromArgb(0xE0, 0xE0, 0xE0);
                    default: return ThemeColourXdark;
                }
            }
        }

        /// <summary>
        /// Accent color for highlighted or "distinguished" text on this
        /// theme's backgrounds. Used for e.g. compressed-but-not-dirty
        /// resource list items. Light themes reuse the OS Highlight color
        /// (blue); Dark theme uses a light cyan-ish accent that stays
        /// readable on dark backgrounds.
        /// </summary>
        public Color ThemeHighlightColor
        {
            get
            {
                switch (ctheme)
                {
                    case GuiTheme.Dark: return Color.FromArgb(0x4E, 0xC9, 0xB0);
                    default: return SystemColors.Highlight;
                }
            }
        }
        #endregion

        #region Manage
        public void AddControl(object o)
        {
            if (!ctrls.Contains(o))
            {
                ctrls.Add(o);
                Theme(o);
            }
        }

        public void Clear()
        {
            ctrls.Clear();
        }

        public void RemoveControl(object o)
        {
            ctrls.Remove(o);
        }

        public void SetTheme()
        {

            foreach (object o in ctrls) Theme(o);
        }
        #endregion

        #region Events
        protected event SimPe.Events.ChangedThemeEvent ChangedTheme;

        /// <summary>
        /// Called when the Theme in the parent was changed
        /// </summary>
        /// <param name="t"></param>
        void ThemeWasChanged(GuiTheme t)
        {
            this.CurrentTheme = t;
        }

        ThemeManager parent;
        /// <summary>
        /// Set the Parent Theme Manager
        /// </summary>
        public ThemeManager Parent
        {
            get { return parent; }
            set
            {
                if (parent!=null) parent.ChangedTheme -= new SimPe.Events.ChangedThemeEvent(ThemeWasChanged);
                parent = value;
                if (parent!=null) parent.ChangedTheme += new SimPe.Events.ChangedThemeEvent(ThemeWasChanged);
            }
        }


        #endregion

        /// <summary>
        /// Global toggle for extended theming — when true, the selected
        /// theme's colors also drive standard controls (buttons in
        /// particular). Corresponds to 0.77's
        /// <c>booby.ThemeManager.ThemedForms</c>. Persisted in the layout
        /// registry so it survives restarts.
        /// </summary>
        public static bool ExtendedTheme
        {
            get { return Helper.WindowsRegistry.Layout.ExtendedTheme; }
            set { Helper.WindowsRegistry.Layout.ExtendedTheme = value; }
        }

        static ThemeManager tm;
        /// <summary>
        /// Returns the Main ThemeManager
        /// </summary>
        public static ThemeManager Global
        {
            get
            {
                if (tm==null) tm = new ThemeManager((GuiTheme)Helper.WindowsRegistry.Layout.SelectedTheme);
                // Install the app-wide idle hook on first access. Runs
                // once per process, and does nothing if ExtendedTheme is
                // off, so the cost is a single subscription.
                EnsureGlobalIdleHook();
                return tm;
            }
        }
        #region IDisposable Member

        public void Dispose()
        {
            this.Parent = null;
            this.Clear();
        }

        #endregion
    }
}
