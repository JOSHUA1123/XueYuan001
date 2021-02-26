using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Common.Param.Method;

namespace Common.Templates
{
	/// <summary>
	/// Webconfig中的模板信息的配置子项
	/// </summary>
	// Token: 0x0200007B RID: 123
	public class TemplateConfingItem
	{
		/// <summary>
		/// 构造函数
		/// </summary>
		/// <param name="bankName">模板库名称，如Web、Mobile</param>
		// Token: 0x0600031E RID: 798 RVA: 0x0001AD58 File Offset: 0x00018F58
		public TemplateConfingItem(string bankName)
		{
			string attr = PlatformInfoHandler.Node(Template.TemplateNodeName).GetAttr("path");
			string @string = PlatformInfoHandler.Node(Template.TemplateNodeName)[bankName].Value.String;
			this._tempbankPath = new ConvertToAnyValue(attr + @string);
			this._nodeItem = PlatformInfoHandler.Node(Template.TemplateNodeName)[bankName];
			string value = string.Empty;
			PlatformInfoHandler.SiteInfoHandlerParaNode children = this._nodeItem.GetChildren("public");
			if (children != null)
			{
				value = children.Value.String;
			}
			if (Directory.Exists(this.Root.Physics))
			{
				DirectoryInfo[] directories = new DirectoryInfo(this.Root.Physics).GetDirectories();
				List<TemplateBank> list = new List<TemplateBank>();
				foreach (DirectoryInfo directoryInfo in directories)
				{
					if (!directoryInfo.Name.Equals("aspnet_client", StringComparison.OrdinalIgnoreCase) && !directoryInfo.Name.Equals(value, StringComparison.OrdinalIgnoreCase) && !directoryInfo.Name.StartsWith("_") && directoryInfo.Name.IndexOf("_") <= -1)
					{
						list.Add(new TemplateBank(directoryInfo, this)
						{
							Path = this.GetPath(directoryInfo.Name),
							Tag = directoryInfo.Name
						});
					}
				}
				this._items = this._orderItem(list.ToArray<TemplateBank>());
				string attr2 = this._nodeItem.GetAttr("default");
				foreach (TemplateBank templateBank in this.Items)
				{
					templateBank.IsCurrent = (templateBank.Tag.ToLower() == attr2.ToLower());
				}
			}
		}

		/// <summary>
		/// 所有模板所处的根路径
		/// </summary>
		// Token: 0x170000DF RID: 223
		// (get) Token: 0x0600031F RID: 799 RVA: 0x0000A5A7 File Offset: 0x000087A7
		public _Path Root
		{
			get
			{
				return new _Path(this._tempbankPath.VirtualPath, "");
			}
		}

		/// <summary>
		/// 默认模板库
		/// </summary>
		// Token: 0x170000E0 RID: 224
		// (get) Token: 0x06000320 RID: 800 RVA: 0x0001AF38 File Offset: 0x00019138
		public TemplateBank Default
		{
			get
			{
				string attr = this._nodeItem.GetAttr("default");
				TemplateBank templateBank = null;
				foreach (TemplateBank templateBank2 in this.Items)
				{
					if (templateBank2.Tag.ToLower() == attr.ToLower().Trim())
					{
						templateBank = templateBank2;
						break;
					}
				}
				if (templateBank == null)
				{
					templateBank = ((this.Items.Length > 0) ? this.Items[0] : null);
				}
				return templateBank;
			}
		}

		/// <summary>
		/// 公共模板库
		/// </summary>
		// Token: 0x170000E1 RID: 225
		// (get) Token: 0x06000321 RID: 801 RVA: 0x0001AFB4 File Offset: 0x000191B4
		public TemplateBank Public
		{
			get
			{
				if (this._public == null)
				{
					PlatformInfoHandler.SiteInfoHandlerParaNode children = this._nodeItem.GetChildren("public");
					if (children != null)
					{
						DirectoryInfo directoryInfo = new DirectoryInfo(this.Root.Physics + children.Value.String);
						if (!directoryInfo.Exists)
						{
							throw new Exception("指定的公共模板库不存在");
						}
						this._public = new TemplateBank(directoryInfo, this);
						this._public.Path = this.GetPath(directoryInfo.Name);
						this._public.Tag = children.Value.String;
					}
					if (this._public == null)
					{
						throw new Exception("未找到公共模板库");
					}
				}
				return this._public;
			}
		}

