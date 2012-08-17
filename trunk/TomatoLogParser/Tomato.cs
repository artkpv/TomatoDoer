using System;

namespace TomatoLogParser
{
	public struct Tomato
	{
		public bool IsDone;
		public DateTime Time;
		public int TimeOfDay
		{
			get { return Time.TimeOfDay.Hours; }
		}

		public string Task;
		public int TomatoNumber;
	}
}