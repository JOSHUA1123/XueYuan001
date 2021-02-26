using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
namespace DataBaseInfo
{
	public class ProcSection : IProcSection, ISqlSection
	{
		private DbProvider dbProvider;
		private DbCommand dbCommand;
		private DbTrans dbTran;
		private int returnValue = -1;
		public int ReturnValue
		{
			get
			{
				return this.returnValue;
			}
		}
		internal ProcSection(string procName, DbProvider dbProvider, DbTrans dbTran)
		{
			this.dbProvider = dbProvider;
			this.dbTran = dbTran;
			this.dbCommand = dbProvider.CreateProcCommand(procName);
		}
		public ProcSection AddParameters(params DbParameter[] parameters)
		{
			this.dbProvider.AddParameter(this.dbCommand, parameters);
			return this;
		}
		public ProcSection AddParameters(params SQLParameter[] parameters)
		{
			this.dbProvider.AddParameter(this.dbCommand, parameters);
			return this;
		}
		public ProcSection AddParameters(IDictionary<string, object> parameters)
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
		public ProcSection AddInputParameter(string parameterName, DbType dbType, int size, object value)
		{
			this.dbProvider.AddInputParameter(this.dbCommand, parameterName, dbType, size, value);
			return this;
		}
		public ProcSection AddInputParameter(string parameterName, DbType dbType, object value)
		{
			this.dbProvider.AddInputParameter(this.dbCommand, parameterName, dbType, value);
			return this;
		}
		public ProcSection AddOutputParameter(string parameterName, DbType dbType, int size)
		{
			this.dbProvider.AddOutputParameter(this.dbCommand, parameterName, dbType, size);
			return this;
		}
		public ProcSection AddOutputParameter(string parameterName, DbType dbType)
		{
			this.dbProvider.AddOutputParameter(this.dbCommand, parameterName, dbType);
			return this;
		}
		public ProcSection AddInputOutputParameter(string parameterName, DbType dbType, int size, object value)
		{
			this.dbProvider.AddInputOutputParameter(this.dbCommand, parameterName, dbType, size, value);
			return this;
		}
		public ProcSection AddInputOutputParameter(string parameterName, DbType dbType, object value)
		{
			this.dbProvider.AddInputOutputParameter(this.dbCommand, parameterName, dbType, value);
			return this;
		}
		public ProcSection AddReturnValueParameter(string parameterName, DbType dbType, int size)
		{
			this.dbProvider.AddReturnValueParameter(this.dbCommand, parameterName, dbType, size);
			return this;
		}
		public ProcSection AddReturnValueParameter(string parameterName, DbType dbType)
		{
			this.dbProvider.AddReturnValueParameter(this.dbCommand, parameterName, dbType);
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
			object value = this.ToScalar();
			return CoreHelper.ConvertValue<TResult>(value);
		}
		public int Execute(out IDictionary<string, object> outValues)
		{
			int result = this.dbProvider.ExecuteNonQuery(this.dbCommand, this.dbTran);
			this.GetOutputParameterValues(this.dbCommand, out outValues);
			return result;
		}
		public T ToFirst<T>(out IDictionary<string, object> outValues) where T : class
		{
			ISourceList<T> sourceList = this.ToList<T>();
			this.GetOutputParameterValues(this.dbCommand, out outValues);
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
		public SourceList<T> ToList<T>(out IDictionary<string, object> outValues) where T : class
		{
			SourceList<T> result = this.ToList<T>();
			this.GetOutputParameterValues(this.dbCommand, out outValues);
			return result;
		}
		public ArrayList<TResult> ToListResult<TResult>(out IDictionary<string, object> outValues)
		{
			ArrayList<TResult> listResult = this.GetListResult<TResult>(this.dbCommand, this.dbTran);
			this.GetOutputParameterValues(this.dbCommand, out outValues);
			return listResult;
		}
		public SourceReader ToReader(out IDictionary<string, object> outValues)
		{
			SourceReader result = this.dbProvider.ExecuteReader(this.dbCommand, this.dbTran);
			this.GetOutputParameterValues(this.dbCommand, out outValues);
			return result;
		}
		public SourceTable ToTable(out IDictionary<string, object> outValues)
		{
			DataTable dt = this.dbProvider.ExecuteDataTable(this.dbCommand, this.dbTran);
			this.GetOutputParameterValues(this.dbCommand, out outValues);
			return new SourceTable(dt);
		}
		public DataSet ToDataSet(out IDictionary<string, object> outValues)
		{
			DataSet result = this.dbProvider.ExecuteDataSet(this.dbCommand, this.dbTran);
			this.GetOutputParameterValues(this.dbCommand, out outValues);
			return result;
		}
		public object ToScalar(out IDictionary<string, object> outValues)
		{
			object result = this.dbProvider.ExecuteScalar(this.dbCommand, this.dbTran);
			this.GetOutputParameterValues(this.dbCommand, out outValues);
			return result;
		}
		public TResult ToScalar<TResult>(out IDictionary<string, object> outValues)
		{
			object value = this.ToScalar(out outValues);
			return CoreHelper.ConvertValue<TResult>(value);
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
		private void GetOutputParameterValues(DbCommand cmd, out IDictionary<string, object> outValues)
		{
			try
			{
				outValues = new Dictionary<string, object>();
				foreach (DbParameter dbParameter in cmd.Parameters)
				{
					if (dbParameter.Direction != ParameterDirection.Input)
					{
						if (dbParameter.Value == DBNull.Value)
						{
							dbParameter.Value = null;
						}
						if (dbParameter.Direction == ParameterDirection.ReturnValue)
						{
							this.returnValue = CoreHelper.ConvertValue<int>(dbParameter.Value);
						}
						outValues.Add(dbParameter.ParameterName.Substring(1), dbParameter.Value);
					}
				}
			}
			catch
			{
				throw;
			}
		}
	}
}
