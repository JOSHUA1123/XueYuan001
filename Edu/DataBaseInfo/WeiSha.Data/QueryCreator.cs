using System;
using System.Collections.Generic;
namespace DataBaseInfo
{
	[Serializable]
	public class QueryCreator : WhereCreator<QueryCreator>, IQueryCreator, IWhereCreator<QueryCreator>, ITableCreator<QueryCreator>
	{
		private IDictionary<string, TableJoin> joinTables;
		private List<OrderByClip> orderList;
		private List<Field> fieldList;
		internal OrderByClip OrderBy
		{
			get
			{
				OrderByClip orderByClip = OrderByClip.None;
				foreach (OrderByClip current in this.orderList)
				{
					orderByClip &= current;
				}
				return orderByClip;
			}
		}
		internal Field[] Fields
		{
			get
			{
				if (this.fieldList.Count == 0)
				{
					return new Field[]
					{
						Field.All.At(base.Table)
					};
				}
				return this.fieldList.ToArray();
			}
		}
		internal bool IsRelation
		{
			get
			{
				return this.joinTables.Count > 0;
			}
		}
		internal IDictionary<string, TableJoin> Relations
		{
			get
			{
				return this.joinTables;
			}
		}
		public static QueryCreator NewCreator()
		{
			return new QueryCreator();
		}
		public static QueryCreator NewCreator(string tableName)
		{
			return new QueryCreator(tableName, null);
		}
		public static QueryCreator NewCreator(string tableName, string aliasName)
		{
			return new QueryCreator(tableName, aliasName);
		}
		public static QueryCreator NewCreator(Table table)
		{
			return new QueryCreator(table);
		}
		private QueryCreator()
		{
			this.orderList = new List<OrderByClip>();
			this.fieldList = new List<Field>();
			this.joinTables = new Dictionary<string, TableJoin>();
		}
		private QueryCreator(string tableName, string aliasName) : base(tableName, aliasName)
		{
			this.orderList = new List<OrderByClip>();
			this.fieldList = new List<Field>();
			this.joinTables = new Dictionary<string, TableJoin>();
		}
		private QueryCreator(Table table) : base(table)
		{
			this.orderList = new List<OrderByClip>();
			this.fieldList = new List<Field>();
			this.joinTables = new Dictionary<string, TableJoin>();
		}
		public QueryCreator Join(string tableName, string where, params SQLParameter[] parameters)
		{
			return this.Join(JoinType.LeftJoin, tableName, null, where, parameters);
		}
		public QueryCreator Join(string tableName, string aliasName, string where, params SQLParameter[] parameters)
		{
			return this.Join(JoinType.LeftJoin, tableName, aliasName, where, parameters);
		}
		public QueryCreator Join(Table table, WhereClip where)
		{
			return this.Join(JoinType.LeftJoin, table, where);
		}
		public QueryCreator Join(JoinType joinType, string tableName, string where, params SQLParameter[] parameters)
		{
			return this.Join(joinType, tableName, null, where, parameters);
		}
		public QueryCreator Join(JoinType joinType, string tableName, string aliasName, string where, params SQLParameter[] parameters)
		{
			Table table = new Table(tableName).As(aliasName);
			if (!this.joinTables.ContainsKey(table.OriginalName))
			{
				TableJoin value = new TableJoin
				{
					Table = table,
					Type = joinType,
					Where = new WhereClip(where, parameters)
				};
				this.joinTables.Add(table.OriginalName, value);
			}
			return this;
		}
		public QueryCreator Join(JoinType joinType, Table table, WhereClip where)
		{
			if (!this.joinTables.ContainsKey(table.OriginalName))
			{
				TableJoin value = new TableJoin
				{
					Table = table,
					Type = JoinType.LeftJoin,
					Where = where
				};
				this.joinTables.Add(table.OriginalName, value);
			}
			return this;
		}
		public QueryCreator AddOrder(OrderByClip order)
		{
			if (DataHelper.IsNullOrEmpty(order))
			{
				return this;
			}
			if (this.orderList.Exists((OrderByClip o) => string.Compare(order.ToString(), o.ToString()) == 0))
			{
				return this;
			}
			this.orderList.Add(order);
			return this;
		}
		public QueryCreator AddOrder(string orderby)
		{
			if (string.IsNullOrEmpty(orderby))
			{
				return this;
			}
			return this.AddOrder(new OrderByClip(orderby));
		}
		public QueryCreator AddOrder(Field field, bool desc)
		{
			if (desc)
			{
				return this.AddOrder(field.Desc);
			}
			return this.AddOrder(field.Asc);
		}
		public QueryCreator AddOrder(string fieldName, bool desc)
		{
			return this.AddOrder(new Field(fieldName), desc);
		}
		public QueryCreator AddField(Field field)
		{
			if (this.fieldList.Exists((Field f) => string.Compare(field.Name, f.Name) == 0))
			{
				return this;
			}
			this.fieldList.Add(field);
			return this;
		}
		public QueryCreator AddField(string fieldName)
		{
			if (string.IsNullOrEmpty(fieldName))
			{
				return this;
			}
			return this.AddField(new Field(fieldName));
		}
		public QueryCreator AddField(string tableName, string fieldName)
		{
			return this.AddField(new Field(fieldName).At(new Table(tableName)));
		}
		public QueryCreator RemoveField(params Field[] fields)
		{
			if (fields == null)
			{
				return this;
			}
			Field field;
			for (int i = 0; i < fields.Length; i++)
			{
				field = fields[i];
				if (this.fieldList.RemoveAll((Field f) => string.Compare(f.OriginalName, field.OriginalName, true) == 0) == 0)
				{
					throw new DataException("指定的字段不存在于Query列表中！");
				}
			}
			return this;
		}
		public QueryCreator RemoveField(params string[] fieldNames)
		{
			if (fieldNames == null)
			{
				return this;
			}
			List<Field> list = new List<Field>();
			for (int i = 0; i < fieldNames.Length; i++)
			{
				string fieldName = fieldNames[i];
				list.Add(new Field(fieldName));
			}
			return this.RemoveField(list.ToArray());
		}
	}
}
