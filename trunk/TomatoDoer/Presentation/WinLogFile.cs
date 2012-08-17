using System;
using System.IO;
using System.Windows.Forms;

namespace TomatoDoer
{
	public class WinLogFile : ILogFile
	{
		private static string _LogFilePath = Path.Combine(Application.StartupPath, "TomatoDoerLogFile.log");

		public void WriteAllText(string logText)
		{
			try
			{
				File.WriteAllText(_LogFilePath, logText);
			}
			catch (UnauthorizedAccessException)
			{
				MessageBox.Show("Failed to read log file. Unauthorized to access.");
			}
		}

		public string ReadAllText()
		{
			if (File.Exists(_LogFilePath))
			{
				try
				{
					return File.ReadAllText(_LogFilePath);
				}
				catch (UnauthorizedAccessException)
				{
					MessageBox.Show("Failed to read log file. Unauthorized to access.");
				}
			}
			return string.Empty;
		}
	}
}