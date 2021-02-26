using Norm.BSON;
using System;
using System.Collections.Generic;
using System.Linq;
namespace Norm.Responses
{
	public class BaseStatusMessage : IExpando
	{
		private Dictionary<string, object> _properties = new Dictionary<string, object>(0);
		public bool WasSuccessful
		{
			get
			{
				return this._properties.ContainsKey("ok") && (this["ok"].Equals(true) || this["ok"].Equals(1.0));
			}
		}
		public object this[string propertyName]
		{
			get
			{
				return this._properties[propertyName];
			}
			set
			{
				this._properties[propertyName] = value;
			}
		}
		public IEnumerable<ExpandoProperty> AllProperties()
		{
			return 
				from j in this._properties
				select new ExpandoProperty(j.Key, j.Value);
		}
		public void Delete(string propertyName)
		{
			this._properties.Remove(propertyName);
		}
	}
}
