using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using TomatoDoer.Model;
namespace TomatoDoer.Tests.Unit
{
	[TestFixture]
	public class TomatoSpanTests
	{
		[Test]
		public void Returns_Time_Remains()
		{
			DateTimeApp.Instance.FreezeTimeAt(DateTime.Now);
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

		[Test]
		public void ShouldInitializeCorrectly()
		{
			var span = new TomatoTimeSpan(TomatoTimeSpan.MediumTomatoSpan);

			//assert
			Assert.That(span.Duration, Is.EqualTo(TomatoTimeSpan.MediumTomatoSpan));
			Assert.That(span.EndTime, Is.Null);
			Assert.That(span.StartTime, Is.Null);
		}
	}
}
