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
using System.Collections.Specialized;
using System.ComponentModel;
using Avalonia.Controls;
using Avalonia.Layout;
using Avalonia.Media;
using Avalonia.Threading;

namespace SimPe.Wizards
{
	/// <summary>
	/// This implements a basic Wizard
	/// </summary>
	public class Wizard : Panel
	{
		int cur;

		public Wizard()
		{
			this.Background = Brushes.Transparent;
			img = null;
			this.Children.CollectionChanged += Wizard_ChildrenChanged;
		}

		internal bool Contains(WizardStepPanel iws)
		{
			return Children.Contains(iws);
		}

		#region IWizard Member

		[Browsable(false)]
		public Control WizardContainer
		{
			get { return this; }
		}

		IImage img;
		public virtual IImage Image
		{
			get { return img; }
			set { img = value; }
		}

		public int StepCount
		{
			get { return Children.Count; }
		}

		public int CurrentStepNumber
		{
			get { return cur; }
			set
			{
				if (value == cur) this.JumpToStep(value);
			}
		}

		[Browsable(false)]
		public WizardStepPanel CurrentStep
		{
			get { return (WizardStepPanel)Children[cur]; }
		}

		bool ne;
		[Browsable(false)]
		public bool NextEnabled
		{
			get { return ne; }
			set
			{
				if (value != ne)
				{
					ne = value;
					ChangedNextState?.Invoke(this);
				}
			}
		}

		bool pe;
		[Browsable(false)]
		public bool PrevEnabled
		{
			get { return pe; }
			set
			{
				if (value != pe)
				{
					pe = value;
					ChangedPrevState?.Invoke(this);
				}
			}
		}

		bool fe;
		[Browsable(false)]
		public bool FinishEnabled
		{
			get { return fe; }
			set
			{
				if (value != fe)
				{
					fe = value;
					ChangedFinishState?.Invoke(this);
				}
			}
		}

		public bool JumpToStep(int nr)
		{
			if (nr < 0) return false;
			if (nr >= Children.Count) return false;

			int lastnr = cur;
			if (nr >= cur)
			{
				for (int i = cur + 1; i <= nr; i++)
				{
					((WizardStepPanel)Children[i]).OnPrepare(this, nr);
					PrepareStep?.Invoke(this, (WizardStepPanel)Children[i], nr);
				}
			}
			else
			{
				for (int i = cur; i > nr; i--)
				{
					((WizardStepPanel)Children[i]).OnRollback(this, nr);
					RollbackStep?.Invoke(this, (WizardStepPanel)Children[i], nr);
				}
				((WizardStepPanel)Children[nr]).OnPrepare(this, nr);
				PrepareStep?.Invoke(this, (WizardStepPanel)Children[nr], nr);
			}

			WizardEventArgs e = new WizardEventArgs(
				(WizardStepPanel)Children[nr],
				!((WizardStepPanel)Children[nr]).Last,
				!((WizardStepPanel)Children[nr]).First,
				((WizardStepPanel)Children[nr]).Last);
			((WizardStepPanel)Children[nr]).OnShow(this, e);

			if (e.Cancel) return false;
			ShowStep?.Invoke(this, e);
			if (e.Cancel) return false;

			foreach (Control c in Children) c.IsVisible = false;
			this.CurrentStep.Client.IsVisible = false;
			this.cur = nr;
			this.CurrentStep.Client.IsVisible = true;
			this.NextEnabled = e.EnableNext;
			this.PrevEnabled = e.EnablePrev;
			this.FinishEnabled = e.CanFinish;

			((WizardStepPanel)Children[nr]).OnShowed(this);
			ShowedStep?.Invoke(this, lastnr);
			return true;
		}

		public void Start()
		{
			Loaded?.Invoke(this);
			this.cur = 0;
			this.JumpToStep(0);
		}

		public bool GoNext() => JumpToStep(CurrentStepNumber + 1);
		public bool GoPrev() => JumpToStep(CurrentStepNumber - 1);
		public void Finish() => Finished?.Invoke(this);
		public void Abort() => Aborted?.Invoke(this);

		public new event WizardHandle Loaded;
		public event WizardHandle Aborted;
		public event WizardHandle Finished;
		public event WizardStepChangeHandle PrepareStep;
		public event WizardStepChangeHandle RollbackStep;
		public event WizardChangeHandle ShowStep;
		public event WizardShowedHandle ShowedStep;
		public event WizardHandle ChangedNextState;
		public event WizardHandle ChangedPrevState;
		public event WizardHandle ChangedFinishState;

		#endregion

		internal string HintName
		{
			get { return Name; }
		}

		private void Wizard_ChildrenChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			if (e.Action == NotifyCollectionChangedAction.Add && e.NewItems != null)
			{
				foreach (var item in e.NewItems)
					Wizard_ControlAdded(item as Control);
			}
			else if (e.Action == NotifyCollectionChangedAction.Remove && e.OldItems != null)
			{
				foreach (var item in e.OldItems)
					Wizard_ControlRemoved(item as Control);
			}
		}

		private void Wizard_ControlAdded(Control control)
		{
			if (!(control is WizardStepPanel))
			{
				// Defer removal to avoid modifying Children during CollectionChanged
				Dispatcher.UIThread.Post(() => Children.Remove(control));
				return;
			}

			WizardStepPanel iws = null;
			for (int i = Children.Count - 1; i >= 0; i--)
			{
				Control c = (Control)Children[i];
				if (c is WizardStepPanel)
				{
					if (iws == null) iws = (WizardStepPanel)c;
					else
					{
						((WizardStepPanel)c).Last = false;
						((WizardStepPanel)c).IsVisible = false;
					}
				}
			}
			if (iws == null) return;
			iws.Client.IsVisible = false;

			iws.SetupParent(this);
			iws.HorizontalAlignment = HorizontalAlignment.Stretch;
			iws.VerticalAlignment = VerticalAlignment.Stretch;
			iws.Last = true;
			iws.First = (Children.Count == 0);
			iws.IsVisible = true;
		}

		private void Wizard_ControlRemoved(Control control)
		{
			if (!(control is WizardStepPanel)) return;
			WizardStepPanel iws = (WizardStepPanel)control;
			iws.RemoveParent(this);
		}
	}
}
