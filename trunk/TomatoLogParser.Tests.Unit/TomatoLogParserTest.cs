using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TomatoDoer.Model;

namespace TomatoLogParser.Tests.Unit
{
	[TestClass]
	public class TomatoLogParserTest
	{

		public void ParseAndAssertParsedCollectionIsEquivalent(string log, List<TomatoTimeSpanDescribed> expectedSpans)
		{
			var parser = new TomatoLogParser();

			var parsedLog = parser.ParseTomatoesLog(log);

			CollectionAssert.AreEquivalent(expectedSpans, parsedLog);
		}

		[TestMethod]
		public void Parse_One_Tomato_Log()
		{
			string log;
			List<TomatoTimeSpanDescribed> spans;
			CreateOneTomatoLogAndParsedSpan(out spans, out log);
			ParseAndAssertParsedCollectionIsEquivalent(log, spans);
		}

		[TestMethod]
		public void Parse_One_Day_Log_Only_Time()
		{
			List<TomatoTimeSpanDescribed> spans;
			string log;
			CreateLogAndParsedSpans(out spans, out log);
			
			ParseAndAssertParsedCollectionIsEquivalent(log, spans);
		}
		
		[TestMethod]
		public void Parse_Log_Without_Duration()
		{
			List<TomatoTimeSpanDescribed> spans;
			string log;
			CreateLogAndParsedSpansWithoutDuration(out spans, out log);

			ParseAndAssertParsedCollectionIsEquivalent(log, spans);
		}

		[TestMethod]
		public void Parse_Log_Taking_Previous_Tomato_Description_If_It_is_not_present()
		{
			List<TomatoTimeSpanDescribed> spans;
			string log;
			log =
				@"20 апр - 1,5ч, 21 апр. - 1ч, 22 апр. -0,5ч


Читаю The joy of sex
20 апр 23:23:53: Tomato 1 was done.
21 апр 00:18:42: Tomato was squashed.
21 апр 01:14:22: Tomato 2 was done.


22 апр 00:16:42: Tomato 1 was done.
22 апр 00:16:44: Tomato 2 was done.

22 апр 13:02:12: Tomato 1 was done.";

			spans =
				new List<TomatoTimeSpanDescribed>()
					{
						new TomatoTimeSpanDescribed()
							{
								Description = "Читаю The joy of sex",
								TomatoTimeSpan = new TomatoTimeSpan(new TimeSpan(0, 25, 0), DateTime.Parse("2011-4-20 23:23:53")),
								TomatoNumber = 1
							},
						new TomatoTimeSpanDescribed()
							{
								Description = "Читаю The joy of sex",
								TomatoTimeSpan = new TomatoTimeSpan(new TimeSpan(0, 25, 0), DateTime.Parse("2011-4-21 01:14:22")),
								TomatoNumber = 2
							},
						new TomatoTimeSpanDescribed()
							{
								Description = "Читаю The joy of sex",
								TomatoTimeSpan = new TomatoTimeSpan(new TimeSpan(0, 25, 0), DateTime.Parse("2011-4-22 00:16:42")),
								TomatoNumber = 1
							},
						new TomatoTimeSpanDescribed()
							{
								Description = "Читаю The joy of sex",
								TomatoTimeSpan = new TomatoTimeSpan(new TimeSpan(0, 25, 0), DateTime.Parse("2011-4-22 00:16:44")),
								TomatoNumber = 2
							},
						new TomatoTimeSpanDescribed()
							{
								Description = "Читаю The joy of sex",
								TomatoTimeSpan = new TomatoTimeSpan(new TimeSpan(0, 25, 0), DateTime.Parse("2011-4-22 13:02:12")),
								TomatoNumber = 1
							}
					};

			ParseAndAssertParsedCollectionIsEquivalent(log, spans);
		}

		private static void CreateLogAndParsedSpansWithoutDuration(out List<TomatoTimeSpanDescribed> spans, out string log)
		{
			log =
				@"Code complete. Analyze.
 10 янв 13:55:19: Tomato 1 was done.
 10 янв 14:20:24: Tomato 2 was done.
 10 янв 14:51:54: Tomato was squashed.
 10 янв 15:17:45: Tomato 3 was done.

Code complete. Reading about testing.
 10 янв 15:52:53: Tomato 4 was done.
 10 янв 16:12:04: Tomato was squashed.
 10 янв 16:39:41: Tomato 5 was done.

TomatoDoer
 10 янв 18:08:49: Tomato 6 was done.
 10 янв 18:47:32: Tomato 7 was done.

Подвожу итоги 2011 года
 10 янв 19:31:06: Tomato was squashed.

TomatoDoer";

			spans = new List<TomatoTimeSpanDescribed>()
			        	{
			        		new TomatoTimeSpanDescribed()
			        			{
			        				TomatoTimeSpan = new TomatoTimeSpan(new TimeSpan(0, 25, 0), DateTime.Parse("2012-1-10 13:55:19")),
			        				Description = "Code complete. Analyze.",
								TomatoNumber = 1
			        			},
							new TomatoTimeSpanDescribed()
			        			{
			        				TomatoTimeSpan = new TomatoTimeSpan(new TimeSpan(0, 25, 0), DateTime.Parse("2012-1-10 14:20:24")),
			        				Description = "Code complete. Analyze.",
								TomatoNumber = 2
			        			},
								new TomatoTimeSpanDescribed()
			        			{
			        				TomatoTimeSpan = new TomatoTimeSpan(new TimeSpan(0, 25, 0), DateTime.Parse("2012-1-10 15:17:45")),
			        				Description = "Code complete. Analyze.",
								TomatoNumber = 3
			        			},
			        			new TomatoTimeSpanDescribed()
			        			{
			        				TomatoTimeSpan = new TomatoTimeSpan(new TimeSpan(0, 25, 0), DateTime.Parse(" 2012-1-10 15:52:53")),
			        				Description =  "Code complete. Reading about testing.",
								TomatoNumber = 4
			        			},
			        		new TomatoTimeSpanDescribed()
			        			{
			        				TomatoTimeSpan = new TomatoTimeSpan(new TimeSpan(0, 25, 0), DateTime.Parse("2012-1-10 16:39:41")),
			        				Description =  "Code complete. Reading about testing.",
								TomatoNumber = 5
			        			},
			        		new TomatoTimeSpanDescribed()
			        			{
			        				TomatoTimeSpan = new TomatoTimeSpan(new TimeSpan(0, 25, 0), DateTime.Parse("2012-1-10 18:08:49")),
			        				Description =  "TomatoDoer",
								TomatoNumber = 6
			        			},
			        		new TomatoTimeSpanDescribed()
			        			{
			        				TomatoTimeSpan = new TomatoTimeSpan(new TimeSpan(0, 25, 0), DateTime.Parse("2012-1-10 18:47:32")),
			        				Description =  "TomatoDoer",
								TomatoNumber = 7
			        			}
			        	};
		}


