using System;
using System.Collections;
using System.Xml;

namespace Common
{
	/// <summary>
	/// 自定义配置，实现一些小范围管理内的参数设置
	/// 说明：基于xml管理的参数设置
	/// </summary>
	// Token: 0x02000017 RID: 23
	public class CustomConfig : IEnumerable
	{
		/// <summary>
		/// 整个配置项目的字符串（xml文档）
		/// </summary>
		// Token: 0x1700000B RID: 11
		// (get) Token: 0x0600006D RID: 109 RVA: 0x0000944A File Offset: 0x0000764A
		// (set) Token: 0x0600006E RID: 110 RVA: 0x00009457 File Offset: 0x00007657
		public string XmlString
		{
			get
			{
				return this.xmldoc.InnerXml;
			}
			set
			{
				this._xml = value;
			}
		}

		/// <summary>
		/// 子级配置项
		/// </summary>
		// Token: 0x1700000C RID: 12
		// (get) Token: 0x0600006F RID: 111 RVA: 0x00009460 File Offset: 0x00007660
		public CustomConfigItem Items
		{
			get
			{
				return this._items;
			}
		}

		// Token: 0x06000070 RID: 112 RVA: 0x0000DC44 File Offset: 0x0000BE44
		public CustomConfig()
		{
			string text = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>";
			text += "<items/>";
			this._xml = text;
			this.xmldoc.LoadXml(this._xml);
			XmlNode lastChild = this.xmldoc.LastChild;
			this._items = new CustomConfigItem(lastChild);
		}

		// Token: 0x06000071 RID: 113 RVA: 0x0000DCA4 File Offset: 0x0000BEA4
		public CustomConfig(string xml)
		{
			if (!string.IsNullOrWhiteSpace(xml))
			{
				this._xml = xml;
				this.xmldoc.LoadXml(this._xml);
				XmlNode lastChild = this.xmldoc.LastChild;
				this._items = new CustomConfigItem(lastChild);
			}
		}

		/// <summary>
		/// 载信配置信息，创建管理对象
		/// </summary>
		/// <param name="xml"></param>
		/// <returns></returns>
		// Token: 0x06000072 RID: 114 RVA: 0x0000DCFC File Offset: 0x0000BEFC
		public static CustomConfig Load(string xml)
		{
			CustomConfig result;
			if (!string.IsNullOrWhiteSpace(xml))
			{
				result = new CustomConfig(xml);
			}
			else
			{
				result = new CustomConfig();
			}
			return result;
		}

		// Token: 0x06000073 RID: 115 RVA: 0x00009468 File Offset: 0x00007668
		public static CustomConfig Load()
		{
			return new CustomConfig();
		}

		// Token: 0x1700000D RID: 13
		public CustomConfigItem this[string key]
		{
			get
			{
				return this._items.ChildSingle(key);
			}
		}

		// Token: 0x06000075 RID: 117 RVA: 0x0000DD24 File Offset: 0x0000BF24
		public IEnumerator GetEnumerator()
		{
			for (int i = 0; i < this._items.Childs.Length; i++)
			{
				yield return this._items.Childs[i];
			}
			yield break;
		}

		// Token: 0x0400001F RID: 31
		private XmlDocument xmldoc = new XmlDocument();

		// Token: 0x04000020 RID: 32
		private string _xml;

		// Token: 0x04000021 RID: 33
		private CustomConfigItem _items;
	}
}
