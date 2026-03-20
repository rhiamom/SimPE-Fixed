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
using System.Collections;
using System.Collections.Generic;

namespace Ambertation.Graphics;

public class MeshList : IEnumerable, IDisposable
{
    private List<MeshBox> list = new List<MeshBox>();

    public MeshBox this[int index]
    {
        get => list[index];
        set
        {
            OnRemove(list[index]);
            list[index] = value;
            OnAdd(value);
        }
    }

    public int Count => list.Count;

    public MeshList()
    {
        list = new List<MeshBox>();
    }

    public void Add(MeshBox m)
    {
        OnAdd(m);
        list.Add(m);
    }

    public void Insert(int index, MeshBox m)
    {
        OnAdd(m);
        list.Insert(index, m);
    }

    public void AddRange(MeshBox[] m)
    {
        foreach (MeshBox m2 in m)
        {
            Add(m2);
        }
    }

    public void AddRange(MeshList m)
    {
        foreach (MeshBox item in (IEnumerable)m)
        {
            Add(item);
        }
    }

    protected virtual void OnRemove(MeshBox m)
    {
    }

    protected virtual void OnAdd(MeshBox m)
    {
    }

    public void Remove(MeshBox m)
    {
        OnRemove(m);
        list.Remove(m);
    }

    public void RemoveAt(int index)
    {
        try
        {
            MeshBox m = this[index];
            Remove(m);
        }
        catch
        {
        }
    }

    public void Clear()
    {
        Clear(dispose: false);
    }

    public void Clear(bool dispose)
    {
        if (dispose)
        {
            for (int i = 0; i < list.Count; i++)
            {
                list[i].Dispose();
            }
        }
        list.Clear();
    }

    public bool Contains(MeshBox m)
    {
        return list.Contains(m);
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return list.GetEnumerator();
    }

    public virtual void Dispose()
    {
        if (list != null)
        {
            Clear(dispose: true);
        }
    }
}
