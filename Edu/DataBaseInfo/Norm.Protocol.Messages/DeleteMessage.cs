using Norm.BSON;
using System;
using System.Text;
namespace Norm.Protocol.Messages
{
	internal class DeleteMessage<U> : Message
	{
		private static readonly byte[] _opBytes = BitConverter.GetBytes(2006);
		private readonly U _templateDocument;
		internal DeleteMessage(IConnection connection, string collection, U templateDocument) : base(connection, collection)
		{
			this._templateDocument = templateDocument;
		}
		internal void Execute()
		{
			byte[] array = BsonSerializer.Serialize<U>(this._templateDocument);
			byte[] bytes = Encoding.UTF8.GetBytes(this._collection);
			int num = 24 + array.Length + bytes.Length + 1;
			byte[] array2 = new byte[num - array.Length];
			Buffer.BlockCopy(BitConverter.GetBytes(num), 0, array2, 0, 4);
			Buffer.BlockCopy(DeleteMessage<U>._opBytes, 0, array2, 12, 4);
			Buffer.BlockCopy(bytes, 0, array2, 20, bytes.Length);
			this._connection.Write(array2, 0, array2.Length);
			this._connection.Write(array, 0, array.Length);
		}
	}
}
