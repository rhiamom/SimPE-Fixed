/***************************************************************************
 *   Copyright (C) 2005 by Ambertation                                     *
 *   quaxi@ambertation.de                                                  *
 *                                                                         *
 *   Copyright (C) 2025 by GramzeSweatshop                                 *
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
using SimPe.PackedFiles.Wrapper;

namespace SimPe.PackedFiles.UserInterface
{
	/// <summary>
	/// Avalonia port of SdscExtendedForm.
	/// PropertyGrid replaced with a simple TextBox (read-only property dump).
	/// ShowDialog replaced with async window pattern — Execute is now async-friendly.
	/// </summary>
	public class SdscExtendedForm : UserControl
	{
		// Replace PropertyGrid with a simple property list display
		private TextBox pg = new TextBox { AcceptsReturn = true, IsReadOnly = true };
        private Panel panel1 = new Panel();
		private RadioButton rbhex = new RadioButton { Content = "Hexadecimal", GroupName = "digitbase" };
		private RadioButton rbdec = new RadioButton { Content = "Decimal", GroupName = "digitbase" };
		private RadioButton rbbin = new RadioButton { Content = "Binary", GroupName = "digitbase" };
		private Button button1 = new Button { Content = "OK" };
		private Button button2 = new Button { Content = "Cancel" };

		public SdscExtendedForm()
		{
            ThemeManager tm = ThemeManager.Global.CreateChild();

			rbhex.IsCheckedChanged += DigitChanged;
			rbdec.IsCheckedChanged += DigitChanged;
			rbbin.IsCheckedChanged += DigitChanged;
			button1.Click += button1_Click;
			button2.Click += button2_Click;

			var topRow = new StackPanel { Orientation = Avalonia.Layout.Orientation.Horizontal };
			topRow.Children.Add(rbbin);
			topRow.Children.Add(rbdec);
			topRow.Children.Add(rbhex);

			var bottomRow = new StackPanel { Orientation = Avalonia.Layout.Orientation.Horizontal };
			bottomRow.Children.Add(button1);
			bottomRow.Children.Add(button2);

			var grid = new Grid();
			grid.RowDefinitions.Add(new RowDefinition(GridLength.Auto));
			grid.RowDefinitions.Add(new RowDefinition(GridLength.Star));
			grid.RowDefinitions.Add(new RowDefinition(GridLength.Auto));
			Grid.SetRow(topRow, 0); grid.Children.Add(topRow);
			Grid.SetRow(pg, 1); grid.Children.Add(pg);
			Grid.SetRow(bottomRow, 2); grid.Children.Add(bottomRow);

			Content = grid;
		}

		public void Dispose() { }

		Ambertation.PropertyObjectBuilder pob;
		Hashtable names;
		bool propchanged;
		SimPe.Plugin.WantNameLoader wnl;
		short[] shortdata;

		string GetName(int i)
		{
            string name = Helper.HexString(0x0a + 2 * i);
            if (i > 0) name += "; 0x" + (Helper.HexString((ushort)(i - 1)));
            name += ": ";
			name += ((string)names[i]);
			return name;
		}

        void LoadWantTable(SDescVersions version)
        {
            wnl = null;
            string flname = System.IO.Path.Combine(PathProvider.Global.GetExpansion(Expansions.BaseGame).InstallFolder, @"TSData/Res/Objects/objects.package");
            if (System.IO.File.Exists(flname))
            {
                SimPe.Packages.File fl = SimPe.Packages.File.LoadFromFile(flname);
                Interfaces.Files.IPackedFileDescriptor pfd = fl.FindFile(0x53545223, 0, 0x7FE59FD0, 0xc8);
                if (pfd != null)
                {
                    SimPe.PackedFiles.Wrapper.Str str = new Str();
                    str.ProcessData(pfd, fl);
                    SimPe.PackedFiles.Wrapper.StrItemList list = str.LanguageItems(1);
                    string xml = "<wantSimulator>" + Helper.lbr;
                    xml += "  <persondata>" + Helper.lbr;
                    for (int sid = 0; sid < list.Length; sid++)
                    {
                        SimPe.PackedFiles.Wrapper.StrToken si = list[sid];
                        xml += "    <persondata id=\"" + (sid + 1).ToString() + "\" name=\"" + si.Title + "\" /> " + Helper.lbr;
                    }
                    xml += "  </persondata>" + Helper.lbr;
                    xml += "</wantSimulator>" + Helper.lbr;
                    wnl = new SimPe.Plugin.WantNameLoader(xml);
                }
            }

            if (wnl == null)
            {
                FileTable.FileIndex.Load();
                wnl = new SimPe.Plugin.WantNameLoader(version);
            }
        }

        void ShowData(byte[] data)
		{
			shortdata = new short[(data.Length - 0xA) / 2 + 1];
			int j = 0;
			for (int i = 0xa; i < data.Length - 1; i += 2)
			{
				try { shortdata[j++] = BitConverter.ToInt16(data, i); }
				catch { break; }
			}

			FileTable.FileIndex.Load();

			propchanged = false;

			names = new Hashtable();
			ArrayList ns = wnl.GetNames(SimPe.Plugin.WantType.Undefined);

            int max = -1;
			foreach (SimPe.Interfaces.IAlias a in ns)
			{
				max = (int)Math.Max(a.Id, max);
				names[(int)a.Id] = a.Name;
			}
			max++;

            Hashtable ht = new Hashtable();
			for (int i = 0; i < Math.Min(max, shortdata.Length); i++)
			{
				string name = GetName(i);
				if (!ht.Contains(name)) ht.Add(name, shortdata[i]);
			}

			pob = new Ambertation.PropertyObjectBuilder(ht);

			// Display as text since no PropertyGrid
			var sb = new System.Text.StringBuilder();
			foreach (DictionaryEntry entry in ht)
				sb.AppendLine($"{entry.Key} = {entry.Value}");
			pg.Text = sb.ToString();
		}

		void UpdateData(byte[] data)
		{
			if (!propchanged) return;
			propchanged = false;

			try
			{
				Hashtable ht = pob.Properties;

				for (int i = 0; i < shortdata.Length; i++)
				{
					string name = GetName(i);
					if (ht.Contains(name)) shortdata[i] = (short)ht[name];
				}

				int j = 0;
				for (int i = 0xa; i < data.Length - 1; i += 2)
				{
					try
					{
						byte[] d = BitConverter.GetBytes(shortdata[j++]);
						data[i] = d[0];
						data[i + 1] = d[1];
					}
					catch { break; }
				}
			}
			catch (Exception ex)
			{
				Helper.ExceptionMessage("", ex);
			}
		}

		private void DigitChanged(object sender, Avalonia.Interactivity.RoutedEventArgs e)
		{
			if (rbhex.IsChecked == true) Ambertation.BaseChangeShort.DigitBase = 16;
			else if (rbbin.IsChecked == true) Ambertation.BaseChangeShort.DigitBase = 2;
			else Ambertation.BaseChangeShort.DigitBase = 10;
			// In Avalonia there is no pg.Refresh() — data would need to be re-rendered
		}

		private void PropChanged()
		{
			propchanged = true;
		}

		/// <summary>
		/// Execute the Extended Form.
		/// Note: ShowDialog is not available in Avalonia UserControl.
		/// Callers should embed this control in a Window and use async pattern.
		/// For build compatibility, returns false immediately (no-op stub).
		/// </summary>
		public static bool Execute(SimPe.PackedFiles.Wrapper.SDesc wrp)
		{
			// Stub: ShowDialog is not available in Avalonia UserControl context.
			// This would need to be reworked to use an Avalonia Window.
			return false;
		}

		bool ok;
		private void button1_Click(object sender, Avalonia.Interactivity.RoutedEventArgs e)
		{
			ok = true;
		}

		private void button2_Click(object sender, Avalonia.Interactivity.RoutedEventArgs e)
		{
			ok = false;
		}
	}
}
