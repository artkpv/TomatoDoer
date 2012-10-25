using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhino.Mocks;
using TomatoDoer.Model;
namespace TomatoDoer.Tests.Unit
{
	[TestClass]
	public class TomatoTimerFormPresenterTests
	{
		private TomatoTimerFormPresenter _Presenter;
		private ITomatoLog _TomatoLogMock;
		private ITomatoTimerFormView _ViewMock;
		private ITomatoTimer _TomatoTimerMock;
		[TestInitialize]
		public void InitializeTest()
		{
			_TomatoTimerMock = MockRepository.GenerateMock<ITomatoTimer>();
			_TomatoLogMock = MockRepository.GenerateMock<ITomatoLog>();
			_ViewMock = MockRepository.GenerateMock<ITomatoTimerFormView>();
			_Presenter = new TomatoTimerFormPresenter(_ViewMock, _TomatoLogMock, _TomatoTimerMock);
		}
		[TestMethod]
		public void Presenter_Should_Bind_View()
		{
			var argumentsOfCalls = _ViewMock.GetArgumentsForCallsMadeOn(view => view.BindTomatoTimer(null, null));
			Assert.AreEqual(1, argumentsOfCalls.Count );
		}
		[TestMethod] 
		public void Presenter_Should_Update_Log_If_Log_Test_Changed()
		{
			string updatedLogText = "Updated log text";
			_ViewMock.Expect(v => v.LogText).Return(updatedLogText);
			_Presenter.TomatoLogChanged(null, null);
			var calls = _TomatoLogMock.GetArgumentsForCallsMadeOn(l => l.OverwriteAll(""));
			Assert.AreEqual(updatedLogText, calls[0][0]);
		}
		[TestMethod]
		public void Presenter_Should_Suspend_And_Resume_Log_Update_Notification_While_Updating_Log()
		{
			_TomatoLogMock.Expect(l => l.SuspendUpdateNotification());
			_TomatoLogMock.Expect(l => l.ResumeUpdateNotification());
			_Presenter.UpdateLog();
			
			_TomatoLogMock.VerifyAllExpectations();
		}
		[TestMethod]
		public void Presenter_Should_Flush_Log_If_Log_Text_Changed()
		{
			_TomatoLogMock.Expect(log => log.Flush());
			_Presenter.TomatoLogChanged(null, null);
			_TomatoLogMock.VerifyAllExpectations();
		}
		[TestMethod]
		public void Presenter_Should_Start_Tomato()
		{
			_TomatoTimerMock.Expect(timer => timer.StartTimer());
			_Presenter.StartOrSquashTomatoButtonClick(null, null);
			_TomatoTimerMock.VerifyAllExpectations();
		}
		[TestMethod]
		public void Presenter_Should_Squash_Tomato()
		{
			_TomatoTimerMock.StartTimer();
			_TomatoTimerMock.Expect(timer => timer.Squash());
			_TomatoTimerMock.Stub(timer => timer.IsStarted).Return(true);
			_Presenter.StartOrSquashTomatoButtonClick(null, null);
			_TomatoTimerMock.VerifyAllExpectations();
		}
		[TestMethod]
		public void Presenter_Should_Take_Tomato_Duration_Before_Starting_New_One()
		{
			var duration = new TimeSpan(0, 10, 0);
			_ViewMock.Stub(view => view.TomatoDuration).Return(duration);
			_TomatoTimerMock.Expect(timer => timer.StartTimer(duration));
			_Presenter.StartOrSquashTomatoButtonClick(null, null);
			_TomatoTimerMock.VerifyAllExpectations();
		}
		[TestMethod]
		public void Presenter_Should_Reset_Tomatoes()
		{
			_TomatoTimerMock.Expect(timer => timer.Reset());
			_Presenter.ResetButtonClick(null, null);
			_TomatoTimerMock.VerifyAllExpectations();
		}

		[TestMethod]
		public void Start_New_Tomato_Beginning_From_Last_By_No_Break_Button_After_5_Minutes()
		{
			//arrange
			_TomatoTimerMock.StartTimer(new TimeSpan(0, 25, 0));
			DateTimeApp.Instance.FreezeTimeAt( DateTime.Now.AddMinutes(30));
			_TomatoTimerMock.Expect(timer => timer.StartTimer(new TimeSpan(0, 20, 0)));
			//act
			_Presenter.StartNewTomatoWithoutBreak();
			//assert
			_TomatoTimerMock.VerifyAllExpectations();
		}

		[TestMethod]
		public void Start_New_Tomato_Beginning_From_Last_By_No_Break_Button_After_10_Minutes()
		{
			//arrange
			_TomatoTimerMock.StartTimer(new TimeSpan(0, 25, 0));
			DateTimeApp.Instance.FreezeTimeAt(DateTime.Now.AddMinutes(35));
			_TomatoTimerMock.Expect(timer => timer.StartTimer(new TimeSpan(0, 15, 0)));
			//act
			_Presenter.StartNewTomatoWithoutBreak();
			//assert
			_TomatoTimerMock.VerifyAllExpectations();
		}
		[TestMethod]
		public void Start_New_25Min_Tomato_Beginning_From_Last_By_No_Break_Button_After_30_Minutes()
		{
			
		}
		[TestMethod]
		public void Tomato_Started_Without_Break_Should_Be_Logged_Fully()
		{
		throw new NotImplementedException();	
		}
	}
}
