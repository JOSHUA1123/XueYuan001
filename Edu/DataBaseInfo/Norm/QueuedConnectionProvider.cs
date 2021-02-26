using System;
using System.Collections.Generic;
namespace Norm
{
	internal class QueuedConnectionProvider : ConnectionProvider
	{
		private readonly ConnectionStringBuilder _builder;
		private readonly Queue<IConnection> _idlePool;
		public override ConnectionStringBuilder ConnectionString
		{
			get
			{
				return this._builder;
			}
		}
		public QueuedConnectionProvider(ConnectionStringBuilder builder)
		{
			this._builder = builder;
			this._idlePool = new Queue<IConnection>(builder.PoolSize);
			this.PreQueueConnections(builder.PoolSize);
		}
		public override IConnection Open(string options)
		{
			IConnection connection = null;
			using (TimedLock.Lock(this._idlePool))
			{
				if (this._idlePool.Count > 0)
				{
					connection = this._idlePool.Dequeue();
				}
			}
			if (connection == null)
			{
				connection = base.CreateNewConnection();
			}
			if (!string.IsNullOrEmpty(options))
			{
				connection.LoadOptions(options);
			}
			return connection;
		}
		public override void Close(IConnection connection)
		{
			using (TimedLock.Lock(this._idlePool))
			{
				if (this._idlePool.Count < this._builder.PoolSize)
				{
					this._idlePool.Enqueue(connection);
				}
				else
				{
					if (connection.Client.Connected)
					{
						connection.Client.Close();
					}
				}
			}
		}
		private void PreQueueConnections(int poolSize)
		{
			for (int i = 0; i < poolSize; i++)
			{
				using (TimedLock.Lock(this._idlePool))
				{
					if (this._idlePool.Count < poolSize)
					{
						this._idlePool.Enqueue(base.CreateNewConnection());
					}
				}
			}
		}
	}
}
