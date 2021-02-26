using System;
namespace DataBaseInfo
{
	internal interface ITableCreator<TCreator> where TCreator : class
	{
		TCreator From(Table table);
		TCreator From(string tableName);
	}
}
