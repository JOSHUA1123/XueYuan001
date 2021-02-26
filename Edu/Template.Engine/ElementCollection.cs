using System;
using System.Collections;
using System.Collections.Generic;

namespace VTemplate.Engine
{
	/// <summary>
	/// 元素集合
	/// </summary>
	/// <typeparam name="T"></typeparam>
	// Token: 0x02000019 RID: 25
	public class ElementCollection<T> : IEnumerable<T>, IEnumerable
	{
		/// <summary>
		/// 构造默认的集合
		/// </summary>
		// Token: 0x0600011C RID: 284 RVA: 0x0000632C File Offset: 0x0000452C
		internal ElementCollection()
		{
			this._List = new List<T>();
		}

		/// <summary>
		/// 构造初始含有一定数量的集合
		/// </summary>
		/// <param name="capacity">初始容量</param>
		// Token: 0x0600011D RID: 285 RVA: 0x0000633F File Offset: 0x0000453F
		internal ElementCollection(int capacity)
		{
			this._List = new List<T>(capacity);
		}

		/// <summary>
		/// 构造集合
		/// </summary>
		/// <param name="collection">要复制的集合列表</param>
		// Token: 0x0600011E RID: 286 RVA: 0x00006353 File Offset: 0x00004553
		internal ElementCollection(IEnumerable<T> collection)
		{
			this._List = new List<T>(collection);
		}

		/// <summary>
		/// 返回某个索引位置的数据
		/// </summary>
		/// <param name="index">索引位置</param>
		/// <returns></returns>
		// Token: 0x1700005D RID: 93
		public T this[int index]
		{
			get
			{
				return this._List[index];
			}
		}

		/// <summary>
		/// 返回当前对象的迭代器
		/// </summary>
		/// <returns></returns>
		// Token: 0x06000120 RID: 288 RVA: 0x00006375 File Offset: 0x00004575
		public IEnumerator<T> GetEnumerator()
		{
			return this._List.GetEnumerator();
		}

		/// <summary>
		/// 返回当前对象的迭代器
		/// </summary>
		/// <returns></returns>
		// Token: 0x06000121 RID: 289 RVA: 0x00006387 File Offset: 0x00004587
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this._List.GetEnumerator();
		}

		/// <summary>
		/// 添加元素
		/// </summary>
		/// <param name="element">模板元素</param>
		// Token: 0x06000122 RID: 290 RVA: 0x00006399 File Offset: 0x00004599
		internal virtual void Add(T element)
		{
			this._List.Add(element);
		}

		/// <summary>
		/// 批量添加元素
		/// </summary>
		/// <param name="collection">集合数据,不可以为null</param>
		// Token: 0x06000123 RID: 291 RVA: 0x000063A7 File Offset: 0x000045A7
		internal virtual void AddRange(IEnumerable<T> collection)
		{
			this._List.AddRange(collection);
		}

		/// <summary>
		/// 返回当前拥有的数目
		/// </summary>
		// Token: 0x1700005E RID: 94
		// (get) Token: 0x06000124 RID: 292 RVA: 0x000063B5 File Offset: 0x000045B5
		public int Count
		{
			get
			{
				return this._List.Count;
			}
		}

		/// <summary>
		/// 清空数据
		/// </summary>
		// Token: 0x06000125 RID: 293 RVA: 0x000063C2 File Offset: 0x000045C2
		internal void Clear()
		{
			this._List.Clear();
		}

		/// <summary>
		/// 存储空间
		/// </summary>
		// Token: 0x0400003C RID: 60
		private List<T> _List;
	}
}
