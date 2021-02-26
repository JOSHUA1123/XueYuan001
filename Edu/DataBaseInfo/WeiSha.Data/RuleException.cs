using System;
namespace DataBaseInfo
{
	[Serializable]
	public class RuleException : Exception
	{
		public RuleException(string msg) : base(msg)
		{
		}
		public RuleException(string msg, Exception inner) : base(msg, inner)
		{
		}
	}
}
