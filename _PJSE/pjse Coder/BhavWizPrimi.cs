/*
 * The majority of the code in this file would not be possible without
 * disaSim2 by dizzy2 and Shy.
 * 
 * disaSim2 is public domain code and, in that spirit, so is the code
 * here (unlike the rest of this project, which is restricted by the
 * GPL).
 * 
 * dizzy2 has the following statement at the start of disaSim2.cpp:
 */

//==============================================================================
// disassemble Sims 2 "SimAntics" (ver 2.4a)
//
// This file is PUBLIC DOMAIN (free as in "freestyle" or "freeway")
//==============================================================================
// dizzy2 (lead bug-inserter, project mascot) would like to thank the following
// individuals for their contributions:
//
// Shy (for lots of bug-squishing, and lots and lots of shiny, new code)
// Tom Bombadil (for pretty html text-rendering routines)
// T.Rowland (for some nice string and output improvements)
//==============================================================================

/*
 * The C# code here was converted by Peter L Jones <peter@drealm.info> from
 * the C source of disaSim2, mostly using some quick global replaces.  I (plj)
 * cannot claim any intellectual property rights over that!  Of course, if you
 * find something that this code gets wrong - whether disaSim2 gets it right
 * or not - please let us know by posting here:
 * http://forums.modthesims2.com/showthread.php?t=33537
 */

using System;
using System.Collections;
using SimPe.PackedFiles.Wrapper;

namespace pjse.BhavNameWizards
{
	/// <summary>
	/// Abstract class for primitive name providers
	/// </summary>
	public abstract class BhavWizPrimi : BhavWiz
	{
		protected BhavWizPrimi(Instruction i) : base (i) { prefix = pjse.Localization.GetString("lcPrim"); }

		public static implicit operator BhavWizPrimi(Instruction i)
		{
			if (i.OpCode >= 0x0100)
				throw new Exception("OpCode not a primative");

			switch(i.OpCode)
			{
				case 0x0000: return new WizPrimi0x0000(i);
				case 0x0001: return new WizPrimi0x0001(i);
				case 0x0002: return new WizPrimi0x0002(i);
				case 0x0003: return new WizPrimi0x0003(i);
				case 0x0004:
				case 0x0005:
				case 0x0006:
					return new WizPrimiUnused(i);
				case 0x0007: return new WizPrimi0x0007(i);
				case 0x0008: return new WizPrimi0x0008(i);
				case 0x0009:
				case 0x000a:
					return new WizPrimiUnused(i);
				case 0x000b: return new WizPrimi0x000b(i);
				case 0x000c: return new WizPrimi0x000c(i);
				case 0x000d: return new WizPrimi0x000d(i);
				case 0x000e: return new WizPrimi0x000e(i);
				case 0x000f: return new WizPrimi0x000f(i);
				case 0x0010: return new WizPrimi0x0010(i);
				case 0x0011: return new WizPrimi0x0011(i);
				case 0x0012: return new WizPrimi0x0012(i);
				case 0x0013: return new WizPrimi0x0013(i);
				case 0x0014: return new WizPrimi0x0014(i);
				case 0x0015:
					return new WizPrimiUnused(i);
				case 0x0016: return new WizPrimi0x0016(i);
				case 0x0017: return new WizPrimi0x0017(i);
				case 0x0018:
					return new WizPrimiUnused(i);
				case 0x0019: return new WizPrimi0x0019(i);
				case 0x001a: return new WizPrimi0x001a(i);
				case 0x001b: return new WizPrimi0x001b(i);
				case 0x001c: return new WizPrimi0x001c(i);
				case 0x001d: return new WizPrimi0x001d(i);
				case 0x001e: return new WizPrimi0x001e(i);
				case 0x001f: return new WizPrimi0x001f(i);
				case 0x0020: return new WizPrimi0x0020(i);
				case 0x0021: return new WizPrimi0x0021(i);
				case 0x0022: return new WizPrimi0x0022(i);
				case 0x0023: return new WizPrimi0x0023(i);
				case 0x0024: return new WizPrimi0x0024(i);
				case 0x0025: return new WizPrimi0x0025(i);
				case 0x0026:
				case 0x0027:
				case 0x0028:
				case 0x0029:
					return new WizPrimiUnused(i);
				case 0x002a: return new WizPrimi0x002a(i);
				case 0x002b:
				case 0x002c:
					return new WizPrimiUnused(i);
				case 0x002d: return new WizPrimi0x002d(i);
				case 0x002e: return new WizPrimi0x002e(i);
				case 0x002f:
					return new WizPrimiUnused(i);
				case 0x0030: return new WizPrimi0x0030(i);
				case 0x0031: return new WizPrimi0x0031(i);
				case 0x0032: return new WizPrimi0x0032(i);
				case 0x0033: return new WizPrimi0x0033(i);
				case 0x0069: return new WizPrimi0x0069(i);
				case 0x006a: return new WizPrimi0x006a(i);
				case 0x006b: return new WizPrimi0x006b(i);
				case 0x006c: return new WizPrimi0x006c(i);
				case 0x006d: return new WizPrimi0x006d(i);
				case 0x006e: return new WizPrimi0x006e(i);
				case 0x006f: return new WizPrimi0x006f(i);
				case 0x0070: return new WizPrimi0x0070(i);
				case 0x0071: return new WizPrimi0x0071(i);
				case 0x0072: return new WizPrimi0x0072(i);
				case 0x0073: return new WizPrimi0x0073(i);
				case 0x0074: return new WizPrimi0x0074(i);
				case 0x0075: return new WizPrimi0x0075(i);
				case 0x0076: return new WizPrimi0x0076(i);
				case 0x0077: return new WizPrimi0x0077(i);
				case 0x0078: return new WizPrimi0x0078(i);
				case 0x0079: return new WizPrimi0x0079(i);
				case 0x007a: return new WizPrimi0x007a(i);
				case 0x007b: return new WizPrimi0x007b(i);
				case 0x007c: return new WizPrimi0x007c(i);
				case 0x007d: return new WizPrimi0x007d(i);
				case 0x007e: return new WizPrimi0x007e(i);
			}

			if (i.OpCode >= 0x0034 && i.OpCode <= 0x0068 || i.OpCode >= 0x007f)
				return new WizPrimiUnused(i);

            throw new Exception("OpCode defies understanding");
        }

		protected override string OpcodeName { get { return readStr(GS.BhavStr.Primitives, instruction.OpCode); } }

	}


	public class WizPrimiUnused : BhavWizPrimi
	{
		public WizPrimiUnused(Instruction i) : base(i) { }

		protected override string Operands(bool lng) { return "-"; }

	}


	public class WizPrimi0x0000 : BhavWizPrimi	// Sleep
	{
		public WizPrimi0x0000(Instruction i) : base(i) { }

		protected override string Operands(bool lng)
		{
			return dataOwner(lng, 0x09, instruction.Operands[0], instruction.Operands[1]); // Param
		}

	}

	public class WizPrimi0x0001 : BhavWizPrimi	// Generic Sims Call
	{
		public WizPrimi0x0001(Instruction i) : base(i) { }

		public override ABhavOperandWiz Wizard()
		{
			return new pjse.BhavOperandWizards.BhavOperandWiz0x0001(instruction);
		}

		protected override string Operands(bool lng)
		{
            return readStr(GS.BhavStr.Generics, instruction.Operands[0])
                + (lng ? " (" + readStr(GS.BhavStr.GenericsDesc, instruction.Operands[0]) + ")" : "" );
		}
	}

	public class WizPrimi0x0002 : BhavWizPrimi	// Expression
	{
		public WizPrimi0x0002(Instruction i) : base(i) { }

		public override ABhavOperandWiz Wizard()
		{
			return new pjse.BhavOperandWizards.BhavOperandWiz0x0002(instruction);
		}

		protected override string Operands(bool lng)
		{
			byte[] o = instruction.Operands;

			byte lhs_data_owner = o[6]; // c2
			ushort lhs_value_word = ToShort(o[0], o[1]); // w1
			byte _operator = o[5]; // c1
			byte rhs_data_owner = o[7]; // b[x+7]
			ushort rhs_value_word = ToShort(o[2], o[3]); // w2

			string s = "";

			s += dataOwner(lng, lhs_data_owner, lhs_value_word)
				+ " " + readStr(GS.BhavStr.Operators, _operator)
				+ " ";

			if (lng && _operator >= 8 && _operator <= 10) // Flag operation
			{
				s+= pjse.Localization.GetString("flagnr") + " " + dataOwner(rhs_data_owner, rhs_value_word);
				if (rhs_data_owner == 7 && flagname(lhs_data_owner, lhs_value_word, rhs_value_word) != null)
					s += " (" + flagname(lhs_data_owner, lhs_value_word, rhs_value_word) + ")";
			}
			else
				s += dataOwner(lng, rhs_data_owner, rhs_value_word);

			return s;
		}
	}

	public class WizPrimi0x0003 : BhavWizPrimi	// Find Best Interaction
	{
		public WizPrimi0x0003(Instruction i) : base(i) { }

		protected override string Operands(bool lng)
		{
			byte[] o = new byte[16];
			((byte[])instruction.Operands).CopyTo(o, 0);
			((byte[])instruction.Reserved1).CopyTo(o, 8);

			string s = "";

			if (o[2] == 0)
                s += pjse.Localization.GetString("bwp03_nworst");
			else
			{
				int motives = ToShort(o[0], o[1]);
                s += pjse.Localization.GetString("bwp03_formotive")
                    + ": ";
                bool found = false;
                for (ushort i = 0; i < 16; i++)   // this should only find 1 motive (if any)
                    if ((motives & (1 << i)) != 0)
                    {
                        s += (found ? "; " : "") + readStr(GS.BhavStr.Motives, i);
                        found = true;
                    }
                if (!found) s += "("
                    + pjse.Localization.GetString("none")
                    + ")";
				if (lng)
                    s += ", "
                        + pjse.Localization.GetString("bwp03_remworst")
                        + ": " + ((o[2] & 0x01) != 0).ToString();
			}
			if (lng)
			{
                s += ", " + pjse.Localization.GetString("bwp03_inroom") + " " + this.dataOwner(0x08, 0) // Temp
                    + ": " + ((o[3] & 0x02) != 0).ToString();
                s += ", " + pjse.Localization.GetString("bwp03_oow")
                    + ": " + ((o[3] & 0x04) != 0).ToString();
                s += ", " + pjse.Localization.GetString("bwp03_nested")
                    + ": " + ((o[3] & 0x08) != 0).ToString();
                s += ", " + pjse.Localization.GetString("bwp03_oninteraction")
                    + ": " + ((o[3] & 0x10) != 0).ToString();
                s += ", " + pjse.Localization.GetString("bwp03_locntemp1")
                    + ": " + ((o[3] & 0x20) != 0).ToString();
            }
			return s;
        }
	}

	public class WizPrimi0x0007 : BhavWizPrimi	// Refresh
	{
		public WizPrimi0x0007(Instruction i) : base(i) { }

		protected override string Operands(bool lng)
		{
			byte[] o = new byte[16];
			((byte[])instruction.Operands).CopyTo(o, 0);
			((byte[])instruction.Reserved1).CopyTo(o, 8);

			string s = "";


			s += readStr(GS.BhavStr.UpdateWho, ToShort(o[0], o[1])) + " " + readStr(GS.BhavStr.UpdateWhat, ToShort(o[2], o[3]));

			return s;
		}
	}

	public class WizPrimi0x0008 : BhavWizPrimi	// Random
	{
		public WizPrimi0x0008(Instruction i) : base(i) { }

        public override ABhavOperandWiz Wizard()
        {
            return new pjse.BhavOperandWizards.BhavOperandWiz0x0008(instruction);
        }

        protected override string Operands(bool lng)
		{
			byte[] o = new byte[16];
			((byte[])instruction.Operands).CopyTo(o, 0);
			((byte[])instruction.Reserved1).CopyTo(o, 8);

            return dataOwner(lng, o[2], o[0], o[1]) + " := 0 .. < " + dataOwner(lng, o[6], o[4], o[5]);
		}
	}

	public class WizPrimi0x000b : BhavWizPrimi	// Get Distance To
	{
		public WizPrimi0x000b(Instruction i) : base(i) { }

		protected override string Operands(bool lng)
		{
			byte[] o = new byte[16];
			((byte[])instruction.Operands).CopyTo(o, 0);
			((byte[])instruction.Reserved1).CopyTo(o, 8);

			string s = "";

            s += dataOwner(lng, 0x08, o[0], o[1]); // temp
            s += " := ";
            s += ((o[2] & 0x01) != 0
                    ? dataOwner(lng, o[3], o[4], o[5])
                    : dnMe() // Me
                    )
                ;
            s += " .. " + dnStkOb(); // Stack Object
            if (lng)
            {
                s += ", "
                    + pjse.Localization.GetString("bwp0b_100tile")
                    + ": " + ((o[6] & 0x02) != 0).ToString();
            }
			return s;
		}
	}

	public class WizPrimi0x000c : BhavWizPrimi	// Get Direction To
	{
		public WizPrimi0x000c(Instruction i) : base(i) { }

		protected override string Operands(bool lng)
		{
			byte[] o = new byte[16];
			((byte[])instruction.Operands).CopyTo(o, 0);
			((byte[])instruction.Reserved1).CopyTo(o, 8);

			string s = "";

            s += dataOwner(lng, o[2], o[0], o[1]);
            s += " := ";
            s += ((o[4] & 0x01) != 0
                    ? dataOwner(lng, o[5], o[6], o[7])
                    : dnMe() // Me
                    )
                ;
            s += " .. " + dnStkOb(); // Stack Object
            if (lng)
            {
                s += ", "
                    + pjse.Localization.GetString("bwp0c_degrees")
                    + ": " + ((o[8] & 0x02) == 0).ToString();
            }

			return s;
		}
	}

	public class WizPrimi0x000d : BhavWizPrimi	// Push Interaction -- for wizard, see edithWiki FunWithControllers
	{
		public WizPrimi0x000d(Instruction i) : base(i) { }

