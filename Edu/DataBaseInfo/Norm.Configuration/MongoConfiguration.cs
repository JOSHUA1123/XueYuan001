using Norm.BSON;
using System;
namespace Norm.Configuration
{
	public static class MongoConfiguration
	{
		private static readonly object _objectLock = new object();
		private static IConfigurationContainer _configuration;
		internal static event Action<Type> TypeConfigurationChanged;
		internal static IConfigurationContainer ConfigurationContainer
		{
			get
			{
				if (MongoConfiguration._configuration == null)
				{
					lock (MongoConfiguration._objectLock)
					{
						if (MongoConfiguration._configuration == null)
						{
							MongoConfiguration._configuration = new ConfigurationContainer();
						}
					}
				}
				return MongoConfiguration._configuration;
			}
		}
		public static void RemoveMapFor<T>()
		{
			if (MongoConfiguration._configuration != null)
			{
				MongoConfiguration._configuration.RemoveFor<T>();
			}
		}
		public static void RemoveTypeConverterFor<TClr>()
		{
			if (MongoConfiguration._configuration != null)
			{
				MongoConfiguration._configuration.RemoveTypeConverterFor<TClr>();
			}
		}
		internal static void FireTypeChangedEvent(Type t)
		{
			if (MongoConfiguration.TypeConfigurationChanged != null)
			{
				MongoConfiguration.TypeConfigurationChanged(t);
			}
		}
		public static void Initialize(Action<IConfigurationContainer> action)
		{
			action(MongoConfiguration.ConfigurationContainer);
		}
		internal static string GetPropertyAlias(Type type, string propertyName)
		{
			if (MongoConfiguration._configuration == null)
			{
				return propertyName;
			}
			return MongoConfiguration._configuration.GetConfigurationMap().GetPropertyAlias(type, propertyName);
		}
		internal static IBsonTypeConverter GetBsonTypeConverter(Type t)
		{
			if (MongoConfiguration._configuration == null)
			{
				return null;
			}
			return MongoConfiguration._configuration.GetTypeConverterFor(t);
		}
		internal static string GetCollectionName(Type type)
		{
			Type discriminatingTypeFor = MongoDiscriminatedAttribute.GetDiscriminatingTypeFor(type);
			if (discriminatingTypeFor != null)
			{
				return discriminatingTypeFor.Name;
			}
			if (MongoConfiguration._configuration == null)
			{
				return ReflectionHelper.GetScrubbedGenericName(type);
			}
			return MongoConfiguration._configuration.GetConfigurationMap().GetCollectionName(type);
		}
		internal static string GetConnectionString(Type type)
		{
			if (MongoConfiguration._configuration == null)
			{
				return null;
			}
			return MongoConfiguration._configuration.GetConfigurationMap().GetConnectionString(type);
		}
		public static string GetTypeDiscriminator(Type type)
		{
			if (MongoConfiguration._configuration == null)
			{
				return null;
			}
			return MongoConfiguration._configuration.GetConfigurationMap().GetTypeDescriminator(type);
		}
	}
}
