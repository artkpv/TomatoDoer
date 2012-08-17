using System;

namespace TomatoDoer
{
	public interface ITimer
	{
		event Action Tick;
		bool IsStarted { get; }
		void Start();
		void Stop();
	}
}