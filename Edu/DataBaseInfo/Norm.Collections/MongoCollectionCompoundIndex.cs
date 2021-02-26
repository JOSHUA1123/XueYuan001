using Norm.Protocol.Messages;
using System;
using System.Linq.Expressions;
namespace Norm.Collections
{
	public class MongoCollectionCompoundIndex<T>
	{
		public Expression<Func<T, object>> Index
		{
			get;
			set;
		}
		public IndexOption Direction
		{
			get;
			set;
		}
		public MongoCollectionCompoundIndex()
		{
		}
		public MongoCollectionCompoundIndex(Expression<Func<T, object>> index, IndexOption direction)
		{
			this.Index = index;
			this.Direction = direction;
		}
	}
}
