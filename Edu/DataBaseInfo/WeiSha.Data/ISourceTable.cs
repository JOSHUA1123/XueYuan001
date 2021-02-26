using System;
using System.Data;
namespace DataBaseInfo
{
	internal interface ISourceTable : IListConvert<IRowReader>, IDataSource<DataTable>, IDisposable
	{
		int RowCount
		{
			get;
		}
		int ColumnCount
		{
			get;
		}
		IRowReader this[int index]
		{
			get;
		}
		SourceTable Select(params string[] names);
		SourceTable Filter(string expression);
		SourceTable Sort(string sort);
		void Add(string name, Type type);
		void Add(string name, Type type, string expression);
		void SetOrdinal(string name, int index);
		void Rename(string oldname, string newname);
		void Revalue<T>(string readName, string changeName, ReturnValue<T> revalue);
		void Remove(params string[] names);
		void Fill(FillRelation relation, params string[] fillNames);
		SourceReader ToReader();
	}
}
