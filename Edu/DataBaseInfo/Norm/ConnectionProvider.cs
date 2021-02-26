using Norm.Collections;
using Norm.Protocol.Messages;
using Norm.Protocol.SystemMessages.Requests;
using Norm.Responses;
using System;
using System.Linq;
namespace Norm
{
	public abstract class ConnectionProvider : IConnectionProvider
	{
		public abstract ConnectionStringBuilder ConnectionString
		{
			get;
		}
		public abstract IConnection Open(string options);
		public abstract void Close(IConnection connection);
		protected IConnection CreateNewConnection()
		{
			Connection connection = new Connection(this.ConnectionString);
			try
			{
				if (!this.Authenticate(connection))
				{
					this.Close(connection);
				}
			}
			catch (Exception)
			{
				this.Close(connection);
				throw;
			}
			return connection;
		}
		protected bool Authenticate(IConnection connection)
		{
			if (string.IsNullOrEmpty(this.ConnectionString.UserName))
			{
				return true;
			}
			GetNonceResponse getNonceResponse = new MongoCollection<GetNonceResponse>("$cmd", new MongoDatabase("admin", connection), connection).FindOne(new
			{
				getnonce = 1
			});
			if (getNonceResponse.WasSuccessful)
			{
				ReplyMessage<GenericCommandResponse> replyMessage = new QueryMessage<GenericCommandResponse, AuthenticationRequest>(connection, connection.Database + ".$cmd")
				{
					NumberToTake = 1,
					Query = new AuthenticationRequest
					{
						User = connection.UserName,
						Nonce = getNonceResponse.Nonce,
						Key = connection.Digest(getNonceResponse.Nonce)
					}
				}.Execute();
				return replyMessage.Results.Count<GenericCommandResponse>() == 1 && replyMessage.Results.ElementAt(0).WasSuccessful;
			}
			return false;
		}
	}
}
