using System;
using System.Collections.Generic;
using System.IO;
using System.Web;
using System.Web.Caching;
using System.Xml;

namespace Common
{
	/// <summary>
	/// 系统的版权信息，来自copyright.xml，用于代理商更改产品版权信息
	/// </summary>
	/// <typeparam name="TKey"></typeparam>
	/// <typeparam name="TValue"></typeparam>
	// Token: 0x02000081 RID: 129
	public class Copyright<TKey, TValue> : Dictionary<TKey, TValue>
	{
		/// <summary>
		/// 信息集
		/// </summary>
		// Token: 0x170000FD RID: 253
		// (get) Token: 0x06000356 RID: 854 RVA: 0x0000A71A File Offset: 0x0000891A
		public static Copyright<string, string> Items
		{
			get
			{
				return Copyright<string, string>.GetCopyright();
			}
		}

		/// <summary>
		/// 如果获取不到，不要报错
		/// </summary>
		/// <param name="key"></param>
		/// <returns></returns>
		// Token: 0x170000FE RID: 254
		public new TValue this[TKey key]
		{
			get
			{
				bool flag = false;
				foreach (TKey tkey in base.Keys)
				{
					if (tkey.Equals(key))
					{
						flag = true;
						break;
					}
				}
				if (flag)
				{
					return base[key];
				}
				return default(TValue);
			}
		}

		/// <summary>
		/// 获取版本信息
		/// </summary>
		/// <returns></returns>
		// Token: 0x06000358 RID: 856 RVA: 0x0001BDA0 File Offset: 0x00019FA0
		public static Copyright<string, string> GetCopyright()
		{
			Cache cache = HttpRuntime.Cache;
			Copyright<string, string> copyright = cache.Get("copyright") as Copyright<string, string>;
			if (copyright == null)
			{
				string text = Server.MapPath("/copyright.xml");
				if (!File.Exists(text))
				{
					return null;
				}
				XmlDocument xmlDocument = new XmlDocument();
				xmlDocument.Load(text);
				XmlNode lastChild = xmlDocument.LastChild;
				if (lastChild != null)
				{
					copyright = new Copyright<string, string>();
					XmlNodeList childNodes = lastChild.ChildNodes;
					foreach (object obj in childNodes)
					{
						XmlNode xmlNode = (XmlNode)obj;
						if (xmlNode.NodeType == XmlNodeType.Element)
						{
							copyright.Add(xmlNode.Name, xmlNode.InnerText);
						}
					}
				}
				cache.Insert("copyright", copyright, new CacheDependency(text));
			}
			return copyright;
		}

		/// <summary>
		/// 转换为json
		/// </summary>
		/// <returns></returns>
		// Token: 0x06000359 RID: 857 RVA: 0x0001BE84 File Offset: 0x0001A084
		public string ToJson()
		{
			Copyright<string, string> copyright = Copyright<string, string>.GetCopyright();
			string text = "{";
			foreach (KeyValuePair<string, string> keyValuePair in copyright)
			{
				string text2 = text;
				text = string.Concat(new string[]
				{
					text2,
					"'",
					keyValuePair.Key,
					"':'",
					HttpContext.Current.Server.UrlEncode(keyValuePair.Value),
					"',"
				});
			}
			if (text.EndsWith(","))
			{
				text = text.Substring(0, text.Length - 1);
			}
			text += "}";
			return text;
		}
	}
}
