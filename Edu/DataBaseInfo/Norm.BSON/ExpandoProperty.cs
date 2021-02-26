using System;
namespace Norm.BSON
{
	public class ExpandoProperty
	{
		public string PropertyName
		{
			get;
			private set;
		}
		public object Value
		{
			get;
			private set;
		}
		public ExpandoProperty(string name, object value)
		{
			this.PropertyName = name;
			this.Value = value;
		}
	}
}
