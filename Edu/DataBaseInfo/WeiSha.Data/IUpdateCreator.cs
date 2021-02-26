using System;
using System.Collections.Generic;
namespace DataBaseInfo
{
	internal interface IUpdateCreator : IWhereCreator<UpdateCreator>, ITableCreator<UpdateCreator>
	{
		UpdateCreator AddUpdate(IDictionary<string, object> dict);
		UpdateCreator AddUpdate(IDictionary<Field, object> dict);
		UpdateCreator AddUpdate(Field field, object value);
		UpdateCreator AddUpdate(string[] fieldNames, object[] values);
		UpdateCreator AddUpdate(string fieldName, object value);
		UpdateCreator AddUpdate(Field[] fields, object[] values);
		UpdateCreator RemoveUpdate(params string[] fieldNames);
		UpdateCreator RemoveUpdate(params Field[] fields);
		UpdateCreator SetEntity<T>(T entity, bool useKeyWhere) where T : Entity;
	}
}
