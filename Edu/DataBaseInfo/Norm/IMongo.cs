using Norm.Collections;
using Norm.Responses;
using System;
namespace Norm
{
	public interface IMongo : IDisposable
	{
		IConnectionProvider ConnectionProvider
		{
			get;
		}
		IMongoDatabase Database
		{
			get;
		}
		IMongoCollection<T> GetCollection<T>(string collectionName);
		IMongoCollection<T> GetCollection<T>();
		LastErrorResponse LastError();
	}
}
