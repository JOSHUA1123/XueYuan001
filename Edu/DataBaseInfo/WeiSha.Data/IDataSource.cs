using System;
namespace DataBaseInfo
{
	public interface IDataSource<T>
	{
		T OriginalData
		{
			get;
		}
	}
}
