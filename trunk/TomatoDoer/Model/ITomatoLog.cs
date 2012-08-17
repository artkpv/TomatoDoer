using System;

namespace TomatoDoer.Model
{
	public interface ITomatoLog
	{
		event Action Updated;
		string Text { get; }
		void Write(string testMessage);
		void OverwriteAll(string message);
		void Flush();
		void Load();
		void SuspendUpdateNotification();
		void ResumeUpdateNotification();

	}
}