using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;
using DataBaseInfo.Design;
namespace DataBaseInfo
{
	public class QuerySection : IUserQuery
	{
		private QuerySection<ViewEntity> query;
		internal QuerySection(QuerySection<ViewEntity> query)
		{
			this.query = query;
		}
		public QuerySection SetPagingField(string pagingFieldName)
		{
			return this.SetPagingField(new Field(pagingFieldName));
		}
		public QuerySection SetPagingField(Field pagingField)
		{
			this.query.SetPagingField(pagingField);
			return this;
		}
		public PageSection GetPage(int pageSize)
		{
			return new PageSection(this.query, pageSize);
		}
		public object ToScalar()
		{
			return this.query.ToScalar();
		}
		public TResult ToScalar<TResult>()
		{
			return this.query.ToScalar<TResult>();
		}
		public int Count()
		{
			return this.query.Count();
		}
		public int GetPageCount(int pageSize)
		{
			return this.query.GetPageCount(pageSize);
		}
		public bool Exists()
		{
			return this.query.Count() > 0;
		}
		public T ToFirst<T>() where T : class
		{
			ISourceList<T> sourceList = this.query.GetTop(1).ToReader().ConvertTo<T>();
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
		public SourceList<T> ToList<T>() where T : class
		{
			return this.query.ToReader().ConvertTo<T>();
		}
		public SourceReader ToReader()
		{
			return this.query.ToReader();
		}
		public SourceTable ToTable()
		{
			return this.query.ToTable();
		}
		public DataSet ToDataSet()
		{
			return this.query.ToDataSet();
		}
		public SourceReader ToReader(int topSize)
		{
			return this.query.GetTop(topSize).ToReader();
		}
		public SourceList<T> ToList<T>(int topSize) where T : class
		{
			return this.ToReader(topSize).ConvertTo<T>();
		}
		public SourceTable ToTable(int topSize)
		{
			return this.query.GetTop(topSize).ToTable();
		}
		public DataSet ToDataSet(int topSize)
		{
			return this.query.GetTop(topSize).ToDataSet();
		}
		public DataPage<IList<T>> ToListPage<T>(int pageSize, int pageIndex) where T : class
		{
			DataPage<IList<T>> dataPage = new DataPage<IList<T>>(pageSize);
			PageSection page = this.GetPage(pageSize);
			dataPage.CurrentPageIndex = pageIndex;
			dataPage.RowCount = page.RowCount;
			dataPage.DataSource = page.ToList<T>(pageIndex);
			return dataPage;
		}
		public DataPage<DataTable> ToTablePage(int pageSize, int pageIndex)
		{
			DataPage<DataTable> dataPage = new DataPage<DataTable>(pageSize);
			PageSection page = this.GetPage(pageSize);
			dataPage.CurrentPageIndex = pageIndex;
			dataPage.RowCount = page.RowCount;
			dataPage.DataSource = page.ToTable(pageIndex);
			return dataPage;
		}
		public DataPage<DataSet> ToDataSetPage(int pageSize, int pageIndex)
		{
			DataPage<DataSet> dataPage = new DataPage<DataSet>(pageSize);
			PageSection page = this.GetPage(pageSize);
			dataPage.CurrentPageIndex = pageIndex;
			dataPage.RowCount = page.RowCount;
			dataPage.DataSource = page.ToDataSet(pageIndex);
			return dataPage;
		}
	}
	public class QuerySection<T> : IQuerySection<T>, IQuery<T>, IPaging where T : Entity
	{
		protected DbProvider dbProvider;
		protected DbTrans dbTran;
		protected string formatString = " SELECT {0} {1} {2} {3} FROM {4} {5} {6} {7} {8} {9}";
		protected string distinctString;
		protected string prefixString;
		protected string suffixString;
		protected string endString;
		private string sqlString;
		private bool fieldSelect;
		private Field pagingField;
		private List<Field> fieldList = new List<Field>();
		private List<SQLParameter> parameterList = new List<SQLParameter>();
		private FromSection<T> fromSection;
		private WhereClip havingWhere;
		private WhereClip pageWhere;
		private WhereClip queryWhere;
		private GroupByClip groupBy;
		private OrderByClip orderBy;
		private DbCommand queryCommand;
		private bool isAddParameter;
		private string countWhereString
		{
			get
			{
				if (DataHelper.IsNullOrEmpty(this.queryWhere))
				{
					return null;
				}
				return " WHERE " + this.queryWhere.ToString();
			}
		}
		private string whereString
		{
			get
			{
				WhereClip whereClip = this.queryWhere && this.pageWhere;
				if (DataHelper.IsNullOrEmpty(whereClip))
				{
					return null;
				}
				return " WHERE " + whereClip.ToString();
			}
		}
		private string groupString
		{
			get
			{
				if (DataHelper.IsNullOrEmpty(this.groupBy))
				{
					return null;
				}
				return " GROUP BY " + this.groupBy.ToString();
			}
		}
		private string havingString
		{
			get
			{
				if (DataHelper.IsNullOrEmpty(this.havingWhere))
				{
					return null;
				}
				return " HAVING " + this.havingWhere.ToString();
			}
		}
		internal virtual string CountString
		{
			get
			{
				string text = string.Empty;
				if (DataHelper.IsNullOrEmpty(this.groupBy) && string.IsNullOrEmpty(this.distinctString))
				{
					string arg_49_0 = this.formatString;
					object[] array = new object[10];
					array[2] = "COUNT(*) AS ROW_COUNT";
					array[4] = this.SqlString;
					array[5] = this.countWhereString;
					text = string.Format(arg_49_0, array);
				}
				else
				{
					text = string.Format(this.formatString, new object[]
					{
						this.distinctString,
						null,
						this.fieldString,
						null,
						this.SqlString,
						this.countWhereString,
						this.groupString,
						this.havingString,
						null,
						this.endString
					});
					text = string.Format("SELECT COUNT(*) AS ROW_COUNT FROM ({0}) TMP_TABLE", text);
				}
				return text;
			}
		}
		private string fieldString
		{
			get
			{
				if (this.fieldList.Count == 0)
				{
					this.fieldList.AddRange(this.fromSection.GetSelectFields());
				}
				StringBuilder stringBuilder = new StringBuilder();
				int num = 0;
				foreach (Field current in this.fieldList)
				{
					num++;
					if (current is IProvider)
					{
						(current as IProvider).SetDbProvider(this.dbProvider, this.dbTran);
					}
					stringBuilder.Append(current.FullName);
					if (num < this.fieldList.Count)
					{
						stringBuilder.Append(",");
					}
				}
				return stringBuilder.ToString();
			}
		}
		private string SqlString
		{
			get
			{
				if (string.IsNullOrEmpty(this.sqlString))
				{
					this.sqlString = this.fromSection.TableName + " " + this.fromSection.Relation;
				}
				return this.sqlString;
			}
			set
			{
				this.sqlString = value;
			}
		}
		internal virtual SQLParameter[] Parameters
		{
			get
			{
				if (!this.isAddParameter)
				{
					WhereClip whereClip = this.queryWhere && this.pageWhere && this.havingWhere;
					SQLParameter[] parameters = whereClip.Parameters;
					SQLParameter p;
					for (int i = 0; i < parameters.Length; i++)
					{
						p = parameters[i];
						if (!this.parameterList.Exists((SQLParameter p1) => p.Name == p1.Name))
						{
							this.parameterList.Add(p);
						}
					}
					this.isAddParameter = true;
				}
				return this.parameterList.ToArray();
			}
			set
			{
				if (value != null && value.Length > 0)
				{
					SQLParameter p;
					for (int i = 0; i < value.Length; i++)
					{
						p = value[i];
						if (!this.parameterList.Exists((SQLParameter p1) => p.Name == p1.Name))
						{
							this.parameterList.Add(p);
						}
					}
				}
			}
		}
		internal FromSection<T> FromSection
		{
			get
			{
				return this.fromSection;
			}
		}
		internal virtual string QueryString
		{
			get
			{
				return string.Format(this.formatString, new object[]
				{
					this.distinctString,
					this.prefixString,
					this.fieldString,
					this.suffixString,
					this.SqlString,
					this.whereString,
					this.groupString,
					this.havingString,
					this.OrderString,
					this.endString
				});
			}
		}
		internal virtual string OrderString
		{
			get
			{
				if (DataHelper.IsNullOrEmpty(this.orderBy))
				{
					return null;
				}
				return " ORDER BY " + this.orderBy.ToString();
			}
		}
		internal WhereClip PageWhere
		{
			get
			{
				return this.pageWhere;
			}
			set
			{
				this.isAddParameter = false;
				this.pageWhere = value;
			}
		}
		internal Field PagingField
		{
			get
			{
				if (this.pagingField == null)
				{
					this.pagingField = this.fromSection.GetPagingField();
				}
				return this.pagingField;
			}
		}
		internal void SetDbProvider(DbProvider dbProvider, DbTrans dbTran)
		{
			this.dbProvider = dbProvider;
			this.dbTran = dbTran;
		}
		void IPaging.Prefix(string prefix)
		{
			this.prefixString = prefix;
		}
		void IPaging.Suffix(string suffix)
		{
			this.suffixString = suffix;
		}
		void IPaging.End(string end)
		{
			this.endString = end;
		}
		internal QuerySection(FromSection<T> fromSection, DbProvider dbProvider, DbTrans dbTran)
		{
			this.fromSection = fromSection;
			this.dbProvider = dbProvider;
			this.dbTran = dbTran;
		}
		internal QuerySection(FromSection<T> fromSection)
		{
			this.fromSection = fromSection;
		}
		public virtual SourceList<TResult> ToList<TResult>() where TResult : Entity, new()
		{
			SourceList<TResult> sourceList = this.ToReader().ConvertTo<TResult>();
			for (int i = 0; i < sourceList.Count; i++)
			{
				if (sourceList[i] != null)
				{
					sourceList[i] = sourceList[i];
					TResult tResult = sourceList[i];
					tResult.Attach(new Field[0]);
				}
			}
			return sourceList;
		}
		public SourceList<TResult> ToList<TResult>(int count) where TResult : Entity, new()
		{
			if (count < 1)
			{
				return this.ToList<TResult>();
			}
			return this.ToList<TResult>(count, 0);
		}
		public SourceList<TResult> ToList<TResult>(int count, int startIndex) where TResult : Entity, new()
		{
			count = ((count < 1) ? 1 : count);
			SourceList<TResult> sourceList = this.ToReader(startIndex + 1, startIndex + count).ConvertTo<TResult>();
			for (int i = 0; i < sourceList.Count; i++)
			{
				if (sourceList[i] != null)
				{
					sourceList[i] = sourceList[i];
					TResult tResult = sourceList[i];
					tResult.Attach(new Field[0]);
				}
			}
			return sourceList;
		}
		public EntityType[] ToArray<EntityType>() where EntityType : Entity, new()
		{
			SourceList<EntityType> sourceList = this.ToReader().ConvertTo<EntityType>();
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
			return this.ToArray<EntityType>(count, 0);
		}
		public EntityType[] ToArray<EntityType>(int count, int startIndex) where EntityType : Entity, new()
		{
			count = ((count < 1) ? 2147483647 : count);
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
		public TResult ToFirst<TResult>() where TResult : class
		{
			SourceReader dataReader = this.GetDataReader(this, 1, 0);
			SourceList<TResult> sourceList = dataReader.ConvertTo<TResult>();
			if (sourceList.Count <= 0)
			{
				return default(TResult);
			}
			if (sourceList[0] is Entity)
			{
				Entity entity = sourceList[0] as Entity;
				entity.Attach(new Field[0]);
				return entity as TResult;
			}
			return sourceList[0];
		}
		public virtual QuerySection<T> SubQuery()
		{
			return this.SubQuery<T>();
		}
		public virtual QuerySection<T> SubQuery(string aliasName)
		{
			return this.SubQuery<T>(aliasName);
		}
		public virtual QuerySection<TSub> SubQuery<TSub>() where TSub : Entity
		{
			return this.SubQuery<TSub>(null);
		}
		public virtual QuerySection<TSub> SubQuery<TSub>(string aliasName) where TSub : Entity
		{
			TSub tSub = CoreHelper.CreateInstance<TSub>();
			string arg = tSub.GetTable().Name;
			if (aliasName != null)
			{
				arg = string.Format("__[{0}]__", aliasName);
			}
			QuerySection<TSub> querySection = new QuerySection<TSub>(new FromSection<TSub>(tSub.GetTable(), aliasName), this.dbProvider, this.dbTran);
			querySection.SqlString = string.Format("({0}) {1}", this.QueryString, arg);
			querySection.Parameters = this.Parameters;
			querySection.Select(new Field[]
			{
				Field.All.At(tSub.GetTable().As(aliasName))
			});
			return querySection;
		}
		internal virtual QuerySection<TResult> CreateQuery<TResult>() where TResult : Entity
		{
			QuerySection<TResult> querySection = new QuerySection<TResult>(new FromSection<TResult>(null, null), this.dbProvider, this.dbTran);
			FromSection<T> fromSection = this.FromSection;
			FromSection<TResult> fromSection2 = querySection.FromSection;
			fromSection2.TableEntities = fromSection.TableEntities;
			fromSection2.TableName = fromSection.TableName;
			fromSection2.Relation = fromSection.Relation;
			fromSection2.SetPagingField(fromSection.GetPagingField());
			querySection.Where(this.queryWhere).OrderBy(this.orderBy).GroupBy(this.groupBy).Having(this.havingWhere);
			querySection.SqlString = this.SqlString;
			querySection.PageWhere = this.PageWhere;
			querySection.Parameters = this.Parameters;
			if (this.fieldSelect)
			{
				querySection.Select(this.fieldList.ToArray());
			}
			return querySection;
		}
		public QuerySection<T> GroupBy(GroupByClip groupBy)
		{
			this.groupBy = groupBy;
			return this;
		}
		public QuerySection<T> OrderBy(OrderByClip orderBy)
		{
			this.orderBy = orderBy;
			return this;
		}
		public QuerySection<T> GetTop(int topSize)
		{
			if (topSize <= 0)
			{
				throw new DataException("选取前N条数据值不能小于等于0！");
			}
			QuerySection<T> query = this.dbProvider.CreatePageQuery<T>(this, topSize, 0);
			return new TopQuery<T>(query, this.dbProvider, this.dbTran, topSize);
		}
		public QuerySection<T> SetPagingField(Field pagingField)
		{
			this.pagingField = pagingField;
			return this;
		}
		public QuerySection<T> Select(params Field[] fields)
		{
			if (fields == null)
			{
				return this;
			}
			this.fieldList.Clear();
			if (fields.Length == 0)
			{
				this.fieldSelect = false;
				return this;
			}
			this.fieldSelect = true;
			this.fieldList.AddRange(fields);
			return this;
		}
		public QuerySection<T> Select(IFieldFilter filter)
		{
			Field[] fields = new Field[0];
			if (filter != null)
			{
				fields = filter.GetFields(this.fromSection.GetSelectFields());
			}
			return this.Select(fields);
		}
		public QuerySection<T> Where(WhereClip where)
		{
			this.isAddParameter = false;
			this.queryWhere = where;
			return this;
		}
		public virtual QuerySection<T> Union(QuerySection<T> query)
		{
			return this.Union(query, false);
		}
		public virtual QuerySection<T> UnionAll(QuerySection<T> query)
		{
			return this.Union(query, true);
		}
		private QuerySection<T> Union(QuerySection<T> query, bool isUnionAll)
		{
			return new UnionQuery<T>(this, query, this.dbProvider, this.dbTran, isUnionAll);
		}
		public QuerySection<T> Having(WhereClip where)
		{
			this.isAddParameter = false;
			this.havingWhere = where;
			return this;
		}
		public QuerySection<T> Distinct()
		{
			this.distinctString = " DISTINCT ";
			return this;
		}
		public virtual PageSection<T> GetPage(int pageSize)
		{
			return new PageSection<T>(this, pageSize);
		}
		public T ToFirst()
		{
			ISourceList<T> list = this.GetList<T>(this, 1, 0);
			if (list.Count == 0)
			{
				return default(T);
			}
			T t = list[0];
			t.Attach(new Field[0]);
			return list[0];
		}
		public ArrayList<object> ToListResult(int startIndex, int endIndex)
		{
			if (startIndex <= 0)
			{
				startIndex = 1;
			}
			int num = endIndex - startIndex + 1;
			return this.GetListResult<object>(this, num, endIndex - num);
		}
		public virtual ArrayList<object> ToListResult()
		{
			return this.ExecuteDataListResult<object>(this);
		}
		public ArrayList<TResult> ToListResult<TResult>(int startIndex, int endIndex)
		{
			if (startIndex <= 0)
			{
				startIndex = 1;
			}
			int num = endIndex - startIndex + 1;
			return this.GetListResult<TResult>(this, num, endIndex - num);
		}
		public virtual ArrayList<TResult> ToListResult<TResult>()
		{
			return this.ExecuteDataListResult<TResult>(this);
		}
		public virtual SourceList<T> ToList()
		{
			return this.ExecuteDataList<T>(this);
		}
		public SourceList<T> ToList(int startIndex, int endIndex)
		{
			if (startIndex <= 0)
			{
				startIndex = 1;
			}
			int num = endIndex - startIndex + 1;
			return this.GetList<T>(this, num, endIndex - num);
		}
		public SourceReader ToReader(int startIndex, int endIndex)
		{
			if (startIndex <= 0)
			{
				startIndex = 1;
			}
			int num = endIndex - startIndex + 1;
			return this.GetDataReader(this, num, endIndex - num);
		}
		public virtual SourceReader ToReader()
		{
			return this.ExecuteDataReader<T>(this);
		}
		public DataSet ToDataSet(int startIndex, int endIndex)
		{
			if (startIndex <= 0)
			{
				startIndex = 1;
			}
			int num = endIndex - startIndex + 1;
			return this.GetDataSet(this, num, endIndex - num);
		}
		public virtual DataSet ToDataSet()
		{
			return this.ExecuteDataSet<T>(this);
		}
		public SourceTable ToTable(int startIndex, int endIndex)
		{
			if (startIndex <= 0)
			{
				startIndex = 1;
			}
			int num = endIndex - startIndex + 1;
			return this.GetDataTable(this, num, endIndex - num);
		}
		public virtual SourceTable ToTable()
		{
			return this.ExecuteDataTable<T>(this);
		}
		public TResult ToScalar<TResult>()
		{
			object value = this.ToScalar();
			return CoreHelper.ConvertValue<TResult>(value);
		}
		public object ToScalar()
		{
			return this.ExecuteScalar<T>(this);
		}
		public int Count()
		{
			return this.GetCount(this);
		}
		public int GetPageCount(int pageSize)
		{
			return this.GetPage(pageSize).PageCount;
		}
		private SourceList<TResult> GetList<TResult>(QuerySection<TResult> query, int itemCount, int skipCount) where TResult : Entity
		{
			query = this.dbProvider.CreatePageQuery<TResult>(query, itemCount, skipCount);
			return this.ExecuteDataList<TResult>(query);
		}
		private ArrayList<TResult> GetListResult<TResult>(QuerySection<T> query, int itemCount, int skipCount)
		{
			query = this.dbProvider.CreatePageQuery<T>(query, itemCount, skipCount);
			return this.ExecuteDataListResult<TResult>(query);
		}
		private SourceReader GetDataReader(QuerySection<T> query, int itemCount, int skipCount)
		{
			query = this.dbProvider.CreatePageQuery<T>(query, itemCount, skipCount);
			return this.ExecuteDataReader<T>(query);
		}
		private SourceTable GetDataTable(QuerySection<T> query, int itemCount, int skipCount)
		{
			query = this.dbProvider.CreatePageQuery<T>(query, itemCount, skipCount);
			return this.ExecuteDataTable<T>(query);
		}
		private DataSet GetDataSet(QuerySection<T> query, int itemCount, int skipCount)
		{
			query = this.dbProvider.CreatePageQuery<T>(query, itemCount, skipCount);
			return this.ExecuteDataSet<T>(query);
		}
		private int GetCount(QuerySection<T> query)
		{
			string countString = query.CountString;
			string cacheKey = this.GetCacheKey(countString, this.Parameters);
			object cache = this.GetCache<T>("Count", cacheKey);
			if (cache != null)
			{
				return CoreHelper.ConvertValue<int>(cache);
			}
			this.queryCommand = this.dbProvider.CreateSqlCommand(countString, query.Parameters);
			object value = this.dbProvider.ExecuteScalar(this.queryCommand, this.dbTran);
			int num = CoreHelper.ConvertValue<int>(value);
			this.SetCache<T>("Count", cacheKey, num);
			return num;
		}
		private ArrayList<TResult> ExecuteDataListResult<TResult>(QuerySection<T> query)
		{
			ArrayList<TResult> result;
			try
			{
				string queryString = query.QueryString;
				string cacheKey = this.GetCacheKey(queryString, query.Parameters);
				object cache = this.GetCache<T>("ListObject", cacheKey);
				if (cache != null)
				{
					result = (SourceList<TResult>)cache;
				}
				else
				{
					using (SourceReader sourceReader = this.ExecuteDataReader(queryString, query.Parameters))
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
						this.SetCache<T>("ListObject", cacheKey, arrayList);
						sourceReader.Close();
						result = arrayList;
					}
				}
			}
			catch
			{
				throw;
			}
			return result;
		}
		private SourceList<TResult> ExecuteDataList<TResult>(QuerySection<TResult> query) where TResult : Entity
		{
			SourceList<TResult> result;
			try
			{
				string queryString = query.QueryString;
				string cacheKey = this.GetCacheKey(queryString, query.Parameters);
				object cache = this.GetCache<TResult>("ListEntity", cacheKey);
				if (cache != null)
				{
					result = (SourceList<TResult>)cache;
				}
				else
				{
					using (SourceReader sourceReader = this.ExecuteDataReader(queryString, query.Parameters))
					{
						SourceList<TResult> sourceList = new SourceList<TResult>();
						FastCreateInstanceHandler fastInstanceCreator = CoreHelper.GetFastInstanceCreator(typeof(TResult));
						while (sourceReader.Read())
						{
							TResult item = (TResult)((object)fastInstanceCreator());
							item.SetDbValues(sourceReader);
							item.Attach(new Field[0]);
							sourceList.Add(item);
						}
						this.SetCache<TResult>("ListEntity", cacheKey, sourceList);
						sourceReader.Close();
						result = sourceList;
					}
				}
			}
			catch
			{
				throw;
			}
			return result;
		}
		private SourceReader ExecuteDataReader(string queryString, SQLParameter[] parameters)
		{
			SourceReader result;
			try
			{
				this.queryCommand = this.dbProvider.CreateSqlCommand(queryString, parameters);
				result = this.dbProvider.ExecuteReader(this.queryCommand, this.dbTran);
			}
			catch
			{
				throw;
			}
			return result;
		}
		private SourceReader ExecuteDataReader<TResult>(QuerySection<TResult> query) where TResult : Entity
		{
			SourceReader result;
			try
			{
				string queryString = query.QueryString;
				this.queryCommand = this.dbProvider.CreateSqlCommand(queryString, query.Parameters);
				result = this.dbProvider.ExecuteReader(this.queryCommand, this.dbTran);
			}
			catch
			{
				throw;
			}
			return result;
		}
		private DataSet ExecuteDataSet<TResult>(QuerySection<TResult> query) where TResult : Entity
		{
			DataSet result;
			try
			{
				string queryString = query.QueryString;
				string cacheKey = this.GetCacheKey(queryString, query.Parameters);
				object cache = this.GetCache<TResult>("DataTable", cacheKey);
				if (cache != null)
				{
					result = (DataSet)cache;
				}
				else
				{
					this.queryCommand = this.dbProvider.CreateSqlCommand(queryString, query.Parameters);
					using (DataSet dataSet = this.dbProvider.ExecuteDataSet(this.queryCommand, this.dbTran))
					{
						this.SetCache<TResult>("DataSet", cacheKey, dataSet);
						result = dataSet;
					}
				}
			}
			catch
			{
				throw;
			}
			return result;
		}
		private SourceTable ExecuteDataTable<TResult>(QuerySection<TResult> query) where TResult : Entity
		{
			SourceTable result;
			try
			{
				string queryString = query.QueryString;
				string cacheKey = this.GetCacheKey(queryString, query.Parameters);
				object cache = this.GetCache<TResult>("DataTable", cacheKey);
				if (cache != null)
				{
					result = (SourceTable)cache;
				}
				else
				{
					this.queryCommand = this.dbProvider.CreateSqlCommand(queryString, query.Parameters);
					using (DataTable dataTable = this.dbProvider.ExecuteDataTable(this.queryCommand, this.dbTran))
					{
						dataTable.TableName = typeof(TResult).Name;
						SourceTable sourceTable = new SourceTable(dataTable);
						this.SetCache<TResult>("DataTable", cacheKey, sourceTable);
						result = sourceTable;
					}
				}
			}
			catch
			{
				throw;
			}
			return result;
		}
		private object ExecuteScalar<TResult>(QuerySection<TResult> query) where TResult : Entity
		{
			object result;
			try
			{
				string queryString = query.QueryString;
				string cacheKey = this.GetCacheKey(queryString, query.Parameters);
				object cache = this.GetCache<TResult>("GetObject", cacheKey);
				if (cache != null)
				{
					result = cache;
				}
				else
				{
					this.queryCommand = this.dbProvider.CreateSqlCommand(queryString, query.Parameters);
					object obj = this.dbProvider.ExecuteScalar(this.queryCommand, this.dbTran);
					this.SetCache<TResult>("GetObject", cacheKey, obj);
					result = obj;
				}
			}
			catch
			{
				throw;
			}
			return result;
		}
		private string GetCacheKey(string sql, SQLParameter[] parameters)
		{
			sql = this.dbProvider.FormatCommandText(sql);
			if (parameters != null && parameters.Length != 0)
			{
				for (int i = 0; i < parameters.Length; i++)
				{
					SQLParameter sQLParameter = parameters[i];
					sql = sql.Replace(sQLParameter.Name, DataHelper.FormatValue(sQLParameter.Value));
				}
			}
			return sql.ToLower();
		}
		private object GetCache<CacheType>(string prefix, string cacheKey) where CacheType : Entity
		{
			string cacheKey2 = prefix + "_" + cacheKey;
			if (this.dbProvider.Cache != null)
			{
				return this.dbProvider.Cache.GetCache<CacheType>(cacheKey2);
			}
			return null;
		}
		private void SetCache<CacheType>(string prefix, string cacheKey, object obj) where CacheType : Entity
		{
			string cacheKey2 = prefix + "_" + cacheKey;
			if (this.dbProvider.Cache != null && !cacheKey.Contains("(0=0)"))
			{
				int tableTimeout = EntityConfig.Instance.GetTableTimeout<CacheType>();
				this.dbProvider.Cache.AddCache<object>(cacheKey2, obj, tableTimeout);
			}
		}
		public DataPage<IList<T>> ToListPage(int pageSize, int pageIndex)
		{
			DataPage<IList<T>> dataPage = new DataPage<IList<T>>(pageSize);
			PageSection<T> page = this.GetPage(pageSize);
			dataPage.CurrentPageIndex = pageIndex;
			dataPage.RowCount = page.RowCount;
			dataPage.DataSource = page.ToList(pageIndex);
			return dataPage;
		}
		public DataPage<DataTable> ToTablePage(int pageSize, int pageIndex)
		{
			DataPage<DataTable> dataPage = new DataPage<DataTable>(pageSize);
			PageSection<T> page = this.GetPage(pageSize);
			dataPage.CurrentPageIndex = pageIndex;
			dataPage.RowCount = page.RowCount;
			dataPage.DataSource = page.ToTable(pageIndex);
			return dataPage;
		}
		public DataPage<DataSet> ToDataSetPage(int pageSize, int pageIndex)
		{
			DataPage<DataSet> dataPage = new DataPage<DataSet>(pageSize);
			PageSection<T> page = this.GetPage(pageSize);
			dataPage.CurrentPageIndex = pageIndex;
			dataPage.RowCount = page.RowCount;
			dataPage.DataSource = page.ToDataSet(pageIndex);
			return dataPage;
		}
	}
}