		protected override string Operands(bool lng)
		{
			byte[] o = new byte[16];
			((byte[])instruction.Operands).CopyTo(o, 0);
			((byte[])instruction.Reserved1).CopyTo(o, 8);

			string s = "";

            if (lng)
                s += pjse.Localization.GetString("Target") + ": " + dnStkOb() + ", "; // Stack Object

            s += (lng ? pjse.Localization.GetString("Object") + ": " : "")
                + dataOwner(lng, (byte)((o[3] & 0x02) != 0 ? 0x19 : 0x09), o[1]);	// local | param

            s += ", " + (lng ? pjse.Localization.GetString("Interaction") + ": " : "");
            if ((o[3] & 0x10) != 0)
				s += dataOwner(lng, o[5], o[6], o[7]);
			else if ((o[14] & 2) != 0)
                s += pjse.Localization.GetString("bwp0d_lastfba");
			else
				s += dataOwner(lng, 0x07, o[0]); // Literal

            s += ", " + readStr(GS.BhavStr.Priorities, o[2]);

			if (lng)
			{
				if ((o[3] & 0x01) != 0)
					s += ", " + pjse.Localization.GetString("bwp0d_IconObject") + ": " + dataOwner(0x19, o[4]); // Local
				else if ((o[14] & 4) != 0)
                    s += ", " + pjse.Localization.GetString("bwp0d_IconObject") + ": " + dataOwner(0x08, 0x04) + ",5"; // Temp

                s += ", " + pjse.Localization.GetString("bwp0d_IconIndex") + ": "
                    + ((o[14] & 0x08) != 0
                        ? dataOwner(0x08, 0x06) // Temp
                        : dataOwner(0x07, o[15]) // Literal
                        )
                    ;

                if ((o[14] & 0x01) != 0) s += ", " + pjse.Localization.GetString("bwp0d_callersparams");
				// if (o[3] & 4) ht_fprintf(outFile,TYPE_NORMAL,", continue as current");
				if ((o[3] & 0x08) != 0) s += ", " + pjse.Localization.GetString("bwp0d_usename");
                if ((o[3] & 0x20) != 0) s += ", " + pjse.Localization.GetString("bwp0d_forcerun");
                if ((o[3] & 0x40) != 0) s += ", " + pjse.Localization.GetString("bwp0d_linkto")
                    + " " + dataOwner(o[8], o[9], o[10]);
                if ((o[3] & 0x80) != 0) s += ", " + pjse.Localization.GetString("bwp0d_returnID")
                    + " " + dataOwner(o[11], o[12], o[13]);
			}
			return s;
		}
	}

	public class WizPrimi0x000e : BhavWizPrimi	// Find Best Object for Function
	{
		public WizPrimi0x000e(Instruction i) : base(i) { }

		protected override string Operands(bool lng)
		{
			byte[] o = new byte[16];
			((byte[])instruction.Operands).CopyTo(o, 0);
			((byte[])instruction.Reserved1).CopyTo(o, 8);

			string s = "";

			s += readStr(GS.BhavStr.FunctionTable, o[0]);
			if (lng)
			{
                byte[] flags = { 0x01, 0x02, 0x04, 0x00, 0x10, 0x00, 0x40, 0x00, };
                for (int i = 0; i < flags.Length; i++)
                    if (flags[i] != 0)
                    s += ", " + readStr(GS.BhavStr.FuncLocationFlags, (ushort)(i+1)) + ": " + ((o[2] & flags[i]) != 0).ToString();
                s += ", " + readStr(GS.BhavStr.FuncLocationFlags, 4) + ": "
                    + ((o[2] & 0x08) != 0 ? dataOwner(o[3], o[4], o[5]) : dnMe()); // Me
			}
			return s;
		}
	}

	public class WizPrimi0x000f : BhavWizPrimi	// Break Point (disaSim2 24b)
	{
		public WizPrimi0x000f(Instruction i) : base(i) { }

		protected override string Operands(bool lng)
		{
			byte[] o = new byte[16];
			((byte[])instruction.Operands).CopyTo(o, 0);
			((byte[])instruction.Reserved1).CopyTo(o, 8);

			return ((o[4] & 0x01) != 0)
				? pjse.Localization.GetString("bwp0f_ignored")
				: pjse.Localization.GetString("bwp0f_if") + " " + dataOwner(lng, o[2], o[0], o[1]) + " != 0";
		}
	}

	public class WizPrimi0x0010 : BhavWizPrimi	// Find location for -- for wizard, see edithWiki AkeaPostMortem
	{
		public WizPrimi0x0010(Instruction i) : base(i) { }

		protected override string Operands(bool lng)
		{
			byte[] o = new byte[16];
			((byte[])instruction.Operands).CopyTo(o, 0);
			((byte[])instruction.Reserved1).CopyTo(o, 8);

			string s = "";

			if ((o[2] & 0x01) != 0)
			{
                s += dnStkOb(); // Stack Object
				if (lng)
                    s += ", " + pjse.Localization.GetString("bwp10_startAt") + " " + dataOwner(0x19, o[1]); // Local
			}
			else
			{
				s += dataOwner(lng, o[4], o[5], o[6]);
				if (lng)
                    s += ", " + pjse.Localization.GetString("bwp10_relativeTo") + " " + dataOwner(o[7], o[8], o[9]);
			}

			if (lng)
			{
				if ((o[2] & 0x08) != 0)
				{
                    s += ", " + pjse.Localization.GetString("bwp10_facing");
                    if ((o[3] & 0x01) != 0) s += " " + pjse.Localization.GetString("compassN");
                    if ((o[3] & 0x02) != 0) s += " " + pjse.Localization.GetString("compassNE");
                    if ((o[3] & 0x04) != 0) s += " " + pjse.Localization.GetString("compassE");
                    if ((o[3] & 0x08) != 0) s += " " + pjse.Localization.GetString("compassSE");
                    if ((o[3] & 0x10) != 0) s += " " + pjse.Localization.GetString("compassS");
                    if ((o[3] & 0x20) != 0) s += " " + pjse.Localization.GetString("compassSW");
                    if ((o[3] & 0x40) != 0) s += " " + pjse.Localization.GetString("compassW");
                    if ((o[3] & 0x80) != 0) s += " " + pjse.Localization.GetString("compassNW");
				}

				s += ", " + readStr(GS.BhavStr.FindGLB, o[0]);
				if (o[0] >= 5 && o[0] <= 8)
					s += " 0x" + SimPe.Helper.HexString(o[10]);

                s += ", " + pjse.Localization.GetString("bwp10_preferEmpty") + ": " + ((o[2] & 0x02) == 0).ToString();
                s += ", " + pjse.Localization.GetString("bwp10_userEditable") + ": " + ((o[2] & 0x04) != 0).ToString();
                s += ", " + pjse.Localization.GetString("bwp10_onLevelGround") + ": " + ((o[2] & 0x10) != 0).ToString();
                s += ", " + pjse.Localization.GetString("bwp10_withEmptyBorder") + ": " + ((o[2] & 0x20) != 0).ToString();
                s += ", " + pjse.Localization.GetString("bwp10_beginInFrontOfRefobj") + ": " + ((o[2] & 0x40) != 0).ToString();
                s += ", " + pjse.Localization.GetString("bwp10_withLineOfSightToCenter") + ": " + ((o[2] & 0x80) != 0).ToString();
			}
			return s;
		}
	}

	public class WizPrimi0x0011 : BhavWizPrimi	// Idle for Input
	{
		public WizPrimi0x0011(Instruction i) : base(i) { }

		protected override string Operands(bool lng)
		{
			byte[] o = new byte[16];
			((byte[])instruction.Operands).CopyTo(o, 0);
			((byte[])instruction.Reserved1).CopyTo(o, 8);

			string s = "";

			if ((o[4] & 0x01) != 0)
                s += pjse.Localization.GetString("bwp11_handleSubQueueInteractions");
			else
                s += pjse.Localization.GetString("bwp_ticks") + ": " + dataOwner(lng, 0x09, o[0]) // Param
                    + ", " + pjse.Localization.GetString("bwp11_allowPush") + ": " + (ToShort(o[2], o[3]) == 0).ToString();

			return s;
		}
	}

	public class WizPrimi0x0012 : BhavWizPrimi	// Remove Object Instance -- for wizard, see edithWiki AkeaPostMortem
	{
		public WizPrimi0x0012(Instruction i) : base(i) { }

		protected override string Operands(bool lng)
		{
			byte[] o = new byte[16];
			((byte[])instruction.Operands).CopyTo(o, 0);
			((byte[])instruction.Reserved1).CopyTo(o, 8);

			string s = "";

            s += (o[0] == 0 ? dnMe() : dnStkOb()); // Me | Stack Object
			if (lng)
			{
                s += ", " + pjse.Localization.GetString("bwp12_returnImmediately") + ": " + ((o[2] & 1) != 0).ToString();
                s += ", " + pjse.Localization.GetString("bwp12_cleanupAll") + ": " + ((o[2] & 2) == 0).ToString();
			}
			return s;
		}
	}

	public class WizPrimi0x0013 : BhavWizPrimi	// Make New Character
	{
		public WizPrimi0x0013(Instruction i) : base(i) { }

		protected override string Operands(bool lng)
		{
			byte[] o = new byte[16];
			((byte[])instruction.Operands).CopyTo(o, 0);
			((byte[])instruction.Reserved1).CopyTo(o, 8);

			string s = "";

			if ((o[9] & 0x01) != 0)
			{
                s += pjse.Localization.GetString("Parent") + " 1"
                    + ((o[9] & 0x02) != 0 ? " " + pjse.Localization.GetString("NeighborID") : "")
                    + ": " + dataOwner(lng, o[6], o[7], o[8]);
                s += ", ";
                s += pjse.Localization.GetString("Parent") + " 2"
                    + ((o[9] & 0x04) != 0 ? " " + pjse.Localization.GetString("NeighborID") : "")
                    + ": " + dataOwner(lng, o[3], o[4], o[5]);
			}
			else
                s += pjse.Localization.GetString("bwp13_noParents");

			if (lng)
			{

                s += ", " + pjse.Localization.GetString("bwp13_personDataSource")
                    + ": (GUID) " + dataOwner(0x08, 0x00) + ",1: " + ((o[9] & 0x08) != 0).ToString(); // Temp
                s += ", " + pjse.Localization.GetString("bwp13_personDataSource")
                    + ": (GUID) " + "temp Token" + ": " + ((o[9] & 0x10) != 0).ToString();
                s += ", " + pjse.Localization.GetString("bwp13_characterFromBin")
                    + ": " + ((o[9] & 0x20) != 0).ToString();
                s += ", " + pjse.Localization.GetString("bwp13_thumbnailOutfit")
                    + ": " + (((o[9] & 0x40) != 0)
                        ? "(GUID) " + (((o[9] & 0x80) != 0)
                            ? dataOwner(0x08, 0x02) + ",3" // Temp
                            : dataOwner(o[10], o[11], o[12]))
                        : pjse.Localization.GetString("default"));

				if (o[0] != 0 && o[0] != 0xFF) 
				{
                    s += ", " + pjse.Localization.GetString("bwp13_skinColor")
                        + ": " + dataOwner(0x19, o[0]); // Local
                    s += ", " + pjse.Localization.GetString("bwp13_age")
                        + ": " + dataOwner(0x19, o[1]); // Local
                    s += ", " + pjse.Localization.GetString("bwp13_gender")
                        + ": " + dataOwner(0x19, o[2]); // Local
				}
				else
                    s += ", " + pjse.Localization.GetString("bwp13_defAgeGenderSkin");
			}
			return s;
		}
	}

	public class WizPrimi0x0014 : BhavWizPrimi	// Run Functional Tree
	{
		public WizPrimi0x0014(Instruction i) : base(i) { }

		protected override string Operands(bool lng)
		{
			byte[] o = new byte[16];
			((byte[])instruction.Operands).CopyTo(o, 0);
			((byte[])instruction.Reserved1).CopyTo(o, 8);

			string s = "";

			s += readStr(GS.BhavStr.FunctionTable, o[0]);
			if (lng)
			{
                s += ", " + pjse.Localization.GetString("bwp14_changeIcon") + ": " + ((o[2] & 0x01) != 0).ToString();
                s += ", " + pjse.Localization.GetString("bwp14_callersParams") + ": " + ((o[2] & 0x02) != 0).ToString();
                s += ", " + pjse.Localization.GetString("bwp14_CTonly") + ": " + ((o[2] & 0x04) != 0).ToString();
			}
			return s;
		}
	}

	public class WizPrimi0x0016 : BhavWizPrimi	// Turn Body
	{
		public WizPrimi0x0016(Instruction i) : base(i) { }

		protected override string Operands(bool lng)
		{
			byte[] o = new byte[16];
			((byte[])instruction.Operands).CopyTo(o, 0);
			((byte[])instruction.Reserved1).CopyTo(o, 8);

			string s = "";

			s += readStr(GS.BhavStr.TurnBody, o[0]);

			return s;
		}
	}

	public class WizPrimi0x0017 : BhavWizPrimi	// Play / Stop Sound Event -- for wizard, see edithWiki CreatingAChair
	{
		public WizPrimi0x0017(Instruction i) : base(i) { }

		protected override string Operands(bool lng)
		{
			byte[] o = new byte[16];
			((byte[])instruction.Operands).CopyTo(o, 0);
			((byte[])instruction.Reserved1).CopyTo(o, 8);

			string s = "";

			s += ((o[4] & 0x40) != 0
                ? pjse.Localization.GetString("Stop")
                : pjse.Localization.GetString("Play")
                );

			int instance = ToShort(o[0], o[1]);
			Scope scope = Scope.Private;
			if (instance >= 10000 && instance < 20000)
			{
				scope = Scope.Global;
				instance -= 10000;
			}
			else if (instance >= 20000)
			{
				scope = Scope.SemiGlobal;
				instance -= 20000;
			}
			string temp = readStr(scope, GS.GlobalStr.Sound, (ushort)(instance), lng ? -1 : 60, lng ? Detail.Normal : Detail.ErrorNames);
			if (temp.Length > 0)
				s += " " + temp;

			if (lng)
			{
                s += ", " + pjse.Localization.GetString("bwp_source")
                    + ": " + dataOwner((byte)((o[4] & 0x02) == 0 ? 0x03 : 0x04), 0x0b);
                s += ", " + pjse.Localization.GetString("bwp17_autoVary")
                    + ": " + ((o[4] & 0x10) != 0).ToString();

                s += ", " + pjse.Localization.GetString("bwp17_sampleRate")
                    + ": 0x" + SimPe.Helper.HexString(ToShort(o[2], o[3]));
                s += ", " + pjse.Localization.GetString("bwp17_volume")
                    + ": 0x" + SimPe.Helper.HexString(o[5]);
			}

			return s;
		}
	}

	public class WizPrimi0x0019 : BhavWizPrimi	// Alter Budget -- for wizard, see edithWiki WorkAndSchool, Chance Card - Results
	{
		public WizPrimi0x0019(Instruction i) : base(i) { }

