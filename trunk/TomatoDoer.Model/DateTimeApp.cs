using System;
namespace TomatoDoer.Model
{
	public class DateTimeApp
	{
		private DateTime? _FreezedTime = null;
		private static DateTimeApp _Instance;

		public DateTime Now
		{
			get { return _FreezedTime ?? DateTime.Now; }
		}
		
		public DateTime FreezeTimeAt(DateTime dateTimeToFreezeAt)
		{
			return (_FreezedTime = dateTimeToFreezeAt).Value;
		}

		public static DateTimeApp Instance
		{
			get { return _Instance ?? (_Instance = new DateTimeApp()); }
		}

	}
}