using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace SimPe.PackedFiles.Wrapper
{
    /// <summary>
    /// Apartment Life specific Data
    /// </summary>
    public class SdscApartment : Serializer
    {
        static IPenisEditor willyeditor;
        public static void RegisterWillyEditor(IPenisEditor dk)
        {
            willyeditor = dk;
        }
        static INookyEditor nookyeditor;
        public static void RegisterNookyEditor(INookyEditor nk)
        {
            nookyeditor = nk;
        }

        internal SdscApartment(SDesc dsc)
        {
            parent = dsc;
        }

        SDesc parent;
        short reputation;
        short probabilityToAppear;
        short titlePostName;

        ushort penislength = 0;
        ushort peniscolour = 0;
        ushort penissate = 0;
        ushort ballsize = 0;
        ushort penisgirth = 0;
        ushort penistoken = 0;
        ushort penischang = 0;

        short publicNooky = 0;
        short npcNooky = 0;
        short standardNooky = 0;
        short groupNooky = 0;
        short soldNooky = 0;

        public short Reputation { get { return reputation; } set { reputation = value; } }
        public short ProbabilityToAppear { get { return probabilityToAppear; } set { probabilityToAppear = value; } }
        public short TitlePostName { get { return titlePostName; } set { titlePostName = value; } }

        public ushort PenisLength { get { return penislength; } set { penislength = value; } }
        public ushort PenisColour { get { return peniscolour; } set { peniscolour = value; } }
        public ushort PenisState { get { return penissate; } set { penissate = value; } }
        public ushort BallSize { get { return ballsize; } set { ballsize = value; } }
        public ushort PenisGirth { get { return penisgirth; } set { penisgirth = value; } }
        public ushort PenisChnged { get { return penischang; } set { penischang = value; } }
        public ushort PenisToken { get { return penistoken; } }

        public short PublicNooky { get { return publicNooky; } }
        public short NPCNooky { get { return npcNooky; } }
        public short StandardNooky { get { return standardNooky; } }
        public short GroupNooky { get { return groupNooky; } }
        public short SoldNooky { get { return soldNooky; } }

        internal void Unserialize(BinaryReader reader)
        {
            reader.BaseStream.Seek(0x1D4, SeekOrigin.Begin);
            reputation = reader.ReadInt16();
            probabilityToAppear = reader.ReadInt16();
            titlePostName = reader.ReadInt16();
            if (booby.PrettyGirls.IsTitsInstalled() || booby.PrettyGirls.IsAngelsInstalled())
                LoadPenisInformation();
            LoadNookyInformation();
        }

        internal void Serialize(BinaryWriter writer)
        {
            writer.BaseStream.Seek(0x1D4, SeekOrigin.Begin);
            writer.Write(reputation);
            writer.Write(probabilityToAppear);
            writer.Write(titlePostName);
            if (booby.PrettyGirls.IsTitsInstalled() || booby.PrettyGirls.IsAngelsInstalled())
                StorePenisInformation();
        }

        internal void LoadPenisInformation()
        {
            if (parent == null || willyeditor == null) return;
            ushort[] dic = willyeditor.LoadPenisInformation(this.parent);
            penislength = dic[0];
            peniscolour = dic[1];
            penissate = dic[2];
            ballsize = dic[3];
            penisgirth = dic[4];
            penistoken = dic[5];
        }

        protected void StorePenisInformation()
        {
            if (parent == null || willyeditor == null) return;
            ushort[] dik = new ushort[6] { penislength, peniscolour, penissate, ballsize, penisgirth, penischang };
            willyeditor.StorePenisInformation(dik, this.parent);
        }

        internal void LoadNookyInformation()
        {
            if (parent == null || nookyeditor == null) return;
            short[] woo = nookyeditor.LoadNookyInformation(this.parent);
            publicNooky = woo[0];
            npcNooky = woo[1];
            standardNooky = woo[2];
            groupNooky = woo[3];
            soldNooky = woo[4];
        }

        protected void StoreNookyInformation() // currently all read only so don't use
        {
            if (parent == null || willyeditor == null) return;
            short[] woo = new short[5] { publicNooky, npcNooky, standardNooky, groupNooky, soldNooky };
            nookyeditor.StoreNookyInformation(woo, this.parent);
        }
    }
}