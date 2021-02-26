using Norm.BSON;
using Norm.Collections;
using Norm.Responses;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
namespace Norm.Linq
{
	internal class MongoQuery<T> : IOrderedQueryable<T>, IQueryable<T>, IEnumerable<T>, IOrderedQueryable, IQueryable, IEnumerable, IMongoQuery
	{
		private readonly Expression _expression;
		private readonly MongoQueryProvider _provider;
		public string CollectionName
		{
			get
			{
				return this._provider.CollectionName;
			}
		}
		Expression IQueryable.Expression
		{
			get
			{
				return this._expression;
			}
		}
		Type IQueryable.ElementType
		{
			get
			{
				return typeof(T);
			}
		}
		IQueryProvider IQueryable.Provider
		{
			get
			{
				return this._provider;
			}
		}
		public MongoQuery(MongoQueryProvider provider)
		{
			if (provider == null)
			{
				throw new ArgumentNullException("provider");
			}
			this._provider = provider;
			this._expression = Expression.Constant(this);
		}
		public MongoQuery(MongoQueryProvider provider, Expression expression)
		{
			if (provider == null)
			{
				throw new ArgumentNullException("provider");
			}
			if (expression == null)
			{
				throw new ArgumentNullException("expression");
			}
			if (!typeof(IQueryable<T>).IsAssignableFrom(expression.Type))
			{
				throw new ArgumentOutOfRangeException("expression");
			}
			this._provider = provider;
			this._expression = expression;
		}
		public Expression GetExpression()
		{
			return this._expression;
		}
		public virtual IEnumerator<T> GetEnumerator()
		{
			return ((IEnumerable<T>)this._provider.ExecuteQuery<T>(this._expression)).GetEnumerator();
		}
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable)this._provider.Execute(this._expression)).GetEnumerator();
		}
		internal ExplainResponse Explain(Expando query)
		{
			return this.GetCollection<ExplainResponse>(this._provider.CollectionName).Explain<Expando>(query);
		}
		private IMongoCollection<TCollection> GetCollection<TCollection>()
		{
			return this.GetCollection<TCollection>(this._provider.CollectionName);
		}
		private IMongoCollection<TCollection> GetCollection<TCollection>(string collectionName)
		{
			return this._provider.DB.GetCollection<TCollection>(collectionName);
		}
		public override string ToString()
		{
			if (this._expression.NodeType == ExpressionType.Constant && ((ConstantExpression)this._expression).Value == this)
			{
				return "Query(" + typeof(T) + ")";
			}
			return this._expression.ToString();
		}
	}
}
