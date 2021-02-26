using System;
using System.Collections.Generic;
using System.Reflection;
using Common.Param.Method;

namespace Common
{
	/// <summary>
	/// 管理页面缓存
	/// </summary>
	// Token: 0x02000092 RID: 146
	public class PageCache
	{
		/// <summary>
		/// 获取参数
		/// </summary>
		// Token: 0x17000118 RID: 280
		// (get) Token: 0x060003BB RID: 955 RVA: 0x0001E904 File Offset: 0x0001CB04
		public static PageCache Get
		{
			get
			{
				if (PageCache._singleton == null)
				{
					lock (PageCache._lockobj)
					{
						if (PageCache._singleton == null)
						{
							PageCache._singleton = new PageCache();
						}
					}
				}
				return PageCache._singleton;
			}
		}

		// Token: 0x060003BC RID: 956 RVA: 0x0001E95C File Offset: 0x0001CB5C
		private PageCache()
		{
			PlatformInfoHandler.SiteInfoHandlerParaNode siteInfoHandlerParaNode = PlatformInfoHandler.Node("PageCache");
			this.ConfingNode = siteInfoHandlerParaNode;
			this.setVale();
			if (siteInfoHandlerParaNode.Children != null)
			{
				foreach (PlatformInfoHandler.SiteInfoHandlerParaNode node in siteInfoHandlerParaNode.Children)
				{
					PageCache item = new PageCache(node, this);
					this._items.Add(item);
				}
			}
		}

		// Token: 0x060003BD RID: 957 RVA: 0x0000A9EB File Offset: 0x00008BEB
		private PageCache(PlatformInfoHandler.SiteInfoHandlerParaNode node, PageCache parent)
		{
			this.ConfingNode = node;
			this.Parent = parent;
			this.setVale();
		}

		/// <summary>
		/// 缓存项的集合
		/// </summary>
		// Token: 0x17000119 RID: 281
		// (get) Token: 0x060003BE RID: 958 RVA: 0x0000AA12 File Offset: 0x00008C12
		public List<PageCache> Items
		{
			get
			{
				return this._items;
			}
		}

		// Token: 0x1700011A RID: 282
		// (get) Token: 0x060003BF RID: 959 RVA: 0x0000AA1A File Offset: 0x00008C1A
		// (set) Token: 0x060003C0 RID: 960 RVA: 0x0000AA22 File Offset: 0x00008C22
		private PlatformInfoHandler.SiteInfoHandlerParaNode ConfingNode { get; set; }

		/// <summary>
		/// 写入静态化文件到硬盘的所在路径
		/// </summary>
		// Token: 0x1700011B RID: 283
		// (get) Token: 0x060003C1 RID: 961 RVA: 0x0000AA2B File Offset: 0x00008C2B
		// (set) Token: 0x060003C2 RID: 962 RVA: 0x0000AA33 File Offset: 0x00008C33
		public string Path { get; set; }

		/// <summary>
		/// 静态文件路径
		/// </summary>
		// Token: 0x1700011C RID: 284
		// (get) Token: 0x060003C3 RID: 963 RVA: 0x0000AA3C File Offset: 0x00008C3C
		public _Path StaticPath
		{
			get
			{
				return new _Path(this.Path, "");
			}
		}

		// Token: 0x1700011D RID: 285
		// (get) Token: 0x060003C4 RID: 964 RVA: 0x0000AA4E File Offset: 0x00008C4E
		// (set) Token: 0x060003C5 RID: 965 RVA: 0x0000AA56 File Offset: 0x00008C56
		public List<string> Statics { get; set; }

		/// <summary>
		/// 缓存的时间，单位：分钟
		/// </summary>
		// Token: 0x1700011E RID: 286
		// (get) Token: 0x060003C6 RID: 966 RVA: 0x0000AA5F File Offset: 0x00008C5F
		// (set) Token: 0x060003C7 RID: 967 RVA: 0x0000AA67 File Offset: 0x00008C67
		public int TimeSpan { get; set; }

		/// <summary>
		/// 是否写入到硬盘
		/// </summary>
		// Token: 0x1700011F RID: 287
		// (get) Token: 0x060003C8 RID: 968 RVA: 0x0000AA70 File Offset: 0x00008C70
		// (set) Token: 0x060003C9 RID: 969 RVA: 0x0000AA78 File Offset: 0x00008C78
		public bool Write { get; set; }

		/// <summary>
		/// 是否启用缓存
		/// </summary>
		// Token: 0x17000120 RID: 288
		// (get) Token: 0x060003CA RID: 970 RVA: 0x0000AA81 File Offset: 0x00008C81
		// (set) Token: 0x060003CB RID: 971 RVA: 0x0000AA89 File Offset: 0x00008C89
		public bool Enable { get; set; }

		/// <summary>
		/// 缓存项的Key值
		/// </summary>
		// Token: 0x17000121 RID: 289
		// (get) Token: 0x060003CC RID: 972 RVA: 0x0000AA92 File Offset: 0x00008C92
		// (set) Token: 0x060003CD RID: 973 RVA: 0x0000AA9A File Offset: 0x00008C9A
		public string Key { get; set; }

		/// <summary>
		/// 缓存项的Value值
		/// </summary>
		// Token: 0x17000122 RID: 290
		// (get) Token: 0x060003CE RID: 974 RVA: 0x0000AAA3 File Offset: 0x00008CA3
		// (set) Token: 0x060003CF RID: 975 RVA: 0x0000AAAB File Offset: 0x00008CAB
		public string Value { get; set; }

