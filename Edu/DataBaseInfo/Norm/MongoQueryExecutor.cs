using Norm.BSON;
using Norm.Protocol.Messages;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
namespace Norm
{
	public class MongoQueryExecutor<T, U, O> : IEnumerable<O>, IEnumerable
	{
		private readonly Expando _hints = new Expando();
		internal string CollectionName
		{
			get;
			set;
		}
		public QueryMessage<T, U> Message
		{
			get;
			private set;
		}
		private Func<T, O> Translator
		{
			get;
			set;
		}
		public MongoQueryExecutor(QueryMessage<T, U> message, Func<T, O> projection)
		{
			this.Message = message;
			this.Translator = projection;
		}
		public void AddHint(string hint, IndexOption direction)
		{
			this._hints.Set<IndexOption>(hint, direction);
		}
		public IEnumerator<O> GetEnumerator()
		{
			ReplyMessage<T> replyMessage;
			if (this._hints.AllProperties().Count<ExpandoProperty>() == 0)
			{
				replyMessage = this.Message.Execute();
			}
			else
			{
				U query = this.Message.Query;
				Expando expando = new Expando();
				expando["$query"] = query;
				expando["$hint"] = this._hints;
				replyMessage = this.Message.Execute();
			}
			foreach (T current in replyMessage.Results)
			{
				yield return this.Translator(current);
			}
			yield break;
		}
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}
	}
	public class MongoQueryExecutor<T, U> : MongoQueryExecutor<T, U, T>
	{
		public MongoQueryExecutor(QueryMessage<T, U> message) : base(message, (T y) => y)
		{
		}
	}
}
