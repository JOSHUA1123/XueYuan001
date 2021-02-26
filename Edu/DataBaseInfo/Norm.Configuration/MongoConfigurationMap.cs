using Norm.BSON;
using Norm.BSON.DbTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
namespace Norm.Configuration
{
	public class MongoConfigurationMap : IMongoConfigurationMap, IHideObjectMembers
	{
		private Dictionary<Type, string> _idProperties = new Dictionary<Type, string>();
		private IDictionary<Type, IBsonTypeConverter> TypeConverters = new Dictionary<Type, IBsonTypeConverter>();
		public void For<T>(Action<ITypeConfiguration<T>> typeConfigurationAction)
		{
			MongoTypeConfiguration<T> obj = new MongoTypeConfiguration<T>();
			typeConfigurationAction(obj);
		}
		public void TypeConverterFor<TClr, TCnv>() where TCnv : IBsonTypeConverter, new()
		{
			Type typeFromHandle = typeof(TClr);
			Type typeFromHandle2 = typeof(TCnv);
			if (this.TypeConverters.ContainsKey(typeFromHandle))
			{
				throw new ArgumentException(string.Format("The type '{0}' has already a type converter registered ({1}). You are trying to register '{2}'", typeFromHandle, this.TypeConverters[typeFromHandle], typeFromHandle2));
			}
			this.TypeConverters.Add(typeFromHandle, (default(TCnv) == null) ? Activator.CreateInstance<TCnv>() : default(TCnv));
		}
		public IBsonTypeConverter GetTypeConverterFor(Type t)
		{
			IBsonTypeConverter result = null;
			this.TypeConverters.TryGetValue(t, out result);
			return result;
		}
		public void RemoveTypeConverterFor<TClr>()
		{
			this.TypeConverters.Remove(typeof(TClr));
		}
		private bool IsIdPropertyForType(Type type, string propertyName)
		{
			bool result = false;
			if (!this._idProperties.ContainsKey(type))
			{
				PropertyInfo propertyInfo = ReflectionHelper.FindIdProperty(type);
				if (propertyInfo != null)
				{
					this._idProperties[type] = propertyInfo.Name;
					result = (propertyInfo.Name == propertyName);
				}
			}
			else
			{
				result = (this._idProperties[type] == propertyName);
			}
			return result;
		}
		private static bool IsDbReference(Type type)
		{
			return type.IsGenericType && (type.GetGenericTypeDefinition() == typeof(DbReference<>) || type.GetGenericTypeDefinition() == typeof(DbReference<, >));
		}
		public string GetPropertyAlias(Type type, string propertyName)
		{
			Dictionary<Type, Dictionary<string, PropertyMappingExpression>> propertyMaps = MongoTypeConfiguration.PropertyMaps;
			string result = propertyName;
			Type discriminatingTypeFor = MongoDiscriminatedAttribute.GetDiscriminatingTypeFor(type);
			if (this.IsIdPropertyForType(type, propertyName) && !MongoConfigurationMap.IsDbReference(type))
			{
				result = "_id";
			}
			else
			{
				if (propertyMaps.ContainsKey(type) && propertyMaps[type].ContainsKey(propertyName))
				{
					result = propertyMaps[type][propertyName].Alias;
				}
				else
				{
					if (discriminatingTypeFor != null && discriminatingTypeFor != type)
					{
						result = this.GetPropertyAlias(discriminatingTypeFor, propertyName);
					}
				}
			}
			return result;
		}
		public string GetTypeDescriminator(Type type)
		{
			IEnumerable<Type> inheritanceChain = this.GetInheritanceChain(type);
			Dictionary<Type, bool> discriminatedTypes = MongoTypeConfiguration.DiscriminatedTypes;
			Type left = inheritanceChain.FirstOrDefault((Type t) => discriminatedTypes.ContainsKey(t) && discriminatedTypes[t]);
			if (left != null)
			{
				return string.Join(",", type.AssemblyQualifiedName.Split(new char[]
				{
					','
				}), 0, 2);
			}
			return null;
		}
		private IEnumerable<Type> GetInheritanceChain(Type type)
		{
			List<Type> list = new List<Type>
			{
				type
			};
			if (type == typeof(object))
			{
				return list;
			}
			list.AddRange(type.GetInterfaces());
			while (type.BaseType != typeof(object))
			{
				list.Add(type.BaseType);
				list.AddRange(type.BaseType.GetInterfaces());
				type = type.BaseType;
			}
			return list;
		}
		public string GetCollectionName(Type type)
		{
			string scrubbedGenericName;
			if (!MongoTypeConfiguration.CollectionNames.TryGetValue(type, out scrubbedGenericName))
			{
				scrubbedGenericName = ReflectionHelper.GetScrubbedGenericName(type);
			}
			return scrubbedGenericName;
		}
		public string GetConnectionString(Type type)
		{
			Dictionary<Type, string> connectionStrings = MongoTypeConfiguration.ConnectionStrings;
			if (!connectionStrings.ContainsKey(type))
			{
				return null;
			}
			return connectionStrings[type];
		}
		public void RemoveFor<T>()
		{
			MongoTypeConfiguration.RemoveMappings<T>();
		}
		Type IHideObjectMembers.GetType()
		{
			return base.GetType();
		}
	}
}
