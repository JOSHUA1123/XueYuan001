using System;
using System.Collections;
using System.Collections.Generic;

namespace VTemplate.Engine
{
	/// <summary>
	/// 变量集合
	/// </summary>
	// Token: 0x02000014 RID: 20
	public class VariableCollection : IEnumerable<Variable>, IEnumerable
	{
		/// <summary>
		/// 构造默认集合
		/// </summary>
		// Token: 0x060000DC RID: 220 RVA: 0x00004C98 File Offset: 0x00002E98
		internal VariableCollection()
		{
			this._Dictionary = new Dictionary<string, Variable>(StringComparer.InvariantCultureIgnoreCase);
		}

		/// <summary>
		/// 构造一定容量的集合
		/// </summary>
		/// <param name="capacity"></param>
		// Token: 0x060000DD RID: 221 RVA: 0x00004CB0 File Offset: 0x00002EB0
		public VariableCollection(int capacity)
		{
			this._Dictionary = new Dictionary<string, Variable>(capacity, StringComparer.InvariantCultureIgnoreCase);
		}

		/// <summary>
		/// 返回某个索引位置的变量
		/// </summary>
		/// <param name="index">索引值</param>
		/// <returns>如果存在此索引位置值,则返回其变量值否则返回null</returns>
		// Token: 0x17000055 RID: 85
		public Variable this[int index]
		{
			get
			{
				if (index >= 0 && index < this._Dictionary.Count)
				{
					IEnumerator<Variable> enumerator = this._Dictionary.Values.GetEnumerator();
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
		/// 返回某个名称的量
		/// </summary>
		/// <param name="name">量名称</param>
		/// <returns>如果存在此量,则返回其量否则返回null</returns>
		// Token: 0x17000056 RID: 86
		public Variable this[string name]
		{
			get
			{
				if (string.IsNullOrEmpty(name))
				{
					return null;
				}
				Variable result;
				if (!this._Dictionary.TryGetValue(name, out result))
				{
					result = null;
				}
				return result;
			}
		}

		/// <summary>
		/// 添加某个变量
		/// </summary>
		/// <param name="item">量元素</param>
		// Token: 0x060000E0 RID: 224 RVA: 0x00004D4C File Offset: 0x00002F4C
		internal void Add(Variable item)
		{
			if (item == null)
			{
				return;
			}
			if (this.Contains(item.Name))
			{
				this._Dictionary[item.Name] = item;
				return;
			}
			this._Dictionary.Add(item.Name, item);
		}

		/// <summary>
		/// 清空所有属性值
		/// </summary>
		// Token: 0x060000E1 RID: 225 RVA: 0x00004D85 File Offset: 0x00002F85
		internal void Clear()
		{
			this._Dictionary.Clear();
		}

		/// <summary>
		/// 判断是否存在某个属性
		/// </summary>
		/// <param name="name">要判断的变量名称</param>
		/// <returns>存在则返回true否则返回false</returns>
		// Token: 0x060000E2 RID: 226 RVA: 0x00004D92 File Offset: 0x00002F92
		public bool Contains(string name)
		{
			return this._Dictionary.ContainsKey(name);
		}

		/// <summary>
		/// 返回属性数目
		/// </summary>
		// Token: 0x17000057 RID: 87
		// (get) Token: 0x060000E3 RID: 227 RVA: 0x00004DA0 File Offset: 0x00002FA0
		public int Count
		{
			get
			{
				return this._Dictionary.Count;
			}
		}

		/// <summary>
		/// 设置某个变量或变量表达式的值
		/// </summary>
		/// <param name="varExp">变量名(如:"user")或变量表达式(如"user.age"则表示设置user变量的age属性/字段值)</param>
		/// <param name="value">变量值</param>
		/// <remarks>
		/// 不管变量表达式中的定义的"属性"或"字段"是否存在于变量实例.都可以设置值.
		/// </remarks>
		// Token: 0x060000E4 RID: 228 RVA: 0x00004DB0 File Offset: 0x00002FB0
		public void SetValue(string varExp, object value)
		{
			if (string.IsNullOrEmpty(varExp))
			{
				throw new ArgumentNullException("varExp");
			}
			string[] array = varExp.Split(".".ToCharArray(), 2);
			Variable variable = this[array[0]];
			if (variable != null)
			{
				if (array.Length == 1)
				{
					variable.Value = value;
					return;
				}
				if (array.Length == 2)
				{
					variable.SetExpValue(array[1], value);
				}
			}
		}

		/// <summary>
		/// 返回当前对象的迭代器
		/// </summary>
		/// <returns></returns>
		// Token: 0x060000E5 RID: 229 RVA: 0x00004E0D File Offset: 0x0000300D
		public IEnumerator<Variable> GetEnumerator()
		{
			return this._Dictionary.Values.GetEnumerator();
		}

		/// <summary>
		/// 返回当前对象的迭代器
		/// </summary>
		/// <returns></returns>
		// Token: 0x060000E6 RID: 230 RVA: 0x00004E24 File Offset: 0x00003024
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this._Dictionary.Values.GetEnumerator();
		}

		/// <summary>
		/// 存放容器
		/// </summary>
		// Token: 0x04000033 RID: 51
		private Dictionary<string, Variable> _Dictionary;
	}
}
