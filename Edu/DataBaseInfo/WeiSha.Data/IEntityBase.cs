using System;
using System.Collections.Generic;
namespace DataBaseInfo
{
	public interface IEntityBase
	{
		TResult As<TResult>();
		IRowReader ToRowReader();
		IDictionary<string, object> ToDictionary();
		EntityState GetObjectState();
		object GetValue(string propertyName);
		void SetValue(string propertyName, object value);
		object GetValue(Field field);
		void SetValue(Field field, object value);
		Field GetField(string propertyName);
	}
}
