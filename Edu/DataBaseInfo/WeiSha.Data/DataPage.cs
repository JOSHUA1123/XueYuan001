using System;
namespace DataBaseInfo
{
	[Serializable]
	public class DataPage : IDataPage
	{
		private int pageSize;
		private int pageIndex;
		private int rowCount;
		private object dataSource;
		public int CurrentPageIndex
		{
			get
			{
				return this.pageIndex;
			}
			set
			{
				this.pageIndex = value;
			}
		}
		public int PageSize
		{
			get
			{
				return this.pageSize;
			}
			set
			{
				this.pageSize = value;
			}
		}
		public int RowCount
		{
			get
			{
				return this.rowCount;
			}
			set
			{
				this.rowCount = value;
			}
		}
		public int PageCount
		{
			get
			{
				return Convert.ToInt32(Math.Ceiling((double)this.rowCount * 1.0 / (double)this.pageSize));
			}
		}
		public bool IsFirstPage
		{
			get
			{
				return this.pageIndex <= 1;
			}
		}
		public bool IsLastPage
		{
			get
			{
				return this.pageIndex >= this.PageCount;
			}
		}
		public int CurrentRowCount
		{
			get
			{
				if (this.IsLastPage)
				{
					return this.rowCount - this.pageSize * (this.pageIndex - 1);
				}
				return this.pageSize;
			}
		}
		public int CurrentStartIndex
		{
			get
			{
				if (this.IsFirstPage)
				{
					return 1;
				}
				return (this.pageIndex - 1) * this.pageSize + 1;
			}
		}
		public int CurrentEndIndex
		{
			get
			{
				if (this.IsLastPage)
				{
					return this.rowCount;
				}
				return this.pageIndex * this.pageSize;
			}
		}
		public object DataSource
		{
			get
			{
				return this.dataSource;
			}
			set
			{
				this.dataSource = value;
			}
		}
		public DataPage()
		{
			this.pageIndex = 1;
		}
		public DataPage(int pageSize) : this()
		{
			this.pageSize = pageSize;
		}
	}
	[Serializable]
	public class DataPage<TSource> : DataPage, IDataPage<TSource>, IDataPage
	{
		public new TSource DataSource
		{
			get
			{
				return (TSource)((object)base.DataSource);
			}
			set
			{
				base.DataSource = value;
			}
		}
		public DataPage()
		{
		}
		public DataPage(int pageSize) : base(pageSize)
		{
		}
	}
}
