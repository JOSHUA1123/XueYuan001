using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using DataBaseInfo.Cache;
using DataBaseInfo.Logger;
namespace DataBaseInfo
{
	public class Gateway : IDbSession, IDbTrans, IDbProcess, ICacheDependent
	{
		public static Gateway Default;
		private DbProvider dbProvider;
		private DbTrans dbTrans;
		private string connectName;
		private static DateTime _calCountTime;
		public static bool IsCorrect
		{
			get
			{
				bool result = false;
				Gateway gateway = new Gateway(DbProviderFactory.Default);
				DbConnection dbConnection = gateway.CreateConnection();
				try
				{
					dbConnection.Open();
					result = true;
				}
				catch
				{
					result = false;
				}
				finally
				{
					if (dbConnection.State == ConnectionState.Open)
					{
						dbConnection.Close();
					}
				}

				return result;
			}
		}
		static Gateway()
		{
			Gateway._calCountTime = DateTime.Now;
			if (Gateway.Default == null)
			{
				try
				{
					Gateway.Default = new Gateway(DbProviderFactory.Default);
				}
				catch
				{
				}
			}
		}
		public Gateway(string connectName) : this(DbProviderFactory.CreateDbProvider(connectName))
		{
			this.connectName = connectName;
		}
		public Gateway(DbProvider dbProvider)
		{
			try
			{
				this.connectName = dbProvider.ToString();
				this.InitSession(dbProvider);
			}
			catch
			{
				throw new DataException("初始化数据库操作对象失败，请检查配置是否正确！");
			}
		}
		public static void SetDefault(string connectName)
		{
			Gateway.Default = new Gateway(connectName);
		}
		public static void SetDefault(DbProvider dbProvider)
		{
			Gateway.Default = new Gateway(dbProvider);
		}
		public static void SetDefault(Gateway dbSession)
		{
			Gateway.Default = dbSession;
		}
		public static int Count(string tablename)
		{
			if ((DateTime.Now - Gateway._calCountTime).TotalSeconds < 3.0)
			{
				return -1;
			}
			Gateway gateway = new Gateway(DbProviderFactory.Default);
			DbConnection dbConnection = gateway.CreateConnection();
			int result = 0;
			try
			{
				if (dbConnection.State == ConnectionState.Closed)
				{
					dbConnection.Open();
				}
				string cmdText = string.Format("SELECT count(*) as 'count' FROM [{0}]", tablename);
				DbCommand dbCommand = new SqlCommand(cmdText, (SqlConnection)dbConnection);
				result = (int)dbCommand.ExecuteScalar();
				Gateway._calCountTime = DateTime.Now;
			}
			catch
			{
			}
			finally
			{
				if (dbConnection.State == ConnectionState.Open)
				{
					dbConnection.Close();
				}
			}
			return result;
		}
		public void SetProvider(string connectName)
		{
			this.SetProvider(DbProviderFactory.CreateDbProvider(connectName));
		}
		public void SetProvider(DbProvider dbProvider)
		{
			this.InitSession(dbProvider);
		}
		public DbTrans BeginTrans()
		{
			return new DbTrans(this.dbProvider, true);
		}
		public DbTrans BeginTrans(IsolationLevel isolationLevel)
		{
			return new DbTrans(this.dbProvider, isolationLevel);
		}
		public DbTrans SetTransaction(DbTransaction trans)
		{
			return new DbTrans(this.dbProvider, trans);
		}
		public DbTransaction BeginTransaction()
		{
			return this.BeginTrans().Transaction;
		}
		public DbTransaction BeginTransaction(IsolationLevel isolationLevel)
		{
			return this.BeginTrans(isolationLevel).Transaction;
		}
		public DbTrans SetConnection(DbConnection connection)
		{
			return new DbTrans(this.dbProvider, connection);
		}
		public DbConnection CreateConnection()
		{
			return this.dbProvider.CreateConnection();
		}
		public DbParameter CreateParameter()
		{
			return this.dbProvider.CreateParameter();
		}
		public T Single<T>(Table table, params object[] pkValues) where T : Entity
		{
			return this.dbTrans.Single<T>(table, pkValues);
		}
		public T Single<T>(Table table, WhereClip where) where T : Entity
		{
			return this.dbTrans.Single<T>(table, where);
		}
		public bool Exists<T>(Table table, T entity) where T : Entity
		{
			return this.dbTrans.Exists<T>(table, entity);
		}
		public bool Exists<T>(Table table, params object[] pkValues) where T : Entity
		{
			return this.dbTrans.Exists<T>(table, pkValues);
		}
		public bool Exists<T>(Table table, WhereClip where) where T : Entity
		{
			return this.dbTrans.Exists<T>(table, where);
		}
		public int Count<T>(Table table, WhereClip where) where T : Entity
		{
			return this.dbTrans.Count<T>(table, where);
		}
		public object Sum<T>(Table table, Field field, WhereClip where) where T : Entity
		{
			return this.dbTrans.Sum<T>(table, field, where);
		}
		public object Avg<T>(Table table, Field field, WhereClip where) where T : Entity
		{
			return this.dbTrans.Avg<T>(table, field, where);
		}
		public object Max<T>(Table table, Field field, WhereClip where) where T : Entity
		{
			return this.dbTrans.Max<T>(table, field, where);
		}
		public object Min<T>(Table table, Field field, WhereClip where) where T : Entity
		{
			return this.dbTrans.Min<T>(table, field, where);
		}
		public TResult Sum<T, TResult>(Table table, Field field, WhereClip where) where T : Entity
		{
			return this.dbTrans.Sum<T, TResult>(table, field, where);
		}
		public TResult Avg<T, TResult>(Table table, Field field, WhereClip where) where T : Entity
		{
			return this.dbTrans.Avg<T, TResult>(table, field, where);
		}
		public TResult Max<T, TResult>(Table table, Field field, WhereClip where) where T : Entity
		{
			return this.dbTrans.Max<T, TResult>(table, field, where);
		}
		public TResult Min<T, TResult>(Table table, Field field, WhereClip where) where T : Entity
		{
			return this.dbTrans.Min<T, TResult>(table, field, where);
		}
		public T Single<T>(params object[] pkValues) where T : Entity
		{
			return this.dbTrans.Single<T>(pkValues);
		}
		public T Single<T>(WhereClip where) where T : Entity
		{
			return this.dbTrans.Single<T>(where);
		}
		public bool Exists<T>(T entity) where T : Entity
		{
			return this.dbTrans.Exists<T>(entity);
		}
		public bool Exists<T>(params object[] pkValues) where T : Entity
		{
			return this.dbTrans.Exists<T>(pkValues);
		}
		public bool Exists<T>(WhereClip where) where T : Entity
		{
			return this.dbTrans.Exists<T>(where);
		}
		public int Count<T>(WhereClip where) where T : Entity
		{
			return this.dbTrans.Count<T>(where);
		}
		public object Sum<T>(Field field, WhereClip where) where T : Entity
		{
			return this.dbTrans.Sum<T>(field, where);
		}
		public object Avg<T>(Field field, WhereClip where) where T : Entity
		{
			return this.dbTrans.Avg<T>(field, where);
		}
		public object Max<T>(Field field, WhereClip where) where T : Entity
		{
			return this.dbTrans.Max<T>(field, where);
		}
		public object Min<T>(Field field, WhereClip where) where T : Entity
		{
			return this.dbTrans.Min<T>(field, where);
		}
		public TResult Sum<T, TResult>(Field field, WhereClip where) where T : Entity
		{
			return this.dbTrans.Sum<T, TResult>(field, where);
		}
		public TResult Avg<T, TResult>(Field field, WhereClip where) where T : Entity
		{
			return this.dbTrans.Avg<T, TResult>(field, where);
		}
		public TResult Max<T, TResult>(Field field, WhereClip where) where T : Entity
		{
			return this.dbTrans.Max<T, TResult>(field, where);
		}
		public TResult Min<T, TResult>(Field field, WhereClip where) where T : Entity
		{
			return this.dbTrans.Min<T, TResult>(field, where);
		}
		public QuerySection<T> From<T>(TableRelation<T> relation) where T : Entity
		{
			return this.dbTrans.From<T>(relation);
		}
		public FromSection<T> From<T>() where T : Entity
		{
			return this.dbTrans.From<T>();
		}
		public FromSection<T> From<T>(Table table) where T : Entity
		{
			return this.dbTrans.From<T>(table);
		}
		public FromSection<T> From<T>(string aliasName) where T : Entity
		{
			return this.dbTrans.From<T>(aliasName);
		}
		public SqlSection FromSql(string sql, params SQLParameter[] parameters)
		{
			return this.dbTrans.FromSql(sql, parameters);
		}
		public ProcSection FromProc(string procName, params SQLParameter[] parameters)
		{
			return this.dbTrans.FromProc(procName, parameters);
		}
		public SqlSection FromSql(string sql, IDictionary<string, object> parameters)
		{
			return this.dbTrans.FromSql(sql, parameters);
		}
		public ProcSection FromProc(string procName, IDictionary<string, object> parameters)
		{
			return this.dbTrans.FromProc(procName, parameters);
		}
		public int Execute(InsertCreator creator)
		{
			return this.dbTrans.Execute(creator);
		}
		public int Execute<TResult>(InsertCreator creator, out TResult identityValue)
		{
			return this.dbTrans.Execute<TResult>(creator, out identityValue);
		}
		public int Execute(DeleteCreator creator)
		{
			return this.dbTrans.Execute(creator);
		}
		public int Execute(UpdateCreator creator)
		{
			return this.dbTrans.Execute(creator);
		}
		public QuerySection From(QueryCreator creator)
		{
			return this.dbTrans.From(creator);
		}
		public DbBatch BeginBatch()
		{
			return this.dbTrans.BeginBatch();
		}
		public DbBatch BeginBatch(int batchSize)
		{
			return this.dbTrans.BeginBatch(batchSize);
		}
		public int Save<T>(Table table, T entity) where T : Entity
		{
			return this.dbTrans.Save<T>(table, entity);
		}
		public int Insert<T>(Table table, Field[] fields, object[] values) where T : Entity
		{
			return this.dbTrans.Insert<T>(table, fields, values);
		}
		public int Insert<T, TResult>(Table table, Field[] fields, object[] values, out TResult retVal) where T : Entity
		{
			return this.dbTrans.Insert<T, TResult>(table, fields, values, out retVal);
		}
		public int Delete<T>(Table table, T entity) where T : Entity
		{
			return this.dbTrans.Delete<T>(table, entity);
		}
		public int Delete<T>(Table table, params object[] pkValues) where T : Entity
		{
			return this.dbTrans.Delete<T>(table, pkValues);
		}
		public int Delete<T>(Table table, WhereClip where) where T : Entity
		{
			return this.dbTrans.Delete<T>(table, where);
		}
		public int InsertOrUpdate<T>(Table table, T entity, params Field[] fields) where T : Entity
		{
			return this.dbTrans.InsertOrUpdate<T>(table, entity, fields);
		}
		public int InsertOrUpdate<T>(Table table, FieldValue[] fvs, WhereClip where) where T : Entity
		{
			return this.dbTrans.InsertOrUpdate<T>(table, fvs, where);
		}
		public int Update<T>(Table table, Field field, object value, WhereClip where) where T : Entity
		{
			return this.dbTrans.Update<T>(table, field, value, where);
		}
		public int Update<T>(Table table, Field[] fields, object[] values, WhereClip where) where T : Entity
		{
			return this.dbTrans.Update<T>(table, fields, values, where);
		}
		public int Save<T>(T entity) where T : Entity
		{
			return this.dbTrans.Save<T>(entity);
		}
		public int Insert<T>(Field[] fields, object[] values) where T : Entity
		{
			return this.dbTrans.Insert<T>(fields, values);
		}
		public int Insert<T, TResult>(Field[] fields, object[] values, out TResult retVal) where T : Entity
		{
			return this.dbTrans.Insert<T, TResult>(fields, values, out retVal);
		}
		public int Delete<T>(T entity) where T : Entity
		{
			return this.dbTrans.Delete<T>(entity);
		}
		public int Delete<T>(params object[] pkValues) where T : Entity
		{
			return this.dbTrans.Delete<T>(pkValues);
		}
		public int Delete<T>(WhereClip where) where T : Entity
		{
			return this.dbTrans.Delete<T>(where);
		}
		public int InsertOrUpdate<T>(T entity, params Field[] fields) where T : Entity
		{
			return this.dbTrans.InsertOrUpdate<T>(entity, fields);
		}
		public int InsertOrUpdate<T>(FieldValue[] fvs, WhereClip where) where T : Entity
		{
			return this.dbTrans.InsertOrUpdate<T>(fvs, where);
		}
		public int Update<T>(Field field, object value, WhereClip where) where T : Entity
		{
			return this.dbTrans.Update<T>(field, value, where);
		}
		public int Update<T>(Field[] fields, object[] values, WhereClip where) where T : Entity
		{
			return this.dbTrans.Update<T>(fields, values, where);
		}
		public string Serialization(WhereClip where)
		{
			string text = this.dbProvider.FormatCommandText(where.ToString());
			SQLParameter[] parameters = where.Parameters;
			for (int i = 0; i < parameters.Length; i++)
			{
				SQLParameter sQLParameter = parameters[i];
				text = text.Replace(sQLParameter.Name, DataHelper.FormatValue(sQLParameter.Value));
			}
			return text;
		}
		public string Serialization(OrderByClip order)
		{
			return this.dbProvider.FormatCommandText(order.ToString());
		}
		public void RegisterDecryptor(DecryptEventHandler handler)
		{
			this.dbProvider.SetDecryptHandler(handler);
		}
		public void RegisterLogger(IExecuteLog logger)
		{
			this.dbProvider.Logger = logger;
		}
		public void RegisterCache(ICacheStrategy cache)
		{
			this.dbProvider.Cache = new DataCacheDependent(cache, this.connectName);
		}
		public void SetCommandTimeout(int timeout)
		{
			this.dbProvider.Timeout = timeout;
		}
		public void SetThrowError(bool throwError)
		{
			this.dbProvider.ThrowError = throwError;
		}
		private void InitSession(DbProvider dbProvider)
		{
			this.dbProvider = dbProvider;
			this.dbTrans = new DbTrans(dbProvider, false);
		}
		public int Insert<T>(FieldValue[] fvs) where T : Entity
		{
			return this.dbTrans.Insert<T>(fvs);
		}
		public int Insert<T, TResult>(FieldValue[] fvs, out TResult retVal) where T : Entity
		{
			return this.dbTrans.Insert<T, TResult>(fvs, out retVal);
		}
		public int Update<T>(FieldValue fv, WhereClip where) where T : Entity
		{
			return this.dbTrans.Update<T>(fv, where);
		}
		public int Update<T>(FieldValue[] fvs, WhereClip where) where T : Entity
		{
			return this.dbTrans.Update<T>(fvs, where);
		}
		public int Insert<T>(Table table, FieldValue[] fvs) where T : Entity
		{
			return this.dbTrans.Insert<T>(table, fvs);
		}
		public int Insert<T, TResult>(Table table, FieldValue[] fvs, out TResult retVal) where T : Entity
		{
			return this.dbTrans.Insert<T, TResult>(table, fvs, out retVal);
		}
		public int Update<T>(Table table, FieldValue fv, WhereClip where) where T : Entity
		{
			return this.dbTrans.Update<T>(table, fv, where);
		}
		public int Update<T>(Table table, FieldValue[] fvs, WhereClip where) where T : Entity
		{
			return this.dbTrans.Update<T>(table, fvs, where);
		}
		public int Insert<T>(T entity, params FieldValue[] fvs) where T : Entity
		{
			return this.dbTrans.Insert<T>(entity, fvs);
		}
		public int Insert<T>(Table table, T entity, params FieldValue[] fvs) where T : Entity
		{
			return this.dbTrans.Insert<T>(table, entity, fvs);
		}
		public void AddCache<T>(string cacheKey, T cacheValue, int cacheTime)
		{
			if (this.dbProvider.Cache != null)
			{
				this.dbProvider.Cache.AddCache<T>(cacheKey, cacheValue, cacheTime);
			}
		}
		public void RemoveCache<T>(string cacheKey)
		{
			if (this.dbProvider.Cache != null)
			{
				this.dbProvider.Cache.RemoveCache<T>(cacheKey);
			}
		}
		public T GetCache<T>(string cacheKey)
		{
			if (this.dbProvider.Cache != null)
			{
				return this.dbProvider.Cache.GetCache<T>(cacheKey);
			}
			return default(T);
		}
	}
}
