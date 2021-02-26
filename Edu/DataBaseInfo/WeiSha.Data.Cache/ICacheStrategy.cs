using System;
using System.Collections.Generic;
namespace DataBaseInfo.Cache
{
	public interface ICacheStrategy
	{
		int Timeout
		{
			get;
			set;
		}
		void SetExpired(string objId, DateTime datetime);
		void AddObject(string objId, object o);
		void AddObject(string objId, object o, TimeSpan expires);
		void AddObject(string objId, object o, DateTime datetime);
		void RemoveObject(string objId);
		object GetObject(string objId);
		T GetObject<T>(string objId);
		object GetMatchObject(string regularExpression);
		T GetMatchObject<T>(string regularExpression);
		void RemoveAllObjects();
		IList<string> GetAllKeys();
		int GetCacheCount();
		IDictionary<string, object> GetAllObjects();
		IDictionary<string, T> GetAllObjects<T>();
		IList<string> GetKeys(string regularExpression);
		void AddObjects<T>(IDictionary<string, T> data);
		void AddObjects(IDictionary<string, object> data);
		void RemoveMatchObjects(string regularExpression);
		void RemoveObjects(IList<string> objIds);
		IDictionary<string, object> GetObjects(IList<string> objIds);
		IDictionary<string, T> GetObjects<T>(IList<string> objIds);
		IDictionary<string, object> GetMatchObjects(string regularExpression);
		IDictionary<string, T> GetMatchObjects<T>(string regularExpression);
	}
}
