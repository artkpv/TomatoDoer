using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Reflection;
using TomatoDoer.Presentation;

namespace TomatoDoer
{
	public partial class About : Form
	{
		public About()
		{
			InitializeComponent();
		}

		private void About_Load(object sender, EventArgs e)
		{
			Version version = Assembly.GetExecutingAssembly().GetName().Version;
			label2.Text = string.Format("v{0}.{1}", version.Major, version.Minor);
		}

		private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			System.Diagnostics.Process.Start("http://www.pomodorotechnique.com/");
		}

		private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			System.Diagnostics.Process.Start("http://sourceforge.net/projects/tomatodoer/");
		}

	}
}
