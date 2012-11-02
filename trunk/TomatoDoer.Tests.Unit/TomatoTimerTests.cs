using System;
using System.Diagnostics;
using System.Text.RegularExpressions;
using Moq;
using NUnit.Framework;
using TomatoDoer.Model;
namespace TomatoDoer.Tests.Unit
{
	[TestFixture]
	public class TomatoTimerTests
	{
		private InMemoryTimer _TimerStub;
		private Mock<ITomatoLog> _TomatoLog;
		private TomatoTimer _TomatoTimer;

		[SetUp]
		public void InitializeTests()
		{
			//arrange
			_TimerStub = new InMemoryTimer();

			_TomatoLog = new Mock<ITomatoLog>();

			_TomatoTimer = new TomatoTimer(_TimerStub, _TomatoLog.Object);
		}

		[Test]
		public void Could_Be_Started()
		{
			//act 
			_TomatoTimer.StartTimer();

			//assert
			Assert.IsTrue(_TomatoTimer.IsStarted);
		}

		[Test]
		public void Could_Be_Squashed()
		{
			//act
			_TomatoTimer.StartTimer(new TimeSpan(0, 1, 0));
			_TomatoTimer.Squash();

			//assert
			Assert.IsFalse(_TomatoTimer.IsStarted);
		}

		[Test]
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

		[Test]
		[ExpectedException(typeof (TomatoException))]
		public void Could_Not_Start_Two_Tomatoes_At_The_Same_Time()
		{
			//act
			_TomatoTimer.StartTimer();
			_TomatoTimer.StartTimer();
		}

		[Test]
		[ExpectedException(typeof (TomatoException))]
		public void Could_Not_Squash_Ended_Tomato()
		{
			//act
			_TomatoTimer.StartTimer(new TimeSpan(0, 0, 0));
			_TimerStub.RaiseTickEvent();
			_TimerStub.RaiseTickEvent();
			_TomatoTimer.Squash();
		}

		[Test]
		public void Timer_Should_Display_Progress()
		{
			//act 
			_TomatoTimer.StartTimer(new TimeSpan(0, 1, 10));
			_TimerStub.RaiseTickEvent();

			//assert
			Assert.AreEqual(1, _TomatoTimer.TomatoTimeRemains.Minutes);
		}

		[Test]
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

		[Test]
		public void Inner_Timer_Could_Be_Started()
		{
			//arrange 
			_TimerStub.Stop();
			//act
			_TomatoTimer.StartTimer();
			//assert
			Assert.IsTrue(_TimerStub.IsStarted);
		}

		[Test]
		public void Inner_Timer_Stopped_If_Tomato_Squashed()
		{
			//act
			_TomatoTimer.StartTimer();
			_TomatoTimer.Squash();

			//assert
			Assert.IsFalse(_TimerStub.IsStarted);
		}

		[Test]
		public void Inner_Timer_Stopped_If_Reseting()
		{
			_TomatoTimer.StartTimer();
			_TomatoTimer.Reset();
			Assert.IsFalse(_TimerStub.IsStarted);
		}

		[Test]
		public void Should_Report_Started()
		{
			bool wasStarted = false;
			_TomatoTimer.Starting += () => wasStarted = true;
			_TomatoTimer.StartTimer();
			Assert.IsTrue(wasStarted);
		}

		[Test]
		public void Should_Report_Tick()
		{
			bool isTickDone = false;
			_TomatoTimer.Tick += () => isTickDone = true;
			_TomatoTimer.StartTimer();
			_TimerStub.RaiseTickEvent();
			Assert.IsTrue(isTickDone);
		}

		[Test]
		public void Should_Report_Ending_Somewhere()
		{
			bool wasTomatoDone = false;
			_TomatoTimer.TomatoDoneOrSquashed += () => wasTomatoDone = _TomatoTimer.State == ETomatoState.Ended;
			_TomatoTimer.StartTimer(new TimeSpan(0, 0, 0));
			_TimerStub.RaiseTickEvent();
			_TimerStub.RaiseTickEvent();
			Assert.IsTrue(wasTomatoDone);
		}

		[Test]
		public void Should_Report_Squashing()
		{
			bool wasTomatoSquashed = false;
			_TomatoTimer.TomatoDoneOrSquashed += () => wasTomatoSquashed = _TomatoTimer.State == ETomatoState.Squashed;
			_TomatoTimer.StartTimer(new TimeSpan(1, 0, 0));
			_TimerStub.RaiseTickEvent();
			_TomatoTimer.Squash();
			Assert.IsTrue(wasTomatoSquashed);
		}

		[Test]
		public void Should_Not_Tick_If_Null_Tomato()
		{
			bool hasTick = false;
			_TomatoTimer.Tick += () => hasTick = true;
			_TomatoTimer.TomatoDuration = new TimeSpan(0, 0, 0);
			_TomatoTimer.StartTimer();
			_TimerStub.RaiseTickEvent();
			Assert.IsFalse(hasTick);
		}

		[Test]
		public void Timer_Should_Log_Tomato_Done()
		{
			//act
			_TomatoTimer.StartTimer(new TimeSpan(0, 0, 0));
			_TimerStub.RaiseTickEvent();

			//assert
			_TomatoLog.Verify(l=>l.Write(It.IsRegex(@"Tomato \d+ was done \(\d\d\:\d\d\)\.")));
		}

		[Test]
		public void Timer_Should_Log_Tomato_Squashed()
		{
			_TomatoTimer.StartTimer(new TimeSpan(0, 10, 0));
			_TomatoTimer.Squash();
			_TomatoLog.Verify(l=>l.Write(It.IsRegex(@"Tomato was squashed\.")));
		}

