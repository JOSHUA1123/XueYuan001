using System;
namespace DataBaseInfo
{
	public interface IFieldFilter
	{
		Field[] GetFields(Field[] fields);
	}
}
