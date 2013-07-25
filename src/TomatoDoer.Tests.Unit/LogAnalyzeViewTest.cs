using System.Linq;
using NUnit.Framework;
using TomatoDoer.Model;
using TomatoDoer.Presentation;
using NUnit;
using System;
using TomatoLogParser;
using System.Collections.Generic;

namespace TomatoDoer.Tests.Unit
{
    /// <summary>
    ///This is a test class for LogAnalyzeViewTest and is intended
    ///to contain all LogAnalyzeViewTest Unit Tests
    ///</summary>
	[TestFixture()]
	public class LogAnalyzeViewTest
	{
		[Test]
		public void TomatoesShouldBeGroupedByWorkingSequences()
		{
			List<TomatoTimeSpanDescribed> spans =
				new List<TomatoTimeSpanDescribed>()
					{
						new TomatoTimeSpanDescribed()
							{
								TomatoTimeSpan = new TomatoTimeSpan(new TimeSpan(0, 25, 0), DateTime.Parse("2011-10-01 13:00:00")),
								TomatoNumber = 1
							},
						new TomatoTimeSpanDescribed()
							{
								TomatoTimeSpan = new TomatoTimeSpan(new TimeSpan(0, 25, 0), DateTime.Parse("2011-10-01 13:30:00")),
								TomatoNumber = 2
							},
						new TomatoTimeSpanDescribed()
							{
								TomatoTimeSpan = new TomatoTimeSpan(new TimeSpan(0, 25, 0), DateTime.Parse("2011-10-01 14:00:00")),
								TomatoNumber = 3
							},
						new TomatoTimeSpanDescribed()
							{
								TomatoTimeSpan = new TomatoTimeSpan(new TimeSpan(0, 25, 0), DateTime.Parse("2011-10-02 13:00:00")),
								TomatoNumber = 1
							}
					};
			LogAnalyzeView view = new LogAnalyzeView();
			var sequences = view.GetWorkingSequences(spans);
			Assert.AreEqual(2, sequences.Count());
		}
		[Test]
		public void SequencesGroupedByDateOfFirstTomato()
		{
			List<TomatoTimeSpanDescribed> spans =
				new List<TomatoTimeSpanDescribed>()
					{
						new TomatoTimeSpanDescribed()
							{
								TomatoTimeSpan = new TomatoTimeSpan(new TimeSpan(0, 25, 0), DateTime.Parse("2011-10-01 23:00:00")),
								TomatoNumber = 1
							},
						new TomatoTimeSpanDescribed()
							{
								TomatoTimeSpan = new TomatoTimeSpan(new TimeSpan(0, 25, 0), DateTime.Parse("2011-10-02 00:30:00")),
								TomatoNumber = 2
							}
					};
			LogAnalyzeView view = new LogAnalyzeView();
			var sequences = view.GetWorkingSequences(spans);
			Assert.AreEqual(1, sequences.Count());
			Assert.AreEqual(DateTime.Parse("2011-10-01"), sequences.First().Key);
		}

		[Test]
		public void ManyTasksTest()
		{
			string log =
				@"
 Planning
 24 янв 16:48:58: Tomato 1 was done (25:00).
Code complete. Analysis.
 24 янв 17:44:43: Tomato was squashed.
 24 янв 18:15:51: Tomato was squashed.
 24 янв 18:15:54: Tomato 2 was done (25:00).
 24 янв 18:20:15: Tomato was squashed.
 24 янв 18:52:14: Tomato 3 was done (25:00).
Searching work
 24 янв 20:12:20: Tomato was squashed.
Code complete. Analysis.
 24 янв 21:11:59: Tomato 4 was done (25:00).
TomatoDoer
 24 янв 22:39:01: Tomato 5 was done (50:00).
 24 янв 23:16:00: Tomato 6 was done (25:00).
 24 янв 23:32:53: Tomato was squashed.
 24 янв 23:33:05: Tomato 7 was done (25:00).
 25 янв 00:09:34: Tomato 8 was done (25:00).
 25 янв 00:35:39: Tomato 9 was done (10:00).

 Planning
 25 янв 16:48:58: Tomato 1 was done (25:00).
 25 янв 17:48:58: Tomato 2 was done (25:00).
";
			var parser = new TomatoLogParser.TomatoLogParser();
			var spans = parser.ParseTomatoesLog(log);
			LogAnalyzeView view = new LogAnalyzeView();
			var sequences = view.GetWorkingSequences(spans);
			Assert.AreEqual(2, sequences.Count());
		}
	}
}
