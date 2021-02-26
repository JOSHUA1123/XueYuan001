using System;
namespace DataBaseInfo
{
	internal interface IDataPage
	{
		int PageSize
		{
			get;
			set;
		}
		int RowCount
		{
			get;
			set;
		}
		int CurrentPageIndex
		{
			get;
			set;
		}
		int PageCount
		{
			get;
		}
		bool IsFirstPage
		{
			get;
		}
		bool IsLastPage
		{
			get;
		}
		int CurrentRowCount
		{
			get;
		}
		int CurrentStartIndex
		{
			get;
		}
		int CurrentEndIndex
		{
			get;
		}
		object DataSource
		{
			get;
			set;
		}
	}
	internal interface IDataPage<T> : IDataPage
	{
		T DataSource
		{
			get;
			set;
		}
	}
}
