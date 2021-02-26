using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
namespace Norm.BSON
{
	public class Expando : IExpando
	{
		private Dictionary<string, object> _kitchenSinkProps = new Dictionary<string, object>(0, StringComparer.InvariantCultureIgnoreCase);
		public object this[string propertyName]
		{
			get
			{
				return this._kitchenSinkProps[propertyName];
			}
			set
			{
				this.Delete(propertyName);
				this._kitchenSinkProps[propertyName] = value;
			}
		}
		public Expando()
		{
		}
		public Expando(object values)
		{
			this.AddValues(values);
		}
		private void AddValues(object values)
		{
			if (values != null)
			{
				foreach (PropertyDescriptor propertyDescriptor in TypeDescriptor.GetProperties(values))
				{
					object value = propertyDescriptor.GetValue(values);
					this._kitchenSinkProps.Add(propertyDescriptor.Name, value);
				}
			}
		}
		public IEnumerable<ExpandoProperty> AllProperties()
		{
			return 
				from y in this._kitchenSinkProps
				select new ExpandoProperty(y.Key, y.Value);
		}
		public void ReverseKitchen()
		{
			IEnumerable<KeyValuePair<string, object>> enumerable = this._kitchenSinkProps.Reverse<KeyValuePair<string, object>>();
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			foreach (KeyValuePair<string, object> current in enumerable)
			{
				dictionary[current.Key] = current.Value;
			}
			this._kitchenSinkProps = dictionary;
		}
		public T Get<T>(string propertyName)
		{
			object obj;
			this._kitchenSinkProps.TryGetValue(propertyName, out obj);
			if (obj == null)
			{
				throw new InvalidOperationException("Can't find the property " + propertyName);
			}
			return (T)((object)obj);
		}
		public bool Contains(string propertyName)
		{
			return this._kitchenSinkProps.ContainsKey(propertyName);
		}
		public void Delete(string propertyName)
		{
			this._kitchenSinkProps.Remove(propertyName);
		}
		public void Set<T>(string propertyName, T value)
		{
			this.Delete(propertyName);
			this._kitchenSinkProps[propertyName] = value;
		}
		public bool TryGet<T>(string propertyName, out T value)
		{
			bool result = false;
			value = default(T);
			try
			{
				value = (T)((object)this._kitchenSinkProps[propertyName]);
				result = true;
			}
			catch
			{
			}
			return result;
		}
		public Expando Merge(Expando expando)
		{
			foreach (ExpandoProperty current in expando.AllProperties())
			{
				this[current.PropertyName] = current.Value;
			}
			return this;
		}
	}
}
