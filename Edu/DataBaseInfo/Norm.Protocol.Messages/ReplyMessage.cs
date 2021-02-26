using Norm.BSON;
using System;
using System.Collections.Generic;
using System.IO;
namespace Norm.Protocol.Messages
{
	public class ReplyMessage<T> : Message, IDisposable
	{
		private readonly List<T> _results;
		private readonly int _limit;
		private MongoOp _originalOperation;
		private ReplyMessage<T> _addedReturns;
		public long CursorID
		{
			get;
			protected set;
		}
		public int CursorPosition
		{
			get;
			protected set;
		}
		public bool HasError
		{
			get;
			protected set;
		}
		public IEnumerable<T> Results
		{
			get
			{
				foreach (T current in this._results)
				{
					yield return current;
				}
				if (this.CursorID != 0L && this._results.Count > 0 && this._limit - this._results.Count > 0)
				{
					this._addedReturns = new GetMoreMessage<T>(this._connection, this._collection, this.CursorID, this._limit - this._results.Count).Execute();
				}
				if (this._addedReturns != null)
				{
					foreach (T current2 in this._addedReturns.Results)
					{
						yield return current2;
					}
				}
				yield break;
			}
		}
		internal ReplyMessage(IConnection connection, string fullyQualifiedCollestionName, BinaryReader reply, MongoOp originalOperation, int limit) : base(connection, fullyQualifiedCollestionName)
		{
			this._originalOperation = originalOperation;
			this._messageLength = reply.ReadInt32();
			this._requestID = reply.ReadInt32();
			this._responseID = reply.ReadInt32();
			this._op = (MongoOp)reply.ReadInt32();
			this._limit = limit;
			this.HasError = (reply.ReadInt32() == 1);
			this.CursorID = reply.ReadInt64();
			this.CursorPosition = reply.ReadInt32();
			int capacity = reply.ReadInt32();
			this._messageLength -= 40;
			this._results = new List<T>(capacity);
			if (this.HasError)
			{
				return;
			}
			while (this._messageLength > 0)
			{
				int num = reply.ReadInt32();
				if (num > 0)
				{
					IDictionary<WeakReference, Expando> dictionary = new Dictionary<WeakReference, Expando>(0);
					T item = BsonDeserializer.Deserialize<T>(num, reply, ref dictionary);
					this._results.Add(item);
				}
				this._messageLength -= num;
			}
		}
		public void Dispose()
		{
		}
	}
}
