using Norm.BSON;
using System;
using System.Threading;
namespace Norm.Collections
{
	public class HiLoIdGenerator
	{
		private class NormHiLoKey
		{
			[MongoIdentifier]
			public string CollectionName
			{
				get;
				set;
			}
			public long ServerHi
			{
				get;
				set;
			}
		}
		private readonly long _capacity;
		private ReaderWriterLockSlim _lockSlim = new ReaderWriterLockSlim();
		private long _currentHi;
		private long _currentLo;
		public HiLoIdGenerator(long capacity)
		{
			this._currentHi = 0L;
			this._capacity = capacity;
			this._currentLo = capacity + 1L;
		}
		public long GenerateId(string collectionName, IMongoDatabase database)
		{
			this._lockSlim.EnterUpgradeableReadLock();
			long num = Interlocked.Increment(ref this._currentLo);
			if (num > this._capacity)
			{
				this._lockSlim.EnterWriteLock();
				if (Thread.VolatileRead(ref this._currentLo) > this._capacity)
				{
					this._currentHi = this.GetNextHi(collectionName, database);
					this._currentLo = 1L;
					num = 1L;
				}
				this._lockSlim.ExitWriteLock();
			}
			this._lockSlim.ExitUpgradeableReadLock();
			return (this._currentHi - 1L) * this._capacity + num;
		}
		private long GetNextHi(string collectionName, IMongoDatabase database)
		{
			long result;
        IL_00:
			try
			{
				
				Expando expando = new Expando();
				expando["$inc"] = new
				{
					ServerHi = 1
				};
				HiLoIdGenerator.NormHiLoKey normHiLoKey = database.GetCollection<HiLoIdGenerator.NormHiLoKey>().FindAndModify(new
				{
					_id = collectionName
				}, expando);
				if (normHiLoKey == null)
				{
					database.GetCollection<HiLoIdGenerator.NormHiLoKey>().Insert(new HiLoIdGenerator.NormHiLoKey[]
					{
						new HiLoIdGenerator.NormHiLoKey
						{
							CollectionName = collectionName,
							ServerHi = 2L
						}
					});
					result = 1L;
				}
				else
				{
					long serverHi = normHiLoKey.ServerHi;
					result = serverHi;
				}
			}
			catch (MongoException ex)
			{
				if (!ex.Message.Contains("duplicate key"))
				{
					throw;
				}
				goto IL_00;
			}
			return result;
		}
	}
}
