using System;
using System.Configuration;
using System.Text.RegularExpressions;
using System.Web.Hosting;
using System.Xml;

namespace WebEditor
{
	/// <summary>
	/// 获取web.config中的KindEditor配置信息
	/// 作者：10522779@qq.com
	/// </summary>
	// Token: 0x02000002 RID: 2
	public class ConfigHandler : IConfigurationSectionHandler
	{
		/// <summary>
		/// 当前页的Web控件
		/// </summary>
		// Token: 0x17000001 RID: 1
		// (get) Token: 0x06000001 RID: 1 RVA: 0x000020D0 File Offset: 0x000002D0
		public static ConfigHandler Items
		{
			get
			{
				return ConfigHandler._item;
			}
		}

		// Token: 0x06000002 RID: 2 RVA: 0x000020D7 File Offset: 0x000002D7
		private ConfigHandler()
		{
		}

		/// <summary>
		/// 根据配置的key值，获取value值
		/// </summary>
		/// <param name="key"></param>
		/// <returns></returns>
		// Token: 0x17000002 RID: 2
		public ConfigHandler.ConfigHandlerParaNode this[string key]
		{
			get
			{
				return ConfigHandler.Node(key);
			}
		}

		/// <summary>
		/// 配置项目个数
		/// </summary>
		// Token: 0x17000003 RID: 3
		// (get) Token: 0x06000004 RID: 4 RVA: 0x000020E8 File Offset: 0x000002E8
		public int Count
		{
			get
			{
				object section = ConfigurationManager.GetSection("KindEditor");
				XmlElement xmlElement = section as XmlElement;
				if (xmlElement == null)
				{
					return 0;
				}
				XmlNodeList elementsByTagName = xmlElement.GetElementsByTagName("item");
				return elementsByTagName.Count;
			}
		}

		/// <summary>
		/// 通过主题名称获取主题配置信息
		/// </summary>
		/// <param name="name"></param>
		/// <returns></returns>
		// Token: 0x06000005 RID: 5 RVA: 0x00002120 File Offset: 0x00000320
		public static ConfigHandler.KindEditorThemeType ThemeType(string name)
		{
			object section = ConfigurationManager.GetSection("KindEditor");
			XmlElement xmlElement = section as XmlElement;
			if (xmlElement == null)
			{
				return null;
			}
			XmlNodeList elementsByTagName = xmlElement.GetElementsByTagName("item");
			ConfigHandler.KindEditorThemeType result = null;
			foreach (object obj in elementsByTagName)
			{
				XmlNode xmlNode = (XmlNode)obj;
				string value = xmlNode.Attributes["key"].Value;
				if (value.IndexOf(":") >= 0 && value.Substring(value.LastIndexOf(":") + 1) == name)
				{
					result = new ConfigHandler.KindEditorThemeType(name, xmlNode.Attributes["value"].Value);
				}
			}
			return result;
		}

		// Token: 0x06000006 RID: 6 RVA: 0x000021FC File Offset: 0x000003FC
		object IConfigurationSectionHandler.Create(object parent, object configContext, XmlNode section)
		{
			return section;
		}

		/// <summary>
		/// 获取参数节点
		/// </summary>
		/// <param name="xpath">节点名称</param>
		/// <returns></returns>
		// Token: 0x06000007 RID: 7 RVA: 0x00002200 File Offset: 0x00000400
		public static ConfigHandler.ConfigHandlerParaNode Node(string key)
		{
			object section = ConfigurationManager.GetSection("KindEditor");
			XmlElement xmlElement = section as XmlElement;
			if (xmlElement == null)
			{
				return null;
			}
			XmlNodeList elementsByTagName = xmlElement.GetElementsByTagName("item");
			XmlNode node = null;
			foreach (object obj in elementsByTagName)
			{
				XmlNode xmlNode = (XmlNode)obj;
				if (xmlNode.Attributes["key"].Value == key)
				{
					node = xmlNode;
					break;
				}
			}
			return new ConfigHandler.ConfigHandlerParaNode(node);
		}

		// Token: 0x04000001 RID: 1
		private static readonly ConfigHandler _item = new ConfigHandler();

		/// <summary>
		/// 对编辑器配置的子项进行分析
		/// </summary>
		// Token: 0x02000003 RID: 3
		public class ConfigHandlerParaNode
		{
			/// <summary>
			/// 构造函数
			/// </summary>
			/// <param name="node">参数分类的节点，如：Upload节点</param>
			// Token: 0x06000009 RID: 9 RVA: 0x000022B0 File Offset: 0x000004B0
			public ConfigHandlerParaNode(XmlNode node)
			{
				if (node != null)
				{
					this._value = node.Attributes["value"].Value;
				}
			}

