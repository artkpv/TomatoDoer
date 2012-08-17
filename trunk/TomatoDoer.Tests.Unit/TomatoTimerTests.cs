using System;
using System.Text.RegularExpressions;
using System.Threading;
using Rhino.Mocks;
using System.Text;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TomatoDoer.Model;

namespace TomatoDoer.Tests.Unit
{
	[TestClass]
	public class TomatoTimerTests
	{
		private InMemoryTimer _TimerStub;
		private ITomatoLog _TomatoLog;
		private TomatoTimer _TomatoTimer;

		[TestInitialize]
		public void InitializeTests()
		{
			//arrange
			_TimerStub = new InMemoryTimer();

			_TomatoLog = MockRepository.GenerateMock<ITomatoLog>();

			_TomatoTimer = new TomatoTimer(_TimerStub, _TomatoLog);
		}

		[TestMethod]
		public void Could_Be_Started()
		{
			//act 
			_TomatoTimer.StartTimer();

			//assert
			Assert.IsTrue(_TomatoTimer.IsStarted);
		}

		[TestMethod]
		public void Could_Be_Squashed()
		{
			//act
			_TomatoTimer.StartTimer(new TimeSpan(0, 1, 0));
			_TomatoTimer.Squash();

			//assert
			Assert.IsFalse(_TomatoTimer.IsStarted);
		}


		[TestMethod]
		public void Could_Be_Reset()
		{
			//act 
			_TomatoTimer.TomatoDuration = new TimeSpan(0, 0, 0);
			_TomatoTimer.StartTimer();
			_TomatoTimer.StartTimer();
			_TomatoTimer.StartTimer();
			_TomatoTimer.StartTimer(new TimeSpan(0, 1, 1));
			_TomatoTimer.Squash();
			_TomatoTimer.Reset();

			//assert
			Assert.AreEqual(0, _TomatoTimer.CountTomatoesDone);
		}

		[TestMethod]
		[ExpectedException(typeof (TomatoException))]
		public void Could_Not_Start_Two_Tomatoes_At_The_Same_Time()
		{
			//act
			_TomatoTimer.StartTimer();
			_TomatoTimer.StartTimer();
		}

		[TestMethod]
		[ExpectedException(typeof (TomatoException))]
		public void Could_Not_Squash_Ended_Tomato()
		{
			//act
			_TomatoTimer.StartTimer(new TimeSpan(0, 0, 0));
			_TimerStub.RaiseTickEvent();
			_TimerStub.RaiseTickEvent();
			_TomatoTimer.Squash();
		}

		[TestMethod]
		public void Timer_Should_Display_Progress()
		{
			//act 
			_TomatoTimer.StartTimer(new TimeSpan(0, 1, 10));
			_TimerStub.RaiseTickEvent();

			//assert
			Assert.AreEqual(1, _TomatoTimer.TomatoTimeRemains.Minutes);
		}

		[TestMethod]
		public void Should_Return_Number_Of_Done_Tomatoes()
		{
			_TomatoTimer.TomatoDuration = new TimeSpan(0, 0, 0);
			_TomatoTimer.StartTimer();
			_TimerStub.RaiseTickEvent();
			_TomatoTimer.StartTimer();
			_TimerStub.RaiseTickEvent();
			_TomatoTimer.StartTimer();
			_TimerStub.RaiseTickEvent();
			_TomatoTimer.TomatoDuration = new TimeSpan(0, 1, 1);
			_TomatoTimer.StartTimer();
			_TomatoTimer.Squash();
			Assert.AreEqual(3, _TomatoTimer.CountTomatoesDone);
		}

		[TestMethod]
		public void Inner_Timer_Could_Be_Started()
		{
			//arrange 
			_TimerStub.Stop();
			//act
			_TomatoTimer.StartTimer();
			//assert
			Assert.IsTrue(_TimerStub.IsStarted);
		}

		[TestMethod]
		public void Inner_Timer_Stopped_If_Tomato_Squashed()
		{
			//act
			_TomatoTimer.StartTimer();
			_TomatoTimer.Squash();

			//assert
			Assert.IsFalse(_TimerStub.IsStarted);
		}

		[TestMethod]
		public void Inner_Timer_Stopped_If_Reseting()
		{
			_TomatoTimer.StartTimer();
			_TomatoTimer.Reset();
			Assert.IsFalse(_TimerStub.IsStarted);
		}

		[TestMethod]
		public void Should_Report_Started()
		{
			bool wasStarted = false;
			_TomatoTimer.Starting += () => wasStarted = true;
			_TomatoTimer.StartTimer();
			Assert.IsTrue(wasStarted);
		}

		[TestMethod]
		public void Should_Report_Tick()
		{
			bool isTickDone = false;
			_TomatoTimer.Tick += () => isTickDone = true;
			_TomatoTimer.StartTimer();
			_TimerStub.RaiseTickEvent();
			Assert.IsTrue(isTickDone);
		}

		[TestMethod]
		public void Should_Report_Ending_Somewhere()
		{
			bool wasTomatoDone = false;
			_TomatoTimer.TomatoDoneOrSquashed += () => wasTomatoDone = _TomatoTimer.State == ETomatoState.Ended;
			_TomatoTimer.StartTimer(new TimeSpan(0, 0, 0));
			_TimerStub.RaiseTickEvent();
			_TimerStub.RaiseTickEvent();
			Assert.IsTrue(wasTomatoDone);
		}

