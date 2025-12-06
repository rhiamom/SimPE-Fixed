/***************************************************************************
 *   Copyright (C) 2005 by Ambertation                                     *
 *   quaxi@ambertation.de                                                  *
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
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;


namespace SimPe
{
	/// <summary>
	/// Zusammenfassung für About.
	/// </summary>
	public class About : SimPe.Windows.Forms.HelpForm
    {
		private System.Windows.Forms.RichTextBox rtb;
		private System.Windows.Forms.Button button1;
        private Button button2;
        private WebBrowser wb;
		/// <summary>
		/// Erforderliche Designervariable.
		/// </summary>
		private System.ComponentModel.Container components = null;

        public About() 
            :this(false)
		{
        }
		public About(bool html)
		{
			//
			// Erforderlich für die Windows Form-Designerunterstützung
			//
			InitializeComponent();
            button2.BackColor = SystemColors.Control;
            this.FormBorderStyle = FormBorderStyle.None;
			           
            wb.Navigating += new WebBrowserNavigatingEventHandler(wb_Navigating);
            wb.Navigated += new WebBrowserNavigatedEventHandler(wb_Navigated);
            wb.IsWebBrowserContextMenuEnabled = Helper.QARelease;
            wb.AllowNavigation = true;

            wb.Visible = html;
            rtb.Visible = !html;
		}

        void wb_Navigated(object sender, WebBrowserNavigatedEventArgs e)
        {
            
        }

        void wb_Navigating(object sender, WebBrowserNavigatingEventArgs e)
        {
            if (e.Url.OriginalString.StartsWith("about:")) return;
            if (e.TargetFrameName != "_blank")
            {
                e.Cancel = true;
                System.Windows.Forms.Help.ShowHelp(wb, e.Url.OriginalString);
                //wb.Navigate(e.Url, true);
            }
        }

		/// <summary>
		/// Die verwendeten Ressourcen bereinigen.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Vom Windows Form-Designer generierter Code
		/// <summary>
		/// Erforderliche Methode für die Designerunterstützung. 
		/// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
		/// </summary>
		private void InitializeComponent()
		{
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(About));
            this.rtb = new System.Windows.Forms.RichTextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.wb = new System.Windows.Forms.WebBrowser();
            this.SuspendLayout();
            // 
            // rtb
            // 
            this.rtb.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.rtb.BackColor = System.Drawing.Color.White;
            this.rtb.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.rtb.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.rtb.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rtb.Location = new System.Drawing.Point(33, 132);
            this.rtb.Name = "rtb";
            this.rtb.ReadOnly = true;
            this.rtb.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
            this.rtb.Size = new System.Drawing.Size(724, 295);
            this.rtb.TabIndex = 2;
            this.rtb.Text = "";
            this.rtb.LinkClicked += new System.Windows.Forms.LinkClickedEventHandler(this.rtb_LinkClicked);
            this.rtb.Enter += new System.EventHandler(this.rtb_Enter);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(342, 170);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 3;
            this.button1.Text = "button1";
            // 
            // button2
            // 
            this.button2.Image = ((System.Drawing.Image)(resources.GetObject("button2.Image")));
            this.button2.Location = new System.Drawing.Point(695, 12);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(64, 23);
            this.button2.TabIndex = 4;
            this.button2.UseVisualStyleBackColor = false;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // wb
            // 
            this.wb.AllowNavigation = false;
            this.wb.AllowWebBrowserDrop = false;
            this.wb.IsWebBrowserContextMenuEnabled = false;
            this.wb.Location = new System.Drawing.Point(33, 132);
            this.wb.MinimumSize = new System.Drawing.Size(20, 20);
            this.wb.Name = "wb";
            this.wb.Size = new System.Drawing.Size(728, 295);
            this.wb.TabIndex = 5;
            this.wb.WebBrowserShortcutsEnabled = false;
            // 
            // About
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.ClientSize = new System.Drawing.Size(773, 443);
            this.Controls.Add(this.wb);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.rtb);
            this.Controls.Add(this.button1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "About";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "About";
            this.ResumeLayout(false);

		}
		#endregion

		void LoadResource(string flname)
		{
            rtb.Visible = true;
			System.Diagnostics.FileVersionInfo v = Helper.SimPeVersion;
			System.IO.Stream s = this.GetType().Assembly.GetManifestResourceStream("SimPe."+flname+"-"+System.Globalization.CultureInfo.CurrentCulture.TwoLetterISOLanguageName+".rtf");
			if (s==null) s = this.GetType().Assembly.GetManifestResourceStream("SimPe."+flname+"-en.rtf");
			if (s!=null) 
			{
				System.IO.StreamReader sr = new System.IO.StreamReader(s);
				string vtext = Helper.VersionToString(v); //v.FileMajorPart +"."+v.FileMinorPart;
				if (Helper.QARelease) vtext = "QA " + vtext;
				if (Helper.DebugMode) vtext += " [debug]";
				rtb.Rtf = sr.ReadToEnd().Replace("\\{Version\\}", vtext);
			} 
			else 
			{
				rtb.Text = "Error: Unknown Resource "+flname+".";
			}
		}

		/// <summary>
		/// Display the About Screen
		/// </summary>
		public static void ShowAbout()
		{
           
			About f = new About();
			f.Text = SimPe.Localization.GetString("About");

			f.LoadResource("about");
            SimPe.Splash.Screen.Stop();
			f.ShowDialog();
		}

		/// <summary>
		/// Display the Welcome Screen
		/// </summary>
		public static void ShowWelcome()
		{
			About f = new About();
			f.Text = SimPe.Localization.GetString("Welcome");

            f.LoadResource("welcome");
            SimPe.Splash.Screen.Stop();

			f.ShowDialog();
		}

        //static System.Threading.Thread uthread;

		/// <summary>
		/// Search for Updates in an async Thread
		/// </summary>
		
        private static string GetHtmlBase()
        {
            System.IO.Stream s = typeof(About).Assembly.GetManifestResourceStream("SimPe.simpe.html");
            string html = "{CONTENT}";
            if (s != null)
            {
                System.IO.StreamReader sr = new System.IO.StreamReader(s);
                html = sr.ReadToEnd();
                sr.Close();
                sr.Dispose();
                sr = null;
            }
            return html;
        }

		static string TutorialTempFile
		{
			get 
			{
				return System.IO.Path.Combine(Helper.SimPeDataPath, "tutorialtemp.rtf");
			}
		}

		static string GetStoredTutorials()
		{
			if (System.IO.File.Exists(TutorialTempFile))
			{
				System.IO.StreamReader sr = System.IO.File.OpenText(TutorialTempFile);
				try 
				{
					return sr.ReadToEnd();
				} 
				finally 
				{
					sr.Close();
					sr.Dispose();
					sr = null;
				}
			}

			return "";
		}

		static void SaveTutorials(string cont)
		{
			System.IO.StreamWriter sw = System.IO.File.CreateText(TutorialTempFile);
			try 
			{
				sw.Write(cont);
			} 
			finally 
			{
				sw.Close();
				sw.Dispose();
				sw = null;
			}
		}

		static string TazzMannTutorial(bool real)
		{
			if (real) return System.IO.Path.Combine(Helper.SimPePath, @"Doc\SimPE_FTGU.pdf");
			else return "http://localhost/Doc/SimPE_FTGU.pdf";
		}

		static string Introduction(bool real)
		{
			if (real) return System.IO.Path.Combine(Helper.SimPePath, @"Doc\Introduction.pdf");
			else return "http://localhost/Doc/Introduction.pdf";
		}

		/// <summary>
		/// Display the Update Screen
		/// </summary>
		/// <param name="show">true, if it should be visible even if no updates were found</param>
		public static void ShowTutorials()
		{
			Wait.SubStart();
			About f = new About(true);
			string text = "";
            string html = GetHtmlBase();
			try 
			{
				f.Text = SimPe.Localization.GetString("Tutorials");			
							

				text += "<p>";
				if (System.IO.File.Exists(Introduction(true)))
				{
					text += "\n                <li>";
					text += "\n                    <a href=\""+Introduction(false)+"\"><span class=\"serif\">Emily:</span> Introduction to the new SimPE</a>";
					text += "\n                </li>";
				}
				if (System.IO.File.Exists(TazzMannTutorial(true)))
				{
					text += "\n                <li>";
					text += "\n                    <a href=\""+TazzMannTutorial(false)+"\"><span class=\"serif\">TazzMann:</span> SimPE - From the Ground Up</a>";
					text += "\n                </li>";
				}
				//text += WebUpdate.GetTutorials().Replace("<ul>", "<ul class=\"nobullet\">");			
				text += "</p>";

				//text = text.Replace("<li>", "");
				//text = text.Replace("</li>", "<br /><br />");

                f.wb.DocumentText = html.Replace("{CONTENT}", text);
                SaveTutorials(text);
				text = Ambertation.Html2Rtf.Convert(text);
				text = text.Replace("(http://", @"\pard\par         (http://");
				
				f.rtb.Rtf = text;
			} 
			catch (Exception ex)
			{
                f.wb.DocumentText = html.Replace("{CONTENT}", GetStoredTutorials());
				f.rtb.Rtf = GetStoredTutorials();
                if (f.rtb.Rtf == "")
                {
                    f.rtb.Rtf = ex.Message;
                    f.wb.DocumentText = html.Replace("{CONTENT}", ex.Message);
                }
			}

            Wait.SubStop();
            SimPe.Splash.Screen.Stop();		
			f.ShowDialog();
		}

		private void rtb_LinkClicked(object sender, System.Windows.Forms.LinkClickedEventArgs e)
		{
			try 
			{
				System.Windows.Forms.Help.ShowHelp(this, e.LinkText.Replace("http://localhost", Helper.SimPePath));
			} 
			catch (Exception ex) 
			{
				Helper.ExceptionMessage(ex);
			}
		}

		private void rtb_Enter(object sender, System.EventArgs e)
		{
			button1.Focus();
		}

        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }
	}
}