		/// <summary>
		/// 配置项的key值
		/// </summary>
		// Token: 0x170000E2 RID: 226
		// (get) Token: 0x06000322 RID: 802 RVA: 0x0000A5BE File Offset: 0x000087BE
		public string Key
		{
			get
			{
				return this._nodeItem.Key;
			}
		}

		/// <summary>
		/// 配置项的value
		/// </summary>
		// Token: 0x170000E3 RID: 227
		// (get) Token: 0x06000323 RID: 803 RVA: 0x0000A5CB File Offset: 0x000087CB
		public ConvertToAnyValue Value
		{
			get
			{
				return this._nodeItem.Value;
			}
		}

		/// <summary>
		/// 模板标识，用于网址路径，如m/xx.htm时,mobile模板的tag为m，则最终路径指向手机端
		/// </summary>
		// Token: 0x170000E4 RID: 228
		// (get) Token: 0x06000324 RID: 804 RVA: 0x0000A5D8 File Offset: 0x000087D8
		public string Tag
		{
			get
			{
				if (string.IsNullOrWhiteSpace(this._tag))
				{
					this._tag = this._nodeItem.GetAttr("tag");
				}
				return this._tag;
			}
		}

		/// <summary>
		/// 当前使用模板
		/// </summary>
		// Token: 0x170000E5 RID: 229
		// (get) Token: 0x06000325 RID: 805 RVA: 0x0001B068 File Offset: 0x00019268
		public TemplateBank Current
		{
			get
			{
				string text = this._currentName;
				if (string.IsNullOrWhiteSpace(this._currentName))
				{
					text = this._nodeItem.GetAttr("default");
				}
				TemplateBank templateBank = null;
				foreach (TemplateBank templateBank2 in this.Items)
				{
					if (templateBank2.Tag.ToLower() == text.ToLower().Trim())
					{
						templateBank = templateBank2;
						templateBank2.IsCurrent = true;
					}
					else
					{
						templateBank2.IsCurrent = false;
					}
				}
				if (templateBank == null)
				{
					templateBank = this.Default;
				}
				return templateBank;
			}
		}

		/// <summary>
		/// 当前模板分类下，所有可供选择的模板库
		/// </summary>
		/// <returns></returns>
		// Token: 0x170000E6 RID: 230
		// (get) Token: 0x06000326 RID: 806 RVA: 0x0000A603 File Offset: 0x00008803
		public TemplateBank[] Items
		{
			get
			{
				return this._items;
			}
		}

		/// <summary>
		/// 获取某模板路径
		/// </summary>
		/// <param name="templateName">模板名称</param>
		/// <returns></returns>
		// Token: 0x06000327 RID: 807 RVA: 0x0000A60B File Offset: 0x0000880B
		public _Path GetPath(string templateName)
		{
			return new _Path(this._tempbankPath.VirtualPath, templateName);
		}

		/// <summary>
		/// 获取当前配置项的属性
		/// </summary>
		/// <param name="attr"></param>
		/// <returns></returns>
		// Token: 0x06000328 RID: 808 RVA: 0x0001B0F4 File Offset: 0x000192F4
		public ConvertToAnyValue GetAttr(string attr)
		{
			string attr2 = this._nodeItem.GetAttr(attr);
			return new ConvertToAnyValue(attr2);
		}

		/// <summary>
		/// 设置当前使用的模板
		/// </summary>
		/// <param name="tag">当前模板的名称</param>
		/// <returns>模板对象</returns>
		// Token: 0x06000329 RID: 809 RVA: 0x0000A61E File Offset: 0x0000881E
		public TemplateBank SetCurrent(string tag)
		{
			this._currentName = tag;
			return this.Current;
		}

