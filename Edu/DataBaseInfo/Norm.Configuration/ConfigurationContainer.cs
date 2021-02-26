using Norm.BSON.TypeConverters;
using System;
using System.Globalization;
namespace Norm.Configuration
{
	public class ConfigurationContainer : MongoConfigurationMap, IConfigurationContainer, IMongoConfigurationMap, IHideObjectMembers
	{
		public ConfigurationContainer()
		{
			base.TypeConverterFor<CultureInfo, CultureInfoTypeConverter>();
		}
		public void AddMap<T>() where T : IMongoConfigurationMap, new()
		{
			if (default(T) != null)
			{
				return;
			}
			Activator.CreateInstance<T>();
		}
		public IMongoConfigurationMap GetConfigurationMap()
		{
			return this;
		}
	}
}
