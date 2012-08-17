namespace TomatoDoer
{
	public interface ILogFile
	{
		void WriteAllText(string logText);
		string ReadAllText();
	}
}