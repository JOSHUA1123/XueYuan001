using System;
using System.Data;
namespace DataBaseInfo
{
	internal class TopQuery<T> : QuerySection<T> where T : Entity
	{
		private QuerySection<T> query;
		private int topSize;
		internal override string QueryString
		{
			get
			{
				return this.query.QueryString;
			}
		}
		internal override SQLParameter[] Parameters
		{
			get
			{
				return this.query.Parameters;
			}
			set
			{
				this.query.Parameters = value;
			}
		}
		internal TopQuery(QuerySection<T> query, DbProvider dbProvider, DbTrans dbTran, int topSize) : base(query.FromSection, dbProvider, dbTran)
		{
			this.query = query;
			this.topSize = topSize;
		}
		internal override QuerySection<TResult> CreateQuery<TResult>()
		{
			return this.query.SubQuery("SUB_QUERY_TABLE").CreateQuery<TResult>();
		}
		public override PageSection<T> GetPage(int pageSize)
		{
			return this.query.SubQuery("SUB_TOP_PAGE_TABLE").GetPage(pageSize);
		}
		public override ArrayList<object> ToListResult()
		{
			return this.query.ToListResult(0, this.topSize);
		}
		public override ArrayList<TResult> ToListResult<TResult>()
		{
			return this.query.ToListResult<TResult>(0, this.topSize);
		}
		public override SourceList<T> ToList()
		{
			return this.query.ToList(0, this.topSize);
		}
		public T[] ToArray()
		{
			SourceList<T> sourceList = this.query.ToList(0, this.topSize);
			T[] array = new T[sourceList.Count];
			for (int i = 0; i < array.Length; i++)
			{
				if (sourceList[i] != null)
				{
					array[i] = sourceList[i];
				}
			}
			return array;
		}
		public override SourceList<TResult> ToList<TResult>()
		{
			return this.query.ToList<TResult>(this.topSize, 0);
		}
		public override SourceReader ToReader()
		{
			return this.query.ToReader(0, this.topSize);
		}
		public override SourceTable ToTable()
		{
			return this.query.ToTable(0, this.topSize);
		}
		public override DataSet ToDataSet()
		{
			return this.query.ToDataSet(0, this.topSize);
		}
		public override QuerySection<T> SubQuery()
		{
			return this.query.SubQuery();
		}
		public override QuerySection<T> SubQuery(string aliasName)
		{
			return this.query.SubQuery(aliasName);
		}
		public override QuerySection<TSub> SubQuery<TSub>()
		{
			return this.query.SubQuery<TSub>();
		}
		public override QuerySection<TSub> SubQuery<TSub>(string aliasName)
		{
			return this.query.SubQuery<TSub>(aliasName);
		}
	}
}
