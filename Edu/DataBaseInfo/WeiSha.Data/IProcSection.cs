using System;
using System.Collections.Generic;
using System.Data;
namespace DataBaseInfo
{
	internal interface IProcSection : ISqlSection
	{
		int ReturnValue
		{
			get;
		}
		int Execute(out IDictionary<string, object> outValues);
		T ToFirst<T>(out IDictionary<string, object> outValues) where T : class;
		SourceList<T> ToList<T>(out IDictionary<string, object> outValues) where T : class;
		ArrayList<TResult> ToListResult<TResult>(out IDictionary<string, object> outValues);
		SourceReader ToReader(out IDictionary<string, object> outValues);
		SourceTable ToTable(out IDictionary<string, object> outValues);
		DataSet ToDataSet(out IDictionary<string, object> outValues);
		TResult ToScalar<TResult>(out IDictionary<string, object> outValues);
		object ToScalar(out IDictionary<string, object> outValues);
	}
}
