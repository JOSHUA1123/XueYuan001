using System;
using System.Collections.Generic;
using System.Data;
namespace DataBaseInfo
{
	internal interface IUserQuery
	{
		QuerySection SetPagingField(string pagingFieldName);
		QuerySection SetPagingField(Field pagingField);
		PageSection GetPage(int pageSize);
		object ToScalar();
		TResult ToScalar<TResult>();
		int Count();
		bool Exists();
		int GetPageCount(int pageSize);
		T ToFirst<T>() where T : class;
		SourceReader ToReader();
		SourceReader ToReader(int topSize);
		SourceList<T> ToList<T>() where T : class;
		SourceList<T> ToList<T>(int topSize) where T : class;
		SourceTable ToTable();
		SourceTable ToTable(int topSize);
		DataSet ToDataSet();
		DataSet ToDataSet(int topSize);
		DataPage<IList<T>> ToListPage<T>(int pageSize, int pageIndex) where T : class;
		DataPage<DataTable> ToTablePage(int pageSize, int pageIndex);
		DataPage<DataSet> ToDataSetPage(int pageSize, int pageIndex);
	}
}
