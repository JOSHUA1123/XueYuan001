using System;
using System.Collections.Generic;
namespace Norm.Configuration
{
	internal class MongoConfigurationTypeMap
	{
		private readonly Dictionary<string, PropertyMappingExpression> _fieldMap = new Dictionary<string, PropertyMappingExpression>();
		internal Dictionary<string, PropertyMappingExpression> FieldMap
		{
			get
			{
				return this._fieldMap;
			}
		}
		internal string CollectionName
		{
			get;
			set;
		}
		internal string ConnectionString
		{
			get;
			set;
		}
	}
}
