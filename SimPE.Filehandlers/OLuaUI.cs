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
using SimPe.Interfaces.Plugin;
using System.Collections;
using Avalonia.Controls;

namespace SimPe.PackedFiles.UserInterface
{
	/// <summary>
	/// Avalonia port of ObjLua UI.
	/// </summary>
	public class ObjLua : UserControl, SimPe.Interfaces.Plugin.IPackedFileUI
	{
		private TreeView tv = new TreeView();
		private Button btSave = new Button { Content = "Export..." };
		private Button btLoad = new Button { Content = "Import..." };
		private TextBlock label1 = new TextBlock { Text = "Name:" };
		private TextBox tbName = new TextBox();
		private Button button1 = new Button { Content = "Commit" };
		private Button button2 = new Button { Content = "Export to Source..." };

		public ObjLua()
		{
			button2.IsEnabled = Helper.QARelease;

			var topRow = new StackPanel { Orientation = Avalonia.Layout.Orientation.Horizontal };
			topRow.Children.Add(label1);
			topRow.Children.Add(tbName);

			var bottomRow = new StackPanel { Orientation = Avalonia.Layout.Orientation.Horizontal };
			bottomRow.Children.Add(btLoad);
			bottomRow.Children.Add(btSave);
			bottomRow.Children.Add(button1);
			bottomRow.Children.Add(button2);

			var grid = new Grid();
			grid.RowDefinitions.Add(new RowDefinition(GridLength.Auto));
			grid.RowDefinitions.Add(new RowDefinition(GridLength.Star));
			grid.RowDefinitions.Add(new RowDefinition(GridLength.Auto));

			Grid.SetRow(topRow, 0); grid.Children.Add(topRow);
			Grid.SetRow(tv, 1); grid.Children.Add(tv);
			Grid.SetRow(bottomRow, 2); grid.Children.Add(bottomRow);

			Content = grid;

			btSave.Click += btSave_Click;
			btLoad.Click += btLoad_Click;
			button1.Click += button1_Click;
			button2.Click += button2_Click;
			tbName.TextChanged += tbName_TextChanged;
		}

		public void Dispose() { }

		void AddFunction(ItemCollection nodes, SimPe.PackedFiles.Wrapper.ObjLuaFunction fkt)
		{
			var tn = new TreeViewItem { Header = fkt.ToString(), Tag = fkt };
			nodes.Add(tn);

			var ctn = new TreeViewItem { Header = "Constants" };
			tn.Items.Add(ctn);
			foreach (SimPe.PackedFiles.Wrapper.ObjLuaConstant olc in fkt.Constants)
			{
				ctn.Items.Add(new TreeViewItem { Header = olc.ToString(), Tag = olc });
			}

			var cltn = new TreeViewItem { Header = "Locals" };
			tn.Items.Add(cltn);
			int ct = 0;
			foreach (SimPe.PackedFiles.Wrapper.ObjLuaLocalVar c in fkt.Locals)
			{
				cltn.Items.Add(new TreeViewItem { Header = Helper.HexString(ct++) + ": " + c.ToString(), Tag = c });
			}

			var cutn = new TreeViewItem { Header = "UpValues" };
			tn.Items.Add(cutn);
			ct = 0;
			foreach (SimPe.PackedFiles.Wrapper.ObjLuaUpValue c in fkt.UpValues)
			{
				cutn.Items.Add(new TreeViewItem { Header = Helper.HexString(ct++) + ": " + c.ToString(), Tag = c });
			}

			var cstn = new TreeViewItem { Header = "SourceLines" };
			tn.Items.Add(cstn);
			ct = 0;
			foreach (SimPe.PackedFiles.Wrapper.ObjLuaSourceLine c in fkt.SourceLine)
			{
				cstn.Items.Add(new TreeViewItem { Header = Helper.HexString(ct++) + ": " + c.ToString(), Tag = c });
			}

			var ftn = new TreeViewItem { Header = "Functions" };
			tn.Items.Add(ftn);
			foreach (SimPe.PackedFiles.Wrapper.ObjLuaFunction olf in fkt.Functions)
			{
				AddFunction(ftn.Items, olf);
			}

			var cdtn = new TreeViewItem { Header = "Instructions" };
			tn.Items.Add(cdtn);
			ct = 0;
			foreach (SimPe.PackedFiles.Wrapper.ObjLuaCode c in fkt.Codes)
			{
				cdtn.Items.Add(new TreeViewItem { Header = Helper.HexString(ct++) + ": " + c.ToString(), Tag = c });
			}
		}

