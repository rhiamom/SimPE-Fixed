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
using System.Collections;
using Avalonia.Controls;

namespace SimPe.Plugin.TabPage
{
	/// <summary>
	/// Summary description for fShapeRefNode.
	/// </summary>
	public class TransformNode : Avalonia.Controls.TabItem
	{
		private Avalonia.Controls.Button ll_tn_add;
		private Avalonia.Controls.TextBox tb_tn_2;
		private Avalonia.Controls.TextBlock label16;
		private Avalonia.Controls.TextBox tb_tn_1;
		private Avalonia.Controls.TextBlock label17;
		internal Avalonia.Controls.ListBox lb_tn;
		private Avalonia.Controls.Button ll_tn_delete;
		internal Avalonia.Controls.TextBox tb_tn_ver;
		private Avalonia.Controls.TextBlock label26;
		internal Avalonia.Controls.TextBox tb_tn_ukn;
		private Avalonia.Controls.TextBlock label19;
		// Translation
		internal Avalonia.Controls.TextBox tb_tn_tx;
		private Avalonia.Controls.TextBlock label49;
		internal Avalonia.Controls.TextBox tb_tn_ty;
		private Avalonia.Controls.TextBlock label50;
		internal Avalonia.Controls.TextBox tb_tn_tz;
		private Avalonia.Controls.TextBlock label51;
		// Quaternion
		internal Avalonia.Controls.TextBox tb_tn_rx;
		private Avalonia.Controls.TextBlock label54;
		internal Avalonia.Controls.TextBox tb_tn_ry;
		private Avalonia.Controls.TextBlock label53;
		internal Avalonia.Controls.TextBox tb_tn_rz;
		private Avalonia.Controls.TextBlock label52;
		internal Avalonia.Controls.TextBox tb_tn_rw;
		private Avalonia.Controls.TextBlock label55;
		// Axis-angle rotation
		internal Avalonia.Controls.TextBox tb_tn_ax;
		private Avalonia.Controls.TextBlock label57;
		internal Avalonia.Controls.TextBox tb_tn_ay;
		private Avalonia.Controls.TextBlock label56;
		internal Avalonia.Controls.TextBox tb_tn_az;
		private Avalonia.Controls.TextBlock label31;
		internal Avalonia.Controls.TextBox tb_tn_a;
		private Avalonia.Controls.TextBlock label30;
		// Euler rotation
		internal Avalonia.Controls.TextBox tb_tn_ey;
		private Avalonia.Controls.TextBlock label62;
		internal Avalonia.Controls.TextBox tb_tn_ep;
		private Avalonia.Controls.TextBlock label61;
		internal Avalonia.Controls.TextBox tb_tn_er;
		private Avalonia.Controls.TextBlock label60;

