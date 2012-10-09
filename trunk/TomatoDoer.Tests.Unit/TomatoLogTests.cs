using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhino.Mocks;
using TomatoDoer.Model;
namespace TomatoDoer.Tests.Unit
{
	[TestClass]
	public class TomatoLogTests
	{
		private ILogFile _LogFile;
		private TomatoLog _Log;
		[TestInitialize]
		public  void InitializeTest()
		{
			_LogFile = MockRepository.GenerateMock<ILogFile>();
			_Log = new TomatoLog(_LogFile);
		}
		[TestMethod]
		public void Could_Write()
		{
			string testMessage = "Test message";
			_Log.Write(testMessage);
			StringAssert.Matches(_Log.Text, new Regex(@"\d{1,2} \w{3,3} \d\d\:\d\d\:\d\d\: " + testMessage));
		}
		[TestMethod]
		public void Could_Flush_To_Disk()
		{
			string testMessage = "Test message";
			_Log.Write(testMessage);
			_Log.Flush();
			IList<object[]> argumentsForCallsMadeOn = _LogFile.GetArgumentsForCallsMadeOn(file => file.WriteAllText(string.Empty));
			Assert.AreEqual(1, argumentsForCallsMadeOn.Count);
			Assert.AreEqual(1, argumentsForCallsMadeOn[0].Length);
			StringAssert.Contains((string)argumentsForCallsMadeOn[0][0], testMessage);
		}
		[TestMethod]
		public void Could_Load_From_Disk()
		{
			string testMessage = @"01 apr 13:20:20: Test message1
01 apr 13:30:20: Test message2";
			_LogFile.Stub(file => file.ReadAllText()).Return(testMessage);
			_Log.Write("Something was here");
			_Log.Load();
			Assert.AreEqual(testMessage, _Log.Text);
		}
		[TestMethod]
		public void Could_Notify_Updating()
		{
			bool wasNotified = false;
			_Log.Updated += () => wasNotified = true;
			_Log.Write("Something");
			Assert.IsTrue(wasNotified);
		}
	}
}
