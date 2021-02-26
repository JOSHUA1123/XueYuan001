using System;
using System.Data;
namespace DataBaseInfo
{
	internal interface IPageQuery
	{
		int PageCount
		{
			get;
		}
		int RowCount
		{
			get;
		}
		SourceReader ToReader(int pageIndex);
		SourceList<T> ToList<T>(int pageIndex) where T : class;
		SourceTable ToTable(int pageIndex);
		DataSet ToDataSet(int pageIndex);
	}
}
