using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Moq;
using NUnit.Framework;
using TomatoDoer.Model;
namespace TomatoDoer.Tests.Unit
{
	[TestFixture]
	public class TomatoTimerFormPresenterTests
	{
		private TomatoTimerFormPresenter _Presenter;
		private Mock<ITomatoLog> _TomatoLogMock;
		private Mock<ITomatoTimerFormView> _ViewMock;
		private Mock<ITomatoTimer> _TomatoTimerMock;
		[SetUp]
		public void InitializeTest()
		{

			_TomatoTimerMock = new Mock<ITomatoTimer>();
			_TomatoLogMock = new Mock<ITomatoLog>();
			_ViewMock = new Mock<ITomatoTimerFormView>();
			_Presenter = new TomatoTimerFormPresenter(_ViewMock.Object, _TomatoLogMock.Object, _TomatoTimerMock.Object);
		}
		[Test]
		public void Presenter_Should_Bind_View()
		{
			_ViewMock.Verify(v=>v.BindTomatoTimer(It.IsAny<ITomatoTimer>(),It.IsAny<ITomatoLog>()));
		}
		[Test] 
		public void Presenter_Should_Update_Log_If_Log_Test_Changed()
		{
			string updatedLogText = "Updated log text";
			_ViewMock.Setup(v => v.LogText).Returns(updatedLogText);
			_TomatoLogMock.Setup(l => l.OverwriteAll(updatedLogText));

			//act
			_Presenter.TomatoLogChanged(null, null);
			
			//assert
			_TomatoLogMock.VerifyAll();
		}
		[Test]
		public void Presenter_Should_Suspend_And_Resume_Log_Update_Notification_While_Updating_Log()
		{
			_TomatoLogMock.Setup(l => l.SuspendUpdateNotification());
			_TomatoLogMock.Setup(l => l.ResumeUpdateNotification());

			//act
			_Presenter.UpdateLog();
			
			_TomatoLogMock.VerifyAll();
		}
		[Test]
		public void Presenter_Should_Flush_Log_If_Log_Text_Changed()
		{
			_TomatoLogMock.Setup(log => log.Flush());
			_Presenter.TomatoLogChanged(null, null);
			_TomatoLogMock.VerifyAll();
		}
		[Test]
		public void Presenter_Should_Start_Tomato()
		{
			_TomatoTimerMock.Setup(timer => timer.StartTimer());
			_Presenter.StartOrSquashTomatoButtonClick(null, null);
			_TomatoTimerMock.VerifyAll();
		}
		[Test]
		public void Presenter_Should_Squash_Tomato()
		{
			_TomatoTimerMock.Setup(timer => timer.Squash());
			_TomatoTimerMock.Object.StartTimer();
			_TomatoTimerMock.SetupGet(timer => timer.IsStarted).Returns(true);

			//act
			_Presenter.StartOrSquashTomatoButtonClick(null, null);
			
			//assert
			_TomatoTimerMock.VerifyAll();
		}
		[Test]
		public void Presenter_Should_Take_Tomato_Duration_Before_Starting_New_One()
		{
			var duration = new TimeSpan(0, 10, 0);
			_ViewMock.Setup(view => view.TomatoDuration).Returns(duration);
			_TomatoTimerMock.Setup(timer => timer.StartTimer(duration));

			//act
			_Presenter.StartOrSquashTomatoButtonClick(null, null);

			//assert
			_TomatoTimerMock.VerifyAll();
		}
		[Test]
		public void Presenter_Should_Reset_Tomatoes()
		{
			_TomatoTimerMock.Setup(timer => timer.Reset());
			_Presenter.ResetButtonClick(null, null);
			_TomatoTimerMock.VerifyAll();
		}

		[Test]
		public void Presenter_Should_Call_ContinueTomato_Of_Timer_With_Specific_Time()
		{
			var tomatoToBeContinuedTimeSpan = new TimeSpan(0, 0, 25);

			_Presenter.ContinueTomato(tomatoToBeContinuedTimeSpan);

			_TomatoTimerMock.Verify(tt=>tt.ContinueTomato(It.Is((TomatoTimeSpan tts)=>tts.Duration.Equals(tomatoToBeContinuedTimeSpan))));
		}

		
	}
}
