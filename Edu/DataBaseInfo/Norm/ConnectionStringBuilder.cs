using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;
namespace Norm
{
	public class ConnectionStringBuilder : IOptionsContainer
	{
		private const string DEFAULT_DATABASE = "admin";
		private const int DEFAULT_PORT = 27017;
		private const string PROTOCOL = "mongodb://";
		private string _connectionString;
		private static readonly IDictionary<string, Action<string, IOptionsContainer>> _optionsHandler;
		public IList<Server> Servers
		{
			get;
			private set;
		}
		public string UserName
		{
			get;
			private set;
		}
		public string Password
		{
			get;
			private set;
		}
		public string Database
		{
			get;
			private set;
		}
		public int QueryTimeout
		{
			get;
			private set;
		}
		public bool StrictMode
		{
			get;
			private set;
		}
		public bool Pooled
		{
			get;
			private set;
		}
		public int PoolSize
		{
			get;
			private set;
		}
		public int Timeout
		{
			get;
			private set;
		}
		public int Lifetime
		{
			get;
			private set;
		}
		public int VerifyWriteCount
		{
			get;
			private set;
		}
		public override string ToString()
		{
			return this._connectionString;
		}
		private ConnectionStringBuilder()
		{
			this.VerifyWriteCount = 1;
		}
		public static ConnectionStringBuilder Create(string connectionString)
		{
			string text = connectionString;
			if (!text.StartsWith("mongodb://", StringComparison.InvariantCultureIgnoreCase))
			{
				try
				{
					text = ConfigurationManager.ConnectionStrings[connectionString].ConnectionString;
				}
				catch (NullReferenceException)
				{
					throw new MongoException("Connection String must start with 'mongodb://' or be the name of a connection string in the app.config.");
				}
			}
			string[] array = text.Split(new char[]
			{
				'?'
			}, StringSplitOptions.RemoveEmptyEntries);
			string options = (array.Length == 2) ? array[1] : null;
			StringBuilder sb = new StringBuilder(array[0].Substring("mongodb://".Length));
			ConnectionStringBuilder connectionStringBuilder = new ConnectionStringBuilder
			{
				QueryTimeout = 30,
				Timeout = 30,
				StrictMode = true,
				Pooled = true,
				PoolSize = 25,
				Lifetime = 15
			};
			connectionStringBuilder.BuildAuthentication(sb).BuildDatabase(sb).BuildServerList(sb);
			ConnectionStringBuilder.BuildOptions(connectionStringBuilder, options);
			connectionStringBuilder._connectionString = text;
			return connectionStringBuilder;
		}
		public void SetQueryTimeout(int timeout)
		{
			this.QueryTimeout = timeout;
		}
		public void SetWriteCount(int writeCount)
		{
			if (writeCount > 1)
			{
				this.VerifyWriteCount = writeCount;
				this.StrictMode = true;
			}
		}
		public void SetStrictMode(bool strict)
		{
			this.StrictMode = strict;
		}
		public void SetPoolSize(int size)
		{
			this.PoolSize = size;
		}
		public void SetPooled(bool pooled)
		{
			this.Pooled = pooled;
		}
		public void SetTimeout(int timeout)
		{
			this.Timeout = timeout;
		}
		public void SetLifetime(int lifetime)
		{
			this.Lifetime = lifetime;
		}
		internal static void BuildOptions(IOptionsContainer container, string options)
		{
			if (string.IsNullOrEmpty(options))
			{
				return;
			}
			string[] array = options.Split(new char[]
			{
				'&'
			}, StringSplitOptions.RemoveEmptyEntries);
			string[] array2 = array;
			for (int i = 0; i < array2.Length; i++)
			{
				string text = array2[i];
				string[] array3 = text.Split(new char[]
				{
					'='
				}, StringSplitOptions.RemoveEmptyEntries);
				if (array3.Length != 2)
				{
					throw new MongoException("Invalid connection option: " + text);
				}
				ConnectionStringBuilder._optionsHandler[array3[0].ToLower()](array3[1], container);
			}
		}
		private ConnectionStringBuilder BuildAuthentication(StringBuilder sb)
		{
			string text = sb.ToString();
			int num = text.IndexOf('@');
			if (num == -1)
			{
				return this;
			}
			string[] array = text.Substring(0, num).Split(new char[]
			{
				':'
			});
			if (array.Length != 2)
			{
				throw new MongoException("Invalid connection string: authentication should be in the form of username:password");
			}
			this.UserName = array[0];
			this.Password = array[1];
			sb.Remove(0, num + 1);
			return this;
		}
		private ConnectionStringBuilder BuildDatabase(StringBuilder sb)
		{
			string text = sb.ToString();
			int num = text.IndexOf('/');
			if (num == -1)
			{
				this.Database = "admin";
			}
			else
			{
				this.Database = text.Substring(num + 1);
				sb.Remove(num, sb.Length - num);
			}
			return this;
		}
		private void BuildServerList(StringBuilder sb)
		{
			string text = sb.ToString();
			string[] array = text.Split(new char[]
			{
				','
			}, StringSplitOptions.RemoveEmptyEntries);
			if (array.Length == 0)
			{
				throw new MongoException("Invalid connection string: at least 1 server is required");
			}
			List<Server> list = new List<Server>(array.Length);
			string[] array2 = array;
			for (int i = 0; i < array2.Length; i++)
			{
				string text2 = array2[i];
				string[] array3 = text2.Split(new char[]
				{
					':'
				}, StringSplitOptions.RemoveEmptyEntries);
				if (array3.Length > 2)
				{
					throw new MongoException(string.Format("Invalid connection string: {0} is not a valid server configuration", text2));
				}
				list.Add(new Server
				{
					Host = array3[0],
					Port = (array3.Length == 2) ? int.Parse(array3[1]) : 27017
				});
			}
			this.Servers = list.AsReadOnly();
		}
		static ConnectionStringBuilder()
		{
			// Note: this type is marked as 'beforefieldinit'.
			Dictionary<string, Action<string, IOptionsContainer>> dictionary = new Dictionary<string, Action<string, IOptionsContainer>>();
			dictionary.Add("strict", delegate(string v, IOptionsContainer b)
			{
				b.SetStrictMode(bool.Parse(v));
			});
			dictionary.Add("querytimeout", delegate(string v, IOptionsContainer b)
			{
				b.SetQueryTimeout(int.Parse(v));
			});
			dictionary.Add("pooling", delegate(string v, IOptionsContainer b)
			{
				b.SetPooled(bool.Parse(v));
			});
			dictionary.Add("poolsize", delegate(string v, IOptionsContainer b)
			{
				b.SetPoolSize(int.Parse(v));
			});
			dictionary.Add("timeout", delegate(string v, IOptionsContainer b)
			{
				b.SetTimeout(int.Parse(v));
			});
			dictionary.Add("lifetime", delegate(string v, IOptionsContainer b)
			{
				b.SetLifetime(int.Parse(v));
			});
			dictionary.Add("verifywritecount", delegate(string v, IOptionsContainer b)
			{
				b.SetWriteCount(int.Parse(v));
			});
			ConnectionStringBuilder._optionsHandler = dictionary;
		}
	}
}
