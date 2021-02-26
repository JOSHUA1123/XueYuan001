using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Diagnostics;
using System.Text;
using DataBaseInfo.Cache;
using DataBaseInfo.Logger;
namespace DataBaseInfo
{
	public abstract class DbProvider : IDbProvider
	{
		private DbHelper dbHelper;
		private char leftToken;
		private char rightToken;
		private char paramPrefixToken;
		private IExecuteLog logger;
		private bool throwError = true;
		internal bool ThrowError
		{
			set
			{
				this.throwError = value;
			}
		}
		internal IExecuteLog Logger
		{
			set
			{
				this.logger = value;
			}
		}
		internal ICacheDependent Cache
		{
			get;
			set;
		}
		internal int Timeout
		{
			get;
			set;
		}
		protected virtual bool AccessProvider
		{
			get
			{
				return false;
			}
		}
		protected virtual bool AllowAutoIncrement
		{
			get
			{
				return false;
			}
		}
		protected internal abstract bool SupportBatch
		{
			get;
		}
		protected abstract string AutoIncrementValue
		{
			get;
		}
		protected DbProvider(string connectionString, System.Data.Common.DbProviderFactory dbFactory, char leftToken, char rightToken, char paramPrefixToken)
		{
			this.leftToken = leftToken;
			this.rightToken = rightToken;
			this.paramPrefixToken = paramPrefixToken;
			this.dbHelper = new DbHelper(connectionString, dbFactory);
			this.Timeout = -1;
		}
		internal void SetDecryptHandler(DecryptEventHandler handler)
		{
			this.dbHelper.SetDecryptHandler(handler);
		}
		private string FormatParameter(string parameterName)
		{
			if (parameterName.IndexOf(this.paramPrefixToken) == 0)
			{
				return parameterName;
			}
			if (parameterName.IndexOf('$') == 0)
			{
				return this.paramPrefixToken + parameterName.TrimStart(new char[]
				{
					'$'
				});
			}
			return this.paramPrefixToken + parameterName;
		}
		public void AddParameter(DbCommand cmd, DbParameter[] parameters)
		{
			for (int i = 0; i < parameters.Length; i++)
			{
				DbParameter dbParameter = parameters[i];
				if (dbParameter.Value == null)
				{
					dbParameter.Value = DBNull.Value;
				}
				else
				{
					if (dbParameter.Value is Enum)
					{
						dbParameter.Value = Convert.ToInt32(dbParameter.Value);
					}
				}
				cmd.Parameters.Add(dbParameter);
			}
		}
		public void AddParameter(DbCommand cmd, SQLParameter[] parameters)
		{
			if (parameters == null || parameters.Length == 0)
			{
				return;
			}
			List<DbParameter> list = new List<DbParameter>();
			for (int i = 0; i < parameters.Length; i++)
			{
				SQLParameter sQLParameter = parameters[i];
				DbParameter dbParameter = this.CreateParameter(sQLParameter.Name, sQLParameter.Value);
				dbParameter.Direction = sQLParameter.Direction;
				list.Add(dbParameter);
			}
			this.AddParameter(cmd, list.ToArray());
		}
		public void AddInputParameter(DbCommand cmd, string parameterName, DbType dbType, int size, object value)
		{
			this.dbHelper.AddInputParameter(cmd, parameterName, dbType, size, value);
		}
		public void AddInputParameter(DbCommand cmd, string parameterName, DbType dbType, object value)
		{
			this.dbHelper.AddInputParameter(cmd, parameterName, dbType, value);
		}
		public void AddOutputParameter(DbCommand cmd, string parameterName, DbType dbType, int size)
		{
			this.dbHelper.AddOutputParameter(cmd, parameterName, dbType, size);
		}
		public void AddOutputParameter(DbCommand cmd, string parameterName, DbType dbType)
		{
			this.dbHelper.AddOutputParameter(cmd, parameterName, dbType);
		}
		public void AddInputOutputParameter(DbCommand cmd, string parameterName, DbType dbType, int size, object value)
		{
			this.dbHelper.AddInputOutputParameter(cmd, parameterName, dbType, size, value);
		}
		public void AddInputOutputParameter(DbCommand cmd, string parameterName, DbType dbType, object value)
		{
			this.dbHelper.AddInputOutputParameter(cmd, parameterName, dbType, value);
		}
		public void AddReturnValueParameter(DbCommand cmd, string parameterName, DbType dbType, int size)
		{
			this.dbHelper.AddReturnValueParameter(cmd, parameterName, dbType, size);
		}
		public void AddReturnValueParameter(DbCommand cmd, string parameterName, DbType dbType)
		{
			this.dbHelper.AddReturnValueParameter(cmd, parameterName, dbType);
		}
		public DbParameter GetParameter(DbCommand cmd, string parameterName)
		{
			return this.dbHelper.GetParameter(cmd, parameterName);
		}
		internal int ExecuteNonQuery(DbCommand cmd, DbTrans trans)
		{
			this.PrepareCommand(cmd);
			this.BeginExecuteCommand(cmd);
			int num = -1;
			Stopwatch stopwatch = Stopwatch.StartNew();
			try
			{
				if (trans.Connection == null && trans.Transaction == null)
				{
					num = this.dbHelper.ExecuteNonQuery(cmd);
				}
				else
				{
					num = this.dbHelper.ExecuteNonQuery(cmd, trans);
				}
				stopwatch.Stop();
				this.EndExecuteCommand(cmd, num, null, stopwatch.ElapsedMilliseconds);
			}
			catch (Exception ex)
			{
				stopwatch.Stop();
				this.EndExecuteCommand(cmd, null, ex, stopwatch.ElapsedMilliseconds);
				if (this.throwError)
				{
					throw new DataException(DataHelper.GetCommandLog(cmd), ex);
				}
				num = 0;
			}
			return num;
		}
		internal SourceReader ExecuteReader(DbCommand cmd, DbTrans trans)
		{
			this.PrepareCommand(cmd);
			this.BeginExecuteCommand(cmd);
			SourceReader result = null;
			Stopwatch stopwatch = Stopwatch.StartNew();
			try
			{
				IDataReader reader;
				if (trans.Connection == null && trans.Transaction == null)
				{
					reader = this.dbHelper.ExecuteReader(cmd);
				}
				else
				{
					reader = this.dbHelper.ExecuteReader(cmd, trans);
				}
				result = new SourceReader(reader);
				stopwatch.Stop();
				this.EndExecuteCommand(cmd, result, null, stopwatch.ElapsedMilliseconds);
			}
			catch (Exception ex)
			{
				stopwatch.Stop();
				this.EndExecuteCommand(cmd, null, ex, stopwatch.ElapsedMilliseconds);
				if (this.throwError)
				{
					throw new DataException(DataHelper.GetCommandLog(cmd), ex);
				}
				result = null;
			}
			return result;
		}
		internal DataSet ExecuteDataSet(DbCommand cmd, DbTrans trans)
		{
			this.PrepareCommand(cmd);
			this.BeginExecuteCommand(cmd);
			DataSet result = null;
			Stopwatch stopwatch = Stopwatch.StartNew();
			try
			{
				if (trans.Connection == null && trans.Transaction == null)
				{
					result = this.dbHelper.ExecuteDataSet(cmd);
				}
				else
				{
					result = this.dbHelper.ExecuteDataSet(cmd, trans);
				}
				stopwatch.Stop();
				this.EndExecuteCommand(cmd, result, null, stopwatch.ElapsedMilliseconds);
			}
			catch (Exception ex)
			{
				stopwatch.Stop();
				this.EndExecuteCommand(cmd, null, ex, stopwatch.ElapsedMilliseconds);
				if (this.throwError)
				{
					throw new DataException(DataHelper.GetCommandLog(cmd), ex);
				}
				result = null;
			}
			return result;
		}
		internal DataTable ExecuteDataTable(DbCommand cmd, DbTrans trans)
		{
			this.PrepareCommand(cmd);
			this.BeginExecuteCommand(cmd);
			DataTable result = null;
			Stopwatch stopwatch = Stopwatch.StartNew();
			try
			{
				if (trans.Connection == null && trans.Transaction == null)
				{
					result = this.dbHelper.ExecuteDataTable(cmd);
				}
				else
				{
					result = this.dbHelper.ExecuteDataTable(cmd, trans);
				}
				stopwatch.Stop();
				this.EndExecuteCommand(cmd, result, null, stopwatch.ElapsedMilliseconds);
			}
			catch (Exception ex)
			{
				stopwatch.Stop();
				this.EndExecuteCommand(cmd, null, ex, stopwatch.ElapsedMilliseconds);
				if (this.throwError)
				{
					throw new DataException(DataHelper.GetCommandLog(cmd), ex);
				}
				result = null;
			}
			return result;
		}
		internal object ExecuteScalar(DbCommand cmd, DbTrans trans)
		{
			this.PrepareCommand(cmd);
			this.BeginExecuteCommand(cmd);
			object result = null;
			Stopwatch stopwatch = Stopwatch.StartNew();
			try
			{
				if (trans.Connection == null && trans.Transaction == null)
				{
					result = this.dbHelper.ExecuteScalar(cmd);
				}
				else
				{
					result = this.dbHelper.ExecuteScalar(cmd, trans);
				}
				stopwatch.Stop();
				this.EndExecuteCommand(cmd, result, null, stopwatch.ElapsedMilliseconds);
			}
			catch (Exception ex)
			{
				stopwatch.Stop();
				this.EndExecuteCommand(cmd, null, ex, stopwatch.ElapsedMilliseconds);
				if (this.throwError)
				{
					throw new DataException(DataHelper.GetCommandLog(cmd), ex);
				}
				result = null;
			}
			return result;
		}
		private void PrepareCommand(DbCommand cmd)
		{
			cmd.CommandText = this.FormatCommandText(cmd.CommandText);
			if (this.Timeout > 0)
			{
				cmd.CommandTimeout = this.Timeout;
			}
			int num = 0;
			foreach (DbParameter dbParameter in cmd.Parameters)
			{
				string parameterName = dbParameter.ParameterName;
				if (cmd.CommandType == CommandType.Text)
				{
					if (parameterName.Length >= 100)
					{
						dbParameter.ParameterName = this.FormatParameter("p" + num++);
						cmd.CommandText = cmd.CommandText.Replace(parameterName, dbParameter.ParameterName);
					}
					else
					{
						dbParameter.ParameterName = this.FormatParameter(parameterName);
						if (parameterName.StartsWith("$"))
						{
							cmd.CommandText = cmd.CommandText.Replace(parameterName, dbParameter.ParameterName);
						}
					}
				}
				else
				{
					dbParameter.ParameterName = this.FormatParameter(parameterName);
				}
			}
			this.PrepareParameter(cmd);
		}
		public DbConnection CreateConnection()
		{
			return this.dbHelper.CreateConnection();
		}
		public DbParameter CreateParameter()
		{
			return this.dbHelper.CreateParameter();
		}
		internal int Insert<T>(Table table, List<FieldValue> fvlist, DbTrans trans, Field identityfield, string autoIncrementName, bool isOutValue, out object retVal) where T : Entity
		{
			retVal = null;
			T t = CoreHelper.CreateInstance<T>();
			DbCommand dbCommand = this.CreateInsert<T>(table, fvlist, identityfield, autoIncrementName);
			string arg = (table == null) ? t.GetTable().Name : table.Name;
			int result;
			if (identityfield != null)
			{
				if (this.AccessProvider)
				{
					result = this.ExecuteNonQuery(dbCommand, trans);
					if (isOutValue)
					{
						dbCommand = this.CreateSqlCommand(string.Format(this.AutoIncrementValue, identityfield.Name, arg));
						retVal = this.ExecuteScalar(dbCommand, trans);
					}
				}
				else
				{
					if (this.AllowAutoIncrement)
					{
						result = this.ExecuteNonQuery(dbCommand, trans);
						if (isOutValue && !string.IsNullOrEmpty(autoIncrementName))
						{
							dbCommand = this.CreateSqlCommand(string.Format(this.AutoIncrementValue, autoIncrementName));
							retVal = this.ExecuteScalar(dbCommand, trans);
						}
					}
					else
					{
						if (isOutValue)
						{
							DbCommand expr_C4 = dbCommand;
							expr_C4.CommandText = expr_C4.CommandText + ";" + string.Format(this.AutoIncrementValue, identityfield.Name, arg);
							retVal = this.ExecuteScalar(dbCommand, trans);
							result = 1;
						}
						else
						{
							result = this.ExecuteNonQuery(dbCommand, trans);
						}
					}
				}
			}
			else
			{
				result = this.ExecuteNonQuery(dbCommand, trans);
			}
			return result;
		}
		internal DbCommand CreateInsert<T>(Table table, List<FieldValue> fvlist, Field identityfield, string autoIncrementName) where T : Entity
		{
			T t = CoreHelper.CreateInstance<T>();
			if (t.GetReadOnly())
			{
				throw new DataException("只读实体" + typeof(T).Name + "只能用于查询！");
			}
			string str = (table == null) ? t.GetTable().Name : table.Name;
			List<SQLParameter> plist = new List<SQLParameter>();
			StringBuilder sbsql = new StringBuilder();
			StringBuilder sbparam = new StringBuilder();
			if (this.AllowAutoIncrement && !string.IsNullOrEmpty(autoIncrementName))
			{
                if (identityfield != null)
                {
                
				string identityName = this.GetAutoIncrement(autoIncrementName);
				bool exist = false;
				fvlist.ForEach(delegate(FieldValue fv)
				{
					if (fv.IsIdentity)
					{
						fv.Value = new DbValue(identityName);
						fv.IsIdentity = false;
						exist = true;
					}
				});
				if (!exist)
				{
					object value = new DbValue(identityName);
					FieldValue item = new FieldValue(identityfield, value);
					fvlist.Insert(0, item);
				}
                }
			}
			sbsql.Append("INSERT INTO " + str + "(");
			sbparam.Append(" VALUES (");
			fvlist.ForEach(delegate(FieldValue fv)
			{
				if (fv.IsIdentity)
				{
					return;
				}
                sbsql.Append(fv.Field.At((string)null).Name);
				if (this.CheckValue(fv.Value))
				{
					sbparam.Append(DataHelper.FormatValue(fv.Value));
				}
				else
				{
					SQLParameter sQLParameter;
					if (CoreHelper.CheckStructType(fv.Value))
					{
						sQLParameter = this.CreateOrmParameter(DataHelper.FormatValue(fv.Value));
					}
					else
					{
						sQLParameter = this.CreateOrmParameter(fv.Value);
					}
					sbparam.Append(sQLParameter.Name);
					plist.Add(sQLParameter);
				}
				sbsql.Append(",");
				sbparam.Append(",");
			});
			sbsql.Remove(sbsql.Length - 1, 1).Append(")");
			sbparam.Remove(sbparam.Length - 1, 1).Append(")");
			string cmdText = string.Format("{0}{1}", sbsql, sbparam);
			return this.CreateSqlCommand(cmdText, plist.ToArray());
		}
		internal int Delete<T>(Table table, WhereClip where, DbTrans trans) where T : Entity
		{
			DbCommand cmd = this.CreateDelete<T>(table, where);
			return this.ExecuteNonQuery(cmd, trans);
		}
		internal DbCommand CreateDelete<T>(Table table, WhereClip where) where T : Entity
		{
			T t = CoreHelper.CreateInstance<T>();
			if (t.GetReadOnly())
			{
				throw new DataException("只读实体" + typeof(T).Name + "只能用于查询！");
			}
			if (where == null)
			{
				throw new DataException("删除条件不能为null！");
			}
			StringBuilder stringBuilder = new StringBuilder();
			string str = (table == null) ? t.GetTable().Name : table.Name;
			stringBuilder.Append("DELETE FROM " + str);
			if (!DataHelper.IsNullOrEmpty(where))
			{
				stringBuilder.Append(" WHERE " + where.ToString());
			}
			return this.CreateSqlCommand(stringBuilder.ToString(), where.Parameters);
		}
		internal int Update<T>(Table table, List<FieldValue> fvlist, WhereClip where, DbTrans trans) where T : Entity
		{
			DbCommand cmd = this.CreateUpdate<T>(table, fvlist, where);
			return this.ExecuteNonQuery(cmd, trans);
		}
		internal DbCommand CreateUpdate<T>(Table table, List<FieldValue> fvlist, WhereClip where) where T : Entity
		{
			T t = CoreHelper.CreateInstance<T>();
			if (t.GetReadOnly())
			{
				throw new DataException("只读实体" + typeof(T).Name + "只能用于查询！");
			}
			if (where == null)
			{
				throw new DataException("更新条件不能为null！");
			}
			string str = (table == null) ? t.GetTable().Name : table.Name;
			List<SQLParameter> plist = new List<SQLParameter>();
			StringBuilder sb = new StringBuilder();
			sb.Append("UPDATE " + str + " SET ");
			fvlist.ForEach(delegate(FieldValue fv)
			{
				if (fv.IsPrimaryKey || fv.IsIdentity)
				{
					return;
				}
				if (fv.IsChanged)
				{
					if (this.CheckValue(fv.Value))
					{
						sb.Append(fv.Field.At((string)null).Name + " = " + DataHelper.FormatValue(fv.Value));
					}
					else
					{
						SQLParameter sQLParameter;
						if (CoreHelper.CheckStructType(fv.Value))
						{
							sQLParameter = this.CreateOrmParameter(DataHelper.FormatValue(fv.Value));
						}
						else
						{
							sQLParameter = this.CreateOrmParameter(fv.Value);
						}
                        sb.Append(fv.Field.At((string)null).Name + " = " + sQLParameter.Name);
						plist.Add(sQLParameter);
					}
					sb.Append(",");
				}
			});
			sb.Remove(sb.Length - 1, 1);
			if (!DataHelper.IsNullOrEmpty(where))
			{
				sb.Append(" WHERE " + where.ToString());
				plist.AddRange(where.Parameters);
			}
			return this.CreateSqlCommand(sb.ToString(), plist.ToArray());
		}
		internal string FormatCommandText(string cmdText)
		{
			return DataHelper.FormatSQL(cmdText, this.leftToken, this.rightToken, this.AccessProvider);
		}
		internal DbCommand CreateSqlCommand(string cmdText)
		{
			return this.dbHelper.CreateSqlStringCommand(cmdText);
		}
		internal DbCommand CreateSqlCommand(string cmdText, SQLParameter[] parameters)
		{
			DbCommand dbCommand = this.dbHelper.CreateSqlStringCommand(cmdText);
			this.AddParameter(dbCommand, parameters);
			return dbCommand;
		}
		internal DbCommand CreateProcCommand(string procName)
		{
			return this.dbHelper.CreateStoredProcCommand(procName);
		}
		private void BeginExecuteCommand(DbCommand command)
		{
			if (this.logger != null)
			{
				try
				{
					this.logger.Begin(command);
				}
				catch
				{
				}
			}
		}
		private void EndExecuteCommand(DbCommand command, object result, Exception ex, long elapsedTime)
		{
			if (this.logger != null)
			{
				try
				{
					ReturnValue retValue = new ReturnValue
					{
						Data = result,
						Error = ex,
						Count = this.GetCount(result)
					};
					this.logger.End(command, retValue, elapsedTime);
				}
				catch
				{
				}
			}
		}
		private int GetCount(object val)
		{
			if (val == null)
			{
				return 0;
			}
			if (val is ICollection)
			{
				return (val as ICollection).Count;
			}
			if (val is Array)
			{
				return (val as Array).Length;
			}
			if (val is DataTable)
			{
				return (val as DataTable).Rows.Count;
			}
			if (val is DataSet)
			{
				DataSet dataSet = val as DataSet;
				if (dataSet.Tables.Count > 0)
				{
					int num = 0;
					foreach (DataTable dataTable in dataSet.Tables)
					{
						num += dataTable.Rows.Count;
					}
					return num;
				}
			}
			return 1;
		}
		protected virtual string GetAutoIncrement(string name)
		{
			return name;
		}
		protected abstract DbParameter CreateParameter(string parameterName, object val);
		protected abstract void PrepareParameter(DbCommand cmd);
		protected internal abstract QuerySection<T> CreatePageQuery<T>(QuerySection<T> query, int itemCount, int skipCount) where T : Entity;
		private SQLParameter CreateOrmParameter(object value)
		{
			string pName = CoreHelper.MakeUniqueKey(100, "$");
			return new SQLParameter(pName, value);
		}
		private bool CheckValue(object value)
		{
			return value == null || value == DBNull.Value || value is Field || value is DbValue;
		}
	}
}
