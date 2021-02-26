using System;
using System.Text;
namespace Norm.Protocol.Messages
{
	internal class UpdateMessage<T, U> : Message
	{
		private static readonly byte[] _opBytes = BitConverter.GetBytes(2001);
		protected T _matchDocument;
		protected UpdateOption _options;
		protected U _valueDocument;
		internal UpdateMessage(IConnection connection, string collection, UpdateOption options, T matchDocument, U valueDocument) : base(connection, collection)
		{
			this._options = options;
			this._matchDocument = matchDocument;
			this._valueDocument = valueDocument;
		}
		public void Execute()
		{
			byte[] payload = Message.GetPayload<T>(this._matchDocument);
			byte[] payload2 = Message.GetPayload<U>(this._valueDocument);
			byte[] bytes = Encoding.UTF8.GetBytes(this._collection);
			int num = bytes.Length + 1;
			int num2 = 24 + payload.Length + payload2.Length + num;
			byte[] array = new byte[num2 - payload.Length - payload2.Length];
			Buffer.BlockCopy(BitConverter.GetBytes(num2), 0, array, 0, 4);
			Buffer.BlockCopy(UpdateMessage<T, U>._opBytes, 0, array, 12, 4);
			Buffer.BlockCopy(bytes, 0, array, 20, bytes.Length);
			Buffer.BlockCopy(BitConverter.GetBytes((int)this._options), 0, array, 20 + num, 4);
			this._connection.Write(array, 0, array.Length);
			this._connection.Write(payload, 0, payload.Length);
			this._connection.Write(payload2, 0, payload2.Length);
		}
	}
}
