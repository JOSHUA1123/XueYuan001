using System;
using System.Collections.Generic;
using System.Xml;
using Common;

namespace SMS
{
	// Token: 0x02000012 RID: 18
	public class Config
	{
		/// <summary>
		/// 获取参数(单件对象)
		/// </summary>
		// Token: 0x1700001C RID: 28
		// (get) Token: 0x06000079 RID: 121 RVA: 0x00002F72 File Offset: 0x00001172
		public static Config Singleton
		{
			get
			{
				return Config._get;
			}
		}

		/// <summary>
		/// 短信平台列表
		/// </summary>
		// Token: 0x1700001D RID: 29
		// (get) Token: 0x0600007A RID: 122 RVA: 0x00002F79 File Offset: 0x00001179
		public static SmsItem[] SmsItems
		{
			get
			{
				return Config.smsItems;
			}
		}

		/// <summary>
		/// 当前采用的短信平台
		/// </summary>
		// Token: 0x1700001E RID: 30
		// (get) Token: 0x0600007B RID: 123 RVA: 0x00002F80 File Offset: 0x00001180
		// (set) Token: 0x0600007C RID: 124 RVA: 0x00002F88 File Offset: 0x00001188
		public string CurrentName
		{
			get
			{
				return this._currentName;
			}
			set
			{
				this._currentName = value;
			}
		}

		// Token: 0x0600007D RID: 125 RVA: 0x00002F94 File Offset: 0x00001194
		private Config()
		{
			XmlNodeList childNodes = PlatformInfoHandler.GetParaNode("SMS").ChildNodes;
			List<SmsItem> list = new List<SmsItem>();
			for (int i = 0; i < childNodes.Count; i++)
			{
				XmlNode node = childNodes[i];
				if (!(this._getValue(node, "isUse") == "false"))
				{
					SmsItem smsItem = new SmsItem();
					smsItem = new SmsItem();
					smsItem.Type = this._getValue(node, "type");
					smsItem.Remarks = this._getValue(node, "remarks");
					smsItem.Name = this._getValue(node, "name");
					smsItem.Domain = this._getValue(node, "domain");
					if (!smsItem.Domain.EndsWith("/"))
					{
						SmsItem smsItem2 = smsItem;
						smsItem2.Domain += "/";
					}
					smsItem.RegisterUrl = this._getValue(node, "regurl");
					smsItem.PayUrl = this._getValue(node, "payurl");
					smsItem.IsUse = true;
					list.Add(smsItem);
				}
			}
			Config.smsItems = list.ToArray();
			foreach (SmsItem smsItem3 in Config.smsItems)
			{
				if (smsItem3.Remarks == this._currentName)
				{
					smsItem3.IsCurrent = true;
					return;
				}
			}
		}

		// Token: 0x0600007E RID: 126 RVA: 0x000030FC File Offset: 0x000012FC
		private string _getValue(XmlNode node, string attr)
		{
			foreach (object obj in node.Attributes)
			{
				XmlAttribute xmlAttribute = (XmlAttribute)obj;
				if (string.Equals(attr, xmlAttribute.Name, StringComparison.CurrentCultureIgnoreCase))
				{
					return xmlAttribute.Value;
				}
			}
			return string.Empty;
		}

		/// <summary>
		/// 当前采用的短信平台
		/// </summary>
		// Token: 0x1700001F RID: 31
		// (get) Token: 0x0600007F RID: 127 RVA: 0x00003170 File Offset: 0x00001370
		public static SmsItem Current
		{
			get
			{
				SmsItem smsItem = null;
				foreach (SmsItem smsItem2 in Config.SmsItems)
				{
					if (smsItem2.IsCurrent)
					{
						smsItem = smsItem2;
						break;
					}
				}
				if (smsItem == null && Config.SmsItems.Length > 0)
				{
					smsItem = Config.SmsItems[0];
				}
				return smsItem;
			}
		}

		/// <summary>
		/// 设置当前的短信平台
		/// </summary>
		/// <param name="remarks"></param>
		// Token: 0x06000080 RID: 128 RVA: 0x000031BC File Offset: 0x000013BC
		public static void SetCurrent(string remarks)
		{
			foreach (SmsItem smsItem in Config.SmsItems)
			{
				smsItem.IsCurrent = (smsItem.Remarks == remarks);
			}
		}

		// Token: 0x0400001D RID: 29
		private static readonly Config _get = new Config();

		// Token: 0x0400001E RID: 30
		private static SmsItem[] smsItems;

		// Token: 0x0400001F RID: 31
		private string _currentName;
	}
}
