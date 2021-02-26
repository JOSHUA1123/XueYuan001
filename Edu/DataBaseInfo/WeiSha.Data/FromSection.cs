using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
namespace DataBaseInfo
{
	public class FromSection<T> : IQuerySection<T>, IQuery<T> where T : Entity
	{
		private QuerySection<T> query;
		private List<TableEntity> entities = new List<TableEntity>();
		private string tableName;
		private string relation;
		internal QuerySection<T> Query
		{
			get
			{
				return this.query;
			}
			set
			{
				this.query = value;
			}
		}
		internal List<TableEntity> TableEntities
		{
			get
			{
				return this.entities;
			}
			set
			{
				this.entities = value;
			}
		}
		internal string TableName
		{
			get
			{
				return this.tableName;
			}
			set
			{
				this.tableName = value;
			}
		}
		internal string Relation
		{
			get
			{
				return this.relation;
			}
			set
			{
				this.relation = value;
			}
		}
		internal FromSection(DbProvider dbProvider, DbTrans dbTran, Table table, string aliasName)
		{
			this.InitForm(table, aliasName);
			this.query = new QuerySection<T>(this, dbProvider, dbTran);
		}
		internal FromSection(Table table, string aliasName)
		{
			this.InitForm(table, aliasName);
			this.query = new QuerySection<T>(this);
		}
		internal void InitForm(Table table, string aliasName)
		{
			T t = CoreHelper.CreateInstance<T>();
			table = (table ?? t.GetTable());
			table.As(aliasName);
			TableEntity item = new TableEntity
			{
				Table = table,
				Entity = t
			};
			this.entities.Add(item);
			this.SetTableName(table);
		}
		internal void SetTableName(Table table)
		{
			this.tableName = table.FullName;
		}
		public TResult ToFirst<TResult>() where TResult : class
		{
			return this.query.ToFirst<TResult>();
		}
		public QuerySection<T> SubQuery()
		{
			return this.query.SubQuery();
		}
		public QuerySection<T> SubQuery(string aliasName)
		{
			return this.query.SubQuery(aliasName);
		}
		public QuerySection<TSub> SubQuery<TSub>() where TSub : Entity
		{
			return this.query.SubQuery<TSub>();
		}
		public QuerySection<TSub> SubQuery<TSub>(string aliasName) where TSub : Entity
		{
			return this.query.SubQuery<TSub>(aliasName);
		}
		public QuerySection<T> GroupBy(GroupByClip groupBy)
		{
			return this.query.GroupBy(groupBy);
		}
		public QuerySection<T> OrderBy(OrderByClip orderBy)
		{
			return this.query.OrderBy(orderBy);
		}
		public QuerySection<T> Select(params Field[] fields)
		{
			return this.query.Select(fields);
		}
		public QuerySection<T> Select(IFieldFilter filter)
		{
			return this.query.Select(filter);
		}
		public QuerySection<T> Where(WhereClip where)
		{
			return this.query.Where(where);
		}
		public QuerySection<T> Union(QuerySection<T> uquery)
		{
			return this.query.Union(uquery);
		}
		public QuerySection<T> UnionAll(QuerySection<T> uquery)
		{
			return this.query.UnionAll(uquery);
		}
		public QuerySection<T> Having(WhereClip where)
		{
			return this.query.Having(where);
		}
		public QuerySection<T> SetPagingField(Field pagingField)
		{
			return this.query.SetPagingField(pagingField);
		}
		public QuerySection<T> GetTop(int topSize)
		{
			return this.query.GetTop(topSize);
		}
		public QuerySection<T> Distinct()
		{
			return this.query.Distinct();
		}
		public PageSection<T> GetPage(int pageSize)
		{
			return this.query.GetPage(pageSize);
		}
		public T ToFirst()
		{
			return this.query.ToFirst();
		}
		public ArrayList<object> ToListResult(int startIndex, int endIndex)
		{
			return this.query.ToListResult(startIndex, endIndex);
		}
		public ArrayList<object> ToListResult()
		{
			return this.query.ToListResult();
		}
		public ArrayList<TResult> ToListResult<TResult>(int startIndex, int endIndex)
		{
			return this.query.ToListResult<TResult>(startIndex, endIndex);
		}
		public ArrayList<TResult> ToListResult<TResult>()
		{
			return this.query.ToListResult<TResult>();
		}
		public SourceReader ToReader(int startIndex, int endIndex)
		{
			return this.query.ToReader(startIndex, endIndex);
		}
		public SourceReader ToReader()
		{
			return this.query.ToReader();
		}
		public SourceList<T> ToList()
		{
			return this.query.ToList();
		}
		public SourceList<T> ToList(int startIndex, int endIndex)
		{
			return this.query.ToList(startIndex, endIndex);
		}
		public SourceList<TResult> ToList<TResult>() where TResult : Entity, new()
		{
			return this.query.ToList<TResult>();
		}
		public SourceList<TResult> ToList<TResult>(int count, int startIndex) where TResult : Entity, new()
		{
			return this.query.ToList<TResult>(count, startIndex);
		}
		public SourceList<TResult> ToList<TResult>(int count) where TResult : Entity, new()
		{
			if (count < 1)
			{
				return this.query.ToList<TResult>();
			}
			return this.query.ToList<TResult>(count);
		}
		public EntityType[] ToArray<EntityType>() where EntityType : Entity, new()
		{
			SourceList<EntityType> sourceList = this.query.ToList<EntityType>();
			EntityType[] array = new EntityType[sourceList.Count];
			for (int i = 0; i < array.Length; i++)
			{
				if (sourceList[i] != null)
				{
					array[i] = sourceList[i];
					array[i].Attach(new Field[0]);
				}
			}
			return array;
		}
		public EntityType[] ToArray<EntityType>(int count) where EntityType : Entity, new()
		{
			if (count < 1)
			{
				return this.ToArray<EntityType>();
			}
			return this.ToArray<EntityType>(1, count);
		}
		public EntityType[] ToArray<EntityType>(int count, int startIndex) where EntityType : Entity, new()
		{
			count = ((count < 1) ? 1 : count);
			SourceList<EntityType> sourceList = this.ToReader(startIndex + 1, startIndex + count).ConvertTo<EntityType>();
			EntityType[] array = new EntityType[sourceList.Count];
			for (int i = 0; i < array.Length; i++)
			{
				if (sourceList[i] != null)
				{
					array[i] = sourceList[i];
					array[i].Attach(new Field[0]);
				}
			}
			return array;
		}
		public SourceTable ToTable(int startIndex, int endIndex)
		{
			return this.query.ToTable(startIndex, endIndex);
		}
		public SourceTable ToTable()
		{
			return this.query.ToTable();
		}
		public DataSet ToDataSet(int startIndex, int endIndex)
		{
			return this.query.ToDataSet(startIndex, endIndex);
		}
		public DataSet ToDataSet()
		{
			return this.query.ToDataSet();
		}
		public TResult ToScalar<TResult>()
		{
			return this.query.ToScalar<TResult>();
		}
		public object ToScalar()
		{
			return this.query.ToScalar();
		}
		public int Count()
		{
			return this.query.Count();
		}
		public int GetPageCount(int pageSize)
		{
			return this.query.GetPageCount(pageSize);
		}
		public DataPage<IList<T>> ToListPage(int pageSize, int pageIndex)
		{
			return this.query.ToListPage(pageSize, pageIndex);
		}
		public DataPage<DataTable> ToTablePage(int pageSize, int pageIndex)
		{
			return this.query.ToTablePage(pageSize, pageIndex);
		}
		public DataPage<DataSet> ToDataSetPage(int pageSize, int pageIndex)
		{
			return this.query.ToDataSetPage(pageSize, pageIndex);
		}
		public FromSection<T> InnerJoin<TJoin>(WhereClip onWhere) where TJoin : Entity
		{
			return this.InnerJoin<TJoin>((Table)null, onWhere);
		}
        public FromSection<T> InnerJoin<TJoin>(string onWhere) where TJoin : Entity
        {
            return this.InnerJoin<TJoin>((Table)null, onWhere);
        }
        public FromSection<T> InnerJoin<TJoin>(Table table, WhereClip onWhere) where TJoin : Entity
		{
			return this.Join<TJoin>(table, null, onWhere, JoinType.InnerJoin);
		}
        public FromSection<T> InnerJoin<TJoin>(Table table, string onWhere) where TJoin : Entity
        {
            return this.Join<TJoin>(table, null, onWhere, JoinType.InnerJoin);
        }
        public FromSection<T> InnerJoin<TJoin>(string aliasName, WhereClip onWhere) where TJoin : Entity
		{
            return this.Join<TJoin>((Table)null, aliasName, onWhere, JoinType.InnerJoin);
		}
		public FromSection<T> InnerJoin<TJoin>(TableRelation<TJoin> relation, WhereClip onWhere) where TJoin : Entity
		{
			return this.InnerJoin<TJoin>(relation, null, onWhere);
		}
		public FromSection<T> InnerJoin<TJoin>(TableRelation<TJoin> relation, string aliasName, WhereClip onWhere) where TJoin : Entity
		{
			return this.Join<TJoin>(relation, aliasName, onWhere, JoinType.InnerJoin);
		}
		public FromSection<T> LeftJoin<TJoin>(WhereClip onWhere) where TJoin : Entity
		{
            return this.LeftJoin<TJoin>((Table)null, onWhere);
		}
		public FromSection<T> LeftJoin<TJoin>(Table table, WhereClip onWhere) where TJoin : Entity
		{
			return this.Join<TJoin>(table, null, onWhere, JoinType.LeftJoin);
		}
		public FromSection<T> LeftJoin<TJoin>(string aliasName, WhereClip onWhere) where TJoin : Entity
		{
            return this.Join<TJoin>((Table)null, aliasName, onWhere, JoinType.LeftJoin);
		}
		public FromSection<T> LeftJoin<TJoin>(TableRelation<TJoin> relation, WhereClip onWhere) where TJoin : Entity
		{
			return this.LeftJoin<TJoin>(relation, null, onWhere);
		}
		public FromSection<T> LeftJoin<TJoin>(TableRelation<TJoin> relation, string aliasName, WhereClip onWhere) where TJoin : Entity
		{
			return this.Join<TJoin>(relation, aliasName, onWhere, JoinType.LeftJoin);
		}
		public FromSection<T> RightJoin<TJoin>(WhereClip onWhere) where TJoin : Entity
		{
            return this.RightJoin<TJoin>((Table)null, onWhere);
		}
		public FromSection<T> RightJoin<TJoin>(Table table, WhereClip onWhere) where TJoin : Entity
		{
			return this.Join<TJoin>(table, null, onWhere, JoinType.RightJoin);
		}
		public FromSection<T> RightJoin<TJoin>(string aliasName, WhereClip onWhere) where TJoin : Entity
		{
            return this.Join<TJoin>((Table)null, aliasName, onWhere, JoinType.RightJoin);
		}
		public FromSection<T> RightJoin<TJoin>(TableRelation<TJoin> relation, WhereClip onWhere) where TJoin : Entity
		{
			return this.RightJoin<TJoin>(relation, null, onWhere);
		}
		public FromSection<T> RightJoin<TJoin>(TableRelation<TJoin> relation, string aliasName, WhereClip onWhere) where TJoin : Entity
		{
			return this.Join<TJoin>(relation, aliasName, onWhere, JoinType.RightJoin);
		}
		private FromSection<T> Join<TJoin>(TableRelation<TJoin> relation, string aliasName, WhereClip onWhere, JoinType joinType) where TJoin : Entity
		{
			this.entities.AddRange(relation.GetFromSection().TableEntities);
			TJoin tJoin = CoreHelper.CreateInstance<TJoin>();
			tJoin.GetTable().As(aliasName);
			if (this.query.PagingField == null)
			{
				this.query.SetPagingField(tJoin.PagingField);
			}
			string str = tJoin.GetTable().Name;
			if (aliasName != null)
			{
				str = aliasName;
			}
			string str2 = "(" + relation.GetFromSection().Query.QueryString + ") " + str;
			this.query.Parameters = relation.GetFromSection().Query.Parameters;
			string str3 = string.Empty;
			if (onWhere != null)
			{
				str3 = " ON " + onWhere.ToString();
			}
			string joinEnumString = this.GetJoinEnumString(joinType);
			if (this.relation != null)
			{
				this.tableName = " __[[ " + this.tableName;
				this.relation += " ]]__ ";
			}
			this.relation = this.relation + joinEnumString + str2 + str3;
			return this;
		}
		private FromSection<T> Join<TJoin>(Table table, string aliasName, WhereClip onWhere, JoinType joinType) where TJoin : Entity
		{
			TJoin tJoin = CoreHelper.CreateInstance<TJoin>();
			table = (table ?? tJoin.GetTable());
			table.As(aliasName);
			TableEntity item = new TableEntity
			{
				Table = table,
				Entity = tJoin
			};
			this.entities.Add(item);
			if (this.query.PagingField == null)
			{
				this.query.SetPagingField(tJoin.PagingField);
			}
			string str = string.Empty;
			if (onWhere != null)
			{
				str = " ON " + onWhere.ToString();
			}
			string joinEnumString = this.GetJoinEnumString(joinType);
			if (this.relation != null)
			{
				this.tableName = " __[[ " + this.tableName;
				this.relation += " ]]__ ";
			}
			this.relation = this.relation + joinEnumString + table.FullName + str;
			return this;
		}

