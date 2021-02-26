using System;
namespace DataBaseInfo
{
	internal interface IProvider
	{
		void SetDbProvider(DbProvider dbProvider, DbTrans dbTran);
	}
}