		public TransformNode()
		{
			this.Header = "TransformNode";

			// Settings group
			label26 = new Avalonia.Controls.TextBlock { Text = "Version:" };
			tb_tn_ver = new Avalonia.Controls.TextBox { Text = "0x00000000" };
			tb_tn_ver.TextChanged += new EventHandler<Avalonia.Controls.TextChangedEventArgs>(this.TNChangeSettings);
			label19 = new Avalonia.Controls.TextBlock { Text = "GMDC joint index:" };
			tb_tn_ukn = new Avalonia.Controls.TextBox { Text = "0x00000000" };
			tb_tn_ukn.TextChanged += new EventHandler<Avalonia.Controls.TextChangedEventArgs>(this.TNChangeSettings);

			// Translation
			label49 = new Avalonia.Controls.TextBlock { Text = "X:" };
			tb_tn_tx = new Avalonia.Controls.TextBox { Text = "0x00000000" };
			tb_tn_tx.TextChanged += new EventHandler<Avalonia.Controls.TextChangedEventArgs>(this.TNChangeSettings);
			label50 = new Avalonia.Controls.TextBlock { Text = "Y:" };
			tb_tn_ty = new Avalonia.Controls.TextBox { Text = "0x00000000" };
			tb_tn_ty.TextChanged += new EventHandler<Avalonia.Controls.TextChangedEventArgs>(this.TNChangeSettings);
			label51 = new Avalonia.Controls.TextBlock { Text = "Z:" };
			tb_tn_tz = new Avalonia.Controls.TextBox { Text = "0x00000000" };
			tb_tn_tz.TextChanged += new EventHandler<Avalonia.Controls.TextChangedEventArgs>(this.TNChangeSettings);

			// Quaternion
			label54 = new Avalonia.Controls.TextBlock { Text = "X:" };
			tb_tn_rx = new Avalonia.Controls.TextBox { Text = "0x00000000" };
			tb_tn_rx.TextChanged += new EventHandler<Avalonia.Controls.TextChangedEventArgs>(this.TNChangeSettings);
			label53 = new Avalonia.Controls.TextBlock { Text = "Y:" };
			tb_tn_ry = new Avalonia.Controls.TextBox { Text = "0x00000000" };
			tb_tn_ry.TextChanged += new EventHandler<Avalonia.Controls.TextChangedEventArgs>(this.TNChangeSettings);
			label52 = new Avalonia.Controls.TextBlock { Text = "Z:" };
			tb_tn_rz = new Avalonia.Controls.TextBox { Text = "0x00000000" };
			tb_tn_rz.TextChanged += new EventHandler<Avalonia.Controls.TextChangedEventArgs>(this.TNChangeSettings);
			label55 = new Avalonia.Controls.TextBlock { Text = "W:" };
			tb_tn_rw = new Avalonia.Controls.TextBox { Text = "0x00000000" };
			tb_tn_rw.TextChanged += new EventHandler<Avalonia.Controls.TextChangedEventArgs>(this.TNChangeSettings);

			// Axis-angle rotation
			label57 = new Avalonia.Controls.TextBlock { Text = "X:" };
			tb_tn_ax = new Avalonia.Controls.TextBox { Text = "0" };
			tb_tn_ax.TextChanged += new EventHandler<Avalonia.Controls.TextChangedEventArgs>(this.TNChangedQuaternion);
			label56 = new Avalonia.Controls.TextBlock { Text = "Y:" };
			tb_tn_ay = new Avalonia.Controls.TextBox { Text = "0" };
			tb_tn_ay.TextChanged += new EventHandler<Avalonia.Controls.TextChangedEventArgs>(this.TNChangedQuaternion);
			label31 = new Avalonia.Controls.TextBlock { Text = "Z:" };
			tb_tn_az = new Avalonia.Controls.TextBox { Text = "0" };
			tb_tn_az.TextChanged += new EventHandler<Avalonia.Controls.TextChangedEventArgs>(this.TNChangedQuaternion);
			label30 = new Avalonia.Controls.TextBlock { Text = "Angle:" };
			tb_tn_a = new Avalonia.Controls.TextBox { Text = "0" };
			tb_tn_a.TextChanged += new EventHandler<Avalonia.Controls.TextChangedEventArgs>(this.TNChangedQuaternion);

			// Euler rotation
			label62 = new Avalonia.Controls.TextBlock { Text = "Y:" };
			tb_tn_ey = new Avalonia.Controls.TextBox { Text = "0" };
			tb_tn_ey.TextChanged += new EventHandler<Avalonia.Controls.TextChangedEventArgs>(this.TNChangedEulerQuaternion);
			label61 = new Avalonia.Controls.TextBlock { Text = "P:" };
			tb_tn_ep = new Avalonia.Controls.TextBox { Text = "0" };
			tb_tn_ep.TextChanged += new EventHandler<Avalonia.Controls.TextChangedEventArgs>(this.TNChangedEulerQuaternion);
			label60 = new Avalonia.Controls.TextBlock { Text = "R:" };
			tb_tn_er = new Avalonia.Controls.TextBox { Text = "0" };
			tb_tn_er.TextChanged += new EventHandler<Avalonia.Controls.TextChangedEventArgs>(this.TNChangedEulerQuaternion);

			// Child nodes list
			lb_tn = new Avalonia.Controls.ListBox();
			lb_tn.SelectionChanged += new EventHandler<Avalonia.Controls.SelectionChangedEventArgs>(this.TNSelect);
			label17 = new Avalonia.Controls.TextBlock { Text = "Unknown 1:" };
			tb_tn_1 = new Avalonia.Controls.TextBox { Text = "0x0000" };
			tb_tn_1.TextChanged += new EventHandler<Avalonia.Controls.TextChangedEventArgs>(this.TNChangedItems);
			label16 = new Avalonia.Controls.TextBlock { Text = "Child Index:" };
			tb_tn_2 = new Avalonia.Controls.TextBox { Text = "0x00000000" };
			tb_tn_2.TextChanged += new EventHandler<Avalonia.Controls.TextChangedEventArgs>(this.TNChangedItems);
			ll_tn_add = new Avalonia.Controls.Button { Content = "add" };
			ll_tn_add.Click += new EventHandler<Avalonia.Interactivity.RoutedEventArgs>(this.TNItemsAdd);
			ll_tn_delete = new Avalonia.Controls.Button { Content = "delete" };
			ll_tn_delete.Click += new EventHandler<Avalonia.Interactivity.RoutedEventArgs>(this.TNItemsDelete);

			Content = new Avalonia.Controls.StackPanel { Children = {
				label26, tb_tn_ver, label19, tb_tn_ukn,
				label49, tb_tn_tx, label50, tb_tn_ty, label51, tb_tn_tz,
				label54, tb_tn_rx, label53, tb_tn_ry, label52, tb_tn_rz, label55, tb_tn_rw,
				label57, tb_tn_ax, label56, tb_tn_ay, label31, tb_tn_az, label30, tb_tn_a,
				label62, tb_tn_ey, label61, tb_tn_ep, label60, tb_tn_er,
				lb_tn, label17, tb_tn_1, label16, tb_tn_2, ll_tn_add, ll_tn_delete
			}};
		}

