using System;
using System.Collections;
using System.Xml;
using Common.Param.Method;

namespace Common
{
	/// <summary>
	/// 配置项的操作对象
	/// </summary>
	// Token: 0x02000019 RID: 25
	public class CustomConfigItem : IEnumerable
	{
		/// <summary>
		/// 配置项的Key
		/// </summary>
		// Token: 0x17000010 RID: 16
		// (get) Token: 0x0600007C RID: 124 RVA: 0x0000949D File Offset: 0x0000769D
		// (set) Token: 0x0600007D RID: 125 RVA: 0x0000DDCC File Offset: 0x0000BFCC
		public string Key
		{
			get
			{
				return this._key;
			}
			set
			{
				this._key = value;
				if (this._node.Attributes["key"] == null)
				{
					((XmlElement)this._node).SetAttribute("key", this._key);
				}
				else
				{
					this._node.Attributes["key"].Value = value;
				}
				this.Refresh();
			}
		}

		/// <summary>
		/// 配置项的值(vlaue)
		/// </summary>
		// Token: 0x17000011 RID: 17
		// (get) Token: 0x0600007E RID: 126 RVA: 0x000094A5 File Offset: 0x000076A5
		// (set) Token: 0x0600007F RID: 127 RVA: 0x0000DE38 File Offset: 0x0000C038
		public string Text
		{
			get
			{
				return this._text;
			}
			set
			{
				this._text = value;
				if (this._node.Attributes["value"] == null)
				{
					((XmlElement)this._node).SetAttribute("value", this._text);
				}
				else
				{
					this._node.Attributes["value"].Value = value;
				}
				this.Refresh();
			}
		}

		// Token: 0x17000012 RID: 18
		// (get) Token: 0x06000080 RID: 128 RVA: 0x000094AD File Offset: 0x000076AD
		public ConvertToAnyValue Value
		{
			get
			{
				return new ConvertToAnyValue(this._text);
			}
		}

		/// <summary>
		/// 子级配置项
		/// </summary>
		// Token: 0x17000013 RID: 19
		// (get) Token: 0x06000081 RID: 129 RVA: 0x000094BA File Offset: 0x000076BA
		public CustomConfigItem[] Childs
		{
			get
			{
				return this._childs;
			}
		}

		// Token: 0x17000014 RID: 20
		// (get) Token: 0x06000082 RID: 130 RVA: 0x000094C2 File Offset: 0x000076C2
		public XmlNode Node
		{
			get
			{
				return this._node;
			}
		}

		// Token: 0x06000083 RID: 131 RVA: 0x000094CA File Offset: 0x000076CA
		public CustomConfigItem(XmlNode node)
		{
			this._node = node;
			this.Refresh();
		}

		/// <summary>
		/// 刷新
		/// </summary>
		// Token: 0x06000084 RID: 132 RVA: 0x0000DEA4 File Offset: 0x0000C0A4
		public void Refresh()
		{
			this._key = ((this._node.Attributes["key"] != null) ? this._node.Attributes["key"].Value : "");
			this._text = ((this._node.Attributes["value"] != null) ? this._node.Attributes["value"].Value : "");
			XmlNodeList childNodes = this._node.ChildNodes;
			if (childNodes.Count > 0)
			{
				this._childs = new CustomConfigItem[childNodes.Count];
				for (int i = 0; i < this._childs.Length; i++)
				{
					this._childs[i] = new CustomConfigItem(childNodes[i]);
				}
			}
		}

		/// <summary>
		/// 添加子项
		/// </summary>
		/// <param name="key"></param>
		/// <param name="value"></param>
		// Token: 0x06000085 RID: 133 RVA: 0x0000DF7C File Offset: 0x0000C17C
		public CustomConfigItem ChildAdd(string key, string value)
		{
			CustomConfigItem customConfigItem = null;
			if (this.Childs != null)
			{
				foreach (CustomConfigItem customConfigItem2 in this.Childs)
				{
					if (customConfigItem2.Key.ToLower() == key.ToLower().Trim())
					{
						customConfigItem = customConfigItem2;
						break;
					}
				}
			}
			if (customConfigItem == null)
			{
				XmlDocument ownerDocument = this._node.OwnerDocument;
				XmlNode xmlNode = ownerDocument.CreateNode("element", "item", "");
				XmlElement xmlElement = (XmlElement)xmlNode;
				xmlElement.SetAttribute("key", key);
				xmlElement.SetAttribute("value", value);
				this._node.AppendChild(xmlNode);
			}
			else
			{
				this.ChildUpdate(key, value);
			}
			this.Refresh();
			return this.ChildSingle(key);
		}

		// Token: 0x06000086 RID: 134 RVA: 0x0000E044 File Offset: 0x0000C244
		public void ChildDelete(string key)
		{
			XmlNodeList childNodes = this._node.ChildNodes;
			foreach (object obj in childNodes)
			{
				XmlNode xmlNode = (XmlNode)obj;
				if (xmlNode.Attributes["key"].Value.ToLower() == key.ToLower().Trim())
				{
					this._node.RemoveChild(xmlNode);
				}
			}
			this.Refresh();
		}

		// Token: 0x06000087 RID: 135 RVA: 0x0000E0DC File Offset: 0x0000C2DC
		public CustomConfigItem ChildUpdate(string key, string value)
		{
			XmlNodeList childNodes = this._node.ChildNodes;
			foreach (object obj in childNodes)
			{
				XmlNode xmlNode = (XmlNode)obj;
				if (xmlNode.Attributes["key"].Value.ToLower() == key.ToLower().Trim())
				{
					((XmlElement)xmlNode).SetAttribute("value", value);
				}
			}
			this.Refresh();
			return this.ChildSingle(key);
		}

		// Token: 0x06000088 RID: 136 RVA: 0x0000E180 File Offset: 0x0000C380
		public CustomConfigItem ChildUpdate(int index, string key, string value)
		{
			XmlNodeList childNodes = this._node.ChildNodes;
			if (index >= 0 && index < childNodes.Count)
			{
				XmlElement xmlElement = (XmlElement)childNodes[index];
				xmlElement.SetAttribute("key", key);
				xmlElement.SetAttribute("value", value);
			}
			this.Refresh();
			return this.ChildSingle(key);
		}

		// Token: 0x06000089 RID: 137 RVA: 0x0000E1D8 File Offset: 0x0000C3D8
		public CustomConfigItem ChildSingle(string key)
		{
			if (this.Childs == null)
			{
				this.ChildAdd(key, "");
			}
			CustomConfigItem customConfigItem = null;
			foreach (CustomConfigItem customConfigItem2 in this.Childs)
			{
				if (customConfigItem2.Key.ToLower() == key.ToLower().Trim())
				{
					customConfigItem = customConfigItem2;
					break;
				}
			}
			if (customConfigItem == null)
			{
				return this.ChildAdd(key, "");
			}
			return customConfigItem;
		}

		/// <summary>
		/// 迭代器
		/// </summary>
		/// <returns></returns>
		// Token: 0x0600008A RID: 138 RVA: 0x0000E248 File Offset: 0x0000C448
		public IEnumerator GetEnumerator()
		{
			for (int i = 0; i < this._childs.Length; i++)
			{
				yield return this._childs[i];
			}
			yield break;
		}

		// Token: 0x04000026 RID: 38
		private string _key;

		// Token: 0x04000027 RID: 39
		private string _text;

		// Token: 0x04000028 RID: 40
		private CustomConfigItem[] _childs;

		// Token: 0x04000029 RID: 41
		private XmlNode _node;
	}
}
