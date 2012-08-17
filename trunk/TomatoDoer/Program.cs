using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using TomatoDoer.Model;

namespace TomatoDoer
{
	class Program
	{

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main()
		{
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);

			
			new SoundProgress(TomatoTimer.Instance);
			var taskbarProgress = new TaskbarProgress(TomatoTimer.Instance);
			TomatoLog.Instance.Load();
			Application.Run(new TomatoTimerForm());
		}

	}
}
