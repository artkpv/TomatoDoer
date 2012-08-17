using System.Windows.Forms;
using TomatoDoer.Model;
using Windows7.DesktopIntegration.WindowsForms;

namespace TomatoDoer
{
	public class TaskbarProgress
	{
		private readonly ITomatoTimer _Timer;
		private Form _MainForm;

		public TaskbarProgress(ITomatoTimer timer)
		{
			_Timer = timer;
			_Timer.Starting += new System.Action(_Timer_Starting);
			_Timer.TomatoDoneOrSquashed += new System.Action(_Timer_TomatoDoneOrSquashed);
			_Timer.Tick += new System.Action(_Timer_Tick);

			
		}

		Form GetMainForm()
		{
			if (_MainForm == null && Application.OpenForms.Count > 0)
				_MainForm = Application.OpenForms[0];
			return _MainForm;
		}

		void _Timer_Tick()
		{
			WindowsFormsExtensions.SetTaskbarProgress(GetMainForm(), (float)(100 * (1 - (_Timer.TomatoTimeRemains.TotalSeconds / _Timer.TomatoDuration.TotalSeconds))));
		}

		void _Timer_TomatoDoneOrSquashed()
		{
			WindowsFormsExtensions.SetTaskbarProgressState(GetMainForm(), Windows7.DesktopIntegration.Windows7Taskbar.ThumbnailProgressState.NoProgress);
		}

		void _Timer_Starting()
		{
			WindowsFormsExtensions.SetTaskbarProgressState(GetMainForm(), Windows7.DesktopIntegration.Windows7Taskbar.ThumbnailProgressState.Normal);
		}
	}
}