		protected override string Operands(bool lng)
		{
			byte[] o = new byte[16];
			((byte[])instruction.Operands).CopyTo(o, 0);
			((byte[])instruction.Reserved1).CopyTo(o, 8);

			string s = "";


            // expense type | operator | amount

            if ((o[4] & 0x01) != 0)
                s += pjse.Localization.GetString("bwp19_test")
                    + ": ";

            s += readStr(GS.BhavStr.ExpenseType, o[6])
                + " " + readStr(GS.BhavStr.Operators, (ushort)(((o[4] & 0x02) != 0) ? 0x03 : 0x04)) // -= | +=
                + " (";

            byte owner = o[1];
            switch (o[0])
            {
                case 0: owner = 0x07; break;	// literal
                case 1: owner = 0x09; break;	// param
                case 2: owner = 0x19; break;	// local
            }
            if ((o[4] & 0x08) != 0 && instruction.NodeVersion != 0)
                s += dataOwner(0x08, 2) + ",3"; // was "temp 3 and 4"  (SimAntics error)
            else
                s += dataOwner(lng, owner, o[2], o[3]);
            s += " * ";
            s += ((o[4] & 0x04) != 0)
                ? dataOwner(0x08, 2)
                : (ToShort(o[7], o[8]) == 0)
                    ? "1"
                    : "0x" + SimPe.Helper.HexString(ToShort(o[7], o[8]));
            s += ")";

			return s;
		}
	}

	public class WizPrimi0x001a : BhavWizPrimi	// Relationship
	{
		public WizPrimi0x001a(Instruction i) : base(i) { }

		protected override string Operands(bool lng)
		{
			byte[] o = new byte[16];
			((byte[])instruction.Operands).CopyTo(o, 0);
			((byte[])instruction.Reserved1).CopyTo(o, 8);

			string s = "";

            if ((o[1] & 0x04) == 0)
            {
                if (instruction.NodeVersion == 0)	// old-style parameter usage
                    s += dataOwner(o[4], ToShort(o[6], o[7]));
                else
                    s += dataOwner(o[8], ToShort(o[9], o[10]));
                s += " := ";
            }

            s += readStr(Scope.Global, GS.GlobalStr.Relationship, o[0], -1, Detail.ErrorNames); // fixed scope and file

            if ((o[1] & 0x04) != 0)
            {
                s += " := ";
                if (instruction.NodeVersion == 0)	// old-style parameter usage
                    s += dataOwner(o[4], ToShort(o[6], o[7]));
                else
                    s += dataOwner(o[8], ToShort(o[9], o[10]));
            }

            s += ", " + pjse.Localization.GetString("bwp1a_relationship")
                + ": ";
            if (instruction.NodeVersion == 0)	// old-style parameter usage
                s += readStr(GS.BhavStr.RelVar, (ushort)(o[1] & 3));
            else	// new-style parameter usage
                s += dataOwner(lng, o[2], ToShort(o[3], o[4])) + " .. " + dataOwner(lng, o[5], ToShort(o[6], o[7]));

            if (lng)
                if (instruction.NodeVersion == 0)	// old-style parameter usage
                {
                    s += ", " + pjse.Localization.GetString("bwp1a_failTooSmall")
                        + ": " + ((o[2] & 0x01) != 0).ToString();
                    s += ", " + pjse.Localization.GetString("bwp1a_useNIDs")
                        + ": " + ((o[2] & 0x02) != 0).ToString();
                }
                else
                {
                    s += ", " + pjse.Localization.GetString("bwp1a_failTooSmall")
                        + ": " + ((o[1] & 0x01) != 0).ToString();
                    s += ", " + pjse.Localization.GetString("bwp1a_useNIDs")
                        + ": " + ((o[1] & 0x02) != 0).ToString();
                    s += ", " + pjse.Localization.GetString("bwp1a_noCheckObj2")
                        + ": " + ((o[1] & 0x08) != 0).ToString(); // "object to sim" relationship
                }
			return s;
		}
	}

	public class WizPrimi0x001b : BhavWizPrimi	// Go To Relative Position
	{
		public WizPrimi0x001b(Instruction i) : base(i) { }

        public override ABhavOperandWiz Wizard()
        {
            return new pjse.BhavOperandWizards.BhavOperandWiz0x001b(instruction);
        }

        protected override string Operands(bool lng)
		{
			byte[] o = new byte[16];
			((byte[])instruction.Operands).CopyTo(o, 0);
			((byte[])instruction.Reserved1).CopyTo(o, 8);

			string s = "";

			s += (lng
                ? pjse.Localization.GetString("bwp_Location")
                    + ": "
                : ""
                ) + readStr(GS.BhavStr.RelativeLocations, (byte)(o[2] + 2));
            s += ", " + (lng
                ? pjse.Localization.GetString("Direction")
                    + ": "
                : ""
                ) + readStr(GS.BhavStr.RelativeDirections, (byte)(o[3] + 2));
			if (lng)
			{
                s += ", " + pjse.Localization.GetString("bwp_noFailureTrees")
                    + ": " + ((o[6] & 0x02) != 0).ToString();
                s += ", " + pjse.Localization.GetString("bwp1b_differentAltitudes")
                    + ": " + ((o[6] & 0x04) != 0).ToString();
			}
			return s;
		}
	}

	public class WizPrimi0x001c : BhavWizPrimi	// Run Tree By Name
	{
		public WizPrimi0x001c(Instruction i) : base(i) { }

        public override ABhavOperandWiz Wizard()
        {
            return new pjse.BhavOperandWizards.BhavOperandWiz0x001c(instruction);
        }

        protected override string Operands(bool lng)
		{
			byte[] o = new byte[16];
			((byte[])instruction.Operands).CopyTo(o, 0);
			((byte[])instruction.Reserved1).CopyTo(o, 8);
            Boolset options = (byte)(o[2] & 0x3f);

			string s = "";

			Scope scope = Scope.Private;
            if      (options[0]) scope = Scope.Global;
            else if (options[1]) scope = Scope.SemiGlobal;

            if (lng)
                s += pjse.Localization.GetString("bwp1c_treeName") + ": ";

            s += readStr(scope, GS.GlobalStr.NamedTree, (ushort)(ToShort(o[4], (byte)(o[2] >> 6)) - 1), lng ? -1 : 60, lng ? Detail.Normal : Detail.ErrorNames);

            if (lng)
            {
                s += ", " + pjse.Localization.GetString("bwp1c_search") + ": ";
                s += pjse.Localization.GetString("Private");
                s += !options[3] ? " " + pjse.Localization.GetString("SemiGlobal") : "";
                s += !options[2] ? " " + pjse.Localization.GetString("Global") : "";

                s += ", " + readStr(GS.BhavStr.RTBNType, o[5]);

                if ((o[2] & 0x30) != 0) s += ", " + pjse.Localization.GetString("manyArgs") + ": ";
                if (options[5])
                    s += pjse.Localization.GetString("bw_callerparams");
                if ((o[2] & 0x30) == 0x30) s += ", ";
                if (options[4]) // Data Owner format
                    for (int i = 0; i < 3; i++)
                        s += (i == 0 ? "" : ", ") + dataOwner(o[6 + i * 3], o[6 + (i * 3) + 1], o[6 + (i * 3) + 2]);
            }
			return s;
		}
	}

	public class WizPrimi0x001d : BhavWizPrimi	// Set Motive Change -- for wizard, see edithWiki CreatingAChair
	{
		public WizPrimi0x001d(Instruction i) : base(i) { }

		protected override string Operands(bool lng)
		{
			byte[] o = new byte[16];
			((byte[])instruction.Operands).CopyTo(o, 0);
			((byte[])instruction.Reserved1).CopyTo(o, 8);

			string s = "";

			s += ((o[3] & 0x01) != 0
                ? pjse.Localization.GetString("bwp1d_clearAll")
				: dataOwner(lng, 0x0E, o[2]) // My Motives
					+ " += " + dataOwner(lng, o[0], o[4], o[5])
                    + " " + (lng
                        ? pjse.Localization.GetString("bwp1d_perHour")
                            + ", " + pjse.Localization.GetString("bwp1d_stopAt")
                            + ":"
                        : "..")
                    + " " + dataOwner(lng, o[1], o[6], o[7])
				);

			if (lng)
			{
                s += ", " + pjse.Localization.GetString("bwp1d_autoClear")
                    + ": " + ((o[3] & 0x02) != 0).ToString();
			}

			return s;
		}
	}

	public class WizPrimi0x001e : BhavWizPrimi	// Gosub Action
	{
		public WizPrimi0x001e(Instruction i) : base(i) { }

		protected override string Operands(bool lng)
		{
			byte[] o = new byte[16];
			((byte[])instruction.Operands).CopyTo(o, 0);
			((byte[])instruction.Reserved1).CopyTo(o, 8);

			string s = "";

			s += readStr(GS.BhavStr.GosubAction, o[0]);

			return s;
		}
	}

	public class WizPrimi0x001f : BhavWizPrimi	// Set to Next
	{
		public WizPrimi0x001f(Instruction i) : base(i) { }

        public override ABhavOperandWiz Wizard()
        {
            return new pjse.BhavOperandWizards.BhavOperandWiz0x001f(instruction);
        }

        protected override string Operands(bool lng)
		{
			byte[] o = new byte[16];
			((byte[])instruction.Operands).CopyTo(o, 0);
			((byte[])instruction.Reserved1).CopyTo(o, 8);

			string s = "";

            s += ((o[4] & 0x80) == 0
                ? dnStkOb() // Stack Object
                : dataOwner(lng, o[5], o[7])) + ", "; // ":=" didn't look right here.

            s += readStr(GS.BhavStr.NextObject, (ushort)(o[4] & 0x7f));
			switch(o[4] & 0x7f)
			{
				case 0x04: case 0x07:
                    s += ": " + BhavWiz.FormatGUID(lng, o, 0);
					break;
				case 0x09: case 0x22:
					s = s.Replace("[local]", dataOwner(lng, 0x19, o[6])); // local
					break;
			}

			if (lng && instruction.NodeVersion != 0)
			{
				if ((o[8] & 0x02) != 0)
					s += " && " + readStr(GS.BhavStr.DataLabels, ToShort(o[9], o[10])) + " == 0x" + SimPe.Helper.HexString(ToShort(o[11], o[12]));
                s += ", " + pjse.Localization.GetString("bwp1f_disabledObjects")
                    + ": " + ((o[8] & 0x01) != 0).ToString();
			}
			return s;
		}
	}

	public class WizPrimi0x0020 : BhavWizPrimi	// Test Object Type
	{
		public WizPrimi0x0020(Instruction i) : base(i) { }

        public override ABhavOperandWiz Wizard()
        {
            return new pjse.BhavOperandWizards.BhavOperandWiz0x0020(instruction);
        }

        protected override string Operands(bool lng)
		{
			byte[] o = new byte[16];
			((byte[])instruction.Operands).CopyTo(o, 0);
			((byte[])instruction.Reserved1).CopyTo(o, 8);

			string s = "";

			s += dataOwner(lng, o[6], o[4], o[5]);

            s += ", " + pjse.Localization.GetString("bwp20_isInstanceOf");

            s += ": " + BhavWiz.FormatGUID(lng, o, 0);
            if (lng)
			{
				//if (d1 == 0x4C7CAB2B)
				//	s += " (temporary inventory token)";

                s += ", " + pjse.Localization.GetString("bwp20_originalGUID")
                    + ": " + ((o[7] & 0x01) != 0).ToString();
                s += ", " + pjse.Localization.GetString("bwp20_neighbourID")
                    + ": " + ((o[7] & 0x02) != 0).ToString();
                s += ", " + pjse.Localization.GetString("bwp20_returnTemp01")
                    + ": " + ((o[7] & 0x04) != 0).ToString();
			}

			return s;
		}
	}

	public class WizPrimi0x0021 : BhavWizPrimi	// Find 5 Worst Motives
	{
		public WizPrimi0x0021(Instruction i) : base(i) { }

		protected override string Operands(bool lng)
		{
			byte[] o = new byte[16];
			((byte[])instruction.Operands).CopyTo(o, 0);
			((byte[])instruction.Reserved1).CopyTo(o, 8);

			string s = "";

            s += dataOwner(0x08, 0) + "..4 :=";

			s += " " + readStr(GS.BhavStr.ShortOwner, ToShort(o[4], o[5]));
            s += " " + readStr(GS.BhavStr.MotiveType, ToShort(o[6], o[7]));

			return s;
		}
	}

	public class WizPrimi0x0022 : BhavWizPrimi	// UI Effect
	{
		public WizPrimi0x0022(Instruction i) : base(i) { }

		protected override string Operands(bool lng)
		{
			byte[] o = new byte[16];
			((byte[])instruction.Operands).CopyTo(o, 0);
			((byte[])instruction.Reserved1).CopyTo(o, 8);

			string s = "";

            s += readStr(GS.BhavStr.UIEffectType, o[0]);

			if (o[0] < 5 || o[0] > 8)
			{
				Scope scope = Scope.Private;
				if      ((o[5] & 0x04) != 0) scope = Scope.Global;
				else if ((o[5] & 0x08) != 0) scope = Scope.SemiGlobal;
                s += " " + readStr(scope, GS.GlobalStr.UIEffect, ToShort(o[3], o[4]), lng ? -1 : 60, lng ? Detail.Normal : Detail.ErrorNames);
			}

			if (lng)
			{
                if (o[0] != 8)
                {
                    Scope scope = Scope.Private;
                    if ((o[5] & 0x01) != 0) scope = Scope.Global;
                    else if ((o[5] & 0x02) != 0) scope = Scope.SemiGlobal;
                    s += ", " + pjse.Localization.GetString("bwp22_windowID")
                        + ": " + readStr(scope, GS.GlobalStr.UIEffect, ToShort(o[1], o[2]), -1, lng ? Detail.Normal : Detail.ErrorNames);
                }
                else
                    s += ", " + pjse.Localization.GetString("bwp_TNSID")
                        + ": " + dataOwner(o[13], o[11], o[12]);

                if (o[0] == 3)
                    s += ", " + (ToShort(o[6], o[7]) != 0
                        ? pjse.Localization.GetString("bwp22_startingEffect")
                        : pjse.Localization.GetString("bwp22_stoppingEffect")
                        );

                if (o[0] == 4 || o[0] == 8)
					{
						Scope scope = Scope.Global;
						if      (o[10] == 0) scope = Scope.Private;
						else if (o[10] == 1) scope = Scope.SemiGlobal;
						bool found = false;
                        s += ", " + pjse.Localization.GetString("bwp_eventTree")
                            + ": " + bhavName(ToShort(o[8], o[9]), ref found);
                        s += " (" + pjse.Localization.GetString(scope.ToString()) + ")";
                    }
			}
			return s;
		}
	}

	public class WizPrimi0x0023 : BhavWizPrimi	// Camera Control
	{
		public WizPrimi0x0023(Instruction i) : base(i) { }

