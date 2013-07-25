using System;
using System.Text;
namespace TomatoDoer.Model
{
	public class TomatoLog : ITomatoLog
	{
		StringBuilder _Builder = new StringBuilder();
		public event Action Updated;
		private ILogFile _LogFile;
		private static TomatoLog _Instance;
		private bool _IsUpdateNotification_Suspended = false;
		private bool _IsLoadedFromDisk = false;
		public TomatoLog(ILogFile logFile)
		{
			_LogFile = logFile;
		}
		public static TomatoLog Instance
		{
			get { return _Instance ?? (_Instance = new TomatoLog(new WinLogFile())); }
		}
		public void Write(string testMessage)
		{
			DateTime logTime = DateTimeApp.Instance.Now;
			_Builder.AppendFormat(
				"{0} {1}: {2}",
				_Builder.Length > 0 ? Environment.NewLine : string.Empty,
				logTime.ToLocalTime().ToString(@"d MMM HH\:mm\:ss"),
				testMessage);
			OnUpdated();
		}
		public void OverwriteAll(string message)
		{
			_Builder.Clear();
			_Builder.Append(message);
			OnUpdated();
		}
		public string Text { 
			get
			{
//				if(!_IsLoadedFromDisk)
//				{
//					_IsLoadedFromDisk = true;
//					Load();
//				}
				return _Builder.ToString(); 
			} 
		}
		public void Flush()
		{
			_LogFile.WriteAllText(Text);
		}
		public void Load()
		{
			_Builder.Clear();
			_Builder.Append(_LogFile.ReadAllText());
			OnUpdated();
		}
		private  void OnUpdated()
		{
			if (Updated != null && !_IsUpdateNotification_Suspended) Updated();
		}
		public void SuspendUpdateNotification()
		{
			_IsUpdateNotification_Suspended = true;
		}
		public void ResumeUpdateNotification()
		{
			_IsUpdateNotification_Suspended = false;
		}
	}
}