			/// <summary>
			/// 参数值的原始值
			/// </summary>
			// Token: 0x17000004 RID: 4
			// (get) Token: 0x0600000A RID: 10 RVA: 0x000022D6 File Offset: 0x000004D6
			public string Value
			{
				get
				{
					if (this._value != null)
					{
						return this._value;
					}
					return this._value.Trim();
				}
			}

			/// <summary>
			/// 物理路径
			/// </summary>
			// Token: 0x17000005 RID: 5
			// (get) Token: 0x0600000B RID: 11 RVA: 0x000022F2 File Offset: 0x000004F2
			public string PhysicsPath
			{
				get
				{
					return this._MapPath(this._value);
				}
			}

			/// <summary>
			/// 虚拟路径
			/// </summary>
			// Token: 0x17000006 RID: 6
			// (get) Token: 0x0600000C RID: 12 RVA: 0x00002300 File Offset: 0x00000500
			public string VirtualPath
			{
				get
				{
					return this._VirtualPath(this._value);
				}
			}

			/// <summary>
			/// 参数的Boolean类型值，如果参数不存在或异常，则返回true;
			/// </summary>
			// Token: 0x17000007 RID: 7
			// (get) Token: 0x0600000D RID: 13 RVA: 0x00002310 File Offset: 0x00000510
			public bool Boolean
			{
				get
				{
					bool result = false;
					if (this._value == "")
					{
						return false;
					}
					try
					{
						result = Convert.ToBoolean(this._value);
					}
					catch
					{
						try
						{
							result = Convert.ToBoolean(Convert.ToInt16(this._value));
						}
						catch
						{
							return false;
						}
					}
					return result;
				}
			}

			/// <summary>
			/// 获取某路径的物理路径，路径末尾带有\符号
			/// </summary>
			/// <param name="path"></param>
			/// <returns></returns>
			// Token: 0x0600000E RID: 14 RVA: 0x0000237C File Offset: 0x0000057C
			private string _MapPath(string path)
			{
				path = HostingEnvironment.MapPath(path);
				if (path.Substring(path.LastIndexOf("\\")).IndexOf(".") < 0 && path.Substring(path.Length - 1) != "\\")
				{
					path += "\\";
				}
				path = Regex.Replace(path, "\\\\+", "\\");
				return path;
			}

			/// <summary>
			/// 获取某路径相对网站根目录的虚拟路径，路径末尾带有/符号
			/// </summary>
			/// <param name="path"></param>
			/// <returns></returns>
			// Token: 0x0600000F RID: 15 RVA: 0x000023EC File Offset: 0x000005EC
			private string _VirtualPath(string path)
			{
				path = path.Replace("\\", "/");
				path = path.Replace("~/", HostingEnvironment.ApplicationVirtualPath);
				if (path.Substring(path.LastIndexOf("/")).IndexOf(".") < 0 && path.Substring(path.Length - 1) != "/")
				{
					path += "/";
				}
				path = Regex.Replace(path, "\\/+", "/");
				return path;
			}

			// Token: 0x04000002 RID: 2
			private string _value;
		}

		/// <summary>
		/// 编辑器的主题
		/// </summary>
		// Token: 0x02000004 RID: 4
		public class KindEditorThemeType
		{
			/// <summary>
			/// 主题的名称
			/// </summary>
			// Token: 0x17000008 RID: 8
			// (get) Token: 0x06000010 RID: 16 RVA: 0x00002475 File Offset: 0x00000675
			// (set) Token: 0x06000011 RID: 17 RVA: 0x0000247D File Offset: 0x0000067D
			public string Name
			{
				get
				{
					return this._name;
				}
				set
				{
					this._name = value;
				}
			}

			/// <summary>
			/// 模板要显示的项
			/// </summary>
			// Token: 0x17000009 RID: 9
			// (get) Token: 0x06000012 RID: 18 RVA: 0x00002486 File Offset: 0x00000686
			// (set) Token: 0x06000013 RID: 19 RVA: 0x0000248E File Offset: 0x0000068E
			public string Items
			{
				get
				{
					return this._items;
				}
				set
				{
					this._items = value;
				}
			}

			// Token: 0x06000014 RID: 20 RVA: 0x00002497 File Offset: 0x00000697
			public KindEditorThemeType(string name, string items)
			{
				this._name = name;
				this._items = items;
			}

			// Token: 0x04000003 RID: 3
			private string _name;

			// Token: 0x04000004 RID: 4
			private string _items;
		}
	}
}
