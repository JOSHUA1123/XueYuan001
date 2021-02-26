using System;
using System.Web.UI;

namespace Common
{
	/// <summary>
	/// webpage的增强
	/// </summary>
	// Token: 0x02000023 RID: 35
	public static class PageHelper
	{
		/// <summary>
		/// 弹出提示
		/// </summary>
		/// <param name="page"></param>
		/// <param name="say"></param>
		// Token: 0x060000CB RID: 203 RVA: 0x0000FCC0 File Offset: 0x0000DEC0
		public static void Alert(this Page page, string say)
		{
			if (string.IsNullOrWhiteSpace(say))
			{
				return;
			}
			if (page == null)
			{
				return;
			}
			say = say.Replace("\\", "\\\\");
			say = say.Replace("\r", "");
			say = say.Replace("\n", "");
			string text = "<script language='JavaScript' type='text/javascript'>alert('{say}');</script>";
			text = text.Replace("{say}", say);
			if (page == null)
			{
				return;
			}
			if (!page.ClientScript.IsStartupScriptRegistered(page.GetType(), "alert"))
			{
				page.ClientScript.RegisterStartupScript(page.GetType(), "alert", text);
			}
		}
	}
}
