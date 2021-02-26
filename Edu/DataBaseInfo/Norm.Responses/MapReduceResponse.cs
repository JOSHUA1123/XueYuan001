using Norm.Collections;
using System;
namespace Norm.Responses
{
	public class MapReduceResponse : BaseStatusMessage
	{
		public class MapReduceCount
		{
			public int Input
			{
				get;
				set;
			}
			public int Emit
			{
				get;
				set;
			}
			public int Output
			{
				get;
				set;
			}
		}
		private IMongoDatabase _database;
		public string Result
		{
			get;
			set;
		}
		public MapReduceResponse.MapReduceCount Counts
		{
			get;
			set;
		}
		public long TimeMillis
		{
			get;
			set;
		}
		internal void PrepareForQuerying(IMongoDatabase database)
		{
			this._database = database;
		}
		public IMongoCollection GetCollection(string collectionName)
		{
			return this._database.GetCollection(collectionName);
		}
		public IMongoCollection<T> GetCollection<T>()
		{
			return this._database.GetCollection<T>(this.Result);
		}
	}
}