		/// <summary>
		/// 通过标识（文件夹名称）获取模板
		/// </summary>
		/// <param name="tag">标识（文件夹名称）</param>
		/// <returns></returns>
		// Token: 0x0600032A RID: 810 RVA: 0x0001B114 File Offset: 0x00019314
		public TemplateBank GetTemplate(string tag)
		{
			TemplateBank templateBank = null;
			foreach (TemplateBank templateBank2 in this.Items)
			{
				if (templateBank2.Tag.ToLower() == tag.ToLower().Trim())
				{
					templateBank = templateBank2;
					break;
				}
			}
			if (templateBank == null)
			{
				TemplateBank @public = this.Public;
				if (@public.Tag.ToLower() == tag.ToLower().Trim())
				{
					templateBank = @public;
				}
			}
			return templateBank;
		}

		/// <summary>
		/// 获取当前模样库的参数(即item设置中的子级item参数）
		/// </summary>
		/// <param name="key">key值</param>
		/// <returns>返回参数项的value值</returns>
		// Token: 0x0600032B RID: 811 RVA: 0x0001B18C File Offset: 0x0001938C
		public ConvertToAnyValue GetParameter(string key)
		{
			ConvertToAnyValue result = null;
			PlatformInfoHandler.SiteInfoHandlerParaNode[] children = this._nodeItem.Children;
			foreach (PlatformInfoHandler.SiteInfoHandlerParaNode siteInfoHandlerParaNode in children)
			{
				if (siteInfoHandlerParaNode.Key == key)
				{
					result = siteInfoHandlerParaNode.Value;
					break;
				}
			}
			return result;
		}

		/// <summary>
		/// 通过模板库的名称进行排序
		/// </summary>
		/// <param name="items"></param>
		/// <returns></returns>
		// Token: 0x0600032C RID: 812 RVA: 0x0001B1D8 File Offset: 0x000193D8
		private TemplateBank[] _orderItem(TemplateBank[] items)
		{
			for (int i = items.Length; i > 0; i--)
			{
				for (int j = 0; j < i - 1; j++)
				{
					if (this.compare(items[j], items[j + 1]))
					{
						TemplateBank templateBank = items[j];
						items[j] = items[j + 1];
						items[j + 1] = templateBank;
					}
				}
			}
			return items;
		}

		/// <summary>
		/// 比较两个模拟名称的大值（按ascii）
		/// </summary>
		/// <param name="s1"></param>
		/// <param name="s2"></param>
		/// <returns>前者为true，后者大为false</returns>
		// Token: 0x0600032D RID: 813 RVA: 0x0001B228 File Offset: 0x00019428
		private bool compare(TemplateBank s1, TemplateBank s2)
		{
			if (string.IsNullOrWhiteSpace(s1.Name))
			{
				return false;
			}
			if (string.IsNullOrWhiteSpace(s2.Name))
			{
				return true;
			}
			if (s1.Name.Trim() == "")
			{
				return false;
			}
			if (s2.Name.Trim() == "")
			{
				return true;
			}
			int num = (s1.Name.Length < s2.Name.Length) ? s1.Name.Length : s2.Name.Length;
			char[] array = s1.Name.ToCharArray();
			char[] array2 = s2.Name.ToCharArray();
			TemplateBank templateBank = null;
			for (int i = 0; i < num; i++)
			{
				if (array[i] != array2[i])
				{
					templateBank = ((array[i] > array2[i]) ? s1 : s2);
					break;
				}
			}
			if (templateBank == null)
			{
				templateBank = ((array.Length > array2.Length) ? s1 : s2);
			}
			return templateBank.Name.Trim() == s1.Name.Trim();
		}

		// Token: 0x04000132 RID: 306
		private ConvertToAnyValue _tempbankPath;

		/// <summary>
		/// 模板库的节点信息
		/// </summary>
		// Token: 0x04000133 RID: 307
		private PlatformInfoHandler.SiteInfoHandlerParaNode _nodeItem;

		// Token: 0x04000134 RID: 308
		private TemplateBank _public;

		// Token: 0x04000135 RID: 309
		private string _tag = string.Empty;

		// Token: 0x04000136 RID: 310
		private string _currentName = string.Empty;

		// Token: 0x04000137 RID: 311
		private TemplateBank[] _items;
	}
}