		[Test]
		public void Timer_Should_Not_Increase_Number_Of_Done_Tomatoes_When_Reset()
		{
			int was = _TomatoTimer.CountTomatoesDone;
			_TomatoTimer.Reset();
			Assert.AreEqual(was, _TomatoTimer.CountTomatoesDone);
		}

		[Test]
		public void Timer_Should_Not_Write_To_Log_Twice_About_Ending_After_ENd_Tick_And_Reset()
		{
			_TomatoTimer.TomatoDuration = new TimeSpan(0, 0, 0);
			
			//act
			_TomatoTimer.StartTimer();
			_TimerStub.RaiseTickEvent();
			_TomatoTimer.Reset();

			_TomatoLog.Verify(l=>l.Write(It.IsAny<string>()), Times.Once());
		}

		[Test]
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

		[Test]
		public void History_Of_Spans_Updates_On_log_Updated()
		{
			_TomatoLog.SetupGet(l => l.Text).Returns(@"
TomatoDoer
 28 июл 22:43:14: Tomato 1 was done.
");
			//act
			_TomatoLog.Raise(t => t.Updated += null);

			Assert.AreEqual(1, _TomatoTimer.CountTomatoesDone);
		}

		[Test]
		public void History_Of_Spans_Updates_Spans_Times()
		{
			_TomatoLog.SetupGet(l => l.Text).Returns(@"
TomatoDoer
 28 июл 22:43:14: Tomato 1 was done (22:00).
 28 июл 23:43:14: Tomato 2 was done (10:00).
");
			_TomatoLog.Raise(t => t.Updated += null);

			Assert.AreEqual(new TimeSpan(0, 32, 0), _TomatoTimer.CountTotalTime());
		}

		[Test]
		public void Timer_Should_Not_Count_Tomato_Reset()
		{
			_TomatoTimer.StartTimer(new TimeSpan(0, 0, 1));
			_TomatoTimer.Squash();
			DateTimeApp.Instance.FreezeTimeAt( DateTime.Now.AddMinutes(1));
			Assert.AreEqual(0, _TomatoTimer.CountTomatoesDone);
		}

		[Test]
		public void Timer_Counts_Total_Time_Duration()
		{
			var defaultTomatoDuration = new TimeSpan(0, 25, 0);

			_TomatoTimer.StartTimer(new TimeSpan(0, 0, 0));
			_TimerStub.RaiseTickEvent();
			_TomatoTimer.StartTimer(defaultTomatoDuration);
			_TimerStub.RaiseTickEvent();
			DateTimeApp.Instance.FreezeTimeAt(DateTime.Now.AddMinutes(40));
			_TimerStub.RaiseTickEvent();

			Assert.AreEqual(defaultTomatoDuration, _TomatoTimer.CountTotalTime());
		}

		[Test]
		public void Timer_Dont_Spend_Time_When_No_Tomato_Done()
		{
			Assert.AreEqual(new TimeSpan(), _TomatoTimer.CountTotalTime());
		}

		[Test]
		public void Timer_Returns_Last_Tomato_Done()
		{
			TimeSpan lastTomatoDuration = new TimeSpan(0, 12, 0);

			//act
			_TomatoTimer.StartTimer(new TimeSpan(0, 25, 0));
			_TimerStub.RaiseTickEvent();
			DateTimeApp.Instance.FreezeTimeAt(DateTime.Now.AddMinutes(26));
			_TimerStub.RaiseTickEvent();
			_TomatoTimer.StartTimer(lastTomatoDuration);
			_TimerStub.RaiseTickEvent();
			DateTimeApp.Instance.FreezeTimeAt( DateTime.Now.AddMinutes(13));
			_TimerStub.RaiseTickEvent();

			//assert
			Assert.IsNotNull(_TomatoTimer.LastTomatoDone);
			Assert.AreEqual( lastTomatoDuration , _TomatoTimer.LastTomatoDone.Value.Duration);
		}
		
		[Test]
		public void Timer_Should_Continue_A_Tomato()
		{
			
			DateTimeApp.Instance.FreezeTimeAt(DateTimeApp.Instance.Now);
			TimeSpan tomatoDuration	= new TimeSpan(0, 0, 25);
			TomatoTimeSpan tomatoToContinue = new TomatoTimeSpan()
			{
				Duration = tomatoDuration,
				StartTime = DateTimeApp.Instance.Now.AddMinutes(-5)
			};

			//act
			_TomatoTimer.ContinueTomato(tomatoToContinue);
			_TimerStub.RaiseTickEvent();
			DateTimeApp.Instance.FreezeTimeAt(DateTimeApp.Instance.Now.AddMinutes(21));
			_TimerStub.RaiseTickEvent();
			
			//assert
			var lastTomato = _TomatoTimer.LastTomatoDone;
			Assert.IsNotNull(lastTomato);
			Assert.AreEqual(lastTomato.Value.State, ETomatoState.Ended);

			Assert.AreEqual(lastTomato.Value.Duration, tomatoDuration);

		}

		[Test]
		[ExpectedException(typeof (TomatoException))]
		public void Could_Not_Continue_Two_Tomatoes_At_The_Same_Time()
		{
			_TomatoTimer.ContinueTomato(
				new TomatoTimeSpan(TomatoTimeSpan.MediumTomatoSpan)
				{
					StartTime = DateTimeApp.Instance.FreezeTimeAt(DateTime.Now.AddMinutes(-1))
				});
			_TomatoTimer.ContinueTomato(new TomatoTimeSpan(TomatoTimeSpan.MediumTomatoSpan));
		}

	}
}