using System;
namespace DataBaseInfo
{
	internal interface IDeleteCreator : IWhereCreator<DeleteCreator>, ITableCreator<DeleteCreator>
	{
	}
}
