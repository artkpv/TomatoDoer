using System;
using System.Runtime.Serialization;

namespace TomatoDoer
{
	[Serializable]
	public class TomatoException : Exception
	{
		public TomatoException()
		{


		}

		public TomatoException(string message) : base(message)
		{
		}

		public TomatoException(string message, Exception inner) : base(message, inner)
		{
		}

		protected TomatoException(
			SerializationInfo info,
			StreamingContext context) : base(info, context)
		{
		}
	}
}