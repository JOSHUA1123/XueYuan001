using System;
using System.Data;
using System.Data.Common;
using System.Data.OracleClient;
namespace DataBaseInfo.Oracle
{
	public class OracleProvider : DbProvider
	{
		protected internal override bool SupportBatch
		{
			get
			{
				return true;
			}
		}
		protected override bool AllowAutoIncrement
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
				return "SELECT {0}.CURRVAL FROM DUAL";
			}
		}
		public OracleProvider(string connectionString) : base(connectionString, OracleClientFactory.Instance, '"', '"', ':')
		{
		}
		protected override string GetAutoIncrement(string name)
		{
			return string.Format("{0}.NEXTVAL", name);
		}
		protected override DbParameter CreateParameter(string parameterName, object val)
		{
			OracleParameter oracleParameter = new OracleParameter();
			oracleParameter.ParameterName = parameterName;
			if (val == null || val == DBNull.Value)
			{
				oracleParameter.Value = DBNull.Value;
			}
			else
			{
				if (val is Enum)
				{
					oracleParameter.Value = Convert.ToInt32(val);
				}
				else
				{
					oracleParameter.Value = val;
				}
			}
			return oracleParameter;
		}
		protected override void PrepareParameter(DbCommand cmd)
		{
			cmd.CommandText = cmd.CommandText.ToUpper();
			cmd.CommandText = cmd.CommandText.Replace("GETDATE()", "SYSDATE");
			foreach (OracleParameter oracleParameter in cmd.Parameters)
			{
				oracleParameter.ParameterName = oracleParameter.ParameterName.ToUpper();
				if (oracleParameter.Direction != ParameterDirection.Output && oracleParameter.Direction != ParameterDirection.ReturnValue && oracleParameter.Value != DBNull.Value)
				{
					if (oracleParameter.DbType == DbType.Guid)
					{
						oracleParameter.OracleType = OracleType.Char;
						oracleParameter.Size = 36;
						oracleParameter.Value = oracleParameter.Value.ToString();
					}
					else
					{
						if (oracleParameter.DbType == DbType.String || oracleParameter.DbType == DbType.AnsiString || oracleParameter.DbType == DbType.AnsiStringFixedLength)
						{
							if (oracleParameter.Size > 4000)
							{
								oracleParameter.OracleType = OracleType.NClob;
							}
							else
							{
								oracleParameter.OracleType = OracleType.NVarChar;
							}
						}
						else
						{
							if (oracleParameter.DbType == DbType.Binary)
							{
								if (oracleParameter.Size > 2000)
								{
									oracleParameter.OracleType = OracleType.LongRaw;
								}
								else
								{
									oracleParameter.OracleType = OracleType.Raw;
								}
							}
							else
							{
								if (oracleParameter.DbType == DbType.Boolean)
								{
									oracleParameter.OracleType = OracleType.Number;
									oracleParameter.Size = 1;
									oracleParameter.Value = (Convert.ToBoolean(oracleParameter.Value) ? 1 : 0);
								}
							}
						}
					}
				}
			}
		}
		protected internal override QuerySection<T> CreatePageQuery<T>(QuerySection<T> query, int itemCount, int skipCount)
		{
			if (skipCount != 0)
			{
				if (string.IsNullOrEmpty(query.OrderString))
				{
					Field pagingField = query.PagingField;
					if (pagingField == null)
					{
						throw new DataBaseInfo.DataException("请设置当前实体主键字段或指定排序！");
					}
					query.OrderBy(pagingField.Asc);
				}
				((IPaging)query).Suffix(",ROW_NUMBER() OVER(" + query.OrderString + ") AS TMP__ROWID");
				query.OrderBy(OrderByClip.None);
				query.SetPagingField(null);
				QuerySection<T> querySection = query.SubQuery("TMP_TABLE");
				querySection.Where(new WhereClip(string.Concat(new object[]
				{
					"TMP__ROWID BETWEEN ",
					skipCount + 1,
					" AND ",
					itemCount + skipCount
				}), new SQLParameter[0]));
				querySection.Select(new Field[]
				{
					Field.All.At("TMP_TABLE")
				});
				return querySection;
			}
			if (itemCount == 1 && string.IsNullOrEmpty(query.OrderString))
			{
				query.PageWhere = new WhereClip("ROWNUM <= 1", new SQLParameter[0]);
				return query;
			}
			QuerySection<T> querySection2 = query.SubQuery("TMP_TABLE");
			querySection2.Where(new WhereClip("ROWNUM <= " + itemCount, new SQLParameter[0]));
			querySection2.Select(new Field[]
			{
				Field.All.At("TMP_TABLE")
			});
			return querySection2;
		}
	}
}
