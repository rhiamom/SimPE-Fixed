using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Windows.Forms;

namespace CollectionCreator;

public class frmAbout : Form
{
	[AccessedThroughProperty("Label13")]
	private Label _Label13;

	[AccessedThroughProperty("Label12")]
	private Label _Label12;

	[AccessedThroughProperty("Label5")]
	private Label _Label5;

	[AccessedThroughProperty("Label6")]
	private Label _Label6;

	[AccessedThroughProperty("Label3")]
	private Label _Label3;

	[AccessedThroughProperty("Label7")]
	private Label _Label7;

	[AccessedThroughProperty("Button1")]
	private Button _Button1;

	[AccessedThroughProperty("Label8")]
	private Label _Label8;

	[AccessedThroughProperty("Label11")]
	private Label _Label11;

	[AccessedThroughProperty("Label9")]
	private Label _Label9;

	[AccessedThroughProperty("Label10")]
	private Label _Label10;

	[AccessedThroughProperty("Label14")]
	private Label _Label14;

	[AccessedThroughProperty("Label2")]
	private Label _Label2;

	[AccessedThroughProperty("Label4")]
	private Label _Label4;

	[AccessedThroughProperty("Label1")]
	private Label _Label1;

	private IContainer components;

	internal virtual Label Label10
	{
		get
		{
			return _Label10;
		}
		[MethodImpl(MethodImplOptions.Synchronized)]
		set
		{
			if (_Label10 != null)
			{
			}
			_Label10 = value;
			if (_Label10 == null)
			{
			}
		}
	}

	internal virtual Label Label9
	{
		get
		{
			return _Label9;
		}
		[MethodImpl(MethodImplOptions.Synchronized)]
		set
		{
			if (_Label9 != null)
			{
			}
			_Label9 = value;
			if (_Label9 == null)
			{
			}
		}
	}

	internal virtual Label Label8
	{
		get
		{
			return _Label8;
		}
		[MethodImpl(MethodImplOptions.Synchronized)]
		set
		{
			if (_Label8 != null)
			{
			}
			_Label8 = value;
			if (_Label8 == null)
			{
			}
		}
	}

	internal virtual Button Button1
	{
		get
		{
			return _Button1;
		}
		[MethodImpl(MethodImplOptions.Synchronized)]
		set
		{
			if (_Button1 != null)
			{
				((Control)_Button1).Click -= Button1_Click;
			}
			_Button1 = value;
			if (_Button1 != null)
			{
				((Control)_Button1).Click += Button1_Click;
			}
		}
	}

	internal virtual Label Label7
	{
		get
		{
			return _Label7;
		}
		[MethodImpl(MethodImplOptions.Synchronized)]
		set
		{
			if (_Label7 != null)
			{
			}
			_Label7 = value;
			if (_Label7 == null)
			{
			}
		}
	}

	internal virtual Label Label6
	{
		get
		{
			return _Label6;
		}
		[MethodImpl(MethodImplOptions.Synchronized)]
		set
		{
			if (_Label6 != null)
			{
			}
			_Label6 = value;
			if (_Label6 == null)
			{
			}
		}
	}

	internal virtual Label Label5
	{
		get
		{
			return _Label5;
		}
		[MethodImpl(MethodImplOptions.Synchronized)]
		set
		{
			if (_Label5 != null)
			{
			}
			_Label5 = value;
			if (_Label5 == null)
			{
			}
		}
	}

	internal virtual Label Label4
	{
		get
		{
			return _Label4;
		}
		[MethodImpl(MethodImplOptions.Synchronized)]
		set
		{
			if (_Label4 != null)
			{
			}
			_Label4 = value;
			if (_Label4 == null)
			{
			}
		}
	}

	internal virtual Label Label3
	{
		get
		{
			return _Label3;
		}
		[MethodImpl(MethodImplOptions.Synchronized)]
		set
		{
			if (_Label3 != null)
			{
			}
			_Label3 = value;
			if (_Label3 == null)
			{
			}
		}
	}

