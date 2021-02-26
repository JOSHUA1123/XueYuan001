using System;
namespace DataBaseInfo
{
	public interface IEntityInfo
	{
		Table Table
		{
			get;
		}
		Field[] Fields
		{
			get;
		}
		FieldValue[] FieldValues
		{
			get;
		}
		Field[] UpdateFields
		{
			get;
		}
		FieldValue[] UpdateFieldValues
		{
			get;
		}
		bool IsUpdate
		{
			get;
		}
		bool IsReadOnly
		{
			get;
		}
	}
}
