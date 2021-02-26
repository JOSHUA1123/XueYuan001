using System;
using System.Collections.Generic;

namespace Common
{
	/// <summary>
	/// 缓存数据，以减少对数据库的读取次数
	/// </summary>
	// Token: 0x020000B7 RID: 183
	public class Cache<T>
	{
		/// <summary>
		/// 缓存名称
		/// </summary>
		// Token: 0x17000179 RID: 377
		// (get) Token: 0x060004E3 RID: 1251 RVA: 0x0000B35C File Offset: 0x0000955C
		// (set) Token: 0x060004E4 RID: 1252 RVA: 0x0000B364 File Offset: 0x00009564
		public string Name { get; set; }

		/// <summary>
		/// 缓存的数据集
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		// Token: 0x1700017A RID: 378
		// (get) Token: 0x060004E5 RID: 1253 RVA: 0x00023F38 File Offset: 0x00022138
		public static CacheDataCollection<T> Data
		{
			get
			{
				CacheDataCollection<T> data;
				lock (Cache<T>._lock)
				{
					Cache<T> cache = null;
					Type typeFromHandle = typeof(T);
					if (Cache<T>.list != null)
					{
						foreach (Cache<T> cache2 in Cache<T>.list)
						{
							if (cache2.Name == typeFromHandle.FullName)
							{
								cache = cache2;
							}
						}
					}
					if (cache == null)
					{
						cache = new Cache<T>(typeFromHandle.FullName);
						cache._data = new CacheDataCollection<T>(typeFromHandle.FullName);
						if (Cache<T>.list == null)
						{
							Cache<T>.list = new List<Cache<T>>();
						}
						Cache<T>.list.Add(cache);
					}
					data = cache._data;
				}
				return data;
			}
		}

		/// <summary>
		/// 构造缓存
		/// </summary>
		/// <param name="cacheName">缓存名称</param>
		// Token: 0x060004E6 RID: 1254 RVA: 0x0000B36D File Offset: 0x0000956D
		private Cache(string cacheName)
		{
			this.Name = cacheName;
		}

		/// <summary>
		/// 缓存集
		/// </summary>
		// Token: 0x040001F9 RID: 505
		private static List<Cache<T>> list = null;

		// Token: 0x040001FA RID: 506
		private static object _lock = new object();

		// Token: 0x040001FB RID: 507
		protected CacheDataCollection<T> _data = new CacheDataCollection<T>();
	}
}