		protected override string Operands(bool lng)
		{
			byte[] o = new byte[16];
			((byte[])instruction.Operands).CopyTo(o, 0);
			((byte[])instruction.Reserved1).CopyTo(o, 8);

			string s = "";

			s += dnStkOb() + ": " + ((o[4] & 1) != 0
                ? pjse.Localization.GetString("bwp23_visible")
                : pjse.Localization.GetString("bwp23_notVisible")
                );

            s += ", " + pjse.Localization.GetString("bwp23_zoom")
                + ": 0x" + SimPe.Helper.HexString(o[3]) + " (";
			switch (o[3])
			{
                case 1: s += pjse.Localization.GetString("bwp23_far")
                    ; break;
                case 2: s += pjse.Localization.GetString("bwp23_mid")
                    ; break;
                case 3: s += pjse.Localization.GetString("bwp23_near")
                    ; break;
                default: s += pjse.Localization.GetString("unk")
                    ; break;
			}
			s += ")";

            s += ", " + pjse.Localization.GetString("bwp23_center")
                + ": " + ((o[4] & 0x08) != 0).ToString();
            s += ", " + pjse.Localization.GetString("bwp_timeout")
                + ": " + ((o[4] & 0x20) != 0 ? dataOwner(0x08, 0) : "0x" + SimPe.Helper.HexString(ToShort(o[0], o[1])));
            s += ", " + pjse.Localization.GetString("bwp23_slowDown")
                + ": " + ((o[4] & 0x40) == 0).ToString();

			return s;
		}
	}

	public class WizPrimi0x0024 : BhavWizPrimi	// Dialog
	{
		public WizPrimi0x0024(Instruction i) : base(i) { }

		public override ABhavOperandWiz Wizard()
		{
			return new pjse.BhavOperandWizards.BhavOperandWiz0x0024(instruction);
		}


		protected override string Operands(bool lng)
		{
			byte[] o = new byte[16];
			((byte[])instruction.Operands).CopyTo(o, 0);
			((byte[])instruction.Reserved1).CopyTo(o, 8);

            bool tvState = false;
            bool tnsState = false;
            bool lvState = false;
            bool gtState = false;
            bool[] states = { false, false, false, false, false }; // message, yes, no, cancel, title

            switch (o[5])
            {
                case 0x00: case 0x03: case 0x04:
                    states[0] = states[1] = states[4] = true; // message, button 1, title
                    break;
                case 0x02:
                    tvState = states[0] = states[1] = states[2] = states[3] = states[4] = true; // message, button 1, button 2, button 3, title
                    break;
                case 0x08: case 0x0a: // TNS, TNS modify
                    tnsState = tvState = states[0] = true; // message
                    break;
                case 0x09: // TNS stop
                    tvState = true;
                    break;
                case 0x0e:
                    lvState = states[0] = states[1] = states[2] = states[4] = true; // message, button 1, button 2, title
                    break;
                case 0x0f:
                    states[1] = states[2] = true; // button 1, button 2
                    break;
                case 0x13:
                    states[1] = states[2] = states[4] = true; // button 1, button 2, title
                    break;
                case 0x0b: case 0x0c: case 0x0d: case 0x10: case 0x11: case 0x12: case 0x14: case 0x15:
                    break;
                case 0x16: case 0x19:
                    states[0] = states[4] = true; // message, title
                    break;
                case 0x1c: // TNS append
                    tvState = states[0] = true; // message
                    break;
                case 0x39: // Game Tip - CJH
                    gtState = true;
                    break;
                default:
                    states[0] = states[1] = states[2] = states[4] = true; // message, button 1, button 2, title
                    break;
            }


			ushort msg, cnc;
			if (instruction.NodeVersion == 0)
			{
				msg = o[2];	// message
				cnc = o[0];	// cancel
			} 
			else 
			{
				msg = ToShort(o[13], o[14]);	// message
				cnc = ToShort(o[0], o[2]);	// cancel
			}

			Scope scope;
			if      ((o[8] & 0x01) != 0) scope = Scope.SemiGlobal;
			else if ((o[8] & 0x40) != 0) scope = Scope.Global;
			else                         scope = Scope.Private;

			string s = "";

            s += readStr(GS.BhavStr.Dialog, o[5]);

            if (gtState) // Game Tips don't set any text from the dialogue, ignoring them speeds up loading a lot - CJH
            s += ".  (" + readStr(GS.BhavStr.DialogDesc, o[5]) + ")";
            else
            {
            if (lng)
                s += ", " + pjse.Localization.GetString("bwp24_strings")
                    + ": " + pjse.Localization.GetString(scope.ToString());

            if (states[4]) s += ", " + pjse.Localization.GetString("bwp24_title")
                + ": " + dialogStr(scope, (o[8] & 0x10) != 0, o[6], lng ? -1 : 60);
            if (states[0]) s += ", " + pjse.Localization.GetString("bwp_message")
                + ": " + dialogStr(scope, (o[8] & 0x02) != 0, msg, lng ? -1 : 60);
            if (lng)
            {
                if (states[1]) s += ", " + pjse.Localization.GetString("bwp24_button1")
                    + ": " + dialogStr(scope, (o[8] & 0x04) != 0, o[3]);
                if (states[2]) s += ", " + pjse.Localization.GetString("bwp24_button2")
                    + ": " + dialogStr(scope, (o[8] & 0x08) != 0, o[4]);
                if (states[3]) s += ", " + pjse.Localization.GetString("bwp24_button3")
                    + ": " + dialogStr(scope, (o[8] & 0x20) != 0, cnc);
            }

            if (tnsState)
            {
                s += ", " + pjse.Localization.GetString("bwp24_TNSStyle")
                    + ": " + readStr(GS.BhavStr.TnsStyle, o[12]);
                if (lng)
                {
                    s += ", " + pjse.Localization.GetString("bwp_priority")
                        + ": 0x" + SimPe.Helper.HexString((byte)(o[9] + 1));
                    s += ", " + pjse.Localization.GetString("bwp_timeout")
                        + ": 0x" + SimPe.Helper.HexString(o[10]);
                }
            }

			if (lng)
			{
                byte tempVar = (byte)((o[7] >> 4) & 0x07);
                if (tvState)
                    s += ", " + (o[5] == 0x02
                        ? pjse.Localization.GetString("bwp_resultIn")
                        : pjse.Localization.GetString("bwp_TNSID")
                        ) + ": " + dataOwner(0x08, tempVar); // temp

                if (lvState)
                    s += ", " + pjse.Localization.GetString("bwp24_Locals")
                        + ": " + dataOwner(0x19, o[11]); // local

                byte iconType = (byte)((o[7] >> 1) & 0x07);
                s += ", " + pjse.Localization.GetString("bwp_icon")
                    + ": " + readStr(GS.BhavStr.DialogIcon, iconType);
                switch (iconType)
                {
                    case 3: s += ": BMP = 0x" + SimPe.Helper.HexString((ushort)(o[1] + 5000)); break;
                    case 4: s += " " + dialogStr(scope, false, o[1]); break;
                }

                s += ", " + pjse.Localization.GetString("bwp24_waitForUser")
                    + ": " + ((o[7] & 0x01) == 0);
                s += ", " + pjse.Localization.GetString("bwp24_blockSimulation")
                    + ": " + ((o[7] & 0x80) == 0);

				s += ".  (" + readStr(GS.BhavStr.DialogDesc, o[5]) + ")";
			}
        }

			return s;
		}


		private string dialogStr(Scope scope, bool temp, ushort instance, int len)
		{
			string s = "";
			if (temp)
				s += GS.GlobalStr.DialogString.ToString() + ":[" + dataOwner(false, 0x08, instance) + "]"; // temp
			else
			{
				if (instance != 0)
                    s += readStr(scope, (uint)GS.GlobalStr.DialogString, (ushort)(instance - 1), len, Detail.ErrorNames, true);
				else
                    s += "[" + pjse.Localization.GetString("none") + "]";
			}
			return s;
		}

		private string dialogStr(Scope scope, bool temp, ushort instance) { return dialogStr(scope, temp, instance, -1); }

	}

	public class WizPrimi0x0025 : BhavWizPrimi	// Test Sim Interacting With
	{
		public WizPrimi0x0025(Instruction i) : base(i) { }

		protected override string Operands(bool lng)
		{
			return dnMe() + " .. " + dnStkOb();
		}

	}

	public class WizPrimi0x002a : BhavWizPrimi	// Create new object instance -- for wizard, see edithWiki AkeaPostMortem
	{
		public WizPrimi0x002a(Instruction i) : base(i) { }

		protected override string Operands(bool lng)
		{
			byte[] o = new byte[16];
			((byte[])instruction.Operands).CopyTo(o, 0);
			((byte[])instruction.Reserved1).CopyTo(o, 8);

			string s = "";

            if ((o[5] & 0x04) != 0) s += DoidName(0x18);
            else if ((o[5] & 0x40) != 0) s += (lng ? "GUID: " : "") + DoidName(0x27);
            else if ((o[5] & 0x80) != 0) s += (lng ? "GUID: " : "") + dataOwner(0x08, 0x00) + ",1";
            else s += BhavWiz.FormatGUID(lng, o, 0);

			if (lng)
			{
                s += ", " + pjse.Localization.GetString("bwp2a_place")
                    + ": " + readStr(GS.BhavStr.CreatePlace, o[4]);
				switch (o[4]) 
				{
                    case 0x04: s = s.Replace(DoidName(0x10), dataOwner(0x10, o[9])); break;
					case 0x08: case 0x09: s = s.Replace(dnLocal(), dataOwner(0x19, o[6])); break;
                    case 0x0A: s = s.Replace("[slot]", "0x" + SimPe.Helper.HexString(o[9])); break;
                }

				s += ", " + readStr(GS.BhavStr.CreateHow, (ushort)(o[5] & 0x03));
                s += ", " + pjse.Localization.GetString("bwp2a_failNonEmpty")
                    + ": " + ((o[5] & 0x08) != 0).ToString();
                s += ", " + pjse.Localization.GetString("bwp2a_passTemp0")
                    + ": " + ((o[5] & 0x10) != 0).ToString();

                s += ", " + pjse.Localization.GetString("bwp2a_moveInNewSim")
                    + ": " + ((o[10] & 0x01) != 0).ToString();
                s += ", " + pjse.Localization.GetString("bwp2a_copyTemp5")
                    + ": " + ((o[10] & 0x02) != 0).ToString();
			}

			return s;
		}
	}

	public class WizPrimi0x002d : BhavWizPrimi	// Go To Routing Slot -- for wizard, see edithWiki CreatingAChair
	{
		public WizPrimi0x002d(Instruction i) : base(i) { }

		public override ABhavOperandWiz Wizard()
		{
            return new pjse.BhavOperandWizards.BhavOperandWiz0x002d(instruction);
		}

		protected override string Operands(bool lng)
		{
			byte[] o = new byte[16];
			((byte[])instruction.Operands).CopyTo(o, 0);
			((byte[])instruction.Reserved1).CopyTo(o, 8);

            string s = "";

			if ((o[4] & 0x02) == 0)
				switch (ToShort(o[2], o[3])) 
				{
					case 0:
						s += dataOwner(lng, 0x09, o[0], o[1]); // Param
						break;
					case 1:
						s += "0x" + SimPe.Helper.HexString(ToShort(o[0], o[1]));
						break;
					case 2:
						s += pjse.Localization.GetString("lcGlobal")
                            + " 0x" + SimPe.Helper.HexString(ToShort(o[0], o[1]));
						break;
					case 3:
						s += dataOwner(lng, 0x19, o[0], o[1]); // Local
						break;
					default:
						s += "??? 0x" + SimPe.Helper.HexString(ToShort(o[0], o[1]));
						break;
				}
			else
				s += dataOwner(lng, 0x08, o[0], o[1]); // Temp

			if (lng)
			{
                s += ", " + pjse.Localization.GetString("bwp_noFailureTrees")
                    + ": " + ((o[4] & 0x01) != 0).ToString();
                s += ", " + pjse.Localization.GetString("bwp2d_ignoreDestObjFootprint")
                    + ": " + ((o[4] & 0x04) != 0).ToString();
                s += ", " + pjse.Localization.GetString("bwp2d_allowDiffAltitudes")
                    + ": " + ((o[4] & 0x08) != 0).ToString();
			}

			return s;
		}
	}

	public class WizPrimi0x002e : BhavWizPrimi	// Snap -- for wizard, see edithWiki CreatingAChair (assume this is s/t/r/s)
	{
		public WizPrimi0x002e(Instruction i) : base(i) { }

		protected override string Operands(bool lng)
		{
			byte[] o = new byte[16];
			((byte[])instruction.Operands).CopyTo(o, 0);
			((byte[])instruction.Reserved1).CopyTo(o, 8);

			string s = "";

			ushort snapType = ToShort(o[2], o[3]);

			s += readStr(GS.BhavStr.SnapType, snapType);

            if ((o[4] & 0x08) != 0)
            {
                if (snapType == 0)
                    s += " [" + dataOwner(0x08, 0x00) + "]"; // Temp
                else if (snapType == 3 || snapType == 4)
                    s = s.Replace("[slot]", "[" + dataOwner(0x08, 0x00) + "]");
            }
            else
            {
                if (snapType == 0)
                    s = s.Replace(dnParam(), dataOwner(lng, 0x09, o[0], o[1])); // Param
                else if (snapType == 3 || snapType == 4)
                    s = s.Replace("[slot]", "0x" + SimPe.Helper.HexString(ToShort(o[0], o[1])));
            }

			if (lng)
			{
                s += ", " + pjse.Localization.GetString("bwp2e_fromTemp1")
                    + ": " + (ToShort(o[8], o[9]) == 1).ToString();

                s += ", " + pjse.Localization.GetString("bwp2e_askSimToMove")
                    + ": " + ((o[4] & 0x02) != 0).ToString();
                s += ", " + pjse.Localization.GetString("bwp_testOnly")
                    + ": " + ((o[4] & 0x10) != 0).ToString();
			}

			return s;
		}
	}

	public class WizPrimi0x0030 : BhavWizPrimi	// Stop ALL Sounds
	{
		public WizPrimi0x0030(Instruction i) : base(i) { }

		protected override string Operands(bool lng)
		{
			byte[] o = new byte[16];
			((byte[])instruction.Operands).CopyTo(o, 0);
			((byte[])instruction.Reserved1).CopyTo(o, 8);

			return (ToShort(o[0], o[1]) == 0 ? dnMe() : dnStkOb());
		}
	}

	public class WizPrimi0x0031 : BhavWizPrimi	// Notify the Stack Object out of Idle
	{
		public WizPrimi0x0031(Instruction i) : base(i) { }

		protected override string Operands(bool lng)
		{
			return "-";
		}

	}

	public class WizPrimi0x0032 : BhavWizPrimi	// Add/Change action string (disaSim2 24b)
	{
		public WizPrimi0x0032(Instruction i) : base(i) { }

        public override ABhavOperandWiz Wizard()
        {
            return new pjse.BhavOperandWizards.BhavOperandWiz0x0032(instruction);
        }