        private FromSection<T> Join<TJoin>(Table table, string aliasName, string onWhere, JoinType joinType) where TJoin : Entity
        {
            TJoin tJoin = CoreHelper.CreateInstance<TJoin>();
            table = (table ?? tJoin.GetTable());
            table.As(aliasName);
            TableEntity item = new TableEntity
            {
                Table = table,
                Entity = tJoin
            };
            this.entities.Add(item);
            if (this.query.PagingField == null)
            {
                this.query.SetPagingField(tJoin.PagingField);
            }
            string str = string.Empty;
            if (onWhere != null)
            {
                str = " ON " + onWhere.ToString();
            }
            string joinEnumString = this.GetJoinEnumString(joinType);
            if (this.relation != null)
            {
                this.tableName = " __[[ " + this.tableName;
                this.relation += " ]]__ ";
            }
            this.relation = this.relation + joinEnumString + table.FullName + str;
            return this;
        }


        private string GetJoinEnumString(JoinType joinType)
		{
			switch (joinType)
			{
			case JoinType.LeftJoin:
				return " LEFT OUTER JOIN ";
			case JoinType.RightJoin:
				return " RIGHT OUTER JOIN ";
			case JoinType.InnerJoin:
				return " INNER JOIN ";
			default:
				return " INNER JOIN ";
			}
		}
		internal Field GetPagingField()
		{
			foreach (TableEntity current in this.entities)
			{
				Field pagingField = current.Entity.PagingField;
				if (pagingField != null)
				{
					return pagingField.At(current.Table);
				}
			}
			return null;
		}
		internal Field[] GetSelectFields()
		{
			Dictionary<string, Field> dictionary = new Dictionary<string, Field>();
			foreach (TableEntity current in this.entities)
			{
				Table table = current.Table;
				Field[] fields = current.Entity.GetFields();
				if (fields == null || fields.Length == 0)
				{
					throw new DataException("没有任何被选中的字段列表！");
				}
				Field[] array = fields;
				for (int i = 0; i < array.Length; i++)
				{
					Field field = array[i];
					if (!dictionary.ContainsKey(field.OriginalName))
					{
						dictionary[field.OriginalName] = field.At(table);
					}
				}
			}
			return (
				from p in dictionary
				select p.Value).ToArray<Field>();
		}
	}
}
