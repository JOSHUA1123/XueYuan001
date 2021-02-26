using System;
namespace Norm
{
	public class NormalConnectionProvider : ConnectionProvider
	{
		private readonly ConnectionStringBuilder _builder;
		public override ConnectionStringBuilder ConnectionString
		{
			get
			{
				return this._builder;
			}
		}
		public NormalConnectionProvider(ConnectionStringBuilder builder)
		{
			this._builder = builder;
		}
		public override IConnection Open(string options)
		{
			return base.CreateNewConnection();
		}
		public override void Close(IConnection connection)
		{
			if (connection.Client.Connected)
			{
				connection.Dispose();
			}
		}
	}
}
