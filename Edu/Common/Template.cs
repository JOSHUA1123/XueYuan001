using System;
using System.IO;
using Common.Param.Method;
using Common.Templates;

namespace Common
{
	/// <summary>
	/// 系统模板信息
	/// </summary>
	// Token: 0x020000CD RID: 205
	public class Template
	{
		/// <summary>
		/// 所有模板配置项
		/// </summary>
		// Token: 0x170001A4 RID: 420
		// (get) Token: 0x060005A0 RID: 1440 RVA: 0x0000B835 File Offset: 0x00009A35
		public static TemplateConfingItem[] All
		{
			get
			{
				return Template._all;
			}
		}

		/// <summary>
		/// 电脑网页模板的集合
		/// </summary>
		// Token: 0x170001A5 RID: 421
		// (get) Token: 0x060005A1 RID: 1441 RVA: 0x0000B83C File Offset: 0x00009A3C
		public static TemplateConfingItem ForWeb
		{
			get
			{
				return Template.ForAny("Web");
			}
		}

		/// <summary>
		/// 手机网页的模板的集合
		/// </summary>
		// Token: 0x170001A6 RID: 422
		// (get) Token: 0x060005A2 RID: 1442 RVA: 0x0000B848 File Offset: 0x00009A48
		public static TemplateConfingItem ForMobile
		{
			get
			{
				return Template.ForAny("Mobile");
			}
		}

		/// <summary>
		/// 根据分类返回指定类型的模版库
		/// </summary>
		/// <param name="type">模板库分类名称，如Web、Mobile</param>
		/// <returns></returns>
		// Token: 0x060005A3 RID: 1443 RVA: 0x00027884 File Offset: 0x00025A84
		public static TemplateConfingItem ForAny(string type)
		{
			TemplateConfingItem result = null;
			if (Template._all == null)
			{
				Template._all = Template.GetTemplates();
			}
			foreach (TemplateConfingItem templateConfingItem in Template._all)
			{
				if (type.Equals(templateConfingItem.Key, StringComparison.CurrentCultureIgnoreCase))
				{
					result = templateConfingItem;
					break;
				}
			}
			return result;
		}

		/// <summary>
		/// 获取模板库
		/// </summary>
		/// <param name="type">模板类型，如web，mobi</param>
		/// <param name="tag">模板库的标识，即文件夹名称</param>
		/// <returns></returns>
		// Token: 0x060005A4 RID: 1444 RVA: 0x000278D0 File Offset: 0x00025AD0
		public static TemplateBank GetTemplate(string type, string tag)
		{
			string attr = PlatformInfoHandler.Node(Template.TemplateNodeName).GetAttr("path");
			PlatformInfoHandler.SiteInfoHandlerParaNode siteInfoHandlerParaNode = PlatformInfoHandler.Node(Template.TemplateNodeName)[type];
			string @string = siteInfoHandlerParaNode.Value.String;
			TemplateConfingItem config = Template.ForAny(@string);
			string virtualPath = new ConvertToAnyValue(attr + @string).VirtualPath;
			DirectoryInfo directoryInfo = new DirectoryInfo(Server.MapPath(virtualPath) + tag);
			return new TemplateBank(directoryInfo, config)
			{
				Path = new _Path(virtualPath, tag),
				Tag = directoryInfo.Name
			};
		}

		/// <summary>
		/// 获取所有模板库
		/// </summary>
		/// <returns></returns>
		// Token: 0x060005A5 RID: 1445 RVA: 0x00027968 File Offset: 0x00025B68
		public static TemplateConfingItem[] GetTemplates()
		{
			PlatformInfoHandler.SiteInfoHandlerParaNode siteInfoHandlerParaNode = PlatformInfoHandler.Node(Template.TemplateNodeName);
			TemplateConfingItem[] array = new TemplateConfingItem[siteInfoHandlerParaNode.Children.Length];
			for (int i = 0; i < array.Length; i++)
			{
				PlatformInfoHandler.SiteInfoHandlerParaNode siteInfoHandlerParaNode2 = siteInfoHandlerParaNode.Children[i];
				array[i] = new TemplateConfingItem(siteInfoHandlerParaNode2.Key);
			}
			return array;
		}

		/// <summary>
		/// 刷新模板信息
		/// </summary>
		// Token: 0x060005A6 RID: 1446 RVA: 0x0000B854 File Offset: 0x00009A54
		public static void RefreshTemplate()
		{
			Template._all = Template.GetTemplates();
		}

		/// <summary>
		/// 模板所处的根文件夹
		/// </summary>
		// Token: 0x170001A7 RID: 423
		// (get) Token: 0x060005A7 RID: 1447 RVA: 0x000279B4 File Offset: 0x00025BB4
		public static _Path Path
		{
			get
			{
				if (Template._path == null)
				{
					string attr = PlatformInfoHandler.Node(Template.TemplateNodeName).GetAttr("path");
					Template._path = new _Path(attr, null);
				}
				return Template._path;
			}
		}

		/// <summary>
		/// 是否精简模样html代码（去掉换行、空格、注释等）
		/// </summary>
		// Token: 0x170001A8 RID: 424
		// (get) Token: 0x060005A8 RID: 1448 RVA: 0x000279F0 File Offset: 0x00025BF0
		public static bool IsTrim
		{
			get
			{
				if (Template._isTrim == null)
				{
					Template._isTrim = new bool?(new ConvertToAnyValue(PlatformInfoHandler.Node(Template.TemplateNodeName).GetAttr("istrim")).Boolean ?? false);
				}
				return Template._isTrim.Value;
			}
		}

		/// <summary>
		/// 模板的默认首页
		/// </summary>
		// Token: 0x170001A9 RID: 425
		// (get) Token: 0x060005A9 RID: 1449 RVA: 0x00027A54 File Offset: 0x00025C54
		public static string Homepage
		{
			get
			{
				if (string.IsNullOrWhiteSpace(Template._homepage))
				{
					string attr = PlatformInfoHandler.Node(Template.TemplateNodeName).GetAttr("homepage");
					Template._homepage = new ConvertToAnyValue(attr).String;
				}
				return Template._homepage;
			}
		}

		/// <summary>
		/// 配置项的节点名称
		/// </summary>
		// Token: 0x04000258 RID: 600
		public static readonly string TemplateNodeName = "Template";

		// Token: 0x04000259 RID: 601
		private static TemplateConfingItem[] _all = Template.GetTemplates();

		// Token: 0x0400025A RID: 602
		private static _Path _path = null;

		// Token: 0x0400025B RID: 603
		private static bool? _isTrim = null;

		// Token: 0x0400025C RID: 604
		private static string _homepage = string.Empty;
	}
}
