using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
namespace Norm.Protocol.Messages
{
	internal class InsertMessage<T> : Message
	{
		private static readonly byte[] _opBytes = BitConverter.GetBytes(2002);
		private readonly T[] _elementsToInsert;
		public InsertMessage(IConnection connection, string collectionName, IEnumerable<T> itemsToInsert) : base(connection, collectionName)
		{
			this._elementsToInsert = itemsToInsert.ToArray<T>();
		}
		public void Execute()
		{
			List<byte> list = new List<byte>();
			T[] elementsToInsert = this._elementsToInsert;
			for (int i = 0; i < elementsToInsert.Length; i++)
			{
				T data = elementsToInsert[i];
				list.AddRange(Message.GetPayload<T>(data));
			}
			byte[] bytes = Encoding.UTF8.GetBytes(this._collection);
			int num = bytes.Length + 1;
			int num2 = 20 + list.Count + num;
			byte[] array = new byte[num2 - list.Count];
			Buffer.BlockCopy(BitConverter.GetBytes(num2), 0, array, 0, 4);
			Buffer.BlockCopy(InsertMessage<T>._opBytes, 0, array, 12, 4);
			Buffer.BlockCopy(bytes, 0, array, 20, bytes.Length);
			this._connection.Write(array, 0, array.Length);
			this._connection.Write(list.ToArray(), 0, list.Count);
		}
	}
}
