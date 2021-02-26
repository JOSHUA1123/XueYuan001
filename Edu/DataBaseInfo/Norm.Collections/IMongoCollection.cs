using Norm.BSON;
using Norm.Commands.Modifiers;
using Norm.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
namespace Norm.Collections
{
	public interface IMongoCollection<T>
	{
		bool Updateable
		{
			get;
		}
		string FullyQualifiedName
		{
			get;
		}
		T FindOne<U>(U template);
		IMongoCollection<U> GetChildCollection<U>(string collectionName) where U : class, new();
		void Update<X, U>(X matchDocument, U valueDocument, bool updateMultiple, bool upsert);
		void Update<X>(X matchDocument, Action<IModifierExpression<T>> action, bool updateMultiple, bool upsert);
		void Delete<U>(U template);
		void Delete(T entity);
		IEnumerable<X> MapReduce<X>(string map, string reduce);
		IEnumerable<X> MapReduce<U, X>(U template, string map, string reduce);
		IEnumerable<X> MapReduce<U, X>(U template, string map, string reduce, string finalize);
		IEnumerable<X> MapReduce<X>(MapReduceOptions<T> options);
		T FindAndModify<U, X, Y>(U query, X update, Y sort);
		IEnumerable<T> Find<U, S>(U template, S orderBy, int limit, int skip, string fullyQualifiedName);
		IEnumerable<T> Find<U, O, Z>(U template, O orderBy, Z fieldSelector, int limit, int skip);
		IEnumerable<Z> Find<U, O, Z>(U template, O orderBy, int limit, int skip, string fullName, Expression<Func<T, Z>> fieldSelection);
		IQueryable<T> AsQueryable();
		void Insert(IEnumerable<T> documentsToInsert);
		ExplainResponse Explain<U>(U template);
		void CreateIndex(Expando key, string indexName, bool isUnique);
		bool DeleteIndex(string indexName, out int numberDeleted);
		void Save(T entity);
		CollectionStatistics GetCollectionStatistics();
		long Count<U>(U query);
		IEnumerable<U> Distinct<U>(string keyName);
		long GenerateId();
	}
	public interface IMongoCollection : IMongoCollection<object>
	{
	}
}
