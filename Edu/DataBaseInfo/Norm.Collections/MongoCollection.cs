using Norm.BSON;
using Norm.Commands.Modifiers;
using Norm.Linq;
using Norm.Protocol;
using Norm.Protocol.Messages;
using Norm.Protocol.SystemMessages.Requests;
using Norm.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
namespace Norm.Collections
{
	public class MongoCollection<T> : IMongoCollection<T>
	{
		private static Dictionary<int, object> _compiledTransforms = new Dictionary<int, object>();
		private static CollectionHiLoIdGenerator _collectionHiLoIdGenerator = new CollectionHiLoIdGenerator(20);
		protected static bool? _updateable;
		protected string _collectionName;
		protected IConnection _connection;
		protected IMongoDatabase _db;
		public bool Updateable
		{
			get
			{
				if (!MongoCollection<T>._updateable.HasValue)
				{
					bool flag = false;
					Type typeFromHandle = typeof(T);
					if (typeFromHandle == typeof(object) || typeFromHandle.GetInterface("IUpdateWithoutId") != null)
					{
						flag = true;
					}
					if (!flag)
					{
						flag = (ReflectionHelper.GetHelperForType(typeof(T)).FindIdProperty() != null);
					}
					MongoCollection<T>._updateable = new bool?(flag);
				}
				return MongoCollection<T>._updateable.Value;
			}
		}
		public string FullyQualifiedName
		{
			get
			{
				return string.Format("{0}.{1}", this._db.DatabaseName, this._collectionName);
			}
		}
		public MongoCollection(string collectionName, IMongoDatabase db, IConnection connection)
		{
			this._db = db;
			this._connection = connection;
			this._collectionName = collectionName;
		}
		public IQueryable<T> AsQueryable()
		{
			return new MongoQuery<T>(MongoQueryProvider.Create(this._db, this._collectionName));
		}
		public void Save(T entity)
		{
			this.AssertUpdatable();
			ReflectionHelper helperForType = ReflectionHelper.GetHelperForType(typeof(T));
			MagicProperty magicProperty = helperForType.FindIdProperty();
			object obj = magicProperty.Getter(entity);
			if (obj == null && (typeof(ObjectId).IsAssignableFrom(magicProperty.Type) || typeof(long?).IsAssignableFrom(magicProperty.Type) || typeof(int?).IsAssignableFrom(magicProperty.Type)))
			{
				this.Insert(new T[]
				{
					entity
				});
				return;
			}
			this.Update(new
			{
				Id = obj
			}, entity, false, true);
		}
		public IMongoCollection<U> GetChildCollection<U>(string collectionName) where U : class, new()
		{
			return new MongoCollection<U>(this._collectionName + "." + collectionName, this._db, this._connection);
		}
		public void Update<X, U>(X matchDocument, U valueDocument, bool updateMultiple, bool upsert)
		{
			this.AssertUpdatable();
			UpdateOption updateOption = UpdateOption.None;
			if (updateMultiple)
			{
				updateOption |= UpdateOption.MultiUpdate;
			}
			if (upsert)
			{
				updateOption |= UpdateOption.Upsert;
			}
			UpdateMessage<X, U> updateMessage = new UpdateMessage<X, U>(this._connection, this.FullyQualifiedName, updateOption, matchDocument, valueDocument);
			updateMessage.Execute();
		}
		public void Update<X>(X matchDocument, Action<IModifierExpression<T>> action, bool updateMultiple, bool upsert)
		{
			ModifierExpression<T> modifierExpression = new ModifierExpression<T>();
			action(modifierExpression);
			if (matchDocument is ObjectId)
			{
				this.Update(new
				{
					_id = matchDocument
				}, modifierExpression.Expression, updateMultiple, upsert);
				return;
			}
			this.Update<X, Expando>(matchDocument, modifierExpression.Expression, updateMultiple, upsert);
		}
		public bool DeleteIndex(string indexName, out int numberDeleted)
		{
			bool result = false;
			IMongoCollection<DeleteIndicesResponse> collection = this._db.GetCollection<DeleteIndicesResponse>("$cmd");
			DeleteIndicesResponse deleteIndicesResponse = collection.FindOne(new
			{
				deleteIndexes = this._collectionName,
				index = indexName
			});
			numberDeleted = 0;
			if (deleteIndicesResponse != null && deleteIndicesResponse.WasSuccessful)
			{
				result = true;
				numberDeleted = deleteIndicesResponse.NumberIndexesWas.Value;
			}
			return result;
		}
		public T FindOne<U>(U template)
		{
			return this.Find(template, 1).FirstOrDefault<T>();
		}
		public CollectionStatistics GetCollectionStatistics()
		{
			return this._db.GetCollectionStatistics(this._collectionName);
		}
		public void CreateIndex(Expando key, string indexName, bool isUnique)
		{
			IMongoCollection<MongoIndex> collection = this._db.GetCollection<MongoIndex>("system.indexes");
			collection.Insert(new MongoIndex[]
			{
				new MongoIndex
				{
					Key = key,
					Namespace = this.FullyQualifiedName,
					Name = indexName,
					Unique = isUnique
				}
			});
		}
		public IEnumerable<U> Distinct<U>(string keyName)
		{
			return this._db.GetCollection<DistinctValuesResponse<U>>("$cmd").FindOne(new
			{
				distinct = this._collectionName,
				key = keyName
			}).Values;
		}
		public void Delete<U>(U template)
		{
			DeleteMessage<U> deleteMessage = new DeleteMessage<U>(this._connection, this.FullyQualifiedName, template);
			deleteMessage.Execute();
		}
		public void Delete(T document)
		{
			ReflectionHelper helperForType = ReflectionHelper.GetHelperForType(typeof(T));
			MagicProperty magicProperty = helperForType.FindIdProperty();
			if (magicProperty == null)
			{
				throw new MongoException(string.Format("Cannot delete {0} since it has no id property", typeof(T).FullName));
			}
			this.Delete(new
			{
				Id = magicProperty.Getter(document)
			});
		}
		public IEnumerable<T> Find<U, S>(U template, S orderBy, int limit, int skip, string fullyQualifiedName)
		{
			QueryMessage<T, U> message = new QueryMessage<T, U>(this._connection, fullyQualifiedName)
			{
				NumberToTake = limit,
				NumberToSkip = skip,
				Query = template,
				OrderBy = orderBy
			};
			return new MongoQueryExecutor<T, U>(message);
		}
		public T FindAndModify<U, X, Y>(U query, X update, Y sort)
		{
			IMongoCollection<FindAndModifyResult<T>> collection = this._db.GetCollection<FindAndModifyResult<T>>("$cmd");
			T result;
			try
			{
				T value = collection.FindOne(new
				{
					findandmodify = this._collectionName,
					query = query,
					update = update,
					sort = sort
				}).Value;
				result = value;
			}
			catch (MongoException ex)
			{
				if (!(ex.Message == "No matching object found"))
				{
					throw;
				}
				result = default(T);
			}
			return result;
		}
		public IEnumerable<T> Find<U, O, Z>(U template, O orderBy, Z fieldSelector, int limit, int skip)
		{
			QueryMessage<T, U> message = new QueryMessage<T, U>(this._connection, this.FullyQualifiedName)
			{
				NumberToTake = limit,
				NumberToSkip = skip,
				Query = template,
				OrderBy = orderBy,
				FieldSelection = fieldSelector
			};
			return new MongoQueryExecutor<T, U>(message)
			{
				CollectionName = this._collectionName
			};
		}
		public IEnumerable<Z> Find<U, O, Z>(U template, O orderBy, int limit, int skip, string fullName, Expression<Func<T, Z>> fieldSelection)
		{
			NewExpression newExpression = fieldSelection.Body as NewExpression;
			Expando expando = new Expando();
			if (newExpression != null)
			{
				using (IEnumerator<MemberExpression> enumerator = newExpression.Arguments.OfType<MemberExpression>().GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						MemberExpression current = enumerator.Current;
						expando[current.GetPropertyAlias()] = 1;
					}
					goto IL_88;
				}
			}
			if (fieldSelection.Body is MemberExpression)
			{
				MemberExpression mex = fieldSelection.Body as MemberExpression;
				expando[mex.GetPropertyAlias()] = 1;
			}
			IL_88:
			QueryMessage<T, U> message = new QueryMessage<T, U>(this._connection, fullName)
			{
				NumberToTake = limit,
				NumberToSkip = skip,
				Query = template,
				OrderBy = orderBy,
				FieldSelection = expando
			};
			object obj = null;
			if (!MongoCollection<T>._compiledTransforms.TryGetValue(fieldSelection.GetHashCode(), out obj))
			{
				obj = fieldSelection.Compile();
				MongoCollection<T>._compiledTransforms[fieldSelection.GetHashCode()] = obj;
			}
			return new MongoQueryExecutor<T, U, Z>(message, (Func<T, Z>)obj)
			{
				CollectionName = this._collectionName
			};
		}
		private IEnumerable<Z> FindFieldSelection<U, O, Z>(U template, O orderBy, int limit, int skip, string fullName, Expression<Func<T, Z>> fieldSelection)
		{
			return this.Find<U, O, Z>(template, orderBy, limit, skip, fullName, fieldSelection);
		}
		public ExplainResponse Explain<U>(U template)
		{
			return this._db.GetCollection<ExplainResponse>(this._collectionName).FindOne<ExplainRequest<U>>(new ExplainRequest<U>(template));
		}
		public long Count<U>(U query)
		{
			Expando expando = this._db.GetCollection<Expando>("$cmd").FindOne(new
			{
				count = this._collectionName,
				query = query
			});
			return (long)expando.Get<double>("n");
		}
		public void Insert(IEnumerable<T> documentsToInsert)
		{
			this.AssertUpdatable();
			this.TrySettingId(documentsToInsert);
			InsertMessage<T> insertMessage = new InsertMessage<T>(this._connection, this.FullyQualifiedName, documentsToInsert);
			insertMessage.Execute();
			if (this._connection.StrictMode)
			{
				LastErrorResponse lastErrorResponse = this._db.LastError(this._connection.VerifyWriteCount);
				if (lastErrorResponse.Code > 0)
				{
					throw new MongoException(lastErrorResponse.Error);
				}
			}
		}
		public IEnumerable<X> MapReduce<X>(string map, string reduce)
		{
			return this.MapReduce<X>(new MapReduceOptions<T>
			{
				Map = map,
				Reduce = reduce
			});
		}
		public IEnumerable<X> MapReduce<U, X>(U template, string map, string reduce)
		{
			return this.MapReduce<X>(new MapReduceOptions<T>
			{
				Query = template,
				Map = map,
				Reduce = reduce
			});
		}
		public IEnumerable<X> MapReduce<U, X>(U template, string map, string reduce, string finalize)
		{
			return this.MapReduce<X>(new MapReduceOptions<T>
			{
				Query = template,
				Map = map,
				Reduce = reduce,
				Finalize = finalize
			});
		}
		public IEnumerable<X> MapReduce<X>(MapReduceOptions<T> options)
		{
			MapReduce mapReduce = new MapReduce(this._db);
			MapReduceResponse mapReduceResponse = mapReduce.Execute(options);
			IMongoCollection<X> collection = mapReduceResponse.GetCollection<X>();
			return collection.Find<X>().ToList<X>();
		}
		private void AssertUpdatable()
		{
			if (!this.Updateable)
			{
				throw new MongoException("This collection does not accept insertions/updates, this is due to the fact that the collection's type " + typeof(T).FullName + " does not specify an identifier property");
			}
		}
		private void TrySettingId(IEnumerable<T> entities)
		{
			Dictionary<Type, Func<object>> dictionary = new Dictionary<Type, Func<object>>();
			dictionary.Add(typeof(long?), () => this.GenerateId());
			dictionary.Add(typeof(int?), () => Convert.ToInt32(this.GenerateId()));
			dictionary.Add(typeof(ObjectId), () => ObjectId.NewObjectId());
			Dictionary<Type, Func<object>> dictionary2 = dictionary;
			if (typeof(T) != typeof(object) && typeof(T).GetInterface("IUpdateWithoutId") == null)
			{
				MagicProperty magicProperty = ReflectionHelper.GetHelperForType(typeof(T)).FindIdProperty();
				if (magicProperty != null && dictionary2.ContainsKey(magicProperty.Type) && magicProperty.Setter != null)
				{
					foreach (T current in entities)
					{
						if (magicProperty.Getter(current) == null)
						{
							magicProperty.Setter(current, dictionary2[magicProperty.Type]());
						}
					}
				}
			}
		}
		public long GenerateId()
		{
			return MongoCollection<T>._collectionHiLoIdGenerator.GenerateId(this._db, this._collectionName);
		}
	}
	public class MongoCollection : MongoCollection<object>, IMongoCollection, IMongoCollection<object>
	{
		public MongoCollection(string collectionName, IMongoDatabase db, IConnection connection) : base(collectionName, db, connection)
		{
		}
	}
}
