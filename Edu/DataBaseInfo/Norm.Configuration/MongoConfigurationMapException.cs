using System;
namespace Norm.Configuration
{
	public class MongoConfigurationMapException : MongoException
	{
		public MongoConfigurationMapException(string message) : base(message)
		{
		}
	}
}
