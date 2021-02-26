using Norm.BSON;
using Norm.Collections;
using Norm.Commands.Modifiers;
using Norm.Linq;
using Norm.Protocol.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
namespace Norm
{
	public static class MongoCollectionExtensions
	{
		public static void CreateIndex<T, U>(this IMongoCollection<T> collection, Expression<Func<T, U>> index, string indexName, bool isUnique, IndexOption direction)
		{
			NewExpression newExpression = index.Body as NewExpression;
			Expando expando = new Expando();
			if (newExpression != null)
			{
				using (IEnumerator<MemberExpression> enumerator = newExpression.Arguments.OfType<MemberExpression>().GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						MemberExpression current = enumerator.Current;
						expando[current.GetPropertyAlias()] = direction;
					}
					goto IL_87;
				}
			}
			if (index.Body is MemberExpression)
			{
				MemberExpression mex = index.Body as MemberExpression;
				expando[mex.GetPropertyAlias()] = direction;
			}
			IL_87:
			collection.CreateIndex(expando, indexName, isUnique);
		}
		public static IEnumerable<Z> Find<T, U, O, Z>(this IMongoCollection<T> collection, U template, O orderBy, int limit, int skip, Expression<Func<T, Z>> fieldSelection)
		{
			return collection.Find<U, O, Z>(template, orderBy, limit, skip, collection.FullyQualifiedName, fieldSelection);
		}
		public static IEnumerable<T> Find<T, U>(this IMongoCollection<T> collection, U template, int limit, int skip, string fullyQualifiedName)
		{
			return collection.Find<U, object>(template, null, limit, skip, fullyQualifiedName);
		}
		public static IEnumerable<T> Find<T, U>(this IMongoCollection<T> collection, U template)
		{
			return collection.Find(template, 2147483647);
		}
		public static IEnumerable<T> Find<T, U>(this IMongoCollection<T> collection, U template, int limit)
		{
			return collection.Find(template, limit, 0, collection.FullyQualifiedName);
		}
		public static IEnumerable<T> Find<T, U>(this IMongoCollection<T> collection, U template, int limit, int skip)
		{
			return collection.Find(template, limit, skip, collection.FullyQualifiedName);
		}
		public static IEnumerable<T> Find<T, U, O>(this IMongoCollection<T> collection, U template, O orderby, int limit, int skip)
		{
			return collection.Find<U, O>(template, orderby, limit, skip, collection.FullyQualifiedName);
		}
		public static IEnumerable<T> Find<T, U>(this IMongoCollection<T> collection, U template, int limit, string fullyQualifiedName)
		{
			return collection.Find(template, limit, 0, fullyQualifiedName);
		}
		public static IEnumerable<T> Find<T, U, S>(this IMongoCollection<T> collection, U template, S orderBy)
		{
			return collection.Find<U, S>(template, orderBy, 2147483647, 0, collection.FullyQualifiedName);
		}
		public static IEnumerable<T> Find<T>(this IMongoCollection<T> collection)
		{
			return collection.Find(new object(), 2147483647, collection.FullyQualifiedName);
		}
		public static void Insert<T>(this IMongoCollection<T> collection, params T[] documentsToInsert)
		{
			collection.Insert(documentsToInsert.AsEnumerable<T>());
		}
		public static T FindAndModify<T, U, X>(this IMongoCollection<T> collection, U query, X update)
		{
			return collection.FindAndModify<U, X, object>(query, update, new
			{

			});
		}
		public static void UpdateOne<T, X, U>(this IMongoCollection<T> collection, X matchDocument, U valueDocument)
		{
			collection.Update<X, U>(matchDocument, valueDocument, false, false);
		}
		public static void Update<T, X>(this IMongoCollection<T> collection, X matchDocument, Action<IModifierExpression<T>> action)
		{
			collection.Update<X>(matchDocument, action, false, false);
		}
		public static long Count<T>(this IMongoCollection<T> collection)
		{
			return collection.Count(new
			{

			});
		}
		public static bool DeleteIndices<T>(this IMongoCollection<T> collection, out int numberDeleted)
		{
			return collection.DeleteIndex("*", out numberDeleted);
		}
	}
}