	internal virtual Label Label2
	{
		get
		{
			return _Label2;
		}
		[MethodImpl(MethodImplOptions.Synchronized)]
		set
		{
			if (_Label2 != null)
			{
			}
			_Label2 = value;
			if (_Label2 == null)
			{
			}
		}
	}

	internal virtual Label Label1
	{
		get
		{
			return _Label1;
		}
		[MethodImpl(MethodImplOptions.Synchronized)]
		set
		{
			if (_Label1 != null)
			{
			}
			_Label1 = value;
			if (_Label1 == null)
			{
			}
		}
	}

	internal virtual Label Label11
	{
		get
		{
			return _Label11;
		}
		[MethodImpl(MethodImplOptions.Synchronized)]
		set
		{
			if (_Label11 != null)
			{
			}
			_Label11 = value;
			if (_Label11 == null)
			{
			}
		}
	}

	internal virtual Label Label12
	{
		get
		{
			return _Label12;
		}
		[MethodImpl(MethodImplOptions.Synchronized)]
		set
		{
			if (_Label12 != null)
			{
			}
			_Label12 = value;
			if (_Label12 == null)
			{
			}
		}
	}

	internal virtual Label Label13
	{
		get
		{
			return _Label13;
		}
		[MethodImpl(MethodImplOptions.Synchronized)]
		set
		{
			if (_Label13 != null)
			{
			}
			_Label13 = value;
			if (_Label13 == null)
			{
			}
		}
	}

	internal virtual Label Label14
	{
		get
		{
			return _Label14;
		}
		[MethodImpl(MethodImplOptions.Synchronized)]
		set
		{
			if (_Label14 != null)
			{
			}
			_Label14 = value;
			if (_Label14 == null)
			{
			}
		}
	}

