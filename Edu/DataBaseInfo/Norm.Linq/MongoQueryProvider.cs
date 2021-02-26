using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
namespace Norm.Linq
{
	internal class MongoQueryProvider : IQueryProvider, IMongoQueryResults
	{
		private QueryTranslationResults _results;
		public IMongoDatabase DB
		{
			get;
			private set;
		}
		public string CollectionName
		{
			get;
			set;
		}
		QueryTranslationResults IMongoQueryResults.TranslationResults
		{
			get
			{
				return this._results;
			}
		}
		internal static MongoQueryProvider Create(IMongoDatabase db, string collectionName)
		{
			return new MongoQueryProvider
			{
				DB = db,
				CollectionName = collectionName
			};
		}
		IQueryable<S> IQueryProvider.CreateQuery<S>(Expression expression)
		{
			return new MongoQuery<S>(this, expression);
		}
		IQueryable IQueryProvider.CreateQuery(Expression expression)
		{
			Type elementType = LinqTypeHelper.GetElementType(expression.Type);
			IQueryable result;
			try
			{
				result = (IQueryable)Activator.CreateInstance(typeof(MongoQuery<>).MakeGenericType(new Type[]
				{
					elementType
				}), new object[]
				{
					this,
					expression
				});
			}
			catch (TargetInvocationException ex)
			{
				throw ex.InnerException;
			}
			return result;
		}
		S IQueryProvider.Execute<S>(Expression expression)
		{
			object value = this.ExecuteQuery<S>(expression);
			return (S)((object)Convert.ChangeType(value, typeof(S)));
		}
		object IQueryProvider.Execute(Expression expression)
		{
			return this.Execute(expression);
		}
		public object ExecuteQuery<T>(Expression expression)
		{
			expression = PartialEvaluator.Eval(expression, new Func<Expression, bool>(this.CanBeEvaluatedLocally));
			QueryTranslationResults queryTranslationResults = new MongoQueryTranslator
			{
				CollectionName = this.CollectionName
			}.Translate(expression);
			this._results = queryTranslationResults;
			MongoQueryExecutor mongoQueryExecutor = new MongoQueryExecutor(this.DB, queryTranslationResults);
			object result;
			if (queryTranslationResults.Select != null)
			{
				MethodInfo method = mongoQueryExecutor.GetType().GetMethod("Execute");
				MethodInfo methodInfo = method.MakeGenericMethod(new Type[]
				{
					queryTranslationResults.OriginalSelectType
				});
				result = methodInfo.Invoke(mongoQueryExecutor, new object[0]);
			}
			else
			{
				result = mongoQueryExecutor.Execute<T>();
			}
			return result;
		}
		public object Execute(Expression expression)
		{
			Type elementType = LinqTypeHelper.GetElementType(expression.Type);
			object result;
			try
			{
				result = typeof(MongoQueryProvider).GetMethod("ExecuteQuery").MakeGenericMethod(new Type[]
				{
					elementType
				}).Invoke(this, new object[]
				{
					expression
				});
			}
			catch (TargetInvocationException ex)
			{
				throw ex.InnerException;
			}
			return result;
		}
		private bool CanBeEvaluatedLocally(Expression expression)
		{
			ConstantExpression constantExpression = expression as ConstantExpression;
			if (constantExpression != null)
			{
				IQueryable queryable = constantExpression.Value as IQueryable;
				if (queryable != null && queryable.Provider == this)
				{
					return false;
				}
			}
			MethodCallExpression methodCallExpression = expression as MethodCallExpression;
			return (methodCallExpression == null || (!(methodCallExpression.Method.DeclaringType == typeof(Enumerable)) && !(methodCallExpression.Method.DeclaringType == typeof(Queryable)))) && ((expression.NodeType == ExpressionType.Convert && expression.Type == typeof(object)) || (expression.NodeType != ExpressionType.Parameter && expression.NodeType != ExpressionType.Lambda));
		}
	}
}
