using System;
using System.Data;
using System.Data.Common;
using DataBaseInfo.Cache;
using DataBaseInfo.Logger;
namespace DataBaseInfo
{
	internal interface IDbSession : IDbTrans, IDbProcess
	{
		void SetProvider(string connectName);
		void SetProvider(DbProvider dbProvider);
		DbTrans BeginTrans();
		DbTrans BeginTrans(IsolationLevel isolationLevel);
		DbTrans SetTransaction(DbTransaction trans);
		DbTrans SetConnection(DbConnection connection);
		DbTransaction BeginTransaction();
		DbTransaction BeginTransaction(IsolationLevel isolationLevel);
		DbConnection CreateConnection();
		DbParameter CreateParameter();
		void RegisterDecryptor(DecryptEventHandler handler);
		void RegisterLogger(IExecuteLog logger);
		void RegisterCache(ICacheStrategy cache);
		void SetCommandTimeout(int timeout);
		string Serialization(WhereClip where);
		string Serialization(OrderByClip order);
	}
}
