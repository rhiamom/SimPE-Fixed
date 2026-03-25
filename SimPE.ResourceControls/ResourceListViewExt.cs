/***************************************************************************
 *   Copyright (C) 2005 by Ambertation                                     *
 *   quaxi@ambertation.de                                                  *
 *                                                                         *
 *   Copyright (C) 2025 by GramzeSweatShop                                 *
 *   rhiamom@mac.com                                                       *
 ***************************************************************************/

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.InteropServices;

namespace SimPe.Windows.Forms
{
    public partial class ResourceListViewExt : Avalonia.Controls.UserControl
    {
        // Stub Handle property for compatibility with ResoureNameSorter which reads parent.Handle
        public IntPtr Handle => IntPtr.Zero;

        ResourceViewManager.ResourceNameList names;
        ResourceViewManager manager;
        SimPe.Windows.Forms.IResourceViewFilter curfilter;
        ResourceViewManager.ResourceNameList lastresources;

        // Avalonia DataGrid replacing the WinForms virtual-mode ListView
        Avalonia.Controls.DataGrid lv;
        ObservableCollection<ResourceListItemExt> rows;

        // Column references for layout management
        Avalonia.Controls.DataGridTextColumn clTNameCol, clTypeCol, clGroupCol,
            clInstHiCol, clInstCol, clOffsetCol, clSizeCol;

        static List<string> colNames = null;
        private List<Avalonia.Controls.DataGridTextColumn> colCols = null;

        public ResourceListViewExt()
        {
            noselectevent = 0;
            rows  = new ObservableCollection<ResourceListItemExt>();
            names = new ResourceViewManager.ResourceNameList();
            lastresources = null;
            sortticket = 0;
            sc = ResourceViewManager.SortColumn.Offset;
            asc = true;

            clTNameCol  = new Avalonia.Controls.DataGridTextColumn { Header = "Name",     Width = new Avalonia.Controls.DataGridLength(106), Binding = new Avalonia.Data.Binding("ColName")   };
            clTypeCol   = new Avalonia.Controls.DataGridTextColumn { Header = "Type",     Width = new Avalonia.Controls.DataGridLength(80),  Binding = new Avalonia.Data.Binding("ColType")   };
            clGroupCol  = new Avalonia.Controls.DataGridTextColumn { Header = "Group",    Width = new Avalonia.Controls.DataGridLength(80),  Binding = new Avalonia.Data.Binding("ColGroup")  };
            clInstHiCol = new Avalonia.Controls.DataGridTextColumn { Header = "Sub Type", Width = new Avalonia.Controls.DataGridLength(67),  Binding = new Avalonia.Data.Binding("ColInstHi") };
            clInstCol   = new Avalonia.Controls.DataGridTextColumn { Header = "Inst",     Width = new Avalonia.Controls.DataGridLength(67),  Binding = new Avalonia.Data.Binding("ColInst")   };
            clOffsetCol = new Avalonia.Controls.DataGridTextColumn { Header = "Offset",   Width = new Avalonia.Controls.DataGridLength(67),  Binding = new Avalonia.Data.Binding("ColOffset") };
            clSizeCol   = new Avalonia.Controls.DataGridTextColumn { Header = "Size",     Width = new Avalonia.Controls.DataGridLength(67),  Binding = new Avalonia.Data.Binding("ColSize")   };

            lv = new Avalonia.Controls.DataGrid
            {
                SelectionMode = Avalonia.Controls.DataGridSelectionMode.Extended,
                CanUserReorderColumns = true,
                CanUserResizeColumns = true,
                IsReadOnly = true,
                AutoGenerateColumns = false,
                ItemsSource = rows,
            };
            lv.Columns.Add(clTNameCol);
            lv.Columns.Add(clTypeCol);
            lv.Columns.Add(clGroupCol);
            lv.Columns.Add(clInstHiCol);
            lv.Columns.Add(clInstCol);
            if (Helper.XmlRegistry.HiddenMode)
            {
                lv.Columns.Add(clOffsetCol);
                lv.Columns.Add(clSizeCol);
            }

            if (!Helper.XmlRegistry.ResourceListShowExtensions)
                lv.Columns.Remove(clTypeCol);

            if (Helper.XmlRegistry.UseBigIcons)
                lv.FontSize = FontSize + 3;

            lv.SelectionChanged += lv_SelectionChanged;
            lv.DoubleTapped     += lv_DoubleTapped;
            lv.PointerReleased  += lv_PointerReleased;
            lv.KeyUp            += lv_KeyUp_Handler;

            Content = lv;

            colCols = new List<Avalonia.Controls.DataGridTextColumn>(
                new[] { clTNameCol, clTypeCol, clGroupCol, clInstHiCol, clInstCol, clOffsetCol, clSizeCol });

            seltimer = new System.Threading.Timer(
                new System.Threading.TimerCallback(SelectionTimerCallback), this,
                System.Threading.Timeout.Infinite, System.Threading.Timeout.Infinite);
        }

