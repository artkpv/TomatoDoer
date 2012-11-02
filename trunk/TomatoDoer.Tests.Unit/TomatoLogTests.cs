using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Moq;
using NUnit.Framework;
using TomatoDoer.Model;
namespace TomatoDoer.Tests.Unit
{
	[TestFixture]
	public class TomatoLogTests
	{
		private Mock<ILogFile> _LogFile;
		private TomatoLog _Log;
		
		[SetUp]
		public  void InitializeTest()
		{
			_LogFile = new Mock<ILogFile>();
			_Log = new TomatoLog(_LogFile.Object);
		}
		[Test]
		public void Could_Write()
		{
			string testMessage = "Test message";
			_Log.Write(testMessage);
			StringAssert.IsMatch(@"\d{1,2} \w{3,3} \d\d\:\d\d\:\d\d\: " + testMessage, _Log.Text);
		}
		[Test]
		public void Could_Flush_To_Disk()
		{
			string testMessage = "Test message";
			_Log.Write(testMessage);

			//act
			_Log.Flush();

			_LogFile.Verify(lf=>lf.WriteAllText(It.IsRegex(testMessage)));
		}

		[Test]
		public void Could_Load_From_Disk()
		{
			string testMessage = @"01 apr 13:20:20: Test message1
01 apr 13:30:20: Test message2";
			_LogFile.Setup(file => file.ReadAllText()).Returns(testMessage);
			_Log.Write("Something was here");

			//act
			_Log.Load();

			//assert
			Assert.AreEqual(testMessage, _Log.Text);
		}

		[Test]
		public void Could_Notify_Updating()
		{
			bool wasNotified = false;
			_Log.Updated += () => wasNotified = true;
			_Log.Write("Something");
			Assert.IsTrue(wasNotified);
		}
	}
}
