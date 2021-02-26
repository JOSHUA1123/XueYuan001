using System;
using System.Collections;
using System.Collections.Generic;

namespace VTemplate.Engine
{
	/// <summary>
	/// 属性集合
	/// </summary>
	// Token: 0x0200001D RID: 29
	public class AttributeCollection : IEnumerable<Attribute>, IEnumerable
	{
		/// <summary>
		/// 构造默认模板属性
		/// </summary>
		/// <param name="ownerElement"></param>
		// Token: 0x06000149 RID: 329 RVA: 0x00007148 File Offset: 0x00005348
		internal AttributeCollection(Element ownerElement)
		{
			this.OwnerElement = ownerElement;
			this._Dictionary = new Dictionary<string, Attribute>(StringComparer.OrdinalIgnoreCase);
		}

		/// <summary>
		/// 构造一定容量的默认模板属性
		/// </summary>
		/// <param name="ownerElement"></param>
		/// <param name="capacity"></param>
		// Token: 0x0600014A RID: 330 RVA: 0x00007167 File Offset: 0x00005367
		internal AttributeCollection(Element ownerElement, int capacity)
		{
			this.OwnerElement = ownerElement;
			this._Dictionary = new Dictionary<string, Attribute>(capacity, StringComparer.OrdinalIgnoreCase);
		}

		/// <summary>
		/// 宿主标签
		/// </summary>
		// Token: 0x1700006D RID: 109
		// (get) Token: 0x0600014B RID: 331 RVA: 0x00007187 File Offset: 0x00005387
		// (set) Token: 0x0600014C RID: 332 RVA: 0x0000718F File Offset: 0x0000538F
		internal Element OwnerElement { get; private set; }

		/// <summary>
		/// 返回某个索引位置的属性元素
		/// </summary>
		/// <param name="index">索引值</param>
		/// <returns>如果存在此索引位置属性元素,则返回其值或者返回null</returns>
		// Token: 0x1700006E RID: 110
		public Attribute this[int index]
		{
			get
			{
				if (index >= 0 && index < this._Dictionary.Count)
				{
					IEnumerator<Attribute> enumerator = this._Dictionary.Values.GetEnumerator();
					int num = 0;
					while (enumerator.MoveNext())
					{
						if (num == index)
						{
							return enumerator.Current;
						}
						num++;
					}
				}
				return null;
			}
		}

		/// <summary>
		/// 返回某个名称的属性元素
		/// </summary>
		/// <param name="name">属性名称</param>
		/// <returns>如果存在此属性,则返回其元素或者返回null</returns>
		// Token: 0x1700006F RID: 111
		public Attribute this[string name]
		{
			get
			{
				if (string.IsNullOrEmpty(name))
				{
					return null;
				}
				Attribute result;
				if (!this._Dictionary.TryGetValue(name, out result))
				{
					result = null;
				}
				return result;
			}
		}

		/// <summary>
		/// 返回某个名称的属性值
		/// </summary>
		/// <param name="name">属性的名称</param>
		/// <returns>如果存在此属性则返回其值,否则返回字符串空值</returns>
		// Token: 0x0600014F RID: 335 RVA: 0x00007218 File Offset: 0x00005418
		public string GetValue(string name)
		{
			return this.GetValue(name, string.Empty);
		}

		/// <summary>
		/// 返回某个名称的属性值
		/// </summary>
		/// <param name="name">属性的名称</param>
		/// <param name="defaultValue">如果不存在则属性则返回此默认值</param>
		/// <returns>如果存在此属性则返回其值,否则返回默认值</returns>
		// Token: 0x06000150 RID: 336 RVA: 0x00007228 File Offset: 0x00005428
		public string GetValue(string name, string defaultValue)
		{
			Attribute attribute = this[name];
			if (attribute == null || attribute.Value == null)
			{
				return defaultValue;
			}
			object value = attribute.Value.GetValue();
			if (value == null)
			{
				return string.Empty;
			}
			return value.ToString();
		}

