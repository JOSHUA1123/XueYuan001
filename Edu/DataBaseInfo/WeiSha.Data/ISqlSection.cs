using System;
using System.Data;
namespace DataBaseInfo
{
	internal interface ISqlSection
	{
		int Execute();
		T ToFirst<T>() where T : class;
		SourceList<T> ToList<T>() where T : class;
		T[] ToArray<T>() where T : class;
		ArrayList<TResult> ToListResult<TResult>();
		SourceReader ToReader();
		SourceTable ToTable();
		DataSet ToDataSet();
		TResult ToScalar<TResult>();
		object ToScalar();
	}
}
