using System;
using System.Reflection;
namespace DataBaseInfo.Converter
{
	public class PropertyHandler
	{
		private FastPropertyGetHandler mGetValue;
		private PropertyInfo mProperty;
		private FastPropertySetHandler mSetValue;
		public FastPropertyGetHandler Get
		{
			get
			{
				return this.mGetValue;
			}
		}
		public PropertyInfo Property
		{
			get
			{
				return this.mProperty;
			}
			set
			{
				this.mProperty = value;
			}
		}
		public FastPropertySetHandler Set
		{
			get
			{
				return this.mSetValue;
			}
		}
		public PropertyHandler(PropertyInfo property)
		{
			if (property.CanWrite)
			{
				this.mSetValue = DynamicCalls.GetPropertySetter(property);
			}
			if (property.CanRead)
			{
				this.mGetValue = DynamicCalls.GetPropertyGetter(property);
			}
			this.mProperty = property;
		}
	}
}