		/// <summary>
		/// 添加某个属性值
		/// </summary>
		/// <param name="item">属性元素</param>
		// Token: 0x06000151 RID: 337 RVA: 0x00007268 File Offset: 0x00005468
		internal void Add(Attribute item)
		{
			if (item == null)
			{
				return;
			}
			item.OwnerElement = this.OwnerElement;
			if (this.Contains(item.Name))
			{
				this._Dictionary[item.Name] = item;
				return;
			}
			this._Dictionary.Add(item.Name, item);
		}

		/// <summary>
		/// 添加某个属性
		/// </summary>
		/// <param name="name"></param>
		/// <param name="text"></param>
		// Token: 0x06000152 RID: 338 RVA: 0x000072B8 File Offset: 0x000054B8
		public void Add(string name, string text)
		{
			Attribute item = new Attribute(this.OwnerElement, name, text);
			this.Add(item);
			this.OnAdding(item);
		}

		/// <summary>
		/// 清空所有属性值
		/// </summary>
		// Token: 0x06000153 RID: 339 RVA: 0x000072E1 File Offset: 0x000054E1
		internal void Clear()
		{
			this._Dictionary.Clear();
		}

		/// <summary>
		/// 判断是否存在某个属性
		/// </summary>
		/// <param name="name">要判断的属性名称</param>
		/// <returns>存在则返回true否则返回false</returns>
		// Token: 0x06000154 RID: 340 RVA: 0x000072EE File Offset: 0x000054EE
		public bool Contains(string name)
		{
			return this._Dictionary.ContainsKey(name);
		}

		/// <summary>
		/// 返回属性数目
		/// </summary>
		// Token: 0x17000070 RID: 112
		// (get) Token: 0x06000155 RID: 341 RVA: 0x000072FC File Offset: 0x000054FC
		public int Count
		{
			get
			{
				return this._Dictionary.Count;
			}
		}

		/// <summary>
		/// 添加新属性时的触发事件
		/// </summary>
		// Token: 0x14000003 RID: 3
		// (add) Token: 0x06000156 RID: 342 RVA: 0x0000730C File Offset: 0x0000550C
		// (remove) Token: 0x06000157 RID: 343 RVA: 0x00007344 File Offset: 0x00005544
		internal event EventHandler<AttributeCollection.AttributeAddingEventArgs> Adding;

		/// <summary>
		/// 添加新属性时触发事件
		/// </summary>
		/// <param name="item"></param>
		// Token: 0x06000158 RID: 344 RVA: 0x0000737C File Offset: 0x0000557C
		protected void OnAdding(Attribute item)
		{
			EventHandler<AttributeCollection.AttributeAddingEventArgs> adding = this.Adding;
			if (adding != null)
			{
				adding(this, new AttributeCollection.AttributeAddingEventArgs(item));
			}
		}

		/// <summary>
		/// 返回当前对象的迭代器
		/// </summary>
		/// <returns></returns>
		// Token: 0x06000159 RID: 345 RVA: 0x000073A0 File Offset: 0x000055A0
		public IEnumerator<Attribute> GetEnumerator()
		{
			return this._Dictionary.Values.GetEnumerator();
		}

		/// <summary>
		/// 返回当前对象的迭代器
		/// </summary>
		/// <returns></returns>
		// Token: 0x0600015A RID: 346 RVA: 0x000073B7 File Offset: 0x000055B7
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this._Dictionary.Values.GetEnumerator();
		}

		/// <summary>
		/// 存放容器
		/// </summary>
		// Token: 0x04000041 RID: 65
		private Dictionary<string, Attribute> _Dictionary;

		/// <summary>
		/// 添加新属性时的触发事件参数
		/// </summary>
		// Token: 0x0200001E RID: 30
		internal class AttributeAddingEventArgs : EventArgs
		{
			/// <summary>
			///
			/// </summary>
			/// <param name="item"></param>
			// Token: 0x0600015B RID: 347 RVA: 0x000073CE File Offset: 0x000055CE
			public AttributeAddingEventArgs(Attribute item)
			{
				this.Item = item;
			}

			/// <summary>
			/// 添加添加的项目
			/// </summary>
			// Token: 0x17000071 RID: 113
			// (get) Token: 0x0600015C RID: 348 RVA: 0x000073DD File Offset: 0x000055DD
			// (set) Token: 0x0600015D RID: 349 RVA: 0x000073E5 File Offset: 0x000055E5
			public Attribute Item { get; private set; }
		}
	}
}