        protected override string Operands(bool lng)
		{
			byte[] o = new byte[16];
			((byte[])instruction.Operands).CopyTo(o, 0);
			((byte[])instruction.Reserved1).CopyTo(o, 8);

			string s = "";

			if (o[9] == 0) 
			{
				Scope scope = Scope.Private;
				if      ((o[2] & 0x04) != 0) scope = Scope.Global;
				else if ((o[2] & 0x08) != 0) scope = Scope.SemiGlobal;

				if (lng)
				{
                    s += pjse.Localization.GetString("bwp32_addChange");
					if (instruction.NodeVersion != 0)
					{
                        s += ", " + pjse.Localization.GetString("bwp32_disabled")
                            + ": ";
                        if      ((o[3] & 0x01) != 0) s += pjse.Localization.GetString("bwp32_propagating");
                        else if ((o[3] & 0x02) != 0) s += pjse.Localization.GetString("bwp32_nonPropagating");
						else                         s += false.ToString();
					}
                    if (instruction.NodeVersion > 2)
                        s += ", " + pjse.Localization.GetString("bwp32_subqueue")
                            + ": " + ((o[3] & 0x10) != 0);
                    s += ", ";
                }

				if ((o[2] & 0x10) != 0) s += GS.GlobalStr.MakeAction.ToString() + ":[" + dataOwner(false, 0x08, 0) + "]"; // Temp 0
				else s += readStr(scope, GS.GlobalStr.MakeAction,
                        (ushort)((instruction.NodeVersion < 2 ? o[0x04] : ToShort(o[0x0e], o[0x0f])) - 1),
                        lng ? -1 : 60, lng ? Detail.Normal : Detail.ErrorNames);
			}
			else 
			{
                s += pjse.Localization.GetString("bwp32_iconChange");

                s += ", " + pjse.Localization.GetString("bwp32_iconIndex")
                    + ": " + (((o[2] & 0x80) != 0)
                    ? dataOwner(false, 0x08, 1) // Temp 1
                    : "0x" + SimPe.Helper.HexString(o[10]));

                if (lng)
                {
                    if ((o[2] & 0x20) != 0)
                        s += ", " + pjse.Localization.GetString("bwp32_thumbnailOutfit")
                            + ": " + (((o[2] & 0x40) != 0)
                            ? "GUID " + dataOwner(false, 0x08, 2) + ",3" // Temp 2,3
                            : BhavWiz.FormatGUID(lng, o, 5));
                    else
                        s += ", " + pjse.Localization.GetString("Object")
                            + ": " + dataOwner(lng, o[11], o[12], o[13]);
                }
			}

			return s;
        }
	}

	public class WizPrimi0x0033 : BhavWizPrimi	// Manage Inventory -- for wizard, see edithWiki WorkAndSchool, Career rewards (disaSim2 24b)
	{
		public WizPrimi0x0033(Instruction i) : base(i) { }

        public override ABhavOperandWiz Wizard()
        {
            return new pjse.BhavOperandWizards.BhavOperandWiz0x0033(instruction);
        }

        private string tokenType(int i, int j, bool all)
		{
            //string[] tokType = { "any", "all", "non-visible", "visible", "non-memory", "memory", "non-shopping", "shopping", };

			string s = "";
            if ((i & 0x04) != 0) s += readStr(GS.BhavStr.TokenType, (ushort)(2 + ((j & 0x10) == 0 ? 0 : 1))) + " ";
            if ((i & 0x08) != 0) s += readStr(GS.BhavStr.TokenType, (ushort)(4 + ((j & 0x20) == 0 ? 0 : 1))) + " ";
            if ((i & 0x20) != 0) s += readStr(GS.BhavStr.TokenType, (ushort)(6 + ((i & 0x01) == 0 ? 0 : 1))) + " ";
            if ((i & 0x2c) == 0) s += readStr(GS.BhavStr.TokenType, (ushort)(all ? 1 : 0)) + " ";
			return s.Trim();
		}

		protected override string Operands(bool lng)
		{
			byte[] o = new byte[16];
			((byte[])instruction.Operands).CopyTo(o, 0);
			((byte[])instruction.Reserved1).CopyTo(o, 8);

			string s = "";

			byte c1 = (instruction.NodeVersion >= 1) ? o[0] : (byte)(((o[0] & 0x3C) << 1) | (o[0] & 0x83));
			byte c2 = (instruction.NodeVersion >= 2) ? o[9] : (byte)0x0c;

            bool byGUID, index2, index, count, frominv, reversed, propno, propval, ignoreinv;
            byGUID = index2 = index = count = frominv = reversed = propno = propval = ignoreinv = false;
            int toktype = 0;

            if ((c1 & 0x08) != 0) // Counted
            {
                s += readStr(GS.BhavStr.TokenOpsCounted, o[4]);
                switch (o[4])
                {
                    case 0x00: byGUID = count = true; toktype = 2; break;
                    case 0x01: index = count = true; break;
                    case 0x02: byGUID = count = true; toktype = 2; break;
                    case 0x03: index = count = true; break;

                    case 0x04: byGUID = true; toktype = 2; break;
                    case 0x05: index = true; break;
                    case 0x06: byGUID = index = count = true; toktype = 2; break; // Find Token Of GUID, returns count, noy sure if index is actually used 
                    case 0x07: byGUID = true; toktype = 2; break;

                    case 0x08: index = true; break;
                    case 0x09: index = true; toktype = 1; break;
                    case 0x0a: byGUID = count = true; toktype = 2; break;
                    case 0x0b: index = true; frominv = lng; toktype = 2; break;
                }
            }
            else // Singular
            {
                s += readStr(GS.BhavStr.TokenOpsSingular, o[4]);
                switch (o[4])
                {
                    case 0x00: byGUID = true; toktype = 2; break;
                    case 0x01: index = true; break;
                    case 0x02: byGUID = true; toktype = 2; break;
                    case 0x03: byGUID = true; toktype = 2; index = reversed = true; break;

                    case 0x04: index = propval = true; break;
                    case 0x05: index = propval = true; break;
                    case 0x06: index = true; break;
                    case 0x07: propno = propval = ignoreinv = true; break;

                    case 0x08: propno = propval = ignoreinv = true; break;
                    case 0x09: ignoreinv = true; break;
                    case 0x0a: count = true; break;
                    case 0x0b: toktype = 2; break;

                    case 0x0c: toktype = 2; index = reversed = true; break;
                    case 0x0d: toktype = 1; count = true; break;
                    case 0x0e: index2 = propno = propval = true; break;
                    case 0x0f: index2 = propno = propval = true; break;

                    case 0x10: toktype = 2; break;
                    case 0x11: index = true; break;
                    case 0x12: toktype = 2; frominv = lng; index = true; break;
                    case 0x13: toktype = 2; break;
                }
            }

            if (byGUID)
            {
                uint d1 = (uint)(o[5] | (o[6] << 8) | (o[7] << 16) | (o[8] << 24));
                s += (lng ? ", " + pjse.Localization.GetString("bwp33_token") + ":" : "") + " "
                    + (d1 == 0 ? dnStkOb() : BhavWiz.FormatGUID(lng, d1));
            }

            if (toktype != 0)
                s += (lng ? ", " + pjse.Localization.GetString("bwp33_category") : "") + ": "
                    + ((instruction.NodeVersion >= 2) ? "0x" + SimPe.Helper.HexString(o[9]) + " - " : "")
                    + tokenType(c2, c1, toktype == 1);

            if (lng)
            {
                if (reversed)
                    s += ", " + pjse.Localization.GetString("bwp33_reversed") + ": " + ((c1 & 0x80) != 0).ToString();

                if (!ignoreinv)
                {
                    s += ", " + pjse.Localization.GetString("bwp33_Inventory");
                    s += " (" + ((c1 & 0x08) != 0
                        ? pjse.Localization.GetString("bwp33_counted")
                        : pjse.Localization.GetString("bwp33_singular")
                        ) + ")";
                    s += ": " + readStr(GS.BhavStr.InventoryType, (ushort)(c1 & 0x07));
                    if ((c1 & 0x07) >= 1 && (c1 & 0x07) <= 3)
                        s += /*", " + "ID" +*/ ": " + dataOwner(o[1], o[2], o[3]);

                    if (frominv)
                    {
                        s += ", " + pjse.Localization.GetString("bwp33_fromInventory") + ": " + readStr(GS.BhavStr.InventoryType, (ushort)(o[6] & 0x07));
                        if ((o[6] & 0x07) >= 1 && (o[6] & 0x07) <= 3)
                            s += /*", " + "ID" +*/ ": " + dataOwner(lng, o[13], o[14], o[15]);
                    }

                    if (index)
                        s += ", " + pjse.Localization.GetString("bwp33_index") + ": " + dataOwner(lng, o[10], o[11], o[12]);
                    if (index2)
                        s += ", " + pjse.Localization.GetString("bwp33_index") + ": " + dataOwner(lng, o[6], o[7], o[8]);
                }

                if (propno)
                    s += ", " + pjse.Localization.GetString("bwp33_property") + ": " + dataOwner(lng, o[10], o[11], o[12]);
                if (propval)
                    s += ", " + pjse.Localization.GetString("Value") + ": " + dataOwner(lng, o[13], o[14], o[15]);

                if (count)
                    s += ", " + pjse.Localization.GetString("bwp33_count") + ": " + dataOwner(lng, o[13], o[14], o[15]);
            }

            return s;
		}
	}

	public class WizPrimi0x0069 : BhavWizPrimi	// Animate Object
	{
		public WizPrimi0x0069(Instruction i) : base(i) { }

        public override ABhavOperandWiz Wizard()
        {
            return new pjse.BhavOperandWizards.BhavOperandWizAnimate(instruction, "bwp_Object");
        }

        protected override string Operands(bool lng)
        {
            byte[] o = new byte[16];
            ((byte[])instruction.Operands).CopyTo(o, 0);
            ((byte[])instruction.Reserved1).CopyTo(o, 8);

            string s = "";

            s += (lng ? pjse.Localization.GetString("Object") + ": " : "")
                + dataOwner(lng, o[6], o[7], o[8]);       // target object

            s += ", " + (lng ? pjse.Localization.GetString("bwp_animation") + ": " : "")
                + ((o[2] & 0x04) != 0
                ? "ObjectAnims:[" + dataOwner(lng, 0x09, o[0], o[1]) + "]" // Param
                : readStr(GS.GlobalStr.ObjectAnims, ToShort(o[0], o[1]), lng ? -1 : 60, lng ? Detail.Normal : Detail.ErrorNames)
                );

            if (lng)
            {
                bool found = false;
                s += ", " + pjse.Localization.GetString("bwp_eventTree") + ": " + bhavName(ToShort(o[4], o[5]), ref found);

                Scope scope = Scope.Global;
                if (o[9] == 0) scope = Scope.Private;
                else if (o[9] == 1) scope = Scope.SemiGlobal;
                s += " (" + pjse.Localization.GetString(scope.ToString()) + ")";

                s += ", " + pjse.Localization.GetString("bwp_animSpeed") + ": " + ((o[2] & 0x02) != 0 ? dataOwner(0x08, 2) : "---"); // Temp 2
                s += ", " + pjse.Localization.GetString("bwp_interruptible") + ": " + ((o[2] & 0x08) != 0).ToString();
                s += ", " + pjse.Localization.GetString("bwp_startTag") + ": " + ((o[2] & 0x10) != 0 ? dataOwner(0x08, 0) : "---"); // Temp 0
                s += ", " + pjse.Localization.GetString("bwp_loopCount") + ": " + ((o[2] & 0x20) != 0 ? dataOwner(0x08, 1) : "---");
                s += ", " + pjse.Localization.GetString("bwp_blendOut") + ": " + ((o[2] & 0x40) == 0).ToString();
                s += ", " + pjse.Localization.GetString("bwp_blendIn") + ": " + ((o[2] & 0x80) == 0).ToString();

                s += ", " + pjse.Localization.GetString("bwp_flipFlag") + ": " + (
                    (o[10] & 0x01) != 0 ? dataOwner(0x08, 3) // Temp 3
                    : ((o[2] & 0x01) != 0).ToString()
                    );
                s += ", " + pjse.Localization.GetString("bwp_sync") + ": " + ((o[10] & 0x04) != 0).ToString();
                s += ", " + pjse.Localization.GetString("bwp_alignBlend") + ": " + ((o[10] & 0x08) != 0).ToString();
                s += ", " + pjse.Localization.GetString("bwp_notHurryable") + ": " + ((o[10] & 0x80) != 0).ToString();
            }

            return s;
        }
	}

	public class WizPrimi0x006a : BhavWizPrimi	// Animate Sim -- for wizard, see edithWiki CreatingAChair
	{
		public WizPrimi0x006a(Instruction i) : base(i) { }

        public override ABhavOperandWiz Wizard()
        {
            return new pjse.BhavOperandWizards.BhavOperandWizAnimate(instruction, "bwp_Sim");
        }

        protected override string Operands(bool lng)
		{
			byte[] o = new byte[16];
			((byte[])instruction.Operands).CopyTo(o, 0);
			((byte[])instruction.Reserved1).CopyTo(o, 8);

			string s = "";

			Scope scope = Scope.Private;
			GS.GlobalStr instance = GS.GlobalStr.ObjectAnims;
			if (o[6] == 0x80)
			{
				instance = GS.GlobalStr.AdultAnims;
				scope = Scope.Global;
			}
			else try
				 {
					 instance = (GS.GlobalStr)o[6];
					 if (!instance.ToString().EndsWith("Anims"))
						 instance = GS.GlobalStr.ObjectAnims;
				 }
				 catch { instance = GS.GlobalStr.ObjectAnims; }

             s += (lng ? pjse.Localization.GetString("bwp_animation") + ": " : "")
                + ((o[2] & 0x04) != 0
                    ? instance.ToString() + ":[" + dataOwner(lng, 0x09, o[0], o[1]) + "]" // Param
				    + " (" + pjse.Localization.GetString(scope.ToString()) + ")"
				: readStr(scope, instance, ToShort(o[0], o[1]), lng ? -1 : 60, lng ? Detail.Normal : Detail.ErrorNames)
				);

			if (lng)
			{
                bool found = false;
                s += ", " + pjse.Localization.GetString("bwp_eventTree") + ": " + bhavName(ToShort(o[4], o[5]), ref found);

                scope = Scope.Global;
                if (o[7] == 0) scope = Scope.Private;
                else if (o[7] == 1) scope = Scope.SemiGlobal;
                s += " (" + pjse.Localization.GetString(scope.ToString()) + ")";

                s += ", " + pjse.Localization.GetString("bwp_animSpeed") + ": " + ((o[2] & 0x02) != 0 ? dataOwner(0x08, 2) : "---"); // Temp 2
                s += ", " + pjse.Localization.GetString("bwp_interruptible") + ": " + ((o[2] & 0x08) != 0).ToString();
                s += ", " + pjse.Localization.GetString("bwp_startTag") + ": " + ((o[2] & 0x10) != 0 ? dataOwner(0x08, 0) : "---"); // Temp 0
                s += ", " + pjse.Localization.GetString("bwp6a_transToIdle") + ": " + ((o[2] & 0x20) != 0).ToString();
                s += ", " + pjse.Localization.GetString("bwp_blendOut") + ": " + ((o[2] & 0x40) == 0).ToString();
                s += ", " + pjse.Localization.GetString("bwp_blendIn") + ": " + ((o[2] & 0x80) == 0).ToString();

                s += ", " + pjse.Localization.GetString("bwp_flipFlag") + ": " + (
                    (o[8] & 0x01) != 0 ? dataOwner(0x08, 3) // Temp 3
                    : ((o[2] & 0x01) != 0).ToString()
                    );
                s += ", " + pjse.Localization.GetString("bwp6a_sync") + ": " + ((o[8] & 0x02) != 0).ToString();
                s += ", " + pjse.Localization.GetString("bwp6a_controllerIsSource") + ": " + ((o[8] & 0x10) != 0).ToString();
                s += ", " + pjse.Localization.GetString("bwp_notHurryable") + ": " + ((o[8] & 0x20) != 0).ToString();

                s += ", " + pjse.Localization.GetString("bwp6a_IK") + ": " + dataOwner(o[9], ToShort(o[10], o[11]));
                s += ", " + pjse.Localization.GetString("bwp_priority") + ": 0x" + SimPe.Helper.HexString(o[12]) + " (";
				switch (o[12]) 
				{
                    case 0: s += pjse.Localization.GetString("bwp_low"); break;
                    case 1: s += pjse.Localization.GetString("bwp_medium"); break;
                    case 2: s += pjse.Localization.GetString("bwp_high"); break;
					default: s += pjse.Localization.GetString("unk"); break;
				}
				s += ")";
			}

			return s;
		}
	}

