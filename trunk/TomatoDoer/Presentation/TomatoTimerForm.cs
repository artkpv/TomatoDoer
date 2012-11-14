using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Media;
using System.Windows.Forms;
using System.IO;
using TomatoDoer.Model;
using TomatoDoer.Presentation;
using TomatoDoer.Properties;
namespace TomatoDoer
{
	public partial class TomatoTimerForm : Form, ITomatoTimerFormView
	{
		private TomatoTimerFormPresenter _Presenter;
		private ITomatoTimer _Timer = null;
		private ITomatoLog _Log;
		public TomatoTimerForm()
		{
			InitializeComponent();
			maskedTextBox1.Validating += TomatoDurationValidated;
			_Presenter = new TomatoTimerFormPresenter(this, TomatoLog.Instance, TomatoTimer.Instance);
			richTextBoxLog.TextChanged += _Presenter.TomatoLogChanged;
			StartOrSquashButton.Click += _Presenter.StartOrSquashTomatoButtonClick;
		}
		public void ResetTitle()
		{
			
			Text = "Tomato doer " + (_Timer!= null 
				? string.Format("({0} in {1})", _Timer.CountTomatoesDone, _Timer.CountTotalTime()) 
				: "");
		}
		public void BindTomatoTimer (ITomatoTimer timer, ITomatoLog log)
		{
			_Timer = timer;
			timer.TomatoHistoryWasRewritten += new Action(timer_TomatoHistoryWasRewritten);
			timer.Tick += new Action(timer_Tick);
			timer.Starting += new Action(timer_Starting);
			timer.TomatoDoneOrSquashed += new Action(timer_TomatoDone);
			//timer.TomatoDoneOrSquashed += new Action(timer_TomatoDone);
			TomatoDuration = _Timer.TomatoDuration;

			if(_Log != null)
			{
				_Log.Updated -= new Action(_Log_Updated);
			}
			_Log = log;
			if(log != null)
			{
				_Log.Updated += new Action(_Log_Updated);
				LogText = _Log.Text;
				ResetTitle();
			}
		}
		void timer_TomatoHistoryWasRewritten()
		{
			ResetTitle();
		}
		void _Log_Updated()
		{
			_Log.SuspendUpdateNotification();
			LogText = _Log.Text;
			_Log.ResumeUpdateNotification();
		}
		void timer_TomatoDone()
		{
			StartOrSquashButton.Text = "Start";
			ContinueButton.Visible = true;
			maskedTextBox1.Enabled = true;
			TomatoDuration = _Timer.TomatoDuration;
			ResetTitle();
		}
		void timer_Starting()
		{
			StartOrSquashButton.Text = "Squash";
			ContinueButton.Visible = false;
			maskedTextBox1.Enabled = false;
		}
		void timer_Tick()
		{
			TomatoDuration = _Timer.TomatoTimeRemains;
		}
		public TimeSpan? TomatoDuration
		{
			get
			{
				TimeSpan parsed;
				return (TimeSpan.TryParse(maskedTextBox1.Text, out parsed) ? (TimeSpan?)parsed : null);
			}
			set
			{
				if (value == null)
					value = TimeSpan.Zero;
				maskedTextBox1.Text = value.Value.ToString(@"hh\:mm\:ss");
			}
		}
		public void SetTomatoDurationValidationError(string message)
		{
			errorProvider1.SetError(maskedTextBox1, message);
		}

		public void TomatoDurationValidated(object sender, CancelEventArgs eventArgs)
		{
			if (TomatoDuration == null)
			{
				SetTomatoDurationValidationError("Invalid time span");
				eventArgs.Cancel = true;
			}
			else
			{
				SetTomatoDurationValidationError(string.Empty);
			}
		}
		public string LogText
		{
			get { return richTextBoxLog.Text; }
			set { richTextBoxLog.Text = value; }
		}
		
		private  void SendCompletedCallback(object sender, AsyncCompletedEventArgs e)
		{
			// Get the unique identifier for this asynchronous operation.
			String token = (string)e.UserState;
			if (e.Error != null)
			{
				ReportMessageSendFailure(e.Error);
			}
			else
			{
				MessageBox.Show("Message to evernote was successfully sent.", "Success", MessageBoxButtons.OK,
				                MessageBoxIcon.Information);
				sendToEvernoteToolStripMenuItem.ToolTipText = "Last sent at " + DateTimeApp.Instance.Now.ToShortTimeString();
			}
			sendToEvernoteToolStripMenuItem.Text = "Send to evernote";
			sendToEvernoteToolStripMenuItem.Enabled = true;
		}
		
		private static void ReportMessageSendFailure(Exception exception)
		{
			StringBuilder errorsBuilder = new StringBuilder();
			while(exception != null)
			{
				errorsBuilder.AppendLine(exception.Message);
				if (exception is SmtpException)
					errorsBuilder.AppendLine("Smtp server response: " + ((SmtpStatusCode) ((SmtpException) exception).StatusCode).ToString());
				exception = exception.InnerException;
			}
			MessageBox.Show(
				string.Format("Failed to send message. Please see settings in .config file. Error message: \n{0}", errorsBuilder.ToString()),
				"Failure", MessageBoxButtons.OK, MessageBoxIcon.Error);
		}
		private void quitToolStripMenuItem_Click(object sender, EventArgs e)
		{
			Application.Exit();
		}
		private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
		{
			var aboutForm = new About();
			aboutForm.ShowDialog(this);
		}
		private void sendToEvernoteToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (string.IsNullOrWhiteSpace(LogText))
			{
				MessageBox.Show("No message to send.");
				return;
			}
			MailMessage message = null;
			try
			{
				SmtpClient client = new SmtpClient();
				message = new MailMessage();
				message.To.Add(Settings.Default.EvernoteUserEmail);
				message.Body = LogText;
				message.Subject = string.Format("{0} {1} {2}",
					string.Format(Settings.Default.EvernoteMailSubject, _Timer.CountTomatoesDone, DateTimeApp.Instance.Now.ToShortDateString()),
					!string.IsNullOrWhiteSpace(Settings.Default.EvernoteMailNotebook) ? "@" + Settings.Default.EvernoteMailNotebook : null,
					!string.IsNullOrWhiteSpace(Settings.Default.EvernoteMailTag) ? "#" + Settings.Default.EvernoteMailTag : null);
				client.SendCompleted += new SendCompletedEventHandler(SendCompletedCallback);
				client.SendAsync(message, null);
				sendToEvernoteToolStripMenuItem.Text = "Sending...";
				sendToEvernoteToolStripMenuItem.Enabled = false;
			}
			catch (Exception ex)
			{
				ReportMessageSendFailure(ex);
			}
			finally
			{
				//if(message != null)	message.Dispose();
			}
			
		}
		public void CallAnalysisOfLog(object sender, EventArgs e)
		{
			_Presenter.CallLogAnalysis();
		}
		private void smallDurationToolStripMenuItem_Click(object sender, EventArgs e)
		{
			TomatoDuration = TomatoTimeSpan.SmallTomatoSpan;
		}
		private void mediumDurationToolStripMenuItem_Click(object sender, EventArgs e)
		{
			TomatoDuration = TomatoTimeSpan.MediumTomatoSpan;
		}
		private void largeDurationToolStripMenuItem_Click(object sender, EventArgs e)
		{
			TomatoDuration = TomatoTimeSpan.BigTomatoSpan;
		}

		private void buttonContinue_Click (object sender, EventArgs e) {
			_Presenter.ContinueTomato(TomatoDuration.Value);
		}

	}
}
