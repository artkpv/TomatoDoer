using System;
using System.Collections.Generic;

namespace TomatoLogParser
{
	public class WorkDay
	{
		public List<Tomato> Tomatoes;
		public int Logevity
		{
			get
			{
				if (Tomatoes.Count == 0)
					return 0;
				var firstTomatoStartTime = Tomatoes[0].Time.AddMinutes(-25);
				var lastTomatoEndTime = Tomatoes[Tomatoes.Count - 1].Time;
				return (int)Math.Round((lastTomatoEndTime - firstTomatoStartTime).TotalHours);
			}
		}

		public DateTime? DateOfDay
		{
			get
			{
				if (Tomatoes.Count > 0)
					return Tomatoes[0].Time.Date;
				return null;
			}
		}
	}
}