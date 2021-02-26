using System;
using System.Collections.Generic;
namespace Norm.Collections
{
	public class CollectionHiLoIdGenerator
	{
		private readonly int _capacity;
		private readonly object generatorLock = new object();
		private IDictionary<string, HiLoIdGenerator> keyGeneratorsByTag = new Dictionary<string, HiLoIdGenerator>();
		public CollectionHiLoIdGenerator(int capacity)
		{
			this._capacity = capacity;
		}
		public long GenerateId(IMongoDatabase db, string collectionName)
		{
			HiLoIdGenerator hiLoIdGenerator;
			lock (this.generatorLock)
			{
				if (this.keyGeneratorsByTag.TryGetValue(collectionName, out hiLoIdGenerator))
				{
					return hiLoIdGenerator.GenerateId(collectionName, db);
				}
				hiLoIdGenerator = new HiLoIdGenerator((long)this._capacity);
				this.keyGeneratorsByTag = new Dictionary<string, HiLoIdGenerator>(this.keyGeneratorsByTag)
				{

					{
						collectionName,
						hiLoIdGenerator
					}
				};
			}
			return hiLoIdGenerator.GenerateId(collectionName, db);
		}
	}
}
