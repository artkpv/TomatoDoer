using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using TomatoDoer.Model;
namespace TomatoLogParser
{
	public class TomatoLogParser
	{
		//Template:
		//10 янв 13:55:19: Tomato 1 was done (25:00).
		private static Regex _TomatoDoneRegex = new Regex(@"(?<time>\d{1,2} \w{3} \d\d\:\d\d\:\d\d)\: +Tomato (?<number>\d+) was done( +\((?<span>\d\d:\d\d)\))?");
		//Template:
		//10 янв 14:51:54: Tomato was squashed.
		private static Regex _TomatoSquashedRegex = new Regex(@"(?<time>\d{1,2} \w{3} \d\d\:\d\d\:\d\d)\: +Tomato was squashed");
		private TimeSpan _TimeSpanDefault;
		public List<TomatoTimeSpanDescribed>  ParseTomatoesLog(string tomatoLog)
		{
			var clearedLog = tomatoLog.Replace("\r", "");
			var matchedTomatoDone = _TomatoDoneRegex.Match(clearedLog);
			var tomatoesParsed = new List<TomatoTimeSpanDescribed>();
			while (matchedTomatoDone.Success)
			{
				Group spanGroup = matchedTomatoDone.Groups["span"];
				_TimeSpanDefault = new TimeSpan(0, 25, 0);
				TimeSpan duration = _TimeSpanDefault;
				if(spanGroup.Success)
					duration = TimeSpan.ParseExact(spanGroup.Value, @"mm\:ss", CultureInfo.CurrentCulture);
				var endTime = DateTime.ParseExact(matchedTomatoDone.Groups["time"].Value, @"d MMM HH\:mm\:ss",
				                                  CultureInfo.CurrentCulture);
				int tomatoNumber = int.Parse(matchedTomatoDone.Groups["number"].Value);
				//todo: Fix this feature!
				if (endTime > DateTimeApp.Now)
					endTime = endTime.AddYears(-1);
				string tomatoDescription = GetTomatoUpperDescription(matchedTomatoDone.Index, clearedLog);
				
				tomatoesParsed.Add(
					new TomatoTimeSpanDescribed()
						{
							TomatoTimeSpan = new TomatoTimeSpan(duration, endTime),
							Description = tomatoDescription,
							TomatoNumber = tomatoNumber
						});
				matchedTomatoDone = matchedTomatoDone.NextMatch();
			}
			return tomatoesParsed;
		}
		public string GetTomatoUpperDescription(int indexOfCharToSearchTill, string log)
		{
			var descriptionRegex = new Regex("(?<=(\n\n|^))(?<descr>.+)");
			
			string description = null;
			bool isStartOfFile = false;
			int indexOfPrecedeousEmptyLine;
			Match match;
			while (description == null && !isStartOfFile)
			{
				indexOfPrecedeousEmptyLine = log.LastIndexOf("\n\n", indexOfCharToSearchTill, indexOfCharToSearchTill + 1);
				indexOfPrecedeousEmptyLine = indexOfPrecedeousEmptyLine == -1 ? 0 : indexOfPrecedeousEmptyLine;
				match = descriptionRegex.Match(
					log,
					indexOfPrecedeousEmptyLine,
					indexOfCharToSearchTill - indexOfPrecedeousEmptyLine);
				if (match.Success)
				{
					description = match.Groups["descr"].Value;
					if (_TomatoDoneRegex.IsMatch(description) || _TomatoSquashedRegex.IsMatch(description))
					{
						description = null;
					}
				}
				indexOfCharToSearchTill = indexOfPrecedeousEmptyLine;
				isStartOfFile = indexOfCharToSearchTill == 0;
			}
			return description;
		}
		/*
				void Analyze(string tomatoesLog)
				{
					DateTime beginDate = new DateTime(2010, 09, 02);
					DateTime endDate = new DateTime(2011, 05, 17);
					TimeSpan analizeDuration = (endDate - beginDate);
					int numberOfTomatoes = Regex.Matches(tomatoesLog, @"Tomato \d+ was done").Count;
					double numberOfWeeks = analizeDuration.TotalDays/7;
					Console.WriteLine("Weeks: {0}, dayes: {3}, begin date: {1}, end date: {2}", numberOfWeeks, beginDate, endDate,
									  analizeDuration.TotalDays);
					Console.WriteLine("Number of tomatoes: {0}", numberOfTomatoes);
					Console.WriteLine(@"Tomatoes per day: {0}/({1}*{2}) = {3}", numberOfTomatoes, numberOfWeeks, 6,
									  numberOfTomatoes/(numberOfWeeks*6));
			
					List<WorkDay> workDays = ParseTomatoLog(tomatoesLog);
		//			int total = workDays.Sum(workDay => workDay.Tomatoes.Count);
		//
		//			Debug.Assert(numberOfTomatoes == total);
					WriteNumberOfTomatoesPerDayHour(workDays, true);
					//WriteNumberOfTomatoesPerDayHour(workDays, false);
					int numberOfProReportTomatoes = workDays.Sum(
						workDay => workDay.Tomatoes.Count(
							(Tomato tomato) => tomato.Task.Contains("ProReport")
									)
						);
					Console.WriteLine("ProReport tomatoes: {0}, {1} hours", numberOfProReportTomatoes, numberOfProReportTomatoes / 2);
					var longevities = new Dictionary<int, int>();
					workDays.ForEach(workDay =>
										{
											int logevity = workDay.Logevity;
											if (logevity < 24)
											{
												if (longevities.ContainsKey(logevity)) longevities[logevity]++;
												else longevities[logevity] = 1;
											}
										});
					var hours = longevities.Keys.ToList();
					hours.Sort();
					int sumOfHoursWorked = 0;
					int numberOfWorkDays = 0;
					foreach (int i in hours)
					{
						sumOfHoursWorked += longevities[i]*i;
						numberOfWorkDays += longevities[i];
						Console.WriteLine("Number of days with {0} hours difference - {1}", i, longevities[i]);
					}
					Console.WriteLine("Longevity of average work day: {0}", sumOfHoursWorked / numberOfWorkDays);
					Console.Read();
				}
				public static List<WorkDay> ParseTomatoLog(string tomatoesLog)
				{
					var workDays = new List<WorkDay>();
					string currentTaskName = null;
					var tomatoRegEx = new Regex(@"(\d{1,2} \w{3} \d{2}\:\d{2}\:\d{2})\: Tomato .+");
					var tomatoDoneRegEx = new Regex(@"Tomato (\d+) .+");
					Match tomatoMatch, tomatoDoneMatch;
					var logReader = new StringReader(tomatoesLog);
					string currentLine= logReader.ReadLine();
					WorkDay currentWorkDay = null;
					while (currentLine != null)
					{
						if (currentLine != string.Empty)
						{
							tomatoMatch = tomatoRegEx.Match(currentLine);
							if (!tomatoMatch.Success)
							{
								currentTaskName = currentLine;
						
							}
							else
							{
								tomatoDoneMatch = tomatoDoneRegEx.Match(tomatoMatch.Value);
								var currentTomato =
									new Tomato
									{
										IsDone = tomatoDoneMatch.Success,
										Time = ParseTomatoTimeExact(tomatoMatch.Groups[1].Value),
										TomatoNumber = (tomatoDoneMatch.Success ? int.Parse(tomatoDoneMatch.Groups[1].Value) : 0),
										Task = currentTaskName
									};
								if (currentTomato.IsDone)
								{
									if (currentTomato.TomatoNumber == 1 || currentWorkDay == null)
									{
										currentWorkDay = new WorkDay() { Tomatoes = new List<Tomato>() };
										workDays.Add(currentWorkDay);
									}
									currentWorkDay.Tomatoes.Add(currentTomato);
								}
							}
						}
				
						currentLine = logReader.ReadLine();
					}
					return workDays;
				}
				private static void WriteNumberOfTomatoesPerDayHour(List<WorkDay> workDays, bool isDone)
				{
					for (int i = 0; i < 24; i++)
					{
						int currentHour = i;
						int numberOfTomatoesAtHour = 0;
						workDays.ForEach(workDay => numberOfTomatoesAtHour += workDay.Tomatoes.Count(tomato => tomato.TimeOfDay == currentHour && isDone));
						Console.WriteLine("Hour {0} - {1} tomatoes", i, numberOfTomatoesAtHour);
					}
				}

				private static DateTime ParseTomatoTimeExact(string dateTimeValue)
				{
					return DateTime.ParseExact(dateTimeValue, "d MMM HH:mm:ss", CultureInfo.GetCultureInfo("ru-RU").DateTimeFormat);
				}*/
	}
}