	public frmAbout()
	{
		InitializeComponent();
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing && components != null)
		{
			components.Dispose();
		}
		((Form)this).Dispose(disposing);
	}

	[DebuggerStepThrough]
	private void InitializeComponent()
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Expected O, but got Unknown
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Expected O, but got Unknown
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Expected O, but got Unknown
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Expected O, but got Unknown
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Expected O, but got Unknown
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Expected O, but got Unknown
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_0063: Expected O, but got Unknown
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		//IL_006e: Expected O, but got Unknown
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0079: Expected O, but got Unknown
		//IL_007a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0084: Expected O, but got Unknown
		//IL_0085: Unknown result type (might be due to invalid IL or missing references)
		//IL_008f: Expected O, but got Unknown
		//IL_0090: Unknown result type (might be due to invalid IL or missing references)
		//IL_009a: Expected O, but got Unknown
		//IL_009b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a5: Expected O, but got Unknown
		Label10 = new Label();
		Label9 = new Label();
		Label8 = new Label();
		Button1 = new Button();
		Label7 = new Label();
		Label6 = new Label();
		Label5 = new Label();
		Label4 = new Label();
		Label3 = new Label();
		Label2 = new Label();
		Label1 = new Label();
		Label11 = new Label();
		Label12 = new Label();
		Label13 = new Label();
		Label14 = new Label();
		((Control)this).SuspendLayout();
		Label label = Label10;
		Point location = new Point(8, 64);
		((Control)label).Location = location;
		((Control)Label10).Name = "Label10";
		Label label2 = Label10;
		Size size = new Size(432, 32);
		((Control)label2).Size = size;
		((Control)Label10).TabIndex = 21;
		((Control)Label10).Text = "Credit for the Sample Icons included goes to JadeElliot of Neighborhood 99, and Kimberly at Sunni Sims.";
		Label label3 = Label9;
		location = new Point(148, 544);
		((Control)label3).Location = location;
		((Control)Label9).Name = "Label9";
		Label label4 = Label9;
		size = new Size(168, 16);
		((Control)label4).Size = size;
		((Control)Label9).TabIndex = 20;
		((Control)Label9).Text = "Version 2.01: Electric Boogaloo!";
		((Control)Label8).Anchor = (AnchorStyles)12;
		Label label5 = Label8;
		location = new Point(72, 520);
		((Control)label5).Location = location;
		((Control)Label8).Name = "Label8";
		Label label6 = Label8;
		size = new Size(304, 16);
		((Control)label6).Size = size;
		((Control)Label8).TabIndex = 19;
		((Control)Label8).Text = "© 2006-2007 DJS Sims and The Sims Programming Group";
		Label8.TextAlign = (ContentAlignment)32;
		Button button = Button1;
		location = new Point(144, 560);
		((Control)button).Location = location;
		((Control)Button1).Name = "Button1";
		Button button2 = Button1;
		size = new Size(160, 24);
		((Control)button2).Size = size;
		((Control)Button1).TabIndex = 18;
		((Control)Button1).Text = "Close This Sappy Stuff!";
		Label label7 = Label7;
		location = new Point(8, 472);
		((Control)label7).Location = location;
		((Control)Label7).Name = "Label7";
		Label label8 = Label7;
		size = new Size(432, 40);
		((Control)label8).Size = size;
		((Control)Label7).TabIndex = 17;
		((Control)Label7).Text = "And finally, thanks to all of you who have said \"Thanks\" and given me encouragement all this time. I've been in this community for more than 4 years now and I would've left long ago had it not been for some of you. :-)";
		Label label9 = Label6;
		location = new Point(8, 432);
		((Control)label9).Location = location;
		((Control)Label6).Name = "Label6";
		Label label10 = Label6;
		size = new Size(432, 32);
		((Control)label10).Size = size;
		((Control)Label6).TabIndex = 16;
		((Control)Label6).Text = "A Very Special Thanks to my two friends Abram and Nate who got me interested into programming in the first place.";
		Label label11 = Label5;
		location = new Point(8, 208);
		((Control)label11).Location = location;
		((Control)Label5).Name = "Label5";
		Label label12 = Label5;
		size = new Size(432, 40);
		((Control)label12).Size = size;
		((Control)Label5).TabIndex = 15;
		((Control)Label5).Text = "Special Thanks to DarkMatter, kichigai, Breon, Karydbis, and DataFarmer--the DatGen team who decoded and provided much valuable information about Sims 2 file formats at the MTS2 wiki.";
		Label label13 = Label4;
		location = new Point(8, 168);
		((Control)label13).Location = location;
		((Control)Label4).Name = "Label4";
		Label label14 = Label4;
		size = new Size(432, 32);
		((Control)label14).Size = size;
		((Control)Label4).TabIndex = 14;
		((Control)Label4).Text = "Thanks to the Neighborhood 99 Forums, whose users gave me some vital input and feedback early on with version 1.0.";
		Label label15 = Label3;
		location = new Point(8, 104);
		((Control)label15).Location = location;
		((Control)Label3).Name = "Label3";
		Label label16 = Label3;
		size = new Size(432, 56);
		((Control)label16).Size = size;
		((Control)Label3).TabIndex = 13;
		((Control)Label3).Text = "Thanks to the Early Beta Testers for Version 1.0: BlueSoup, Jysudo, JadeElliot, Waverly, MissMokie, sww, Thomas Riordan, BettyNewbie2, Kiwiana Girl, DMDye, and spottsmom of TSR, Green Sims, Neighborhood 99, and More Awesome Than You";
		Label label17 = Label2;
		location = new Point(8, 40);
		((Control)label17).Location = location;
		((Control)Label2).Name = "Label2";
		Label label18 = Label2;
		size = new Size(432, 16);
		((Control)label18).Size = size;
		((Control)Label2).TabIndex = 12;
		((Control)Label2).Text = "Credit For the Interface Buttons and Design by: sww of TSR";
		Label label19 = Label1;
		location = new Point(8, 0);
		((Control)label19).Location = location;
		((Control)Label1).Name = "Label1";
		Label label20 = Label1;
		size = new Size(432, 32);
		((Control)label20).Size = size;
		((Control)Label1).TabIndex = 11;
		((Control)Label1).Text = "I'd like to give credit to the following people to thank them for making this program possible by helping with the interface design etc.";
		Label label21 = Label11;
		location = new Point(8, 296);
		((Control)label21).Location = location;
		((Control)Label11).Name = "Label11";
		Label label22 = Label11;
		size = new Size(432, 32);
		((Control)label22).Size = size;
		((Control)Label11).TabIndex = 22;
		((Control)Label11).Text = "Thanks to the SimPE team (namely Quaxi) for being open source. Because of that, I was able to get code for the CRC24 hash used the pull the thumbnails.";
		Label label23 = Label12;
		location = new Point(8, 336);
		((Control)label23).Location = location;
		((Control)Label12).Name = "Label12";
		Label label24 = Label12;
		size = new Size(432, 40);
		((Control)label24).Size = size;
		((Control)Label12).TabIndex = 23;
		((Control)Label12).Text = "Thanks too goes to coders of the \"Hasher\" tool which is included in this program for the creation of CRC24 hashes for thumbnails. The Hasher tool is available free here: http://hasher.classless.net/ under the MPL license.";
		Label label25 = Label13;
		location = new Point(8, 384);
		((Control)label25).Location = location;
		((Control)Label13).Name = "Label13";
		Label label26 = Label13;
		size = new Size(432, 40);
		((Control)label26).Size = size;
		((Control)Label13).TabIndex = 24;
		((Control)Label13).Text = "Thanks to the Version 2.0 Testers: Hecubus, Benny Boy, Waverly, Gwendolyne, macgirlffx, Tree_Hugger:), evilredduckie, and others who I'm probably forgetting, (and for which I apologize.)";
		Label label27 = Label14;
		location = new Point(8, 256);
		((Control)label27).Location = location;
		((Control)Label14).Name = "Label14";
		Label label28 = Label14;
		size = new Size(432, 32);
		((Control)label28).Size = size;
		((Control)Label14).TabIndex = 25;
		((Control)Label14).Text = "Big thanks to Pinhead at MTS2 who posted information about how object thumbnails were linked to custom content, without that info, there'd be no previews.";
		size = new Size(5, 13);
		((Form)this).AutoScaleBaseSize = size;
		size = new Size(448, 592);
		((Form)this).ClientSize = size;
		((Control)this).Controls.Add((Control)(object)Label14);
		((Control)this).Controls.Add((Control)(object)Label13);
		((Control)this).Controls.Add((Control)(object)Label12);
		((Control)this).Controls.Add((Control)(object)Label11);
		((Control)this).Controls.Add((Control)(object)Label10);
		((Control)this).Controls.Add((Control)(object)Label9);
		((Control)this).Controls.Add((Control)(object)Label8);
		((Control)this).Controls.Add((Control)(object)Button1);
		((Control)this).Controls.Add((Control)(object)Label7);
		((Control)this).Controls.Add((Control)(object)Label6);
		((Control)this).Controls.Add((Control)(object)Label5);
		((Control)this).Controls.Add((Control)(object)Label4);
		((Control)this).Controls.Add((Control)(object)Label3);
		((Control)this).Controls.Add((Control)(object)Label2);
		((Control)this).Controls.Add((Control)(object)Label1);
		((Form)this).FormBorderStyle = (FormBorderStyle)5;
		((Control)this).Name = "frmAbout";
		((Form)this).StartPosition = (FormStartPosition)1;
		((Control)this).Text = "The \"About\" Page";
		((Control)this).ResumeLayout(false);
	}

	private void Button1_Click(object sender, EventArgs e)
	{
		frmAbout frmAbout2 = new frmAbout();
		((Control)frmAbout2).Visible = false;
		((Form)this).Close();
	}
}
