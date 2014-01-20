using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using TomatoDoer.Model;
namespace TomatoDoer
{
    public static class Program
	{
	    /// <summary>
	    /// The main entry point for the application.
	    /// </summary>
	    [STAThread]
	    private static void Main()
	    {
	        Application.EnableVisualStyles();
	        Application.SetCompatibleTextRenderingDefault(false);

	        new SoundProgress(TomatoTimer.Instance);

	         new TaskbarProgress(TomatoTimer.Instance);
	        TomatoLog.Instance.Load();
	        var formMain = new TomatoTimerForm();
	        var applicationContext = new ApplicationContext();
	        applicationContext.MainForm = formMain;
	        Application.Run(applicationContext);
	        Application.Run(new TomatoTimerForm());

	    }
	}
}