	public class WizPrimi0x006b : BhavWizPrimi	// Animate Overlay
	{
		public WizPrimi0x006b(Instruction i) : base(i) { }

        public override ABhavOperandWiz Wizard()
        {
            return new pjse.BhavOperandWizards.BhavOperandWizAnimate(instruction, "bwp_Overlay");
        }

        protected override string Operands(bool lng)
		{
			byte[] o = new byte[16];
			((byte[])instruction.Operands).CopyTo(o, 0);
			((byte[])instruction.Reserved1).CopyTo(o, 8);

			string s = "";

            s += (lng ? pjse.Localization.GetString("Object") + ": " : "")
                + dataOwner(lng, o[6], o[7], o[8]);       // target object

			Scope scope = Scope.Private;
			GS.GlobalStr instance = GS.GlobalStr.ObjectAnims;
			if (o[9] == 0x80)
			{
				instance = GS.GlobalStr.AdultAnims;
				scope = Scope.Global;
			}
			else try
				 {
					 instance = (GS.GlobalStr)o[9];
					 if (!instance.ToString().EndsWith("Anims"))
						 instance = GS.GlobalStr.ObjectAnims;
				 }
				 catch { instance = GS.GlobalStr.ObjectAnims; }

             s += ", " + (lng ? pjse.Localization.GetString("bwp_animation") + ": " : "")
                 + ((o[2] & 0x04) != 0
                 ? instance.ToString() + ":[" + dataOwner(lng, 0x09, o[0], o[1]) + "]" // Param
                    + (lng ? " (" + pjse.Localization.GetString(scope.ToString()) + ")" : "")
                 : readStr(scope, instance, ToShort(o[0], o[1]), lng ? -1 : 60, lng ? Detail.Full : Detail.ErrorNames) // variable instance
                 );

			if (lng)
			{
                bool found = false;
                s += ", " + pjse.Localization.GetString("bwp_eventTree") + ": " + bhavName(ToShort(o[4], o[5]), ref found);

                scope = Scope.Global;
                if (o[14] == 0) scope = Scope.Private;
                else if (o[14] == 1) scope = Scope.SemiGlobal;
                s += " (" + pjse.Localization.GetString(scope.ToString()) + ")";

                s += ", " + pjse.Localization.GetString("bwp_animSpeed") + ": " + ((o[2] & 0x02) != 0 ? dataOwner(0x08, 2) : "---"); // Temp 2
                s += ", " + pjse.Localization.GetString("bwp_interruptible") + ": " + ((o[2] & 0x08) != 0).ToString();
                s += ", " + pjse.Localization.GetString("bwp_startTag") + ": " + ((o[2] & 0x10) != 0 ? dataOwner(0x08, 0) : "---"); // Temp 0
                s += ", " + pjse.Localization.GetString("bwp_loopCount") + ": " + ((o[2] & 0x20) != 0 ? dataOwner(0x08, 1) : "---");
                s += ", " + pjse.Localization.GetString("bwp_blendOut") + ": " + ((o[2] & 0x40) == 0).ToString();
                s += ", " + pjse.Localization.GetString("bwp_blendIn") + ": " + ((o[2] & 0x80) == 0).ToString();

                s += ", " + pjse.Localization.GetString("bwp_flipFlag") + ": " + (
                    (o[15] & 0x01) != 0 ? dataOwner(0x08, 3) // Temp 3
                    : ((o[2] & 0x01) != 0).ToString()
                    );
                s += ", " + pjse.Localization.GetString("bwp_sync") + ": " + ((o[15] & 0x10) != 0).ToString();
                s += ", " + pjse.Localization.GetString("bwp_alignBlend") + ": " + ((o[15] & 0x20) != 0).ToString();

				byte priority;
				if (instruction.NodeVersion != 0)
				{
                    s += ", " + pjse.Localization.GetString("bwp_notHurryable") + ": " + ((o[12] & 0x01) != 0).ToString();
					priority = o[11];
				}
				else
					priority = o[12];

                s += ", " + pjse.Localization.GetString("bwp_priority") + ": 0x" + SimPe.Helper.HexString(priority) + " (";
                switch (priority)
                {
                    case 0: s += pjse.Localization.GetString("bwp_low"); break;
                    case 1: s += pjse.Localization.GetString("bwp_medium"); break;
                    case 2: s += pjse.Localization.GetString("bwp_high"); break;
                    default: s += pjse.Localization.GetString("unk"); break;
                }
                s += ")";
            }

			return s;
		}
	}

	public class WizPrimi0x006c : BhavWizPrimi	// Animate Stop (disaSim2 24b)
	{
		public WizPrimi0x006c(Instruction i) : base(i) { }

		protected override string Operands(bool lng)
		{
			byte[] o = new byte[16];
			((byte[])instruction.Operands).CopyTo(o, 0);
			((byte[])instruction.Reserved1).CopyTo(o, 8);

			string s = "";

            s += (lng ? pjse.Localization.GetString("Object") + ": " : "")
                + dataOwner(lng, o[3], o[4], o[5]);       // target object

            s += ", " + (lng ? pjse.Localization.GetString("bwp_animation") + ": " : "");
            if (o[7] == 0)
            {
                    Scope scope = Scope.Private;
                    GS.GlobalStr instance = GS.GlobalStr.ObjectAnims;
                    if (o[6] == 0x80)
                    {
                        instance = GS.GlobalStr.AdultAnims;
                        scope = Scope.Global;
                    }
                    else try
                        {
                            instance = (GS.GlobalStr)o[6];
                            if (!instance.ToString().EndsWith("Anims"))
                                instance = GS.GlobalStr.ObjectAnims;
                        }
                        catch { instance = GS.GlobalStr.ObjectAnims; }

                    s += ((o[2] & 0x04) != 0
                        ? instance.ToString() + ":[" + dataOwner(lng, 0x09, o[0], o[1]) + "]" // Param
                           + (lng ? " (" + pjse.Localization.GetString(scope.ToString()) + ")" : "")
                        : readStr(scope, instance, ToShort(o[0], o[1]), lng ? -1 : 60, lng ? Detail.Full : Detail.ErrorNames) // variable instance
                        );
                }
                else
                    s += readStr(GS.BhavStr.StopAnimType, o[7]);

			if (lng)
			{
                s += ", " + pjse.Localization.GetString("bwp_blendOut") + ": " + ((o[2] & 0x02) != 0).ToString();
                s += ", " + pjse.Localization.GetString("bwp_flipFlag") + ": " + (
                    (o[2] & 0x08) != 0 ? dataOwner(0x08, 3) // Temp 3
                    : ((o[2] & 0x01) != 0).ToString()
                    );
                s += ", " + pjse.Localization.GetString("bwp6c_shortBlendOut") + ": " + ((o[2] & 0x20) != 0).ToString();
                s += ", " + pjse.Localization.GetString("bwp6c_normalAndFlipped") + ": " + ((o[2] & 0x40) != 0).ToString();

                s += ", " + pjse.Localization.GetString("bwp_priority") + ": 0x" + SimPe.Helper.HexString(o[8]) + " (";
                switch (o[8])
                {
                    case 0: s += pjse.Localization.GetString("bwp_low"); break;
                    case 1: s += pjse.Localization.GetString("bwp_medium"); break;
                    case 2: s += pjse.Localization.GetString("bwp_high"); break;
                    default: s += pjse.Localization.GetString("unk"); break;
                }
                s += ")";
            }

			return s;
		}
	}

	public class WizPrimi0x006d : BhavWizPrimi	// Change Material
	{
		public WizPrimi0x006d(Instruction i) : base(i) { }

        public override ABhavOperandWiz Wizard()
        {
            return new pjse.BhavOperandWizards.BhavOperandWiz0x006d(instruction);
        }

        protected override string Operands(bool lng)
		{
			byte[] o = new byte[16];
			((byte[])instruction.Operands).CopyTo(o, 0);
			((byte[])instruction.Reserved1).CopyTo(o, 8);

			string s = "";

            s += (lng ? pjse.Localization.GetString("Target") + ": " : "")
                + dataOwner(lng, o[5], o[6], o[7]);       // target object

			Scope matScope = Scope.Private;
			if ((o[2] & 0x02) != 0) matScope = Scope.Global;
			else if ((o[2] & 0x04) != 0) matScope = Scope.SemiGlobal;

            s += ", " + (lng ? pjse.Localization.GetString("bwp6d_materialFrom") + ": " : "");
			if ((o[13] & 0x02) == 0)
			{
                s += ((o[2] & 0x08) != 0 ? pjse.Localization.GetString("bwp_source") : dnMe());
				s += " (" + (lng ? ((o[13] & 0x01) != 0
                    ? pjse.Localization.GetString("bwp6d_movingTexture")
                    : pjse.Localization.GetString("bwp6d_material")
                    ) + ": " : "");
                if ((o[2] & 0x10) != 0) s += GS.GlobalStr.MaterialName.ToString() + ":[" + dataOwner(lng, 0x08, 0) // Temp 0
                    + "]" + (lng ? " (" + matScope.ToString() + ")" : "");
                else if ((o[2] & 0x08) == 0) s += readStr(matScope, GS.GlobalStr.MaterialName, ToShort(o[0], o[1]), lng ? -1 : 30, lng ? Detail.Normal : Detail.ErrorNames);
                else s += GS.GlobalStr.MaterialName.ToString() + ":[0x" + SimPe.Helper.HexString(ToShort(o[0], o[1])) + "]" + (lng ? " (" + matScope.ToString() + ")" : "");
				s += ")";
			}
			else
                s += pjse.Localization.GetString("bwp6d_screenShot");

			Scope mgScope = Scope.Private;
			if ((o[2] & 0x40) != 0) mgScope = Scope.Global;
			else if ((o[2] & 0x80) != 0) mgScope = Scope.SemiGlobal;

            s += ", " + (lng ? pjse.Localization.GetString("bwp6d_meshFrom") + ": " : "") + ((o[2] & 0x01) != 0 ? pjse.Localization.GetString("bwp_source") : dnMe());
            if ((o[4] & 0x80) == 0) // w3 < 0
			{
                s += " (" + (lng ? pjse.Localization.GetString("bwp6d_meshGroup") + ": " : "");
                if ((o[2] & 0x20) != 0) s += GS.GlobalStr.MeshGroup.ToString() + ":[" + dataOwner(lng, 0x08, 1) // Temp 1
                    + "]" + (lng ? " (" + mgScope.ToString() + ")" : "");
                else if ((o[2] & 0x01) == 0) s += readStr(mgScope, GS.GlobalStr.MeshGroup, ToShort(o[3], o[4]), lng ? -1 : 30, lng ? Detail.Normal : Detail.ErrorNames);
                else s += GS.GlobalStr.MeshGroup.ToString() + ":[0x" + SimPe.Helper.HexString(ToShort(o[3], o[4])) + "]" + (lng ? " (" + mgScope.ToString() + ")" : "");
				s += ")";
			}
			else
                s += " (" + pjse.Localization.GetString("bwp6d_allOver") + ")";

            if (((o[13] & 0x02) == 0 && (o[2] & 0x08) != 0) || (o[2] & 0x01) != 0)
            {
                s += ", " + pjse.Localization.GetString("bwp_source") + ": "
                    + dataOwner(lng, o[8], o[9], o[10]);
            }

			return s;
		}

	}

	public class WizPrimi0x006e : BhavWizPrimi	// Look At
	{
		public WizPrimi0x006e(Instruction i) : base(i) { }

		protected override string Operands(bool lng)
		{
			byte[] o = new byte[16];
			((byte[])instruction.Operands).CopyTo(o, 0);
			((byte[])instruction.Reserved1).CopyTo(o, 8);

			string s = "";

            s += (lng ? pjse.Localization.GetString("Target") + ": " : "") + ((o[0] & 0x80) == 0
                ? dataOwner(lng, o[1], o[2], o[3])
                : pjse.Localization.GetString("bwp6e_camera")
                );

            s += ", " + Slot(o[14], o[8]);

			if ((o[0] & 0x01) == 0) 
			{
				if (lng)
				{
                    bool found = false;
                    s += ", " + pjse.Localization.GetString("bwp_eventTree") + ": " + bhavName(ToShort(o[9], o[10]), ref found);

                    Scope scope = Scope.Global;
                    if (o[11] == 0) scope = Scope.Private;
                    else if (o[11] == 1) scope = Scope.SemiGlobal;
                    s += " (" + pjse.Localization.GetString(scope.ToString()) + ")";

                    s += ", " + pjse.Localization.GetString("bwp6e_earlyExit") + ": " + ((o[0] & 0x02) == 0).ToString();
                    s += ", " + pjse.Localization.GetString("bwp6e_includeSpine") + ": " + ((o[0] & 0x04) != 0).ToString();
                    s += ", " + pjse.Localization.GetString("bwp6e_duration") + ": "
                        + ((o[0] & 0x10) != 0 ? dataOwner(0x08, 0) : "---"); // Temp 0
				}
			} 
			else
                s += ": " + pjse.Localization.GetString("bwp6e_STOP");

			if (lng)
			{
				if (instruction.NodeVersion != 0)
				{
                    s += ", " + pjse.Localization.GetString("bwp6e_turnTowards")
                        + ": " + ((o[0] & 0x08) != 0 ? dataOwner(0x08, 1) // Temp 1
                        : (2 * o[4]).ToString() + " " + pjse.Localization.GetString("bwp6e_deg_s"));
                    s += ", " + pjse.Localization.GetString("bwp6e_turnAway") + ": ";
                    if ((o[15] & 0x02) != 0) s += dataOwner(0x08, 1); // Temp 1
                    else if ((o[15] & 0x01) != 0) s += dataOwner(0x08, 2); // Temp 2
                    else s += (2 * o[5]).ToString() + " " + pjse.Localization.GetString("bwp6e_deg_s");

                    s += ", " + pjse.Localization.GetString("bwp_notHurryable")
                        + ": " + ((o[15] & 0x04) != 0).ToString();
				}
				else
                    s += ", " + pjse.Localization.GetString("bwp6e_speed")
                        + ": " + ((o[0] & 0x08) != 0 ? dataOwner(0x08, 1) // Temp 1
                        : o[4].ToString() + " " + pjse.Localization.GetString("bwp6e_deg_s"));

                s += ", " + pjse.Localization.GetString("bwp6e_ignoreRoom")
                    + ": " + ((o[0] & 0x20) != 0).ToString();
                s += ", " + pjse.Localization.GetString("bwp6e_ignoreFrustrum")
                    + ": " + ((o[0] & 0x40) != 0).ToString();
			}

			return s;
		}
	}

