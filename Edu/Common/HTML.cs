using System;
using System.Text.RegularExpressions;

namespace Common
{
	// Token: 0x0200008B RID: 139
	public class HTML
	{
		/// <summary>
		/// 去除HTML标签，并返回指定长度
		/// </summary>
		/// <param name="html"></param>
		/// <param name="length"></param>
		/// <returns></returns>
		// Token: 0x06000395 RID: 917 RVA: 0x0001E110 File Offset: 0x0001C310
		public static string ClearTag(string html, int length = 0)
		{
			if (string.IsNullOrWhiteSpace(html))
			{
				return html;
			}
			string text = Regex.Replace(html, "<[^>]+>", "");
			text = Regex.Replace(text, "&[^;]+;", "");
			if (length > 0 && text.Length > length)
			{
				return text.Substring(0, length);
			}
			return text;
		}
	}
}
