/***************************************************************************
 *   Copyright (C) 2005 by Ambertation                                     *
 *   quaxi@ambertation.de                                                  *
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

// ThemeManager — ported for Avalonia.
// All WinForms ToolStrip/DockManager theming removed; only the GuiTheme enum,
// color properties, and the control-list management remain.

using System;
using System.Drawing;

namespace SimPe.Events
{
    /// <summary>This event is called when the Theme should be changed.</summary>
    public delegate void ChangedThemeEvent(GuiTheme gt);
}

namespace SimPe
{
    /// <summary>Available GUI themes.</summary>
    public enum GuiTheme : byte
    {
        Everett    = 0,
        Office2003 = 1,
        Whidbey    = 2,
        Glossy     = 3
    }

    /// <summary>
    /// Manages the GUI theme.  WinForms ToolStrip rendering removed for Avalonia port.
    /// </summary>
    public class ThemeManager : System.IDisposable
    {
        #region Fields
        GuiTheme ctheme;
        System.Collections.ArrayList ctrls;

        Color clight, c, cdark;
        #endregion

        public GuiTheme CurrentTheme
        {
            get { return ctheme; }
            set
            {
                if (ctheme != value)
                {
                    ctheme = value;
                    SetTheme();
                    ChangedTheme?.Invoke(value);
                }
            }
        }

        public ThemeManager(GuiTheme t)
        {
            ctheme = t;
            ctrls  = new System.Collections.ArrayList();

            // Colours derived from the former WhidbeyColorTable stub values.
            var wct = new Ambertation.Windows.Forms.WhidbeyColorTable();
            clight = wct.DockButtonHighlightBackgroundBottom;
            c      = Ambertation.Drawing.GraphicRoutines.InterpolateColors(
                         wct.DockButtonBackgroundBottom, wct.DockBorderColor, 0.5f);
            cdark  = wct.DockBorderColor;
        }

        ~ThemeManager() { try { Dispose(); } catch { } }

        public ThemeManager CreateChild()
        {
            var tm = new ThemeManager(ctheme);
            tm.Parent = this;
            return tm;
        }

        #region Default Colors
        public Color ThemeColor
        {
            get
            {
                if (ctheme == GuiTheme.Office2003) return SystemColors.InactiveCaption;
                if (ctheme == GuiTheme.Everett)    return SystemColors.ControlDark;
                if (ctheme == GuiTheme.Glossy)     return Color.FromArgb(0xAD, 0xBC, 0xCE);
                return c;
            }
        }

        public Color ThemeColorLight
        {
            get
            {
                if (ctheme == GuiTheme.Office2003) return SystemColors.ControlLight;
                if (ctheme == GuiTheme.Everett)    return SystemColors.ControlLight;
                if (ctheme == GuiTheme.Glossy)     return Color.FromArgb(0xDB, 0xE4, 0xEE);
                return clight;
            }
        }

        public Color ThemeColorDark
        {
            get
            {
                if (ctheme == GuiTheme.Office2003) return SystemColors.Highlight;
                if (ctheme == GuiTheme.Everett)    return SystemColors.ControlDarkDark;
                if (ctheme == GuiTheme.Glossy)     return Color.FromArgb(0x75, 0x84, 0x97);
                return cdark;
            }
        }
        #endregion

        #region Apply Themes
        /// <summary>
        /// Apply a theme to the passed control object.
        /// In the Avalonia port only WrapperBaseControl is themed; all
        /// WinForms ToolStrip/DockManager overloads have been removed.
        /// </summary>
        public void Theme(object o)
        {
            if (o is SimPe.Windows.Forms.WrapperBaseControl gp)
            {
                gp.BackgroundColor = ThemeColorLight;
                gp.GradientColor   = ThemeColor;
            }
        }
        #endregion

        #region Manage
        public void AddControl(object o)
        {
            if (!ctrls.Contains(o)) { ctrls.Add(o); Theme(o); }
        }

        public void Clear() { ctrls.Clear(); }

        public void RemoveControl(object o) { ctrls.Remove(o); }

        public void SetTheme()
        {
            foreach (object o in ctrls) Theme(o);
        }
        #endregion

        #region Events
        protected event SimPe.Events.ChangedThemeEvent ChangedTheme;

        void ThemeWasChanged(GuiTheme t) { CurrentTheme = t; }

        ThemeManager _parent;
        public ThemeManager Parent
        {
            get { return _parent; }
            set
            {
                if (_parent != null) _parent.ChangedTheme -= ThemeWasChanged;
                _parent = value;
                if (_parent != null) _parent.ChangedTheme += ThemeWasChanged;
            }
        }
        #endregion

        static ThemeManager tm;
        public static ThemeManager Global
        {
            get
            {
                if (tm == null) tm = new ThemeManager((GuiTheme)Helper.XmlRegistry.Layout.SelectedTheme);
                return tm;
            }
        }

        public void Dispose()
        {
            Parent = null;
            Clear();
        }
    }
}