	public class WizPrimi0x006f : BhavWizPrimi	// Change Light
	{
		public WizPrimi0x006f(Instruction i) : base(i) { }

		protected override string Operands(bool lng)
		{
			byte[] o = new byte[16];
			((byte[])instruction.Operands).CopyTo(o, 0);
			((byte[])instruction.Reserved1).CopyTo(o, 8);

			string s = "";

			s += (lng ? pjse.Localization.GetString("Target") + ": " : "") + dataOwner(lng, o[2], o[3], o[4]);       // target object
            s += ", " + pjse.Localization.GetString("bwp6f_light") + ": " + (o[8] == 0xFF
                ? pjse.Localization.GetString("bwp6f_all")
                : readStr(GS.GlobalStr.LightSource, o[8], lng ? -1 : 60, Detail.ErrorNames)); // Fixed instance and scope
			if (lng)
			{
                s += ", " + pjse.Localization.GetString("bwp6f_ticks") + ": "
                    + ((o[1] & 0x01) != 0 ? dataOwner(0x08, 1) : "0x" + SimPe.Helper.HexString(ToShort(o[5], o[6])));
                s += ", " + pjse.Localization.GetString("bwp6f_intensity") + ": "
                    + ((o[1] & 0x02) != 0 ? dataOwner(0x08, 0) : o[7].ToString() + "%");
			}

			return s;
		}
	}

	public class WizPrimi0x0070 : BhavWizPrimi	// Effect Stop/Start
	{
		public WizPrimi0x0070(Instruction i) : base(i) { }

		protected override string Operands(bool lng)
		{
			byte[] o = new byte[16];
			((byte[])instruction.Operands).CopyTo(o, 0);
			((byte[])instruction.Reserved1).CopyTo(o, 8);

			string s = "";

            s += readStr(GS.BhavStr.EffectSSType, o[0]);

            s += ", " + (lng ? pjse.Localization.GetString("Target") + ": " : "") + dataOwner(lng, o[1], o[2], o[3]);       // target object

            if (lng && o[0] != 0x9 && o[0] != 0xE)
                s += ", " + Slot(o[9], o[6]);

			Scope scope = Scope.Private;
			if      ((o[10] & 0x01) != 0) scope = Scope.Global;
			else if ((o[10] & 0x02) != 0) scope = Scope.SemiGlobal;

            if (o[0] == 0x04 || o[0] == 0x05)
                s += ", " + pjse.Localization.GetString("bwp70_effectID") + ": "
                    + ((o[10] & 0x40) != 0 ? dataOwner(0x08, 1) : "---"); // Temp 1

            else if (o[0] < 0x04 || o[0] == 0x06 || o[0] == 0x0E)
            {
                if (o[4] != 0xFF)
                    s += ", " + readStr(scope, pjse.GS.GlobalStr.Effect, o[4], lng ? -1 : 60, lng ? Detail.Normal : Detail.ErrorNames);
                else
                    s += ", " + pjse.Localization.GetString("bwp70_defaultEffect");
            }

			if (lng)
			{
                s += ", " + pjse.Localization.GetString("bwp_icon") + ": ";
				if ((o[10] & 0x04) != 0)
					s += dataOwner(o[12], o[13], o[14]);
				else if ((o[10] & 0x10) != 0)
                    s += dataOwner(o[12], o[13], o[14]) + " (" + pjse.Localization.GetString("NeighborID") + ")";
				else if ((o[10] & 0x20) != 0)
                    s += dataOwner(o[12], o[13], o[14]) + " (" + pjse.Localization.GetString("bwp70_conversation") + ")"
                        + ", " + pjse.Localization.GetString("bwp70_sheet")
                            + ": " + readStr(scope, pjse.GS.GlobalStr.IconTexture, o[15], -1, lng ? Detail.Normal : Detail.ErrorNames);
				else if ((o[11] & 0x04) != 0)
                    s += "GUID [" + dataOwner(0x08, 4) + ",5]"; // Temp 4
				else if ((o[11] & 0x10) != 0)
                    s += dataOwner(0x08, 6); // Temp 6
				else
                    s += pjse.Localization.GetString("bwp70_noIcon");

                s += ", " + pjse.Localization.GetString("bwp_priority") + ": " + ((o[10] & 0x80) != 0).ToString();

                s += ", " + pjse.Localization.GetString("bwp70_model")
                    + ": " + ((o[11] & 0x08) != 0 ? dataOwner(0x08, 6) // Temp 6
                    : pjse.Localization.GetString("default"));
			}

			return s;
		}

	}

	public class WizPrimi0x0071 : BhavWizPrimi	// Snap Into -- for wizard, see edithWiki CreatingAChair
	{
		public WizPrimi0x0071(Instruction i) : base(i) { }

		protected override string Operands(bool lng)
		{
			byte[] o = new byte[16];
			((byte[])instruction.Operands).CopyTo(o, 0);
			((byte[])instruction.Reserved1).CopyTo(o, 8);

			string s = "";

            s += (lng ? pjse.Localization.GetString("Object") + ": " : "") + dataOwner(lng, o[0], o[1], o[2])
                + ", " + (lng ? pjse.Localization.GetString("Target") + ": " : "") + dataOwner(lng, o[3], o[4], o[5]);
            s += ", " + (lng ? pjse.Localization.GetString("bwp_slot") + ": " : "")
                + ((o[9] & 0x01) != 0 ? dataOwner(0x08, 0) : "0x" + SimPe.Helper.HexString(o[6]));
			if (lng)
			{
                s += ", " + pjse.Localization.GetString("bwp_testOnly") + ": " + ((o[9] & 0x02) != 0).ToString();
                s += ", " + pjse.Localization.GetString("bwp71_resetRootBones") + ": " + ((o[9] & 0x04) != 0).ToString();
			}

			return s;
		}
	}

	public class WizPrimi0x0072 : BhavWizPrimi	// Assign Locomotion Animations
	{
		public WizPrimi0x0072(Instruction i) : base(i) { }

		protected override string Operands(bool lng)
		{
			byte[] o = new byte[16];
			((byte[])instruction.Operands).CopyTo(o, 0);
			((byte[])instruction.Reserved1).CopyTo(o, 8);

			string s = "";

			switch (ToShort(o[0], o[1])) 
			{
                case 0: s += pjse.Localization.GetString("bwp72_popAll"); break;
                case 1: s += pjse.Localization.GetString("bwp72_pop"); break;
				default:
					Scope scope = Scope.Private;
					if      ((o[2] & 0x04) != 0) scope = Scope.Global;
					else if ((o[2] & 0x02) != 0) scope = Scope.SemiGlobal;
                    s += pjse.Localization.GetString("bwp72_push") + ": "
                        + readStr(scope, GS.GlobalStr.LocoAnims, (ushort)(ToShort(o[0], o[1]) - 2), lng ? -1 : 60, lng ? Detail.Normal : Detail.ErrorNames);
					break;
			}

			return s;
		}
	}

	public class WizPrimi0x0073 : BhavWizPrimi	// Debug
	{
		public WizPrimi0x0073(Instruction i) : base(i) { }

		protected override string Operands(bool lng)
		{
			byte[] o = new byte[16];
			((byte[])instruction.Operands).CopyTo(o, 0);
			((byte[])instruction.Reserved1).CopyTo(o, 8);

			string s = "";

			Scope scope = Scope.Private;
			if ((o[14] & 0x04) != 0) scope = Scope.Global;
			else if ((o[14] & 0x02) != 0) scope = Scope.SemiGlobal;

            if (o[13] == 0)
            {
                s += readStr(scope, GS.GlobalStr.DebugString, o[12], lng ? -1 : 60, lng ? Detail.Normal : Detail.ErrorNames);
                if (lng)
                {
                    s += " (";
                    for (int i = 0; i < 4; i++) s += (i == 0 ? "" : ", ") + dataOwner(o[i * 3], o[i * 3 + 1], o[i * 3 + 2]);
                    s += ")";
                }
            }
            else
            {
                s += readStr(GS.BhavStr.DebugType, o[13]);
                if (o[13] == 6)
                    s += ": " + readStr(scope, GS.GlobalStr.DebugString, o[12], lng ? -1 : 60, lng ? Detail.Normal : Detail.ErrorNames);
            }

			return s;
		}
	}

	public class WizPrimi0x0074 : BhavWizPrimi	// Reach/Put
	{
		public WizPrimi0x0074(Instruction i) : base(i) { }

		protected override string Operands(bool lng)
		{
			byte[] o = new byte[16];
			((byte[])instruction.Operands).CopyTo(o, 0);
			((byte[])instruction.Reserved1).CopyTo(o, 8);

			string s = "";

			switch (o[10]) 
			{
                case 0: s += pjse.Localization.GetString("bwp74_pickUp") + ": " + dataOwner(lng, o[3], ToShort(o[4], o[5])); break;
                case 1: s += pjse.Localization.GetString("bwp74_dropOnto") + ": "
                    + pjse.Localization.GetString("bwp74_floor"); break;
                default: s += pjse.Localization.GetString("bwp74_dropOnto") + ": " + dataOwner(lng, o[3], ToShort(o[4], o[5])); break;
			}

            s += ", " + pjse.Localization.GetString("bwp_slot") + ": " + ((o[9] & 0x01) != 0
                ? dataOwner(0x08, 0) // Temp 0
                : "0x" + SimPe.Helper.HexString(o[6]));

			if (lng)
			{
                s += ", " + pjse.Localization.GetString("bwp74_objectAnim") + ": " + (ToShort(o[13], o[14]) != 0xFFFF
                    ? readStr(GS.GlobalStr.ObjectAnims, ToShort(o[13], o[14]), -1, Detail.ErrorNames)
					: pjse.Localization.GetString("none")
					);

                s += ", " + pjse.Localization.GetString("bwp74_graspAnim") + ": " + (ToShort(o[11], o[12]) != 0xFFFF
                    ? readStr(GS.GlobalStr.AdultAnims, ToShort(o[11], o[12]), -1, Detail.ErrorNames)
					: pjse.Localization.GetString("none")
					);

                s += ", " + pjse.Localization.GetString("bwp74_handedness") + ": " + ((o[9] & 0x02) != 0 ? dataOwner(0x08, 3) : "---"); // Temp 3
                s += ", " + pjse.Localization.GetString("bwp74_agedAnim") + ": " + ((o[9] & 0x04) != 0).ToString();
			}

			return s;
		}
	}

	public class WizPrimi0x0075 : BhavWizPrimi	// Age
	{
		public WizPrimi0x0075(Instruction i) : base(i) { }

		protected override string Operands(bool lng)
		{
			byte[] o = new byte[16];
			((byte[])instruction.Operands).CopyTo(o, 0);
			((byte[])instruction.Reserved1).CopyTo(o, 8);

			string s = "";

			if ((o[1] & 0x01) != 0)
				s += dataOwner(0x08, 0);
			else
                s += readStr(pjse.GS.BhavStr.AgePrimAges, o[0]);

			return s;
		}
	}

	public class WizPrimi0x0076 : BhavWizPrimi	// Array Operation
	{
		public WizPrimi0x0076(Instruction i) : base(i) { }

        public override ABhavOperandWiz Wizard()
        {
            return new pjse.BhavOperandWizards.BhavOperandWiz0x0076(instruction);
        }

        protected override string Operands(bool lng)
		{
			byte[] o = new byte[16];
			((byte[])instruction.Operands).CopyTo(o, 0);
			((byte[])instruction.Reserved1).CopyTo(o, 8);

			string s = "";

            s += pjse.Localization.GetString(o[2] == 0 ? "bwp_myArray" : "bwp_stackObjectArray");
            // See discussion around whether this is a bit vs boolean:
            // http://simlogical.com/SMF/index.php?topic=917.msg6641#msg6641
            s = s.Replace("[array]", ArrayName(lng, ToShort(o[3], o[4])));
            s += ", ";

			switch (o[1]) 
			{
                case 0x00: s += pjse.Localization.GetString("bwp76_clearContents"); break;
                case 0x01: s += pjse.Localization.GetString("bwp76_getSize") + ": " + dataOwner(lng, o[5], o[6], o[7]); break;
                case 0x02: s += pjse.Localization.GetString("bwp76_setSize") + ": " + dataOwner(lng, o[5], o[6], o[7]); break;
                case 0x03: s += pjse.Localization.GetString("bwp76_setAll") + ": " + dataOwner(lng, o[5], o[6], o[7]); break;
                case 0x04: s += pjse.Localization.GetString("bwp76_unshift") + ": " + dataOwner(lng, o[5], o[6], o[7]); break;
                case 0x05: s += pjse.Localization.GetString("bwp76_push") + ": " + dataOwner(lng, o[5], o[6], o[7]); break;
                case 0x06: s += pjse.Localization.GetString("bwp76_insert") + ": " + dataOwner(lng, o[5], o[6], o[7])
                    + ", " + pjse.Localization.GetString("bwp76_at")
                    + ": " + dataOwner(lng, o[8], o[9], o[10]); break;
                case 0x07: s += pjse.Localization.GetString("bwp76_shift");
                    s += ", ?" + pjse.Localization.GetString("bwp76_into")
                        + ": " + dataOwner(lng, o[5], o[6], o[7]) + "?";
					break;
                case 0x08: s += pjse.Localization.GetString("bwp76_pop");
                    s += ", ?" + pjse.Localization.GetString("bwp76_into")
                        + ": " + dataOwner(lng, o[5], o[6], o[7]) + "?";
                    break;
                case 0x09: s += pjse.Localization.GetString("bwp76_remove") + ": " + dataOwner(lng, o[8], o[9], o[10]);
                    s += ", ?" + pjse.Localization.GetString("bwp76_into")
                        + ": " + dataOwner(lng, o[5], o[6], o[7]) + "?";
                    break;
                case 0x0a: s += pjse.Localization.GetString("bwp76_set") + ": " + dataOwner(lng, o[8], o[9], o[10])
                    + ", " + pjse.Localization.GetString("bwp76_toNext")
                    + ": " + dataOwner(lng, o[5], o[6], o[7]); break;
                case 0x0b: s += pjse.Localization.GetString("bwp76_swap") + ": " + dataOwner(lng, o[5], o[6], o[7])
                    + ", " + dataOwner(lng, o[8], o[9], o[10]); break;
                case 0x0c: s += pjse.Localization.GetString("bwp76_sortHiLo"); break;
                case 0x0d: s += pjse.Localization.GetString("bwp76_sortLoHi"); break;
				default: s += pjse.Localization.GetString("unk") + ": 0x" + SimPe.Helper.HexString(o[1]); break;
			}

			return s;
		}
	}

