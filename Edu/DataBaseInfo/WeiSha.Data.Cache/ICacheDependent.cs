using System;
namespace DataBaseInfo.Cache
{
	public interface ICacheDependent
	{
		void AddCache<T>(string cacheKey, T cacheValue, int cacheTime);
		void RemoveCache<T>(string cacheKey);
		T GetCache<T>(string cacheKey);
	}
}
