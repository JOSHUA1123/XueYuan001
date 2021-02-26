using System;
using System.Data;
using System.Data.Common;
namespace DataBaseInfo
{
	public sealed class DbHelper
	{
		private DecryptEventHandler handler;
		private System.Data.Common.DbProviderFactory dbFactory;
		private string dbConnectionString;
		public DbHelper(string connectionString, System.Data.Common.DbProviderFactory dbFactory)
		{
			this.dbConnectionString = connectionString;
			this.dbFactory = dbFactory;
		}
		public void SetDecryptHandler(DecryptEventHandler handler)
		{
			this.handler = handler;
		}
		public DbConnection CreateConnection()
		{
			DbConnection dbConnection = this.dbFactory.CreateConnection();
			if (this.handler != null)
			{
				this.dbConnectionString = this.handler(this.dbConnectionString);
				this.handler = null;
			}
			dbConnection.ConnectionString = this.dbConnectionString;
			return dbConnection;
		}
		public DbCommand CreateCommand()
		{
			return this.dbFactory.CreateCommand();
		}
		public DbParameter CreateParameter()
		{
			return this.dbFactory.CreateParameter();
		}
		public DbCommand CreateStoredProcCommand(string procName)
		{
			DbCommand dbCommand = this.dbFactory.CreateCommand();
			dbCommand.CommandType = CommandType.StoredProcedure;
			dbCommand.CommandText = procName;
			return dbCommand;
		}
		public DbCommand CreateSqlStringCommand(string cmdText)
		{
			DbCommand dbCommand = this.dbFactory.CreateCommand();
			dbCommand.CommandType = CommandType.Text;
			dbCommand.CommandText = cmdText;
			return dbCommand;
		}
		public void AddParameterCollection(DbCommand cmd, DbParameterCollection dbParameterCollection)
		{
			foreach (DbParameter value in dbParameterCollection)
			{
				cmd.Parameters.Add(value);
			}
		}
		public void AddInputParameter(DbCommand cmd, string parameterName, DbType dbType, int size, object value)
		{
			DbParameter dbParameter = cmd.CreateParameter();
			dbParameter.DbType = dbType;
			dbParameter.ParameterName = parameterName;
			dbParameter.Size = size;
			dbParameter.Value = value;
			dbParameter.Direction = ParameterDirection.Input;
			cmd.Parameters.Add(dbParameter);
		}
		public void AddInputParameter(DbCommand cmd, string parameterName, DbType dbType, object value)
		{
			DbParameter dbParameter = cmd.CreateParameter();
			dbParameter.DbType = dbType;
			dbParameter.ParameterName = parameterName;
			dbParameter.Value = value;
			dbParameter.Direction = ParameterDirection.Input;
			cmd.Parameters.Add(dbParameter);
		}
		public void AddOutputParameter(DbCommand cmd, string parameterName, DbType dbType, int size)
		{
			DbParameter dbParameter = cmd.CreateParameter();
			dbParameter.DbType = dbType;
			dbParameter.ParameterName = parameterName;
			dbParameter.Size = size;
			dbParameter.Direction = ParameterDirection.Output;
			cmd.Parameters.Add(dbParameter);
		}
		public void AddOutputParameter(DbCommand cmd, string parameterName, DbType dbType)
		{
			DbParameter dbParameter = cmd.CreateParameter();
			dbParameter.DbType = dbType;
			dbParameter.ParameterName = parameterName;
			dbParameter.Direction = ParameterDirection.Output;
			cmd.Parameters.Add(dbParameter);
		}
		public void AddInputOutputParameter(DbCommand cmd, string parameterName, DbType dbType, int size, object value)
		{
			DbParameter dbParameter = cmd.CreateParameter();
			dbParameter.DbType = dbType;
			dbParameter.ParameterName = parameterName;
			dbParameter.Value = value;
			dbParameter.Size = size;
			dbParameter.Direction = ParameterDirection.InputOutput;
			cmd.Parameters.Add(dbParameter);
		}
		public void AddInputOutputParameter(DbCommand cmd, string parameterName, DbType dbType, object value)
		{
			DbParameter dbParameter = cmd.CreateParameter();
			dbParameter.DbType = dbType;
			dbParameter.ParameterName = parameterName;
			dbParameter.Value = value;
			dbParameter.Direction = ParameterDirection.InputOutput;
			cmd.Parameters.Add(dbParameter);
		}
		public void AddReturnValueParameter(DbCommand cmd, string parameterName, DbType dbType, int size)
		{
			DbParameter dbParameter = cmd.CreateParameter();
			dbParameter.DbType = dbType;
			dbParameter.Size = size;
			dbParameter.ParameterName = parameterName;
			dbParameter.Direction = ParameterDirection.ReturnValue;
			cmd.Parameters.Add(dbParameter);
		}
		public void AddReturnValueParameter(DbCommand cmd, string parameterName, DbType dbType)
		{
			DbParameter dbParameter = cmd.CreateParameter();
			dbParameter.DbType = dbType;
			dbParameter.ParameterName = parameterName;
			dbParameter.Direction = ParameterDirection.ReturnValue;
			cmd.Parameters.Add(dbParameter);
		}
		public DbParameter GetParameter(DbCommand cmd, string parameterName)
		{
			return cmd.Parameters[parameterName];
		}
		public DataSet ExecuteDataSet(DbCommand cmd)
		{
			cmd.Connection = this.CreateConnection();
			cmd.Connection.Open();
			DataSet result;
			try
			{
				using (DbDataAdapter dbDataAdapter = this.dbFactory.CreateDataAdapter())
				{
					dbDataAdapter.SelectCommand = cmd;
					DataSet dataSet = new DataSet();
					dbDataAdapter.Fill(dataSet);
					if (cmd.Connection.State != ConnectionState.Closed)
					{
						cmd.Connection.Close();
						cmd.Connection.Dispose();
					}
					result = dataSet;
				}
			}
			catch
			{
				if (cmd.Connection.State != ConnectionState.Closed)
				{
					cmd.Connection.Close();
					cmd.Connection.Dispose();
				}
				throw;
			}
			return result;
		}
		public DataTable ExecuteDataTable(DbCommand cmd)
		{
			cmd.Connection = this.CreateConnection();
			cmd.Connection.Open();
			DataTable result;
			try
			{
				using (DbDataAdapter dbDataAdapter = this.dbFactory.CreateDataAdapter())
				{
					dbDataAdapter.SelectCommand = cmd;
					DataTable dataTable = new DataTable();
					dbDataAdapter.Fill(dataTable);
					if (cmd.Connection.State != ConnectionState.Closed)
					{
						cmd.Connection.Close();
						cmd.Connection.Dispose();
					}
					result = dataTable;
				}
			}
			catch
			{
				if (cmd.Connection.State != ConnectionState.Closed)
				{
					cmd.Connection.Close();
					cmd.Connection.Dispose();
				}
				throw;
			}
			return result;
		}
		public DbDataReader ExecuteReader(DbCommand cmd)
		{
			cmd.Connection = this.CreateConnection();
			cmd.Connection.Open();
			DbDataReader result;
			try
			{
				DbDataReader dbDataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
				result = dbDataReader;
			}
			catch
			{
				if (cmd.Connection.State != ConnectionState.Closed)
				{
					cmd.Connection.Close();
					cmd.Connection.Dispose();
				}
				throw;
			}
			return result;
		}
		public int ExecuteNonQuery(DbCommand cmd)
		{
			cmd.Connection = this.CreateConnection();
			cmd.Connection.Open();
			int result;
			try
			{
				int num = cmd.ExecuteNonQuery();
				if (cmd.Connection.State != ConnectionState.Closed)
				{
					cmd.Connection.Close();
					cmd.Connection.Dispose();
				}
				result = num;
			}
			catch
			{
				if (cmd.Connection.State != ConnectionState.Closed)
				{
					cmd.Connection.Close();
					cmd.Connection.Dispose();
				}
				throw;
			}
			return result;
		}
		public object ExecuteScalar(DbCommand cmd)
		{
			cmd.Connection = this.CreateConnection();
			cmd.Connection.Open();
			object result;
			try
			{
				object obj = cmd.ExecuteScalar();
				if (cmd.Connection.State != ConnectionState.Closed)
				{
					cmd.Connection.Close();
					cmd.Connection.Dispose();
				}
				if (obj == DBNull.Value)
				{
					obj = null;
				}
				result = obj;
			}
			catch
			{
				if (cmd.Connection.State != ConnectionState.Closed)
				{
					cmd.Connection.Close();
					cmd.Connection.Dispose();
				}
				throw;
			}
			return result;
		}
		public DataSet ExecuteDataSet(DbCommand cmd, DbTrans t)
		{
			cmd.Connection = t.Connection;
			if (t.Transaction != null)
			{
				cmd.Transaction = t.Transaction;
			}
			DataSet result;
			try
			{
				using (DbDataAdapter dbDataAdapter = this.dbFactory.CreateDataAdapter())
				{
					dbDataAdapter.SelectCommand = cmd;
					DataSet dataSet = new DataSet();
					dbDataAdapter.Fill(dataSet);
					result = dataSet;
				}
			}
			catch
			{
				throw;
			}
			return result;
		}
		public DataTable ExecuteDataTable(DbCommand cmd, DbTrans t)
		{
			cmd.Connection = t.Connection;
			if (t.Transaction != null)
			{
				cmd.Transaction = t.Transaction;
			}
			DataTable result;
			try
			{
				using (DbDataAdapter dbDataAdapter = this.dbFactory.CreateDataAdapter())
				{
					dbDataAdapter.SelectCommand = cmd;
					DataTable dataTable = new DataTable();
					dbDataAdapter.Fill(dataTable);
					result = dataTable;
				}
			}
			catch
			{
				throw;
			}
			return result;
		}
		public DbDataReader ExecuteReader(DbCommand cmd, DbTrans t)
		{
			cmd.Connection = t.Connection;
			if (t.Transaction != null)
			{
				cmd.Transaction = t.Transaction;
			}
			DbDataReader result;
			try
			{
				DbDataReader dbDataReader = cmd.ExecuteReader();
				result = dbDataReader;
			}
			catch
			{
				throw;
			}
			return result;
		}
		public int ExecuteNonQuery(DbCommand cmd, DbTrans t)
		{
			cmd.Connection = t.Connection;
			if (t.Transaction != null)
			{
				cmd.Transaction = t.Transaction;
			}
			int result;
			try
			{
				int num = cmd.ExecuteNonQuery();
				result = num;
			}
			catch
			{
				throw;
			}
			return result;
		}
		public object ExecuteScalar(DbCommand cmd, DbTrans t)
		{
			cmd.Connection = t.Connection;
			if (t.Transaction != null)
			{
				cmd.Transaction = t.Transaction;
			}
			object result;
			try
			{
				object obj = cmd.ExecuteScalar();
				if (obj == DBNull.Value)
				{
					obj = null;
				}
				result = obj;
			}
			catch
			{
				throw;
			}
			return result;
		}
	}
}
