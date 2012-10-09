using System;
using System.IO;
using System.Media;
using System.Windows.Forms;
using TomatoDoer.Model;
using Windows7.DesktopIntegration.WindowsForms;
namespace TomatoDoer
{
	public class SoundProgress
	{
		private ITomatoTimer _Timer;
		public SoundProgress(ITomatoTimer timer)
		{
			_Timer = timer;
			_Timer.TomatoDoneOrSquashed += new System.Action(_Timer_TomatoDoneOrSquashed);
			
		}
		void _Timer_TomatoDoneOrSquashed()
		{
			SoundPlayer sp = new SoundPlayer(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Windows) , "media\\notify.wav"));
			sp.Play();
		}
	}
}