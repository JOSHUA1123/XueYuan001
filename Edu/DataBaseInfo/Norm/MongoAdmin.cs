using Norm.Protocol.SystemMessages.Requests;
using Norm.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
namespace Norm
{
	public class MongoAdmin : IMongoAdmin, IDisposable
	{
		private readonly IConnection _connection;
		private readonly IConnectionProvider _connectionProvider;
		private bool _disposed;
		public IMongoDatabase Database
		{
			get;
			private set;
		}
		public MongoAdmin(string connectionString)
		{
			this._connectionProvider = ConnectionProviderFactory.Create(connectionString);
			this._connection = this._connectionProvider.Open(null);
			this.Database = new MongoDatabase(this._connectionProvider.ConnectionString.Database, this._connection);
		}
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}
		public DroppedDatabaseResponse DropDatabase()
		{
			return this.Database.GetCollection<DroppedDatabaseResponse>("$cmd").FindOne<DropDatabaseRequest>(new DropDatabaseRequest());
		}
		public IEnumerable<CurrentOperationResponse> GetCurrentOperations()
		{
			return this.Database.GetCollection<CurrentOperationContainer>("$cmd.sys.inprog").FindOne<object>(new object()).Responses;
		}
		public bool ResetLastError()
		{
			GenericCommandResponse genericCommandResponse = this.Database.GetCollection<GenericCommandResponse>("$cmd").FindOne(new
			{
				reseterror = 1.0
			});
			return genericCommandResponse != null && genericCommandResponse.WasSuccessful;
		}
		public PreviousErrorResponse PreviousErrors()
		{
			return this.Database.GetCollection<PreviousErrorResponse>("$cmd").FindOne(new
			{
				getpreverror = 1.0
			});
		}
		public AssertInfoResponse AssertionInfo()
		{
			return this.Database.GetCollection<AssertInfoResponse>("$cmd").FindOne(new
			{
				assertinfo = 1.0
			});
		}
		public ServerStatusResponse ServerStatus()
		{
			return this.Database.GetCollection<ServerStatusResponse>("$cmd").FindOne(new
			{
				serverStatus = 1.0
			});
		}
		public bool SetProfileLevel(int value)
		{
			ProfileLevelResponse profileLevelResponse = this.Database.GetCollection<ProfileLevelResponse>("$cmd").FindOne(new
			{
				profile = value
			});
			return profileLevelResponse != null && profileLevelResponse.WasSuccessful;
		}
		public bool SetProfileLevel(int value, out int previousLevel)
		{
			ProfileLevelResponse profileLevelResponse = this.Database.GetCollection<ProfileLevelResponse>("$cmd").FindOne(new
			{
				profile = value
			});
			previousLevel = profileLevelResponse.PreviousLevel;
			return profileLevelResponse != null && profileLevelResponse.WasSuccessful;
		}
		public int GetProfileLevel()
		{
			ProfileLevelResponse profileLevelResponse = this.Database.GetCollection<ProfileLevelResponse>("$cmd").FindOne(new
			{
				profile = -1
			});
			return profileLevelResponse.PreviousLevel;
		}
		public bool RepairDatabase(bool preserveClonedFilesOnFailure, bool backupOriginalFiles)
		{
			GenericCommandResponse genericCommandResponse = this.Database.GetCollection<GenericCommandResponse>("$cmd").FindOne(new
			{
				repairDatabase = 1.0,
				preserveClonedFilesOnFailure = preserveClonedFilesOnFailure,
				backupOriginalFiles = backupOriginalFiles
			});
			return genericCommandResponse != null && genericCommandResponse.WasSuccessful;
		}
		public GenericCommandResponse KillOperation(double operationId)
		{
			this.AssertConnectedToAdmin();
			return this.Database.GetCollection<GenericCommandResponse>("$cmd.sys.killop").FindOne(new
			{
				op = operationId
			});
		}
		public IEnumerable<DatabaseInfo> GetAllDatabases()
		{
			this.AssertConnectedToAdmin();
			ListDatabasesResponse listDatabasesResponse = this.Database.GetCollection<ListDatabasesResponse>("$cmd").FindOne<ListDatabasesRequest>(new ListDatabasesRequest());
			if (listDatabasesResponse == null || !listDatabasesResponse.WasSuccessful)
			{
				return Enumerable.Empty<DatabaseInfo>();
			}
			return listDatabasesResponse.Databases;
		}
		public ForceSyncResponse ForceSync(bool async)
		{
			this.AssertConnectedToAdmin();
			return this.Database.GetCollection<ForceSyncResponse>("$cmd").FindOne(new
			{
				fsync = 1.0,
				async = async
			});
		}
		public BuildInfoResponse BuildInfo()
		{
			this.AssertConnectedToAdmin();
			return this.Database.GetCollection<BuildInfoResponse>("$cmd").FindOne(new
			{
				buildinfo = 1.0
			});
		}
		private void AssertConnectedToAdmin()
		{
			if (this._connectionProvider.ConnectionString.Database != "admin")
			{
				throw new MongoException("This command is only valid when connected to admin");
			}
		}
		protected virtual void Dispose(bool disposing)
		{
			if (!this._disposed && disposing && this._connection != null)
			{
				this._connectionProvider.Close(this._connection);
			}
			this._disposed = true;
		}
		~MongoAdmin()
		{
			this.Dispose(false);
		}
	}
}
