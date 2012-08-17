using System;

namespace TomatoDoer.Model
{
	public interface ITomatoTimer
	{
		event Action TomatoDoneOrSquashed;
		event Action Starting;
		event Action Tick;
		event Action TomatoHistoryWasRewritten;
		ITomatoLog TomatoLog { get; set; }
		TimeSpan TomatoDuration { get; set; }
		int CountTomatoesDone { get; }
		TimeSpan CountTotalTime();
		bool IsStarted { get; }
		TimeSpan TomatoTimeRemains { get; }
		ETomatoState State { get; }
		void StartTimer();
		void StartTimer(TimeSpan tomatoDuration);
		void Squash();
		void Reset();
	}
}