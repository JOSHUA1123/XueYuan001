using Norm.BSON;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
namespace Norm.Configuration
{
	public class MongoTypeConfiguration
	{
		private static readonly Dictionary<Type, string> _collectionNames = new Dictionary<Type, string>();
		private static readonly Dictionary<Type, string> _connectionStrings = new Dictionary<Type, string>();
		private static readonly Dictionary<Type, Dictionary<string, PropertyMappingExpression>> _typeConfigurations = new Dictionary<Type, Dictionary<string, PropertyMappingExpression>>();
		private static readonly Dictionary<Type, Type> _summaryTypes = new Dictionary<Type, Type>();
		private static readonly Dictionary<Type, bool> _discriminatedTypes = new Dictionary<Type, bool>();
		internal static Dictionary<Type, Dictionary<string, PropertyMappingExpression>> PropertyMaps
		{
			get
			{
				return MongoTypeConfiguration._typeConfigurations;
			}
		}
		internal static Dictionary<Type, string> ConnectionStrings
		{
			get
			{
				return MongoTypeConfiguration._connectionStrings;
			}
		}
		internal static Dictionary<Type, string> CollectionNames
		{
			get
			{
				return MongoTypeConfiguration._collectionNames;
			}
		}
		internal static Dictionary<Type, bool> DiscriminatedTypes
		{
			get
			{
				return MongoTypeConfiguration._discriminatedTypes;
			}
		}
		internal static void RemoveMappings<T>()
		{
			Type typeFromHandle = typeof(T);
			if (typeFromHandle.Assembly == typeof(MongoTypeConfiguration).Assembly)
			{
				throw new NotSupportedException("You may not remove mappings for Norm types. The type you attempted to remove was " + typeFromHandle.FullName);
			}
			if (MongoTypeConfiguration._typeConfigurations.ContainsKey(typeFromHandle))
			{
				MongoTypeConfiguration._typeConfigurations.Remove(typeFromHandle);
			}
			if (MongoTypeConfiguration._collectionNames.ContainsKey(typeFromHandle))
			{
				MongoTypeConfiguration._collectionNames.Remove(typeFromHandle);
			}
			if (MongoTypeConfiguration._connectionStrings.ContainsKey(typeFromHandle))
			{
				MongoTypeConfiguration._connectionStrings.Remove(typeFromHandle);
			}
			if (MongoTypeConfiguration._discriminatedTypes.ContainsKey(typeFromHandle))
			{
				MongoTypeConfiguration._discriminatedTypes.Remove(typeFromHandle);
			}
		}
	}
	public class MongoTypeConfiguration<T> : MongoTypeConfiguration, ITypeConfiguration<T>, ITypeConfiguration
	{
		private void CheckForPropertyMap(Type typeKey)
		{
			if (!MongoTypeConfiguration.PropertyMaps.ContainsKey(typeKey))
			{
				MongoTypeConfiguration.PropertyMaps.Add(typeKey, new Dictionary<string, PropertyMappingExpression>());
			}
		}
		public IPropertyMappingExpression ForProperty(Expression<Func<T, object>> sourcePropery)
		{
			string text = ReflectionHelper.FindProperty(sourcePropery);
			Type typeFromHandle = typeof(T);
			this.CheckForPropertyMap(typeFromHandle);
			PropertyMappingExpression propertyMappingExpression = new PropertyMappingExpression
			{
				SourcePropertyName = text
			};
			MongoTypeConfiguration.PropertyMaps[typeFromHandle][text] = propertyMappingExpression;
			MongoConfiguration.FireTypeChangedEvent(typeof(T));
			return propertyMappingExpression;
		}
		public void IdIs(Expression<Func<T, object>> idProperty)
		{
			string key = ReflectionHelper.FindProperty(idProperty);
			Type typeFromHandle = typeof(T);
			this.CheckForPropertyMap(typeFromHandle);
			MongoTypeConfiguration.PropertyMaps[typeFromHandle][key] = new PropertyMappingExpression
			{
				IsId = true
			};
		}
		public void UseCollectionNamed(string connectionStrings)
		{
			MongoTypeConfiguration.CollectionNames[typeof(T)] = connectionStrings;
			MongoConfiguration.FireTypeChangedEvent(typeof(T));
		}
		public void UseConnectionString(string connectionString)
		{
			MongoTypeConfiguration.ConnectionStrings[typeof(T)] = connectionString;
			MongoConfiguration.FireTypeChangedEvent(typeof(T));
		}
		public void UseAsDiscriminator()
		{
			MongoTypeConfiguration.DiscriminatedTypes[typeof(T)] = true;
			MongoConfiguration.FireTypeChangedEvent(typeof(T));
		}
	}
}
