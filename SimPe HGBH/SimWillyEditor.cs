using System;
using System.Collections.Generic;
using System.Text;
using SimPe.PackedFiles.Wrapper;

namespace SimPe.Plugin
{
    class SimWillyEditor : IPenisEditor
    {
        public const uint COCK_TOKEN_GUID = 0xEC191A1B;
        public const uint FEMA_TOKEN_GUID = 0x0036BC01;
        ushort hastoken = 1;
        public ushort[] LoadPenisInformation(SDesc sim)
        {            
            if (sim == null) return null;
            LoadMemoryResource(sim);
            if (sim.CharacterDescription.Gender == SimPe.Data.MetaData.Gender.Female)
            {
                ushort sc = GetFemaleColour(sim);
                ushort ss = GetFemaleSetting(sim);
                ushort[] ret = new ushort[6] { 0, sc, ss, 0, 0, hastoken };
                return ret;
            }
            else
            {
                ushort sl = GetPenisLength(sim);
                ushort sc = GetPenisColour(sim);
                ushort ss = GetPenisState(sim);
                ushort sb = GetBallSize(sim);
                ushort sg = GetPenisGirth(sim);
                ushort[] ret = new ushort[6] { sl, sc, ss, sb, sg, hastoken };
                return ret;
            }
        }

        public void StorePenisInformation(ushort[] diks, SDesc sim)
        {
            if (sim == null || diks == null) return;
            if (diks.Length < 6) return;
            if (!SimPe.Helper.WindowsRegistry.AllowChangeOfSecondaryAspiration || diks[5] == 0) return; // diks[5] is zero unless a change is made
            LoadMemoryResource(sim);
            NgbhItem ni;
            if (sim.CharacterDescription.Gender == SimPe.Data.MetaData.Gender.Female)
            {
                ni = GetFemaleSettingToken(sim, true);
                if (ni == null) return;
                ni.SetValue(2, diks[1]);
                ni.SetValue(3, diks[2]);
            }
            else
            {
                ni = GetPenisSettingToken(sim, true);
                if (ni == null) return;
                ni.SetValue(5, diks[0]);
                ni.SetValue(1, diks[1]);
                ni.SetValue(4, diks[2]);
                ni.SetValue(6, diks[3]);
                ni.SetValue(8, diks[4]);
            }
            ngbh.SynchronizeUserData();
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

        protected NgbhItem GetPenisSettingToken(SDesc sim, bool create)
        {
            if (ngbh == null) return null;
            NgbhSlot slot = ngbh.Sims.GetInstanceSlot(sim.Instance,false);           
            if (slot != null)
            {
                NgbhItem item = slot.FindItem(COCK_TOKEN_GUID);
                if (create && item == null)
                {
                    item = slot.ItemsB.AddNew(SimMemoryType.Token);
                    item.Guid = COCK_TOKEN_GUID;
                    item.Value = 5;
                    for (int i = 1; i < 13; i++)
                        item.PutValue(i, 0);
                }
                return item;
            }
            return null;
        }

        protected NgbhItem GetFemaleSettingToken(SDesc sim, bool create)
        {
            if (ngbh == null) return null;
            NgbhSlot slot = ngbh.Sims.GetInstanceSlot(sim.Instance, false);
            if (slot != null)
            {
                NgbhItem item = slot.FindItem(FEMA_TOKEN_GUID);
                if (create && item == null)
                {
                    item = slot.ItemsB.AddNew(SimMemoryType.Token);
                    item.Guid = FEMA_TOKEN_GUID;
                    item.Value = sim.Instance;
                    item.PutValue(1, sim.CharacterDescription.Realage);
                    item.PutValue(2, 2);
                    item.PutValue(3, 0);
                    item.PutValue(4, 1);
                    item.PutValue(5, showflags(sim));
                }
                return item;
            }
            return null;
        }
        // these are all wrong, have gone by attributes but token setting is not same
        // length is in 5 although it is set to 6 so 0 must be setting 1

        protected ushort GetFemaleColour(SDesc sim)
        {
            NgbhItem ni = GetFemaleSettingToken(sim, false);
            if (ni == null) { hastoken = 0; return 0; }
            hastoken = 1;
            return ni.GetValue(2);
        }
        protected ushort GetFemaleSetting(SDesc sim)
        {
            NgbhItem ni = GetFemaleSettingToken(sim, false);
            if (ni == null) return 0;
            return ni.GetValue(3);
        }

        protected ushort GetPenisLength(SDesc sim)
        {
            NgbhItem ni = GetPenisSettingToken(sim, false);
            if (ni == null) { hastoken = 0; return 0; }
            hastoken = 1;
            return ni.GetValue(5);
        }
        protected ushort GetPenisColour(SDesc sim)
        {
            NgbhItem ni = GetPenisSettingToken(sim, false);
            if (ni == null) return 0;
            return ni.GetValue(1);
        }
        protected ushort GetPenisState(SDesc sim)
        {
            NgbhItem ni = GetPenisSettingToken(sim, false);
            if (ni == null) return 0;
            return ni.GetValue(4);
        }
        protected ushort GetBallSize(SDesc sim)
        {
            NgbhItem ni = GetPenisSettingToken(sim, false);
            if (ni == null) return 0;
            return ni.GetValue(6);
        }
        protected ushort GetPenisGirth(SDesc sim)
        {
            NgbhItem ni = GetPenisSettingToken(sim, false);
            if (ni == null) return 0;
            return ni.GetValue(8);
        }
        private ushort showflags(SDesc sim)
        {
            ushort shflg = 1;
            if (sim.CharacterDescription.GenitaliaFlag.Allways) return 0x00FF;
            if (sim.CharacterDescription.GenitaliaFlag.Undies) shflg += 2;
            if (sim.CharacterDescription.GenitaliaFlag.Pyjamas) shflg += 4;
            if (sim.CharacterDescription.GenitaliaFlag.Swimsuit) shflg += 8;
            if (sim.CharacterDescription.GenitaliaFlag.Casual) shflg += 16;
            if (sim.CharacterDescription.GenitaliaFlag.Gym) shflg += 64;
            return shflg;
        }
    }
}
