using System;
using System.Collections.Generic;
using System.Text;
using TomatoDoer.Model;
using TomatoDoer.Presentation;
using TomatoLogParser;
namespace TomatoDoer
{
	public class TomatoTimerFormPresenter
	{
		private readonly ITomatoTimerFormView _View;
		private readonly ITomatoLog _TomatoLog;
		private readonly ITomatoTimer _TomatoTimer;
		public TomatoTimerFormPresenter(ITomatoTimerFormView view, ITomatoLog tomatoLog, ITomatoTimer tomatoTimer)
		{
			_View = view;
			_TomatoLog = tomatoLog;
			_TomatoTimer = tomatoTimer;
			_View.BindTomatoTimer(_TomatoTimer, _TomatoLog);
		}
		public void TomatoLogChanged(object sender, EventArgs eventArgs)
		{
			UpdateLog();
		}
		public void UpdateLog()
		{
			_TomatoLog.SuspendUpdateNotification();
			_TomatoLog.OverwriteAll(_View.LogText);
			_TomatoLog.Flush();
			_TomatoLog.ResumeUpdateNotification();
		}
		public void StartOrSquashTomatoButtonClick(object sender, EventArgs eventArgs)
		{
			if (_TomatoTimer.IsStarted)
			{
				_TomatoTimer.Squash();
			}
			else
			{
				if (_View.TomatoDuration == null)
					_TomatoTimer.StartTimer();
				else
					_TomatoTimer.StartTimer(_View.TomatoDuration.Value);
			}
		}
		public void ResetButtonClick(object sender, EventArgs eventArgs)
		{
			_TomatoTimer.Reset();
			_View.ResetTitle();
		}
		private string GetAnalyze(string log)
		{
			var analyze = new StringBuilder();
			analyze.Append("Analysis of TomatoDoer log at ");
			analyze.Append(DateTimeApp.Instance.Now.ToString());
			analyze.AppendLine();
			var parser = new TomatoLogParser.TomatoLogParser();
			List<TomatoTimeSpanDescribed> spansParsed = null;
			try
			{
				spansParsed = parser.ParseTomatoesLog(log);
			}
			catch (Exception ex)
			{
				analyze.AppendLine("Failed to analyze: error occured.");
				analyze.AppendLine(ex.ToString());
			}
			LogAnalyzeView view = new LogAnalyzeView();
			analyze.AppendLine(view.GetView(spansParsed));
			return analyze.ToString();
		}
		public void CallLogAnalysis()
		{
			_View.LogText = GetAnalyze(_View.LogText);
		}
		public void StartNewTomatoWithoutBreak()
		{
//			TomatoTimeSpan? lastTomato = _TomatoTimer.LastTomatoDone;
//			if(lastTomato.HasValue && lastTomato.Value.State == ETomatoState.Ended)
//			{
//				TimeSpan timeLeft = DateTimeApp.Instance.Now - lastTomato.Value.EndTime.Value;
//				if(timeLeft > lastTomato.Value.Duration)
//					_TomatoTimer.
//				_TomatoTimer.StartTimer(new TimeSpan(0, 20, 0));
//			}
		}
	}
}