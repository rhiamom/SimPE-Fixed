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
using System.Media;

namespace SimPe.Plugin.Tool.Action
{
	/// <summary>
	/// The Intrigued Neighbourhood Action
	/// </summary>
	public class ActionIntriguedNeighborhood : SimPe.Interfaces.IToolAction
	{
		
		#region IToolAction Member

		public virtual bool ChangeEnabledStateEventHandler(object sender, SimPe.Events.ResourceEventArgs es)
		{
            if (es.Loaded && Helper.IsNeighborhoodFile(es.LoadedPackage.FileName)) return true;

			return false;
		}

        private bool RealChangeEnabledStateEventHandler(object sender, SimPe.Events.ResourceEventArgs es)
        {
            if (!es.Loaded) return false;

            return es.LoadedPackage.Package.FindFiles(Data.MetaData.SIM_DESCRIPTION_FILE).Length > 0;
        }

		public void ExecuteEventHandler(object sender, SimPe.Events.ResourceEventArgs e)
		{
            if (!RealChangeEnabledStateEventHandler(null, e))
            {
                System.Windows.Forms.MessageBox.Show(SimPe.Localization.GetString("This is not an appropriate context in which to use this tool"),
                    this.ToString());
                return;
            }
            SimPe.PackedFiles.Wrapper.SimDNA sdna; // Fuck

			SimPe.Interfaces.Files.IPackedFileDescriptor[] pfds = e.LoadedPackage.Package.FindFiles(Data.MetaData.SIM_DESCRIPTION_FILE);

            SimPe.PackedFiles.Wrapper.SDesc sdesc = new SimPe.PackedFiles.Wrapper.SDesc(null, null, null);
            Random slt = new Random();
            uint booty = 0;
            foreach (SimPe.Interfaces.Files.IPackedFileDescriptor pfd in pfds)
            {
                sdesc.ProcessData(pfd, e.LoadedPackage.Package);
                if ((booby.PrettyGirls.IsTitsInstalled() || booby.PrettyGirls.IsAngelsInstalled()) && sdesc.CharacterDescription.Bodyshape != SimPe.Data.MetaData.Bodyshape.Default) continue;
                if (sdesc.Nightlife.Species != 0 || ((int)sdesc.Version == (int)SimPe.PackedFiles.Wrapper.SDescVersions.Castaway && sdesc.Castaway.Subspecies > 0)) continue;
                SimPe.Interfaces.Files.IPackedFileDescriptor pfb = e.LoadedPackage.Package.FindFileAnyGroup(Data.MetaData.SDNA, 0, pfd.Instance);
                if (pfb != null)
                {
                    sdna = new SimPe.PackedFiles.Wrapper.SimDNA();
                    sdna.ProcessData(pfb, e.LoadedPackage.Package, true);
                    booty = SimPe.Data.MetaData.GetBodyShapeid(sdna.Dominant.Skintone);
                }
                else booty = 0;

                sdesc.Interests.Animals = (ushort)slt.Next(400, 600);
                sdesc.Interests.Crime = (ushort)slt.Next(400, 600);
                sdesc.Interests.Culture = (ushort)slt.Next(400, 600);
                sdesc.Interests.Entertainment = (ushort)slt.Next(600, 1000);
                sdesc.Interests.Environment = (ushort)slt.Next(400, 600);
                sdesc.Interests.Fashion = (ushort)slt.Next(700, 1000);
                sdesc.Interests.Food = (ushort)slt.Next(400, 600);
                sdesc.Interests.Health = (ushort)slt.Next(400, 600);
                sdesc.Interests.Money = (ushort)slt.Next(400, 600);
                sdesc.Interests.Paranormal = 10;
                sdesc.Interests.Politics = (ushort)slt.Next(400, 600);
                sdesc.Interests.School = (ushort)slt.Next(400, 600);
                sdesc.Interests.Scifi = (ushort)slt.Next(100);
                sdesc.Interests.Sports = (ushort)slt.Next(400, 600);
                sdesc.Interests.Toys = (ushort)slt.Next(400, 600);
                sdesc.Interests.Travel = (ushort)slt.Next(600, 1000);
                sdesc.Interests.Weather = (ushort)slt.Next(400, 600);
                sdesc.Interests.Work = (ushort)slt.Next(400, 600);
                if (booby.PrettyGirls.IsTitsInstalled() || booby.PrettyGirls.IsAngelsInstalled())
                {
                    sdesc.Skills.Body = 800;
                    sdesc.Skills.Charisma = 800;
                    sdesc.Skills.Cooking = 800;
                    sdesc.Skills.Cleaning = 800;
                    sdesc.Skills.Creativity = 800;
                    sdesc.Skills.Logic = 800;
                    sdesc.Skills.Mechanical = 800;
                    sdesc.Skills.Fatness = 0;
                    sdesc.Skills.Romance = 1000;
                    sdesc.Skills.Art = 800;
                    sdesc.Skills.Music = 800;
                    sdesc.Character.Neat = (ushort)slt.Next(400, 800);
                    sdesc.Character.Outgoing = (ushort)slt.Next(100, 1000);
                    sdesc.Character.Active = (ushort)slt.Next(400, 800);
                    sdesc.Character.Playful = (ushort)slt.Next(800, 1000);
                    sdesc.Character.Nice = (ushort)slt.Next(850, 1000);
                    sdesc.GeneticCharacter.Neat = sdesc.Character.Neat;
                    sdesc.GeneticCharacter.Outgoing = sdesc.Character.Outgoing;
                    sdesc.GeneticCharacter.Active = sdesc.Character.Active;
                    sdesc.GeneticCharacter.Playful = sdesc.Character.Playful;
                    sdesc.GeneticCharacter.Nice = sdesc.Character.Nice;
                    sdesc.CharacterDescription.BodyFlag.Fit = true;
                    sdesc.CharacterDescription.BodyFlag.Fat = false;
                    if ((int)sdesc.Version >= (int)SimPe.PackedFiles.Wrapper.SDescVersions.Freetime)
                        sdesc.Freetime.HobbyPredistined = SimPe.PackedFiles.Wrapper.Hobbies.Nature;
                    if (sdesc.CharacterDescription.Gender == Data.MetaData.Gender.Female)
                    {
                        sdesc.Interests.FemalePreference = 1000;
                        sdesc.Interests.MalePreference = 100;
                        if (booty != 0) // Real NPCs don't have dna so this should save them
                            sdesc.CharacterDescription.Bodyshape = (Data.MetaData.Bodyshape)booty;
                        else if (sdesc.CharacterDescription.Bodyshape == Data.MetaData.Bodyshape.Default)
                            sdesc.CharacterDescription.Bodyshape = Data.MetaData.Bodyshape.Maxis; // don't parse this one again
                    }
                    else
                    {
                        sdesc.Interests.FemalePreference = 1000;
                        sdesc.Interests.MalePreference = -400;
                        if (booty != 0)
                        {
                            if (booty != 0x13 && booty != 0x1e)
                                sdesc.CharacterDescription.Bodyshape = Data.MetaData.Bodyshape.LeanBB;
                            else
                                sdesc.CharacterDescription.Bodyshape = (Data.MetaData.Bodyshape)booty;
                        }
                        else if (sdesc.CharacterDescription.Bodyshape == Data.MetaData.Bodyshape.Default)
                            sdesc.CharacterDescription.Bodyshape = Data.MetaData.Bodyshape.Maxis;
                    }
                    if (booby.PrettyGirls.IsTitsInstalled())
                    {
                        sdesc.Nightlife.AttractionTurnOns1 = 30329;
                        sdesc.Nightlife.AttractionTurnOns3 = 3853;
                        sdesc.Nightlife.AttractionTurnOffs1 = 128;
                        sdesc.Nightlife.AttractionTurnOffs2 = 0;
                        sdesc.Nightlife.AttractionTurnOffs3 = 0;
                        if (sdesc.CharacterDescription.Gender == Data.MetaData.Gender.Female)
                            sdesc.Nightlife.AttractionTurnOns2 = 65503; // same as male except has well hung
                        else
                            sdesc.Nightlife.AttractionTurnOns2 = 32735;
                    }
                    else if (booby.PrettyGirls.IsAngelsInstalled())
                    {
                        sdesc.Nightlife.AttractionTurnOns1 = 30329;
                        sdesc.Nightlife.AttractionTurnOns2 = 32735;
                        sdesc.Nightlife.AttractionTurnOns3 = 3840;
                        sdesc.Nightlife.AttractionTurnOffs1 = 128;
                        sdesc.Nightlife.AttractionTurnOffs2 = 0;
                        sdesc.Nightlife.AttractionTurnOffs3 = 0;
                    }
                    if (sdesc.FamilyInstance > 32512) sdesc.CharacterDescription.PersonFlags1.WantHistory = true;
                }
                sdesc.SynchronizeUserData();
            }
            if (booby.PrettyGirls.PervyMode)
            {
                SoundPlayer alldid = new SoundPlayer(booby.NoisyGirls.ooh);
                alldid.Play();
            }
		}

		#endregion		

		
		#region IToolPlugin Member
		public override string ToString()
		{
            if (booby.PrettyGirls.PervyMode) return "Lustomize Neighbourhood";
            else return "Intrigued Neighbourhood";
		}
		#endregion

		#region IToolExt Member
		public System.Windows.Forms.Shortcut Shortcut
		{
			get
			{
				return System.Windows.Forms.Shortcut.None;
			}
		}

		public System.Drawing.Image Icon
		{
			get
			{
				return System.Drawing.Image.FromStream(this.GetType().Assembly.GetManifestResourceStream("SimPe.Plugin.emoticon.png"));
			}
		}

		public virtual bool Visible 
		{
			get {return true;}
		}

		#endregion
	}
}