		/// <summary>
		/// 当前节点的父节点
		/// </summary>
		// Token: 0x17000123 RID: 291
		// (get) Token: 0x060003D0 RID: 976 RVA: 0x0000AAB4 File Offset: 0x00008CB4
		// (set) Token: 0x060003D1 RID: 977 RVA: 0x0000AABC File Offset: 0x00008CBC
		public PageCache Parent { get; set; }

		/// <summary>
		/// 通过网址来获取缓存像
		/// </summary>
		/// <param name="key">来自Request.Url.PathAndQuery，即网址+参数</param>
		/// <returns></returns>
		// Token: 0x17000124 RID: 292
		public PageCache this[string key]
		{
			get
			{
				string text = key;
				string empty = string.Empty;
				if (key.IndexOf("?") > -1)
				{
					text = key.Substring(0, key.IndexOf("?"));
					key.Substring(key.IndexOf("?") + 1);
				}
				PageCache pageCache = null;
				foreach (PageCache pageCache2 in this.Items)
				{
					if (text.Equals(pageCache2.Key, StringComparison.CurrentCultureIgnoreCase))
					{
						pageCache = pageCache2;
						break;
					}
				}
				pageCache = ((pageCache == null) ? this : pageCache);
				return pageCache;
			}
		}

		/// <summary>
		/// 设置当前对象的值（来自config文件）
		/// </summary>
		// Token: 0x060003D3 RID: 979 RVA: 0x0001EA74 File Offset: 0x0001CC74
		private void setVale()
		{
			if (this.ConfingNode == null)
			{
				return;
			}
			string[] array = new string[]
			{
				"Path",
				"Write",
				"TimeSpan",
				"Enable",
				"Key",
				"Value"
			};
			Type type = base.GetType();
			foreach (PropertyInfo propertyInfo in type.GetProperties())
			{
				bool flag = false;
				foreach (string value in array)
				{
					if (propertyInfo.Name.Equals(value, StringComparison.CurrentCultureIgnoreCase))
					{
						flag = true;
						break;
					}
				}
				if (flag)
				{
					string attr = this.ConfingNode.GetAttr(propertyInfo.Name.ToLower());
					if (string.IsNullOrWhiteSpace(attr) && this.Parent != null)
					{
						attr = this.Parent.ConfingNode.GetAttr(propertyInfo.Name.ToLower());
					}
					if (!string.IsNullOrWhiteSpace(attr))
					{
						try
						{
							object value2 = Convert.ChangeType(attr, propertyInfo.PropertyType);
							propertyInfo.SetValue(this, value2, null);
						}
						catch
						{
							if (this.Parent != null)
							{
								attr = this.Parent.ConfingNode.GetAttr(propertyInfo.Name.ToLower());
								if (!string.IsNullOrWhiteSpace(attr))
								{
									object value3 = Convert.ChangeType(attr, propertyInfo.PropertyType);
									propertyInfo.SetValue(this, value3, null);
								}
							}
						}
					}
				}
			}
		}

		// Token: 0x04000167 RID: 359
		private static PageCache _singleton = null;

		// Token: 0x04000168 RID: 360
		private static readonly object _lockobj = new object();

		// Token: 0x04000169 RID: 361
		private List<PageCache> _items = new List<PageCache>();

		/// <summary>
		/// 缓存项
		/// </summary>
		// Token: 0x02000093 RID: 147
		public class PageCache_StaticItem
		{
			/// <summary>
			/// 缓存项的键值
			/// </summary>
			// Token: 0x17000125 RID: 293
			// (get) Token: 0x060003D5 RID: 981 RVA: 0x0000AAD7 File Offset: 0x00008CD7
			// (set) Token: 0x060003D6 RID: 982 RVA: 0x0000AADF File Offset: 0x00008CDF
			public string Key { get; set; }

			/// <summary>
			/// 缓存项的value，即参数项
			/// </summary>
			// Token: 0x17000126 RID: 294
			// (get) Token: 0x060003D7 RID: 983 RVA: 0x0000AAE8 File Offset: 0x00008CE8
			// (set) Token: 0x060003D8 RID: 984 RVA: 0x0000AAF0 File Offset: 0x00008CF0
			public string Value { get; set; }

			/// <summary>
			/// 缓存创建时间
			/// </summary>
			// Token: 0x17000127 RID: 295
			// (get) Token: 0x060003D9 RID: 985 RVA: 0x0000AAF9 File Offset: 0x00008CF9
			// (set) Token: 0x060003DA RID: 986 RVA: 0x0000AB01 File Offset: 0x00008D01
			public DateTime CreateTime { get; set; }

			/// <summary>
			/// 缓存项的内容
			/// </summary>
			// Token: 0x17000128 RID: 296
			// (get) Token: 0x060003DB RID: 987 RVA: 0x0000AB0A File Offset: 0x00008D0A
			// (set) Token: 0x060003DC RID: 988 RVA: 0x0000AB12 File Offset: 0x00008D12
			public string Context { get; set; }

			// Token: 0x060003DD RID: 989 RVA: 0x0000AB1B File Offset: 0x00008D1B
			public PageCache_StaticItem(string key, DateTime time, string value)
			{
				this.Key = key;
				this.CreateTime = time;
				this.Value = value;
			}
		}
	}
}
