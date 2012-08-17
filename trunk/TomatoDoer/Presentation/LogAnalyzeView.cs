using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TomatoLogParser;

namespace TomatoDoer.Presentation
{
	public class LogAnalyzeView
	{
		public string GetView(List<TomatoTimeSpanDescribed> spansParsed)
		{
			var analyze = new StringBuilder();
			if (spansParsed.Count == 0)
			{
				analyze.AppendLine("No tomatoes found to analyze");
			}
			else
			{
				analyze.Append("Number of tomatoes done: ");
				analyze.AppendLine(spansParsed.Count.ToString());
				analyze.AppendFormat("From {0} till {1}", spansParsed[0].TomatoTimeSpan.StartTime,
				                     spansParsed[spansParsed.Count - 1].TomatoTimeSpan.EndTime);
				analyze.AppendLine();


				double workHoursTotal = spansParsed.Sum(s => s.TomatoTimeSpan.Duration.TotalHours);
				analyze.AppendFormat("Work hours total: {0}",
				                     workHoursTotal.ToString("F2"));

				var groupedByWorkingSequences= GetWorkingSequences(spansParsed);
				analyze.AppendLine();
				analyze.AppendFormat("Total working sequences (days): {0}", groupedByWorkingSequences.Count());
				
				analyze.AppendLine();
				var groupedByDayes =
					(from span in spansParsed
					 group span by span.TomatoTimeSpan.StartTime.Value.Date
					 into date
					 select new
					        	{
					        		Date = date.Key,
					        		HoursWorked = date.Sum(d => d.TomatoTimeSpan.Duration.TotalHours),
					        		TomatoesDone = date.Count()
					        	}
					);

				analyze.AppendLine("Average hours per day: " + groupedByDayes.Average(g => g.HoursWorked));
				analyze.AppendLine();
				analyze.AppendLine("Average tomatoes per day: " + groupedByDayes.Average(g => g.TomatoesDone));

				analyze.AppendLine();
				analyze.AppendLine();

				/*var tomatoesGroupedByDescription =
					 from span in spansParsed
					 orderby span.TomatoTimeSpan.StartTime.Value
					 group span by span.Description
					 into d
					 select d;

				 analyze.AppendLine("Tomatoes grouped by description:");

				 foreach (var tomatoes in tomatoesGroupedByDescription)
				 {
					 analyze.AppendFormat("Description: \"{0}\".", tomatoes.Key);
					 analyze.AppendLine();
					 analyze.AppendFormat("Number of tomatoes: {0}", tomatoes.Count());
					 analyze.AppendLine();
					 analyze.AppendFormat("Start of first: {0}", tomatoes.First().TomatoTimeSpan.StartTime.Value.ToString());
					 analyze.AppendLine();
					 analyze.AppendFormat("End of last: {0}", tomatoes.Last().TomatoTimeSpan.EndTime.Value.ToString());
					 analyze.AppendLine();
					 analyze.AppendFormat("Total hours: {0}", tomatoes.Sum(s => s.TomatoTimeSpan.Duration.TotalHours).ToString("F2"));
					 analyze.AppendLine();

					 analyze.AppendLine();
				 }*/

				IEnumerable<IGrouping<string, TomatoTimeSpanDescribed>> tomatoesGroupedByLabel =
					from span in spansParsed
					orderby span.TomatoTimeSpan.StartTime.Value
					group span by span.Label
					into d
					select d;


				analyze.AppendLine("Tomatoes grouped by labels:");

				foreach (var descriptionGroup in tomatoesGroupedByLabel.OrderBy(g => g.Count()))
				{
					analyze.AppendFormat("Label: \"{0}\".", descriptionGroup.Key);
					analyze.AppendLine();
					analyze.AppendFormat("Number of tomatoes: {0}", descriptionGroup.Count());
					analyze.AppendLine();
					analyze.AppendFormat("Start of first: {0}", descriptionGroup.First().TomatoTimeSpan.StartTime.Value.ToString());
					analyze.AppendLine();
					analyze.AppendFormat("End of last: {0}", descriptionGroup.Last().TomatoTimeSpan.EndTime.Value.ToString());
					analyze.AppendLine();
					analyze.AppendFormat("Total hours: {0}",
					                     descriptionGroup.Sum(s => s.TomatoTimeSpan.Duration.TotalHours).ToString("F2"));

					var numberOfWorkSequences = 0;

					foreach (var sequence in groupedByWorkingSequences)
					{
						if(sequence.FirstOrDefault(s=> s.Label == descriptionGroup.Key) != null)
						{
							numberOfWorkSequences++;
						}
					}

					analyze.AppendLine();
					analyze.AppendFormat("Work sequences (days): {0}", numberOfWorkSequences);

					analyze.AppendLine();
					analyze.AppendLine("Desciprtions: ");
					foreach (var groupInGroupByDescription in descriptionGroup.GroupBy(g => g.Description))
					{
						analyze.AppendFormat("[{0}] ", groupInGroupByDescription.Key);
					}
					analyze.AppendLine();

					analyze.AppendLine();
				}


				analyze.AppendLine("------------------------");
			}
			return analyze.ToString();
		}
		
		public IEnumerable<IGrouping<DateTime, TomatoTimeSpanDescribed>> GetWorkingSequences(IEnumerable<TomatoTimeSpanDescribed> spans)
		{
			var orderedSpans = from span in spans
			                   orderby span.TomatoTimeSpan.EndTime.Value
			                   select span;
							   
			var sequences =
				from span in orderedSpans
				let firstInSequence =
					orderedSpans.LastOrDefault(
					s => s.TomatoTimeSpan.StartTime <= span.TomatoTimeSpan.StartTime && s.TomatoNumber == 1)
				group span by firstInSequence != null
				              	? firstInSequence.TomatoTimeSpan.StartTime.Value.Date
				              	: span.TomatoTimeSpan.StartTime.Value.Date;

			return sequences;
		}
	}
}