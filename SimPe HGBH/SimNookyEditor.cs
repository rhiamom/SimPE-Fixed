using System;
using System.Collections.Generic;
using System.Text;
using SimPe.PackedFiles.Wrapper;

namespace SimPe.Plugin
{
    class SimNookyEditor : INookyEditor
    {
        public const uint Public_GUID = 0x8CAB091A;
        public const uint NPCs_GUID = 0xADCA2D1B;
        public const uint Plain_GUID = 0x2C8CB358;
        public const uint Group_GUID = 0x00277CF2;
        public const uint Sold_GUID = 0x00845DB3;

        public short[] LoadNookyInformation(SDesc sim)
        {
            if (sim == null || !Helper.IsNeighborhoodFile(sim.Package.FileName)) return new short[5] { 0, 0, 0, 0, 0 };
            LoadMemoryResource(sim);
            NgbhSlot slot = ngbh.Sims.GetInstanceSlot(sim.Instance,false);
            if (slot == null) return new short[5] { 0, 0, 0, 0, 0 };
            short pb = (short)slot.CountItem(Public_GUID);
            short np = (short)slot.CountItem(NPCs_GUID);
            short pl = (short)slot.CountItem(Plain_GUID);
            short gr = (short)slot.CountItem(Group_GUID);
            short so = (short)slot.CountItem(Sold_GUID);
            short[] ret = new short[5] { pb, np, pl, gr, so };
            return ret;
        }

        public void StoreNookyInformation(short[] wooh, SDesc sim)
        {
            if (sim == null || wooh == null) return;
            if (wooh.Length < 5) return;
            if (!SimPe.Helper.WindowsRegistry.AllowChangeOfSecondaryAspiration) return;
            LoadMemoryResource(sim);
            return;
            //NgbhItem ni;
            // cut this all out since storing is not yet supported

            //ngbh.SynchronizeUserData();
        }

        Ngbh ngbh = null;
        SimPe.Interfaces.Files.IPackageFile pkg = null;
        protected void LoadMemoryResource(SDesc sim)
        {
            if (sim!=null && pkg == sim.Package) return;
            if (sim != null) pkg = sim.Package;
            else pkg = null;

            ngbh = null;
            if (sim == null) return;
            if (sim.Package == null) return;

            SimPe.Interfaces.Plugin.IFileWrapper wrapper =
                (SimPe.Interfaces.Plugin.IFileWrapper)FileTable.WrapperRegistry.FindHandler(SimPe.Data.MetaData.MEMORIES);

            if (wrapper == null) return;

            SimPe.Interfaces.Files.IPackedFileDescriptor[] mems = sim.Package.FindFiles(SimPe.Data.MetaData.MEMORIES);
            foreach (SimPe.Interfaces.Files.IPackedFileDescriptor pfd in mems)
            {
                ngbh = new Ngbh(SimPe.FileTable.ProviderRegistry);
                ngbh.ProcessData(pfd, pkg, false);
                return;
            }
        }
    }
}
