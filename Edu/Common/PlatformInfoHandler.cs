using System;
using System.Configuration;
using System.Xml;
using Common.Param.Method;

namespace Common
{
	/// <summary>
	/// 解析web.config自定义的Platform节点
	/// </summary>
	// Token: 0x02000032 RID: 50
	public class PlatformInfoHandler : IConfigurationSectionHandler
	{
		// Token: 0x06000154 RID: 340 RVA: 0x000098FF File Offset: 0x00007AFF
		object IConfigurationSectionHandler.Create(object parent, object configContext, XmlNode section)
		{
			return section;
		}

		/// <summary>
		/// 获取配置的具体参数节点，即某一类参数的节点，如:Upload、Template等
		/// </summary>
		/// <param name="xpath">节点路径，例如：Upload、Template等</param>
		/// <returns></returns>
		// Token: 0x06000155 RID: 341 RVA: 0x0001215C File Offset: 0x0001035C
		public static XmlNode GetParaNode(string xpath)
		{
			object section = ConfigurationManager.GetSection("Platform");
			XmlElement xmlElement = section as XmlElement;
			return xmlElement.SelectSingleNode(xpath);
		}

		/// <summary>
		/// 获取参数节点
		/// </summary>
		/// <param name="xpath">节点名称</param>
		/// <returns></returns>
		// Token: 0x06000156 RID: 342 RVA: 0x00012184 File Offset: 0x00010384
		public static PlatformInfoHandler.SiteInfoHandlerParaNode Node(string xpath)
		{
			object section = ConfigurationManager.GetSection("Platform");
			XmlElement xmlElement = section as XmlElement;
			if (xmlElement == null)
			{
				return null;
			}
			XmlNode xmlNode = xmlElement.SelectSingleNode(xpath);
			if (xmlNode == null)
			{
				return null;
			}
			return new PlatformInfoHandler.SiteInfoHandlerParaNode(xmlNode);
		}

		/// <summary>
		/// 对配置参数的节点的解析；如：Upload节点的属性、子项等
		/// </summary>
		// Token: 0x02000033 RID: 51
		public class SiteInfoHandlerParaNode
		{
			/// <summary>
			/// 节点的Key值
			/// </summary>
			// Token: 0x1700003E RID: 62
			// (get) Token: 0x06000158 RID: 344 RVA: 0x00009902 File Offset: 0x00007B02
			public string Key
			{
				get
				{
					return this._key;
				}
			}

			/// <summary>
			/// 节点的Value值
			/// </summary>
			// Token: 0x1700003F RID: 63
			// (get) Token: 0x06000159 RID: 345 RVA: 0x0000990A File Offset: 0x00007B0A
			public ConvertToAnyValue Value
			{
				get
				{
					return new ConvertToAnyValue(this._value);
				}
			}

			/// <summary>
			/// 第一个子节点
			/// </summary>
			// Token: 0x17000040 RID: 64
			// (get) Token: 0x0600015A RID: 346 RVA: 0x00009917 File Offset: 0x00007B17
			public PlatformInfoHandler.SiteInfoHandlerParaNode FirstChildren
			{
				get
				{
					if (this._list.Count < 1)
					{
						return null;
					}
					return new PlatformInfoHandler.SiteInfoHandlerParaNode(this._list[0]);
				}
			}

			/// <summary>
			/// 所有的子节点对象
			/// </summary>
			// Token: 0x17000041 RID: 65
			// (get) Token: 0x0600015B RID: 347 RVA: 0x000121C0 File Offset: 0x000103C0
			public PlatformInfoHandler.SiteInfoHandlerParaNode[] Children
			{
				get
				{
					if (this._list.Count < 1)
					{
						return null;
					}
					PlatformInfoHandler.SiteInfoHandlerParaNode[] array = new PlatformInfoHandler.SiteInfoHandlerParaNode[this._list.Count];
					for (int i = 0; i < this._list.Count; i++)
					{
						array[i] = new PlatformInfoHandler.SiteInfoHandlerParaNode(this._list[i]);
					}
					return array;
				}
			}

			/// <summary>
			/// 通过key值，获取下级子节点
			/// </summary>
			/// <param name="key"></param>
			/// <returns></returns>
			// Token: 0x0600015C RID: 348 RVA: 0x0001221C File Offset: 0x0001041C
			public PlatformInfoHandler.SiteInfoHandlerParaNode GetChildren(string key)
			{
				if (this._list.Count < 1)
				{
					return null;
				}
				PlatformInfoHandler.SiteInfoHandlerParaNode result = null;
				PlatformInfoHandler.SiteInfoHandlerParaNode[] children = this.Children;
				for (int i = 0; i < children.Length; i++)
				{
					if (string.Equals(children[i].GetAttr("key"), key, StringComparison.CurrentCultureIgnoreCase))
					{
						result = children[i];
					}
				}
				return result;
			}

			/// <summary>
			/// 构造函数
			/// </summary>
			/// <param name="node">参数分类的节点，如：Upload节点</param>
			// Token: 0x0600015D RID: 349 RVA: 0x0001226C File Offset: 0x0001046C
			public SiteInfoHandlerParaNode(XmlNode node)
			{
				XmlNodeList list = node.SelectNodes("./item");
				this._list = list;
				this._node = node;
				this._key = ((node.Attributes["key"] != null) ? node.Attributes["key"].Value : "");
				this._value = ((node.Attributes["value"] != null) ? node.Attributes["value"].Value : "");
			}

			/// <summary>
			/// 获取属性值
			/// </summary>
			/// <param name="attr"></param>
			/// <returns></returns>
			// Token: 0x0600015E RID: 350 RVA: 0x0000993A File Offset: 0x00007B3A
			public string GetAttr(string attr)
			{
				if (this._node.Attributes[attr] == null)
				{
					return "";
				}
				return this._node.Attributes[attr].Value;
			}

			/// <summary>
			/// 获取当前参数节点的属性
			/// </summary>
			/// <param name="key">下级节点的Key值</param>
			/// <returns></returns>
			// Token: 0x17000042 RID: 66
			public PlatformInfoHandler.SiteInfoHandlerParaNode this[string key]
			{
				get
				{
					if (this._list == null)
					{
						return null;
					}
					XmlNode node = null;
					foreach (object obj in this._list)
					{
						XmlNode xmlNode = (XmlNode)obj;
						if (xmlNode.Attributes["key"].Value.ToLower() == key.Trim().ToLower())
						{
							node = xmlNode;
							break;
						}
					}
					return new PlatformInfoHandler.SiteInfoHandlerParaNode(node);
				}
			}

			/// <summary>
			/// 根据子项(Item)的key值，获取Value值
			/// </summary>
			/// <param name="key"></param>
			/// <returns></returns>
			// Token: 0x06000160 RID: 352 RVA: 0x00012398 File Offset: 0x00010598
			public string ItemValue(string key)
			{
				string result = "";
				if (this._list == null)
				{
					return result;
				}
				foreach (object obj in this._list)
				{
					XmlNode xmlNode = (XmlNode)obj;
					if (xmlNode != null && xmlNode.Attributes["key"] != null && xmlNode.Attributes["key"].Value.ToLower() == key.Trim().ToLower())
					{
						result = xmlNode.Attributes["value"].Value;
						break;
					}
				}
				return result;
			}

			// Token: 0x0400006D RID: 109
			private XmlNodeList _list;

			// Token: 0x0400006E RID: 110
			private XmlNode _node;

			// Token: 0x0400006F RID: 111
			private string _key;

			// Token: 0x04000070 RID: 112
			private string _value;
		}
	}
}
