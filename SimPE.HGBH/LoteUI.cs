using System;
using SimPe.Interfaces.Plugin;

namespace SimPe.Plugin
{
	/// <summary>
	/// This class is used to fill the UI for this FileType with Data
	/// </summary>
    public partial class LoteUI : SimPe.Windows.Forms.WrapperBaseControl, IPackedFileUI
    {
        protected new Lot Wrapper
        {
            get { return base.Wrapper as Lot; }
        }
        public Lot TPFW
        {
            get { return (Lot)Wrapper; }
        }
        
        #region WrapperBaseControl Member

        public LoteUI()
		{
            InitializeComponent();
            if (booby.ThemeManager.ThemedForms)
            {
                booby.ThemeManager tm = booby.ThemeManager.Global.CreateChild();
                tm.AddControl(this.rtLotDef);
                tm.AddControl(this.cbtype);
                this.lbnotes.ForeColor = booby.ThemeManager.Global.ThemeColourXdark;
                if (booby.ThemeManager.savedTheme == 4 || booby.ThemeManager.savedTheme == 7)
                    this.rtLotDef.Font = new System.Drawing.Font("Comic Sans MS", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            }
            if (booby.PrettyGirls.PervyMode)
            {
                if (!booby.Infos.IsFontinstalled("Blackadder ITC"))
                    this.label1.Font = new System.Drawing.Font("Comic Sans MS", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                this.label1.Visible = true;
                this.HeaderText = "Boobies";
            }

            this.cbtype.Items.Clear();
            this.cbtype.Items.Add(Ltxt.LotType.Unknown);
            this.cbtype.Items.Add(Ltxt.LotType.Residential);
            this.cbtype.Items.Add(Ltxt.LotType.Community);
            if (SimPe.PathProvider.Global.EPInstalled > 0)
            {
                this.cbtype.Items.Add(Ltxt.LotType.Dorm);
                this.cbtype.Items.Add(Ltxt.LotType.GreekHouse);
                this.cbtype.Items.Add(Ltxt.LotType.SecretSociety);
            }
            if (SimPe.PathProvider.Global.EPInstalled > 9)
            {
                this.cbtype.Items.Add(Ltxt.LotType.Hotel);
                this.cbtype.Items.Add(Ltxt.LotType.SecretHoliday);
            }
            if (SimPe.PathProvider.Global.EPInstalled > 11)
            {
                this.cbtype.Items.Add(Ltxt.LotType.Hobby);
            }
            if (SimPe.PathProvider.Global.EPInstalled > 15)
            {
                this.cbtype.Items.Add(Ltxt.LotType.ApartmentBase);
                this.cbtype.Items.Add(Ltxt.LotType.ApartmentSublot);
                this.cbtype.Items.Add(Ltxt.LotType.Witches);
            }
            if (booby.PrettyGirls.IsTitsInstalled() || booby.PrettyGirls.IsAngelsInstalled())
            {
                this.cbtype.Items.Add(Ltxt.LotType.Hospital);
                this.cbtype.Items.Add(Ltxt.LotType.Heaven);
                this.cbtype.Items.Add(Ltxt.LotType.Hell);
                this.cbtype.Items.Add(Ltxt.LotType.PublicSchool);
                this.cbtype.Items.Add(Ltxt.LotType.PrivateSchool);
                this.cbtype.Items.Add(Ltxt.LotType.Childcare);
                this.cbtype.Items.Add(Ltxt.LotType.BattleField);
                this.cbtype.Items.Add(Ltxt.LotType.SpaceHotel);
                this.cbtype.Items.Add(Ltxt.LotType.SpaceTour);
            }
		}

        public override void RefreshGUI()
        {
            base.RefreshGUI();
            reddy = false;
            if (Helper.StartedGui != Executable.Classic && booby.PrettyGirls.PervyMode)
            {
                if (Wrapper.Type == Ltxt.LotType.Residential || Wrapper.Type == Ltxt.LotType.ApartmentSublot || Wrapper.Type == Ltxt.LotType.Dorm)
                    this.BackgroundImage = booby.PrettyGirls.GoldenGirl;
                else if (Wrapper.Type == Ltxt.LotType.Hospital || Wrapper.Type == Ltxt.LotType.Hotel || Wrapper.Type == Ltxt.LotType.SpaceHotel)
                    this.BackgroundImage = booby.PrettyGirls.BikiniBabe;
                else if (Wrapper.Type == Ltxt.LotType.Heaven || Wrapper.Type == Ltxt.LotType.Hell || Wrapper.Type == Ltxt.LotType.BattleField)
                    this.BackgroundImage = booby.PrettyGirls.BadGirl;
                else if (Wrapper.Type == Ltxt.LotType.PublicSchool || Wrapper.Type == Ltxt.LotType.PrivateSchool || Wrapper.Type == Ltxt.LotType.Childcare)
                    this.BackgroundImage = booby.PrettyGirls.PurpleShades;
                else this.BackgroundImage = booby.PrettyGirls.Knockers;
            }

            if (this.cbtype.Items.Contains(Wrapper.Type))
                this.cbtype.SelectedIndex = this.cbtype.Items.IndexOf(Wrapper.Type);
            else
                this.cbtype.SelectedIndex = 0;

            string Descrpty;
            if (Wrapper.LotDesc.Length < 2) Descrpty = "  -None Included-";
            else Descrpty = Wrapper.LotDesc;

            string classy = "";
            string flagery = Convert.ToString(Wrapper.Unknown0);
            Boolset bby = Wrapper.Unknown0;
            if (bby[7]) flagery += " -(has a beach)";
            if (bby[4]) flagery += " -(hidden)";
            if (bby[12]) classy = "\r\n Low Class";
            else if (bby[13]) classy = "\r\n Medium Class";
            else if (bby[14]) classy = "\r\n High Class";

            string rotat = "to the Left";
            if (Wrapper.LotRotation == 1) rotat = "to the Top";
            if (Wrapper.LotRotation == 3) rotat = "to the Right";
            if (Wrapper.LotRotation == 4) rotat = "to the Bottom";

            byte rode = Wrapper.LotRoads;
            string roads = "";
            if (rode == 0) roads = " gone, it has no Road";
            else
            {
                if (rode >= 8) { rode -= 8; roads = " on the Bottom"; }
                if (rode >= 4) { rode -= 4; roads += " on the Right"; }
                if (rode >= 2) { rode -= 2; roads += " on the Top"; }
                if (rode >= 1) roads += " on the Left";
            }

            rtLotDef.Text = Wrapper.LotName + " is a " + Enum.GetName(typeof(Ltxt.LotType), Wrapper.Type) + " Lot\r\n\r\n"
            + "It is " + Convert.ToString(Wrapper.LotSize.Width) + " wide by " + Convert.ToString(Wrapper.LotSize.Height) + " deep.\r\n\r\n" +
            "Description:\r\n" + Descrpty + classy;
            rtLotDef.Text += "\r\n\r\n" + Wrapper.LotName + "'s flags are " + flagery;
            rtLotDef.Text += "\r\n" + Wrapper.LotName + "'s Rotation is " + rotat;
            rtLotDef.Text += "\r\n" + Wrapper.LotName + "'s Roads are" + roads;

            reddy = true;
        }

        internal bool reddy = false;

        public override void OnCommit()
        {
            base.OnCommit();
            TPFW.SynchronizeUserData(true, false);
        }
        #endregion

        #region IPackedFileUI Member
        System.Windows.Forms.Control IPackedFileUI.GUIHandle
        {
            get { return this; }
        }
        #endregion

        #region IDisposable Member

        void IDisposable.Dispose()
        {
            this.TPFW.Dispose();
        }
        #endregion

        private void cbtype_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (reddy == false) return;
            if (Enum.IsDefined(typeof(Ltxt.LotType), this.cbtype.SelectedItem))
                Wrapper.Type = (Ltxt.LotType)this.cbtype.SelectedItem;
            else
                Wrapper.Type = Ltxt.LotType.Unknown;
            RefreshGUI();
        }

    }
}
