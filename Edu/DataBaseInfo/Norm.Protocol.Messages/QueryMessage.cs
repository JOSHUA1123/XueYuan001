using Norm.BSON;
using Norm.Protocol.SystemMessages;
using System;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Threading;
namespace Norm.Protocol.Messages
{
	public class QueryMessage<T, U> : Message
	{
		[Flags]
		internal enum QueryOptions
		{
			None = 0,
			TailabileCursor = 2,
			SlaveOK = 4,
			NoCursorTimeout = 16
		}
		private static readonly byte[] _opBytes = BitConverter.GetBytes(2004);
		internal U Query
		{
			get;
			set;
		}
		internal object FieldSelection
		{
			get;
			set;
		}
		internal object OrderBy
		{
			get;
			set;
		}
		internal int NumberToTake
		{
			get;
			set;
		}
		public int NumberToSkip
		{
			get;
			set;
		}
		public QueryMessage(IConnection connection, string fullyQualifiedCollName) : base(connection, fullyQualifiedCollName)
		{
			this.NumberToTake = 2147483647;
		}
		public ReplyMessage<T> Execute()
		{
			byte[] payload = this.GetPayload();
			byte[] array = new byte[0];
			if (this.FieldSelection != null)
			{
				array = BsonSerializer.Serialize<object>(this.FieldSelection);
			}
			byte[] bytes = Encoding.UTF8.GetBytes(this._collection);
			int num = bytes.Length + 1;
			int num2 = 28 + num;
			int value = num2 + payload.Length + array.Length;
			byte[] array2 = new byte[num2];
			Buffer.BlockCopy(BitConverter.GetBytes(value), 0, array2, 0, 4);
			Buffer.BlockCopy(QueryMessage<T, U>._opBytes, 0, array2, 12, 4);
			Buffer.BlockCopy(bytes, 0, array2, 20, bytes.Length);
			Buffer.BlockCopy(BitConverter.GetBytes(this.NumberToSkip), 0, array2, 20 + num, 4);
			Buffer.BlockCopy(BitConverter.GetBytes(this.NumberToTake), 0, array2, 24 + num, 4);
			this._connection.Write(array2, 0, array2.Length);
			this._connection.Write(payload, 0, payload.Length);
			this._connection.Write(array, 0, array.Length);
			NetworkStream stream = this._connection.GetStream();
			while (!stream.DataAvailable)
			{
				Thread.Sleep(1);
			}
			if (!stream.DataAvailable)
			{
				throw new TimeoutException("MongoDB did not return a reply in the specified time for this context: " + this._connection.QueryTimeout.ToString());
			}
			return new ReplyMessage<T>(this._connection, this._collection, new BinaryReader(new BufferedStream(stream)), MongoOp.Query, this.NumberToTake);
		}
		private byte[] GetPayload()
		{
			if (this.Query != null && this.Query is ISystemQuery)
			{
				return BsonSerializer.Serialize<U>(this.Query);
			}
			Expando expando = new Expando();
			expando["query"] = this.Query;
			if (this.OrderBy != null)
			{
				expando["orderby"] = this.OrderBy;
			}
			return BsonSerializer.Serialize<Expando>(expando);
		}
	}
}
