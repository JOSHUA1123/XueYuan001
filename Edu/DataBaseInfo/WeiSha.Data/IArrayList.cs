using System;
using System.Collections;
using System.Collections.Generic;
namespace DataBaseInfo
{
	internal interface IArrayList<T> : IList<T>, ICollection<T>, IEnumerable<T>, IEnumerable, IDataSource<IList<T>>
	{
		T this[int index]
		{
			get;
			set;
		}
		T[] ToArray();
		IDictionary<TResult, IList<T>> ToDictionary<TResult>(string propertyName);
		IDictionary<TResult, IList<T>> ToDictionary<TResult>(Field groupField);
	}
}
