using System;
namespace DataBaseInfo
{
	internal interface IPageSection<T>
	{
		int PageCount
		{
			get;
		}
		int RowCount
		{
			get;
		}
		ArrayList<object> ToListResult(int pageIndex);
		ArrayList<TResult> ToListResult<TResult>(int pageIndex);
		SourceTable ToTable(int pageIndex);
		SourceList<T> ToList(int pageIndex);
		T[] ToArray(int pageIndex);
		SourceReader ToReader(int pageIndex);
	}
}
