using System;
namespace DataBaseInfo
{
	internal interface IWhereCreator<TCreator> : ITableCreator<TCreator> where TCreator : class
	{
		TCreator AddWhere(Field field, object value);
		TCreator AddWhere(string fieldName, object value);
		TCreator AddWhere(WhereClip where);
		TCreator AddWhere(string where, params SQLParameter[] parameters);
	}
}
