using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using Ambertation.Drawing;
using Ambertation.Scenes;
using Ambertation.Scenes.Collections;

namespace Ambertation.Graphics;

public class RenderSelection : UserControl
{
	private IContainer components;

	private ListBox lb;

	private Scene scn;

	private SceneToMesh stm;

	private DirectXPanel dx;

	public DirectXPanel DirectXPanel
	{
		get
		{
			return dx;
		}
		set
		{
			if (dx != null)
			{
				dx.ResetDevice -= dx_ResetDevice;
			}
			dx = value;
			if (dx != null)
			{
				dx.ResetDevice += dx_ResetDevice;
			}
			SetContent();
		}
	}

	public Scene Scene
	{
		get
		{
			return scn;
		}
		set
		{
			scn = value;
			SetContent();
		}
	}

	public RenderSelection()
	{
		InitializeComponent();
		lb.DrawMode = DrawMode.OwnerDrawVariable;
		lb.DrawItem += DrawItemHandler;
		lb.MeasureItem += MeasureItemHandler;
	}

	protected override void Dispose(bool disposing)
	{
		dx = null;
		scn = null;
		if (stm != null)
		{
			stm.Dispose();
		}
		stm = null;
		if (disposing && components != null)
		{
			components.Dispose();
		}
		base.Dispose(disposing);
	}

	private void DrawItemHandler(object sender, DrawItemEventArgs e)
	{
		e.DrawBackground();
		if (e.Index < lb.Items.Count && e.Index >= 0)
		{
			object obj = lb.Items[e.Index];
			Color foreColor = lb.ForeColor;
			if (!(obj is Joint))
			{
				System.Drawing.Graphics graphics = e.Graphics;
				string s = obj.ToString();
				Font font = lb.Font;
				SolidBrush brush = new SolidBrush(foreColor);
				Rectangle bounds = e.Bounds;
				Rectangle bounds2 = e.Bounds;
				Rectangle bounds3 = e.Bounds;
				Rectangle bounds4 = e.Bounds;
				graphics.DrawString(s, font, brush, new Rectangle(bounds.Left + 3, bounds2.Top + 4, bounds3.Width - 6, bounds4.Height - 4));
				return;
			}
			e.Graphics.CompositingQuality = CompositingQuality.HighQuality;
			e.Graphics.SmoothingMode = SmoothingMode.HighQuality;
			e.Graphics.InterpolationMode = InterpolationMode.High;
			foreColor = stm.GetJointColor((Joint)((obj is Joint) ? obj : null));
			System.Drawing.Graphics graphics2 = e.Graphics;
			SolidBrush solidBrush = new SolidBrush(foreColor);
			Rectangle bounds5 = e.Bounds;
			Rectangle bounds6 = e.Bounds;
			Rectangle bounds7 = e.Bounds;
			Rectangle bounds8 = e.Bounds;
			Rectangle rectangle = new Rectangle(bounds5.Left + 3, bounds6.Top + 2, bounds7.Width - 6, bounds8.Height - 5);
			Routines.FillRoundRect(graphics2, (Brush)solidBrush, rectangle, e.Bounds.Height / 3);
			System.Drawing.Graphics graphics3 = e.Graphics;
			SolidBrush brush2 = new SolidBrush(foreColor);
			Rectangle bounds9 = e.Bounds;
			int top = e.Bounds.Top;
			Rectangle bounds10 = e.Bounds;
			Rectangle bounds11 = e.Bounds;
			graphics3.FillEllipse(brush2, new Rectangle(bounds9.Left + 1, top, bounds10.Height + 4, bounds11.Height - 1));
			System.Drawing.Graphics graphics4 = e.Graphics;
			string s2 = obj.ToString();
			Font font2 = new Font(lb.Font.FontFamily, lb.Font.Size, FontStyle.Bold, lb.Font.Unit);
			SolidBrush brush3 = new SolidBrush(Color.Black);
			int left = e.Bounds.Left;
			Rectangle bounds12 = e.Bounds;
			Rectangle bounds13 = e.Bounds;
			int num = e.Bounds.Width;
			Rectangle bounds14 = e.Bounds;
			Rectangle bounds15 = e.Bounds;
			graphics4.DrawString(s2, font2, brush3, new Rectangle(left + bounds12.Height + 4, bounds13.Top + 3, num - bounds14.Height - 5, bounds15.Height - 4));
		}
	}

	private void dx_ResetDevice(object sender, EventArgs e)
	{
		//IL_0080: Unknown result type (might be due to invalid IL or missing references)
		//IL_0086: Expected O, but got Unknown
		if (!(sender is DirectXPanel directXPanel))
		{
			return;
		}
		try
		{
			directXPanel.Meshes.Clear(dispose: true);
			if (lb.SelectedItem == null)
			{
				directXPanel.Meshes.AddRange(stm.ConvertToDx());
			}
			else if (!(lb.SelectedItem is Joint))
			{
				directXPanel.Meshes.AddRange(stm.ConvertToDx());
			}
			else if (lb.SelectedItems.Count != 1)
			{
				JointCollection val = new JointCollection();
				foreach (object selectedItem2 in lb.SelectedItems)
				{
					if (selectedItem2 is Joint)
					{
						val.Add((Joint)((selectedItem2 is Joint) ? selectedItem2 : null));
					}
				}
				directXPanel.Meshes.AddRange(stm.ConvertToDx((JointCollectionBase)(object)val));
			}
			else
			{
				MeshList meshes = directXPanel.Meshes;
				SceneToMesh sceneToMesh = stm;
				object selectedItem = lb.SelectedItem;
				meshes.AddRange(sceneToMesh.ConvertToDx((Joint)((selectedItem is Joint) ? selectedItem : null)));
			}
			_ = stm;
		}
		catch
		{
		}
	}

	private void InitializeComponent()
	{
		this.lb = new System.Windows.Forms.ListBox();
		base.SuspendLayout();
		this.lb.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lb.IntegralHeight = false;
		this.lb.Location = new System.Drawing.Point(0, 0);
		this.lb.Name = "lb";
		this.lb.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
		this.lb.Size = new System.Drawing.Size(172, 329);
		this.lb.TabIndex = 0;
		this.lb.SelectedIndexChanged += new System.EventHandler(lb_SelectedIndexChanged);
		base.Controls.Add(this.lb);
		base.Name = "RenderSelection";
		base.Size = new System.Drawing.Size(172, 329);
		base.ResumeLayout(false);
	}

	private void lb_SelectedIndexChanged(object sender, EventArgs e)
	{
		if (dx != null)
		{
			dx.Reset();
		}
	}

	private void MeasureItemHandler(object sender, MeasureItemEventArgs e)
	{
		e.ItemHeight += 8;
	}

	private void SetContent()
	{
		//IL_0089: Unknown result type (might be due to invalid IL or missing references)
		//IL_008f: Expected O, but got Unknown
		lb.Items.Clear();
		stm = null;
		if (scn == null || dx == null)
		{
			return;
		}
		stm = new SceneToMesh(scn, dx);
		dx.Reset();
		dx.ResetDefaultViewport();
		lb.Items.Add("--- [Display Mesh] ---");
		foreach (Joint item2 in scn.JointCollection)
		{
			Joint item = item2;
			lb.Items.Add(item);
		}
	}
}
