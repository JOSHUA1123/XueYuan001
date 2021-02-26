using System;
using System.Collections.Generic;
using System.Threading;
namespace Norm
{
	internal class PooledConnectionProvider : ConnectionProvider
	{
		private readonly ConnectionStringBuilder _builder;
		private readonly Stack<IConnection> _freeConnections = new Stack<IConnection>();
		private readonly List<IConnection> _invalidConnections = new List<IConnection>();
		private readonly int _lifetime;
		private readonly object _lock = new object();
		private readonly Timer _maintenanceTimer;
		private readonly int _poolSize;
		private readonly int _timeout;
		private readonly List<IConnection> _usedConnections = new List<IConnection>();
		public override ConnectionStringBuilder ConnectionString
		{
			get
			{
				return this._builder;
			}
		}
		public PooledConnectionProvider(ConnectionStringBuilder builder)
		{
			this._builder = builder;
			this._timeout = builder.Timeout * 1000;
			this._poolSize = builder.PoolSize;
			this._lifetime = builder.Lifetime;
			this._maintenanceTimer = new Timer(delegate(object o)
			{
				this.Cleanup();
			}, null, 30000, 30000);
		}
		public override IConnection Open(string options)
		{
			IConnection connection = null;
			lock (this._lock)
			{
				if (this._freeConnections.Count > 0)
				{
					connection = this._freeConnections.Pop();
					this._usedConnections.Add(connection);
				}
				else
				{
					if (this._freeConnections.Count + this._usedConnections.Count >= this._poolSize)
					{
						if (!Monitor.Wait(this._lock, this._timeout))
						{
							throw new MongoException("Connection timeout trying to get connection from connection pool");
						}
						return this.Open(options);
					}
				}
			}
			if (connection == null)
			{
				connection = base.CreateNewConnection();
				lock (this._lock)
				{
					this._usedConnections.Add(connection);
				}
			}
			connection.LoadOptions(options);
			return connection;
		}
		public override void Close(IConnection connection)
		{
			if (!this.IsAlive(connection))
			{
				lock (this._lock)
				{
					this._usedConnections.Remove(connection);
					this._invalidConnections.Add(connection);
				}
				return;
			}
			connection.ResetOptions();
			lock (this._lock)
			{
				this._usedConnections.Remove(connection);
				this._freeConnections.Push(connection);
				Monitor.Pulse(this._lock);
			}
		}
		public void Cleanup()
		{
			this.CheckFreeConnectionsAlive();
			this.DisposeInvalidConnections();
		}
		private bool IsAlive(IConnection connection)
		{
			return (this._lifetime <= 0 || !(connection.Created.AddMinutes((double)this._lifetime) < DateTime.Now)) && connection.IsConnected && !connection.IsInvalid;
		}
		private void CheckFreeConnectionsAlive()
		{
			lock (this._lock)
			{
				IConnection[] array = this._freeConnections.ToArray();
				this._freeConnections.Clear();
				IConnection[] array2 = array;
				for (int i = 0; i < array2.Length; i++)
				{
					IConnection connection = array2[i];
					if (this.IsAlive(connection))
					{
						this._freeConnections.Push(connection);
					}
					else
					{
						this._invalidConnections.Add(connection);
					}
				}
			}
		}
		private void DisposeInvalidConnections()
		{
			IConnection[] array;
			lock (this._lock)
			{
				array = this._invalidConnections.ToArray();
				this._invalidConnections.Clear();
			}
			IConnection[] array2 = array;
			for (int i = 0; i < array2.Length; i++)
			{
				IConnection connection = array2[i];
				connection.Dispose();
			}
		}
	}
}
