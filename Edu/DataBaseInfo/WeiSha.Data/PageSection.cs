using System;
using System.Data;
using DataBaseInfo.Design;
namespace DataBaseInfo
{
	public class PageSection : IPageQuery
	{
		private QuerySection<ViewEntity> query;
		private int? rowCount;
		private int pageSize;
		public int PageCount
		{
			get
			{
				if (!this.rowCount.HasValue)
				{
					this.rowCount = new int?(this.query.Count());
				}
				return Convert.ToInt32(Math.Ceiling(1.0 * (double)this.rowCount.Value / (double)this.pageSize));
			}
		}
		public int RowCount
		{
			get
			{
				if (!this.rowCount.HasValue)
				{
					this.rowCount = new int?(this.query.Count());
				}
				return this.rowCount.Value;
			}
		}
		internal PageSection(QuerySection<ViewEntity> query, int pageSize)
		{
			this.query = query;
			this.pageSize = pageSize;
		}
		public SourceReader ToReader(int pageIndex)
		{
			return this.query.GetPage(this.pageSize).ToReader(pageIndex);
		}
		public SourceList<T> ToList<T>(int pageIndex) where T : class
		{
			return this.ToReader(pageIndex).ConvertTo<T>();
		}
		public SourceTable ToTable(int pageIndex)
		{
			return this.query.GetPage(this.pageSize).ToTable(pageIndex);
		}
		public DataSet ToDataSet(int pageIndex)
		{
			return this.query.GetPage(this.pageSize).ToDataSet(pageIndex);
		}
	}
	public class PageSection<T> : IPageSection<T> where T : Entity
	{
		private QuerySection<T> query;
		private int? rowCount;
		private int pageSize;
		public int PageCount
		{
			get
			{
				if (!this.rowCount.HasValue)
				{
					this.rowCount = new int?(this.query.Count());
				}
				return Convert.ToInt32(Math.Ceiling(1.0 * (double)this.rowCount.Value / (double)this.pageSize));
			}
		}
		public int RowCount
		{
			get
			{
				if (!this.rowCount.HasValue)
				{
					this.rowCount = new int?(this.query.Count());
				}
				return this.rowCount.Value;
			}
		}
		internal PageSection(QuerySection<T> query, int pageSize)
		{
			this.pageSize = pageSize;
			this.query = query;
		}
		public ArrayList<object> ToListResult(int pageIndex)
		{
			int startIndex = this.pageSize * (pageIndex - 1) + 1;
			int endIndex = this.pageSize * pageIndex;
			return this.query.ToListResult(startIndex, endIndex);
		}
		public ArrayList<TResult> ToListResult<TResult>(int pageIndex)
		{
			int startIndex = this.pageSize * (pageIndex - 1) + 1;
			int endIndex = this.pageSize * pageIndex;
			return this.query.ToListResult<TResult>(startIndex, endIndex);
		}
		public SourceReader ToReader(int pageIndex)
		{
			int startIndex = this.pageSize * (pageIndex - 1) + 1;
			int endIndex = this.pageSize * pageIndex;
			return this.query.ToReader(startIndex, endIndex);
		}
		public SourceTable ToTable(int pageIndex)
		{
			int startIndex = this.pageSize * (pageIndex - 1) + 1;
			int endIndex = this.pageSize * pageIndex;
			return this.query.ToTable(startIndex, endIndex);
		}
		public DataSet ToDataSet(int pageIndex)
		{
			int startIndex = this.pageSize * (pageIndex - 1) + 1;
			int endIndex = this.pageSize * pageIndex;
			return this.query.ToDataSet(startIndex, endIndex);
		}
		public SourceList<T> ToList(int pageIndex)
		{
			int startIndex = this.pageSize * (pageIndex - 1) + 1;
			int endIndex = this.pageSize * pageIndex;
			return this.query.ToList(startIndex, endIndex);
		}
		public T[] ToArray(int pageIndex)
		{
			SourceList<T> sourceList = this.ToList(pageIndex);
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
	}
}
