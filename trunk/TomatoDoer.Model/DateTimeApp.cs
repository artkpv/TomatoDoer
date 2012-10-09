using System;
namespace TomatoDoer.Model
{
	public static class DateTimeApp
	{
		private static DateTime? _Now = null; 
		public static DateTime Now
		{
			get { return _Now ?? DateTime.Now; }
			set { _Now = value; }
		}

	}
}