using Norm.Protocol.Messages;
using Norm.Responses;
using System;
using System.Collections.Generic;
namespace Norm
{
	public class MapReduce
	{
		private readonly IMongoDatabase _database;
		private readonly IList<string> _temporaryCollections;
		internal MapReduce(IMongoDatabase database)
		{
			this._database = database;
			this._temporaryCollections = new List<string>(5);
		}
		public MapReduceResponse Execute(MapReduceOptions options)
		{
			MapReduceResponse mapReduceResponse = this._database.GetCollection<MapReduceResponse>("$cmd").FindOne<MapReduceMessage>(new MapReduceMessage
			{
				Map = options.Map,
				Reduce = options.Reduce,
				Query = options.Query,
				MapReduce = options.CollectionName,
				KeepTemp = options.Permanant,
				Out = options.OutputCollectionName,
				Limit = options.Limit,
				Finalize = options.Finalize
			});
			if (!options.Permanant && !string.IsNullOrEmpty(mapReduceResponse.Result))
			{
				this._temporaryCollections.Add(mapReduceResponse.Result);
			}
			mapReduceResponse.PrepareForQuerying(this._database);
			return mapReduceResponse;
		}
	}
}
