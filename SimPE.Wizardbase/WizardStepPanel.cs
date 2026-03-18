/***************************************************************************
 *   Copyright (C) 2005 by Ambertation                                     *
 *   quaxi@ambertation.de                                                  *
 *                                                                         *
 *   Copyright (C) 2025 by GramzeSweatShop                                 *
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
using System.ComponentModel;
using Avalonia.Controls;
using Avalonia.Media;

namespace SimPe.Wizards
{
	/// <summary>
	/// Abstract Implementaion of a Wizard Step
	/// </summary>
	public class WizardStepPanel : Panel
	{
		public WizardStepPanel()
		{
			this.Background = Brushes.Transparent;
		}

		internal string HintName
		{
			get { return "Step "+index.ToString()+" ("+Name+")"; }
		}

		#region IWizardStep Member

		[Browsable(false)]
		public Control Client
		{
			get
			{
				return this;
			}
		}

		internal void SetupParent(Wizard parent)
		{
			this.parent = parent;
			index = 0;
			if (parent==null) return;
			index = parent.Children.Count-1;
			first = (index==0);

			parent.Aborted += new WizardHandle(OnAborted);
			parent.Finished += new WizardHandle(OnFinished);
			parent.Loaded += new WizardHandle(OnLoaded);
		}

		internal void RemoveParent(Wizard parent)
		{
			if (parent==null) return;
			parent.Aborted -= new WizardHandle(OnAborted);
			parent.Finished -= new WizardHandle(OnFinished);
			parent.Loaded -= new WizardHandle(OnLoaded);
		}

		Wizard parent;
		public Wizard ParentWizard
		{
			get
			{
				return parent;
			}
		}

		bool first;
		public bool First
		{
			get { return first; }
			set { first = value; }
		}

		bool last;
		public bool Last
		{
			get { return last; }
			set { last = value; }
		}

		int index;
		public int Index
		{
			get { return index; }
		}

		protected void OnLoaded(Wizard sender)
		{
			if (Loaded!=null) Loaded(sender, this);
		}

		protected void OnAborted(Wizard sender)
		{
			if (Aborted!=null) Aborted(sender, this);
		}

		protected void OnFinished(Wizard sender)
		{
			if (Finished!=null) Finished(sender, this);
		}

		internal void OnPrepare(Wizard sender, int target)
		{
			if (Prepare!=null) Prepare(sender, this, target);
		}

		internal void OnRollback(Wizard sender, int target)
		{
			if (Rollback!=null) Rollback(sender, this, target);
		}

		internal void OnShow(Wizard sender, WizardEventArgs e)
		{
			if (Activate!=null) Activate(sender, e);
		}

		internal void OnShowed(Wizard sender)
		{
            if (Activated != null)
            {
                foreach (Delegate d in Activated.GetInvocationList())
                {
                    try
                    {
                        d.DynamicInvoke(sender, this);
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine("WizardStepPanel.Activated handler failed: " +
                            d.Method.DeclaringType.FullName + "." + d.Method.Name);
                        System.Diagnostics.Debug.WriteLine(ex.ToString());
                        throw;
                    }
                }
            }
        }

		public new event WizardStepHandle Loaded;
		public event WizardStepHandle Aborted;
		public event WizardStepHandle Finished;

		public event WizardStepChangeHandle Prepare;
		public event WizardStepChangeHandle Rollback;
		public event WizardChangeHandle Activate;
		public event WizardStepHandle Activated;

		#endregion

	}
}
