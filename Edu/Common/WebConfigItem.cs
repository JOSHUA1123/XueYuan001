using System;
using System.Xml;

namespace Common
{
	/// <summary>
	/// 自定义webconfig节点的item项
	/// </summary>
	// Token: 0x02000072 RID: 114
	public class WebConfigItem
	{
		// Token: 0x170000CC RID: 204
		// (get) Token: 0x060002F0 RID: 752 RVA: 0x0000A3DC File Offset: 0x000085DC
		// (set) Token: 0x060002F1 RID: 753 RVA: 0x0000A3E4 File Offset: 0x000085E4
		public string Key
		{
			get
			{
				return this._key;
			}
			set
			{
				this._key = value;
			}
		}

		// Token: 0x170000CD RID: 205
		// (get) Token: 0x060002F2 RID: 754 RVA: 0x0000A3ED File Offset: 0x000085ED
		// (set) Token: 0x060002F3 RID: 755 RVA: 0x0000A3F5 File Offset: 0x000085F5
		public string Value
		{
			get
			{
				return this._value;
			}
			set
			{
				this._value = value;
			}
		}

		// Token: 0x170000CE RID: 206
		// (get) Token: 0x060002F4 RID: 756 RVA: 0x0000A3FE File Offset: 0x000085FE
		// (set) Token: 0x060002F5 RID: 757 RVA: 0x0000A406 File Offset: 0x00008606
		public XmlNode Item
		{
			get
			{
				return this._item;
			}
			set
			{
				this._item = value;
			}
		}

		// Token: 0x060002F6 RID: 758 RVA: 0x0000A40F File Offset: 0x0000860F
		public WebConfigItem(XmlNode node)
		{
			this._item = node;
		}

		// Token: 0x0400011B RID: 283
		private string _key;

		// Token: 0x0400011C RID: 284
		private string _value;

		// Token: 0x0400011D RID: 285
		private XmlNode _item;
	}
}
