using Norm.Configuration;
using System;
namespace Norm.BSON
{
	public class BsonSerializerBase
	{
		private static IMongoConfigurationMap _configuration;
		protected static IMongoConfigurationMap Configuration
		{
			get
			{
				if (BsonSerializerBase._configuration != null)
				{
					return BsonSerializerBase._configuration;
				}
				return MongoConfiguration.ConfigurationContainer;
			}
			set
			{
				BsonSerializerBase._configuration = value;
			}
		}
		public static void UseConfiguration(IMongoConfigurationMap config)
		{
			BsonSerializerBase.Configuration = config;
		}
	}
}
