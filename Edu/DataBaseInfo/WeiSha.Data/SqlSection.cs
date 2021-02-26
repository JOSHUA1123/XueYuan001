using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
namespace DataBaseInfo
{
	public class SqlSection : ISqlSection
	{
		private DbProvider dbProvider;
		private DbCommand dbCommand;
		private DbTrans dbTran;
		internal SqlSection(string sql, DbProvider dbProvider, DbTrans dbTran)
		{
			this.dbProvider = dbProvider;
			this.dbTran = dbTran;
			this.dbCommand = dbProvider.CreateSqlCommand(sql);
		}
		public SqlSection AddParameters(params DbParameter[] parameters)
		{
			this.dbProvider.AddParameter(this.dbCommand, parameters);
			return this;
		}
		public SqlSection AddParameters(params SQLParameter[] parameters)
		{
			this.dbProvider.AddParameter(this.dbCommand, parameters);
			return this;
		}
		public SqlSection AddParameters(IDictionary<string, object> parameters)
		{
			if (parameters != null && parameters.Count > 0)
			{
				List<SQLParameter> list = new List<SQLParameter>();
				foreach (KeyValuePair<string, object> current in parameters)
				{
					list.Add(new SQLParameter(current.Key, current.Value));
				}
				return this.AddParameters(list.ToArray());
			}
			return this;
		}
		public SqlSection AddInputParameter(string paramName, DbType dbType, int size, object value)
		{
			this.dbProvider.AddInputParameter(this.dbCommand, paramName, dbType, size, value);
			return this;
		}
		public SqlSection AddInputParameter(string paramName, DbType dbType, object value)
		{
			this.dbProvider.AddInputParameter(this.dbCommand, paramName, dbType, value);
			return this;
		}
		public int Execute()
		{
			return this.dbProvider.ExecuteNonQuery(this.dbCommand, this.dbTran);
		}
		public T ToFirst<T>() where T : class
		{
			ISourceList<T> sourceList = this.ToList<T>();
			if (sourceList.Count == 0)
			{
				return default(T);
			}
			if (sourceList[0] is Entity)
			{
				Entity entity = sourceList[0] as Entity;
				entity.Attach(new Field[0]);
				return entity as T;
			}
			return sourceList[0];
		}
		public SourceList<T> ToList<T>() where T : class
		{
			return this.ToReader().ConvertTo<T>();
		}
		public T[] ToArray<T>() where T : class
		{
			SourceList<T> sourceList = this.ToReader().ConvertTo<T>();
			T[] array = new T[sourceList.Count];
			for (int i = 0; i < array.Length; i++)
			{
				if (sourceList[i] != null)
				{
					array[i] = sourceList[i];
				}
			}
			return array;
		}
		public ArrayList<TResult> ToListResult<TResult>()
		{
			return this.GetListResult<TResult>(this.dbCommand, this.dbTran);
		}
		public SourceReader ToReader()
		{
			return this.dbProvider.ExecuteReader(this.dbCommand, this.dbTran);
		}
		public SourceTable ToTable()
		{
			DataTable dt = this.dbProvider.ExecuteDataTable(this.dbCommand, this.dbTran);
			return new SourceTable(dt);
		}
		public DataSet ToDataSet()
		{
			return this.dbProvider.ExecuteDataSet(this.dbCommand, this.dbTran);
		}
		public object ToScalar()
		{
			return this.dbProvider.ExecuteScalar(this.dbCommand, this.dbTran);
		}
		public TResult ToScalar<TResult>()
		{
			object obj = this.ToScalar();
			if (obj == null)
			{
				return default(TResult);
			}
			return CoreHelper.ConvertValue<TResult>(obj);
		}
		private ArrayList<TResult> GetListResult<TResult>(DbCommand cmd, DbTrans dbTran)
		{
			ArrayList<TResult> result;
			try
			{
				using (ISourceReader sourceReader = this.dbProvider.ExecuteReader(cmd, dbTran))
				{
					ArrayList<TResult> arrayList = new ArrayList<TResult>();
					if (typeof(TResult) == typeof(object[]))
					{
						while (sourceReader.Read())
						{
							List<object> list = new List<object>();
							for (int i = 0; i < sourceReader.FieldCount; i++)
							{
								list.Add(sourceReader.GetValue(i));
							}
							TResult item = (TResult)((object)list.ToArray());
							arrayList.Add(item);
						}
					}
					else
					{
						while (sourceReader.Read())
						{
							arrayList.Add(sourceReader.GetValue<TResult>(0));
						}
					}
					sourceReader.Close();
					result = arrayList;
				}
			}
			catch
			{
				throw;
			}
			return result;
		}
	}
}
