using Norm.Collections;
using Norm.Responses;
using System;
namespace Norm
{
	public class Mongo : IMongo, IDisposable
	{
		private readonly string _options;
		private IConnection _connection;
		private bool _disposed;
		public IMongoDatabase Database
		{
			get;
			private set;
		}
		public IConnectionProvider ConnectionProvider
		{
			get;
			private set;
		}
		public Mongo(IConnectionProvider provider, string options)
		{
			ConnectionStringBuilder connectionString = provider.ConnectionString;
			this._options = options;
			this.ConnectionProvider = provider;
			this.Database = new MongoDatabase(connectionString.Database, this.ServerConnection());
		}
		public Mongo(string db, string server, string port, string options)
		{
			if (string.IsNullOrEmpty(options))
			{
				options = "strict=false";
			}
			string connectionString = string.Format("mongodb://{0}:{1}/", server, port);
			this._options = options;
			this.ConnectionProvider = ConnectionProviderFactory.Create(connectionString);
			this.Database = new MongoDatabase(db, this.ServerConnection());
		}
		public static IMongo Create(string connectionString)
		{
			return Mongo.Create(connectionString, string.Empty);
		}
		public static IMongo Create(string connectionString, string options)
		{
			return new Mongo(ConnectionProviderFactory.Create(connectionString), options);
		}
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}
		public IMongoCollection<T> GetCollection<T>()
		{
			return this.Database.GetCollection<T>();
		}
		public IMongoCollection<T> GetCollection<T>(string collectionName)
		{
			return this.Database.GetCollection<T>(collectionName);
		}
		public LastErrorResponse LastError()
		{
			return this.Database.LastError();
		}
		internal IConnection ServerConnection()
		{
			if (this._connection == null)
			{
				this._connection = this.ConnectionProvider.Open(this._options);
			}
			return this._connection;
		}
		protected virtual void Dispose(bool disposing)
		{
			if (!this._disposed && disposing && this._connection != null)
			{
				this.ConnectionProvider.Close(this._connection);
			}
			this._disposed = true;
		}
		~Mongo()
		{
			this.Dispose(false);
		}
	}
}
