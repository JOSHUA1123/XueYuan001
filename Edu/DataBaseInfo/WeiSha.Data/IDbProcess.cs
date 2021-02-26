using System;
namespace DataBaseInfo
{
	internal interface IDbProcess
	{
		int Save<T>(T entity) where T : Entity;
		int Delete<T>(T entity) where T : Entity;
		int Delete<T>(params object[] pkValues) where T : Entity;
		int InsertOrUpdate<T>(T entity, params Field[] fields) where T : Entity;
		int InsertOrUpdate<T>(FieldValue[] fvs, WhereClip where) where T : Entity;
		int Save<T>(Table table, T entity) where T : Entity;
		int Delete<T>(Table table, T entity) where T : Entity;
		int Delete<T>(Table table, params object[] pkValues) where T : Entity;
		int InsertOrUpdate<T>(Table table, T entity, params Field[] fields) where T : Entity;
		int InsertOrUpdate<T>(Table table, FieldValue[] fvs, WhereClip where) where T : Entity;
	}
}
