/***************************************************************************
 *   Copyright (C) 2005 by Ambertation                                     *
 *   quaxi@ambertation.de                                                  *
 *                                                                         *
 *   Copyright (C) 2025 by GramzeSweatshop                                 *
 *   rhiamom@mac.com                                                       *
 *                                                                         *
 *   This program is free software; you can redistribute it and/or modify  *
 *   it under the terms of the GNU General Public License as published by  *
 *   the Free Software Foundation; either version 2 of the License, or     *
 *   (at your option) any later version.                                   *
 ***************************************************************************/

using System;
using System.Collections;
using System.ComponentModel;
using Avalonia.Controls;

namespace Ambertation.Windows.Forms
{
    internal class EnumComboBoxItem
    {
        string name;
        object obj;

        public object Content => obj;
        public string Name    => name;

        internal EnumComboBoxItem(Type type, object obj, System.Resources.ResourceManager rm)
        {
            this.name = obj.ToString();
            if (rm != null)
            {
                string nname = rm.GetString(type.Namespace + "." + type.Name + "." + obj.ToString());
                if (nname != null) name = nname;
            }
            this.obj = obj;
        }

        public override string ToString() => Name;
    }

    /// <summary>
    /// ComboBox that is populated from an enum type, with optional
    /// resource-manager-driven display names.
    /// </summary>
    public class EnumComboBox : ComboBox
    {
        public EnumComboBox() { }

        #region public Properties
        Type myenum;
        public Type Enum
        {
            get => myenum;
            set
            {
                if (value != myenum) { myenum = value; UpdateContent(false); }
            }
        }

        System.Resources.ResourceManager rm;
        public System.Resources.ResourceManager ResourceManager
        {
            get => rm;
            set
            {
                if (value != rm) { rm = value; UpdateContent(true); }
            }
        }

        public new object SelectedValue
        {
            get
            {
                if (SelectedIndex < 0) return null;
                object o = Items[SelectedIndex];
                if (o is EnumComboBoxItem item) return item.Content;
                return o;
            }
            set
            {
                if (value == null)
                {
                    SelectedIndex = -1;
                }
                else
                {
                    Type vtype = value.GetType();
                    int sel = -1;
                    if (vtype.IsEnum)
                    {
                        for (int i = 0; i < Items.Count; i++)
                        {
                            object o = Items[i];
                            if (o is EnumComboBoxItem ei) o = ei.Content;
                            if (((System.Enum)o).CompareTo(value) == 0) { sel = i; break; }
                        }
                    }
                    SelectedIndex = sel;
                }
            }
        }
        #endregion

        public void UpdateContent(bool keepselection)
        {
            int last = SelectedIndex;
            Items.Clear();
            if (myenum != null)
            {
                Array ls = System.Enum.GetValues(myenum);
                foreach (object o in ls)
                    Items.Add(new EnumComboBoxItem(myenum, o, rm));
            }
            if (keepselection)
                SelectedIndex = last < Items.Count ? last : Items.Count - 1;
        }
    }
}
