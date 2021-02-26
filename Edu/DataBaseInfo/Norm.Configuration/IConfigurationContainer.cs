using System;
namespace Norm.Configuration
{
	public interface IConfigurationContainer : IMongoConfigurationMap, IHideObjectMembers
	{
		void AddMap<T>() where T : IMongoConfigurationMap, new();
		IMongoConfigurationMap GetConfigurationMap();
	}
}
