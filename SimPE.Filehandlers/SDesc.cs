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
using System.Windows.Forms;
using System.Drawing;
using SimPe.Interfaces.Plugin;
using SimPe.Interfaces;
using SimPe.PackedFiles.Wrapper.Supporting;
using SimPe.Data;
using System.Threading;
using System.Reflection;
using System.Reflection.Emit;
using static SimPe.Data.LocalizedNeighbourhoodEP;
using SdscWrapper = SimPe.PackedFiles.Wrapper.SDesc;

namespace SimPe.PackedFiles.UserInterface
{
	/// <summary>
	/// handles Packed SDSC Files
	/// </summary>
	public class SDesc : UIBase, IPackedFileUI
	{
        /*static AssemblyBuilder myAssemblyBuilder;
		static ModuleBuilder myModuleBuilder;
		static EnumBuilder myEnumBuilder;*/

        // Minimal standalone SDSC panel (no longer using Elements)
        private SdscPanel sdscPanel = new SdscPanel();

        /// <summary>
        /// Creates a new Instance and fills the aspiration Types into the correct Form
        /// </summary>
        /// <summary>
        /// Minimal UI setup: only fill aspiration and life-section lists
        /// for the standalone SdscPanel.
        /// </summary>
        protected void ResetUI(Wrapper.SDesc sdesc)
        {
            // Aspiration list
            sdscPanel.cbaspiration.Items.Clear();
            sdscPanel.cbaspiration.Items.Add(new LocalizedAspirationTypes(Data.MetaData.AspirationTypes.Nothing));
            sdscPanel.cbaspiration.Items.Add(new LocalizedAspirationTypes(Data.MetaData.AspirationTypes.Fortune));
            sdscPanel.cbaspiration.Items.Add(new LocalizedAspirationTypes(Data.MetaData.AspirationTypes.Family));
            sdscPanel.cbaspiration.Items.Add(new LocalizedAspirationTypes(Data.MetaData.AspirationTypes.Knowledge));
            sdscPanel.cbaspiration.Items.Add(new LocalizedAspirationTypes(Data.MetaData.AspirationTypes.Reputation));
            sdscPanel.cbaspiration.Items.Add(new LocalizedAspirationTypes(Data.MetaData.AspirationTypes.Romance));
            sdscPanel.cbaspiration.Items.Add(new LocalizedAspirationTypes(Data.MetaData.AspirationTypes.Growup));

            // Life section list
            sdscPanel.cblifesection.Items.Clear();
            sdscPanel.cblifesection.Items.Add(new LocalizedLifeSections(Data.MetaData.LifeSections.Unknown));
            sdscPanel.cblifesection.Items.Add(new LocalizedLifeSections(Data.MetaData.LifeSections.Baby));
            sdscPanel.cblifesection.Items.Add(new LocalizedLifeSections(Data.MetaData.LifeSections.Toddler));
            sdscPanel.cblifesection.Items.Add(new LocalizedLifeSections(Data.MetaData.LifeSections.Child));
            sdscPanel.cblifesection.Items.Add(new LocalizedLifeSections(Data.MetaData.LifeSections.Teen));
            sdscPanel.cblifesection.Items.Add(new LocalizedLifeSections(Data.MetaData.LifeSections.Adult));
            sdscPanel.cblifesection.Items.Add(new LocalizedLifeSections(Data.MetaData.LifeSections.Elder));
        }

        #region IPackedFileHandler Member

        public Control GUIHandle
        {
            get
            {
                // For now we expose the whole SdscPanel as the UI for SDesc.
                return sdscPanel;
            }
        }

        public void UpdateGUI(SimPe.Interfaces.Plugin.IFileWrapper wrapper)
        {
            SdscWrapper sdesc = (SdscWrapper)wrapper;

            // Prepare dropdown lists (aspiration, life section)
            ResetUI(sdesc);

            // Basic identity
            sdscPanel.tbsim.Text = "0x" + Helper.HexString(sdesc.SimId);
            sdscPanel.tbsimdescname.Text = sdesc.SimName;
            sdscPanel.tbsimdescfamname.Text = sdesc.SimFamilyName;
            sdscPanel.tbfaminst.Text = "0x" + Helper.HexString(sdesc.FamilyInstance);

            // Aspiration: pick matching entry
            sdscPanel.cbaspiration.SelectedIndex = 0;
            for (int i = 0; i < sdscPanel.cbaspiration.Items.Count; i++)
            {
                // 1) Cast the item back to the localized wrapper
                LocalizedAspirationTypes lat =
                    (LocalizedAspirationTypes)sdscPanel.cbaspiration.Items[i];

                // 2) Let the implicit operator convert wrapper ? enum
                Data.MetaData.AspirationTypes at = lat;

                if (at == sdesc.CharacterDescription.Aspiration)
                {
                    sdscPanel.cbaspiration.SelectedIndex = i;
                    break;
                }
            }

            // Life section: pick matching entry
            sdscPanel.cblifesection.SelectedIndex = 0;
            for (int i = 0; i < sdscPanel.cblifesection.Items.Count; i++)
            {
                LocalizedLifeSections lls =
                    (LocalizedLifeSections)sdscPanel.cblifesection.Items[i];

                Data.MetaData.LifeSections ls = lls;

                if (ls == sdesc.CharacterDescription.LifeSection)
                {
                    sdscPanel.cblifesection.SelectedIndex = i;
                    break;
                }
            }

            // Species combo is present on SdscPanel but not yet populated here.
            // You can add species handling later once this minimal view works.
        }



        #endregion
    }
}
