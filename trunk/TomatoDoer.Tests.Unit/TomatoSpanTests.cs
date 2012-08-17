using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TomatoDoer.Model;

namespace TomatoDoer.Tests.Unit
{
	[TestClass]
	public class TomatoSpanTests
	{

		[TestMethod]
		public void Returns_Time_Remains()
		{
			DateTimeApp.Now = DateTime.Now;
			//arrange
			var span = new TomatoTimeSpan()
			{
				Duration = new TimeSpan(0, 25, 0),
				StartTime = DateTime.Now.AddMinutes(-4).AddSeconds(-10)
			};

			//act
			int minutes = span.TimeRemains.Minutes;

			//assert
			Assert.AreEqual(20, minutes);
		}

	}
}
