using System;
using System.Data;
namespace DataBaseInfo
{
	internal interface IQuery<T> where T : Entity
	{
		T ToFirst();
		DataSet ToDataSet();
		DataSet ToDataSet(int startIndex, int endIndex);
		SourceTable ToTable();
		SourceTable ToTable(int startIndex, int endIndex);
		SourceList<T> ToList();
		SourceList<T> ToList(int startIndex, int endIndex);
		QuerySection<T> SetPagingField(Field pagingField);
		QuerySection<T> Distinct();
		QuerySection<T> OrderBy(OrderByClip orderBy);
		QuerySection<T> GetTop(int topSize);
		PageSection<T> GetPage(int pageSize);
		TResult ToFirst<TResult>() where TResult : class;
		SourceList<TResult> ToList<TResult>() where TResult : Entity, new();
		SourceList<TResult> ToList<TResult>(int count, int startIndex) where TResult : Entity, new();
	}
}
