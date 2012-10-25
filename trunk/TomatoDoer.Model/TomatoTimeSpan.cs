using System;
using System.Globalization;
namespace TomatoDoer.Model
{
	/// <summary>
	/// Represents tomato time span and its state
	/// </summary>
	public struct TomatoTimeSpan
	{
		public static TimeSpan MediumTomatoSpan = new TimeSpan(0, 25, 0);
		public static TimeSpan BigTomatoSpan = new TimeSpan(0, 50, 0);
		public static TimeSpan SmallTomatoSpan = new TimeSpan(0, 12, 0);
		public TimeSpan Duration;
		public DateTime? StartTime;
		public DateTime? EndTime;
		
		public TimeSpan TimeRemains
		{
			get { return GetTimeRemains(DateTimeApp.Instance.Now); }
		}
		public TomatoTimeSpan(TimeSpan duration, DateTime? endTime = null) : this()
		{
			Duration = duration;
			EndTime = endTime;
			StartTime = endTime - duration;
		}
		public TimeSpan GetTimeRemains(DateTime nowTimePoint)
		{
			var state = GetState(nowTimePoint);
			switch (state)
			{
				case ETomatoState.Ended :
					return TimeSpan.Zero;
				case ETomatoState.Ready:
					return Duration.Duration();
				case ETomatoState.Squashed:
					return Duration.Duration() - (EndTime.Value - StartTime.Value);
				case ETomatoState.Started:
					return Duration.Duration() - (nowTimePoint - StartTime.Value);
				default :
					throw new NotImplementedException();
			}
		}
		public ETomatoState State
		{
			get { return GetState(DateTimeApp.Instance.Now); }
		}
		/// <summary>
		/// Gets state of tomato on its time span for specified time point
		/// </summary>
		/// <param name="nowTimePoint"></param>
		/// <returns></returns>
		public ETomatoState GetState(DateTime nowTimePoint)
		{
			if (StartTime == null || StartTime.Value > nowTimePoint) return ETomatoState.Ready;
			TimeSpan elapsedTillNow = nowTimePoint - StartTime.Value;
			TimeSpan elapsedTillEnd = (EndTime.HasValue && EndTime >= StartTime ? EndTime.Value - StartTime.Value : TimeSpan.MaxValue);
			TimeSpan elapsed = elapsedTillEnd < elapsedTillNow ? elapsedTillEnd : elapsedTillNow;
			if (elapsed < Duration.Duration())
				if (EndTime == null || EndTime < StartTime) return ETomatoState.Started;
				else return ETomatoState.Squashed;
			else return ETomatoState.Ended;
		}
		public override string ToString()
		{
			return string.Format("TomatoTimeSpan of ({0}). Started: {1}. Ended: {2}.",
			                     Duration,
			                     (StartTime.HasValue ? StartTime.Value.ToString(CultureInfo.InvariantCulture) : "n/a"),
			                     (EndTime.HasValue ? EndTime.Value.ToString(CultureInfo.InvariantCulture) : "n/a")
								 );
		}

	}
}