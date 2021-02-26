using System;
using System.IO;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;
namespace Norm
{
	public class Connection : IConnection, IDisposable, IOptionsContainer
	{
		private readonly ConnectionStringBuilder _builder;
		private readonly TcpClient _client;
		private NetworkStream _netStream;
		private bool _disposed;
		private int? _queryTimeout;
		private bool? _strictMode;
		public TcpClient Client
		{
			get
			{
				return this._client;
			}
		}
		public bool IsConnected
		{
			get
			{
				return this.Client.Connected;
			}
		}
		public bool IsInvalid
		{
			get;
			private set;
		}
		public DateTime Created
		{
			get;
			private set;
		}
		public int QueryTimeout
		{
			get
			{
				int? queryTimeout = this._queryTimeout;
				if (!queryTimeout.HasValue)
				{
					return this._builder.QueryTimeout;
				}
				return queryTimeout.GetValueOrDefault();
			}
		}
		public bool StrictMode
		{
			get
			{
				bool? strictMode = this._strictMode;
				if (!strictMode.HasValue)
				{
					return this._builder.StrictMode;
				}
				return strictMode.GetValueOrDefault();
			}
			set
			{
				this._strictMode = new bool?(value);
			}
		}
		public string UserName
		{
			get
			{
				return this._builder.UserName;
			}
		}
		public string Database
		{
			get
			{
				return this._builder.Database;
			}
		}
		public int VerifyWriteCount
		{
			get;
			private set;
		}
		public string ConnectionString
		{
			get;
			private set;
		}
		internal Connection(ConnectionStringBuilder builder)
		{
			this._builder = builder;
			this.Created = DateTime.Now;
			this._client = new TcpClient
			{
				NoDelay = true,
				ReceiveTimeout = builder.QueryTimeout * 1000,
				SendTimeout = builder.QueryTimeout * 1000
			};
			this._client.Connect(builder.Servers[0].Host, builder.Servers[0].Port);
			this.ConnectionString = builder.ToString();
		}
		public string Digest(string nonce)
		{
			string result;
			using (MD5 mD = MD5.Create())
			{
				byte[] bytes = Encoding.UTF8.GetBytes(nonce + this.UserName + this.CreatePasswordDigest());
				byte[] array = mD.ComputeHash(bytes);
				StringBuilder sb = new StringBuilder(array.Length * 2);
				Array.ForEach<byte>(array, delegate(byte b)
				{
					sb.Append(b.ToString("X2"));
				});
				result = sb.ToString().ToLower();
			}
			return result;
		}
		private string CreatePasswordDigest()
		{
			string result;
			using (MD5 mD = MD5.Create())
			{
				byte[] bytes = Encoding.UTF8.GetBytes(this._builder.UserName + ":mongo:" + this._builder.Password);
				byte[] array = mD.ComputeHash(bytes);
				StringBuilder sb = new StringBuilder(array.Length * 2);
				Array.ForEach<byte>(array, delegate(byte b)
				{
					sb.Append(b.ToString("X2"));
				});
				result = sb.ToString().ToLower();
			}
			return result;
		}
		public NetworkStream GetStream()
		{
			if (this._netStream == null)
			{
				this._netStream = this.Client.GetStream();
			}
			return this._netStream;
		}
		public void LoadOptions(string options)
		{
			ConnectionStringBuilder.BuildOptions(this, options);
		}
		public void SetWriteCount(int writeCount)
		{
			if (writeCount > 1)
			{
				this.VerifyWriteCount = writeCount;
				this.StrictMode = true;
			}
		}
		public void ResetOptions()
		{
			this._queryTimeout = null;
			this._strictMode = null;
		}
		public void Write(byte[] bytes, int start, int size)
		{
			try
			{
				this.GetStream().Write(bytes, 0, size);
			}
			catch (IOException)
			{
				this.IsInvalid = true;
				throw;
			}
		}
		public void SetQueryTimeout(int timeout)
		{
			this._queryTimeout = new int?(timeout);
		}
		public void SetStrictMode(bool strict)
		{
			this._strictMode = new bool?(strict);
		}
		public void SetPoolSize(int size)
		{
			throw new MongoException("PoolSize cannot be provided as an override option");
		}
		public void SetPooled(bool pooled)
		{
			throw new MongoException("Connection pooling cannot be provided as an override option");
		}
		public void SetTimeout(int timeout)
		{
			throw new MongoException("Timeout cannot be provided as an override option");
		}
		public void SetLifetime(int lifetime)
		{
			throw new MongoException("Lifetime cannot be provided as an override option");
		}
		public void Dispose()
		{
			this.Dispose(true);
		}
		protected virtual void Dispose(bool disposing)
		{
			if (this._disposed)
			{
				return;
			}
			this._client.Close();
			if (this._netStream != null)
			{
				this._netStream.Flush();
				this._netStream.Close();
			}
			this._disposed = true;
		}
		~Connection()
		{
			this.Dispose(false);
		}
	}
}
