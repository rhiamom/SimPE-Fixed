/**********************************************************************
 *   Copyright (C) 2005 by Ambertation                                *
 *   quaxi@ambertation.de                                             *
 *                                                                    *
 *   This has been almost wiped out                                   *
 *   it handles passing HexViewControl and  WrapperBaseControl to     *
 *   GDF.dll so GDF doesn't require any part of SimPe                 *
 **********************************************************************/
using System;
using System.Drawing;

namespace SimPe
{
	/// <summary>
	/// Classes used to manage the Theme of our GUI
	/// </summary>
	public class ThemeManager : System.IDisposable
	{
		#region Fields, Properties, Constructors

        booby.GuiTheme ctheme;
		System.Collections.ArrayList ctrls;

        public booby.GuiTheme CurrentTheme
		{
			get { return ctheme; }
			set { 
				if (ctheme!=value) 
				{
					ctheme = value; 
					SetTheme();
					if (ChangedTheme!=null) ChangedTheme(value);
				}
			}
		}

        public ThemeManager(booby.GuiTheme t)
		{
			ctheme = t;
			parent = null;
			ctrls = new System.Collections.ArrayList();
		}

		~ThemeManager()
		{
			try 
			{
				this.Dispose();
			} 
			catch {}
		}
	
		/// <summary>
		/// Creates a Child Theme Manager and returns it
		/// </summary>
		/// <returns></returns>
		public ThemeManager CreateChild()
		{
			ThemeManager tm = new ThemeManager(this.ctheme);
			tm.Parent = this;
			return tm;
		}
		#endregion

		#region Apply Themes

        void SetTheme(Ambertation.Windows.Forms.HexViewControl sdm)
        {
            

        void SetTheme(SimPe.Windows.Forms.WrapperBaseControl gp)
		{
           
		}

		public void Theme(object o) 
		{
            if (o is SimPe.Windows.Forms.WrapperBaseControl) SetTheme((SimPe.Windows.Forms.WrapperBaseControl)o);
            else if (o is Ambertation.Windows.Forms.HexViewControl) SetTheme((Ambertation.Windows.Forms.HexViewControl)o);
		}
		#endregion

		#region Manage
		public void AddControl(object o) 
		{
			if (!ctrls.Contains(o)) 
			{
				ctrls.Add(o);
				Theme(o);
			}
		}

		public void Clear()
		{
			ctrls.Clear();
		}

		public void RemoveControl(object o) 
		{
			ctrls.Remove(o);
		}

		public void SetTheme()
		{

			foreach (object o in ctrls) Theme(o);
		}
		#endregion

		#region Events
        protected event booby.Events.ChangedThemeEvent ChangedTheme;

		/// <summary>
		/// Called when the Theme in the parent was changed
		/// </summary>
		/// <param name="t"></param>
        void ThemeWasChanged(booby.GuiTheme t) 
		{
			this.CurrentTheme = t;
            SetTheme();
		}

		ThemeManager parent;
		/// <summary>
		/// Set the Parent Theme Manager
		/// </summary>
		public ThemeManager Parent 
		{
			get { return parent; }
			set 
			{
                if (parent != null) parent.ChangedTheme -= new booby.Events.ChangedThemeEvent(ThemeWasChanged);
				parent = value;
                if (parent != null) parent.ChangedTheme += new booby.Events.ChangedThemeEvent(ThemeWasChanged);
			}
		}		
		#endregion

		static ThemeManager tm;
		/// <summary>
		/// Returns the Main ThemeManager
		/// </summary>
		public static ThemeManager Global 
		{
			get 
			{
                if (tm == null) tm = new ThemeManager((booby.GuiTheme)Helper.WindowsRegistry.Layout.SelectedTheme);
				return tm;
			}
		}
		#region IDisposable Member

		public void Dispose()
		{
			this.Parent = null;
			this.Clear();
		}

		#endregion
	}
}