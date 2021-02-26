using System;
namespace DataBaseInfo.Cache
{
	public interface IDistributedCacheStrategy : ICacheStrategy
	{
		void SetLocalCacheTimeout(int timeout);
	}
}
