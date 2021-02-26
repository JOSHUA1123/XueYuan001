using Norm.BSON;
using Norm.Collections;
using Norm.Responses;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
namespace Norm.Linq
{
	internal class MongoQueryExecutor
	{
		private readonly IMongoDatabase _db;
		private readonly QueryTranslationResults _translationResults;
		public MongoQueryExecutor(IMongoDatabase db, QueryTranslationResults translationResults)
		{
			this._db = db;
			this._translationResults = translationResults;
		}
		public object Execute<T>()
		{
			IMongoCollection<T> mongoCollection = new MongoCollection<T>(this._translationResults.CollectionName, this._db, this._db.CurrentConnection);
			string methodCall;
			object obj;
			switch (methodCall = this._translationResults.MethodCall)
			{
			case "Any":
				obj = (mongoCollection.Count<Expando>(this._translationResults.Where) > 0L);
				return obj;
			case "Count":
				obj = mongoCollection.Count<Expando>(this._translationResults.Where);
				return obj;
			case "Sum":
				obj = this.ExecuteMapReduce<double>(this._translationResults.TypeName, this.BuildSumMapReduce());
				return obj;
			case "Average":
				obj = this.ExecuteMapReduce<double>(this._translationResults.TypeName, this.BuildAverageMapReduce());
				return obj;
			case "Min":
				obj = this.ExecuteMapReduce<double>(this._translationResults.TypeName, this.BuildMinMapReduce());
				return obj;
			case "Max":
				obj = this.ExecuteMapReduce<double>(this._translationResults.TypeName, this.BuildMaxMapReduce());
				return obj;
			}
			this._translationResults.Take = (this.IsSingleResultMethod(this._translationResults.MethodCall) ? 1 : this._translationResults.Take);
			this._translationResults.Sort.ReverseKitchen();
			if (this._translationResults.Select == null)
			{
				obj = mongoCollection.Find<Expando, Expando>(this._translationResults.Where, this._translationResults.Sort, this._translationResults.Take, this._translationResults.Skip, mongoCollection.FullyQualifiedName);
				string methodCall2;
				if ((methodCall2 = this._translationResults.MethodCall) != null)
				{
					if (!(methodCall2 == "SingleOrDefault"))
					{
						if (!(methodCall2 == "Single"))
						{
							if (!(methodCall2 == "FirstOrDefault"))
							{
								if (methodCall2 == "First")
								{
									obj = ((IEnumerable<T>)obj).First<T>();
								}
							}
							else
							{
								obj = ((IEnumerable<T>)obj).FirstOrDefault<T>();
							}
						}
						else
						{
							obj = ((IEnumerable<T>)obj).Single<T>();
						}
					}
					else
					{
						obj = ((IEnumerable<T>)obj).SingleOrDefault<T>();
					}
				}
			}
			else
			{
				Type type = mongoCollection.GetType();
				MethodInfo method = type.GetMethod("FindFieldSelection", BindingFlags.Instance | BindingFlags.NonPublic);
				object obj2 = this._translationResults.Sort ?? new object();
				Type[] typeArguments = new Type[]
				{
					typeof(Expando),
					obj2.GetType(),
					this._translationResults.Select.Body.Type
				};
				MethodInfo methodInfo = method.MakeGenericMethod(typeArguments);
				obj = methodInfo.Invoke(mongoCollection, new object[]
				{
					this._translationResults.Where,
					this._translationResults.Sort,
					this._translationResults.Take,
					this._translationResults.Skip,
					mongoCollection.FullyQualifiedName,
					this._translationResults.Select
				});
				string methodCall3;
				if ((methodCall3 = this._translationResults.MethodCall) != null)
				{
					if (!(methodCall3 == "SingleOrDefault"))
					{
						if (!(methodCall3 == "Single"))
						{
							if (!(methodCall3 == "FirstOrDefault"))
							{
								if (methodCall3 == "First")
								{
									obj = ((IEnumerable)obj).OfType<object>().First<object>();
								}
							}
							else
							{
								obj = ((IEnumerable)obj).OfType<object>().FirstOrDefault<object>();
							}
						}
						else
						{
							obj = ((IEnumerable)obj).OfType<object>().Single<object>();
						}
					}
					else
					{
						obj = ((IEnumerable)obj).OfType<object>().SingleOrDefault<object>();
					}
				}
			}
			return obj;
		}
		private MapReduceParameters InitializeDefaultMapReduceParameters()
		{
			string map = "";
			string finalize = "";
			if (!string.IsNullOrEmpty(this._translationResults.AggregatePropName))
			{
				map = "function(){emit(0, {val:this." + this._translationResults.AggregatePropName + ",tSize:1} )};";
				if (!string.IsNullOrEmpty(this._translationResults.Query))
				{
					map = string.Concat(new string[]
					{
						"function(){if (",
						this._translationResults.Query,
						") {emit(0, {val:this.",
						this._translationResults.AggregatePropName,
						",tSize:1} )};}"
					});
				}
				finalize = "function(key, res){ return res.val; }";
			}
			return new MapReduceParameters
			{
				Map = map,
				Reduce = string.Empty,
				Finalize = finalize
			};
		}
		private MapReduceParameters BuildSumMapReduce()
		{
			MapReduceParameters mapReduceParameters = this.InitializeDefaultMapReduceParameters();
			mapReduceParameters.Reduce = "function(key, values){var sum = 0; for(var i = 0; i < values.length; i++){ sum+=values[i].val;} return {val:sum};}";
			return mapReduceParameters;
		}
		private MapReduceParameters BuildAverageMapReduce()
		{
			MapReduceParameters mapReduceParameters = this.InitializeDefaultMapReduceParameters();
			mapReduceParameters.Reduce = "function(key, values){var sum = 0, tot = 0; for(var i = 0; i < values.length; i++){sum += values[i].val; tot += values[i].tSize; } return {val:sum,tSize:tot};}";
			mapReduceParameters.Finalize = "function(key, res){ return res.val / res.tSize; }";
			return mapReduceParameters;
		}
		private MapReduceParameters BuildMinMapReduce()
		{
			MapReduceParameters mapReduceParameters = this.InitializeDefaultMapReduceParameters();
			mapReduceParameters.Reduce = "function(key, values){var least = 0; for(var i = 0; i < values.length; i++){if(i==0 || least > values[i].val){least=values[i].val;}} return {val:least};}";
			return mapReduceParameters;
		}
		private MapReduceParameters BuildMaxMapReduce()
		{
			MapReduceParameters mapReduceParameters = this.InitializeDefaultMapReduceParameters();
			mapReduceParameters.Reduce = "function(key, values){var least = 0; for(var i = 0; i < values.length; i++){if(i==0 || least < values[i].val){least=values[i].val;}} return {val:least};}";
			return mapReduceParameters;
		}
		private T ExecuteMapReduce<T>(string typeName, MapReduceParameters parameters)
		{
			MapReduce mapReduce = this._db.CreateMapReduce();
			MapReduceResponse mapReduceResponse = mapReduce.Execute(new MapReduceOptions(typeName)
			{
				Map = parameters.Map,
				Reduce = parameters.Reduce,
				Finalize = parameters.Finalize
			});
			IMongoCollection<MapReduceResult<T>> collection = mapReduceResponse.GetCollection<MapReduceResult<T>>();
			MapReduceResult<T> mapReduceResult = collection.Find<MapReduceResult<T>>().FirstOrDefault<MapReduceResult<T>>();
			return (mapReduceResult != null) ? mapReduceResult.Value : default(T);
		}
		private bool IsAggregateMethod(string method)
		{
			return new string[]
			{
				"Min",
				"Max",
				"Average",
				"Sum"
			}.Contains(method);
		}
		private bool IsSingleResultMethod(string method)
		{
			return new string[]
			{
				"Single",
				"SingleOrDefault",
				"First",
				"FirstOrDefault"
			}.Contains(method);
		}
	}
}
