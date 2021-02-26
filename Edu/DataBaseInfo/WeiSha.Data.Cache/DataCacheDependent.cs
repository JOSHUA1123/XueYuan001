using System;
namespace DataBaseInfo.Cache
{
	internal class DataCacheDependent : ICacheDependent
	{
		private ICacheStrategy strategy;
		private string connectName;
		public DataCacheDependent(ICacheStrategy strategy, string connectName)
		{
			this.strategy = strategy;
			this.connectName = connectName;
		}
		public void AddCache<T>(string cacheKey, T cacheValue, int cacheTime)
		{
			if (cacheTime > 0)
			{
				cacheKey = string.Format("{0}_{1}_{2}", this.connectName, typeof(T).FullName, cacheKey);
				this.strategy.AddObject(cacheKey, cacheValue, TimeSpan.FromSeconds((double)cacheTime));
			}
		}
		public void RemoveCache<T>(string cacheKey)
		{
			cacheKey = string.Format("{0}_{1}_{2}", this.connectName, typeof(T).FullName, cacheKey);
			this.strategy.RemoveObject(cacheKey);
		}
		public T GetCache<T>(string cacheKey)
		{
			cacheKey = string.Format("{0}_{1}_{2}", this.connectName, typeof(T).FullName, cacheKey);
			return this.strategy.GetObject<T>(cacheKey);
		}
	}
}
