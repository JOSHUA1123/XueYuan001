using System;
using System.Collections.Generic;
using System.Data;
namespace DataBaseInfo
{
	public interface IRepository<T> where T : Entity
	{
		IList<T> QueryPageList(WhereClip where, QueryPage query);
		DataTable QueryPageTable(string sql, SQLParameter[] parameters, QueryPage query);
		IList<T> QueryPageList(string sql, SQLParameter[] parameters, QueryPage query);
		IList<T> QueryList(WhereClip where, OrderByClip order);
		DataTable QueryTable(string sql, params SQLParameter[] parameters);
		IList<T> QueryList(string sql, params SQLParameter[] parameters);
		IList<TResult> QueryList<TResult>(string sql, params SQLParameter[] parameters) where TResult : Entity;
		T QuerySingle(WhereClip where);
		T QuerySingle(string sql, params SQLParameter[] parameters);
		TResult QuerySingle<TResult>(string sql, params SQLParameter[] parameters) where TResult : Entity;
		int Save(T model);
		int Update(IDictionary<Field, object> dict, WhereClip where);
		int Update(Field field, object value, WhereClip where);
		int Delete(WhereClip where);
		int Count(WhereClip where);
		TResult Sum<TResult>(Field field, WhereClip where);
		TResult Avg<TResult>(Field field, WhereClip where);
		bool Exists(WhereClip where);
	}
}