		SimPe.PackedFiles.Wrapper.ObjLua lua;
		protected SimPe.PackedFiles.Wrapper.ObjLua Wrapper
		{
			get { return lua; }
		}

		#region IPackedFileUI Member

		public void UpdateGUI(IFileWrapper wrapper)
		{
			lua = (SimPe.PackedFiles.Wrapper.ObjLua)wrapper;

			tv.Items.Clear();
			AddFunction(tv.Items, lua.Root);

			tbName.Text = lua.FileName;
		}

		public Avalonia.Controls.Control GUIHandle
		{
			get { return this; }
		}

		#endregion

		private async void btSave_Click(object sender, Avalonia.Interactivity.RoutedEventArgs e)
		{
			var topLevel = TopLevel.GetTopLevel(this);
			if (topLevel == null) return;
			var file = await topLevel.StorageProvider.SaveFilePickerAsync(new Avalonia.Platform.Storage.FilePickerSaveOptions
			{
				Title = "Export Lua",
				SuggestedFileName = Wrapper.FileName,
				FileTypeChoices = new[] { new Avalonia.Platform.Storage.FilePickerFileType("Lua Script") { Patterns = new[] { "*.lua" } },
				                          new Avalonia.Platform.Storage.FilePickerFileType("All Files") { Patterns = new[] { "*.*" } } }
			});
			if (file != null)
				Wrapper.ExportLua(file.Path.LocalPath);
		}

		private async void btLoad_Click(object sender, Avalonia.Interactivity.RoutedEventArgs e)
		{
			var topLevel = TopLevel.GetTopLevel(this);
			if (topLevel == null) return;
			var files = await topLevel.StorageProvider.OpenFilePickerAsync(new Avalonia.Platform.Storage.FilePickerOpenOptions
			{
				Title = "Import Lua",
				AllowMultiple = false,
				FileTypeFilter = new[] { new Avalonia.Platform.Storage.FilePickerFileType("Lua Script") { Patterns = new[] { "*.lua" } },
				                         new Avalonia.Platform.Storage.FilePickerFileType("All Files") { Patterns = new[] { "*.*" } } }
			});
			if (files != null && files.Count > 0)
				Wrapper.ImportLua(files[0].Path.LocalPath);

			Wrapper.SynchronizeUserData(true, false);
			UpdateGUI(Wrapper);
		}

		private void tbName_TextChanged(object sender, Avalonia.Controls.TextChangedEventArgs e)
		{
			Wrapper.FileName = tbName.Text;
		}

		private void button1_Click(object sender, Avalonia.Interactivity.RoutedEventArgs e)
		{
			Wrapper.SynchronizeUserData(true, false);
		}

		private async void button2_Click(object sender, Avalonia.Interactivity.RoutedEventArgs e)
		{
			var topLevel = TopLevel.GetTopLevel(this);
			if (topLevel == null) return;
			var file = await topLevel.StorageProvider.SaveFilePickerAsync(new Avalonia.Platform.Storage.FilePickerSaveOptions
			{
				Title = "Export to Source",
				SuggestedFileName = Wrapper.FileName,
				FileTypeChoices = new[] { new Avalonia.Platform.Storage.FilePickerFileType("Lua Script") { Patterns = new[] { "*.lua" } } }
			});
			if (file != null)
			{
				string src = Wrapper.ToSource();
				System.IO.StreamWriter sw = System.IO.File.CreateText(file.Path.LocalPath);
				try
				{
					sw.Write(src);
				}
				finally
				{
					sw.Close();
					sw.Dispose();
					sw = null;
				}
			}
		}
	}
}