	public class WizPrimi0x0077 : BhavWizPrimi	// Message
	{
		public WizPrimi0x0077(Instruction i) : base(i) { }

		protected override string Operands(bool lng)
		{
			byte[] o = new byte[16];
			((byte[])instruction.Operands).CopyTo(o, 0);
			((byte[])instruction.Reserved1).CopyTo(o, 8);

			string s = "";

            s += (lng ? pjse.Localization.GetString("bwp_message") + ": " : "") + dataOwner(lng, o[15], o[1], o[2]);

			s += ", " + (lng ? pjse.Localization.GetString("Target") + ": " : "");
			if ((o[4] & 0x04) != 0)
				s += dataOwner(lng, o[5], o[6], o[7]);
			else
                switch (o[3])
                {
                    case 0: s += pjse.Localization.GetString("bwp77_selectableSims"); break;
                    case 1: s += pjse.Localization.GetString("bwp77_selectableSims")
                        + " + " + pjse.Localization.GetString("bwp77_neighbors"); break;
                    case 2: s += pjse.Localization.GetString("bwp77_selectableSims")
                        + " + " + pjse.Localization.GetString("bwp77_npcs"); break;
                    case 3: s += pjse.Localization.GetString("bwp77_neighbors"); break;
                    case 4: s += pjse.Localization.GetString("bwp77_npcs"); break;
                    case 5: s += pjse.Localization.GetString("bwp77_allSims"); break;
                    case 6: s += pjse.Localization.GetString("bwp77_objects"); break;
                    case 7: s += pjse.Localization.GetString("bwp77_everything"); break;
                    default: s += pjse.Localization.GetString("unk") + ": 0x" + SimPe.Helper.HexString(o[3]); break;
                }

            if (lng)
            {
                s += ", " + (lng ? pjse.Localization.GetString("bwp_Location") + ": " : "");
                switch (o[0])
                {
                    case 0: s += pjse.Localization.GetString("bwp77_room")
                        + ": " + ((o[4] & 0x01) == 0 ? pjse.Localization.GetString("bwp77_same") : dataOwner(o[5], o[6], o[7])); break;
                    case 1: s += pjse.Localization.GetString("bwp77_onSameLevel"); break;
                    case 2: s += pjse.Localization.GetString("bwp77_onLot"); break;
                    case 3: s += pjse.Localization.GetString("bwp77_insideBuilding"); break;
                    case 4: s += pjse.Localization.GetString("bwp77_outsideBuilding"); break;
                    default: s += pjse.Localization.GetString("unk") + ": 0x" + SimPe.Helper.HexString(o[0]); break;
                }

                s += ", " + pjse.Localization.GetString("bwp_priority") + ": 0x" + SimPe.Helper.HexString(o[8]);

                s += ", " + pjse.Localization.GetString("bwp77_userData")
                    + ": (" + dataOwner(o[9], o[10], o[11]) + ", " + dataOwner(o[12], o[13], o[14]) + ")";
            }

			return s;
		}
	}

	public class WizPrimi0x0078 : BhavWizPrimi	// RayTrace
	{
		public WizPrimi0x0078(Instruction i) : base(i) { }

		protected override string Operands(bool lng)
		{
			byte[] o = new byte[16];
			((byte[])instruction.Operands).CopyTo(o, 0);
			((byte[])instruction.Reserved1).CopyTo(o, 8);

			string s = "";

			s += (lng ? pjse.Localization.GetString("Object") + ": " : "")
                + dataOwner(lng, o[1], o[2], o[3]) + ", " + Slot(o[4], o[5]);
            s += ", " + (lng ? pjse.Localization.GetString("Target") + ": " : "")
                + dataOwner(lng, o[8], o[9], o[10]) + ", " + Slot(o[11], o[12]);

            if (lng)
            {
                s += ", " + pjse.Localization.GetString("bwp78_windowsIgnored") + ": " + ((o[15] & 0x01) != 0).ToString();
                s += ", " + pjse.Localization.GetString("bwp_resultIn") + ": " + dataOwner(0x08, 0); // Temp 0
            }

			return s;
		}
	}

	public class WizPrimi0x0079 : BhavWizPrimi	// Change Outfit
	{
        public WizPrimi0x0079(Instruction i) : base(i) { }

        public override ABhavOperandWiz Wizard()
        {
            return new pjse.BhavOperandWizards.BhavOperandWiz0x0079(instruction);
        }

		protected override string Operands(bool lng)
		{
			byte[] o = new byte[16];
			((byte[])instruction.Operands).CopyTo(o, 0);
			((byte[])instruction.Reserved1).CopyTo(o, 8);

			string s = "";

            if ((o[0] & 0x10) != 0) s += pjse.Localization.GetString("bwp79_rebuild") + ", ";
			//else s += "change outfit";

			s += (lng ? pjse.Localization.GetString("Target") + ": " : "") + dataOwner(lng, o[9], o[10], o[11]);

			if (lng)
			{
                s += ", " + pjse.Localization.GetString("bwp_source") + ": ";
                if ((o[0] & 0x01) != 0) s += dnStkOb();
                else if ((o[0] & 0x02) != 0) s += BhavWiz.FormatGUID(lng, o, 4);
                else if ((o[0] & 0x40) != 0) s += "GUID [" + dataOwner(0x08, 0) + ",1]";
                else s += pjse.Localization.GetString("bwp79_self");

				s += ", ";
                if ((o[0] & 4) == 0) s += pjse.Localization.GetString("bwp79_outfit") + ": " + readStr(GS.BhavStr.PersonOutfits, o[8]);
                else s += pjse.Localization.GetString("bwp79_outfitIndex") + ": " + dataOwner(o[1], o[2], o[3]);

                s += ", " + pjse.Localization.GetString("bwp79_personData") + ": " + ((o[0] & 0x20) != 0).ToString();
                s += ", " + pjse.Localization.GetString("bwp79_save") + ": " + ((o[0] & 0x08) != 0).ToString();
			}

			return s;
		}
	}

	public class WizPrimi0x007a : BhavWizPrimi	// Timer
	{
		public WizPrimi0x007a(Instruction i) : base(i) { }

		protected override string Operands(bool lng)
		{
			byte[] o = new byte[16];
			((byte[])instruction.Operands).CopyTo(o, 0);
			((byte[])instruction.Reserved1).CopyTo(o, 8);

			string s = "";

			switch (o[15]) 
			{
                case 0: s += pjse.Localization.GetString("bwp7a_start"); break;
                case 1: s += pjse.Localization.GetString("bwp7a_modify"); break;
                case 2: s += pjse.Localization.GetString("bwp7a_delete"); break;
                default: s += pjse.Localization.GetString("unk") + ": 0x" + SimPe.Helper.HexString(o[15]); break;
			}

            if (o[15] != 2)
            {
                s += ", " + pjse.Localization.GetString("bwp_ticks") + ": "
                    + ((o[5] & 0x08) != 0 ? dataOwner(0x08, 1) // Temp 1
                    : "0x" + SimPe.Helper.HexString(ToShort(o[0], o[1])));

                if (lng)
                {
                    bool found = false;
                    s += ", " + pjse.Localization.GetString("bwp_eventTree") + ": " + bhavName(ToShort(o[3], o[4]), ref found);

                    Scope scope = Scope.Global;
                    if (o[14] == 0) scope = Scope.Private;
                    else if (o[14] == 1) scope = Scope.SemiGlobal;
                    s += " (" + pjse.Localization.GetString(scope.ToString()) + ")";

                    s += ", " + pjse.Localization.GetString("manyArgs") + ": ";
                    if ((o[5] & 0x01) != 0)
                        s += pjse.Localization.GetString("bw_callerparams");
                    else
                        for (int i = 0; i < 3; i++)
                            s += (i == 0 ? "" : ", ") + dataOwner(o[3 * i + 6], o[3 * i + 7], o[3 * i + 8]);

                    s += ", " + pjse.Localization.GetString("bwp7a_looping") + ": " + ((o[5] & 0x02) != 0).ToString();

                    if (o[15] == 1)
                        s += ", " + pjse.Localization.GetString("bwp7a_reset") + ": " + ((o[5] & 0x04) != 0).ToString();
                }
            }

			return s;
		}
	}

	public class WizPrimi0x007b : BhavWizPrimi	// Cinematic
	{
		public WizPrimi0x007b(Instruction i) : base(i) { }

		protected override string Operands(bool lng)
		{
			byte[] o = new byte[16];
			((byte[])instruction.Operands).CopyTo(o, 0);
			((byte[])instruction.Reserved1).CopyTo(o, 8);

			string s = "";

			Scope scope = Scope.Private;
			if      ((o[5] & 0x20) != 0) scope = Scope.Global;
			else if ((o[5] & 0x40) != 0) scope = Scope.SemiGlobal;

            s += (lng ? pjse.Localization.GetString("bwp7b_scene") + ": " : "") + ((o[5] & 0x10) != 0
				? dataOwner(lng, o[6], o[7], o[8])
                : readStr(scope, GS.GlobalStr.CineCam, ToShort(o[0], o[1]), lng ? -1 : 60, lng ? Detail.Normal : Detail.ErrorNames)
				);

            if (lng)
            {
                s += ", " + pjse.Localization.GetString("bwp7b_array") + ": " + dataOwner(lng, o[9], o[10], o[11]);

                s += ", " + pjse.Localization.GetString("bwp_flipFlag") + ": " + (
                    (o[5] & 0x02) != 0 ? dataOwner(0x08, 0) // Temp 0
                    : ((o[5] & 0x01) != 0).ToString());
                s += ", " + pjse.Localization.GetString("bwp7b_start") + ": " + ((o[5] & 0x04) != 0).ToString();
                s += ", " + pjse.Localization.GetString("bwp7b_showHouse") + ": " + ((o[5] & 0x08) != 0).ToString();
            }

			return s;
		}
	}

	public class WizPrimi0x007c : BhavWizPrimi	// Want Satisfy -- for wizard, see edithWiki WantSatisfacton
	{
        public WizPrimi0x007c(Instruction i) : base(i) { }

        public override ABhavOperandWiz Wizard()
        {
            return new pjse.BhavOperandWizards.BhavOperandWiz0x007c(instruction);
        }

		protected override string Operands(bool lng)
		{
			byte[] o = new byte[16];
			((byte[])instruction.Operands).CopyTo(o, 0);
			((byte[])instruction.Reserved1).CopyTo(o, 8);

			string s = "";

            s += (lng ? pjse.Localization.GetString("Target") + ": " : "") + dataOwner(lng, o[7], o[8], o[9]);
            // Mmm, wants don't appear to use OBJDs, so GUID lookups don't work...
			uint want = (uint)(o[3] | o[4] << 8 | o[5] << 16 | o[6] << 24);
            s += ", " + pjse.Localization.GetString("bwp7c_want") + ": 0x" + SimPe.Helper.HexString(want);
            if (lng)
                s += ", " + pjse.Localization.GetString("bwp7c_level") + ": " + dataOwner(o[10], o[11], o[12]);

			return s;
		}
	}

	public class WizPrimi0x007d : BhavWizPrimi	// Influence
	{
		public WizPrimi0x007d(Instruction i) : base(i) { }

		protected override string Operands(bool lng)
		{
			byte[] o = new byte[16];
			((byte[])instruction.Operands).CopyTo(o, 0);
			((byte[])instruction.Reserved1).CopyTo(o, 8);

			string s = "";

			s += (lng ? pjse.Localization.GetString("Target") + ": " : "") + dataOwner(lng, o[0], o[1], o[2]);

            if (lng)
            {
                s += ", " + pjse.Localization.GetString("bwp_resultIn") + ": ";
                s += pjse.Localization.GetString(o[5] == 0 ? "bwp_myArray" : "bwp_stackObjectArray");
                s = s.Replace("[array]", ArrayName(lng, ToShort(o[3], o[4])));
            }

			return s;
		}
	}

	public class WizPrimi0x007e : BhavWizPrimi	// Lua (disaSim2 24b)
	{
		public WizPrimi0x007e(Instruction i) : base(i) { }

		protected override string Operands(bool lng)
		{
			byte[] o = new byte[16];
			((byte[])instruction.Operands).CopyTo(o, 0);
			((byte[])instruction.Reserved1).CopyTo(o, 8);

			ushort o4_5 = ToShort(o[4], o[5]);

			string s = "";

            if (lng)
                s += pjse.Localization.GetString("bwp7e_script") + ": ";

			if (ToShort(o[2], o[3]) != 0) 
			{
				Scope scope = Scope.Global;
				if      ((o4_5 & 0x02) != 0) scope = Scope.Private;
				else if ((o4_5 & 0x04) != 0) scope = Scope.SemiGlobal;

                s += readStr(scope, ToShort(o[0], o[1]), (ushort)(ToShort(o[2], o[3]) - 1), lng ? -1 : 60, lng ? Detail.Full : Detail.Errors, false);

				if ((o4_5 & 0x08) != 0)
				{
                    s += lng ? ", " + pjse.Localization.GetString("manyArgs") + ": " : ", ";
					for (int i = 0; i < 3; i++) s += (i != 0 ? ", " : "") + dataOwner(lng, o[6+3*i], o[7+3*i], o[8+3*i]);
				}

                if (lng)
                {
                    s += ", " + pjse.Localization.GetString("bwp7e_type") + ": " + ((o4_5 & 0x01) != 0
                        ? pjse.Localization.GetString("bwp7e_definition")
                        : pjse.Localization.GetString("bwp7e_dynamic"));
                    s += ", " + pjse.Localization.GetString("bwp7e_definitionIn") + ": " + (((o4_5 & 0x01) != 0)
                        ? pjse.Localization.GetString("bwp7e_objLuaFile")
                        : pjse.Localization.GetString("bwp7e_description"));
                }
            }
			else
				s += pjse.Localization.GetString("none");

			return s;
		}
	}

}
