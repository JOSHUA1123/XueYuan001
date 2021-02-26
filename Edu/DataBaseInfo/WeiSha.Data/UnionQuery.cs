using System;
using System.Collections.Generic;
using System.Text;
namespace DataBaseInfo
{
	internal class UnionQuery<T> : QuerySection<T> where T : Entity
	{
		private Table unionTable = new Table("SUB_UNION_QUERY");
		private IList<UnionItem<T>> queries = new List<UnionItem<T>>();
		internal override string CountString
		{
			get
			{
				StringBuilder stringBuilder = new StringBuilder();
				int num = 0;
				foreach (UnionItem<T> current in this.queries)
				{
					stringBuilder.Append(current.Query.CountString);
					num++;
					if (num < this.queries.Count)
					{
						stringBuilder.Append(current.IsUnionAll ? " UNION ALL " : " UNION ");
					}
				}
				return string.Format("SELECT SUM({0}.ROW_COUNT) AS ROW_COUNT FROM ({1}) {0}", "SUB_UNION_QUERY", stringBuilder.ToString());
			}
		}
		internal override string QueryString
		{
			get
			{
				StringBuilder stringBuilder = new StringBuilder();
				int num = 0;
				foreach (UnionItem<T> current in this.queries)
				{
					stringBuilder.Append(current.Query.QueryString);
					num++;
					if (num < this.queries.Count)
					{
						stringBuilder.Append(current.IsUnionAll ? " UNION ALL " : " UNION ");
					}
				}
				string text = string.Format("({0}) {1}", stringBuilder.ToString(), this.unionTable.Name);
				return string.Format(this.formatString, new object[]
				{
					this.distinctString,
					this.prefixString,
					Field.All.At(this.unionTable).Name,
					this.suffixString,
					text,
					null,
					null,
					null,
					this.OrderString,
					this.endString
				});
			}
		}
		internal override string OrderString
		{
			get
			{
				if (base.PagingField != null)
				{
					return " ORDER BY " + base.PagingField.At(this.unionTable).Asc.ToString();
				}
				return null;
			}
		}
		internal override SQLParameter[] Parameters
		{
			get
			{
				List<SQLParameter> list = new List<SQLParameter>();
				foreach (UnionItem<T> current in this.queries)
				{
					list.AddRange(current.Query.Parameters);
				}
				return list.ToArray();
			}
		}
		public UnionQuery(QuerySection<T> query1, QuerySection<T> query2, DbProvider dbProvider, DbTrans dbTran, bool isUnionAll) : base(query1.FromSection, dbProvider, dbTran)
		{
			this.queries.Add(new UnionItem<T>
			{
				Query = query1,
				IsUnionAll = isUnionAll
			});
			this.queries.Add(new UnionItem<T>
			{
				Query = query2,
				IsUnionAll = isUnionAll
			});
		}
		public override QuerySection<T> Union(QuerySection<T> query)
		{
			this.queries.Add(new UnionItem<T>
			{
				Query = query,
				IsUnionAll = false
			});
			return this;
		}
		public override QuerySection<T> UnionAll(QuerySection<T> query)
		{
			this.queries.Add(new UnionItem<T>
			{
				Query = query,
				IsUnionAll = true
			});
			return this;
		}
	}
}
