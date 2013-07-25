using System;
using TomatoDoer.Model;
namespace TomatoDoer
{
	public interface ITomatoTimerFormView
	{
		TimeSpan? TomatoDuration { get; set; }
		string LogText{ get; set; }
		void BindTomatoTimer(ITomatoTimer timer, ITomatoLog log);
		void ResetTitle();
		void CallAnalysisOfLog(object sender, EventArgs e);
	}
}