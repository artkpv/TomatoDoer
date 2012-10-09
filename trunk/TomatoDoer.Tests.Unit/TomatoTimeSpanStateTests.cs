using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TomatoDoer.Model;
namespace TomatoDoer.Tests.Unit
{
	[TestClass]
	public class TomatoTimeSpanStateTests
	{
		private ETomatoState GetTomatoState(TimeSpan duration, DateTime? start, DateTime? end)
		{
			return GetTomatoState(duration, start, end, DateTime.Now);
		}
		private ETomatoState GetTomatoState(TimeSpan duration, DateTime? start, DateTime? end, DateTime nowTimePoint)
		{
			var span = new TomatoTimeSpan()
			           	{
			           		Duration = duration,
			           		StartTime = start,
			           		EndTime = end
			           	};
			return span.GetState(nowTimePoint);
		}
		[TestMethod]
		public void Returns_Ready_If_Not_Started()
		{
			Assert.AreEqual(ETomatoState.Ready, GetTomatoState(new TimeSpan(), null, null));
		}
		[TestMethod]
		public void Returns_Started_If_Started()
		{
			Assert.AreEqual(ETomatoState.Started, GetTomatoState(new TimeSpan(0, 25, 0), DateTime.Now, null));
		}
		[TestMethod]
		public void Returns_Ended_If_Ended()
		{
			Assert.AreEqual(ETomatoState.Ended, GetTomatoState(new TimeSpan(0, 25, 0), DateTime.Now.AddMinutes(-26), null));
			
		}
		[TestMethod]
		public void Returns_Squashed_If_Squashed_While_Started()
		{
			Assert.AreEqual(ETomatoState.Squashed, GetTomatoState(new TimeSpan(0, 50, 0), DateTime.Now.AddMinutes(-26), DateTime.Now));
		}
		[TestMethod]
		public void Returns_Ended_If_Squashed_After_Started()
		{
			Assert.AreEqual(ETomatoState.Ended, GetTomatoState(new TimeSpan(0, 10, 0), DateTime.Now.AddMinutes(-26), DateTime.Now));
		}
		[TestMethod]
		public void Returns_Started_If_Squashed_Before_Started_And_Not_Ended()
		{
			Assert.AreEqual(ETomatoState.Started, GetTomatoState(new TimeSpan(0, 20, 0), DateTime.Now.AddMinutes(-10), DateTime.Now.AddMinutes(-50)));
		}
		[TestMethod]
		public void Returns_Ended_If_Squashed_Before_Started_And_Ended()
		{
			Assert.AreEqual(ETomatoState.Ended, GetTomatoState(new TimeSpan(0, 20, 0), DateTime.Now.AddMinutes(-30), DateTime.Now.AddMinutes(-50)));
		}
		[TestMethod]
		public void Returns_Started_If_Negative_Duration_And_Started()
		{
			Assert.AreEqual(ETomatoState.Started, GetTomatoState(-new TimeSpan(0, 20, 0), DateTime.Now.AddMinutes(-10), DateTime.Now.AddMinutes(-50)));
		}
		[TestMethod]
		public void Returns_Ready_If_It_Is_In_Future_And_Started()
		{
			Assert.AreEqual(ETomatoState.Ready, GetTomatoState(-new TimeSpan(0, 20, 0), DateTime.Now.AddMinutes(-10), DateTime.Now.AddMinutes(-50), DateTime.Now.AddMinutes(-100)));
		}
		[TestMethod]
		public void Returns_Ended_If_Started_And_Duration_Is_Zero()
		{
			DateTime nowTimePoint = DateTime.Now;
			Assert.AreEqual(ETomatoState.Ended, GetTomatoState(new TimeSpan(0,0,0), nowTimePoint, nowTimePoint.AddHours(1), nowTimePoint ));
		}
		[TestMethod]
		public void Returns_Squashed_If_Start_And_End_Time_Equal_And_Duration_1_Sec()
		{
			DateTime now = DateTime.Now;
			Assert.AreEqual(ETomatoState.Squashed, GetTomatoState(new TimeSpan(0,0,1), now, now));
		}
	}
}
