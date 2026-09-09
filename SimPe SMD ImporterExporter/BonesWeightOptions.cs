using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace SimPe_SmdImporterExporter;

public class BonesWeightOptions : Form
{
	public Button sksmdbutton;

	private Label sksmderr;

	private RadioButton sksmdRB4;

	private Button button2;

	private Button button1;

	private RadioButton sksmdRB1;

	private RadioButton sksmdRB0;

	private TextBox textBox1;

	private TextBox textBox2;

	private CheckBox cbmorph1;

	private CheckBox cbmorph2;

	private Panel panel1;

	private CheckBox cbbump;

	private OpenFileDialog sksmdOFDialog;

	private Label sksmdlabel1;

	private RadioButton sksmdRB3;

	private RadioButton sksmdRB2;

	private int mdial;

	private bool bdial;

	private int bwdial;

	private string MorphFileName1 = "";

	private string MorphFileName2 = "";

	private int bt = 0;

	private int originalfilesize;

	public int MorphType
	{
		get
		{
			if (cbmorph1.Checked && cbmorph2.Checked)
			{
				return 2;
			}
			if (cbmorph1.Checked)
			{
				return 1;
			}
			return 0;
		}
		set
		{
			mdial = value;
		}
	}

	public bool BumpType
	{
		get
		{
			if (cbbump.Checked)
			{
				return true;
			}
			return false;
		}
		set
		{
			bdial = value;
		}
	}

	public int BWType
	{
		get
		{
			if (sksmdRB1.Checked)
			{
				return 1;
			}
			if (sksmdRB2.Checked)
			{
				return 2;
			}
			if (sksmdRB3.Checked)
			{
				return 3;
			}
			if (sksmdRB4.Checked)
			{
				return 4;
			}
			return 0;
		}
		set
		{
			bwdial = value;
		}
	}

	public string MorphFile1
	{
		get
		{
			return textBox1.Text;
		}
		set
		{
			MorphFileName1 = value;
		}
	}

	public string MorphFile2
	{
		get
		{
			return textBox2.Text;
		}
		set
		{
			MorphFileName2 = value;
		}
	}

	public int BoneType
	{
		get
		{
			return bt;
		}
		set
		{
			bt = value;
		}
	}

	public BonesWeightOptions()
	{
		InitializeComponent();
		BackColor = Color.White;
		panel1.BackColor = Color.White;
		button1.BackColor = Color.White;
		button2.BackColor = Color.White;
		sksmdbutton.BackColor = Color.White;
	}

