using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlTypes;
using Common;
using DataBaseInfo.Design;
namespace DataBaseInfo
{
	public class DbTrans : IDbTrans, IDbProcess, IDisposable
	{
		private DbConnection dbConnection;
		private DbTransaction dbTransaction;
		private DbProvider dbProvider;
		private DbBatch dbBatch;
		internal DbConnection Connection
		{
			get
			{
				return this.dbConnection;
			}
		}
		internal DbTransaction Transaction
		{
			get
			{
				return this.dbTransaction;
			}
		}
		internal DbTrans(DbProvider dbProvider, DbTransaction dbTran)
		{
			this.dbConnection = dbTran.Connection;
			this.dbTransaction = dbTran;
			if (this.dbConnection.State != ConnectionState.Open)
			{
				this.dbConnection.Open();
			}
			this.dbProvider = dbProvider;
			this.dbBatch = new DbBatch(dbProvider, this);
		}
		internal DbTrans(DbProvider dbProvider, DbConnection dbConnection)
		{
			this.dbConnection = dbConnection;
			if (this.dbConnection.State != ConnectionState.Open)
			{
				this.dbConnection.Open();
			}
			this.dbProvider = dbProvider;
			this.dbBatch = new DbBatch(dbProvider, this);
		}
		internal DbTrans(DbProvider dbProvider, bool useTrans)
		{
			if (useTrans)
			{
				this.dbConnection = dbProvider.CreateConnection();
				this.dbConnection.Open();
				this.dbTransaction = this.dbConnection.BeginTransaction();
			}
			this.dbProvider = dbProvider;
			this.dbBatch = new DbBatch(dbProvider, this);
		}
		internal DbTrans(DbProvider dbProvider, IsolationLevel isolationLevel)
		{
			this.dbConnection = dbProvider.CreateConnection();
			this.dbConnection.Open();
			this.dbTransaction = this.dbConnection.BeginTransaction(isolationLevel);
			this.dbProvider = dbProvider;
			this.dbBatch = new DbBatch(dbProvider, this);
		}
		public DbBatch BeginBatch()
		{
			return this.BeginBatch(10);
		}
		public DbBatch BeginBatch(int batchSize)
		{
			return new DbBatch(this.dbProvider, this, batchSize);
		}
		public int Save<T>(Table table, T entity) where T : Entity
		{
			return this.dbBatch.Save<T>(table, entity);
		}
		public int Insert<T>(Table table, Field[] fields, object[] values) where T : Entity
		{
			List<FieldValue> fvlist = DataHelper.CreateFieldValue(fields, values, true);
			object obj;
			return this.dbBatch.Insert<T>(table, fvlist, out obj);
		}
		public int Insert<T, TResult>(Table table, Field[] fields, object[] values, out TResult retVal) where T : Entity
		{
			List<FieldValue> fvlist = DataHelper.CreateFieldValue(fields, values, true);
			object value;
			int result = this.dbBatch.Insert<T>(table, fvlist, out value);
			retVal = CoreHelper.ConvertValue<TResult>(value);
			return result;
		}
		public int Delete<T>(Table table, T entity) where T : Entity
		{
			return this.dbBatch.Delete<T>(table, entity);
		}
		public int Delete<T>(Table table, params object[] pkValues) where T : Entity
		{
			return this.dbBatch.Delete<T>(table, pkValues);
		}
		public int Save<T>(T entity) where T : Entity
		{
			return this.dbBatch.Save<T>(entity);
		}
		public int Insert<T>(Field[] fields, object[] values) where T : Entity
		{
			return this.Insert<T>(null, fields, values);
		}
		public int Insert<T, TResult>(Field[] fields, object[] values, out TResult retVal) where T : Entity
		{
			return this.Insert<T, TResult>(null, fields, values, out retVal);
		}
		public int Delete<T>(T entity) where T : Entity
		{
			return this.dbBatch.Delete<T>(entity);
		}
		public int Delete<T>(params object[] pkValues) where T : Entity
		{
			return this.dbBatch.Delete<T>(pkValues);
		}
		public int InsertOrUpdate<T>(T entity, params Field[] fields) where T : Entity
		{
			return this.dbBatch.InsertOrUpdate<T>(entity, fields);
		}
		public int InsertOrUpdate<T>(FieldValue[] fvs, WhereClip where) where T : Entity
		{
			return this.dbBatch.InsertOrUpdate<T>(fvs, where);
		}
		public void Commit()
		{
			try
			{
				this.dbTransaction.Commit();
			}
			catch
			{
				this.Close();
			}
		}
		public void Rollback()
		{
			try
			{
				this.dbTransaction.Rollback();
			}
			catch
			{
				this.Close();
			}
		}
		public void Dispose()
		{
			this.Close();
		}
		public void Close()
		{
			if (this.dbConnection.State != ConnectionState.Closed)
			{
				this.dbConnection.Close();
				this.dbConnection.Dispose();
			}
		}
		public T Single<T>(Table table, params object[] pkValues) where T : Entity
		{
			WhereClip pkWhere = DataHelper.GetPkWhere<T>(table, pkValues);
			return this.Single<T>(table, pkWhere);
		}
		public T Single<T>(Table table, WhereClip where) where T : Entity
		{
			return this.From<T>(table).Where(where).ToFirst();
		}
		public bool Exists<T>(Table table, T entity) where T : Entity
		{
			WhereClip pkWhere = DataHelper.GetPkWhere<T>(table, entity);
			return this.Exists<T>(table, pkWhere);
		}
		public bool Exists<T>(Table table, params object[] pkValues) where T : Entity
		{
			WhereClip pkWhere = DataHelper.GetPkWhere<T>(table, pkValues);
			return this.Exists<T>(table, pkWhere);
		}
		public bool Exists<T>(Table table, WhereClip where) where T : Entity
		{
			return this.Count<T>(table, where) > 0;
		}
		public int Count<T>(Table table, WhereClip where) where T : Entity
		{
			return this.From<T>(table).Where(where).Count();
		}
		public object Sum<T>(Table table, Field field, WhereClip where) where T : Entity
		{
			return this.From<T>(table).Select(new Field[]
			{
				field.Sum()
			}).Where(where).ToScalar();
		}
		public object Avg<T>(Table table, Field field, WhereClip where) where T : Entity
		{
			return this.From<T>(table).Select(new Field[]
			{
				field.Avg()
			}).Where(where).ToScalar();
		}
		public object Max<T>(Table table, Field field, WhereClip where) where T : Entity
		{
			return this.From<T>(table).Select(new Field[]
			{
				field.Max()
			}).Where(where).ToScalar();
		}
		public object Min<T>(Table table, Field field, WhereClip where) where T : Entity
		{
			return this.From<T>(table).Select(new Field[]
			{
				field.Min()
			}).Where(where).ToScalar();
		}
		public TResult Sum<T, TResult>(Table table, Field field, WhereClip where) where T : Entity
		{
			return this.From<T>(table).Select(new Field[]
			{
				field.Sum()
			}).Where(where).ToScalar<TResult>();
		}
		public TResult Avg<T, TResult>(Table table, Field field, WhereClip where) where T : Entity
		{
			return this.From<T>(table).Select(new Field[]
			{
				field.Avg()
			}).Where(where).ToScalar<TResult>();
		}
		public TResult Max<T, TResult>(Table table, Field field, WhereClip where) where T : Entity
		{
			return this.From<T>(table).Select(new Field[]
			{
				field.Max()
			}).Where(where).ToScalar<TResult>();
		}
		public TResult Min<T, TResult>(Table table, Field field, WhereClip where) where T : Entity
		{
			return this.From<T>(table).Select(new Field[]
			{
				field.Min()
			}).Where(where).ToScalar<TResult>();
		}
		public T Single<T>(params object[] pkValues) where T : Entity
		{
			return this.Single<T>(null, pkValues);
		}
		public T Single<T>(WhereClip where) where T : Entity
		{
			return this.Single<T>(null, where);
		}
		public bool Exists<T>(T entity) where T : Entity
		{
			return this.Exists<T>(null, entity);
		}
		public bool Exists<T>(params object[] pkValues) where T : Entity
		{
			return this.Exists<T>(null, pkValues);
		}
		public bool Exists<T>(WhereClip where) where T : Entity
		{
			return this.Exists<T>(null, where);
		}
		public int Count<T>(WhereClip where) where T : Entity
		{
			return this.Count<T>(null, where);
		}
		public object Sum<T>(Field field, WhereClip where) where T : Entity
		{
			return this.Sum<T>(null, field, where);
		}
		public object Avg<T>(Field field, WhereClip where) where T : Entity
		{
			return this.Avg<T>(null, field, where);
		}
		public object Max<T>(Field field, WhereClip where) where T : Entity
		{
			return this.Max<T>(null, field, where);
		}
		public object Min<T>(Field field, WhereClip where) where T : Entity
		{
			return this.Min<T>(null, field, where);
		}
		public TResult Sum<T, TResult>(Field field, WhereClip where) where T : Entity
		{
			return this.Sum<T, TResult>(null, field, where);
		}
		public TResult Avg<T, TResult>(Field field, WhereClip where) where T : Entity
		{
			return this.Avg<T, TResult>(null, field, where);
		}
		public TResult Max<T, TResult>(Field field, WhereClip where) where T : Entity
		{
			return this.Max<T, TResult>(null, field, where);
		}
		public TResult Min<T, TResult>(Field field, WhereClip where) where T : Entity
		{
			return this.Min<T, TResult>(null, field, where);
		}
		public FromSection<T> From<T>() where T : Entity
		{
			try
			{
				if (string.IsNullOrWhiteSpace(License.Value.ServerDomain))
				{
					License.Value.ServerDomain = Server.Domain;
				}
				if (string.IsNullOrWhiteSpace(License.Value.ServerPort))
				{
					License.Value.ServerPort = Server.Port;
				}
			}
			catch
			{
			}
            return this.From<T>((string)null);
		}
		public FromSection<T> From<T>(Table table) where T : Entity
		{
			return new FromSection<T>(this.dbProvider, this, table, null);
		}
		public FromSection<T> From<T>(string aliasName) where T : Entity
		{
			return new FromSection<T>(this.dbProvider, this, null, aliasName);
		}
		public QuerySection<T> From<T>(TableRelation<T> relation) where T : Entity
		{
			QuerySection<T> querySection = new QuerySection<T>(relation.GetFromSection(), this.dbProvider, this);
			if (relation.GetTopSize() > 0)
			{
				querySection = querySection.GetTop(relation.GetTopSize());
			}
			return querySection;
		}
		public SqlSection FromSql(string sql, params SQLParameter[] parameters)
		{
			SqlSection sqlSection = new SqlSection(sql, this.dbProvider, this);
			return sqlSection.AddParameters(parameters);
		}
		public ProcSection FromProc(string procName, params SQLParameter[] parameters)
		{
			ProcSection procSection = new ProcSection(procName, this.dbProvider, this);
			return procSection.AddParameters(parameters);
		}
		public SqlSection FromSql(string sql, IDictionary<string, object> parameters)
		{
			SqlSection sqlSection = new SqlSection(sql, this.dbProvider, this);
			return sqlSection.AddParameters(parameters);
		}
		public ProcSection FromProc(string procName, IDictionary<string, object> parameters)
		{
			ProcSection procSection = new ProcSection(procName, this.dbProvider, this);
			return procSection.AddParameters(parameters);
		}
		public int Execute(InsertCreator creator)
		{
			if (creator.Table == null)
			{
				throw new DataException("用创建器操作时，表不能为null！");
			}
			object obj;
			return this.dbProvider.Insert<ViewEntity>(creator.Table, creator.FieldValues, this, creator.IdentityField, creator.SequenceName, false, out obj);
		}
		public int Execute<TResult>(InsertCreator creator, out TResult identityValue)
		{
			identityValue = default(TResult);
			if (creator.Table == null)
			{
				throw new DataException("用创建器操作时，表不能为null！");
			}
			if (creator.IdentityField == null)
			{
				throw new DataException("返回主键值时需要设置KeyField！");
			}
			object value;
			int result = this.dbProvider.Insert<ViewEntity>(creator.Table, creator.FieldValues, this, creator.IdentityField, creator.SequenceName, true, out value);
			identityValue = CoreHelper.ConvertValue<TResult>(value);
			return result;
		}
		public int Execute(DeleteCreator creator)
		{
			if (creator.Table == null)
			{
				throw new DataException("用创建器操作时，表不能为null！");
			}
			if (DataHelper.IsNullOrEmpty(creator.Where))
			{
				throw new DataException("用删除创建器操作时，条件不能为空！");
			}
			return this.Delete<ViewEntity>(creator.Table, creator.Where);
		}
		public int Execute(UpdateCreator creator)
		{
			if (creator.Table == null)
			{
				throw new DataException("用创建器操作时，表不能为null！");
			}
			if (DataHelper.IsNullOrEmpty(creator.Where))
			{
				throw new DataException("用更新创建器操作时，条件不能为空！");
			}
			return this.Update<ViewEntity>(creator.Table, creator.Fields, creator.Values, creator.Where);
		}
		public QuerySection From(QueryCreator creator)
		{
			if (creator.Table == null)
			{
				throw new DataException("用创建器操作时，表不能为null！");
			}
			FromSection<ViewEntity> fromSection = this.From<ViewEntity>(creator.Table);
			if (creator.IsRelation)
			{
				foreach (TableJoin current in creator.Relations.Values)
				{
					if (current.Type == JoinType.LeftJoin)
					{
						fromSection.LeftJoin<ViewEntity>(current.Table, current.Where);
					}
					else
					{
						if (current.Type == JoinType.RightJoin)
						{
							fromSection.RightJoin<ViewEntity>(current.Table, current.Where);
						}
						else
						{
							fromSection.InnerJoin<ViewEntity>(current.Table, current.Where);
						}
					}
				}
			}
			QuerySection<ViewEntity> query = fromSection.Select(creator.Fields).Where(creator.Where).OrderBy(creator.OrderBy);
			return new QuerySection(query);
		}
		public int Delete<T>(WhereClip where) where T : Entity
		{
			return this.Delete<T>(null, where);
		}
		public int Update<T>(Field field, object value, WhereClip where) where T : Entity
		{
			return this.Update<T>(null, field, value, where);
		}
		public int Update<T>(Field[] fields, object[] values, WhereClip where) where T : Entity
		{
			return this.Update<T>(null, fields, values, where);
		}
		public int Delete<T>(Table table, WhereClip where) where T : Entity
		{
			return this.dbProvider.Delete<T>(table, where, this);
		}
		public int InsertOrUpdate<T>(Table table, T entity, params Field[] fields) where T : Entity
		{
			return this.dbBatch.InsertOrUpdate<T>(table, entity, fields);
		}
		public int InsertOrUpdate<T>(Table table, FieldValue[] fvs, WhereClip where) where T : Entity
		{
			return this.dbBatch.InsertOrUpdate<T>(table, fvs, where);
		}
		public int Update<T>(Table table, Field field, object value, WhereClip where) where T : Entity
		{
			return this.Update<T>(table, new Field[]
			{
				field
			}, new object[]
			{
				value
			}, where);
		}
		public int Update<T>(Table table, Field[] fields, object[] values, WhereClip where) where T : Entity
		{
			for (int i = 0; i < values.Length; i++)
			{
				if (values[i] != null && values[i].GetType().FullName == "System.DateTime")
				{
					DateTime dateTime = (DateTime)values[i];
					if (dateTime < SqlDateTime.MinValue.Value)
					{
						dateTime = SqlDateTime.MinValue.Value;
					}
					if (dateTime > SqlDateTime.MaxValue.Value)
					{
						dateTime = SqlDateTime.MaxValue.Value;
					}
					values[i] = dateTime;
				}
			}
			List<FieldValue> fvlist = DataHelper.CreateFieldValue(fields, values, false);
			return this.dbProvider.Update<T>(table, fvlist, where, this);
		}
		public int Insert<T>(FieldValue[] fvs) where T : Entity
		{
			return this.Insert<T>((Table)null, fvs);
		}
		public int Insert<T, TResult>(FieldValue[] fvs, out TResult retVal) where T : Entity
		{
			return this.Insert<T, TResult>(null, fvs, out retVal);
		}
		public int Update<T>(FieldValue fv, WhereClip where) where T : Entity
		{
			return this.Update<T>(null, fv, where);
		}
		public int Update<T>(FieldValue[] fvs, WhereClip where) where T : Entity
		{
			return this.Update<T>(null, fvs, where);
		}
		public int Insert<T>(Table table, FieldValue[] fvs) where T : Entity
		{
			List<FieldValue> list = new List<FieldValue>(fvs);
			list.ForEach(delegate(FieldValue p)
			{
				if (p.Value is Field)
				{
					p.IsIdentity = true;
				}
			});
			object obj;
			return this.dbBatch.Insert<T>(table, list, out obj);
		}
		public int Insert<T, TResult>(Table table, FieldValue[] fvs, out TResult retVal) where T : Entity
		{
			List<FieldValue> list = new List<FieldValue>(fvs);
			list.ForEach(delegate(FieldValue p)
			{
				if (p.Value is Field)
				{
					p.IsIdentity = true;
				}
			});
			object value;
			int result = this.dbBatch.Insert<T>(table, list, out value);
			retVal = CoreHelper.ConvertValue<TResult>(value);
			return result;
		}
		public int Update<T>(Table table, FieldValue fv, WhereClip where) where T : Entity
		{
			return this.Update<T>(table, new FieldValue[]
			{
				fv
			}, where);
		}
		public int Update<T>(Table table, FieldValue[] fvs, WhereClip where) where T : Entity
		{
			List<FieldValue> list = new List<FieldValue>(fvs);
			list.ForEach(delegate(FieldValue p)
			{
				p.IsChanged = true;
			});
			return this.dbProvider.Update<T>(table, list, where, this);
		}
		public int Insert<T>(T entity, params FieldValue[] fvs) where T : Entity
		{
			return this.Insert<T>(null, entity, fvs);
		}
		public int Insert<T>(Table table, T entity, params FieldValue[] fvs) where T : Entity
		{
			List<FieldValue> fieldValues = entity.GetFieldValues();
			if (fvs != null && fvs.Length > 0)
			{
				FieldValue fv;
				for (int i = 0; i < fvs.Length; i++)
				{
					fv = fvs[i];
					if (fv.Value != null)
					{
						FieldValue fieldValue = fieldValues.Find((FieldValue p) => p.Field.Name == fv.Field.Name);
						if (fieldValue != null)
						{
							if (fv.Value is DbValue && DbValue.Default.Equals(fv.Value))
							{
								fieldValues.Remove(fieldValue);
							}
							fieldValue.Value = fv.Value;
						}
					}
				}
			}
			ValidateResult validateResult = entity.Validation();
			if (!validateResult.IsSuccess)
			{
				List<string> list = new List<string>();
				foreach (InvalidValue current in validateResult.InvalidValues)
				{
					list.Add(current.Field.PropertyName + " : " + current.Message);
				}
				string message = string.Join("\r\n", list.ToArray());
				throw new DataException(message);
			}
			object obj;
			int result = this.Insert<T, object>(table, fieldValues.ToArray(), out obj);
			if (obj != null )
			{
                if (entity.IdentityField != null)
                {
                    CoreHelper.SetPropertyValue(entity, entity.IdentityField.PropertyName, obj);
                }
				
			}
			return result;
		}
	}
}
