using System;
using System.Windows.Forms;

namespace TomatoDoer
{
	public class WinTimer : ITimer
	{
		private readonly System.Windows.Forms.Timer _Timer;

		public event Action Tick;

		public WinTimer()
		{
			_Timer = new Timer();
			_Timer.Interval = 1000;
			_Timer.Tick += new EventHandler(TimerTick);
		}

		void TimerTick(object sender, EventArgs e)
		{
			if (Tick != null) Tick();
		}

		public bool IsStarted
		{
			get { return _Timer.Enabled; }
		}

		public void Start()
		{
			_Timer.Start();
		}

		public void Stop()
		{
			_Timer.Stop();
		}
	}
}