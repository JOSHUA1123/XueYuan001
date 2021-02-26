using System;
using System.Collections.Generic;
using System.Linq;

namespace Common
{
	/// <summary>
	/// 缓存的数据集
	/// </summary>
	/// <typeparam name="T"></typeparam>
	// Token: 0x020000B8 RID: 184
	public class CacheDataCollection<T>
	{
		/// <summary>
		/// 构造缓存
		/// </summary>
		/// <param name="cacheType">缓存类型</param>
		// Token: 0x060004E8 RID: 1256 RVA: 0x0000B399 File Offset: 0x00009599
		public CacheDataCollection(string cacheType) : this()
		{
			this._cacheType = cacheType;
		}

		/// <summary>
		/// 构造缓存
		/// </summary>
		// Token: 0x060004E9 RID: 1257 RVA: 0x00024020 File Offset: 0x00022220
		public CacheDataCollection()
		{
			lock (CacheDataCollection<T>._lock)
			{
				this._list = new List<T>();
			}
		}

		/// <summary>
		/// 被缓存的数据集，列表形式存放
		/// </summary>
		// Token: 0x1700017B RID: 379
		// (get) Token: 0x060004EA RID: 1258 RVA: 0x0000B3A8 File Offset: 0x000095A8
		public List<T> List
		{
			get
			{
				return this._list;
			}
		}

		/// <summary>
		/// 被缓存的数据集，数组形式存放
		/// </summary>
		// Token: 0x1700017C RID: 380
		// (get) Token: 0x060004EB RID: 1259 RVA: 0x0000B3B0 File Offset: 0x000095B0
		public T[] Array
		{
			get
			{
				return this._list.ToArray<T>();
			}
		}

		/// <summary>
		/// 添加缓存对象
		/// </summary>
		/// <param name="t"></param>
		// Token: 0x060004EC RID: 1260 RVA: 0x00024078 File Offset: 0x00022278
		public void Add(T t)
		{
			if (t == null)
			{
				return;
			}
			lock (CacheDataCollection<T>._lock)
			{
				this._list.Add(t);
			}
		}

		/// <summary>
		/// 按索引删除缓存对象
		/// </summary>
		/// <param name="index"></param>
		// Token: 0x060004ED RID: 1261 RVA: 0x000240C8 File Offset: 0x000222C8
		public void Delete(int index)
		{
			if (index < 0)
			{
				return;
			}
			lock (CacheDataCollection<T>._lock)
			{
				this._list.RemoveAt(index);
			}
		}

		/// <summary>
		/// 删除索引对象
		/// </summary>
		/// <param name="t"></param>
		// Token: 0x060004EE RID: 1262 RVA: 0x00024114 File Offset: 0x00022314
		public void Delete(T t)
		{
			if (t == null)
			{
				return;
			}
			lock (CacheDataCollection<T>._lock)
			{
				this._list.Remove(t);
			}
		}

		/// <summary>
		/// 更新
		/// </summary>
		/// <param name="old"></param>
		/// <param name="_new"></param>
		// Token: 0x060004EF RID: 1263 RVA: 0x00024164 File Offset: 0x00022364
		public void Update(T old, T _new)
		{
			if (_new == null)
			{
				return;
			}
			lock (CacheDataCollection<T>._lock)
			{
				for (int i = 0; i < this._list.Count; i++)
				{
					T t = this._list[i];
					if (t.Equals(old))
					{
						this._list[i] = _new;
						break;
					}
				}
			}
		}

		/// <summary>
		/// 清理所有缓存对象
		/// </summary>
		// Token: 0x060004F0 RID: 1264 RVA: 0x000241F0 File Offset: 0x000223F0
		public void Clear()
		{
			lock (CacheDataCollection<T>._lock)
			{
				this._list.RemoveRange(0, this._list.Count);
				this._list.Clear();
			}
		}

		/// <summary>
		/// 填充缓存数据
		/// </summary>
		/// <param name="list"></param>
		// Token: 0x060004F1 RID: 1265 RVA: 0x0002424C File Offset: 0x0002244C
		public void Fill(T[] list)
		{
			if (list == null)
			{
				return;
			}
			lock (CacheDataCollection<T>._lock)
			{
				foreach (T item in list)
				{
					this._list.Add(item);
				}
			}
		}

		/// <summary>
		/// 刷新数据
		/// </summary>
		/// <param name="list"></param>
		// Token: 0x060004F2 RID: 1266 RVA: 0x000242B4 File Offset: 0x000224B4
		public void Fresh(T[] list)
		{
			this.Clear();
			if (list == null)
			{
				return;
			}
			lock (CacheDataCollection<T>._lock)
			{
				foreach (T item in list)
				{
					this._list.Add(item);
				}
			}
		}

		// Token: 0x040001FD RID: 509
		private string _cacheType = "";

		// Token: 0x040001FE RID: 510
		private static object _lock = new object();

		// Token: 0x040001FF RID: 511
		private List<T> _list;
	}
}
