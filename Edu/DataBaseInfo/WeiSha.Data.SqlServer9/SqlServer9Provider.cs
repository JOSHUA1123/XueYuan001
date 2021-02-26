using System;
using DataBaseInfo.SqlServer;
namespace DataBaseInfo.SqlServer9
{
	public class SqlServer9Provider : SqlServerProvider
	{
		public SqlServer9Provider(string connectionString) : base(connectionString)
		{
		}
		protected internal override QuerySection<T> CreatePageQuery<T>(QuerySection<T> query, int itemCount, int skipCount)
		{
			if (skipCount == 0)
			{
				((IPaging)query).Prefix("TOP " + itemCount);
				return query;
			}
			if (string.IsNullOrEmpty(query.OrderString))
			{
				Field pagingField = query.PagingField;
				if (pagingField == null)
				{
					throw new DataException("请指定分页主键或设置排序！");
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
	}
}
