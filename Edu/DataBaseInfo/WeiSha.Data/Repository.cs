using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
namespace DataBaseInfo
{
	public class Repository<T> : IRepository<T> where T : Entity
	{
		private Gateway _dbSession;
		public Gateway DB
		{
			get
			{
				return this._dbSession;
			}
		}
		public Repository()
		{
			this._dbSession = new Gateway("default");
		}
		public Repository(Gateway dbSession)
		{
			this._dbSession = dbSession;
		}
		public IList<T> QueryPageList(WhereClip where, QueryPage query)
		{
			IList<T> list = new List<T>();
			return this._dbSession.From<T>().Where(where).ToList();
		}
		public virtual DataTable QueryPageTable(string sql, SQLParameter[] parameters, QueryPage query)
		{
			return this._dbSession.FromSql(sql, parameters).ToTable();
		}
		public virtual IList<T> QueryPageList(string sql, SQLParameter[] parameters, QueryPage query)
		{
			return this._dbSession.FromSql(sql, parameters).ToList<T>();
		}
		public IList<T> QueryList(WhereClip where, OrderByClip order)
		{
			IList<T> list = new List<T>();
			return this._dbSession.From<T>().Where(where).OrderBy(order).ToList();
		}
		public virtual DataTable QueryTable(string sql, params SQLParameter[] parameters)
		{
			return this._dbSession.FromSql(sql, parameters).ToTable();
		}
		public virtual IList<T> QueryList(string sql, params SQLParameter[] parameters)
		{
			return this._dbSession.FromSql(sql, parameters).ToList<T>();
		}
		public virtual IList<TResult> QueryList<TResult>(string sql, params SQLParameter[] parameters) where TResult : Entity
		{
			return this._dbSession.FromSql(sql, parameters).ToList<TResult>();
		}
		public virtual T QuerySingle(WhereClip where)
		{
			return this._dbSession.From<T>().Where(where).ToFirst();
		}
		public virtual T QuerySingle(string sql, params SQLParameter[] parameters)
		{
			return this._dbSession.FromSql(sql, parameters).ToFirst<T>();
		}
		public virtual TResult QuerySingle<TResult>(string sql, params SQLParameter[] parameters) where TResult : Entity
		{
			return this._dbSession.FromSql(sql, parameters).ToFirst<TResult>();
		}
		public virtual int Save(T model)
		{
			return this._dbSession.Save<T>(model);
		}
		public virtual int Update(IDictionary<Field, object> dict, WhereClip where)
		{
			return this._dbSession.Update<T>(dict.Keys.ToArray<Field>(), dict.Values.ToArray<object>(), where);
		}
		public virtual int Update(Field field, object value, WhereClip where)
		{
			return this._dbSession.Update<T>(field, value, where);
		}
		public virtual int Delete(WhereClip where)
		{
			return this._dbSession.Delete<T>(where);
		}
		public virtual int Count(WhereClip where)
		{
			return this._dbSession.Count<T>(where);
		}
		public virtual TResult Sum<TResult>(Field field, WhereClip where)
		{
			return this._dbSession.Sum<T, TResult>(field, where);
		}
		public virtual TResult Avg<TResult>(Field field, WhereClip where)
		{
			return this._dbSession.Avg<T, TResult>(field, where);
		}
		public virtual bool Exists(WhereClip where)
		{
			return this._dbSession.Exists<T>(where);
		}
	}
}
