using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
namespace TomatoDoer.Model
{
	/// <summary>
	/// Represents tomato timer which counts tomatoes.
	/// </summary>
	public class TomatoTimer : ITomatoTimer
	{
		private List<TomatoTimeSpan> _TomatoHistory = new List<TomatoTimeSpan>();
		private ITimer _Timer;
		private TomatoTimeSpan _TomatoTimeSpan;
		private static TomatoTimer _Instance = null;
		public event Action TomatoDoneOrSquashed;
		public event Action Starting;
		public event Action Tick;
		public event Action TomatoHistoryWasRewritten; 
		public ITomatoLog TomatoLog { get; set; }
		public TomatoTimer(ITimer timer, ITomatoLog tomatoLog)
		{
			TomatoLog = tomatoLog;
			TomatoDuration = TomatoTimeSpan.MediumTomatoSpan;
			_Timer = timer;
			_Timer.Tick += new Action(TimerTick);
			TomatoLog.Updated += new Action(TomatoLog_Updated);
		}
		public void TomatoLog_Updated()
		{
			var parser = new TomatoLogParser.TomatoLogParser();
			var parsedLog = parser.ParseTomatoesLog(TomatoLog.Text);
			_TomatoHistory.Clear();
			_TomatoHistory = (from describedSpan in parsedLog
			                  select describedSpan.TomatoTimeSpan).ToList();
			
			if (TomatoHistoryWasRewritten != null) TomatoHistoryWasRewritten();
		}
		public TomatoTimer(ITimer timer) : this(timer, null)
		{
			
		}
		public TimeSpan TomatoDuration { get; set; }
		public static TomatoTimer Instance
		{
			get { return _Instance ?? (GetInitializedTimer()); }
		}
		private static TomatoTimer GetInitializedTimer()
		{
			_Instance = new TomatoTimer(new WinTimer(), Model.TomatoLog.Instance);
			return _Instance;
		}
		public int CountTomatoesDone
		{
			get { return _TomatoHistory.Count(tomato => tomato.State == ETomatoState.Ended); }
		}
		public bool IsStarted
		{
			get { return _TomatoTimeSpan.State == ETomatoState.Started; }
		}
		public TimeSpan TomatoTimeRemains
		{
			get { return _TomatoTimeSpan.TimeRemains; }
		}
		public ETomatoState State
		{
			get { return  _TomatoTimeSpan.State; }
		}
		private void TimerTick()
		{
			if (_TomatoTimeSpan.State != ETomatoState.Started) StopTimer();
			else if (Tick != null) Tick();
		}
		public void StartTimer() {  StartTimer(TomatoDuration); }
		public void StartTimer(TimeSpan tomatoDuration)
		{
			if (IsStarted)
				throw new TomatoException("Can't start new tomato. There is a started tomato already.");
			InitializeNewTomatoTimeSpan(tomatoDuration);
			TomatoDuration = tomatoDuration;
			_Timer.Start();
			OnStarting();
		}
		private void InitializeNewTomatoTimeSpan(TimeSpan tomatoDuration)
		{
			_TomatoTimeSpan.Duration = tomatoDuration;
			_TomatoTimeSpan.StartTime = DateTimeApp.Now;
			_TomatoTimeSpan.EndTime = null;
		}
		public void Squash()
		{
			if(_TomatoTimeSpan.State != ETomatoState.Started)
				throw new TomatoException("Can't squash tomato. It isn't started.");
			StopTimer();
		}
		public void Reset()
		{
			if(_Timer.IsStarted)	
				StopTimer();
			_TomatoHistory.Clear();
		}
		private void StopTimer()
		{
			_TomatoTimeSpan.EndTime = DateTimeApp.Now;
			_TomatoHistory.Add(_TomatoTimeSpan);
			OnTomatoDoneOrSquashed();
			if (_Timer.IsStarted) _Timer.Stop();
		}
		private void OnTomatoDoneOrSquashed()
		{
			if (TomatoLog != null)
			{
				if (_TomatoTimeSpan.State == ETomatoState.Squashed)
					TomatoLog.Write("Tomato was squashed.");
				
				if (_TomatoTimeSpan.State == ETomatoState.Ended)
					TomatoLog.Write(string.Format("Tomato {0} was done ({1:00}:{2:00}).", CountTomatoesDone, TomatoDuration.Minutes, TomatoDuration.Seconds));
			}
			
			if (TomatoDoneOrSquashed != null) TomatoDoneOrSquashed();
		}
		
		private void OnStarting()
		{
			if (Starting != null) Starting();
		}
		public TimeSpan CountTotalTime()
		{
			TimeSpan total = new TimeSpan();
			foreach (var spans in _TomatoHistory.Where(s => s.State == ETomatoState.Ended))
				total += spans.Duration;
			return total;
		}
	}
}