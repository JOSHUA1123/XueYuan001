using Norm.Collections;
using Norm.Configuration;
using Norm.Protocol.SystemMessages;
using Norm.Protocol.SystemMessages.Request;
using Norm.Responses;
using System;
using System.Collections.Generic;
namespace Norm
{
	public class MongoDatabase : IMongoDatabase
	{
		private readonly IConnection _connection;
		private readonly string _databaseName;
		public IConnection CurrentConnection
		{
			get
			{
				return this._connection;
			}
		}
		public string DatabaseName
		{
			get
			{
				return this._databaseName;
			}
		}
		public MongoDatabase(string databaseName, IConnection connection)
		{
			this._databaseName = databaseName;
			this._connection = connection;
		}
		public MapReduce CreateMapReduce()
		{
			return new MapReduce(this);
		}
		public IMongoCollection GetCollection(string collectionName)
		{
			return new MongoCollection(collectionName, this, this.CurrentConnection);
		}
		public IMongoCollection<T> GetCollection<T>(string collectionName)
		{
			return new MongoCollection<T>(collectionName, this, this._connection);
		}
		public IMongoCollection<T> GetCollection<T>()
		{
			string collectionName = MongoConfiguration.GetCollectionName(typeof(T));
			return this.GetCollection<T>(collectionName);
		}
		public IEnumerable<CollectionInfo> GetAllCollections()
		{
			return this.GetCollection<CollectionInfo>("system.namespaces").Find<CollectionInfo>();
		}
		public CollectionStatistics GetCollectionStatistics(string collectionName)
		{
			CollectionStatistics result;
			try
			{
				result = this.GetCollection<CollectionStatistics>("$cmd").FindOne(new
				{
					collstats = collectionName
				});
			}
			catch (MongoException ex)
			{
				if (this._connection.StrictMode || ex.Message != "ns not found")
				{
					throw;
				}
				result = null;
			}
			return result;
		}
		public bool DropCollection(string collectionName)
		{
			bool result;
			try
			{
				result = this.GetCollection<DroppedCollectionResponse>("$cmd").FindOne(new
				{
					drop = collectionName
				}).WasSuccessful;
			}
			catch (MongoException ex)
			{
				if (this._connection.StrictMode || ex.Message != "ns not found")
				{
					throw;
				}
				result = false;
			}
			return result;
		}
		public bool CreateCollection(CreateCollectionOptions options)
		{
			bool result;
			try
			{
				result = this.GetCollection<GenericCommandResponse>("$cmd").FindOne<CreateCollectionRequest>(new CreateCollectionRequest(options)).WasSuccessful;
			}
			catch (MongoException ex)
			{
				if (this._connection.StrictMode || ex.Message != "collection already exists")
				{
					throw;
				}
				result = false;
			}
			return result;
		}
		public SetProfileResponse SetProfileLevel(ProfileLevel level)
		{
			return this.GetCollection<SetProfileResponse>("$cmd").FindOne<SetProfileResponse>(new SetProfileResponse
			{
				Profile = new int?((int)level)
			});
		}
		public IEnumerable<ProfilingInformationResponse> GetProfilingInformation()
		{
			return this.GetCollection<ProfilingInformationResponse>("system.profile").Find<ProfilingInformationResponse>();
		}
		public ValidateCollectionResponse ValidateCollection(string collectionName, bool scanData)
		{
			return this.GetCollection<ValidateCollectionResponse>("$cmd").FindOne(new
			{
				validate = collectionName,
				scandata = scanData
			});
		}
		public LastErrorResponse LastError()
		{
			return this.GetCollection<LastErrorResponse>("$cmd").FindOne(new
			{
				getlasterror = 1
			});
		}
		public LastErrorResponse LastError(int waitCount)
		{
			return this.GetCollection<LastErrorResponse>("$cmd").FindOne(new
			{
				getlasterror = 1,
				w = waitCount
			});
		}
		public LastErrorResponse LastError(int waitCount, int waitTimeout)
		{
			LastErrorResponse result;
			try
			{
				result = this.GetCollection<LastErrorResponse>("$cmd").FindOne(new
				{
					getlasterror = 1,
					w = waitCount,
					wtimeout = waitTimeout
				});
			}
			catch (MongoException ex)
			{
				if (ex.Message == null)
				{
					ex = new MongoException("Get Last Error timed out.");
				}
				throw ex;
			}
			return result;
		}
	}
}
