using System;
namespace DataBaseInfo.Cache
{
	public interface ILocalCacheStrategy : ICacheStrategy
	{
		void AddObjectWithFileChange(string objId, object o, string[] files);
		void AddObjectWithDepend(string objId, object o, string[] dependKey);
		void AddObjectWithFileChange(string objId, object o, TimeSpan expires, string[] files);
		void AddObjectWithDepend(string objId, object o, TimeSpan expires, string[] dependKey);
	}
}
