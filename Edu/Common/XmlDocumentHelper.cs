using System;
using System.Text.RegularExpressions;
using System.Xml;

namespace Common
{
	/// <summary>
	/// XML文档操作的扩展
	/// </summary>
	// Token: 0x02000069 RID: 105
	public static class XmlDocumentHelper
	{
		// Token: 0x060002A7 RID: 679 RVA: 0x0001835C File Offset: 0x0001655C
		public static void LoadXml(this XmlDocument doc, string xml, bool isDtd)
		{
			if (!isDtd)
			{
				Regex regex = new Regex("<!DOCTYPE|<!ENTITY", RegexOptions.IgnoreCase | RegexOptions.Multiline);
				if (regex.IsMatch(xml))
				{
					throw new Exception("禁止使用DTD定义！请移除DOCTYPE或ENTITY标签。");
				}
			}
			doc.LoadXml(xml);
		}
	}
}