        static ResourceListViewExt()
        {
            colNames = new List<string>(new string[] { "Name", "Type", "Group", "InstHi", "Inst", "Offset", "Size" });
        }

        void RebuildRows()
        {
            rows.Clear();
            lock (names)
            {
                foreach (var pfd in names)
                    rows.Add(new ResourceListItemExt(pfd, manager, true));
            }
        }

        public void BeginUpdate()
        {
            noselectevent++;
            selresea = null;
            resselchgea = null;
        }

        public void EndUpdate() { EndUpdate(true); }

        public void EndUpdate(bool fireevents)
        {
            noselectevent--;
            noselectevent = Math.Max(0, noselectevent);
            if (noselectevent <= 0)
            {
                if (fireevents)
                {
                    if (resselchgea != null) SelectionChanged(this, resselchgea);
                    if (selresea != null) SelectedResource(this, selresea);
                }
                resselchgea = null; selresea = null;
            }
        }

        public SimPe.Windows.Forms.IResourceViewFilter Filter
        {
            get { return curfilter; }
            set {
                if (curfilter != value)
                {
                    if (curfilter != null) curfilter.ChangedFilter -= new EventHandler(curfilter_ChangedFilter);
                    curfilter = value;
                    if (curfilter != null) curfilter.ChangedFilter += new EventHandler(curfilter_ChangedFilter);
                }
            }
        }

        void curfilter_ChangedFilter(object sender, EventArgs e)
        {
            ReplaySetResources();
        }

        public void SetResources(ResourceViewManager.ResourceList resources, SimPe.Interfaces.Files.IPackageFile pkg)
        {
            ResourceViewManager.ResourceNameList nn = new ResourceViewManager.ResourceNameList();
            foreach (SimPe.Interfaces.Files.IPackedFileDescriptor pfd in resources)
                nn.Add(new NamedPackedFileDescriptor(pfd, pkg));
            SetResources(nn);
        }

        protected void ReplaySetResources()
        {
            if (lastresources != null) SetResources(lastresources);
        }

        internal void SetResources(ResourceViewManager.ResourceNameList resources)
        {
            ResourceViewManager.ResourceNameList rnl = this.SelectedItems;
            this.Clear();
            seltimer.Change(System.Threading.Timeout.Infinite, System.Threading.Timeout.Infinite);
            this.CancelThreads();
            lock (names)
            {
                foreach (NamedPackedFileDescriptor pfd in names)
                {
                    pfd.Descriptor.ChangedUserData -= new SimPe.Events.PackedFileChanged(Descriptor_ChangedUserData);
                    pfd.Descriptor.DescriptionChanged -= new EventHandler(Descriptor_DescriptionChanged);
                    pfd.Descriptor.ChangedData -= new SimPe.Events.PackedFileChanged(Descriptor_ChangedData);
                }

                names.Clear();

                foreach (NamedPackedFileDescriptor pfd in resources)
                {
                    bool add = true;
                    if (curfilter != null && curfilter.Active)
                        add = !curfilter.IsFiltered(pfd.Descriptor);

                    if (add)
                    {
                        names.Add(pfd);
                        pfd.Descriptor.ChangedData        += new SimPe.Events.PackedFileChanged(Descriptor_ChangedData);
                        pfd.Descriptor.DescriptionChanged += new EventHandler(Descriptor_DescriptionChanged);
                        pfd.Descriptor.ChangedUserData    += new SimPe.Events.PackedFileChanged(Descriptor_ChangedUserData);
                    }
                }

                SortResources();
                // Re-select previously selected items
                foreach (NamedPackedFileDescriptor q in rnl)
                    foreach (var row in rows)
                        if (row.Descriptor == q.Descriptor) { lv.SelectedItem = row; break; }

                lastresources = resources;
                FireSelectionChangedOnUIThread();
            }
        }

