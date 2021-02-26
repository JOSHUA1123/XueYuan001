using System;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Threading;
namespace Norm.Protocol.Messages
{
	internal class GetMoreMessage<T> : Message
	{
		private static readonly byte[] _opBytes = BitConverter.GetBytes(2005);
		private static readonly byte[] _numberToGet = BitConverter.GetBytes(100);
		private readonly long _cursorId;
		private readonly int _limit;
		internal GetMoreMessage(IConnection connection, string fullyQualifiedCollectionName, long cursorID, int limit) : base(connection, fullyQualifiedCollectionName)
		{
			this._cursorId = cursorID;
			this._limit = limit;
		}
		public ReplyMessage<T> Execute()
		{
			byte[] bytes = Encoding.UTF8.GetBytes(this._collection);
			int num = bytes.Length + 1;
			int num2 = 32 + num;
			byte[] array = new byte[num2];
			Buffer.BlockCopy(BitConverter.GetBytes(num2), 0, array, 0, 4);
			Buffer.BlockCopy(BitConverter.GetBytes(this._requestID), 0, array, 4, 4);
			Buffer.BlockCopy(GetMoreMessage<T>._opBytes, 0, array, 12, 4);
			Buffer.BlockCopy(bytes, 0, array, 20, bytes.Length);
			Buffer.BlockCopy(GetMoreMessage<T>._numberToGet, 0, array, 20 + num, 4);
			Buffer.BlockCopy(BitConverter.GetBytes(this._cursorId), 0, array, 24 + num, 8);
			this._connection.Write(array, 0, num2);
			NetworkStream stream = this._connection.GetStream();
			while (!stream.DataAvailable)
			{
				Thread.Sleep(1);
			}
			if (!stream.DataAvailable)
			{
				throw new TimeoutException("MongoDB did not return a reply in the specified time for this context: " + this._connection.QueryTimeout.ToString());
			}
			return new ReplyMessage<T>(this._connection, this._collection, new BinaryReader(new BufferedStream(stream)), MongoOp.GetMore, this._limit);
		}
	}
}
