using Norm.BSON;
using System;
namespace Norm.Protocol
{
	public class Message
	{
		protected const int FOUR_MEGABYTES = 4194304;
		protected string _collection;
		protected IConnection _connection;
		protected int _messageLength;
		protected int _requestID;
		protected int _responseID;
		protected MongoOp _op;
		protected Message(IConnection connection, string fullyQualifiedCollName)
		{
			this._connection = connection;
			this._collection = fullyQualifiedCollName;
		}
		protected static byte[] GetPayload<X>(X data)
		{
			byte[] array = BsonSerializer.Serialize<X>(data);
			if (array.Length > 4194304)
			{
				throw new DocumentExceedsSizeLimitsException<X>(data, array.Length);
			}
			return array;
		}
	}
}