        void FireSelectionChangedOnUIThread()
        {
            Avalonia.Threading.Dispatcher.UIThread.Post(OnResourceSelectionChanged);
        }

        public new void Refresh()
        {
            Avalonia.Threading.Dispatcher.UIThread.Post(() => lv.InvalidateArrange());
        }

        void Descriptor_ChangedData(SimPe.Interfaces.Files.IPackedFileDescriptor sender)
        {
            this.Refresh();
        }

        void Descriptor_ChangedUserData(SimPe.Interfaces.Files.IPackedFileDescriptor sender)
        {
            this.Refresh();
        }

        void Descriptor_DescriptionChanged(object sender, EventArgs e)
        {
            if (manager != null && Helper.XmlRegistry.UpdateResourceListWhenTGIChanges)
                manager.UpdateTree();
            this.Refresh();
        }

        public event EventHandler SelectionChanged;

        internal void SetManager(ResourceViewManager manager)
        {
            if (this.manager != manager)
                this.manager = manager;
        }

        public void Clear()
        {
            lv.SelectedItem = null;
            rows.Clear();
            lock (names) { names.Clear(); }
        }

        public new System.Windows.Forms.ContextMenuStrip ContextMenuStrip
        {
            get { return null; }
            set { /* Avalonia context menu would need separate handling */ }
        }

        public void StoreLayout()
        {
            Helper.XmlRegistry.Layout.NameColumnWidth          = (int)clTNameCol.ActualWidth;
            Helper.XmlRegistry.Layout.TypeColumnWidth          = (int)clTypeCol.ActualWidth;
            Helper.XmlRegistry.Layout.GroupColumnWidth         = (int)clGroupCol.ActualWidth;
            Helper.XmlRegistry.Layout.InstanceHighColumnWidth  = (int)clInstHiCol.ActualWidth;
            Helper.XmlRegistry.Layout.InstanceColumnWidth      = (int)clInstCol.ActualWidth;
            Helper.XmlRegistry.Layout.OffsetColumnWidth        = (int)clOffsetCol.ActualWidth;
            Helper.XmlRegistry.Layout.SizeColumnWidth          = (int)clSizeCol.ActualWidth;
        }

        public void RestoreLayout()
        {
            SetColWidth(clTNameCol,  Helper.XmlRegistry.Layout.NameColumnWidth);
            SetColWidth(clTypeCol,   Helper.XmlRegistry.Layout.TypeColumnWidth);
            SetColWidth(clGroupCol,  Helper.XmlRegistry.Layout.GroupColumnWidth);
            SetColWidth(clInstHiCol, Helper.XmlRegistry.Layout.InstanceHighColumnWidth);
            SetColWidth(clInstCol,   Helper.XmlRegistry.Layout.InstanceColumnWidth);
            SetColWidth(clOffsetCol, Helper.XmlRegistry.Layout.OffsetColumnWidth);
            SetColWidth(clSizeCol,   Helper.XmlRegistry.Layout.SizeColumnWidth);
        }

        static void SetColWidth(Avalonia.Controls.DataGridTextColumn col, int width)
        {
            if (width > 0) col.Width = new Avalonia.Controls.DataGridLength(width);
        }
    }
}
