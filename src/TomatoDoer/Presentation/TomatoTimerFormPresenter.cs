using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using DiffuseDlgDemo;
using Liensberger;
using TomatoDoer.Model;
using TomatoDoer.Presentation;
using TomatoLogParser;
namespace TomatoDoer
{
	public class TomatoTimerFormPresenter
	{
        private KeyboardHook _Hook = new KeyboardHook();
		private readonly ITomatoTimerFormView _View;
		private readonly ITomatoLog _TomatoLog;
		private readonly ITomatoTimer _TomatoTimer;
		public TomatoTimerFormPresenter(ITomatoTimerFormView view, ITomatoLog tomatoLog, ITomatoTimer tomatoTimer)
		{
			_View = view;
			_TomatoLog = tomatoLog;
			_TomatoTimer = tomatoTimer;
			_View.BindTomatoTimer(_TomatoTimer, _TomatoLog);
		    InstallHooks();

		}

	    private void InstallHooks()
	    {
            _Hook.KeyPressed+=_Hook_KeyPressed;

	        string keyboardShortcutStartOrSquash = Properties.Settings.Default.KeyboardShortcut_StartOrSquash;
            if (!string.IsNullOrWhiteSpace(keyboardShortcutStartOrSquash))
            {
                var split = keyboardShortcutStartOrSquash.Split('+');
                IEnumerable<string> modifierKeysMatches = split.Where(k => Enum.GetNames(typeof (KeyboardHook.ModifierKeys)).Contains(k, StringComparer.InvariantCultureIgnoreCase));

                var modifierKeys = modifierKeysMatches
                     .Select(s => (KeyboardHook.ModifierKeys)Enum.Parse(typeof (KeyboardHook.ModifierKeys), s));

                var keys = split.Except(modifierKeysMatches).Where(
                    k =>
                    Enum.GetNames(typeof (Keys))
                        .Contains(k, StringComparer.InvariantCultureIgnoreCase))
                     .Select(s => (Keys)Enum.Parse(typeof (Keys), s));

                if(modifierKeys.Any() || keys.Any())
                    _Hook.RegisterHotKey( modifierKeys.Aggregate(KeyboardHook.ModifierKeys.None, (keys1, u) => keys1 |u),
                        keys.Aggregate(Keys.None,(keys1, i) => keys1 | i) );
            }
	    }

	    private void _Hook_KeyPressed(object sender, KeyboardHook.KeyPressedEventArgs e)
	    {
            StartOrSquashTomatoButtonClick(this,EventArgs.Empty);
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
		public void ContinueTomato(TimeSpan tomatoToBeContinuedTimeSpan)
		{
			_TomatoTimer.ContinueTomato(tomatoToBeContinuedTimeSpan);
		}
	}
}