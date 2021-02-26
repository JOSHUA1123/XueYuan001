using System;
using System.Collections.Generic;

namespace VTemplate.Engine
{
	/// <summary>
	/// 变量
	/// </summary>
	// Token: 0x0200000B RID: 11
	public class Variable : ICloneableElement<Variable>
	{
		/// <summary>
		///
		/// </summary>
		/// <param name="ownerTemplate"></param>
		/// <param name="name"></param>
		// Token: 0x0600007F RID: 127 RVA: 0x00003B11 File Offset: 0x00001D11
		internal Variable(Template ownerTemplate, string name)
		{
			this.OwnerTemplate = ownerTemplate;
			this.Name = name;
			this.cacheExpItems = new Dictionary<string, object>(StringComparer.InvariantCultureIgnoreCase);
		}

		/// <summary>
		/// 变量的宿主模板
		/// </summary>
		// Token: 0x1700002C RID: 44
		// (get) Token: 0x06000080 RID: 128 RVA: 0x00003B37 File Offset: 0x00001D37
		// (set) Token: 0x06000081 RID: 129 RVA: 0x00003B3F File Offset: 0x00001D3F
		public Template OwnerTemplate { get; private set; }

		/// <summary>
		/// 变量名称
		/// </summary>
		// Token: 0x1700002D RID: 45
		// (get) Token: 0x06000082 RID: 130 RVA: 0x00003B48 File Offset: 0x00001D48
		// (set) Token: 0x06000083 RID: 131 RVA: 0x00003B50 File Offset: 0x00001D50
		public string Name { get; private set; }

		/// <summary>
		/// 此变量的值
		/// </summary>
		// Token: 0x1700002E RID: 46
		// (get) Token: 0x06000084 RID: 132 RVA: 0x00003B59 File Offset: 0x00001D59
		// (set) Token: 0x06000085 RID: 133 RVA: 0x00003B61 File Offset: 0x00001D61
		public object Value
		{
			get
			{
				return this._Value;
			}
			set
			{
				this._Value = value;
				this.Reset();
			}
		}

		/// <summary>
		/// 获取缓存的个数
		/// </summary>
		// Token: 0x06000086 RID: 134 RVA: 0x00003B70 File Offset: 0x00001D70
		internal int GetCacheCount()
		{
			return this.cacheExpItems.Count;
		}

		/// <summary>
		/// 添加变量表达式的值
		/// </summary>
		/// <param name="exp">变量表达式.以"."号开始.如".age"则表示此变量下的age属性/字段值</param>
		/// <param name="value"></param>
		// Token: 0x06000087 RID: 135 RVA: 0x00003B80 File Offset: 0x00001D80
		internal void AddExpValue(string exp, object value)
		{
			lock (this.cacheExpItems)
			{
				if (this.cacheExpItems.ContainsKey(exp))
				{
					this.cacheExpItems[exp] = value;
				}
				else
				{
					this.cacheExpItems.Add(exp, value);
				}
			}
		}

		/// <summary>
		/// 获取变量表达的值
		/// </summary>
		/// <param name="exp"></param>
		/// <param name="exist"></param>
		/// <returns></returns>
		// Token: 0x06000088 RID: 136 RVA: 0x00003BE4 File Offset: 0x00001DE4
		internal object GetExpValue(string exp, out bool exist)
		{
			exist = false;
			object result;
			lock (this.cacheExpItems)
			{
				exist = this.cacheExpItems.TryGetValue(exp, out result);
			}
			return result;
		}

		/// <summary>
		/// 设置变量中某种表达式所表示的值
		/// </summary>
		/// <param name="exp">变量表达式.如"age"则表示此变量下的age属性/字段值,"age.tostring()"则表示此变量下的age属性/字段的值的tostring方法所返回的值</param>
		/// <param name="value"></param>
		// Token: 0x06000089 RID: 137 RVA: 0x00003C34 File Offset: 0x00001E34
		public void SetExpValue(string exp, object value)
		{
			if (string.IsNullOrEmpty(exp))
			{
				throw new ArgumentNullException("exp");
			}
			this.AddExpValue("." + exp, value);
		}

		/// <summary>
		/// 重设(清空)当前变量中已缓存的表达式值
		/// </summary>
		// Token: 0x0600008A RID: 138 RVA: 0x00003C5B File Offset: 0x00001E5B
		public void Reset()
		{
			this.cacheExpItems.Clear();
		}

		/// <summary>
		/// 克隆当前变量对象到新的宿主模板
		/// </summary>
		/// <param name="ownerTemplate"></param>
		/// <returns></returns>
		// Token: 0x0600008B RID: 139 RVA: 0x00003C68 File Offset: 0x00001E68
		public Variable Clone(Template ownerTemplate)
		{
			return new Variable(ownerTemplate, this.Name);
		}

		/// <summary>
		/// 变量的值
		/// </summary>
		// Token: 0x0400001D RID: 29
		private object _Value;

		/// <summary>
		/// 缓存表达式数据
		/// </summary>
		// Token: 0x0400001E RID: 30
		private Dictionary<string, object> cacheExpItems;
	}
}
