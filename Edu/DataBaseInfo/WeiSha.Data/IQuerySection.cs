using System;
using System.Collections.Generic;
using System.Data;
namespace DataBaseInfo
{
	internal interface IQuerySection<T> : IQuery<T> where T : Entity
	{
		QuerySection<T> GroupBy(GroupByClip groupBy);
		QuerySection<T> Having(WhereClip where);
		QuerySection<T> Select(params Field[] fields);
		QuerySection<T> Select(IFieldFilter filter);
		QuerySection<T> Where(WhereClip where);
		QuerySection<T> Union(QuerySection<T> query);
		QuerySection<T> UnionAll(QuerySection<T> query);
		QuerySection<T> SubQuery();
		QuerySection<T> SubQuery(string aliasName);
		QuerySection<TSub> SubQuery<TSub>() where TSub : Entity;
		QuerySection<TSub> SubQuery<TSub>(string aliasName) where TSub : Entity;
		ArrayList<object> ToListResult();
		ArrayList<object> ToListResult(int startIndex, int endIndex);
		ArrayList<TResult> ToListResult<TResult>();
		ArrayList<TResult> ToListResult<TResult>(int startIndex, int endIndex);
		SourceReader ToReader();
		SourceReader ToReader(int startIndex, int endIndex);
		TResult ToScalar<TResult>();
		object ToScalar();
		int Count();
		int GetPageCount(int pageSize);
		DataPage<IList<T>> ToListPage(int pageSize, int pageIndex);
		DataPage<DataTable> ToTablePage(int pageSize, int pageIndex);
		DataPage<DataSet> ToDataSetPage(int pageSize, int pageIndex);
	}
}
