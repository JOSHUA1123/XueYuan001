using System;
using System.Data;
namespace DataBaseInfo
{
	internal interface ISourceReader : IListConvert<IRowReader>, IRowReader, IDataSource<IDataReader>, IDisposable
	{
		int FieldCount
		{
			get;
		}
		void Close();
		bool Read();
		SourceTable ToTable();
		bool NextResult();
	}
}
