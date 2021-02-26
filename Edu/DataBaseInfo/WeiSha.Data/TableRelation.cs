using System;
namespace DataBaseInfo
{
	public class TableRelation<T> : ITableRelation<T> where T : Entity
	{
		private int topSize = -1;
		private FromSection<T> section;
		internal int GetTopSize()
		{
			return this.topSize;
		}
		internal FromSection<T> GetFromSection()
		{
			return this.section;
		}
		internal TableRelation(Table table, string aliasName)
		{
			this.section = new FromSection<T>(table, aliasName);
		}
		public TableRelation<T> LeftJoin<TJoin>(Table table, WhereClip onWhere) where TJoin : Entity
		{
			this.section.LeftJoin<TJoin>(table, onWhere);
			return this;
		}
		public TableRelation<T> RightJoin<TJoin>(Table table, WhereClip onWhere) where TJoin : Entity
		{
			this.section.RightJoin<TJoin>(table, onWhere);
			return this;
		}
		public TableRelation<T> InnerJoin<TJoin>(Table table, WhereClip onWhere) where TJoin : Entity
		{
			this.section.InnerJoin<TJoin>(table, onWhere);
			return this;
		}
		public TableRelation<T> LeftJoin<TJoin>(WhereClip onWhere) where TJoin : Entity
		{
			this.section.LeftJoin<TJoin>(onWhere);
			return this;
		}
		public TableRelation<T> RightJoin<TJoin>(WhereClip onWhere) where TJoin : Entity
		{
			this.section.RightJoin<TJoin>(onWhere);
			return this;
		}
		public TableRelation<T> InnerJoin<TJoin>(WhereClip onWhere) where TJoin : Entity
		{
			this.section.InnerJoin<TJoin>(onWhere);
			return this;
		}
		public TableRelation<T> LeftJoin<TJoin>(string aliasName, WhereClip onWhere) where TJoin : Entity
		{
			this.section.LeftJoin<TJoin>(aliasName, onWhere);
			return this;
		}
		public TableRelation<T> RightJoin<TJoin>(string aliasName, WhereClip onWhere) where TJoin : Entity
		{
			this.section.RightJoin<TJoin>(aliasName, onWhere);
			return this;
		}
		public TableRelation<T> InnerJoin<TJoin>(string aliasName, WhereClip onWhere) where TJoin : Entity
		{
			this.section.InnerJoin<TJoin>(aliasName, onWhere);
			return this;
		}
		public TableRelation<T> SubQuery()
		{
			this.section.Query = this.section.SubQuery();
			return this;
		}
		public TableRelation<T> SubQuery(string aliasName)
		{
			this.section.Query = this.section.SubQuery(aliasName);
			return this;
		}
		public TableRelation<TSub> SubQuery<TSub>() where TSub : Entity
		{
			TableRelation<TSub> tableRelation = new TableRelation<TSub>(null, null);
			tableRelation.GetFromSection().Query = this.section.SubQuery<TSub>();
			return tableRelation;
		}
		public TableRelation<TSub> SubQuery<TSub>(string aliasName) where TSub : Entity
		{
			return new TableRelation<TSub>(null, aliasName)
			{
				section = 
				{
					Query = this.section.SubQuery<TSub>(aliasName)
				}
			};
		}
		public TableRelation<T> Where(WhereClip where)
		{
			this.section.Where(where);
			return this;
		}
		public TableRelation<T> OrderBy(OrderByClip orderBy)
		{
			this.section.OrderBy(orderBy);
			return this;
		}
		public TableRelation<T> GroupBy(GroupByClip groupBy)
		{
			this.section.GroupBy(groupBy);
			return this;
		}
		public TableRelation<T> Select(params Field[] fields)
		{
			this.section.Select(fields);
			return this;
		}
		public TableRelation<T> GetTop(int topSize)
		{
			this.topSize = topSize;
			return this;
		}
	}
}
