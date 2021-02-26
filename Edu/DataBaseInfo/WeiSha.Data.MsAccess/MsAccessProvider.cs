using System;
using System.Data;
using System.Data.Common;
using System.Data.OleDb;
using DataBaseInfo.SqlServer;
namespace DataBaseInfo.MsAccess
{
	public class MsAccessProvider : SqlServerProvider
	{
		protected override bool AccessProvider
		{
			get
			{
				return true;
			}
		}
		protected internal override bool SupportBatch
		{
			get
			{
				return false;
			}
		}
		protected override string AutoIncrementValue
		{
			get
			{
				return "SELECT MAX({0}) FROM {1}";
			}
		}
		public MsAccessProvider(string connectionString) : base(connectionString, OleDbFactory.Instance)
		{
		}
		protected override DbParameter CreateParameter(string parameterName, object val)
		{
			OleDbParameter oleDbParameter = new OleDbParameter();
			oleDbParameter.ParameterName = parameterName;
			if (val == null || val == DBNull.Value)
			{
				oleDbParameter.Value = DBNull.Value;
			}
			else
			{
				if (val is Enum)
				{
					oleDbParameter.Value = Convert.ToInt32(val);
				}
				else
				{
					oleDbParameter.Value = val;
				}
			}
			return oleDbParameter;
		}
		protected override void PrepareParameter(DbCommand cmd)
		{
			cmd.CommandText = cmd.CommandText.Replace("GETDATE()", "DATE()");
			foreach (OleDbParameter oleDbParameter in cmd.Parameters)
			{
				if (oleDbParameter.Direction != ParameterDirection.Output && oleDbParameter.Direction != ParameterDirection.ReturnValue && oleDbParameter.Value != DBNull.Value)
				{
					if (oleDbParameter.DbType == DbType.String || oleDbParameter.DbType == DbType.AnsiString || oleDbParameter.DbType == DbType.AnsiStringFixedLength)
					{
						if (oleDbParameter.Size > 4000)
						{
							oleDbParameter.OleDbType = OleDbType.LongVarWChar;
						}
						else
						{
							oleDbParameter.OleDbType = OleDbType.VarWChar;
						}
					}
					else
					{
						if (oleDbParameter.DbType == DbType.Binary)
						{
							if (oleDbParameter.Size > 8000)
							{
								oleDbParameter.OleDbType = OleDbType.LongVarWChar;
							}
						}
						else
						{
							if (oleDbParameter.DbType == DbType.Date || oleDbParameter.DbType == DbType.Time || oleDbParameter.DbType == DbType.DateTime)
							{
								oleDbParameter.Value = oleDbParameter.Value.ToString();
							}
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