		private void TNChangeSettings(object sender, System.EventArgs e)
		{
			if (this.tb_tn_a.Tag!=null) return;
			if (Tag==null) return;
			try
			{
				SimPe.Plugin.TransformNode tn = (SimPe.Plugin.TransformNode)Tag;

				tn.Version = Convert.ToUInt32(tb_tn_ver.Text, 16);
				tn.JointReference = Convert.ToInt32(tb_tn_ukn.Text, 16);

				tn.TransformX = Convert.ToSingle(tb_tn_tx.Text);
				tn.TransformY = Convert.ToSingle(tb_tn_ty.Text);
				tn.TransformZ = Convert.ToSingle(tb_tn_tz.Text);

				tn.RotationX = Convert.ToSingle(tb_tn_rx.Text);
				tn.RotationY = Convert.ToSingle(tb_tn_ry.Text);
				tn.RotationZ = Convert.ToSingle(tb_tn_rz.Text);
				tn.RotationW = Convert.ToSingle(tb_tn_rw.Text);

				SimPe.Geometry.Quaternion q = tn.Rotation;
				TNUpdateTextValues(q, false, true, true);

				tn.Changed = true;
			}
			catch (Exception)
			{
				//Helper.ExceptionMessage("", ex);
			}
		}

		private void TNChangedQuaternion(object sender, System.EventArgs e)
		{
			if (this.tb_tn_a.Tag!=null) return;
			if (Tag==null) return;
			try
			{
				SimPe.Plugin.TransformNode tn = (SimPe.Plugin.TransformNode)Tag;
				SimPe.Geometry.Quaternion q = SimPe.Geometry.Quaternion.FromAxisAngle(
					new SimPe.Geometry.Vector3f(
					Convert.ToSingle(tb_tn_ax.Text),
					Convert.ToSingle(tb_tn_ay.Text),
					Convert.ToSingle(tb_tn_az.Text)),
					SimPe.Geometry.Quaternion.DegToRad(Convert.ToSingle(tb_tn_a.Text)));

				tn.Rotation = q;

				TNUpdateTextValues(q, true, false, true);
			}
			catch {}
			finally
			{
				this.tb_tn_a.Tag = null;
			}
		}

		internal void TNUpdateTextValues(SimPe.Geometry.Quaternion q, bool quat, bool axis, bool euler)
		{
			//set Angles
			try
			{
				this.tb_tn_a.Tag = true;
				if (quat)
				{
					this.tb_tn_rx.Text = q.X.ToString("N6");
					this.tb_tn_ry.Text = q.Y.ToString("N6");
					this.tb_tn_rz.Text = q.Z.ToString("N6");
					this.tb_tn_rw.Text = q.W.ToString("N6");
				}

				if (axis)
				{
					this.tb_tn_ax.Text = q.Axis.X.ToString("N6");
					this.tb_tn_ay.Text = q.Axis.Y.ToString("N6");
					this.tb_tn_az.Text = q.Axis.Z.ToString("N6");
					this.tb_tn_a.Text = SimPe.Geometry.Quaternion.RadToDeg(q.Angle).ToString("N6");
				}

				if (euler)
				{
					SimPe.Geometry.Vector3f ea = q.GetEulerAngles();
					this.tb_tn_ey.Text = SimPe.Geometry.Quaternion.RadToDeg(ea.Y).ToString("N6");
					this.tb_tn_ep.Text = SimPe.Geometry.Quaternion.RadToDeg(ea.X).ToString("N6");
					this.tb_tn_er.Text = SimPe.Geometry.Quaternion.RadToDeg(ea.Z).ToString("N6");
				}
			}
			finally
			{
				this.tb_tn_a.Tag = null;
			}
		}

