using System;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
namespace DataBaseInfo.SqlServer
{
	public class SqlServerProvider : DbProvider
	{
		protected internal override bool SupportBatch
		{
			get
			{
				return true;
			}
		}
		protected override string AutoIncrementValue
		{
			get
			{
				return "SELECT SCOPE_IDENTITY()";
			}
		}
		public SqlServerProvider(string connectionString) : this(connectionString, SqlClientFactory.Instance)
		{
		}
		public SqlServerProvider(string connectionString, System.Data.Common.DbProviderFactory dbFactory) : base(connectionString, dbFactory, '[', ']', '@')
		{
		}
		protected override DbParameter CreateParameter(string parameterName, object val)
		{
			SqlParameter sqlParameter = new SqlParameter();
			sqlParameter.ParameterName = parameterName;
			if (val == null || val == DBNull.Value)
			{
				sqlParameter.Value = DBNull.Value;
			}
			else
			{
				if (val is Enum)
				{
					sqlParameter.Value = Convert.ToInt32(val);
				}
				else
				{
					sqlParameter.Value = val;
				}
			}
			return sqlParameter;
		}
		protected override void PrepareParameter(DbCommand cmd)
		{
			foreach (SqlParameter sqlParameter in cmd.Parameters)
			{
				if (sqlParameter.Direction != ParameterDirection.Output && sqlParameter.Direction != ParameterDirection.ReturnValue && sqlParameter.Value != DBNull.Value)
				{
					if (sqlParameter.DbType == DbType.String || sqlParameter.DbType == DbType.AnsiString || sqlParameter.DbType == DbType.AnsiStringFixedLength)
					{
						if (sqlParameter.Size > 4000)
						{
							sqlParameter.SqlDbType = SqlDbType.NText;
						}
						else
						{
							sqlParameter.SqlDbType = SqlDbType.NVarChar;
						}
					}
					else
					{
						if (sqlParameter.DbType == DbType.Binary && sqlParameter.Size > 8000)
						{
							sqlParameter.SqlDbType = SqlDbType.Image;
						}
					}
				}
			}
		}
		protected internal override QuerySection<T> CreatePageQuery<T>(QuerySection<T> query, int itemCount, int skipCount)
		{
			if (skipCount == 0)
			{
				((IPaging)query).Prefix("TOP " + itemCount);
				return query;
			}
			((IPaging)query).Prefix("TOP " + itemCount);
			Field pagingField = query.PagingField;
			if (pagingField == null)
			{
				throw new DataBaseInfo.DataException("SqlServer2000或Access请使用SetPagingField设定分页主键！");
			}
			QuerySection<T> querySection = query.CreateQuery<T>();
			((IPaging)querySection).Prefix("TOP " + skipCount);
			querySection.Select(new Field[]
			{
				pagingField
			});
			query.PageWhere = !pagingField.In<T>(querySection);
			return query;
		}
	}
}
