using Norm.Collections;
using Norm.Protocol.SystemMessages;
using Norm.Responses;
using System;
using System.Collections.Generic;
namespace Norm
{
	public interface IMongoDatabase
	{
		IConnection CurrentConnection
		{
			get;
		}
		string DatabaseName
		{
			get;
		}
		bool CreateCollection(CreateCollectionOptions options);
		MapReduce CreateMapReduce();
		bool DropCollection(string collectionName);
		IEnumerable<CollectionInfo> GetAllCollections();
		IMongoCollection<T> GetCollection<T>(string collectionName);
		IMongoCollection GetCollection(string collectionName);
		IMongoCollection<T> GetCollection<T>();
		CollectionStatistics GetCollectionStatistics(string collectionName);
		IEnumerable<ProfilingInformationResponse> GetProfilingInformation();
		LastErrorResponse LastError();
		LastErrorResponse LastError(int verifyCount);
		LastErrorResponse LastError(int waitCount, int waitTimeout);
		SetProfileResponse SetProfileLevel(ProfileLevel level);
		ValidateCollectionResponse ValidateCollection(string collectionName, bool scanData);
	}
}
