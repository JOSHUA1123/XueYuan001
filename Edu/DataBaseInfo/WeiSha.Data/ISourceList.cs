using System;
using System.Collections;
using System.Collections.Generic;
namespace DataBaseInfo
{
	internal interface ISourceList<T> : IListConvert<T>, IArrayList<T>, IList<T>, ICollection<T>, IEnumerable<T>, IEnumerable, IDataSource<IList<T>>
	{
		SourceTable ToTable();
		void ForEach(Action<T> action);
		void Sort(IComparer<T> comparer);
		SourceList<T> FindAll(Predicate<T> match);
		SourceList<T> GetRange(int index, int count);
	}
}
