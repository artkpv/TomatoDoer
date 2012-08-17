using System;
using System.Text.RegularExpressions;
using TomatoDoer.Model;

namespace TomatoLogParser
{
	public class TomatoTimeSpanDescribed
	{
		private static Regex _LabelRegex = new Regex(@"#\w+");
		public TomatoTimeSpan TomatoTimeSpan { get; set; }
		public string Description { get; set; }

		public string Label
		{
			get
			{
				string label = "N/A";
				if (!string.IsNullOrWhiteSpace(Description))
				{
					var match = _LabelRegex.Match(Description, 0);
					if (match.Success)
						label = match.Value;
				}

				return label;
			}
		}

		public int? TomatoNumber { get; set; }

		public bool Equals(TomatoTimeSpanDescribed other)
		{
			if (ReferenceEquals(null, other)) return false;
			if (ReferenceEquals(this, other)) return true;
			return other.TomatoTimeSpan.Equals(TomatoTimeSpan) && Equals(other.Description, Description) && other.TomatoNumber.Equals(TomatoNumber);
		}

		public override bool Equals(object obj)
		{
			if (ReferenceEquals(null, obj)) return false;
			if (ReferenceEquals(this, obj)) return true;
			if (obj.GetType() != typeof (TomatoTimeSpanDescribed)) return false;
			return Equals((TomatoTimeSpanDescribed) obj);
		}

		public override int GetHashCode()
		{
			unchecked
			{
				int result = TomatoTimeSpan.GetHashCode();
				result = (result*397) ^ (Description != null ? Description.GetHashCode() : 0);
				result = (result*397) ^ (TomatoNumber.HasValue ? TomatoNumber.Value : 0);
				return result;
			}
		}
	}
}