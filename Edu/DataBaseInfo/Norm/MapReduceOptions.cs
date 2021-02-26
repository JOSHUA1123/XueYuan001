using Norm.Configuration;
using System;
namespace Norm
{
	public class MapReduceOptions
	{
		public string Map
		{
			get;
			set;
		}
		public string Reduce
		{
			get;
			set;
		}
		public object Query
		{
			get;
			set;
		}
		public string CollectionName
		{
			get;
			set;
		}
		public bool Permanant
		{
			get;
			set;
		}
		public string OutputCollectionName
		{
			get;
			set;
		}
		public int? Limit
		{
			get;
			set;
		}
		public new string Finalize
		{
			get;
			set;
		}
		public MapReduceOptions()
		{
		}
		public MapReduceOptions(string collectionName)
		{
			this.CollectionName = collectionName;
		}
	}
	public class MapReduceOptions<T> : MapReduceOptions
	{
		public MapReduceOptions()
		{
			base.CollectionName = MongoConfiguration.GetCollectionName(typeof(T));
		}
	}
}