		[TestMethod]
		public void Should_Report_Squashing()
		{
			bool wasTomatoSquashed = false;
			_TomatoTimer.TomatoDoneOrSquashed += () => wasTomatoSquashed = _TomatoTimer.State == ETomatoState.Squashed;
			_TomatoTimer.StartTimer(new TimeSpan(1, 0, 0));
			_TimerStub.RaiseTickEvent();
			_TomatoTimer.Squash();
			Assert.IsTrue(wasTomatoSquashed);
		}

		[TestMethod]
		public void Should_Not_Tick_If_Null_Tomato()
		{
			bool hasTick = false;
			_TomatoTimer.Tick += () => hasTick = true;
			_TomatoTimer.TomatoDuration = new TimeSpan(0, 0, 0);
			_TomatoTimer.StartTimer();
			_TimerStub.RaiseTickEvent();
			Assert.IsFalse(hasTick);
		}

		[TestMethod]
		public void Timer_Should_Log_Tomato_Done()
		{
			_TomatoTimer.StartTimer(new TimeSpan(0, 0, 0));
			_TimerStub.RaiseTickEvent();
			var arguments = _TomatoLog.GetArgumentsForCallsMadeOn(log => log.Write(null));
			Assert.AreEqual(1, arguments.Count);
			Assert.AreEqual(1, arguments[0].Length);
			StringAssert.Matches((string) arguments[0][0], new Regex(@"Tomato \d+ was done \(\d\d\:\d\d\)\."));
		}

		[TestMethod]
		public void Timer_Should_Log_Tomato_Squashed()
		{
			_TomatoTimer.StartTimer(new TimeSpan(0, 10, 0));
			_TomatoTimer.Squash();
			var arguments = _TomatoLog.GetArgumentsForCallsMadeOn(log => log.Write(null));
			Assert.AreEqual(1, arguments.Count);
			Assert.AreEqual(1, arguments[0].Length);
			StringAssert.Matches((string) arguments[0][0], new Regex(@"Tomato was squashed\."));
		}

		[TestMethod]
		public void Timer_Should_Not_Increase_Number_Of_Done_Tomatoes_When_Reset()
		{
			var was = _TomatoTimer.CountTomatoesDone;
			_TomatoTimer.Reset();
			Assert.AreEqual(was, _TomatoTimer.CountTomatoesDone);
		}

		[TestMethod]
		public void Timer_Should_Not_Write_To_Log_Twice_About_Ending_After_ENd_Tick_And_Reset()
		{
			_TomatoTimer.TomatoDuration = new TimeSpan(0, 0, 0);
			_TomatoTimer.StartTimer();
			_TimerStub.RaiseTickEvent();
			_TomatoTimer.Reset();

			var callsOfWrite = _TomatoLog.GetArgumentsForCallsMadeOn(l => l.Write(null));

			Assert.AreEqual(1, callsOfWrite.Count);
		}

		[TestMethod]
		public void Timer_Ended_Duration_Remains_The_Last_Specified()
		{
			var tomatoDuration = new TimeSpan(0, 0, 0, 0);
			_TomatoTimer.TomatoDuration = tomatoDuration;
			_TomatoTimer.StartTimer();
			tomatoDuration = new TimeSpan(0, 0, 30);
			_TomatoTimer.StartTimer(tomatoDuration);
			_TomatoTimer.Squash();
			Assert.AreEqual(tomatoDuration, _TomatoTimer.TomatoDuration);
		}

		[TestMethod]
		public void History_Of_Spans_Updates_On_log_Updated()
		{
			_TomatoLog.Stub(t => t.Text).Return(
				@"
TomatoDoer
 28 июл 22:43:14: Tomato 1 was done.
");
			_TomatoLog.Raise(t => t.Updated += null);

			Assert.AreEqual(1, _TomatoTimer.CountTomatoesDone);

		}

		[TestMethod]
		public void History_Of_Spans_Updates_Spans_Times()
		{
			_TomatoLog.Stub(t => t.Text).Return(
				@"
TomatoDoer
 28 июл 22:43:14: Tomato 1 was done (22:00).
 28 июл 23:43:14: Tomato 2 was done (10:00).
");
			_TomatoLog.Raise(t => t.Updated += null);

			Assert.AreEqual(new TimeSpan(0, 32, 0), _TomatoTimer.CountTotalTime());

		}

		[TestMethod]
		public void Timer_Should_Not_Count_Tomato_Reset()
		{
			_TomatoTimer.StartTimer(new TimeSpan(0, 0, 1));
			_TomatoTimer.Squash();
			DateTimeApp.Now = DateTime.Now.AddMinutes(1);
			Assert.AreEqual(0, _TomatoTimer.CountTomatoesDone);
		}

		[TestMethod]
		public void Timer_Counts_Total_Time_Duration()
		{
			var defaultTomatoDuration = new TimeSpan(0, 25, 0);

			_TomatoTimer.StartTimer(new TimeSpan(0, 0, 0));
			_TimerStub.RaiseTickEvent();
			_TomatoTimer.StartTimer(defaultTomatoDuration);
			_TimerStub.RaiseTickEvent();
			DateTimeApp.Now = DateTime.Now.AddMinutes(40);
			_TimerStub.RaiseTickEvent();


			Assert.AreEqual(defaultTomatoDuration, _TomatoTimer.CountTotalTime());
		}

		[TestMethod]
		public void Timer_Dont_Spend_Time_When_No_Tomato_Done()
		{
			Assert.AreEqual(new TimeSpan(), _TomatoTimer.CountTotalTime());
		}
	
	}
}
