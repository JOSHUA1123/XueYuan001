using System;
using System.Collections.Generic;
using System.Web;

namespace Common
{
	/// <summary>
	/// 页面跳转，如果是手机端访问电脑端，则跳转到指定手机端，通过web.config中的skip配置项
	/// </summary>
	// Token: 0x020000B2 RID: 178
	public class Skip
	{
		/// <summary>
		/// 获取参数
		/// </summary>
		// Token: 0x17000168 RID: 360
		// (get) Token: 0x060004B2 RID: 1202 RVA: 0x0000B1EE File Offset: 0x000093EE
		public static Skip Get
		{
			get
			{
				return Skip._get;
			}
		}

		// Token: 0x060004B3 RID: 1203 RVA: 0x0000B1F5 File Offset: 0x000093F5
		private Skip()
		{
			if (Skip._list == null)
			{
				this._init();
			}
		}

		/// <summary>
		/// 初始化
		/// </summary>
		// Token: 0x060004B4 RID: 1204 RVA: 0x00023080 File Offset: 0x00021280
		private void _init()
		{
			Skip._list = new List<Skip.SkipItem>();
			PlatformInfoHandler.SiteInfoHandlerParaNode siteInfoHandlerParaNode = PlatformInfoHandler.Node("Skip");
			if (!(siteInfoHandlerParaNode.GetAttr("enable") == "true"))
			{
				return;
			}
			PlatformInfoHandler.SiteInfoHandlerParaNode[] children = siteInfoHandlerParaNode.Children;
			foreach (PlatformInfoHandler.SiteInfoHandlerParaNode siteInfoHandlerParaNode2 in children)
			{
				if (siteInfoHandlerParaNode2.GetAttr("enable") != "false")
				{
					Skip.SkipItem item = new Skip.SkipItem(siteInfoHandlerParaNode2.GetAttr("web"), siteInfoHandlerParaNode2.GetAttr("mobile"));
					Skip._list.Add(item);
				}
			}
		}

		/// <summary>
		/// 通过web页，获取对应的mobile
		/// </summary>
		/// <param name="path">web页</param>
		/// <returns>mobile页</returns>
		// Token: 0x060004B5 RID: 1205 RVA: 0x00023120 File Offset: 0x00021320
		public string forWeb(string path)
		{
			if (Skip._list == null)
			{
				this._init();
			}
			string result = string.Empty;
			foreach (Skip.SkipItem skipItem in Skip._list)
			{
				if (path.Equals(skipItem.Web, StringComparison.CurrentCultureIgnoreCase))
				{
					result = skipItem.Mobile;
					break;
				}
			}
			return result;
		}

		/// <summary>
		/// 通过mobile页，获取对应的web页
		/// </summary>
		/// <param name="path">mobile页</param>
		/// <returns>web页</returns>
		// Token: 0x060004B6 RID: 1206 RVA: 0x00023198 File Offset: 0x00021398
		public string forMoible(string path)
		{
			if (Skip._list == null)
			{
				this._init();
			}
			string result = string.Empty;
			foreach (Skip.SkipItem skipItem in Skip._list)
			{
				if (path.Equals(skipItem.Mobile, StringComparison.CurrentCultureIgnoreCase))
				{
					result = skipItem.Web;
					break;
				}
			}
			return result;
		}

		/// <summary>
		/// 返回跳转的页面
		/// </summary>
		/// <returns>返回跳转的页面</returns>
		// Token: 0x060004B7 RID: 1207 RVA: 0x00023210 File Offset: 0x00021410
		public static string GetUrl()
		{
			HttpContext httpContext = HttpContext.Current;
			string a = (httpContext.Request.QueryString["skip"] != null) ? httpContext.Request.QueryString["skip"] : "yes";
			if (a != "yes")
			{
				return string.Empty;
			}
			string path = httpContext.Request.Path;
			string text = string.Empty;
			string text2 = httpContext.Request.QueryString.ToString();
			bool flag = Skip.Get.isMobilePage();
			bool flag2 = Browser.IsMobile || Browser.IsIPad;
			if (!(flag ^ flag2))
			{
				return text;
			}
			text = (flag2 ? Skip.Get.forWeb(path) : Skip.Get.forMoible(path));
			if (string.IsNullOrWhiteSpace(text) || text.Equals(path, StringComparison.CurrentCultureIgnoreCase))
			{
				text = string.Empty;
			}
			if (!string.IsNullOrWhiteSpace(text) && !string.IsNullOrWhiteSpace(text2))
			{
				text = text + "?" + text2;
			}
			return text;
		}

		/// <summary>
		/// 是否是手机端
		/// </summary>
		/// <returns></returns>
		// Token: 0x060004B8 RID: 1208 RVA: 0x0002330C File Offset: 0x0002150C
		protected bool isMobilePage()
		{
			HttpContext httpContext = HttpContext.Current;
			bool result = false;
			string text = "/mobile";
			string absolutePath = httpContext.Request.Url.AbsolutePath;
			if (absolutePath.Length >= text.Length)
			{
				string text2 = absolutePath.Substring(0, text.Length);
				if (text2.ToLower() == text.ToLower())
				{
					result = true;
				}
			}
			return result;
		}

		// Token: 0x040001EA RID: 490
		private static readonly Skip _get = new Skip();

		// Token: 0x040001EB RID: 491
		private static List<Skip.SkipItem> _list = null;

		/// <summary>
		/// 配置项
		/// </summary>
		// Token: 0x020000B3 RID: 179
		public class SkipItem
		{
			// Token: 0x17000169 RID: 361
			// (get) Token: 0x060004BA RID: 1210 RVA: 0x0000B21C File Offset: 0x0000941C
			// (set) Token: 0x060004BB RID: 1211 RVA: 0x0000B224 File Offset: 0x00009424
			public string Web { get; set; }

			// Token: 0x1700016A RID: 362
			// (get) Token: 0x060004BC RID: 1212 RVA: 0x0000B22D File Offset: 0x0000942D
			// (set) Token: 0x060004BD RID: 1213 RVA: 0x0000B235 File Offset: 0x00009435
			public string Mobile { get; set; }

			// Token: 0x060004BE RID: 1214 RVA: 0x0000B23E File Offset: 0x0000943E
			public SkipItem(string web, string mobi)
			{
				this.Web = web;
				this.Mobile = mobi;
			}
		}
	}
}