	private void InitializeComponent()
	{
		this.sksmdRB2 = new System.Windows.Forms.RadioButton();
		this.sksmdRB3 = new System.Windows.Forms.RadioButton();
		this.sksmdlabel1 = new System.Windows.Forms.Label();
		this.sksmdOFDialog = new System.Windows.Forms.OpenFileDialog();
		this.cbbump = new System.Windows.Forms.CheckBox();
		this.panel1 = new System.Windows.Forms.Panel();
		this.cbmorph2 = new System.Windows.Forms.CheckBox();
		this.cbmorph1 = new System.Windows.Forms.CheckBox();
		this.textBox2 = new System.Windows.Forms.TextBox();
		this.textBox1 = new System.Windows.Forms.TextBox();
		this.sksmdRB0 = new System.Windows.Forms.RadioButton();
		this.sksmdRB1 = new System.Windows.Forms.RadioButton();
		this.button1 = new System.Windows.Forms.Button();
		this.button2 = new System.Windows.Forms.Button();
		this.sksmdRB4 = new System.Windows.Forms.RadioButton();
		this.sksmderr = new System.Windows.Forms.Label();
		this.sksmdbutton = new System.Windows.Forms.Button();
		this.panel1.SuspendLayout();
		base.SuspendLayout();
		this.sksmdRB2.Location = new System.Drawing.Point(16, 97);
		this.sksmdRB2.Name = "sksmdRB2";
		this.sksmdRB2.Size = new System.Drawing.Size(136, 22);
		this.sksmdRB2.TabIndex = 2;
		this.sksmdRB2.Text = "2 bones per vertex";
		this.sksmdRB2.Click += new System.EventHandler(sksmdRB2CheckedChanged);
		this.sksmdRB3.Location = new System.Drawing.Point(16, 126);
		this.sksmdRB3.Name = "sksmdRB3";
		this.sksmdRB3.Size = new System.Drawing.Size(136, 23);
		this.sksmdRB3.TabIndex = 3;
		this.sksmdRB3.Text = "3 bones per vertex";
		this.sksmdRB3.Click += new System.EventHandler(sksmdRB3CheckedChanged);
		this.sksmdlabel1.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.World);
		this.sksmdlabel1.Location = new System.Drawing.Point(8, 7);
		this.sksmdlabel1.Name = "sksmdlabel1";
		this.sksmdlabel1.Size = new System.Drawing.Size(168, 22);
		this.sksmdlabel1.TabIndex = 0;
		this.sksmdlabel1.Text = "Choose Bones Weight Type";
		this.sksmdlabel1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.sksmdOFDialog.Filter = "smd files|*.smd";
		this.cbbump.Location = new System.Drawing.Point(216, 141);
		this.cbbump.Name = "cbbump";
		this.cbbump.Size = new System.Drawing.Size(168, 15);
		this.cbbump.TabIndex = 9;
		this.cbbump.Text = "Create BumpMapNormals";
		this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.panel1.Controls.Add(this.sksmdRB4);
		this.panel1.Controls.Add(this.sksmdlabel1);
		this.panel1.Controls.Add(this.sksmdRB0);
		this.panel1.Controls.Add(this.sksmdRB1);
		this.panel1.Controls.Add(this.sksmdRB2);
		this.panel1.Controls.Add(this.sksmdRB3);
		this.panel1.Controls.Add(this.cbmorph2);
		this.panel1.Controls.Add(this.cbmorph1);
		this.panel1.Controls.Add(this.cbbump);
		this.panel1.Controls.Add(this.textBox1);
		this.panel1.Controls.Add(this.textBox2);
		this.panel1.Controls.Add(this.button1);
		this.panel1.Controls.Add(this.button2);
		this.panel1.Controls.Add(this.sksmderr);
		this.panel1.Location = new System.Drawing.Point(8, 7);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(400, 208);
		this.panel1.TabIndex = 14;
		this.cbmorph2.Location = new System.Drawing.Point(216, 82);
		this.cbmorph2.Name = "cbmorph2";
		this.cbmorph2.Size = new System.Drawing.Size(144, 15);
		this.cbmorph2.TabIndex = 8;
		this.cbmorph2.Text = "Import Morph Mesh #2";
		this.cbmorph2.MouseHover += new System.EventHandler(Cbmorph2MouseHover);
		this.cbmorph2.MouseLeave += new System.EventHandler(Cbmorph1MouseLeave);
		this.cbmorph2.CheckedChanged += new System.EventHandler(Cbmorph2CheckedChanged);
		this.cbmorph1.Location = new System.Drawing.Point(216, 37);
		this.cbmorph1.Name = "cbmorph1";
		this.cbmorph1.Size = new System.Drawing.Size(144, 15);
		this.cbmorph1.TabIndex = 7;
		this.cbmorph1.Text = "Import Morph Mesh #1";
		this.cbmorph1.MouseHover += new System.EventHandler(Cbmorph1MouseHover);
		this.cbmorph1.MouseLeave += new System.EventHandler(Cbmorph1MouseLeave);
		this.cbmorph1.CheckedChanged += new System.EventHandler(Cbmorph1CheckedChanged);
		this.textBox2.Location = new System.Drawing.Point(224, 104);
		this.textBox2.Name = "textBox2";
		this.textBox2.Size = new System.Drawing.Size(128, 20);
		this.textBox2.TabIndex = 11;
		this.textBox2.Text = "";
		this.textBox1.Location = new System.Drawing.Point(224, 59);
		this.textBox1.Name = "textBox1";
		this.textBox1.Size = new System.Drawing.Size(128, 20);
		this.textBox1.TabIndex = 10;
		this.textBox1.Text = "";
		this.sksmdRB0.Location = new System.Drawing.Point(16, 37);
		this.sksmdRB0.Name = "sksmdRB0";
		this.sksmdRB0.Size = new System.Drawing.Size(136, 22);
		this.sksmdRB0.TabIndex = 6;
		this.sksmdRB0.Text = "No Bones";
		this.sksmdRB0.Click += new System.EventHandler(sksmdRB0CheckedChanged);
		this.sksmdRB1.Location = new System.Drawing.Point(16, 67);
		this.sksmdRB1.Name = "sksmdRB1";
		this.sksmdRB1.Size = new System.Drawing.Size(136, 22);
		this.sksmdRB1.TabIndex = 1;
		this.sksmdRB1.Text = "1 bone per vertex";
		this.sksmdRB1.Click += new System.EventHandler(sksmdRB1CheckedChanged);
		this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.button1.Location = new System.Drawing.Point(360, 59);
		this.button1.Name = "button1";
		this.button1.Size = new System.Drawing.Size(24, 20);
		this.button1.TabIndex = 12;
		this.button1.Text = "...";
		this.button1.Click += new System.EventHandler(Button1Click);
		this.button2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.button2.Location = new System.Drawing.Point(360, 104);
		this.button2.Name = "button2";
		this.button2.Size = new System.Drawing.Size(24, 19);
		this.button2.TabIndex = 13;
		this.button2.Text = "...";
		this.button2.Click += new System.EventHandler(Button2Click);
		this.sksmdRB4.Location = new System.Drawing.Point(16, 156);
		this.sksmdRB4.Name = "sksmdRB4";
		this.sksmdRB4.Size = new System.Drawing.Size(136, 22);
		this.sksmdRB4.TabIndex = 14;
		this.sksmdRB4.Text = "4 bones per vertex";
		this.sksmdRB4.Click += new System.EventHandler(sksmdRB4CheckedChanged);
		this.sksmdRB4.Enter += new System.EventHandler(sksmdRB4MouseEnter);
		this.sksmdRB4.MouseLeave += new System.EventHandler(Cbmorph1MouseLeave);
		this.sksmderr.Font = new System.Drawing.Font("Tahoma", 11f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.World);
		this.sksmderr.Location = new System.Drawing.Point(16, 186);
		this.sksmderr.Name = "sksmderr";
		this.sksmderr.Size = new System.Drawing.Size(368, 15);
		this.sksmderr.TabIndex = 5;
		this.sksmderr.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
		this.sksmdbutton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.sksmdbutton.Location = new System.Drawing.Point(144, 230);
		this.sksmdbutton.Name = "sksmdbutton";
		this.sksmdbutton.Size = new System.Drawing.Size(136, 23);
		this.sksmdbutton.TabIndex = 4;
		this.sksmdbutton.Text = "Continue";
		this.sksmdbutton.Click += new System.EventHandler(sksmdbuttonClick);
		this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
		base.ClientSize = new System.Drawing.Size(416, 293);
		base.Controls.Add(this.panel1);
		base.Controls.Add(this.sksmdbutton);
		base.MaximumSize = new System.Drawing.Size(424, 320);
		base.MinimumSize = new System.Drawing.Size(424, 320);
		base.Name = "BonesWeightOptions";
		this.Text = "Smd Import - Options";
		this.panel1.ResumeLayout(false);
		base.ResumeLayout(false);
	}

	private void sksmdRB0CheckedChanged(object sender, EventArgs e)
	{
		if (!sksmdRB0.Checked)
		{
			sksmdRB0.Checked = true;
			sksmdRB1.Checked = false;
			sksmdRB2.Checked = false;
			sksmdRB3.Checked = false;
			sksmdRB4.Checked = false;
			sksmderr.Text = "";
		}
	}

	private void sksmdRB1CheckedChanged(object sender, EventArgs e)
	{
		if (!sksmdRB1.Checked)
		{
			sksmdRB0.Checked = false;
			sksmdRB1.Checked = true;
			sksmdRB2.Checked = false;
			sksmdRB3.Checked = false;
			sksmdRB4.Checked = false;
			sksmderr.Text = "";
		}
	}

	private void sksmdRB2CheckedChanged(object sender, EventArgs e)
	{
		if (!sksmdRB2.Checked)
		{
			sksmdRB0.Checked = false;
			sksmdRB1.Checked = false;
			sksmdRB2.Checked = true;
			sksmdRB3.Checked = false;
			sksmdRB4.Checked = false;
			sksmderr.Text = "";
		}
	}

	private void sksmdRB3CheckedChanged(object sender, EventArgs e)
	{
		if (!sksmdRB3.Checked)
		{
			sksmdRB0.Checked = false;
			sksmdRB1.Checked = false;
			sksmdRB2.Checked = false;
			sksmdRB3.Checked = true;
			sksmdRB4.Checked = false;
			sksmderr.Text = "";
		}
	}

	private void sksmdRB4CheckedChanged(object sender, EventArgs e)
	{
		if (!sksmdRB4.Checked)
		{
			sksmdRB0.Checked = false;
			sksmdRB1.Checked = false;
			sksmdRB2.Checked = false;
			sksmdRB3.Checked = false;
			sksmdRB4.Checked = true;
			sksmderr.Text = "";
		}
	}

	private void sksmdRB4MouseEnter(object sender, EventArgs e)
	{
		sksmderr.Text = "Only Compatible with NightLife and OpenForBusiness";
	}

	private void Cbmorph1CheckedChanged(object sender, EventArgs e)
	{
		if (!cbmorph1.Checked)
		{
			cbmorph2.Checked = false;
		}
	}

	private void Cbmorph2CheckedChanged(object sender, EventArgs e)
	{
		if (cbmorph2.Checked)
		{
			cbmorph1.Checked = true;
			cbmorph2.Checked = true;
		}
	}

	public void Checkvalues(string path)
	{
		originalfilesize = CountVertices(path);
		sksmdOFDialog.InitialDirectory = Path.GetDirectoryName(path);
		textBox1.Text = MorphFileName1;
		textBox2.Text = MorphFileName2;
	}

	public void CheckBT()
	{
		if (bt == 0)
		{
			sksmdRB0.Checked = true;
		}
		else if (bt == 1)
		{
			sksmdRB1.Checked = true;
		}
		else if (bt == 2)
		{
			sksmdRB2.Checked = true;
		}
		else if (bt == 3)
		{
			sksmdRB3.Checked = true;
		}
	}

	public void sksmdbuttonClick(object sender, EventArgs e)
	{
		if (!sksmdRB0.Checked && !sksmdRB1.Checked && !sksmdRB2.Checked && !sksmdRB3.Checked && !sksmdRB4.Checked)
		{
			sksmderr.Text = "Bones weight type not specified !!";
			return;
		}
		if (MorphType == 2)
		{
			if (MorphFile1.IndexOf("smd") < 0 && MorphFile2.IndexOf("smd") < 0)
			{
				sksmderr.Text = "Both Morph Files not specified";
				return;
			}
			if (MorphFile2.IndexOf("smd") < 0)
			{
				sksmderr.Text = "Morph File 2 not specified";
				return;
			}
			if (MorphFile1.IndexOf("smd") < 0)
			{
				sksmderr.Text = "Morph File 1 not specified";
				return;
			}
		}
		if (MorphType == 1 && MorphFile1.IndexOf("smd") < 0)
		{
			sksmderr.Text = "Morph File 1 not specified";
			return;
		}
		if (MorphType == 2)
		{
			int num = CountVertices(MorphFile1);
			int num2 = CountVertices(MorphFile1);
			if (originalfilesize != num || originalfilesize != num2)
			{
				sksmderr.Text = "Vertices Number of Morph File(s) doesn't match !!";
				return;
			}
		}
		else if (MorphType == 1)
		{
			int num3 = CountVertices(MorphFile1);
			if (originalfilesize != num3)
			{
				sksmderr.Text = "Vertices Number of Morph File(s) doesn't match !!";
				return;
			}
		}
		Close();
	}

	private void Cbmorph1MouseHover(object sender, EventArgs e)
	{
		sksmderr.Text = "Used for Objects Morph or Body Fat Morph";
	}

	private void Cbmorph2MouseHover(object sender, EventArgs e)
	{
		sksmderr.Text = "Used for Body Pregnant Morph";
	}

	private void Cbmorph1MouseLeave(object sender, EventArgs e)
	{
		sksmderr.Text = "";
	}

	private void Button1Click(object sender, EventArgs e)
	{
		DialogResult dialogResult = sksmdOFDialog.ShowDialog();
		textBox1.Text = sksmdOFDialog.FileName;
	}

	private void Button2Click(object sender, EventArgs e)
	{
		DialogResult dialogResult = sksmdOFDialog.ShowDialog();
		textBox2.Text = sksmdOFDialog.FileName;
	}

	private int CountVertices(string path)
	{
		int num = 0;
		StreamReader streamReader = new StreamReader(File.OpenRead(path));
		while (streamReader.Peek() != -1)
		{
			string line = streamReader.ReadLine();
			line = CleanLine(line);
			if (!(line == "triangles"))
			{
				continue;
			}
			int num2 = 0;
			while (true)
			{
				line = streamReader.ReadLine();
				line = CleanLine(line);
				if (num2 % 4 != 0)
				{
					num++;
				}
				if (line == "end")
				{
					break;
				}
				num2++;
			}
		}
		streamReader.Close();
		return num;
	}

	private string CleanLine(string line)
	{
		while (line.Substring(0, 1) == " ")
		{
			line = line.Substring(1, line.Length - 1);
		}
		while (line.Substring(line.Length - 1, 1) == " ")
		{
			line = line.Substring(0, line.Length - 1);
		}
		while (line.IndexOf("  ") != -1)
		{
			line = line.Replace("  ", " ");
		}
		line = line.Replace("\"", "");
		return line;
	}
}