		private static void CreateLogAndParsedSpans(out List<TomatoTimeSpanDescribed> spans, out string log)
		{
			log =
				@"Code complete. Analyze.
 10 янв 13:55:19: Tomato 1 was done (25:00).
 10 янв 14:20:24: Tomato 2 was done (25:00).
 10 янв 14:51:54: Tomato was squashed.
 10 янв 15:17:45: Tomato 3 was done (25:00).

Code complete. Reading about testing.
 10 янв 15:52:53: Tomato 4 was done (25:00).
 10 янв 16:12:04: Tomato was squashed.
 10 янв 16:39:41: Tomato 5 was done (25:00).

TomatoDoer
 10 янв 18:08:49: Tomato 6 was done (50:00).
 10 янв 18:47:32: Tomato 7 was done (25:00).

Подвожу итоги 2011 года
 10 янв 19:31:06: Tomato was squashed.

TomatoDoer";

			spans = new List<TomatoTimeSpanDescribed>()
			        	{
			        		new TomatoTimeSpanDescribed()
			        			{
			        				TomatoTimeSpan = new TomatoTimeSpan(new TimeSpan(0, 25, 0), DateTime.Parse("2012-1-10 13:55:19")),
			        				Description = "Code complete. Analyze.",
								TomatoNumber = 1
			        			},
							new TomatoTimeSpanDescribed()
			        			{
			        				TomatoTimeSpan = new TomatoTimeSpan(new TimeSpan(0, 25, 0), DateTime.Parse("2012-1-10 14:20:24")),
			        				Description = "Code complete. Analyze.",
								TomatoNumber = 2
			        			},
								new TomatoTimeSpanDescribed()
			        			{
			        				TomatoTimeSpan = new TomatoTimeSpan(new TimeSpan(0, 25, 0), DateTime.Parse("2012-1-10 15:17:45")),
			        				Description = "Code complete. Analyze.",
								TomatoNumber = 3
			        			},
			        			new TomatoTimeSpanDescribed()
			        			{
			        				TomatoTimeSpan = new TomatoTimeSpan(new TimeSpan(0, 25, 0), DateTime.Parse(" 2012-1-10 15:52:53")),
			        				Description =  "Code complete. Reading about testing.",
								TomatoNumber = 4
			        			},
			        		new TomatoTimeSpanDescribed()
			        			{
			        				TomatoTimeSpan = new TomatoTimeSpan(new TimeSpan(0, 25, 0), DateTime.Parse("2012-1-10 16:39:41")),
			        				Description =  "Code complete. Reading about testing.",
								TomatoNumber = 5
			        			},
			        		new TomatoTimeSpanDescribed()
			        			{
			        				TomatoTimeSpan = new TomatoTimeSpan(new TimeSpan(0, 50, 0), DateTime.Parse("2012-1-10 18:08:49")),
			        				Description =  "TomatoDoer",
								TomatoNumber = 6
			        			},
			        		new TomatoTimeSpanDescribed()
			        			{
			        				TomatoTimeSpan = new TomatoTimeSpan(new TimeSpan(0, 25, 0), DateTime.Parse("2012-1-10 18:47:32")),
			        				Description =  "TomatoDoer",
								TomatoNumber = 7
			        			}
			        	};
		}

		private static void CreateOneTomatoLogAndParsedSpan(out List<TomatoTimeSpanDescribed> spans, out string log)
		{
			log = @"Code complete. Analyze.
 10 янв 13:55:19: Tomato 1 was done (25:00).";
			spans = new List<TomatoTimeSpanDescribed>
			        	{
			        		new TomatoTimeSpanDescribed()
			        			{
									TomatoTimeSpan = new TomatoTimeSpan() {
			        					Duration = new TimeSpan(0, 25, 0),
			        					EndTime = DateTime.Parse("2012-1-10 13:55:19"),
			        					StartTime = DateTime.Parse("2012-1-10 13:30:19")
									},
			        				Description = "Code complete. Analyze.",
								TomatoNumber = 1
			        			}
			        	};
		}
	}
}
