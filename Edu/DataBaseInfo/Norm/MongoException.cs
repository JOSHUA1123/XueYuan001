using System;
namespace Norm
{
	public class MongoException : Exception
	{
		public MongoException(string message) : base(message)
		{
		}
	}
}
