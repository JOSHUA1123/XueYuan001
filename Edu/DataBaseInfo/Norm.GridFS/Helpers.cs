using Norm.Collections;
using System;
namespace Norm.GridFS
{
	public static class Helpers
	{
		public static GridFileCollection Files<T>(this IMongoCollection<T> rootCollection)
		{
			return new GridFileCollection(rootCollection.GetChildCollection<GridFile>("files"), rootCollection.GetChildCollection<FileChunk>("chunks"));
		}
		public static GridFileCollection Files(this IMongoDatabase database)
		{
			return new GridFileCollection(database.GetCollection<GridFile>("files"), database.GetCollection<FileChunk>("chunks"));
		}
	}
}
