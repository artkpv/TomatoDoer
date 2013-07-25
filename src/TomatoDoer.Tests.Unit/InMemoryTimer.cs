using System;
namespace TomatoDoer.Tests.Unit
{
	public class InMemoryTimer : ITimer
	{
		private bool _IsStarted = false;
		public event Action Tick;
		public bool IsStarted
		{
			get { return _IsStarted; }
		}
		public void Start()
		{
			_IsStarted = true;
		}
		public void Stop()
		{
			_IsStarted = false;
		}
		public void RaiseTickEvent()
		{
			if (Tick != null) Tick();
		}
	}
}