		private void TNChangedEulerQuaternion(object sender, System.EventArgs e)
		{
			if (this.tb_tn_a.Tag!=null) return;
			if (Tag==null) return;
			try
			{
				SimPe.Plugin.TransformNode tn = (SimPe.Plugin.TransformNode)Tag;
				SimPe.Geometry.Quaternion q = SimPe.Geometry.Quaternion.FromEulerAngles(
					SimPe.Geometry.Quaternion.DegToRad(Convert.ToSingle(tb_tn_ey.Text)),
					SimPe.Geometry.Quaternion.DegToRad(Convert.ToSingle(tb_tn_ep.Text)),
					SimPe.Geometry.Quaternion.DegToRad(Convert.ToSingle(tb_tn_er.Text))
					);
				tn.Rotation = q;

				TNUpdateTextValues(q, true, true, false);
			}
			catch {}
			finally
			{
				this.tb_tn_a.Tag = null;
			}
		}

		#region Select TN Items
		private void TNSelect(object sender, System.EventArgs e)
		{
			if (lb_tn.Tag != null) return;
			if (this.lb_tn.SelectedIndex<0) return;

			try
			{
				lb_tn.Tag = true;
				SimPe.Plugin.TransformNode tn = (SimPe.Plugin.TransformNode)Tag;
				TransformNodeItem b = (TransformNodeItem)lb_tn.Items[lb_tn.SelectedIndex];

				tb_tn_1.Text = "0x"+Helper.HexString((ushort)b.Unknown1);
				tb_tn_2.Text = "0x"+Helper.HexString((uint)b.ChildNode);
				tn.Changed = true;
			}
			catch (Exception)
			{
				//Helper.ExceptionMessage("", ex);
			}
			finally
			{
				lb_tn.Tag = null;
			}
		}

		private void TNChangedItems(object sender, System.EventArgs e)
		{
			if (lb_tn.Tag != null) return;
			if (this.lb_tn.SelectedIndex<0) return;

			try
			{
				lb_tn.Tag = true;
				SimPe.Plugin.TransformNode tn = (SimPe.Plugin.TransformNode)Tag;
				TransformNodeItem b = (TransformNodeItem)lb_tn.Items[lb_tn.SelectedIndex];

				b.Unknown1 = Convert.ToUInt16(tb_tn_1.Text, 16);
				b.ChildNode = (int)Convert.ToUInt32(tb_tn_2.Text, 16);

				lb_tn.Items[lb_tn.SelectedIndex] = b;
				tn.Changed = true;
			}
			catch (Exception)
			{
				//Helper.ExceptionMessage("", ex);
			}
			finally
			{
				lb_tn.Tag = null;
			}
		}

		private void TNItemsAdd(object sender, Avalonia.Interactivity.RoutedEventArgs e)
		{
			if (Tag==null) return;
			try
			{
				lb_tn.Tag = true;
				SimPe.Plugin.TransformNode tn = (SimPe.Plugin.TransformNode)Tag;
				TransformNodeItem b = new TransformNodeItem();

				b.Unknown1 = Convert.ToUInt16(tb_tn_1.Text, 16);
				b.ChildNode = (int)Convert.ToUInt32(tb_tn_2.Text, 16);

				tn.Items.Add(b);//= (TransformNodeItem[])Helper.Add(tn.Items, b);
				lb_tn.Items.Add(b);
				tn.Changed = true;
			}
			catch (Exception ex)
			{
				Helper.ExceptionMessage("", ex);
			}
			finally
			{
				lb_tn.Tag = null;
			}
		}

		private void TNItemsDelete(object sender, Avalonia.Interactivity.RoutedEventArgs e)
		{
			if (Tag==null) return;
			if (lb_tn.SelectedIndex<0) return;
			try
			{
				lb_tn.Tag = true;
				SimPe.Plugin.TransformNode tn = (SimPe.Plugin.TransformNode)Tag;
				TransformNodeItem b = (TransformNodeItem)lb_tn.Items[lb_tn.SelectedIndex];

				tn.Items.Remove(b);// = (TransformNodeItem[])Helper.Delete(tn.Items, b);
				lb_tn.Items.Remove(b);
				tn.Changed = true;
			}
			catch (Exception ex)
			{
				Helper.ExceptionMessage("", ex);
			}
			finally
			{
				lb_tn.Tag = null;
			}
		}
		#endregion
	}
}
