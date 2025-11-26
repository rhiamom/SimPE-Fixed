/***************************************************************************
 *   Copyright (C) 2005 by Ambertation                                     *
 *   quaxi@ambertation.de                                                  *
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
using System.Collections;
using System.Collections.Generic;

namespace SimPe.Data
{
	public enum NeighborhoodSlots
	{
		LotsIntern = 0,
		Lots = 1,
		FamiliesIntern = 2,
		Families = 3,
		SimsIntern = 4,
        Sims = 5
	}
	/// <summary>
	/// Determins the concrete Type of an Overlay Item (texture or mesh overlay)
	/// </summary>
	public enum TextureOverlayTypes : uint
	{
		Beard = 0x00,
		EyeBrow = 0x01,
		Lipstick = 0x02,
        Eye = 0x03,
		Mask = 0x04,
		Glasses = 0x05,
		Blush = 0x06,
        EyeShadow = 0x07
	}
	
	/// <summary>
	/// Ages used for Property Sets (Character Data, Skins)
	/// </summary>
	public enum Ages:uint
	{
		Baby = 0x20,
		Toddler = 0x01,
		Child = 0x02,
		Teen = 0x04,
		Adult = 0x08,
		Elder = 0x10,
		YoungAdult = 0x40
	}

	/// <summary>
	/// Categories used for Property Sets (Skins) (Updated by Theo)
	/// </summary>
    [Flags]
    public enum SkinCategories:uint
    {
        Casual1 = 0x01,
        Casual2 = 0x02,
        Casual3 = 0x04,
        Everyday = Casual1 | Casual2 | Casual3,
        Swimmwear = 0x08,
        PJ = 0x10,
        Formal = 0x20,
        Undies = 0x40,
        Skin = 0x80,
        Pregnant = 0x100,
        Activewear = 0x200,
        TryOn = 0x400,
        NakedOverlay = 0x800,
        Outerwear = 0x1000,
        Hair = 0x2000 // does not exist so won't be used but gives me somewhere to shove hair out of the way when browsing for clothes
    }

    /// <summary>
    /// outfit type used for Property Sets (Skins)
    /// </summary>
    [Flags]
    public enum SkinParts : uint
    {
        Hair = 0x01,
        Face = 0x02,
        Top = 0x04,
        Body = 0x08,
        Bottom = 0x10,
        Jewellery = 0x20,
        LongTail = 0x40,
        Ear_Up = 0x80,
        ShortTail = 0x100,
        Ear_Flop = 0x200,
        LongBrushTail = 0x400,
        ShortBrushTail = 0x800,
        SpitzTail = 0x1000,
        LongFlowingTail = 0x2000
    }

    /// <summary>
    /// Categories used for the clothing scanner, the naming above appears and is bloody awfull
    /// </summary>
    [Flags]
    public enum OutfitCats : uint
    {
        Everyday = 0x07,
        Swimsuit = 0x08, // Swimmwear
        Pyjamas = 0x10,
        Formal = 0x20,
        Underwear = 0x40, //Undies
        Skin = 0x80,
        Maternity = 0x100, //Pregnant
        Gym = 0x200, //Activewear
        TryOn = 0x400,
        NakedOverlay = 0x800,
        WinterWear = 0x1000 // Outerwear
    }

    /// <summary>
    /// Gender of a Sim used for Property Sets (Skins & Clothing)
    /// </summary>
    public enum Sex : uint
    {
        Male = 0x02,
        Female = 0x01
    }

    public enum SimGender
    {
        Unspecified = 0,
        Female = 1,
        Male = 2,
        Both = Female | Male
    }

	/// <summary>
	/// 
	/// </summary>
	public enum Majors:uint 
	{
		Unset = 0,
		Unknown = 0xffffffff,
		Art = 0x2e9cf007,
		Biology = 0x4e9cf02b,
		Drama = 0x4e9cf04d,
		Economics = 0xEe9cf044,
		History = 0x2e9cf074,
		Literature = 0xce9cf085,
		Mathematics = 0xee9cf08d,
		Philosophy = 0x2e9cf057,
		Physics = 0xae9cf063,
		PoliticalScience = 0x4e9cf06d,
		Psychology = 0xCE9CF07C,
		Undeclared = 0x8e97bf1d
    }

    /// <summary>
    /// Catalogue Use Flag
    /// </summary>
    public enum ObjCatalogueUseBits : byte
    {
        Adults = 0x00,
        Children = 0x01,
        Group = 0x02,
        Teens = 0x03,
        Elders = 0x04,
        Toddlers = 0x05
    }

	/// <summary>
	/// Room Sort Flag
	/// </summary>
	public enum ObjRoomSortBits:byte
	{
		Kitchen = 0x00,
		Bedroom = 0x01,
		Bathroom = 0x02,
		LivingRoom = 0x03,
		Outside = 0x04,
		DiningRoom = 0x05,
		Misc = 0x06,
		Study = 0x07,
		Kids = 0x08
    }

    /// <summary>
    /// Build type Flag 
    /// </summary>
    public enum ObjBuildTypeBits : byte
    {
        General = 0x00,
        unknown = 0x01,
        Garden = 0x02,
        Openings = 0x03
    }

	/// <summary>
    /// Build Function Sort Flag 
	/// </summary>
	/// <remarks>the higher 2 bytes contains the <see cref="ObjFunctionSortBits"/>, the lower one the actual SubSort</remarks>
    public enum BuildFunctionSubSort : uint
    {
        none = 0x00000,
        General_Columns = 0x10008,
        General_Stairs = 0x10020,
        General_Pool = 0x10040,
        General_TallColumns = 0x10100,
        General_Arch = 0x10200,
        General_Driveway = 0x10400,
        General_Elevator = 0x10800,
        General_Architectural = 0x11000,

        Garden_Trees = 0x40001,
        Garden_Shrubs = 0x40002,
        Garden_Flowers = 0x40004,
        Garden_Objects = 0x40010,

        Openings_Door = 0x80001,
        Openings_TallWindow = 0x80002,
        Openings_Window = 0x80004,
        Openings_Gate = 0x80008,
        Openings_Arch = 0x80010,
        Openings_TallDoor = 0x80100,

        unknown = 0x00069 // just to locate unknown things, is read but not written
    }

    /// <summary>
    /// Community Room Sort Flag
    /// </summary>
    public enum CommRoomSortBits : byte
    {
        Dining = 0x00,
        Shopping = 0x01,
        Outdoor = 0x02,
        Street = 0x03,
        Misc = 0x07
    }

	/// <summary>
	/// Function Sort Flag 
	/// </summary>
	public enum ObjFunctionSortBits:byte
	{
		Seating = 0x00,
		Surfaces = 0x01,
		Appliances = 0x02,
		Electronics = 0x03,
		Plumbing = 0x04,
		Decorative = 0x05,
		General = 0x06,
		Lighting = 0x07,
		Hobbies = 0x08,
		AspirationRewards = 0x0a,
		CareerRewards = 0x0b
	}

	/// <summary>
	/// Function for xml Based Objects
	/// </summary>	
	public enum XObjFunctionSubSort:uint
	{
		Roof = 0x0100,

		Floor_Brick = 0x0201,
		Floor_Carpet = 0x0202,
		Floor_Lino = 0x0204,
		Floor_Poured = 0x0208,
		Floor_Stone = 0x0210,
		Floor_Tile = 0x0220,
		Floor_Wood = 0x0240,
		Floor_Other = 0x0200,

		Fence_Rail = 0x0400,
		Fence_Halfwall = 0x0401,

		Wall_Brick = 0x0501,
		Wall_Masonry = 0x0502,
		Wall_Paint = 0x0504,
		Wall_Paneling = 0x0508,
		Wall_Poured = 0x0510,
		Wall_Siding = 0x0520,
		Wall_Tile = 0x0540,
		Wall_Wallpaper = 0x0580,
		Wall_Other = 0x0500,

		Terrain = 0x0600,

		Hood_Landmark = 0x0701,
		Hood_Flora = 0x0702,
		Hood_Effects = 0x0703,
		Hood_Misc = 0x0704,
		Hood_Stone = 0x0705,
		Hood_Other = 0x0700
	}

	/// <summary>
	/// Function Sort Flag 
	/// </summary>
	/// <remarks>the higher byte contains the <see cref="ObjFunctionSortBits"/>, the lower one the actual SubSort</remarks>
	public enum ObjFunctionSubSort:uint
	{
        none = 0x0000,
		Seating_DiningroomChair = 0x101,
		Seating_LivingroomChair = 0x102,
		Seating_Sofas = 0x104,
		Seating_Beds = 0x108,
		Seating_Recreation = 0x110,
		Seating_UnknownA = 0x120,
		Seating_UnknownB = 0x140,
		Seating_Misc = 0x180,

		Surfaces_Counter = 0x201,
		Surfaces_Table = 0x202,
		Surfaces_EndTable = 0x204,
		Surfaces_Desks = 0x208,
		Surfaces_Coffeetable = 0x210,
		Surfaces_Business = 0x220,
		Surfaces_UnknownB = 0x240,
		Surfaces_Misc = 0x280,

		Decorative_Wall = 0x2001,
		Decorative_Sculpture = 0x2002,
		Decorative_Rugs = 0x2004,
		Decorative_Plants = 0x2008,
		Decorative_Mirror = 0x2010,
		Decorative_Curtain = 0x2020,
		Decorative_UnknownB = 0x2040,
		Decorative_Misc = 0x2080,

		Plumbing_Toilet = 0x1001,
		Plumbing_Shower = 0x1002,
		Plumbing_Sink = 0x1004,
		Plumbing_HotTub = 0x1008,
		Plumbing_UnknownA = 0x1010,
		Plumbing_UnknownB = 0x1020,
		Plumbing_UnknownC = 0x1040,
		Plumbing_Misc = 0x1080,

		Appliances_Cooking = 0x401,
		Appliances_Refrigerator = 0x402,
		Appliances_Small = 0x404,
		Appliances_Large = 0x408,
		Appliances_UnknownA = 0x410,
		Appliances_UnknownB = 0x420,
		Appliances_UnknownC = 0x440,
		Appliances_Misc = 0x480,

		Electronics_Entertainment = 0x801,
		Electronics_TV_and_Computer = 0x802,
		Electronics_Audio = 0x804,
		Electronics_Small = 0x808,
		Electronics_UnknownA = 0x810,
		Electronics_UnknownB = 0x820,
		Electronics_UnknownC = 0x840,
		Electronics_Misc = 0x880,
		
		Lighting_TableLamp = 0x8001,
		Lighting_FloorLamp = 0x8002,
		Lighting_WallLamp = 0x8004,
		Lighting_CeilingLamp = 0x8008,
		Lighting_Outdoor = 0x8010,
		Lighting_UnknownA = 0x8020,
		Lighting_UnknownB = 0x8040,
		Lighting_Misc = 0x8080,
		
		Hobbies_Creative = 0x10001,
		Hobbies_Knowledge = 0x10002,
		Hobbies_Excerising = 0x10004,
		Hobbies_Recreation = 0x10008,
		Hobbies_UnknownA = 0x10010,
		Hobbies_UnknownB = 0x10020,
		Hobbies_UnknownC = 0x10040,
		Hobbies_Misc = 0x10080,

		General_UnknownA = 0x4001,
		General_Dresser = 0x4002,
		General_UnknownB = 0x4004,
		General_Party = 0x4008,
		General_Child = 0x4010,
		General_Car = 0x4020,
		General_Pets = 0x4040,
		General_Misc = 0x4080,
				
		AspirationRewards_UnknownA = 0x40001,
		AspirationRewards_UnknownB = 0x40002,
		AspirationRewards_UnknownC = 0x40004,
		AspirationRewards_UnknownD = 0x40008,
		AspirationRewards_UnknownE = 0x40010,
		AspirationRewards_UnknownF = 0x40020,
		AspirationRewards_UnknownG = 0x40040,
		AspirationRewards_UnknownH = 0x40080,

		CareerRewards_UnknownA = 0x80001,
		CareerRewards_UnknownB = 0x80002,
		CareerRewards_UnknownC = 0x80004,
		CareerRewards_UnknownD = 0x80008,
		CareerRewards_UnknownE = 0x80010,
		CareerRewards_UnknownF = 0x80020,
		CareerRewards_UnknownG = 0x80040,
		CareerRewards_UnknownH = 0x80080,
	}

	/// <summary>
	/// Enumerates known Object Types
	/// </summary>
	public enum ObjectTypes:ushort 
	{
		Unknown = 0x0000,
		Person = 0x0002,
		Normal = 0x0004,
		ArchitecturalSupport = 0x0005,
		SimType = 0x0007,
		Door = 0x0008,
		Window = 0x0009,
		Stairs = 0x000A,
		ModularStairs = 0x000B,
		ModularStairsPortal = 0x000C,
		Vehicle = 0x000D,
		Outfit = 0x000E,
		Memory = 0x000F,
        Template = 0x0010,
        UnlinkedSim = 0x0011,
		Tiles = 0x0013
	}

	/// <summary>
	/// Hold Constants, Enumerations and other Metadata
	/// </summary>
    public class MetaData
    {
        /// <summary>
        /// Color of a Sim that is either Unlinked or does not have Character Data
        /// </summary>
        public static Color SpecialSimColor = Color.FromArgb(0xD0, Color.Black);

        /// <summary>
        /// Color of a Sim that is unlinked
        /// </summary>
        public static Color UnlinkedSim = Color.FromArgb(0xEF, Color.SteelBlue);

        /// <summary>
        /// Color of a NPC Sim
        /// </summary>
        public static Color NPCSim = Color.FromArgb(0xEF, Color.YellowGreen);

        /// <summary>
        /// Color of a Sim that has no Character Data
        /// </summary>
        public static Color InactiveSim = Color.FromArgb(0xEF, Color.LightCoral);

        #region Constants

        /// <summary>
        /// Group for Costum Content
        /// </summary>
        public const UInt32 CUSTOM_GROUP = 0x1C050000;

        /// <summary>
        /// Group for Global Content
        /// </summary>
        public const UInt32 GLOBAL_GROUP = 0x1C0532FA;

        /// <summary>
        /// Group for Local Content
        /// </summary>
        public const UInt32 LOCAL_GROUP = 0xffffffff;

        /// <summary>
        /// A Directory file will have this Type in the fileindex.
        /// </summary>
        public const UInt32 DIRECTORY_FILE = 0xE86B1EEF; //0xEF1E6BE8;

        /// <summary>
        /// Stores the relationship Value for a Sim
        /// </summary>
        public const UInt32 RELATION_FILE = 0xCC364C2A;

        /// <summary>
        /// File Containing Strings
        /// </summary>
        public const UInt32 STRING_FILE = 0x53545223;

        /// <summary>
        /// File Containing Pie Strings
        /// </summary>
        public const UInt32 PIE_STRING_FILE = 0x54544173;

        /// <summary>
        /// File Containing Sim Descriptions
        /// </summary>
        public const UInt32 SIM_DESCRIPTION_FILE = 0xAACE2EFB;

        /// <summary>
        /// Files Containing Sim Images
        /// </summary>
        public const UInt32 SIM_IMAGE_FILE = 0x856DDBAC;

        /// <summary>
        /// The File containing all Family Ties
        /// </summary>
        public const UInt32 FAMILY_TIES_FILE = 0x8C870743;

        /// <summary>
        /// File containing BHAV Informations
        /// </summary>
        public const UInt32 BHAV_FILE = 0x42484156;

        /// <summary>
        /// File containng Global Data
        /// </summary>
        public const UInt32 GLOB_FILE = 0x474C4F42;

        /// <summary>
        /// File Containing Object Data
        /// </summary>
        public const UInt32 OBJD_FILE = 0x4F424A44;

        /// <summary>
        /// File Containing Catalog Strings
        /// </summary>
        public const UInt32 CTSS_FILE = 0x43545353;

        /// <summary>
        /// File Containing Name Maps
        /// </summary>
        public const UInt32 NAME_MAP = 0x4E6D6150;

        /// <summary>
        /// Neighborhood/Memory File Typesss
        /// </summary>
        public const UInt32 MEMORIES = 0x4E474248;


        /// <summary>
        /// Sim DNA
        /// </summary>
        public const uint SDNA = 0xEBFEE33F;

        /// <summary>
        /// Signature identifying a compressed PackedFile
        /// </summary>
        public const ushort COMPRESS_SIGNATURE = 0xFB10;

        public const uint GZPS = 0xEBCF3E27;
        public const uint XWNT = 0xED7D7B4D;
        public const uint REF_FILE = 0xAC506764;
        public const uint IDNO = 0xAC8A7A2E;
        public const uint HOUS = 0x484F5553;
        public const uint SLOT = 0x534C4F54;

        public const uint GMND = 0x7BA3838C;
        public const uint TXMT = 0x49596978;
        public const uint TXTR = 0x1C4A276C;
        public const uint LIFO = 0xED534136;
        public const uint ANIM = 0xFB00791E;
        public const uint SHPE = 0xFC6EB1F7;
        public const uint CRES = 0xE519C933;
        public const uint GMDC = 0xAC4F8687;
        public const uint LDIR = 0xC9C81B9B;
        public const uint LAMB = 0xC9C81BA3;
        public const uint LPNT = 0xC9C81BA9;
        public const uint LSPT = 0xC9C81BAD;

        public const uint MMAT = 0x4C697E5A;
        public const uint XOBJ = 0xCCA8E925;
        public const uint XROF = 0xACA8EA06;
        public const uint XFLR = 0x4DCADB7E;
        public const uint XFNC = 0x2CB230B8;
        public const uint XNGB = 0x6D619378;

        public const uint GLUA = 0x9012468A;
        public const uint OLUA = 0x9012468B;
        public const uint GINV = 0x0ABA73AF;

        public const uint XHTN = 0x8C1580B5;
        public const uint XTOL = 0x2C1FD8A1;
        public const uint XMOL = 0x0C1FE246;
        public const uint XSTN = 0x4C158081;
        public const uint AGED = 0xAC598EAC;
        public const uint LxNR = 0xCCCEF852;
        public const uint BINX = 0x0C560F39;
        #endregion

        #region CEP Strings

        public static string GMND_PACKAGE = System.IO.Path.Combine(PathProvider.SimSavegameFolder, "Downloads\\_EnableColorOptionsGMND.package");
        public static string MMAT_PACKAGE = System.IO.Path.Combine(PathProvider.Global.GetExpansion(Expansions.BaseGame).InstallFolder, "TSData\\Res\\Sims3D\\_EnableColorOptionsMMAT.package");
        public static string ZCEP_FOLDER  = System.IO.Path.Combine(PathProvider.SimSavegameFolder, "zCEP-EXTRA");
        public static string CTLG_FOLDER  = System.IO.Path.Combine(PathProvider.Global.GetExpansion(Expansions.BaseGame).InstallFolder, "TSData\\Res\\Catalog\\zCEP-EXTRA");

        #endregion

        #region Enums

        /// <summary>
        /// Type of school a Sim attends
        /// </summary>
        public enum SchoolTypes : uint
        {
            Unknown = 0xFFFFFFFF,
            NoSchool = 0x00000000,
            PublicSchool = 0xD06788B5,
            PrivateSchool = 0xCC8F4C11,
            SaintTrinians = 0x008BB232
        }

        /// <summary>
        /// Available Grades
        /// </summary>
        public enum Grades : ushort
        {
            Unknown = 0x00,
            F = 0x01,
            DMinus = 0x02,
            D = 0x03,
            DPlus = 0x04,
            CMinus = 0x05,
            C = 0x06,
            CPlus = 0x07,
            BMinus = 0x08,
            B = 0x09,
            BPlus = 0x0A,
            AMinus = 0x0B,
            A = 0x0C,
            APlus = 0x0D
        }

        /// <summary>
        /// Enumerates known Languages - CJH
        /// </summary>
        public enum Languages : byte
        {
            Unknown = 0x00,
            English = 0x01,
            English_uk = 0x02,
            French = 0x03,
            German = 0x04,
            Italian = 0x05,
            Spanish = 0x06,
            Dutch = 0x07,
            Danish = 0x08,
            Swedish = 0x09,
            Norwegian = 0x0a,
            Finnish = 0x0b,
            Hebrew = 0x0c,
            Russian = 0x0d,
            Portuguese = 0x0e,
            Japanese = 0x0f,
            Polish = 0x10,
            SimplifiedChinese = 0x11,
            TraditionalChinese = 0x12,
            Thai = 0x13,
            Korean = 0x14,
            Hindi = 0x15,
            Arabic = 0x16,
            Bulgarian = 0x17,
            Cyrillic = 0x18,
            Ukranian = 0x19,
            Czech = 0x1a,
            Greek = 0x1b,
            Hungarian = 0x1c,
            Icelandic = 0x1d,
            Romanian = 0x1e,
            Latin = 0x1f,
            Slovak = 0x20,
            Albabian = 0x21,
            Turkish = 0x22,
            Brazilian = 0x23,
            SwissFrench = 0x24,
            CanadianFrench = 0x25,
            BelgianFrench = 0x26,
            SwissGerman = 0x27,
            SwissItalian = 0x28,
            BelgianDutch = 0x29,
            Mexican = 0x2a,
            Tagalog = 0x2b,
            Vietnamese = 0x2c
        }

        /// <summary>
        /// Enumerates available Datatypes
        /// </summary>
        public enum DataTypes : uint
        {
            dtUInteger = 0xEB61E4F7,
            dtString = 0x0B8BEA18,
            dtSingle = 0xABC78708,
            dtBoolean = 0xCBA908E1,
            dtInteger = 0x0C264712
        }

        /// <summary>
        /// Available Format Codes
        /// </summary>
        public enum FormatCode : ushort
        {
            normal = 0xFFFD,
        };

        /// <summary>
        /// Is an Item within the PackedFile Index new Alias(0x20 , "or 0x24 Bytes long"),
        /// </summary>
        public enum IndexTypes : uint
        {
            ptShortFileIndex = 0x01,
            ptLongFileIndex = 0x02
        }

        /// <summary>
        /// Which general apiration does a Sim have
        /// </summary>
        public enum AspirationTypes : ushort
        {
            Nothing = 0x00,
            Romance = 0x01,
            Family = 0x02,
            Fortune = 0x04,
            // Power = 0x08,
            Reputation = 0x10,
            Knowledge = 0x20,
            Growup = 0x40,
            Pleasure = 0x80,
            Chees = 0x100
        }

        /// <summary>
        /// Relationships a Sim can have
        /// </summary>
        public enum RelationshipStateBits : byte
        {
            Crush = 0x00,
            Love = 0x01,
            Engaged = 0x02,
            Married = 0x03,
            Friends = 0x04,
            Buddies = 0x05,
            Steady = 0x06,
            Enemy = 0x07,
            Family = 0x0E,
            Known = 0x0F,
        }

        /// <summary>
        /// UIFlags2 - more relationship states
        /// </summary>
        public enum UIFlags2Names : byte
        {
            BestFriendForever = 0x00,
            PlatonicFreind = 0x02,
            SecretLover = 0x03,
        };


        /// <summary>
        /// Available Zodia Signes
        /// </summary>
        public enum ZodiacSignes : ushort
        {
            Aries = 0x01,		 //de: Widder
            Taurus = 0x02,
            Gemini = 0x03,
            Cancer = 0x04,
            Leo = 0x05,
            Virgo = 0x06,		 //de: Jungfrau
            Libra = 0x07,		 //de: Waage
            Scorpio = 0x08,
            Sagittarius = 0x09,  //de: Schütze
            Capricorn = 0x0A,	 //de: Steinbock
            Aquarius = 0x0B,
            Pisces = 0x0C		 //de: Fische
        }

        /// <summary>
        /// Known Types for Family ties
        /// </summary>
        public enum FamilyTieTypes : uint
        {
            MyMotherIs = 0x00,
            MyFatherIs = 0x01,
            ImMarriedTo = 0x02,
            MySiblingIs = 0x03,
            MyChildIs = 0x04
        }

        /// <summary>
        /// Detailed Relationships between Sims
        /// </summary>
        public enum RelationshipTypes : uint
        {
            Unset_Unknown = 0x00,
            Parent = 0x01,
            Child = 0x02,
            Sibling = 0x03,
            Gradparent = 0x04,
            Grandchild = 0x05,
            Nice_Nephew = 0x07,
            Aunt = 0x06,
            Cousin = 0x08,
            Spouses = 0x09,
            Child_Inlaw = 0x0a,
            Parent_Inlaw = 0x0b,
            Sibling_Inlaw = 0x0c
        }

        /// <summary>
        /// Known NPC Types
        /// </summary>
        public enum ServiceTypes : uint
        {
            Normal = 0x00,
            Bartenderb = 0x01,
            Bartenderp = 0x02,
            Boss = 0x03,
            Burglar = 0x04,
            Driver = 0x05,
            Streaker = 0x06,
            Coach = 0x07,
            LunchLady = 0x08,
            Cop = 0x09,
            Delivery = 0x0A,
            Exterminator = 0x0B,
            FireFighter = 0x0C,
            Gardener = 0x0D,
            Barista = 0x0E,
            Grim = 0x0F,
            Handy = 0x10,
            Headmistress = 0x11,
            Matchmaker = 0x12,
            Maid = 0x13,
            MailCarrier = 0x14,
            Nanny = 0x15,
            Paper = 0x16,
            Pizza = 0x17,
            Professor = 0x18,
            EvilMascot = 0x19,
            Repo = 0x1A,
            CheerLeader = 0x1B,
            Mascot = 0x1C,
            SocialBunny = 0x1D,
            SocialWorker = 0x1E,
            Register = 0x1F,
            Therapist = 0x20,
            Chinese = 0x21,
            Podium = 0x22,
            Waitress = 0x23,
            Chef = 0x24,
            DJ = 0x25,
            Crumplebottom = 0x26,
            Vampyre = 0x27,
            Servo = 0x28,
            Reporter = 0x29,
            Salon = 0x2A,
            Wolf = 0x2B,
            WolfLOTP = 0x2C,
            Skunk = 0x2D,
            AnimalControl = 0x2E,
            Obedience = 0x2F,
            Masseuse = 0x30,
            Bellhop = 0x31,
            Villain = 0x32,
            TourGuide = 0x33,
            Hermit = 0x34,
            Ninja = 0x35,
            BigFoot = 0x36,
            Housekeeper = 0x37,
            FoodStandChef = 0x38,
            FireDancer = 0x39,
            WitchDoctor = 0x3A,
            GhostCaptain = 0x3B,
            FoodJudge = 0x3C,
            Genie = 0x3D,
            exDJ = 0x3E,
            exGypsy = 0x3F,
            Witch1 = 0x40,
            Breakdancer = 0x41,
            SpectralCat = 0x42,
            Statue = 0x43,
            Landlord = 0x44,
            Butler = 0x45,
            hotdogchef = 0x46,
            assistant = 0x47,
            exWitch2 = 0x48,
            Mermaid = 0x49,
            MeterMaid = 0x4A,
            Servant = 0x4B,
            Teacher = 0x4C,
            God = 0x4D,
            Preacher = 0x4E,
            TinySim = 0x4F,
            Nurse = 0x50,
            Pandora = 0xAC,
            DMASim = 0xDA,
            icontrol = 0xE9
        }
        
        /// <summary>
        /// How old (in Life Sections) is the Sim
        /// </summary>
        public enum LifeSections : ushort
        {
            Unknown = 0x00,
            Baby = 0x01,
            Toddler = 0x02,
            Child = 0x03,
            Teen = 0x10,
            Adult = 0x13,
            Elder = 0x33,
            YoungAdult = 0x40
        }
        
        /// <summary>
        /// Gender of a Sim
        /// </summary>
        public enum Gender : ushort
        {
            Male = 0x00,
            Female = 0x01
        }

        /// <summary>
        /// The Jobs known by SimPe
        /// </summary>
        /// <remarks>Use finder dock object search for JobData*</remarks>
        public enum Careers : uint
        {
            Unknown = 0xFFFFFFFF,
            Unemployed = 0x00000000,
            TeenElderAthletic = 0xAC89E947,
            TeenElderBusiness = 0x4C1E0577,
            TeenElderCriminal = 0xACA07ACD,
            TeenElderCulinary = 0x4CA07B0C,
            TeenElderLawEnforcement = 0x6CA07B39,
            TeenElderMedical = 0xAC89E918,
            TeenElderMilitary = 0xCCA07B66,
            TeenElderPolitics = 0xCCA07B8D,
            TeenElderScience = 0xECA07BB0,
            TeenElderSlacker = 0x6CA07BDC,
            TeenElderAdventurer = 0xF240D235,
            TeenElderEducation = 0xD243BBEC,
            TeenElderGamer = 0x1240C962,
            TeenElderJournalism = 0x5240E212,
            TeenElderLaw = 0x1243BBDE,
            TeenElderMusic = 0xB243BBD2,
            TeenElderConstruction = 0x53E1C30F,
            TeenElderDance = 0xD3E094A5,
            TeenElderEntertainment = 0x53E09494,
            TeenElderIntelligence = 0x93E094C0,
            TeenElderOcenography = 0x13E09443,
            TeenElderCrafter = 0xF3A37D20,
            TeenElderGatherer = 0xB3A37CE1,
            TeenElderHunter = 0x7383E1DD,
            Military = 0x6C9EBD32,
            Politics = 0x2C945B14,
            Science = 0x0C9EBD47,
            Medical = 0x0C7761FD,
            Athletic = 0x2C89E95F,
            Economy = 0x45196555,
            LawEnforcement = 0xAC9EBCE3,
            Culinary = 0xEC9EBD5F,
            Slacker = 0xEC77620B,
            Criminal = 0x6C9EBD0E,
            Paranormal = 0x2E6FFF87,
            NaturalScientist = 0xEE70001C,
            ShowBiz = 0xAE6FFFB0,
            Artist = 0x4E6FFFBC,
            Adventurer = 0x3240CBA5,
            Education = 0x72428B30,
            Gamer = 0xF240C306,
            Journalism = 0x7240D944,
            Law = 0x12428B19,
            Music = 0xB2428B0C,
            Construction = 0xF3E1C301,
            Dance = 0xD3E09422,
            Entertainment = 0xB3E09417,
            Intelligence = 0x33E0940E,
            Ocenography = 0x73E09404,
            LiveInServant = 0x00845D99,
            Crafter = 0xD38D6534,
            Gatherer = 0x738D6245,
            Hunter = 0x93701850,
            EntertainLS = 0x117DF1D4,
            GameDevelopment = 0x713E7857,
            PetSecurity = 0xD188A400,
            PetService = 0xB188A4C1,
            PetShowBiz = 0xD175CC2D,
            OrangutanCrafter = 0xD3ACF0E0,
            OrangutanGatherer = 0x53ACF0CD,
            OrangutanHunter = 0xF3ACF09E,
            OwnedBuss = 0xD08F400A,
            TeenOwnedBuss = 0x316BD91F
        }

        public enum NeighbourhoodEP : uint
        {
            BaseGame = 0x00,
            University = 0x01,
            Nightlife = 0x02,
            Business = 0x03,
            FamilyFun = 0x04,
            GlamourLife = 0x05,
            Pets = 0x06,
            Seasons = 0x07,
            Celebration = 0x08,
            Fashion = 0x09,
            BonVoyage = 0x0a,
            TeenStyle = 0x0b,
            StoreEdition_old = 0x0c,
            Freetime = 0x0d,
            KitchenBath = 0x0e,
            IkeaHome = 0x0f,
            ApartmentLife = 0x10,
            MansionGarden = 0x11,
            StoreEdition = 0x1f
        }

        #endregion

       
        #region Dictionarys
        // Here is me learning & trying out stuff. Dictionary allows spaces and stuff in the returned string
        // whereas enum don't because enum is a nasty poo poo. Dictionary also allows conditional adding of values.
        //  Would be nice if nasty poo poo did that as well, could add careers according to EPs installed.

        public static Dictionary<uint, string> NPCNameFromID = new Dictionary<uint, string>();
        
        /// <summary>
        /// Convert the GUID to a name for known NPCs
        /// </summary>
        public static string GetKnownNPC(uint id)
        {
            if (NPCNameFromID.Count < 2) InitializeNPCNameFromID();
            if (NPCNameFromID.ContainsKey(id)) return NPCNameFromID[id];
            return "not found";
        }

        private static void InitializeNPCNameFromID()
        {
            NPCNameFromID.Clear();
            NPCNameFromID.Add(0x00000000, "-none-");
            NPCNameFromID.Add(0x4D9530C6, "Therapist");
            NPCNameFromID.Add(0x13269F2D, "Bigfoot");
            NPCNameFromID.Add(0x0F67E576, "Mrs. CrumpleBottom");
            NPCNameFromID.Add(0x2E17B9FC, "Pollination Technician");
            NPCNameFromID.Add(0x724CD298, "ideal plantsim");
            NPCNameFromID.Add(0x71B85E0D, "Skunk Skin (not a real sim)");
            NPCNameFromID.Add(0x745B11D1, "Rod Humble");
            NPCNameFromID.Add(0x341FB0E2, "Genie Midlock");
            NPCNameFromID.Add(0x136B1F2A, "Pirate Captain Edward Dregg");
            NPCNameFromID.Add(0xF03AE97B, "Father Time");
            NPCNameFromID.Add(0x7040237A, "Toddler Time");
            
            NPCNameFromID.Add(0xD55EF625, "Good Witch Cat");
            NPCNameFromID.Add(0x2C996F9C, "Remote Control Car (Controller)");
            NPCNameFromID.Add(0xB38590EB, "Witch Doctor");
            NPCNameFromID.Add(0xF036D5C3, "Santa Claus");
            NPCNameFromID.Add(0x50596292, "Robot (Controller)");
            NPCNameFromID.Add(0x84EC24A8, "Grim Reaper");
            NPCNameFromID.Add(0xF51A5E5B, "Spectral Assistant");
            NPCNameFromID.Add(0x31946C3B, "Bird (Controller)");
            NPCNameFromID.Add(0x7250E297, "Penguin (Controller)");
            NPCNameFromID.Add(0x2D7EB2DC, "Hula Zombie");
            NPCNameFromID.Add(0x51BFB2CD, "Stinky Skunk");
            NPCNameFromID.Add(0xF036D596, "Mrs. Santa Claus");
            NPCNameFromID.Add(0x00845D9E, "Josaphine Prim");
            NPCNameFromID.Add(0x0055FD4B, "Doctor Abbie");
            
            // The following is a template from Pet Stories but Maxis left it in as sim so it will exist
            NPCNameFromID.Add(0x926DF19F, "Dog Show Judge");
            // The following are templates from Castaway Stories but Maxis left them in as sims so they will exist
            NPCNameFromID.Add(0x73352057, "Wild Dog Template");
            NPCNameFromID.Add(0x134B4BCC, "Jaguar Template");
            NPCNameFromID.Add(0xB350BB5B, "Orangutan Template");
            NPCNameFromID.Add(0x33F54396, "Penguin Party Template");

            // The following is a templates not a sim but Maxis left it in Bluewater Village as a sim
            NPCNameFromID.Add(0x0F83C946, "Dog Template");
        }

        public static Dictionary<short, string> TitlePostName = new Dictionary<short, string>();

        /// <summary>
        /// Convert the id to a name for Post Title Names
        /// </summary>
        public static string GetTitleName(short id)
        {
            if (TitlePostName.Count < 2) InitializeTitlePostName();
            if (TitlePostName.ContainsKey(id)) return TitlePostName[id];
            return "";
        }
        /// <summary>
        /// Convert the name to an id for Post Title Names, for easy combobox use the string is object
        /// </summary>
        public static short GetTitleId(object ob)
        {
            string val = Convert.ToString(ob);
            if (TitlePostName.Count < 2) InitializeTitlePostName();
            if (TitlePostName.ContainsValue(val))
                foreach (KeyValuePair<short, string> kvp in TitlePostName)
                    if (kvp.Value == val) return kvp.Key;

            return 0;
        }

        private static void InitializeTitlePostName()
        {
            TitlePostName.Clear();
            TitlePostName.Add(0, "");
            TitlePostName.Add(1, " (Atrocious Witch)");
            TitlePostName.Add(2, " (Atrocious Warlock)");
            TitlePostName.Add(3, " (Evil Witch)");
            TitlePostName.Add(4, " (Evil Warlock)");
            TitlePostName.Add(5, " (Mean Witch)");
            TitlePostName.Add(6, " (Mean Warlock)");
            TitlePostName.Add(7, " (Witch)");
            TitlePostName.Add(8, " (Warlock)");
            TitlePostName.Add(9, " (Nice Witch)");
            TitlePostName.Add(10, " (Nice Warlock)");
            TitlePostName.Add(11, " (Good Witch)");
            TitlePostName.Add(12, " (Good Warlock)");
            TitlePostName.Add(13, " (Infallible Witch)");
            TitlePostName.Add(14, " (Infallible Warlock)");
        }

        public static Dictionary<uint, string> KnownFences = new Dictionary<uint, string>();

        /// <summary>
        /// Convert the GUID to a name for known Fences
        /// </summary>
        public static string GetKnownFence(uint id)
        {
            if (KnownFences.Count < 2) InitializeKnownFences();
            if (KnownFences.ContainsKey(id)) return KnownFences[id];
            return "not found";
        }

        /// <summary>
        /// Convert the name to a GUID for Known Fences, for easy combobox use the string is object
        /// </summary>
        public static uint GetFenceId(object ob)
        {
            string val = Convert.ToString(ob);
            if (KnownFences.Count < 2) InitializeKnownFences();
            if (KnownFences.ContainsValue(val))
                foreach (KeyValuePair<uint, string> kvp in KnownFences)
                    if (kvp.Value == val) return kvp.Key;
            return 0;
        }

        private static void InitializeKnownFences()
        {
            KnownFences.Clear();

            // Base game flowerbeds / fences
            KnownFences.Add(0x8D0B3B3A, "Flowerbed Malabar Black Bamboo Fence");
            KnownFences.Add(0xCD0F1DED, "Flowerbed Malabar Green Bamboo Fence");
            KnownFences.Add(0x8D0B3C31, "Flowerbed Malabar Natural Bamboo Fence");
            KnownFences.Add(0x4CDF8C41, "Tornado Solid Steel Fence");
            KnownFences.Add(0xCCDF918F, "Cut Stump Flowerbed Fence");
            KnownFences.Add(0xAD0DABD2, "PINEGULTCHER Wood Rail in Light Pine");
            KnownFences.Add(0x8D0B34FD, "Longhorn Balustrade in Light Wood");

            // BaseGame / Stories variants
            if (PathProvider.Global.GetExpansion(SimPe.Expansions.BaseGame).Exists
                || PathProvider.Global.GetExpansion(SimPe.Expansions.PetStories).Exists
                || PathProvider.Global.GetExpansion(SimPe.Expansions.LifeStories).Exists)
            {
                KnownFences.Add(0x6D0B2B38, "Nouvelle Fence in Liberty Green");
                KnownFences.Add(0x6D0B3239, "Nouvelle Fence in The New Black");
                KnownFences.Add(0xAD0B35F2, "Longhorn Balustrade in White");
                KnownFences.Add(0x8CDF8BF6, "Stone Cul-De-Sac Brick Fence");
                KnownFences.Add(0x6D0DA9F9, "PINEGULTCHER Wood Rail in Cedar");
                KnownFences.Add(0x8D0DB64C, "InvisiBarrier Fencing in Ether");
                KnownFences.Add(0x0D0DB771, "InvisiBarrier Fencing in Hallucination");
                KnownFences.Add(0x0D0DB7E1, "InvisiBarrier Fencing in Steam");
                KnownFences.Add(0xCD0DC03C, "Royal Courtyard Fencing");
                KnownFences.Add(0x0D0DBF66, "Windowed Royal Courtyard Fencing");
                KnownFences.Add(0xAD0F207E, "White Royal Courtyard Fencing");
                KnownFences.Add(0xED0EA468, "Short Mortar Brick Wall");
                KnownFences.Add(0x0D0F2115, "Short Mortar Reclaimed Brick Wall");
                KnownFences.Add(0x6D0EA622, "Short Mortar White Brick Wall");
                KnownFences.Add(0x6D0EA75D, "Low Stakes Fencing in Pastoral Green");
                KnownFences.Add(0x2CDF8E7D, "Low Stakes Fencing in Redwood");
                KnownFences.Add(0x2D0EA907, "Classic Arched Picket Fence");
                KnownFences.Add(0xECE5E3C5, "Genuine Railway Tie Fencing");
                KnownFences.Add(0x0D0EABDE, "Ancient Guardian Brick Wall");
                KnownFences.Add(0x6D0EAA68, "Ancient Guardian Rock Wall");
                KnownFences.Add(0x0D0EACC7, "Zig-Jag Flowerbed Fence in Red Brick");
                KnownFences.Add(0xED0EAE6A, "Zig-Jag Flowerbed Fence with Recycled Bricks");
                KnownFences.Add(0x8D0EB171, "Zig-Jag Flowerbed Fence in White Brick");
                KnownFences.Add(0xAD05BEE9, "FauxStone Wall by Valy");
                KnownFences.Add(0x0CDF8C99, "WroughtWright, Inc. Iron Age Fence");
            }

            // University
            if (PathProvider.Global.GetExpansion(SimPe.Expansions.University).Exists
                || PathProvider.Global.GetExpansion(SimPe.Expansions.PetStories).Exists
                || PathProvider.Global.GetExpansion(SimPe.Expansions.LifeStories).Exists)
            {
                KnownFences.Add(0xCEC0E4C4, "Running the Rapids Rock Fence");
                // KnownFences.Add(0x8CDF8BF6, "Stone Cul-De-Sac Brick Fence"); exists in base game
                KnownFences.Add(0x2ECB56F5, "Academia Red Brick Wall with Stone Rail");
                KnownFences.Add(0x2EBB5127, "Mini-Pediment in Discreet Concrete");
                KnownFences.Add(0xAEBB4FCD, "Mini-Pediment in Red Brick");
                KnownFences.Add(0x8EBB5130, "Mini-Pediment in Stucco");
                KnownFences.Add(0x4EBB4DA1, "Mini-Pediment in Verdant Green Stone with Brick Accents");
                KnownFences.Add(0x4EBB5037, "Patrician Balustrade in Discreet Concrete");
                KnownFences.Add(0xCEBB535D, "Patrician Balustrade in Neutral White");
                KnownFences.Add(0x2EC0E553, "Patrician Stone Wall in Concrete");
                KnownFences.Add(0xCEC0E2A8, "Romanesque Wall in Red Stone");
            }

            // Nightlife + Stories half-walls/fences
            if (PathProvider.Global.GetExpansion(SimPe.Expansions.Nightlife).Exists
                || PathProvider.Global.GetExpansion(SimPe.Expansions.PetStories).Exists
                || PathProvider.Global.GetExpansion(SimPe.Expansions.LifeStories).Exists)
            {
                KnownFences.Add(0x6FB1CF18, "The Wave Half Wall-DarkWood");
                KnownFences.Add(0x2FB1CF58, "The Wave Half Wall-Stainless");
                KnownFences.Add(0xAF911CBC, "The Wave Half Wall-LightWood");
                KnownFences.Add(0x6FB1CF3B, "The Wave Half Wall-BlackWood");
                KnownFences.Add(0x6F61A564, "Arboreal Wooden Fence");
                KnownFences.Add(0x6FB1CEB6, "Brass n' Glass Half Wall in Florish");
                KnownFences.Add(0x0F911C64, "Brass n' Glass Half Wall in Harvest");
                KnownFences.Add(0x2FB1CEDC, "Brass n' Glass Half Wall in Sleek-DarkWood");
                KnownFences.Add(0xCFD7E823, "Brass n' Glass Half Wall in Sleek-WhiteWood");
                KnownFences.Add(0x0FB1CE11, "Nocturnal Rumors Half Wall in Blush");
                KnownFences.Add(0xEFB1CDF1, "Nocturnal Rumors Half Wall in Gossip");
                KnownFences.Add(0x8F911CA3, "Nocturnal Rumors Half Wall in Whisper");
                KnownFences.Add(0x0FB1CE33, "Nocturnal Rumors Half Wall in Wink");
                KnownFences.Add(0x8FB1CD3A, "Perfectly Plank Half Wall in Blue");
                KnownFences.Add(0x0FB1CCEE, "Perfectly Plank Half Wall in Green");
                KnownFences.Add(0x6F911BF2, "Perfectly Plank Half Wall in Pine");
                KnownFences.Add(0x2FB1CD68, "Perfectly Plank Half Wall in Pink");
                KnownFences.Add(0x4FB1CD8C, "Perfectly Plank Half Wall in Red");
                KnownFences.Add(0xEFB1CDB3, "Perfectly Plank Half Wall in Stainless Steel");
                KnownFences.Add(0x6F851F53, "Quaint Half Wall in Brown");
                KnownFences.Add(0xCF6BD156, "Quaint Half Wall in Cornflower");
                KnownFences.Add(0x8F6BD1DB, "Quaint Half Wall in Green");
                KnownFences.Add(0xCF6BD1FB, "Quaint Half Wall in Light Wood");
                KnownFences.Add(0x8F6BD243, "Quaint Half Wall in White");
                KnownFences.Add(0x8F83EA15, "Quaint Half Wall in Rose");
            }

            // Chic Fence – Nightlife and packs that include it
            if (PathProvider.Global.GetExpansion(SimPe.Expansions.Nightlife).Exists
                || PathProvider.Global.GetExpansion(SimPe.Expansions.PetStories).Exists
                || PathProvider.Global.GetExpansion(SimPe.Expansions.LifeStories).Exists
                || PathProvider.Global.GetExpansion(SimPe.Expansions.Mansions).Exists)
            {
                KnownFences.Add(0xCF61A57E, "Chic Fence");
            }

            // More Nightlife fences
            if (PathProvider.Global.GetExpansion(SimPe.Expansions.Nightlife).Exists)
            {
                KnownFences.Add(0x0F584F14, "Relvet Vope Fence");
                KnownFences.Add(0xCF61A594, "Wooden Fence");
            }

            // Open for Business
            if (PathProvider.Global.GetExpansion(SimPe.Expansions.Business).Exists
                || PathProvider.Global.GetExpansion(SimPe.Expansions.PetStories).Exists)
            {
                KnownFences.Add(0x309DD1A8, "A Most Splendid Partition in Blue");
                KnownFences.Add(0x709DD1FB, "A Most Splendid Partition in Red");
                KnownFences.Add(0xB09DD226, "A Most Splendid Partition in White");
                KnownFences.Add(0xF09DD638, "Antiquity Fence by Swift Ltd.");
                KnownFences.Add(0x509DD0A5, "Balustrade Minimaal");
                KnownFences.Add(0x709DD0F2, "Balustrade Minimaal in Red");
                KnownFences.Add(0x909DD2C6, "Classical Ascension Railing in Blue");
                KnownFences.Add(0x709DD304, "Classical Ascension Railing in Red");
                KnownFences.Add(0xB09DD32A, "Classical Ascension Railing in White");
                KnownFences.Add(0x309DD49A, "Gelander Railing in Purple");
                KnownFences.Add(0xD09DD350, "Gelander Railing in Rose");
                KnownFences.Add(0x309DD474, "Gelander Railing in Yellow");
                KnownFences.Add(0xF09DD570, "Half Centurion in Blue");
                KnownFences.Add(0x709DD59C, "Half Centurion in Red");
                KnownFences.Add(0x709DD5C5, "Half Centurion in White");
                KnownFences.Add(0x509DD041, "Schifting Partition in Blue");
                KnownFences.Add(0xD09DD03A, "Schifting Partition in Red");
                KnownFences.Add(0x909DD079, "Schifting Partition in Yellow");
                KnownFences.Add(0xF09DD252, "The Iron Gardener Fence");
                KnownFences.Add(0xB09DD180, "Zaunfach Partition Fence in Purple");
                KnownFences.Add(0x109DD113, "Zaunfach Partition Fence in Rose");
                KnownFences.Add(0x309DD155, "Zaunfach Partition Fence in Yellow");
            }

            // Pets
            if (PathProvider.Global.GetExpansion(SimPe.Expansions.Pets).Exists
                || PathProvider.Global.GetExpansion(SimPe.Expansions.PetStories).Exists)
            {
                KnownFences.Add(0xD1AF063A, "Railing Atomica in Black");
                KnownFences.Add(0x91AF070C, "Railing Atomica in Red");
                KnownFences.Add(0xB1AF074A, "Railing Atomica in Stainless Steel");
                KnownFences.Add(0x71AF07DC, "Space-O-Rama Divider by Spitz in Aqua");
                KnownFences.Add(0x71AF0812, "Space-O-Rama Divider by Spitz in Gold");
                KnownFences.Add(0xB1AF084B, "Space-O-Rama Divider by Spitz in Green");
                KnownFences.Add(0xB1AF0885, "Space-O-Rama Divider by Spitz in Rose");
                KnownFences.Add(0x518B4159, "The Great Divide by Divisive Divicrats in Black");
                KnownFences.Add(0x318B42A4, "The Great Divide by Divisive Divicrats in Brass");
                KnownFences.Add(0x718B4308, "The Great Divide by Divisive Divicrats in Dark Wood");
                KnownFences.Add(0x3175D2B6, "The Great Divide by Divisive Divicrats in Light Wood");
                KnownFences.Add(0x518B436E, "The Great Divide by Divisive Divicrats in Medium Wood");
                KnownFences.Add(0x518B43B1, "The Great Divide by Divisive Divicrats in Silver");
            }

            // Seasons / Island Stories
            if (PathProvider.Global.GetExpansion(SimPe.Expansions.Seasons).Exists
                || PathProvider.Global.GetExpansion(SimPe.Expansions.IslandStories).Exists)
            {
                KnownFences.Add(0xF25CF2A4, "Failed Fence of Analogia");
                KnownFences.Add(0xF25CF2FB, "Schellacked Failed Fence of Analogia");
                KnownFences.Add(0xF25CE8FA, "Weathered Failed Fence of Analogia");
                KnownFences.Add(0xB25CF389, "Farmer Thompson's Stone Wall");
            }

            if (PathProvider.Global.GetExpansion(SimPe.Expansions.Seasons).Exists)
            {
                KnownFences.Add(0x924E0E65, "Green's Greenhouse Wall in Green");
                KnownFences.Add(0x924E0F6B, "Green's Greenhouse Wall in White");
                KnownFences.Add(0x724E0F1B, "Green's Greenhouse Wall in Wood");
            }

            // Bon Voyage / Island Stories
            if (PathProvider.Global.GetExpansion(SimPe.Expansions.Voyage).Exists
                || PathProvider.Global.GetExpansion(SimPe.Expansions.IslandStories).Exists)
            {
                KnownFences.Add(0xF34D05CF, "Dark Bamboo Barricade By Tropical Touches");
                KnownFences.Add(0x534D0580, "Light Bamboo Barricade By Tropical Touches");
                KnownFences.Add(0xD34D053C, "Medium Bamboo Barricade By Tropical Touches");
                KnownFences.Add(0xB34D0605, "Painted Bamboo Barricade By Tropical Touches");
                KnownFences.Add(0x53742EF3, "Green Bamboo Shortie Fence");
            }

            // H&M Fashion
            if (PathProvider.Global.GetExpansion(SimPe.Expansions.Fashion).Exists)
            {
                KnownFences.Add(0xD2F1C049, "Glimmie Black by Irokthis Stage Lamp Co.");
                KnownFences.Add(0xD2F0A4DB, "Glimmie White by Irokthis Stage Lamp Co.");
            }

            // Bon Voyage again
            if (PathProvider.Global.GetExpansion(SimPe.Expansions.Voyage).Exists)
            {
                KnownFences.Add(0xB330F5A9, "Fallen Fir Fence-Hickory");
                KnownFences.Add(0x5330F6C7, "Fallen Fir Fence-Rustic");
                KnownFences.Add(0xD354C607, "Please Fence Me In Fence");
            }

            // CEP / Extra content
            if (PathProvider.Global.GetExpansion(SimPe.Expansions.Extra).Exists && Helper.ECCorNewSEfound)
            {
                KnownFences.Add(0x6B4BF4A5, "Stonework Wall (White)");
                KnownFences.Add(0x6899FF71, "Stonework Wall (Pink)");
                KnownFences.Add(0x2E0B65EB, "Stonework Wall (Red)");
                KnownFences.Add(0x1801DAD1, "Stonework Wall (Black)");
            }

            // Apartments
            if (PathProvider.Global.GetExpansion(SimPe.Expansions.Apartments).Exists)
            {
                KnownFences.Add(0x753F2D78, "Belladonna Beautiful Balustrade in Grey Gold");
                KnownFences.Add(0xF53F2DB9, "Belladonna Beautiful Balustrade in Grey Patina");
                KnownFences.Add(0xB53F2DF1, "Belladonna Beautiful Balustrade In Grey Silver");
                KnownFences.Add(0xD53F2E64, "Belladonna Beautiful Balustrade in White Gold");
                KnownFences.Add(0xB53F2EED, "Belladonna Beautiful Balustrade in White Patina");
                KnownFences.Add(0xD53F2F3B, "Belladonna Beautiful Balustrade in White Silver");
                KnownFences.Add(0x352E31BD, "Digit Fence in Black");
                KnownFences.Add(0x152E31E6, "Digit Fence in Grey");
                KnownFences.Add(0x952E320D, "Digit Fence in White");
                KnownFences.Add(0xB51D228B, "Elegant Rails in Bronze");
                KnownFences.Add(0x351D226F, "Elegant Rails in Marble");
                KnownFences.Add(0x351D2233, "Elegant Rails in Silver");
                KnownFences.Add(0x552C7FEA, "Linear Eloquence in Bronze");
                KnownFences.Add(0xB52C7F06, "Linear Eloquence in Copper");
                KnownFences.Add(0xF52C7F61, "Linear Eloquence in Gold");
                KnownFences.Add(0xF52C7FBE, "Linear Eloquence in Silver");
                KnownFences.Add(0xF5387F52, "One Complete Diner Enclosure in Black");
                KnownFences.Add(0xF5387FBB, "One Complete Diner Enclosure in Dark Wood");
                KnownFences.Add(0x55388004, "One Complete Diner Enclosure in Medium Wood");
                KnownFences.Add(0x15388022, "One Complete Diner Enclosure in True White");
                KnownFences.Add(0xD51D22B7, "Rails of Style in Dark Wood");
                KnownFences.Add(0x951D22F5, "Rails of Style in Light Wood");
                KnownFences.Add(0x751D2316, "Rails of Style in Medium Wood");
                KnownFences.Add(0x551D208B, "Sleek and Secure in Black");
                KnownFences.Add(0xB51D2161, "Sleek and Secure in Grey");
                KnownFences.Add(0xD51D21AB, "Sleek and Secure in Silver");
                KnownFences.Add(0xD52C6B99, "Werknothom Half Wall in Black");
                KnownFences.Add(0x352C6C49, "Werknothom Half Wall in Grey");
                KnownFences.Add(0x752C6D51, "Werknothom Half Wall in White");
            }

            // Island Stories fences only when that pack exists
            if (PathProvider.Global.GetExpansion(SimPe.Expansions.IslandStories).Exists)
            {
                KnownFences.Add(0x1354A999, "Boarskin Fence");
                KnownFences.Add(0x93BE3964, "Low-Lying Garden Edging Rocks");
                KnownFences.Add(0xD354BA2D, "The Super Sturdy Perimeter Fence");
            }
        }


        public static Dictionary<short, string> LanguageName = new Dictionary<short, string>();

        /// <summary>
        /// Convert the Windows registry id to a name for Sims2 Language Names
        /// </summary>
        public static string GetLanguageName(short id)
        {
            if (LanguageName.Count < 2) InitializeLanguageName();
            if (LanguageName.ContainsKey(id)) return LanguageName[id];
            return "Invalid Language Id";
        }
        /// <summary>
        /// Convert the name to an id for Sims2 Language Names, for easy combobox use the string is object
        /// </summary>
        public static short GetLanguageId(object ob)
        {
            string val = Convert.ToString(ob);
            if (LanguageName.Count < 2) InitializeLanguageName();
            if (LanguageName.ContainsValue(val))
                foreach (KeyValuePair<short, string> kvp in LanguageName)
                    if (kvp.Value == val) return kvp.Key;

            return 0;
        }

        private static void InitializeLanguageName()
        {
            LanguageName.Clear();
            LanguageName.Add(0, "Invalid Language Id");
            LanguageName.Add(1, "US English");
            LanguageName.Add(2, "French");
            LanguageName.Add(3, "German");
            LanguageName.Add(4, "Italian");
            LanguageName.Add(5, "Spanish");
            LanguageName.Add(6, "Swedish");
            LanguageName.Add(7, "Finnish");
            LanguageName.Add(8, "Dutch");
            LanguageName.Add(9, "Danish");
            LanguageName.Add(10, "Brazilian Portuguese");
            LanguageName.Add(11, "Czech");
            LanguageName.Add(14, "Japanese");
            LanguageName.Add(15, "Korean");
            LanguageName.Add(16, "Russian");
            LanguageName.Add(17, "Simplified Chinese");
            LanguageName.Add(18, "Traditional Chinese");
            LanguageName.Add(19, "English");
            LanguageName.Add(20, "Polish");
            LanguageName.Add(21, "Thai");
            LanguageName.Add(22, "Norwegian");
            LanguageName.Add(23, "Portuguese");
            LanguageName.Add(24, "Hungarian");
        }

        public static Dictionary<uint, string> NPCFamilyFromInstance = new Dictionary<uint, string>();

        /// <summary>
        /// These known families may not have a FAMI entry in the neighbourhood
        /// but they are recognized by the game.
        /// </summary>
        public static string NPCFamily(uint id)
        {
            if (NPCFamilyFromInstance.Count < 2) InitializeNPCFamilyFromInstance();
            if (NPCFamilyFromInstance.ContainsKey(id)) return NPCFamilyFromInstance[id];
            if (id == 0) return "No Family";
            if (id < 32512) return "Playable Family";
            return "Unknown NPC Family";
        }

        private static void InitializeNPCFamilyFromInstance()
        {
            NPCFamilyFromInstance.Clear();
            NPCFamilyFromInstance.Add(0x7f65, "West World Locals");
            NPCFamilyFromInstance.Add(0x7f66, "Natives (castaway)");
            NPCFamilyFromInstance.Add(0x7f67, "Tau Ceti Locals");
            NPCFamilyFromInstance.Add(0x7f68, "Alpine Locals");
            NPCFamilyFromInstance.Add(0x7f69, "Spare Sims Pool");
            NPCFamilyFromInstance.Add(0x7fdf, "Elite Social Group");
            NPCFamilyFromInstance.Add(0x7fe0, "High Social Group");
            NPCFamilyFromInstance.Add(0x7fe1, "Medium Social Group");
            NPCFamilyFromInstance.Add(0x7fe2, "Low Social Group");
            NPCFamilyFromInstance.Add(0x7fe3, "Bogan Social Group");
            NPCFamilyFromInstance.Add(0x7fe4, "Iconic Hobby Sims");
            NPCFamilyFromInstance.Add(0x7fe5, "Holiday Tourists");
            NPCFamilyFromInstance.Add(0x7fe6, "Holiday Tourists");
            NPCFamilyFromInstance.Add(0x7fe7, "Holiday Tourists");
            NPCFamilyFromInstance.Add(0x7fe8, "Holiday Tourists");
            NPCFamilyFromInstance.Add(0x7fe9, "Holiday Tourists");
            NPCFamilyFromInstance.Add(0x7fea, "Holiday Tourists");
            NPCFamilyFromInstance.Add(0x7feb, "Holiday Tourists");
            NPCFamilyFromInstance.Add(0x7fec, "Holiday Tourists");
            NPCFamilyFromInstance.Add(0x7fed, "Holiday Tourists");
            NPCFamilyFromInstance.Add(0x7fee, "Holiday Tourists");
            NPCFamilyFromInstance.Add(0x7fef, "Holiday Tourists");
            NPCFamilyFromInstance.Add(0x7ff0, "Holiday Tourists");
            NPCFamilyFromInstance.Add(0x7FF1, "Tropical Locals");
            NPCFamilyFromInstance.Add(0x7FF2, "Mountain Locals");
            NPCFamilyFromInstance.Add(0x7FF3, "Asian Locals");
            NPCFamilyFromInstance.Add(0x7FF4, "Tourists");
            NPCFamilyFromInstance.Add(0x7FF5, "Unused - (Castaway)");
            NPCFamilyFromInstance.Add(0x7FF6, "Garden Club");
            NPCFamilyFromInstance.Add(0x7FF7, "Display Pets - In Use");
            NPCFamilyFromInstance.Add(0x7FF8, "Display Pets - Available");
            NPCFamilyFromInstance.Add(0x7FF9, "Orphan Pets");
            NPCFamilyFromInstance.Add(0x7FFA, "Strays");
            NPCFamilyFromInstance.Add(0x7FFB, "Baby Club");
            NPCFamilyFromInstance.Add(0x7FFC, "Downtownies");
            NPCFamilyFromInstance.Add(0x7FFD, "Orphans");
            NPCFamilyFromInstance.Add(0x7FFE, "Townies");
            NPCFamilyFromInstance.Add(0x7FFF, "Service NPCs");
        }
        #endregion

        #region Arrays

        /// <summary>
        /// all Known SemiGlobal Groups
        /// </summary>

        static SemiGlobalListing sgl;
        public static System.Collections.Generic.List<SemiGlobalAlias> SemiGlobals{
             get {
                 if (sgl == null) LoadSemGlobList();
                 return sgl;
             }
        }
        static void LoadSemGlobList()
        {
            sgl = new SemiGlobalListing();
            sgl.Sort();
        }
        public static uint SemiGlobalID(string sgname)
        {
            foreach (SemiGlobalAlias sga in SemiGlobals) if (sga.Name.Trim().ToLowerInvariant().Equals(sgname.Trim().ToLowerInvariant())) return sga.Id;
            return 0;
        }
        public static string SemiGlobalName(uint sgid)
        {
            foreach (SemiGlobalAlias sga in SemiGlobals) if (sga.Id == sgid) return sga.Name;
            return "";
        }

        #endregion

        #region Supporting Methods
        /// <summary>
        /// Returns the describing TypeAlias for the passed Type
        /// </summary>
        /// <param name="type">The type you want to load the TypeAlias for</param>
        /// <returns>The TypeAlias representing the Type</returns>
        public static TypeAlias FindTypeAlias(UInt32 type)
        {
            Data.TypeAlias a = Helper.TGILoader.GetByType(type);
            if (a == null) a = new Data.TypeAlias(false, Localization.Manager.GetString("unk") + "", type, "0x" + Helper.HexString(type));
            return a;
        }

        /// <summary>
        /// Returns the Group Number of a SemiGlobal File
        /// </summary>
        /// <param name="name">the nme of the semi global</param>
        /// <returns>The group Vlue of the Global</returns>
        public static Alias FindSemiGlobal(string name)
        {
            name = name.ToLower();
            foreach (Alias a in Data.MetaData.SemiGlobals)
            {
                if (a.Name.ToLower() == name) return a;
            } //for

            //unknown SemiGlobal
            return new Alias(0xffffffff, name.ToLower());
        }

        static SimPe.Interfaces.IAlias[] addonskins;
        /// <summary>
        /// Returns a List of Userdefined Add On Skins
        /// </summary>
        public static SimPe.Interfaces.IAlias[] AddonSkins
        {
            get
            {
                if (addonskins == null) addonskins = Alias.LoadFromXml(System.IO.Path.Combine(Helper.SimPeDataPath, "additional_skins.xml"));
                return addonskins;
            }
        }
        #endregion

        #region Map's
        static ArrayList rcollist;
        static ArrayList complist;
        static Hashtable agelist;
        static System.Collections.Generic.List<uint> cachedft;

        public static System.Collections.Generic.List<uint> CachedFileTypes
        {
            get
            {
                if (cachedft == null)
                {
                    cachedft = new System.Collections.Generic.List<uint>();

                    foreach (uint i in RcolList)
                        cachedft.Add(i);

                    cachedft.Add(OBJD_FILE);
                    cachedft.Add(CTSS_FILE);
                    cachedft.Add(STRING_FILE);

                    cachedft.Add(XFLR);
                    cachedft.Add(XFNC);
                    cachedft.Add(XNGB);
                    cachedft.Add(XOBJ);
                    cachedft.Add(XROF);
                    cachedft.Add(XWNT);
                }
                return cachedft;
            }
        }

        //Returns a List of all RCOl Compatible File Types
        public static ArrayList RcolList
        {
            get
            {
                if (rcollist == null)
                {
                    rcollist = new ArrayList();

                    rcollist.Add((uint)GMDC);	//GMDC
                    rcollist.Add((uint)TXTR);	//TXTR
                    rcollist.Add((uint)LIFO);	//LIFO
                    rcollist.Add((uint)TXMT);	//MATD
                    rcollist.Add((uint)ANIM);	//ANIM
                    rcollist.Add((uint)GMND);	//GMND
                    rcollist.Add((uint)SHPE);	//SHPE
                    rcollist.Add((uint)CRES);	//CRES
                    rcollist.Add(LDIR);
                    rcollist.Add(LAMB);
                    rcollist.Add(LSPT);
                    rcollist.Add(LPNT);
                }
                return rcollist;
            }
        }

        //Returns a List of File Types that should be compressed
        public static ArrayList CompressionCandidates
        {
            get
            {
                if (complist == null)
                {
                    complist = RcolList;

                    complist.Add(MetaData.STRING_FILE);
                    complist.Add((uint)0x0C560F39); //Binary Index
                    complist.Add((uint)0xAC506764); //3D IDR
                }
                return complist;
            }
        }

        /// <summary>
        /// translates the Ages from a SDesc to a Property Set age 
        /// </summary>
        public static Data.Ages AgeTranslation(MetaData.LifeSections age)
        {
            agelist = new Hashtable();
            if (age == MetaData.LifeSections.Adult) return Data.Ages.Adult;
            else if (age == MetaData.LifeSections.Baby) return Data.Ages.Baby;
            else if (age == MetaData.LifeSections.Child) return Data.Ages.Child;
            else if (age == MetaData.LifeSections.Elder) return Data.Ages.Elder;
            else if (age == MetaData.LifeSections.Teen) return Data.Ages.Teen;
            else if (age == MetaData.LifeSections.Toddler) return Data.Ages.Toddler;
            else return Data.Ages.Adult;
        }
        #endregion
    }
}
