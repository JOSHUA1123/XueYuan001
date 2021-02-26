using Norm.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
namespace Norm.BSON
{
	public class IdPropertyFinder
	{
		private enum IdType
		{
			MapDefined,
			AttributeDefined,
			MongoDefault,
			Conventional
		}
		private readonly Dictionary<IdPropertyFinder.IdType, PropertyInfo> _idDictionary;
		private readonly Type _type;
		private PropertyInfo[] _properties;
		private PropertyInfo[] _interfaceProperties;
		public PropertyInfo IdProperty
		{
			get
			{
				this.AddCandidates();
				this.CheckForConflictingCandidates();
				return this._idDictionary.Values.FirstOrDefault((PropertyInfo value) => value != null);
			}
		}
		public IdPropertyFinder(Type type)
		{
			this._type = type;
			this._idDictionary = new Dictionary<IdPropertyFinder.IdType, PropertyInfo>(4)
			{

				{
					IdPropertyFinder.IdType.MongoDefault,
					null
				},

				{
					IdPropertyFinder.IdType.MapDefined,
					null
				},

				{
					IdPropertyFinder.IdType.AttributeDefined,
					null
				},

				{
					IdPropertyFinder.IdType.Conventional,
					null
				}
			};
		}
		public IdPropertyFinder(Type type, PropertyInfo[] properties) : this(type)
		{
			this._properties = properties;
		}
		private bool PropertyIsExplicitlyMappedToId(string idPropertyCandidate)
		{
			Dictionary<Type, Dictionary<string, PropertyMappingExpression>> propertyMaps = MongoTypeConfiguration.PropertyMaps;
			return propertyMaps.ContainsKey(this._type) && propertyMaps[this._type].ContainsKey(idPropertyCandidate) && propertyMaps[this._type][idPropertyCandidate].IsId;
		}
		private void CheckForConflictingCandidates()
		{
			if (this._idDictionary[IdPropertyFinder.IdType.MongoDefault] != null && (this._idDictionary[IdPropertyFinder.IdType.MapDefined] != null || this._idDictionary[IdPropertyFinder.IdType.AttributeDefined] != null))
			{
				throw new MongoConfigurationMapException(this._type.Name + " exposes a property called _id and defines a an Id using MongoIndentifier or by explicit mapping.");
			}
		}
		private static bool HasMongoIdentifierAttribute(ICustomAttributeProvider idPropertyCandidate)
		{
			return idPropertyCandidate.GetCustomAttributes(BsonHelper.MongoIdentifierAttribute, true).Length > 0;
		}
		private bool PropertyIsAttributeDefinedId(MemberInfo idPropertyCandidate)
		{
			if (IdPropertyFinder.HasMongoIdentifierAttribute(idPropertyCandidate))
			{
				return true;
			}
			if (this._interfaceProperties != null)
			{
				IEnumerable<PropertyInfo> enumerable = 
					from propertyInfo in this._interfaceProperties
					where propertyInfo.Name == idPropertyCandidate.Name
					select propertyInfo;
				foreach (PropertyInfo current in enumerable)
				{
					if (IdPropertyFinder.HasMongoIdentifierAttribute(current))
					{
						return true;
					}
				}
				return false;
			}
			return false;
		}
		private void AddCandidate(PropertyInfo property)
		{
			if (this.PropertyIsExplicitlyMappedToId(property.Name))
			{
				this._idDictionary[IdPropertyFinder.IdType.MapDefined] = property;
				return;
			}
			if (this.PropertyIsAttributeDefinedId(property))
			{
				this._idDictionary[IdPropertyFinder.IdType.AttributeDefined] = property;
				return;
			}
			if (property.Name.Equals("_id", StringComparison.InvariantCultureIgnoreCase))
			{
				this._idDictionary[IdPropertyFinder.IdType.MongoDefault] = property;
				return;
			}
			if (property.Name.Equals("Id", StringComparison.InvariantCultureIgnoreCase))
			{
				this._idDictionary[IdPropertyFinder.IdType.Conventional] = property;
			}
		}
		private void AddCandidates()
		{
			if (this._properties == null)
			{
				this._properties = ReflectionHelper.GetProperties(this._type);
			}
			this._interfaceProperties = ReflectionHelper.GetInterfaceProperties(this._type);
			PropertyInfo[] properties = this._properties;
			for (int i = 0; i < properties.Length; i++)
			{
				PropertyInfo property = properties[i];
				this.AddCandidate(property);
			}
		}
	}
}
